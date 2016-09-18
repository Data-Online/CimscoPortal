using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CimscoPortal.Models
{
    public class CostConsumptionOptions
    {
        public bool previous12 { get; set; }
        public string userId { get; set; }
        public int siteId { get; set; }
        public string filter { get; set; }
        public bool includeMissing { get; set; }
    }
}
