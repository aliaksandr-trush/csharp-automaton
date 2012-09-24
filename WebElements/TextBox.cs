namespace RegOnline.RegressionTest.WebElements
{
    using System;
    using RegOnline.RegressionTest.UIUtility;

    public class TextBox : WebElement
    {
        public TextBox(string locator, LocateBy locatorType)
            : base(locator, locatorType) { }

        public void Type(object value)
        {
            WebDriverUtility.DefaultProvider.Type(Locator, value, TypeOfLocator);
        }

        public void SetValue(string value)
        {
            if (this.TypeOfLocator != LocateBy.Id)
            {
                throw new InvalidOperationException("Can only set value for element with id locator!");
            }

            WebDriverUtility.DefaultProvider.ExecuteJavaScript(string.Format("document.getElementById('{0}').value='{1}';", this.Locator, value));
        }

        public void Click()
        {
            WebDriverUtility.DefaultProvider.Click(Locator, TypeOfLocator);
        }
    }
}
