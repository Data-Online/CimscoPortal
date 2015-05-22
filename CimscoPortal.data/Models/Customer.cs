using System;
using System.Collections.Generic;

namespace CimscoPortal.Data.Models
{
    public partial class Customer
    {
        public Customer()
        {
            this.PortalMessages = new List<PortalMessage>();
            this.Contacts = new List<Contact>();
            this.Addresses = new List<Address>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> AddressId { get; set; }
        public virtual ICollection<PortalMessage> PortalMessages { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
