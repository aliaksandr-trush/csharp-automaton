namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    
    
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public partial class FormDetailManager : ManagerBase
    {
        public const string CreateAgendaItem = "Create Agenda Item";
        public const string AddAgendaItemLink = "Add Agenda Item";
        public const string CreateActivityItem = "Create Activities";
        public const string AddActivityItemLink = "Add Activities";
        public const string ListViewLink = "List View";
        public const string FormViewLink = "Form View";
        private readonly DateTime AgendaItemStartDateTime_Default = DefaultEventStartDateTime.AddDays(1);
        private readonly DateTime AgendaItemEndDateTime_Default = DefaultEventEndDateTime.AddDays(-2);
        
        public AgendaItemManager AGMgr { get; set; }

        public void VerifyEventAgendaPage()
        {
            ////ReloadEvent();

            // Ensure "create agenda item" link is present
            UIUtilityProvider.UIHelper.WaitForElementPresent(CreateAgendaItem, LocateBy.LinkText);
        }

        public void ClickAddAgendaItem()
        {
            UIUtilityProvider.UIHelper.SwitchToMainContent();

            // Click "create/add agenda item"
            if (UIUtilityProvider.UIHelper.IsElementDisplay(AddAgendaItemLink, LocateBy.LinkText))
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddAgendaItemLink, LocateBy.LinkText);
                Utility.ThreadSleep(1);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            }
            else
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(CreateAgendaItem, LocateBy.LinkText);
                Utility.ThreadSleep(1);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            }
        }

        public void ClickAddActivities()
        {
            // Click "create/add agenda item"
            if (UIUtilityProvider.UIHelper.IsElementDisplay(AddActivityItemLink, LocateBy.LinkText))
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddActivityItemLink, LocateBy.LinkText);
                Utility.ThreadSleep(1);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            }
            else
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(CreateActivityItem, LocateBy.LinkText);
                Utility.ThreadSleep(1);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            }
        }

        [Step]
        public void AddAgendaItem(AgendaItemManager.AgendaItemType type, string name)
        {
            this.ClickAddAgendaItem();
            this.AGMgr.SetName(name);
            this.AGMgr.SetTypeWithDefaults(type);

            // Enter dates
            if (type == AgendaItemManager.AgendaItemType.CheckBox ||
                type == AgendaItemManager.AgendaItemType.RadioButton ||
                type == AgendaItemManager.AgendaItemType.Dropdown ||
                type == AgendaItemManager.AgendaItemType.AlwaysSelected)
            {
                AGMgr.SetStartEndDateTime(AgendaItemStartDateTime_Default, AgendaItemEndDateTime_Default);
            }

            // Enter amount
            if (type == AgendaItemManager.AgendaItemType.CheckBox ||
                type == AgendaItemManager.AgendaItemType.RadioButton ||
                type == AgendaItemManager.AgendaItemType.Dropdown ||
                type == AgendaItemManager.AgendaItemType.FileUpload ||
                type == AgendaItemManager.AgendaItemType.AlwaysSelected)
            {
                AGMgr.FeeMgr.SetStandardPrice(Math.Round(new Random((int)DateTime.Now.Ticks).NextDouble() * 1000 + 1, 2));
            }

            AGMgr.ClickSaveItem();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void AddActivity(AgendaItemManager.AgendaItemType type, string name, double? price)
        {
            this.ClickAddActivities();
            this.AGMgr.SetName(name);
            this.AGMgr.SetTypeWithDefaults(type);

            // Enter dates
            if (type == AgendaItemManager.AgendaItemType.CheckBox ||
                type == AgendaItemManager.AgendaItemType.RadioButton ||
                type == AgendaItemManager.AgendaItemType.Dropdown ||
                type == AgendaItemManager.AgendaItemType.AlwaysSelected)
            {
                AGMgr.SetStartEndDateTime(AgendaItemStartDateTime_Default, AgendaItemEndDateTime_Default);
            }

            // Enter amount
            if (type == AgendaItemManager.AgendaItemType.CheckBox ||
                type == AgendaItemManager.AgendaItemType.RadioButton ||
                type == AgendaItemManager.AgendaItemType.Dropdown ||
                type == AgendaItemManager.AgendaItemType.FileUpload ||
                type == AgendaItemManager.AgendaItemType.AlwaysSelected &&
                price != null)
            {
                AGMgr.FeeMgr.SetStandardPrice(price);
            }

            AGMgr.ClickSaveItem();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void AddAgendaItemWithPriceAndNoDate(
            AgendaItemManager.AgendaItemType type, 
            string name, 
            double? standardPrice = null)
        {
            ClickAddAgendaItem();
            AGMgr.SetName(name);
            AGMgr.SetTypeWithDefaults(type);

            if (type == AgendaItemManager.AgendaItemType.CheckBox ||
                type == AgendaItemManager.AgendaItemType.RadioButton ||
                type == AgendaItemManager.AgendaItemType.Dropdown ||
                type == AgendaItemManager.AgendaItemType.FileUpload ||
                type == AgendaItemManager.AgendaItemType.AlwaysSelected)
            {
                AGMgr.FeeMgr.SetStandardPrice(standardPrice);
            }

            AGMgr.ClickSaveItem();
        }

        public void AddAgendaItemWithNoPriceNoDate(AgendaItemManager.AgendaItemType type, string name)
        {
            ClickAddAgendaItem();
            AGMgr.SetName(name);
            AGMgr.SetTypeWithDefaults(type);
            AGMgr.ClickSaveItem();
        }

        [Verify]
        public void VerifyAgendaItemInDatabase(AgendaItemManager.AgendaItemType type, string name)
        {
            //int typeInNetTiersEntity = AgendaItemManager.AgendaItemTypeInNetTiersEntityAttribute.GetTypeInNetTiersEntity(type);
            ////ReloadEvent();

            List<Custom_Field> ai = null;

            ClientDataContext db = new ClientDataContext();
            ai = (from a in db.Custom_Fields where a.LocationId == 0 && a.Description == name && a.TypeId == (int)type orderby a.Id ascending select a).ToList();

            //E.CustomFields ai = Event.CustomFieldsCollection.Find(
            //    delegate(E.CustomFields aiInner)
            //    {
            //        return aiInner.Description == name &&
            //            aiInner.ReportDescription == aiInner.Description &&
            //            aiInner.BadgeCaption == aiInner.Description &&
            //            aiInner.Fieldname == aiInner.Description &&
            //            aiInner.LocationId == (int)E.CustomFieldLocationsList.Agenda &&
            //            aiInner.TypeId == (int)type;
            //    }
            //);

            Assert.That(ai.Count != 0);

            switch (type)
            {
                case AgendaItemManager.AgendaItemType.RadioButton:
                case AgendaItemManager.AgendaItemType.Dropdown:
                    Assert.That(ai.Last().Custom_Field_List_Items != null);
                    break;
                case AgendaItemManager.AgendaItemType.Number:
                case AgendaItemManager.AgendaItemType.OneLineText:
                    Assert.That(ai.Last().Length == ((int)type == 2 ? 50 : 5));
                    break;
                case AgendaItemManager.AgendaItemType.Contribution:
                    Assert.That(ai.Last().MinVarAmount > 0);
                    Assert.That(ai.Last().MaxVarAmount > 0);
                    break;
            }

            // check dates
            if (type == AgendaItemManager.AgendaItemType.CheckBox ||
                type == AgendaItemManager.AgendaItemType.RadioButton ||
                type == AgendaItemManager.AgendaItemType.Dropdown ||
                type == AgendaItemManager.AgendaItemType.AlwaysSelected)
            {
                Assert.AreEqual(AgendaItemStartDateTime_Default, ai.Last().SessionDate);
                Assert.AreEqual(AgendaItemEndDateTime_Default, ai.Last().SessionEndDate);
            }

            // check amount
            if (type == AgendaItemManager.AgendaItemType.CheckBox ||
                type == AgendaItemManager.AgendaItemType.RadioButton ||
                type == AgendaItemManager.AgendaItemType.Dropdown ||
                type == AgendaItemManager.AgendaItemType.FileUpload ||
                type == AgendaItemManager.AgendaItemType.AlwaysSelected)
            {
                Assert.That(ai.Last().Amount > 0);
            }
        }

        [Verify]
        public void VerifyFormView()
        {
            string locator = "formGridBody";
            Assert.That(!UIUtilityProvider.UIHelper.IsElementPresent(locator, LocateBy.Id));
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(FormViewLink, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            Assert.That(UIUtilityProvider.UIHelper.IsElementPresent(locator, LocateBy.Id));
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ListViewLink, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void OpenAgendaDetailPage(int agendaNumStartFromZero)
        {
            char[] sp1 = { '\'' };

            string AgNum = agendaNumStartFromZero.ToString();
            string Link1 = UIUtilityProvider.UIHelper.GetAttribute("//tr[@id='sdgr_" + AgNum + "']/td[1]", "onclick", LocateBy.XPath);

            //get agenda id
            string[] Link1_parts1 = Link1.Split(sp1, 3);
            string ID = Link1_parts1[1];

            //open agenda detail page
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("listGridTD" + ID + "2", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SetScheduleConflictChecking(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkEnableScheduleConflictChecking", check, LocateBy.Id);
        }

        public void SetShoppingCart(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkEventsIsCart", check, LocateBy.Id);
        }
    }
}
