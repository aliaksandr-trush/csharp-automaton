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
            Console.WriteLine(Path.GetFullPath("ButtonDesigner/CustomCode.html").Replace('\\', '/'));
            Console.WriteLine(98 / 100.0);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss tt"));
            Console.Read();
        }
    }
}
