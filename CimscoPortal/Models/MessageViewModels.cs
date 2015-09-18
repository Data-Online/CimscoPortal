using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CimscoPortal.Models
{
    public class MessageViewModel
    {
        public string TypeName { get; set; }
        public string CategoryName { get; set; }
        public string Message { get; set; }
        public string Footer { get; set; }
        public string Subject { get; set; }

        public string TimeStamp { get; set; }
        public DateTime _timeStamp { get; set; }

        public string Element1 { get; set; }
        public string Element2 { get; set; }

        public string Name { get; set; }
    }
}
