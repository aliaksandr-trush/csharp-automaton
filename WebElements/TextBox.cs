namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.UIUtility;

    public class TextBox : WebElement
    {
        public TextBox(string locator, LocateBy locatorType)
            : base(locator, locatorType) { }

        public void Type(object value)
        {
            WebDriverUtility.DefaultProvider.Type(Locator, value, TypeOfLocator);
        }

        public void Click()
        {
            WebDriverUtility.DefaultProvider.Click(Locator, TypeOfLocator);
        }
    }
}
