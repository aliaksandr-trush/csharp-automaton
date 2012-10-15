namespace RegOnline.RegressionTest.Fixtures.SpecialOptions
{
    using System.Collections.Generic;
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
    public class RequiredItemsGroupFixture : FixtureBase
    {
        private const string EventName = "RequiredItemsGroup- ProEvent";
        private const int PICFQuantity = 4;
        private const int AGCFQuantity = 4;
        private const int LTLodgingCFQuantity = 4;
        private const int LTTravelCFQuantity = 4;
        private const int LTPreferencesCFQuantity = 4;
        private readonly List<string> ErrorMessagesListPIPage;
        private readonly List<string> ErrorMessagesListAGPage;
        private readonly List<string> ErrorMessagesListLTPageLodging;
        private readonly List<string> ErrorMessagesListLTPageTravel;
        private readonly List<string> ErrorMessagesListLTPagePreferences;

        private enum ItemTypeList
        {
            PersonalInfo,
            AgendaItem,
            Travel,
            Lodging,
            Preferences
        }

        private int eventID = ManagerBase.InvalidId;
        private string sessionId = string.Empty;

        public RequiredItemsGroupFixture()
        {
            ErrorMessagesListPIPage = new List<string>(PICFQuantity);

            for (int cnt = 0; cnt < PICFQuantity; cnt++)
            {
                ErrorMessagesListPIPage.Add(RegisterManager.Error.SelectAllRequiredFields);
            }

            ErrorMessagesListAGPage = new List<string>(AGCFQuantity);

            for (int cnt = 0; cnt < AGCFQuantity; cnt++)
            {
                ErrorMessagesListAGPage.Add(RegisterManager.Error.SelectAllRequiredFields);
            }

            ErrorMessagesListLTPageLodging = new List<string>(LTLodgingCFQuantity);

            for (int cnt = 0; cnt < LTLodgingCFQuantity; cnt++)
            {
                ErrorMessagesListLTPageLodging.Add(RegisterManager.Error.SelectAllRequiredFields);
            }

            ErrorMessagesListLTPageTravel = new List<string>(LTTravelCFQuantity);

            for (int cnt = 0; cnt < LTTravelCFQuantity; cnt++)
            {
                ErrorMessagesListLTPageTravel.Add(RegisterManager.Error.SelectAllRequiredFields);
            }

            ErrorMessagesListLTPagePreferences = new List<string>(LTPreferencesCFQuantity);

            for (int cnt = 0; cnt < LTPreferencesCFQuantity; cnt++)
            {
                ErrorMessagesListLTPagePreferences.Add(RegisterManager.Error.SelectAllRequiredFields);
            }
        }

        [Test]
        [Category(Priority.Three)]
        [Description("224")]
        public void RequiredItemsGroup()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            ManagerSiteMgr.DeleteEventByName(EventName);
            this.CreateEvent();
            RegisterMgr.OpenRegisterPage(eventID);

            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            RegisterMgr.EnterProfileInfo();
            RegisterMgr.ContinueWithErrors();
            RegisterMgr.VerifyErrorMessagesCount(PICFQuantity);
            RegisterMgr.VerifyErrorMessages(ErrorMessagesListPIPage);
            RegisterMgr.SelectPersonalInfoCustomFields();
            RegisterMgr.EnterPersonalInfoPassword();
            RegisterMgr.Continue();

            RegisterMgr.ContinueWithErrors();
            RegisterMgr.VerifyErrorMessagesCount(AGCFQuantity);
            RegisterMgr.VerifyErrorMessages(ErrorMessagesListAGPage);
            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();

            RegisterMgr.ContinueWithErrors();
            RegisterMgr.VerifyErrorMessagesCount(LTLodgingCFQuantity + LTTravelCFQuantity + LTPreferencesCFQuantity);
            RegisterMgr.VerifyErrorMessages(ErrorMessagesListLTPageLodging);
            RegisterMgr.VerifyErrorMessages(ErrorMessagesListLTPageTravel);
            RegisterMgr.VerifyErrorMessages(ErrorMessagesListLTPagePreferences);
            RegisterMgr.SelectTravelCustomFields();
            RegisterMgr.ContinueWithErrors();
            RegisterMgr.VerifyErrorMessagesCount(LTLodgingCFQuantity + LTPreferencesCFQuantity);
            RegisterMgr.VerifyErrorMessages(ErrorMessagesListLTPageLodging);
            RegisterMgr.VerifyErrorMessages(ErrorMessagesListLTPagePreferences);
            RegisterMgr.SelectLodgingCustomFields();
            RegisterMgr.SelectPreferencesCustomFields();
            RegisterMgr.Continue();

            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        public void CreateEvent()
        {
            ManagerSiteMgr.OpenLogin();

            // login to regression testing account
            ManagerSiteMgr.Login();

            // make sure we're on the Events tab
            ManagerSiteMgr.GoToEventsTabIfNeeded();

            // click "Regression" folder
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();

            // delete event
            ManagerSiteMgr.DeleteEventByName(EventName);

            // select "Pro Event" from drop down
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

            // set up Start page
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName);
            BuilderMgr.SaveAndStay();
            BuilderMgr.GotoPage(FormDetailManager.Page.PI);

            // set up Personal Information page
            AddRequiredGroupItem(ItemTypeList.PersonalInfo, "CFPI_Group", "CFPI_One");
            AddRequiredGroupItem(ItemTypeList.PersonalInfo, "CFPI_Group", "CFPI_Two");
            AddRequiredGroupItem(ItemTypeList.PersonalInfo, "CFPI_Group", "CFPI_Three");
            AddRequiredGroupItem(ItemTypeList.PersonalInfo, "CFPI_Group", "CFPI_Four");
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);

            // set up Agenda page
            // add agenda group
            BuilderMgr.ClickYesOnSplashPage();
            AddRequiredGroupItem(ItemTypeList.AgendaItem, "AG_Group", "AG_One");
            AddRequiredGroupItem(ItemTypeList.AgendaItem, "AG_Group", "AG_Two");
            AddRequiredGroupItem(ItemTypeList.AgendaItem, "AG_Group", "AG_Three");
            AddRequiredGroupItem(ItemTypeList.AgendaItem, "AG_Group", "AG_Four");
            BuilderMgr.GotoPage(FormDetailManager.Page.LodgingTravel);

            // set up Lodging page
            BuilderMgr.ClickYesOnSplashPage();
            AddRequiredGroupItem(ItemTypeList.Lodging, "CFL_Group", "CFL_One");
            AddRequiredGroupItem(ItemTypeList.Lodging, "CFL_Group", "CFL_Two");
            AddRequiredGroupItem(ItemTypeList.Lodging, "CFL_Group", "CFL_Three");
            AddRequiredGroupItem(ItemTypeList.Lodging, "CFL_Group", "CFL_Four");
            AddRequiredGroupItem(ItemTypeList.Travel, "CFT_Group", "CFT_One");
            AddRequiredGroupItem(ItemTypeList.Travel, "CFT_Group", "CFT_Two");
            AddRequiredGroupItem(ItemTypeList.Travel, "CFT_Group", "CFT_Three");
            AddRequiredGroupItem(ItemTypeList.Travel, "CFT_Group", "CFT_Four");
            AddRequiredGroupItem(ItemTypeList.Preferences, "CFP_Group", "CFP_One");
            AddRequiredGroupItem(ItemTypeList.Preferences, "CFP_Group", "CFP_Two");
            AddRequiredGroupItem(ItemTypeList.Preferences, "CFP_Group", "CFP_Three");
            AddRequiredGroupItem(ItemTypeList.Preferences, "CFP_Group", "CFP_Four");
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);

            // set up Checkout page
            BuilderMgr.EnterEventCheckoutPage();

            // get event id
            eventID = BuilderMgr.GetEventId();

            //get sessionId
            sessionId = BuilderMgr.GetEventSessionId();

            // save and close
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void AddRequiredGroupItem(ItemTypeList itemType, string group, string name)
        {
            if (itemType == ItemTypeList.AgendaItem)
            {
                BuilderMgr.ClickAddAgendaItem();
                BuilderMgr.AGMgr.SetName(name);
                BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.CheckBox);
                BuilderMgr.AGMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Required);
                UIUtil.DefaultProvider.ExpandAdvanced();
                BuilderMgr.AGMgr.SetGroupName(group);
                BuilderMgr.AGMgr.ClickSaveItem();
            }
            else
            {
                switch (itemType)
                {
                    case ItemTypeList.PersonalInfo:
                        BuilderMgr.ClickAddPICustomField();
                        break;
                    case ItemTypeList.Travel:
                        BuilderMgr.ClickAddTravelCustomField();
                        break;
                    case ItemTypeList.Lodging:
                        BuilderMgr.ClickAddLodgingCustomField();
                        break;
                    case ItemTypeList.Preferences:
                        BuilderMgr.ClickAddPreferencesCustomField();
                        break;
                }
                BuilderMgr.CFMgr.SetName(name);
                BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.CheckBox);
                BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Required);
                UIUtil.DefaultProvider.ExpandAdvanced();
                BuilderMgr.CFMgr.SetGroupName(group);
                BuilderMgr.CFMgr.SaveAndClose();
            }
        }
    }
}
