namespace RegOnline.RegressionTest.ConsoleTool
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.DataAccess;

    public class Program
    {
        static void Main(string[] args)
        {
            ////string input = Console.ReadLine();
            ////string[] eventIds = input.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
            ////foreach (string eventId in eventIds)
            ////{
            ////    AccessData.RemoveLiveRegForEvent(Convert.ToInt32(eventId));
            ////}
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss tt"));
            Console.Read();
        }
    }
}
