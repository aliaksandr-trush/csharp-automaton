namespace RegOnline.RegressionTest.Configuration
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlConfiguration
    {
        private readonly string XmlConfigFilePath = "RegressionTestConfig.xml";

        public enum Environment
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

        public enum WebService
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

        public enum Browser
        {
            Firefox,
            Chrome
        }

        public RegOnlineRegressionTestConfig AllConfiguration
        {
            get;
            private set;
        }

        public RegOnlineRegressionTestConfigEnvironmentsEnvironment EnvironmentConfiguration
        {
            get;
            private set;
        }

        public RegOnlineRegressionTestConfigEnvironmentsEnvironmentAccount AccountConfiguration
        {
            get;
            private set;
        }

        public Dictionary<WebService, RegOnlineRegressionTestConfigWebService> WebServiceConfiguration
        {
            get;
            private set;
        }

        public RegOnlineRegressionTestConfigBrowsersBrowser CurrentBrowser
        {
            get;
            private set;
        }

        public XmlConfiguration()
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
            XmlSerializer serializer = new XmlSerializer(typeof(RegOnlineRegressionTestConfig));
            XmlTextReader reader = new XmlTextReader(XmlConfigFilePath);
            this.AllConfiguration = (RegOnlineRegressionTestConfig)serializer.Deserialize(reader);
        }

        private void LoadEnvironmentAndAccountConfiguration(string environment, string accountType)
        {
            this.LoadEnvironmentConfiguration(environment);
            this.LoadAccountConfiguration(accountType);
        }

        private void LoadEnvironmentConfiguration(string environment)
        {
            foreach (RegOnlineRegressionTestConfigEnvironmentsEnvironment en in this.AllConfiguration.Environments.Environment)
            {
                if (en.Name.Equals(environment))
                {
                    this.EnvironmentConfiguration = en;
                }
            }
        }

        private void LoadAccountConfiguration(string accountType)
        {
            foreach (RegOnlineRegressionTestConfigEnvironmentsEnvironmentAccount account in this.EnvironmentConfiguration.Account)
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
                this.WebServiceConfiguration = new Dictionary<WebService, RegOnlineRegressionTestConfigWebService>();
            }
            else
            {
                this.WebServiceConfiguration.Clear();
            }

            this.AddWebServiceToDictionary(WebService.LoginService);
            this.AddWebServiceToDictionary(WebService.RegistrationService);
            this.AddWebServiceToDictionary(WebService.EventService);
            this.AddWebServiceToDictionary(WebService.Default);
            this.AddWebServiceToDictionary(WebService.CFResponseService);
            this.AddWebServiceToDictionary(WebService.CheckInRegService);
            this.AddWebServiceToDictionary(WebService.CheckinService);
            this.AddWebServiceToDictionary(WebService.GetEventFieldsService);
            this.AddWebServiceToDictionary(WebService.GetEventRegistrationsService);
            this.AddWebServiceToDictionary(WebService.GetEventsService);
            this.AddWebServiceToDictionary(WebService.MemberAuthService);
            this.AddWebServiceToDictionary(WebService.RegTrackerService);
            this.AddWebServiceToDictionary(WebService.RegUpdateService);
            this.AddWebServiceToDictionary(WebService.ReportsService);
            this.AddWebServiceToDictionary(WebService.RetrieveAllRegsService);
            this.AddWebServiceToDictionary(WebService.RetrieveSingleRegService);
            this.AddWebServiceToDictionary(WebService.XmlUpdaterService);
        }

        private void AddWebServiceToDictionary(WebService webService)
        {
            foreach (RegOnlineRegressionTestConfigWebService service in this.AllConfiguration.WebServices)
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
            foreach (RegOnlineRegressionTestConfigBrowsersBrowser br in this.AllConfiguration.Browsers.Browser)
            {
                if (br.Name.Equals(browserString))
                {
                    this.CurrentBrowser = br;
                }
            }
        }
    }
}