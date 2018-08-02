using Newtonsoft.Json.Linq;
using System.Linq;

namespace MergeARM.Core
{
    public class ArmTemplate
    {
        public string FileName { get; }

        public string FileText { get; }

        public bool NeedsExpansion => OriginalContent.SelectTokens("$..templateLink").Any();

        public bool IsExpanded => !ExpandedContent.SelectTokens("$..templateLink").Any();

        //public dynamic OriginalContent { get; }

        public JObject OriginalContent { get; }

        public JObject ExpandedContent { get; set; }

        public ArmTemplate(string fileName, string fileText)
        {
            FileName = fileName;
            FileText = fileText;

            OriginalContent = JObject.Parse(FileText);
            ExpandedContent = OriginalContent;
        }
    }
}
