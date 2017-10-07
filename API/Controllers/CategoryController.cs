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
    public class CategoryController : ApiController
    {
        CategoryBusiness _categoryBusiness = new CategoryBusiness();

        [HttpGet]
        public CategorySearchResultDTO Search([FromUri] int pagesize, [FromUri] int pageindex)
        {
            return _categoryBusiness.Search(new CategorySearchCriteriaDTO() { PageSize = pagesize, PageIndex = pageindex });
        }

        [HttpGet]
        public List<CategoryDTO> GetAll()
        {
            return _categoryBusiness.GetAll();
        }

        [HttpGet]
        public CategoryDTO Get(int id)
        {
            return _categoryBusiness.Get(id);
        }

        [HttpPost]
        public ResponseDTO Add(CategoryDTO dto)
        {
            return _categoryBusiness.Add(dto);
        }

        [HttpPut]
        public ResponseDTO Update(CategoryDTO dto)
        {
            return _categoryBusiness.Update(dto);
        }
        [HttpDelete]
        public ResponseDTO Delete(int id)
        {
            return _categoryBusiness.Delete(id);
        }
    }
}
