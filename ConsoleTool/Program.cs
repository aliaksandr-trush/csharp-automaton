namespace RegOnline.RegressionTest.ConsoleTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Linq;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Configuration;

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(string.Format("https://{0}/", "beta.regonline.com"));
            Console.WriteLine(string.Format("http://{0}/", "cbeta.regonline.com"));
            Console.Read();
        }
    }
}
