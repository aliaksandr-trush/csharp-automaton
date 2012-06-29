namespace RegOnline.RegressionTest.WebElements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.UIUtility;

    public class MultiChoiceRadioButton : WebElement
    {
        public MultiChoiceRadioButton(string locatorFormat, LocateBy locatorType)
            : base(locatorFormat, locatorType) { }

        public void SelectWithText(object value)
        {
            UIUtilityProvider.UIHelper.Click(string.Format(Locator, value), TypeOfLocator);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }
    }
}
