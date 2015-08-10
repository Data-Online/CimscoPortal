using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models.Mapping
{
    public class EnergyChargeMap : EntityTypeConfiguration<EnergyCharge>
    {
        public EnergyChargeMap()
        {
           // this.HasKey(t => t.InvoiceSummary);

            this.ToTable("EnergyCharges");
            //this.Property(t => t.EnergyChargeId).HasColumnName("EnergyChargeId");
            //this.Property(t => t.LossRate).HasPrecision(8, 6);
          //  this.HasRequired(o => o.InvoiceSummary);
        }
    }
}
