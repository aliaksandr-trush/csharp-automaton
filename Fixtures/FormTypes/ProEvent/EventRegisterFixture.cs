namespace RegOnline.RegressionTest.Fixtures.FormTypes.ProEvent
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class EventRegisterFixture : FixtureBase
    {
        private const string EventName = "EventRegisterFixture";
        private const int InvalidEventID = FormDetailManager.InvalidId;
        private const double EventFee = 50;
        private const int FiveDaysBefore = -5;
        private const int TenDaysBefore = -10;
        private const string AgendaItemName = "AgendaEarlyLatePriceItem";
        private const double AgendaItemStandardPrice = EventFee;
        private const double AgendaEarlyPrice = EventFee / 2;
        private const double AgendaLatePrice = EventFee * 2;
        private readonly DateTime EarlyDatetime = DateTime.Now.AddDays(TenDaysBefore);
        private readonly DateTime LateDatetime = DateTime.Now.AddDays(FiveDaysBefore);
        private const string MerchItemName = "MerchandiseItemFixedPrice";
        private const int MerchQuantity = 2;
        private const double MerchPrice = EventFee;

        private enum RegType
        {
            Beginner,
            Professional,
            Master
        }

        private enum PICustomField
        {
            Checkbox,
            OneLineText
        }

        private int eventID;
        private string eventSessionID;
        private Dictionary<RegType, double?> regTypeFees;

        public EventRegisterFixture() : base()
        {
            this.regTypeFees = new Dictionary<RegType, double?>();
            this.regTypeFees.Add(RegType.Beginner, null);
            this.regTypeFees.Add(RegType.Professional, EventFee / 2);
            this.regTypeFees.Add(RegType.Master, EventFee);
        }

        [Test]
        [Category(Priority.One)]
        [Description("336")]
        public void AdminReg()
        {
            this.CreateEvent();

            double totalFee = 0;
            RegisterMgr.OpenAdminRegisterPage(this.eventID, this.eventSessionID);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(RegType.Master.ToString());
            string emailAddress = RegisterMgr.CurrentEmail;
            totalFee += EventFee;
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfoWithoutPassword();
            RegisterMgr.EnterPersonalInfoStatus(RegisterManager.RegistrationStatus.Approved);

            RegisterMgr.CurrentRegistrationId = RegisterMgr.GetRegIdFromSession();

            // Filling in Custom Fields
            RegisterMgr.SelectPersonalInfoCustomFields();
            RegisterMgr.Continue();

            // Selecting Agenda Item
            RegisterMgr.SelectAgendaItem(AgendaItemName);

            // Chosing alternate price
            RegisterMgr.SelectPricingSchedule(AgendaItemName, RegisterManager.PricingSchedule.Early);
            totalFee += AgendaEarlyPrice;

            RegisterMgr.FillCustomFieldResponseList(
                RegisterMgr.CurrentRegistrationId,
                RegisterMgr.customFieldResponses,
                RegisterMgr.GetCustomFieldIDForCheckboxItem(AgendaItemName),
                AgendaItemName,
                2,
                AgendaItemName,
                RegisterMgr.GetPriceForPricingSchedule(RegisterMgr.GetCustomFieldIDForCheckboxItem(AgendaItemName), RegisterManager.PricingSchedule.Early).ToString(),
                null);

            RegisterMgr.Continue();

            RegisterMgr.SelectMerchandise(MerchQuantity);
            totalFee += EventFee * MerchQuantity;
            RegisterMgr.Continue();

            RegisterMgr.SelectPaymentMethod(RegisterManager.PaymentMethod.Check);
            RegisterMgr.PaymentMgr.ClickApplyPaymentNow();
            RegisterMgr.PaymentMgr.EnterCheckNumber("7451");
            RegisterMgr.VerifyCheckoutTotal(totalFee);
            
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            string status = Convert.ToString(
                RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.Status));

            VerifyTool.VerifyValue(
                StringEnum.GetStringValue(RegisterManager.RegistrationStatus.Approved),
                status,
                "RegistrationStatus on confirmation page: {0}");

            this.VerifyAttendee(RegisterMgr.CurrentRegistrationId, true, true);

            string total = MoneyTool.FormatMoney(totalFee);

            // Balance due should be 0
            string zeroAmount = MoneyTool.FormatMoney(0);

            BackendMgr.VerifyTotalCharges(total);
            BackendMgr.VerifyTotalTransactions(zeroAmount);
            BackendMgr.VerifyTotalBalanceDue(zeroAmount);
        }

        [Test]
        [Category(Priority.One)]
        [Description("337")]
        public void GroupReg()
        {
            double totalFee = 0;
            this.CreateEvent();
            RegisterMgr.CurrentEventId = this.eventID;
            RegisterMgr.OpenRegisterPage();
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(RegType.Beginner.ToString());
            string emailAddressOne = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();

            int registerIDOne = RegisterMgr.GetRegIdFromSession();
            RegisterMgr.CurrentRegistrationId = registerIDOne;

            // Filling in or selecting Custom Fields
            RegisterMgr.SelectPersonalInfoCustomFields();
            RegisterMgr.Continue();
            
            // Select Agenda Items
            RegisterMgr.SelectAgendaItems();
            totalFee += AgendaLatePrice;

            string emailAddressTwo = string.Empty;
            int registerIDTwo = RegisterManager.InvalidId;
            
            this.AddAnotherPerson(RegType.Professional, ref emailAddressTwo, ref registerIDTwo);
            totalFee += this.regTypeFees[RegType.Professional].Value + AgendaLatePrice;

            string emailAddressThree = string.Empty;
            int registerIDThree = RegisterManager.InvalidId;

            this.AddAnotherPerson(RegType.Master, ref emailAddressThree, ref registerIDThree);
            totalFee += this.regTypeFees[RegType.Master].Value + AgendaLatePrice;
            
            RegisterMgr.VerifyHasAddAnotherPersonButton(true);
            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.GoBackToPreviousPage();

            string emailAddressFour = string.Empty;
            int registerIDFour = RegisterManager.InvalidId;
            
            this.AddAnotherPerson(RegType.Master, ref emailAddressFour, ref registerIDFour);
            totalFee += this.regTypeFees[RegType.Master].Value + AgendaLatePrice;
            
            RegisterMgr.Continue();
            RegisterMgr.CurrentRegistrationId = registerIDOne;
            RegisterMgr.SelectMerchandise(MerchQuantity);
            totalFee += MerchPrice * MerchQuantity;
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            
            // Unfolding the Reg Details of group members
            RegisterMgr.UnfoldGroupMember(0);
            RegisterMgr.UnfoldGroupMember(1);
            RegisterMgr.UnfoldGroupMember(2);

            RegisterMgr.CurrentEmail = emailAddressOne;
            RegisterMgr.ConfirmRegistration();

            //Registration.CloseSeleniumAndReOpenPreviousURL();
            this.VerifyAttendee(registerIDOne, true, true);
            string total = MoneyTool.FormatMoney(totalFee);

            BackendMgr.VerifyTotalCharges(total);
            BackendMgr.VerifyTotalTransactions(total);
            BackendMgr.VerifyTotalBalanceDue(total);

            //Registration.CloseSeleniumAndReOpenPreviousURL();
            this.VerifyAttendee(registerIDTwo, true, false);

            //Registration.CloseSeleniumAndReOpenPreviousURL();
            this.VerifyAttendee(registerIDThree, true, false);

            //Registration.CloseSeleniumAndReOpenPreviousURL();
            this.VerifyAttendee(registerIDFour, true, false);
        }

        [Test]
        [Category(Priority.One)]
        [Description("338")]
        public void UpdateReg()
        {
            this.CreateEvent();
            RegisterMgr.CurrentEventId = this.eventID;
            RegisterMgr.OpenRegisterPage();
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(RegType.Beginner.ToString());
            string emailAddress = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();

            RegisterMgr.CurrentRegistrationId = RegisterMgr.GetRegIdFromSession();

            // Filling in or selecting Custom Fields
            RegisterMgr.SelectPersonalInfoCustomFields();
            RegisterMgr.Continue();

            // Select Agenda Items
            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();

            RegisterMgr.SelectMerchandise(MerchQuantity);
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            this.UpdateRegistration(emailAddress);
            this.VerifyAttendee(RegisterMgr.CurrentRegistrationId, true, true);
            this.UpdateRegistration(emailAddress);
            this.VerifyAttendee(RegisterMgr.CurrentRegistrationId, true, true);
        }

        [Step]
        public void CreateEvent()
        {
            ManagerSiteMgr.OpenLogin();
            this.eventSessionID = ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (ManagerSiteMgr.EventExists(EventName))
            {
                this.eventID = ManagerSiteMgr.GetFirstEventId(EventName);
            }
            else
            {
                ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
                this.SetStartPage();
                BuilderMgr.Next();
                this.SetPIPage();
                BuilderMgr.Next();
                this.SetAgendaPage();
                BuilderMgr.GotoPage(FormDetailManager.Page.Merchandise);
                this.SetMerchandisePage();
                BuilderMgr.Next();
                this.SetCheckoutPage();
                this.eventID = BuilderMgr.GetEventId();
                BuilderMgr.SaveAndClose();
            }
        }

        private void SetStartPage()
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName);
            BuilderMgr.SetEventFee(EventFee);
            BuilderMgr.AddRegTypeWithEventFee(RegType.Beginner.ToString(), this.regTypeFees[RegType.Beginner]);
            BuilderMgr.AddRegTypeWithEventFee(RegType.Professional.ToString(), this.regTypeFees[RegType.Professional]);
            BuilderMgr.AddRegTypeWithEventFee(RegType.Master.ToString(), this.regTypeFees[RegType.Master]);
            BuilderMgr.SaveAndStay();
        }

        private void SetPIPage()
        {
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.CheckBox, PICustomField.Checkbox.ToString());
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.OneLineText, PICustomField.OneLineText.ToString());
            BuilderMgr.SaveAndStay();
        }

        private void SetAgendaPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.VerifyEventAgendaPage();

            BuilderMgr.ClickAddAgendaItem();

            BuilderMgr.AGMgr.SetName(AgendaItemName);
            BuilderMgr.AGMgr.SetTypeWithDefaults(AgendaItemManager.AgendaItemType.CheckBox);

            BuilderMgr.AGMgr.FeeMgr.ExpandOption();
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(AgendaItemStandardPrice);

            BuilderMgr.AGMgr.FeeMgr.Pricing.SetEarlyPricingSchedule(AgendaEarlyPrice, EarlyDatetime);

            BuilderMgr.AGMgr.FeeMgr.Pricing.SetLatePricingSchedule(AgendaLatePrice, LateDatetime);

            BuilderMgr.AGMgr.ClickSaveItem();

            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("AI-Always");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.AlwaysSelected);
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(0);
            BuilderMgr.AGMgr.FeeMgr.ExpandOption();
            BuilderMgr.AGMgr.FeeMgr.Tax.ClickAddTaxRatesLink();
            BuilderMgr.AGMgr.FeeMgr.Tax.SetTaxRateOne("tax1", 10);
            BuilderMgr.AGMgr.FeeMgr.Tax.SetTaxRateTwo("tax2", 20);
            BuilderMgr.AGMgr.FeeMgr.Tax.SaveAndClose();
            BuilderMgr.AGMgr.FeeMgr.ExpandOption();
            BuilderMgr.AGMgr.FeeMgr.Tax.ApplyTaxOneToFee(true);
            BuilderMgr.AGMgr.FeeMgr.Tax.ApplyTaxTwoToFee(true);
            BuilderMgr.AGMgr.ClickSaveItem();

            BuilderMgr.SaveAndStay();
        }

        private void SetMerchandisePage()
        {
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Fixed, MerchItemName, MerchPrice, null, null);
            BuilderMgr.SaveAndStay();
        }

        private void SetCheckoutPage()
        {
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndStay();
        }

        [Verify]
        private void VerifyAttendee(int registerID, bool verifyCF, bool verifyMerch)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionID = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionID, registerID);

            if (verifyCF)
            {
                BackendMgr.VerifyCustomFields(RegisterMgr.customFieldResponses, registerID);
                BackendMgr.VerifyAgendaItems(RegisterMgr.customFieldResponses, registerID);
            }

            if (verifyMerch)
            {
                BackendMgr.VerifyMerchandise(RegisterMgr.merchandiseResponses, registerID);
            }
        }

        [Step]
        private void AddAnotherPerson(RegType regType, ref string email, ref int regID)
        {
            RegisterMgr.VerifyHasAddAnotherPersonButton(true);
            RegisterMgr.ClickAddAnotherPerson();
            
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(regType.ToString());
            email = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();

            regID = RegisterMgr.GetRegIdFromSession();
            RegisterMgr.CurrentRegistrationId = regID;

            // Filling in or selecting Custom Fields
            RegisterMgr.SelectPersonalInfoCustomFields();
            RegisterMgr.Continue();

            // Select Agenda Items
            RegisterMgr.SelectAgendaItems();
        }

        [Step]
        private void UpdateRegistration(string emailAddress)
        {
            string newPICFText = "Text changed!";
            int newMerchQuantity = 1;

            RegisterMgr.OpenRegisterPage();
            RegisterMgr.CheckinWithEmail(emailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterPassword(ConfigurationProvider.XmlConfig.AccountConfiguration.Password);
            RegisterMgr.Continue();

            RegisterMgr.ClickEditPersonalInformationLink(0);
            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry("Freelance Journalist", null, null, null);
            RegisterMgr.EnterPersonalInfoAddress(null, null, null, "Maine", null, null);
            RegisterMgr.EnterPersonalInfoPhoneNumbers(null, "None", null, null, null);

            // Make some changes to PI custom fields
            RegisterMgr.SetCustomFieldCheckbox(PICustomField.Checkbox.ToString(), false);

            RegisterMgr.customFieldResponses.Find(s => s.RegistrationID == RegisterMgr.CurrentRegistrationId).customFields.Remove(
                RegisterMgr.GetCustomFieldIDForCheckboxItem(PICustomField.Checkbox.ToString()));

            RegisterMgr.TypeCustomField(PICustomField.OneLineText.ToString(), newPICFText);

            RegisterMgr.customFieldResponses.Find(
                s => s.RegistrationID == RegisterMgr.CurrentRegistrationId
                ).customFields[RegisterMgr.GetCustomFieldIDForCheckboxItem(PICustomField.OneLineText.ToString())].response = newPICFText;

            RegisterMgr.Continue();
            RegisterMgr.AttendeeCheckEditAgenda();

            // selecting Agenda Items
            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();
            RegisterMgr.Continue();

            RegisterMgr.SelectMerchandise(newMerchQuantity);

            RegisterManager.MerchandiseResponse merchResponse = 
                RegisterMgr.merchandiseResponses.Find(
                s => s.RegistrationID == RegisterMgr.CurrentRegistrationId
                ).merchandises[RegisterMgr.GetCustomFieldIDForCheckboxItem(MerchItemName)];

            merchResponse.response = newMerchQuantity.ToString();

            RegisterMgr.Continue();

            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
    }
}
