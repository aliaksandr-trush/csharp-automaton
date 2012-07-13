namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;

    [TestFixture]
    public class xAuthFixture_SetupBasic : ExternalAuthenticationFixtureBase
    {
        private const string EventName = "xAuthFixture_SetupBasic";
        private const string ExistingEventName = "xAuthFixture_SetupBasic_ExistingEvent";
        private int eventID;
        private string eventSessionId;

        [Test]
        public void SetupBasic()
        {
            string existingEventRegType = "existingRegType";

            //remove current customer's xAuth role first
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthRoleForCustomer();

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.DeleteEventByName(ExistingEventName);

            //create an existing event and add reg type
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            // Get sessionId
            this.eventSessionId = BuilderMgr.GetEventSessionId();
            // Get event id
            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(ExistingEventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(existingEventRegType);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            //create an event and add reg type 1
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenAndCloseXAuthWhatIsThis();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            BuilderMgr.RegTypeMgr.VerifyEnableXAuthTextIsEnableExternalAuthentication(false);
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            //verify wrong Service Endpoint URL
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.TypeServiceEndpointURL("Url_Doesn't_Start_With_HTTPS");
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyErrorMessages(FormData.TestAccountResult.RequiredEmailPasswordAndUrlStartWithHttps);

            //verify require email and password
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyErrorMessages(FormData.TestAccountResult.RequiredEmailPassword);

            //verify test success
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyErrorMessages(FormData.TestAccountResult.Success);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(5);
            BuilderMgr.RegTypeMgr.VerifyEnableXAuthTextIsEnableExternalAuthentication(true);
            BuilderMgr.RegTypeMgr.VerifyEnableXAuthIsChecked(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            //create reg type 2
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype2");
            BuilderMgr.RegTypeMgr.VerifyEnableXAuthIsChecked(false);
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyErrorMessages(FormData.TestAccountResult.Success);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            ManagerSiteMgr.OpenEventBuilderStartPage(eventID, eventSessionId);
            BuilderMgr.OpenRegType(existingEventRegType);
            BuilderMgr.RegTypeMgr.VerifyEnableXAuthIsChecked(false);
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyErrorMessages(FormData.TestAccountResult.Success);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            Managers.ManagerProvider.XAuthMgr.ApprovedXAuthRoleForCustomer(true);
        }
    }
}