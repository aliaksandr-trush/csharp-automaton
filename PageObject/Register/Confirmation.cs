namespace RegOnline.RegressionTest.PageObject.Register
{
    using System.Collections.Generic;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class Confirmation : Window
    {
        public Label Total = new Label("//td[text()='Total:']/following-sibling::td[@class='currency']", LocateBy.XPath);
        public ButtonOrLink ChangeMyRegistration = new ButtonOrLink("ctl00_cphNoForm_btnRegEdit", LocateBy.Id);
        public Label RegistrationId = new Label("//th[@scope='row'][text()='Registration ID:']/../td", LocateBy.XPath);

        public ButtonOrLink AddToCalendar(DataCollection.AgendaItem agendaItem)
        {
            return new ButtonOrLink(string.Format("//li[@class='addToCalendar']/a[contains(@href,'{0}')]", agendaItem.Id), LocateBy.XPath);
        }

        public void ChangeMyRegistration_Click()
        {
            this.ChangeMyRegistration.WaitForDisplay();
            this.ChangeMyRegistration.Click();
            Utility.ThreadSleep(2);
            WaitForLoad();
        }

        public List<Label> GetSelectedAgendaItems()
        {
            List<Label> agendaItems = new List<Label>();
            Label agendaName = new Label("//fieldset[@class='registrantDetailSection'][legend[text()='Agenda']]//h4", LocateBy.XPath);

            for (int i = 1; i <= agendaName.Count; i++)
            {
                string agendaItemName = "//fieldset[@class='registrantDetailSection'][legend[text()='Agenda']]//h4[" + i.ToString() +"]";
                agendaItems.Add(new Label(agendaItemName, LocateBy.XPath));
            }

            return agendaItems;
        }
    }
}
