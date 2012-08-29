namespace RegOnline.RegressionTest.Fixtures.Gateway
{
    using NUnit.Framework;
    using System;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Builder;
    using System.Collections.Generic;
    

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class CardinalRegistrationFixture : FixtureBase
    {
        private const string EventName = "CardinalTestEvent";
        private string eventSessionId;
        private int eventId;
        private double[] Price = {24.95, 19.95, 1.02, 29.35};
        private const double EventFee = 10.00;
        private string[] AgendaName = { "Cardinal", "Bishop", "Hierarch", "Polly-O Appreciation" };

        [Test]
        [Category(Priority.Two)]
        [Description("139")]
        public void CardinalRegistration()
        {
            this.LoginAndGetSessionID();

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateEvent(EventName, AgendaName, Price, EventFee);
                this.ActivateEvent(eventId);
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }

            this.Register(eventId, "01 - Jan", "2013");

            Assert.True(RegisterMgr.OnCardinalVerificationPage());
            RegisterMgr.SubmitCardinalPassword("1234");

            this.ConfirmationPage();

            DataHelperTool.ChangeAllRegsToTestAndDelete(this.eventId);
        }

        [Test]
        [Category(Priority.One)]
        [Description("728")]
        public void NoCardinalRegistration()
        {
            this.LoginAndGetSessionID();

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateEvent(EventName, AgendaName, Price, EventFee);
                this.ActivateEvent(eventId);
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }

            this.Register(eventId, "06 - Jun", "2015");

            Assert.False(RegisterMgr.OnCardinalVerificationPage());

            this.ConfirmationPage();

            DataHelperTool.ChangeAllRegsToTestAndDelete(this.eventId);
        }

        [Test]
        [Category(Priority.One)]
        [Description("152")]
        public void CardinalRegistration_MC()
        {
            this.LoginAndGetSessionID();

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateEvent(EventName, AgendaName, Price, EventFee);
                this.ActivateEvent(eventId);
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }

            this.Register(eventId);

            DataHelperTool.ChangeAllRegsToTestAndDelete(this.eventId);
        }

        
        
        private void SetAgendaPage(string[] agendaname, double[] price)
        {
            int i = 0;
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
            BuilderMgr.ClickYesOnSplashPage();

            foreach (var name in agendaname)
            {
                BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.CheckBox, name, price[i]);
                i++;
            }
        }

        private void LoginAndGetSessionID()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.eventSessionId = BuilderMgr.GetEventSessionId();
        }
        
        private void CreateEvent(string eventname, string[] agendaname, double[] price, double eventfee)
        {
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventname);
            BuilderMgr.SetEventFee(eventfee);
            BuilderMgr.SaveAndStay();
            this.SetAgendaPage(agendaname, price);
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndClose(); 
        }

        private void ActivateEvent(int eventid)
        {
            ManagerSiteMgr.OpenEventDashboard(eventId);
            ManagerSiteMgr.DashboardMgr.ActiveEvent(); 
        }

        private void Register(int eventId)
        {
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();
            RegisterMgr.PaymentMgr.EnterCreditCardNumberInfo("5500005555555559", "123", "01 - Jan", "2013");
            RegisterMgr.PaymentMgr.EnterCreditCardNameCountryType("test test", "United States", null);
            RegisterMgr.PaymentMgr.EnterCreditCardAddressInfo(null, null, null, "CO", null);
            RegisterMgr.FinishRegistration();
            Assert.True(RegisterMgr.OnCardinalVerificationPage());
            RegisterMgr.SubmitCardinalPassword("1234");

            if (RegisterMgr.OnConfirmationRedirectPage())
            {
                RegisterMgr.ClickAdvantageNo();
            }
            
            RegisterMgr.ConfirmRegistration();
        }

        private void Register(int EventId, string Date, string Year)
        {
            RegisterMgr.OpenRegisterPage(EventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();
            RegisterMgr.PaymentMgr.EnterCreditCardNumberInfo("4444444444444448", "123", Date, Year);
            RegisterMgr.PaymentMgr.EnterCreditCardNameCountryType("test test", "United States", null);
            RegisterMgr.PaymentMgr.EnterCreditCardAddressInfo(null, null, null, "CO", null);
            RegisterMgr.FinishRegistration();
        }

        private void ConfirmationPage()
        {
            if (RegisterMgr.OnConfirmationRedirectPage())
            {
                RegisterMgr.ClickAdvantageNo();
            }

            RegisterMgr.ConfirmRegistration();      
        }
    }
}