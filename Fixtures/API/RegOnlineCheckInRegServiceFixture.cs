namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineAPI;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineCheckInRegService;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineCheckInRegServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegOnlineCheckInRegServiceFixture";

        private int eventId;
        private int registrationId;
        private CheckInRegSoapClient service;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineCheckInRegServiceFixture()
            : base(ConfigReader.WebServiceEnum.CheckInRegService)
        {
            RequiresBrowser = true;

            this.service = new CheckInRegSoapClient(
                CurrentWebServiceConfig.EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("794")]
        public void CheckIn_Returns_Success_For_Valid_RegId()
        {
            string serviceSuccessResponse = "Success!";

            this.PrepareEvent();
            this.CreateRegistration();

            string response = this.service.CheckIn(this.registrationId, this.eventId);

            Assert.That(response.Equals(serviceSuccessResponse));

            // Check the reg's status == Attended
            RegOnlineAPIFixture apiFixture = new RegOnlineAPIFixture();
            ResultsOfListOfRegistration result = apiFixture.FetchRegistration(this.registrationId);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(this.registrationId, result.Data[0].ID);
            Assert.AreEqual(RegOnline.RegressionTest.Managers.Report.ReportManager.AttendeeStatus.Attended.ToString(), result.Data[0].StatusDescription);
        }

        /// <summary>
        /// We should change the error message to be static.  We should not tell the customer, 
        /// "Failure: The ConnectionString property has not been initialized." -- instead
        /// something generic would probably be safer.
        /// </summary>
        [Test]
        [Category(Priority.Three)]
        [Description("793")]
        public void CheckIn_Returns_Failure_For_Invalid_RegId_EventId_Combo()
        {
            this.PrepareEvent();

            string response = this.service.CheckIn(
                RegOnline.RegressionTest.Managers.ManagerBase.InvalidId,
                RegOnline.RegressionTest.Managers.ManagerBase.InvalidId);

            Assert.That(response.StartsWith("Failure: "));
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
