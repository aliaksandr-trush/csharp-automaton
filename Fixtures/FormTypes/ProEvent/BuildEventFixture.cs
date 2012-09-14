namespace RegOnline.RegressionTest.Fixtures.FormTypes.ProEvent
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    
    /// <summary>
    /// Builds an event with as many features enabled as possible.
    /// </summary>
    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class BuildEventFixture : FixtureBase
    {
        public const string EventName = "BuildEventFixture";

        private enum RegType
        {
            First,
            Second,
            Third,
            Fourth
        }

        private enum PersonalInfoCustomField
        {
            [StringValue("CF-Checkbox")]
            Checkbox,
            [StringValue("CF-Radio")]
            RadioButton,
            [StringValue("CF-DropDown")]
            DropDown,
            [StringValue("CF-Numeric")]
            Numeric,
            [StringValue("CF-Text")]
            OneLineText,
            [StringValue("CF-Time")]
            Time,
            [StringValue("CF-Header")]
            SectionHeader,
            [StringValue("CF-Always")]
            AlwaysSelected,
            [StringValue("CF-Continue")]
            ContinueButton,
            [StringValue("CF-Paragraph")]
            Paragraph,
            [StringValue("CF-Date")]
            Date,
            [StringValue("CF-File")]
            FileUpload
        }

        private enum AgendaItem
        {
            [StringValue("AI-Checkbox")]
            Checkbox,
            [StringValue("AI-Radio")]
            RadioButton,
            [StringValue("AI-DropDown")]
            DropDown,
            [StringValue("AI-Numeric")]
            Numeric,
            [StringValue("AI-Text")]
            OneLineText,
            [StringValue("AI-Time")]
            Time,
            [StringValue("AI-Header")]
            SectionHeader,
            [StringValue("AI-Always")]
            AlwaysSelected,
            [StringValue("AI-Continue")]
            ContinueButton,
            [StringValue("AI-Paragraph")]
            Paragraph,
            [StringValue("AI-Date")]
            Date,
            [StringValue("AI-File")]
            FileUpload,
            [StringValue("AI-Contribution")]
            Contribution
        }

        private enum LodgingCustomField
        {
            [StringValue("LDG-Checkbox")]
            Checkbox,
            [StringValue("LDG-Radio")]
            RadioButton,
            [StringValue("LDG-DropDown")]
            DropDown,
            [StringValue("LDG-Numeric")]
            Numeric,
            [StringValue("LDG-Text")]
            OneLineText,
            [StringValue("LDG-Time")]
            Time,
            [StringValue("LDG-Header")]
            SectionHeader,
            [StringValue("LDG-Always")]
            AlwaysSelected,
            [StringValue("LDG-Continue")]
            ContinueButton,
            [StringValue("LDG-Paragraph")]
            Paragraph,
            [StringValue("LDG-Date")]
            Date,
            [StringValue("LDG-File")]
            FileUpload
        }

        private enum TravelCustomField
        {
            [StringValue("TRV-Checkbox")]
            Checkbox,
            [StringValue("TRV-Radio")]
            RadioButton,
            [StringValue("TRV-DropDown")]
            DropDown,
            [StringValue("TRV-Numeric")]
            Numeric,
            [StringValue("TRV-Text")]
            OneLineText,
            [StringValue("TRV-Time")]
            Time,
            [StringValue("TRV-Header")]
            SectionHeader,
            [StringValue("TRV-Always")]
            AlwaysSelected,
            [StringValue("TRV-Continue")]
            ContinueButton,
            [StringValue("TRV-Paragraph")]
            Paragraph,
            [StringValue("TRV-Date")]
            Date,
            [StringValue("TRV-File")]
            FileUpload
        }

        private enum MerchandiseItem
        {
            [StringValue("FEE-Header")]
            SectionHeader,
            [StringValue("FEE-Fixed")]
            FixedPrice,
            [StringValue("FEE-Variable")]
            VariableAmount
        }

        private string eventSessionId;

        [Test]
        [Category(Priority.One)]
        [Description("334")]
        public void FullEventBuild()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

            this.SetEventStartPage();
            BuilderMgr.GotoPage(FormDetailManager.Page.PI);
            this.SetEventPersonalInfoPage();
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
            this.SetEventAgendaPage();
            BuilderMgr.GotoPage(FormDetailManager.Page.LodgingTravel);
            this.SetEventLodgingTravelPage();
            BuilderMgr.GotoPage(FormDetailManager.Page.Merchandise);
            this.SetEventMerchandisePage();
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            this.SetEventCheckoutPage();
            BuilderMgr.GotoPage(FormDetailManager.Page.Confirmation);
            this.SetEventConfirmationPage();
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void SetEventStartPage()
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName);
            BuilderMgr.SetStartEndDateTimeDefault();
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyStartPageSettingsAreSaved(ManagerSiteManager.EventType.ProEvent, EventName);

            // Add reg types
            BuilderMgr.AddRegType(RegType.First.ToString());
            BuilderMgr.VerifyHasRegTypeInDatabase(RegType.First.ToString());
            BuilderMgr.AddRegType(RegType.Second.ToString());
            BuilderMgr.VerifyHasRegTypeInDatabase(RegType.Second.ToString());
            BuilderMgr.AddRegType(RegType.Third.ToString());
            BuilderMgr.VerifyHasRegTypeInDatabase(RegType.Third.ToString());
            BuilderMgr.AddRegType(RegType.Fourth.ToString());
            BuilderMgr.VerifyHasRegTypeInDatabase(RegType.Fourth.ToString());
        }

        [Step]
        private void SetEventPersonalInfoPage()
        {
            BuilderMgr.VerifyPersonalInfoPageDefaults();
            BuilderMgr.SetPersonalInfoPage();

            // Set this option once more because of webdriver problem:
            // Checkbox for 'CellPhone' field is not checked when webdriver scrolls down the builder window,
            // as well as SSN
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(RegOnline.RegressionTest.Managers.Builder.FormDetailManager.PersonalInfoField.Cell, true, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(RegOnline.RegressionTest.Managers.Builder.FormDetailManager.PersonalInfoField.SocialSecurityNumber, true, null);

            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyPersonalInfoPageSettingsAreSaved();

            // Add/verify custom fields of each type
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.CheckBox, StringEnum.GetStringValue(PersonalInfoCustomField.Checkbox));
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.RadioButton, StringEnum.GetStringValue(PersonalInfoCustomField.RadioButton));
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.Dropdown, StringEnum.GetStringValue(PersonalInfoCustomField.DropDown));
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.Number, StringEnum.GetStringValue(PersonalInfoCustomField.Numeric));
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.OneLineText, StringEnum.GetStringValue(PersonalInfoCustomField.OneLineText));
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.Time, StringEnum.GetStringValue(PersonalInfoCustomField.Time));
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.SectionHeader, StringEnum.GetStringValue(PersonalInfoCustomField.SectionHeader));
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.AlwaysSelected, StringEnum.GetStringValue(PersonalInfoCustomField.AlwaysSelected));
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.ContinueButton, StringEnum.GetStringValue(PersonalInfoCustomField.ContinueButton));
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.Paragraph, StringEnum.GetStringValue(PersonalInfoCustomField.Paragraph));
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.Date, StringEnum.GetStringValue(PersonalInfoCustomField.Date));
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.FileUpload, StringEnum.GetStringValue(PersonalInfoCustomField.FileUpload));
        }

        private void AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType type, string name)
        {
            BuilderMgr.AddPICustomField(type, name);
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(type, name);
        }

        [Step]
        private void SetEventAgendaPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.VerifyEventAgendaPage();

            // Add/verify agenda items of each type
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.CheckBox, StringEnum.GetStringValue(AgendaItem.Checkbox));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.RadioButton, StringEnum.GetStringValue(AgendaItem.RadioButton));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.Dropdown, StringEnum.GetStringValue(AgendaItem.DropDown));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.Number, StringEnum.GetStringValue(AgendaItem.Numeric));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.OneLineText, StringEnum.GetStringValue(AgendaItem.OneLineText));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.Time, StringEnum.GetStringValue(AgendaItem.Time));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.SectionHeader, StringEnum.GetStringValue(AgendaItem.SectionHeader));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.AlwaysSelected, StringEnum.GetStringValue(AgendaItem.AlwaysSelected));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.ContinueButton, StringEnum.GetStringValue(AgendaItem.ContinueButton));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.Paragraph, StringEnum.GetStringValue(AgendaItem.Paragraph));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.Date, StringEnum.GetStringValue(AgendaItem.Date));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.FileUpload, StringEnum.GetStringValue(AgendaItem.FileUpload));
            this.AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType.Contribution, StringEnum.GetStringValue(AgendaItem.Contribution));

            BuilderMgr.VerifyFormView();
        }

        private void AddAndVerifyAgendaItem(AgendaItemManager.AgendaItemType type, string name)
        {
            BuilderMgr.AddAgendaItem(type, name);
            BuilderMgr.VerifyAgendaItemInDatabase(type, name);
        }

        [Step]
        private void SetEventLodgingTravelPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.EnterEventLodgingTravelPage();
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventLodgingTravelPage();

            // Add/verify lodging custom fields of each type
            this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.CheckBox, StringEnum.GetStringValue(LodgingCustomField.Checkbox));
            this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.RadioButton, StringEnum.GetStringValue(LodgingCustomField.RadioButton));
            this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.Dropdown, StringEnum.GetStringValue(LodgingCustomField.DropDown));
            this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.Number, StringEnum.GetStringValue(LodgingCustomField.Numeric));
            this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.OneLineText, StringEnum.GetStringValue(LodgingCustomField.OneLineText));
            this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.Time, StringEnum.GetStringValue(LodgingCustomField.Time));
            ////this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.SectionHeader, StringEnum.GetStringValue(LodgingCustomField.SectionHeader));
            ////this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.AlwaysSelected, StringEnum.GetStringValue(LodgingCustomField.AlwaysSelected));
            ////this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.ContinueButton, StringEnum.GetStringValue(LodgingCustomField.ContinueButton));
            ////this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.Paragraph, StringEnum.GetStringValue(LodgingCustomField.Paragraph));
            ////this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.Date, StringEnum.GetStringValue(LodgingCustomField.Date));
            ////this.AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType.FileUpload, StringEnum.GetStringValue(LodgingCustomField.FileUpload));

            // Add/verify travel custom fields of each type
            ////this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.CheckBox, StringEnum.GetStringValue(TravelCustomField.Checkbox));
            ////this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.RadioButton, StringEnum.GetStringValue(TravelCustomField.RadioButton));
            ////this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.Dropdown, StringEnum.GetStringValue(TravelCustomField.DropDown));
            ////this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.Number, StringEnum.GetStringValue(TravelCustomField.Numeric));
            ////this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.OneLineText, StringEnum.GetStringValue(TravelCustomField.OneLineText));
            ////this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.Time, StringEnum.GetStringValue(TravelCustomField.Time));
            this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.SectionHeader, StringEnum.GetStringValue(TravelCustomField.SectionHeader));
            this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.AlwaysSelected, StringEnum.GetStringValue(TravelCustomField.AlwaysSelected));
            this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.ContinueButton, StringEnum.GetStringValue(TravelCustomField.ContinueButton));
            this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.Paragraph, StringEnum.GetStringValue(TravelCustomField.Paragraph));
            this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.Date, StringEnum.GetStringValue(TravelCustomField.Date));
            this.AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType.FileUpload, StringEnum.GetStringValue(TravelCustomField.FileUpload));
        }

        private void AddAndVerifyLodgingCustomField(CustomFieldManager.CustomFieldType type, string name)
        {
            BuilderMgr.AddLodgingCustomField(type, name);
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(type, name);
        }

        private void AddAndVerifyTravelCustomField(CustomFieldManager.CustomFieldType type, string name)
        {
            BuilderMgr.AddTravelCustomField(type, name);
            BuilderMgr.VerifyTravelCustomFieldInDatabase(type, name);
        }

        [Step]
        private void SetEventMerchandisePage()
        {
            // Add/verify merchandise items of each type
            BuilderMgr.AddMerchandiseItem(MerchandiseManager.MerchandiseType.Header, StringEnum.GetStringValue(MerchandiseItem.SectionHeader));
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Header, StringEnum.GetStringValue(MerchandiseItem.SectionHeader));
            BuilderMgr.AddMerchandiseItem(MerchandiseManager.MerchandiseType.Fixed, StringEnum.GetStringValue(MerchandiseItem.FixedPrice));
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Fixed, StringEnum.GetStringValue(MerchandiseItem.FixedPrice));
            BuilderMgr.AddMerchandiseItem(MerchandiseManager.MerchandiseType.Variable, StringEnum.GetStringValue(MerchandiseItem.VariableAmount));
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Variable, StringEnum.GetStringValue(MerchandiseItem.VariableAmount));
        }

        [Step]
        private void SetEventCheckoutPage()
        {
            BuilderMgr.EnterEventCheckoutPageFull();
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventCheckoutPage();
        }

        [Step]
        private void SetEventConfirmationPage()
        {
            BuilderMgr.SetEventConfirmationPage();
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventConfirmationPage();
        }
    }
}