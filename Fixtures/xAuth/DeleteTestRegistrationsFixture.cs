namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;

    [TestFixture]
    public class DeleteTestRegistrationsFixture : ExternalAuthenticationFixtureBase
    {
        private const string EventName = "DeleteTestRegistrations";
        private int eventID;
        private string eventSessionId;

        [Test]
        public void DeleteTestRegistrationsTest()
        {
            string noXAuthRegisterEmailAddress = string.Empty;
            string xAuthRegisterEmailAddress = "joe@rrr.com";

            Managers.ManagerProvider.XAuthMgr.ApprovedXAuthRoleForCustomer(false);
            Managers.ManagerProvider.XAuthMgr.RemoveLiveXAuthEventForCustomer();

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            ManagerSiteMgr.DeleteEventByName(EventName);

            //create an event and add reg type 1
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            // Get sessionId
            this.eventSessionId = BuilderMgr.GetEventSessionId();
            // Get event id
            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            //update xAuth type as EmailPassword
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(DataCollection.EventData_Common.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(DataCollection.EventData_Common.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyErrorMessages(DataCollection.EventData_Common.TestAccountResult.Success);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(2);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            //create reg type 2
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype2");
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            //register regtype1:xAuth regtype
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(xAuthRegisterEmailAddress);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //register regtype2:no xAuth regtype
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype2");
            noXAuthRegisterEmailAddress = RegisterMgr.ComposeUniqueEmailAddress();
            RegisterMgr.ContinueWithErrors();
            RegisterMgr.CheckinWithEmail(noXAuthRegisterEmailAddress);
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //cancel xauth registers
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(xAuthRegisterEmailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.ClickCancelEntireGroup(true);

            //cancel non-xauth registers
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.Checkin(noXAuthRegisterEmailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword();
            RegisterMgr.Continue();
            RegisterMgr.ClickCancelEntireGroup(true);

            //re-register regtype1:xAuth regtype
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //re-register regtype2:no xAuth regtype
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype2");
            noXAuthRegisterEmailAddress = RegisterMgr.ComposeUniqueEmailAddress();
            RegisterMgr.ContinueWithErrors();
            RegisterMgr.CheckinWithEmail(noXAuthRegisterEmailAddress);
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //delete registers
            this.eventSessionId = ManagerSiteMgr.LoginAndDeleteTestRegsReturnToManagerScreen(this.eventID, "xAuth");
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, eventSessionId);
            ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);
            //Assert.AreEqual(0, ManagerSiteMgr.Dashboard.GetTotalRegCountFromDashboard());

            //re-register regtype1:xAuth regtype
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //re-register regtype2:no xAuth regtype
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.CheckinWithEmail(noXAuthRegisterEmailAddress);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            Managers.ManagerProvider.XAuthMgr.ApprovedXAuthRoleForCustomer(true);

            //activate the event
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            this.eventSessionId = BuilderMgr.GetEventSessionId();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, eventSessionId);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.CheckAgreementForTermsOfService(true);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.TypeInitialing("123");
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();

            //all registrations are deleted
            ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);
            //Assert.AreEqual(0, ManagerSiteMgr.Dashboard.GetTotalRegCountFromDashboard());

            Managers.ManagerProvider.XAuthMgr.ApprovedXAuthRoleForCustomer(false);
            Managers.ManagerProvider.XAuthMgr.RemoveLiveXAuthEventForCustomer();
        }
    }
}
