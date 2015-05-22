using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            // Primary Key
            this.HasKey(t => t.AddressId);

            // Properties
            this.Property(t => t.Address1)
                .HasMaxLength(50);

            this.Property(t => t.Address2)
                .HasMaxLength(50);

            this.Property(t => t.Address3)
                .HasMaxLength(50);

            this.Property(t => t.PostCode)
                .HasMaxLength(4);

            // Table & Column Mappings
            this.ToTable("Addresses");
            this.Property(t => t.AddressId).HasColumnName("AddressId");
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.Address3).HasColumnName("Address3");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.PostCode).HasColumnName("PostCode");

            // Relationships
            this.HasMany(t => t.Customers)
                .WithMany(t => t.Addresses)
                .Map(m =>
                    {
                        m.ToTable("CustomerAddressLink");
                        m.MapLeftKey("AddressId");
                        m.MapRightKey("CustomerId");
                    });

            this.HasMany(t => t.Groups)
                .WithMany(t => t.Addresses)
                .Map(m =>
                    {
                        m.ToTable("GroupAddressLink");
                        m.MapLeftKey("AddressId");
                        m.MapRightKey("GroupId");
                    });


        }
    }
}
