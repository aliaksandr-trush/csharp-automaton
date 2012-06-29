namespace RegOnline.RegressionTest.Fixtures.RegisterAndBuilder.OtherFormTypeSpecificTests.Donation
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class DonationFixture : FixtureBase
    {
        private const string EventName = "DonationFixture";
        private const string AdminRegName = "DonationFixtureAdminReg";
        private const string CustomFieldName = "Donor Custom Field";
        private const string DonationConfirmationMessage = "The donation is completed. You can close the window now.";  

        private enum CFs
        {
            [StringValue("CFVisible")]
            CFVisible,

            [StringValue("CFInvisible")]
            CFInvisible
        }

        private enum DonationOptions
        {
            [StringValue("DonationOptionVisible")]
            DonationOptionVisible = 80,

            [StringValue("DonationOptionInvisible")]
            DonationOptionInvisible = 90
        }

        private enum Merchandises
        {
            [StringValue("MerchandiseVisible")]
            MerchandiseVisible = 100,

            [StringValue("MerchandiseInvisible")]
            MerchandiseInvisible = 110
        }

        private double totalCharge = 0;
        private int eventId;
        private string eventSessionId;
        private int registrationId;
        private DonorTyeps donorTypes = new DonorTyeps();

        private struct DonationOptionName
        {
            public const string CheckboxItem = "Even Ten";
            public const string VariableAmountItem = "Variable";
        }

        private struct DonationOptionPrice
        {
            public const double CheckboxItem = 10;
            public const double VariableAmountItemMax = 177;
        }

        [Test]
        [Category(Priority.Two)]
        [Description("330")]
        public void CreateDonationForm()
        {
            // Step #1
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.DonationForm);

            // Step #2
            this.SetStartPage();

            // Step #3
            BuilderMgr.Next();
            this.SetPIPage();

            // Step #4
            BuilderMgr.Next();
            this.SetDonationOptionsPage();

            // Step #5
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            this.SetCheckoutPage();
            BuilderMgr.SaveAndClose();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("331")]
        public void RegisterDonationForm()
        {
            // Step #1
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateDonationForm();
            }

            this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);

            // Step #2
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            // Step #3
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.TypeCustomField(CustomFieldName, "Make a donation?");
            RegisterMgr.Continue();

            // Step #4
            RegisterMgr.SetCustomFieldCheckBox(DonationOptionName.CheckboxItem, true);

            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(
                DonationOptionName.VariableAmountItem, 
                Convert.ToString(DonationOptionPrice.VariableAmountItemMax));

            RegisterMgr.Continue();

            // Step #5
            RegisterMgr.PayMoneyAndVerify(
                DonationOptionPrice.CheckboxItem + DonationOptionPrice.VariableAmountItemMax, 
                ManagerBase.PaymentMethod.Check);

            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("332")]
        public void AdminRegisterDonationForm()
        {
            this.CreateDonationFormForAdminRegister();
            this.ConfirmDisplayOfItemsAndAdminRegister(this.eventId, this.eventSessionId);
            this.ConfirmRegistrationSaved(this.registrationId);
        }

        [Test]
        [Category(Priority.Five)]
        [Description("682")]
        public void PreviewDonationForm()
        {
            this.CreateDonationFormForAdminRegister();

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);
            this.PreviewDonationWithRegType();
            this.PreviewDonationWithOutRegType();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("583")]
        public void CopyDonationForm()
        {
            string copiedEventName_TheFirstTime = EventName + " (Copy)";
            string copiedEventName_TheSecondTime = copiedEventName_TheFirstTime + " (Copy)";
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(copiedEventName_TheFirstTime);
            ManagerSiteMgr.DeleteEventByName(copiedEventName_TheSecondTime);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateDonationForm();
            }

            this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);

            // Copy from event list on manager screen
            ManagerSiteMgr.CopyEventById(eventId);
            ManagerSiteMgr.OpenEventDashboard(copiedEventName_TheFirstTime);

            // Copy from form dashboard
            ManagerSiteMgr.DashboardMgr.CopyEventFromDashboard(copiedEventName_TheSecondTime);
            ManagerSiteMgr.DashboardMgr.ReturnToList();
        }

        private void CreateDonationFormForAdminRegister()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(AdminRegName);
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.DonationForm);
            this.eventId = BuilderMgr.GetEventId();

            //Set start page and add reg types
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.DonationForm, AdminRegName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(donorTypes.RegTypeAdminOnly);
            BuilderMgr.RegTypeMgr.SetVisibilities(false, true, null);
            BuilderMgr.RegTypeMgr.SetFee(donorTypes.RegTypeAdminOnlyFee);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            donorTypes.RegTypeAdminOnlyId = BuilderMgr.Fetch_RegTypeID(eventId, donorTypes.RegTypeAdminOnly);
            
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(donorTypes.RegTypeVisibleToAll);
            BuilderMgr.RegTypeMgr.SetVisibilities(true, true, null);
            BuilderMgr.RegTypeMgr.SetFee(donorTypes.RegTypeVisibleToAllFee);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            donorTypes.RegTypeVisibleToAllId = BuilderMgr.Fetch_RegTypeID(eventId, donorTypes.RegTypeVisibleToAll);

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(donorTypes.RegTypeInvisible);
            BuilderMgr.RegTypeMgr.SetVisibilities(false, false, null);
            BuilderMgr.RegTypeMgr.SetFee(donorTypes.RegTypeInvisibleFee);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            donorTypes.RegTypeInvisibleId = BuilderMgr.Fetch_RegTypeID(eventId, donorTypes.RegTypeInvisible);

            BuilderMgr.Next();

            //Set donor infomation page
            BuilderMgr.ClickAddCustomFieldOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(CFs.CFVisible));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(CFs.CFVisible));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(CFs.CFVisible));
            BuilderMgr.OldAGAndCFMgr.SetFieldVisibility(true);
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.ClickAddCustomFieldOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(CFs.CFInvisible));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(CFs.CFInvisible));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(CFs.CFInvisible));
            BuilderMgr.OldAGAndCFMgr.SetFieldVisibility(false);
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.Next();

            //Set donation options
            BuilderMgr.ClickAddAgendaItemOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(DonationOptions.DonationOptionVisible));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(DonationOptions.DonationOptionVisible));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(DonationOptions.DonationOptionVisible));
            BuilderMgr.OldAGAndCFMgr.SetFieldVisibility(true);
            BuilderMgr.OldAGAndCFMgr.SetRegularPrice((double)(DonationOptions.DonationOptionVisible));
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.ClickAddAgendaItemOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(StringEnum.GetStringValue(DonationOptions.DonationOptionInvisible));
            BuilderMgr.OldAGAndCFMgr.SetTitle(StringEnum.GetStringValue(DonationOptions.DonationOptionInvisible));
            BuilderMgr.OldAGAndCFMgr.SetNameOnReports(StringEnum.GetStringValue(DonationOptions.DonationOptionInvisible));
            BuilderMgr.OldAGAndCFMgr.SetFieldVisibility(false);
            BuilderMgr.OldAGAndCFMgr.SetRegularPrice((double)(DonationOptions.DonationOptionInvisible));
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.Next();

            //Set merchandise tiems
            BuilderMgr.ClickAddMerchandiseItem();
            BuilderMgr.MerchMgr.SetName(StringEnum.GetStringValue(Merchandises.MerchandiseVisible));
            BuilderMgr.MerchMgr.SetFixedPrice((double)(Merchandises.MerchandiseVisible));
            BuilderMgr.MerchMgr.SetMerchVisibility(true);
            BuilderMgr.MerchMgr.SaveAndClose();

            BuilderMgr.ClickAddMerchandiseItem();
            BuilderMgr.MerchMgr.SetName(StringEnum.GetStringValue(Merchandises.MerchandiseInvisible));
            BuilderMgr.MerchMgr.SetFixedPrice((double)(Merchandises.MerchandiseInvisible));
            BuilderMgr.MerchMgr.SetMerchVisibility(false);
            BuilderMgr.MerchMgr.SaveAndClose();

            BuilderMgr.Next();

            //Set Payment method
            BuilderMgr.PaymentMethodMgr.AddPaymentMethod(ManagerBase.PaymentMethod.Check);
            BuilderMgr.PaymentMethodMgr.SetPaymentMethodVisibility(ManagerBase.PaymentMethod.Check, false, true, null);

            BuilderMgr.Next();

            //Set confrimation message
            ////BuilderMgr.AddConfirmationMessage(DonationConfirmationMessage);

            BuilderMgr.SaveAndClose();
        }

        private void ConfirmDisplayOfItemsAndAdminRegister(int eventId, string eventSessionID)
        {
            //Login page
            RegisterMgr.OpenAdminRegisterPage(eventId, eventSessionID);
            RegisterMgr.Checkin();
            Assert.True(RegisterMgr.HasRegType(donorTypes.RegTypeAdminOnlyId));
            Assert.True(RegisterMgr.HasRegType(donorTypes.RegTypeVisibleToAllId));
            Assert.False(RegisterMgr.HasRegType(donorTypes.RegTypeInvisibleId));
            RegisterMgr.SelectRegType(donorTypes.RegTypeAdminOnly);
            totalCharge += donorTypes.RegTypeAdminOnlyFee;
            RegisterMgr.Continue();
            
            //Donor Info page
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFVisible), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFInvisible), false);
            RegisterMgr.EnterProfileInfoWithoutPassword();
            this.registrationId = RegisterMgr.GetRegIdFromSession();
            RegisterMgr.Continue();

            //Donation Options page
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(DonationOptions.DonationOptionVisible), true);
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(DonationOptions.DonationOptionInvisible), false);
            RegisterMgr.SelectAgendaItem(StringEnum.GetStringValue(DonationOptions.DonationOptionVisible));
            totalCharge += (double)(DonationOptions.DonationOptionVisible);
            RegisterMgr.Continue();

            //Merchandise page
            RegisterMgr.VerifyMerchandiseItemPresent(StringEnum.GetStringValue(Merchandises.MerchandiseVisible), true);
            RegisterMgr.VerifyMerchandiseItemPresent(StringEnum.GetStringValue(Merchandises.MerchandiseInvisible), false);
            RegisterMgr.SelectMerchandiseQuantityByName(StringEnum.GetStringValue(Merchandises.MerchandiseVisible), 2);
            totalCharge += (double)(Merchandises.MerchandiseVisible) * 2;
            RegisterMgr.Continue();

            //Check out page
            RegisterMgr.SelectPaymentMethod(ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
        }

        private void ConfirmRegistrationSaved(int registrationID)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, registrationID);
            string total = MoneyTool.FormatMoney(totalCharge);
            BackendMgr.VerifyTotalCharges(total);
            BackendMgr.VerifyTotalTransactions(total);
            BackendMgr.VerifyTotalBalanceDue(total);
        }

        private void PreviewDonationWithRegType()
        {
            ManagerSiteMgr.DashboardMgr.ClickPreviewForm();

            //Start page
            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyHasRegTypeWhenPreview(donorTypes.RegTypeVisibleToAll, true);
            BuilderMgr.SelectRegTypeWhenPreview(donorTypes.RegTypeVisibleToAll);
            BuilderMgr.Next();

            //Donor information page
            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CFs.CFVisible), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CFs.CFInvisible), false);
            BuilderMgr.Next();

            //Donationi options page
            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(DonationOptions.DonationOptionVisible), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(DonationOptions.DonationOptionInvisible), false);
            BuilderMgr.Next();

            //Merchandise page
            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(Merchandises.MerchandiseVisible), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(Merchandises.MerchandiseInvisible), false);
            BuilderMgr.Next();
            BuilderMgr.Next();

            //Confirmation page
            BuilderMgr.SetMobileViewMode(false);
            ////BuilderMgr.VerifyCustomFieldPresentWhenPreview(DonationConfirmationMessage, true);
            BuilderMgr.SaveAndClose();
        }

        private void PreviewDonationWithOutRegType()
        {
            ManagerSiteMgr.DashboardMgr.ClickOption(Managers.Manager.Dashboard.DashboardManager.EventSetupFunction.EditRegForm);
            BuilderMgr.DeleteRegType(donorTypes.RegTypeAdminOnly);
            BuilderMgr.DeleteRegType(donorTypes.RegTypeVisibleToAll);
            BuilderMgr.DeleteRegType(donorTypes.RegTypeInvisible);
            BuilderMgr.TogglePreviewAndEditMode();
            BuilderMgr.SetMobileViewMode(false);
            UIUtilityProvider.UIHelper.SelectPopUpFrameById("ctl00_cph_ifrmPreview");
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format("//*[text()='{0}']", donorTypes.RegTypeAdminOnly), false, UIUtility.LocateBy.XPath);
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format("//*[text()='{0}']", donorTypes.RegTypeVisibleToAll), false, UIUtility.LocateBy.XPath);
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format("//*[text()='{0}']", donorTypes.RegTypeInvisible), false, UIUtility.LocateBy.XPath);
            BuilderMgr.SelectBuilderWindow();
        }

        [Step]
        private void SetStartPage()
        {
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.DonationForm, EventName);
            BuilderMgr.SetRegistrationTarget(5);
            BuilderMgr.SaveAndStay();
        }

        [Step]
        private void SetPIPage()
        {
            BuilderMgr.VerifyPersonalInfoPageDefaults();
            BuilderMgr.ClickAddCustomFieldOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.OneLineText);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(CustomFieldName);
            BuilderMgr.OldAGAndCFMgr.SetLineTextItemLength(177);
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();
            BuilderMgr.SaveAndStay();
        }

        [Step]
        private void SetDonationOptionsPage()
        {
            BuilderMgr.ClickAddAgendaItemOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(DonationOptionName.CheckboxItem);
            BuilderMgr.OldAGAndCFMgr.SetRegularPrice(DonationOptionPrice.CheckboxItem);
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.ClickAddAgendaItemOld();
            BuilderMgr.OldAGAndCFMgr.SelectType(OtherEventTypeAgendaAndCFManager.FieldType.VariableAmount);
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(DonationOptionName.VariableAmountItem);
            BuilderMgr.OldAGAndCFMgr.SetVariableAmount(1, DonationOptionPrice.VariableAmountItemMax);
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();

            BuilderMgr.SaveAndStay();
        }

        [Step]
        private void SetCheckoutPage()
        {
            BuilderMgr.PaymentMethodMgr.AddPaymentMethod(PaymentMethodManager.PaymentMethod.Check);
            BuilderMgr.SaveAndStay();
        }
    }

    public class DonorTyeps
    {
        public  string RegTypeAdminOnly = "RegTypeAdminOnly";
        public string RegTypeVisibleToAll = "RegTypeVisibleToAll";
        public string RegTypeInvisible = "RegTypeInvisible";

        public double RegTypeAdminOnlyFee = 50;
        public double RegTypeVisibleToAllFee = 60;
        public double RegTypeInvisibleFee = 70;

        public int RegTypeAdminOnlyId;
        public int RegTypeVisibleToAllId;
        public int RegTypeInvisibleId;
    }
}
