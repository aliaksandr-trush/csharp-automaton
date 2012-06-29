namespace RegOnline.RegressionTest.Fixtures.RegisterAndBuilder.OtherFormTypeSpecificTests.ActiveEurope
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
        [Test]
        [Category(Priority.Five)]
        [Description("262")]
        public void TestWithRegTypes()
        {
            // Use endurance event with reg types that collect Team Name- Use Event 614068 on ActiveEuropeBeta Account 377431
            ConfigurationProvider.XmlConfig.ReloadAccount(XmlConfiguration.AccountType.ActiveEurope);
            RegisterMgr.OpenRegisterPage(632819);

            // Generate unique team name
            string uniqueTeam = "teamname{0}";
            string teamName = string.Format(uniqueTeam, System.DateTime.Now.Ticks);
            // Generate array of Reg Types
            string[] regName = new string[] { "U11 Girls", "U12 Girls", "U13 Girls", "U11 Boys", "U12 Boys", "U13 Boys" };
            // Generate array of custom field radio button selections
            string[] cfPositionPlayed = new string[] { "Striker", "Forward", "Mid Field", "Defender", "Sweeper", "Keeper" };

            // Register First person on the Team using random email
            RegisterMgr.Checkin();
            string primaryEmail = RegisterMgr.CurrentEmail;

            // Select Reg Type that collects Team Name according to the day of the week the script is run
            GetRegTypeRandom(0, 6, regName);
            RegisterMgr.Continue();

            // On Personal Info page, enter a unique Team Name in the Group Name field
            RegisterMgr.EnterPersonalInfoEnduranceNewsletterPartners(null, null);
            RegisterMgr.EnterPersonalInfoEnduranceTeamNameNew(teamName);
            RegisterMgr.EnterPersonalInfoEndurancePrefix("Player");
            RegisterMgr.EnterPersonalInfoEnduranceForename("First");
            // To get middle name and Suffix, use the original Method and null the other fields)
            RegisterMgr.EnterPersonalInfoNamePrefixSuffix(null, null, "Middle", null, "Esquire");
            RegisterMgr.EnterPersonalInfoEnduranceSurname("Teammate");
            // NOTE: Selecting United States as the country means you need to fill in the US State Field
            RegisterMgr.EnterPersonalInfoEnduranceTitleBadgeCompanyCountry("Player", "Player", "CSYSA", "United States");
            RegisterMgr.EnterPersonalInfoEnduranceAddress("123 Penny Lane", "2nd Floor", "Boulder", "Colorado", null, "80301");
            RegisterMgr.EnterPersonalInfoEndurancePhoneNumbers("3033033333", "3035775100", "666", "1234567890", "9876543210");
            // Additional fields
            ////RegisterMgr.EnterPersonalInfoDateOfBirthGender("01/08/1969", "Male");
            RegisterMgr.TypePersonalInfoDateOfBirth_Endurance(new DateTime(1969, 1, 8));
            RegisterMgr.SelectPersonalInfoGender(Managers.Register.RegisterManager.Gender.Male);
            RegisterMgr.EnterPersonalInfoEnduranceNationality("Italy");
            RegisterMgr.EnterPersonalInfoTaxNumberMembershipNumberCustomerNumber("tax12345", "666666", "999999");

            RegisterMgr.EnterPersonalInfoPassword("zzzzzz");


            // Select Position radio button based on the day of the week the script is run
            //GetPositionPlayed();
            GetPositionPlayedRandom(0, 6, cfPositionPlayed);

            // Activities page has Always selected item.
            RegisterMgr.Continue();

            // Register second person
            RegisterMgr.ClickAddAnotherPerson();
            AddSecondary("Player", "second", "teammate");
            GetPositionPlayedRandom(cfPositionPlayed);
            // Verify the unique Team Name displays and cannot be edited
            ValidateTeamName(teamName);
            // Activities page has Always selected item.
            RegisterMgr.Continue();

            // Register third person 
            RegisterMgr.ClickAddAnotherPerson();
            AddSecondary("Player", "third", "teammate");
            GetPositionPlayedRandom(cfPositionPlayed);
            // Verify the unique Team Name displays and cannot be edited
            ValidateTeamName(teamName);
            // Activities page has Always selected item.
            RegisterMgr.Continue();

            // Register fourth person
            RegisterMgr.ClickAddAnotherPerson();
            AddSecondary("Player", "fourth", "teammate");
            GetPositionPlayedRandom(cfPositionPlayed);
            // Verify the unique Team Name displays and cannot be edited
            ValidateTeamName(teamName);
            // Activities page has Always selected item.
            RegisterMgr.Continue();

            // Continue to the next step to finalize the group registration
            RegisterMgr.Continue();

            // Finalize
            //Click("ctl00_cph_chkAITerms");
            RegisterMgr.ClickCheckoutActiveWaiver();
            ////RegisterMgr.PaymentMgr.EnterCreditCardNumberInfo("4444444444444448", "123", "10 - Oct", "2013");
            ////RegisterMgr.PaymentMgr.EnterCreditCardNameCountryType(null, "United States", null);
            ////RegisterMgr.PaymentMgr.EnterCreditCardAddressInfo(null, null, null, "CO", null);
            RegisterMgr.FinishRegistration();
            RegisterMgr.CurrentEmail = primaryEmail;
            RegisterMgr.ConfirmRegistration();
        }

        //AddSecondary(string firstName, string lastName, bool addAnother);
        [Step]
        private void AddSecondary(string ePrefix, string fName, string lName)
        {
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            // Enter info for next team mate
            RegisterMgr.EnterPersonalInfoEndurancePrefix(ePrefix);
            RegisterMgr.EnterPersonalInfoEnduranceForename(fName);
            RegisterMgr.EnterPersonalInfoEnduranceSurname(lName);

        }

        //ValidateTeamName(string teamName)
        [Verify]
        private void ValidateTeamName(string tName)
        {
            // Verify the unique Team Name displays and cannot be edited
            Assert.IsFalse(UIUtilityProvider.UIHelper.IsElementPresent("Team Name", LocateBy.Name));
            Assert.IsTrue(UIUtilityProvider.UIHelper.IsTextPresent(tName));
            //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
            bool isEditable = UIUtilityProvider.UIHelper.IsElementPresent("//td[text()='Team Name:']/../td/input", LocateBy.XPath);
            //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(M3.TimeOutSpan));

            Assert.IsFalse(isEditable);
        }

        //Get a Reg Type based on random number
        [Step]
        private void GetRegTypeRandom(int min, int max, string[] regName)
        {
            Random random = new Random();
            int numr = random.Next(min, max);
            RegisterMgr.SelectRegType(regName[numr]);
        }

        #region GetPositionPlayedRandom

        //Get a custom field multiple choice radio button based on random number
        //  Note: SelectAgendaItemByName also works on the Personal Info page
        [Step]
        private void GetPositionPlayedRandom(int min, int max, string[] cfPositionPlayed)
        {
            Random random = new Random();
            int num = random.Next(min, max);
            RegisterMgr.SelectAgendaItem(cfPositionPlayed[num]);
        }

        [Step]
        private void GetPositionPlayedRandom(string[] cfPositionPlayed)
        {
            int min = 0;
            int max = cfPositionPlayed.Length - 1;
            GetPositionPlayedRandom(min, max, cfPositionPlayed);
        }

        #endregion
    }
}