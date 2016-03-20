using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace CimscoPortal.Data.Models.Mapping
{
    public class CommonDataMap : EntityTypeConfiguration<CommonData>
    {
        public CommonDataMap()
        {
            // Primary Key
            this.HasKey(t => t.CommonDataId);

            this.ToTable("CommonData");
        }
    }
}
