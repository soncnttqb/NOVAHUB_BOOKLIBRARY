using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIBusiness.DataTransferObject
{
    public class BookSearchCriteriaDTO : BaseSearchCriteriaDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Year { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
    }
}
