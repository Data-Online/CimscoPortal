using System;
using System.Collections.Generic;

namespace CimscoPortal.Data.Models
{
    public partial class Address
    {
        public Address()
        {
            //this.Customers = new List<Customer>();
            //this.Groups = new List<Group>();
        }

        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public Nullable<int> CityId { get; set; }
        public string PostCode { get; set; }
        //public virtual ICollection<Customer> Customers { get; set; }
        //public virtual ICollection<Group> Groups { get; set; }
    }
}
