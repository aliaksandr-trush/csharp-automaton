namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    
    /// <summary>
    /// this testfixture is used to test directory login with password required
    /// 1. In an account with XAuth, set authenticate on UserID only
    /// 2. Create an event with an XAuth regtype
    /// 3. create a Directory with enabled the following configuration
    ///   1)Include this Directory in the list of available Directories
    ///   2)Require loin to access directory
    /// </summary>
    [TestFixture]
    public class XAuthDirectory : ExternalAuthenticationFixtureBase
    {
        private string testEventName = "XAuth_Directories";
        private string xauthRegTypeName = "XAuth_RegType";
        private string nonXAuthRegTypeName = "NonXAuth_RegType";
        private string directoryName = "XAuth_Directory";
        private string wrongUserName = "wrongName";
        private string wrongPassword = "wrongPassword";
        private string xauthForgotPasswordURL = "http://wwww.bing.com";
        private int eventID;
        private string eventSessionId;
        private Managers.Report.DirectoryReportManager drManager = new Managers.Report.DirectoryReportManager();

        /// <summary>
        /// 1. Login manager
        /// 2. add a new event without regtype
        /// 3. return to manager
        /// </summary>
        ///[TestFixtureSetUp]
        public void Start()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            ManagerSiteMgr.DeleteEventByName(testEventName);

            // Create an existing event and add reg type
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            // Get sessionId
            this.eventSessionId = BuilderMgr.GetEventSessionId();
            // Get event id
            BuilderMgr.SetEventNameAndShortcut(testEventName);
            BuilderMgr.SaveAndClose();

            this.eventID = ManagerSiteMgr.GetFirstEventId(testEventName);

            this.CreateDirectory();
        }

        /// <summary>
        /// create an attendee directory with
        /// 1. "Include this Directory in the list of available Directories" is enabled
        /// 2. "Require login to access directory" is enabled
        /// </summary>
        private void CreateDirectory()
        {
            //create attendee directories
            ManagerSiteMgr.OpenEventDashboard(this.eventID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.Directories);

            ManagerSiteMgr.DashboardMgr.AddAttendeeDirectory(directoryName);

            Managers.Manager.Dashboard.AttendeeDirectoryCreator directory = ManagerSiteMgr.DashboardMgr.AttendeeDirectoryCreationMgr;

            directory.ChooseTab(Managers.Manager.Dashboard.AttendeeDirectoryCreator.DirectoryTab.LinksSecurity);

            directory.PutItOnListOfAvailableDirectories();
            directory.EnablePassword();

            directory.Apply();
            directory.Cancel();
        }

        private void addRegTypeAndXauth(FormData.XAuthType xauthType, string forgotPasswordUrl = "")
        {
            ManagerSiteMgr.OpenEventBuilderStartPage(eventID, eventSessionId);

            //delete existing regtype first
            if (BuilderMgr.HasRegType(xauthRegTypeName))
            {
                BuilderMgr.DeleteRegType(xauthRegTypeName);
            }
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(xauthRegTypeName);
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetValidateMemberRequirePassword(true);
            Managers.ManagerProvider.XAuthMgr.TypeForgetPasswordUrl(forgotPasswordUrl);
            Managers.ManagerProvider.XAuthMgr.SetValidateMemberRequirePassword(false);

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(xauthType);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(xauthType);

            if (xauthType == FormData.XAuthType.ByUserName ||
                xauthType == FormData.XAuthType.ByUserNamePassword)
            {
                Managers.ManagerProvider.XAuthMgr.TypeDescriptionForIdentifer("User Label");
            }

            if (xauthType == FormData.XAuthType.ByUserNamePassword)
            {
                Managers.ManagerProvider.XAuthMgr.TypeForgetPasswordUrl(forgotPasswordUrl);
            }

            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyPassTest();
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();

            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
        }

        [TestCase(FormData.XAuthType.ByUserName)]
        [TestCase(FormData.XAuthType.ByUserNamePassword)]
        [TestCase(FormData.XAuthType.ByEmail)]
        [TestCase(FormData.XAuthType.ByEmailPassword)]
        public void XAuthRegistration(FormData.XAuthType xauthType)
        {
            Start();
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();

            addRegTypeAndXauth(xauthType, xauthType == FormData.XAuthType.ByUserNamePassword ? xauthForgotPasswordURL : "");

            Managers.Register.RegisterManager.XAuthPersonalInfo personalInfo = RegisterMgr.TestAccounts[Managers.Register.RegisterManager.TestAccountType.DefaultAccount1];

            RegisterMgr.ProcessRegistration(personalInfo,
                eventID, xauthType, SelectRegType);

            RegisterMgr.ClickViewDirectory(directoryName);

            ManagerSiteMgr.DashboardMgr.SelectAttendeeDirectoryWindow();

            processDirectoryVerify(xauthType, personalInfo);
            //DeleteEvent();
        }

        [Test]
        public void XAuthWithWrongUserIDPassword()
        {
            Start();
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();

            addRegTypeAndXauth(FormData.XAuthType.ByUserName);

            Managers.Register.RegisterManager.XAuthPersonalInfo personalInfo = RegisterMgr.TestAccounts[Managers.Register.RegisterManager.TestAccountType.DefaultAccount1];

            RegisterMgr.ProcessRegistration(personalInfo, eventID, FormData.XAuthType.ByUserName, SelectRegType);

            RegisterMgr.ClickViewDirectory(directoryName);

            ManagerSiteMgr.DashboardMgr.SelectAttendeeDirectoryWindow();

            drManager.EnterEmailAddressOnLogin(personalInfo.Email);
            drManager.Continue();

            //verify userID field, password are displayed
            drManager.VerifyUserIDTextboxIsPresent();
            drManager.VerifyPasswordTextboxIsPresent();
            drManager.VerifyXAuthForgotPasswordLinkIsPresent(false);

            //input correct userName but wrong password
            drManager.EnterUserIDOnLogin(personalInfo.UserName);
            drManager.EnterPasswordOnLogin(wrongPassword);
            drManager.Continue();

            //verify we are still on directory login page
            drManager.VerifyIsOnDirectoryLoginPage();
            //verify the error message shows up
            drManager.VerifyErrorMessage(XAuthManager.ErrorMsg_MustSignInCorrectValue);

            //input correct password but wrong userid
            drManager.EnterUserIDOnLogin(wrongUserName);
            drManager.EnterPasswordOnLogin(personalInfo.XAuthPassword);
            drManager.Continue();

            //verify we are still on directory login page
            drManager.VerifyIsOnDirectoryLoginPage();
            //verify the error message shows up
            drManager.VerifyErrorMessage(XAuthManager.ErrorMsg_MustSignInCorrectValue);
            
            drManager.EnterUserIDOnLogin(personalInfo.UserName);
            drManager.EnterPasswordOnLogin(personalInfo.XAuthPassword);
            drManager.Continue();

            drManager.VerifyIsOnReportPage();
            //DeleteEvent();
        }


        [Test]
        public void NonXauthRegistration()
        {
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();
            Start();
            addRegTypeAndXauth(FormData.XAuthType.ByUserName);

            BuilderMgr.AddRegType(nonXAuthRegTypeName);

            Managers.Register.RegisterManager.PersonalInfo personalInfo = RegisterMgr.NormalAccount;

            RegisterMgr.ProcessRegistration(personalInfo, eventID, FormData.XAuthType.NotUse, SelectRegType);

            RegisterMgr.ClickViewDirectory(directoryName);

            ManagerSiteMgr.DashboardMgr.SelectAttendeeDirectoryWindow();

            drManager.EnterEmailAddressOnLogin(personalInfo.Email);
            drManager.Continue();

            //verify userID field, password are displayed
            drManager.VerifyUserIDTextboxIsPresent(false);
            drManager.VerifyPasswordTextboxIsPresent();
            drManager.VerifyNonXAuthForgotPasswordLinkIsPresent();

            drManager.ClickNonXauthForgotPasswordLink();
            drManager.VerifyNonXAuthForgotPasswordLinkIsPresent();

            drManager.EnterFirstName(personalInfo.FirstName);
            drManager.EnterLastName(personalInfo.LastName);
            drManager.EnterForgotPasswordEmail(personalInfo.Email);
            drManager.Submit();

            drManager.VerifySuccessSentMsgIsPresent();

            //DeleteEvent();

        }

        public void SelectRegType(FormData.XAuthType xAuthType)
        {
            if (xAuthType == FormData.XAuthType.NotUse)
            {
                RegisterMgr.SelectRegType(nonXAuthRegTypeName);
            }
            else
            {
                RegisterMgr.SelectRegType(xauthRegTypeName);
            }
        }

        private void processDirectoryVerify(FormData.XAuthType authType,
            Managers.Register.RegisterManager.XAuthPersonalInfo personalInfo)
        {
            drManager.EnterEmailAddressOnLogin(personalInfo.Email);

            drManager.Continue();

            drManager.VerifyEmailTextboxIsPresent();
            if (authType == FormData.XAuthType.ByUserName ||
                authType == FormData.XAuthType.ByUserNamePassword)
            {
                drManager.VerifyUserIDTextboxIsPresent();
                drManager.EnterUserIDOnLogin(personalInfo.UserName);
            }
            if (authType == FormData.XAuthType.ByUserNamePassword)
            {
                drManager.VerifyXAuthForgotPasswordLinkIsPresent(true);
            }
            drManager.VerifyPasswordTextboxIsPresent();

            drManager.EnterEmailAddressOnLogin(personalInfo.Email);
            drManager.EnterPasswordOnLogin(personalInfo.XAuthPassword);

            drManager.Continue();

            drManager.VerifyIsOnReportPage();
        }
    }
}
