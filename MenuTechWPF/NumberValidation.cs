using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace MenuTechWPF
{
    class NumberValidation : ValidationRule
    {
        public string ErrorMessage { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value.ToString();
            Regex regex = new Regex(@"^\d*(\.\d+)?$");
            bool rt = regex.IsMatch(input);
            if (!rt || input.Equals("0"))
            {
                return new ValidationResult(false, this.ErrorMessage);
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }

        
    }
}
