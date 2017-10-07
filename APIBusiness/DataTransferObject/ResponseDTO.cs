using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static APIBusiness.Utilities.Enums;

namespace APIBusiness.DataTransferObject
{
    public class ResponseDTO
    {
        public string Message { get; set; }
        public ResponseCode ResponseCode { get; set; }
    }
}