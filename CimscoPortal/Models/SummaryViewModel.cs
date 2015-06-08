using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class SummaryViewModel
    {
        public CustomerHierarchyViewModel CustomerHierarchy { get; set; }
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

    public class CustomerHierarchyViewModel
    {
        public List<CustomerData> CustomerData { get; set; }
        public string GroupName { get; set; }
    }

    public class CustomerData
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address1 { get; set; }
    }

    public class InvoiceDetail
    {
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public Int16 PercentChange { get; set; }
        public int InvoiceId { get; set; }
    }
}
