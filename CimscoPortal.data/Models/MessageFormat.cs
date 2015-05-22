using System;
using System.Collections.Generic;

namespace CimscoPortal.Data.Models
{
    public partial class MessageFormat
    {
        public MessageFormat()
        {
            this.PortalMessages = new List<PortalMessage>();
        }

        public int MessageFormatId { get; set; }
        public Nullable<int> MessageTypeId { get; set; }
        public string Element1 { get; set; }
        public string Element2 { get; set; }
        public string DisplayFormat { get; set; }
        public virtual MessageType MessageType { get; set; }
        public virtual ICollection<PortalMessage> PortalMessages { get; set; }
    }
}
