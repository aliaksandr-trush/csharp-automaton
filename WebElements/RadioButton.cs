﻿namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.UIUtility;

    public class RadioButton : Clickable
    {
        public RadioButton(string locator, LocateBy locatorType)
            : base(locator, locatorType) { }

        public virtual bool IsChecked
        {
            get
            {
                return WebDriverUtility.DefaultProvider.IsChecked(Locator, TypeOfLocator);
            }
        }
    }
}
