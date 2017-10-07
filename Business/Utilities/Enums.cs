using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities
{
    public class Enums
    {
        public enum MessageBoxCaption
        {
            Error = 1,
            Information = 2,
            Confirmation = 3
        }
        public enum RoleTpe
        {
            Admin = 1,
            User = 2
        }
        public enum ResponseCode
        {
            Success = 1,
            Duplicate = 2,
            NotExist = 3,
            InUse = 4,
            Error = 5
        }
        public enum RequestType
        {
            Get = 1,
            Post = 2,
            Put = 3,
            Delete = 4
        }
    }
}
