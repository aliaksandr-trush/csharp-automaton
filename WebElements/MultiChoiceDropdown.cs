namespace RegOnline.RegressionTest.WebElements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.UIUtility;

    public class MultiChoiceDropdown : WebElement
    {
        public MultiChoiceDropdown(string locator, LocateBy locatorType)
            : base(locator, locatorType) { }

        public void SelectWithText(object text)
        {
            UIUtilityProvider.UIHelper.SelectWithText(Locator, text, TypeOfLocator);
        }

        public void SelectWithIndex(int index)
        {
            UIUtilityProvider.UIHelper.SelectWithIndex(Locator, index, TypeOfLocator);
        }

        public void SelectWithValue(string value)
        {
            UIUtilityProvider.UIHelper.SelectWithValue(Locator, value, TypeOfLocator);
        }
    }
}
