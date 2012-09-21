﻿namespace RegOnline.RegressionTest.PageObject.Register
{
    using System;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

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

                    reg.EventFee_Response.RegType.RegTypeId = AccessData.FetchRegTypeId(reg.Event.Id, reg.EventFee_Response.RegType.RegTypeName);

                    url = string.Format(string.Format(
                        "{0}?eventID={1}&rTypeID={2}", 
                        ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl,
                        reg.Event.Id, 
                        reg.EventFee_Response.RegType.RegTypeId));

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

            WebDriverUtility.DefaultProvider.OpenUrl(url);
        }

        public RadioButton RegTypeRadio(RegType regType)
        {
            Label label = this.GetRegTypeLabel(regType);
            string attribute_For = label.GetAttribute("for");
            string tmp = attribute_For.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries)[1];
            regType.RegTypeId = Convert.ToInt32(tmp);

            return new RadioButton(attribute_For, LocateBy.Id);
        }

        public void VerifyRegTypeDisplay(RegType regType, bool expected)
        {
            Label label = this.GetRegTypeLabel(regType);
            bool actual = label.IsDisplay;

            WebDriverUtility.DefaultProvider.VerifyValue(
                expected,
                actual,
                string.Format("Check display of regtype '{0}'", regType.RegTypeName));
        }

        private Label GetRegTypeLabel(RegType regType)
        {
            return new Label(
                string.Format("//ol[@id='radRegTypes']//label[contains(text(),'{0}')]", regType.RegTypeName), 
                LocateBy.XPath);
        }

        public void SelectRegTypeRadioButton(RegType regType)
        {
            RadioButton RegTypeRadioButton = this.RegTypeRadio(regType);

            RegTypeRadioButton.WaitForDisplay();
            RegTypeRadioButton.Click();
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

        public void RegTypeDetails_Click(RegType regType)
        {
            Clickable detailsLink = new Clickable(string.Format(
                "//label[contains(text(),'AdditionalDetails')]//a", regType.RegTypeName), LocateBy.XPath);

            detailsLink.WaitForDisplay();
            detailsLink.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AdditionalDetailsClose_Click()
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
}
