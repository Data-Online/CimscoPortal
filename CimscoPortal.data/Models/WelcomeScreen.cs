using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class WelcomeScreen
    {
        public WelcomeScreen()
        {

        }

        public int WelcomeId { get; set; }
        public string Initial { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
    }
}
