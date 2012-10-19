namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class TaxRateDefine : Frame
    {
        public TaxRateDefine(string name) : base(name) { }

        #region WebElements
        public Input TaxOneCaption = new Input("ctl00_cphDialog_ucTaxRates_txtEventsTaxCaption1", LocateBy.Id);
        public Input TaxOneRate = new Input("ctl00_cphDialog_ucTaxRates_txtEventsTaxRate1_text", LocateBy.Id);
        public Input TaxTwoCaption = new Input("ctl00_cphDialog_ucTaxRates_txtEventsTaxCaption2", LocateBy.Id);
        public Input TaxTwoRate = new Input("ctl00_cphDialog_ucTaxRates_txtEventsTaxRate2_text", LocateBy.Id);
        public CheckBox ApplyToSelectedCountry = new CheckBox("ctl00_cphDialog_ucTaxRates_chkEventsApplyTaxesCountries", LocateBy.Id);
        #endregion

        public CheckBox ApplyToCountry(DataCollection.FormData.Country country)
        {
            return new CheckBox(string.Format("//table[@class='countryTable']//input[following-sibling::label[text()='{0}']]",
                CustomStringAttribute.GetCustomString(country)), LocateBy.XPath);
        }

        #region Basic Actions
        public void SaveAndStay_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndStay_Click();
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
            PageObjectHelper.PopupFrame_Helper.SaveAndClose_Click();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            PageObjectHelper.PopupFrame_Helper.Cancel_Click();
            SwitchToMain();
        }
        #endregion
    }
}
