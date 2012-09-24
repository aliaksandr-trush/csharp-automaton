namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventRegistrationsService;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineGetEventRegistrationsServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegOnlineGetEventRegistrationsServiceFixture";
        private const int TotalRegCount = 3;

        private int eventId;
        private List<int> registrationIds;
        private getEventRegistrationsSoapClient service;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineGetEventRegistrationsServiceFixture()
            : base(ConfigReader.WebServiceEnum.GetEventRegistrationsService)
        {
            RequiresBrowser = true;

            this.service = new getEventRegistrationsSoapClient(
                CurrentWebServiceConfig.EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("771")]
        [ExpectedException()]
        public void RetrieveRegistrationInfo_Throws_Exception_For_Invalid_Params()
        {
            string response = this.service.RetrieveRegistrationInfo(ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password, ManagerBase.InvalidId);
            Assert.Fail();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("770")]
        public void RetrieveRegistrationInfo_No_Exception_For_Valid_Params()
        {
            this.PrepareEventAndRegistration();

            string response = this.service.RetrieveRegistrationInfo(ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password, this.eventId);
                
            foreach (int registrationId in this.registrationIds)
            {
                Assert.IsTrue(response.Contains(registrationId.ToString()));
            }
        }

        [Step]
        private void PrepareEventAndRegistration()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();

            ManagerSiteMgr.DeleteEventByName(EventName);

            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.SaveAndClose();

            for (int cnt = 0; cnt < TotalRegCount; cnt++)
            {
                this.CreateRegistration();
            }
        }

        [Step]
        private void CreateRegistration()
        {
            this.registrationIds = new List<int>();
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            this.registrationIds.Add(RegisterMgr.GetRegistrationIdOnConfirmationPage());
        }
    }
}
