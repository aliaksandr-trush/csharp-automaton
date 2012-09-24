namespace RegOnline.RegressionTest.UIUtility
{
    using System;
    using System.Collections;
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

        private void SetStartupOptions()
        {
            // Firefox binary
            if (ConfigReader.DefaultProvider.CurrentBrowser.BinaryPath.Enable)
            {
                this.binary = new FirefoxBinary(ConfigReader.DefaultProvider.CurrentBrowser.BinaryPath.Path);

                this.capa.SetCapability(
                    "firefox_binary",
                    ConfigReader.DefaultProvider.CurrentBrowser.BinaryPath.Path);
            }
            else
            {
                this.binary = new FirefoxBinary();
            }

            // Firefox profile
            if (ConfigReader.DefaultProvider.CurrentBrowser.ProfilePath.Enable)
            {
                this.profile = new FirefoxProfile(ConfigReader.DefaultProvider.CurrentBrowser.ProfilePath.Path);

                this.capa.SetCapability(
                    "firefox_profile",
                    ConfigReader.DefaultProvider.CurrentBrowser.ProfilePath.Path);
            }
            else
            {
                this.profile = new FirefoxProfile();
                this.profile.AcceptUntrustedCertificates = true;
            }

            // SSL certificate exception
            this.capa.SetCapability("acceptSslCerts", true);

            // Remote server url
            this.remoteServerUrl = new Uri(string.Format(
                "http://{0}:{1}/wd/hub",
                ConfigReader.DefaultProvider.CurrentBrowser.Server.Host,
                ConfigReader.DefaultProvider.CurrentBrowser.Server.Port));
        }

        public IWebDriver GetWebDriver()
        {
            this.SetStartupOptions();

            if (ConfigReader.DefaultProvider.AllConfiguration.Browsers.DirectStartup)
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
        private ArrayList switches = new ArrayList(2);
        private Uri remoteServerUrl;

        private void SetStartupOptions()
        {
            // Chrome binary
            if (ConfigReader.DefaultProvider.CurrentBrowser.BinaryPath.Enable)
            {
                this.options.BinaryLocation =
                    ConfigReader.DefaultProvider.CurrentBrowser.BinaryPath.Path;

                this.capa.SetCapability(
                    "chrome.binary",
                    ConfigReader.DefaultProvider.CurrentBrowser.BinaryPath.Path);
            }

            // Chrome profile
            if (ConfigReader.DefaultProvider.CurrentBrowser.ProfilePath.Enable)
            {
                this.options.AddArgument(string.Format(
                    "--user-data-dir={0}",
                    ConfigReader.DefaultProvider.CurrentBrowser.ProfilePath.Path));

                this.switches.Add(string.Format(
                    "--user-data-dir={0}", 
                    ConfigReader.DefaultProvider.CurrentBrowser.ProfilePath.Path));
            }

            // SSL certificate exception
            this.capa.SetCapability("acceptSslCerts", true);

            // Allow running insecure content from http in https
            this.options.AddArgument("--allow-running-insecure-content");
            this.switches.Add("--allow-running-insecure-content");

            // Chrome switches (see a full list of switches here: http://peter.sh/experiments/chromium-command-line-switches/)
            this.capa.SetCapability("chrome.switches", this.switches);

            // Remote server url
            this.remoteServerUrl = new Uri(string.Format(
                "http://{0}:{1}",
                ConfigReader.DefaultProvider.CurrentBrowser.Server.Host,
                ConfigReader.DefaultProvider.CurrentBrowser.Server.Port));
        }

        public IWebDriver GetWebDriver()
        {
            this.SetStartupOptions();

            if (ConfigReader.DefaultProvider.AllConfiguration.Browsers.DirectStartup)
            {
                return new ChromeDriver(ConfigReader.DefaultProvider.CurrentBrowser.Server.Path, this.options);
            }
            else
            {
                return new RemoteWebDriver(this.remoteServerUrl, this.capa);
            }
        }
    }

    internal class Browser_HtmlUnit : IGetWebDriver
    {
        private DesiredCapabilities capa = DesiredCapabilities.HtmlUnitWithJavaScript();
        private Uri remoteServerUrl;

        public IWebDriver GetWebDriver()
        {
            this.remoteServerUrl = new Uri(string.Format(
                "http://{0}:{1}/wd/hub",
                ConfigReader.DefaultProvider.CurrentBrowser.Server.Host,
                ConfigReader.DefaultProvider.CurrentBrowser.Server.Port));

            if (ConfigReader.DefaultProvider.CurrentBrowser.BinaryPath.Enable)
            {
                capa.SetCapability(
                    "firefox_binary",
                    ConfigReader.DefaultProvider.CurrentBrowser.BinaryPath.Path);
            }

            if (ConfigReader.DefaultProvider.CurrentBrowser.ProfilePath.Enable)
            {
                capa.SetCapability(
                    "firefox_profile",
                    ConfigReader.DefaultProvider.CurrentBrowser.ProfilePath.Path);
            }

            capa.SetCapability("acceptSslCerts", true);

            return new RemoteWebDriver(this.remoteServerUrl, capa);
        }
    }
}
