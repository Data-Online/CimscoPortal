using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class StackedBarChartViewModel //: IEnumerable<EnergyData>
    {
        public List<EnergyData> MonthlyData { get; set; }
        public BarChartSummaryData BarChartSummaryData { get; set; }
    }

    public class BarChartSummaryData
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public decimal PercentChange { get; set; }
    }

    public class EnergyData
    {
        public string Month { get { return InvoiceDate.ToString("MMM");} }
        public DateTime InvoiceDate { get; set; }
        public decimal Energy { get; set; }
        public decimal Line { get; set; }
        public decimal Other { get; set; }
        public decimal TotalCharge { get { return Energy + Line + Other; } }
    }

    public class StackedBarChartViewModelB //: IEnumerable<EnergyData>
    {
        public List<EnergyDataB> MonthlyData { get; set; }
    }

    public class EnergyDataB
    {
        public string Month { get; set; }
        public string Energy { get; set; }
        public string Line { get; set; }
        public string Other { get; set; }
    }

}
;
