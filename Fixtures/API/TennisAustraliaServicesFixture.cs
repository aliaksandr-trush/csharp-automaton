namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.EventService;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineAPI;
    using ES = RegOnline.RegressionTest.Fixtures.API.EventService;
    using RS = RegOnline.RegressionTest.Fixtures.API.RegistrationService;

    /// <summary>
    /// Regression test to verify that Tournament Planner-related web service 
    /// functionality is OK
    /// </summary>
    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class TennisAustraliaServicesFixture : APIFixtureBase
    {
        private const string User = "betatennisau";
        private const string Password = "b8tatennisau";

        private EventServiceSoapClient service;

        protected override Uri RemoteAddressUri { get; set; }

        public TennisAustraliaServicesFixture()
        {
            // Because the provided user and password only exist on beta, we have to run this test against beta
            ConfigurationProvider.XmlConfig.ReloadAccount(XmlConfiguration.AccountType.Default);

            // Re-initialize base uri after changing environment above
            BaseUriWithHttps = new Uri(ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrlWithHttps);

            this.RemoteAddressUri = new Uri(
                BaseUriWithHttps,
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebService.EventService].Url);

            this.service = new EventServiceSoapClient(
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebService.EventService].EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        /// <summary>
        /// sets credentials for BETA Tournament Planner account
        /// </summary>
        private ES.AuthenticationHeader GetEventServiceAuthHeader()
        {
            ES.AuthenticationHeader header = new ES.AuthenticationHeader();
            header.UserName = User;
            header.Password = Password;
            return header;
        }

        /// <summary>
        /// sets credentials for BETA Tournament Planner account
        /// </summary>
        private RS.AuthenticationHeader GetRegistrationServiceAuthHeader()
        {
            RS.AuthenticationHeader header = new RS.AuthenticationHeader();
            header.UserName = User;
            header.Password = Password;

            return header;
        }

        private RS.personalInfo setupTemplatePersonalInfo()
        {
            RS.personalInfo pi = new RS.personalInfo();
            pi.address1 = "address1";
            pi.address2 = "address2";
            pi.birthDate = DateTime.Now;
            pi.birthDateSpecified = true;
            pi.city = "boulder";
            pi.emailAddress = "bburch@regonline.com";
            pi.firstName = "brett";
            pi.gender = "M";
            pi.homePhone = string.Empty;
            pi.lastName = "burch";
            pi.memberId = "123456789";
            pi.middleName = string.Empty;
            pi.mobilePhone = string.Empty;
            pi.postalCode = "80301";
            pi.state = "CO";
            pi.workPhone = "303-577-5134";
            pi.country = "USA";

            RS.nameValueSet[] nvs = new RS.nameValueSet[2];
            nvs[0] = new RS.nameValueSet();
            nvs[0].name = "Single Level";
            nvs[0].value = "3.5";
            nvs[1] = new RS.nameValueSet();
            nvs[1].name = "Double Level";
            nvs[1].value = "3.0";
            pi.additionalDetails = nvs;

            return pi;
        }

        [Test]
        [Category(Priority.Three)]
        [Description("765")]
        public void TennisAustraliaWebservice()
        {
            int newEventId = -1;
            using (ES.EventServiceSoapClient eventService = new ES.EventServiceSoapClient())
            {
                //eventService.Url = "https://beta.regonline.com/webservices/Events/EventService.asmx";
                //eventService. .AuthenticationHeaderValue = GetEventServiceAuthHeader();
                int badEventId = 0;

                int sourceEventId = 625868;
                int desitinationCustomerId = 377427;
                DateTime now = DateTime.Now;
                string clientEventId = "TPTestClientEventID";
                string eventTitle = "The Visual Test Event";
                string city = "Amsterdam";
                string state = "NH";
                string country = "Netherlands";
                string postalCode = "1111AB";
                string redirectConfirmationURL = "http://www.tournamentsoftware.com/link/onlinepaymentreturn.aspx?id={rolid}";
                DateTime? startDate = now;
                DateTime? endDate = now.AddDays(5);
                string creditCardDescriptor = "Test Descriptor";

                ES.EventBasics eventBasics = new ES.EventBasics();
                eventBasics.ClientEventID = clientEventId;
                eventBasics.EventTitle = eventTitle;
                eventBasics.City = city;
                eventBasics.State = state;
                eventBasics.PostalCode = postalCode;
                eventBasics.Country = country;
                eventBasics.StartDate = startDate;
                eventBasics.EndDate = endDate;
                eventBasics.CreditCardDescriptor = creditCardDescriptor;
                eventBasics.RedirectConfirmationURL = redirectConfirmationURL;

                ES.RegOnlineResponseOfInt32 eventResponse = eventService.CreateEventFromTemplate(GetEventServiceAuthHeader(), sourceEventId, desitinationCustomerId, eventBasics);
                newEventId = eventResponse.Value;

                Assert.Greater(eventResponse.Value, badEventId);
                
                RegOnlineAPIFixture apiFixture = new RegOnlineAPIFixture();
                ResultsOfListOfEvent result = apiFixture.FetchEvent(User, Password, newEventId);
                Assert.IsTrue(result.Success);
                Assert.AreEqual(eventTitle, result.Data[0].Title);
                Assert.AreEqual(RegOnline.RegressionTest.Managers.Manager.Dashboard.DashboardManager.EventStatus.Active.ToString(), result.Data[0].Status);

                using (RegistrationService.RegistrationServiceSoapClient regService = new RegistrationService.RegistrationServiceSoapClient())
                {
                    //regService.Url = "https://beta.regonline.com/webservices/Registrations/RegistrationService.asmx";
                    //regService.AuthenticationHeaderValue = GetRegistrationServiceAuthHeader();

                    RS.RegOnlineResponseOfInitializeRegistrationSessionResponseType regResponse =
                        regService.InitializeRegistrationSession(GetRegistrationServiceAuthHeader(), newEventId, 10, setupTemplatePersonalInfo());

                    Assert.That(regResponse.Status.Success);
                    Assert.That(regResponse.Value.SessionId.Length > 0);
                    Assert.That(regResponse.Value.RegistrationId > 0);
                    Assert.That(regResponse.Value.URL.Length > 0);

                    var db = new Linq.ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
                    var regs = (from r in db.Registrations where r.Register_Id == regResponse.Value.RegistrationId select r).ToList();
                    Assert.That(regs[0].Test == false);
                }

                //now update the event
                eventBasics.City = "Sydney";
                eventBasics.ID = newEventId;
                ES.RegOnlineResponseOfBoolean updateResponse = eventService.UpdateEvent(GetEventServiceAuthHeader(), eventBasics);
                Assert.That(updateResponse.Value);
                Assert.That(updateResponse.Status.Success);
            }
        }
    }
}