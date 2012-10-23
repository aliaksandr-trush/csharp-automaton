namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;

    [TestFixture]
    public class SetupWithOnlyUserIdFixture : ExternalAuthenticationFixtureBase
    {
        private const string EventName = "xAuthFixture_SetupWithOnlyUserId";
        private int eventId;
           
        [Test]
        public void TestSetupWithOnlyUserId()
        {
            RegisterManager.XAuthPersonalInfo testAccount = RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1];

            //remove current customer's xAuth registers first
            //because a new register or an old register (register customer's another event) for the customer,
            //the register process is different, so i need remove the test register, treat it as a new register everytime.
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_UserDefined("xAuth");
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();

            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            //5
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(DataCollection.EventData_Common.XAuthType.ByEmail);
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(DataCollection.EventData_Common.XAuthType.ByUserName);

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

            Assert.AreEqual(false, UIUtil.DefaultProvider.IsElementDisplay("spForgetPasswordUrl", LocateBy.Id));

            //5.1 
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(DataCollection.EventData_Common.XAuthType.ByUserName);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(DataCollection.EventData_Common.XAuthType.ByUserName);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyPassTest();
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(2);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            //5.2
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin(testAccount.Email);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_TypeUserId(testAccount.UserName);
            RegisterMgr.Continue();

            ////Assert PI information
            RegisterMgr.XAuth_SetDefaultStandardPersonalInfoFields(testAccount, false);
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.EnterPersonalInfoPassword(testAccount.Password);

            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }


        
    }
}