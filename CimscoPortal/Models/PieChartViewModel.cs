using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{

    class PieChartViewModel
    {
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string label { get; set; }
        public decimal data { get; set; }
        public Bars bars { get; set; }
    }

    public class Bars
    {
        public bool show { get; set; }
        public int order { get; set; }
        public Fillcolor fillColor { get; set; }
    }

    public class Fillcolor
    {
        public Color[] colors { get; set; }
    }

    public class Color
    {
        public string color { get; set; }
    }

    public class Datalist
    {
        public Values[] values { get; set; }
    }
    public class Values
    {
        public int reading { get; set; }
    }

    public class StackedBarChartViewModel
    {
        public EnergyData[] MonthlyData { get; set; }
    }

    public class EnergyData
    {
        public string Month { get; set; }
        public decimal Energy { get; set; }
        public decimal Line { get; set; }
        public decimal Other { get; set; }
    }

}
