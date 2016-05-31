using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class IndustryClassificationMap : EntityTypeConfiguration<IndustryClassification>
    {
        public IndustryClassificationMap()
        {
            // Primary Key
            this.HasKey(t => t.IndustryId);

            // Properties
            this.Property(t => t.IndustryDescription)
                .HasMaxLength(90);
            this.Property(t => t.IndustryCode)
                .HasMaxLength(8);

            // Table & Column Mappings
            this.ToTable("IndustryClassifications");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");
        }
    }
}
