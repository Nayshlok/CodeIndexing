using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeIndexing.Parser
{
    public class Keywords
    {
        public static readonly List<string> keywords = new List<string>
        {
            "namespace",
            "class",
        };

        public static readonly IEnumerable<string> signatureKeywords = new List<string>
        {
            "public",
            "private",
            "internal",
            "static",
            "readonly",
        };
    }
}
