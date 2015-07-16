using System;
using System.Collections.Generic;

namespace CimscoPortal.Data.Models
{
    public partial class Group
    {
        public Group()
        {
            //this.Contacts = new List<Contact>();
            //this.Addresses = new List<Address>();
            //this.Customers = new List<Customer>();
            this.Sites = new List<Site>();
            this.Users = new List<AspNetUser>();
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> AddressId { get; set; }
        //public virtual ICollection<Contact> Contacts { get; set; }
        //public virtual ICollection<Address> Addresses { get; set; }

        //public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Site> Sites { get; set; }

        public virtual ICollection<AspNetUser> Users { get; set; }
    }
}
