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
        internal static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts => MainImpl(opts))
                .WithNotParsed((errs) => HandleParseError(errs));
        }

        private static void MainImpl(Options options)
        {
            try
            {
                if (options.Verbose)
                {
                    Console.WriteLine($"Verbose output enabled. Current Arguments: -i {options.InputFile} -o {options.OutputFile} -v {options.Verbose}");
                    Console.WriteLine("ExpandARM is in Verbose mode!");
                }
                else
                {
                    Console.WriteLine("ExpandARM");
                }

                var armio = ArmIO.Create(new FileSystem());
                var armTemplate = armio.LoadArmTemplate(options.InputFile);
                armio.ExpandArmTemplate(armTemplate);
                armio.SaveExpandedTemplate(armTemplate, options.OutputFile);
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

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Missing required option");
        }
    }
}
