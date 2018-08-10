using Newtonsoft.Json.Linq;
using System.IO.Abstractions;
using System.Linq;

namespace ExpandARM.Core
{
    public class ArmIO : IArmIO
    {
        private readonly IFileSystem fileSystem;

        private ArmIO(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public ArmTemplate LoadArmTemplate(string filePath)
        {
            var fileText = fileSystem.File.ReadAllText(filePath);
            var jObject = JObject.Parse(fileText);
            return new ArmTemplate(filePath, jObject);
        }

        public void ExpandArmTemplate(ArmTemplate armTemplate)
        {
            armTemplate.ExpandedContent
                .SelectTokens("$..templateLink")
                .ToList()
                .ForEach(t =>
                {
                    var templateFilePath = ((string)((dynamic)t).uri).Replace("file://", "").Replace("/", "\\");
                    string templateFullPath = GetFullPath(armTemplate.FilePath, templateFilePath);

                    var nestedTemplate = LoadArmTemplate(templateFullPath);
                    ExpandArmTemplate(nestedTemplate);
                    t.Parent.Parent["template"] = nestedTemplate.ExpandedContent;

                    ((JObject)t.Parent.Parent).Property("templateLink").Remove();
                });
        }

        public void SaveExpandedTemplate(ArmTemplate armTemplate)
        {
            var storeFileName = fileSystem.Path.ChangeExtension(armTemplate.FilePath, "expanded.josn");

            armTemplate.ExpandedFileName = storeFileName;

            fileSystem.File.WriteAllText(storeFileName, armTemplate.ExpandedContent.ToString());
        }

        public static IArmIO Create(IFileSystem fileSystem)
        {
            return new ArmIO(fileSystem);
        }

        private string GetFullPath(string hostFilePath, string templateFilePath)
        {
            return fileSystem.Path.IsPathRooted(templateFilePath) ?
                templateFilePath :
                fileSystem.Path.Combine(fileSystem.Path.GetDirectoryName(hostFilePath), templateFilePath);
        }
    }
}
