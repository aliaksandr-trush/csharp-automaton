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
            AccessData.SetLiveRegToTest(new List<int>() { 639231 });
            Console.Read();
        }
    }
}
