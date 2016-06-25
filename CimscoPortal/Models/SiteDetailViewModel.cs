using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class SiteDetailViewModel
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int TotalFloorSpaceSqMeters { get; set; }
        public int ProductiveFloorSpaceSqMeters { get; set; }
        public int LandSqMeters { get; set; }
        public string IndustryDescription { get; set; }
    }
}
