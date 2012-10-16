namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using System.Data;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineRegTrackerService;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineRegTrackerServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegOnlineRegTrackerServiceFixture";
        private const int TotalRegCount = 3;
        private const string SnapshotReportUrl = "/ActiveReports/ReportServer/EventSnapshot.aspx?rptType=110&EventSessionId=";
        private const string AttendeeReportUrl = "/activereports/reportserver/AttendeeReport.aspx?rptType=40&EventSessionId=";
        private const string DateTimeStringInUrl = "&StartDateTime=";

        private WSRegBugSoapClient service;
        private int eventId;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineRegTrackerServiceFixture()
            : base(ConfigReader.WebServiceEnum.RegTrackerService)
        {
            RequiresBrowser = true;

            this.service = new WSRegBugSoapClient(
                CurrentWebServiceConfig.EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        /*
         * need tests for all the other web methods in this service...
         */
        [Test]
        [Category(Priority.Three)]
        [Description("790")]
        public void Validate_Returns_True_For_Valid_Credentials()
        {
            Assert.That(this.service.Validate(ConfigReader.DefaultProvider.AccountConfiguration.Id, ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("785")]
        public void GetServerDateTime_Returns_DateTime()
        {
            Assert.IsNotNull(this.service.GetServerDateTime());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("788")]
        public void GetUserForms_Returns_DataSet()
        {
            DataSet ds = this.service.GetUserForms(Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            Assert.IsNotNull(ds);
            Assert.IsTrue(ds.Tables[0].Rows.Count > 0);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("789")]
        public void GetUserFormsStatistics_Returns_DataSet()
        {
            this.PrepareEventAndRegistrations();

            DataSet ds = this.service.GetUserFormsStatistics(Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), this.eventId.ToString(), DateTime.Now);
            Assert.IsNotNull(ds);
            Assert.IsTrue(ds.Tables[0].Rows.Count > 0);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("777")]
        public void CreateWebSession_Returns_UniqueId_String()
        {
            int uniqueIdLength = (this.service.CreateWebSession(Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password)).Length;
            Assert.AreNotEqual(String.Empty, this.service.CreateWebSession(Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password));
            Assert.IsTrue(uniqueIdLength > 0);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("784")]
        public void GetLoginUrl_Returns_Url_String_With_True_DirectAccess()
        {
            string url = "/LoginDirector.ashx?EventsessionID=";
            string return_url = this.service.GetLoginUrl(true, Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            Assert.IsTrue(return_url.Contains(url));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("783")]
        public void GetLoginUrl_Returns_Url_String_With_False_DirectAccess()
        {
            string url = "/manager/login.aspx";
            string return_url = this.service.GetLoginUrl(false, Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            Assert.IsTrue(return_url.Contains(url));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("787")]
        public void GetSnapshotReportUrl_Returns_Url_String_With_True_DirectAccess()
        {
            this.PrepareEventAndRegistrations();

            string return_url = this.service.GetSnapshotReportUrl(true, this.eventId, Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            Assert.IsTrue(return_url.Contains(SnapshotReportUrl));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("786")]
        public void GetSnapshotReportUrl_Returns_Url_String_With_False_DirectAccess()
        {
            this.PrepareEventAndRegistrations();

            string return_url = this.service.GetSnapshotReportUrl(false, this.eventId, Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            Assert.IsFalse(return_url.Contains(SnapshotReportUrl));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("779")]
        public void GetAttendeeReportUrl_Returns_Url_String_With_True_DirectAccess()
        {
            this.PrepareEventAndRegistrations();

            string return_url = this.service.GetAttendeeReportUrl(true, this.eventId, Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            Assert.IsTrue(return_url.Contains(AttendeeReportUrl));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("778")]
        public void GetAttendeeReportUrl_Returns_Url_String_With_False_DirectAccess()
        {
            this.PrepareEventAndRegistrations();

            string return_url = this.service.GetAttendeeReportUrl(false, this.eventId, Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            Assert.IsFalse(return_url.Contains(AttendeeReportUrl));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("781")]
        public void GetAttendeeReportUrlWithDate_Returns_Url_String_With_True_DirectAccess()
        {
            this.PrepareEventAndRegistrations();

            string return_url = this.service.GetAttendeeReportUrlWithDate(true, this.eventId, DateTime.Now, Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            Assert.IsTrue(return_url.Contains(DateTimeStringInUrl));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("780")]
        public void GetAttendeeReportUrlWithDate_Returns_Url_String_With_False_DirectAccess()
        {
            this.PrepareEventAndRegistrations();

            string return_url = this.service.GetAttendeeReportUrlWithDate(false, this.eventId, DateTime.Now, Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            Assert.IsFalse(return_url.Contains(DateTimeStringInUrl));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("782")]
        public void GetHelpUrl_Returns_Url_String()
        {
            string url = "http://forums.regonline.com/forums/thread/1277.aspx";
            Assert.AreEqual(url, this.service.GetHelpUrl());
        }

        [Step]
        private void PrepareEventAndRegistrations()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();

            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateEvent();

                for (int cnt = 0; cnt < TotalRegCount; cnt++)
                {
                    this.CreateRegistration();
                }
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
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
    }
}