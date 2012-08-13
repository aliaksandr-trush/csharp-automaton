﻿namespace RegOnline.RegressionTest.Managers.Manager.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Utilities;
    using CommonReportType = Managers.Report.ReportManager.CommonReportType;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Managers.Manager.SelfRegistrationKiosk;
    using RegOnline.RegressionTest.Attributes;

    public class DashboardManager : ManagerBase
    {
        private const string DeleteTestRegButtonFormat =
            "//div[@id='ctl00_cphDialog_ctl00_cphDialog_rapDeleteTestRegistrationsPanel']/following-sibling::div/span/a/span[text()='{0}']";

        private const string EventWebsiteURLLinkLocatorFormat = "//span[text()='({0})']/a";
        private const string ManagerActivateButton = "ctl00_ctl00_cphDialog_cpMgrMain_btnStatusChange";
        private const string ChangeStatusLink = "ctl00_ctl00_cphDialog_cpMgrMain_lnkStatusChange";
        private const string ChangeStatusNow = "chkChangeStatusNow";
        private const string SetStatusNowLocator = "ddlStatusesCurrent";
        private const string AddStatusMessage = "elStatusMessage_linkCheckmarkfrmEventTextsStatusMessage";
        private const string OkButton = "btnOK";
        private const string CurrentEventStatus = "ctl00_ctl00_cphDialog_cpMgrMain_lblCurrentStatus";
        private const string CustomReportTableLocator = "ctl00_ctl00_cphDialog_cpMgrMain_grdCustomReports";
        private const string XEventReportTableLocator = "ctl00_ctl00_cphDialog_cpMgrMain_listXEventReports_grdXReports";

        private const string CustomReportURLFormat =
            "ActiveReports/ReportServer/CustomReport.aspx?eventSessionId={0}&EventId={1}&rptType=200&rptId={2}";

        private const string XEventReportURLFormat =
            "ActiveReports/ReportServer/CustomReport.aspx?EventSessionId={0}&rptType=30&rptId={1}";

        private const string ReportLinkOnFormDashboard = "ctl00_ctl00_cphDialog_cpMgrMain_lnkTotalRegs";
        private const string AddAttendeeDirectoryLocator = "ctl00_ctl00_cphDialog_cpMgrMain_aNewDirectory";
        private const string UrlPath = "manager/forms/details.aspx";
        private const string UrlPathFormat_SpecificEvent = "manager/forms/details.aspx?EventSessionId={0}&eventId={1}";

        #region Enums

        public enum DashboardTab
        {
            [StringValue("Event Details")]
            EventDetails,

            [StringValue("Reports")]
            Reports,

            [StringValue("Cross-Event Reports")]
            XEventReports,

            [StringValue("Labels and Badges")]
            LabelsBadges,

            [StringValue("Attendee Directories")]
            Directories
        }

        public enum EventSetupFunction
        {
            EditRegForm
        }

        public enum EventAdditionalFunction
        {
            CopyEvent,
            DeleteEvent,
            ButtonDesigner,
            ThirdPartyIntegration,
            DownloadRegTracker,
            ManageSeating,
            ApplicationIntegration,
            BibNumberingTool
        }

        public enum EventRegistrationFunction
        {
            TestRegister,
            OnsiteCheckIn,
            SelfRegistrationKiosk,
            AdminRegister,
            DeleteTestRegistrations
        }

        public enum ReportsFinancialReport
        {
            Transactions,
            CreditCardTransactions,
            TransactionFees
        }

        public enum EventStatus
        {
            [StringValue("Inactive")]
            Inactive,

            [StringValue("Sold Out")]
            SoldOut,

            [StringValue("Cancelled")]
            Cancelled,

            [StringValue("Testing")]
            Testing,

            [StringValue("On-site")]
            OnSite,

            [StringValue("Update Only")]
            UpdateOnly,

            [StringValue("Archived")]
            Archived,

            [StringValue("Active")]
            Active,
        }
        #endregion

        private struct CommonReportLinkLocator
        {
            public const string RegistrantList =
                "ctl00_ctl00_cphDialog_cpMgrMain_lnkAttendeeRoster";

            public const string AgendaSelections =
                "ctl00_ctl00_cphDialog_cpMgrMain_lnkAgendaReport";

            public const string Transaction =
                "ctl00_ctl00_cphDialog_cpMgrMain_lnkTransactionReport";

            public const string CreditCardTransaction = "ctl00_ctl00_cphDialog_cpMgrMain_lnkCCRecon";

            public const string TransactionFees = "ctl00_ctl00_cphDialog_cpMgrMain_lnkTransFees";

            public const string RoomingList =
                "ctl00_ctl00_cphDialog_cpMgrMain_lnkRoomingListReport";

            public const string EventSnapshot =
                "ctl00_ctl00_cphDialog_cpMgrMain_lnkEventSummary";

            public const string RegistrationWaitlist =
                "ctl00_ctl00_cphDialog_cpMgrMain_lnkRegistrationWaitlistReport";

            public const string Resource =
                "ctl00_ctl00_cphDialog_cpMgrMain_lnkSeatingReport";

            public const string Travel =
                "ctl00_ctl00_cphDialog_cpMgrMain_lnkTravelReport";

            public const string Lodging =
                "ctl00_ctl00_cphDialog_cpMgrMain_lnkLodgingReport";

            // Open attendee list first, then select group discount report from dropdown
            public const string GroupDiscount =
                "ctl00_ctl00_cphDialog_cpMgrMain_lnkAttendeeRoster";

            public const string ConfirmedRegistrants =
                "ctl00_ctl00_cphDialog_cpMgrMain_lnkConfirmedReport";
        }

        private struct CommonReportURLFormat
        {
            public const string RegistrantList =
                "activereports/Standard/Attendees.aspx?EventSessionId={0}&eventid={1}&rpttype=40";

            public const string AgendaSelections =
                "activereports/reportserver/agendareport.aspx?eventsessionid={0}&eventid={1}&rpttype=70";

            public const string Transaction =
                "activereports/reportserver/console.aspx?eventsessionid={0}&eventid={1}&rpttype=80";

            public const string CreditCardTransaction = "";

            public const string TransactionFees =
                "activereports/reportserver/TransactionFeesReport.aspx?EventSessionId={0}&eventID={1}&rptType=710&run=true";

            public const string RoomingList =
                "activereports/reportserver/ldgbkingrpt.aspx?eventsessionid={0}&eventid={1}&rpttype=90";

            public const string EventSnapshot =
                "activereports/reportserver/eventsnapshot.aspx?eventsessionid={0}&eventid={1}&rpttype=110";

            public const string RegistrationWaitlist =
                "activereports/reportserver/registrationwaitlist.aspx?eventsessionid={0}&eventid={1}&rpttype=800";

            public const string Resource =
                "activereports/reportserver/console.aspx?eventsessionid={0}&eventid={1}&rpttype=140";

            public const string Travel =
                "activereports/reportserver/console.aspx?eventsessionid={0}&eventid={1}&rpttype=150";

            public const string Lodging =
                "activereports/reportserver/console.aspx?eventsessionid={0}&eventid={1}&rpttype=160";

            public const string GroupDiscount =
                "activereports/reportserver/groupdiscountreport.aspx?eventsessionid={0}&eventid={1}&rpttype=705";

            public const string ConfirmedRegistrants =
                "activereports/Standard/Attendees.aspx?RegStatus=2&rptType=220&EventSessionId={0}&eventID={1}";
        }

        private struct CommonReportWindowNameFormat
        {
            public const string RegistrantList =
                "RolwinActiveReportsReportServerattendeeReportaspxrptType40EventSessionId{0}eventID{1}";

            public const string AgendaSelections =
                "RolwinActiveReportsReportServerAgendaReportaspxrptType70EventSessionId{0}eventID{1}";

            public const string Transaction =
                "RolwinActiveReportsReportServerconsoleaspxrptType80EventSessionId{0}eventID{1}";

            public const string RoomingList =
                "RolwinActiveReportsReportServerLdgBkingRptaspxrptType90EventSessionId{0}eventID{1}";

            public const string EventSnapshot =
                "RolwinActiveReportsReportServerEventSnapshotaspxEventSessionId{0}eventID{1}";

            public const string RegistrationWaitlist =
                "RolwinActiveReportsReportServerRegistrationWaitlistaspxrptType800EventSessionId{0}eventID{1}";

            public const string Resource =
                "RolwinActiveReportsReportServerconsoleaspxrptType140EventSessionId{0}eventID{1}";

            public const string Travel =
                "RolwinActiveReportsReportServerconsoleaspxrptType150EventSessionId{0}eventID{1}";

            public const string Lodging =
                "RolwinActiveReportsReportServerconsoleaspxrptType160EventSessionId{0}eventID{1}";

            public const string GroupDiscount =
                "RolwinActiveReportsReportServerattendeeReportaspxrptType40EventSessionId{0}eventID{1}";

            public const string ConfirmedRegistrants =
                "RolwinActiveReportsReportServerattendeeReportaspxRegStatus2rptType220EventSessionId{0}eventID{1}";
        }

        private Dictionary<CommonReportType, string> commonReportLinkLocators;
        private Dictionary<CommonReportType, string> commonReportURLFormats;

        public ButtonDesignerManager ButtonDesignerMgr
        {
            get;
            private set;
        }

        public CustomReportCreator CustomReportCreationMgr
        {
            get;
            private set;
        }

        public AttendeeDirectoryCreator AttendeeDirectoryCreationMgr
        {
            get;
            private set;
        }

        public ActivateEventManager ActivateEventMgr
        {
            get;
            private set;
        }

        private SelfRegistrationKioskManager _kioskMgr;
        public SelfRegistrationKioskManager KioskMgr
        {
            get { return _kioskMgr; }
            set { _kioskMgr = value; }
        }

        public DashboardManager()
        {
            this.ButtonDesignerMgr = new ButtonDesignerManager();
            this.CustomReportCreationMgr = new CustomReportCreator();
            this.ActivateEventMgr = new ActivateEventManager();
            this.AttendeeDirectoryCreationMgr = new AttendeeDirectoryCreator();
            this._kioskMgr = new SelfRegistrationKioskManager();
            this.InitializeCommonReportStuff();
        }

        private void InitializeCommonReportStuff()
        {
            this.commonReportLinkLocators = new Dictionary<CommonReportType, string>();
            this.commonReportLinkLocators.Add(CommonReportType.RegistrantList, CommonReportLinkLocator.RegistrantList);
            this.commonReportLinkLocators.Add(CommonReportType.AgendaSelections, CommonReportLinkLocator.AgendaSelections);
            this.commonReportLinkLocators.Add(CommonReportType.Transaction, CommonReportLinkLocator.Transaction);
            this.commonReportLinkLocators.Add(CommonReportType.CreditCardTransaction, CommonReportLinkLocator.CreditCardTransaction);
            this.commonReportLinkLocators.Add(CommonReportType.TransactionFees, CommonReportLinkLocator.TransactionFees);
            this.commonReportLinkLocators.Add(CommonReportType.RoomingList, CommonReportLinkLocator.RoomingList);
            this.commonReportLinkLocators.Add(CommonReportType.EventSnapshot, CommonReportLinkLocator.EventSnapshot);
            this.commonReportLinkLocators.Add(CommonReportType.RegistrationWaitlist, CommonReportLinkLocator.RegistrationWaitlist);
            this.commonReportLinkLocators.Add(CommonReportType.Resource, CommonReportLinkLocator.Resource);
            this.commonReportLinkLocators.Add(CommonReportType.Travel, CommonReportLinkLocator.Travel);
            this.commonReportLinkLocators.Add(CommonReportType.Lodging, CommonReportLinkLocator.Lodging);
            this.commonReportLinkLocators.Add(CommonReportType.GroupDiscount, CommonReportLinkLocator.GroupDiscount);
            this.commonReportLinkLocators.Add(CommonReportType.ConfirmedRegistrants, CommonReportLinkLocator.ConfirmedRegistrants);

            this.commonReportURLFormats = new Dictionary<CommonReportType, string>();
            this.commonReportURLFormats.Add(CommonReportType.RegistrantList, CommonReportURLFormat.RegistrantList);
            this.commonReportURLFormats.Add(CommonReportType.AgendaSelections, CommonReportURLFormat.AgendaSelections);
            this.commonReportURLFormats.Add(CommonReportType.Transaction, CommonReportURLFormat.Transaction);
            this.commonReportURLFormats.Add(CommonReportType.CreditCardTransaction, CommonReportURLFormat.CreditCardTransaction);
            this.commonReportURLFormats.Add(CommonReportType.TransactionFees, CommonReportURLFormat.TransactionFees);
            this.commonReportURLFormats.Add(CommonReportType.RoomingList, CommonReportURLFormat.RoomingList);
            this.commonReportURLFormats.Add(CommonReportType.EventSnapshot, CommonReportURLFormat.EventSnapshot);
            this.commonReportURLFormats.Add(CommonReportType.RegistrationWaitlist, CommonReportURLFormat.RegistrationWaitlist);
            this.commonReportURLFormats.Add(CommonReportType.Resource, CommonReportURLFormat.Resource);
            this.commonReportURLFormats.Add(CommonReportType.Travel, CommonReportURLFormat.Travel);
            this.commonReportURLFormats.Add(CommonReportType.Lodging, CommonReportURLFormat.Lodging);
            this.commonReportURLFormats.Add(CommonReportType.GroupDiscount, CommonReportURLFormat.GroupDiscount);
            this.commonReportURLFormats.Add(CommonReportType.ConfirmedRegistrants, CommonReportURLFormat.ConfirmedRegistrants);
        }

        [Step]
        public void ReturnToList()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_ctl00_cphDialog_cpMgrMain_hplBack", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        [Step]
        public void ChooseTab(DashboardTab tabChoice)
        {
            string tabLocatorFormat = StringEnum.GetStringValue(tabChoice);
            //string tabName = string.Empty;
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(tabLocatorFormat, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void ChooseTabAndVerify(DashboardTab tabChoice)
        {
            ChooseTab(tabChoice);
            VerifyTab(tabChoice);
        }

        public void VerifyTab(DashboardTab tabChoice)
        {
            bool isCorrect = false;

            switch (tabChoice)
            {
                case DashboardTab.EventDetails:
                    //Verify presence of setup, registration, and additional sections, as well as event statistics
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_ulSetupLinks", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_ulRegistrationLinks", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_ulAdditionalLinks", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_wrpEventStats", LocateBy.Id))
                        isCorrect = true;
                    break;
                case DashboardTab.Reports:
                    //Verify presence of sections for common, attendee, financial, miscellaneous, and custom reports
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlMainReports", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlAttendeeReports", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlFinancialReports", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlMiscReports", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlCustomReports", LocateBy.Id))
                        isCorrect = true;
                    break;
                case DashboardTab.XEventReports:
                    //Verify presence of standard and custom XEvent Report list
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_listXEventReports_pnlStandardXReports", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_listXEventReports_pnlCustomXEventReports", LocateBy.Id))
                        isCorrect = true;
                    break;
                case DashboardTab.LabelsBadges:
                    //Verify presence of labels and badges section
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlBadges", LocateBy.Id))
                        isCorrect = true;
                    break;
                case DashboardTab.Directories:
                    //Verify presence of user list
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlDirectories", LocateBy.Id))
                        isCorrect = true;
                    break;
            }

            VerifyTool.VerifyValue(true, isCorrect, "On dashboard tab " + tabChoice.ToString());
        }

        public void ClickOption(EventSetupFunction option)
        {
            string locator = string.Empty;

            VerifyTab(DashboardTab.EventDetails);

            switch (option)
            {
                case EventSetupFunction.EditRegForm:
                    locator = "ctl00_ctl00_cphDialog_cpMgrMain_lnkEdit";
                    break;
                default:
                    break;
            }

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void ClickOption(EventAdditionalFunction optionChoice)
        {
            string locator = string.Empty;

            VerifyTab(DashboardTab.EventDetails);

            switch (optionChoice)
            {
                case EventAdditionalFunction.CopyEvent:
                    locator = "ctl00_ctl00_cphDialog_cpMgrMain_lnkCopyEvent";
                    break;
                case EventAdditionalFunction.DeleteEvent:
                    locator = "ctl00_ctl00_cphDialog_cpMgrMain_lnkDeleteEvent";
                    break;
                case EventAdditionalFunction.ButtonDesigner:
                    locator = "ctl00_ctl00_cphDialog_cpMgrMain_lnkButtonDesigner";
                    break;
                case EventAdditionalFunction.ThirdPartyIntegration:
                    locator = "lnkIntegrations";
                    break;
                case EventAdditionalFunction.DownloadRegTracker:
                    locator = "lnkRegTracker";
                    break;
                case EventAdditionalFunction.BibNumberingTool:
                    locator = "ctl00_ctl00_cphDialog_cpMgrMain_lnkBibTool";
                    break;
                case EventAdditionalFunction.ManageSeating:
                    locator = "lnkSeating";
                    break;
                case EventAdditionalFunction.ApplicationIntegration:
                    locator = "ctl00_ctl00_cphDialog_cpMgrMain_lnkAppIntegration";
                    break;
            }

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        [Step]
        public void ClickOption(EventRegistrationFunction optionChoice)
        {
            string locator = string.Empty;

            VerifyTab(DashboardTab.EventDetails);

            switch (optionChoice)
            {
                case EventRegistrationFunction.TestRegister:
                    locator = "ctl00_ctl00_cphDialog_cpMgrMain_lnkRegister";
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.Id);
                    break;
                case EventRegistrationFunction.OnsiteCheckIn:
                    locator = "ctl00_ctl00_cphDialog_cpMgrMain_lnkOnsiteCheckin";
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.Id);
                    break;
                case EventRegistrationFunction.SelfRegistrationKiosk:
                    locator = "//a[@class='frmDashLink kiosk']";
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.XPath);
                    break;
                case EventRegistrationFunction.AdminRegister:
                    locator = "ctl00_ctl00_cphDialog_cpMgrMain_lnkAdminReg";
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.Id);
                    break;
                case EventRegistrationFunction.DeleteTestRegistrations:
                    locator = "ctl00_ctl00_cphDialog_cpMgrMain_lnkDeleteTestRegs";
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.Id);
                    break;
            }

            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void DeleteTestRegs()
        {
            this.ChooseTabAndVerify(DashboardManager.DashboardTab.EventDetails);
            this.ClickOption(DashboardManager.EventRegistrationFunction.DeleteTestRegistrations);
            this.DeleteTestReg_ClickDelete();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        public void DeleteTestReg_ClickDelete()
        {
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("plain");
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(DeleteTestRegButtonFormat, "Delete"), LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
        }

        public void DeleteTestReg_ClickCancel()
        {
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("plain");
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(DeleteTestRegButtonFormat, "Cancel"), LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
        }

        private int GetTestRegCountWhenTryingToDeleteThem()
        {
            int regCount;

            string confirmMessageLocator =
                "//div[@id='ctl00_cphDialog_rapDeleteTestRegistrations']/div[@id='ctl00_cphDialog_divMessage']";

            UIUtilityProvider.UIHelper.WaitForElementPresent(confirmMessageLocator, LocateBy.XPath);
            string confirmMssg = UIUtilityProvider.UIHelper.GetText(confirmMessageLocator, LocateBy.XPath);
            string numfound = Regex.Match(confirmMssg, @"\b\d+\b").Value;
            regCount = int.Parse(numfound);
            return regCount;
        }

        public void VerifyTestRegCountWhenTryingToDeleteThem(int regCount)
        {
            VerifyTool.VerifyValue(regCount, this.GetTestRegCountWhenTryingToDeleteThem(), "Test Registrations: {0}");
        }

        public string ComposeEventWebsiteURLLinkLocator(int eventID)
        {
            return string.Format(EventWebsiteURLLinkLocatorFormat, eventID);
        }

        public string GetEventWebsiteURL(int eventID)
        {
            string eventWebsiteURLLinkLocator = this.ComposeEventWebsiteURLLinkLocator(eventID);
            UIUtilityProvider.UIHelper.WaitForElementPresent(eventWebsiteURLLinkLocator, LocateBy.XPath);
            return UIUtilityProvider.UIHelper.GetText(eventWebsiteURLLinkLocator, LocateBy.XPath);
        }

        public void ClickEventWebsiteURLLink(int eventID)
        {
            string eventWebsiteURLLinkLocator = this.ComposeEventWebsiteURLLinkLocator(eventID);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(eventWebsiteURLLinkLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.SelectTopWindow();
        }

        [Step]
        public void ClickManagerActivateEventButton()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ManagerActivateButton, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void ActiveEvent()
        {
            this.ClickManagerActivateEventButton();
            this.SelectActivateEventFrame();
            this.ActivateEventMgr.ActivateEvent();
            this.ChooseTab(DashboardTab.EventDetails);
        }

        [Step]
        public void ChangeStatusIfNecessary(EventStatus status, string statusMessage = null)
        {
            

            string StatusToChangeTo = StringEnum.GetStringValue(status);

            if (this.GetEventStatusFromDashboard().Equals(StatusToChangeTo))
            {
                return;
            }
            
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ChangeStatusLink, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();

            // Select 'Change Form Status' rad window
            string locator_Name_ChangeFormStatusFrame = "plain";
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(locator_Name_ChangeFormStatusFrame);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ChangeStatusNow, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectWithText(SetStatusNowLocator, StatusToChangeTo, LocateBy.Id);

            if (!string.IsNullOrEmpty(statusMessage))
            {
                this.AddOrEditStatusMessage(statusMessage);
                UIUtilityProvider.UIHelper.SwitchToMainContent();
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName(locator_Name_ChangeFormStatusFrame);
            }

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(OkButton, LocateBy.Id);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        private void AddOrEditStatusMessage(string message)
        {
            RADContentEditorHelper helper = new RADContentEditorHelper();

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddStatusMessage, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            Utility.ThreadSleep(2);

            // Select 'Edit Status Message' rad window
            string locator_Name_EditStatusMessageFrame = "dialog2";
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(locator_Name_EditStatusMessageFrame);

            // Switch to HTML mode
            helper.SwitchMode(RADContentEditorHelper.Mode.HTML);
            UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(1);
            UIUtilityProvider.UIHelper.Type("//textarea", message, LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(locator_Name_EditStatusMessageFrame);
            UIUtilityProvider.UIHelper.SaveAndCloseContentEditorFrame();
        }

        [Step]
        private string GetEventStatusFromDashboard()
        {
            return UIUtilityProvider.UIHelper.GetText(CurrentEventStatus, LocateBy.Id);
        }

        public void VerifyEventStatus(EventStatus expectedStatus)
        {
            VerifyTool.VerifyValue(
                StringEnum.GetStringValue(expectedStatus), 
                this.GetEventStatusFromDashboard(), 
                "Event Status: {0}");
        }

        [Step]
        public bool IsCustomReportExist(string reportName)
        {
            return UIUtilityProvider.UIHelper.IsElementDisplay(this.ComposeCustomReportLinkLocator(reportName), LocateBy.XPath);
        }

        private string ComposeCustomReportLinkLocator(string reportName)
        {
            return string.Format(
                "//table[@id='{0}']//a[text()='{1}']",
                CustomReportTableLocator,
                reportName);
        }

        public void DeleteCustomReport(string reportName)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//a[text()='{0}']/../..//*[contains(@id,'Delete')]", reportName), LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='OK']/../..", LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            Utility.ThreadSleep(2);
        }

        [Step]
        public int GetCustomReportId(string reportName)
        {
            string href = UIUtilityProvider.UIHelper.GetAttribute(this.ComposeCustomReportLinkLocator(reportName), "href", LocateBy.XPath);

            string id = href.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
            id = id.Split(new char[] { '(' }, StringSplitOptions.RemoveEmptyEntries)[1];

            return Convert.ToInt32(id);
        }

        public int GetXEventReportId(string reportName)
        {
            string customReportLinkLocator = string.Format(
                "//table[@id='{0}']//a[text()='{1}']",
                XEventReportTableLocator,
                reportName);

            string href = UIUtilityProvider.UIHelper.GetAttribute(customReportLinkLocator, "href", LocateBy.XPath);

            string id = href.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
            id = id.Split(new char[] { '(' }, StringSplitOptions.RemoveEmptyEntries)[1];

            return Convert.ToInt32(id);
        }

        public void OpenOnSiteCheckin()
        {
            this.ClickOption(EventRegistrationFunction.OnsiteCheckIn);
            UIUtilityProvider.UIHelper.SelectWindowByTitle("RegOnline");
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void LaunchKiosk(
            bool enableBarcodeSearchByRegId,
            bool paidInFull,
            bool allowCC,
            string paidInFullMessage,
            bool requirePassword,
            bool allowOnSiteRegs,
            bool allowUpdates,
            bool allowGroupCheckIn,
            bool allowBadgePrinting)
        {
            this.ClickOption(DashboardManager.EventRegistrationFunction.SelfRegistrationKiosk);
            this.KioskMgr.SelectThisFrame();

            this.KioskMgr.ConfigureSelfRegistrationKioskOptions(
                enableBarcodeSearchByRegId,
                paidInFull,
                allowCC,
                paidInFullMessage,
                requirePassword,
                allowOnSiteRegs,
                allowUpdates,
                allowGroupCheckIn,
                allowBadgePrinting);

            this.KioskMgr.ClickLaunchSelfRegistrationKiosk();
        }

        [Step]
        public void OpenCommonReport(CommonReportType type)
        {
            switch (type)
            {
                case CommonReportType.RegistrantList:
                case CommonReportType.AgendaSelections:
                case CommonReportType.Transaction:
                case CommonReportType.CreditCardTransaction:
                case CommonReportType.TransactionFees:
                
                case CommonReportType.EventSnapshot:
                case CommonReportType.RegistrationWaitlist:
                case CommonReportType.Resource:
                case CommonReportType.Travel:
                case CommonReportType.Lodging:
                case CommonReportType.ConfirmedRegistrants:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(this.commonReportLinkLocators[type], LocateBy.Id);
                    SelectReportPopupWindow();
                    break;

                case CommonReportType.RoomingList:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(this.commonReportLinkLocators[type], LocateBy.Id);
                    this.ByPassRoomingListReportFilter();
                    SelectReportPopupWindow();
                    break;

                case CommonReportType.GroupDiscount:
                    throw new Exception("No direct link for GroupDiscount report!");

                default:
                    break;
            }

            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        private void ByPassRoomingListReportFilter()
        {
            // Select the FiltersConsole window
            UIUtilityProvider.UIHelper.SelectTopWindow();

            // Click OK
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnApply", LocateBy.Id);

            Utility.ThreadSleep(2);
        }

        [Step]
        public void OpenCustomReport(string reportName)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//table[@id='{0}']//a[text()='{1}']", CustomReportTableLocator, reportName), LocateBy.XPath);
            Utility.ThreadSleep(2);
            SelectReportPopupWindow();
        }

        public void OpenXEventReport(string reportName)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//table[@id='{0}']//a[text()='{1}']", XEventReportTableLocator, reportName), LocateBy.XPath);
            Utility.ThreadSleep(5);
            SelectReportPopupWindow();
        }

        public void OpenCommonReportURL(CommonReportType type, int eventId, string eventSessionId)
        {
            if (type == CommonReportType.CreditCardTransaction)
            {
                throw new NotImplementedException("CCTransactionsReport URL format not implemented!");
            }

            UIUtilityProvider.UIHelper.OpenUrl(ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrl + (string.Format(this.commonReportURLFormats[type], eventSessionId, eventId)));
        }

        public void OpenCustomReportURL(int eventId, string eventSessionId, int reportId)
        {
            UIUtilityProvider.UIHelper.OpenUrl(string.Format(CustomReportURLFormat, eventSessionId, eventId, reportId));
        }

        public void OpenXEventReportURL(string eventSessionId, int reportId)
        {
            UIUtilityProvider.UIHelper.OpenUrl(string.Format(XEventReportURLFormat, eventSessionId, reportId));
        }

        [Step]
        public void CreateCustomReport(string name)
        {
            this.CustomReportCreationMgr.OpenCustomReportCreator();
            this.CustomReportCreationMgr.SetName(name);
            this.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.Fields);
            this.CustomReportCreationMgr.MoveAllItemsToCurrentChoices();
            this.CustomReportCreationMgr.Apply();
            this.CustomReportCreationMgr.Cancel();
        }

        public void ClickAttendeeReportLinkOnFormDashboard()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ReportLinkOnFormDashboard, LocateBy.Id);
        }

        [Step]
        public int GetTotalRegCountFromDashboard()
        {
            string regCount = string.Empty;
            regCount = UIUtilityProvider.UIHelper.GetText(ReportLinkOnFormDashboard, LocateBy.Id);
            return Convert.ToInt32(regCount);
        }

        [Step]
        public void OpenAttendeeReportFromEventDashboard()
        {
            this.ClickAttendeeReportLinkOnFormDashboard();
            SelectReportPopupWindow();
        }

        public void CopyEventFromDashboard(string copiedEventName)
        {
            ClickOption(DashboardManager.EventAdditionalFunction.CopyEvent);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='OK']/../..", LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            Utility.ThreadSleep(6);
            string test = UIUtilityProvider.UIHelper.GetText("builderSectionHeaderText", LocateBy.ClassName);
            Assert.True(test.Contains(copiedEventName));
        }

        public void OpenBadge(string badgeName)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//table[@id='ctl00_ctl00_cphDialog_cpMgrMain_grdBadges']//a[text()='{0}']", badgeName), LocateBy.XPath);
            Utility.ThreadSleep(3);
            UIUtilityProvider.UIHelper.SelectWindowByTitle("BadgeFilters");
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='OK']", LocateBy.XPath);
            Utility.ThreadSleep(5);
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
            this.SelectBadgeWindow();
        }

        private void SelectBadgeWindow()
        {
            string badgeWindowId = string.Empty;
            ////ReadOnlyCollection<string> popupWindowIds = WebDriverManager.driver.WindowHandles;

            ////foreach (string id in popupWindowIds)
            ////{
            ////    if (WebDriverManager.driver.SwitchTo().Window(id).Title.Contains("Badge"))
            ////    {
            ////        badgeWindowId = id;
            ////        break;
            ////    }
            ////}

            ////UIUtilityProvider.UIHelper.SelectWindow(badgeWindowId, RegOnline.RegressionTest.UIBase.UIManager.SelectWindowBy.Name);
            UIUtilityProvider.UIHelper.SelectWindowByTitle("Badge");
        }

        public void CloseBadge()
        {
            this.SelectBadgeWindow();
            UIUtilityProvider.UIHelper.CloseWindow();
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
        }

        public void OpenAttendeeDirectory(string directoryName)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//table[@id='ctl00_ctl00_cphDialog_cpMgrMain_grdDirectories']//a[text()='{0}']", directoryName), LocateBy.XPath);
            Utility.ThreadSleep(3);
            this.SelectAttendeeDirectoryWindow();
        }

        public void AddAttendeeDirectory(string directoryName)
        {
            if (!UIUtilityProvider.UIHelper.IsElementPresent(string.Format("//table[@id='ctl00_ctl00_cphDialog_cpMgrMain_grdDirectories']//a[text()='{0}']", directoryName), LocateBy.XPath))
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddAttendeeDirectoryLocator, LocateBy.Id);
                Utility.ThreadSleep(3);
                this.SelectAttendeeDirectoryWindow();
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
                AttendeeDirectoryCreationMgr.SetName(directoryName);
            }
            else
            {
                OpenAttendeeDirectory(directoryName);
            }
        }

        public void SelectAttendeeDirectoryWindow()
        {
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.SelectWindowByTitle("DirectoryReport");
        }

        public void CloseAttendeeDirectory()
        {
            this.SelectAttendeeDirectoryWindow();
            UIUtilityProvider.UIHelper.CloseWindow();
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
        }

        [Step]
        public void SelectActivateEventFrame()
        {
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("plain");
        }

        public void ClickPreviewForm()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_ctl00_cphDialog_cpMgrMain_lnkPreview", LocateBy.Id);
        }

        public void VerifyOnDashboard()
        {
            Assert.True(UIUtilityProvider.UIHelper.UrlContainsAbsolutePath(UrlPath));
        }

        public void VerifyOnDashboard(int eventId)
        {
            Assert.True(UIUtilityProvider.UIHelper.UrlContainsPath(
                string.Format(UrlPathFormat_SpecificEvent, GetEventSessionId(), eventId)));
        }
    }
}
