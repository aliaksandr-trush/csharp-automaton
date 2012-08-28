namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineRegUpdateService;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineRegUpdateServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegOnlineRegUpdateServiceFixture";
        private const string CFName = "CFCheckbox";
        private const string AgendaItemName = "AgendaCheckbox";
        private const double AgendaFee = 10;

        private RegistrationUpdateServiceSoapClient service;
        private int eventId;
        private int registerId;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineRegUpdateServiceFixture()
        {
            RequiresBrowser = true;

            this.RemoteAddressUri = new Uri(
                BaseUri,
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebServiceEnum.RegUpdateService].Url);

            this.service = new RegistrationUpdateServiceSoapClient(
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebServiceEnum.RegUpdateService].EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("861")]
        public void UpdateRegistrations()
        {
            this.PrepareEvent();
            this.CreateRegistration();

            updateRegistrationsResponse response = this.service.UpdateRegistrations(this.GetHeader(), this.GetRequest());
            Assert.IsTrue(response.updateRegistrationsResult.Contains(this.registerId.ToString()));
        }

        private updateRegistrationsRequestHeader GetHeader()
        {
            updateRegistrationsRequestHeader header = new updateRegistrationsRequestHeader();
            header.login = ConfigurationProvider.XmlConfig.AccountConfiguration.Login;
            header.password = ConfigurationProvider.XmlConfig.AccountConfiguration.Password;

            return header;
        }

        private updateRegistrationsRequest GetRequest()
        {
            registrationData registration = new registrationData();
            registration.registrationId = this.registerId;

            nameValueQuantitySet agendaItem = new nameValueQuantitySet() { fieldName = AgendaItemName, value = "True", quantity=1 };
            registration.agendaItems = new nameValueQuantitySet[] { agendaItem };

            updateRegistrationsRequest request = new updateRegistrationsRequest();
            request.eventID = this.eventId;
            request.registrations = new registrationData[] { registration };

            return request;
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
                this.AddEvent();
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }
        }

        [Step]
        private void AddEvent()
        {
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.PI);
            BuilderMgr.AddPICustomField(Managers.Builder.CustomFieldManager.CustomFieldType.CheckBox, CFName);
            BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Agenda);
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(Managers.Builder.AgendaItemManager.AgendaItemType.CheckBox, AgendaItemName, null);
            BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Checkout);
            BuilderMgr.EnterEventCheckoutPage();
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
            RegisterMgr.SetCustomFieldCheckBox(CFName, true);
            RegisterMgr.Continue();
            RegisterMgr.SetCustomFieldCheckBox(AgendaItemName, true);
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            this.registerId = RegisterMgr.GetRegistrationIdOnConfirmationPage();
        }
    }
}