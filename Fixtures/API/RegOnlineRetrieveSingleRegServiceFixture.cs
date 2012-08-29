namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using System.Text;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineRetrieveSingleRegService;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineRetrieveSingleRegServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegOnlineRetrieveSingleRegServiceFixture";
        private const string EventNameAlternative = "WebService - RegOnlineRetrieveSingleRegServiceFixture - Alternative";

        private RetrieveSingleRegistrationManagerSoapClient service;
        private int registerId;
        private int eventIdRegistrantBelongsTo;
        private int eventIdRegistrantDoesNotBelongTo;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineRetrieveSingleRegServiceFixture()
        {
            RequiresBrowser = true;

            this.RemoteAddressUri = new Uri(
                BaseUri,
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebServiceEnum.RetrieveSingleRegService].Url);

            this.service = new RetrieveSingleRegistrationManagerSoapClient(
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebServiceEnum.RetrieveSingleRegService].EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("767")]
        public void RetrieveSingleRegistration_Returns_Data_For_Valid_Params()
        {
            this.PrepareEvent();
            this.CreateRegistration();

            //The registrant belongs to specific event
            string response = this.service.RetrieveSingleRegistration(
                ConfigurationProvider.XmlConfig.AccountConfiguration.Login, 
                ConfigurationProvider.XmlConfig.AccountConfiguration.Password,
                this.eventIdRegistrantBelongsTo,
                this.registerId);

            //System.Console.WriteLine("original response: {0}", response);

            byte[] encoded = Convert.FromBase64String(response);
            byte[] decoded = Utility.ZipDecode(encoded);
            string decodedResponse = Encoding.Unicode.GetString(decoded);
            //System.Console.WriteLine("decoded: {0}", decodedResponse);

            this.VerifyContainsRegisterId(decodedResponse, true);
        }

        /// <summary>
        /// This test is currently failing due to a bug.  See TFS 23607.  The QA for that bug should just
        /// be running this test.
        /// </summary>
        [Test]
        [Category(Priority.Three)]
        [Description("769")]
        public void RetrieveSingleRegistration_Returns_Data_For_Invalid_Params()
        {
            this.PrepareEvent();
            this.CreateRegistration();

            // The registrant doesn't belong to specific event
            string response = this.service.RetrieveSingleRegistration(
                ConfigurationProvider.XmlConfig.AccountConfiguration.Login, 
                ConfigurationProvider.XmlConfig.AccountConfiguration.Password,
                this.eventIdRegistrantDoesNotBelongTo,
                this.registerId);

            byte[] encoded = Convert.FromBase64String(response);
            byte[] decoded = Utility.ZipDecode(encoded);
            string decodedResponse = Encoding.Unicode.GetString(decoded);
                
            this.VerifyContainsRegisterId(decodedResponse, false);
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
                this.AddEvent(EventName);
            }

            this.eventIdRegistrantBelongsTo = ManagerSiteMgr.GetFirstEventId(EventName);

            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventNameAlternative);

            if (!ManagerSiteMgr.EventExists(EventNameAlternative))
            {
                this.AddEvent(EventNameAlternative);
            }

            this.eventIdRegistrantDoesNotBelongTo = ManagerSiteMgr.GetFirstEventId(EventNameAlternative);
        }

        [Step]
        private void AddEvent(string eventName)
        {
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(eventName);
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void CreateRegistration()
        {
            RegisterMgr.OpenRegisterPage(this.eventIdRegistrantBelongsTo);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            this.registerId = RegisterMgr.GetRegistrationIdOnConfirmationPage();
        }

        [Verify]
        private void VerifyContainsRegisterId(string decodedResponse, bool contains)
        {
            bool actualResult = decodedResponse.Contains(string.Format("<registrationID>{0}</registrationID>", this.registerId));

            if (contains)
            {
                Assert.That(actualResult);
            }
            else
            {
                Assert.That(!actualResult);
            }
        }
    }
}
