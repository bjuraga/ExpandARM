using CommandLine;

namespace ExpandARM
{
    internal class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = false, HelpText = "Filename where the expanded ARM template will be saved.")]
        public string OutputFile { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }
}
