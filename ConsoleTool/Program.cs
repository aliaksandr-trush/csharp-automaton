namespace RegOnline.RegressionTest.ConsoleTool
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using RegOnline.RegressionTest.DataAccess;

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Path.Combine(new StreamReader(@"\\10.107.100.63\AutomationStuff\Output\ConfigLocation.txt").ReadLine(), "TestConfig.xml"));
            Console.WriteLine(Path.Combine("", "TestConfig.xml"));
            Console.Read();
        }
    }
}
