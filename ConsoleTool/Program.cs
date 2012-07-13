namespace RegOnline.RegressionTest.ConsoleTool
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
            XmlConfiguration.Browser browser;
            Enum.TryParse<XmlConfiguration.Browser>(ConfigurationProvider.XmlConfig.CurrentBrowser.Name, out browser);
            DataHelper helper = new DataHelper();
            helper.RemoveXAuthLiveRegistration(245012);
            Console.WriteLine(browser.ToString());
            Console.Read();
        }
    }
}
