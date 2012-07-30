namespace RegOnline.RegressionTest.Fixtures.Manager
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class SearchFixture : FixtureBase
    {
        private const string EventName_EventSearch = "SearchFixture_EventSearch";
        private const string EventName_AttendeeSearch = "SearchFixture_AttendeeSearch";
        private const double EventFee = 10;

        private string eventName_Guid = Guid.NewGuid().ToString();
        private string eventSessionId;
        private int eventId;
        private string[] firstName = { "Evelyn", "Evelyn1" };
        private string lastName = "Zou";

        private List<int> registrationIds = new List<int>();

        [Test]
        [Category(Priority.Three)]
        [Description("364")]
        public void EventSearch()
        {
            this.LoginAndGetSessionID();
            
            this.CreateEvent(eventName_Guid);
            ManagerSiteMgr.SearchMgr.SearchEvent(this.eventId.ToString());
            ManagerSiteMgr.DashboardMgr.VerifyOnDashboard(this.eventId);
            ManagerSiteMgr.DashboardMgr.ReturnToList();
            ManagerSiteMgr.DeleteEventByName(eventName_Guid);

            ManagerSiteMgr.DeleteEventByName(EventName_EventSearch);

            for (int cnt = 0; cnt < 2; cnt++)
            {
                this.CreateEvent(EventName_EventSearch);
            }

            ManagerSiteMgr.SearchMgr.SearchEvent(EventName_EventSearch);
            ManagerSiteMgr.SearchMgr.VerifyOnSearchPage(SearchManager.SearchMode.Event);
            ManagerSiteMgr.SearchMgr.ClickEventLinkOnSearchResultPage(this.eventId);
            ManagerSiteMgr.DashboardMgr.ReturnToList();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("363")]
        public void AttendeeSearch()
        {
            this.LoginAndGetSessionID();
            ManagerSiteMgr.DeleteEventByName(EventName_AttendeeSearch);
            this.CreateEvent(EventName_AttendeeSearch);

            for (int i = 0; i < 2; i++)
            {
                this.Register(firstName[i], this.eventId);
            }

            this.LoginAndGetSessionID();
            ManagerSiteMgr.SearchMgr.SearchAttendee(registrationIds[0].ToString());
            this.SelectAttendeeInfoWindowAndVerifyRegIdAndClose(registrationIds[0]);
            ManagerSiteMgr.SearchMgr.SearchAttendee(lastName);
            ManagerSiteMgr.SearchMgr.VerifyOnSearchPage(SearchManager.SearchMode.Attendee);
            ManagerSiteMgr.SearchMgr.ClickAttendeeLinkOnSearchResultPage(registrationIds[0]);
            this.SelectAttendeeInfoWindowAndVerifyRegIdAndClose(registrationIds[0]);
        }

        private void LoginAndGetSessionID()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.eventSessionId = BuilderMgr.GetEventSessionId();
        }

        private void Register(string firstname, int EventID)
        {
            RegisterMgr.OpenRegisterPage(EventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            this.EnterPIPage(firstname);
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            this.registrationIds.Add(RegisterMgr.GetRegistrationIdOnConfirmationPage());
        }

        private void EnterPIPage(string firstname)
        {
            RegisterMgr.TypePersonalInfoFirstName(firstname);
            RegisterMgr.TypePersonalInfoLastName(this.lastName);
            RegisterMgr.TypePersonalInfoJobTitle(RegisterManager.DefaultPersonalInfo.JobTitle);
            RegisterMgr.TypePersonalInfoCompany(RegisterManager.DefaultPersonalInfo.Company);
            RegisterMgr.TypePersonalInfoAddressLineOne(RegisterManager.DefaultPersonalInfo.AddressLineOne);
            RegisterMgr.TypePersonalInfoCity(RegisterManager.DefaultPersonalInfo.City);
            RegisterMgr.SelectPersonalInfoState(RegisterManager.DefaultPersonalInfo.State);
            RegisterMgr.TypePersonalInfoZipCode(RegisterManager.DefaultPersonalInfo.ZipCode);
            RegisterMgr.TypePersonalInfoWorkPhone(RegisterManager.DefaultPersonalInfo.WorkPhone);
            RegisterMgr.TypePersonalInfoPassword(Configuration.ConfigurationProvider.XmlConfig.AccountConfiguration.Password);
            RegisterMgr.TypePersonalInfoVerifyPassword(Configuration.ConfigurationProvider.XmlConfig.AccountConfiguration.Password);
        }

        private void SetStartPage(string eventName)
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(eventName);
            BuilderMgr.SaveAndStay();
        }

        private void CreateEvent(string EventName)
        {
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            this.SetStartPage(EventName);
            BuilderMgr.SaveAndClose();
        }

        private void SelectAttendeeInfoWindowAndVerifyRegIdAndClose(int regId)
        {
            BackendMgr.SelectAttendeeInfoWindow();
            BackendMgr.VerifyRegistrationId(regId);
            BackendMgr.CloseAttendeeInfo();
            ManagerSiteMgr.SelectManagerWindow();
            ManagerSiteMgr.SearchMgr.Return();
        }
    }
}