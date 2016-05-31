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
        public int Month { get { return InvoiceDate.Month; } }
        public int Year { get { return InvoiceDate.Year; } }
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
        public DateTime InvoiceDate { get; set; }
        public DateTime KeyInvoiceDate { get { return new DateTime(InvoiceDate.Year, InvoiceDate.Month, 1); } }
    };
}