using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities
{
    public static class NetWorkHelper
    {
        public static bool IsNetWorkAvailable()
        {
            string host = ConfigurationManager.AppSettings[Constants.ConfigKey.Host].ToString();
            try
            {
                return (new Ping()).Send(host).Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
