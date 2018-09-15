using CommandLine;

namespace ExpandARM
{
    internal class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Filename where the expanded ARM template will be saved.")]
        public string OutputFile { get; set; }
    }
}
