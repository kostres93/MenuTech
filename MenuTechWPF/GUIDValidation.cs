using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace MenuTechWPF
{
    class GUIDValidation : ValidationRule
    {
        public string ErrorMessage { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value.ToString(); 
            Regex regex = new Regex(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$");
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
