namespace RegOnline.RegressionTest.PageObject.Register
{
    using System;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;
    using OpenQA.Selenium;
    using System.Collections.Generic;

    public class Checkin : Window
    {
        public Input EmailAddress = new Input("ctl00_cph_ctlEmailMemID_txtEmail", LocateBy.Id);
        public Input VerifyEmailAddress = new Input("//input[@id='ctl00_cph_ctlEmailMemID_txtVerifyEmail']", LocateBy.XPath);
        public MultiChoiceDropdown RegTypeDropDown = new MultiChoiceDropdown("//select[@id='ctl00_cph_ctlRegType_ddlRegistrantTypes']", LocateBy.XPath);
        public Input EventFeeDiscountCode = new Input("//input[@id='ctl00_cph_ctlRegType_txtDiscountCode']", LocateBy.XPath);
        public Label DiscountCodeRequired = new Label("//img[@alt='Required']/..[following-sibling::*[text()='Enter a discount code:']]", LocateBy.XPath);
        public Clickable ViewExistingRegistration = new Clickable("ctl00_cph_lnkLogin", LocateBy.Id);
        public Label RegTypeRadioButton = new Label("//li[@class='radioRight']", LocateBy.XPath);
        public Label EventLimitReachedMessage = new Label("//div[@id = 'pageContent']/p", LocateBy.XPath);
        public Clickable GoBack = new Clickable("Go back", LocateBy.LinkText);
        public Label AdditionalDetails = new Label(
            "//*[@class='tooltipWrapper tooltipLightbox ui-dialog-content ui-widget-content'][last()]//*[@class='tooltipWrapperContent']",
            LocateBy.XPath);
        public Clickable AdditionalDetailsClose = new Clickable("//*[text()='close']", LocateBy.XPath);
        public Clickable AddToWaitlist = new Clickable("//div[@class='buttonGroup']/button[text()='Add Yourself to Waitlist']", LocateBy.XPath);
        public Label AddedToWaitlistOfEvent = new Label("//*[text()='You have been added to the waitlist for this event.']", LocateBy.XPath);

        public void OpenUrl(DataCollection.Registrant reg)
        {
            string url = string.Empty;

            switch (reg.Register_Method)
            {
                case RegisterMethod.EventId:

                    url = string.Format(
                        "{0}{1}", 
                        ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl, 
                        reg.Event.Id);

                    break;

                case RegisterMethod.Shortcut:
                case RegisterMethod.EventWebsite:
                case RegisterMethod.EventCalendar:

                    Manager.EventList_EventRow eventRow = new Manager.EventList_EventRow(reg.Event.Id);
                    url = eventRow.EventURL;

                    break;

                case RegisterMethod.RegTypeDirectUrl:

                    reg.EventFee_Response.RegType.Id = AccessData.FetchRegTypeId(reg.Event.Id, reg.EventFee_Response.RegType.Name);

                    url = string.Format(string.Format(
                        "{0}?eventID={1}&rTypeID={2}", 
                        ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl,
                        reg.Event.Id, 
                        reg.EventFee_Response.RegType.Id));

                    break;

                case RegisterMethod.Admin:

                    url = string.Format(
                        "{0}register/checkin.aspx?MethodId=1&eventsessionId={1}&eventID={2}",
                        ConfigReader.DefaultProvider.AccountConfiguration.BaseUrlWithHttps,
                        DataCollection.FormData.EventSessionId,
                        reg.Event.Id);

                    break;

                default:
                    break;
            }

            UIUtil.DefaultProvider.OpenUrl(url);
        }

        public void ViewExistingRegistration_Click()
        {
            this.ViewExistingRegistration.WaitForDisplay();
            this.ViewExistingRegistration.Click();
            Utility.ThreadSleep(2);
            WaitForLoad();
        }

        public void GoBack_Click()
        {
            this.GoBack.WaitForDisplay();
            this.GoBack.Click();
            Utility.ThreadSleep(1);
            WaitForLoad();
        }

        public void VerifyRegTypeDisplay(RegType regType, bool expected)
        {
            RegTypeRow row = new RegTypeRow(regType);
            bool actual = true;

            if (row.Label_RegTypeName == null)
            {
                actual = false;
            }

            UIUtil.DefaultProvider.VerifyValue(
                expected,
                actual,
                string.Format("Check display of regtype '{0}'", regType.Name));
        }

        public void SelectRegTypeRadioButton(RegType regType)
        {
            RegTypeRow row = new RegTypeRow(regType);
            row.Radio_Button.WaitForDisplayAndClick();
        }

        public void RegTypeDetails_Click(RegType regType)
        {
            RegTypeRow row = new RegTypeRow(regType);
            row.Details_ClickToOpen();
        }

        public void RegTypeDetails_Close_Click()
        {
            this.AdditionalDetailsClose.WaitForDisplay();
            this.AdditionalDetailsClose.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddToWaitlist_Click()
        {
            this.AddToWaitlist.WaitForDisplay();
            this.AddToWaitlist.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class RegTypeRow
    {
        public Label Label_RegTypeName { get; set; }
        public RadioButton Radio_Button { get; set; }
        public Clickable Link_Details { get; set; }

        public RegTypeRow(DataCollection.RegType regType)
        {
            List<IWebElement> labels = UIUtil.DefaultProvider.GetElements(
                string.Format("//ol[@id='radRegTypes']//label[contains(text(),'{0}')]", regType.Name),
                LocateBy.XPath);

            foreach (IWebElement label in labels)
            {
                string labelText = label.Text.Trim();

                // Note that a regtype with fee and additional details should be displayed like "RegType1: $80 Details"
                if (!string.IsNullOrEmpty(regType.AdditionalDetails))
                {
                    labelText = labelText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                }

                if (regType.Price.HasValue)
                {
                    labelText = labelText.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0];
                }

                if (labelText.Equals(regType.Name))
                {
                    string forAttribute = label.GetAttribute("for");
                    regType.Id = Convert.ToInt32(forAttribute.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries)[1]);

                    this.Label_RegTypeName = new Label(
                        string.Format("//ol[@id='radRegTypes']//label[@for='radRegType_{0}']", regType.Id),
                        LocateBy.XPath);

                    this.Radio_Button = new RadioButton(string.Format("radRegType_{0}", regType.Id), LocateBy.Id);
                    this.Link_Details = new Clickable(string.Format("//a[@data-id='details_{0}']", regType.Id), LocateBy.XPath);
                }
            }
        }

        public void Details_ClickToOpen()
        {
            this.Link_Details.WaitForDisplay();
            this.Link_Details.Click();
            Utility.ThreadSleep(1);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.WaitForAJAX();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.WaitForLoad();
        }
    }
}
