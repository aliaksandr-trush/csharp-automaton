namespace RegOnline.RegressionTest.Fixtures.SpecialOptions
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class PrePopulateFixture : FixtureBase
    {
        private int eventID;
        private const string PrePopulateEventName = "PrePopulateEvent";
        private const string PreFillEventName = "PreFillEvent";
        private const string OneLineText = "OneLine";
        private const string OneLineUpdateText = "lineone";
        private const string NumberText = "1234";
        private const string NumberUpdateText = "4321";
        private const string ParagraphText = "Paragraph Text";
        private const string ParagraphUpdateText = "Text Paragraph";
        private const string RadioAgree = "Agree";
        private const string RadioFour = "4";
        private const string DropDisagree = "Disagree";
        private const string DropSix = "6";
        private const string PIPage = "PI";
        private const string AGPage = "AG";
        private const string LTPage = "LT";
        private const string Pop = "Pop";
        private const string PopNoGroup = "PopNoGroup";
        private const string NoPop = "NoPop";
        private const string CheckboxFormat = "{0}{1}Check";
        private const string DropDownFormat = "{0}{1}Drop";
        private const string RadioFormat = "{0}{1}Radio";
        private const string NumberFormat = "{0}{1}Number";
        private const string OneLineFormat = "{0}{1}OneLine";
        private const string ParagraphFormat = "{0}{1}Para";
        private const string DateFormat = "{0}{1}Date";
        private const string TimeFormat = "{0}{1}Time";
        private string Checkbox;
        private string DropDown;
        private string Radio;
        private string Number;
        private string OneLine;
        private string Paragraph;
        private string Date;
        private string Time;

        [Test]
        [Category(Priority.Three)]
        [Description("864")]
        public void PrePopulateGroupCustomFields()
        {
            CreateEventIfNeeded();
            GroupRegistration();   
            
            //TODO: add verification to the second registration updates, make sure custom field reponses are correct!
        }

        /*[Test]
        [Ignore]
        public void TestPreFill()
        {
            CreatePreFillEventIfNeeded();
            RegisterWithPreFill();
        }*/

        [Step]
        private void CreateEventIfNeeded()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            if (ManagerSiteMgr.EventExists(PrePopulateEventName))
            {
                this.eventID = ManagerSiteMgr.GetFirstEventId(PrePopulateEventName);
            }
            else
            {
                ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
                this.CreateEvent();
            }
        }

        [Step]
        private void CreatePreFillEventIfNeeded()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            if (ManagerSiteMgr.EventExists(PreFillEventName))
            {
                eventID = ManagerSiteMgr.GetFirstEventId(PreFillEventName);
            }
            else
            {
                ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
                CreatePreFillEvent();
                ManagerSiteMgr.GoToEventsTabIfNeeded();
                ManagerSiteMgr.SelectFolder();
                eventID = ManagerSiteMgr.GetFirstEventId(PreFillEventName);
            }
        }

        [Step]
        private void GroupRegistration()
        {
            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            //RegisterCustomFields(PIPage, Pop, RadioAgree, DropDisagree);
            RegisterCustomFields(PIPage, PopNoGroup, RadioAgree, DropDisagree);
            RegisterCustomFields(PIPage, NoPop, RadioAgree, DropDisagree);
            RegisterMgr.Continue();
            //RegisterCustomFields(AGPage, Pop, RadioAgree, DropDisagree);
            RegisterCustomFields(AGPage, PopNoGroup, RadioAgree, DropDisagree);
            RegisterCustomFields(AGPage, NoPop, RadioAgree, DropDisagree);
            RegisterMgr.Continue();
            //RegisterCustomFields(LTPage, Pop, RadioFour, DropSix);
            RegisterCustomFields(LTPage, PopNoGroup, RadioFour, DropSix);
            RegisterCustomFields(LTPage, NoPop, RadioFour, DropSix);
            RegisterMgr.ClickAddAnotherPerson(); 
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.GenerateCurrentRegistrantLastName();
            RegisterMgr.TypePersonalInfoFirstName(RegisterManager.DefaultPersonalInfo.FirstName);
            RegisterMgr.TypePersonalInfoMiddleName(RegisterManager.DefaultPersonalInfo.MiddleName);
            RegisterMgr.TypePersonalInfoLastName(RegisterMgr.CurrentRegistrantLastName);
            //VerifySecondRegCustomFields(PIPage, Pop, RadioAgree, DropDisagree, true, true);
            VerifySecondRegCustomFields(PIPage, PopNoGroup, RadioAgree, DropDisagree, true, false);
            VerifyCustomFieldsNotPrePopulated(PIPage, NoPop, RadioAgree);
            //UpdateCustomFields(PIPage, Pop, RadioAgree, DropDisagree);
            UpdateCustomFields(PIPage, NoPop, RadioAgree, DropDisagree);

            RegisterMgr.Continue();
            //VerifySecondRegCustomFields(AGPage, Pop, RadioAgree, DropDisagree, true, true);
            VerifySecondRegCustomFields(AGPage, PopNoGroup, RadioAgree, DropDisagree, true, false);
            VerifyCustomFieldsNotPrePopulated(AGPage, NoPop, RadioAgree);
            //UpdateCustomFields(AGPage, Pop, RadioAgree, DropDisagree);
            UpdateCustomFields(AGPage, NoPop, RadioAgree, DropDisagree);

            RegisterMgr.Continue();
            //VerifySecondRegCustomFields(LTPage, Pop, RadioFour, DropSix, true, true);
            VerifySecondRegCustomFields(LTPage, PopNoGroup, RadioFour, DropSix, true, false);
            VerifyCustomFieldsNotPrePopulated(LTPage, NoPop, RadioFour);
            //UpdateCustomFields(LTPage, Pop, DropSix, RadioFour);
            UpdateCustomFields(LTPage, NoPop, DropSix, RadioFour);

            RegisterMgr.Continue();
           // RegisterMgr.SelectPaymentMethod(ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
        }

        [Step]
        private void RegisterWithPreFill()
        {
            // register primary
            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            // personal info
            RegisterMgr.EnterProfileInfo();
            string primaryLastName = RegisterMgr.CurrentRegistrantLastName;
            EnterPIStandardFields();
            EnterCustomFields("PI");
            RegisterMgr.Continue();
            // agenda
            EnterCustomFields("AI");
            RegisterMgr.Continue();
            // LT
            RegisterMgr.EnterLodgingInfo();
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("LDG-Text", "Primary Lodging");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("TRV-Text", "Primary Travel");

            // add a secondary
            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            PreFillSecondary(primaryLastName);

            // checkout
            RegisterMgr.FinishRegistration();
        }

        private void EnterPIStandardFields()
        {
            RegisterMgr.TypePersonalInfoBadge("Primary Badge");
            RegisterMgr.SelectPersonalInfoCountry("United States");
            RegisterMgr.TypePersonalInfoAddressLineTwo("Primary Line2");
            RegisterMgr.TypePersonalInfoHomePhone("111.111.1111");
            RegisterMgr.TypePersonalInfoExtension("111");
            RegisterMgr.TypePersonalInfoFax("1.111.111.1111");
            RegisterMgr.TypePersonalInfoCellPhone("111.1111");
            RegisterMgr.TypePersonalInfoDateOfBirth(DateTime.Parse("11/01/1974"));
            RegisterMgr.SelectPersonalInfoGender(RegisterManager.Gender.Male);
            RegisterMgr.TypePersonalInfoTaxNumber("11223344");
            RegisterMgr.TypePersonalInfoCustomerNumber("1234");
            RegisterMgr.TypePersonalInfoSocialSecurityNumber("111-22-3344");
            RegisterMgr.TypePersonalInfoContactName("Primary Contact");
            RegisterMgr.TypePersonalInfoContactPhone("111.22.3333");
        }

        private void EnterCustomFields(string page)
        {
            RegisterMgr.SetCustomFieldCheckBox(page + "-Checkbox", true);
            RegisterMgr.SelectCustomFieldRadioButtons(page + "-Radio", "Yes");
            RegisterMgr.SelectCustomFieldDropDown(page + "-DropDown", "Agree");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(page + "-Numeric", "1.11");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(page + "-Text", "Primary Text");
            RegisterMgr.FillOutCustomTimeField(page + "-Time", "5:25 PM");
            RegisterMgr.FillOutCustomParagraphField(page + "-Paragraph", "Primary Paragraph");
            RegisterMgr.FillOutCustomDateField(page + "-Date", "10/25/2011");
        }

        private void VerifyCustomFields(string page)
        {
            RegisterMgr.VerifyCustomFieldCheckBox(page + "-Checkbox", true, true);
            RegisterMgr.VerifyCustomFieldRadioButtons(page + "-Radio", "Yes", true, true);
            RegisterMgr.VerifyCustomFieldDropDown(page + "-DropDown", "Agree", true);
            RegisterMgr.VerifyCustomOneLineTextOrNumberOrContribution(page + "-Numeric", "1.11", true);
            RegisterMgr.VerifyCustomOneLineTextOrNumberOrContribution(page + "-Text", "Primary Text", true);
            RegisterMgr.VerifyCustomTimeField(page + "-Time", "5:25 PM", true);
            RegisterMgr.VerifyCustomParagraphField(page + "-Paragraph", "Primary Paragraph", true);
            RegisterMgr.VerifyCustomDateField(page + "-Date", "10/25/2011", true);
        }

        private void PreFillSecondary(string primaryLastName)
        {
            string primaryName = RegisterManager.DefaultPersonalInfo.FirstName + ' ' + primaryLastName;

            // enter name fields
            RegisterMgr.GenerateCurrentRegistrantLastName();
            RegisterMgr.TypePersonalInfoFirstName(RegisterManager.DefaultPersonalInfo.FirstName);
            RegisterMgr.TypePersonalInfoMiddleName(RegisterManager.DefaultPersonalInfo.MiddleName);
            RegisterMgr.TypePersonalInfoLastName(RegisterMgr.CurrentRegistrantLastName);
            // enter a field that is not pre-filled
            RegisterMgr.TypePersonalInfoJobTitle("Secondary Title");
            // enter a field that is pre-filled
            RegisterMgr.TypePersonalInfoCompany("Secondary Company");
            // select the primary in the pre-fill drop down
            RegisterMgr.SelectPreFillDropDown(primaryName);
            // check results
            //   last name and title should remain unchanged
            RegisterMgr.VerifyPersonalInfoLastName(RegisterMgr.CurrentRegistrantLastName);
            RegisterMgr.VerifyPersonalInfoJobTitle("Secondary Title");
            //   company should change to primary's entry
            RegisterMgr.VerifyPersonalInfoCompany(RegisterManager.DefaultPersonalInfo.Company);
            VerifyCustomFields("PI");

            // agenda
            RegisterMgr.Continue();
            RegisterMgr.SelectPreFillDropDown(primaryName);
            // check results
            VerifyCustomFields("AI");

            // LT
            RegisterMgr.Continue();
            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.SelectBedPreference(RegisterManager.BedPreference.Queen);
            RegisterMgr.SelectPreFillDropDown(primaryName);
            // check results
            RegisterMgr.VerifyBedPreference(RegisterManager.BedPreference.King);
            RegisterMgr.VerifyCustomOneLineTextOrNumberOrContribution("LDG-Text", "Primary Lodging", true);
            RegisterMgr.VerifyCustomOneLineTextOrNumberOrContribution("TRV-Text", "Primary Travel", true);
            RegisterMgr.Continue();
        }

        private void RegisterCustomFields(string page, string pop, string radioChoice, string dropChoice)
        {
            SetStrings(page, pop);
            RegisterMgr.SetCustomFieldCheckBox(Checkbox, true);
            RegisterMgr.SelectCustomFieldRadioButtons(Radio, radioChoice);
            RegisterMgr.SelectCustomFieldDropDown(DropDown, dropChoice);
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(Number, NumberText);
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(OneLine, OneLineText);
            RegisterMgr.FillOutCustomParagraphField(Paragraph,ParagraphText);
            RegisterMgr.FillOutCustomDateField(Date,"10/25/2011");
            RegisterMgr.FillOutCustomTimeField(Time,"5:25 PM");
        }

        private void UpdateCustomFields(string page, string pop, string radioChoice, string dropChoice)
        {
            SetStrings(page, pop);
            RegisterMgr.SetCustomFieldCheckBox(Checkbox, true);
            RegisterMgr.SelectCustomFieldRadioButtons(Radio, dropChoice);
            RegisterMgr.SelectCustomFieldDropDown(DropDown, radioChoice);
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(Number, NumberUpdateText);
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(OneLine, OneLineUpdateText);
            RegisterMgr.FillOutCustomParagraphField(Paragraph, ParagraphUpdateText);
            RegisterMgr.FillOutCustomDateField(Date, "10/24/2011");
            RegisterMgr.FillOutCustomTimeField(Time, "5:25 AM");
        }

        private void VerifySecondRegCustomFields(string page, string pop, string radioChoice, string dropChoice, bool selected, bool editable)
        {
            SetStrings(page, pop);
            RegisterMgr.VerifyCustomFieldCheckBox(Checkbox, selected, editable);
            RegisterMgr.VerifyCustomFieldRadioButtons(Radio, radioChoice, selected, editable);
            RegisterMgr.VerifyCustomFieldDropDown(DropDown, dropChoice, editable);
            RegisterMgr.VerifyCustomOneLineTextOrNumberOrContribution(Number, NumberText, editable);
            RegisterMgr.VerifyCustomOneLineTextOrNumberOrContribution(OneLine, OneLineText, editable);
            RegisterMgr.VerifyCustomParagraphField(Paragraph, ParagraphText, editable);
            RegisterMgr.VerifyCustomDateField(Date, "10/25/2011", editable);
            RegisterMgr.VerifyCustomTimeField(Time, "5:25 PM", editable);
        }

        private void VerifyCustomFieldsNotPrePopulated(string page, string pop, string radioChoice)
        {
            SetStrings(page, pop);
            RegisterMgr.VerifyCustomFieldCheckBox(Checkbox, false, true);
            RegisterMgr.VerifyCustomFieldRadioButtons(Radio, radioChoice, false, true);
            RegisterMgr.VerifyCustomFieldDropDown(DropDown, "", true);
            RegisterMgr.VerifyCustomOneLineTextOrNumberOrContribution(Number, "", true);
            RegisterMgr.VerifyCustomOneLineTextOrNumberOrContribution(OneLine, "", true);
            RegisterMgr.VerifyCustomParagraphField(Paragraph, "", true);
            RegisterMgr.VerifyCustomDateField(Date, "mm/dd/yyyy", true);
            RegisterMgr.VerifyCustomTimeField(Time, "hh:mm AM/PM", true);
        }

        [Step]
        private void CreateEvent()
        {
            SetStartPage();
            SetPIPage();
            SetAgendaPage();
            SetLodgingPage();
            SetCheckOutPage();
        }

        private void CreatePreFillEvent()
        {
            SetStartPage(PreFillEventName);
            SetPreFillPIPage();
            SetPreFillAgendaPage();
            SetPreFillLodgingPage();
            SetCheckOutPage();
        }

        private void SetStartPage(string eventName = null)
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent,
                eventName ?? PrePopulateEventName);
            BuilderMgr.Next();
        }

        private void SetPIPage()
        {
            BuilderMgr.VerifyPersonalInfoPageDefaults();
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.CheckBox, "PopPICheck", true, true);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.RadioButton, "PopPIRadio", true, true);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Dropdown, "PopPIDrop", true, true);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Number, "PopPINumber", true, true);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.OneLineText, "PopPIOneLine", true, true);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Paragraph, "PopPIPara", true, true);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Date, "PopPIDate", true, true);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Time, "PopPITime", true, true);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.CheckBox, "PopNoGroupPICheck", true, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.RadioButton, "PopNoGroupPIRadio", true, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Dropdown, "PopNoGroupPIDrop", true, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Number, "PopNoGroupPINumber", true, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.OneLineText, "PopNoGroupPIOneLine", true, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Paragraph, "PopNoGroupPIPara", true, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Date, "PopNoGroupPIDate", true, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Time, "PopNoGroupPITime", true, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.CheckBox, "NoPopPICheck", false, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.RadioButton, "NoPopPIRadio", false, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Dropdown, "NoPopPIDrop", false, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Number, "NoPopPINumber", false, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.OneLineText, "NoPopPIOneLine", false, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Paragraph, "NoPopPIPara", false, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Date, "NoPopPIDate", false, false);
            BuilderMgr.ClickAddPICustomField();
            CustomField(CustomFieldManager.CustomFieldType.Time, "NoPopPITime", false, false);
            BuilderMgr.SaveAndStay();
            BuilderMgr.Next();
        }

        private void SetPreFillPIPage()
        {
            // turn on all PI standard fields
            BuilderMgr.VerifyPersonalInfoPageDefaults();
            BuilderMgr.SetPersonalInfoPage();
            BuilderMgr.SaveAndStay();

            // add PI custom fields of each type
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.CheckBox, "PI-Checkbox");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.RadioButton, "PI-Radio");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Dropdown, "PI-DropDown");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Number, "PI-Numeric");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.OneLineText, "PI-Text");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Time, "PI-Time");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.SectionHeader, "PI-Header");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.AlwaysSelected, "PI-Always");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.ContinueButton, "PI-Continue");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Paragraph, "PI-Paragraph");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Date, "PI-Date");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.FileUpload, "PI-File");

            BuilderMgr.Next();
        }

        private void SetAgendaPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            AgendaItem(AgendaItemManager.AgendaItemType.CheckBox, "PopAGCheck", true, true);
            AgendaItem(AgendaItemManager.AgendaItemType.RadioButton, "PopAGRadio", true, true);
            AgendaItem(AgendaItemManager.AgendaItemType.Dropdown, "PopAGDrop", true, true);
            AgendaItem(AgendaItemManager.AgendaItemType.Number, "PopAGNumber", true, true);
            AgendaItem(AgendaItemManager.AgendaItemType.OneLineText, "PopAGOneLine", true, true);
            AgendaItem(AgendaItemManager.AgendaItemType.Paragraph, "PopAGPara", true, true);
            AgendaItem(AgendaItemManager.AgendaItemType.Date, "PopAGDate", true, true);
            AgendaItem(AgendaItemManager.AgendaItemType.Time, "PopAGTime", true, true);
            AgendaItem(AgendaItemManager.AgendaItemType.CheckBox, "PopNoGroupAGCheck", true, false);
            AgendaItem(AgendaItemManager.AgendaItemType.RadioButton, "PopNoGroupAGRadio", true, false);
            AgendaItem(AgendaItemManager.AgendaItemType.Dropdown, "PopNoGroupAGDrop", true, false);
            AgendaItem(AgendaItemManager.AgendaItemType.Number, "PopNoGroupAGNumber", true, false);
            AgendaItem(AgendaItemManager.AgendaItemType.OneLineText, "PopNoGroupAGOneLine", true, false);
            AgendaItem(AgendaItemManager.AgendaItemType.Paragraph, "PopNoGroupAGPara", true, false);
            AgendaItem(AgendaItemManager.AgendaItemType.Date, "PopNoGroupAGDate", true, false);
            AgendaItem(AgendaItemManager.AgendaItemType.Time, "PopNoGroupAGTime", true, false);
            AgendaItem(AgendaItemManager.AgendaItemType.CheckBox, "NoPopAGCheck", false, false);
            AgendaItem(AgendaItemManager.AgendaItemType.RadioButton, "NoPopAGRadio", false, false);
            AgendaItem(AgendaItemManager.AgendaItemType.Dropdown, "NoPopAGDrop", false, false);
            AgendaItem(AgendaItemManager.AgendaItemType.Number, "NoPopAGNumber", false, false);
            AgendaItem(AgendaItemManager.AgendaItemType.OneLineText, "NoPopAGOneLine", false, false);
            AgendaItem(AgendaItemManager.AgendaItemType.Paragraph, "NoPopAGPara", false, false);
            AgendaItem(AgendaItemManager.AgendaItemType.Date, "NoPopAGDate", false, false);
            AgendaItem(AgendaItemManager.AgendaItemType.Time, "NoPopAGTime", false, false);
            BuilderMgr.SaveAndStay();
            BuilderMgr.Next();
        }

        private void SetPreFillAgendaPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();

            // add agenda items of each type
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.CheckBox,
                "AI-Checkbox");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.RadioButton,
                "AI-Radio");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.Dropdown,
                "AI-DropDown");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.Number,
                "AI-Numeric");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.OneLineText,
                "AI-Text");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.Time,
                "AI-Time");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(
                AgendaItemManager.AgendaItemType.SectionHeader, "AI-Header");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(
                AgendaItemManager.AgendaItemType.AlwaysSelected, "AI-Always");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(
                AgendaItemManager.AgendaItemType.ContinueButton, "AI-Continue");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.Paragraph,
                "AI-Paragraph");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.Date,
                "AI-Date");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.FileUpload,
                "AI-File");
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(
                AgendaItemManager.AgendaItemType.Contribution, "AI-Contribution");

            BuilderMgr.Next();
        }

        private void SetLodgingPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();

            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.CheckBox, "PopLTCheck", true, true);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.RadioButton, "PopLTRadio", true, true);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Dropdown, "PopLTDrop", true, true);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Number, "PopLTNumber", true, true);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.OneLineText, "PopLTOneLine", true, true);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Paragraph, "PopLTPara", true, true);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Date, "PopLTDate", true, true);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Time, "PopLTTime", true, true);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.CheckBox, "PopNoGroupLTCheck", true, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.RadioButton, "PopNoGroupLTRadio", true, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Dropdown, "PopNoGroupLTDrop", true, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Number, "PopNoGroupLTNumber", true, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.OneLineText, "PopNoGroupLTOneLine", true, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Paragraph, "PopNoGroupLTPara", true, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Date, "PopNoGroupLTDate", true, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Time, "PopNoGroupLTTime", true, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.CheckBox, "NoPopLTCheck", false, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.RadioButton, "NoPopLTRadio", false, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Dropdown, "NoPopLTDrop", false, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Number, "NoPopLTNumber", false, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.OneLineText, "NoPopLTOneLine", false, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Paragraph, "NoPopLTPara", false, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Date, "NoPopLTDate", false, false);
            BuilderMgr.ClickAddLodgingCustomField();
            LodgingCustomField(CustomFieldManager.CustomFieldType.Time, "NoPopLTTime", false, false);
            BuilderMgr.SaveAndStay();
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
        }

        private void SetPreFillLodgingPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();

            // turn on all LT standard fields
            BuilderMgr.EnterEventLodgingTravelPage();
            BuilderMgr.SaveAndStay();

            // add a lodging custom field and a travel custom field
            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.OneLineText, "LDG-Text");
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.OneLineText, "TRV-Text");

            BuilderMgr.SaveAndStay();
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
        }

        private void SetCheckOutPage()
        {
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndClose();
        }

        private void CustomField(CustomFieldManager.CustomFieldType type, string name, bool prePopulate, bool groupEdit)
        {
            BuilderMgr.CFMgr.SetName(name);
            BuilderMgr.CFMgr.SetType(type);
            if (type == CustomFieldManager.CustomFieldType.Dropdown || type == CustomFieldManager.CustomFieldType.RadioButton)
            {
                BuilderMgr.CFMgr.AddPredefinedMultiChoiceItem(PredifinedMultiChoiceItemManagerBase.PredefinedItemType.Agreement);
            }
            if (type == CustomFieldManager.CustomFieldType.Number|| type == CustomFieldManager.CustomFieldType.OneLineText)
            {
                BuilderMgr.CFMgr.SetOneLineLength(25); 
            }
            if (type == CustomFieldManager.CustomFieldType.Paragraph)
            {
                BuilderMgr.CFMgr.SetParagraphCharacterLimit(10000);
            }
            UIUtilityProvider.UIHelper.ExpandAdvanced();
            BuilderMgr.CFMgr.PrePopulateGroupSelections(prePopulate);
            if (groupEdit)
            {
                BuilderMgr.CFMgr.AllowGroupSelectionEditing(groupEdit);
            }
            BuilderMgr.CFMgr.SaveAndClose(); 
        }
        
        private void LodgingCustomField(CustomFieldManager.CustomFieldType type, string name, bool prePopulate, bool groupEdit)
        {
            BuilderMgr.CFMgr.SetName(name);
            BuilderMgr.CFMgr.SetType(type);
            if (type == CustomFieldManager.CustomFieldType.Dropdown || type == CustomFieldManager.CustomFieldType.RadioButton)
            {
                BuilderMgr.CFMgr.AddPredefinedMultiChoiceItem(PredifinedMultiChoiceItemManagerBase.PredefinedItemType.OneToTen);
            }
            if (type == CustomFieldManager.CustomFieldType.Number || type == CustomFieldManager.CustomFieldType.OneLineText)
            {
                BuilderMgr.CFMgr.SetOneLineLength(25);
            }
            if (type == CustomFieldManager.CustomFieldType.Paragraph)
            {
                BuilderMgr.CFMgr.SetParagraphCharacterLimit(10000);
            }
            UIUtilityProvider.UIHelper.ExpandAdvanced();
            BuilderMgr.CFMgr.PrePopulateGroupSelections(prePopulate);
            if (groupEdit)
            {
                BuilderMgr.CFMgr.AllowGroupSelectionEditing(groupEdit);
            }
            BuilderMgr.CFMgr.SaveAndClose();
        }

        private void AgendaItem(AgendaItemManager.AgendaItemType type, string name, bool prePopulate, bool groupEdit)
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(name);
            BuilderMgr.AGMgr.SetType(type);
            if (type == AgendaItemManager.AgendaItemType.Dropdown || type == AgendaItemManager.AgendaItemType.RadioButton)
            {
                BuilderMgr.AGMgr.AddPredefinedMultiChoiceItem(PredifinedMultiChoiceItemManagerBase.PredefinedItemType.Agreement);
            }
            if (type == AgendaItemManager.AgendaItemType.Number || type == AgendaItemManager.AgendaItemType.OneLineText)
            {
                BuilderMgr.AGMgr.SetOneLineLength(25);
            }
            if (type == AgendaItemManager.AgendaItemType.Paragraph)
            {
                BuilderMgr.AGMgr.SetParagraphCharacterLimit(10000);
            }
            BuilderMgr.AGMgr.PrePopulateAgendaGroupSelections(prePopulate);
            if (groupEdit)
            {
                BuilderMgr.AGMgr.AllowAgendaGroupSelectionEditing(groupEdit);
            }
            BuilderMgr.AGMgr.ClickSaveItem(); 
        }

        private void SetStrings(string page, string popStatus)
        {
            Checkbox = string.Format(CheckboxFormat, popStatus, page);
            DropDown = string.Format(DropDownFormat, popStatus, page);
            Radio = string.Format(RadioFormat, popStatus, page);
            Number = string.Format(NumberFormat, popStatus, page);
            OneLine = string.Format(OneLineFormat, popStatus, page);
            Paragraph = string.Format(ParagraphFormat, popStatus, page);
            Date = string.Format(DateFormat, popStatus, page);
            Time = string.Format(TimeFormat, popStatus, page);
        }
    }
}
