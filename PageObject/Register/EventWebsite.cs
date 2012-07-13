namespace RegOnline.RegressionTest.PageObject.Register
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;

    public class EventWebsite : Window
    {
        public ButtonOrLink RegisterNow = new ButtonOrLink("aRegBtn", UIUtility.LocateBy.Id);

        public void RegisterNow_Click()
        {
            this.RegisterNow.WaitForDisplay();
            this.RegisterNow.Click();
            WaitForLoad();
        }
    }
}
