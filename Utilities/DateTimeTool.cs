namespace RegOnline.RegressionTest.Utilities
{
    using System;
    using RegOnline.RegressionTest.Configuration;

    public class DateTimeTool
    {
        /// <summary>
        /// This is just for china test to handle the time difference
        /// </summary>
        /// <param name="now">china time now</param>
        /// <returns>RegOnline time</returns>
        public static DateTime ConvertToRegOnlineTime(DateTime dateTime)
        {
            if (TimeZone.CurrentTimeZone.StandardName == "China Standard Time")
            {
                int diff = Convert.ToInt32(ConfigurationProvider.XmlConfig.AllConfiguration.TimeZoneDifference) * -1;
                dateTime = dateTime.AddHours(diff);
            }

            return dateTime;
        }

        public static DateTime FirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime LastDayOfMonth(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
    }
}
