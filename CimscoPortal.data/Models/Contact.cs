using System;
using System.Collections.Generic;

namespace CimscoPortal.Data.Models
{
    public partial class Contact
    {
        public Contact()
        {
            this.Customers = new List<Customer>();
            this.Groups = new List<Group>();
        }

        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }
        public string PreferredName { get; set; }
        public string PhoneNumber { get; set; }
        public string eMail { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
