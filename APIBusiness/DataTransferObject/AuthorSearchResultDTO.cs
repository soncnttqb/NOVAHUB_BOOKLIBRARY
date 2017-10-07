using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIBusiness.DataTransferObject
{
    public class AuthorSearchResultDTO
    {
        public int Total { get; set; }
        public List<AuthorDTO> Results { get; set; }
    }
}