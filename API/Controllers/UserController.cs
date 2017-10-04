using Entities;
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
        UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        public List<UserDTO> GetAllUsers()
        {
            return (from us in _unitOfWork.UserRepository.GetAll()
                         select new UserDTO()
                         {
                             Id = us.Id,
                             Password = us.Password,
                             Email = us.Email,
                             FirstName = us.FirstName,
                             LastName = us.LastName
                         }).ToList();
        }

        [HttpGet]
        public UserDTO Login([FromUri]string email, [FromUri]string password)
        {
            return (from us in _unitOfWork.UserRepository.GetAll()
                    select new UserDTO()
                    {
                        Id = us.Id,
                        Password = us.Password,
                        Email = us.Email,
                        FirstName = us.FirstName,
                        LastName = us.LastName,
                        RoleType = us.Role.RoleType
                    }).Where(x=>x.Email.Equals(email) && x.Password.Equals(password)).FirstOrDefault();
        }
    }
}
