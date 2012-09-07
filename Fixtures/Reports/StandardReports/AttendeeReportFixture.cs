namespace RegOnline.RegressionTest.Fixtures.Reports.StandardReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Managers.Report;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AttendeeReportFixture : FixtureBase
    {
        private const string EventName = "AttendeeReportFixture";
        private const string ConfirmedEventName = "ConfirmedReportFixture";
        private const double EventFee = 10;
        private const int EventExpireTime = 60;
        private const int RegCount = 3;

        private enum RegType
        {
            Student,
            Vendor,
            Speaker,
            VIP
        }

        private int eventID;
        private string sessionID;
        private List<int> registrationIds;
        private Dictionary<RegType, double?> regTypeFees;
        private Dictionary<RegType, int> regTypeIDs;

        public AttendeeReportFixture()
        {
            this.regTypeFees = new Dictionary<RegType, double?>();
            this.regTypeFees.Add(RegType.Student, 10);
            this.regTypeFees.Add(RegType.Vendor, 50);
            this.regTypeFees.Add(RegType.Speaker, null);
            this.regTypeFees.Add(RegType.VIP, 500);

            this.registrationIds = new List<int>();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("287")]
        public void VerifyReportCount()
        {
            this.CreateEventAndRegistration();

            // pull up the attendee report
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.OpenAttendeeReportFromEventDashboard();

            // test row count
            ReportMgr.VerifyAttendeeReportRowCount(this.eventID);
            WebDriverUtility.DefaultProvider.CloseWindow();
            WebDriverUtility.DefaultProvider.SelectOriginalWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("698")]
        public void TestFilteringByRegType()
        {
            RegType regType = RegType.Speaker;

            this.CreateEventAndRegistration();

            // pull up the attendee report
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.OpenAttendeeReportFromEventDashboard();

            // pull up the filters popup
            ReportMgr.ClickReportsFilterButton();

            // set filter value
            ReportMgr.SelectFilterRegType(regType.ToString());

            // verify results
            ReportMgr.VerifyReportFilteredByRegType(this.regTypeIDs[regType]);
            WebDriverUtility.DefaultProvider.CloseWindow();
            WebDriverUtility.DefaultProvider.SelectOriginalWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("762")]
        public void TestFilteringByRegistrationStatus()
        {
            this.CreateEventAndRegistration();
            
            //C.Event evt = C.Event.Get(this.eventID);
            //E.TList<E.Registrations> regs = this.regService.GetByEventId(this.eventID);

            Event evt = new Event();
            List<Registration> regs = new List<Registration>();

            ClientDataContext db = new ClientDataContext();
            regs = (from r in db.Registrations where r.Event_Id == this.eventID orderby r.Register_Id descending select r).ToList();
            evt = (from e in db.Events where e.Id == this.eventID select e).Single();

            int FirstRegID = regs[0].Register_Id;

            // set the reg status to some known value
            ReportMgr.SetRegistrationStatus(FirstRegID, 2, evt);

            // pull up the attendee report
            this.LoginAndGetSessionID();

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);

            // pull up the attendee report
            ManagerSiteMgr.DashboardMgr.OpenAttendeeReportFromEventDashboard();

            ReportMgr.SelectAttendeeReportRecord(FirstRegID);

            // click the button
            ReportMgr.ClickChangeStatusButton();

            // select statuses dropdown values and click change
            ReportMgr.ChangeStatus(ReportManager.AttendeeStatus.Confirmed, ReportManager.AttendeeStatus.NoShow);

            // click OK on the check in confirmation
            ReportMgr.ClickOKOnChangeStatusConfirmationPopup();
            ReportMgr.ClickOKOnChangeStatusConfirmationPopup();

            ReportMgr.SelectReportPopupWindow();

            // pull up the filters popup
            ReportMgr.ClickReportsFilterButton();

            //set filter value
            ReportMgr.SetFilterRegStatus("No-show");

            //verify results
            ReportMgr.VerifyReportFilteredByRegStatus(this.eventID, 9);
            WebDriverUtility.DefaultProvider.CloseWindow();
            WebDriverUtility.DefaultProvider.SelectOriginalWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("700")]
        public void TestSmartLinkingWithPassword()
        {
            this.CreateEventAndRegistration();

            // pull up the attendee report
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.OpenAttendeeReportFromEventDashboard();
            
            // test smart link
            ReportMgr.ClickSmartLinkButton();
            string smartLinkReportUrl = ReportMgr.EnableSmartLinkAccess(true);
            ReportMgr.SelectReportPopupWindow();
            WebDriverUtility.DefaultProvider.OpenUrl(smartLinkReportUrl);
            ReportMgr.LoginToSmartLinkReport(ReportManager.DefaultSmartLinkPassword);
            ReportMgr.VerifyAttendeeReportRowCount(this.eventID);
            WebDriverUtility.DefaultProvider.CloseWindow();
            WebDriverUtility.DefaultProvider.SelectOriginalWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("293")]
        public void TestCheckInButton()
        {
            this.registrationIds.Clear();
            this.CreateEventAndRegistration();

            //E.TList<E.Registrations> regs = this.regService.GetByEventId(this.eventID);
            List<Registration> regs = new List<Registration>();

            ClientDataContext db = new ClientDataContext();
            regs = (from r in db.Registrations where r.Event_Id == this.eventID orderby r.Register_Id descending select r).ToList();

            List<int> regID = new List<int>();
            regID.Add(regs[0].Register_Id);
            regID.Add(regs[1].Register_Id);

            // make sure attendees are not checked in originally
            ReportMgr.UndoCheckIn(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection, regID[0]);
            ReportMgr.UndoCheckIn(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection, regID[1]);

            // pull up the attendee report
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.OpenAttendeeReportFromEventDashboard();

            // select attendees to be checked in
            ReportMgr.SelectAttendeeReportRecord(regID[0]);
            ReportMgr.SelectAttendeeReportRecord(regID[1]);

            // click the button
            ReportMgr.ClickCheckInButton();

            // click OK on the check in confirmation
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            ReportMgr.ClickOKOnCheckInConfirmationPopup();

            ReportMgr.OpenReportUrl(40, this.eventID);

            // make sure report is reloaded
            ReportMgr.WaitForReportToLoad();

            // verify attendees are checked in
            ReportMgr.VerifyRegistrationStatusOnAttendeeReport(
                regID[0], 
                ReportManager.AttendeeStatus.Attended);

            ReportMgr.VerifyRegistrationStatusOnAttendeeReport(
                regID[1],
                ReportManager.AttendeeStatus.Attended);

            WebDriverUtility.DefaultProvider.CloseWindow();
            WebDriverUtility.DefaultProvider.SelectOriginalWindow();

            // cleanup
            ReportMgr.UndoCheckIn(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection, regID[0]);
            ReportMgr.UndoCheckIn(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection, regID[1]);
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("296")]
        public void TestChangeStatusButton()
        {
            this.registrationIds.Clear();
            this.CreateEventAndRegistration();

            //C.Event evt = C.Event.Get(this.eventID);
            //E.TList<E.Registrations> regs = this.regService.GetByEventId(this.eventID);

            Event evt = new Event();
            List<Registration> regs = new List<Registration>();

            ClientDataContext db = new ClientDataContext();
            regs = (from r in db.Registrations where r.Event_Id == this.eventID orderby r.Register_Id descending select r).ToList();
            evt = (from e in db.Events where e.Id == this.eventID select e).Single();

            int FirstRegID = regs[0].Register_Id;
            int SecondRegID = regs[1].Register_Id;
            ReportMgr.SetRegistrationStatus(FirstRegID, 2, evt);
            ReportMgr.SetRegistrationStatus(SecondRegID, 2, evt);


            // set the reg status to some known value
            ReportMgr.SetRegistrationStatus(FirstRegID, 2, evt);

            // pull up the attendee report
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.OpenAttendeeReportFromEventDashboard();

            // select an attendee
            ReportMgr.SelectAttendeeReportRecord(FirstRegID);

            // click the button
            ReportMgr.ClickChangeStatusButton();

            // select statuses dropdown values and click change
            ReportMgr.ChangeStatus(ReportManager.AttendeeStatus.Confirmed, ReportManager.AttendeeStatus.Approved);
            
            // click OK on the check in confirmation
            ReportMgr.ClickOKOnChangeStatusConfirmationPopup();
            ReportMgr.ClickOKOnChangeStatusConfirmationPopup();

            ReportMgr.SelectReportPopupWindow();
            
            // verify attendees are checked in
            ReportMgr.VerifyRegistrationStatusOnAttendeeReport(FirstRegID, ReportManager.AttendeeStatus.Approved);

            // cleanup
            ReportMgr.SetRegistrationStatus(FirstRegID, 2, evt);
            WebDriverUtility.DefaultProvider.CloseWindow();
            WebDriverUtility.DefaultProvider.SelectOriginalWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("294")]
        public void TestSendEmailButton()
        {
            this.registrationIds.Clear();
            this.CreateEventAndRegistration();

            //E.TList<E.Registrations> regs = this.regService.GetByEventId(this.eventID);

            List<Registration> regs = new List<Registration>();

            ClientDataContext db = new ClientDataContext();
            regs = (from r in db.Registrations where r.Event_Id == this.eventID orderby r.Register_Id descending select r).ToList();

            // pull up the attendee report
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.OpenAttendeeReportFromEventDashboard();
             
            // select attendee(s)
            ReportMgr.SelectAttendeeReportRecord(regs[0].Register_Id);
            ReportMgr.SelectAttendeeReportRecord(regs[1].Register_Id);

            //click the button
            ReportMgr.ClickSendEmailButton();

            // send the email
            string emailSubject = "regression " + RegOnline.RegressionTest.Utilities.Utility.RandomString(15, false);
            ReportMgr.SendGroupEmail(emailSubject, "regression group email content, event id: " + this.eventID.ToString());
            ReportMgr.CloseReportPopupWindow();
            Utility.ThreadSleep(2);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();

            // verify results
            ReportMgr.VerifyGroupEmailJobWasCreated(this.eventID, emailSubject, regs[0].Attendee_Id);
            ReportMgr.VerifyGroupEmailJobWasCreated(this.eventID, emailSubject, regs[1].Attendee_Id);
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("699")]
        public void TestHeaderClickSorting()
        {
            this.registrationIds.Clear();
            this.CreateEventAndRegistration();

            //E.TList<E.Registrations> regs = this.regService.GetByEventId(this.eventID);

            List<Registration> regs = new List<Registration>();

            ClientDataContext db = new ClientDataContext();
            regs = (from r in db.Registrations where r.Event_Id == this.eventID orderby r.Register_Id descending select r).ToList();

            List<int> regIDs = new List<int>();

            foreach (Registration registration in regs)
            {
                regIDs.Add(registration.Register_Id);
            }
            
            int minRegID = regIDs[0];
            int maxRegID = regIDs[0];

            foreach (int regID in regIDs)
            {
                if(regID < minRegID)
                {
                    minRegID = regID;
                }

                if (regID > maxRegID)
                {
                    maxRegID = regID;
                }
            }

            // pull up the attendee report
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ////Report.CurrentEventID = this.eventID;
            ManagerSiteMgr.DashboardMgr.OpenAttendeeReportFromEventDashboard();

            // Verify ID column click/sorting
            // Click the ID column for the first time, and this will perform an ascending ordering
            ReportMgr.ClickAttendeeReportColumnHeader(1);
            ReportMgr.VerifyReportValue(1, 2, minRegID.ToString());

            // Click the ID column for the first time, and this will perform a descending order
            ReportMgr.ClickAttendeeReportColumnHeader(1);
            ReportMgr.VerifyReportValue(1, 2, maxRegID.ToString());
            WebDriverUtility.DefaultProvider.CloseWindow();
            WebDriverUtility.DefaultProvider.SelectOriginalWindow();
            this.BackToEventList();
        }


        //This might need to be pulled out into its own fixture, but for now 
        //I have it here as it uses so much from the other tests. And is only one
        //test. Once we get to the point where we have more report coverage we
        //should move it. It should be incredibly easy to move. 
        [Test]
        [Category(Priority.Two)]
        [Description("288")]
        public void TestConfirmedReport()
        {
            RegType regType1 = RegType.Student;
            RegType regType2 = RegType.Vendor;
            this.registrationIds.Clear();
            this.CreateConfirmedEventAndRegistration();

            List<Registration> regs = new List<Registration>();

            ClientDataContext db = new ClientDataContext();
            regs = (from r in db.Registrations where r.Event_Id == this.eventID orderby r.Register_Id descending select r).ToList();

            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.Reports);
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(ReportManager.CommonReportType.ConfirmedRegistrants);
            ReportMgr.VerifyReportTableRowCount(regs.Count);

            ReportMgr.SelectAttendeeReportRecord(regs[0].Register_Id);

            // click the button
            ReportMgr.ClickChangeStatusButton();

            // select statuses dropdown values and click change
            ReportMgr.ChangeStatus(ReportManager.AttendeeStatus.Confirmed, ReportManager.AttendeeStatus.Approved);
            ReportMgr.ClickOKOnChangeStatusConfirmationPopup();
            ReportMgr.ClickOKOnChangeStatusConfirmationPopup();

            ReportMgr.SelectReportPopupWindow();

            ReportMgr.VerifyReportTableRowCount(regs.Count - 1);
            BackendMgr.OpenAttendeeInfoURL(this.sessionID, regs[1].Register_Id);
            BackendMgr.CancelRegistrationAndVerify();

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.Reports);
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(ReportManager.CommonReportType.ConfirmedRegistrants);
            ReportMgr.VerifyReportTableRowCount(regs.Count - 2);

            ReportMgr.ClickReportsFilterButton();
            ReportMgr.SelectFilterRegType(regType1.ToString());
            ReportMgr.VerifyReportTableRowCount(this.GetRegCountByRegType(regs, regType1.ToString()));

            ReportMgr.ClickReportsFilterButton();
            ReportMgr.SelectFilterRegType(regType2.ToString());
            ReportMgr.VerifyReportTableRowCount(this.GetRegCountByRegType(regs, regType1.ToString()));

            ReportMgr.CloseReportPopupWindow();
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(ReportManager.CommonReportType.ConfirmedRegistrants);
            ReportMgr.VerifyReportTableRowCount(regs.Count - 2);

            ReportMgr.ClickSmartLinkButton();
            string smartLinkReportUrl = ReportMgr.EnableSmartLinkAccess(false);
            ReportMgr.SelectReportPopupWindow();
            WebDriverUtility.DefaultProvider.OpenUrl(smartLinkReportUrl);
            ReportMgr.VerifyReportTableRowCount(regs.Count - 2);
            WebDriverUtility.DefaultProvider.CloseWindow();
            WebDriverUtility.DefaultProvider.SelectOriginalWindow();
            this.BackToEventList();
        }

        private int GetRegCountByRegType(List<Registration> regs, string regTypeName)
        {
            int count = 0;

            foreach (Registration reg in regs)
            {
                if (reg.EventRegType.Description.Equals(regTypeName))
                {
                    count++;
                }
            }

            return count;
        }

        private void BackToEventList()
        {
            ManagerSiteMgr.SelectManagerWindow();
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ReturnToList();
        }

        #region Event creation
        [Step]
        private void LoginAndGetSessionID()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.sessionID = BuilderMgr.GetEventSessionId();
        }

        [Step]
        private void CreateEventAndRegistration()
        {
            this.LoginAndGetSessionID();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (ManagerSiteMgr.EventExists(EventName))
            {
                this.eventID = ManagerSiteMgr.GetFirstEventId(EventName);
                this.GetRegTypeID();
            }
            else
            {
                ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
                this.eventID = BuilderMgr.GetEventId();
                this.SetStartPage(EventName);
                BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
                this.SetCheckoutPage();
                BuilderMgr.SaveAndClose();
                this.Register();
            }
        }

        [Step]
        private void CreateConfirmedEventAndRegistration()
        {
            this.LoginAndGetSessionID();
            ManagerSiteMgr.DeleteEventByName(ConfirmedEventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            this.SetStartPage(ConfirmedEventName);
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            this.SetCheckoutPage();
            BuilderMgr.SaveAndClose();
            this.Register();
        }

        private void SetStartPage(string eventName)
        {
            this.regTypeIDs = new Dictionary<RegType, int>();
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(eventName);
            BuilderMgr.SetEventFee(EventFee);

            foreach (KeyValuePair<RegType, double?> fee in this.regTypeFees)
            {
                BuilderMgr.AddRegTypeWithEventFee(fee.Key.ToString(), fee.Value);
                this.regTypeIDs.Add(fee.Key, BuilderMgr.GetRegTypeIdFromHref(fee.Key.ToString()));
            }

            BuilderMgr.SaveAndStay();
        }

        private void SetCheckoutPage()
        {
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndStay();
        }

        private void GetRegTypeID()
        {
            List<EventRegType> regTypes = BuilderMgr.RegTypeMgr.Fetch_RegTypes(this.eventID);
            this.regTypeIDs = new Dictionary<RegType, int>();
            
            foreach (EventRegType regType in regTypes)
            {
                if (regType.Description == RegType.Speaker.ToString())
                {
                    this.regTypeIDs.Add(RegType.Speaker, regType.Id);
                }

                if (regType.Description == RegType.Student.ToString())
                {
                    this.regTypeIDs.Add(RegType.Student, regType.Id);
                }

                if (regType.Description == RegType.Vendor.ToString())
                {
                    this.regTypeIDs.Add(RegType.Vendor, regType.Id);
                }

                if (regType.Description == RegType.VIP.ToString())
                {
                    this.regTypeIDs.Add(RegType.VIP, regType.Id);
                }
            }
        }
        #endregion

        #region Registration
        private void Register()
        {
            RegisterMgr.CurrentEventId = this.eventID;

            foreach (KeyValuePair<RegType, double?> fee in this.regTypeFees)
            {
                for (int count = 0; count < RegCount; count++)
                {
                    this.RegisterWithRegType(fee.Key);
                }
            }
        }

        private void RegisterWithRegType(RegType regType)
        {
            RegisterMgr.OpenRegisterPage();
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(regType.ToString());
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            if (this.regTypeFees[regType] != null)
            {
                RegisterMgr.PayMoney(RegisterManager.PaymentMethod.Check);
            }

            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            this.registrationIds.Add(Convert.ToInt32(RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(
                RegisterManager.ConfirmationPageField.RegistrationId)));
        }
        #endregion
    }
}
