namespace RegOnline.RegressionTest.Fixtures.Base
{
    using RegOnline.RegressionTest.Configuration;
    using NUnit.Framework;

    public class ExternalAuthenticationFixtureBase : FixtureBase
    {
        [SetUp]
        public void RemoveXAuthLiveReg()
        {
            DataHelperTool.RemoveXAuthLiveRegistration(ConfigReader.DefaultProvider.AccountConfiguration.Id); 
        }
    }
}
