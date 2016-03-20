using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class UserDetail
    {
        public int UserDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual AspNetUser UserId { get; set; }
    }
}
