namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.UIUtility;

    public class ButtonOrLink : WebElement
    {
        public ButtonOrLink(string locator, LocateBy locatorType)
            : base(locator, locatorType) { }

        public void Click()
        {
            WebDriverUtility.DefaultProvider.Click(Locator, TypeOfLocator);
        }
    }
}
