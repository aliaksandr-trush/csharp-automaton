namespace RegOnline.RegressionTest.PageObject.Register
{
    using System;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public class Agenda
    {
        public AgendaRow GetAgendaItem(AgendaItem agenda)
        {
            return new AgendaRow(agenda);
        }
    }

    public class AgendaRow
    {
        private string locator = "//li[@data-id='{0}']";

        public WebElement AgendaType = new WebElement();
        public WebElement AgendaLabel = new WebElement();
        public DateTime? StartDate;
        public DateTime? EndDate;
        public string Location;
        public double? Price;

        public AgendaRow(AgendaItem agenda)
        {
            switch (agenda.Type)
            {
                case FormData.CustomFieldType.CheckBox:
                    AgendaType = new CheckBox(string.Format(locator + "//input", agenda.Id.ToString()), LocateBy.XPath);
                    AgendaLabel = new Label(string.Format(locator + "//label", agenda.Id.ToString()), LocateBy.XPath);
                    this.GetAgendaDate(agenda);
                    this.GetAgendaLocation(agenda);
                    this.GetAgendaPrice(agenda);
                    break;
                case FormData.CustomFieldType.RadioButton:
                    AgendaLabel = new WebElement(string.Format(locator + "//p[@class='label']", agenda.Id.ToString()), LocateBy.XPath);
                    this.GetAgendaDate(agenda);
                    this.GetAgendaLocation(agenda);
                    this.GetAgendaPrice(agenda);
                    break;
                case FormData.CustomFieldType.Dropdown:
                    AgendaType = new MultiChoiceDropdown(string.Format(locator + "//select", agenda.Id.ToString()), LocateBy.XPath);
                    AgendaLabel = new Label(string.Format(locator + "//label", agenda.Id.ToString()), LocateBy.XPath);
                    this.GetAgendaDate(agenda);
                    this.GetAgendaLocation(agenda);
                    this.GetAgendaPrice(agenda);
                    break;
                default:
                    break;
            }
        }

        private void GetAgendaDate(AgendaItem agenda)
        {
            Label dateTime = new Label(string.Format(locator + "//div[@class='place'][span[text()='Date:']]", agenda.Id.ToString()), LocateBy.XPath);
            Label time = new Label(string.Format(locator + "//div[@class='place'][span[text()='Time:']]", agenda.Id.ToString()), LocateBy.XPath);
            
            if (time.IsPresent)
            {
                dateTime = time;
            }

            if (dateTime.IsPresent)
            {
                string dateTimeString = dateTime.Text;
                string tmp = null;

                if (!time.IsPresent)
                {
                    tmp = dateTimeString.Split(new string[] { "te:" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                }
                else
                {
                    tmp = dateTimeString.Split(new string[] { "me:" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                }

                if (tmp.Contains("-"))
                {
                    string startDateTime = tmp.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    string endDateTimeTmp = tmp.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                    string endDateTime = endDateTimeTmp.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    StartDate = Convert.ToDateTime(startDateTime);
                    EndDate = Convert.ToDateTime(endDateTime);
                }
                else
                {
                    StartDate = EndDate = Convert.ToDateTime(tmp);
                }
            }
        }

        private void GetAgendaPrice(AgendaItem agenda)
        {
            Label price = new Label(string.Format(locator + "//div[@class='place'][span[text()='Price:']]", agenda.Id.ToString()), LocateBy.XPath);

            if (price.IsPresent)
            {
                string priceString = price.Text;
                string tmp = priceString.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                string PriceString = "";

                for (int i = 0; i < tmp.Length; i++)
                {
                    if (Char.IsNumber(tmp, i) || (tmp.Substring(i, 1) == "."))
                    {
                        PriceString += tmp.Substring(i, 1);
                    }
                }

                Price = Convert.ToDouble(PriceString);
            }
        }

        private void GetAgendaLocation(AgendaItem agenda)
        {
            Label location = new Label(string.Format(locator + "//div[@class='place'][span[text()='Location:']]", agenda.Id.ToString()), LocateBy.XPath);

            if (location.IsPresent)
            {
                string locationString = location.Text;
                Location = locationString.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            }
        }
    }
}

