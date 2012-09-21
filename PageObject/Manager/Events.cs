namespace RegOnline.RegressionTest.PageObject.Manager
{
    using System;
    using System.Collections.Generic;
    using OpenQA.Selenium;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class Events : Window
    {
        public Clickable AddEvent = new Clickable("//div[@id='createNewEvent']//img[@class='rmLeftImage'][1]", LocateBy.XPath);

        public void AddEvent_Click()
        {
            this.AddEvent.WaitForDisplay();
            this.AddEvent.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void EventType_Select(DataCollection.FormData.FormType formType)
        {
            Clickable EventType = new Clickable(string.Format("//div[@id='createNewEvent']//span[text()='{0}']", CustomStringAttribute.GetCustomString(formType)), LocateBy.XPath);

            EventType.WaitForDisplay();
            EventType.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            WebDriverUtility.DefaultProvider.HideActiveSpecificFooter(true);
        }

        public void Folder_Click(string folderName)
        {
            string folderLocator = string.Format("//div[@id='tree']//span[text()='{0}']", folderName);
            Clickable Folder = new Clickable(folderLocator, LocateBy.XPath);
            Folder.WaitForDisplay();
            string folderDivClassAttribute = WebDriverUtility.DefaultProvider.GetAttribute(string.Format("{0}/parent::div", folderLocator), "class", LocateBy.XPath);

            if (!folderDivClassAttribute.Equals("rtMid rtSelected"))
            {
                Folder.WaitForDisplay();
                Folder.Click();
                WaitForAJAX();
                Utility.ThreadSleep(1);
            }
        }
    }

    public class EventList_EventRow : Window
    {
        private ElementBase Tr;
        private Clickable OKButton_EventDeletePopup = new Clickable("//span[text()='OK']", LocateBy.XPath);
        public int RowIndex;
        public int EventId;
        public Clickable Title;
        public Clickable Delete;
        public string EventURL;

        public EventList_EventRow(int eventId)
        {
            this.EventId = eventId;

            this.Tr = new ElementBase(
                string.Format("//table[@id='ctl00_ctl00_cphDialog_cpMgrMain_rdgrdgrdForms_ctl00']/tbody/tr[@data-id='{0}']", eventId),
                LocateBy.XPath);

            string[] idAttribute = Tr.GetAttribute("id").Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            this.RowIndex = Convert.ToInt32(idAttribute[idAttribute.Length - 1]) + 1;

            this.Title = new Clickable(
                string.Format("{0}/td/a[@class='listEventTile']", this.Tr.Locator),
                LocateBy.XPath);

            this.Delete = new Clickable(
                string.Format("{0}/td/div[@class='actions']/a[@title='Delete event']", this.Tr.Locator),
                LocateBy.XPath);

            this.EventURL = new Clickable(
                string.Format("{0}/td//a[contains(@title,'regonline.com')]", this.Tr.Locator),
                LocateBy.XPath).GetAttribute("href");
        }

        public static List<EventList_EventRow> GetEventRows(string eventName)
        {
            List<IWebElement> elements = WebDriverUtility.DefaultProvider.GetElements(
                string.Format("//table[@id='ctl00_ctl00_cphDialog_cpMgrMain_rdgrdgrdForms_ctl00']/tbody/tr/td/a[text()='{0}']/parent::td/parent::tr", eventName),
                LocateBy.XPath);

            List<EventList_EventRow> rows = new List<EventList_EventRow>();

            foreach (IWebElement element in elements)
            {
                rows.Add(new EventList_EventRow(Convert.ToInt32(element.GetAttribute("data-id"))));
            }

            return rows;
        }

        public void DeleteEvent()
        {
            this.Delete.WaitForDisplay();
            this.Delete.Click();
            this.OKButton_EventDeletePopup.WaitForDisplay();
            this.OKButton_EventDeletePopup.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
        }

        public void ClickTitleToOpenDashboard()
        {
            this.Title.WaitForDisplay();
            this.Title.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
