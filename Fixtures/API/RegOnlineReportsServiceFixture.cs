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
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Utilities;

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
        private List<DateTime> regDates = new List<DateTime>();

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

            // Set endDate to be two days forward, so that registrants added today can be retrieved.
            string endDate = DateTime.Now.AddDays(2).ToShortDateString();

            string response = this.service.getNonCompressedReport(
                ConfigurationProvider.XmlConfig.AccountConfiguration.Login,
                ConfigurationProvider.XmlConfig.AccountConfiguration.Password, 
                Convert.ToInt32(ConfigurationProvider.XmlConfig.AccountConfiguration.Id),
                this.reportId,
                this.eventId, 
                startDate, 
                endDate,
                false);

            Assert.That(Regex.IsMatch(response, "<last_Name>.+</last_Name>"));
            string returnedDateString = Regex.Match(response, "<RegDate>[^<]+</RegDate>").Value.Split(new char[]{' '})[0].Split(new char[]{'>'})[1];
            VerifyTool.VerifyValue(this.regDates[0].ToString("M/d/yyyy"), returnedDateString, "RegDate: {0}");
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1343")]
        public void GetNonCompressedReport_CheckDateFormat_BritishEnglishCulture()
        {
            ConfigurationProvider.XmlConfig.ReloadAccount(XmlConfiguration.AccountType.ActiveEurope);

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ActiveEuropeEvent);
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.SelectEventType(Managers.Builder.FormDetailManager.ActiveEuropeEventType.Running);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SaveAndClose();

            this.PrepareCustomReport();
            this.regDates.Clear();

            for (int cnt = 0; cnt < TotalRegCount; cnt++)
            {
                RegisterMgr.OpenRegisterPage(this.eventId);
                RegisterMgr.Checkin();
                RegisterMgr.Continue();
                RegisterMgr.EnterProfileInfoEnduranceNew();
                RegisterMgr.Continue();
                RegisterMgr.ClickCheckoutActiveWaiver();
                RegisterMgr.FinishRegistration();
                RegisterMgr.ConfirmRegistration();
                this.regDates.Add(DateTimeTool.ConvertTo(DateTime.Now, DateTimeTool.TimeZoneIdentifier.GMTStandardTime));
            }

            string startDate = DateTime.Now.AddDays(-2).ToShortDateString();
            string endDate = DateTime.Now.AddDays(2).ToShortDateString();

            string response = this.service.getNonCompressedReport(
                ConfigurationProvider.XmlConfig.AccountConfiguration.Login,
                ConfigurationProvider.XmlConfig.AccountConfiguration.Password,
                Convert.ToInt32(ConfigurationProvider.XmlConfig.AccountConfiguration.Id),
                this.reportId,
                this.eventId,
                startDate,
                endDate,
                false);

            string returnedDateString = Regex.Match(response, "<RegDate>[^<]+</RegDate>").Value.Split(new char[] { ' ' })[0].Split(new char[] { '>' })[1];
            VerifyTool.VerifyValue(this.regDates[0].ToString("dd/MM/yyyy"), returnedDateString, "RegDate: {0}");
        }

        private void PrepareEventAndReportAndRegistrations()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder();

            ManagerSiteMgr.DeleteEventByName(EventName);

            this.CreateEvent();
            this.PrepareCustomReport();
            this.regDates.Clear();

            for (int cnt = 0; cnt < TotalRegCount; cnt++)
            {
                this.CreateRegistration();
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
