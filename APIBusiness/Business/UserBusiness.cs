using APIBusiness.DataTransferObject;
using APIBusiness.Utilities;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace APIBusiness.Business
{
    public class UserBusiness : BaseBusiness
    {
        public UserDTO Login(string email, string password)
        {
            User user = UnitOfWork.UserRepository.GetAll().Where(x => x.Email.Equals(email) && x.Password.Equals(password)).FirstOrDefault();
            return Mapper.ToDTO(user);
        }
    }
}
