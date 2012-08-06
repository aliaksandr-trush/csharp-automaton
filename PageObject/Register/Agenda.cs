namespace RegOnline.RegressionTest.PageObject.Register
{
    using System;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public class Agenda : Window
    {
        public Label AgendaDetailsPopup = new Label(
            "//div[@class='tooltipWrapper tooltipLightbox ui-dialog-content ui-widget-content']/div[@class='tooltipWrapperContent']",
            LocateBy.XPath);
        public ButtonOrLink CloseDetailsPopup = new ButtonOrLink("//span[@class='ui-icon ui-icon-closethick']", LocateBy.XPath);
        public Window AgendaDetailsWindow = new Window();

        public AgendaRow GetAgendaItem(AgendaItem agenda)
        {
            return new AgendaRow(agenda);
        }

        public bool IsChoiceItemPresent(ChoiceItem choice)
        {
            WebElement a = new WebElement(choice.Id.ToString(), LocateBy.Id);
            return a.IsPresent;
        }

        public void CloseDetailsPopup_Click()
        {
            this.CloseDetailsPopup.WaitForDisplay();
            this.CloseDetailsPopup.Click();
            Utilities.Utility.ThreadSleep(1);
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
        public TextBox DiscountCodeInput;
        public Label LimitFullMessage;
        public Label WaitlistMessage;
        public ButtonOrLink Details;

        public AgendaRow(AgendaItem agenda)
        {
            switch (agenda.Type)
            {
                case FormData.CustomFieldType.CheckBox:
                    AgendaType = new CheckBox(string.Format(locator + "//input", agenda.Id.ToString()), LocateBy.XPath);
                    AgendaLabel = new Label(string.Format(locator + "//label", agenda.Id.ToString()), LocateBy.XPath);
                    DiscountCodeInput = new TextBox("dc" + agenda.Id.ToString(), LocateBy.Id);
                    LimitFullMessage = new Label(string.Format(locator + "//div[@class='capacityMsg']", agenda.Id.ToString()), LocateBy.XPath);
                    WaitlistMessage = new Label(string.Format(locator + "//span[@class='wlist']", agenda.Id.ToString()), LocateBy.XPath);
                    Details = new ButtonOrLink(string.Format(locator + "//span/a[@href]", agenda.Id.ToString()), LocateBy.XPath);
                    this.GetAgendaDate(agenda);
                    this.GetAgendaLocation(agenda);
                    this.GetAgendaPrice(agenda);
                    break;
                case FormData.CustomFieldType.RadioButton:
                    AgendaLabel = new Label(string.Format(locator + "//p[@class='label']", agenda.Id.ToString()), LocateBy.XPath);
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
                case FormData.CustomFieldType.Number:
                case FormData.CustomFieldType.OneLineText:
                case FormData.CustomFieldType.Contribution:
                case FormData.CustomFieldType.Paragraph:
                    AgendaType = new TextBox(agenda.Id.ToString(), LocateBy.Id);
                    AgendaLabel = new Label(string.Format(locator + "//label", agenda.Id.ToString()), LocateBy.XPath);
                    break;
                case FormData.CustomFieldType.Date:
                    AgendaType = new TextBox(string.Format("//input[@id='{0}'][contains(@class, 'Datepicker')]", agenda.Id.ToString()), LocateBy.XPath);
                    AgendaLabel = new Label(string.Format(locator + "//label", agenda.Id.ToString()), LocateBy.XPath);
                    break;
                case FormData.CustomFieldType.Time:
                    AgendaType = new TextBox(string.Format("//input[@id='{0}'][contains(@class, 'Timepicker')]", agenda.Id.ToString()), LocateBy.XPath);
                    AgendaLabel = new Label(string.Format(locator + "//label", agenda.Id.ToString()), LocateBy.XPath);
                    break;
                case FormData.CustomFieldType.FileUpload:
                    AgendaType = new ButtonOrLink(string.Format(locator + "//a[@class='add_button']", agenda.Id.ToString()), LocateBy.XPath);
                    AgendaLabel = new Label(string.Format(locator + "//p[@class='label']", agenda.Id.ToString()), LocateBy.XPath);
                    this.GetAgendaLocation(agenda);
                    this.GetAgendaPrice(agenda);
                    break;
                case FormData.CustomFieldType.AlwaysSelected:
                    AgendaType = new CheckBox(string.Format("//input[@id='{0}'][@disabled='disabled'][@checked='checked']", agenda.Id.ToString()), LocateBy.XPath);
                    AgendaLabel = new Label(string.Format(locator + "//label", agenda.Id.ToString()), LocateBy.XPath);
                    this.GetAgendaDate(agenda);
                    this.GetAgendaLocation(agenda);
                    this.GetAgendaPrice(agenda);
                    break;
                case FormData.CustomFieldType.SectionHeader:
                    AgendaLabel = new Label(string.Format(locator + "//div", agenda.Id.ToString()), LocateBy.XPath);
                    break;
                case FormData.CustomFieldType.ContinueButton:
                    AgendaType = new ButtonOrLink(string.Format(locator + "//button", agenda.Id.ToString()), LocateBy.XPath);
                    break;

                case FormData.CustomFieldType.Duration:

                    AgendaType = new TextBox(
                        string.Format(locator + "//input[@id='{0}'][@class='durationEntry hasTimepicker dtPickerShadow']", agenda.Id), 
                        LocateBy.XPath);

                    AgendaLabel = new Label(string.Format(locator + "//label[@for='{0}']", agenda.Id), LocateBy.XPath);

                    break;

                default:
                    break;
            }
        }

        public DateTime? GetAgendaDate(AgendaItem agenda)
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

            return StartDate;
        }

        public double? GetAgendaPrice(AgendaItem agenda)
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

            return Price;
        }

        public string GetAgendaLocation(AgendaItem agenda)
        {
            Label location = new Label(string.Format(locator + "//div[@class='place'][span[text()='Location:']]", agenda.Id.ToString()), LocateBy.XPath);

            if (location.IsPresent)
            {
                string locationString = location.Text;
                Location = locationString.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            }

            return Location;
        }
    }
}

