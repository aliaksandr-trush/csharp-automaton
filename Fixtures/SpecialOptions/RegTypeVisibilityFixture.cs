namespace RegOnline.RegressionTest.Fixtures.SpecialOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegTypeVisibilityFixture : FixtureBase
    {
        private const string EventName = "RegTypeVisibility";
        private const double EventFee = 10;
        private const double RegTypeFee = EventFee;
        private const double AgendaFee = 10;
        private const string RegTypeName = "RegType";

        private int eventID = ManagerBase.InvalidId;
        private string sessionID;
        private string firstPrimaryEmailAddress;
        private string secondPrimaryEmailAddress;
        private string eventWebsiteURL;
        private string regTypeDirectLink;

        private enum AgendaItem
        {
            VisibleForAll = 0,
            VisibleForRegType = 1,
        }

        #region Test methods
        [Test]
        [Category(Priority.Two)]
        [Description("650")]
        public void RegTypeVisibility()
        {
            // Step #1
            this.CreateNewEvent();

            // Step #2
            // Register for event, complete registration without selecting agenda item, should pay $10
            this.AddTheFirstRegistration();

            // Step #3
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventID, this.sessionID);

            // Add the first reg type
            this.AddRegType();
            BuilderMgr.SaveAndStay();

            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);

            // Add agenda item only visible for RegType
            this.AddAgendaItemOnlyForNewRegType();
            BuilderMgr.SaveAndClose();

            // Step #4 and #5 and #6
            // Update registration, select the agenda item,add a new group member,do not choose any agenda items - complete reg with $20 adjustment
            this.UpdateTheFirstRegistrationAndAddTheSecondGroupMember();

            // Step #7
            //M3.OpenEventBuilderStartPage(this.eventID, this.sessionID);

            // Get event website url
            //this.eventWebsiteURL = Builder.GetEventWebsiteURL();

            // Register for event from the event website- choose no agenda items, pay $10
            this.AddTheSecondRegistrationThroughEventWebSite();

            // Step #8
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventID, this.sessionID);

            // Edit event, make reg type not visible to public
            this.EditRegTypeNotVisibleToPublic();

            // Step #9
            // Open registration page, verify that no reg types are visible
            this.VerifyRegTypeUnavailableWhenTestReg();

            // Step #10
            // Open admin reg page, verify that reg type is present
            this.VerifyRegTypeNumberWhenAdminReg(1);

            // Step #11
            // Open direct link copied from builder, register, don't choose any agenda -pay $10
            this.AddTheThirdRegistrationThroughDirectLink();

            // Step #12
            // Update first primary, remove one agenda item from primary, add two agenda items to secondary, add another person with no reg type, pay with additional $20
            this.UpdateTheFirstRegistrationAndAddTheThirdGroupMember();

            // Step #13
            // Update second primary, select regtype-specific agenda item, complete with additional $10
            this.UpdateSecondPrimarySelectRegTypeSpecificAgendaItem();

            // Step #14
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventID, this.sessionID);

            // Change reg type to be not visible to Admin
            this.EditRegTypeNotVisibleToAdmin();

            // Step #15
            // Open registration page, verify that no reg types are visible
            this.VerifyRegTypeUnavailableWhenTestReg();

            // Select non-regtype-specific agenda item - verify two present, complete with additional $10
            this.UpdateSecondPrimarySelectNonRegTypeSpecificAgendaItem();

            // Step #16
            // Open admin reg page, verify that no reg type is present
            this.VerifyRegTypeNumberWhenAdminReg(0);

            // Step #17
            // Attempt to open direct link copied from builder
            UIUtilityProvider.UIHelper.OpenUrl(this.regTypeDirectLink);

            // Verify error message
            RegisterMgr.VerifyIncorrectURL(EventName);
        }
        #endregion

        #region Create and Edit Event Helper Methods
        [Step]
        private void LoginAndGetSessionID()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.sessionID = BuilderMgr.GetEventSessionId();
        }

        [Step]
        private void CreateNewEvent()
        {
            this.LoginAndGetSessionID();

            // Always create new event
            ManagerSiteMgr.DeleteEventByName(EventName);
           
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            this.SetEventStartPage();
            this.eventWebsiteURL = BuilderMgr.GetEventWebsiteUrl();
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
            this.SetAgendaPage();
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
        }

        private void SetAgendaPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.VerifyEventAgendaPage();

            // Add the check-box agenda item
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(
                AgendaItemManager.AgendaItemType.CheckBox, 
                AgendaItem.VisibleForAll.ToString(), 
                AgendaFee);
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

        #region Registrations
        [Step]
        private void AddTheFirstRegistration()
        {
            RegisterMgr.OpenRegisterPage(this.eventID);

            // Verify no reg type and only one agenda item
            this.RegisterToAgenda(0, 1);
           
            // Select no agenda item
            RegisterMgr.Continue();
            this.firstPrimaryEmailAddress = RegisterMgr.CurrentEmail;

            this.PayOrUpdateToConfirmation(EventFee);
        }

        [Step]
        private void UpdateTheFirstRegistrationAndAddTheSecondGroupMember()
        {
            double totalFee = EventFee;

            RegisterMgr.OpenRegisterPage(eventID);

            // Update first primary - verify one reg type and one agenda item
            this.UpdatePrimaryToAgenda(this.firstPrimaryEmailAddress, 1, 1);

            // Select agenda item
            RegisterMgr.SelectAgendaItems();
            totalFee += AgendaFee;

            // Add another person
            RegisterMgr.ClickAddAnotherPerson();
            totalFee += RegTypeFee;

            // Verify only one reg type and two agenda items
            this.RegisterToAgenda(1, 2);

            // Select no agenda item
            RegisterMgr.Continue();
            RegisterMgr.CurrentEmail = this.firstPrimaryEmailAddress;

            this.PayOrUpdateToConfirmation(totalFee);
        }

        [Step]
        private void AddTheSecondRegistrationThroughEventWebSite()
        {
            RegisterMgr.OpenRegisterPage(this.eventID, this.eventWebsiteURL);

            // Verify one reg type and two agenda items
            this.RegisterToAgenda(1, 2);

            // Select no agenda item
            RegisterMgr.Continue();
            this.secondPrimaryEmailAddress = RegisterMgr.CurrentEmail;
            
            this.PayOrUpdateToConfirmation(RegTypeFee);
        }

        [Step]
        private void AddTheThirdRegistrationThroughDirectLink()
        {
            RegisterMgr.OpenRegisterPage(this.eventID, this.regTypeDirectLink);

            // Verify no reg type and two agenda items
            this.RegisterToAgenda(0, 2);

            // Select no agenda item
            RegisterMgr.Continue();

            this.PayOrUpdateToConfirmation(RegTypeFee);
        }

        [Step]
        private void UpdateTheFirstRegistrationAndAddTheThirdGroupMember()
        {
            double totalFee = EventFee + AgendaFee + RegTypeFee;

            RegisterMgr.OpenRegisterPage(this.eventID);

            // Update first primary,verify no reg type and only one ageda item present
            this.UpdatePrimaryToAgenda(this.firstPrimaryEmailAddress, 0, 1);

            // Unselect one agenda item from primary
            RegisterMgr.SetCustomFieldCheckbox(AgendaItem.VisibleForAll.ToString(), false);
            totalFee -= AgendaFee;
            RegisterMgr.Continue();

            // Update secondary,add two agenda items - verify two present
            RegisterMgr.ClickEditAgendaLink(1);
            RegisterMgr.VerifyCountOfAgendaItems(2);
            RegisterMgr.SelectAgendaItem(AgendaItem.VisibleForAll.ToString());
            RegisterMgr.SelectAgendaItem(AgendaItem.VisibleForRegType.ToString());
            totalFee += AgendaFee * 2;
            RegisterMgr.Continue();

            RegisterMgr.ClickAddAnotherPerson();
            totalFee += EventFee;

            // Verify no reg type offered and only one agenda item present
            this.RegisterToAgenda(0, 1);

            // Select no agenda item 
            RegisterMgr.Continue();
            RegisterMgr.CurrentEmail = this.firstPrimaryEmailAddress;

            this.PayOrUpdateToConfirmation(totalFee);
        }

        [Step]
        private void UpdateSecondPrimarySelectRegTypeSpecificAgendaItem()
        {
            double totalFee = RegTypeFee;

            RegisterMgr.OpenRegisterPage(this.eventID);

            // Update second primary - verify no reg type and two agenda item
            this.UpdatePrimaryToAgenda(this.secondPrimaryEmailAddress, 0, 2);

            // Select regtype-specific agenda item (visible for RegType)
            RegisterMgr.SelectAgendaItem(AgendaItem.VisibleForRegType.ToString());
            totalFee += AgendaFee;
            RegisterMgr.Continue();

            RegisterMgr.Continue();
            this.PayOrUpdateToConfirmation(totalFee);
        }

        [Step]
        private void UpdateSecondPrimarySelectNonRegTypeSpecificAgendaItem()
        {
            double totalFee = RegTypeFee + AgendaFee;

            RegisterMgr.OpenRegisterPage(this.eventID);
            // Update second primary - verify no reg type and two agenda item
            this.UpdatePrimaryToAgenda(this.secondPrimaryEmailAddress, 0, 2);

            // Select non-regtype-specific agenda item (visible for all)
            RegisterMgr.SelectAgendaItem(AgendaItem.VisibleForAll.ToString());
            totalFee += AgendaFee;
            RegisterMgr.Continue();

            RegisterMgr.Continue();
            this.PayOrUpdateToConfirmation(totalFee);
        }
        #endregion

        #region Builder Helper Methods
        [Step]
        private void AddRegType()
        {
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(RegTypeName);
            BuilderMgr.RegTypeMgr.ExpandAdvancedSection();
            BuilderMgr.RegTypeMgr.VerifyFee(EventFee);
            BuilderMgr.RegTypeMgr.SaveAndClose();
        }

        [Step]
        private void AddAgendaItemOnlyForNewRegType()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(AgendaItem.VisibleForRegType.ToString());
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.CheckBox);
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(AgendaFee);
            BuilderMgr.AGMgr.ShowAllRegTypes();
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, RegTypeName);
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        [Step]
        private void EditRegTypeNotVisibleToPublic()
        {
            BuilderMgr.OpenRegType(RegTypeName);
            BuilderMgr.RegTypeMgr.ExpandAdvancedSection();
            BuilderMgr.RegTypeMgr.SetVisibility(RegTypeManager.VisibilityOption.Public, false);
            this.regTypeDirectLink = BuilderMgr.RegTypeMgr.GetRegTypeDirectLink();
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndStay();
        }

        [Step]
        private void EditRegTypeNotVisibleToAdmin()
        {
            BuilderMgr.OpenRegType(RegTypeName);
            BuilderMgr.RegTypeMgr.ExpandAdvancedSection();
            BuilderMgr.RegTypeMgr.SetVisibility(RegTypeManager.VisibilityOption.Admin, false);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndStay();
        }
        #endregion

        #region Registration Helper Methods
        [Step]
        private void VerifyRegTypeUnavailableWhenTestReg()
        {
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.VerifyHasRegTypes(false);
        }

        [Step]
        private void VerifyRegTypeNumberWhenAdminReg(int regTypeNumber)
        {
            RegisterMgr.OpenAdminRegisterPage(this.eventID, this.sessionID);
            RegisterMgr.VerifyCountOfRegTypes(regTypeNumber);
        }

        private void RegisterToAgenda(int regTypesCount, int agendaItemsCount)
        {
            RegisterMgr.VerifyCountOfRegTypes(regTypesCount);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.VerifyCountOfAgendaItems(agendaItemsCount);
        }

        private void UpdatePrimaryToAgenda(string emailAddress,int regTypesCount, int agendaItemsCount)
        {
            RegisterMgr.VerifyCountOfRegTypes(regTypesCount);
            
            RegisterMgr.CheckinWithEmail(emailAddress);
            RegisterMgr.Continue();
            RegisterMgr.EnterPassword(ConfigurationProvider.XmlConfig.AccountConfiguration.Password);
            RegisterMgr.Continue();

            // Update agenda
            RegisterMgr.ClickEditAgendaLink(0);

            // Verify count of agenda items
            RegisterMgr.VerifyCountOfAgendaItems(agendaItemsCount);
        }

        private void PayOrUpdateToConfirmation(double total)
        {
            RegisterMgr.PayMoneyAndVerify(total, RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
        #endregion
    }
}
