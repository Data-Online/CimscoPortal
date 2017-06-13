using CimscoPortal.Infrastructure;
using System.Collections.Generic;
using CimscoPortal.Data.Models;

namespace CimscoPortal.Models
{

    public class UserHierachyViewModel
    {
        public string CustomerGroupSiteName { get; set; }
        public string TopLevelName { get; set; }
        public IList<EditUserViewModel> UserList { get; set; }
    }

}
