using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE_API_V2.Utility
{
    public static class LanguageHelper
    {
        public static string[] GetAvailableLanguages()
        {
            // Returns static list for now, should read SchemaFiles
            return new string[] { "en-GB", "de-DE", "fr-FR" };
        }
    }
}