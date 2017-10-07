
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
    public class BookController : ApiController
    {
        BookBusiness _bookBusiness = new BookBusiness();
        [HttpGet]
        public BookSearchResultDTO Search([FromUri] string title,
            [FromUri] string description,
            [FromUri] string publisher,
            [FromUri] string year,
            [FromUri] string category,
            [FromUri] string author,
            [FromUri] int pagesize,
            [FromUri] int pageindex)
        {
            return _bookBusiness.Search(new BookSearchCriteriaDTO()
            {
                Title = title,
                Description = description,
                Publisher = publisher,
                Year = year,
                Category = category,
                Author = author,
                PageSize = pagesize,
                PageIndex = pageindex
            });
        }

        [HttpGet]
        public BookDTO Get(int id)
        {
            return _bookBusiness.Get(id);
        }

        [HttpPost]
        public ResponseDTO Add(BookDTO dto)
        {
            return _bookBusiness.Add(dto);
        }

        [HttpPut]
        public ResponseDTO Update(BookDTO dto)
        {
            return _bookBusiness.Update(dto);
        }

        [HttpDelete]
        public ResponseDTO Delete(int id)
        {
            return _bookBusiness.Delete(id);
        }
    }
}
