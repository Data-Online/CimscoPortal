using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models.Mapping
{
    public class EnergySupplierMap : EntityTypeConfiguration<EnergySupplier>
    {
        public EnergySupplierMap()
        {

            this.HasKey(t => t.SupplierId);

            this.Property(t => t.SupplierName)
                   .IsRequired()
                   .HasMaxLength(50);               
        }

    }
}
