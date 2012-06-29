namespace RegOnline.RegressionTest.UIUtility
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using RegOnline.RegressionTest.Configuration;

    internal class Browser_Firefox
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
    }

    internal class Browser_Chrome
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
    }
}
