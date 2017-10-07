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
    public class AuthorController : ApiController
    {
        AuthorBusiness _authorBusiness = new AuthorBusiness();

        [HttpGet]
        public AuthorSearchResultDTO Search([FromUri] int pagesize, [FromUri] int pageindex)
        {
            return _authorBusiness.Search(new AuthorSearchCriteriaDTO() { PageSize = pagesize, PageIndex = pageindex });
        }

        [HttpGet]
        public List<AuthorDTO> GetAll()
        {
            return _authorBusiness.GetAll();
        }

        [HttpGet]
        public AuthorDTO Get(int id)
        {
            return _authorBusiness.Get(id);
        }

        [HttpPost]
        public ResponseDTO Add(AuthorDTO dto)
        {
            return _authorBusiness.Add(dto);
        }

        [HttpPut]
        public ResponseDTO Update(AuthorDTO dto)
        {
            return _authorBusiness.Update(dto);
        }

        [HttpDelete]
        public ResponseDTO Delete(int id)
        {
            return _authorBusiness.Delete(id);
        }
    }
}
