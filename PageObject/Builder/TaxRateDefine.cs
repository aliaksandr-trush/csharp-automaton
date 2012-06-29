namespace RegOnline.RegressionTest.PageObject.Builder
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public class TaxRateDefine : Frame
    {
        public TaxRateDefine(string name) : base(name) { }
        public TaxRateDefine(string name, string parentFrame) : base(name, parentFrame) { }

        #region WebElements
        public TextBox TaxOneCaption = new TextBox("ctl00_cphDialog_ucTaxRates_txtEventsTaxCaption1", LocateBy.Id);
        public TextBox TaxOneRate = new TextBox("ctl00_cphDialog_ucTaxRates_txtEventsTaxRate1_text", LocateBy.Id);
        public TextBox TaxTwoCaption = new TextBox("ctl00_cphDialog_ucTaxRates_txtEventsTaxCaption2", LocateBy.Id);
        public TextBox TaxTwoRate = new TextBox("ctl00_cphDialog_ucTaxRates_txtEventsTaxRate2_text", LocateBy.Id);
        #endregion

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        #region Basic Actions
        public void SaveAndStay_Click()
        {
            popupFrameHelper.SaveAndStay_Click();
        }

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            SelectParentFrame();
        }

        public void Cancel_Click()
        {
            popupFrameHelper.Cancel_Click();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            SelectParentFrame();
        }
        #endregion
    }
}
