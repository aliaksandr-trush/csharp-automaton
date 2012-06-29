﻿namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.LoginService;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class LoginServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - LoginServiceFixture";

        private int eventId;
        private LoginSoapClient service;

        protected override Uri RemoteAddressUri { get; set; }

        public LoginServiceFixture()
        {
            RequiresBrowser = true;

            this.RemoteAddressUri = new Uri(
                BaseUri,
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebService.LoginService].Url);

            this.service = new LoginSoapClient(
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebService.LoginService].EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("791")]
        public void GetCustomerIdIfAuthorized()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
                BuilderMgr.SetStartPage(Managers.Manager.ManagerSiteManager.EventType.ProEvent, EventName);
                this.eventId = BuilderMgr.GetEventId();
                BuilderMgr.SaveAndClose();
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }

            AuthenticationHeader header = new AuthenticationHeader();
            header.UserName = ConfigurationProvider.XmlConfig.AccountConfiguration.Login;
            header.Password = ConfigurationProvider.XmlConfig.AccountConfiguration.Password;

            RegOnlineResponseOfInt32 customerId = this.service.GetCustomerIdIfAuthorized(header, this.eventId);

            Assert.AreEqual(Convert.ToInt32(ConfigurationProvider.XmlConfig.AccountConfiguration.Id), customerId.Value);
            Assert.IsTrue(customerId.Status.Success);
        }
    }
}
