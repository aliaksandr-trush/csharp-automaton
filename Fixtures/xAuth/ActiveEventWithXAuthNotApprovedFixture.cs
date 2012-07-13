namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    public class ActiveEventWithXAuthNotApprovedFixture : ExternalAuthenticationFixtureBase
    {
        private const string EventName = "XAuth_ActiveEventWithXAuthNotApproved";

        [Test]
        public void ActiveEventWithXAuthNotApproved()
        {
            Managers.ManagerProvider.XAuthMgr.ApprovedXAuthRoleForCustomer(false);

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder("xAuth");
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(EventName);
            int eventID = BuilderMgr.GetEventId();

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regType1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByUserName);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByUserName);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();

            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.VerifyUnableToActivateEventWhenXAuthNotApprovedMessageShown(true);

            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.TypeForgetPasswordUrl("http://beta.regonline.com");
            Managers.ManagerProvider.XAuthMgr.TypeMessageToRegistration("This is it.");
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();

            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.VerifyUnableToActivateEventWhenXAuthNotApprovedMessageShown(true);

            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            ManagerSiteMgr.OpenEventDashboard(eventID);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.RemoveTestRegs(true);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.SelectPricingOption(0);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.CheckAgreementForTermsOfService(true);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.TypeInitialing("YES");
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();

            ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.VerifyUnableToActivateEventMessageShown(true);

            Managers.ManagerProvider.XAuthMgr.ApprovedXAuthRoleForCustomer(true);
        }
    }
}
