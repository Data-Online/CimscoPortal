//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CimscoPortal.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class PortalMessage
    {
        public int PortalMessageId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Message { get; set; }
        public Nullable<int> MessageFormatId { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public string Footer { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
    
        public virtual MessageFormat MessageFormat { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
