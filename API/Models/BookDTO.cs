using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace API
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CoverPhoto { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> AuthorId { get; set; }

        public  AuthorDTO Author { get; set; }
        public  CategoryDTO Category { get; set; }
    }
}