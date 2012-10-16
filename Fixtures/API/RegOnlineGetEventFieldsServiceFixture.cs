namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineGetEventFieldsService;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineGetEventFieldsServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegOnlineGetEventFieldsServiceFixture";
        private const double AgendaItemPrice = 10;

        public enum AgendaItemName
        {
            WithPrice,
            WithoutPrice
        }

        private int eventId;
        private getEventFieldsSoapClient service;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineGetEventFieldsServiceFixture()
            : base(ConfigReader.WebServiceEnum.GetEventFieldsService)
        {
            RequiresBrowser = true;

            this.service = new getEventFieldsSoapClient(
                CurrentWebServiceConfig.EndpointConfigName,
                RemoteAddressUri.ToString());
        }
        
        [Test]
        [Category(Priority.Three)]
        [Description("798")]
        [ExpectedException()]
        public void RetrieveEventFields_Throws_Exception_For_Invalid_Data()
        {
            string response = this.service.RetrieveEventFields(ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password, ManagerBase.InvalidId);
            Assert.Fail();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("797")]
        public void RetrieveEventFields_Only_Returns_Items_Without_Amounts()
        {
            this.PrepareEvent();

            string response = this.service.RetrieveEventFields(ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password, this.eventId);
            Assert.That(response.IndexOf(AgendaItemName.WithPrice.ToString()) <= 0);
            Assert.Greater(response.IndexOf(AgendaItemName.WithoutPrice.ToString()), 1);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("800")]
        [ExpectedException()]
        public void RetrieveEventFields2_Throws_Exception_For_Invalid_Data()
        {
            string response = this.service.RetrieveEventFields2(ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password, ManagerBase.InvalidId, false);
            Assert.Fail();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("799")]
        public void RetrieveEventFields2_Returns_Items_With_Amounts()
        {
            this.PrepareEvent();

            string response = this.service.RetrieveEventFields2(ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password, eventId, false);
            Assert.That(response.IndexOf(AgendaItemName.WithPrice.ToString()) > 0);
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
                this.eventId = BuilderMgr.GetEventId();
                BuilderMgr.SetEventNameAndShortcut(EventName);
                BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Agenda);
                BuilderMgr.ClickYesOnSplashPage();
                BuilderMgr.AddAgendaItemWithPriceAndNoDate(Managers.Builder.AgendaItemManager.AgendaItemType.CheckBox, AgendaItemName.WithPrice.ToString(), AgendaItemPrice);
                BuilderMgr.AddAgendaItemWithPriceAndNoDate(Managers.Builder.AgendaItemManager.AgendaItemType.CheckBox, AgendaItemName.WithoutPrice.ToString(), null);
                BuilderMgr.SaveAndClose();
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }
        }
    }
}
