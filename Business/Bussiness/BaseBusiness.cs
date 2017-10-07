using Business.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Business.Utilities.Enums;

namespace Business.Bussiness
{
    public class BaseBusiness
    {
        public T DoRequest<T, K>(string url, RequestType requestType, K model)
        {
            if (!NetWorkHelper.IsNetWorkAvailable())
            {
                MessageBox.Show("Opp! Lose internet connection.\n Application will exit now.",
                    Enums.MessageBoxCaption.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = null;
                switch (requestType)
                {
                    case RequestType.Get:
                        response = client.GetAsync(url).Result;
                        break;
                    case RequestType.Post:
                        response = client.PostAsJsonAsync(url, model).Result;
                        break;
                    case RequestType.Put:
                        response = client.PutAsJsonAsync(url, model).Result;
                        break;
                    case RequestType.Delete:
                        response = client.DeleteAsync(url).Result;
                        break;
                }

                if (response != null && response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<T>().Result;
                }
            }
            return Activator.CreateInstance<T>();// default(T);
        }

        public string GetFolderImagePath()
        {
            return DoRequest<string, string>("api/File/GetFolderImagePath", RequestType.Get, null);
        }
    }
}
