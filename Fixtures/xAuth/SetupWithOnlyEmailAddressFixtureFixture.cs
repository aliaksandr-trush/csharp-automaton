namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.DataCollection;

    [TestFixture]
    public class SetupWithOnlyEmailAddressFixtureFixture : ExternalAuthenticationFixtureBase
    {
        private const string EventName = "SetupWithOnlyEmailAddressTest";
        private int eventID;

        [Test]
        public void SetupWithOnlyEmailAddressTest()
        {
            RegisterManager.XAuthPersonalInfo testAccount = RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1];

            //remove current customer's xAuth registers first
            //because a new register or an old register (register customer's another event) for the customer,
            //the register process is different, so i need remove the test register, treat it as a new register everytime.
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmail);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByEmail);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(2);
            BuilderMgr.RegTypeMgr.VerifyEnableXAuthTextIsEnableExternalAuthentication(true);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            //a new register for the customer, register this event
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin(testAccount.Email);
            RegisterMgr.Continue();
            if (!RegisterMgr.OnLoginPage())
            {
                Assert.Fail("Not on login page!");
            }
            RegisterMgr.Continue();
            if (!RegisterMgr.OnPersonalInfoPage())
            {
                Assert.Fail("Not on Personal Info page!");
            }
            RegisterMgr.XAuth_SetDefaultStandardPersonalInfoFields(testAccount,false);
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.EnterPersonalInfoPassword(testAccount.Password);
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
    }
}
