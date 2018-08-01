using System.IO.Abstractions;

namespace MergeARM.Core
{
    public class ArmIO : IArmIO
    {
        private readonly IFileSystem fileSystem;

        public ArmIO(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public ArmTemplate LoadArmTemplate(string filePath)
        {
            return new ArmTemplate(filePath, fileSystem.File.ReadAllText(filePath));
        }
    }
}
