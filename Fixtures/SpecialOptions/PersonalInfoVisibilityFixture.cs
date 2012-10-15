namespace RegOnline.RegressionTest.Fixtures.SpecialOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class PersonalInfoVisibilityFixture : FixtureBase
    {
        private const string EventName = "PersonalInfoVisibilityFixture";
        private const int SpacesAvailable = 1;

        private enum RegTypes
        {
            [StringValue("RegType1")]
            RegType1,

            [StringValue("RegType2")]
            RegType2
        }

        private enum CFConditional
        {
            [StringValue("CF-1")]
            CF1,

            [StringValue("CF-1-1")]
            CF11,

            [StringValue("CF-1-2")]
            CF12,

            [StringValue("CF-1-3")]
            CF13,

            [StringValue("CF-RegType1")]
            CFRegType1,

            [StringValue("CF-2")]
            CF2,

            [StringValue("CF-RegType2-CF-2")]
            CFRegType2CF2
        }

        private enum CFs
        {
            [StringValue("CF-AdminOnly")]
            CFAdminOnly,

            [StringValue("CF-Capacity")]
            CFCapacity = 1,

            [StringValue("CF-Required")]
            CFRequired,

            [StringValue("CF-ConditionalDisplay")]
            CFConditionalDisplay,

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

        private int eventId;
        private string sessionId;

        [Test]
        [Category(Priority.Three)]
        [Description("445")]
        public void PIStandardFieldVisibility()
        {
            this.PreviewVisibility();
            this.PreviewRequired();
            this.RegisterVisibility();
            this.RegisterRequired();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("446")]
        public void PICustomFieldVisibility()
        {
            this.CreateEventForCFVisibility();
            this.RegisterAndCheckCFVisibility();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("447")]
        public void PIConditionalLogic()
        {
            this.CreateEventForConditionalLogic();
            this.RegisterEventAndCheckConditionalLogic();
        }

        private void CreateEventForConditionalLogic()
        {
            this.LoginAndAddEvent();
            BuilderMgr.AddRegType(StringEnum.GetStringValue(RegTypes.RegType1));
            BuilderMgr.AddRegType(StringEnum.GetStringValue(RegTypes.RegType2));
            BuilderMgr.Next();

            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.CheckBox, StringEnum.GetStringValue(CFConditional.CF1));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.CheckBox, StringEnum.GetStringValue(CFConditional.CF2));

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(CFConditional.CF11));
            BuilderMgr.CFMgr.SetConditionalLogic(true, StringEnum.GetStringValue(CFConditional.CF1));
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(CFConditional.CF12));
            BuilderMgr.CFMgr.SetConditionalLogic(true, StringEnum.GetStringValue(CFConditional.CF1));
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(CFConditional.CF13));
            BuilderMgr.CFMgr.SetConditionalLogic(true, StringEnum.GetStringValue(CFConditional.CF11));
            BuilderMgr.CFMgr.SetConditionalLogic(true, StringEnum.GetStringValue(CFConditional.CF12));
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(CFConditional.CFRegType1));
            BuilderMgr.CFMgr.ShowAllRegTypes();
            BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Visible, StringEnum.GetStringValue(RegTypes.RegType1));
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(CFConditional.CFRegType2CF2));
            BuilderMgr.CFMgr.ShowAllRegTypes();
            BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Visible, StringEnum.GetStringValue(RegTypes.RegType2));
            BuilderMgr.CFMgr.SetConditionalLogic(true, StringEnum.GetStringValue(CFConditional.CF2));
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.SaveAndStay();
        }

        private void RegisterEventAndCheckConditionalLogic()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(StringEnum.GetStringValue(RegTypes.RegType1));
            RegisterMgr.Continue();

            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF1), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF11), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF12), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF13), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF2), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CFRegType1), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CFRegType2CF2), false);

            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CFConditional.CF1), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF1), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF11), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF12), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF13), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF2), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CFRegType1), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CFRegType2CF2), false);

            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CFConditional.CF11), true);
            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CFConditional.CF12), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF1), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF11), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF12), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF13), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF2), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CFRegType1), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CFRegType2CF2), false);

            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(StringEnum.GetStringValue(RegTypes.RegType2));
            RegisterMgr.Continue();

            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF1), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF11), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF12), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF13), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF2), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CFRegType1), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CFRegType2CF2), false);

            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CFConditional.CF2), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF1), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF11), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF12), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF13), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CF2), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CFRegType1), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFConditional.CFRegType2CF2), true);
        }                                     

        private void CreateEventForCFVisibility()
        {
            this.LoginAndAddEvent();
            BuilderMgr.Next();

            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.CheckBox, StringEnum.GetStringValue(CFs.CFCheckbox));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.RadioButton, StringEnum.GetStringValue(CFs.CFRadio));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Dropdown, StringEnum.GetStringValue(CFs.CFDropDown));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Number, StringEnum.GetStringValue(CFs.CFNumeric));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.OneLineText, StringEnum.GetStringValue(CFs.CFText));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Time, StringEnum.GetStringValue(CFs.CFTime));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.SectionHeader, StringEnum.GetStringValue(CFs.CFHeader));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.AlwaysSelected, StringEnum.GetStringValue(CFs.CFAlways));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.ContinueButton, StringEnum.GetStringValue(CFs.CFContinue));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Paragraph, StringEnum.GetStringValue(CFs.CFParagraph));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.Date, StringEnum.GetStringValue(CFs.CFDate));
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.FileUpload, StringEnum.GetStringValue(CFs.CFFile));

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(CFs.CFAdminOnly));
            BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Admin);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(CFs.CFCapacity));
            BuilderMgr.CFMgr.SetSpacesAvailable(SpacesAvailable);
            BuilderMgr.CFMgr.SetLimitReachOption(CustomFieldManager.LimitReachedOption.HideThisItem);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(CFs.CFRequired));
            BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Required);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(CFs.CFConditionalDisplay));
            BuilderMgr.CFMgr.SetConditionalLogic(true, StringEnum.GetStringValue(CFs.CFRequired));
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.SaveAndStay();
        }

        private void RegisterAndCheckCFVisibility()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();

            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFCheckbox), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFRadio), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFDropDown), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFNumeric), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFText), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFTime), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFHeader), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFAlways), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFContinue), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFParagraph), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFDate), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFFile), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFAdminOnly), false);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFCapacity), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFRequired), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFConditionalDisplay), false);
            RegisterMgr.VerifyCustomFieldRequired(StringEnum.GetStringValue(CFs.CFRequired), true);

            RegisterMgr.SelectAgendaItem(StringEnum.GetStringValue(CFs.CFCapacity));
            RegisterMgr.SelectAgendaItem(StringEnum.GetStringValue(CFs.CFRequired));
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFConditionalDisplay), true);

            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFCapacity), false);

            RegisterMgr.OpenAdminRegisterPage(this.eventId, this.sessionId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(CFs.CFAdminOnly), true);
        }

        private void PreviewVisibility()
        {
            this.LoginAndAddEvent();
            BuilderMgr.Next();

            BuilderMgr.SetRecommendedFieldsVisibility(true);
            BuilderMgr.SetOptionalFieldsVisibility(true);
            BuilderMgr.VerifyStandardFieldsVisibilityWhenPreview(true, true);

            BuilderMgr.SetRecommendedFieldsVisibility(true);
            BuilderMgr.SetOptionalFieldsVisibility(false);
            BuilderMgr.VerifyStandardFieldsVisibilityWhenPreview(true, false);

            BuilderMgr.SetRecommendedFieldsVisibility(false);
            BuilderMgr.SetOptionalFieldsVisibility(true);
            BuilderMgr.VerifyStandardFieldsVisibilityWhenPreview(false, true);

            BuilderMgr.SetRecommendedFieldsVisibility(false);
            BuilderMgr.SetOptionalFieldsVisibility(false);
            BuilderMgr.VerifyStandardFieldsVisibilityWhenPreview(false, false);
        }

        private void PreviewRequired()
        {
            BuilderMgr.SetRecommendedFieldsRequired(true);
            BuilderMgr.SetOptionalFieldsRequired(true);
            BuilderMgr.VerifyStandardFieldsRequiredWhenPreview(true, true);

            BuilderMgr.SetRecommendedFieldsRequired(true);
            BuilderMgr.SetOptionalFieldsRequired(false);
            BuilderMgr.VerifyStandardFieldsRequiredWhenPreview(true, false);

            BuilderMgr.SetRecommendedFieldsRequired(false);
            BuilderMgr.SetOptionalFieldsRequired(true);
            BuilderMgr.VerifyStandardFieldsRequiredWhenPreview(false, true);

            BuilderMgr.SetRecommendedFieldsRequired(false);
            BuilderMgr.SetOptionalFieldsRequired(false);
            BuilderMgr.VerifyStandardFieldsRequiredWhenPreview(false, false);
        }

        private void RegisterVisibility()
        {
            BuilderMgr.SetRecommendedFieldsVisibility(true);
            BuilderMgr.SetOptionalFieldsVisibility(true);
            BuilderMgr.SaveAndStay();
            this.RegisterAndCheckVisibility(true, true);

            this.GoToBuilderPIPage();

            BuilderMgr.SetRecommendedFieldsVisibility(true);
            BuilderMgr.SetOptionalFieldsVisibility(false);
            BuilderMgr.SaveAndStay();
            this.RegisterAndCheckVisibility(true, false);

            this.GoToBuilderPIPage();

            BuilderMgr.SetRecommendedFieldsVisibility(false);
            BuilderMgr.SetOptionalFieldsVisibility(true);
            BuilderMgr.SaveAndStay();
            this.RegisterAndCheckVisibility(false, true);

            this.GoToBuilderPIPage();

            BuilderMgr.SetRecommendedFieldsVisibility(false);
            BuilderMgr.SetOptionalFieldsVisibility(false);
            BuilderMgr.SaveAndStay();
            this.RegisterAndCheckVisibility(false, false);

            this.GoToBuilderPIPage();
        }

        private void RegisterRequired()
        {
            BuilderMgr.SetRecommendedFieldsRequired(true);
            BuilderMgr.SetOptionalFieldsRequired(true);
            BuilderMgr.SaveAndStay();
            this.RegisterAndCheckRequired(true, true);

            this.GoToBuilderPIPage();

            BuilderMgr.SetRecommendedFieldsRequired(true);
            BuilderMgr.SetOptionalFieldsRequired(false);
            BuilderMgr.SaveAndStay();
            this.RegisterAndCheckRequired(true, false);

            this.GoToBuilderPIPage();

            BuilderMgr.SetRecommendedFieldsRequired(false);
            BuilderMgr.SetOptionalFieldsRequired(true);
            BuilderMgr.SaveAndStay();
            this.RegisterAndCheckRequired(false, true);

            this.GoToBuilderPIPage();

            BuilderMgr.SetRecommendedFieldsRequired(false);
            BuilderMgr.SetOptionalFieldsRequired(false);
            BuilderMgr.SaveAndStay();
            this.RegisterAndCheckRequired(false, false);

            this.GoToBuilderPIPage();
        }

        private void GoToBuilderPIPage()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            ManagerSiteMgr.ClickEditRegistrationForm(EventName);
            BuilderMgr.Next();        
        }

        private void RegisterAndCheckVisibility(bool recommended, bool optional)
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            RegisterMgr.VerifyStandardFieldsPresent(recommended, optional);
        }

        private void RegisterAndCheckRequired(bool recommended, bool optional)
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            RegisterMgr.VerifyStandardFieldsRequired(recommended, optional);
        }

        private void LoginAndAddEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.DeleteEventByName(EventName);

            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName);
        }
    }
}
