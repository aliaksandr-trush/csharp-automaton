namespace RegOnline.RegressionTest.Utilities
{
    using System;
    using RegOnline.RegressionTest.Configuration;

    public class DateTimeTool
    {
        public enum TimeZoneIdentifier
        {
            Local,

            [CustomString("GMT Standard Time")]
            GMTStandardTime,

            [CustomString("US Mountain Standard Time")]
            USMountainStandardTime
        }

        public static DateTime ConvertRegardingTimeZone(
            DateTime dateTimeToConvert, 
            TimeZoneIdentifier targetTimeZone,
            TimeZoneIdentifier sourceTimeZone = TimeZoneIdentifier.Local)
        {
            TimeZoneInfo sourceTimeZoneInfo = TimeZoneInfo.Local;

            if (sourceTimeZone != TimeZoneIdentifier.Local)
            {
                sourceTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(CustomStringAttribute.GetCustomString(sourceTimeZone));
            }

            TimeZoneInfo targetTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(CustomStringAttribute.GetCustomString(targetTimeZone));
            return TimeZoneInfo.ConvertTime(dateTimeToConvert, targetTimeZoneInfo);
        }

        public static DateTime ConvertForCurrentAccount(DateTime dateTimeToConvert, ConfigReader.AccountEnum targetAccount)
        {
            Account targetAccountConfig = null;

            foreach (Account ac in ConfigReader.DefaultProvider.EnvironmentConfiguration.Account_Config)
            {
                if (ac.Name.Equals(targetAccount.ToString()))
                {
                    targetAccountConfig = ac;
                }
            }

            return dateTimeToConvert.AddHours(
                targetAccountConfig.TimeZoneOffset - 
                ConfigReader.DefaultProvider.AllConfiguration.Environments_Config.CurrentMachine_TimeZoneOffset);
        }

        /// <summary>
        /// This is just for china test to handle the time difference
        /// </summary>
        /// <param name="now">china time now</param>
        /// <returns>RegOnline time</returns>
        public static DateTime ConvertToRegOnlineTime(DateTime dateTime)
        {
            if (TimeZone.CurrentTimeZone.StandardName == "China Standard Time")
            {
                int diff = Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.TimeZoneOffset) * -1;
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
