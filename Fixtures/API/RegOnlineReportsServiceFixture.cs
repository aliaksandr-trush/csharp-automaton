namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using System.Text.RegularExpressions;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineReportsService;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineReportsServiceFixture : APIFixtureBase
    {
        private const string ReportErrorMsg = "Error 4458: unable to process request.";
        private const string EventName = "WebService - RegOnlineReportsServiceFixture";
        private const string CustomReportName = "AllReg";
        private const int TotalRegCount = 2;

        private RegonlineWebServicesSoapClient service;
        private string eventSessionId;
        private int eventId;
        private int reportId;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineReportsServiceFixture()
        {
            RequiresBrowser = true;

            this.RemoteAddressUri = new Uri(
                BaseUri,
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebService.ReportsService].Url);

            this.service = new RegonlineWebServicesSoapClient(
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebService.ReportsService].EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("803")]
        public void GetReport_No_Exception_FriendlyMessage_For_Invalid_Params()
        {
            string response = this.service.getReport("user", "pass", 111111, 0, 0, string.Empty, string.Empty, false);
            Assert.AreEqual(ReportErrorMsg, response);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("802")]
        public void GetReport_No_Exception_For_Valid_Params()
        {
            this.PrepareEventAndReportAndRegistrations();

            string startDate = DateTime.Now.AddYears(-1).ToShortDateString();
            string endDate = DateTime.Now.ToShortDateString();

            string response = this.service.getReport(
                ConfigurationProvider.XmlConfig.AccountConfiguration.Login,
                ConfigurationProvider.XmlConfig.AccountConfiguration.Password,
                Convert.ToInt32(ConfigurationProvider.XmlConfig.AccountConfiguration.Id), 
                this.reportId,
                this.eventId, 
                startDate, 
                endDate,
                false);
                
            Assert.AreNotEqual(ReportErrorMsg, response);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("801")]
        public void GetNonCompressedReport_Returns_Noncompressed_Data()
        {
            this.PrepareEventAndReportAndRegistrations();

            // Search for any regs in this report modified in the last 2 days
            string startDate = DateTime.Now.AddDays(-2).ToShortDateString();

            // Set endDate to be one day forward, so that registrants added today can be retrieved.
            string endDate = DateTime.Now.AddDays(1).ToShortDateString();

            string response = this.service.getNonCompressedReport(
                ConfigurationProvider.XmlConfig.AccountConfiguration.Login,
                ConfigurationProvider.XmlConfig.AccountConfiguration.Password, 
                Convert.ToInt32(ConfigurationProvider.XmlConfig.AccountConfiguration.Id),
                this.reportId,
                this.eventId, 
                startDate, 
                endDate,
                false);

            //Assert.Fail(response);
            Assert.That(Regex.IsMatch(response, "<last_Name>.+</last_Name>"));
        }

        private void PrepareEventAndReportAndRegistrations()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder();

            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateEvent();
                this.PrepareCustomReport();

                for (int cnt = 0; cnt < TotalRegCount; cnt++)
                {
                    this.CreateRegistration();
                }
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
                this.PrepareCustomReport();
            }
        }

        [Step]
        private void CreateEvent()
        {
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(EventName);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void CreateRegistration()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        [Step]
        private void PrepareCustomReport()
        {
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.Reports);

            if (!ManagerSiteMgr.DashboardMgr.IsCustomReportExist(CustomReportName))
            {
                ManagerSiteMgr.DashboardMgr.CreateCustomReport(CustomReportName);
            }

            this.reportId = ManagerSiteMgr.DashboardMgr.GetCustomReportId(CustomReportName);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ReturnToList();
        }
    }
}
