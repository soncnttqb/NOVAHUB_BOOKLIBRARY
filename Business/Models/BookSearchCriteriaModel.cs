using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BookSearchCriteriaModel : BasePagingModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Filter { get; set; }
    }
}
