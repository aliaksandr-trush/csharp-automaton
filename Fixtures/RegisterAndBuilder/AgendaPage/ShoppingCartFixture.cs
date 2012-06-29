namespace RegOnline.RegressionTest.Fixtures.RegisterAndBuilder.AgendaPage
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class ShoppingCartFixture : FixtureBase
    {
        private const string EventName = "ShoppingCartFixture";

        private string eventSessionId;
        private int eventId;
        
        int agendaItemCount = 3;
        List<int> agendaItemIds = new List<int>();

        [Test]
        [Category(Priority.Three)]
        [Description("724")]
        public void ShoppingCart()
        {
            this.CreateEvent();

            RegisterMgr.OpenRegisterPage(this.eventId);

            // Change Calendar View to Location View - the Month view defaults to the current month.
            RegisterMgr.EventCalendarMgr.SelectViewBy(EventCalendarManager.ViewBy.Location);

            for (int cnt = 0; cnt < agendaItemCount; cnt++)
            {
                RegisterMgr.EventCalendarMgr.AddToCart(agendaItemIds[cnt]);
            }

            RegisterMgr.EventCalendarMgr.OpenRegister();

            // Register person
            RegisterMgr.Checkin();
            string primaryEmail = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
     
            // Personal Info page
            RegisterMgr.EnterProfileInfo();

            // Continue to the next step to finalize the group registration
            RegisterMgr.Continue();

            // Finalize
            RegisterMgr.SelectPaymentMethod(RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void CreateEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
                this.eventId = BuilderMgr.GetEventId();
                BuilderMgr.SetEventNameAndShortcut(EventName);
                BuilderMgr.SelectEventCategory(FormDetailManager.EventCategory.Other);
                BuilderMgr.SelectEventIndustry(FormDetailManager.EventIndustry.ProfessionalAndContinuingEducation);
                BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
                BuilderMgr.ClickYesOnSplashPage();
                BuilderMgr.AddAgendaItem(AgendaItemManager.AgendaItemType.CheckBox, "SessionOne");
                BuilderMgr.AddAgendaItem(AgendaItemManager.AgendaItemType.CheckBox, "SessionTwo");
                BuilderMgr.AddAgendaItem(AgendaItemManager.AgendaItemType.CheckBox, "SessionThree");

                this.agendaItemIds.Clear();

                for (int cnt = 0; cnt < agendaItemCount; cnt++)
                {
                    agendaItemIds.Add(BuilderMgr.AGMgr.GetAgendaItemID(cnt));
                }

                BuilderMgr.SetScheduleConflictChecking(false);
                BuilderMgr.SetShoppingCart(true);
                BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
                BuilderMgr.PaymentMethodMgr.AddPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
                BuilderMgr.SaveAndClose();
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
                ManagerSiteMgr.OpenEventBuilderStartPage(this.eventId, this.eventSessionId);
                BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);

                this.agendaItemIds.Clear();

                for (int cnt = 0; cnt < agendaItemCount; cnt++)
                {
                    agendaItemIds.Add(BuilderMgr.AGMgr.GetAgendaItemID(cnt));
                }
            }
        }
    }
}
