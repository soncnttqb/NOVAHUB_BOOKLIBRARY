using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace APIBusiness.DataTransferObject
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CoverPhoto { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int? Year { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }

        public  AuthorDTO Author { get; set; }
        public  CategoryDTO Category { get; set; }
    }
}