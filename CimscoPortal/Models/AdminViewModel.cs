using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CimscoPortal.Models
{
    //public class RoleViewModel
    //{
    //    public string Id { get; set; }
    //    [Required(AllowEmptyStrings = false)]
    //    [Display(Name = "RoleName")]
    //    public string Name { get; set; }
    //}

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
        //public virtual ICollection<Data.Models.AspNetRole> AspNetRoles { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        // Define user position in current hierachy - allowing management on user entry screen
        public string UserLevel { get; set; }
        public string TopLevelName { get; set; }
        public string ParentLevel { get; set; }

    }
}
