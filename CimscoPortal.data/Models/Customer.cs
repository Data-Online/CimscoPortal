using System;
using System.Collections.Generic;

namespace CimscoPortal.Data.Models
{
    public partial class Customer
    {
        public Customer()
        {
            //this.PortalMessages = new List<PortalMessage>();
            //this.Contacts = new List<Contact>();
            //this.Addresses = new List<Address>();
            this.Sites = new List<Site>();
            this.Users = new List<AspNetUser>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> AddressId { get; set; }
        //public Nullable<int> GroupId { get; set; }
        //public virtual ICollection<PortalMessage> PortalMessages { get; set; }
        //public virtual ICollection<Contact> Contacts { get; set; }
        //public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<Site> Sites { get; set; }
        public virtual ICollection<AspNetUser> Users { get; set; }

        //public virtual Group Group { get; set; }
    }
}
