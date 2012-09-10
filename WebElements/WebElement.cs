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
                return WebDriverUtility.DefaultProvider.GetText(Locator, TypeOfLocator);
            }
        }

        public virtual string Value
        {
            get
            {
                return WebDriverUtility.DefaultProvider.GetValue(Locator, TypeOfLocator);
            }
        }

        public virtual int Count
        {
            get
            {
                return WebDriverUtility.DefaultProvider.GetElementsCount(Locator, TypeOfLocator);
            }
        }

        public virtual bool IsPresent
        {
            get
            {
                return WebDriverUtility.DefaultProvider.IsElementPresent(Locator, TypeOfLocator);
            }
        }

        public virtual bool IsDisplay
        {
            get
            {
                return WebDriverUtility.DefaultProvider.IsElementDisplay(Locator, TypeOfLocator);
            }
        }

        public virtual bool IsHidden
        {
            get
            {
                return WebDriverUtility.DefaultProvider.IsElementHidden(Locator, TypeOfLocator);
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
            WebDriverUtility.DefaultProvider.WaitForElementPresent(Locator, TypeOfLocator);
        }

        public virtual void WaitForDisplay()
        {
            WebDriverUtility.DefaultProvider.WaitForElementDisplay(Locator, TypeOfLocator);
        }

        public string GetAttribute(string att)
        {
            return WebDriverUtility.DefaultProvider.GetAttribute(Locator, att, TypeOfLocator);
        }

        public bool HasAttribute(string att)
        {
            try
            {
                string attri = GetAttribute(att);

                if (attri == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
