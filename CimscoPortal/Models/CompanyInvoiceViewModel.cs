using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class CompanyInvoiceViewModel
    {
        public int SiteId { get; set; }
        public int Index { get; set; }
        public string Month { get; set; }
        public string YearA { get; set; }
        public string YearB { get; set; }
    }
}
