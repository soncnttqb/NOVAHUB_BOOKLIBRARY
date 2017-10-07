using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class FileController : ApiController
    {
        [HttpGet]
        public string GetFolderImagePath()
        {
            return ConfigurationManager.AppSettings["ServerImageFolder"]?.ToString();
        }
    }
}
