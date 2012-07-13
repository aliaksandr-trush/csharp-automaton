namespace RegOnline.RegressionTest.PageObject.Manager
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.DataCollection;

    public class SSOBase : Frame
    {
        public SSOBase(string name) : base(name) { }
        public SSOBase(string name, string parentFrame) : base(name, parentFrame) { }

        public RadioButton SSORadio = new RadioButton("ctl00_cphDialog_xAuth_rdoSSO", LocateBy.Id);
        public TextBox EndpointURL = new TextBox("ctl00_cphDialog_xAuth_txtSSOEndpointUrl", LocateBy.Id);
        public TextBox LoginURL = new TextBox("ctl00_cphDialog_xAuth_txtSSOLoginUrl", LocateBy.Id);
        public ButtonOrLink SaveAndClose = new ButtonOrLink("ctl00_cphDialog_saveForm", LocateBy.Id);
        public ButtonOrLink Cancel = new ButtonOrLink("//div[@class='divButtons']/input[@class='ButtonCancel']", LocateBy.XPath);

        public Label ErrorMessage(string errorMessage)
        {
            return new Label(string.Format("//*[@id='ctl00_cphDialog_xAuth_xAuthValSummary']//li[text()='{0}']", errorMessage), LocateBy.XPath);
        }

        public CheckBox RegTypeEnabled(RegType regType)
        {
            return new CheckBox(string.Format("ctl00_cphDialog_xAuth_{0}", regType.RegTypeId), LocateBy.Id);
        }

        public void SaveAndClose_Click()
        {
            this.SaveAndClose.WaitForDisplay();
            this.SaveAndClose.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            SelectParentFrame();
        }

        public void Cancel_Click()
        {
            this.Cancel.WaitForDisplay();
            this.Cancel.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            SelectParentFrame();
        }
    }
}
