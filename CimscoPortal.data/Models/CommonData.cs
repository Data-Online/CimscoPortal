using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class CommonData
    {
        public int CommonDataId { get; set; }
        public string LocationId { get; set; }
        public int Temperature { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
