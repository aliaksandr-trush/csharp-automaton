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
            : base(locator, locatorType) 
        {
            this.GetOptions();
        }

        public List<Label> Options = new List<Label>();

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

        private void GetOptions()
        {
            Label optionCount = new Label(Locator + "/option", LocateBy.XPath);

            for (int i = 1; i <= optionCount.Count; i++)
            {
                Label option = new Label(optionCount.Locator + "[" + i + "]", LocateBy.XPath);
                Options.Add(option);
            }
        }
    }
}
