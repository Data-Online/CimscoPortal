using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class InvoiceStatsBySiteViewModel
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int Missing { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int TotalInvoices { get; set; }
        public int TotalInvoicesOnFile { get; set; }
        public decimal ApprovedByPercent { get; set; }
        public decimal PendingByPercent { get; set; }
        public decimal MissingByPercent { get; set; }
    }

    //public class InvoiceStatsBySiteViewModel_
    //{
    //    public List<SiteSummary> SiteSummary { get; set; }
    //}

    //public class SiteSummary
    //{
    //    public int SiteId { get; set; }
    //    public string SiteName { get; set; }
    //    public int MissingInvoices { get; set; }
    //    public int PendingInvoices { get; set; }
    //    public int TotalInvoices { get; set; }
    //    public int TotalInvoicesOnFile { get; set; }
    //}
}
