using CimscoPortal.Infrastructure;
using System.Collections.Generic;
using CimscoPortal.Data.Models;

namespace CimscoPortal.Models
{

    public class UserHierachyViewModel
    {
        public IList<AspNetUser> UserList { get; set; }
        public string CustomerGroupSiteName { get; set; }
        public string TopLevelName { get; set; }
    }

}
