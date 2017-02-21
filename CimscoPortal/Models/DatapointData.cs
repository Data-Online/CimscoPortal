using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class DatapointIdentity
    {
        public string name { get; set; }
        public string filter { get; set; }
        public string date { get; set; }
        //public int line { get; set; }
    }

    public class DatapointDetailView
    {
        public string Status { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }

        public IList<DatapointSiteDetails> SitesMissingInvoices { get; set; }
    }

    public class DatapointSiteDetails
    {
        public string SiteName { get; set; }
        public int SiteId { get; set; }
    }
}
