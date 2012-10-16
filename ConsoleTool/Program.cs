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
            Console.WriteLine(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width);
            Console.WriteLine(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 40);
            Console.Read();
        }
    }
}
