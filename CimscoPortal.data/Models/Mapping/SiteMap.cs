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
            this.Property(t => t.Address1)
                .HasMaxLength(50);
            this.Property(t => t.Address2)
                .HasMaxLength(50);
            this.Property(t => t.Address3)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sites");
            this.Property(t => t.SiteId).HasColumnName("SiteId");
            this.Property(t => t.SiteName).HasColumnName("SiteName");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.GroupDivisionId).HasColumnName("GroupDivisionId");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");

            // Relationships
            this.HasRequired(t => t.Group)
                .WithMany(t => t.Sites)
                .HasForeignKey(d => d.GroupId);

            this.HasRequired(t => t.Customer)
                .WithMany(t => t.Sites)
                .HasForeignKey(d => d.CustomerId);

            // Relationships
            this.HasOptional(t => t.Users)
                .WithOptionalDependent(t => t.Site)
                .Map(m => m.MapKey("UserId"));
            //.Map(m =>
            //{
            //    m.ToTable("SiteUserLink");
            //});

            this.HasOptional(t => t.GroupDivision);
            this.HasOptional(t => t.IndustryClassification);

        }
    }
}
