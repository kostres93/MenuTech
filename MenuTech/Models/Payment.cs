 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuTech.Models
{
    public class Payment
    {
        public int customerId { get; set; }
        public int currency { get; set; }
        public string amount { get; set; }
    }
}
