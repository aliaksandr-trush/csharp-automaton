namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class Checkin : Window
    {
        public TextBox EmailAddress = new TextBox("ctl00_cph_ctlEmailMemID_txtEmail", LocateBy.Id);
        public TextBox VerifyEmailAddress = new TextBox("//input[@id='ctl00_cph_ctlEmailMemID_txtVerifyEmail']", LocateBy.XPath);
        public MultiChoiceDropdown RegTypeDropDown = new MultiChoiceDropdown("//select[@id='ctl00_cph_ctlRegType_ddlRegistrantTypes']", LocateBy.XPath);
        public TextBox EventFeeDiscountCode = new TextBox("//input[@id='ctl00_cph_ctlRegType_txtDiscountCode']", LocateBy.XPath);
        public Label DiscountCodeRequired = new Label("//img[@alt='Required']/..[following-sibling::*[text()='Enter a discount code:']]", LocateBy.XPath);
        public ButtonOrLink ViewExistingRegistration = new ButtonOrLink("ctl00_cph_lnkLogin", LocateBy.Id);
        public Label RegTypeRadioButton = new Label("//li[@class='radioRight']", LocateBy.XPath);
        public Label EventLimitReachedMessage = new Label("//div[@id = 'pageContent']/p", LocateBy.XPath);
        public ButtonOrLink GoBack = new ButtonOrLink("Go back", LocateBy.LinkText);
        public Label AdditionalDetails = new Label(
            "//*[@class='tooltipWrapper tooltipLightbox ui-dialog-content ui-widget-content'][last()]//*[@class='tooltipWrapperContent']",
            LocateBy.XPath);
        public ButtonOrLink AdditionalDetailsClose = new ButtonOrLink("//*[text()='close']", LocateBy.XPath);
        public ButtonOrLink AddToWaitlist = new ButtonOrLink("//div[@class='buttonGroup']/button[text()='Add Yourself to Waitlist']", LocateBy.XPath);
        public Label AddedToWaitlistOfEvent = new Label("//*[text()='You have been added to the waitlist for this event.']", LocateBy.XPath);

        public RadioButton RegTypeRadio(string regTypeName)
        {
            return new RadioButton(
                string.Format("//ol[@id='radRegTypes']/li/label[contains(text(),'{0}')]/../input", regTypeName), LocateBy.XPath);
        }

        public void SelectRegTypeRadioButton(string regTypeName)
        {
            RadioButton RegTypeRadioButton = this.RegTypeRadio(regTypeName);

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
            ButtonOrLink detailsLink = new ButtonOrLink(string.Format(
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
