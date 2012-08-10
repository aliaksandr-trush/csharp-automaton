namespace RegOnline.RegressionTest.Fixtures.Gateway
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Builder;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class CardinalRegistrationVisaMCFixture : FixtureBase
    {
        private const string EventName = "CardinalTestEvent";
        private string eventSessionId;
        private int eventId;
        private double[] Price = {24.95, 19.95, 1.02, 29.35};
        private const double EventFee = 10.00;
        private string[] AgendaName = { "Cardinal", "Bishop", "Hierarch", "Polly-O Appreciation" };

        [Test]
        [Category(Priority.One)]
        [Description("152")]

        public void CardinalRegistration()
        {
            this.LoginAndGetSessionID();

            ManagerSiteMgr.DeleteEventByName(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateEvent(EventName, AgendaName, Price, EventFee);
                this.ActivateEvent(eventId);
            }
            else
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);

            this.Register(eventId);

        }     
        
        
        //public void CardinalRegistrationMC()
        //{
        //    CardinalRegistrationVisaMC("5500005555555559");
        //}

        //private void CardinalRegistrationVisaMC(string ccNumber)
        //{
            // Use event 609305 on beta - eventually change to sprint08 account
            //SpecifyConfiguration(TestConfiguration.ConfigName.Beta); removed 8/30/10
            //RegisterMgr.OpenRegisterPage(609305);

            // Register person
            //RegisterMgr.Checkin();
            //string primaryEmail = RegisterMgr.CurrentEmail;
            //RegisterMgr.Continue();

            // Personal Info page
            //RegisterMgr.EnterPersonalInfoNamePrefixSuffix(null, "Cardinal", null, "Tester", null);
            //RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry(null, null, "Test the Card", null);
            //RegisterMgr.EnterPersonalInfoAddress("123 Card Lane", null, "Boulder", "Colorado", null, "80301");
            //RegisterMgr.EnterPersonalInfoPhoneNumbers(null, "3035775100", null, null, null);
            //RegisterMgr.EnterPersonalInfoPassword("zzzzzz");

            // Continue to the next step to finalize the group registration
            //RegisterMgr.Continue();

            //agenda page
           //RegisterMgr.SelectAgendaItems();

            // Continue to the next step to finalize the group registration
            //RegisterMgr.Continue();

            // Finalize - must use Jan 2011 expiration date
            //RegisterMgr.PaymentMgr.EnterCreditCardNumberInfo(ccNumber, "123", "01 - Jan", "2013");
            //RegisterMgr.PaymentMgr.EnterCreditCardNameCountryType(null, "United States", null);
            //RegisterMgr.PaymentMgr.EnterCreditCardAddressInfo(null, null, null, "CO", null);
            //RegisterMgr.FinishRegistration();

            //Cardinal Verification Page
            //Registration.VerifyOnPage(Registration.OnCardinalVerificationPage(), "cardinal verification");
            //UIUtilityProvider.UIHelper.VerifyOnPage(RegisterMgr.OnCardinalVerificationPage(), "cardinal verification");
            //RegisterMgr.SubmitCardinalPassword("1234");

            //Watch out for Active Advantage page here
            //if (RegisterMgr.OnConfirmationRedirectPage())
            //{

            //    UIUtilityProvider.UIHelper.VerifyOnPage(RegisterMgr.OnConfirmationRedirectPage(), "Active Advantage");
            //    RegisterMgr.ClickAdvantageNo();
            //}

            //RegisterMgr.ConfirmRegistration();
        //}

        private void SetAgendaPage(string[] agendaname, double[] price)
        {
            int i = 0;
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.VerifyEventAgendaPage();
            foreach (var name in agendaname)
            {
                BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.CheckBox, name, price[i]);
                i++;
            }            
        }

        private void LoginAndGetSessionID()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.eventSessionId = BuilderMgr.GetEventSessionId();
        }
        
        private void CreateEvent(string eventname, string[] agendaname, double[] price, double eventfee)
        {
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventname);
            BuilderMgr.SetEventFee(eventfee);
            BuilderMgr.SaveAndStay();
            this.SetAgendaPage(agendaname, price);
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndClose(); 
        }

        private void ActivateEvent(int eventid)
        {
            ManagerSiteMgr.OpenEventDashboard(eventId);
            ManagerSiteMgr.DashboardMgr.ActiveEvent(); 
        }

        private void Register(int EventId)
        {
            RegisterMgr.OpenRegisterPage(EventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.SetDefaultStandardPersonalInfoFields();
            RegisterMgr.EnterPersonalInfoPassword(Configuration.ConfigurationProvider.XmlConfig.AccountConfiguration.Password);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();
            RegisterMgr.PaymentMgr.EnterCreditCardNumberInfo("5500005555555559", "123", "01 - Jan", "2013");
            RegisterMgr.PaymentMgr.EnterCreditCardNameCountryType("test test", "United States", null);
            RegisterMgr.PaymentMgr.EnterCreditCardAddressInfo(null, null, null, "CO", null);
            RegisterMgr.FinishRegistration();
            Assert.True(RegisterMgr.OnCardinalVerificationPage());
            RegisterMgr.SubmitCardinalPassword("1234");
            if (RegisterMgr.OnConfirmationRedirectPage())
                RegisterMgr.ClickAdvantageNo();
            RegisterMgr.ConfirmRegistration();   
        }
    }
}