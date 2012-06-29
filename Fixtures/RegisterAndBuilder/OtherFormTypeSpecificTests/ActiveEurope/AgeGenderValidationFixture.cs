namespace RegOnline.RegressionTest.Fixtures.RegisterAndBuilder.OtherFormTypeSpecificTests.ActiveEurope
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgeGenderValidationFixture : FixtureBase
    {
        private string eventSessionId;
        private int eventId;
        private const string EventName = "AgeGenderWithConditionalLogicAndLimits";
        private const string AgendaParent = "Agenda Parent";
        private const string AgendaConditionalOne = "Agenda Male Conditional Limited";
        private const string AgendaConditionalTwo = "Agenda Female Conditional Limited";
        private const string AgeGreaterThan = "18";
        private string AgeGreaterThanDate = DateTime.Today.AddDays(-1).ToString("0:MM/dd/yyyy");
        private const string AgeLessThan = "50";
        private string AgeLessThanDate = DateTime.Today.AddDays(-1).ToString("0:MM/dd/yyyy");

        /// <summary>
        /// The limits are just to ensure that a bug from USM 3 doesn't show up again. 
        /// This only tests age gender validation on the agenda page
        /// </summary>
        [Test]
        [Category(Priority.Three)]
        [Description("866")]
        public void AgeGenderWithConditionalLogicAndLimits()
        {
            ConfigurationProvider.XmlConfig.ReloadAccount(XmlConfiguration.AccountType.ActiveEurope);
            CreateEvent();
            RegisterForEvent(Managers.Register.RegisterManager.Gender.Male, 19);
            RegisterForEvent(Managers.Register.RegisterManager.Gender.Female, 19);
            RegisterForEvent(Managers.Register.RegisterManager.Gender.Male, 51);
            RegisterForEvent(Managers.Register.RegisterManager.Gender.Female, 17); 
        }

        [Step]
        private void RegisterForEvent(Managers.Register.RegisterManager.Gender gender, int yearsOld)
        {
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfoEnduranceNew();
            RegisterMgr.TypePersonalInfoDateOfBirth_Endurance(DateTime.Today.AddYears(-yearsOld));
            RegisterMgr.SelectPersonalInfoGender(gender);
            RegisterMgr.Continue();
            RegisterMgr.SetCustomFieldCheckBox(AgendaParent, true);

            if (gender == Managers.Register.RegisterManager.Gender.Male && Enumerable.Range(18,32).Contains(yearsOld))
            {
                RegisterMgr.SetCustomFieldCheckBox(AgendaConditionalOne, true);
            }

            if (gender == Managers.Register.RegisterManager.Gender.Female && Enumerable.Range(18, 32).Contains(yearsOld))
            {
                RegisterMgr.SetCustomFieldCheckBox(AgendaConditionalTwo, true);
            }

            RegisterMgr.Continue();
            RegisterMgr.ClickCheckoutActiveWaiver();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        [Step]
        private void CreateEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ActiveEuropeEvent);
            StartPage();
            PIPage();
            AgendaPage();
            BuilderMgr.SaveAndClose();
        }

        private void StartPage()
        {
            BuilderMgr.SetStartPage(Managers.Manager.ManagerSiteManager.EventType.ActiveEuropeEvent, EventName);
            BuilderMgr.SaveAndStay();
            eventId = BuilderMgr.GetEventId();
        }

        private void PIPage()
        {
            BuilderMgr.GotoPage(FormDetailManager.Page.PI);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.DateOfBirth, true, true);
            BuilderMgr.SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Gender, true, true);
            BuilderMgr.SaveAndStay();
        }

        private void AgendaPage()
        {
            BuilderMgr.Next();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.AddActivity(AgendaItemManager.AgendaItemType.CheckBox, AgendaParent, null);
            BuilderMgr.AGMgr.ClickSaveItem();
            AddAgendaWithConditionalLogicAndAgeGenderValidation(AgendaConditionalOne, Managers.Register.RegisterManager.Gender.Male);
            AddAgendaWithConditionalLogicAndAgeGenderValidation(AgendaConditionalTwo, Managers.Register.RegisterManager.Gender.Female);
        }

        private void AddAgendaWithConditionalLogicAndAgeGenderValidation(string name, Managers.Register.RegisterManager.Gender gender)
        {
            BuilderMgr.ClickAddActivities();
            BuilderMgr.AGMgr.SetName(name);
            BuilderMgr.AGMgr.SetLimit(2);
            BuilderMgr.AGMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Required);
            BuilderMgr.AGMgr.ExpandConditionalLogic();
            BuilderMgr.AGMgr.SetConditionalLogic(true, AgendaParent);
            BuilderMgr.AGMgr.EnterAgeGenderValidation(gender,
                AgeGreaterThan, AgeGreaterThanDate, AgeLessThan, AgeLessThanDate);
            BuilderMgr.AGMgr.ClickSaveItem();
        }
    }
}
