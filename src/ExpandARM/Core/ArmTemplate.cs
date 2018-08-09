using Newtonsoft.Json.Linq;
using System.Linq;

namespace ExpandARM.Core
{
    public class ArmTemplate
    {
        public string FilePath { get; }

        public bool NeedsExpansion => OriginalContent.SelectTokens("$..templateLink").Any();

        public bool IsExpanded => !ExpandedContent.SelectTokens("$..templateLink").Any();

        public JObject OriginalContent { get; }

        public JObject ExpandedContent { get; set; }

        public string ExpandedFileName { get; internal set; }

        public ArmTemplate(string filePath, JObject jObject)
        {
            FilePath = filePath;
            OriginalContent = jObject;
            ExpandedContent = OriginalContent;
        }
    }
}
