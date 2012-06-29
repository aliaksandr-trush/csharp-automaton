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

        protected abstract Uri RemoteAddressUri { get; set; }

        public APIFixtureBase()
        {
            RequiresBrowser = false;
            BaseUri = new Uri(ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrl);
            BaseUriWithHttps = new Uri(ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrlWithHttps);
        }
    }
}
