using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models.Mapping
{
    public class EnergyPointMap : EntityTypeConfiguration<EnergyPoint>
    {
        public EnergyPointMap()
        {

            this.HasKey(t => t.EnergyPointId);

            this.Property(t => t.EnergyPointNumber)
                   .IsRequired()
                   .HasMaxLength(30);               
        }

    }
}
