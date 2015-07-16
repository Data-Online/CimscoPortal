using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models.Mapping
{
    public class UserDetailMap : EntityTypeConfiguration<UserDetail>
    {
        public UserDetailMap()
        {
            this.HasKey(t => t.UserDetailId);

            this.Property(t => t.UserDetailId)
                .HasColumnName("UserId")
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FirstName)
                .HasMaxLength(100);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(100);

            //this.HasOptional(o => o.UserId)
            //    .WithOptionalPrincipal(l => l.UserDetail);

        }
    }
}
