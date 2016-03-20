using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CimscoPortal.Models
{
    public class MonthlyConsumptionViewModal
    {
        public Decimal ConsumptionBusinessDay { get; set; }
        public Decimal ConsumptionNonBusinessDay { get; set; }
        public int TotalConsumption { get { return (int)Math.Round(ConsumptionBusinessDay + ConsumptionNonBusinessDay); } }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public Decimal CostBusinessDay { get; set; }
        public Decimal CostNonBusinessDay { get; set; }
        public int TotalCost { get { return (int)Math.Round(CostBusinessDay + CostNonBusinessDay); } }
        public string Month { get { return InvoiceDate.ToString("MMM-yy", CultureInfo.InvariantCulture); } }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int Days { get {  return (int)((PeriodEnd - PeriodStart).TotalDays + 1);} }
        public int AvgCost { get { return (int)Math.Round(((decimal)(TotalCost / Days))); } }
        public int AvgConsumption { get { return (int)Math.Round(((decimal)(TotalConsumption / Days))); } }

    }
}
