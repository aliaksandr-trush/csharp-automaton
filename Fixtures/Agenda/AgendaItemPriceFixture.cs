namespace RegOnline.RegressionTest.Fixtures.Agenda
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgendaItemPriceFixture : FixtureBase
    {
        private const string EventName = "AgendaItemPriceFixture";
        private const double AgendaItemPrice = 10;

        public enum AgendaItemName
        {
            WithPrice,
            WithoutPrice
        }

        private int eventId;

        /// <summary>
        /// Bruce 2012-4-12
        /// This test was originally created for bug #24001, but as it is recalled now,
        /// I decide to leave it in the checklist
        /// </summary>
        [Test]
        [Category(Priority.Two)]
        [Description("1278")]
        public void AgendaItemCreation()
        {
            //24001.
            //It was claimed that the discount code didn't get calculated right when they saved a reg and then updated it.
            //In the process, it was found that setting up an agenda item with a 100% discount code and a main price,
            // with gaps in the list item price... gave error message (but you could work around that).
            //This test covers both agenda setup and registration/update.
            PrepareEvent();

            //step 4
            //Register for the event using single registrations
            RegisterMgr.CurrentEventId = this.eventId;
                        
            RegisterMgr.OpenRegisterPage(eventId);
            string email = RegisterMgr.Checkin();

            // Go to PI page and fill it out
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            //now in Agenda page
            RegisterMgr.SelectCustomFieldRadioButtons("Radio",  1.ToString());//finds for text

            int agendaCFId = RegisterMgr.GetAgendaItemIDFromRadio("Radio");//finds for text
            RegisterMgr.EnterAgendaItemDiscountCode(agendaCFId, "code");

            //no L&T page nor merchandise page, and AttendeeCheck isn't in the first path either
            //the next page will be Checkout
            RegisterMgr.Continue();
            
            //no payment... remember we have 100% discount
            RegisterMgr.FinishRegistration();

            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.CheckinWithEmail(email);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(ConfigurationProvider.XmlConfig.AccountConfiguration.Password);//=EnterProfileInfo pw
            // This will go AttendeeCheck
            RegisterMgr.Continue();
            //we didn't do a group, so assume it will always be for the first reg [it's zero-based]
            RegisterMgr.ClickEditAgendaLink(0);

            //now in Agenda page again!
            RegisterMgr.SelectCustomFieldRadioButtons("Radio", 2.ToString());//finds for text
            RegisterMgr.Continue();//gets us back to the AttendeeCheck page
            RegisterMgr.Continue();//gets us to...

            //...checkout page.
            //may as well look at the total here
            RegisterMgr.VerifyCheckoutTotal(0D);
            RegisterMgr.FinishRegistration();
        }

        [Step]
        private void PrepareEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();

            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
                this.eventId = BuilderMgr.GetEventId();
                BuilderMgr.SetEventNameAndShortcut(EventName);

                BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Agenda);
                BuilderMgr.ClickYesOnSplashPage();
                BuilderMgr.ClickAddAgendaItem();
                BuilderMgr.AGMgr.SetName("name");
                BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(AgendaItemPrice);

                BuilderMgr.AGMgr.FeeMgr.ExpandOption();
                BuilderMgr.AGMgr.FeeMgr.DC.ClickAddCode();
                BuilderMgr.AGMgr.FeeMgr.DC.SetDiscountCodeValues("code", DiscountCodeManager.ChangePriceDirection.Decrease, DiscountCodeManager.ChangeType.Percent, 100, null);
                BuilderMgr.AGMgr.FeeMgr.DC.SaveAndCloseDiscountCode();

                BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.RadioButton);
                BuilderMgr.AGMgr.SetName("Radio");

                List<Custom_Field_List_Item> lst = new List<Custom_Field_List_Item>()
                {
                    new Custom_Field_List_Item()
                    {
                        Description = "1",
                        Active = true
                    },                    
                    new Custom_Field_List_Item()
                    {
                        Description = "2",
                        Active = true
                    }
                };

                BuilderMgr.AGMgr.AddMultiChoiceItems(lst);

                BuilderMgr.AGMgr.ClickSaveItem();

                BuilderMgr.SaveAndClose();
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }
        }
    }
}
