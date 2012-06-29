namespace RegOnline.RegressionTest.Fixtures.RegisterAndBuilder.AgendaPage
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class GroupDiscountFixture : FixtureBase
    {
        #region Constants
        private const string EventName = "GroupDiscountRule";
        private const int InvalidEventID = -1;
        private const double EventFee = 10;
        private const double GroupDiscountPrice = 50;
        private const double RegTypeFee = 10;
        private const string RegTypeOne = "RegType1";
        private const string RegTypeTwo = "RegType2";
        private const double AgendaFee = 10;
        private const string AgendaItemName = "AgendaItem1";
        #endregion

        #region Private fields
        private int eventID = InvalidEventID;
        private string sessionID;
        private RegType regTypeOne;
        private RegType regTypeTwo;
        #endregion

        private class RegType
        {
            public string Name { get; set; }
            public bool HasDiscount { get; set; }

            public RegType()
            {
            }

            public RegType(string name, bool hasDiscount)
            {
                this.Name = name;
                this.HasDiscount = hasDiscount;
            }
        }

        public GroupDiscountFixture()
        {
            this.regTypeOne = new RegType(RegTypeOne, false);
            this.regTypeTwo = new RegType(RegTypeTwo, false);
        }

        #region Test Methods
        [Test]
        [Category(Priority.Two)]
        [Description("435")]
        public void CheckListTwoOrMorePercentTest()
        {
            this.CreateNewEvent();
            this.GroupRegWithNoRegTypeNoAgendaItemAndGroupDiscountRuleApplyToAllItem();

            this.AddRegTypesAndApplyGroupDiscountRuleToOneRegType(this.regTypeOne);
            this.GroupRegWithRegTypeAndNoAgendaItemAndGroupDiscountRuleApplyToOneRegType(regTypeOne);
            this.GroupRegWithRegTypeAndNoAgendaItemAndGroupDiscountRuleApplyToOneRegType(regTypeTwo);

            this.AddAgendaItemAndApplyGroupDiscountRuleOnlyToAgendaItem();
            this.GroupRegWithRegTypeAndAgendaItemAndGroupDiscountRuleApplyToAgendaItem();
        }
        #endregion

        #region Create Event Methods
        [Step]
        private void CreateNewEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionID = BuilderMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            this.SetEventStartPage();
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            this.SetCheckoutPage();
            BuilderMgr.Next();
            this.SetConfirmationPage();
            BuilderMgr.SaveAndClose();
        }

        private void SetEventStartPage()
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName);
            BuilderMgr.SetEventFee(EventFee);
            BuilderMgr.SaveAndStay();

            BuilderMgr.SetGroupRegistration(true);

            // Set group discount rule
            BuilderMgr.OpenDiscountRule();

            BuilderMgr.GroupDiscountRuleMgr.SetGroupDiscountRule(
                2,
                GroupDiscountRuleManager.GroupSizeOption.SizeOrMore,
                GroupDiscountPrice, 
                GroupDiscountRuleManager.DiscountType.Percent,
                GroupDiscountRuleManager.AdditionalRegOption.All,
                null );

            BuilderMgr.SaveAndStay();
        }

        private void SetCheckoutPage()
        {
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndStay();
        }

        private void SetConfirmationPage()
        {
            BuilderMgr.SetEventConfirmationPage();
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventConfirmationPage();
        }
        #endregion

        #region Update Event Methods
        [Step]
        private void AddRegTypesAndApplyGroupDiscountRuleToOneRegType(RegType setDiscount)
        {
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventID, this.sessionID);
            BuilderMgr.AddRegTypeWithEventFee(RegTypeOne, RegTypeFee);
            BuilderMgr.AddRegTypeWithEventFee(RegTypeTwo, RegTypeFee);

            BuilderMgr.OpenDiscountRule();
            //BuilderMgr.GroupDiscount.ExpandApplyRuleToAllItems();
            BuilderMgr.GroupDiscountRuleMgr.SetApplyRuleToAll(false);
            BuilderMgr.GroupDiscountRuleMgr.SetApplyRuleToItemByName(true, string.Format(GroupDiscountRuleManager.RegTypeFeeDefultName, setDiscount.Name));
            setDiscount.HasDiscount = true;

            BuilderMgr.GroupDiscountRuleMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void AddAgendaItemAndApplyGroupDiscountRuleOnlyToAgendaItem()
        {
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventID, this.sessionID);
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
            this.AddAgendaItem(AgendaItemName, AgendaFee);
            BuilderMgr.SaveAndStay();

            BuilderMgr.GotoPage(FormDetailManager.Page.Start);
            BuilderMgr.OpenDiscountRule();
            BuilderMgr.GroupDiscountRuleMgr.SetApplyRuleToAll(true);
            BuilderMgr.GroupDiscountRuleMgr.SetApplyRuleToAll(false);
            BuilderMgr.GroupDiscountRuleMgr.SetApplyRuleToItemByName(true, AgendaItemName);

            BuilderMgr.GroupDiscountRuleMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();
        }

        private void AddAgendaItem(string name, double price)
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.VerifyEventAgendaPage();
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.CheckBox, name, price);
        }
        #endregion

        #region Registrations
        [Step]
        private void GroupRegWithNoRegTypeNoAgendaItemAndGroupDiscountRuleApplyToAllItem()
        {
            string primaryEmail = string.Empty;
            int regCount = 0;
            double groupSaving = 0;
            double totalFee = 0;

            RegisterMgr.CurrentEventId = this.eventID;
            RegisterMgr.OpenRegisterPage(this.eventID);
            regCount++;

            // Register check in
            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            // Enter profile info
            RegisterMgr.EnterProfileInfo();
            primaryEmail = RegisterMgr.CurrentEmail;

            // Add another person
            RegisterMgr.ClickAddAnotherPerson();
            regCount++;

            // Register check in
            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            // Enter profile info and verify
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.CurrentEmail = primaryEmail;

            // Verify group discount save
            totalFee = regCount * EventFee;
            groupSaving += GroupDiscountPrice / 100 * totalFee;
            RegisterMgr.VerifyCheckoutGroupDiscountSavings(groupSaving);

            // Pay money and verify
            RegisterMgr.PayMoneyAndVerify(totalFee - groupSaving, RegisterManager.PaymentMethod.Check);

            // Finish registration and check the registration details
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        [Step]
        private void GroupRegWithRegTypeAndNoAgendaItemAndGroupDiscountRuleApplyToOneRegType(RegType regType)
        {
            string primaryEmail = string.Empty;
            int regCount = 0;
            double groupSaving = 0;
            double totalFee = 0;

            // Initialize Registration
            RegisterMgr.CurrentEventId = this.eventID;

            // Start new registration
            RegisterMgr.OpenRegisterPage(this.eventID);
            regCount++;

            // Register check in
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(regType.Name);
            RegisterMgr.Continue();

            // Enter profile info
            RegisterMgr.EnterProfileInfo();
            primaryEmail = RegisterMgr.CurrentEmail;

            // Add another person
            RegisterMgr.ClickAddAnotherPerson();
            regCount++;

            // Register check in
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(regType.Name);
            RegisterMgr.Continue();

            // Enter profile info and verify
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.CurrentEmail = primaryEmail;

            // Verify group discount save
            RegisterMgr.VerifyHasDiscountMessage(regType.HasDiscount);

            totalFee = regCount * RegTypeFee;

            if (regType.HasDiscount)
            {
                groupSaving += GroupDiscountPrice / 100 * totalFee;
                RegisterMgr.VerifyCheckoutGroupDiscountSavings(groupSaving);
            }

            // Pay money and verify
            if (regType.HasDiscount)
            {
                totalFee -= groupSaving;
            }

            RegisterMgr.PayMoneyAndVerify(totalFee, RegisterManager.PaymentMethod.Check);

            // Finish registration and check the registration details
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        [Step]
        private void GroupRegWithRegTypeAndAgendaItemAndGroupDiscountRuleApplyToAgendaItem()
        {
            string primaryEmail = string.Empty;
            int regCount = 0;
            double groupSaving = 0;
            double totalFee = 0;

            // Initialize Registration
            RegisterMgr.CurrentEventId = this.eventID;

            // Start new registration
            RegisterMgr.OpenRegisterPage(this.eventID);
            regCount++;

            // Register check in
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(RegTypeOne);
            RegisterMgr.Continue();

            // Enter profile info
            RegisterMgr.EnterProfileInfo();
            primaryEmail = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();

            // Select agenda item
            RegisterMgr.SelectAgendaItems();

            // Add another person
            RegisterMgr.ClickAddAnotherPerson();
            regCount++;

            // Register check in
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(RegTypeTwo);
            RegisterMgr.Continue();

            // Enter profile info and verify
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            // Select agenda item
            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();
            RegisterMgr.CurrentEmail = primaryEmail;

            // Verify group discount save
            totalFee += regCount * (RegTypeFee + AgendaFee);
            groupSaving += GroupDiscountPrice / 100 * regCount * AgendaFee;
            RegisterMgr.VerifyCheckoutGroupDiscountSavings(groupSaving);

            // Pay money and verify
            RegisterMgr.PayMoneyAndVerify(totalFee - groupSaving, RegisterManager.PaymentMethod.Check);

            // Finish registration and check the registration details
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
        #endregion

        private void LoginAndGetSessionID()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionID = BuilderMgr.GetEventSessionId();
        }
    }
}
