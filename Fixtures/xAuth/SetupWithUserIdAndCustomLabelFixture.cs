namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    public class SetupWithUserIdAndCustomLabelFixture : FixtureBase
    {
        private const string EventName = "SetupWithUserIdAndCustomLabel";
        private const string DescriptionForIdentifer = "Active Regonline User ID";       
        private int eventID;

        [Test]
        public void SetupWithUserIdAndCustomLabelTest()
        {
            //Test Case 7
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder("xAuth");
            ManagerSiteMgr.DeleteEventByName(EventName);

            //create existing email address event
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByUserNamePassword);
            Managers.ManagerProvider.XAuthMgr.TypeDescriptionForIdentifer(DescriptionForIdentifer);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByUserNamePassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();            
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            
            BuilderMgr.SaveAndClose();

            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.CheckinWithEmail(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyUserIdLabel(DescriptionForIdentifer + ":");
            RegisterMgr.XAuth_TypeUserId(Managers.ManagerProvider.XAuthMgr.DefaultAccount_UserName);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
    }
}
