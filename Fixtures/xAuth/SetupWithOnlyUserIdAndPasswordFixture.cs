namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;

    [TestFixture]
    public class SetupWithOnlyUserIdAndPasswordFixture : ExternalAuthenticationFixtureBase
    {
        private const string EventName = "XAuthFixture_SetupWithOnlyUserIdAndPassword";

        [Test]
        public void TestSetupWithOnlyUserIdAndPassword()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_UserDefined("xAuth");
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            int eventId = BuilderMgr.GetEventId();
            string eventSessionId = BuilderMgr.GetEventSessionId();

            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            //6.1
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByUserNamePassword);

            if (Configuration.ConfigReader.DefaultProvider.AccountConfiguration.XAuthVersion == "Old")
            {
                Assert.AreEqual(true, UIUtil.DefaultProvider.IsChecked("ctl00_cphDialog_rdoMembership", LocateBy.Id));
                Assert.AreEqual(false, UIUtil.DefaultProvider.IsChecked("ctl00_cphDialog_rdoEmail", LocateBy.Id));
            }
            else
            {
                Assert.AreEqual(true, UIUtil.DefaultProvider.IsChecked("ctl00_cphDialog_xAuth_rdoMembership", LocateBy.Id));
                Assert.AreEqual(false, UIUtil.DefaultProvider.IsChecked("ctl00_cphDialog_xAuth_rdoEmail", LocateBy.Id));
            }

            //6.2 
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByUserNamePassword);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByUserNamePassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyPassTest();

            Managers.ManagerProvider.XAuthMgr.ClickOKButton();

            //6.3
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            //6.4
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_TypeUserId(Managers.ManagerProvider.XAuthMgr.DefaultAccount_UserName);
            RegisterMgr.EnterPassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();

            //Assert PI information
            RegisterMgr.XAuth_VerifyPI(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);

            RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);

            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
    }
}