namespace RegOnline.RegressionTest.Fixtures.Warmup
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    [Category("Regression")]
    public class WarmupFixture : FixtureBase
    {
        private const string EventName = "AWarmUpFixture";

        private string eventSessionId;
        private int eventId;
        private int registrationId;

        [Test]
        public void WarmUp()
        {
            WebDriverUtility.DefaultProvider.SetTimeoutSpan(TimeSpan.FromMinutes(4));
            this.TouchManagerSite();
            this.TouchBuilderSite();
            this.TouchRegisterSite();
            this.TouchDashboardAndReportSite();
            this.TouchRegTransferPage();
            WebDriverUtility.DefaultProvider.SetTimeoutSpan();
        }

        [Step]
        private void TouchManagerSite()
        {
            ManagerSiteMgr.OpenLogin();
            this.eventSessionId = ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(EventName);           
        }

        [Step]
        private void TouchBuilderSite()
        {
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(Managers.Manager.ManagerSiteManager.EventType.ProEvent, EventName);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void TouchRegisterSite()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();

            this.registrationId = Convert.ToInt32(RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(
                Managers.Register.RegisterManager.ConfirmationPageField.RegistrationId));

            RegisterMgr.ConfirmRegistration();
        }

        [Step]
        private void TouchDashboardAndReportSite()
        {
            ManagerSiteMgr.OpenLogin();
            this.eventSessionId = ManagerSiteMgr.Login();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.Reports);
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(Managers.Report.ReportManager.CommonReportType.RegistrantList);
            ReportMgr.CloseReportPopupWindow();
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ReturnToList();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);
        }

        /// <summary>
        /// /reports/regtransfer.aspx is extremely slow after a deployment,
        /// which usually result in failure of TC743 - AttendeeInfoMenuOptions,
        /// touching it in warming up to avoid failure
        /// </summary>
        private void TouchRegTransferPage()
        {
            string url__Format_regTransferPage = "{0}reports/RegTransfer.aspx?EventSessionId={1}&EventId={2}&RegisterId=0";

            WebDriverUtility.DefaultProvider.OpenUrl(string.Format(
                url__Format_regTransferPage, 
                Configuration.ConfigReader.DefaultProvider.AccountConfiguration.BaseUrlWithHttps, 
                this.eventSessionId, 
                this.eventId));
        }
    }
}
