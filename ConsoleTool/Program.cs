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

        static abc? tmp { get; set; }

        static void Main(string[] args)
        {
            if (tmp != null)
            Console.WriteLine("not null");
            Console.Read();
            Console.WriteLine(Math.Round(419.90, 2, MidpointRounding.AwayFromZero));
            Console.WriteLine(Math.Round(419.90, 2, MidpointRounding.ToEven));
            Console.WriteLine("************4113".Substring(12, 4));
            Console.WriteLine("4112344112344113".Substring(12, 4));
            Console.WriteLine(Convert.ToDateTime("22-Oct-2012").ToString());
        }
    }
}
