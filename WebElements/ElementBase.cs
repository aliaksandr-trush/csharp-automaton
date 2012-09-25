namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.UIUtility;

    public class ElementBase
    {
        public string Locator { get; set; }

        public LocateBy TypeOfLocator { get; set; }

        public string Text
        {
            get
            {
                return UIUtil.DefaultProvider.GetText(Locator, TypeOfLocator);
            }
        }

        public virtual string Value
        {
            get
            {
                return UIUtil.DefaultProvider.GetValue(Locator, TypeOfLocator);
            }
        }

        public virtual int Count
        {
            get
            {
                return UIUtil.DefaultProvider.GetElementsCount(Locator, TypeOfLocator);
            }
        }

        public virtual bool IsPresent
        {
            get
            {
                return UIUtil.DefaultProvider.IsElementPresent(Locator, TypeOfLocator);
            }
        }

        public virtual bool IsDisplay
        {
            get
            {
                return UIUtil.DefaultProvider.IsElementDisplay(Locator, TypeOfLocator);
            }
        }

        public virtual bool IsHidden
        {
            get
            {
                return UIUtil.DefaultProvider.IsElementHidden(Locator, TypeOfLocator);
            }
        }

        public ElementBase()
        {
        }

        public ElementBase(string locator, LocateBy locatorType)
        {
            Locator = locator;
            TypeOfLocator = locatorType;
        }

        public virtual void WaitForPresent()
        {
            UIUtil.DefaultProvider.WaitForElementPresent(Locator, TypeOfLocator);
        }

        public virtual void WaitForDisplay()
        {
            UIUtil.DefaultProvider.WaitForElementDisplay(Locator, TypeOfLocator);
        }

        public virtual void Click()
        {
            UIUtil.DefaultProvider.Click(Locator, TypeOfLocator);
        }

        public virtual void WaitForDisplayAndClick()
        {
            this.WaitForDisplay();
            this.Click();
        }

        public string GetAttribute(string att)
        {
            return UIUtil.DefaultProvider.GetAttribute(Locator, att, TypeOfLocator);
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

        public static bool IsElementPresent(ElementBase element)
        {
            return UIUtil.DefaultProvider.IsElementPresent(element.Locator, element.TypeOfLocator);
        }

        public static bool IsElementDisplay(ElementBase element)
        {
            return UIUtil.DefaultProvider.IsElementDisplay(element.Locator, element.TypeOfLocator);
        }

        public static void VerifyPresent(ElementBase element, bool isPresent)
        {
            bool actual = UIUtil.DefaultProvider.IsElementPresent(element.Locator, element.TypeOfLocator);
            Utilities.VerifyTool.VerifyValue(isPresent, actual, "Element '" + element.Locator + "' is present: {0}");
        }

        public static void VerifyDisplay(ElementBase element, bool isDisplayed)
        {
            bool actual = UIUtil.DefaultProvider.IsElementDisplay(element.Locator, element.TypeOfLocator);
            Utilities.VerifyTool.VerifyValue(isDisplayed, actual, "Element '" + element.Locator + "' is display: {0}");
        }
    }
}
