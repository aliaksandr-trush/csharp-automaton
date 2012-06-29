namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;

    [TestFixture]
    public class SetupWithOnlyEmailAddressAndPasswordFixture : FixtureBase
    {
        private const string EventName = "XAuthFixture_SetupWithOnlyEmailAddressAndPassword";

        [Test]
        public void TestSetupWithOnlyEmailAddressAndPassword()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder("xAuth");
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            int eventId = BuilderMgr.GetEventId();
            string eventSessionId =BuilderMgr.GetEventSessionId();

            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            //4.1
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByUserNamePassword);

            if (Configuration.ConfigurationProvider.XmlConfig.AccountConfiguration.XAuthVersion == "Old")
            {
                Assert.AreEqual(true, UIUtilityProvider.UIHelper.IsChecked("ctl00_cphDialog_rdoMembership", LocateBy.Id));
                Assert.AreEqual(false, UIUtilityProvider.UIHelper.IsChecked("ctl00_cphDialog_rdoEmail", LocateBy.Id));
            }
            else
            {
                Assert.AreEqual(true, UIUtilityProvider.UIHelper.IsChecked("ctl00_cphDialog_xAuth_rdoMembership", LocateBy.Id));
                Assert.AreEqual(false, UIUtilityProvider.UIHelper.IsChecked("ctl00_cphDialog_xAuth_rdoEmail", LocateBy.Id));
            }        

            //4.2 
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyPassTest();
            
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();

            //4.3
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            //4.4
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
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