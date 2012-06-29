namespace RegOnline.RegressionTest.PageObject.Manager
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class PageObjectHelper : Window
    {
        public void DashboardTab_Click(FormData.DashboardTab tab)
        {
            ButtonOrLink targetTab = new ButtonOrLink(CustomStringAttribute.GetCustomString(tab), LocateBy.LinkText);
            targetTab.WaitForDisplay();
            targetTab.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
