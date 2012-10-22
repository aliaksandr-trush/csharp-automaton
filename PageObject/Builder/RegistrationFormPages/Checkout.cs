namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class Checkout : Window
    {
        public Clickable AddPaymentMethod = new Clickable("//*[@id='ctl00_cph_divAddPaymentMethod']/a", LocateBy.XPath);
        public PaymentMethodSelections PaymentMethodSelections = new PaymentMethodSelections("dialog");
        public Clickable CheckoutPageHeader = new Clickable("//*[text()='Add Checkout Page Header']", LocateBy.XPath);
        public HtmlEditor CheckoutPageHeaderEditor = new HtmlEditor("dialog");
        public Clickable CheckoutPageFooter = new Clickable("//*[text()='Add Checkout Page Footer']", LocateBy.XPath);
        public HtmlEditor CheckoutPageFooterEditor = new HtmlEditor("dialog");
        public RadioButton AddServiceFee = new RadioButton("ctl00_cph_radShare", LocateBy.Id);

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

        public void SaveAndClose_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndClose_Click();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            PageObjectHelper.PopupFrame_Helper.Cancel_Click();
            SwitchToMain();
        }
    }
}
