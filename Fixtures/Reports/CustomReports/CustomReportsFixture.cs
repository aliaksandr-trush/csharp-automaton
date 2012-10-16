namespace RegOnline.RegressionTest.Fixtures.Reports.CustomReports
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class CustomReportsFixture : FixtureBase
    {
        private const string ReportNameStandard = "NameOfReportStandardFields";
        private const string ReportNameCustom = "NameOfReportCustomFields";
        private const string EventName = "CustomReportFixture";
        private const string SmartLinkPassword = "123";
        private const string EmailSubject = "ThisIsSubject";
        private const string EmailContent = "This email is to test sending email from custom report<br>";

        private enum ReportKinds
        {
            EventFee,
            AgendaItem,
            Merchandise,
            Tax,
            Filter,
            Grouping,
            Sorting,
            SmartLink
        }

        private enum EventBasics
        {
            [CustomString("First")]
            First,

            [CustomString("Second")]
            Second,

            [CustomString("Third")]
            Third,

            [CustomString("tax1")]
            Tax1 = 10,

            [CustomString("tax2")]
            Tax2 = 20,

            [CustomString("AI-Always")]
            AIAlways = 100,

            [CustomString("MerchandiseItemFixedPrice")]
            MerchandiseItemFixedPrice = 30,

            EventFee = 50
        }

        private enum PersonalInfoCustomField
        {
            [CustomString("CF-Checkbox")]
            Checkbox,
            [CustomString("CF-Radio")]
            RadioButton,
            [CustomString("CF-DropDown")]
            DropDown,
            [CustomString("CF-Numeric")]
            Numeric,
            [CustomString("CF-Text")]
            OneLineText,
            [CustomString("CF-Time")]
            Time,
            [CustomString("CF-Header")]
            SectionHeader,
            [CustomString("CF-Always")]
            AlwaysSelected,
            [CustomString("CF-Continue")]
            ContinueButton,
            [CustomString("CF-Paragraph")]
            Paragraph,
            [CustomString("CF-Date")]
            Date,
            [CustomString("CF-File")]
            FileUpload
        }

        private enum AgendaItem
        {
            [CustomString("AI-Checkbox")]
            Checkbox,
            [CustomString("AI-Radio")]
            RadioButton,
            [CustomString("AI-DropDown")]
            DropDown,
            [CustomString("AI-Numeric")]
            Numeric,
            [CustomString("AI-Text")]
            OneLineText,
            [CustomString("AI-Time")]
            Time,
            [CustomString("AI-Header")]
            SectionHeader,
            [CustomString("AI-Continue")]
            ContinueButton,
            [CustomString("AI-Paragraph")]
            Paragraph,
            [CustomString("AI-Date")]
            Date,
            [CustomString("AI-File")]
            FileUpload,
            [CustomString("AI-Contribution")]
            Contribution
        }

        private int eventId;
        private string sessionId;
        private int timesChangeFilter = 0;
        private int regTypeId;
        private List<int> regIds = new List<int>();

        [Test]
        [Category(Priority.Three)]
        [Description("590")]
        public void CustomReportStandardField()
        {
            this.GoToReportTab();
            this.CreateCustomReportStandardFields(ReportNameStandard);
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("591")]
        public void CustomReportCustomFields()
        {
            this.GoToReportTab();
            this.CreateCustomReportCustomFields(ReportNameCustom);
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("301")]
        public void CustomReportEventFee()
        {
            this.GoToReportTab();
            this.CreateCustomReportStandardFields(ReportNameStandard);
            this.ChangeReportSetting(ReportKinds.EventFee, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.EventFee);
            ReportMgr.CloseReportPopupWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("302")]
        public void CustomReportAgendaItem()
        {
            this.GoToReportTab();
            this.CreateCustomReportStandardFields(ReportNameStandard);
            this.ChangeReportSetting(ReportKinds.AgendaItem, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.AgendaItem);
            ReportMgr.CloseReportPopupWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("303")]
        public void CustomReportMerchandise()
        {
            this.GoToReportTab();
            this.CreateCustomReportStandardFields(ReportNameStandard);
            this.ChangeReportSetting(ReportKinds.Merchandise, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.Merchandise);
            ReportMgr.CloseReportPopupWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("304")]
        public void CustomReportTaxes()
        {
            this.GoToReportTab();
            this.CreateCustomReportStandardFields(ReportNameStandard);
            this.ChangeReportSetting(ReportKinds.Tax, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.Tax);
            ReportMgr.CloseReportPopupWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("305")]
        public void CustomReportFilters()
        {
            this.GoToReportTab();
            this.CreateCustomReportStandardFields(ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.Filter);
            ReportMgr.CloseReportPopupWindow();
            this.ChangeReportSetting(ReportKinds.Filter, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.Filter);
            ReportMgr.CloseReportPopupWindow();
            this.ChangeReportSetting(ReportKinds.Filter, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.Filter);
            ReportMgr.CloseReportPopupWindow();
            this.ChangeReportSetting(ReportKinds.Filter, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.Filter);
            ReportMgr.CloseReportPopupWindow();
            this.ChangeReportSetting(ReportKinds.Filter, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.Filter);
            ReportMgr.CloseReportPopupWindow();
            this.ChangeReportSetting(ReportKinds.Filter, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.Filter);
            ReportMgr.CloseReportPopupWindow();
            this.ChangeReportSetting(ReportKinds.Filter, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.Filter);
            ReportMgr.CloseReportPopupWindow();
            this.ChangeReportSetting(ReportKinds.Filter, ReportNameStandard);
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("306")]
        public void CustomReportSortingGrouping()
        {
            this.GoToReportTab();
            this.CreateCustomReportStandardFields(ReportNameStandard);
            this.ChangeReportSetting(ReportKinds.Grouping, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            this.VerifyReport(ReportKinds.Grouping);
            ReportMgr.CloseReportPopupWindow();
            this.ChangeReportSetting(ReportKinds.Sorting, ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            // There is a deliberate bug here, so may fail when sort.
            this.VerifyReport(ReportKinds.Sorting);
            ReportMgr.CloseReportPopupWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("307")]
        public void CustomReportSmartLink()
        {
            this.GoToReportTab();
            this.CreateCustomReportStandardFields(ReportNameStandard);
            this.ChangeReportSetting(ReportKinds.SmartLink, ReportNameStandard);
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("308")]
        public void CustomReportCheckIn()
        {
            this.GoToReportTab();
            this.CreateCustomReportStandardFields(ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            ReportMgr.SelectAllAttendees();
            ReportMgr.ClickCheckInButton();
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            ReportMgr.VerifyAllAttendeesStatus(Managers.Report.ReportManager.AttendeeStatus.Attended);
            ReportMgr.CloseReportPopupWindow();
            ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ClickAttendeeReportLinkOnFormDashboard();
            ReportMgr.SelectReportPopupWindow();
            ReportMgr.SelectAllAttendees();
            ReportMgr.ClickChangeStatusButton();
            ReportMgr.ChangeStatus(Managers.Report.ReportManager.AttendeeStatus.Attended, Managers.Report.ReportManager.AttendeeStatus.Confirmed);
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            ReportMgr.SelectReportPopupWindow();
            ReportMgr.VerifyAllAttendeesStatus(Managers.Report.ReportManager.AttendeeStatus.Confirmed);
            ReportMgr.CloseReportPopupWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("309")]
        public void CustomReportEmailAttendee()
        {
            this.GoToReportTab();
            this.CreateCustomReportStandardFields(ReportNameStandard);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            ReportMgr.SelectAllAttendees();
            ReportMgr.ClickSendEmailButton();
            ReportMgr.SendGroupEmail(EmailSubject, EmailContent);
            ReportMgr.CloseReportPopupWindow();
            this.BackToEventList();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("310")]
        public void CustomReportChangeStatus()
        {
            this.GoToReportTab();
            this.CreateCustomReportStandardFields(ReportNameStandard);

            // Check in all attendees to make their status as 'Attended'
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            ReportMgr.SelectAllAttendees();
            ReportMgr.ClickCheckInButton();
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            ReportMgr.VerifyAllAttendeesStatus(Managers.Report.ReportManager.AttendeeStatus.Attended);
            ReportMgr.CloseReportPopupWindow();
            ManagerSiteMgr.SelectManagerWindow();

            // Change their status on custom report
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(Managers.Report.ReportManager.CommonReportType.RegistrantList);
            ReportMgr.SelectReportPopupWindow();
            ReportMgr.SelectAllAttendees();
            ReportMgr.ClickChangeStatusButton();
            ReportMgr.ChangeStatus(Managers.Report.ReportManager.AttendeeStatus.Attended, Managers.Report.ReportManager.AttendeeStatus.Approved);
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            ReportMgr.SelectReportPopupWindow();
            ReportMgr.CloseReportPopupWindow();
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(ReportNameStandard);
            ReportMgr.SelectAllAttendees();
            ReportMgr.ClickChangeStatusButtonOnCustomReport();
            ReportMgr.ChangeStatusOnCustomReport(Managers.Report.ReportManager.AttendeeStatus.Approved, Managers.Report.ReportManager.AttendeeStatus.Confirmed);
            ReportMgr.ClickOKOnCheckInConfirmationPopup();
            ReportMgr.SelectReportPopupWindow();
            ReportMgr.VerifyAllAttendeesStatus(Managers.Report.ReportManager.AttendeeStatus.Confirmed);
            ReportMgr.CloseReportPopupWindow();
            this.BackToEventList();
        }

        private void BackToEventList()
        {
            ManagerSiteMgr.SelectManagerWindow();
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ReturnToManagerScreenEventList();
        }

        private void PrepareEventForReport()
        {
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName);
            BuilderMgr.AddRegType(CustomStringAttribute.GetCustomString(EventBasics.First));
            BuilderMgr.AddRegType(CustomStringAttribute.GetCustomString(EventBasics.Second));
            BuilderMgr.AddRegType(CustomStringAttribute.GetCustomString(EventBasics.Third));
            this.regTypeId = ManagerSiteMgr.Fetch_RegTypeID(this.eventId, CustomStringAttribute.GetCustomString(EventBasics.Third));
            BuilderMgr.SetEventFee((double)(EventBasics.EventFee));
            BuilderMgr.ClickEventFeeAdvanced();
            BuilderMgr.EventFeeMgr.ExpandOption();
            BuilderMgr.EventFeeMgr.Tax.ClickAddTaxRatesLink();

            BuilderMgr.EventFeeMgr.Tax.SetTaxRateOne(
                CustomStringAttribute.GetCustomString(EventBasics.Tax1), 
                (double)(EventBasics.Tax1));

            BuilderMgr.EventFeeMgr.Tax.SetTaxRateTwo(
                CustomStringAttribute.GetCustomString(EventBasics.Tax2), 
                (double)(EventBasics.Tax2));

            BuilderMgr.EventFeeMgr.Tax.SaveAndClose();
            BuilderMgr.EventFeeMgr.SaveAndClose();
            BuilderMgr.Next();

            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.CheckBox, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.Checkbox));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.RadioButton, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.RadioButton));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Dropdown, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.DropDown));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Number, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.Numeric));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.OneLineText, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.OneLineText));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Time, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.Time));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.SectionHeader, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.SectionHeader));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.AlwaysSelected, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.AlwaysSelected));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.ContinueButton, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.ContinueButton));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Paragraph, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.Paragraph));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Date, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.Date));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.FileUpload, CustomStringAttribute.GetCustomString(PersonalInfoCustomField.FileUpload));
            BuilderMgr.Next();

            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(CustomStringAttribute.GetCustomString(EventBasics.AIAlways));
            BuilderMgr.AGMgr.SetType(Managers.Builder.AgendaItemManager.AgendaItemType.AlwaysSelected);
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice((double)(EventBasics.AIAlways));
            BuilderMgr.AGMgr.FeeMgr.ExpandOption();
            BuilderMgr.AGMgr.FeeMgr.Tax.ApplyTaxRatesToFee(true, true);
            BuilderMgr.AGMgr.ClickSaveItem();
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.CheckBox, CustomStringAttribute.GetCustomString(AgendaItem.Checkbox));
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.RadioButton, CustomStringAttribute.GetCustomString(AgendaItem.RadioButton));
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.Dropdown, CustomStringAttribute.GetCustomString(AgendaItem.DropDown));
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.Number, CustomStringAttribute.GetCustomString(AgendaItem.Numeric));
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.OneLineText, CustomStringAttribute.GetCustomString(AgendaItem.OneLineText));
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.Time, CustomStringAttribute.GetCustomString(AgendaItem.Time));
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.SectionHeader, CustomStringAttribute.GetCustomString(AgendaItem.SectionHeader));
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.ContinueButton, CustomStringAttribute.GetCustomString(AgendaItem.ContinueButton));
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.Paragraph, CustomStringAttribute.GetCustomString(AgendaItem.Paragraph));
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.Date, CustomStringAttribute.GetCustomString(AgendaItem.Date));
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.FileUpload, CustomStringAttribute.GetCustomString(AgendaItem.FileUpload));
            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.Contribution, CustomStringAttribute.GetCustomString(AgendaItem.Contribution));

            BuilderMgr.GotoPage(FormDetailManager.Page.Merchandise);

            BuilderMgr.AddMerchandiseItemWithFeeAmount(
                MerchandiseManager.MerchandiseType.Fixed, 
                CustomStringAttribute.GetCustomString(EventBasics.MerchandiseItemFixedPrice), 
                (double)(EventBasics.MerchandiseItemFixedPrice), 
                null, 
                null);

            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            BuilderMgr.PaymentMethodMgr.AddPaymentMethod(ManagerBase.PaymentMethod.Check);
            BuilderMgr.SaveAndClose();
        }

        private void PrepareRegistrationsForReport()
        {
            this.regIds.Clear();
            this.Register(CustomStringAttribute.GetCustomString(EventBasics.First));
            this.Register(CustomStringAttribute.GetCustomString(EventBasics.Second));
            this.Register(CustomStringAttribute.GetCustomString(EventBasics.Second));
            this.Register(CustomStringAttribute.GetCustomString(EventBasics.Third));
            this.Register(CustomStringAttribute.GetCustomString(EventBasics.Third));
            this.Register(CustomStringAttribute.GetCustomString(EventBasics.Third));
            this.UpdateRegistrationsInBackend();
        }

        private void Register(string regType)
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(regType);
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectMerchandise(1);
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(PaymentMethodManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            regIds.Add(RegisterMgr.GetRegistrationIdOnConfirmationPage());
        }

        private void UpdateRegistrationsInBackend()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.sessionId);
            ManagerSiteMgr.DashboardMgr.ClickAttendeeReportLinkOnFormDashboard();
            ReportMgr.SelectReportPopupWindow();
            ReportMgr.OpenAttendeeInfoByRegId(regIds[2]);
            BackendMgr.OpenNewTransaction();
            BackendMgr.SelectTransactionTypeAndNext(Managers.Backend.BackendManager.TransactionType.ManualOfflinePayment);

            BackendMgr.EnterRevenueAdjustmentsInfo(
                Managers.Backend.BackendManager.NewTransactionPayMethod.OfflineCCPayment, 
                (double)(EventBasics.AIAlways) * 2);

            BackendMgr.SaveAndCloseTransaction();
            BackendMgr.CloseAttendeeInfo();
            ReportMgr.SelectReportPopupWindow();
            ReportMgr.OpenAttendeeInfoByRegId(regIds[4]);
            BackendMgr.OpenNewTransaction();
            BackendMgr.SelectTransactionTypeAndNext(Managers.Backend.BackendManager.TransactionType.ManualOfflinePayment);

            BackendMgr.EnterRevenueAdjustmentsInfo(
                Managers.Backend.BackendManager.NewTransactionPayMethod.OfflineCCPayment,
                (double)(EventBasics.AIAlways) + (double)(EventBasics.MerchandiseItemFixedPrice) +
                (double)(EventBasics.AIAlways) * (double)(EventBasics.Tax1) / 100 +
                (double)(EventBasics.AIAlways) * (double)(EventBasics.Tax2) / 100);

            BackendMgr.SaveAndCloseTransaction();
            BackendMgr.CloseAttendeeInfo();
            ReportMgr.SelectReportPopupWindow();
            ReportMgr.OpenAttendeeInfoByRegId(regIds[5]);
            BackendMgr.OpenNewTransaction();
            BackendMgr.SelectTransactionTypeAndNext(Managers.Backend.BackendManager.TransactionType.ManualOfflinePayment);

            BackendMgr.EnterRevenueAdjustmentsInfo(
                Managers.Backend.BackendManager.NewTransactionPayMethod.OfflineCCPayment, 
                (double)(EventBasics.AIAlways) * 2);

            BackendMgr.SaveAndCloseTransaction();
            BackendMgr.CloseAttendeeInfo();
            ReportMgr.SelectReportPopupWindow();
            ReportMgr.CloseReportPopupWindow();
        }

        private void GoToReportTab()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.PrepareEventForReport();
            }

            this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);

            if (ManagerSiteMgr.GetEventRegCount(this.eventId).Equals(0))
            {
                this.PrepareRegistrationsForReport();
            }

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.sessionId);
            ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.Reports);
        }

        private int GetRegTypeId(string regType)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            ManagerSiteMgr.ClickEditRegistrationForm(EventName);
            this.eventId = BuilderMgr.GetEventId();
            return ManagerSiteMgr.Fetch_RegTypeID(this.eventId, regType);
        }

        private void CreateCustomReportStandardFields(string reportName)
        {
            if (ManagerSiteMgr.DashboardMgr.IsCustomReportExist(reportName))
            {
                ManagerSiteMgr.DashboardMgr.DeleteCustomReport(reportName);
            }

            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.OpenCustomReportCreator();
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SetName(reportName);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.Fields);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.VerifyFieldsCategorySelected(CustomReportCreator.FieldsCategory.Standard);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AddField(StringEnum.GetStringValue(CustomReportCreator.StandardFields.FullName));
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AddField(StringEnum.GetStringValue(CustomReportCreator.StandardFields.Email));
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AddField(StringEnum.GetStringValue(CustomReportCreator.StandardFields.AddressLine1));
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AddField(StringEnum.GetStringValue(CustomReportCreator.StandardFields.EventTitle));
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AdjustCurrentChoicesOrder(CustomReportCreator.StandardFields.EventTitle, CustomReportCreator.MoveActions.Up);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AddField(StringEnum.GetStringValue(CustomReportCreator.StandardFields.Balance));
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AddField(StringEnum.GetStringValue(CustomReportCreator.StandardFields.RegStatus));
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.SmartLink);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SetEnableSmartLink(true);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.TypeSmartLinkPassword(SmartLinkPassword);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.Apply();
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.Cancel();
        }

        private void CreateCustomReportCustomFields(string reportName)
        {
            if (ManagerSiteMgr.DashboardMgr.IsCustomReportExist(reportName))
            {
                ManagerSiteMgr.DashboardMgr.DeleteCustomReport(reportName);
            }

            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.OpenCustomReportCreator();
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SetName(reportName);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.Fields);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseFieldsCategory(CustomReportCreator.FieldsCategory.Custom);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.MoveAllItemsToCurrentChoices();
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.SmartLink);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SetEnableSmartLink(true);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.TypeSmartLinkPassword(SmartLinkPassword);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.Apply();
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.Cancel();
        }

        private void ChangeReportSetting(ReportKinds kind,string reportName)
        {
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ClickEditCustomReport(reportName);

            switch(kind)
            {
                case ReportKinds.EventFee:
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.Fields);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AddField(StringEnum.GetStringValue(CustomReportCreator.StandardFields.TotalCharge));
                    break;
                case ReportKinds.AgendaItem:
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.Fields);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseFieldsCategory(CustomReportCreator.FieldsCategory.EventFeeAndAgenda);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AddField(CustomStringAttribute.GetCustomString(EventBasics.AIAlways));
                    break;
                case ReportKinds.Merchandise:
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.Fields);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseFieldsCategory(CustomReportCreator.FieldsCategory.Merchandise);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AddField(CustomStringAttribute.GetCustomString(EventBasics.MerchandiseItemFixedPrice));
                    break;
                case ReportKinds.Tax:
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.Fields);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseFieldsCategory(CustomReportCreator.FieldsCategory.EventFeeAndAgenda);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AddField(CustomStringAttribute.GetCustomString(EventBasics.AIAlways));
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SetShownColumn(CustomReportCreator.EventFeeAndAgendaFieldsColumn.Taxes, true);
                    break;
                case ReportKinds.Filter:
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.Filters);
                    string fromDate = string.Format("{0}/{1}/{2}", (DateTime.Today.AddDays(-50)).Month, (DateTime.Today.AddDays(-50)).Day, (DateTime.Today.AddDays(-50)).Year);
                    string toDate = string.Format("{0}/{1}/{2}", DateTime.Today.Month, DateTime.Today.Day, DateTime.Today.Year);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.TypeFiltersDate(fromDate, toDate);

                    if (timesChangeFilter == 0)
                    {
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectFiltersRegType(CustomStringAttribute.GetCustomString(EventBasics.Third));
                    }
                    if (timesChangeFilter == 1)
                    {
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectFiltersRegType("All");
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectFiltersRegStatus(CustomReportCreator.FilterRegStatus.Confirmed);
                    }
                    if (timesChangeFilter == 2)
                    {
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectFiltersRegStatus(CustomReportCreator.FilterRegStatus.All);
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectFiltersBalance(CustomReportCreator.FilterBalance.Positive);
                    }
                    if (timesChangeFilter == 3)
                    {
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectFiltersBalance(CustomReportCreator.FilterBalance.Zero);
                    }
                    if (timesChangeFilter == 4)
                    {
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectFiltersBalance(CustomReportCreator.FilterBalance.Negative);
                    }
                    if (timesChangeFilter == 5)
                    {
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectFiltersBalance(CustomReportCreator.FilterBalance.All);
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SetCustomFilterOne(CustomReportCreator.CustomFilter.RegType, 
                            CustomReportCreator.FilterOperators.EqualTo, CustomStringAttribute.GetCustomString(EventBasics.Third));
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SetCustomFilterTwo(CustomReportCreator.CustomFilter.BalanceDue,
                            CustomReportCreator.FilterOperators.EqualTo,  "0");
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.Fields);
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.AddField(StringEnum.GetStringValue(CustomReportCreator.StandardFields.RegType));
                    }
                    if (timesChangeFilter == 6)
                    {
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ClickAddCustomFilter();
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.VerifyThirdFilterPresents(true);
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ClickClearCustomFilters();
                        ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.VerifyThirdFilterPresents(false);
                    }

                    timesChangeFilter += 1;
                    break;
                case ReportKinds.Grouping:
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.SortingAndGrouping);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectGroupBy(Convert.ToString(97));
                    break;
                case ReportKinds.Sorting:
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.SortingAndGrouping);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectGroupBy(Convert.ToString(0));
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectFirstSortBy(CustomReportCreator.SortingFields.FirstLastName);
                    break;
                case ReportKinds.SmartLink:
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.SmartLink);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ClickViewSmartLink();
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectSmartLinkPopupWindow();
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.VerifySmartLinkNeedsPassword(true);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.TypeSmartLinkPasswordAndSubmit(SmartLinkPassword);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.CloseSmartLinkPopupWindow();
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.TypeSmartLinkPassword("");
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.Apply();
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ClickViewSmartLink();
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectSmartLinkPopupWindow();
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.VerifySmartLinkNeedsPassword(false);
                    ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.CloseSmartLinkPopupWindow();
                    break;
                default:
                    break;
            }

            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.Apply();
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.Cancel();
        }

        private void VerifyReport(ReportKinds kind)
        {
            ReportMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CustomReportCreator.StandardFields.FullName), true);
            ReportMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CustomReportCreator.StandardFields.Email), true);
            ReportMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CustomReportCreator.StandardFields.EventTitle), true);
            ReportMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CustomReportCreator.StandardFields.AddressLine1), true);
            ReportMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CustomReportCreator.StandardFields.Balance), true);

            switch(kind)
            {
                case ReportKinds.EventFee:
                    ReportMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CustomReportCreator.StandardFields.TotalCharge), true);
                    break;
                case ReportKinds.AgendaItem:
                    ReportMgr.VerifyCustomFieldPresent(CustomStringAttribute.GetCustomString(EventBasics.AIAlways), true);
                    break;
                case ReportKinds.Merchandise:
                    ReportMgr.VerifyCustomFieldPresent(CustomStringAttribute.GetCustomString(EventBasics.MerchandiseItemFixedPrice), true);
                    break;
                case ReportKinds.Tax:
                    ReportMgr.VerifyCustomFieldPresent(CustomStringAttribute.GetCustomString(EventBasics.AIAlways), true);
                    ReportMgr.VerifyCustomFieldPresent(CustomStringAttribute.GetCustomString(EventBasics.AIAlways) + "-" + 
                        CustomStringAttribute.GetCustomString(EventBasics.Tax1), true);
                    ReportMgr.VerifyCustomFieldPresent(CustomStringAttribute.GetCustomString(EventBasics.AIAlways) + "-" +
                        CustomStringAttribute.GetCustomString(EventBasics.Tax2), true);
                    ReportMgr.VerifyCustomFieldPresent(CustomStringAttribute.GetCustomString(EventBasics.AIAlways) + "-TaxTotal", true);
                    break;
                case ReportKinds.Filter:
                    if (timesChangeFilter == 0)
                        Assert.True(ReportMgr.ReportHasRecords());
                    if (timesChangeFilter == 1)
                        ReportMgr.VerifyReportFilteredByRegType(this.regTypeId);
                    if (timesChangeFilter == 2)
                        ReportMgr.VerifyReportFilteredByRegStatus(this.eventId, 2);
                    if (timesChangeFilter == 3)
                        ReportMgr.VerifyReportFilteredByBalance(Managers.Report.ReportManager.BalanceTypes.Positive);
                    if (timesChangeFilter == 4)
                        ReportMgr.VerifyReportFilteredByBalance(Managers.Report.ReportManager.BalanceTypes.Zero);
                    if (timesChangeFilter == 5)
                        ReportMgr.VerifyReportFilteredByBalance(Managers.Report.ReportManager.BalanceTypes.Negative);
                    if (timesChangeFilter == 6)
                    {
                        ReportMgr.VerifyReportFilteredByRegType(CustomStringAttribute.GetCustomString(EventBasics.Third));
                        ReportMgr.VerifyReportFilteredByBalance(Managers.Report.ReportManager.BalanceTypes.Zero);
                    }
                    break;
                case ReportKinds.Grouping:
                    ReportMgr.VerifyGroupBy(CustomStringAttribute.GetCustomString(EventBasics.First));
                    ReportMgr.VerifyGroupBy(CustomStringAttribute.GetCustomString(EventBasics.Second));
                    ReportMgr.VerifyGroupBy(CustomStringAttribute.GetCustomString(EventBasics.Third));
                    break;
                case ReportKinds.Sorting:
                    ReportMgr.VerifyReportSortedByFullName(Managers.Report.ReportManager.SortTypes.Ascending);
                    break;
                default:
                    break;
            }
            
        }
    }
}
