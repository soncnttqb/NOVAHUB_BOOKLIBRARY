using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIBusiness.DataTransferObject
{
    public class BaseSearchCriteriaDTO
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
