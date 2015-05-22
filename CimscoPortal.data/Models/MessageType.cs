using System;
using System.Collections.Generic;

namespace CimscoPortal.Data.Models
{
    public partial class MessageType
    {
        public MessageType()
        {
            this.MessageFormats = new List<MessageFormat>();
        }

        public int MessageTypeId { get; set; }
        public string Description { get; set; }
        public string PageElement { get; set; }
        public virtual ICollection<MessageFormat> MessageFormats { get; set; }
    }
}
