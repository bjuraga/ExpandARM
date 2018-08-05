using Newtonsoft.Json.Linq;
using System.IO.Abstractions;
using System.Linq;

namespace MergeARM.Core
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
            return new ArmTemplate(filePath, fileSystem.File.ReadAllText(filePath));
        }

        public void ExpandArmTemplate(ArmTemplate armTemplate)
        {
            armTemplate.ExpandedContent
                .SelectTokens("$..templateLink")
                .ToList()
                .ForEach(t =>
                {
                    var templateFilePath = ((string)((dynamic)t).uri).Replace("file://", "").Replace("/", "\\");
                    var templateFileContents = fileSystem.File.ReadAllText(templateFilePath);
                    var templateJson = JObject.Parse(templateFileContents);
                    t.Parent.Parent["template"] = templateJson;
                    ((JObject)t.Parent.Parent).Property("templateLink").Remove();
                });
        }

        public void SaveExpandedTemplate(ArmTemplate armTemplate)
        {
            var storeFileName = fileSystem.Path.ChangeExtension(armTemplate.FileName, "expanded.josn");

            armTemplate.ExpandedFileName = storeFileName;

            fileSystem.File.WriteAllText(storeFileName, armTemplate.ExpandedContent.ToString());
        }

        public static IArmIO Create(IFileSystem fileSystem)
        {
            return new ArmIO(fileSystem);
        }
    }
}
