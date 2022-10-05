using System;
using System.Collections.Generic;

#nullable disable

namespace MenuTech.Models
{
    public partial class StoreAccount
    {
        public int IdStoreAccount { get; set; }
        public int? IdStore { get; set; }
        public decimal Balance { get; set; }
        public Guid? TransactionId { get; set; }
        public decimal? TransactionPlus { get; set; }
        public decimal? TransactionMinus { get; set; }
        public int? CustomerId { get; set; }
        public int? Refund { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
