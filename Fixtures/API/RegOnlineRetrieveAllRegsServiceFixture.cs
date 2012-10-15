namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using System.Text;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineRetrieveAllRegsService;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineRetrieveAllRegsServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegOnlineRetrieveAllRegsServiceFixture";

        private RetrieveAllRegistrationsManagerSoapClient service;
        private int registerId;
        private int eventId;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineRetrieveAllRegsServiceFixture()
            : base(ConfigReader.WebServiceEnum.RetrieveAllRegsService)
        {
            RequiresBrowser = true;

            this.service = new RetrieveAllRegistrationsManagerSoapClient(
                CurrentWebServiceConfig.EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("768")]
        public void RetrieveAll_Returns_Data_For_Valid_Params()
        {
            this.PrepareEvent();
            this.CreateRegistration();

            string response = this.service.RetrieveAllRegistrations(
                ConfigReader.DefaultProvider.AccountConfiguration.Login, 
                ConfigReader.DefaultProvider.AccountConfiguration.Password, 
                this.eventId);

            byte[] encoded = Convert.FromBase64String(response);
            byte[] decoded = Utility.ZipDecode(encoded);

            string decodedResponse = Encoding.Unicode.GetString(decoded);

            Assert.That(decodedResponse.Contains(string.Format("<registrationID>{0}</registrationID>", this.registerId)));
        }

        [Step]
        private void PrepareEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();

            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
                BuilderMgr.SetEventNameAndShortcut(EventName);
                BuilderMgr.SaveAndClose();
            }

            this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
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
            this.registerId = RegisterMgr.GetRegistrationIdOnConfirmationPage();
        }
    }
}
