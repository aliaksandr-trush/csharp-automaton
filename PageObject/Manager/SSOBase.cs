namespace RegOnline.RegressionTest.PageObject.Manager
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.DataCollection;

    public class SSOBase : Frame
    {
        public SSOBase(string name) : base(name) { }

        public RadioButton SSORadio = new RadioButton("ctl00_cphDialog_xAuth_rdoSSO", LocateBy.Id);
        public Input EndpointURL = new Input("ctl00_cphDialog_xAuth_txtSSOEndpointUrl", LocateBy.Id);
        public Input LoginURL = new Input("ctl00_cphDialog_xAuth_txtSSOLoginUrl", LocateBy.Id);
        public Clickable SaveAndClose = new Clickable("ctl00_cphDialog_saveForm", LocateBy.Id);
        public Clickable Cancel = new Clickable("//div[@class='divButtons']/input[@class='ButtonCancel']", LocateBy.XPath);

        public Label ErrorMessage(string errorMessage)
        {
            return new Label(string.Format("//*[@id='ctl00_cphDialog_xAuth_xAuthValSummary']//li[text()='{0}']", errorMessage), LocateBy.XPath);
        }

        public CheckBox RegTypeEnabled(RegType regType)
        {
            return new CheckBox(string.Format("//td[text()='{0}']/following-sibling::td/input", regType.RegTypeName), LocateBy.XPath);
        }

        public void SaveAndClose_Click()
        {
            this.SaveAndClose.WaitForDisplay();
            this.SaveAndClose.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            this.Cancel.WaitForDisplay();
            this.Cancel.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            SwitchToMain();
        }
    }
}
