namespace RegOnline.RegressionTest.UIUtility
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.Remote;
    using RegOnline.RegressionTest.Configuration;

    internal interface IGetWebDriver
    {
        IWebDriver GetWebDriver();
    }

    internal class Browser_Firefox : IGetWebDriver
    {
        private FirefoxBinary binary;
        private FirefoxProfile profile;

        private void ResetBinary()
        {
            if (ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Enable)
            {
                this.binary = new FirefoxBinary(ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Value);
            }
            else
            {
                this.binary = new FirefoxBinary();
            }
        }

        private void ResetProfile()
        {
            if (ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Enable)
            {
                this.profile = new FirefoxProfile(ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Value);
            }
            else
            {
                this.profile = new FirefoxProfile();
            }
        }

        public IWebDriver GetWebDriver()
        {
            this.ResetProfile();
            this.ResetBinary();
            return new FirefoxDriver(this.binary, this.profile);
        }

        ////public IWebDriver GetWebDriver()
        ////{
        ////    DesiredCapabilities capa = DesiredCapabilities.Firefox();

        ////    if (ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Enable)
        ////    {
        ////        capa.SetCapability(
        ////            "firefox_binary",
        ////            ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Value);
        ////    }

        ////    if (ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Enable)
        ////    {
        ////        capa.SetCapability(
        ////            "firefox_profile",
        ////            ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Value);
        ////    }

        ////    return new RemoteWebDriver(capa);
        ////}
    }

    internal class Browser_Chrome : IGetWebDriver
    {
        private ChromeOptions options = new ChromeOptions();

        private void ResetBinary()
        {
            if (ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Enable)
            {
                this.options.BinaryLocation =
                    ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Value;
            }
        }

        private void ResetProfile()
        {
            if (ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Enable)
            {
                this.options.AddArgument(string.Format(
                    "user-data-dir={0}",
                    ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Value));
            }
        }

        public IWebDriver GetWebDriver()
        {
            this.ResetProfile();
            this.ResetBinary();

            return new ChromeDriver(
                ConfigurationProvider.XmlConfig.AllConfiguration.Browsers.ChromeDriverPath,
                this.options);
        }

        ////public IWebDriver GetWebDriver()
        ////{
        ////    DesiredCapabilities capa = DesiredCapabilities.Chrome();

        ////    if (ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Enable)
        ////    {
        ////        capa.SetCapability(
        ////            "chrome.binary",
        ////            ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Value);
        ////    }

        ////    if (ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Enable)
        ////    {
        ////        capa.SetCapability(
        ////            "chrome.switches",
        ////            string.Format("user-data-dir={0}", ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Value));
        ////    }

        ////    return new RemoteWebDriver(new Uri("http://127.0.0.1:9515"), capa);
        ////}
    }
}
