namespace RegOnline.RegressionTest.Fixtures.RegisterAndBuilder.CheckoutPage
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class CardinalRegistrationVisaMCFixture : FixtureBase
    {
        [Test]
        [Category(Priority.One)]
        [Description("152")]
        public void CardinalRegistrationMC()
        {
            CardinalRegistrationVisaMC("5500005555555559");
        }

        private void CardinalRegistrationVisaMC(string ccNumber)
        {
            // Use event 609305 on beta - eventually change to sprint08 account
            //SpecifyConfiguration(TestConfiguration.ConfigName.Beta); removed 8/30/10
            RegisterMgr.OpenRegisterPage(609305);

            // Register person
            RegisterMgr.Checkin();
            string primaryEmail = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();

            // Personal Info page
            RegisterMgr.EnterPersonalInfoNamePrefixSuffix(null, "Cardinal", null, "Tester", null);
            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry(null, null, "Test the Card", null);
            RegisterMgr.EnterPersonalInfoAddress("123 Card Lane", null, "Boulder", "Colorado", null, "80301");
            RegisterMgr.EnterPersonalInfoPhoneNumbers(null, "3035775100", null, null, null);
            RegisterMgr.EnterPersonalInfoPassword("zzzzzz");

            // Continue to the next step to finalize the group registration
            RegisterMgr.Continue();

            //agenda page
            RegisterMgr.SelectAgendaItems();

            // Continue to the next step to finalize the group registration
            RegisterMgr.Continue();

            // Finalize - must use Jan 2011 expiration date
            RegisterMgr.PaymentMgr.EnterCreditCardNumberInfo(ccNumber, "123", "01 - Jan", "2013");
            RegisterMgr.PaymentMgr.EnterCreditCardNameCountryType(null, "United States", null);
            RegisterMgr.PaymentMgr.EnterCreditCardAddressInfo(null, null, null, "CO", null);
            RegisterMgr.FinishRegistration();

            //Cardinal Verification Page
            //Registration.VerifyOnPage(Registration.OnCardinalVerificationPage(), "cardinal verification");
            UIUtilityProvider.UIHelper.VerifyOnPage(RegisterMgr.OnCardinalVerificationPage(), "cardinal verification");
            RegisterMgr.SubmitCardinalPassword("1234");

            //Watch out for Active Advantage page here
            if (RegisterMgr.OnConfirmationRedirectPage())
            {

                UIUtilityProvider.UIHelper.VerifyOnPage(RegisterMgr.OnConfirmationRedirectPage(), "Active Advantage");
                RegisterMgr.ClickAdvantageNo();
            }

            RegisterMgr.ConfirmRegistration();
        }
    }
}