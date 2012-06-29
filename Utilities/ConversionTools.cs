namespace RegOnline.RegressionTest.Utilities
{
    using System;

    public abstract class ConversionTools
    {
        public ConversionTools() { }

        public static double CurrencyToDouble(string amount)
        {
            string a = string.Empty;

            for (int i = 0; i < amount.Length; i++)
            {
                if (Char.IsNumber(amount, i) || (amount.Substring(i, 1) == "."))
                {
                    a += amount.Substring(i, 1);
                }
            }

            return Convert.ToDouble(a);
        }

        public static string ConvertGroupMemberIndexToTwoDigitsString(int memberNumber)
        {
            string number = Convert.ToString(memberNumber);

            if (number.Length == 1)
            {
                number = "0" + number;
            }

            return number;
        }

    }
}
