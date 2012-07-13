namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.UIUtility;

    public class RadioButton : ButtonOrLink
    {
        public RadioButton(string locator, LocateBy locatorType)
            : base(locator, locatorType) { }

        public virtual bool IsChecked
        {
            get
            {
                return UIUtilityProvider.UIHelper.IsChecked(Locator, TypeOfLocator);
            }
        }
    }
}
