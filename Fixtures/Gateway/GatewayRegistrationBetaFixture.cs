namespace RegOnline.RegressionTest.Fixtures.Gateway
{
    using NUnit.Framework;
    using RegOnline.RegressionTest;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class GatewayRegistrationBetaFixture : FixtureBase
    {
        [Test]
        [Category(Priority.One)]
        [Description("459")]
        public void Authorize_dot_net_regTest()
        {
            RegisterWithCreditCard(610986, "4111111111111111", null, "05 - May", "2014");
        }
      
        [Test]
        [Category(Priority.One)]
        [Description("460")]
        public void Cybersource_regTest()
        {
            RegisterWithCreditCard(611044, "4111111111111111", null, "05 - May", "2014");
        }

        [Test]
        [Category(Priority.One)]
        [Description("463")]
        public void Moneris_regTest()
        {
            RegisterWithCreditCard(611734, "4005554444444403", null, "08 - Aug", "2015");
        }

        [Test]
        [Category(Priority.One)]
        [Description("470")]
        public void Payflo_Pro_regTest()
        {
            RegisterWithCreditCard(610989, "4111111111111111", null, "05 - May", "2014");
        }
        
        [Test]
        [Category(Priority.One)]
        [Description("471")]
        public void Paymentech_regTest()
        {
            RegisterWithCreditCard(613402, "4444444444444448", "086", "06 - Jun", "2013");
        }
        
        [Test]
        [Category(Priority.One)]
        [Description("472")]
        public void Website_Payments_Pro_regTest()
        {
            RegisterWithCreditCard(611046, "4801680258796905", "111", "01 - Jan", "2013");
        }

        // Insert more BETA gateway tests here

        [Step]
        private void RegisterWithCreditCard(int eventId, string cardNumber, string cvv, string expMonth, string expYear)
        {
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.CheckinWithEmail("jmariani@regonline.com");
            RegisterMgr.Continue();
            RegisterMgr.ClickPasswordBeginNewReg();
            RegisterMgr.EnterPersonalInfoNamePrefixSuffix(null, "regonline", null, "merchant", null);
            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry(null, null, "merchant", "United States");
            RegisterMgr.EnterPersonalInfoAddress("10182 Telesis Court", null, "San Diego", "California", null, "92121");
            RegisterMgr.EnterPersonalInfoPhoneNumbers(null, "3035775100", null, null, null);
            RegisterMgr.EnterPersonalInfoPassword("zzzzzz");
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.PaymentMgr.EnterCreditCardNumberInfo(cardNumber, cvv, expMonth, expYear);
            RegisterMgr.FinishRegistration();

            // Watch out for Active Advantage page here
            if (RegisterMgr.OnConfirmationRedirectPage())
            {
                RegisterMgr.ClickAdvantageNo();
            }

            RegisterMgr.ConfirmRegistration();
        }
    }
}
