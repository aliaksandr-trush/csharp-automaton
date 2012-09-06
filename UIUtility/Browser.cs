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
        // To startup Firefox directly is not stable,
        // because the port number used to start Firefox may be occupied by some other programs.
        // The second method to start firefox is stable,
        // but you have to run Reference/SeleniumServer/StartServer.bat first.
        // It's best to start that file along with system startup to ensure the port number would not be occupied.

        private FirefoxBinary binary;
        private FirefoxProfile profile;
        private DesiredCapabilities capa = DesiredCapabilities.Firefox();
        private Uri remoteServerUrl;

        private void ResetBinary()
        {
            if (ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Enable)
            {
                this.binary = new FirefoxBinary(ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Path);

                this.capa.SetCapability(
                    "firefox_binary",
                    ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Path);
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
                this.profile = new FirefoxProfile(ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Path);

                this.capa.SetCapability(
                    "firefox_profile",
                    ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Path);

                this.capa.SetCapability(
                    "acceptSslCerts",
                    true);
            }
            else
            {
                this.profile = new FirefoxProfile();
                this.profile.AcceptUntrustedCertificates = true;
            }
        }

        private void ResetRemoteServerUrl()
        {
            this.remoteServerUrl = new Uri(string.Format(
                "http://{0}:{1}/wd/hub", 
                ConfigurationProvider.XmlConfig.CurrentBrowser.Server.Host,
                ConfigurationProvider.XmlConfig.CurrentBrowser.Server.Port));
        }

        public IWebDriver GetWebDriver()
        {
            this.ResetProfile();
            this.ResetBinary();
            this.ResetRemoteServerUrl();

            if (ConfigurationProvider.XmlConfig.AllConfiguration.Browsers.DirectStartup)
            {
                return new FirefoxDriver(this.binary, this.profile);
            }
            else
            {
                return new RemoteWebDriver(this.remoteServerUrl, this.capa);
            }
        }
    }

    internal class Browser_Chrome : IGetWebDriver
    {
        private ChromeOptions options = new ChromeOptions();
        private DesiredCapabilities capa = DesiredCapabilities.Chrome();
        private Uri remoteServerUrl;

        private void ResetBinary()
        {
            if (ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Enable)
            {
                this.options.BinaryLocation =
                    ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Path;

                this.capa.SetCapability(
                    "chrome.binary",
                    ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Path);
            }
        }

        private void ResetProfile()
        {
            if (ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Enable)
            {
                this.options.AddArgument(string.Format(
                    "user-data-dir={0}",
                    ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Path));

                this.capa.SetCapability(
                    "chrome.switches",
                    string.Format("user-data-dir={0}", ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Path));

                this.capa.SetCapability(
                    "acceptSslCerts",
                    true);
            }
        }

        private void ResetRemoteServerUrl()
        {
            this.remoteServerUrl = new Uri(string.Format(
                "http://{0}:{1}",
                ConfigurationProvider.XmlConfig.CurrentBrowser.Server.Host,
                ConfigurationProvider.XmlConfig.CurrentBrowser.Server.Port));
        }

        public IWebDriver GetWebDriver()
        {
            this.ResetProfile();
            this.ResetBinary();
            this.ResetRemoteServerUrl();

            if (ConfigurationProvider.XmlConfig.AllConfiguration.Browsers.DirectStartup)
            {
                return new ChromeDriver(this.options);
            }
            else
            {
                return new RemoteWebDriver(this.remoteServerUrl, this.capa);
            }
        }
    }

    internal class Browser_HtmlUnit : IGetWebDriver
    {
        public IWebDriver GetWebDriver()
        {
            DesiredCapabilities capa = DesiredCapabilities.HtmlUnitWithJavaScript();

            if (ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Enable)
            {
                capa.SetCapability(
                    "firefox_binary",
                    ConfigurationProvider.XmlConfig.CurrentBrowser.BinaryPath.Path);
            }

            if (ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Enable)
            {
                capa.SetCapability(
                    "firefox_profile",
                    ConfigurationProvider.XmlConfig.CurrentBrowser.ProfilePath.Path);
            }

            return new RemoteWebDriver(capa);
        }
    }
}
