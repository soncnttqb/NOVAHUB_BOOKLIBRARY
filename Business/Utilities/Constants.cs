using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities
{
    public class Constants
    {
        public const string ErrorLogin = "Wrong email or password!\n Please re-enter to Login.";
        public const string WrongExtensionFile = "You must select a image file.";
        public struct ConfigKey
        {
            public const string TempFolder = "TempFolder";
            public const string FilterExtension = "FilterExtension";
            public const string BaseAddress = "BaseAddress";
            public const string PageSize = "PageSize";
            public const string Extensions = "Extensions";
            public const string Host = "Host";
        }
    }
}
