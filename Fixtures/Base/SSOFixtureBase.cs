namespace RegOnline.RegressionTest.Fixtures.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.DataCollection;

    public class SSOFixtureBase : FixtureBase
    {
        [SetUp]
        public void RemoveSSOLiveReg()
        {
            ConfigReader.DefaultProvider.ReloadAccount(ConfigReader.AccountEnum.SSO);
            DataHelperTool.RemoveXAuthLiveRegistration(ConfigReader.DefaultProvider.AccountConfiguration.Id);
        }

        /// <summary>
        /// There is one test case that modify endpoint url to verify test registrations are deleted after endpoint url changed,
        /// so we have to set it back to the correct value, in case that case fail half way to leftthe url incorrect.
        /// </summary>
        [TearDown]
        public void SetEndpointUrl()
        {
            AccessData.SetSSOEndpointUrl(ConfigReader.DefaultProvider.AccountConfiguration.Id, SSOData.SSOEndpointURL);
        }
    }
}
