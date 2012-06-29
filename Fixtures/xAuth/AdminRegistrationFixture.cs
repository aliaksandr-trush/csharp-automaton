namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    public class AdminRegistrationFixture : FixtureBase
    {
        private const string EventName = "AdminRegistration";
        private int eventID;
        private string eventSessionId;

        [Test]
        public void AdminRegistrationTest()
        {
            string noAdminXAuthRegistrationEmail = "joe@rrr.com";

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
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyErrorMessages(FormData.TestAccountResult.Success);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(2);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            //Do Registration with an xAuth reg type
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin(noAdminXAuthRegistrationEmail);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //Do an Admin Registration with an xAuth reg type
            RegisterMgr.OpenAdminRegisterPage(this.eventID, eventSessionId);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //Do an Admin Update of an xAuth registration
            RegisterMgr.OpenAdminRegisterPage(this.eventID, eventSessionId);
            RegisterMgr.Checkin(noAdminXAuthRegistrationEmail);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //Do a (standard) update of an Admin xAuth registration
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //Do an Admin Update of an Admin xAuth registration
            RegisterMgr.OpenAdminRegisterPage(this.eventID, eventSessionId);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
    }
}
