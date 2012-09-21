namespace RegOnline.RegressionTest.PageObject.Builder
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class EventWebsite : Window
    {
        public CheckBox UseEventWebsiteAsTheStartingPageForEvent = new CheckBox("ctl00_cph_chkStartonSite", LocateBy.Id);
        public EventWebsiteFrame EventWebsiteFrame = new EventWebsiteFrame("ctl00_cph_ifrmSite");
    }

    public class EventWebsiteFrame : Frame
    {
        public EventWebsiteFrame(string id)
        {
            Id = id;
        }

        public Clickable ShowNavigation = new Clickable("//div[@id='ctl00_ctl00_efTabsPanel']//a[text()='Show']", LocateBy.XPath);

        public void ShowNavigation_Click()
        {
            this.ShowNavigation.WaitForDisplay();
            this.ShowNavigation.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }
    }
}
