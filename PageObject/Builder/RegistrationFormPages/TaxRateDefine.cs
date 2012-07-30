namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
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
        public CheckBox ApplyToSelectedCountry = new CheckBox("ctl00_cphDialog_ucTaxRates_chkEventsApplyTaxesCountries", LocateBy.Id);
        #endregion

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        public CheckBox ApplyToCountry(DataCollection.FormData.Countries country)
        {
            return new CheckBox(string.Format("//table[@class='countryTable']//input[following-sibling::label[text()='{0}']]",
                CustomStringAttribute.GetCustomString(country)), LocateBy.XPath);
        }

        #region Basic Actions
        public void SaveAndStay_Click()
        {
            popupFrameHelper.SaveAndStay_Click();
        }

        public void ApplyToSelectedCountry_Set(bool check)
        {
            this.ApplyToSelectedCountry.WaitForDisplay();
            this.ApplyToSelectedCountry.Set(check);
            Utility.ThreadSleep(1);
            WaitForAJAX();
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
