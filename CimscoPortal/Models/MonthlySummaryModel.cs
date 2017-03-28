using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CimscoPortal.Models
{
    public class MonthlySummaryModel
    {
        //public int SiteId { get; set; }
        public int Month { get { return InvoicePeriodDate.Month; } }
        public int Year { get { return InvoicePeriodDate.Year; } }
        public string MonthName
        {
            get
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(this.Month);
            }
        }
        public decimal InvoiceTotal { get; set; }
        public decimal EnergyTotal { get; set; }
        public int TotalInvoices { get; set; }
        public DateTime InvoicePeriodDate { get; set; }
        public DateTime InvoiceKeyDate { get { return new DateTime(InvoicePeriodDate.Year, InvoicePeriodDate.Month, 1); } }
        // New
        public bool Missing { get; set; }
        public bool Verified { get; set; }
        public bool Approved { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ApproversName { get; set; }
        public decimal PercentageChange { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int InvoiceId { get; set; }
        public bool InvoicePdf { get; set; }
        public string BlobUri { get; set; }

       // public string _uniqueID { get { return InvoicePeriodDate.ToString("MM-YY-") + SiteId.ToString(); } }
    };
}