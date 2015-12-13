using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class OtherCharge
    {

        [Key, ForeignKey("InvoiceSummary")]
        public int OtherChargeId { get; set; }

        // Service charges and rates
        public decimal BDSVC { get; set; }
        public decimal BDSVCR { get; set; }
        public decimal NBDSVC { get; set; }
        public decimal NBDSVCR { get; set; }

        // Levy
        public decimal EALevy { get; set; }
        public decimal EALevyR { get; set; }

        // Admin
        public decimal AdminCharge { get; set; }

        public virtual InvoiceSummary InvoiceSummary { get; set; }
    }
}
