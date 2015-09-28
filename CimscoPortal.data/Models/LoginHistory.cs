using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class LoginHistory
    {
        public LoginHistory()
        {

        }
        public string UserId { get; set; }

        public DateTime LastLoginDateTime { get; set; }

        public string Ip { get; set; }

        public string Details { get; set; }

    }
}
