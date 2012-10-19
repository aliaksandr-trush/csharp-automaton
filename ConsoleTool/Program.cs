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
        enum abc
        {
            a,
            b,
            c
        }

        static abc tmp { get; set; }

        static void Main(string[] args)
        {
            if (tmp != null)
            Console.WriteLine("not null");
            Console.Read();
        }
    }
}
