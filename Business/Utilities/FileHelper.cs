using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
   public static class FileHelper
    {
        public static string FilterExtension { get { return ConfigurationManager.AppSettings["FilterExtension"].ToString(); } }
        public static bool IsValidExtensionFile(string extension)
        {
            List<string> listExtension = ConfigurationManager.AppSettings["Extensions"].Split(new char[] { '|' }).Select(x => x.ToUpper()).ToList();
            return listExtension.Contains(extension.ToUpper());
        }
    }
}
