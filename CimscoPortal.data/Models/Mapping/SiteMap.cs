using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class SiteMap : EntityTypeConfiguration<Site>
    {
        public SiteMap()
        {
            // Primary Key
            this.HasKey(t => t.SiteId);

            // Properties
            this.Property(t => t.SiteName)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("Sites");
            this.Property(t => t.SiteId).HasColumnName("SiteId");
            this.Property(t => t.SiteName).HasColumnName("SiteName");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");

            // Relationships
            this.HasRequired(t => t.Group)
                .WithMany(t => t.Sites)
                .HasForeignKey(d => d.GroupId);

            this.HasRequired(t => t.Customer)
                .WithMany(t => t.Sites)
                .HasForeignKey(d => d.CustomerId);

        }
    }
}
