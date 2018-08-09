using CommandLine;

namespace ExpandARM
{
    internal class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }
    }
}
