using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CimscoPortal.Models
{
    public class CommonInfoViewModel
    {
        public string Temperature { get; set; }
        public DateTime Today { get { return DateTime.Now; } }
        public string WeatherIcon { get; set; }
    }
}
