using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{

    public class DetailBySiteViewModel
    {
        public List<string> Divisions { get; set; }
        public int MaxTotalInvoices { get; set; }
        public List<SiteDetailData> SiteDetailData { get; set; }
    }
    
    public class SiteDetailData
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int? GroupDivisionId { get; set; }
        public string DivisionName { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string CustomerName { get; set; }

        public int MonthsOfData { get; set; }

        public InvoiceKeyData InvoiceKeyData { get; set; }
        public InvoiceCosts_ InvoiceCosts { get; set; }
    }
    public class InvoiceKeyData
    {
        public int ApprovedInvoices { get; set; }
        public int PendingInvoices { get; set; }
        public int MissingInvoices { get; set; }
        public DateTime FirstInvoiceDate { get; set; }
        public DateTime LatestInvoiceDate { get; set; }
        public int TotalInvoicesOnFile { get { return ApprovedInvoices + PendingInvoices; } }
        public int TotalInvoices { get { return TotalInvoicesOnFile + MissingInvoices; } }
        public decimal CalculatedLossRate { get; set; }
    }

    public class InvoiceCosts_
    {
        public Decimal InvoiceValue { get; set; }
        public Decimal EnergyCharge { get; set; }
        public Decimal TotalKwh { get; set; }
        public int? SiteArea { get; set; }
        public Decimal CostPerSqm { get { return EnergyCharge / Math.Max((SiteArea ?? 0), 1) * 1.0M; } }
        public Decimal UnitsPerSqm { get { return TotalKwh / Math.Max((SiteArea ?? 0), 1) * 1.0M; } }
    }
}
