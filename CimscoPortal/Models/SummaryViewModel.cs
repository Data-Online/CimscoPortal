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
        public List<CompanyInvoiceViewModel2> InvoiceHistory { get; set; }  //refactor:rename
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

    public class InvoiceDetail2
    {
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public Int16 PercentChange { get; set; }
        public int InvoiceId { get; set; }
    }

    public class InvoiceDetail
    {
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal MiscChargesTotal { get; set; }
        public decimal NetworkChargesTotal { get; set; }
        public decimal EnergyChargesTotal { get; set; }
        public decimal PercentageChange { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public bool Approved { get; set; }
        public string ApproversName { get; set;  }
        public DateTime ApprovedDate { get; set; }
    }

    public class SiteHierarchyViewModel
    {
        public List<SiteData> SiteData { get; set; }
        public string HeaderName { get; set; }
    }

    public class SiteData
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int Index { get; set; }
    }


}
