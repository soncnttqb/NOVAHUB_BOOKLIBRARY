using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIBusiness.Utilities
{
    public class Enums
    {
        public enum ResponseCode
        {
            Success = 1,
            Duplicate = 2,
            NotExist = 3,
            InUse = 4,
            Error = 5
        }
    }
}
