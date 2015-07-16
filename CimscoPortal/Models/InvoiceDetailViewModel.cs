using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class InvoiceDetailViewModel
    {
        public DonutChartViewModel ChartData { get; set; }
        public List<EnergyDataModel> EnergyCostData { get; set; }
    }

    public class InvoiceDetailViewModel_
    {
        public List<DonutChartData> DonutChartData { get; set; }
        //public List<EnergyDataModel> EnergyCostData { get; set; }
        public EnergyCosts EnergyCosts { get; set; }
        public InvoiceDetail InvoiceDetail { get; set; }
    }

    public class EnergyCosts
    {
        public List<EnergyDataModel> EnergyCostSeries { get; set; }
    }
}
