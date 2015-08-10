using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models.Mapping
{
    public class NetworkChargeMap : EntityTypeConfiguration<NetworkCharge>
    {
        public NetworkChargeMap()
        {
           // this.HasKey(t => t.InvoiceSummary);

            this.ToTable("NetworkCharges");
            //this.Property(t => t.EnergyChargeId).HasColumnName("EnergyChargeId");

          //  this.HasRequired(o => o.InvoiceSummary);
        }
    }
}
