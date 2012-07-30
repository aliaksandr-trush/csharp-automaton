namespace RegOnline.RegressionTest.Fixtures.SpecialOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegisterTwiceFixture : FixtureBase
    {
        private int eventID = 0;
        private string sessionId = string.Empty;
        private string registrationEmail = string.Empty;

        private enum RegisterWithSameEmailSetting
        {
            Allowed,
            Unallowed
        }

        #region Test Methods
        [Test]
        [Category(Priority.One)]
        [Description("707")]
        public void AllowRegisterWithSameEmail()
        {
            CreateNewEvent("AllowRegister", RegisterWithSameEmailSetting.Allowed);

            // Register for the register twice test event
            FirstRegistration();

            // Register for the test event using the same email
            // allow to start a new registration with the same email 
            SecondRegistrationWithSameEmail(RegisterWithSameEmailSetting.Allowed);
        }

        [Test]
        [Category(Priority.One)]
        [Description("342")]
        public void UnallowRegisterWithSameEmail()
        {
            CreateNewEvent("UnallowRegister", RegisterWithSameEmailSetting.Unallowed);

            // Register for the register twice test event
            FirstRegistration();

            // Register for the test event using the same email
            // don't allow to start a new registration with the same email and get error message
            SecondRegistrationWithSameEmail(RegisterWithSameEmailSetting.Unallowed);
        }

        #endregion

        #region Helper Methods
        [Step]
        private void CreateNewEvent(string eventName, RegisterWithSameEmailSetting setting)
        {
            ManagerSiteMgr.OpenLogin();

            // login to regression testing account
            ManagerSiteMgr.Login();

            // make sure we're on the Events tab
            ManagerSiteMgr.GoToEventsTabIfNeeded();

            // click "Regression" folder
            ManagerSiteMgr.SelectFolder();

            // delete the event
            ManagerSiteMgr.DeleteEventByName(eventName);

            // select "Pro Event" from drop down
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

            // test start page
            CreateEventStartPage(eventName, setting);

            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);

            // test agenda page
            CreateEventAgendaPage("Test-AI", 10);

            // go to next page
            BuilderMgr.Next();

            // go to next page
            BuilderMgr.Next();

            // test merchandise page
            CreateEventMerchandisePage("TestMerchandise", 20);

            // go to next page
            BuilderMgr.Next();

            // test checkout page
            CreateEventCheckoutPage();

            // go to next page
            BuilderMgr.Next();

            // test confirmation page
            CreateEventConfirmationPage();

            // get event id
            eventID = BuilderMgr.GetEventId();

            //get sessionId
            sessionId = BuilderMgr.GetEventSessionId();

            // save and close
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void FirstRegistration()
        {
            InitializeRegistration();

            // Check in
            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            FinishRemainingRegistration();

            // Get registrant's emial after registration is finished.
            registrationEmail = RegisterMgr.CurrentEmail;
        }

        [Step]
        private void SecondRegistrationWithSameEmail(RegisterWithSameEmailSetting eventDupsetting)
        {
            InitializeRegistration();

            // Check in
            RegisterMgr.Checkin(this.registrationEmail);
            
            if (eventDupsetting == RegisterWithSameEmailSetting.Unallowed)
            {
                RegisterMgr.ContinueWithErrors();
                RegisterMgr.VerifyErrorMessage(Managers.Register.RegisterManager.Error.CheckinDisallowDuplicateEmail);

                // Check in using another email
                RegisterMgr.Checkin();
                RegisterMgr.Continue();
            }
            else if (eventDupsetting == RegisterWithSameEmailSetting.Allowed)
            {
                RegisterMgr.Continue();

                // Begin a New Registration with This Email
                RegisterMgr.OnPasswordDupEmailPage();
                RegisterMgr.ClickPasswordBeginNewReg();
            }

            FinishRemainingRegistration();
        }

        private void InitializeRegistration()
        {
            RegisterMgr.CurrentEventId = eventID;
            RegisterMgr.OpenRegisterPage(eventID);
        }

        private void FinishRemainingRegistration()
        {
            // Enter profile info
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();

            // Registration.SelectMerchandise(1);
            RegisterMgr.SelectMerchandiseQuantityByName("TestMerchandise", 1);
            RegisterMgr.Continue();

            // TODO: need to abstract calculating the total
            RegisterMgr.PayMoneyAndVerify(30.00, 30.00, RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void CreateEventStartPage(string eventName, RegisterWithSameEmailSetting setting)
        {
            // Verify initial defaults
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);

            // Enter event start page info
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventName);

            if (setting == RegisterWithSameEmailSetting.Unallowed)
            {
                BuilderMgr.ClickStartPageEventAdvancedSettings();
                BuilderMgr.AdvancedSettingsMgr.SetFieldValue(Managers.Builder.EventAdvancedSettingsManager.AuthenticationField.NoDupEmails, true.ToString());
                BuilderMgr.AdvancedSettingsMgr.ClickSaveAndClose();
            }

            // Save and stay
            BuilderMgr.SaveAndStay();
        }

        private void CreateEventAgendaPage(string agendaname, double price)
        {
            // Verify splash page
            BuilderMgr.VerifySplashPage();

            // continue to agenda page
            BuilderMgr.ClickYesOnSplashPage();

            // verify agenda page
            BuilderMgr.VerifyEventAgendaPage();

            // add/modify agenda items of each type
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.CheckBox, agendaname, price);
        }

        private void CreateEventMerchandisePage(string merchname, double price)
        {
            // add test merchandise
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Fixed, merchname, price, null, null);
        }

        private void CreateEventCheckoutPage()
        {
            // enter event checkout page info
            BuilderMgr.EnterEventCheckoutPage();

            // save and stay
            BuilderMgr.SaveAndStay();
        }

        private void CreateEventConfirmationPage()
        {
            // enter event checkout page info
            BuilderMgr.SetEventConfirmationPage();

            // save and stay
            BuilderMgr.SaveAndStay();

            // verify event checkout page info
            BuilderMgr.VerifyEventConfirmationPage();
        }
        #endregion
    }
}
