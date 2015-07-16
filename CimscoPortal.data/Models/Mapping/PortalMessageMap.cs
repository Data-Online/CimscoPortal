using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class PortalMessageMap : EntityTypeConfiguration<PortalMessage>
    {
        public PortalMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.PortalMessageId);

            // Properties
            this.Property(t => t.Message)
                .HasMaxLength(100);

            this.Property(t => t.Footer)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("PortalMessages");
            this.Property(t => t.PortalMessageId).HasColumnName("PortalMessageId");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.MessageFormatId).HasColumnName("MessageFormatId");
            this.Property(t => t.TimeStamp).HasColumnName("TimeStamp");
            this.Property(t => t.Footer).HasColumnName("Footer");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");

            // Relationships
            //this.HasOptional(t => t.Customer)
            //    .WithMany(t => t.PortalMessages)
            //    .HasForeignKey(d => d.CustomerId);
            //this.HasOptional(t => t.MessageFormat)
            //    .WithMany(t => t.PortalMessages)
            //    .HasForeignKey(d => d.MessageFormatId);

        }
    }
}
