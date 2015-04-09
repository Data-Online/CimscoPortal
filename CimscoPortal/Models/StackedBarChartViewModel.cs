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
    }

    public class EnergyData
    {
        public string Month { get; set; }
        public decimal Energy { get; set; }
        public decimal Line { get; set; }
        public decimal Other { get; set; }
    }
}
;
