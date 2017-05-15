using CimscoPortal.Helpers;
using System;
using System.ComponentModel;

namespace CimscoPortal.Models
{
    public class CommonInfoViewModel
    {
        public UsefulInfo UsefulInfo { get; set; }
        public string FullName { get; set; }
        public string eMail { get; set; }
        public string CompanyLogo { get; set; }
        public string TopLevelName { get; set; }

        public CommonInfoViewModel()
        {
            CompanyLogo = Settings.DefaultLogo;
        }
    }

    public class UsefulInfo
    {
        public string Temperature { get; set; }
        public DateTime Today { get { return DateTime.Now; } }
        public string WeatherIcon { get; set; }
    }
}
