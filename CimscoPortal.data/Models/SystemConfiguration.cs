using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class SystemConfiguration
    {
        public int SystemConfigurationId { get; set; }
        public string Key { get; set; }
        public string Values { get; set; }
    }
}

