namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class MerchandiseDefine : Frame
    {
        public MerchandiseDefine(string name) : base(name) { }

        #region WebElements
        public MultiChoiceDropdown MerchandiseType = new MultiChoiceDropdown("ctl00_cphDialog_feeAmountTypeId", LocateBy.Id);
        public TextBox FeeAmount = new TextBox("ctl00_cphDialog_feeAmount_text", LocateBy.Id);
        public TextBox VariableFeeMinAmount = new TextBox("ctl00_cphDialog_feeMinVarAmount_text", LocateBy.Id);
        public TextBox VariableFeeMaxAmount = new TextBox("ctl00_cphDialog_feeMaxVarAmount_text", LocateBy.Id);
        public TextBox NameOnForm = new TextBox("ctl00_cphDialog_elDescription_TextArea", LocateBy.Id);
        public TextBox NameOnReceipt = new TextBox("ctl00_cphDialog_elReportDescription_TextArea", LocateBy.Id);
        public TextBox NameOnReports = new TextBox("ctl00_cphDialog_feeFieldName", LocateBy.Id);
        public ButtonOrLink AddTaxRate = new ButtonOrLink("ctl00_cphDialog_lbnTaxes", LocateBy.Id);
        public CheckBox ApplyTaxOne = new CheckBox("ctl00_cphDialog_chkListTaxRates_0", LocateBy.Id);
        public CheckBox ApplyTaxTwo = new CheckBox("ctl00_cphDialog_chkListTaxRates_1", LocateBy.Id);
        public TaxRateDefine TaxRateDefine = new TaxRateDefine("dialog2", "dialog");
        #endregion

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        public void MerchandiseType_Select(string type)
        {
            this.MerchandiseType.SelectWithValue(type);
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void AddTaxRate_Click()
        {
            this.AddTaxRate.WaitForDisplay();
            this.AddTaxRate.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void SaveAndStay_Click()
        {
            popupFrameHelper.SaveAndStay_Click();
        }

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            popupFrameHelper.Cancel_Click();
            SwitchToMain();
        }
    }
}
