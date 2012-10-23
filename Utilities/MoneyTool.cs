namespace RegOnline.RegressionTest.Utilities
{
    using System;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.DataAccess;

    public class MoneyTool
    {
        public enum CurrencyCode
        {
            [CustomString("$")]
            USD,

            [CustomString("£")]
            GBP,

            AUD
        }

        public static double RemoveMoneyCurrencyCode(string money)
        {
            string tmp = money;
            string amountString = string.Empty;

            for (int i = 0; i < tmp.Length; i++)
            {
                if (Char.IsNumber(tmp, i) || (tmp.Substring(i, 1) == ".") || (tmp.Substring(i, 1) == "-"))
                {
                    amountString += tmp.Substring(i, 1);
                }
            }

            return Convert.ToDouble(amountString);
        }

        public static string FormatMoney(
            double amount, 
            CurrencyCode currencyCode = CurrencyCode.USD, 
            int decimalDigits = 2)
        {
            StringBuilder moneyString = new StringBuilder();

            if (amount < 0)
            {
                moneyString.Append("-");
                amount = -amount;
            }

            moneyString.Append(CustomStringAttribute.GetCustomString(currencyCode));

            moneyString.Append(amount.ToString(string.Format("C{0}", decimalDigits)).Remove(0, 1));

            return moneyString.ToString();
        }

        public static double ConvertAmount(
            double amount, 
            CurrencyCode originalCurrencyCode, 
            CurrencyCode targetCurrencyCode,
            int decimalDigits = 2,
            MidpointRounding midPointRounding = MidpointRounding.AwayFromZero)
        {
            double originalExchangeRate = GetExchangeRate(originalCurrencyCode);
            double targetExchangeRate = GetExchangeRate(targetCurrencyCode);

            double amount_USD = amount / originalExchangeRate;
            double amount_TargetCurrency = Math.Round(amount_USD * targetExchangeRate, decimalDigits, midPointRounding);

            return amount_TargetCurrency;
        }

        private static double GetExchangeRate(CurrencyCode currencyCode)
        {
            if (currencyCode == CurrencyCode.USD)
            {
                return 1;
            }
            else
            {
                ClientDataContext db = new ClientDataContext();
                var currency = from c in db.Currencies where c.CurrencyCode.Equals(currencyCode.ToString()) select c;
                return currency.FirstOrDefault().Rate.Value;
            }
        }
    }
}
