using MergeARM.Core;
using System.IO.Abstractions;

namespace MergeARM
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fileName = args[0];

            // Load file
            var armTemplate = new ArmIO(new FileSystem()).LoadArmTemplate(fileName);

            // Expand file

            // Save a copy of the file
        }
    }
}
