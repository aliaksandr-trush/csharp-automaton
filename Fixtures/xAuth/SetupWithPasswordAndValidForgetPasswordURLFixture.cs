namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    public class SetupWithPasswordAndValidForgetPasswordURLFixture : FixtureBase
    {
        private const string EventName = "SetupWithPasswordAndValidForgetPasswordURL";
        private const string ForgotPasswordURL = "http://www.baidu.com/";
        private const string ForgotPasswordURL2 = "http://www.google.com.hk/";
        private int eventId;

        [Test]
        public void SetupWithPasswordAndValidForgetPasswordURLTest()
        {
            //Test Case 8            
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder("xAuth");
            ManagerSiteMgr.DeleteEventByName(EventName);

            //create existing email address event
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();            
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByUserNamePassword);
            Managers.ManagerProvider.XAuthMgr.TypeForgetPasswordUrl(ForgotPasswordURL);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByUserNamePassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyPassTest();
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            BuilderMgr.SaveAndClose();

            //8.2 ,8.3
            //we can't validate the link in that email, So we only validate that the link "Forgot Your Password" is defined.
            VerifyForgotPasswordUrl(ForgotPasswordURL);

            //8.4,8.5
            SetForgotPasswordUrl(ForgotPasswordURL2);            
            VerifyForgotPasswordUrl(ForgotPasswordURL2);
            
            //8.6
            SetForgotPasswordUrl(string.Empty);
            //we can't validate the link in that email, So we only validate that the link "Forgot Your Password" is defined.
            VerifyForgotPasswordUrl(string.Empty);
        }

        [Step]
        private void SetForgotPasswordUrl(string url)
        {
            string sessionId = ManagerSiteMgr.LoginAndDeleteTestRegsReturnToManagerScreen(this.eventId, "xAuth");
            
            ManagerSiteMgr.SelectFolder("xAuth");
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventId, sessionId);

            //create existing email address event
            BuilderMgr.OpenRegType("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByUserNamePassword);
            Managers.ManagerProvider.XAuthMgr.TypeForgetPasswordUrl(url);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByUserNamePassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyPassTest();
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            BuilderMgr.SaveAndClose();
        }

        [Verify]
        private void VerifyForgotPasswordUrl(string expectedUrl)
        {
            //we can't validate the link in that email, So we only validate that the link "Forgot Your Password" is defined.
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.CheckinWithEmail(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();            
            if(string.IsNullOrEmpty(expectedUrl))
            {
                RegisterMgr.VerifyForgotYourPasswordLinkVisibility(false);
            }
            else
            {
                RegisterMgr.VerifyForgotYourPasswordLinkVisibility(true);
                RegisterMgr.VerifyLoginPageForgotYourPasswordLinkURL(expectedUrl);
                RegisterMgr.ClickLoginPageForgotYourPasswordLinkAndVerify(expectedUrl);
            }
            RegisterMgr.XAuth_TypeUserId(Managers.ManagerProvider.XAuthMgr.DefaultAccount_UserName);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
    }
}
