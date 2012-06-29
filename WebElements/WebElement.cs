namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.UIUtility;

    public class WebElement
    {
        public string Locator { get; set; }

        public LocateBy TypeOfLocator { get; set; }

        public string Text
        {
            get
            {
                return UIUtilityProvider.UIHelper.GetText(Locator, TypeOfLocator);
            }
        }

        public virtual string Value
        {
            get
            {
                return UIUtilityProvider.UIHelper.GetValue(Locator, TypeOfLocator);
            }
        }

        public virtual int Count
        {
            get
            {
                return UIUtilityProvider.UIHelper.GetElementsCount(Locator, TypeOfLocator);
            }
        }

        public virtual bool IsPresent
        {
            get
            {
                return UIUtilityProvider.UIHelper.IsElementPresent(Locator, TypeOfLocator);
            }
        }

        public virtual bool IsDisplay
        {
            get
            {
                return UIUtilityProvider.UIHelper.IsElementDisplay(Locator, TypeOfLocator);
            }
        }

        public virtual bool IsHidden
        {
            get
            {
                return UIUtilityProvider.UIHelper.IsElementHidden(Locator, TypeOfLocator);
            }
        }

        public WebElement(string locator, LocateBy locatorType)
        {
            Locator = locator;
            TypeOfLocator = locatorType;
        }

        public WebElement() { }

        public virtual void WaitForPresent()
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent(Locator, TypeOfLocator);
        }

        public virtual void WaitForDisplay()
        {
            UIUtilityProvider.UIHelper.WaitForElementDisplay(Locator, TypeOfLocator);
        }

        public string GetAttribute(string att)
        {
            return UIUtilityProvider.UIHelper.GetAttribute(Locator, att, TypeOfLocator);
        }
    }
}
