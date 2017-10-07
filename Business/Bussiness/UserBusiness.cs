using Business.Models;
using Business.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Business.Bussiness
{
    public class UserBusiness : BaseBusiness
    {
        public GenericPrincipal Login(UserModel user)
        {
            string passHash = Utils.GetMd5Hash(user.Password);
            string url = "api/User/Login?email=" + user.Email + "&password=" + passHash;
            UserModel result = DoRequest<UserModel, UserModel>(url, Enums.RequestType.Get, null);
            if (result != null)
            {
                return new GenericPrincipal(new GenericIdentity(result.Email), new string[] { result.RoleType });
            }
            return null;
        }
    }
}
