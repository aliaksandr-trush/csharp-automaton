namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;

    public class CheckBox : ElementBase
    {
        public CheckBox(string locator, LocateBy locatorType)
            : base(locator, locatorType){ }

        public void Set(bool check)
        {
            UIUtil.DefaultProvider.SetCheckbox(Locator, check, TypeOfLocator);
        }

        public virtual bool IsChecked
        {
            get
            {
                return UIUtil.DefaultProvider.IsChecked(Locator, TypeOfLocator);
            }
        }
    }
}
