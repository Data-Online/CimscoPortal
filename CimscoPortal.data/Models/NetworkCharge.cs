using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class NetworkCharge
    {

        [Key, ForeignKey("InvoiceSummary")]
        public int NetworkChargeId { get; set; }

        public decimal VariableBD { get; set; }
        public decimal VariableNBD { get; set; }
        public decimal CapacityCharge { get; set; }
        public decimal DemandCharge { get; set; }
        public decimal FixedCharge { get; set; }

        // Calculated values
        public decimal TotalCharge
        { get { return VariableBD + VariableNBD + CapacityCharge + DemandCharge + FixedCharge; } }

        public virtual InvoiceSummary InvoiceSummary { get; set; }
    }
}
