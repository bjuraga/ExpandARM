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
                    var templateFilePath = ((string)((dynamic)t).uri).Replace("file://", "");
                    var templateFileContents = fileSystem.File.ReadAllText(templateFilePath);
                    var templateJson = JObject.Parse(templateFileContents);
                    t.Parent.Parent["template"] = templateJson;
                    ((JObject)t.Parent.Parent).Property("templateLink").Remove();
                });
        }

        public static IArmIO Create(IFileSystem fileSystem)
        {
            return new ArmIO(fileSystem);
        }
    }
}
