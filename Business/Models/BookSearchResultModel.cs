using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BookSearchResultModel
    {
        public int Total { get; set; }
        public List<BookModel> Results { get; set; }
    }
}
