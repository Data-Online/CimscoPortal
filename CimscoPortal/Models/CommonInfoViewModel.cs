using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CimscoPortal.Models
{
    public class CommonInfoViewModel
    {
        public UsefulInfo UsefulInfo { get; set; }
        public string FullName { get; set; }
        public string eMail { get; set; }
        public string CompanyLogo { get; set; }

    }

    public class UsefulInfo
    {
        public string Temperature { get; set; }
        public DateTime Today { get { return DateTime.Now; } }
        public string WeatherIcon { get; set; }
    }
}
