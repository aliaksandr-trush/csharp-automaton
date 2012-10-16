namespace RegOnline.RegressionTest.Fixtures.FormTypes.ExpressEvent
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class ExpressEventFixture : FixtureBase
    {
        private const string EventName = "ExpressEvent - Checklist";
        private const int ExpireTime = 30;

        private int eventID = ManagerBase.InvalidId;
        private string sessionID = ManagerBase.InvalidSessionId;

        [Test]
        [Category(Priority.Two)]
        [Description("588")]
        public void ExpressEventChecklistEventCreation()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ExpressEvent);
            this.SetEventStartPage();
            BuilderMgr.Next();
            this.SetEventPIPage();
            BuilderMgr.Next();
            this.SetEventAgendaPage();
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            BuilderMgr.EnterEventCheckoutPage();
            this.eventID = BuilderMgr.GetEventId();
            this.sessionID = BuilderMgr.GetEventSessionId();
            BuilderMgr.SaveAndClose();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("589")]
        public void ExpressEventChecklistTestReg()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();

            this.eventID = ManagerSiteMgr.GetFirstEventId(EventName);

            if (this.eventID == ManagerBase.InvalidId)
            {
                this.ExpressEventChecklistEventCreation();
            }

            RegisterMgr.CurrentEventId = this.eventID;
            RegisterMgr.OpenRegisterPage();
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.SelectPersonalInfoCustomFields();
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();
            RegisterMgr.PayMoney(RegisterManager.PaymentMethod.CreditCard);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        [Test]
        [Category(Priority.Four)]
        [Description("582")]
        public void CopyExpressEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.sessionID = ManagerSiteMgr.GetEventSessionId();

            this.CopyFromManagerSite();
            this.CopyFromDashboard();

            ManagerSiteMgr.DeleteEventByName(EventName + " (Copy)");
            ManagerSiteMgr.DeleteEventByName(EventName + " (Copy) (Copy)");
        }

        private void CopyFromManagerSite()
        {
            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.ExpressEventChecklistEventCreation();
            }

            ManagerSiteMgr.CopyEventByName(EventName);
            Assert.True(ManagerSiteMgr.EventExists(EventName + " (Copy)"));
        }

        private void CopyFromDashboard()
        {
            this.eventID = ManagerSiteMgr.GetFirstEventId(EventName + " (Copy)");
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.CopyEventFromDashboard(EventName + " (Copy) (Copy)");
            ManagerSiteMgr.DashboardMgr.ReturnToManagerScreenEventList();
            Assert.True(ManagerSiteMgr.EventExists(EventName + " (Copy) (Copy)"));
        }

        [Step]
        private void SetEventStartPage()
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName);
            BuilderMgr.SaveAndStay();
        }

        [Step]
        private void SetEventPIPage()
        {
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.OneLineText, "CF- Text");
            BuilderMgr.AddPICustomField(CustomFieldManager.CustomFieldType.RadioButton, "CF- Radio");
            BuilderMgr.SaveAndStay();
        }

        [Step]
        private void SetEventAgendaPage()
        {
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.AddAgendaItem(AgendaItemManager.AgendaItemType.CheckBox, "AG- Checkbox");
            BuilderMgr.AddAgendaItem(AgendaItemManager.AgendaItemType.Dropdown, "AG- Dropdown");
            BuilderMgr.SetScheduleConflictChecking(false);
            BuilderMgr.SaveAndStay();
        }
    }
}
