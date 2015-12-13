using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models.Mapping
{
    public class OtherChargeMap : EntityTypeConfiguration<OtherCharge>
    {
        public OtherChargeMap()
        {
           // this.HasKey(t => t.InvoiceSummary);

            this.ToTable("OtherCharges");
            //this.Property(t => t.EnergyChargeId).HasColumnName("EnergyChargeId");

          //  this.HasRequired(o => o.InvoiceSummary);
        }
    }
}
