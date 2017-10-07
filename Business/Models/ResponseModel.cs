using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Utilities.Enums;

namespace Business.Models
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public ResponseCode ResponseCode { get; set; }
    }
}
