using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models.Mapping
{
    public class WelcomeScreenMap : EntityTypeConfiguration<WelcomeScreen>
    {
        public WelcomeScreenMap()
        {
            // Primary key
            this.HasKey(t => t.WelcomeId);

            this.Property(t => t.Initial)
                .HasMaxLength(250);
            this.Property(t => t.Line2)
                .HasMaxLength(250);
            this.Property(t => t.Line3)
                .HasMaxLength(250);
            this.Property(t => t.Line4)
                .HasMaxLength(250);
            this.Property(t => t.Line5)
                .HasMaxLength(250);

            this.ToTable("WelcomeScreen");
        }
    }
}
