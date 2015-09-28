using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class LoginHistoryMap : EntityTypeConfiguration<LoginHistory>
    {
        public LoginHistoryMap()
        {
            this.HasKey(t => t.UserId);

            this.Property(t => t.UserId).
               IsRequired()
               .HasMaxLength(128);

            this.ToTable("LoginHistorys");



        }
    }
}
