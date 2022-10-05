using System;
using System.Collections.Generic;
using System.Text;

namespace MenuTechWPF
{
    public class Customer
    {

        public Customer() { }
        public int customerId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public  string loginMsg { get; set; }
        public string transactionId { get; set; }
        public decimal? transactionPlus { get; set; }
        public decimal amount { get; set; }
        public int? refund { get; set; }





    }
}
