using Business.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
   public class BasePagingModel
    {
        public int PageIndex { get; set; }
        public int Total { get; set; }
        public int PageSize { get { return int.Parse(ConfigurationManager.AppSettings[Constants.ConfigKey.PageSize].ToString()); } }
    }
}
