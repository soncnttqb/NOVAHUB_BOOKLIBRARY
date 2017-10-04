using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public static class UserBusiness
    {

        public static GenericPrincipal Login(UserModel user)
        {
            string passHash = Utils.GetMd5Hash(user.Password);
            using (var client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);

                HttpResponseMessage response = client.GetAsync("api/User/Login?email=" + user.Email + "&password=" + passHash).Result;
                if (response.IsSuccessStatusCode)
                {
                    UserModel result = response.Content.ReadAsAsync<UserModel>().Result;
                    if (result != null)
                    {
                        return new GenericPrincipal(new GenericIdentity(result.Email), new string[] { result.RoleType });
                    }
                }
            }
            return null;
        }
    }
}
