namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;

    public abstract class APIFixtureBase : FixtureBase
    {
        // Note that some api methods use http, some other use https!
        protected Uri BaseUri;
        protected Uri BaseUriWithHttps;

        protected WebService CurrentWebServiceConfig;

        protected abstract Uri RemoteAddressUri { get; set; }

        public APIFixtureBase()
        {
            RequiresBrowser = false;
            BaseUri = new Uri(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl);
            BaseUriWithHttps = new Uri(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrlWithHttps);
        }

        public APIFixtureBase(ConfigReader.WebServiceEnum webService) : this()
        {
            CurrentWebServiceConfig = ConfigReader.DefaultProvider.WebServiceConfiguration[webService];

            if (CurrentWebServiceConfig.HTTPS)
            {
                RemoteAddressUri = new Uri(BaseUriWithHttps, CurrentWebServiceConfig.Url);
            }
            else
            {
                RemoteAddressUri = new Uri(BaseUri, CurrentWebServiceConfig.Url);
            }
        }
    }
}
