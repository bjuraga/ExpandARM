using Newtonsoft.Json.Linq;
using System.Linq;

namespace MergeARM.Core
{
    public class ArmTemplate
    {
        public string FileName { get; }

        public string FileText { get; }

        public bool NeedsExpansion { get; }

        public bool IsExpanded { get; }

        public dynamic OriginalContent { get; }

        private JObject originalContent;

        public ArmTemplate(string fileName, string fileText)
        {
            FileName = fileName;
            FileText = fileText;

            originalContent = JObject.Parse(FileText);
            OriginalContent = originalContent;

            NeedsExpansion = originalContent.SelectTokens("$..templateLink").Any();
        }
    }
}
