namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;

    public class CheckBox : WebElement
    {
        public CheckBox(string locator, LocateBy locatorType)
            : base(locator, locatorType){ }

        public void Set(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(Locator, check, TypeOfLocator);
        }
    }
}
