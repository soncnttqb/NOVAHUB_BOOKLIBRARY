using APIBusiness.Business;
using APIBusiness.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class UserController : ApiController
    {
        UserBusiness _userBusiness = new UserBusiness();

        [HttpGet]
        public UserDTO Login([FromUri]string email, [FromUri]string password)
        {
            return _userBusiness.Login(email, password);
        }
    }
}
