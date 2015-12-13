using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class EnergySupplier
    {
        public EnergySupplier()
        { }
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }
    }
}
