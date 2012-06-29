namespace RegOnline.RegressionTest.Utilities
{
    public class StringTool
    {
        public static string Left(string TargetString, int ExtractCharLength)
        {
            string val = string.Empty;

            if (TargetString.Length < ExtractCharLength)
            {
                val = TargetString;
            }
            else
            {
                val = TargetString.Substring(0, ExtractCharLength);
            }

            return val;
        }

        /// <summary>
        /// Formats the amount to guarantee we send 2 decimals (ie ".00" on whole numbers)
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string FormatAmount(decimal amount)
        {
            return amount.ToString("#######.00");
        }
    }
}
