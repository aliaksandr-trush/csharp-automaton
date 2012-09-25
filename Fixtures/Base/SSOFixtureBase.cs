namespace RegOnline.RegressionTest.Fixtures.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;

    public class SSOFixtureBase : FixtureBase
    {
        [SetUp]
        public void RemoveSSOLiveReg()
        {
            ConfigReader.DefaultProvider.ReloadAccount(ConfigReader.AccountEnum.SSO);
            DataHelperTool.RemoveXAuthLiveRegistration(ConfigReader.DefaultProvider.AccountConfiguration.Id);
        }
    }
}
