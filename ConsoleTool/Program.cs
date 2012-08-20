﻿namespace RegOnline.RegressionTest.ConsoleTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Configuration;

    public class Program
    {
        static void Main(string[] args)
        {
            //XmlConfiguration.Browser browser;
            //Enum.TryParse<XmlConfiguration.Browser>(ConfigurationProvider.XmlConfig.CurrentBrowser.Name, out browser);
            //DataHelper helper = new DataHelper();
            //helper.RemoveXAuthLiveRegistration(245012);
            //Console.WriteLine(browser.ToString());
            //Console.Read();
            ////Console.WriteLine(AccessData.GetEncryptString("375859"));
            ////Console.WriteLine(ManagerProvider.EmailMgr.FetchConfirmationEmailId(Managers.Emails.EmailManager.EmailCategory.Complete, 637115));
            DateTimeConvertTest();
            Console.Read();
        }

        private static void DateTimeConvertTest()
        {
            // Define times to be converted.
            DateTime[] times = { new DateTime(2010, 1, 1, 0, 1, 0), 
                           new DateTime(2010, 1, 1, 0, 1, 0, DateTimeKind.Utc), 
                           new DateTime(2010, 1, 1, 0, 1, 0, DateTimeKind.Local),                            
                           new DateTime(2012, 8, 16, 23, 30, 0, DateTimeKind.Local),
                           new DateTime(2012, 8, 17, 14, 30, 0, DateTimeKind.Local) };

            // Retrieve the time zone for Eastern Standard Time (U.S. and Canada).
            TimeZoneInfo est;
            try
            {
                est = TimeZoneInfo.FindSystemTimeZoneById("US Mountain Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                Console.WriteLine("Unable to retrieve the Greenwich Standard time zone.");
                return;
            }
            catch (InvalidTimeZoneException)
            {
                Console.WriteLine("Unable to retrieve the Greenwich Standard time zone.");
                return;
            }

            // Display the current time zone name.
            Console.WriteLine("Local time zone display name: {0}\n", TimeZoneInfo.Local.DisplayName);
            Console.WriteLine("Local time zone base UTC offset: {0}\n", TimeZoneInfo.Local.BaseUtcOffset);
            Console.WriteLine("Local time zone daylight name: {0}\n", TimeZoneInfo.Local.DaylightName);
            Console.WriteLine("Target time zone display name: {0}\n", est.DisplayName);
            Console.WriteLine("Target time zone standard name: {0}\n", est.StandardName);
            Console.WriteLine("Target time zone support daylight saving time: {0}\n", est.SupportsDaylightSavingTime);

            // Convert each time in the array.
            foreach (DateTime timeToConvert in times)
            {
                DateTime targetTime = TimeZoneInfo.ConvertTime(timeToConvert, est);
                Console.WriteLine("Converted {0} {1} to {2}.", timeToConvert,
                                  timeToConvert.Kind, targetTime);
            }
        }
    }
}
