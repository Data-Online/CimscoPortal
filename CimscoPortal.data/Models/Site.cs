using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class Site
    {
        public Site()
        {

        }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
     //   public decimal SiteArea { get; set; } // Obsolete
        public int? TotalFloorSpaceSqMeters { get; set; }
        public int? LandSqMeters { get; set; }
        public int? ProductiveFloorSpaceSqMeters { get; set; }
        //public Nullable<int> GroupId { get; set; }
        //public Nullable<int> CustomerId { get; set; }
        public int GroupId { get; set; }
        public int CustomerId { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }

        public int? GroupDivisionId { get; set; }
        public int? IndustryId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual AspNetUser Users { get; set; }

        [ForeignKey("GroupDivisionId")]
        public virtual Division GroupDivision { get; set; }

        [ForeignKey("IndustryId")]
        public virtual IndustryClassification IndustryClassification { get; set; }
    }
}
