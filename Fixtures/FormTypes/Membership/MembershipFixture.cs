namespace RegOnline.RegressionTest.Fixtures.FormTypes.Membership
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class MembershipFixture : FixtureBase
    {
        private const string EventName = "MembershipFixture";
        private const string ConfirmationMessage = "Membership is completed. You can close the window now.";
        private const string CopiedEventName = EventName + " (Copy)";
        private const string CopiedTwiceEventName = CopiedEventName + " (Copy)";
        private const string AdminRegName = "MembershipAdminReg";
        private const string UpdateRegName = "MembershipUpdateReg";
        private const string DiscountCode = "half";
        private const int MerchQuantity = 2;

        private enum CFAdmin
        {
            [StringValue("CFVisible")]
            CFVisible,

            [StringValue("CFInvisible")]
            CFInvisible
        }

        private enum MemberAdmin
        {
            [StringValue("MemberFeeVisible")]
            MemberFeeVisible = 80,

            [StringValue("MemberFeeInvisible")]
            MemberFeeInvisible = 90
        }

        private enum MerchandiseAdmin
        {
            [StringValue("MerchandiseVisible")]
            MerchandiseVisible = 100,

            [StringValue("MerchandiseInvisible")]
            MerchandiseInvisible = 110
        }

        private enum RegTypes
        {
            [StringValue("First")]
            FirstRegType,

            [StringValue("Second")]
            SecondRegType,

            [StringValue("Third")]
            ThirdRegType,

            [StringValue("Fourth")]
            FourthRegType
        }

        private enum CFs
        {
            [StringValue("CF-Checkbox")]
            CFCheckbox,

            [StringValue("CF-Radio")]
            CFRadio,

            [StringValue("CF-DropDown")]
            CFDropDown,

            [StringValue("CF-Numeric")]
            CFNumeric,

            [StringValue("CF-Text")]
            CFText,

            [StringValue("CF-Time")]
            CFTime,

            [StringValue("CF-Header")]
            CFHeader,

            [StringValue("CF-Always")]
            CFAlways,

            [StringValue("CF-Continue")]
            CFContinue,

            [StringValue("CF-Paragraph")]
            CFParagraph,

            [StringValue("CF-Date")]
            CFDate,

            [StringValue("CF-File")]
            CFFile
        }

        private enum MFs
        {
            [StringValue("MF-Checkbox")]
            MFCheckbox,

            [StringValue("MF-Radio")]
            MFRadio,

            [StringValue("MF-DropDown")]
            MFDropDown,

            [StringValue("MF-Numeric")]
            MFNumeric,

            [StringValue("MF-Text")]
            MFText,

            [StringValue("MF-Time")]
            MFTime,

            [StringValue("MF-Header")]
            MFHeader,

            [StringValue("MF-Always")]
            MFAlways,

            [StringValue("MF-Continue")]
            MFContinue,

            [StringValue("MF-Paragraph")]
            MFParagraph,

            [StringValue("MF-Date")]
            MFDate,

            [StringValue("MF-File")]
            MFFile
        }

        private enum Merchandises
        {
            [StringValue("FEE-Header")]
            FEEHeader,

            [StringValue("FEE-Fixed")]
            FEEFixed,

            [StringValue("FEE-Variable")]
            FEEVariable
        }

        private enum Recurrings
        {
            [StringValue("Recurring Checkbox no discount")]
            CheckboxNoDiscount,

            [StringValue("Recurring Always no discount")]
            AlwaysNoDiscount,

            [StringValue("Recurring Checkbox Discount")]
            CheckboxDiscount,

            [StringValue("Recurring Always Discount")]
            AlwaysDiscount,

            [StringValue("Recurring Checkbox One Time Discount")]
            CheckboxOneTimeDiscount,

            [StringValue("Recurring Always One Time Discount")]
            AlwaysOneTimeDiscount
        }

        private double price = 10.00;
        private double allFees = 50.00;
        private double partialFees = 30.00;
        private double allRecurringFees = 40.00;
        private double partialRecurringFees = 20.00;
        private int eventId;
        private List<Registration> regs = new List<Registration>();
        private string sessionId;
        private double totalCharge = 0;
        private int registrationId;
        private MemberTypeAdmin memberTypeAdmin = new MemberTypeAdmin();

        [Test]
        [Category(Priority.Two)]
        [Description("344")]
        public void CreateNewMembership()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(EventName);
            this.CreateEvent();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("345")]
        public void RegisterForMembership()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            eventId = ManagerSiteMgr.GetFirstEventId(EventName);

            CreateRegistrations();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("349")]
        public void RenewMemberhsip()
        {
            ManagerSiteMgr.OpenLogin();
            sessionId = ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            eventId = ManagerSiteMgr.GetFirstEventId(EventName);

            regs = DataHelperTool.GetEventRegistrations(eventId);

            // If there are less than 3 registrants, create new regs
            // Because AutomaticRenewal() and ManualRenewal() should be against the correct reg
            // (which is generated by CreateRegistrations()) with specific index(0, 1, 2)
            if (regs.Count < 3)
            {
                ManagerSiteMgr.OpenEventDashboardUrlAndDeleteTestRegsAndReturnToManagerScreen(eventId, sessionId);
                this.CreateRegistrations();
                regs = DataHelperTool.GetEventRegistrations(eventId);
            }

            AutomaticRenewal(regs[1].Register_Id);
            ManualRenewal(DataHelperTool.GetRegistrationEmail(regs[0]), allFees);

            ManualRenewal(DataHelperTool.GetRegistrationEmail(regs[2]), partialFees);
        }

        [Test]
        [Category(Priority.Five)]
        [Description("684")]
        public void PreviewMembership()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateNewMembership();
            }

            this.PreviewWithRegTypes();
            this.PreviewWithoutRegTypes();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("586")]
        public void CopyMembership()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();

            this.CopyMembershipFromManagerSite();
            this.CopyMembershipFromDashboard();

            ManagerSiteMgr.DeleteEventByName(CopiedEventName);
            ManagerSiteMgr.DeleteEventByName(CopiedTwiceEventName);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("346")]
        public void AdminRegMembership()
        {
            this.CreateMembershipForAdminReg();
            this.AdminRegAndConfirmItemDisplay();
            this.ConfirmRegistrationSaved();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("348")]
        public void UpdateRegMembership()
        {
            this.CreateMembershipForUpdateReg();

            string email = RegisterMgr.ComposeUniqueEmailAddress();

            this.Register(email);
            this.DoUpdateMembership(email);
            this.VerifyRegMembership(this.registrationId);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("347")]
        public void AdminUpdateRegMembership()
        {
            this.CreateMembershipForAdminReg();

            string email = RegisterMgr.ComposeUniqueEmailAddress();

            this.RegisterForAdminUpdate(email);
            this.DoAdminUpdateMembership(email);
            this.VerifyRegMembershipAdmin(this.registrationId);
        }

        private void RegisterForAdminUpdate(string emailAddress)
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.CheckinWithEmail(emailAddress);
            RegisterMgr.SelectRegType(memberTypeAdmin.RegTypeVisibleToAll);
            RegisterMgr.Continue();

            RegisterMgr.EnterProfileInfo();
            this.registrationId = RegisterMgr.GetRegIdFromSession();

            RegisterMgr.SelectPersonalInfoCustomFields();
            RegisterMgr.Continue();

            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();

            RegisterMgr.SelectMerchandise(MerchQuantity);
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void DoAdminUpdateMembership(string emailAddress)
        {
            int newMerchQuantity = 1;

            RegisterMgr.OpenAdminRegisterPage(this.eventId, this.sessionId);
            RegisterMgr.CheckinWithEmail(emailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.Continue();

            RegisterMgr.ClickEditPersonalInformationLink(0);
            RegisterMgr.ClickEditRegistrationType();
            RegisterMgr.ChangeRegistrationType(memberTypeAdmin.RegTypeAdminOnly);
            RegisterMgr.ConfirmChangingRegistrationType();

            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry("Freelance Journalist", null, null, null);
            RegisterMgr.EnterPersonalInfoAddress(null, null, null, "Maine", null, null);
            RegisterMgr.EnterPersonalInfoPhoneNumbers(null, "None", null, null, null);

            RegisterMgr.SetCustomFieldCheckbox(StringEnum.GetStringValue(CFAdmin.CFVisible), false);

            RegisterMgr.customFieldResponses.Find(s => s.RegistrationID == RegisterMgr.CurrentRegistrationId).customFields.Remove(
                RegisterMgr.GetCustomFieldIDForCheckboxItem(StringEnum.GetStringValue(CFAdmin.CFVisible)));

            RegisterMgr.Continue();

            RegisterMgr.SelectAgendaItems();

            RegisterMgr.Continue();

            RegisterMgr.SelectMerchandise(newMerchQuantity);

            RegisterManager.MerchandiseResponse merchResponse =
                RegisterMgr.merchandiseResponses.Find(
                s => s.RegistrationID == RegisterMgr.CurrentRegistrationId
                ).merchandises[RegisterMgr.GetCustomFieldIDForCheckboxItem(StringEnum.GetStringValue(MerchandiseAdmin.MerchandiseVisible))];

            merchResponse.response = newMerchQuantity.ToString();

            RegisterMgr.Continue();

            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void VerifyRegMembershipAdmin(int registerID)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.sessionId, registerID);

            BackendMgr.VerifyCustomFields(RegisterMgr.customFieldResponses, RegisterMgr.CurrentRegistrationId);
            BackendMgr.VerifyMerchandise(RegisterMgr.merchandiseResponses, RegisterMgr.CurrentRegistrationId);
            BackendMgr.VerifyCustomFieldPresent(memberTypeAdmin.RegTypeAdminOnly, true);
            BackendMgr.VerifyCustomFieldPresent(memberTypeAdmin.RegTypeVisibleToAll, false);
        }

        private void CreateMembershipForUpdateReg()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            if (ManagerSiteMgr.EventExists(UpdateRegName))
            {
                ManagerSiteMgr.DeleteEventByName(UpdateRegName);
            }
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.Membership);
            this.eventId = BuilderMgr.GetEventId();

            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.Membership, UpdateRegName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(StringEnum.GetStringValue(RegTypes.FirstRegType));
            BuilderMgr.RegTypeMgr.SaveAndClose();

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(StringEnum.GetStringValue(RegTypes.SecondRegType));
            BuilderMgr.RegTypeMgr.SaveAndClose();

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(StringEnum.GetStringValue(RegTypes.ThirdRegType));
            BuilderMgr.RegTypeMgr.SaveAndClose();

            BuilderMgr.SetEventAllowUpdateRegistration(true);
            BuilderMgr.SelectBuilderWindow();

            BuilderMgr.Next();

            BuilderMgr.ClickAddCustomFieldOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(CFs.CFCheckbox));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(CFs.CFCheckbox));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(CFs.CFCheckbox));
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.RadioButton, StringEnum.GetStringValue(CFs.CFRadio));

            BuilderMgr.Next();

            BuilderMgr.ClickAddAgendaItemOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(MFs.MFCheckbox));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(MFs.MFCheckbox));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(MFs.MFCheckbox));
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.AddAgendaItemOldStyleWithPrice(OtherEventTypeAgendaAndCFManager.FieldType.RadioButton, StringEnum.GetStringValue(MFs.MFRadio), price);

            BuilderMgr.Next();

            BuilderMgr.ClickAddMerchandiseItem();
            BuilderMgr.MerchMgr.SetName(StringEnum.GetStringValue(Merchandises.FEEFixed));
            BuilderMgr.MerchMgr.SetFixedPrice((double)(MerchandiseAdmin.MerchandiseVisible));
            BuilderMgr.MerchMgr.SaveAndClose();

            BuilderMgr.Next();

            BuilderMgr.PaymentMethodMgr.AddPaymentMethod(ManagerBase.PaymentMethod.Check);
            BuilderMgr.PaymentMethodMgr.SetPaymentMethodVisibility(ManagerBase.PaymentMethod.Check, true, true, null);

            BuilderMgr.Next();

            ////BuilderMgr.AddConfirmationMessage(ConfirmationMessage);

            BuilderMgr.SaveAndClose();
        }

        private void Register(string emailAddress)
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.CheckinWithEmail(emailAddress);
            RegisterMgr.SelectRegType(StringEnum.GetStringValue(RegTypes.FirstRegType));
            RegisterMgr.Continue();

            RegisterMgr.EnterProfileInfo();
            this.registrationId = RegisterMgr.GetRegIdFromSession();

            RegisterMgr.SelectPersonalInfoCustomFields();
            RegisterMgr.Continue();

            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();

            RegisterMgr.SelectMerchandise(MerchQuantity);
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void DoUpdateMembership(string emailAddress)
        {
            RegisterMgr.OpenRegisterPage();
            RegisterMgr.CheckinWithEmail(emailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterPassword(ConfigurationProvider.XmlConfig.AccountConfiguration.Password);
            RegisterMgr.Continue();

            RegisterMgr.ClickEditPersonalInformationLink(0);
            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry("Freelance Journalist", null, null, null);
            RegisterMgr.EnterPersonalInfoAddress(null, null, null, "Maine", null, null);
            RegisterMgr.EnterPersonalInfoPhoneNumbers(null, "None", null, null, null);

            RegisterMgr.SetCustomFieldCheckbox(StringEnum.GetStringValue(CFs.CFCheckbox), false);

            RegisterMgr.customFieldResponses.Find(s => s.RegistrationID == RegisterMgr.CurrentRegistrationId).customFields.Remove(
                RegisterMgr.GetCustomFieldIDForCheckboxItem(StringEnum.GetStringValue(CFs.CFCheckbox)));

            RegisterMgr.Continue();
            RegisterMgr.AttendeeCheckEditAgenda();

            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(MFs.MFCheckbox), false);

            RegisterMgr.Continue();
            RegisterMgr.Continue();

            RegisterMgr.Continue();

            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void VerifyRegMembership(int registerID)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.sessionId, registerID);

            BackendMgr.VerifyCustomFields(RegisterMgr.customFieldResponses, RegisterMgr.CurrentRegistrationId);
            BackendMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(MFs.MFCheckbox), false);
        }

        private void CreateMembershipForAdminReg()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            if (ManagerSiteMgr.EventExists(AdminRegName))
            {
                ManagerSiteMgr.DeleteEventByName(AdminRegName);
            }
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.Membership);
            this.eventId = BuilderMgr.GetEventId();

            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.Membership, AdminRegName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(memberTypeAdmin.RegTypeAdminOnly);
            BuilderMgr.RegTypeMgr.SetVisibilities(false, true, null);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            memberTypeAdmin.RegTypeAdminOnlyId = BuilderMgr.Fetch_RegTypeID(eventId, memberTypeAdmin.RegTypeAdminOnly);

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(memberTypeAdmin.RegTypeVisibleToAll);
            BuilderMgr.RegTypeMgr.SetVisibilities(true, true, null);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            memberTypeAdmin.RegTypeVisibleToAllId = BuilderMgr.Fetch_RegTypeID(eventId, memberTypeAdmin.RegTypeVisibleToAll);

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(memberTypeAdmin.RegTypeInvisible);
            BuilderMgr.RegTypeMgr.SetVisibilities(false, false, null);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            memberTypeAdmin.RegTypeInvisibleId = BuilderMgr.Fetch_RegTypeID(eventId, memberTypeAdmin.RegTypeInvisible);

            BuilderMgr.SetEventAllowUpdateRegistration(true);
            BuilderMgr.SetEventAllowChangeRegistrationType(true);
            BuilderMgr.SelectBuilderWindow();

            BuilderMgr.Next();

            BuilderMgr.ClickAddCustomFieldOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(CFAdmin.CFVisible));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(CFAdmin.CFVisible));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(CFAdmin.CFVisible));
            BuilderMgr.OldAGAndCFMgr.SetFieldVisibility(true);
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.ClickAddCustomFieldOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(CFAdmin.CFInvisible));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(CFAdmin.CFInvisible));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(CFAdmin.CFInvisible));
            BuilderMgr.OldAGAndCFMgr.SetFieldVisibility(false);
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.Next();

            BuilderMgr.ClickAddAgendaItemOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(MemberAdmin.MemberFeeVisible));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(MemberAdmin.MemberFeeVisible));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(MemberAdmin.MemberFeeVisible));
            BuilderMgr.OldAGAndCFMgr.SetFieldVisibility(true);
            BuilderMgr.OldAGAndCFMgr.SetRegularPrice((double)(MemberAdmin.MemberFeeVisible));
            BuilderMgr.OldAGAndCFMgr.SetMembershipFeeType(OtherEventTypeAgendaAndCFManager.FeeType.OneTimeFee);
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.ClickAddAgendaItemOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(MemberAdmin.MemberFeeInvisible));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(MemberAdmin.MemberFeeInvisible));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(MemberAdmin.MemberFeeInvisible));
            BuilderMgr.OldAGAndCFMgr.SetFieldVisibility(false);
            BuilderMgr.OldAGAndCFMgr.SetRegularPrice((double)(MemberAdmin.MemberFeeInvisible));
            BuilderMgr.OldAGAndCFMgr.SetMembershipFeeType(OtherEventTypeAgendaAndCFManager.FeeType.OneTimeFee);
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.Next();

            BuilderMgr.ClickAddMerchandiseItem();
            BuilderMgr.MerchMgr.SetName(StringEnum.GetStringValue(MerchandiseAdmin.MerchandiseVisible));
            BuilderMgr.MerchMgr.SetFixedPrice((double)(MerchandiseAdmin.MerchandiseVisible));
            BuilderMgr.MerchMgr.SetMerchVisibility(true);
            BuilderMgr.MerchMgr.SaveAndClose();

            BuilderMgr.ClickAddMerchandiseItem();
            BuilderMgr.MerchMgr.SetName(StringEnum.GetStringValue(MerchandiseAdmin.MerchandiseInvisible));
            BuilderMgr.MerchMgr.SetFixedPrice((double)(MerchandiseAdmin.MerchandiseInvisible));
            BuilderMgr.MerchMgr.SetMerchVisibility(false);
            BuilderMgr.MerchMgr.SaveAndClose();

            BuilderMgr.Next();

            BuilderMgr.PaymentMethodMgr.AddPaymentMethod(ManagerBase.PaymentMethod.Check);
            BuilderMgr.PaymentMethodMgr.SetPaymentMethodVisibility(ManagerBase.PaymentMethod.Check, true, true, null);

            BuilderMgr.Next();

            ////BuilderMgr.AddConfirmationMessage(ConfirmationMessage);

            BuilderMgr.SaveAndClose();
        }

        private void AdminRegAndConfirmItemDisplay()
        {
            RegisterMgr.OpenAdminRegisterPage(eventId, sessionId);
            RegisterMgr.Checkin();
            Assert.True(RegisterMgr.HasRegType(memberTypeAdmin.RegTypeAdminOnlyId));
            Assert.True(RegisterMgr.HasRegType(memberTypeAdmin.RegTypeVisibleToAllId));
            Assert.False(RegisterMgr.HasRegType(memberTypeAdmin.RegTypeInvisibleId));
            RegisterMgr.SelectRegType(memberTypeAdmin.RegTypeAdminOnly);
            RegisterMgr.Continue();

            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFAdmin.CFVisible), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFAdmin.CFInvisible), false);
            RegisterMgr.EnterProfileInfoWithoutPassword();
            this.registrationId = RegisterMgr.GetRegIdFromSession();
            RegisterMgr.Continue();

            //Donation Options page
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(MemberAdmin.MemberFeeVisible), true);
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(MemberAdmin.MemberFeeInvisible), false);
            RegisterMgr.SelectAgendaItem(StringEnum.GetStringValue(MemberAdmin.MemberFeeVisible));
            totalCharge += (double)(MemberAdmin.MemberFeeVisible);
            RegisterMgr.Continue();

            //Merchandise page
            RegisterMgr.VerifyMerchandiseItemPresent(StringEnum.GetStringValue(MerchandiseAdmin.MerchandiseVisible), true);
            RegisterMgr.VerifyMerchandiseItemPresent(StringEnum.GetStringValue(MerchandiseAdmin.MerchandiseInvisible), false);
            RegisterMgr.SelectMerchandiseQuantityByName(StringEnum.GetStringValue(MerchandiseAdmin.MerchandiseVisible), 2);
            totalCharge += (double)(MerchandiseAdmin.MerchandiseVisible) * 2;
            RegisterMgr.Continue();

            //Check out page
            RegisterMgr.SelectPaymentMethod(ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
        }

        private void ConfirmRegistrationSaved()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.sessionId, this.registrationId);
            string total = MoneyTool.FormatMoney(totalCharge);
            BackendMgr.VerifyTotalCharges(total);
            BackendMgr.VerifyTotalTransactions(total);
            BackendMgr.VerifyTotalBalanceDue(total);
        }

        private void CopyMembershipFromManagerSite()
        {
            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateEvent();
            }

            ManagerSiteMgr.CopyEventByName(EventName);
            Assert.True(ManagerSiteMgr.EventExists(CopiedEventName));
        }

        private void CopyMembershipFromDashboard()
        {
            this.eventId = ManagerSiteMgr.GetFirstEventId(CopiedEventName);
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.sessionId);
            ManagerSiteMgr.DashboardMgr.CopyEventFromDashboard(CopiedTwiceEventName);
            ManagerSiteMgr.DashboardMgr.ReturnToList();
        }

        private void PreviewWithRegTypes()
        {
            this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.sessionId);
            ManagerSiteMgr.DashboardMgr.ClickPreviewForm();

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyHasRegTypeWhenPreview(StringEnum.GetStringValue(RegTypes.FirstRegType), true);
            BuilderMgr.VerifyHasRegTypeWhenPreview(StringEnum.GetStringValue(RegTypes.SecondRegType), true);
            BuilderMgr.VerifyHasRegTypeWhenPreview(StringEnum.GetStringValue(RegTypes.ThirdRegType), true);
            BuilderMgr.VerifyHasRegTypeWhenPreview(StringEnum.GetStringValue(RegTypes.FourthRegType), true);
            BuilderMgr.SelectRegTypeWhenPreview(StringEnum.GetStringValue(RegTypes.ThirdRegType));
            BuilderMgr.SelectBuilderWindow();
            BuilderMgr.Next();

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CFs.CFCheckbox), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CFs.CFRadio), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CFs.CFDropDown), true);
            BuilderMgr.Next();

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(MFs.MFCheckbox), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(MFs.MFRadio), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(MFs.MFDropDown), true);
            BuilderMgr.Next();

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(Merchandises.FEEFixed), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(Merchandises.FEEVariable), true);
            BuilderMgr.Next();
            BuilderMgr.Next();

            ////BuilderMgr.SetMobileViewMode(false);
            ////BuilderMgr.VerifyCustomFieldPresentWhenPreview(ConfirmationMessage, true);
            ////BuilderMgr.SaveAndClose();
        }

        private void PreviewWithoutRegTypes()
        {
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventId, this.sessionId);
            BuilderMgr.DeleteRegType(StringEnum.GetStringValue(RegTypes.FirstRegType));
            BuilderMgr.DeleteRegType(StringEnum.GetStringValue(RegTypes.SecondRegType));
            BuilderMgr.DeleteRegType(StringEnum.GetStringValue(RegTypes.ThirdRegType));
            BuilderMgr.DeleteRegType(StringEnum.GetStringValue(RegTypes.FourthRegType));

            BuilderMgr.TogglePreviewAndEditMode();
            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(RegTypes.FirstRegType), false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(RegTypes.SecondRegType), false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(RegTypes.ThirdRegType), false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(RegTypes.FourthRegType), false);
            BuilderMgr.TogglePreviewAndEditMode();

            this.SetStartPage();
            BuilderMgr.SaveAndClose();
        }

        #region build event methods
        private void CreateEvent()
        {
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.Membership);
            this.SetStartPage();
            this.SetPersonalInfoPage();
            this.SetMemberFeesPage();
            this.SetMerchandisePage();
            this.SetCheckoutPage();
            this.SetConfirmationPage();
            BuilderMgr.SaveAndClose();
        }

        [Step]
        public void SetStartPage()
        {
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.Membership, EventName);

            BuilderMgr.AddRegType(StringEnum.GetStringValue(RegTypes.FirstRegType));
            BuilderMgr.VerifyHasRegTypeInDatabase(StringEnum.GetStringValue(RegTypes.FirstRegType));
            BuilderMgr.AddRegType(StringEnum.GetStringValue(RegTypes.SecondRegType));
            BuilderMgr.VerifyHasRegTypeInDatabase(StringEnum.GetStringValue(RegTypes.SecondRegType));
            BuilderMgr.AddRegType(StringEnum.GetStringValue(RegTypes.ThirdRegType));
            BuilderMgr.VerifyHasRegTypeInDatabase(StringEnum.GetStringValue(RegTypes.ThirdRegType));
            BuilderMgr.AddRegType(StringEnum.GetStringValue(RegTypes.FourthRegType));
            BuilderMgr.VerifyHasRegTypeInDatabase(StringEnum.GetStringValue(RegTypes.FourthRegType));

            BuilderMgr.Next();
        }

        [Step]
        public void SetPersonalInfoPage()
        {
            BuilderMgr.VerifyPersonalInfoPageDefaults();
            BuilderMgr.SetPersonalInfoPage();

            // Set this option once more because of webdriver problem:
            // Checkbox for 'CellPhone' field is not checked when webdriver scrolls down the builder window
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(RegOnline.RegressionTest.Managers.Builder.FormDetailManager.PersonalInfoField.Cell, true, null);

            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyPersonalInfoPageSettingsAreSaved();

            // Add/verify custom fields of each type
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox, StringEnum.GetStringValue(CFs.CFCheckbox));
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.RadioButton, StringEnum.GetStringValue(CFs.CFRadio));
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.Dropdown, StringEnum.GetStringValue(CFs.CFDropDown));
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.Number, StringEnum.GetStringValue(CFs.CFNumeric));
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.OneLineText, StringEnum.GetStringValue(CFs.CFText));
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.Time, StringEnum.GetStringValue(CFs.CFTime));
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.SectionHeader, StringEnum.GetStringValue(CFs.CFHeader));
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.AlwaysSelected, StringEnum.GetStringValue(CFs.CFAlways));
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.ContinueButton, StringEnum.GetStringValue(CFs.CFContinue));
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.Paragraph, StringEnum.GetStringValue(CFs.CFParagraph));
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.Date, StringEnum.GetStringValue(CFs.CFDate));
            BuilderMgr.AddCustomFieldOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.FileUpload, StringEnum.GetStringValue(CFs.CFFile));

            BuilderMgr.Next();
        }

        [Step]
        public void SetMemberFeesPage()
        {
            BuilderMgr.AddAgendaItemOldStyleWithPrice(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox, StringEnum.GetStringValue(MFs.MFCheckbox), price);
            BuilderMgr.AddAgendaItemOldStyleWithPrice(OtherEventTypeAgendaAndCFManager.FieldType.RadioButton, StringEnum.GetStringValue(MFs.MFRadio), price);
            BuilderMgr.AddAgendaItemOldStyleWithPrice(OtherEventTypeAgendaAndCFManager.FieldType.Dropdown, StringEnum.GetStringValue(MFs.MFDropDown), price);
            BuilderMgr.AddAgendaItemOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.Number, StringEnum.GetStringValue(MFs.MFNumeric));
            BuilderMgr.AddAgendaItemOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.OneLineText, StringEnum.GetStringValue(MFs.MFText));
            BuilderMgr.AddAgendaItemOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.Time, StringEnum.GetStringValue(MFs.MFTime));
            BuilderMgr.AddAgendaItemOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.SectionHeader, StringEnum.GetStringValue(MFs.MFHeader));
            BuilderMgr.AddAgendaItemOldStyleWithPrice(OtherEventTypeAgendaAndCFManager.FieldType.AlwaysSelected, StringEnum.GetStringValue(MFs.MFAlways), price);
            BuilderMgr.AddAgendaItemOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.ContinueButton, StringEnum.GetStringValue(MFs.MFContinue));
            BuilderMgr.AddAgendaItemOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.Paragraph, StringEnum.GetStringValue(MFs.MFParagraph));
            BuilderMgr.AddAgendaItemOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.Date, StringEnum.GetStringValue(MFs.MFDate));
            BuilderMgr.AddAgendaItemOldStyle(OtherEventTypeAgendaAndCFManager.FieldType.FileUpload, StringEnum.GetStringValue(MFs.MFFile));
            AddRecurringFeeItem(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox, StringEnum.GetStringValue(Recurrings.CheckboxNoDiscount), price);
            AddRecurringFeeItem(OtherEventTypeAgendaAndCFManager.FieldType.AlwaysSelected, StringEnum.GetStringValue(Recurrings.AlwaysNoDiscount), price);
            AddRecurringFeeItemWithDiscount(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox, StringEnum.GetStringValue(Recurrings.CheckboxDiscount), price, "half=-5", false, false);
            AddRecurringFeeItemWithDiscount(OtherEventTypeAgendaAndCFManager.FieldType.AlwaysSelected, StringEnum.GetStringValue(Recurrings.AlwaysDiscount), price, "half=-50%", false, false);
            AddRecurringFeeItemWithDiscount(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox, StringEnum.GetStringValue(Recurrings.CheckboxOneTimeDiscount), price, "half=-5", false, true);
            AddRecurringFeeItemWithDiscount(OtherEventTypeAgendaAndCFManager.FieldType.AlwaysSelected, StringEnum.GetStringValue(Recurrings.AlwaysOneTimeDiscount), price, "half=-50%", false, true);

            BuilderMgr.Next();
        }

        [Step]
        public void SetMerchandisePage()
        {
            BuilderMgr.AddMerchandiseItem(MerchandiseManager.MerchandiseType.Header, StringEnum.GetStringValue(Merchandises.FEEHeader));
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Header, StringEnum.GetStringValue(Merchandises.FEEHeader));
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Fixed, StringEnum.GetStringValue(Merchandises.FEEFixed), price, null, null);
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Fixed, StringEnum.GetStringValue(Merchandises.FEEFixed));
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Variable, StringEnum.GetStringValue(Merchandises.FEEVariable), null, 1.00, 100.00);
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Variable, StringEnum.GetStringValue(Merchandises.FEEVariable));

            BuilderMgr.Next();
        }

        [Step]
        public void SetCheckoutPage()
        {
            BuilderMgr.EnterEventCheckoutPageFull();
            BuilderMgr.SetMembershipRenewalOptions(FormDetailManager.AutoRenewals.MemberSelection, FormDetailManager.Frequency.Annually, false, false);
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventCheckoutPage();

            BuilderMgr.Next();
        }

        [Step]
        public void SetConfirmationPage()
        {
            ////BuilderMgr.AddConfirmationMessage(ConfirmationMessage);
            BuilderMgr.SelectBuilderWindow();
        }

        [Step]
        public void AddRecurringFeeItem(OtherEventTypeAgendaAndCFManager.FieldType type, string name, double price)
        {
            BuilderMgr.ClickAddAgendaItemOld();
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(name);
            BuilderMgr.OldAGAndCFMgr.SelectType(type);
            BuilderMgr.OldAGAndCFMgr.SetRegularPrice(price);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void AddRecurringFeeItemWithDiscount(OtherEventTypeAgendaAndCFManager.FieldType type, string name, double price, string discount, bool required, bool oneTime)
        {
            BuilderMgr.ClickAddAgendaItemOld();
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(name);
            BuilderMgr.OldAGAndCFMgr.SelectType(type);
            BuilderMgr.OldAGAndCFMgr.SetRegularPrice(price);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            BuilderMgr.OldAGAndCFMgr.AddMembershipDiscountCodes("Discount Code", discount, required);
            BuilderMgr.OldAGAndCFMgr.ApplyMembershipDiscountCodeOnce(oneTime);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }
        #endregion

        #region Register Methods
        [Step]
        public void CreateRegistrations()
        {
            //First Registration, Selects all, no auto renewals
            OpenStartPageAndCheckin(StringEnum.GetStringValue(RegTypes.FirstRegType));
            PersonalInfoPage(true);
            AgendaPage(true, true, true);
            MerchandisePage();
            CheckoutPage(allRecurringFees, partialRecurringFees, false);

            //Second Registration, Selects all, with auto renewal
            OpenStartPageAndCheckin(StringEnum.GetStringValue(RegTypes.FirstRegType));
            PersonalInfoPage(true);
            AgendaPage(true, true, true);
            MerchandisePage();
            CheckoutPage(allRecurringFees, partialRecurringFees, true);

            //Third Registration, No selection/discounts, no auto renewal
            OpenStartPageAndCheckin(StringEnum.GetStringValue(RegTypes.FirstRegType));
            PersonalInfoPage(false);
            AgendaPage(false, false, false);
            MerchandisePage();
            CheckoutPage(partialFees, null, false);

        }

        public void OpenStartPageAndCheckin(string regTypeName)
        {
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegTypeRadioButton(regTypeName);
            RegisterMgr.Continue();
        }

        public void PersonalInfoPage(bool customFields)
        {
            RegisterMgr.EnterProfileInfo();
            if (customFields)
            {
                RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CFs.CFCheckbox), true);
                RegisterMgr.SelectCustomFieldRadioButtons(StringEnum.GetStringValue(CFs.CFRadio), "Yes");
                RegisterMgr.SelectCustomFieldDropDown(StringEnum.GetStringValue(CFs.CFDropDown), "Agree");
                RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(StringEnum.GetStringValue(CFs.CFNumeric), "654");
                RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(StringEnum.GetStringValue(CFs.CFText), "Something");
                RegisterMgr.FillOutCustomTimeField(StringEnum.GetStringValue(CFs.CFTime), "8:45 PM");
                RegisterMgr.FillOutCustomParagraphField(StringEnum.GetStringValue(CFs.CFParagraph), "Paragraph Text");
                RegisterMgr.FillOutCustomDateField(StringEnum.GetStringValue(CFs.CFDate), "9/12/2015");
            }

            RegisterMgr.Continue();
        }

        public void AgendaPage(bool selectNoDiscountItem, bool selectDiscountItem, bool selectOneTimeDiscountItem)
        {
            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(MFs.MFCheckbox), true);
            RegisterMgr.SelectCustomFieldRadioButtons(StringEnum.GetStringValue(MFs.MFRadio), "Yes");
            RegisterMgr.SelectCustomFieldDropDown(StringEnum.GetStringValue(MFs.MFDropDown), "Agree");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(StringEnum.GetStringValue(MFs.MFNumeric), "654");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(StringEnum.GetStringValue(MFs.MFText), "Something");
            RegisterMgr.FillOutCustomTimeField(StringEnum.GetStringValue(MFs.MFTime), "8:45 PM");
            RegisterMgr.FillOutCustomParagraphField(StringEnum.GetStringValue(MFs.MFParagraph), "Paragraph Text");
            RegisterMgr.FillOutCustomDateField(StringEnum.GetStringValue(MFs.MFDate), "9/12/2015");

            if (selectNoDiscountItem)
            {
                RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(Recurrings.CheckboxNoDiscount), true);
            }

            if (selectDiscountItem)
            {
                RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(Recurrings.CheckboxDiscount), true);
                RegisterMgr.EnterAgendaItemDiscountCode(StringEnum.GetStringValue(Recurrings.CheckboxDiscount), DiscountCode);
                RegisterMgr.EnterAgendaItemDiscountCode(StringEnum.GetStringValue(Recurrings.AlwaysDiscount), DiscountCode);
            }

            if (selectOneTimeDiscountItem)
            {
                RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(Recurrings.CheckboxOneTimeDiscount), true);
                RegisterMgr.EnterAgendaItemDiscountCode(StringEnum.GetStringValue(Recurrings.CheckboxOneTimeDiscount), DiscountCode);
                RegisterMgr.EnterAgendaItemDiscountCode(StringEnum.GetStringValue(Recurrings.AlwaysOneTimeDiscount), DiscountCode);
            }

            RegisterMgr.Continue();
        }

        public void MerchandisePage()
        {
            RegisterMgr.SelectMerchandiseQuantityByName(StringEnum.GetStringValue(Merchandises.FEEFixed), 1);
            RegisterMgr.EnterMerchandiseVariableAmountByName(StringEnum.GetStringValue(Merchandises.FEEVariable), 10);
            RegisterMgr.VerifyMerchandisePageTotal(20.00);
            RegisterMgr.Continue();
        }

        public void CheckoutPage(double recurringTotals, double? recurringSavings, bool autoRenew)
        {
            RegisterMgr.VerifyCheckoutTotal(60.00);
            RegisterMgr.VerifyCheckoutMembershipRecurringTotal(recurringTotals);
            RegisterMgr.VerifyCheckoutMembershipRecurringSubtotal(recurringTotals);
            if (recurringSavings.HasValue)
            {
                RegisterMgr.VerifyCheckoutMembershipRecurringSaving((double)recurringSavings);
            }
            RegisterMgr.VerifyCheckoutMembershipRecurringDate(DateTime.Today.AddYears(1).ToString("M/d/yyyy"));
            RegisterMgr.SetMembershipAutomaticRenewalCheckbox(autoRenew);
            RegisterMgr.PayMoney(ManagerBase.PaymentMethod.CreditCard);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
        #endregion

        #region Renew Memberships
        [Verify]
        public void ManualRenewal(string emailAddress, double expectedBalanceDue)
        {
            string currentRenewalDate = string.Empty;
            RegisterMgr.CheckinToExistingMembership(eventId, emailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.LoginToMembersip();
            currentRenewalDate = RegisterMgr.GetCurrentRenewalDate();
            DateTime renewalDate = Convert.ToDateTime(currentRenewalDate);
            RegisterMgr.ClickRenewNowButton();

            //We should also be checking recurring total and subtotals, but need to wait for bug 22297 to be fixed. 

            RegisterMgr.VerifyCheckoutCurrentBalance(expectedBalanceDue);
            RegisterMgr.PayMoney(ManagerBase.PaymentMethod.CreditCard);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            RegisterMgr.VerifySuccessfulRenewal(renewalDate.AddYears(1).ToString("M/d/yyyy"));
        }

        [Verify]
        public void AutomaticRenewal(int registrantId)
        {
            UpdateRenewDateAndRunReccuringFeeProcessor(registrantId);
            ManagerSiteMgr.OpenLogin();
            sessionId = ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            BackendMgr.OpenAttendeeInfoURL(sessionId, registrantId);
            BackendMgr.VerifyNextRenewDate(DateTime.Now.AddYears(1));
            BackendMgr.VerifyTransactionHistory(null, Managers.Backend.BackendManager.TransactionTypeString.OnlineCCPayment, PaymentManager.DefaultPaymentInfo.CCNumber, -50.00, eventId);
        }

        public void UpdateRenewDateAndRunReccuringFeeProcessor(int registrantId)
        {
            var db = new DataAccess.ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            int rowsAffected;

            // Set time back to one hour ago so that Recurring Fee Processor can process it
            string command = string.Format(
                "UPDATE dbo.recurringFee SET nextPayDate = '{0}', renewDate = '{0}' WHERE registrationID = {1}",
                DateTime.Now.AddHours(-1).ToString("M/d/yyyy HH:mm:ss.fff"), registrantId);

            rowsAffected = db.ExecuteCommand(command);

            // Set registration date to sometime before now to make one time discount code works correctly
            command = string.Format(
                "UPDATE dbo.Registrations SET RegistrationDate = '{0}' WHERE Register_Id = {1}",
                DateTime.Now.AddYears(-1).ToString("M/d/yyyy HH:mm:ss.fff"), registrantId);

            rowsAffected = db.ExecuteCommand(command);

            UIUtilityProvider.UIHelper.OpenUrl(
                ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrlWithHttps + "recurringFeeProcessor.aspx?Token=E4627417-0596-427C-AC92-399F6EF5B826");

            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }
        #endregion
    }

    public class MemberTypeAdmin
    {
        public int RegTypeAdminOnlyId;
        public int RegTypeVisibleToAllId;
        public int RegTypeInvisibleId;

        public string RegTypeAdminOnly = "RegTypeAdminOnly";
        public string RegTypeVisibleToAll = "RegTypeVisibleToAll";
        public string RegTypeInvisible = "RegTypeInvisible";
    }
}
