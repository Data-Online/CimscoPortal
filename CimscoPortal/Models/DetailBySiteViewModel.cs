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
        public int TotalInvoicesToApprove { get; set; }
    }
    
    public class SiteDetailData
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int? TotalFloorSpaceSqMeters { get; set; }
        public int? GroupDivisionId { get; set; }
        public string DivisionName { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string CustomerName { get; set; }

        public int MonthsOfData { get; set; }

        public InvoiceKeyData InvoiceKeyData { get; set; }
        public InvoiceCosts_ InvoiceCosts { get; set; }

        public HistoryData InvoiceHistory { get; set; }
        public HistoryData KwhHistory { get; set; }
        public HistoryData CostPerSqmHistory { get; set; }
        public HistoryData UnitsPerSqmHistory { get; set; }
    }
    public class InvoiceKeyData
    {
        public int Approved { get; set; }
        public int Pending { get; set; }
        public int Missing { get; set; }
        public decimal ApprovedByPercent { get; set; }
        public decimal PendingByPercent { get; set; }
        public decimal MissingByPercent { get; set; }
        public DateTime FirstInvoiceDate { get; set; }
        public DateTime LatestInvoiceDate { get; set; }
        public int TotalInvoicesOnFile { get { return Approved + Pending; } }
        public int TotalInvoices { get { return TotalInvoicesOnFile + Missing; } }
        public decimal CalculatedLossRate { get; set; }
    }

    public class HistoryData
    {
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:C}")]
        public IEnumerable<decimal> Totals { get; set; }
    }

    public class InvoiceCosts_
    {
        public Decimal InvoiceValue { get; set; }
        public Decimal EnergyCharge { get; set; }
        public Decimal EnergyChargeByPercent { get; set; }
        public Decimal EnergyChargeByPercentForDivision { get; set; }
        public Decimal KwhByPercent { get; set; }
        public Decimal KwhByPercentForDivision { get; set; }
        public Decimal EnergyChargePerSqmByPercent { get; set; }
        //public Decimal CostPerSqmByPercentForDivision { get; set; }
        public Decimal EnergyChargePerSqmByPercentForDivision { get; set; }
        public Decimal KwhPerSqmByPercent { get; set; }
        public Decimal KwhPerSqmByPercentForDivision { get; set; }
        public Decimal TotalKwh { get; set; }
        public int? SiteArea { get; set; }
        //public Decimal CostPerSqm { get { return EnergyCharge / Math.Max((SiteArea ?? 0), 1) * 1.0M; } }
        public Decimal EnergyChargePerSqm { get {
            if ((SiteArea ?? 0) > 0)
            {
                return (EnergyCharge / SiteArea ?? 0) * 1.0M;
            }
            return 0.0M;
            } }
        //public Decimal KwhPerSqm { get { return TotalKwh / Math.Max((SiteArea ?? 0), 1) * 1.0M; } }
        public Decimal KwhPerSqm
        {
            get
            {
                if ((SiteArea ?? 0) > 0)
                {
                    return (TotalKwh / SiteArea ?? 0) * 1.0M;
                }
                return 0.0M;
            }
        }
    }

    public class CollatedInvoiceTotals
    {
        public int siteId { get; set; }
        public decimal invoiceTotal { get; set; }
        public decimal invoiceValue { get; set; }
        public decimal energyCharge { get; set; }
        public decimal totalKwh { get; set; }
        public int? siteArea { get; set; }
    }
}
