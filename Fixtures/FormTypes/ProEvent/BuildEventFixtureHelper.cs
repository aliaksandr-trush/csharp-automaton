namespace RegOnline.RegressionTest.Fixtures.FormTypes.ProEvent
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Fixtures.Base;

    public class BuildEventFixtureHelper : FixtureBase
    {
        public void VerifyEvent(string eventName, out int eventID)
        {
            VerifyStartPage(eventName, out eventID);
            BuilderMgr.Next();
            VerifyPersonalInfoPage();
            BuilderMgr.Next();
            VerifyAgedaPage();
            BuilderMgr.Next();
            VerifyLAndTPage();
            BuilderMgr.Next();
            VerifyMerchPage();
            BuilderMgr.Next();
            VerifyCheckoutPage();
            BuilderMgr.Next();
            VerifyConfirmationPage();
            BuilderMgr.SaveAndClose();
        }

        public void VerifyStartPage(string eventName, out int eventID)
        {
            BuilderMgr.SetEventNameAndShortcut(eventName);
            BuilderMgr.SaveAndStay();
            BuilderMgr.SetStartEndDateTimeDefault();
            BuilderMgr.SaveAndStay();
            eventID = BuilderMgr.GetEventId();
            BuilderMgr.VerifyStartPageSettingsAreSaved(ManagerSiteManager.EventType.ProEvent, eventName);
            BuilderMgr.VerifyHasRegTypeInDatabase("First");
            BuilderMgr.VerifyHasRegTypeInDatabase("Second");
            BuilderMgr.VerifyHasRegTypeInDatabase("Third");
            BuilderMgr.VerifyHasRegTypeInDatabase("Fourth");
        }

        public void VerifyPersonalInfoPage()
        {
            BuilderMgr.VerifyPersonalInfoPageSettingsAreSaved();

            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.CheckBox, "CF-Checkbox");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.RadioButton, "CF-Radio");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Dropdown, "CF-DropDown");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Number, "CF-Numeric");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.OneLineText, "CF-Text");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Time, "CF-Time");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.SectionHeader, "CF-Header");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.AlwaysSelected, "CF-Always");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.ContinueButton, "CF-Continue");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Paragraph, "CF-Paragraph");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Date, "CF-Date");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.FileUpload, "CF-File");

            //BuilderMgr.fie
        }

        public void VerifyAgedaPage()
        {
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.CheckBox, "AI-Checkbox");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.RadioButton, "AI-Radio");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Dropdown, "AI-DropDown");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Number, "AI-Numeric");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.OneLineText, "AI-Text");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Time, "AI-Time");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.SectionHeader, "AI-Header");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.AlwaysSelected, "AI-Always");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.ContinueButton, "AI-Continue");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Paragraph, "AI-Paragraph");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Date, "AI-Date");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.FileUpload, "AI-File");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Contribution, "AI-Contribution");

            BuilderMgr.VerifyFormView();
        }

        public void VerifyLAndTPage()
        {
            BuilderMgr.VerifyEventLodgingTravelPage();

            // verify lodging custom fields of each type
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.CheckBox, "LDG-Checkbox");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.RadioButton, "LDG-Radio");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Dropdown, "LDG-DropDown");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Number, "LDG-Numeric");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.OneLineText, "LDG-Text");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Time, "LDG-Time");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.SectionHeader, "LDG-Header");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.AlwaysSelected, "LDG-Always");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.ContinueButton, "LDG-Continue");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Paragraph, "LDG-Paragraph");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Date, "LDG-Date");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.FileUpload, "LDG-File");

            // verify travel custom fields of each type
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.CheckBox, "TRV-Checkbox");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.RadioButton, "TRV-Radio");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Dropdown, "TRV-DropDown");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Number, "TRV-Numeric");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.OneLineText, "TRV-Text");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Time, "TRV-Time");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.SectionHeader, "TRV-Header");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.AlwaysSelected, "TRV-Always");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.ContinueButton, "TRV-Continue");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Paragraph, "TRV-Paragraph");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Date, "TRV-Date");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.FileUpload, "TRV-File");
        }

        public void VerifyMerchPage()
        {
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Header, "FEE-Header");
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Fixed, "FEE-Fixed");
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Variable, "FEE-Variable");
        }

        public void VerifyCheckoutPage()
        {
            BuilderMgr.VerifyEventCheckoutPage();
        }

        public void VerifyConfirmationPage()
        {
            BuilderMgr.VerifyEventConfirmationPage();
        }

        public void Checkin(int id)
        {
            RegisterMgr.OpenRegisterPage(id);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType("First");
            RegisterMgr.Continue();
        }

        public void PersonalInfo()
        {
            RegisterMgr.EnterPersonalInfoNamePrefixSuffix("Mr.", "Test", "Test", "Test", "Esq.");
            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry("test", "Test", "Test", "United States");
            RegisterMgr.EnterPersonalInfoAddress("Test", "Test", "Test", "Colorado", null, "80304");
            RegisterMgr.EnterPersonalInfoPhoneNumbers("Test", "Test", "Test", "Test", "Test");
            RegisterMgr.EnterPersonalInfoDateOfBirthGender(new DateTime(1986, 2, 18), RegisterManager.Gender.Male);
            RegisterMgr.EnterPersonalInfoTaxNumberMembershipNumberCustomerNumber("1234", "4321", "1111");
            RegisterMgr.TypePersonalInfoPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
            RegisterMgr.TypePersonalInfoVerifyPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
            RegisterMgr.SetCustomFieldCheckBox("CF-Checkbox", true);
            RegisterMgr.SelectCustomFieldRadioButtons("CF-Radio", "Yes");
            RegisterMgr.SelectCustomFieldDropDown("CF-DropDown", "Agree");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("CF-Text", "Test");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("CF-Numeric", "1234");
            RegisterMgr.FillOutCustomTimeField("CF-Time", "4:00 AM");
            RegisterMgr.FillOutCustomDateField("CF-Date", "04/15/1996");
            RegisterMgr.FillOutCustomParagraphField("CF-Paragraph", "Testing");
            List<string> custFields = RegisterMgr.GetCustomFieldNames();
            Assert.IsTrue(custFields.Count == 12);
            RegisterMgr.Continue();
        }

        public void Agenda()
        {
            RegisterMgr.VerifyCustomFieldPresent("AI-Checkbox", true);
            RegisterMgr.SetCustomFieldCheckBox("AI-Checkbox", true);
            RegisterMgr.VerifyCustomFieldPresent("AI-Radio", true);
            RegisterMgr.VerifyCustomFieldPresent("AI-DropDown", true);
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("AI-Text", "Test");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("AI-Numeric", "1234");
            RegisterMgr.FillOutCustomTimeField("AI-Time", "4:00 AM");
            RegisterMgr.FillOutCustomParagraphField("AI-Paragraph", "Testing");
            RegisterMgr.FillOutCustomDateField("AI-Date", "04/15/1996");
            RegisterMgr.Continue();
            RegisterMgr.CheckErrorMessage(Managers.Register.RegisterManager.Error.OverLappingAgendaItems);
            RegisterMgr.SetCustomFieldCheckBox("AI-Checkbox", false);
            List<int> custFields = RegisterMgr.Fetch_AgendaIdList();
            Assert.IsTrue(custFields.Count == 13);

            RegisterMgr.Continue();
        }

        public void SetupCustomerAndEventForActivation(string currencyName)
        {
            ManagerSiteMgr.OpenLogin();
            string username = System.Guid.NewGuid().ToString();
            username = username.Replace("-", "");
            ManagerSiteMgr.CreateNewAccount(username, ConfigReader.DefaultProvider.AccountConfiguration.Password, currencyName);

            string eventTitle = "Account Currency? " + currencyName;
            BuilderMgr.SetEventNameAndShortcut(eventTitle);
            BuilderMgr.SaveAndClose();
            int newCustomerId = ManagerSiteMgr.GetCurrentAccountId();

            //C.Customer c = new C.Customer(newCustomerId);
            //c.HasACHBillingEnabled = true;//we don't have the admin tool regression yet.
            //Remember btw to add this UpdateHasACHBillingEnabled to that admin tool:
            //NewCustomerDb . BillingAdmin . lbSaveACH_Click.

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login(username, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            
            // Deal with the 'Validate Email' popup
            ManagerSiteMgr.SkipEmailValidation();

            int eventID = ManagerSiteMgr.GetFirstEventId(eventTitle);

            //partial EventActivation()
            ManagerSiteMgr.OpenEventDashboard(eventID);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
        }

        public void LAndT()
        {
            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.FillOutCheckInOutDates("7/31/2020", "8/1/2020");
            RegisterMgr.SelectRoomPreference(RegisterManager.RoomPreference.KingSuite);
            RegisterMgr.SelectBedPreference(RegisterManager.BedPreference.King);
            RegisterMgr.SelectSmokingPreference(RegisterManager.SmokingPreference.Smoking);
            RegisterMgr.SetCustomFieldCheckBox("LDG-Checkbox", true);
            RegisterMgr.SelectCustomFieldRadioButtons("LDG-Radio", "Yes");
            RegisterMgr.SelectCustomFieldDropDown("LDG-DropDown", "Agree");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("LDG-Text", "Test");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("LDG-Numeric", "1234");
            RegisterMgr.FillOutCustomTimeField("LDG-Time", "4:00 AM");
            ////RegisterMgr.FillOutCustomParagraphField("LDG-Paragraph", "Testing");
            ////RegisterMgr.FillOutCustomDateField("LDG-Date", "04/15/1996");
            RegisterMgr.FillOutSharingWith("Test Test");
            RegisterMgr.FillOutAdjoiningWith("Tester Tester");
            RegisterMgr.FillOutLodgingAdditionalInfo("Loding Additional Info");
            RegisterMgr.FillOutLodgingCCInfo_Default();
            RegisterMgr.EnterTravelInfo();
            RegisterMgr.FillOutTravelCCInfo_Default();
            ////RegisterMgr.SetCustomFieldCheckBox("TRV-Checkbox", true);
            ////RegisterMgr.SelectCustomFieldRadioButtons("TRV-Radio", "Yes");
            ////RegisterMgr.SelectCustomFieldDropDown("TRV-DropDown", "Agree");
            ////RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("TRV-Text", "Test");
            ////RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("TRV-Numeric", "1234");
            ////RegisterMgr.FillOutCustomTimeField("TRV-Time", "4:00 AM");
            RegisterMgr.FillOutCustomParagraphField("TRV-Paragraph", "Testing");
            RegisterMgr.FillOutCustomDateField("TRV-Date", "04/15/1996");
            List<string> custFields = RegisterMgr.GetLAndTCustomFieldNames(RegisterManager.Section.Lodging);
            Assert.IsTrue(custFields.Count == 12);
            custFields = RegisterMgr.GetLAndTCustomFieldNames(RegisterManager.Section.Travel);
            Assert.IsTrue(custFields.Count == 12);
            RegisterMgr.Continue();
        }

        public void Merchandise()
        {
            RegisterMgr.SelectMerchandiseQuantityByName("FEE-Fixed", 1);
            RegisterMgr.EnterMerchandiseVariableAmountByName("FEE-Variable", 75.00);
            RegisterMgr.Continue();
        }

        public void Finish()
        {
            RegisterMgr.PayMoney(RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        public void UpdateRegistration(string emailAddr)
        {
            RegisterMgr.EnterEmailAddress(emailAddr);
            RegisterMgr.Continue();
            RegisterMgr.EnterPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
            RegisterMgr.Continue();
            RegisterMgr.OnAttendeeCheckPage();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
    }
}
