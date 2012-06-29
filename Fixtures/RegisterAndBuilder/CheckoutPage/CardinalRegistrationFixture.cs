namespace RegOnline.RegressionTest.Fixtures.RegisterAndBuilder.CheckoutPage
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class CardinalRegistrationFixture : FixtureBase
    {
        [Test]
        [Category(Priority.Two)]
        [Description("139")]
        public void CardinalRegistration()
        {
			// Use event 609305 on beta - eventually change to sprint08 account
            //SpecifyConfiguration(TestConfiguration.ConfigName.Beta);
            RegisterMgr.OpenRegisterPage(619675); // Event name for 619675 is 'Cardinal Test'

            // Register person
            RegisterMgr.Checkin();
            string primaryEmail = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            
            // Personal Info page
            RegisterMgr.EnterPersonalInfoNamePrefixSuffix(null, "Cardinal", null, "Tester", null);
            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry(null, null, "Test the Card", null);
            RegisterMgr.EnterPersonalInfoAddress("123 Card Lane", null, "Boulder", "Colorado", null, "99701");
            RegisterMgr.EnterPersonalInfoPhoneNumbers(null, "3035775100", null, null, null);
            RegisterMgr.EnterPersonalInfoPassword("zzzzzz");

            // Continue to the next step to finalize the group registration
            RegisterMgr.Continue();

            //agenda page
            RegisterMgr.SelectAgendaItems();

            // Continue to the next step to finalize the group registration
            RegisterMgr.Continue();

            // Finalize - must use Jan 2012 expiration date
            RegisterMgr.PaymentMgr.EnterCreditCardNumberInfo("4444444444444448", "123", "01 - Jan", "2013");
            RegisterMgr.PaymentMgr.EnterCreditCardNameCountryType("test test", "United States", null);
            RegisterMgr.PaymentMgr.EnterCreditCardAddressInfo(null, null, null, "CO", null);
            RegisterMgr.FinishRegistration();

            // Cardinal Verification Page
            UIUtilityProvider.UIHelper.VerifyOnPage(RegisterMgr.OnCardinalVerificationPage(), "cardinal verification");
            RegisterMgr.SubmitCardinalPassword("1234");

            // Watch out for Active Advantage page here
            if (RegisterMgr.OnConfirmationRedirectPage())
            {

                UIUtilityProvider.UIHelper.VerifyOnPage(RegisterMgr.OnConfirmationRedirectPage(), "Active Advantage");
                RegisterMgr.ClickAdvantageNo();
            }
            RegisterMgr.ConfirmRegistration();
        }

        [Test]
        [Category(Priority.One)]
        [Description("728")]
        public void NoCardinalRegistration()
        {
            // Use event 609305 on beta - eventually change to sprint08 account
            //SpecifyConfiguration(TestConfiguration.ConfigName.Beta);
            RegisterMgr.OpenRegisterPage(619675);

            // Register person
            RegisterMgr.Checkin();
            string primaryEmail = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();

            // Personal Info page
            RegisterMgr.EnterPersonalInfoNamePrefixSuffix(null, "Cardinal", null, "Tester", null);
            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry(null, null, "Test the Card", null);
            RegisterMgr.EnterPersonalInfoAddress("123 Card Lane", null, "Boulder", "Colorado", null, "99701");
            RegisterMgr.EnterPersonalInfoPhoneNumbers(null, "3035775100", null, null, null);
            RegisterMgr.EnterPersonalInfoPassword("zzzzzz");

            // Continue to the next step to finalize the group registration
            RegisterMgr.Continue();

            //agenda page
            RegisterMgr.SelectAgendaItems();

            // Continue to the next step to finalize the group registration
            RegisterMgr.Continue();

            // Finalize - must use Jan 2012 expiration date
            //Registration.Payment.EnterCreditCardNumberInfo("4111111111111111", "123", "06 - Jun", "2014");
            RegisterMgr.PaymentMgr.EnterCreditCardNumberInfo("4444444444444448", "123", "06 - Jun", "2015");
            RegisterMgr.PaymentMgr.EnterCreditCardNameCountryType("test test", "United States", null);
            RegisterMgr.PaymentMgr.EnterCreditCardAddressInfo(null, null, null, "CO", null);
            RegisterMgr.FinishRegistration();

            // Verify Active Advantage, make sure no cardinal page is present
            if (RegisterMgr.OnConfirmationRedirectPage())
            {

                UIUtilityProvider.UIHelper.VerifyOnPage(RegisterMgr.OnConfirmationRedirectPage(), "Active Advantage");
                RegisterMgr.ClickAdvantageNo();
            }

            RegisterMgr.ConfirmRegistration();
        }
    }
}