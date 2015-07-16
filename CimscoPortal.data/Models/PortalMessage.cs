using System;
using System.Collections.Generic;

namespace CimscoPortal.Data.Models
{
    public partial class PortalMessage
    {
        public int PortalMessageId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Message { get; set; }
        public Nullable<int> MessageFormatId { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public string Footer { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        //public virtual Customer Customer { get; set; }
        public virtual MessageFormat MessageFormat { get; set; }
    }
}
