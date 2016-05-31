using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CimscoPortal.Models
{
    public class InvoiceTallyViewModel
    {
        
        public int MonthsOfData { get; set; }
        public List<InvoiceTally> InvoiceTallies { get; set; }
        public List<InvoiceCosts> InvoiceCosts { get; set; }
        public GroupCompanyDetail GroupCompanyDetail { get; set; }
        public List<CustomerHeader> CustomerList { get; set; }    
    }

    public class CustomerHeader
    {
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
    }

    public class GroupCompanyDetail
    {
        public string GroupName { get; set; }
     //   public string GroupCompanyName { get; set; }
        public string TopLevelName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
    }

    public class InvoiceTally
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> GroupId { get; set; }
        public int ApprovedInvoices { get; set; }
        public int PendingInvoices { get; set; }
        public int MissingInvoices { get; set; }
        //public List<InvoiceStats> InvoiceStats { get; set; }
        public DateTime FirstInvoiceDate { get; set; }
        public DateTime LatestInvoiceDate { get; set; }
        public int TotalInvoicesOnFile { get { return ApprovedInvoices + PendingInvoices; } }
        public int TotalInvoices { get { return TotalInvoicesOnFile + MissingInvoices;  } }
        public decimal CalculatedLossRate { get; set; }
    }

    public class InvoiceCosts
    {
        public int SiteId { get; set; }
        public Decimal InvoiceValue { get; set; }
        public Decimal EnergyCharge { get; set; }
        public Decimal TotalKwh { get; set; }
        public int? SiteArea { get; set; }
        public Decimal CostPerSqm { get { return EnergyCharge / Math.Max((SiteArea ?? 0),1)*1.0M; } }
        public Decimal UnitsPerSqm { get { return TotalKwh / Math.Max((SiteArea ?? 0), 1)*1.0M; } }
    }

    //public class InvoiceStats
    //{
       
    //    public int noOfInv { get; set; }
    //    public int percent { get; set; }
    //}
}
