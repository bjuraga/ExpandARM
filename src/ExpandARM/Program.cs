using CommandLine;
using ExpandARM.Core;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace ExpandARM
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Missing required option");
        }

        private static void RunOptionsAndReturnExitCode(Options commandLineOptions)
        {
            var armio = ArmIO.Create(new FileSystem());

            // Load file
            var armTemplate = armio.LoadArmTemplate(commandLineOptions.InputFile);

            // Expand file
            armio.ExpandArmTemplate(armTemplate);

            // Save a copy of the file
            armio.SaveExpandedTemplate(armTemplate);
        }
    }
}
