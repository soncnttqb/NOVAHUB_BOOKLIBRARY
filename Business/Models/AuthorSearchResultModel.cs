using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class AuthorSearchResultModel
    {
        public int Total { get; set; }
        public List<AuthorModel> Results { get; set; }
    }
}
