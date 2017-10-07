using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class BookSearchCriteriaModel : BasePagingModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public string Publisher { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
    }
}
