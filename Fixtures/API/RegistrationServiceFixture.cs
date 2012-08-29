namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegistrationService;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegistrationServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegistrationServiceFixture";
        private const string RegTypeName = "Runner";
        private const double RegTypeFee = 2;
        private const decimal Amount = (decimal)RegTypeFee;

        private int eventId = ManagerBase.InvalidId;
        private int regId = ManagerBase.InvalidId;
        private AuthenticationHeader header;
        private RegistrationServiceSoapClient service;

        protected override Uri RemoteAddressUri { get; set; }

        public RegistrationServiceFixture()
        {
            RequiresBrowser = true;

            this.RemoteAddressUri = new Uri(
                BaseUri,
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebServiceEnum.RegistrationService].Url);

            this.service = new RegistrationServiceSoapClient(
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebServiceEnum.RegistrationService].EndpointConfigName,
                RemoteAddressUri.ToString());

            header = new AuthenticationHeader();
            header.UserName = ConfigurationProvider.XmlConfig.AccountConfiguration.Login;
            header.Password = ConfigurationProvider.XmlConfig.AccountConfiguration.Password;
        }

        [Test]
        [Category(Priority.Three)]
        [Description("826")]
        public void CheckRegistrationStatus()
        {
            this.CreateEvent();
            this.CreateRegistration();

            RegOnlineResponseOfInt32 response = this.service.CheckRegistrationStatus(header, this.regId);
            Assert.IsTrue(response.Status.Success);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("827")]
        public void InitializeRegistrationSession()
        {
            this.CreateEvent();

            personalInfo pi = new personalInfo();
            pi.address1 = "Colorado";
            pi.address2 = "Boulder";
            pi.birthDate = DateTime.Now;
            pi.birthDateSpecified = true;
            pi.city = "boulder";
            pi.emailAddress = "this" + pi.birthDate.Value.Ticks + "@isatest.com";
            pi.firstName = "Tom";
            pi.gender = "M";
            pi.homePhone = string.Empty;
            pi.lastName = "McTester";
            pi.memberId = "123456789";
            pi.middleName = string.Empty;
            pi.mobilePhone = string.Empty;
            pi.postalCode = "99701";
            pi.state = "CO";
            pi.workPhone = "303-577-5134";
            pi.country = "USA";

            nameValueSet[] nvs = new nameValueSet[2];
            nvs[0] = new nameValueSet();
            nvs[0].name = RegTypeName;
            nvs[0].value = Convert.ToString(RegTypeFee);
            pi.additionalDetails = nvs;

            RegOnlineResponseOfInitializeRegistrationSessionResponseType response =
                this.service.InitializeRegistrationSession(header, eventId, Amount, pi);

            try
            {
                Assert.IsTrue(response.Status.Success);
                Assert.Greater(response.Value.RegistrationId, 0);
                Assert.IsNotNullOrEmpty(response.Value.SessionId);

                // Verify payment url is not null or empty
                Assert.IsNotNullOrEmpty(response.Value.URL);
            }
            catch
            {
                Assert.Fail("Error msg: " + response.Status.ErrorMessage);
            }
        }

        [Step]
        private void CreateEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
                BuilderMgr.SetEventNameAndShortcut(EventName);
                BuilderMgr.AddRegTypeWithEventFee(RegTypeName, RegTypeFee);
                BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Checkout);
                BuilderMgr.EnterEventCheckoutPage();
                this.eventId = BuilderMgr.GetEventId();
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
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(RegTypeName);
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            this.regId = RegisterMgr.GetRegistrationIdOnConfirmationPage();
        }
    }
}