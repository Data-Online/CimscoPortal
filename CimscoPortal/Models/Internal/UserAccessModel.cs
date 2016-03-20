using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CimscoPortal.Models
{
    public class UserAccessModel
    {
        public bool ViewInvoices { get; set; }
        public List<int> ValidSites { get; set; }
        public bool CanApproveInvoices { get; set; }
    }
}
