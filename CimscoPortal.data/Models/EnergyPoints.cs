using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class EnergyPoint
    {
        public EnergyPoint()
        { }
        public int EnergyPointId { get; set; }

        [Index(IsUnique = true)]
        public string EnergyPointNumber { get; set; }

       // public virtual InvoiceSummary InvoiceSummary { get; set; }
    }
}
