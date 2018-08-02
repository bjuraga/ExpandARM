using MergeARM.Core;
using System.IO.Abstractions;

namespace MergeARM
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fileName = args[0];

            var armio = ArmIO.Create(new FileSystem());

            // Load file
            var armTemplate = armio.LoadArmTemplate(fileName);

            // Expand file
            armio.ExpandArmTemplate(armTemplate);

            // Save a copy of the file
        }
    }
}
