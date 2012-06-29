namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnlineCFResponseService;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineCFResponseServiceFixture : APIFixtureBase
    {
        private const string endpointConfigName = "CustomFieldResponseWSSoap";
        private const int eventId = 619714;
        private const int regId = 11487466;
        private const int cfId = 398875;

        protected override Uri RemoteAddressUri { get; set; }

        [Test]
        [Ignore]
        public void TestModify()
        {
            using (CustomFieldResponseWSSoapClient service = new CustomFieldResponseWSSoapClient(endpointConfigName))
            {
                service.Modify(ConfigurationProvider.XmlConfig.PrivateLabelConfiguration.AccountName, ConfigurationProvider.XmlConfig.PrivateLabelConfiguration.Password, cfId, regId, eventId, "");
            }
        }

        [Test]
        [Ignore]
        public void TestAssignSeat()
        {
            using (CustomFieldResponseWSSoapClient service = new CustomFieldResponseWSSoapClient(endpointConfigName))
            {
                service.AssignSeat(ConfigurationProvider.XmlConfig.PrivateLabelConfiguration.AccountName, ConfigurationProvider.XmlConfig.PrivateLabelConfiguration.Password, cfId, regId, eventId, "", "", "", 1, "");
            }
        }
    }
}