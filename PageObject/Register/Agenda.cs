﻿namespace RegOnline.RegressionTest.PageObject.Register
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
        public ButtonOrLink RecalculateTotal = new ButtonOrLink("//div[@class='sectionTotal']/button[text()='Recalculate Total']", LocateBy.XPath);
        public Label Total = new Label("totalAmt", LocateBy.Id);

        public AgendaRow GetAgendaItem(AgendaItem agenda)
        {
            return new AgendaRow(agenda);
        }

        public bool IsChoiceItemPresent(ChoiceItem choice)
        {
            WebElement a = new WebElement(string.Format("//*[contains(text(),'{0}')]", choice.Name), LocateBy.XPath);
            return a.IsPresent;
        }

        public void CloseDetailsPopup_Click()
        {
            this.CloseDetailsPopup.WaitForDisplay();
            this.CloseDetailsPopup.Click();
            Utilities.Utility.ThreadSleep(1);
        }

        public void RecalculateTotal_Click()
        {
            this.RecalculateTotal.WaitForDisplay();
            this.RecalculateTotal.Click();
            Utilities.Utility.ThreadSleep(1);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class AgendaRow
    {
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
            agenda.Id = this.GetAgendaItemId(agenda);
            this.AgendaLabel = this.GetAngedaLabel(agenda);

            switch (agenda.Type)
            {
                case FormData.CustomFieldType.AlwaysSelected:
                    this.AgendaType = new CheckBox(agenda.Id.ToString(), LocateBy.Id);
                    this.DiscountCodeInput = new TextBox("dc" + agenda.Id.ToString(), LocateBy.Id);
                    this.GetAgendaDate(agenda);
                    this.GetAgendaLocation(agenda);
                    this.GetAgendaPrice(agenda);
                    break;

                case FormData.CustomFieldType.CheckBox:
                    this.AgendaType = new CheckBox(agenda.Id.ToString(), LocateBy.Id);
                    this.DiscountCodeInput = new TextBox("dc" + agenda.Id.ToString(), LocateBy.Id);
                    this.LimitFullMessage = new Label(string.Format("//li[@data-id='{0}']//div[@class='capacityMsg']", agenda.Id.ToString()), LocateBy.XPath);
                    this.WaitlistMessage = new Label(string.Format("//li[@data-id='{0}']//span[@class='wlist']", agenda.Id.ToString()), LocateBy.XPath);
                    this.Details = new ButtonOrLink(string.Format("//li[@data-id='{0}']//span/a[@href]", agenda.Id.ToString()), LocateBy.XPath);
                    this.GetAgendaDate(agenda);
                    this.GetAgendaLocation(agenda);
                    this.GetAgendaPrice(agenda);
                    break;

                case FormData.CustomFieldType.RadioButton:
                    this.DiscountCodeInput = new TextBox("dc" + agenda.Id.ToString(), LocateBy.Id);
                    this.GetAgendaDate(agenda);
                    this.GetAgendaLocation(agenda);
                    this.GetAgendaPrice(agenda);
                    break;

                case FormData.CustomFieldType.Dropdown:
                    this.AgendaType = new MultiChoiceDropdown(agenda.Id.ToString(), LocateBy.Id);
                    this.DiscountCodeInput = new TextBox("dc" + agenda.Id.ToString(), LocateBy.Id);
                    this.GetAgendaDate(agenda);
                    this.GetAgendaLocation(agenda);
                    this.GetAgendaPrice(agenda);
                    break;

                case FormData.CustomFieldType.Number:
                case FormData.CustomFieldType.OneLineText:
                case FormData.CustomFieldType.Contribution:
                case FormData.CustomFieldType.Paragraph:
                case FormData.CustomFieldType.Date:
                case FormData.CustomFieldType.Time:
                case FormData.CustomFieldType.Duration:
                    this.AgendaType = new TextBox(agenda.Id.ToString(), LocateBy.Id);
                    break;

                case FormData.CustomFieldType.FileUpload:
                    this.AgendaType = new ButtonOrLink(
                        string.Format("//li[@data-id='{0}']//a[@class='add_button']", agenda.Id.ToString()), 
                        LocateBy.XPath);

                    this.GetAgendaLocation(agenda);
                    this.GetAgendaPrice(agenda);
                    break;

                case FormData.CustomFieldType.SectionHeader:
                    break;

                case FormData.CustomFieldType.ContinueButton:
                    this.AgendaType = new ButtonOrLink(string.Format("ctl00${0}", agenda.Id.ToString()), LocateBy.Name);
                    break;

                default:
                    break;
            }
        }

        private int GetAgendaItemId(AgendaItem agenda)
        {
            WebElement element_Li = null;
            
            switch (agenda.Type)
            {
                case FormData.CustomFieldType.OneLineText:
                case FormData.CustomFieldType.CheckBox:
                case FormData.CustomFieldType.Paragraph:
                case FormData.CustomFieldType.Date:
                case FormData.CustomFieldType.Time:
                case FormData.CustomFieldType.AlwaysSelected:
                case FormData.CustomFieldType.Contribution:
                case FormData.CustomFieldType.Number:
                case FormData.CustomFieldType.Dropdown:
                case FormData.CustomFieldType.Duration:
                    element_Li = new WebElement(
                        string.Format("//div[@id='pageContent']//legend/following-sibling::ol/li[div[label[text()='{0}']]]", agenda.NameOnForm),
                        LocateBy.XPath);

                    break;

                case FormData.CustomFieldType.SectionHeader:
                    element_Li = new WebElement(
                        string.Format("//div[@id='pageContent']//legend/following-sibling::ol/li[div[contains(text(),'{0}')]]", agenda.NameOnForm),
                        LocateBy.XPath);

                    break;

                case FormData.CustomFieldType.RadioButton:
                case FormData.CustomFieldType.FileUpload:
                case FormData.CustomFieldType.ContinueButton:
                    element_Li = new WebElement(
                        string.Format("//div[@id='pageContent']//legend/following-sibling::ol/li[div[p[text()='{0}']]]", agenda.NameOnForm), 
                        LocateBy.XPath);

                    break;

                default:
                    break;
            }
            
            return Convert.ToInt32(element_Li.GetAttribute("data-id"));
        }

        private Label GetAngedaLabel(AgendaItem agenda)
        {
            switch (agenda.Type)
            {
                case FormData.CustomFieldType.OneLineText:
                case FormData.CustomFieldType.CheckBox:
                case FormData.CustomFieldType.Paragraph:
                case FormData.CustomFieldType.Date:
                case FormData.CustomFieldType.Time:
                case FormData.CustomFieldType.AlwaysSelected:
                case FormData.CustomFieldType.Contribution:
                case FormData.CustomFieldType.Number:
                case FormData.CustomFieldType.Dropdown:
                case FormData.CustomFieldType.Duration:
                    return new Label(
                        string.Format("//li[@data-id='{0}']//label[@for='{0}']", agenda.Id.ToString()), 
                        LocateBy.XPath);

                case FormData.CustomFieldType.SectionHeader:
                    return new Label(
                        string.Format("//li[@data-id='{0}']//div[contains(text(),'{1}')]", agenda.Id.ToString(), agenda.NameOnForm),
                        LocateBy.XPath);
                    
                case FormData.CustomFieldType.RadioButton:
                case FormData.CustomFieldType.FileUpload:
                case FormData.CustomFieldType.ContinueButton:
                    return new Label(
                        string.Format("//li[@data-id='{0}']//p[text()='{1}']", agenda.Id.ToString(), agenda.NameOnForm), 
                        LocateBy.XPath);

                default:
                    return null;
            }
        }

        public DateTime? GetAgendaDate(AgendaItem agenda)
        {
            Label dateTime = new Label(string.Format("//li[@data-id='{0}']//div[@class='place'][span[text()='Date:']]", agenda.Id.ToString()), LocateBy.XPath);
            Label time = new Label(string.Format("//li[@data-id='{0}']//div[@class='place'][span[text()='Time:']]", agenda.Id.ToString()), LocateBy.XPath);
            
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
            Label price = new Label(string.Format("//li[@data-id='{0}']//div[@class='place'][span[text()='Price:']]", agenda.Id.ToString()), LocateBy.XPath);

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
            Label location = new Label(string.Format("//li[@data-id='{0}']//div[@class='place'][span[text()='Location:']]", agenda.Id.ToString()), LocateBy.XPath);

            if (location.IsPresent)
            {
                string locationString = location.Text;
                Location = locationString.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            }

            return Location;
        }
    }
}

