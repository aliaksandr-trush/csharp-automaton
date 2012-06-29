namespace RegOnline.RegressionTest.Utilities
{
    using System;
    using System.Text.RegularExpressions;

    public class CreditCardNumberFormatter : IFormatProvider, ICustomFormatter
    {
        // String.Format calls this method to get an instance of an
        // ICustomFormatter to handle the formatting.
        public object GetFormat(Type service)
        {
            if (service == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        // After String.Format gets the ICustomFormatter, it calls this format
        // method on each argument.
        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (Regex.IsMatch(arg.ToString(), "[0-9]{16}") && arg.ToString().Length == 16)
            {
                return Regex.Replace(arg.ToString(), "[0-9]{12}", "************");
            }

            return String.Format("{0}", arg);
        }
    }
}
