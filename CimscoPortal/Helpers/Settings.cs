using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CimscoPortal.Helpers
{
    public class Settings
    {
        public static string DefaultLogo => ConfigurationManager.AppSettings["DefaultLogo"];
    }
}