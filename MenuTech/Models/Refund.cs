using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MenuTech.Models
{
    public class Refund
    {

        public int customerId { get; set; }

        public Guid transactionId { get; set; }
        public string amount { get; set; }
        
    }
}
