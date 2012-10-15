namespace RegOnline.RegressionTest.Fixtures.FormTypes.VirtualEvent
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class VirtualEventFixture :FixtureBase
    {
        private const string EventName = "VirtualEventFixture";
        private const string CopiedEventName = EventName + " (Copy)";
        private const string CopiedTwiceEventName = CopiedEventName + " (Copy)";
        private const string ConfirmationMessage = "Virtual event is completed. You can close the window now.";
        private const string AdminRegName = "VirtualAdminReg";
        private const string UpdateRegName = "VirtualUpdateReg";
        private const int MerchQuantity = 2;

        private enum CFAdmin
        {
            [StringValue("CFVisible")]
            CFVisible,

            [StringValue("CFInvisible")]
            CFInvisible
        }

        private enum AgendaAdmin
        {
            [StringValue("AgendaVisible")]
            AgendaVisible = 80,

            [StringValue("AgendaInvisible")]
            AgendaInvisible = 90
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

        double price = 10.00;
        int eventId;
        int registrationId;
        private string sessionId;
        private RegTypeAdmin regTypeAdmin = new RegTypeAdmin();
        private double totalCharge = 0;

        [Test]
        [Category(Priority.Two)]
        [Description("355")]
        public void CreateVirtualEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            ManagerSiteMgr.DeleteEventByName(EventName);
            this.CreateEvent();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("356")]
        public void RegisterForVirtualEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            eventId = ManagerSiteMgr.GetFirstEventId(EventName);

            CreateRegistration();
            ////EmailMgr.OpenConfirmationEmailUrl(Managers.Emails.EmailManager.EmailCategory.Complete, eventId, registrationId);
            ////EmailMgr.OpenVirtualEventConfirmationLink();
            ////RegisterMgr.JoinAndVerifyWebEvent(FormDetailManager.StartPageDefaultInfo.ConferenceURL);        
        }

        [Test]
        [Category(Priority.Four)]
        [Description("587")]
        public void CopyVirtualEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateEvent();
            }

            this.CopyVirtualEventFromManagerSite();
            this.CopyVirtualEventFromDashboard();

            ManagerSiteMgr.DeleteEventByName(CopiedEventName);
            ManagerSiteMgr.DeleteEventByName(CopiedTwiceEventName);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("357")]
        public void VirtualAdminReg()
        {
            this.CreateVirtualForAdminReg();
            this.AdminRegVirtualAndCheckItemDisplay();
            this.ConfirmVirtualRegistrationSaved();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("358")]
        public void UpdateVirtualReg()
        {
            this.CreateVirtualForUpdateReg();

            string email = RegisterMgr.ComposeUniqueEmailAddress();

            this.Register(email);
            this.DoUpdate(email);
            this.VerifyReg(this.registrationId);
        }

        private void DoUpdate(string emailAddress)
        {
            int newMerchQuantity = 1;

            RegisterMgr.OpenRegisterPage();
            RegisterMgr.CheckinWithEmail(emailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
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

            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();
            RegisterMgr.Continue();

            RegisterMgr.SelectMerchandise(newMerchQuantity);

            RegisterManager.MerchandiseResponse merchResponse =
                RegisterMgr.merchandiseResponses.Find(
                s => s.RegistrationID == RegisterMgr.CurrentRegistrationId
                ).merchandises[RegisterMgr.GetCustomFieldIDForCheckboxItem(StringEnum.GetStringValue(Merchandises.FEEFixed))];

            merchResponse.response = newMerchQuantity.ToString();

            RegisterMgr.Continue();

            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void VerifyReg(int registerID)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.sessionId, registerID);

            BackendMgr.VerifyCustomFields(RegisterMgr.customFieldResponses, RegisterMgr.CurrentRegistrationId);
            BackendMgr.VerifyMerchandise(RegisterMgr.merchandiseResponses, RegisterMgr.CurrentRegistrationId);
        }

        private void CreateVirtualForUpdateReg()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            if (ManagerSiteMgr.EventExists(UpdateRegName))
            {
                ManagerSiteMgr.DeleteEventByName(UpdateRegName);
            }
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.WebEvent);
            this.eventId = BuilderMgr.GetEventId();

            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.WebEvent, UpdateRegName);
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

        private void Register(string emailstring)
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.CheckinWithEmail(emailstring);
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

        private void CreateVirtualForAdminReg()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            if (ManagerSiteMgr.EventExists(AdminRegName))
            {
                ManagerSiteMgr.DeleteEventByName(AdminRegName);
            }
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.WebEvent);
            this.eventId = BuilderMgr.GetEventId();

            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.WebEvent, AdminRegName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(regTypeAdmin.RegTypeAdminOnly);
            BuilderMgr.RegTypeMgr.SetVisibilities(false, true, null);
            BuilderMgr.RegTypeMgr.SetFee(regTypeAdmin.RegTypeAdminOnlyFee);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            regTypeAdmin.RegTypeAdminOnlyId = BuilderMgr.Fetch_RegTypeID(eventId, regTypeAdmin.RegTypeAdminOnly);

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(regTypeAdmin.RegTypeVisibleToAll);
            BuilderMgr.RegTypeMgr.SetVisibilities(true, true, null);
            BuilderMgr.RegTypeMgr.SetFee(regTypeAdmin.RegTypeVisibleToAllFee);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            regTypeAdmin.RegTypeVisibleToAllId = BuilderMgr.Fetch_RegTypeID(eventId, regTypeAdmin.RegTypeVisibleToAll);

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(regTypeAdmin.RegTypeInvisible);
            BuilderMgr.RegTypeMgr.SetVisibilities(false, false, null);
            BuilderMgr.RegTypeMgr.SetFee(regTypeAdmin.RegTypeInvisibleFee);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            regTypeAdmin.RegTypeInvisibleId = BuilderMgr.Fetch_RegTypeID(eventId, regTypeAdmin.RegTypeInvisible);

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
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(AgendaAdmin.AgendaVisible));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(AgendaAdmin.AgendaVisible));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(AgendaAdmin.AgendaVisible));
            BuilderMgr.OldAGAndCFMgr.SetFieldVisibility(true);
            BuilderMgr.OldAGAndCFMgr.SetRegularPrice((double)(AgendaAdmin.AgendaVisible));
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.ClickAddAgendaItemOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(AgendaAdmin.AgendaInvisible));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(AgendaAdmin.AgendaInvisible));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(AgendaAdmin.AgendaInvisible));
            BuilderMgr.OldAGAndCFMgr.SetFieldVisibility(false);
            BuilderMgr.OldAGAndCFMgr.SetRegularPrice((double)(AgendaAdmin.AgendaInvisible));
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
            BuilderMgr.PaymentMethodMgr.SetPaymentMethodVisibility(ManagerBase.PaymentMethod.Check, false, true, null);

            BuilderMgr.Next();

            ////BuilderMgr.AddConfirmationMessage(ConfirmationMessage);

            BuilderMgr.SaveAndClose();
        }

        private void AdminRegVirtualAndCheckItemDisplay()
        {
            RegisterMgr.OpenAdminRegisterPage(eventId, sessionId);
            RegisterMgr.Checkin();
            Assert.True(RegisterMgr.HasRegType(regTypeAdmin.RegTypeAdminOnlyId));
            Assert.True(RegisterMgr.HasRegType(regTypeAdmin.RegTypeVisibleToAllId));
            Assert.False(RegisterMgr.HasRegType(regTypeAdmin.RegTypeInvisibleId));
            RegisterMgr.SelectRegType(regTypeAdmin.RegTypeAdminOnly);
            totalCharge += regTypeAdmin.RegTypeAdminOnlyFee;
            RegisterMgr.Continue();

            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFAdmin.CFVisible), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFAdmin.CFInvisible), false);
            RegisterMgr.EnterProfileInfoWithoutPassword();
            this.registrationId = RegisterMgr.GetRegIdFromSession();
            RegisterMgr.Continue();

            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(AgendaAdmin.AgendaVisible), true);
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(AgendaAdmin.AgendaInvisible), false);
            RegisterMgr.SelectAgendaItem(StringEnum.GetStringValue(AgendaAdmin.AgendaVisible));
            totalCharge += (double)(AgendaAdmin.AgendaVisible);
            RegisterMgr.Continue();

            RegisterMgr.VerifyMerchandiseItemPresent(StringEnum.GetStringValue(MerchandiseAdmin.MerchandiseVisible), true);
            RegisterMgr.VerifyMerchandiseItemPresent(StringEnum.GetStringValue(MerchandiseAdmin.MerchandiseInvisible), false);
            RegisterMgr.SelectMerchandiseQuantityByName(StringEnum.GetStringValue(MerchandiseAdmin.MerchandiseVisible), 2);
            totalCharge += (double)(MerchandiseAdmin.MerchandiseVisible) * 2;
            RegisterMgr.Continue();

            RegisterMgr.SelectPaymentMethod(ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
        }

        private void ConfirmVirtualRegistrationSaved()
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

        private void CopyVirtualEventFromManagerSite()
        {
            ManagerSiteMgr.CopyEventByName(EventName);
            Assert.True(ManagerSiteMgr.EventExists(CopiedEventName));
        }

        private void CopyVirtualEventFromDashboard()
        {
            this.eventId = ManagerSiteMgr.GetFirstEventId(CopiedEventName);
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.sessionId);
            ManagerSiteMgr.DashboardMgr.CopyEventFromDashboard(CopiedTwiceEventName);
            ManagerSiteMgr.DashboardMgr.ReturnToManagerScreenEventList();
        }

        #region build event methods
        private void CreateEvent()
        {
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.WebEvent);
            this.eventId = BuilderMgr.GetEventId();
            this.SetStartPage();
            this.SetPersonalInfoPage();
            this.SetMemberFeesPage();
            this.SetMerchandisePage();
            this.SetCheckoutPage();
            BuilderMgr.SaveAndClose();
        }

        [Step]
        public void SetStartPage()
        {
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.WebEvent, EventName);
            BuilderMgr.SetStartEndDateTimeDefault();

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
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventCheckoutPage();
        }
        #endregion

        #region Registration
        [Step]
        public void CreateRegistration()
        {
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegTypeRadioButton(StringEnum.GetStringValue(RegTypes.ThirdRegType));
            RegisterMgr.Continue();

            RegisterMgr.EnterProfileInfo();
            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CFs.CFCheckbox), true);
            RegisterMgr.SelectCustomFieldRadioButtons(StringEnum.GetStringValue(CFs.CFRadio), "Yes");
            RegisterMgr.SelectCustomFieldDropDown(StringEnum.GetStringValue(CFs.CFDropDown), "Agree");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(StringEnum.GetStringValue(CFs.CFNumeric), "654");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(StringEnum.GetStringValue(CFs.CFText), "Something");
            RegisterMgr.FillOutCustomTimeField(StringEnum.GetStringValue(CFs.CFTime), "8:45 PM");
            RegisterMgr.FillOutCustomParagraphField(StringEnum.GetStringValue(CFs.CFParagraph), "Paragraph Text");
            RegisterMgr.FillOutCustomDateField(StringEnum.GetStringValue(CFs.CFDate), "9/12/2015");
            RegisterMgr.Continue();

            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(MFs.MFCheckbox), true);
            RegisterMgr.SelectCustomFieldRadioButtons(StringEnum.GetStringValue(MFs.MFRadio), "Yes");
            RegisterMgr.SelectCustomFieldDropDown(StringEnum.GetStringValue(MFs.MFDropDown), "Agree");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(StringEnum.GetStringValue(MFs.MFNumeric), "654");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(StringEnum.GetStringValue(MFs.MFText), "Something");
            RegisterMgr.FillOutCustomTimeField(StringEnum.GetStringValue(MFs.MFTime), "8:45 PM");
            RegisterMgr.FillOutCustomParagraphField(StringEnum.GetStringValue(MFs.MFParagraph), "Paragraph Text");
            RegisterMgr.FillOutCustomDateField(StringEnum.GetStringValue(MFs.MFDate), "9/12/2015");
            RegisterMgr.Continue();

            RegisterMgr.SelectMerchandiseQuantityByName(StringEnum.GetStringValue(Merchandises.FEEFixed), 1);
            RegisterMgr.EnterMerchandiseVariableAmountByName(StringEnum.GetStringValue(Merchandises.FEEVariable), 10);
            RegisterMgr.VerifyMerchandisePageTotal(20.00);
            RegisterMgr.Continue();

            RegisterMgr.VerifyCheckoutTotal(60.00);
            RegisterMgr.PayMoney(ManagerBase.PaymentMethod.CreditCard);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            registrationId = RegisterMgr.GetRegID();
        }

        #endregion
    }

    public class RegTypeAdmin
    {
        public int RegTypeAdminOnlyId;
        public int RegTypeVisibleToAllId;
        public int RegTypeInvisibleId;

        public string RegTypeAdminOnly = "RegTypeAdminOnly";
        public string RegTypeVisibleToAll = "RegTypeVisibleToAll";
        public string RegTypeInvisible = "RegTypeInvisible";

        public double RegTypeAdminOnlyFee = 50;
        public double RegTypeVisibleToAllFee = 60;
        public double RegTypeInvisibleFee = 70;
    }
}
