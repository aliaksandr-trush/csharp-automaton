namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineAPI;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineCheckinService;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineCheckinServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegOnlineCheckInServiceFixture";

        private int eventId;
        private int registrationId;
        private CheckInSoapClient service;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineCheckinServiceFixture()
        {
            RequiresBrowser = true;

            this.RemoteAddressUri = new Uri(
                BaseUri,
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebService.CheckinService].Url);

            this.service = new CheckInSoapClient(
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebService.CheckinService].EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("796")]
        public void CheckInRegistration_Returns_Success_For_Valid_RegId()
        {
            string serviceSuccessResponse = "Success!";
            
            this.PrepareEvent();
            this.CreateRegistration();

            string response = this.service.CheckInRegistration(this.registrationId);
            Assert.That(response.Equals(serviceSuccessResponse));

            // Check the reg's status == Attended
            RegOnlineAPIFixture apiFixture = new RegOnlineAPIFixture();
            ResultsOfListOfRegistration result = apiFixture.FetchRegistration(this.registrationId);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(this.registrationId, result.Data[0].ID);
            Assert.AreEqual(RegOnline.RegressionTest.Managers.Report.ReportManager.AttendeeStatus.Attended.ToString(), result.Data[0].StatusDescription);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("795")]
        public void CheckInRegistration_Returns_Failure_For_Invalid_RegId()
        {
            string serviceFailureResponse = "Failure: Either an invalid registration ID was passed, or a database problem occurred.";

            string response = this.service.CheckInRegistration(RegOnline.RegressionTest.Managers.ManagerBase.InvalidId);
            Assert.That(response.Equals(serviceFailureResponse));
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
                BuilderMgr.SaveAndClose();
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }
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
            this.registrationId = RegisterMgr.GetRegistrationIdOnConfirmationPage();
        }
    }
}
