using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class SystemConfigurationMap : EntityTypeConfiguration<SystemConfiguration>
    {
        public SystemConfigurationMap()
        {
        // Primary Key
            this.HasKey(t => t.SystemConfigurationId);

            this.Property(t => t.Key)
                .HasMaxLength(50);

            this.Property(t => t.Values)
                .HasMaxLength(200);

            this.ToTable("SystemConfiguration");
        }
    }
}
