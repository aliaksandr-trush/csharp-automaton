namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventsService;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineGetEventsServiceFixture : APIFixtureBase
    {
        private const string XmlFilterDataFormat = "<filters><ClientEventId>{0}</ClientEventId></filters>";
        private const string FilterOperator = "AND";
        private const string EventName = "WebService - RegOnlineGetEventsServiceFixture";
        private const string EventInternalCode = "APITest";
        private const double EventFee = 10;

        private int eventId;
        private getEventsSoapClient service;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineGetEventsServiceFixture()
        {
            RequiresBrowser = true;

            this.RemoteAddressUri = new Uri(
                BaseUri,
                ConfigReader.DefaultProvider.WebServiceConfiguration[ConfigReader.WebServiceEnum.GetEventsService].Url);

            this.service = new getEventsSoapClient(
                ConfigReader.DefaultProvider.WebServiceConfiguration[ConfigReader.WebServiceEnum.GetEventsService].EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("776")]
        [ExpectedException(typeof(Exception), ExpectedMessage="The credentials you supplied are not valid.")]
        public void ByAccountIDWithFilters_Throws_Exception_For_Invalid_UserData()
        {
            string response = this.service.ByAccountIDWithFilters(
                ManagerBase.InvalidId,
                "user",
                "xxx", 
                string.Format(XmlFilterDataFormat, "abcd"), 
                FilterOperator,
                false);

            if (response.Equals("The credentials you supplied are not valid."))
            {
                throw new Exception(response);
            }
            else
            {
                throw new Exception(string.Format("Not expected exception: {0}", response));
            }
        }

        [Test]
        [Category(Priority.Three)]
        [Description("775")]
        public void ByAccountIDWithFilters_No_Exception_For_Valid_Params()
        {
            this.PrepareEvent();

            string response = this.service.ByAccountIDWithFilters(
                ConfigReader.DefaultProvider.AccountConfiguration.Id, 
                ConfigReader.DefaultProvider.AccountConfiguration.Login, 
                ConfigReader.DefaultProvider.AccountConfiguration.Password,
                string.Format(XmlFilterDataFormat, EventInternalCode), 
                FilterOperator,
                false);
                
            Assert.That(response.Contains(string.Format("<ID>{0}</ID>", this.eventId.ToString())));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("773")]
        public void ByAccountID_No_Exception_For_Valid_Params()
        {
            ConfigReader.DefaultProvider.ReloadAccount(
                ConfigReader.AccountType.Alternative);

            this.PrepareEvent();

            // This service call returns a very large response (~4MB).  The app.config has been 
            // edited to account for this.
            string response = this.service.ByAccountID(
                ConfigReader.DefaultProvider.AccountConfiguration.Id, 
                ConfigReader.DefaultProvider.AccountConfiguration.Login, 
                ConfigReader.DefaultProvider.AccountConfiguration.Password);

            Assert.That(response.Contains(string.Format("<ID>{0}</ID>", this.eventId.ToString())));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("774")]
        public void ByAccountIDEventID_No_Exception_For_Valid_Params()
        {
            this.PrepareEvent();

            string response = this.service.ByAccountIDEventID(
                ConfigReader.DefaultProvider.AccountConfiguration.Id, 
                ConfigReader.DefaultProvider.AccountConfiguration.Login, 
                ConfigReader.DefaultProvider.AccountConfiguration.Password, 
                this.eventId);

            Assert.That(response.Contains(string.Format("<ID>{0}</ID>", this.eventId.ToString())));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("772")]
        public void All_Methods_Return_EventFeeAmount()
        {
            ConfigReader.DefaultProvider.ReloadAccount(
                ConfigReader.AccountType.Alternative);

            this.PrepareEvent();

            string response = this.service.ByAccountIDEventID(
                ConfigReader.DefaultProvider.AccountConfiguration.Id, 
                ConfigReader.DefaultProvider.AccountConfiguration.Login, 
                ConfigReader.DefaultProvider.AccountConfiguration.Password, 
                eventId);

            Assert.That(response.Contains(this.GetEventFeeAmountString()));

            response = this.service.ByAccountID(
                ConfigReader.DefaultProvider.AccountConfiguration.Id, 
                ConfigReader.DefaultProvider.AccountConfiguration.Login, 
                ConfigReader.DefaultProvider.AccountConfiguration.Password);

            Assert.That(response.Contains(this.GetEventFeeAmountString()));

            response = this.service.ByAccountIDWithFilters(
                ConfigReader.DefaultProvider.AccountConfiguration.Id, 
                ConfigReader.DefaultProvider.AccountConfiguration.Login, 
                ConfigReader.DefaultProvider.AccountConfiguration.Password,
                string.Format(XmlFilterDataFormat, EventInternalCode), 
                FilterOperator, 
                false);

            Assert.That(response.Contains(this.GetEventFeeAmountString()));
        }

        [Step]
        private void PrepareEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();

            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
                this.eventId = BuilderMgr.GetEventId();
                BuilderMgr.SetEventNameAndShortcut(EventName);
                BuilderMgr.SetEventFee(EventFee);
                BuilderMgr.ClickStartPageEventAdvancedSettings();
                BuilderMgr.AdvancedSettingsMgr.SetInternalCode(EventInternalCode);
                BuilderMgr.AdvancedSettingsMgr.ClickSaveAndClose();
                BuilderMgr.SaveAndClose();
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }
        }

        private string GetEventFeeAmountString()
        {
            return string.Format("<EventFeeAmount>{0}</EventFeeAmount>", EventFee.ToString("0.0000"));
        }
    }
}
