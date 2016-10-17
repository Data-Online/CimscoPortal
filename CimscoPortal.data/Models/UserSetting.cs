using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class UserSetting
    {
        public int UserSettingId { get; set; }

        public bool ShowWelcomeMessage { get; set; }
        public int MonthSpan { get; set; }
        public string UserIdentifier { get; set; }

        [ForeignKey("UserIdentifier")]
        public virtual AspNetUser UserId { get; set; }
    }
}
