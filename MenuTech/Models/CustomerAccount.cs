using System;
using System.Collections.Generic;

#nullable disable

namespace MenuTech.Models
{
    public partial class CustomerAccount
    {
        public int IdCustomerAccount { get; set; }
        public int? CustomerId { get; set; }
        public decimal Balance { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
