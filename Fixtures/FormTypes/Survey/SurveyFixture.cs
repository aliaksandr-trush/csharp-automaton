namespace RegOnline.RegressionTest.Fixtures.FormTypes.Survey
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class SurveyFixture : FixtureBase
    {
        private const string EventName = "SurveyFixture";
        private const string EventNameAlternative = "SurveyFixtureAlternative";
        private const string CopiedEventName = EventName + " (Copy)";
        private const string CopiedTwiceEventName = CopiedEventName + " (Copy)";
        private const string IntroductoryMessage = "Welcome to the survey fixture!";
        private const string ConfirmationMessage = "The survey is completed. You can close the window now.";
        private const int NumberOfCustomQuestions = 5;
        private const string SurveyAdminReg = "SurveyAdminReg";

        private enum Questions
        {
            [StringValue("QuestionAdminOnly")]
            QuestionAdminOnly,

            [StringValue("QuestioinVisibleToAll")]
            QuestioinVisibleToAll,

            [StringValue("QuestionInvisible")]
            QuestionInvisible
        }

        private enum CustomQuestion
        {
            [StringValue("Years Playing")]
            YearsPlaying, // Visible to MidFielder

            [StringValue("Years at this position")]
            YearsAtThisPosition, // Visible to Keeper

            [StringValue("Check here to attend camp")]
            CheckHereToAttendCamp, // Visible to All

            [StringValue("What do you hope to accomplish?")]
            WhatDoYouHopeToAccomplish, // Visible to Keeper, Sweeper, Striker

            [StringValue("Do you like Goalie Wars?")]
            DoYouLikeGoalieWars // Visible to Keeper
        }

        private int eventId;
        private int eventIdAlternative;
        private string eventSessionId;
        private SurveyManager surveyMgr;
        private int registrationId;
        private string email;
        private SurveyRegType surveyRegType = new SurveyRegType();

        private enum ParticipantType
        {
            Keeper,
            Defender,
            MidFielder,
            Forward,
            Sweeper,
            Striker
        }

        public SurveyFixture()
            : base()
        {
            this.surveyMgr = new SurveyManager();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("351")]
        public void CreateSurvey()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.AddNewSurvey(EventName, true);
            this.AddNewSurvey(EventNameAlternative, false);
        }

        [Test]
        [Category(Priority.Four)]
        [Description("685")]
        public void PreviewSurvey()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.AddNewSurvey(EventName, true);
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }

            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventId, this.eventSessionId);

            // Enter previewing mode
            BuilderMgr.TogglePreviewAndEditMode();

            // Make sure mobile view mode is turned off
            BuilderMgr.SetMobileViewMode(false);

            BuilderMgr.SelectRegTypeWhenPreview(ParticipantType.Keeper.ToString());

            // Verify reg types in Preview
            VerifyParticipantTypesVisibilities();

            // Change preview to a different participant type
            BuilderMgr.SelectRegTypeWhenPreview(ParticipantType.Striker.ToString());

            VerifyParticipantTypesVisibilities();

            // Verify Introductory Message on start page
            this.surveyMgr.VerifyIntroductoryMessageWhenPreview(IntroductoryMessage);

            // Go to Participant Information Tab
            BuilderMgr.GotoPage(FormDetailManager.Page.PI);

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.SelectRegTypeWhenPreview(ParticipantType.Keeper.ToString());
            VerifyCustomQuestionsVisibilities();

            // Go to the Custom Questions Tab
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);

            BuilderMgr.SetMobileViewMode(false);
            BuilderMgr.SelectRegTypeWhenPreview(ParticipantType.Keeper.ToString());
            VerifyCustomQuestionsVisibilities();

            // Go to Confirmation Tab
            BuilderMgr.GotoPage(FormDetailManager.Page.Confirmation);

            // Verify Confirmation Page Message
            this.surveyMgr.VerifyConfirmationMessageWhenPreview(ConfirmationMessage);

            // All done previewing the survey, back to edit mode, then save and close
            BuilderMgr.TogglePreviewAndEditMode();
            BuilderMgr.SaveAndClose();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("352")]
        public void RegisterSurvey()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventNameAlternative);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.AddNewSurvey(EventName, true);
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }

            if (!ManagerSiteMgr.EventExists(EventNameAlternative))
            {
                this.AddNewSurvey(EventNameAlternative, false);
            }
            else
            {
                this.eventIdAlternative = ManagerSiteMgr.GetFirstEventId(EventNameAlternative);
            }

            // 1st registration with participant type 'Keeper'
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.SelectRegType(ParticipantType.Keeper.ToString());
            RegisterMgr.Continue();
            RegisterMgr.GenerateCurrentRegistrantLastName();
            RegisterMgr.TypePersonalInfoUniqueEmail();
            RegisterMgr.TypePersonalInfoFirstName(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.FirstName);
            RegisterMgr.TypePersonalInfoLastName(RegisterMgr.CurrentRegistrantLastName);
            RegisterMgr.TypePersonalInfoCompany(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.Company);
            RegisterMgr.TypePersonalInfoCity(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.City);
            RegisterMgr.EnterPersonalInfoDateOfBirthGender(new DateTime(2000, 12, 31), Managers.Register.RegisterManager.Gender.Female);
            RegisterMgr.TypeCustomField(StringEnum.GetStringValue(CustomQuestion.YearsAtThisPosition), "3");
            RegisterMgr.SetCustomFieldCheckbox(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), true);
            RegisterMgr.TypeCustomField(StringEnum.GetStringValue(CustomQuestion.WhatDoYouHopeToAccomplish), "World's best!");
            RegisterMgr.TypeCustomField(StringEnum.GetStringValue(CustomQuestion.DoYouLikeGoalieWars), "Not so much.");
            RegisterMgr.Continue();
            RegisterMgr.ConfirmRegistration();

            // 2nd registration with participant type 'MidFielder'
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.SelectRegType(ParticipantType.MidFielder.ToString());
            RegisterMgr.Continue();
            RegisterMgr.GenerateCurrentRegistrantLastName();
            RegisterMgr.TypePersonalInfoUniqueEmail();
            RegisterMgr.TypePersonalInfoFirstName(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.FirstName);
            RegisterMgr.TypePersonalInfoLastName(RegisterMgr.CurrentRegistrantLastName);
            RegisterMgr.TypePersonalInfoCompany(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.Company);
            RegisterMgr.TypePersonalInfoCity(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.City);
            RegisterMgr.EnterPersonalInfoDateOfBirthGender(new DateTime(1980, 2, 20), Managers.Register.RegisterManager.Gender.Male);
            RegisterMgr.TypeCustomField(StringEnum.GetStringValue(CustomQuestion.YearsPlaying), "10");
            RegisterMgr.SetCustomFieldCheckbox(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), false);
            RegisterMgr.Continue();
            RegisterMgr.ConfirmRegistration();

            // 3rd registration with participant type 'Sweeper'
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.SelectRegType(ParticipantType.Sweeper.ToString());
            RegisterMgr.Continue();
            RegisterMgr.GenerateCurrentRegistrantLastName();
            RegisterMgr.TypePersonalInfoUniqueEmail();
            RegisterMgr.TypePersonalInfoFirstName(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.FirstName);
            RegisterMgr.TypePersonalInfoLastName(RegisterMgr.CurrentRegistrantLastName);
            RegisterMgr.TypePersonalInfoCompany(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.Company);
            RegisterMgr.TypePersonalInfoCity(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.City);
            RegisterMgr.EnterPersonalInfoDateOfBirthGender(new DateTime(1993, 5, 1), Managers.Register.RegisterManager.Gender.Male);
            RegisterMgr.SetCustomFieldCheckbox(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), true);
            RegisterMgr.TypeCustomField(StringEnum.GetStringValue(CustomQuestion.WhatDoYouHopeToAccomplish), "To be the best of myself.");
            RegisterMgr.Continue();
            RegisterMgr.ConfirmRegistration();

            // 4th registration with participant type 'Striker'
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.SelectRegType(ParticipantType.Striker.ToString());
            RegisterMgr.Continue();
            RegisterMgr.GenerateCurrentRegistrantLastName();
            RegisterMgr.TypePersonalInfoUniqueEmail();
            RegisterMgr.TypePersonalInfoFirstName(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.FirstName);
            RegisterMgr.TypePersonalInfoLastName(RegisterMgr.CurrentRegistrantLastName);
            RegisterMgr.TypePersonalInfoCompany(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.Company);
            RegisterMgr.TypePersonalInfoCity(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.City);
            RegisterMgr.EnterPersonalInfoDateOfBirthGender(new DateTime(1999, 6, 10), Managers.Register.RegisterManager.Gender.Male);
            RegisterMgr.SetCustomFieldCheckbox(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), true);
            RegisterMgr.TypeCustomField(StringEnum.GetStringValue(CustomQuestion.WhatDoYouHopeToAccomplish), "Attack?");
            RegisterMgr.Continue();
            RegisterMgr.ConfirmRegistration();

            // Make personal info email field as required, and make verify email field as visible
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.OpenEventBuilderStartPage(eventId, eventSessionId);
            BuilderMgr.Next();
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Email, null, true);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.VerifyEmail, true, null);
            BuilderMgr.SaveAndClose();

            // 5th registration: required to enter email address, and verify email address
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.SelectRegType(ParticipantType.Striker.ToString());
            RegisterMgr.ContinueWithErrors();
            RegisterMgr.VerifyErrorMessage(Managers.Register.RegisterManager.Error.EmailAddressIsBlank);
            string email = RegisterMgr.ComposeUniqueEmailAddress();
            RegisterMgr.EnterEmailAddress(email);
            RegisterMgr.ContinueWithErrors();
            RegisterMgr.VerifyErrorMessage(Managers.Register.RegisterManager.Error.EmailAddressesDoNotMatch);
            RegisterMgr.EnterVerifyEmailAddress(email);
            RegisterMgr.Continue();
            RegisterMgr.GenerateCurrentRegistrantLastName();
            RegisterMgr.TypePersonalInfoFirstName(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.FirstName);
            RegisterMgr.TypePersonalInfoLastName(RegisterMgr.CurrentRegistrantLastName);
            RegisterMgr.TypePersonalInfoCompany(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.Company);
            RegisterMgr.TypePersonalInfoCity(RegOnline.RegressionTest.Managers.Register.RegisterManager.DefaultPersonalInfo.City);
            RegisterMgr.EnterPersonalInfoDateOfBirthGender(new DateTime(2005, 11, 11), Managers.Register.RegisterManager.Gender.Female);
            RegisterMgr.SetCustomFieldCheckbox(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), true);
            RegisterMgr.TypeCustomField(StringEnum.GetStringValue(CustomQuestion.WhatDoYouHopeToAccomplish), "Attack?");
            RegisterMgr.Continue();
            RegisterMgr.ConfirmRegistration();

            // Change survey to anonymous(remove all participant types and personal info fields visibilities in builder)
            ////this.CopySurvey();

            // 6th registration: an anonymous registration
            RegisterMgr.OpenRegisterPage(this.eventIdAlternative);
            RegisterMgr.Continue();
            RegisterMgr.TypeCustomField(StringEnum.GetStringValue(CustomQuestion.YearsPlaying), "7");
            RegisterMgr.TypeCustomField(StringEnum.GetStringValue(CustomQuestion.YearsAtThisPosition), "5");
            RegisterMgr.SetCustomFieldCheckbox(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), true);
            RegisterMgr.TypeCustomField(StringEnum.GetStringValue(CustomQuestion.WhatDoYouHopeToAccomplish), "Champion");
            RegisterMgr.TypeCustomField(StringEnum.GetStringValue(CustomQuestion.DoYouLikeGoalieWars), "Yes");
            RegisterMgr.Continue();
            RegisterMgr.ConfirmRegistration();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("585")]
        public void CopySurveyForm()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();

            this.CopySurveyFromManagerSite();
            this.CopySurveyFromDashboard();

            ManagerSiteMgr.DeleteEventByName(CopiedEventName);
            ManagerSiteMgr.DeleteEventByName(CopiedTwiceEventName);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("353")]
        public void AdminReg()
        {
            this.CreateSurveyForAdminReg();
            this.AdminRegSurvey();
            this.ConfirmRegistrationSaved(this.registrationId);
        }

        private void CreateSurveyForAdminReg()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();

            if (ManagerSiteMgr.EventExists(SurveyAdminReg))
            {
                ManagerSiteMgr.DeleteEventByName(SurveyAdminReg);
            }

            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.Survey);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(SurveyAdminReg);
            this.surveyMgr.SetIntroductoryMessage(IntroductoryMessage);

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(surveyRegType.RegTypeAdminOnly);
            BuilderMgr.RegTypeMgr.SetVisibilities(false, true, null);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            surveyRegType.RegTypeAdminOnlyId = BuilderMgr.Fetch_RegTypeID(eventId, surveyRegType.RegTypeAdminOnly);

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(surveyRegType.RegTypeVisibleToAll);
            BuilderMgr.RegTypeMgr.SetVisibilities(true, true, null);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            surveyRegType.RegTypeVisibleToAllId = BuilderMgr.Fetch_RegTypeID(eventId, surveyRegType.RegTypeVisibleToAll);

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(surveyRegType.RegTypeInvisible);
            BuilderMgr.RegTypeMgr.SetVisibilities(false, false, null);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            surveyRegType.RegTypeInvisibleId = BuilderMgr.Fetch_RegTypeID(eventId, surveyRegType.RegTypeInvisible);

            BuilderMgr.Next();

            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Email, true, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.FirstName, true, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.LastName, true, null);

            BuilderMgr.Next();

            this.surveyMgr.ClickAddCustomQuestion();
            this.surveyMgr.SetCustomQuestionName(StringEnum.GetStringValue(Questions.QuestioinVisibleToAll));
            this.surveyMgr.SetCustomQuestionVisibilities(true, false, false);
            this.surveyMgr.SaveAndNewQuestion();
            this.surveyMgr.SetCustomQuestionName(StringEnum.GetStringValue(Questions.QuestionInvisible));
            this.surveyMgr.SetCustomQuestionVisibilities(false, false, false);
            this.surveyMgr.SaveAndNewQuestion();
            this.surveyMgr.SetCustomQuestionName(StringEnum.GetStringValue(Questions.QuestionAdminOnly));
            this.surveyMgr.SetCustomQuestionVisibilities(false, false, true);
            UIUtilityProvider.UIHelper.SelectOriginalWindow();

            BuilderMgr.Next();

            this.surveyMgr.SetConfirmationMessage(ConfirmationMessage);

            BuilderMgr.SaveAndClose();
        }

        private void AdminRegSurvey()
        {
            RegisterMgr.OpenAdminRegisterPage(this.eventId, this.eventSessionId);
            RegisterMgr.VerifyCustomFieldPresent(IntroductoryMessage, true);
            Assert.True(RegisterMgr.HasRegType(surveyRegType.RegTypeAdminOnlyId));
            Assert.True(RegisterMgr.HasRegType(surveyRegType.RegTypeVisibleToAllId));
            Assert.False(RegisterMgr.HasRegType(surveyRegType.RegTypeInvisibleId));
            RegisterMgr.SelectRegType(surveyRegType.RegTypeAdminOnly);
            RegisterMgr.Continue();

            Assert.True(RegisterMgr.IsEmailFieldPresent());
            Assert.True(RegisterMgr.IsFirstNameFieldPresent());
            Assert.True(RegisterMgr.IsLastNameFieldPresent());
            Assert.True(RegisterMgr.IsStatusFieldPresent());
            this.email = RegisterMgr.ComposeUniqueEmailAddress();
            string lastName = RegisterMgr.GenerateCurrentRegistrantLastName();
            RegisterMgr.TypePersonalInfoEmail(email);
            RegisterMgr.TypePersonalInfoFirstName(Managers.Register.RegisterManager.DefaultPersonalInfo.FirstName);
            RegisterMgr.TypePersonalInfoLastName(lastName);
            this.registrationId = RegisterMgr.GetRegIdFromSession();

            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(Questions.QuestionAdminOnly), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(Questions.QuestioinVisibleToAll), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(Questions.QuestionInvisible), false);
            RegisterMgr.SetCustomFieldCheckbox(StringEnum.GetStringValue(Questions.QuestionAdminOnly), true);
            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(Questions.QuestioinVisibleToAll), true);
            RegisterMgr.Continue();

            RegisterMgr.VerifyCustomFieldPresent(ConfirmationMessage, true);
        }

        private void ConfirmRegistrationSaved(int RegId)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, RegId);

            BackendMgr.VerifyCustomFieldPresent(surveyRegType.RegTypeAdminOnly, true);
            BackendMgr.VerifyCustomFieldPresent(this.email, true);
            BackendMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(Questions.QuestionAdminOnly), true);
            BackendMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(Questions.QuestioinVisibleToAll), true);
        }

        private void CopySurveyFromManagerSite()
        {
            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.AddNewSurvey(EventName, false);
            }

            ManagerSiteMgr.CopyEventByName(EventName);
            Assert.True(ManagerSiteMgr.EventExists(CopiedEventName));
        }

        private void CopySurveyFromDashboard()
        {
            this.eventId = ManagerSiteMgr.GetFirstEventId(CopiedEventName);
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);
            ManagerSiteMgr.DashboardMgr.CopyEventFromDashboard(CopiedTwiceEventName);
            ManagerSiteMgr.DashboardMgr.ReturnToList();
        }

        [Step]
        private void AddNewSurvey(string eventName, bool withParticipantType)
        {
            // Delete previous versions
            ManagerSiteMgr.DeleteEventByName(eventName);

            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.Survey);
            this.SetStartPage(eventName, withParticipantType);
            BuilderMgr.Next();
            this.SetPIPage(true);
            BuilderMgr.Next();
            this.SetCustomQuestionPage(withParticipantType);
            BuilderMgr.Next();
            this.SetConfirmationPage();

            if (withParticipantType)
            {
                this.eventId = BuilderMgr.GetEventId();
            }
            else
            {
                this.eventIdAlternative = BuilderMgr.GetEventId();
            }

            BuilderMgr.SaveAndClose();
        }

        private void SetStartPage(string eventName, bool withParticipantType)
        {
            // Enter title and shortcut
            BuilderMgr.SetEventNameAndShortcut(eventName);

            if (withParticipantType)
            {
                // Add reg types
                BuilderMgr.AddRegType(ParticipantType.Keeper.ToString());
                BuilderMgr.VerifyHasRegTypeInDatabase(ParticipantType.Keeper.ToString());
                BuilderMgr.AddRegType(ParticipantType.Defender.ToString());
                BuilderMgr.VerifyHasRegTypeInDatabase(ParticipantType.Defender.ToString());
                BuilderMgr.AddRegType(ParticipantType.MidFielder.ToString());
                BuilderMgr.VerifyHasRegTypeInDatabase(ParticipantType.MidFielder.ToString());
                BuilderMgr.AddRegType(ParticipantType.Forward.ToString());
                BuilderMgr.VerifyHasRegTypeInDatabase(ParticipantType.Forward.ToString());
                BuilderMgr.AddRegType(ParticipantType.Sweeper.ToString());
                BuilderMgr.VerifyHasRegTypeInDatabase(ParticipantType.Sweeper.ToString());
                BuilderMgr.AddRegType(ParticipantType.Striker.ToString());
                BuilderMgr.VerifyHasRegTypeInDatabase(ParticipantType.Striker.ToString());
            }

            // Add introductory message
            this.surveyMgr.SetIntroductoryMessage(IntroductoryMessage);

            BuilderMgr.SaveAndStay();
        }

        private void SetPIPage(bool visible)
        {
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Email, visible, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.FirstName, visible, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.LastName, visible, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Company, visible, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.City, visible, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.DateOfBirth, visible, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Gender, visible, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.EmergencyContactName, visible, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.EmergencyContactPhone, visible, null);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.UploadPhoto, visible, null);

            BuilderMgr.SaveAndStay();
        }

        private void SetCustomQuestionPage(bool withParticipantType)
        {
            if (withParticipantType)
            {
                this.AddCustomQuestionLineNumeric(
                    StringEnum.GetStringValue(CustomQuestion.YearsPlaying),
                    2,
                    ParticipantType.MidFielder.ToString());

                this.AddCustomQuestionLineNumeric(
                    StringEnum.GetStringValue(CustomQuestion.YearsAtThisPosition),
                    3,
                    ParticipantType.Keeper.ToString());

                this.AddCustomQuestionCheckBox(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), "All");

                this.AddCustomQuestionParagraph(
                    StringEnum.GetStringValue(CustomQuestion.WhatDoYouHopeToAccomplish),
                    1000,
                    ParticipantType.Sweeper.ToString(),
                    ParticipantType.Striker.ToString(),
                    ParticipantType.Keeper.ToString());

                this.AddCustomQuestionParagraph(
                    StringEnum.GetStringValue(CustomQuestion.DoYouLikeGoalieWars),
                    100,
                    ParticipantType.Keeper.ToString(),
                    null,
                    null);
            }
            else
            {
                this.AddCustomQuestionLineNumeric(
                    StringEnum.GetStringValue(CustomQuestion.YearsPlaying),
                    2,
                    null);

                this.AddCustomQuestionLineNumeric(
                    StringEnum.GetStringValue(CustomQuestion.YearsAtThisPosition),
                    3,
                    null);

                this.AddCustomQuestionCheckBox(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), null);

                this.AddCustomQuestionParagraph(
                    StringEnum.GetStringValue(CustomQuestion.WhatDoYouHopeToAccomplish),
                    1000,
                    null,
                    null,
                    null);

                this.AddCustomQuestionParagraph(
                    StringEnum.GetStringValue(CustomQuestion.DoYouLikeGoalieWars),
                    100,
                    null,
                    null,
                    null);
            }
            
            BuilderMgr.SaveAndStay();
        }

        private void SetCustomQuestionsVisibilities(bool visible)
        {
            for (int order = 0; order < NumberOfCustomQuestions; order++)
            {
                BuilderMgr.AGMgr.OpenAgendaInListByOrder(order);
                BuilderMgr.AGMgr.SetVisibilityOption(visible, CFManagerBase.VisibilityOption.Visible);
                BuilderMgr.AGMgr.ClickSaveItem();
            }
        }

        private void SetConfirmationPage()
        {
            this.surveyMgr.SetConfirmationMessage(ConfirmationMessage);
        }

        // Add One-line Numeric Custom Question
        private void AddCustomQuestionLineNumeric(string qName, int qNumLength, string qRegType)
        {
            this.surveyMgr.ClickAddCustomQuestion();
            BuilderMgr.AGMgr.SetName(qName);
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.Number);
            BuilderMgr.AGMgr.SetOneLineLength(qNumLength);

            if (!string.IsNullOrEmpty(qRegType))
            {
                BuilderMgr.AGMgr.ShowAllRegTypes();
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, qRegType);
            }

            BuilderMgr.AGMgr.ClickSaveItem();
        }

        // Add Checkbox Custom Question
        private void AddCustomQuestionCheckBox(string qName, string qRegType)
        {
            this.surveyMgr.ClickAddCustomQuestion();
            BuilderMgr.AGMgr.SetName(qName);
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.CheckBox);

            if (!string.IsNullOrEmpty(qRegType))
            {
                BuilderMgr.AGMgr.ShowAllRegTypes();
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, qRegType);
            }

            BuilderMgr.AGMgr.ClickSaveItem();
        }

        // Add Paragraph Custom Question
        private void AddCustomQuestionParagraph(string qName, int charLen, string qRegType, string qRegType2, string qRegType3)
        {
            this.surveyMgr.ClickAddCustomQuestion();
            BuilderMgr.AGMgr.SetName(qName);
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.Paragraph);
            BuilderMgr.AGMgr.SetParagraphCharacterLimit(charLen);

            if (!string.IsNullOrEmpty(qRegType) || !string.IsNullOrEmpty(qRegType2) || !string.IsNullOrEmpty(qRegType3))
            {
                BuilderMgr.AGMgr.ShowAllRegTypes();
            }

            if (!string.IsNullOrEmpty(qRegType))
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, qRegType);
            }

            if (!string.IsNullOrEmpty(qRegType2))
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, qRegType2);
            }

            if (!string.IsNullOrEmpty(qRegType3))
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, qRegType3);
            }

            BuilderMgr.AGMgr.ClickSaveItem();
        }

        [Verify]
        private void VerifyParticipantTypesVisibilities()
        {
            BuilderMgr.VerifyHasRegTypeWhenPreview(ParticipantType.Keeper.ToString(), true);
            BuilderMgr.VerifyHasRegTypeWhenPreview(ParticipantType.Defender.ToString(), true);
            BuilderMgr.VerifyHasRegTypeWhenPreview(ParticipantType.MidFielder.ToString(), true);
            BuilderMgr.VerifyHasRegTypeWhenPreview(ParticipantType.Forward.ToString(), true);
            BuilderMgr.VerifyHasRegTypeWhenPreview(ParticipantType.Sweeper.ToString(), true);
            BuilderMgr.VerifyHasRegTypeWhenPreview(ParticipantType.Striker.ToString(), true);
        }

        [Verify]
        private void VerifyCustomQuestionsVisibilities()
        {
            // Participant type 'Keeper' is selected by default
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CustomQuestion.YearsAtThisPosition), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CustomQuestion.WhatDoYouHopeToAccomplish), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CustomQuestion.DoYouLikeGoalieWars), true);

            // Verify custom fields' visibilities for participant type 'MidFielder'
            BuilderMgr.SelectRegTypeWhenPreview(ParticipantType.MidFielder.ToString());
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CustomQuestion.YearsPlaying), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), true);

            // Verify custom fields' visibilities for participant type 'Sweeper'
            BuilderMgr.SelectRegTypeWhenPreview(ParticipantType.Sweeper.ToString());
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CustomQuestion.WhatDoYouHopeToAccomplish), true);

            // Verify custom fields' visibilities for participant type 'Striker'
            BuilderMgr.SelectRegTypeWhenPreview(ParticipantType.Striker.ToString());
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CustomQuestion.CheckHereToAttendCamp), true);
            BuilderMgr.VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(CustomQuestion.WhatDoYouHopeToAccomplish), true);
        }

        private void CopySurvey()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            eventSessionId = ManagerSiteMgr.GetEventSessionId();

            ManagerSiteMgr.DeleteEventByName(CopiedEventName);

            ManagerSiteMgr.OpenEventDashboard(eventId);

            // Make a copy of the current survey
            ManagerSiteMgr.DashboardMgr.CopyEventFromDashboard(CopiedEventName);

            ManagerSiteMgr.DashboardMgr.ClickOption(Managers.Manager.Dashboard.DashboardManager.EventSetupFunction.EditRegForm);
            eventId = BuilderMgr.GetEventId();
            BuilderMgr.DeleteRegType(ParticipantType.Keeper.ToString());
            BuilderMgr.DeleteRegType(ParticipantType.Defender.ToString());
            BuilderMgr.DeleteRegType(ParticipantType.MidFielder.ToString());
            BuilderMgr.DeleteRegType(ParticipantType.Forward.ToString());
            BuilderMgr.DeleteRegType(ParticipantType.Sweeper.ToString());
            BuilderMgr.DeleteRegType(ParticipantType.Striker.ToString());
            BuilderMgr.Next();
            SetPIPage(false);
            BuilderMgr.Next();
            SetCustomQuestionsVisibilities(true);
            BuilderMgr.SaveAndClose();
        }
    }

    public class SurveyRegType
    {
        public  string RegTypeAdminOnly = "RegTypeAdminOnly";
        public string RegTypeVisibleToAll = "RegTypeVisibleToAll";
        public string RegTypeInvisible = "RegTypeInvisible";
        public int RegTypeAdminOnlyId;
        public int RegTypeVisibleToAllId;
        public int RegTypeInvisibleId;
    }
}
