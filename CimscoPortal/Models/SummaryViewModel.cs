using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class SummaryViewModel
    {
      //  public CustomerHierarchyViewModel CustomerHierarchy { get; set; }
        public SiteHierarchyViewModel SiteHierarchy { get; set; }
        public List<InvoiceDataForCompany> SummaryData { get; set; }
        public List<InvoiceDetail> InvoicesDue { get; set; }
        public int MaxValue { get; set; }
    }

    public class InvoiceDataForCompany
    {
        public List<CompanyInvoiceViewModel> InvoiceHistory { get; set; }  //refactor:rename
        public List<InvoiceDetail> InvoicesDue { get; set; }
        public int Year { get; set; }
    }

    //public class CustomerHierarchyViewModel
    //{
    //    public List<CustomerData> CustomerData { get; set; }
    //    public string GroupName { get; set; }
    //}

    public class CustomerData
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address1 { get; set; }
    }

    public class InvoiceOverviewViewModel
    {
        public DateTime InvoiceDueDate { get; set; }
        public bool Approved { get; set; }
        public bool Verified { get; set; }
        public bool Missing { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ApproversName { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal PercentageChange { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public int SiteId { get; set; }
        public int InvoiceId { get; set; }
        public bool InvoicePdf { get; set; }
        public DateTime InvoiceKeyDate { get; set; }
    }

    public class InvoiceDetail
    {
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public DateTime PeriodEnd { get; set; }
        public DateTime InvoiceKeyDate { get { return new DateTime(PeriodEnd.Year, PeriodEnd.Month, 1); } }
        public decimal InvoiceTotal { get; set; }
        public decimal MiscChargesTotal { get; set; }
        public decimal NetworkChargesTotal { get; set; }
        public decimal EnergyChargesTotal { get; set; }
        public decimal PercentageChange { get; set; }
        public decimal GstTotal { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public bool Approved { get; set; }
        public string ApproversName { get; set;  }
        public DateTime ApprovedDate { get; set; }
        public decimal LossRate { get; set; }
        public decimal BDLossCharge { get; set; }
        public decimal BDMeteredKwh { get; set; }
        public bool ValidationError { get { return InvoiceTotal != (MiscChargesTotal + NetworkChargesTotal + EnergyChargesTotal); } }
        public int SiteId { get; set; }
        public bool InvoicePdf { get; set; }
        public bool OnFile { get; private set; }
        public string MiscChargesLabel { get { return "Other"; } }
        public string NetworkChargesLabel { get { return "Delivery"; } }
        public string EnergyChargesLabel { get { return "Energy"; } }      
       // public string PdfSourceLocation { get; set; }
    }

    public class SiteHierarchyViewModel
    {
        public List<SiteData> SiteData { get; set; }
        public string UserLevel { get; set; }
       // public string GroupCompanyName { get; set; }
        public string TopLevelName { get; set; }    // Based on user assignment at Site, Group or Company level
        public string GroupName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
    }

    public class SiteData
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> GroupId { get; set; }
   //     public int Index { get; set; }
    }

}
