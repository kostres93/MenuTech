using System;
using System.Collections.Generic;

#nullable disable

namespace MenuTech.Models
{
    public partial class CurrencyRate
    {
        public int IdCurrencyRate { get; set; }
        public int? CurrencyCode { get; set; }
        public string Currency { get; set; }
        public decimal? CurrencyRateValue { get; set; }
    }
}
