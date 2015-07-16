using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.CustomerId);

            // Properties
            this.Property(t => t.CustomerName)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("Customers");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.AddressId).HasColumnName("AddressId");

            //this.Property(t => t.GroupId).HasColumnName("GroupId");

            //// Relationships
            //this.HasOptional(t => t.Group)
            //    .WithMany(t => t.Customers)
            //    .HasForeignKey(d => d.GroupId);

            // Relationships
            this.HasMany(t => t.Users)
                .WithMany(t => t.Customers)
                .Map(m =>
                {
                    m.ToTable("CustomerUserLink");
                    m.MapLeftKey("CustomerId");
                    m.MapRightKey("UserId");
                });

        }
    }
}
