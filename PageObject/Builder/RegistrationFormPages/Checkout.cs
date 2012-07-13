namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class Checkout : Window
    {
        public ButtonOrLink AddPaymentMethod = new ButtonOrLink("//*[@id='ctl00_cph_divAddPaymentMethod']/a", LocateBy.XPath);
        public PaymentMethodSelections PaymentMethodSelections = new PaymentMethodSelections("dialog");
        public ButtonOrLink CheckoutPageHeader = new ButtonOrLink("//*[text()='Add Checkout Page Header']", LocateBy.XPath);
        public HtmlEditor CheckoutPageHeaderEditor = new HtmlEditor("dialog");
        public ButtonOrLink CheckoutPageFooter = new ButtonOrLink("//*[text()='Add Checkout Page Footer']", LocateBy.XPath);
        public HtmlEditor CheckoutPageFooterEditor = new HtmlEditor("dialog");

        public void AddPaymentMethod_Click()
        {
            this.AddPaymentMethod.WaitForDisplay();
            this.AddPaymentMethod.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void CheckoutPageHeader_Click()
        {
            this.CheckoutPageHeader.WaitForDisplay();
            this.CheckoutPageHeader.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void CheckoutPageFooter_Click()
        {
            this.CheckoutPageFooter.WaitForDisplay();
            this.CheckoutPageFooter.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class PaymentMethodSelections : Frame
    {
        public PaymentMethodSelections(string name) : base(name) { }

        public MultiChoiceDropdown PaymentMethods = new MultiChoiceDropdown("ctl00_cphDialog_ddlPaymentMethod", LocateBy.Id);

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        public void Cancel_Click()
        {
            popupFrameHelper.Cancel_Click();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }
    }
}
