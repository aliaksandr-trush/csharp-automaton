namespace RegOnline.RegressionTest.Fixtures.FormTypes.ActiveEurope
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class TeamNameFixture : FixtureBase
    {
        private const string EventName = "TeamNameFixture";

        private string[] regTypeNames = new string[] { "U11 Girls", "U12 Girls", "U13 Girls", "U11 Boys", "U12 Boys", "U13 Boys" };
        private string[] piCFMultipleChoices = new string[] { "Striker", "Forward", "Mid Field", "Defender", "Sweeper", "Keeper" };

        private int eventId;

        [Test]
        [Category(Priority.Five)]
        [Description("262")]
        public void TestWithRegTypes()
        {
            // Use endurance event with reg types that collect Team Name
            ConfigReader.DefaultProvider.ReloadAccount(ConfigReader.AccountEnum.ActiveEurope);

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.CreateEvent();
            this.Register();
        }

        private void CreateEvent()
        {
            if (ManagerSiteMgr.EventExists(EventName))
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }
            else
            {
                ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ActiveEuropeEvent);
                this.eventId = BuilderMgr.GetEventId();
                BuilderMgr.SetEventNameAndShortcut(EventName);
                BuilderMgr.SelectEventType(Managers.Builder.FormDetailManager.ActiveEuropeEventType.Soccer);

                foreach (string regTypeName in this.regTypeNames)
                {
                    this.AddRegType(regTypeName);
                }

                BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.PI);
                this.AddCustomField();

                BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Agenda);
                this.AddActivity();

                BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Checkout);
                BuilderMgr.PaymentMethodMgr.AddPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);

                BuilderMgr.SaveAndClose();
            }
        }

        private void AddRegType(string name)
        {
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(name);
            BuilderMgr.RegTypeMgr.SetCollectTeamName(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
        }

        private void AddCustomField()
        {
            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName("What position do you play?");
            BuilderMgr.CFMgr.SetType(Managers.Builder.CustomFieldManager.CustomFieldType.RadioButton);

            foreach (string piCFChoiceItem in this.piCFMultipleChoices)
            {
                BuilderMgr.CFMgr.AddMultiChoiceItem(piCFChoiceItem);
            }

            BuilderMgr.CFMgr.SaveAndClose();
        }

        private void AddActivity()
        {
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.ClickAddActivities();
            BuilderMgr.AGMgr.SetName("Fee per player");
            BuilderMgr.AGMgr.SetType(Managers.Builder.AgendaItemManager.AgendaItemType.AlwaysSelected);
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(5);
        }

        private void Register()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);

            // Generate unique team name
            string uniqueTeam = "teamname{0}";
            string teamName = string.Format(uniqueTeam, System.DateTime.Now.Ticks);

            // Register First person on the Team using random email
            RegisterMgr.Checkin();
            string primaryEmail = RegisterMgr.CurrentEmail;

            // Select Reg Type that collects Team Name according to the day of the week the script is run
            this.SelectRegTypeRandom();
            RegisterMgr.Continue();

            // On Personal Info page, enter a unique Team Name in the Group Name field
            RegisterMgr.EnterPersonalInfoEnduranceNewsletterPartners(null, null);
            RegisterMgr.EnterPersonalInfoEnduranceTeamNameNew(teamName);
            RegisterMgr.EnterPersonalInfoEnduranceForename("First");
            RegisterMgr.EnterPersonalInfoNamePrefixSuffix(null, null, "Middle", null, null);
            RegisterMgr.EnterPersonalInfoEnduranceSurname("Teammate");

            // NOTE: Selecting United States as the country means you need to fill in the US State Field
            RegisterMgr.EnterPersonalInfoEnduranceTitleBadgeCompanyCountry("Player", null, "CSYSA", null);
            RegisterMgr.EnterPersonalInfoEnduranceAddress("123 Penny Lane", "2nd Floor", "Boulder", null, "Colorado", "99701");
            RegisterMgr.EnterPersonalInfoEndurancePhoneNumbers(null, "3035775100", "666", "1234567890", null);

            RegisterMgr.EnterPersonalInfoPassword();

            // Select Position radio button based on the day of the week the script is run
            this.SelectPositionPlayedRandom();

            // Activities page has Always selected item.
            RegisterMgr.Continue();

            // Register second person
            RegisterMgr.ClickAddAnotherPerson();
            this.AddAnother("Player", "second", "teammate");
            this.SelectPositionPlayedRandom();

            // Verify the unique Team Name displays and cannot be edited
            this.ValidateTeamName(teamName);

            // Activities page has Always selected item.
            RegisterMgr.Continue();

            // Register third person 
            RegisterMgr.ClickAddAnotherPerson();
            this.AddAnother("Player", "third", "teammate");
            this.SelectPositionPlayedRandom();

            // Verify the unique Team Name displays and cannot be edited
            this.ValidateTeamName(teamName);

            // Activities page has Always selected item.
            RegisterMgr.Continue();

            // Register fourth person
            RegisterMgr.ClickAddAnotherPerson();
            this.AddAnother("Player", "fourth", "teammate");
            this.SelectPositionPlayedRandom();

            // Verify the unique Team Name displays and cannot be edited
            this.ValidateTeamName(teamName);

            // Activities page has Always selected item.
            RegisterMgr.Continue();

            // Continue to the next step to finalize the group registration
            RegisterMgr.Continue();

            // Finalize
            RegisterMgr.ClickCheckoutActiveWaiver();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Cheque);
            RegisterMgr.FinishRegistration();
            RegisterMgr.CurrentEmail = primaryEmail;
            RegisterMgr.ConfirmRegistration();
        }

        [Step]
        private void AddAnother(string ePrefix, string fName, string lName)
        {
            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            // Enter info for next team mate
            RegisterMgr.EnterPersonalInfoEnduranceForename(fName);
            RegisterMgr.EnterPersonalInfoEnduranceSurname(lName);
        }

        [Verify]
        private void ValidateTeamName(string tName)
        {
            // Verify the unique Team Name displays and cannot be edited
            Assert.IsFalse(WebDriverUtility.DefaultProvider.IsElementPresent("Team Name", LocateBy.Name));
            Assert.IsTrue(WebDriverUtility.DefaultProvider.IsTextPresent(tName));
            bool isEditable = WebDriverUtility.DefaultProvider.IsElementPresent("//td[text()='Team Name:']/../td/input", LocateBy.XPath);

            Assert.IsFalse(isEditable);
        }

        // Get a Reg Type based on random number
        [Step]
        private void SelectRegTypeRandom()
        {
            Random random = new Random();
            int numr = random.Next(0, this.regTypeNames.Length);
            RegisterMgr.SelectRegType(this.regTypeNames[numr]);
        }

        // Get a custom field multiple choice radio button based on random number
        // Note: SelectAgendaItemByName also works on the Personal Info page
        [Step]
        private void SelectPositionPlayedRandom()
        {
            Random random = new Random();
            int num = random.Next(0, this.piCFMultipleChoices.Length);
            RegisterMgr.SelectAgendaItem(this.piCFMultipleChoices[num]);
        }
    }
}