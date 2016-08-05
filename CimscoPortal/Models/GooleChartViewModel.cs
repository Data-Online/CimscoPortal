using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class GoogleChartViewModel
    {
        public IList<GoogleCols> Columns { get; set; }
        public IList<GoogleRows> Rows { get; set; }
    }

    public class GoogleCols
    {
       // public string id { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public string role { get; set; }
    }

    public class GoogleRows
    {
        public IList<CPart> Cparts { get; set; }
    }

    public class CPart
    {
        public string v { get; set;}
        public string f { get; set;}
    }
}
