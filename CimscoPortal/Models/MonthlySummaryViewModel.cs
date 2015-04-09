using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{

    public class MonthlySummaryViewModel
    {
        public List<PieChartData> PieChartData { get; set; }
        public SummaryData SummaryData { get; set; }
    }

    public class PieChartData
    {
        public string label { get; set; }
        public decimal value { get; set; }
    }

    public class SummaryData
    {
        public string TotalCharge { get; set; }
        public string DueDate { get; set; }
        public Int16 Approved { get; set; }
        public string Month { get; set; }
    }
}

