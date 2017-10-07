using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIBusiness.DataTransferObject
{
    public class BookSearchResultDTO
    {
        public int Total { get; set; }
        public List<BookDTO> Results { get; set; }
    }
}