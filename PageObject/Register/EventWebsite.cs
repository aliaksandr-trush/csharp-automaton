namespace RegOnline.RegressionTest.PageObject.Register
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;

    public class EventWebsite : Window
    {
        public Clickable RegisterNow = new Clickable("aRegBtn", UIUtility.LocateBy.Id);
        public Clickable Agenda = new Clickable("ctl00_hpTab2", UIUtility.LocateBy.Id);

        public Label Text(string text)
        {
            return new Label(string.Format("//div[@class='agenda_repeater']/div[contains(text(),'{0}')]", text), UIUtility.LocateBy.XPath);
        }

        public void RegisterNow_Click()
        {
            this.RegisterNow.WaitForDisplay();
            this.RegisterNow.Click();
            WaitForLoad();
        }

        public void Agenda_Click()
        {
            this.Agenda.WaitForDisplay();
            this.Agenda.Click();
            Utilities.Utility.ThreadSleep(1);
            WaitForLoad();
        }
    }
}
