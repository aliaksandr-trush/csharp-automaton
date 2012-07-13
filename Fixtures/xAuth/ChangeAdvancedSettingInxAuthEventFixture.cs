namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Managers.Register;

    [TestFixture]
    public class ChangeAdvancedSettingInxAuthEventFixture : ExternalAuthenticationFixtureBase
    {
        private const string EventName = "XAuthFixture_ChangeAdvancedSettingInxAuthEvent";
        private const string OtherEventName = "XAuthFixture_ChangeAdvancedSettingInxAuthEvent_Prepare";

        [TestCase(FormData.XAuthType.ByEmailPassword, false)]
        [TestCase(FormData.XAuthType.ByUserNamePassword, false)]
        [TestCase(FormData.XAuthType.ByEmail, false)]
        [TestCase(FormData.XAuthType.ByUserName, false)]
        [Test]
        public void TestChangeAdvancedSettingInxAuthEvent(FormData.XAuthType xAuthType, bool onSite)
        {
            this.ChangeAdvancedSettingInxAuthEvent(xAuthType, onSite);
        }

        public int ChangeAdvancedSettingInxAuthEvent(FormData.XAuthType xAuthType, bool onSite)
        {
            RegisterManager.XAuthPersonalInfo testAccount = RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1];

            //Clear up
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();

            //Create an other event
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder("xAuth");
            ManagerSiteMgr.DeleteEventByName(OtherEventName + "_" + xAuthType.ToString());
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            int otherEventId = BuilderMgr.GetEventId();
            string otherEventSessionId = BuilderMgr.GetEventSessionId();

            BuilderMgr.SetEventNameAndShortcut(OtherEventName + "_" + xAuthType.ToString());
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(xAuthType);
            if (xAuthType == FormData.XAuthType.ByUserName || xAuthType == FormData.XAuthType.ByUserNamePassword)
            {
                Managers.ManagerProvider.XAuthMgr.TypeDescriptionForIdentifer("USERID");
            }
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(xAuthType);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();

            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            if (onSite)
            {
                ManagerSiteMgr.OpenEventDashboard(otherEventId);
                ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
                ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
                ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();
                ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);
                ManagerSiteMgr.DashboardMgr.ChangeStatusIfNecessary(DashboardManager.EventStatus.OnSite);
            }

            //Create a register in an other event using XAUTH user's email.
            if (onSite)
                RegisterMgr.OpenOnSiteRegisterPage(otherEventId);
            else
                RegisterMgr.OpenRegisterPage(otherEventId);

            RegisterMgr.Checkin(testAccount.Email);
            RegisterMgr.Continue();
            switch (xAuthType)
            {
                case FormData.XAuthType.ByUserName:
                    RegisterMgr.XAuth_EnterUserName(testAccount.UserName);
                    break;
                case FormData.XAuthType.ByUserNamePassword:
                    RegisterMgr.XAuth_EnterUserName(testAccount.UserName);
                    RegisterMgr.EnterPassword(testAccount.XAuthPassword);
                    break;
                case FormData.XAuthType.ByEmail:
                    break;
                case FormData.XAuthType.ByEmailPassword:
                    RegisterMgr.EnterPassword(testAccount.XAuthPassword);
                    break;
                default:
                    break;
            }
            RegisterMgr.Continue();
            switch (xAuthType)
            {
                case FormData.XAuthType.ByUserName:
                case FormData.XAuthType.ByEmail:
                    RegisterMgr.XAuth_SetDefaultStandardPersonalInfoFields(testAccount, false);
                    RegisterMgr.EnterPersonalInfoPassword(testAccount.Password);
                    break;
                case FormData.XAuthType.ByUserNamePassword:
                case FormData.XAuthType.ByEmailPassword:
                    RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
                    break;
                default:
                    break;
            }
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();


            //Begin Test
            // 14
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder("xAuth");
            ManagerSiteMgr.DeleteEventByName(EventName + "_" + xAuthType.ToString());
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            int eventId = BuilderMgr.GetEventId();
            string eventSessionId = BuilderMgr.GetEventSessionId();
            BuilderMgr.SetEventNameAndShortcut(EventName + "_" + xAuthType.ToString());
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");

            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            //14.1
            BuilderMgr.ClickStartPageEventAdvancedSettings();
            Utilities.Utility.ThreadSleep(1);
            BuilderMgr.AdvancedSettingsMgr.VerifyAuthernticationMethodDropDownListIsEditable(false);
            BuilderMgr.AdvancedSettingsMgr.VerifyAuthernticationMethodType(Managers.Builder.EventAdvancedSettingsManager.AuthenicationMothedType.EmailAddressAndExternalAuthentication);

            //14.2 
            BuilderMgr.AdvancedSettingsMgr.EnableRecall(true);
            Utilities.Utility.ThreadSleep(1);
            BuilderMgr.AdvancedSettingsMgr.EnableRecall(false);
            Utilities.Utility.ThreadSleep(1);
            BuilderMgr.AdvancedSettingsMgr.EnableRecall(true);
            Utilities.Utility.ThreadSleep(1);
            //set CrossAccountOnRecall to FLASE
            BuilderMgr.AdvancedSettingsMgr.ClickSaveAndClose();
            BuilderMgr.SaveAndClose();

            if (onSite)
            {
                ManagerSiteMgr.OpenEventDashboard(eventId);
                ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
                ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
                ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();
                ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);
                ManagerSiteMgr.DashboardMgr.ChangeStatusIfNecessary(DashboardManager.EventStatus.OnSite);
            }

            //14.3
            if (onSite)
                RegisterMgr.OpenOnSiteRegisterPage(eventId);
            else
                RegisterMgr.OpenRegisterPage(eventId);

            RegisterMgr.Checkin(testAccount.Email);
            RegisterMgr.Continue();

            RegisterMgr.XAuth_VerifyRegisteredBeforeNoteIsDisplayed(true);

            RegisterMgr.XAuth_VerifyEmailAddressFieldIsDisplayed(true);
            RegisterMgr.XAuth_VerifyEmailAddressFieldIsEditable(false);
            RegisterMgr.XAuth_VerifyEmailAddressFieldValue(testAccount.Email);
            RegisterMgr.XAuth_VerifyPasswordFieldIsDisplayed(true);
            switch (xAuthType)
            {
                case FormData.XAuthType.ByUserName:
                    RegisterMgr.XAuth_VerifyUserNameFieldIsDisplayed(true);
                    RegisterMgr.XAuth_EnterUserName(testAccount.UserName);
                    RegisterMgr.EnterPassword(testAccount.Password);
                    break;
                case FormData.XAuthType.ByUserNamePassword:
                    RegisterMgr.XAuth_VerifyUserNameFieldIsDisplayed(true);
                    RegisterMgr.XAuth_EnterUserName(testAccount.UserName);
                    RegisterMgr.EnterPassword(testAccount.XAuthPassword);
                    break;
                case FormData.XAuthType.ByEmail:
                    RegisterMgr.XAuth_VerifyUserNameFieldIsDisplayed(false);
                    RegisterMgr.EnterPassword(testAccount.Password);
                    break;
                case FormData.XAuthType.ByEmailPassword:
                    RegisterMgr.XAuth_VerifyUserNameFieldIsDisplayed(false);
                    RegisterMgr.EnterPassword(testAccount.XAuthPassword);
                    break;
                default:
                    break;
            }

            //14.4
            RegisterMgr.Continue();
            switch (xAuthType)
            {
                case FormData.XAuthType.ByUserName:
                case FormData.XAuthType.ByEmail:
                    RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
                    RegisterMgr.EnterPersonalInfoPassword(testAccount.Password);
                    break;
                case FormData.XAuthType.ByUserNamePassword:
                case FormData.XAuthType.ByEmailPassword:
                    RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
                    break;
                default:
                    break;
            }

            //14.5
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //14.6
            if (onSite)
                RegisterMgr.OpenOnSiteRegisterPage(eventId);
            else
                RegisterMgr.OpenRegisterPage(eventId);

            RegisterMgr.Checkin(testAccount.Email);
            RegisterMgr.ContinueWithErrors();
            RegisterMgr.XAuth_VerifyAlreadyBeenUsedErrorMessageIsDisplayed(true);

            //14.7
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.XAuth_VerifyRegisteredBeforeNoteIsDisplayed(false);
            RegisterMgr.XAuth_VerifyEmailAddressFieldValue(testAccount.Email);
            RegisterMgr.XAuth_VerifyStartNewRegistrationLinkIsDisplay(true);

            //14.8
            RegisterMgr.ClickStartNewRegistration();
            Assert.AreEqual(true, RegisterMgr.OnCheckinPage());
            RegisterMgr.XAuth_VerifyEmailAddressFieldValue(string.Empty);

            return eventId;
        }
    }
}
