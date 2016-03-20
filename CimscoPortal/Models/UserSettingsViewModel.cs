using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CimscoPortal.Models
{
    public class UserSettingsViewModel
    {
        public List<int> MonthSpanOptions { get { return new List<int> { 3, 6, 12, 24 }; } }
        public int MonthSpan { get; set; }
    }
}
