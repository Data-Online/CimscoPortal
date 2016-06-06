using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class DivisionMap : EntityTypeConfiguration<Division>
    {
        public DivisionMap()
        {
            // Primary Key
            this.HasKey(t => t.DivisionId);

            // Properties
            this.Property(t => t.DivisionName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Divisions");
            this.Property(t => t.DivisionId).HasColumnName("DivisionId");
            this.Property(t => t.DivisionName).HasColumnName("DivisionName");
            this.Property(t => t.GroupId).HasColumnName("GroupId");


        }
    }
}
