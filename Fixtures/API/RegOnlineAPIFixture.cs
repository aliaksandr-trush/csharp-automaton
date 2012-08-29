namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.API.RegOnlineAPI;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineAPIFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegOnlineAPIFixture";
        private const string EventNameForIncompleteReg = "WebService - RegOnlineAPIFixture - For incomplete regs";
        private const string EventNameForPublishing = "WebService - RegOnlineAPIFixture - ForPublishing";
        private const string AccountNameForIncompleteReg = "internal";
        private const string PasswordForIncompleteReg = "abcd1234";
        private readonly DateTime EventStartDate = DateTime.Now.AddDays(40);
        private readonly DateTime EventEndDate = DateTime.Now.AddDays(70);
        private const int TotalRegCount = 3;
        private const double EventFee = 10;
        private const string CFName = "CFCheckbox";
        private const double AgendaFee = EventFee;
        private const string AgendaItemName = "AgendaCheckbox";
        private const string Currency = "USD";

        /*
        * pageSectionID
        This is the ID of the page you want the custom fields for. The available options for this parameter are:

        1 = Personal Information
        2 = Lodging Section
        3 = Travel Section
        4 = Preferences
        5 = Event Fee
        0 = Agenda Items
        */
        private enum PageSectionId
        {
            AgendaItems = 0,
            PersonalInformation,
            LodgingSection,
            TravelSection,
            Preferences,
            EventFee
        }

        private RegOnlineAPISoapClient service;
        private TokenHeader header;
        private APIRegistration reg;
        private APICustomFieldResponse[] cfResponses;
        private APICustomFieldResponse[] agendaResponses;
        private int eventId;
        private string eventSessionId;
        private int registrationId;
        private List<int> registrationIds = new List<int>();
        private int eventRegCount;
        private double totalRevenue;
        private int cfId;
        private int agendaItemId;
        private string registrationSessionId;
        private List<string> registrationSessionIds;
        private string registrantAPIToken = string.Empty;
        private string userAPIToken = string.Empty;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineAPIFixture()
        {
            RequiresBrowser = true;

            this.RemoteAddressUri = new Uri(
                BaseUriWithHttps,
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebServiceEnum.Default].Url);

            this.service = new RegOnlineAPISoapClient(
                ConfigurationProvider.XmlConfig.WebServiceConfiguration[XmlConfiguration.WebServiceEnum.Default].EndpointConfigName,
                RemoteAddressUri.ToString());

            header = new TokenHeader();
        }

        /*
         * API description for GetRegistrationsForEvent: http://developer.regonline.com/getregistrationsforevent/
         * 
         * Parameter "filter" sample:
         * (Valid filter expressions are basically C# syntax and can contain some .NET framework method calls) 
         *  1.IsSubstitute && FirstName.Contains("John")
         *  2.AddDate >= DateTime(2010, 1, 1)
         *  3.CancelDate.HasValue
         * 
         * Parameter "orderBy" sample:
         * (Valid expression syntax is similar to a SQL ORDER BY clause)
         *  1.AddBy, AddDate DESC
         *  2.FirstName ASC
         */

        [Test]
        [Category(Priority.Three)]
        [Description("830")]
        public void Login()
        {
            userAPIToken = this.LoginToGetAPIToken(
                ConfigurationProvider.XmlConfig.AccountConfiguration.Login, 
                ConfigurationProvider.XmlConfig.AccountConfiguration.Password);
            Assert.That(!string.IsNullOrEmpty(userAPIToken));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1328")]
        public void LoginRegistrant()
        {
            PrepareEventAndRegistrations();

            // Get most recent registrant's email address
            string email = string.Empty;
            var db = new DataAccess.ClientDataContext();
            
            var registrations = (from r in db.Registrations
                                 where r.Event_Id == this.eventId
                                 orderby r.Add_Date descending
                                 select r).ToList();

            email = registrations[0].Attendee.Email_Address;
            Assert.That(!string.IsNullOrEmpty(email));

            // Ensure the registrant can login to the API
            registrantAPIToken = this.LoginRegistrantToGetAPIToken(email);
            Assert.That(!string.IsNullOrEmpty(registrantAPIToken));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("831")]
        public void GetEvent()
        {
            RunTestToGetAPIToken();
            this.PrepareEventAndRegistrations();
            ResultsOfListOfEvent result = this.service.GetEvent(header, eventId);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(EventName, result.Data[0].Title);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("832")]
        public void GetEvents()
        {
            ConfigurationProvider.XmlConfig.ReloadAccount(
                XmlConfiguration.AccountType.Alternative);

            this.Login();
            header.APIToken = userAPIToken;

            ResultsOfListOfEvent result = this.service.GetEvents(header, "IsActive", "AddDate DESC");
            Assert.IsTrue(result.Success);

            // Assert that the first event in result list is activated
            Assert.IsTrue(result.Data[0].IsActive);
                
            // Assert that the AddDate of the first event is greater or equal than the second one's,
            // so that the result list is in descending order
            Assert.GreaterOrEqual(result.Data[0].AddDate, result.Data[1].AddDate);

            userAPIToken = null;
        }

        /*
         * The GetPublicEvents API method returns a list of events across all customers that meet the following criteria:
         *  1.The event is active
         *  2.The start date of the event is in the next 90 days
         *  3.The event was promoted to social outlets by the event organizer
         */
        [Test]
        [Category(Priority.Three)]
        [Description("833")]
        public void GetPublicEvents()
        {
            RunTestToGetAPIToken();

            this.PrepareEventForPublishing();
                
            ResultsOfListOfEvent result = this.service.GetPublicEvents(
                header,
                string.Format("CustomerID={0}", Convert.ToInt32(ConfigurationProvider.XmlConfig.AccountConfiguration.Id)), 
                "ID DESC");

            Assert.IsTrue(result.Success);

            Assert.IsTrue(result.Data.First(
                delegate(APIEvent evt)
                {
                    return evt.ID == this.eventId;
                }) != null);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("834")]
        public void GetEventStatistics()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();
            this.GetEventRegCount();

            ResultsOfListOfEvent result = this.service.GetEventStatistics(header, this.eventId);
            APIEvent evt = result.Data[0];
            Assert.IsTrue(result.Success);
            Assert.AreEqual(this.eventId, evt.ID);
            Assert.AreEqual(Convert.ToInt32(ConfigurationProvider.XmlConfig.AccountConfiguration.Id), evt.CustomerID);
            Assert.AreEqual(EventName, evt.Title);
            Assert.AreEqual(DashboardManager.EventStatus.Testing.ToString(), evt.Status);
            Assert.AreEqual(this.eventRegCount, evt.TotalRegistrations);

            this.totalRevenue = this.eventRegCount * (EventFee + AgendaFee);
            Assert.AreEqual((decimal)(this.totalRevenue), evt.TotalRevenue);
        }

        /*[Test]
        public void GetSurveysForEvent()
        {
            // login as a registrant
            RunTestToGetRegistrantAPIToken();
            var result = service.GetSurveysForEvent(header, null);
            Assert.IsTrue(result.Success);

            // login as a user
            RunTestToGetAPIToken();
            result = service.GetSurveysForEvent(header, eventId);
            Assert.IsTrue(result.Success);
            result = service.GetSurveysForEvent(header, null);
            Assert.IsFalse(result.Success);
        }*/

        [Test]
        [Category(Priority.Three)]
        [Description("835")]
        public void GetRegistration()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();
            this.CreateRegistration();

            ResultsOfListOfRegistration result = this.service.GetRegistration(this.header, registrationId);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(this.registrationId, result.Data[0].ID);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("836")]
        public void GetRegistrations()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();
            this.GetEventRegCount();

            DateTime filterDate = DateTime.Now.AddDays(-2);
            string filterYear = filterDate.Year.ToString();
            string filterMonth = filterDate.Month.ToString();
            string filterDay = filterDate.Day.ToString();

            ResultsOfListOfRegistration result = this.service.GetRegistrations(
                header,
                string.Format("AddDate >= DateTime({0}, {1}, {2}) && EventID == {3}", filterYear, filterMonth, filterDay, this.eventId), 
                "AddDate DESC");

            Assert.IsTrue(result.Success);
            Assert.AreEqual(this.eventRegCount, result.Data.Length);
            Assert.GreaterOrEqual(result.Data[0].AddDate, filterDate);
            Assert.GreaterOrEqual(result.Data[0].AddDate, result.Data[1].AddDate);
            Assert.AreEqual(this.eventId, result.Data[0].EventID);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("838")]
        public void GetRegistrationsForCustomField()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();
            this.GetEventRegCount();

            ResultsOfListOfRegistration result = this.service.GetRegistrationsForCustomField(
                header, 
                this.eventId, 
                this.cfId, 
                string.Empty, 
                string.Empty);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(this.eventRegCount, result.Data.Length);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("839")]
        public void GetRegistrationsForEvent()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();
            this.GetEventRegCount();

            ResultsOfListOfRegistration result = this.service.GetRegistrationsForEvent(header, this.eventId, string.Empty, string.Empty);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(this.eventRegCount, result.Data.Length);

            this.reg = result.Data[0];
        }

        [Test]
        [Category(Priority.Three)]
        [Description("840")]
        public void GetIncompleteRegistrations()
        {
            RunTestToGetAPIToken();

            this.registrationSessionIds = new List<string>();
            this.PrepareEventAndRegistrationsForIncompleteRegistrations();

            ResultsOfListOfRegistration result = this.service.GetIncompleteRegistrations(
                header, 
                string.Format("EventID={0}", this.eventId), 
                string.Empty);
                
            Assert.IsTrue(result.Success);
            Assert.AreEqual(TotalRegCount, result.Data.Length);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("841")]
        public void GetIncompleteRegistrationsForEvent()
        {
            RunTestToGetAPIToken();

            this.registrationSessionIds = new List<string>();
            this.PrepareEventAndRegistrationsForIncompleteRegistrations();

            ResultsOfListOfRegistration result = this.service.GetIncompleteRegistrationsForEvent(header, this.eventId, string.Empty);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(TotalRegCount, result.Data.Length);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("842")]
        public void UpdateRegistration()
        {
            RunTestToGetAPIToken();

            if (this.reg == null)
            {
                this.GetRegistrationsForEvent();
            }

            reg.FirstName = "ModifiedFirstName";
            reg.LastName = "ModifiedLastName";

            ResultsOfBoolean result = this.service.UpdateRegistration(header, reg);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data);

            ResultsOfListOfRegistration updatedReg = this.service.GetRegistration(header, reg.ID); 
            reg = updatedReg.Data[0];
            Utilities.VerifyTool.VerifyValue("ModifiedFirstName", reg.FirstName, "First Name: {0}");
            Utilities.VerifyTool.VerifyValue("ModifiedLastName", reg.LastName, "Last Name: {0}");
        }

        [Test]
        [Category(Priority.Three)]
        [Description("843")]
        public void CheckinRegistrationsForEvent()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();

            StringBuilder stringForRegIds = new StringBuilder();

            for (int cnt = 0; cnt < TotalRegCount; cnt++)
            {
                this.CreateRegistration();
                stringForRegIds.Append(this.registrationId.ToString());

                if (cnt < TotalRegCount - 1)
                {
                    stringForRegIds.Append(",");
                }
            }

            // RegistrationIDs are a comma delimited list of IDs you want to check-in.
            ResultsOfBoolean result = this.service.CheckinRegistrationsForEvent(header, stringForRegIds.ToString());
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("844")]
        public void CheckinRegistrationsForAgendaItem()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();

            StringBuilder stringForRegIds = new StringBuilder();

            for (int cnt = 0; cnt < TotalRegCount; cnt++)
            {
                this.CreateRegistration();
                stringForRegIds.Append(this.registrationId.ToString());

                if (cnt < TotalRegCount - 1)
                {
                    stringForRegIds.Append(",");
                }
            }

            ResultsOfBoolean result = this.service.CheckinRegistrationsForAgendaItem(
                header, 
                stringForRegIds.ToString(), 
                this.agendaItemId, 
                this.eventId);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("845")]
        public void GetCustomFieldResponses()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();

            ResultsOfListOfCustomFieldResponse result = this.service.GetCustomFieldResponses(
                header, 
                this.eventId, 
                this.cfId, 
                string.Empty);
                
            Assert.IsTrue(result.Success);
            APICustomFieldResponse response = result.Data[0];
            Assert.AreEqual(this.eventId, response.EventID);
            Assert.AreEqual(this.cfId, response.CFID);
            Assert.AreEqual(null, response.Amount);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("846")]
        public void GetCustomFieldResponsesForRegistration()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();
            this.CreateRegistration();

            ResultsOfListOfCustomFieldResponse result = this.service.GetCustomFieldResponsesForRegistration(
                header, 
                this.eventId, 
                this.registrationId,
                (int)(PageSectionId.PersonalInformation), 
                string.Empty);
                
            Assert.IsTrue(result.Success);
            APICustomFieldResponse response = result.Data[0];
            Assert.AreEqual(this.eventId, response.EventID);
            Assert.AreEqual(this.registrationId, response.RegistrationID);
            Assert.AreEqual(this.cfId, response.CFID);
            Assert.AreEqual(null, response.Amount);

            this.cfResponses = result.Data;
        }

        [Test]
        [Category(Priority.Three)]
        [Description("847")]
        public void GetCustomFields()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();

            ResultsOfListOfCustomField result = this.service.GetCustomFields(
                header, 
                this.eventId, 
                (int)(PageSectionId.PersonalInformation), 
                string.Empty);

            Assert.IsTrue(result.Success);

            // This event has only one custom field on personal info page
            Assert.AreEqual(1, result.Data.Length);

            APICustomField cf = result.Data[0];
            Assert.AreEqual(this.cfId, cf.ID);
            Assert.AreEqual(this.eventId, cf.EventID);
            Assert.AreEqual((int)(PageSectionId.PersonalInformation), cf.PageSectionID);
            Assert.AreEqual(CFName, cf.NameOnForm);
        }

        /*
         * Any property that is updated in ActiveEvents.Core.Registration.SaveCustomFieldResponses can be updated. 
         * These fields currently are:
        •	AttendeeStatus
        •	ListItemID
        •	Response
        •	Amount
        •	PricingScheduleType
        •	TaxRateTypeId
        •	Tax1
        •	Tax2
        •	Discount Code Amount
        •	Discount Code
        •	IsWaitlisted
        */
        [Test]
        [Category(Priority.Three)]
        [Description("848")]
        public void UpdateCustomFieldResponsesForRegistration()
        {
            RunTestToGetAPIToken();

            if (this.cfResponses == null)
            {
                this.GetCustomFieldResponsesForRegistration();
            }

            // There is only one custom field
            this.cfResponses[0].Response = "False";

            ResultsOfBoolean result = this.service.UpdateCustomFieldResponsesForRegistration(
                header, 
                this.eventId, 
                this.registrationId, 
                this.cfResponses);
                
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data);

            var db = new DataAccess.ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
                var response = from cfResponse in db.Custom_Field_Responses
                                where cfResponse.rRegisterId == this.registrationId
                                && cfResponse.EventId == this.eventId
                                && cfResponse.CFId == this.cfId
                                select cfResponse;

                List<DataAccess.Custom_Field_Response> responseList = response.ToList();
                Assert.AreEqual(1, responseList.Count);
                Assert.AreEqual("False", responseList[0].Response);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("849")]
        public void GetAgendaItems()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();

            ResultsOfListOfCustomField result = this.service.GetAgendaItems(header, this.eventId, string.Empty);
            Assert.IsTrue(result.Success);

            // Assert there is only one agenda item
            Assert.AreEqual(1, result.Data.Length);

            APICustomField agendaItem = result.Data[0];
            Assert.AreEqual(this.agendaItemId, agendaItem.ID);
            Assert.AreEqual(this.eventId, agendaItem.EventID);
            Assert.AreEqual((int)(PageSectionId.AgendaItems), agendaItem.PageSectionID);
            Assert.AreEqual(AgendaItemName, agendaItem.NameOnForm);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1329")]
        public void GetCustomFieldsForRegistration()
        {
            // login as a registrant
            RunTestToGetRegistrantAPIToken();

            var result = service.GetCustomFieldsForRegistration(header, null, null,
                (int)PageSectionId.Preferences);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(0, result.Data.Length);
            
            result = service.GetAgendaItemsForRegistration(header, null, null);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.Data.Length);

            // login as a user
            RunTestToGetAPIToken();

            // success
            result = service.GetAgendaItemsForRegistration(header, eventId, registrationId);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.Data.Length);

            // missing params
            result = service.GetAgendaItemsForRegistration(header, eventId, null);
            Assert.IsFalse(result.Success);
            result = service.GetAgendaItemsForRegistration(header, null, null);
            Assert.IsFalse(result.Success);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("850")]
        public void GetAgendaItemResponsesForRegistration()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();
            this.CreateRegistration();

            ResultsOfListOfCustomFieldResponse result = this.service.GetAgendaItemResponsesForRegistration(
                header, 
                this.eventId, 
                this.registrationId, 
                string.Empty);
                
            Assert.IsTrue(result.Success);
            APICustomFieldResponse response = result.Data[0];
            Assert.AreEqual(this.eventId, response.EventID);
            Assert.AreEqual(this.registrationId, response.RegistrationID);
            Assert.AreEqual(this.agendaItemId, response.CFID);
            Assert.AreEqual(AgendaFee, response.Amount);

            this.agendaResponses = result.Data;
        }

        [Test]
        [Category(Priority.Three)]
        [Description("851")]
        public void UpdateAgendaItemResponsesForRegistration()
        {
            RunTestToGetAPIToken();

            if (this.agendaResponses == null)
            {
                this.GetAgendaItemResponsesForRegistration();
            }

            agendaResponses[0].Response = "False";

            ResultsOfBoolean result = this.service.UpdateAgendaItemResponsesForRegistration(
                header, 
                this.eventId, 
                this.registrationId, 
                this.agendaResponses);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data);

            var db = new DataAccess.ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            var response = from agendaResponse in db.Custom_Field_Responses
                            where agendaResponse.rRegisterId == this.registrationId
                            && agendaResponse.EventId == this.eventId
                            && agendaResponse.CFId == this.agendaItemId
                            select agendaResponse;

            List<DataAccess.Custom_Field_Response> responseList = response.ToList();
            Assert.AreEqual(1, responseList.Count);
            Assert.AreEqual("False", responseList[0].Response);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("852")]
        public void GetTransaction()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();
            this.CreateRegistration();
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);
            int transactionId = Convert.ToInt32(BackendMgr.GetTransactions()[0].Id);

            ResultsOfListOfTransaction result = this.service.GetTransaction(header, transactionId);
            Assert.IsTrue(result.Success);
            APITransaction transaction = result.Data[0];
            Assert.AreEqual(transactionId, transaction.ID);
            Assert.AreEqual(this.eventId, transaction.EventID);
            Assert.AreEqual(this.registrationId, transaction.RegistrationID);
            Assert.AreEqual(EventFee + AgendaFee, transaction.Amount);
            Assert.AreEqual(Currency, transaction.EventCurrency);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("853")]
        public void GetTransactions()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();

            ResultsOfListOfTransaction result = this.service.GetTransactions(
                header, 
                string.Format("EventID={0}", this.eventId.ToString()), 
                "Date DESC");
                
            Assert.IsTrue(result.Success);
            APITransaction transaction = result.Data[0];
            Assert.AreEqual(this.eventId, transaction.EventID);
            Assert.AreEqual(EventFee + AgendaFee, transaction.Amount);
            Assert.AreEqual(Currency, transaction.EventCurrency);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("854")]
        public void GetTransactionsForEvent()
        {
            RunTestToGetAPIToken();

            this.PrepareEventAndRegistrations();

            ResultsOfListOfTransaction result = this.service.GetTransactionsForEvent(header, this.eventId, string.Empty);
            Assert.IsTrue(result.Success);
            APITransaction transaction = result.Data[0];
            Assert.AreEqual(this.eventId, transaction.EventID);
            Assert.AreEqual(EventFee + AgendaFee, transaction.Amount);
            Assert.AreEqual(Currency, transaction.EventCurrency);
        }

        /*[Test]
        public void GetPublicDirectories()
        {
            // log in as a registrant
            RunTestToGetRegistrantAPIToken();
            var result = service.GetPublicDirectories(header, null, null);
            Assert.IsTrue(result.Success);
            // TODO:  can build directories when building the event to check .Data

            // ensure additional params are ignored when authenticating via registrant API token
            result = service.GetPublicDirectories(header, 1234, 5678);
            Assert.IsTrue(result.Success);

            // login as a user
            RunTestToGetAPIToken();

            // missing event ID
            result = service.GetPublicDirectories(header, null, null);
            Assert.IsFalse(result.Success);

            // success
            result = service.GetPublicDirectories(header, eventId, null);
            Assert.IsTrue(result.Success);
            result = service.GetPublicDirectories(header, eventId, registrationId);
            Assert.IsTrue(result.Success);

            // invalid params
            result = service.GetPublicDirectories(header, 1234, 5678);
            Assert.IsFalse(result.Success);
        }*/

        [Test]
        [Category(Priority.Three)]
        [Description("1330")]
        public void GetAttendeeMobileAppSettings()
        {
            PrepareEventAndRegistrations();
            var result = service._GetAttendeeMobileAppSettings(eventId, null);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(eventId, result.Data.eventid);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1327")]
        public void CheckSecurity()
        {
            this.registrationIds = new List<int>();

            // Login as a registrant
            RunTestToGetRegistrantAPIToken();

            // Ensure appropriate methods require a user API token, instead
            // of the current registrant token
            // NOTE:  methods that allow registrant access are included below, as well
            Assert.IsFalse(service.CheckinRegistrationsForAgendaItem(
                header, registrationId.ToString(), agendaItemId, eventId).Success);
            Assert.IsFalse(service.CheckinRegistrationsForEvent(
                header, registrationId.ToString()).Success);
            Assert.IsTrue(service.GetAgendaItemResponsesForRegistration(
                header, null, null, string.Empty).Success);
            Assert.IsFalse(service.GetAgendaItems(
                header, eventId, string.Empty).Success);
            Assert.IsTrue(service.GetAgendaItemsForRegistration(
                header, null, null).Success);
            Assert.IsFalse(service.GetCustomFieldResponses(
                header, eventId, cfId, string.Empty).Success);
            Assert.IsTrue(service.GetCustomFieldResponsesForRegistration(
                header, null, null, (int)PageSectionId.AgendaItems,
                string.Empty).Success);
            Assert.IsFalse(service.GetCustomFields(
                header, eventId, (int)PageSectionId.AgendaItems, string.Empty).Success);
            Assert.IsTrue(service.GetCustomFieldsForRegistration(
                header, null, null, (int)PageSectionId.AgendaItems).Success);
            Assert.IsFalse(service.GetCustomReports(
                header, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetCustomReportsForEvent(
                header, eventId, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetEvent(
                header, eventId).Success);
            Assert.IsFalse(service.GetEventStatistics(
                header, eventId).Success);
            Assert.IsFalse(service.GetEvents(
                header, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetIncompleteRegistrations(
                header, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetIncompleteRegistrationsForEvent(
                header, eventId, string.Empty).Success);
            /*Assert.IsTrue(service.GetPublicDirectories(
                header, null, null).Success);*/
            Assert.IsFalse(service.GetPublicEvents(
                header, string.Empty, string.Empty).Success);

            Assert.IsTrue(service.GetRegistration(
                header, registrationId).Success);

            Assert.IsTrue(service.GetRegistration(
                header, null).Success);

            Assert.IsFalse(service.GetRegistrations(
                header, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetRegistrationsByEventID(
                header, eventId, string.Empty).Success);
            Assert.IsFalse(service.GetRegistrationsForCustomField(
                header, eventId, cfId, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetRegistrationsForEvent(
                header, eventId, string.Empty, string.Empty).Success);
            /*Assert.IsTrue(service.GetSurveysForEvent(
                header, null).Success);*/
            Assert.IsFalse(service.GetTransaction(
                header, 0).Success);
            Assert.IsFalse(service.GetTransactions(
                header, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetTransactionsForEvent(
                header, eventId, string.Empty).Success);
            Assert.IsFalse(service.UpdateAgendaItemResponsesForRegistration(
                header, eventId, registrationId, new APICustomFieldResponse[1]).Success);
            Assert.IsFalse(service.UpdateCustomFieldResponsesForRegistration(
                header, eventId, registrationId, new APICustomFieldResponse[1]).Success);
            Assert.IsFalse(service.UpdateRegistration(
                header, new APIRegistration()).Success);

            // ensure secure methods require an API token
            Assert.IsFalse(service.CheckinRegistrationsForAgendaItem(
                null, registrationId.ToString(), agendaItemId, eventId).Success);
            Assert.IsFalse(service.CheckinRegistrationsForEvent(
                null, registrationId.ToString()).Success);
            Assert.IsFalse(service.GetAgendaItemResponsesForRegistration(
                null, eventId, registrationId, string.Empty).Success);
            Assert.IsFalse(service.GetAgendaItems(
                null, eventId, string.Empty).Success);
            Assert.IsFalse(service.GetAgendaItemsForRegistration(
                null, eventId, registrationId).Success);
            Assert.IsFalse(service.GetCustomFieldResponses(
                null, eventId, cfId, string.Empty).Success);
            Assert.IsFalse(service.GetCustomFieldResponsesForRegistration(
                null, eventId, registrationId, (int)PageSectionId.AgendaItems,
                string.Empty).Success);
            Assert.IsFalse(service.GetCustomFields(
                null, eventId, (int)PageSectionId.AgendaItems, string.Empty).Success);
            Assert.IsFalse(service.GetCustomFieldsForRegistration(
                null, eventId, registrationId, (int)PageSectionId.AgendaItems).Success);
            Assert.IsFalse(service.GetCustomReports(
                null, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetCustomReportsForEvent(
                null, eventId, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetEvent(
                null, eventId).Success);
            Assert.IsFalse(service.GetEventStatistics(
                null, eventId).Success);
            Assert.IsFalse(service.GetEvents(
                null, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetIncompleteRegistrations(
                null, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetIncompleteRegistrationsForEvent(
                null, eventId, string.Empty).Success);
            /*Assert.IsFalse(service.GetPublicDirectories(
                null, eventId, registrationId).Success);*/
            Assert.IsFalse(service.GetPublicEvents(
                null, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetRegistration(
                null, registrationId).Success);
            Assert.IsFalse(service.GetRegistrations(
                null, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetRegistrationsByEventID(
                null, eventId, string.Empty).Success);
            Assert.IsFalse(service.GetRegistrationsForCustomField(
                null, eventId, cfId, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetRegistrationsForEvent(
                null, eventId, string.Empty, string.Empty).Success);
            /*Assert.IsFalse(service.GetSurveysForEvent(
                null, eventId).Success);*/
            Assert.IsFalse(service.GetTransaction(
                null, 0).Success);
            Assert.IsFalse(service.GetTransactions(
                null, string.Empty, string.Empty).Success);
            Assert.IsFalse(service.GetTransactionsForEvent(
                null, eventId, string.Empty).Success);
            Assert.IsFalse(service.UpdateAgendaItemResponsesForRegistration(
                null, eventId, registrationId, new APICustomFieldResponse[1]).Success);
            Assert.IsFalse(service.UpdateCustomFieldResponsesForRegistration(
                null, eventId, registrationId, new APICustomFieldResponse[1]).Success);
            Assert.IsFalse(service.UpdateRegistration(
                null, new APIRegistration()).Success);
        }

        [Step]
        private void RunTestToGetAPIToken()
        {
            // Login first to get APIToken if it is null or empty
            if (string.IsNullOrEmpty(userAPIToken))
            {
                this.Login();
            }
            header.APIToken = userAPIToken;
        }

        [Step]
        private void RunTestToGetRegistrantAPIToken()
        {
            if (string.IsNullOrEmpty(registrantAPIToken))
            {
                this.LoginRegistrant();
            }

            header.APIToken = registrantAPIToken;

            // get ID's
            ////Assert.IsTrue(C.API.Registration.ProcessDecryptedToken(U.EncryptionTools.Decrypt(header.APIToken), out eventId, out registrationId));
        }

        [Step]
        private string LoginToGetAPIToken(string userName, string password)
        {
            ResultsOfLoginResults result = this.service.Login(userName, password);
            Assert.IsTrue(result.Success);
            return result.Data.APIToken;
        }

        [Step]
        private string LoginRegistrantToGetAPIToken(string email)
        {
            ResultsOfListOfRegistration result = this.service.LoginRegistrant(
                eventId, 
                email,
                ConfigurationProvider.XmlConfig.AccountConfiguration.Password);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data.Length > 0);
            return result.Data[0].APIToken;
        }

        [Step]
        private void PrepareEventAndRegistrations()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder();
            
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.AddEvent();

                this.registrationIds.Clear();

                for (int cnt = 0; cnt < TotalRegCount; cnt++)
                {
                    this.CreateRegistration();
                    this.registrationIds.Add(this.registrationId);
                }

                registrantAPIToken = string.Empty;
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
                ManagerSiteMgr.OpenEventBuilderStartPage(this.eventId, this.eventSessionId);
                BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.PI);
                this.cfId = BuilderMgr.GetCustomFieldID(CFName);
                BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Agenda);
                this.agendaItemId = BuilderMgr.AGMgr.GetAgendaItemID(AgendaItemName);
                BuilderMgr.Close();
            }
        }

        [Step]
        private void PrepareEventAndRegistrationsForIncompleteRegistrations()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();

            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventNameForIncompleteReg);

            if (!ManagerSiteMgr.EventExists(EventNameForIncompleteReg))
            {
                this.AddEventForIncompleteRegistrations();

                for (int cnt = 0; cnt < TotalRegCount; cnt++)
                {
                    this.CreateIncompleteRegistration();
                }

                DataHelperTool.DeleteRegistrationSession(this.registrationSessionIds);
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventNameForIncompleteReg);
            }
        }

        [Step]
        private void PrepareEventForPublishing()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(EventNameForPublishing);

            // Create event
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventNameForPublishing);
            DateTime newStartDate = DateTime.Now.AddDays(60);
            DateTime newEndDate = DateTime.Now.AddDays(70);
            BuilderMgr.SetStartDate(newStartDate);
            BuilderMgr.SetEndDate(newEndDate);
            BuilderMgr.SaveAndClose();

            // Activate event
            ManagerSiteMgr.OpenEventDashboard(this.eventId);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();
            ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);

            // Return to event list, so that browser will not go to dashboard(after login) directly when running other tests next
            ManagerSiteMgr.DashboardMgr.ReturnToList();

            // Publish event
            ClientDataContext db = new ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            string command = "Exec SAV_EventPublicizeData " +
                                "{0}," +    //@eventId int,
                                "{1}," +    //@Title nvarchar(510),
                                "{2}," +    //@StartDate datetime,
                                "{3}," +    //@EndDate datetime = null,
                                "{4}," +    //@Description nvarchar(max),
                                "{5}," +    //@Keywords nvarchar(50),
                                "{6}," +    //@Price money = null,
                                "{7}," +    //@EventfulCategory1 nvarchar(20),
                                "{8}," +    //@EventfulCategory2 nvarchar(20),
                                "{9}," +    //@UpcomingCategory nvarchar(20),
                                "{10}," +   //@VenueName nvarchar(100),
                                "{11}," +   //@VenueDescription nvarchar(500),
                                "{12}," +   //@Country varchar(10),
                                "{13}," +   //@Address nvarchar(200),
                                "{14}," +   //@City nvarchar(200),
                                "{15}," +   //@State nvarchar(100),
                                "{16}," +   //@Zip varchar(20),
                                "{17}," +   //@VenueURL nvarchar(255),
                                "{18}," +   //@VenueImage nvarchar(50) = null,
                                "{19}," +   //@EventfulVenueId varchar(50),
                                "{20}," +   //@UpcomingVenueId varchar(50),
                                "{21}," +   //@MediaTypeID int = 0,
                                "{22}";     //@ChannelID int = 0

            object[] parameters = new object[] 
            {
                this.eventId,          //@eventId int,
                "ParkRunning",         //@Title nvarchar(510),
                newStartDate,        //@StartDate datetime,
                newEndDate,          //@EndDate datetime = null,
                "PublishTesting",      //@Description nvarchar(max),
                "Running",             //@Keywords nvarchar(50),
                EventFee,              //@Price money = null,
                "outdoors_recreation", //@EventfulCategory1 nvarchar(20),
                "clubs_associations",  //@EventfulCategory2 nvarchar(20),
                "8",                   //@UpcomingCategory nvarchar(20),
                "Scott Carpenter Park",//@VenueName nvarchar(100),
                "Scott Carpenter Park is located on the southwest corner of 30th St & Arapahoe. Onsite parking is first come first serve.",//@VenueDescription nvarchar(500),
                "US",                  //@Country varchar(10),
                "30th and Arapahoe",   //@Address nvarchar(200),
                FormDetailManager.StartPageDefaultInfo.City,//@City nvarchar(200),
                FormDetailManager.StartPageDefaultInfo.State,//@State nvarchar(100),
                FormDetailManager.StartPageDefaultInfo.ZipCode,//@Zip varchar(20),
                ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrl, //@VenueURL nvarchar(255),
                "",                  //@VenueImage nvarchar(50) = null,
                "V0-001-000268888-3",  //@EventfulVenueId varchar(50),
                "21662",               //@UpcomingVenueId varchar(50),
                0,                     //@MediaTypeID int = 0,
                0                      //@ChannelID int = 0
            };

            db.ExecuteCommand(command, parameters);
        }

        private void AddEvent()
        {
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.SetStartDate(EventStartDate);
            BuilderMgr.SetEndDate(EventEndDate);
            BuilderMgr.SetEventFee(EventFee);
            BuilderMgr.Next();
            BuilderMgr.AddPICustomField(Managers.Builder.CustomFieldManager.CustomFieldType.CheckBox, CFName);
            this.cfId = BuilderMgr.GetCustomFieldID(CFName);
            BuilderMgr.Next();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(Managers.Builder.AgendaItemManager.AgendaItemType.CheckBox, AgendaItemName, AgendaFee);
            this.agendaItemId = BuilderMgr.AGMgr.GetAgendaItemID(AgendaItemName);
            BuilderMgr.Next();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.LodgingStandardFieldsMgr.SetRoomType(true, true);
            BuilderMgr.LodgingStandardFieldsMgr.SetBedType(true, true);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetAirline(true, true, true, true);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetFlightNumber(true, true, true, true);
            BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Checkout);
            BuilderMgr.EnterEventCheckoutPage();
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SaveAndClose();
        }

        private void AddEventForIncompleteRegistrations()
        {
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventNameForIncompleteReg);
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
            RegisterMgr.SelectRoomPreference(Managers.Register.RegisterManager.RoomPreference.Single);
            RegisterMgr.SelectBedPreference(Managers.Register.RegisterManager.BedPreference.Queen);
            RegisterMgr.EnterArrivalAirline();
            RegisterMgr.EnterDepartureAirline();
            RegisterMgr.EnterArrivalFlightNumber();
            RegisterMgr.EnterDepartureFlightNumber();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            this.registrationId = RegisterMgr.GetRegistrationIdOnConfirmationPage();
        }

        private void CreateIncompleteRegistration()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            this.registrationSessionId = RegisterMgr.GetSessionId();
            this.registrationSessionIds.Add(this.registrationSessionId);
        }

        [Step]
        private void GetEventRegCount()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder();
            this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            this.eventRegCount = ManagerSiteMgr.GetEventRegCount(this.eventId);
        }

        public ResultsOfListOfRegistration FetchRegistration(string userName, string password, int registrationId)
        {
            TokenHeader header = new TokenHeader();
            header.APIToken = this.LoginToGetAPIToken(userName, password);
            return this.service.GetRegistration(header, registrationId);
        }

        public ResultsOfListOfRegistration FetchRegistration(int registrationId)
        {
            return this.FetchRegistration(ConfigurationProvider.XmlConfig.AccountConfiguration.Login, ConfigurationProvider.XmlConfig.AccountConfiguration.Password, registrationId);
        }

        public ResultsOfListOfEvent FetchEvent(string userName, string password, int eventId)
        {
            TokenHeader header = new TokenHeader();
            header.APIToken = this.LoginToGetAPIToken(userName, password);
            return this.service.GetEvent(header, eventId);
        }
    }
}