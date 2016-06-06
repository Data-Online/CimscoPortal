using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    //public class InvoiceDetailViewModel_zz
    //{
    //    public DonutChartViewModel ChartData { get; set; }
    //    public List<EnergyDataModel> EnergyCostData { get; set; }
    //}

    public class DashboardViewData
    {
        public ByMonthViewModel Cost { get; set; }
        public ByMonthViewModel Consumption { get; set; }
        public int TotalSites { get; set; }
    }

    public class ByMonthViewModel
    {
        public int MonthsOfData { get; set; }
        public List<string> Months { get; set; }
        public List<int> Years { get; set; }
        public List<decimal> Values { get; set; }
        public List<decimal> Values12 { get; set; }
        public List<int> TotalInvoices { get; set; }
        public List<int> TotalInvoices12 { get; set; }
    }
}
