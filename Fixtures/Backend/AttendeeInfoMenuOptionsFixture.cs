namespace RegOnline.RegressionTest.Fixtures.Backend
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Backend;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Managers.Report;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using PIViewField = RegOnline.RegressionTest.Managers.Backend.BackendManager.PersonalInfoViewField;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AttendeeInfoMenuOptionsFixture : FixtureBase
    {
        private class MenuOptionsEvent
        {
            public const string EventName = "Test menu options and deleting individual registrations";
        }

        private class EventToTransferAttendee
        {
            public const string NameForEventToTransferFrom = "Test menu options - Event to transfer from!";
            public const string NameForEventToTransferTo = "Test menu options - Event to transfer to!";

            public enum CustomField
            {
                [StringValue("personal info checkbox!")]
                Checkbox,

                [StringValue("personal info radio buttons!")]
                RadioButton,

                [StringValue("Personal Info 1 line text!")]
                OneLineText
            }

            public enum AgendaItem
            {
                [StringValue("agenda checkbox no fee")]
                CheckboxNoFee,

                [StringValue("Agenda Checkbox w/fee")]
                CheckboxWithFee,

                [StringValue("Agenda Radio Buttons")]
                RadioButton,

                [StringValue("Agenda 1 line text")]
                OneLineText
            }

            public enum MerchandiseItem
            {
                [StringValue("Merchandise1")]
                MerchandiseOne
            }
        }

        private BackendFixtureHelper helper = new BackendFixtureHelper();

        private int eventId;
        private string eventSessionId;
        private int registrationId;
        private string emailAddress;

        [Test]
        [Category(Priority.One)]
        [Description("743")]
        public void PartialMenuOptions()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            this.eventId = ManagerSiteMgr.GetFirstEventId(MenuOptionsEvent.EventName);

            // Register for the event
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            emailAddress = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            this.helper.EnterPersonalInfoDuringRegistration("Test", "T", "McTester");
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            this.registrationId = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.RegistrationId));

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();

            // Open attendee info screen
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);

            // Re-send Confirmation
            BackendMgr.ResendConfirmation();

            // Generate Invoice
            BackendMgr.GenerateInvoiceAndVerify(this.registrationId);

            // Cancel Registration
            BackendMgr.CancelRegistrationAndVerify();

            // Un-cancel Registration
            BackendMgr.UndoCancelRegistrationAndVerify();

            // Generate Reg Details
            BackendMgr.GenerateRegDetailsAndVerify(this.registrationId);

            // Print Badge
            // Cannot verify pdf at the moment
            ////AttendeeInfo.PrintBadgeAndVerify();

            // Check-In Attendee
            BackendMgr.CheckInAndVerify();

            // Undo Check-in
            BackendMgr.UndoCheckinAndVerify();

            // Open Attendee Profile
            BackendMgr.OpenProfileAndVerify(emailAddress, "Test McTester");

            // Add to a group
            BackendMgr.AddToGroupAndVerify();

            // Re-Register for the same event and create a new group by adding an attendee. 
            this.CreateGroupByAddingNewAttendeeAndVerify();
        }

        [Test]
        [Category(Priority.One)]
        [Description("1342")]
        public void AttendeeTransfer()
        {
            Dictionary<EventToTransferAttendee.CustomField, int> cfIds =
                new Dictionary<EventToTransferAttendee.CustomField, int>();

            Dictionary<EventToTransferAttendee.AgendaItem, int> agendaIds =
                new Dictionary<EventToTransferAttendee.AgendaItem, int>();

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            int eventIdToTransferFrom = ManagerSiteMgr.GetFirstEventId(EventToTransferAttendee.NameForEventToTransferFrom);
            int eventIdToTransferTo = ManagerSiteMgr.GetFirstEventId(EventToTransferAttendee.NameForEventToTransferTo);

            DataHelperTool.ChangeAllRegsToTestAndDelete(eventIdToTransferFrom);
            DataHelperTool.ChangeAllRegsToTestAndDelete(eventIdToTransferTo);

            RegisterMgr.OpenRegisterPage(eventIdToTransferFrom);
            RegisterMgr.Checkin();
            string emailAddress = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            this.helper.EnterPersonalInfoDuringRegistration("Tommy", "T", "McTransfer");


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // These ids are for the new custom fields in the event to transfer attendee 'to', not 'from', though they have same names
            cfIds.Add(
                EventToTransferAttendee.CustomField.Checkbox,
                RegisterMgr.DataTool.GetPersonalInfoCustomField(eventIdToTransferTo, StringEnum.GetStringValue(EventToTransferAttendee.CustomField.Checkbox)).Id);

            cfIds.Add(
                EventToTransferAttendee.CustomField.RadioButton,
                RegisterMgr.DataTool.GetPersonalInfoCustomField(eventIdToTransferTo, StringEnum.GetStringValue(EventToTransferAttendee.CustomField.RadioButton)).Id);

            cfIds.Add(
                EventToTransferAttendee.CustomField.OneLineText,
                RegisterMgr.DataTool.GetPersonalInfoCustomField(eventIdToTransferTo, StringEnum.GetStringValue(EventToTransferAttendee.CustomField.OneLineText)).Id);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////


            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(EventToTransferAttendee.CustomField.Checkbox), true);

            RegisterMgr.SelectCustomFieldRadioButtons(
                StringEnum.GetStringValue(EventToTransferAttendee.CustomField.RadioButton),
                "5");

            RegisterMgr.TypeCustomField(
                StringEnum.GetStringValue(EventToTransferAttendee.CustomField.OneLineText),
                "PERSONAL INFO8765678");

            RegisterMgr.Continue();


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // These ids are for the new agenda items in the event to transfer attendee 'to', not 'from', though they have same names
            agendaIds.Add(
                EventToTransferAttendee.AgendaItem.CheckboxNoFee,
                RegisterMgr.DataTool.GetAgendaItem(
                    eventIdToTransferTo,
                    StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.CheckboxNoFee)).Id
                );

            agendaIds.Add(
                EventToTransferAttendee.AgendaItem.CheckboxWithFee,
                RegisterMgr.DataTool.GetAgendaItem(
                    eventIdToTransferTo,
                    StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.CheckboxWithFee)).Id
                );

            agendaIds.Add(
                EventToTransferAttendee.AgendaItem.RadioButton,
                RegisterMgr.DataTool.GetAgendaItem(
                    eventIdToTransferTo,
                    StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.RadioButton)).Id
                );

            agendaIds.Add(
                EventToTransferAttendee.AgendaItem.OneLineText,
                RegisterMgr.DataTool.GetAgendaItem(
                    eventIdToTransferTo,
                    StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.OneLineText)).Id
                );
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////


            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.CheckboxNoFee), true);

            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.CheckboxWithFee), true);

            RegisterMgr.SelectCustomFieldRadioButtons(
                StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.RadioButton),
                "45-54");

            RegisterMgr.TypeCustomField(
                StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.OneLineText),
                "AGENDA PAGE 12345432");

            RegisterMgr.Continue();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Theis id is for the new merchandise item in the event to transfer attendee 'to', not 'from', though they have same names
            int merchId = RegisterMgr.DataTool.GetMerchandiseItem(
                eventIdToTransferTo,
                StringEnum.GetStringValue(EventToTransferAttendee.MerchandiseItem.MerchandiseOne)).Id;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            RegisterMgr.SelectMerchandiseQuantityByName(
                StringEnum.GetStringValue(EventToTransferAttendee.MerchandiseItem.MerchandiseOne),
                2);

            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            this.registrationId = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.RegistrationId));

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            //this.eventId = M3.GetFirstEventId(EventToTransferAttendee.NameForEventToTransferTo);
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);

            // Transfering workflow
            int newRegistrationId = BackendMgr.TransferAttendee(eventIdToTransferTo, EventToTransferAttendee.NameForEventToTransferTo);

            UIUtil.DefaultProvider.SelectOriginalWindow();
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, newRegistrationId);

            // Verify personal info
            BackendMgr.VerifyFieldValue(PIViewField.Id, newRegistrationId);

            BackendMgr.VerifyFieldValue(
                PIViewField.Status,
                StringEnum.GetStringValue(ReportManager.AttendeeStatus.Confirmed));

            BackendMgr.VerifyFieldValue(PIViewField.FullName, "Tommy T McTransfer");
            BackendMgr.VerifyFieldValue(PIViewField.JobTitle, "QA Tester");
            BackendMgr.VerifyFieldValue(PIViewField.Company, "RegOnline");
            BackendMgr.VerifyFieldValue(PIViewField.AddressLine1, "4750 Walnut st.");
            BackendMgr.VerifyFieldValue(PIViewField.CityStateZip, "Boulder, CO 80301");
            BackendMgr.VerifyFieldValue(PIViewField.WorkPhone, "3035775100");
            BackendMgr.VerifyFieldValue(PIViewField.Email, emailAddress);

            // Verify custom fields
            BackendMgr.VerifyCFCheckboxItem(
                cfIds[EventToTransferAttendee.CustomField.Checkbox],
                StringEnum.GetStringValue(EventToTransferAttendee.CustomField.Checkbox),
                true,
                null);

            BackendMgr.VerifyCFMultiChoice(
                cfIds[EventToTransferAttendee.CustomField.RadioButton],
                StringEnum.GetStringValue(EventToTransferAttendee.CustomField.RadioButton),
                "5",
                null);

            BackendMgr.VerifyCFNumberTextDateTime(
                cfIds[EventToTransferAttendee.CustomField.OneLineText],
                StringEnum.GetStringValue(EventToTransferAttendee.CustomField.OneLineText),
                "PERSONAL INFO8765678",
                null);

            // Verify agenda items
            BackendMgr.VerifyCFCheckboxItem(
                agendaIds[EventToTransferAttendee.AgendaItem.CheckboxNoFee],
                StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.CheckboxNoFee),
                true,
                null);

            BackendMgr.VerifyCFCheckboxItem(
                 agendaIds[EventToTransferAttendee.AgendaItem.CheckboxWithFee],
                 StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.CheckboxWithFee),
                 true,
                 null,
                 10);

            BackendMgr.VerifyCFMultiChoice(
                 agendaIds[EventToTransferAttendee.AgendaItem.RadioButton],
                 StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.RadioButton),
                 "45-54",
                 null);

            BackendMgr.VerifyCFNumberTextDateTime(
                 agendaIds[EventToTransferAttendee.AgendaItem.OneLineText],
                 StringEnum.GetStringValue(EventToTransferAttendee.AgendaItem.OneLineText),
                "AGENDA PAGE 12345432",
                 null);

            // Verify merchandise items
            BackendMgr.VerifyMerchandiseItemResponse(
                merchId,
                StringEnum.GetStringValue(EventToTransferAttendee.MerchandiseItem.MerchandiseOne));

            BackendMgr.VerifyMerchandiseItemQuantity(merchId, 2);
            BackendMgr.VerifyMerchandiseItemAmount(merchId, 10);
            BackendMgr.VerifyMerchandiseItemSubtotal(merchId, 20);

            // Verify updating history
            BackendMgr.VerifyUpdateHistory("Transfer from event " + eventIdToTransferFrom + ". Old registration id is " + this.registrationId);

            // Verify payment
            BackendMgr.VerifyPaymentMethod(PaymentMethodManager.PaymentMethod.Check);
            BackendMgr.VerifyAttendeeFees(30.00, 30.00, 30.00, 30.00);
        }

        [Test]
        [Category(Priority.One)]
        [Description("384")]
        public void DeleteIndividualAttendee()
        {
            string emailAddress = string.Empty;

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            this.eventId = ManagerSiteMgr.GetFirstEventId(MenuOptionsEvent.EventName);

            // Register for the event
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin();
            emailAddress = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            this.registrationId = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.RegistrationId));

            this.DeleteAttendeeAndVerify();
        }

        [Verify]
        public void CreateGroupByAddingNewAttendeeAndVerify()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            string emailAddress = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            this.helper.EnterPersonalInfoDuringRegistration("Test", "T", "McTester");
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            this.registrationId = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.RegistrationId));

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);

            // Open register check in page to add a new attendee
            BackendMgr.ClickMenuOptionUnderMore(BackendManager.MoreOption.CreateNewGroupByAddingANewAttendee); ;
            UIUtil.DefaultProvider.SelectWindowByName("Checkin");

            RegisterMgr.Checkin();
            emailAddress = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            this.helper.EnterPersonalInfoDuringRegistration("GroupMember", "T", "McTester");
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            RegisterMgr.UnfoldGroupMember(0);

            int registrationId2 = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForGroupMembers(RegisterManager.ConfirmationPageField.RegistrationId, 0));

            BackendMgr.OpenAttendeeInfoURL(eventSessionId, registrationId2);
            UIUtil.DefaultProvider.IsTextPresent("This Attendee is part of a group");
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Test McTester (primary attendee)", LocateBy.LinkText);
            UIUtil.DefaultProvider.WaitForPageToLoad();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
            UIUtil.DefaultProvider.IsTextPresent(registrationId.ToString());
            UIUtil.DefaultProvider.CloseWindow();
            BackendMgr.SelectAttendeeInfoWindow();
        }

        [Verify]
        public void DeleteAttendeeAndVerify()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();

            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);
            BackendMgr.ClickMenuOptionUnderMore(BackendManager.MoreOption.DeleteThisAttendee);

            UIUtil.DefaultProvider.GetConfirmation();

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();

            ManagerSiteMgr.DashboardMgr.OpenCommonReportURL(ReportManager.CommonReportType.RegistrantList, this.eventId, this.eventSessionId);

            if (UIUtil.DefaultProvider.IsTextPresent(registrationId.ToString()))
            {
                UIUtil.DefaultProvider.FailTest(
                    "RegistrationId '" + registrationId + "' should not be shown cause it has been deleted!");
            }

            // There is no option 'Delete this attendee' for a real registrant
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, 11459508);
            BackendMgr.VerifyMenuOptionPresent(BackendManager.MoreOption.DeleteThisAttendee, false);
        }
    }
}
