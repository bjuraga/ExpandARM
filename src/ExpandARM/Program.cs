using CommandLine;
using ExpandARM.Core;
using ExpandARM.Core.Exceptions;
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
                .WithParsed(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed((errs) => HandleParseError(errs));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Missing required option");
        }

        private static void RunOptionsAndReturnExitCode(Options commandLineOptions)
        {
            try
            {
                var armio = ArmIO.Create(new FileSystem());

                // Load file
                var armTemplate = armio.LoadArmTemplate(commandLineOptions.InputFile);

                // Expand file
                armio.ExpandArmTemplate(armTemplate);

                // Save a copy of the file
                armio.SaveExpandedTemplate(armTemplate);
            }
            catch (ExpandArmException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ExpandArmException("Unhandled exception caught.", e);
            }
        }
    }
}
