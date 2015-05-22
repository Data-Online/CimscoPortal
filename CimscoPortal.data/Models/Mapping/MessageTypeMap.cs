using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class MessageTypeMap : EntityTypeConfiguration<MessageType>
    {
        public MessageTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.MessageTypeId);

            // Properties
            this.Property(t => t.Description)
                .HasMaxLength(20);

            this.Property(t => t.PageElement)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("MessageTypes");
            this.Property(t => t.MessageTypeId).HasColumnName("MessageTypeId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.PageElement).HasColumnName("PageElement");
        }
    }
}
