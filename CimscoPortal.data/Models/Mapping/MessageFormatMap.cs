using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class MessageFormatMap : EntityTypeConfiguration<MessageFormat>
    {
        public MessageFormatMap()
        {
            // Primary Key
            this.HasKey(t => t.MessageFormatId);

            // Properties
            this.Property(t => t.Element1)
                .HasMaxLength(50);

            this.Property(t => t.Element2)
                .HasMaxLength(50);

            this.Property(t => t.DisplayFormat)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MessageFormats");
            this.Property(t => t.MessageFormatId).HasColumnName("MessageFormatId");
            this.Property(t => t.MessageTypeId).HasColumnName("MessageTypeId");
            this.Property(t => t.Element1).HasColumnName("Element1");
            this.Property(t => t.Element2).HasColumnName("Element2");
            this.Property(t => t.DisplayFormat).HasColumnName("DisplayFormat");

            // Relationships
            this.HasOptional(t => t.MessageType)
                .WithMany(t => t.MessageFormats)
                .HasForeignKey(d => d.MessageTypeId);

        }
    }
}
