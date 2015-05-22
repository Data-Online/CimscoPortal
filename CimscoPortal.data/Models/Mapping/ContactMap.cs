using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class ContactMap : EntityTypeConfiguration<Contact>
    {
        public ContactMap()
        {
            // Primary Key
            this.HasKey(t => t.ContactId);

            // Properties
            this.Property(t => t.FirstName)
                .HasMaxLength(20);

            this.Property(t => t.MiddleName)
                .HasMaxLength(20);

            this.Property(t => t.LastName)
                .HasMaxLength(20);

            this.Property(t => t.Salutation)
                .HasMaxLength(5);

            this.Property(t => t.PreferredName)
                .HasMaxLength(20);

            this.Property(t => t.PhoneNumber)
                .HasMaxLength(20);

            this.Property(t => t.eMail)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Contacts");
            this.Property(t => t.ContactId).HasColumnName("ContactId");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.MiddleName).HasColumnName("MiddleName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.Salutation).HasColumnName("Salutation");
            this.Property(t => t.PreferredName).HasColumnName("PreferredName");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.eMail).HasColumnName("eMail");

            // Relationships
            this.HasMany(t => t.Customers)
                .WithMany(t => t.Contacts)
                .Map(m =>
                    {
                        m.ToTable("ContactCustomerLink");
                        m.MapLeftKey("ContactId");
                        m.MapRightKey("CustomerId");
                    });

            this.HasMany(t => t.Groups)
                .WithMany(t => t.Contacts)
                .Map(m =>
                    {
                        m.ToTable("ContactGroupLink");
                        m.MapLeftKey("ContactId");
                        m.MapRightKey("GroupId");
                    });


        }
    }
}
