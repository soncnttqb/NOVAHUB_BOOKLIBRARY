using Business.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities
{
    public static class Utils
    {
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
        public static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return hashOfInput.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }

        internal static void ConfigHttpClient(HttpClient client)
        {
            string baseAddress = ConfigurationManager.AppSettings[Constants.ConfigKey.BaseAddress];
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static List<SelectionItem> GetListYears()
        {
            List<SelectionItem> list = new List<SelectionItem>();
            list.Add(new SelectionItem() { ValueMember = 0, DisplayMember = "" });
            for (int i = 2000; i <= DateTime.Now.Year; i++)
            {
                list.Add(new SelectionItem() { ValueMember = i, DisplayMember = i });
            }
            return list;
        }
    }
}
