namespace RegOnline.RegressionTest.Fixtures.RegisterAndBuilder.AgendaPage
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgendaVisibilityFixture : FixtureBase
    {
        private int EventID = 0;
        private string SessionId = string.Empty;
        private string ProEventName = "AgendaVisibility";
        private string RTNameAll = AgendaItemManager.VisibilityAllRegTypes;
        private string RTNameOne = "Odd";
        private string RTNameTwo = "Even";

        [Test]
        [Category(Priority.Three)]
        [Description("693")]
        public void SetAgendaVisibility_NoRegType()
        {
            string myEventName = ProEventName + "_NoRegType";

            // open login page on BETA
            ManagerSiteMgr.OpenLogin();

            // login to regression testing account
            ManagerSiteMgr.Login();

            // make sure we're on the Events tab
            ManagerSiteMgr.GoToEventsTabIfNeeded();

            // click "Regression" folder
            ManagerSiteMgr.SelectFolder();

            //(always build the event for now)
            ManagerSiteMgr.DeleteEventByName(myEventName);

            // select "Pro Event" from drop down
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

            // set up Start page
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, myEventName);
            BuilderMgr.SaveAndStay();
            BuilderMgr.Next(); // to Personal Info

            // set up Personal Information page
            BuilderMgr.Next(); // to Agenda

            // set up Agenda page
            BuilderMgr.ClickYesOnSplashPage();
            TestForAllVisibility();

            BuilderMgr.Next(); // to Lodging
            BuilderMgr.Next(); // to Merchandise
            BuilderMgr.Next(); // to Checkout

            // set up Checkout page
            BuilderMgr.EnterEventCheckoutPage();

            // get event id
            EventID = BuilderMgr.GetEventId();

            //get sessionId
            SessionId = BuilderMgr.GetEventSessionId();

            // save and close
            BuilderMgr.SaveAndClose();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("694")]
        public void SetAgendaVisibility_RegTypes()
        {
            string myEventName = ProEventName + "_RegTypes";

            // open login page on BETA
            ManagerSiteMgr.OpenLogin();

            // login to regression testing account
            ManagerSiteMgr.Login();

            // make sure we're on the Events tab
            ManagerSiteMgr.GoToEventsTabIfNeeded();

            // click "Regression" folder
            ManagerSiteMgr.SelectFolder();

            //(always build the event for now)
            ManagerSiteMgr.DeleteEventByName(myEventName);

            // select "Pro Event" from drop down
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

            // set up Start page
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, myEventName);
            BuilderMgr.AddRegType(RTNameTwo);
            BuilderMgr.AddRegType(RTNameOne);
            BuilderMgr.SaveAndStay();
            BuilderMgr.Next(); // to Personal Info

            // set up Personal Information page
            BuilderMgr.Next(); // to Agenda

            // set up Agenda page
            BuilderMgr.ClickYesOnSplashPage();
            TestForAllVisibility();
            TestForEvenRequired();
            TestForOddAdmin();
            TestForEvenOddVisibility();

            BuilderMgr.Next(); // to Lodging
            BuilderMgr.Next(); // to Merchandise
            BuilderMgr.Next(); // to Checkout

            // set up Checkout page
            BuilderMgr.EnterEventCheckoutPage();

            // get event id
            EventID = BuilderMgr.GetEventId();

            //get sessionId
            SessionId = BuilderMgr.GetEventSessionId();

            // save and close
            BuilderMgr.SaveAndClose();
        }

        [Verify]
        private void TestForAllVisibility()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("AG_All_NotViz");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.CheckBox);
            VerifyVisibilityChecked(RTNameAll, true, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required);
            VerifyVisibilityChecked(RTNameAll, true, true, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Visible);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.ClickSaveItem();

            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("AG_All_Required");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.CheckBox);
            BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Visible);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required);
            VerifyVisibilityChecked(RTNameAll, true, true, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.ClickSaveItem();

            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("AG_All_Admin");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.CheckBox);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Admin);
            VerifyVisibilityChecked(RTNameAll, false, true, true);
            VerifyVisibilityEnabled(RTNameAll, false, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Admin);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Admin);
            VerifyVisibilityChecked(RTNameAll, false, false, true);
            VerifyVisibilityEnabled(RTNameAll, false, true, true);
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        [Verify]
        private void TestForEvenRequired()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("AG_Even_Visible");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.CheckBox);
            BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Visible, RTNameAll);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, false, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required, RTNameTwo);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, true, true, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Admin, RTNameTwo);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, false, true, true);
            VerifyVisibilityEnabled(RTNameTwo, false, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Admin, RTNameTwo);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, false, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required, RTNameTwo);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, true, true, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Required, RTNameTwo);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, true, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        [Verify]
        private void TestForOddAdmin()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName("AG_Odd_ReqAdmin");
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.CheckBox);
            BuilderMgr.AGMgr.ShowAllRegTypes();
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameTwo, false, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameAll, true, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Admin, RTNameOne);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameOne, false, false, true);
            VerifyVisibilityEnabled(RTNameOne, false, true, true);
            VerifyVisibilityChecked(RTNameTwo, false, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required, RTNameOne);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameOne, false, true, true);
            VerifyVisibilityEnabled(RTNameOne, false, true, true);
            VerifyVisibilityChecked(RTNameTwo, false, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.HideAllRegTypes();
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(false);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.ShowAllRegTypes();
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameOne, false, true, true);
            VerifyVisibilityEnabled(RTNameOne, false, true, true);
            VerifyVisibilityChecked(RTNameTwo, false, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        [Verify]
        private void TestForEvenOddVisibility()
        {
            //Even Visible
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required, RTNameAll);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(false);
            VerifyVisibilityChecked(RTNameAll, true, true, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Visible, RTNameAll);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, false, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, RTNameTwo);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, true, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Admin, RTNameOne);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, true, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, true);
            VerifyVisibilityEnabled(RTNameOne, false, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Admin, RTNameAll);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(false);
            VerifyVisibilityChecked(RTNameAll, false, false, true);
            VerifyVisibilityEnabled(RTNameAll, false, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Admin, RTNameAll);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, false, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required, RTNameTwo);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, true, true, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, RTNameAll);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(false);
            VerifyVisibilityChecked(RTNameAll, true, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Visible, RTNameAll);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, false, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, RTNameTwo);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, true, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, false, false, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required, RTNameOne);
            BuilderMgr.AGMgr.VerifyVisibilityRegTypesVisible(true);
            VerifyVisibilityChecked(RTNameTwo, true, false, false);
            VerifyVisibilityEnabled(RTNameTwo, true, true, true);
            VerifyVisibilityChecked(RTNameOne, true, true, false);
            VerifyVisibilityEnabled(RTNameOne, true, true, true);
            VerifyVisibilityChecked(RTNameAll, false, false, false);
            VerifyVisibilityEnabled(RTNameAll, true, true, true);
        }

        private void VerifyVisibilityChecked(string regTypeName, bool forViz, bool forReq, bool forAdm)
        {
            BuilderMgr.AGMgr.VerifyValueVisibilityOption(AgendaItemManager.VisibilityOption.Visible, regTypeName, forViz);
            BuilderMgr.AGMgr.VerifyValueVisibilityOption(AgendaItemManager.VisibilityOption.Required, regTypeName, forReq);
            BuilderMgr.AGMgr.VerifyValueVisibilityOption(AgendaItemManager.VisibilityOption.Admin, regTypeName, forAdm);
        }

        private void VerifyVisibilityEnabled(string regTypeName, bool forViz, bool forReq, bool forAdm)
        {
            BuilderMgr.AGMgr.VerifyEnabledVisibilityOption(AgendaItemManager.VisibilityOption.Visible, regTypeName, forViz);
            BuilderMgr.AGMgr.VerifyEnabledVisibilityOption(AgendaItemManager.VisibilityOption.Required, regTypeName, forReq);
            BuilderMgr.AGMgr.VerifyEnabledVisibilityOption(AgendaItemManager.VisibilityOption.Admin, regTypeName, forAdm);
        }
    }
}
