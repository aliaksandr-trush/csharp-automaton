namespace RegOnline.RegressionTest.Fixtures.Manager.EventDashboard.OnSite
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Managers.Report;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class OnSiteCheckinFixture : FixtureBase
    {
        private const string EventName = "OnSite Checkin";

        private string eventSessionId;
        private int eventId;
        private int regCount;

        private List<OnSiteFixtureHelper.RegistrationInfo> regs = new List<OnSiteFixtureHelper.RegistrationInfo>();
        private OnSiteFixtureHelper helper = new OnSiteFixtureHelper();

        [Test]
        [Category(Priority.One)]
        [Description("339")]
        public void OnSiteCheckIn()
        {
            this.eventSessionId = this.helper.Login();

            // If event does not exist then it builds the evet. 
            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateEvent();
                ManagerSiteMgr.GoToEventsTabIfNeeded();
            }

            this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            
            ManagerSiteMgr.OpenEventDashboard(this.eventId);
            DataHelperTool.ChangeAllRegsToTestForEvent(this.eventId);
            ManagerSiteMgr.DashboardMgr.DeleteTestRegs();

            // Sets event to Active if needed
            ManagerSiteMgr.DashboardMgr.ChangeStatusIfNecessary(DashboardManager.EventStatus.Active);

            // Creates registrations for the test
            this.CreateAllRegistrationsForOnSite();

            this.eventSessionId = this.helper.Login();
            ManagerSiteMgr.OpenEventDashboard(this.eventId);

            // Set event to On-Site if needed
            ManagerSiteMgr.DashboardMgr.ChangeStatusIfNecessary(DashboardManager.EventStatus.OnSite);

            // Gets total registrations for the event
            this.regCount = ManagerSiteMgr.DashboardMgr.GetTotalRegCountFromDashboard();

            // Open OnSite Checkin
            ManagerSiteMgr.DashboardMgr.OpenOnSiteCheckin();

            // Runs report with various sorting and ordering options
            this.RunReportWithSortsAndOrders();

            // Fliters report and checks in attendees
            this.RunReportsBasedOnNamesAndCheckIn();

            // Verify all attendees are checked in
            this.RunReportAndVerifyAllRegistrantsPresent(0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        [Step]
        public void CreateAllRegistrationsForOnSite()
        {
            CreateRegistrationsForOnSiteCheckIn("Aaron", "Aaronson", "Dulche");
            CreateRegistrationsForOnSiteCheckIn("Alice", "Aliceson", "Delta");
            CreateRegistrationsForOnSiteCheckIn("Barry", "Barryson", "Clairol");
            CreateRegistrationsForOnSiteCheckIn("Betty", "Bettyson", "Cisco");
            CreateRegistrationsForOnSiteCheckIn("Charles", "Charleson", "Boutique");
            CreateRegistrationsForOnSiteCheckIn("Claire", "Claireson", "Boeing");
            CreateRegistrationsForOnSiteCheckIn("David", "Davidson", "Avon");
            CreateRegistrationsForOnSiteCheckIn("Donna", "Donnason", "ACME");
        }

        public void CreateRegistrationsForOnSiteCheckIn(string firstName, string lastName, string company)
        {
            OnSiteFixtureHelper.RegistrationInfo RegInfo = new OnSiteFixtureHelper.RegistrationInfo();

            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType("One");
            RegInfo.emailAddress = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            RegisterMgr.TypePersonalInfoFirstName(firstName);
            RegisterMgr.TypePersonalInfoMiddleName(RegisterManager.DefaultPersonalInfo.MiddleName);
            RegisterMgr.TypePersonalInfoLastName(lastName);
            RegisterMgr.TypePersonalInfoJobTitle(RegisterManager.DefaultPersonalInfo.JobTitle);
            RegisterMgr.TypePersonalInfoCompany(company);
            RegisterMgr.TypePersonalInfoAddressLineOne(RegisterManager.DefaultPersonalInfo.AddressLineOne);
            RegisterMgr.TypePersonalInfoCity(RegisterManager.DefaultPersonalInfo.City);
            RegisterMgr.SelectPersonalInfoState(RegisterManager.DefaultPersonalInfo.State);
            RegisterMgr.TypePersonalInfoZipCode(RegisterManager.DefaultPersonalInfo.ZipCode);
            RegisterMgr.TypePersonalInfoWorkPhone(RegisterManager.DefaultPersonalInfo.WorkPhone);
            RegInfo.fullName = RegisterMgr.CurrentRegistrantFullName;
            RegisterMgr.EnterPersonalInfoPassword();
            RegisterMgr.Continue();
            this.helper.CheckoutPage(RegisterManager.PaymentMethod.CreditCard, 10.00);
            RegisterMgr.ConfirmRegistration();

            RegInfo.regId = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.RegistrationId));

            regs.Add(RegInfo);
        }
        
        [Step]
        public void RunReportWithSortsAndOrders()
        {
            RunReportAndVerifyAllRegistrantsPresent(8, 1, 2, 3, 4, 5, 6, 7, 8);
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.SortNames(OnSiteCheckinManager.OnSiteSortByOptions.LastName,
                OnSiteCheckinManager.OnSiteOrderByOptions.Descending);
            RunReportAndVerifyAllRegistrantsPresent(8, 8, 7, 6, 5, 4, 3, 2, 1);
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.SortNames(OnSiteCheckinManager.OnSiteSortByOptions.Company,
                OnSiteCheckinManager.OnSiteOrderByOptions.Ascending);
            RunReportAndVerifyAllRegistrantsPresent(8, 8, 7, 6, 5, 4, 3, 2, 1);
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.SortNames(OnSiteCheckinManager.OnSiteSortByOptions.Company,
                OnSiteCheckinManager.OnSiteOrderByOptions.Descending);
            RunReportAndVerifyAllRegistrantsPresent(8, 1, 2, 3, 4, 5, 6, 7, 8);
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.SortNames(OnSiteCheckinManager.OnSiteSortByOptions.ReferenceNum,
                OnSiteCheckinManager.OnSiteOrderByOptions.Descending);
            RunReportAndVerifyAllRegistrantsPresent(8, 8, 7, 6, 5, 4, 3, 2, 1);
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.SortNames(OnSiteCheckinManager.OnSiteSortByOptions.ReferenceNum,
                OnSiteCheckinManager.OnSiteOrderByOptions.Ascending);
            RunReportAndVerifyAllRegistrantsPresent(8, 1, 2, 3, 4, 5, 6, 7, 8);
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.SortNames(OnSiteCheckinManager.OnSiteSortByOptions.RegistrationDate,
                OnSiteCheckinManager.OnSiteOrderByOptions.Descending);
            RunReportAndVerifyAllRegistrantsPresent(8, 8, 7, 6, 5, 4, 3, 2, 1);
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.SortNames(OnSiteCheckinManager.OnSiteSortByOptions.RegistrationDate,
                OnSiteCheckinManager.OnSiteOrderByOptions.Ascending);
            RunReportAndVerifyAllRegistrantsPresent(8, 1, 2, 3, 4, 5, 6, 7, 8);
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.SelectWhichNamesYouWishToView(OnSiteCheckinManager.OnSiteNameFilter.CheckedIn);
            RunReportAndVerifyAllRegistrantsPresent(regCount - 8, 0, 0, 0, 0, 0, 0, 0, 0);
            ////ReportMgr.VerifyReportValue(1, 3, "Aaronson, Aaron");
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.SelectWhichNamesYouWishToView(OnSiteCheckinManager.OnSiteNameFilter.Both);
            RunReportAndVerifyAllRegistrantsPresent(regCount, 0, 0, 0, 0, 0, 0, 0, 0);
            ReportMgr.VerifyReportValue(1, 3, "Aaronson, Aaron");
            CloseReportAndSelectCheckInPage();
        }

        [Step]
        public void RunReportsBasedOnNamesAndCheckIn()
        {
            ReportMgr.OnSiteCheckinMgr.FilterReportByCharacter("A");
            RunReportAndVerifyAllRegistrantsPresent(2, 1, 2, 0, 0, 0, 0, 0, 0);
            ReportMgr.SelectAllAttendees();
            ReportMgr.ClickCheckInButton();
            UIUtilityProvider.UIHelper.GetConfirmation();
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.FilterReportByCharacter("B");
            ReportMgr.OnSiteCheckinMgr.SortNames(OnSiteCheckinManager.OnSiteSortByOptions.Company,
                OnSiteCheckinManager.OnSiteOrderByOptions.Ascending);
            RunReportAndVerifyAllRegistrantsPresent(2, 0, 0, 2, 1, 0, 0, 0, 0);
            ReportMgr.SelectAllAttendees();
            ReportMgr.ClickCheckInButton();
            UIUtilityProvider.UIHelper.GetConfirmation();
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.FilterReportByCharacter("C");
            ReportMgr.OnSiteCheckinMgr.SortNames(OnSiteCheckinManager.OnSiteSortByOptions.LastName,
                OnSiteCheckinManager.OnSiteOrderByOptions.Descending);
            RunReportAndVerifyAllRegistrantsPresent(2, 0, 0, 0, 0, 2, 1, 0, 0);
            ReportMgr.SelectAllAttendees();
            ReportMgr.ClickCheckInButton();
            UIUtilityProvider.UIHelper.GetConfirmation();
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            CloseReportAndSelectCheckInPage();

            ReportMgr.OnSiteCheckinMgr.SelectSearchByNameOrCompany(OnSiteCheckinManager.OnSiteSearchByNameOrCompany.Company);
            ReportMgr.OnSiteCheckinMgr.FilterReportByCharacter("A");
            ReportMgr.OnSiteCheckinMgr.SortNames(OnSiteCheckinManager.OnSiteSortByOptions.ReferenceNum,
                OnSiteCheckinManager.OnSiteOrderByOptions.Ascending);
            RunReportAndVerifyAllRegistrantsPresent(2, 0, 0, 0, 0, 0, 0, 1, 2);
            ReportMgr.SelectAllAttendees();
            ReportMgr.ClickCheckInButton();
            UIUtilityProvider.UIHelper.GetConfirmation();
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            CloseReportAndSelectCheckInPage();
        }

        /// <summary>
        /// Runs the report from the checkins.asp page and verifies the attendees position in the list based on an index
        /// in this case there are 8 total regs added that will be used, hence the 8 parameters. 
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="reg1Index"></param>
        /// <param name="reg2Index"></param>
        /// <param name="reg3Index"></param>
        /// <param name="reg4Index"></param>
        /// <param name="reg5Index"></param>
        /// <param name="reg6Index"></param>
        /// <param name="reg7Index"></param>
        /// <param name="reg8Index"></param>
        [Verify]
        public void RunReportAndVerifyAllRegistrantsPresent(int rowCount, int reg1Index, int reg2Index, int reg3Index,
            int reg4Index, int reg5Index, int reg6Index, int reg7Index, int reg8Index)
        {
            ReportMgr.OnSiteCheckinMgr.RunReport();
            SelectAttendeeReportPopUp();
            ReportMgr.VerifyReportTableRowCount(rowCount);

            if (reg1Index != 0)
            {
                ReportMgr.VerifyAttendeeRowIndex(regs[0].regId, reg1Index);
            }

            if (reg2Index != 0)
            {
                ReportMgr.VerifyAttendeeRowIndex(regs[1].regId, reg2Index);
            }

            if (reg3Index != 0)
            {
                ReportMgr.VerifyAttendeeRowIndex(regs[2].regId, reg3Index);
            }

            if (reg4Index != 0)
            {
                ReportMgr.VerifyAttendeeRowIndex(regs[3].regId, reg4Index);
            }

            if (reg5Index != 0)
            {
                ReportMgr.VerifyAttendeeRowIndex(regs[4].regId, reg5Index);
            }

            if (reg6Index != 0)
            {
                ReportMgr.VerifyAttendeeRowIndex(regs[5].regId, reg6Index);
            }

            if (reg7Index != 0)
            {
                ReportMgr.VerifyAttendeeRowIndex(regs[6].regId, reg7Index);
            }

            if (reg8Index != 0)
            {
                ReportMgr.VerifyAttendeeRowIndex(regs[7].regId, reg8Index);
            }
        }

        private void SelectAttendeeReportPopUp()
        {
            UIUtilityProvider.UIHelper.SelectWindowByName("attendee");
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        private void CloseReportAndSelectCheckInPage()
        {
            UIUtilityProvider.UIHelper.ClosePopUpWindow();
            UIUtilityProvider.UIHelper.SelectWindowByTitle("RegOnline");
        }

        [Step]
        public void CreateEvent()
        {
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.helper.SetupStartPage(EventName);
            BuilderMgr.Next();
            BuilderMgr.Next();
            BuilderMgr.Next();
            BuilderMgr.Next();
            BuilderMgr.Next();
            this.helper.SetupCheckoutPage();
            BuilderMgr.SaveAndClose();
            this.eventSessionId = this.helper.Login();
            eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            ManagerSiteMgr.OpenEventDashboard(eventId);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();
        }
    }
}
