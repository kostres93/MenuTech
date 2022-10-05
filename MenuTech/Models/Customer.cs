using System;
using System.Collections.Generic;

#nullable disable

namespace MenuTech.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerAccounts = new HashSet<CustomerAccount>();
            StoreAccounts = new HashSet<StoreAccount>();
        }

        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual ICollection<CustomerAccount> CustomerAccounts { get; set; }
        public virtual ICollection<StoreAccount> StoreAccounts { get; set; }
    }
}
