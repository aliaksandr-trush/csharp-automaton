namespace RegOnline.RegressionTest.Configuration
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class ConfigReader
    {
        private readonly string XmlConfigFilePath = "TestConfig.xml";
        private static ConfigReader Default = new ConfigReader();

        public enum EnvironmentEnum
        {
            Alpha,
            Beta,
            Production
        }

        public enum AccountType
        {
            Default,
            Alternative,
            ActiveEurope,
            SSO
        }

        public enum WebServiceEnum
        {
            LoginService,
            RegistrationService,
            EventService,
            Default,
            CFResponseService,
            CheckInRegService,
            CheckinService,
            GetEventFieldsService,
            GetEventRegistrationsService,
            GetEventsService,
            MemberAuthService,
            RegTrackerService,
            RegUpdateService,
            ReportsService,
            RetrieveAllRegsService,
            RetrieveSingleRegService,
            XmlUpdaterService
        }

        public enum BrowserEnum
        {
            Firefox,
            Chrome
        }

        public TestConfig AllConfiguration
        {
            get;
            private set;
        }

        public Environment EnvironmentConfiguration
        {
            get;
            private set;
        }

        public Account AccountConfiguration
        {
            get;
            private set;
        }

        public Dictionary<WebServiceEnum, WebService> WebServiceConfiguration
        {
            get;
            private set;
        }

        public Browser CurrentBrowser
        {
            get;
            private set;
        }

        public static ConfigReader DefaultProvider
        {
            get
            {
                return ConfigReader.Default;
            }
        }

        public ConfigReader()
        {
            this.ReloadAllConfiguration();
        }

        /// <summary>
        /// Reload all configuration according to specified environment and private-label
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="accountType"></param>
        public void ReloadAllConfiguration(Environment environment, AccountType accountType)
        {
            this.ReloadAllConfiguration(environment.ToString(), accountType.ToString());
        }

        /// <summary>
        /// Reload all configuration according to preferred environment(which is specified in the xml configuration file) and private-label
        /// </summary>
        public void ReloadAllConfiguration()
        {
            this.DeserializeFromXml();

            // Cos preferred environment and private-label must be read from xml first, 
            // we cannot call this.ReloadAllConfiguration(this.AllConfiguration.Environments.Preferred.Environment, this.AllConfiguration.Environments.Preferred.PrivateLabel)
            this.LoadEnvironmentAndAccountConfiguration(this.AllConfiguration.Environments.Preferred.Environment, this.AllConfiguration.Environments.Preferred.PrivateLabel);
            
            this.LoadWebServiceConfiguration();
            this.LoadBrowser();
        }

        private void ReloadAllConfiguration(string environment, string accountType)
        {
            this.DeserializeFromXml();
            this.LoadEnvironmentAndAccountConfiguration(environment, accountType);
            this.LoadWebServiceConfiguration();
            this.LoadBrowser();
        }

        /// <summary>
        /// Switch to another environment and private label, 
        /// the new private label in the new environment
        /// has the same name with that of the current environment
        /// </summary>
        /// <param name="environment"></param>
        public void ReloadEnvironment(Environment environment)
        {
            this.LoadEnvironmentAndAccountConfiguration(environment.ToString(), this.AccountConfiguration.Name);
        }

        /// <summary>
        /// Switch to another private label under the current environment
        /// </summary>
        /// <param name="accountType"></param>
        public void ReloadAccount(AccountType accountType)
        {
            this.LoadAccountConfiguration(accountType.ToString());
        }

        private void DeserializeFromXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestConfig));
            XmlTextReader reader = new XmlTextReader(XmlConfigFilePath);
            this.AllConfiguration = (TestConfig)serializer.Deserialize(reader);
        }

        private void LoadEnvironmentAndAccountConfiguration(string environment, string accountType)
        {
            this.LoadEnvironmentConfiguration(environment);
            this.LoadAccountConfiguration(accountType);
        }

        private void LoadEnvironmentConfiguration(string environment)
        {
            foreach (Environment en in this.AllConfiguration.Environments.Environment)
            {
                if (en.Name.Equals(environment))
                {
                    this.EnvironmentConfiguration = en;
                }
            }
        }

        private void LoadAccountConfiguration(string accountType)
        {
            foreach (Account account in this.EnvironmentConfiguration.Account)
            {
                if (account.Name.Equals(accountType))
                {
                    this.AccountConfiguration = account;
                }
            }
        }

        private void LoadWebServiceConfiguration()
        {
            if (this.WebServiceConfiguration == null)
            {
                this.WebServiceConfiguration = new Dictionary<WebServiceEnum, WebService>();
            }
            else
            {
                this.WebServiceConfiguration.Clear();
            }

            this.AddWebServiceToDictionary(WebServiceEnum.LoginService);
            this.AddWebServiceToDictionary(WebServiceEnum.RegistrationService);
            this.AddWebServiceToDictionary(WebServiceEnum.EventService);
            this.AddWebServiceToDictionary(WebServiceEnum.Default);
            this.AddWebServiceToDictionary(WebServiceEnum.CFResponseService);
            this.AddWebServiceToDictionary(WebServiceEnum.CheckInRegService);
            this.AddWebServiceToDictionary(WebServiceEnum.CheckinService);
            this.AddWebServiceToDictionary(WebServiceEnum.GetEventFieldsService);
            this.AddWebServiceToDictionary(WebServiceEnum.GetEventRegistrationsService);
            this.AddWebServiceToDictionary(WebServiceEnum.GetEventsService);
            this.AddWebServiceToDictionary(WebServiceEnum.MemberAuthService);
            this.AddWebServiceToDictionary(WebServiceEnum.RegTrackerService);
            this.AddWebServiceToDictionary(WebServiceEnum.RegUpdateService);
            this.AddWebServiceToDictionary(WebServiceEnum.ReportsService);
            this.AddWebServiceToDictionary(WebServiceEnum.RetrieveAllRegsService);
            this.AddWebServiceToDictionary(WebServiceEnum.RetrieveSingleRegService);
            this.AddWebServiceToDictionary(WebServiceEnum.XmlUpdaterService);
        }

        private void AddWebServiceToDictionary(WebServiceEnum webService)
        {
            foreach (WebService service in this.AllConfiguration.WebServices.WebService)
            {
                if (service.Name.Equals(webService.ToString()))
                {
                    this.WebServiceConfiguration.Add(webService, service);
                }
            }
        }

        private void LoadBrowser()
        {
            this.LoadBrowser(this.AllConfiguration.Browsers.Current);
        }

        private void LoadBrowser(string browserString)
        {
            foreach (Browser br in this.AllConfiguration.Browsers.Browser)
            {
                if (br.Name.Equals(browserString))
                {
                    this.CurrentBrowser = br;
                }
            }
        }
    }
}