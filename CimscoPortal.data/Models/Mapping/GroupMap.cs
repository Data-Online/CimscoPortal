using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class GroupMap : EntityTypeConfiguration<Group>
    {
        public GroupMap()
        {
            // Primary Key
            this.HasKey(t => t.GroupId);

            // Properties
            this.Property(t => t.GroupName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Groups");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.GroupName).HasColumnName("GroupName");
            this.Property(t => t.AddressId).HasColumnName("AddressId");
        }
    }
}
