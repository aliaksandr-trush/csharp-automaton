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
        public Input FeeAmount = new Input("ctl00_cphDialog_feeAmount_text", LocateBy.Id);
        public Input VariableFeeMinAmount = new Input("ctl00_cphDialog_feeMinVarAmount_text", LocateBy.Id);
        public Input VariableFeeMaxAmount = new Input("ctl00_cphDialog_feeMaxVarAmount_text", LocateBy.Id);
        public Input NameOnForm = new Input("ctl00_cphDialog_elDescription_TextArea", LocateBy.Id);
        public Input NameOnReceipt = new Input("ctl00_cphDialog_elReportDescription_TextArea", LocateBy.Id);
        public Input NameOnReports = new Input("ctl00_cphDialog_feeFieldName", LocateBy.Id);
        public Clickable AddTaxRate = new Clickable("ctl00_cphDialog_lbnTaxes", LocateBy.Id);
        public CheckBox ApplyTaxOne = new CheckBox("ctl00_cphDialog_chkListTaxRates_0", LocateBy.Id);
        public CheckBox ApplyTaxTwo = new CheckBox("ctl00_cphDialog_chkListTaxRates_1", LocateBy.Id);
        public TaxRateDefine TaxRate_Define = new TaxRateDefine("dialog2");
        public Input DiscountCodes = new Input("ctl00_cphDialog_feepassword", LocateBy.Id);
        #endregion

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
            PageObjectHelper.PopupFrame_Helper.SaveAndStay_Click();
        }

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
