namespace RegOnline.RegressionTest.PageObject
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class PageObjectHelper
    {
        public ButtonOrLink Allow = new ButtonOrLink("//button[@class='ui-button ui-widget ui-corner-all ui-button-text-only dialog-allow-button']", LocateBy.XPath);

        public void Allow_Click()
        {
            if (Allow.IsPresent)
            {
                Utility.ThreadSleep(1);
                Allow.WaitForDisplay();
                Allow.Click();
            }
        }
    }
}
