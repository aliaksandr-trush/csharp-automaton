namespace RegOnline.RegressionTest.Fixtures.Backend
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers.Backend;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Utilities;
    using Agreement = RegOnline.RegressionTest.Managers.Builder.CFPredefinedMultiChoiceItemManager.Agreement;
    using AttendeeInfoSubPage = RegOnline.RegressionTest.Managers.Backend.BackendManager.AttendeeSubPage;
    using CFStatus = RegOnline.RegressionTest.Managers.Backend.BackendManager.CustomFieldStatus;
    using DateField = RegOnline.RegressionTest.Managers.Backend.BackendManager.DateField;
    using Gender = RegOnline.RegressionTest.Managers.Builder.CFPredefinedMultiChoiceItemManager.Gender;
    using LodgingEditField = RegOnline.RegressionTest.Managers.Backend.BackendManager.LodgingEditField;
    using LodgingViewField = RegOnline.RegressionTest.Managers.Backend.BackendManager.LodgingViewField;
    using LTBookingStatus = RegOnline.RegressionTest.Managers.Backend.BackendManager.LodgingTravelBookingStatus;
    using PIEditField = RegOnline.RegressionTest.Managers.Backend.BackendManager.PersonalInfoEditField;
    using PIViewField = RegOnline.RegressionTest.Managers.Backend.BackendManager.PersonalInfoViewField;
    using TimeField = RegOnline.RegressionTest.Managers.Backend.BackendManager.TimeField;
    using TravelEditField = RegOnline.RegressionTest.Managers.Backend.BackendManager.TravelEditField;
    using TravelViewField = RegOnline.RegressionTest.Managers.Backend.BackendManager.TravelViewField;
    using WouldYou = RegOnline.RegressionTest.Managers.Builder.CFPredefinedMultiChoiceItemManager.WouldYou;
    using YesOrNo = RegOnline.RegressionTest.Managers.Builder.CFPredefinedMultiChoiceItemManager.YesOrNo;
    using RegOnline.RegressionTest.Managers.Builder;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.Managers.Manager;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class BackendEditFixture : FixtureBase
    {
        private BackendFixtureHelper.AttendeeInfoEvent attendeeInfoEvent = new BackendFixtureHelper.AttendeeInfoEvent();
        private BackendFixtureHelper helper = new BackendFixtureHelper();

        private int eventId;
        private string eventSessionId;
        private int registrationId;

        [Test]
        [Category(Priority.One)]
        [Description("716")]
        public void EditEventFeePIAgendaLTMerch()
        {
            BackendFixtureHelper.PersonalInfoFields personalInfoFields = BackendFixtureHelper.PersonalInfoFields.CreateDefaultPersonalInfoFields();

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            this.eventId = ManagerSiteMgr.GetFirstEventId(BackendFixtureHelper.AttendeeInfoEvent.EventName);

            RegisterMgr.CurrentEventId = this.eventId;
            RegisterMgr.OpenRegisterPage();
            RegisterMgr.CheckinWithEmail(personalInfoFields.Email);
            RegisterMgr.SelectRegType(personalInfoFields.RegType);

            // Go to PI page
            RegisterMgr.Continue();

            this.helper.EnterPersonalInfoDuringRegistration();
            RegisterMgr.EnterPersonalInfoPassword();

            // Go to agenda page
            RegisterMgr.Continue();

            // Go to L&T page
            RegisterMgr.Continue();

            // Go to merchandise page
            RegisterMgr.Continue();

            this.attendeeInfoEvent.merchandiseItemsIds.Add(
                BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPrice,
                RegisterMgr.GetMerchandiseItemId(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPrice)));

            this.attendeeInfoEvent.merchandiseItemsIds.Add(
                BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPriceWithMCItems,
                RegisterMgr.GetMerchandiseItemId(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPriceWithMCItems)));

            this.attendeeInfoEvent.merchandiseItemsIds.Add(
                BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmount,
                RegisterMgr.GetMerchandiseItemId(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmount)));

            this.attendeeInfoEvent.merchandiseItemsIds.Add(
                BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmountWithMCItems,
                RegisterMgr.GetMerchandiseItemId(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmountWithMCItems)));

            // Go to checkout page
            RegisterMgr.Continue();

            RegisterMgr.SelectPaymentMethod(RegisterManager.PaymentMethod.Check);

            RegisterMgr.VerifyCheckoutTotal(
                this.attendeeInfoEvent.regTypeFee[BackendFixtureHelper.AttendeeInfoEvent.RegType.One] + BackendFixtureHelper.AttendeeInfoEvent.AgendaAlwaysSelectedFee);

            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            this.registrationId = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.RegistrationId));

            // Search for registration ID from previous registration
            ManagerSiteMgr.OpenLogin();
            this.eventSessionId = ManagerSiteMgr.Login();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);
            BackendMgr.OpenEditAttendeeSubPage(AttendeeInfoSubPage.PersonalInformation);

            personalInfoFields.RegType = StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.RegType.Two);
            personalInfoFields.FirstName = "Tommy";
            this.SetPersonalInfoFields(personalInfoFields);

            BackendMgr.SetGender(BackendManager.Gender.Male);

            BackendMgr.SaveAndCloseEditPersonalInformation();
            BackendMgr.ConfirmWhenSaveAndCloseEditPI(BackendManager.ConfirmWhenSaveEditPI.Correction);

            BackendMgr.SelectAttendeeInfoWindow();

            // Verify That Personal Info Changes Are Saved
            BackendMgr.VerifyFieldValue(PIViewField.Id, registrationId);
            this.VerifyPersonalInfoFields(personalInfoFields);

            // Edit Personal Information (Removing/Changing Feilds)
            BackendMgr.OpenEditAttendeeSubPage(AttendeeInfoSubPage.PersonalInformation);

            personalInfoFields.RegType = StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.RegType.One);
            personalInfoFields.FirstName = "Tom";

            personalInfoFields.Prefix =
                personalInfoFields.Suffix =
                personalInfoFields.JobTitle =
                personalInfoFields.Company =
                personalInfoFields.Country =
                personalInfoFields.AddressLineOne =
                personalInfoFields.AddressLineTwo =
                personalInfoFields.City =
                personalInfoFields.State =
                personalInfoFields.StateShortForm =
                personalInfoFields.ZipCode =
                personalInfoFields.HomePhone =
                personalInfoFields.WorkPhone =
                personalInfoFields.Extension =
                personalInfoFields.Fax =
                personalInfoFields.CellPhone =
                personalInfoFields.SecondaryEmail =
                personalInfoFields.BadgeName =
                personalInfoFields.MembershipNumber =
                personalInfoFields.CustomerNumber =
                personalInfoFields.SocialSecurityNumber =
                personalInfoFields.DateOfBirth_Month =
                personalInfoFields.DateOfBirth_Day =
                personalInfoFields.DateOfBirth_Year =
                personalInfoFields.EmergencyContactName =
                personalInfoFields.EmergencyContactPhone =
                personalInfoFields.ContactName =
                personalInfoFields.ContactPhone =
                personalInfoFields.ContactEmail = string.Empty;

            this.SetPersonalInfoFields(personalInfoFields);
            BackendMgr.SetGender(BackendManager.Gender.ClearSelection);

            BackendMgr.SaveAndCloseEditPersonalInformation();
            BackendMgr.ConfirmWhenSaveAndCloseEditPI(BackendManager.ConfirmWhenSaveEditPI.Correction);

            // Verify Personal Info Changes Took Place
            BackendMgr.SelectAttendeeInfoWindow();
            BackendMgr.VerifyFieldValue(PIViewField.Id, registrationId);
            this.VerifyPersonalInfoFields(personalInfoFields);

            // Start Editing Custom Fields
            BackendMgr.OpenEditAttendeeSubPage(BackendManager.AttendeeSubPage.CustomFields);

            this.attendeeInfoEvent.customFieldsIds.Clear();

            int id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Checkbox));
            this.attendeeInfoEvent.customFieldsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Checkbox, id);
            BackendMgr.SetCheckboxCFItem(id, true);

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.RadioButton));
            this.attendeeInfoEvent.customFieldsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.CustomField.RadioButton, id);
            BackendMgr.SelectCFMultiChoiceRadioButton(id, StringEnum.GetStringValue(Agreement.Agree));

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Dropdown));
            this.attendeeInfoEvent.customFieldsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Dropdown, id);
            BackendMgr.SelectCFDropDownList(id, StringEnum.GetStringValue(Gender.Male));

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Number));
            this.attendeeInfoEvent.customFieldsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Number, id);
            BackendMgr.TypeCFText(id, "12345");

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.OneLineText));
            this.attendeeInfoEvent.customFieldsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.CustomField.OneLineText, id);
            BackendMgr.TypeCFText(id, "TEXT!");

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Time));
            this.attendeeInfoEvent.customFieldsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Time, id);

            BackendMgr.SelectCFTimeField(id, TimeField.Hour, "2");

            BackendMgr.SelectCFTimeField(id, TimeField.Minute, "30");

            BackendMgr.SelectCFTimeField(id, TimeField.ampm, "pm");

            BackendMgr.SelectCFStatus(id, CFStatus.Confirmed);

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Paragraph));
            this.attendeeInfoEvent.customFieldsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Paragraph, id);

            BackendMgr.TypeCFText(id, BackendFixtureHelper.AttendeeInfoEvent.CFParagraphText);

            BackendMgr.SelectCFStatus(id, CFStatus.Approved);

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Date));
            this.attendeeInfoEvent.customFieldsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Date, id);

            BackendMgr.TypeCFDateField(id, DateField.Month, "5");

            BackendMgr.TypeCFDateField(id, DateField.Day, "05");

            BackendMgr.TypeCFDateField(id, DateField.Year, "2011");

            BackendMgr.SelectCFStatus(id, CFStatus.Pending);

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.AlwaysSelected));
            this.attendeeInfoEvent.customFieldsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.CustomField.AlwaysSelected, id);
            BackendMgr.SetCheckboxCFItem(id, false);

            BackendMgr.SaveAndCloseEditCF();

            // Verify custon field changes are Saved
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.VerifyCFCheckboxItem(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Checkbox],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Checkbox),
                true,
                null);

            BackendMgr.VerifyCFMultiChoice(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.RadioButton],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.RadioButton),
                StringEnum.GetStringValue(Agreement.Agree),
                null);

            BackendMgr.VerifyCFMultiChoice(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Dropdown],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Dropdown),
                StringEnum.GetStringValue(Gender.Male),
                null);

            BackendMgr.VerifyCFNumberTextDateTime(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Number],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Number),
                "12345",
                null);

            BackendMgr.VerifyCFNumberTextDateTime(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.OneLineText],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.OneLineText),
                "TEXT!",
                null);

            BackendMgr.VerifyCFNumberTextDateTime(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Time],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Time),
                "02:30 PM",
                CFStatus.Confirmed);

            BackendMgr.VerifyCFParagraph(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Paragraph],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Paragraph),
                BackendFixtureHelper.AttendeeInfoEvent.CFParagraphText,
                CFStatus.Approved);

            BackendMgr.VerifyCFNumberTextDateTime(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Date],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Date),
                "05-May-2011",
                CFStatus.Pending);

            // Clear Custom Field selections. 
            BackendMgr.OpenEditAttendeeSubPage(AttendeeInfoSubPage.CustomFields);

            BackendMgr.SetCheckboxCFItem(this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Checkbox], false);

            BackendMgr.ClearCFSelection(this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.RadioButton]);

            BackendMgr.SelectCFDropDownList(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Dropdown],
                StringEnum.GetStringValue(Gender.PreferNotToAnswer));

            BackendMgr.TypeCFText(this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Number], string.Empty);

            BackendMgr.TypeCFText(this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.OneLineText], string.Empty);

            BackendMgr.SelectCFTimeField(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Time],
                TimeField.Hour,
                string.Empty);

            BackendMgr.SelectCFTimeField(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Time],
                TimeField.Minute,
                string.Empty);

            BackendMgr.SelectCFTimeField(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Time],
                TimeField.ampm,
                string.Empty);

            BackendMgr.SelectCFStatus(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Time],
                CFStatus.None);

            BackendMgr.TypeCFText(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Paragraph],
                 string.Empty);

            BackendMgr.SelectCFStatus(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Paragraph],
                CFStatus.None);

            BackendMgr.TypeCFDateField(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Date],
                DateField.Month,
                string.Empty);

            BackendMgr.TypeCFDateField(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Date],
                DateField.Day,
                string.Empty);

            BackendMgr.TypeCFDateField(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Date],
                DateField.Year,
                string.Empty);

            BackendMgr.SelectCFStatus(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Date],
                CFStatus.None);

            BackendMgr.SetCheckboxCFItem(this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.AlwaysSelected], true);

            BackendMgr.SelectCFStatus(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.AlwaysSelected],
                CFStatus.Approved);

            BackendMgr.SaveAndCloseEditCF();

            // Verify custom field changes are saved
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.VerifyCFMultiChoice(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.Dropdown],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.Dropdown),
                StringEnum.GetStringValue(Gender.PreferNotToAnswer),
                null);

            BackendMgr.VerifyCFCheckboxItem(
                this.attendeeInfoEvent.customFieldsIds[BackendFixtureHelper.AttendeeInfoEvent.CustomField.AlwaysSelected],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.CustomField.AlwaysSelected),
                true,
                CFStatus.Approved);

            // Edit Event Cost
            BackendMgr.OpenEditAttendeeEventCostWindow();

            BackendMgr.SetCheckboxCFItem(
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.RegType.One) + BackendFixtureHelper.AttendeeInfoEvent.RegTypeFeeSuffix,
                false);

            BackendMgr.SetCheckboxCFItem(
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.RegType.Two) + BackendFixtureHelper.AttendeeInfoEvent.RegTypeFeeSuffix,
                true);

            BackendMgr.SelectCFStatus(
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.RegType.Two) + BackendFixtureHelper.AttendeeInfoEvent.RegTypeFeeSuffix,
                CFStatus.Confirmed);

            BackendMgr.SaveAndCloseEditCF();
            BackendMgr.SaveAndBypassTransaction();

            // Verify changes saved and fees are correct
            BackendMgr.VerifyEventCost(2, CFStatus.Confirmed);
            BackendMgr.VerifyAttendeeFees(3.00, 3.00, 3.00, 3.00);

            // Edit Agenda Section
            BackendMgr.OpenEditAttendeeSubPage(AttendeeInfoSubPage.Agenda);

            this.attendeeInfoEvent.agendaItemsIds.Clear();

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.CheckboxWithoutFee));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.CheckboxWithoutFee, id);
            BackendMgr.SetCheckboxCFItem(id, true);

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.CheckboxWithFee));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.CheckboxWithFee, id);
            BackendMgr.SetCheckboxCFItem(id, true);

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.RadioButtonsWithoutFee));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.RadioButtonsWithoutFee, id);

            BackendMgr.SelectCFMultiChoiceRadioButton(
                id,
                StringEnum.GetStringValue(WouldYou.NotSure));

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.RadioButtonsWithFee));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.RadioButtonsWithFee, id);

            BackendMgr.SelectCFMultiChoiceRadioButton(
                id,
                StringEnum.GetStringValue(YesOrNo.No));

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.DropdownWithoutFee));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.DropdownWithoutFee, id);

            BackendMgr.SelectCFDropDownList(
                id,
                StringEnum.GetStringValue(WouldYou.Definitely));

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.DropdownWithFee));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.DropdownWithFee, id);

            BackendMgr.SelectCFDropDownList(
                id,
                StringEnum.GetStringValue(YesOrNo.Yes) + ": " + MoneyTool.FormatMoney(1));

            BackendMgr.SelectCFStatus(id, CFStatus.Pending);

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Number));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Number, id);

            BackendMgr.TypeCFText(id, "8495162");

            BackendMgr.SelectCFStatus(id, CFStatus.Confirmed);

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Time));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Time, id);

            BackendMgr.SelectCFTimeField(id, TimeField.Hour, "3");

            BackendMgr.SelectCFTimeField(id, TimeField.Minute, "40");

            BackendMgr.SelectCFTimeField(id, TimeField.ampm, "pm");

            BackendMgr.SelectCFStatus(id, CFStatus.Approved);

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Paragraph));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Paragraph, id);

            BackendMgr.TypeCFText(id, BackendFixtureHelper.AttendeeInfoEvent.AgendaParagraphText);

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Date));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Date, id);

            BackendMgr.ClickShowCalendar(id);

            BackendMgr.TypeCFDateField(id, DateField.Month, "6");

            BackendMgr.TypeCFDateField(id, DateField.Day, "6");

            BackendMgr.TypeCFDateField(id, DateField.Year, "2006");

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Contribution));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Contribution, id);

            BackendMgr.TypeCFText(id, "10");

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.AlwaysSelectedWithoutFee));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.AlwaysSelectedWithoutFee, id);
            BackendMgr.SetCheckboxCFItem(id, false);

            id = BackendMgr.GetCFItemID(StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.AlwaysSelectedWithFee));
            this.attendeeInfoEvent.agendaItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.AlwaysSelectedWithFee, id);
            BackendMgr.SetCheckboxCFItem(id, false);

            BackendMgr.SaveAndCloseEditCF();
            BackendMgr.SaveAndBypassTransaction();

            // Verify changes to the agenda and fees are updated properly
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.VerifyCFCheckboxItem(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.CheckboxWithoutFee],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.CheckboxWithoutFee),
                true,
                null);

            BackendMgr.VerifyCFCheckboxItem(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.CheckboxWithFee],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.CheckboxWithFee),
                true,
                null,
                1);

            BackendMgr.VerifyCFMultiChoice(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.RadioButtonsWithoutFee],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.RadioButtonsWithoutFee),
                StringEnum.GetStringValue(WouldYou.NotSure),
                null);

            BackendMgr.VerifyCFMultiChoice(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.RadioButtonsWithFee],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.RadioButtonsWithFee),
                StringEnum.GetStringValue(YesOrNo.No),
                null);

            BackendMgr.VerifyCFMultiChoice(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.DropdownWithoutFee],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.DropdownWithoutFee),
                StringEnum.GetStringValue(WouldYou.Definitely),
                null);

            BackendMgr.VerifyCFMultiChoice(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.DropdownWithFee],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.DropdownWithFee),
                StringEnum.GetStringValue(YesOrNo.Yes),
                CFStatus.Pending,
                1);

            BackendMgr.VerifyCFNumberTextDateTime(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Number],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Number),
                "8495162",
                CFStatus.Confirmed);

            BackendMgr.VerifyCFNumberTextDateTime(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Time],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Time),
                "03:40 PM",
                CFStatus.Approved);

            BackendMgr.VerifyCFParagraph(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Paragraph],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Paragraph),
                BackendFixtureHelper.AttendeeInfoEvent.AgendaParagraphText,
                null);

            BackendMgr.VerifyCFNumberTextDateTime(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Date],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Date),
                "06-Jun-2006",
                null);

            BackendMgr.VerifyCFContribution(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Contribution],
                StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Contribution),
                null,
                10);

            // Verify correct fee totals
            BackendMgr.VerifyAttendeeFees(14, 14, 14, 14);

            // Clear Agenda item Selections
            BackendMgr.OpenEditAttendeeSubPage(AttendeeInfoSubPage.Agenda);

            BackendMgr.SetCheckboxCFItem(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.CheckboxWithoutFee],
                false);

            BackendMgr.SetCheckboxCFItem(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.CheckboxWithFee],
                false);

            BackendMgr.ClearCFSelection(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.RadioButtonsWithoutFee]);

            BackendMgr.ClearCFSelection(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.RadioButtonsWithFee]);

            BackendMgr.SelectCFDropDownList(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.DropdownWithoutFee],
                string.Empty);

            BackendMgr.SelectCFDropDownList(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.DropdownWithFee],
                string.Empty);

            BackendMgr.SelectCFStatus(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.DropdownWithFee],
                BackendManager.CustomFieldStatus.None);

            BackendMgr.TypeCFText(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Number],
                string.Empty);

            BackendMgr.SelectCFStatus(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Number],
                CFStatus.None);

            BackendMgr.SelectCFTimeField(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Time],
                TimeField.Hour,
                string.Empty);

            BackendMgr.SelectCFTimeField(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Time],
                TimeField.Minute,
                string.Empty);

            BackendMgr.SelectCFTimeField(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Time],
                TimeField.ampm,
                string.Empty);

            BackendMgr.SelectCFStatus(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Time],
                CFStatus.None);

            BackendMgr.TypeCFText(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Paragraph],
                string.Empty);

            BackendMgr.TypeCFDateField(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Date],
                DateField.Month,
                string.Empty);

            BackendMgr.TypeCFDateField(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Date],
                DateField.Day,
                string.Empty);

            BackendMgr.TypeCFDateField(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Date],
                DateField.Year,
                string.Empty);

            BackendMgr.TypeCFText(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.Contribution],
                string.Empty);

            BackendMgr.SetCheckboxCFItem(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.AlwaysSelectedWithoutFee],
                true);

            BackendMgr.SetCheckboxCFItem(
                this.attendeeInfoEvent.agendaItemsIds[BackendFixtureHelper.AttendeeInfoEvent.AgendaItem.AlwaysSelectedWithFee],
                true);

            BackendMgr.SaveAndCloseEditCF();
            BackendMgr.SaveAndBypassTransaction();

            // Verify correct fee totals
            BackendMgr.VerifyAttendeeFees(3, 3, 3, 3);

            // Editing Lodging and Travel
            BackendMgr.OpenEditAttendeeSubPage(AttendeeInfoSubPage.LodgingAndTravel);

            BackendMgr.SetFieldValue(LodgingEditField.BookingStatus, LTBookingStatus.Confirmed.ToString());
            BackendMgr.SetFieldValue(LodgingEditField.BookingAgent, "Test, Regression");
            BackendMgr.SetFieldValue(LodgingEditField.ConfirmationCode, "9846512575");
            BackendMgr.SetFieldValue(LodgingEditField.HotelPrimary, "St. Julien Hotel & Spa");

            // This handles Java pop-up
            BackendMgr.VerifyConfirmationOnHotelChange();

            // Continue editing lodging selections
            BackendMgr.SetFieldValue(LodgingEditField.HotelSecondary, "Boulder Marriott");
            BackendMgr.TypeLodgingArrivalDateField(DateField.Month, "4");
            BackendMgr.TypeLodgingArrivalDateField(DateField.Day, "26");
            BackendMgr.TypeLodgingArrivalDateField(DateField.Year, "2013");
            //Commented out for bug 23395
            //BackendMgr.SetFieldValue(LodgingEditField.ArrivalTime, "12:00 AM");
            BackendMgr.TypeLodgingDepartureDateField(DateField.Month, "5");
            BackendMgr.TypeLodgingDepartureDateField(DateField.Day, "1");
            BackendMgr.TypeLodgingDepartureDateField(DateField.Year, "2013");
            //BackendMgr.SetFieldValue(LodgingEditField.DepartureTime, "08:22 AM");
            BackendMgr.SetFieldValue(LodgingEditField.RoomPreference, "Queen Downtown Boulder View");
            BackendMgr.SetFieldValue(LodgingEditField.BedPreference, "Two Doubles");
            BackendMgr.SetFieldValue(LodgingEditField.SmokingPreference, "Smoking");
            BackendMgr.SetFieldValue(LodgingEditField.BookingFee, "10");
            BackendMgr.SetFieldValue(LodgingEditField.ShareWith, "Mrs. McTester");

            // The 'System Assigned Sharer' always has a value, even if you input an empty string, 
            // and the default sharer is the current reg itself
            BackendMgr.SetFieldValue(LodgingEditField.RoomSharerId, string.Empty);

            BackendMgr.SetFieldValue(LodgingEditField.AdjoinWith, "Old Man Waterfall");
            BackendMgr.SetFieldValue(LodgingEditField.AdditionalInfo, "I need all silk sheets, I'm allergic to cotton");

            BackendMgr.SetFieldValue(TravelEditField.BookingStatus, LTBookingStatus.Pending.ToString());
            BackendMgr.SetFieldValue(TravelEditField.BookingAgent, "Test, Regression");
            BackendMgr.SetFieldValue(TravelEditField.ConfirmationCode, "6584265173");
            BackendMgr.SetFieldValue(TravelEditField.ArrivalCity, "Denver");
            BackendMgr.SetFieldValue(TravelEditField.ArrivalAirport, "DIA");
            BackendMgr.SetFieldValue(TravelEditField.ArrivalAirline, "British Airways");
            BackendMgr.SetFieldValue(TravelEditField.ArrivalConnection, "No Connections");
            BackendMgr.SetFieldValue(TravelEditField.DepartureCity, "Salt Lake City");
            BackendMgr.SetFieldValue(TravelEditField.DepartureAirport, "Salt Lake International");
            BackendMgr.SetFieldValue(TravelEditField.DepartureAirline, "United");
            BackendMgr.SetFieldValue(TravelEditField.DepartureConnection, "Stop in Seattle");

            BackendMgr.TypeTravelArrivalDateField(DateField.Month, "4");
            BackendMgr.TypeTravelArrivalDateField(DateField.Day, "25");
            BackendMgr.TypeTravelArrivalDateField(DateField.Year, "2013");
            BackendMgr.SetFieldValue(TravelEditField.ArrivalTime, "11:35 PM");

            BackendMgr.TypeTravelDepartureDateField(DateField.Month, "5");
            BackendMgr.TypeTravelDepartureDateField(DateField.Day, "2");
            BackendMgr.TypeTravelDepartureDateField(DateField.Year, "2013");
            BackendMgr.SetFieldValue(TravelEditField.DepartureTime, "08:22 AM");

            BackendMgr.SetFieldValue(TravelEditField.SeatingPreference, "Aisle");
            BackendMgr.SetFieldValue(TravelEditField.FrequentFlyerNumber, "412541254");
            BackendMgr.SetFieldValue(TravelEditField.PassportNumber, "589658965");
            BackendMgr.SetFieldValue(TravelEditField.CreditCardNumber, "4111111111111111");
            BackendMgr.SelectTravelCCExpirationDate(DateField.Month, "2");
            BackendMgr.SelectTravelCCExpirationDate(DateField.Year, "2020");
            BackendMgr.SetFieldValue(TravelEditField.CreditCardHolderName, "test test");

            BackendMgr.SetFieldValue(
                TravelEditField.CreditCardInfo,
                "In case of a water landing my seat cushion can be used a flotation  device.");

            BackendMgr.SetFieldValue(TravelEditField.GroundTransportation, "Stretch Limo");
            BackendMgr.SetFieldValue(TravelEditField.GroundTransportationInfo, "H2 limo please!");
            BackendMgr.SaveAndCloseEditLodingTravel();
            BackendMgr.SaveAndBypassTransaction();

            // Verify Lodging changes are saved
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.VerifyLodgingField(LodgingViewField.BookingStatus, LTBookingStatus.Confirmed.ToString());
            BackendMgr.VerifyLodgingField(LodgingViewField.ConfirmationCode, "9846512575");
            BackendMgr.VerifyLodgingField(LodgingViewField.HotelPrimary, "St. Julien Hotel & Spa");
            BackendMgr.VerifyLodgingField(LodgingViewField.HotelSecondary, "Boulder Marriott");
            BackendMgr.VerifyLodgingField(LodgingViewField.ArrivalDate, "26-Apr-2013" /*12:00 AM"*/);
            //BackendMgr.VerifyLodgingField(LodgingViewField.DepartureDate, "01-May-2013 08:22 AM");
            BackendMgr.VerifyLodgingField(LodgingViewField.DepartureDate, "01-May-2013" /*12:00 AM"*/);
            BackendMgr.VerifyLodgingField(LodgingViewField.RoomPreference, "Queen Downtown Boulder View");
            BackendMgr.VerifyLodgingField(LodgingViewField.BedPreference, "Two Doubles");
            BackendMgr.VerifyLodgingField(LodgingViewField.SmokingPreference, "Smoking");
            BackendMgr.VerifyLodgingField(LodgingViewField.ShareWith, "Mrs. McTester");
            BackendMgr.VerifyLodgingField(LodgingViewField.RoomSharerId, this.registrationId.ToString() + ";");
            BackendMgr.VerifyLodgingField(LodgingViewField.AdjoinWith, "Old Man Waterfall");
            BackendMgr.VerifyLodgingField(LodgingViewField.AdditionalInfo, "I need all silk sheets, I'm allergic to cotton");

            BackendMgr.VerifyTravelField(TravelViewField.BookingStatus, LTBookingStatus.Pending.ToString());
            BackendMgr.VerifyTravelField(TravelViewField.ConfirmationCode, "6584265173");
            BackendMgr.VerifyTravelField(TravelViewField.ArrivalAirline, "British Airways");
            BackendMgr.VerifyTravelField(TravelViewField.ArrivalCity, "Denver");
            BackendMgr.VerifyTravelField(TravelViewField.ArrivalAirport, "DIA");
            BackendMgr.VerifyTravelField(TravelViewField.ArrivalDateTime, "25-Apr-2013 11:35 PM");
            BackendMgr.VerifyTravelField(TravelViewField.ArrivalConnection, "No Connections");
            BackendMgr.VerifyTravelField(TravelViewField.DepartureAirline, "United");
            BackendMgr.VerifyTravelField(TravelViewField.DepartureCity, "Salt Lake City");
            BackendMgr.VerifyTravelField(TravelViewField.DepartureAirport, "Salt Lake International");
            BackendMgr.VerifyTravelField(TravelViewField.DepartureDateTime, "02-May-2013 08:22 AM");
            BackendMgr.VerifyTravelField(TravelViewField.DepartureConnection, "Stop in Seattle");

            BackendMgr.VerifyTravelField(TravelViewField.SeatingPreference, "Aisle");
            BackendMgr.VerifyTravelField(TravelViewField.FrequentFlyerNumber, "412541254");
            BackendMgr.VerifyTravelField(TravelViewField.PassportNumber, "589658965");
            BackendMgr.VerifyTravelField(TravelViewField.CreditCardNumber, "4111111111111111");
            BackendMgr.VerifyTravelField(TravelViewField.CreditCardExpiration, "02/20");
            BackendMgr.VerifyTravelField(TravelViewField.CreditCardHolderName, "test test");

            BackendMgr.VerifyTravelField(
                TravelViewField.CreditCardInfo,
                "In case of a water landing my seat cushion can be used a flotation device.");

            BackendMgr.VerifyTravelField(TravelViewField.GroundTransportation, "Stretch Limo");
            BackendMgr.VerifyTravelField(TravelViewField.GroundTransportationInfo, "H2 limo please!");

            // Verfiy total fees
            BackendMgr.VerifyAttendeeFees(1308, 1308, 3, 1308);

            // Clear Lodging and Travel Selections
            BackendMgr.OpenEditAttendeeSubPage(AttendeeInfoSubPage.LodgingAndTravel);

            BackendMgr.SetFieldValue(LodgingEditField.BookingStatus, LTBookingStatus.Initiated.ToString());
            BackendMgr.SetFieldValue(LodgingEditField.BookingAgent, string.Empty);
            BackendMgr.SetFieldValue(LodgingEditField.ConfirmationCode, string.Empty);
            BackendMgr.SetFieldValue(LodgingEditField.HotelPrimary, "I do not require accommodations");

            BackendMgr.VerifyConfirmationOnHotelChange();

            BackendMgr.SetFieldValue(LodgingEditField.HotelSecondary, "I do not require accommodations");
            BackendMgr.TypeLodgingArrivalDateField(DateField.Month, string.Empty);
            BackendMgr.TypeLodgingArrivalDateField(DateField.Day, string.Empty);
            BackendMgr.TypeLodgingArrivalDateField(DateField.Year, string.Empty);
            //BackendMgr.SetFieldValue(LodgingEditField.ArrivalTime, string.Empty);
            BackendMgr.TypeLodgingDepartureDateField(DateField.Month, string.Empty);
            BackendMgr.TypeLodgingDepartureDateField(DateField.Day, string.Empty);
            BackendMgr.TypeLodgingDepartureDateField(DateField.Year, string.Empty);
            //BackendMgr.SetFieldValue(LodgingEditField.DepartureTime, string.Empty);
            BackendMgr.SetFieldValue(LodgingEditField.RoomPreference, string.Empty);
            BackendMgr.SetFieldValue(LodgingEditField.BedPreference, string.Empty);
            BackendMgr.SetFieldValue(LodgingEditField.SmokingPreference, string.Empty);
            BackendMgr.SetFieldValue(LodgingEditField.BookingFee, string.Empty);
            BackendMgr.SetFieldValue(LodgingEditField.ShareWith, string.Empty);
            BackendMgr.SetFieldValue(LodgingEditField.RoomSharerId, string.Empty);
            BackendMgr.SetFieldValue(LodgingEditField.AdjoinWith, string.Empty);
            BackendMgr.SetFieldValue(LodgingEditField.AdditionalInfo, string.Empty);

            BackendMgr.SetFieldValue(TravelEditField.BookingStatus, LTBookingStatus.Initiated.ToString());
            BackendMgr.SetFieldValue(TravelEditField.BookingAgent, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.ConfirmationCode, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.ArrivalCity, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.ArrivalAirport, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.ArrivalAirline, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.ArrivalConnection, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.DepartureCity, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.DepartureAirport, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.DepartureAirline, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.DepartureConnection, string.Empty);

            BackendMgr.TypeTravelArrivalDateField(DateField.Month, string.Empty);
            BackendMgr.TypeTravelArrivalDateField(DateField.Day, string.Empty);
            BackendMgr.TypeTravelArrivalDateField(DateField.Year, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.ArrivalTime, string.Empty);

            BackendMgr.TypeTravelDepartureDateField(DateField.Month, string.Empty);
            BackendMgr.TypeTravelDepartureDateField(DateField.Day, string.Empty);
            BackendMgr.TypeTravelDepartureDateField(DateField.Year, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.DepartureTime, string.Empty);

            BackendMgr.SetFieldValue(TravelEditField.SeatingPreference, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.FrequentFlyerNumber, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.PassportNumber, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.CreditCardNumber, string.Empty);
            BackendMgr.SelectTravelCCExpirationDate(DateField.Month, "-Month-");
            BackendMgr.SelectTravelCCExpirationDate(DateField.Year, "-Year-");
            BackendMgr.SetFieldValue(TravelEditField.CreditCardHolderName, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.CreditCardInfo, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.GroundTransportation, string.Empty);
            BackendMgr.SetFieldValue(TravelEditField.GroundTransportationInfo, string.Empty);

            BackendMgr.SaveAndCloseEditLodingTravel();
            BackendMgr.SaveAndBypassTransaction();

            // Verify Changes took place
            BackendMgr.VerifyLodgingField(LodgingViewField.BookingStatus, LTBookingStatus.Initiated.ToString());
            BackendMgr.VerifyLodgingField(LodgingViewField.RoomSharerId, this.registrationId.ToString() + ";");

            // Verfiy total fees
            BackendMgr.VerifyAttendeeFees(3, 3, 3, 3);

            // Edit Merchandise
            BackendMgr.OpenEditAttendeeSubPage(AttendeeInfoSubPage.Merchandise);

            BackendMgr.TypeMerchandiseItemQuantity(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPrice],
                10);

            BackendMgr.TypeMerchandiseItemQuantity(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPriceWithMCItems],
                5);

            BackendMgr.SelectMerchandiseItemMultiChoice(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPriceWithMCItems],
                "numero 2");

            BackendMgr.TypeMerchandiseItemAmount(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmount],
                25);

            BackendMgr.SelectMerchandiseItemMultiChoice(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmountWithMCItems],
                "Letter B");

            BackendMgr.TypeMerchandiseItemAmount(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmountWithMCItems],
                15);

            BackendMgr.SaveAndCloseEditMerchandise();
            BackendMgr.SaveAndBypassTransaction();
            BackendMgr.SelectAttendeeInfoWindow();

            // Verify Changes Saved
            BackendMgr.VerifyMerchandiseItemQuantity(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPrice],
                10);

            BackendMgr.VerifyMerchandiseItemSubtotal(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPrice],
                10 * BackendFixtureHelper.AttendeeInfoEvent.MerchandiseFixedPrice);

            BackendMgr.VerifyMerchandiseItemResponse(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPriceWithMCItems],
                "numero 2");

            BackendMgr.VerifyMerchandiseItemQuantity(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPriceWithMCItems],
                5);

            BackendMgr.VerifyMerchandiseItemSubtotal(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPriceWithMCItems],
                5 * BackendFixtureHelper.AttendeeInfoEvent.MerchandiseFixedPrice);

            BackendMgr.VerifyMerchandiseItemAmount(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmount],
                25);

            BackendMgr.VerifyMerchandiseItemSubtotal(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmount],
                25);

            BackendMgr.VerifyMerchandiseItemResponse(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmountWithMCItems],
                "Letter B");

            BackendMgr.VerifyMerchandiseItemAmount(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmountWithMCItems],
                15);

            BackendMgr.VerifyMerchandiseItemSubtotal(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmountWithMCItems],
                15);

            // Verify Fees and Totals
            BackendMgr.VerifyAttendeeFees(58.00, 58.00, 58.00, 58.00);

            // Clear out merchandise selections
            BackendMgr.OpenEditAttendeeSubPage(AttendeeInfoSubPage.Merchandise);

            BackendMgr.TypeMerchandiseItemQuantity(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPrice],
                null);

            BackendMgr.TypeMerchandiseItemQuantity(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPriceWithMCItems],
                null);

            BackendMgr.TypeMerchandiseItemAmount(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmount],
                null);

            BackendMgr.TypeMerchandiseItemAmount(
                this.attendeeInfoEvent.merchandiseItemsIds[BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmountWithMCItems],
                null);

            BackendMgr.SaveAndCloseEditMerchandise();
            BackendMgr.SaveAndBypassTransaction();

            // Verify Changes Saved
            BackendMgr.VerifyAttendeeFees(3.00, 3.00, 3.00, 3.00);
        }

        private void SetPersonalInfoFields(BackendFixtureHelper.PersonalInfoFields personalInfoFields)
        {
            BackendMgr.SetFieldValue(PIEditField.RegType, personalInfoFields.RegType);
            BackendMgr.SetFieldValue(PIEditField.Prefix, personalInfoFields.Prefix);
            BackendMgr.SetFieldValue(PIEditField.First_Name, personalInfoFields.FirstName);
            BackendMgr.SetFieldValue(PIEditField.Last_Name, personalInfoFields.LastName);
            BackendMgr.SetFieldValue(PIEditField.Suffix, personalInfoFields.Suffix);
            BackendMgr.SetFieldValue(PIEditField.Title, personalInfoFields.JobTitle);
            BackendMgr.SetFieldValue(PIEditField.Company, personalInfoFields.Company);
            BackendMgr.SetFieldValue(PIEditField.Country, personalInfoFields.Country);
            BackendMgr.SetFieldValue(PIEditField.City, personalInfoFields.City);
            BackendMgr.SetFieldValue(PIEditField.StateUSCanada, personalInfoFields.State);
            BackendMgr.SetFieldValue(PIEditField.Postal_Code, personalInfoFields.ZipCode);
            BackendMgr.SetFieldValue(PIEditField.Address_1, personalInfoFields.AddressLineOne);
            BackendMgr.SetFieldValue(PIEditField.Address_2, personalInfoFields.AddressLineTwo);
            BackendMgr.SetFieldValue(PIEditField.HomePhone, personalInfoFields.HomePhone);
            BackendMgr.SetFieldValue(PIEditField.WorkPhone, personalInfoFields.WorkPhone);
            BackendMgr.SetFieldValue(PIEditField.Extension, personalInfoFields.Extension);
            BackendMgr.SetFieldValue(PIEditField.Fax, personalInfoFields.Fax);
            BackendMgr.SetFieldValue(PIEditField.CellPhone, personalInfoFields.CellPhone);
            BackendMgr.SetFieldValue(PIEditField.BadgeName, personalInfoFields.BadgeName);
            BackendMgr.SetFieldValue(PIEditField.MembershipNumber, personalInfoFields.MembershipNumber);
            BackendMgr.SetFieldValue(PIEditField.CustomerNumber, personalInfoFields.CustomerNumber);
            BackendMgr.SetFieldValue(PIEditField.SecondaryEmail, personalInfoFields.SecondaryEmail);
            BackendMgr.SetFieldValue(PIEditField.SocialSecurityNumber, personalInfoFields.SocialSecurityNumber);

            BackendMgr.SetFieldValue(PIEditField.DateOfBirth_Month, personalInfoFields.DateOfBirth_Month);
            BackendMgr.SetFieldValue(PIEditField.DateOfBirth_Day, personalInfoFields.DateOfBirth_Day);
            BackendMgr.SetFieldValue(PIEditField.DateOfBirth_Year, personalInfoFields.DateOfBirth_Year);

            BackendMgr.SetFieldValue(PIEditField.EmergencyContactName, personalInfoFields.EmergencyContactName);
            BackendMgr.SetFieldValue(PIEditField.EmergencyContactPhone, personalInfoFields.EmergencyContactPhone);
            BackendMgr.SetFieldValue(PIEditField.ContactName, personalInfoFields.ContactName);
            BackendMgr.SetFieldValue(PIEditField.ContactPhone, personalInfoFields.ContactPhone);
            BackendMgr.SetFieldValue(PIEditField.ContactEmail, personalInfoFields.ContactEmail);
        }

        private void VerifyPersonalInfoFields(BackendFixtureHelper.PersonalInfoFields personalInfoFields)
        {
            BackendMgr.VerifyFieldValue(PIViewField.Status, personalInfoFields.Status);
            BackendMgr.VerifyFieldValue(PIViewField.RegType, personalInfoFields.RegType);
            BackendMgr.VerifyFieldValue(PIViewField.FullName, personalInfoFields.FullName);
            BackendMgr.VerifyFieldValue(PIViewField.JobTitle, personalInfoFields.JobTitle);
            BackendMgr.VerifyFieldValue(PIViewField.Company, personalInfoFields.Company);
            BackendMgr.VerifyFieldValue(PIViewField.Country, personalInfoFields.Country);
            BackendMgr.VerifyFieldValue(PIViewField.AddressLine1, personalInfoFields.AddressLineOne);
            BackendMgr.VerifyFieldValue(PIViewField.AddressLine2, personalInfoFields.AddressLineTwo);
            BackendMgr.VerifyFieldValue(PIViewField.CityStateZip, personalInfoFields.CityStateZip);
            BackendMgr.VerifyFieldValue(PIViewField.WorkPhone, personalInfoFields.WorkPhone);
            BackendMgr.VerifyFieldValue(PIViewField.Extension, personalInfoFields.Extension);
            BackendMgr.VerifyFieldValue(PIViewField.HomePhone, personalInfoFields.HomePhone);
            BackendMgr.VerifyFieldValue(PIViewField.Fax, personalInfoFields.Fax);
            BackendMgr.VerifyFieldValue(PIViewField.CellPhone, personalInfoFields.CellPhone);
            BackendMgr.VerifyFieldValue(PIViewField.OptOutDirectory, personalInfoFields.OptOutDirectory);
            BackendMgr.VerifyFieldValue(PIViewField.SecondaryEmail, personalInfoFields.SecondaryEmail);
            BackendMgr.VerifyFieldValue(PIViewField.NameOnBadge, personalInfoFields.BadgeName);
            BackendMgr.VerifyFieldValue(PIViewField.MembershipNumber, personalInfoFields.MembershipNumber);
            BackendMgr.VerifyFieldValue(PIViewField.CustomerNumber, personalInfoFields.CustomerNumber);
            BackendMgr.VerifyFieldValue(PIViewField.SocialSecurityNumber, personalInfoFields.SocialSecurityNumber);
            BackendMgr.VerifyFieldValue(PIViewField.DateOfBirth, personalInfoFields.DateOfBirth);
            BackendMgr.VerifyFieldValue(PIViewField.EmergencyContactName, personalInfoFields.EmergencyContactName);
            BackendMgr.VerifyFieldValue(PIViewField.EmergencyContactPhone, personalInfoFields.EmergencyContactPhone);
            BackendMgr.VerifyFieldValue(PIViewField.ContactName, personalInfoFields.ContactName);
            BackendMgr.VerifyFieldValue(PIViewField.ContactPhone, personalInfoFields.ContactPhone);
            BackendMgr.VerifyFieldValue(PIViewField.ContactEmail, personalInfoFields.ContactEmail);
        }

        private void AddMerchandiseIds(Fee merch)
        {
            if (merch.Description == StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPrice))
            {
                attendeeInfoEvent.merchandiseItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPrice, merch.Id);
            }

            if (merch.Description == StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPriceWithMCItems))
            {
                attendeeInfoEvent.merchandiseItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.FixedPriceWithMCItems, merch.Id);
            }

            if (merch.Description == StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmount))
            {
                attendeeInfoEvent.merchandiseItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmount, merch.Id);
            }

            if (merch.Description == StringEnum.GetStringValue(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmountWithMCItems))
            {
                attendeeInfoEvent.merchandiseItemsIds.Add(BackendFixtureHelper.AttendeeInfoEvent.MerchandiseItem.VariableAmountWithMCItems, merch.Id);
            }
        }

        private void CreateEventForAttendeeInfo()
        {
            this.LoginAndGetSessionID();

            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();

            this.SetStartPageForAttendeeInfoEvent(BackendFixtureHelper.AttendeeInfoEvent.EventName, new DateTime(2013, 4, 25), new DateTime(2013, 5, 2));

            this.SetPersonalInfoPageForAttendeeInfoEvent();

            this.SetAgendaPageForAttendeeInfoEvent();

            this.SetLTPageForAttendeeInfoEvent(5);

            this.SetMerchandisePageForAttendeeInfoEvent();

            this.SetCheckoutPageForAttendeeInfoEvent();

            this.SetConfrimationPageForAttendeeInfoEvent();

            BuilderMgr.SaveAndClose();
        }

        private void LoginAndGetSessionID()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.eventSessionId = BuilderMgr.GetEventSessionId();
        }

        private void SetStartPageForAttendeeInfoEvent(string eventName, DateTime startDate, DateTime endDate)
        {
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventName);
            BuilderMgr.SetEventFee(1.00);
            BuilderMgr.SetStartDate(startDate);
            BuilderMgr.SetEndDate(endDate);
            BuilderMgr.SelectEventCategory(FormDetailManager.EventCategory.Other);
            BuilderMgr.SelectEventIndustry(FormDetailManager.EventIndustry.ProfessionalAndContinuingEducation);
            BuilderMgr.AddRegTypeWithEventFee("Reg Type one", 1.00);
            BuilderMgr.AddRegTypeWithEventFee("Reg Type two", 2.00);
            BuilderMgr.SaveAndStay();
        }

        private void SetPersonalInfoPageForAttendeeInfoEvent()
        {
            string[] ItemName1 = { "Strongly Agree", "Agree", "Neutral", "Disagree", "Strongly Disagree", "N/A" };
            string[] ItemName2 = { "Male", "Female", "Prefer Not to Answer" };
            
            BuilderMgr.GotoPage(FormDetailManager.Page.PI);

            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.CheckBox, "Personal info checkbox");

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName("Personal info radio button");
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.RadioButton);

            foreach (string name in ItemName1)
            {
                BuilderMgr.CFMgr.AddMultiChoiceItem(name);
            }

            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName("Personal info drop down");
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.Dropdown);
            foreach (string name in ItemName2)
            {
                BuilderMgr.CFMgr.AddMultiChoiceItem(name);
            }
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName("Personal info number");
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.Number);
            BuilderMgr.CFMgr.SetOneLineLength(6);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName("Personal info 1 line text");
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.OneLineText);
            BuilderMgr.CFMgr.SetOneLineLength(5);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName("Personal info time");
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.Time);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName("Personal info paragraph");
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.Paragraph);
            BuilderMgr.CFMgr.SetParagraphCharacterLimit(32000);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName("Personal Info date");
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.Date);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName("Personal info file upload");
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.FileUpload);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName("Personal info always selected");
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.AlwaysSelected);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.SaveAndStay();
        }

        private void SetAgendaPageForAttendeeInfoEvent()
        {
            string[] ItemName1 = { "Definitely", "Probably", "Not Sure", "Probably Not", "Definitely Not" };

            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
            BuilderMgr.ClickYesOnSplashPage();

            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.CheckBox, "Agenda checkbox no fee");

            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.CheckBox, "Agenda checkbox w/fee", 1.00);

            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("Agenda Radio Buttons no fee");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.RadioButton);
            foreach (string name in ItemName1)
            {
                BuilderMgr.AGMgr.AddMultiChoiceItem(name, null);
            }
            BuilderMgr.AGMgr.ClickSaveItem();

            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("Agenda Radio Buttons w/fee");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.RadioButton);
            BuilderMgr.AGMgr.AddMultiChoiceItem("Yes", 1.00);
            BuilderMgr.AGMgr.AddMultiChoiceItem("No", null);
            BuilderMgr.AGMgr.ClickSaveItem();

            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("Agenda dropdown no fee");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.Dropdown);
            foreach (string name in ItemName1)
            {
                BuilderMgr.AGMgr.AddMultiChoiceItem(name, null);
            }
            BuilderMgr.AGMgr.ClickSaveItem();

            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("Agenda dropdown with fee");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.Dropdown);
            BuilderMgr.AGMgr.AddMultiChoiceItem("Yes", 1.00);
            BuilderMgr.AGMgr.AddMultiChoiceItem("No", null);
            BuilderMgr.AGMgr.ClickSaveItem();

            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("Agenda number");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.Number);
            BuilderMgr.AGMgr.SetOneLineLength(10);
            BuilderMgr.AGMgr.ClickSaveItem();

            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.Time, "Agenda Time");

            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.FileUpload, "Agenda File Upload");

            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("Agenda paragraph");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.Paragraph);
            BuilderMgr.AGMgr.SetParagraphCharacterLimit(32000);
            BuilderMgr.AGMgr.ClickSaveItem();

            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.Date, "Agenda Date");


            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("Agenda Contribution");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.Contribution);
            BuilderMgr.AGMgr.SetVariableMinMax(1, 1000);
            BuilderMgr.AGMgr.ClickSaveItem();

            BuilderMgr.AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType.AlwaysSelected, "Agenda Always Selected no fee");

            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.AlwaysSelected, "Agenda always selected w/fee", 1.00);

            BuilderMgr.SaveAndStay();
        }


        private void SetLTPageForAttendeeInfoEvent(int number)
        {
            List<string> RoomTypes = new List<string>();
            string[] HotalName = { "Boulder Marriott", "St. Julien Hotel & Spa" };
            BuilderMgr.GotoPage(FormDetailManager.Page.LodgingTravel);
            BuilderMgr.ClickYesOnSplashPage();

            foreach (string hotal in HotalName)
            {
                BuilderMgr.ClickAddHotel();
                BuilderMgr.HotelMgr.SelectHotelTemplate(hotal);
                RoomTypes = BuilderMgr.GetRoomTypes();

                BuilderMgr.HotelMgr.AddRoomBlock("4/26/2013");

                for (int i = 0; i < number; i++)
                {
                    BuilderMgr.HotelMgr.AddRoomBlockNoDate();
                }

                foreach (string type in RoomTypes)
                {
                    BuilderMgr.HotelMgr.SetCapacityAndRates(type, 500, null, number);
                }

                BuilderMgr.HotelMgr.SaveAndClose();
            }

            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetAssignRoomToRegistrant(true);

            BuilderMgr.LodgingStandardFieldsMgr.SetRoomType(true, true);
            BuilderMgr.LodgingStandardFieldsMgr.SetBedType(true, false);
            BuilderMgr.LodgingStandardFieldsMgr.SetSmokingPreference(true, false);
            BuilderMgr.LodgingStandardFieldsMgr.SetSharingWith(true, false);
            BuilderMgr.LodgingStandardFieldsMgr.SetAdjoiningWith(true, false);
            BuilderMgr.LodgingStandardFieldsMgr.SetCheckInOutDate(true, true);
            BuilderMgr.LodgingStandardFieldsMgr.SetValidDateRangeForCheckInOut(new DateTime(2013, 4, 25), new DateTime(2013, 4, 30));
            BuilderMgr.LodgingStandardFieldsMgr.SetAdditionalInfo(true, false);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.ChoosePaymentOption(LodgingSettingsAndPaymentOptionsManager.PaymentOption.ChargeForLodging);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetHotelBookingFee(10.00);

            BuilderMgr.TravelStandardAdditionalFieldsMgr.ChoicePurposeForCollectingTravelInfo(TravelStandardAdditionalFieldsManager.TravelInfo.PurposeBooking);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetAirline(true, false, true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetAirport(true, false, true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetCity(true, false, true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetDateTime(true, false, true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetConnectionInfo(true, false, true, false);

            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetFrequentFlyerNumber(true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetSeatingPreference(true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetPassportNumber(true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetGroundTransportPreference(true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetAdditionalInfo(true, false);

            BuilderMgr.SaveAndStay();
        }

        private void SetMerchandisePageForAttendeeInfoEvent()
        {
            string[] itemname_fixed = { "numero 1", "numero 2" };
            string[] itemname_var = { "Letter A", "Letter B" };
            BuilderMgr.GotoPage(FormDetailManager.Page.Merchandise);
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Fixed, "Fixed price", 1.00, null, null);
            BuilderMgr.AddMerchandiseItemWithMultipleChoiceItem(MerchandiseManager.MerchandiseType.Fixed, "Fixed price w/MC items", 1.00, null, null, itemname_fixed, null);

            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Variable, "Variable amount", null, 1.00, 1000.00);
            BuilderMgr.AddMerchandiseItemWithMultipleChoiceItem(MerchandiseManager.MerchandiseType.Variable, "Variable amount w/MC items", null, 1.00, 1000.00, itemname_var, null);
        }


        private void SetCheckoutPageForAttendeeInfoEvent()
        {
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndStay();
        }

        private void SetConfrimationPageForAttendeeInfoEvent()
        {
            BuilderMgr.GotoPage(FormDetailManager.Page.Confirmation);
            BuilderMgr.SetEnableHotelSearchFeature(false);
        }
    }
}
