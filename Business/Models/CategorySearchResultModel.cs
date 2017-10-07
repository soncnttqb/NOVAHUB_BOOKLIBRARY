using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class CategorySearchResultModel
    {
        public int Total { get; set; }
        public List<CategoryModel> Results { get; set; }
    }
}
