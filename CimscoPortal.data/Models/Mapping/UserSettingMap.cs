using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class UserSettingMap : EntityTypeConfiguration<UserSetting>
    {
        public UserSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.UserSettingId);

            this.ToTable("UserSettings");
        }
    }
}
