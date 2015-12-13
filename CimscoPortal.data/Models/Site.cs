using System;
using System.Collections.Generic;
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
        public decimal SiteArea { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<int> CustomerId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
