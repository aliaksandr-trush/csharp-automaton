namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using System.Text;

    public class Checkout : Window
    {
        public MultiChoiceDropdown PaymentMethodList = new MultiChoiceDropdown("ctl00_cph_ddlPaymentMethods", LocateBy.Id);
        public Clickable Finish = new Clickable("//div[@class='buttonGroup']//button[@type='submit']", LocateBy.XPath);
        public Clickable AANoThanks = new Clickable("//div[@class='offerSubmit']//span[text()='No thanks']/..", LocateBy.XPath);
        public CheckBox AggreementToWaiver = new CheckBox("ctl00_cph_chkAITerms", LocateBy.Id);
        public MultiChoiceDropdown BillingInfo_CCType = new MultiChoiceDropdown("ctl00_cph_ddlCCType", LocateBy.Id);
        public Input BillingInfo_CCNumber = new Input("ctl00_cph_txtCC", LocateBy.Id);
        public Input BillingInfo_CVV = new Input("ctl00_cph_txtCVV", LocateBy.Id);
        public MultiChoiceDropdown BillingInfo_ExpirationDate_Month = new MultiChoiceDropdown("ctl00_cph_ddlCCExpMonth", LocateBy.Id);
        public MultiChoiceDropdown BillingInfo_ExpirationDate_Year = new MultiChoiceDropdown("ctl00_cph_ddlCCExpYear", LocateBy.Id);
        public Input BillingInfo_CardholderName = new Input("ctl00_cph_txtCCName", LocateBy.Id);
        public MultiChoiceDropdown BillingInfo_Country = new MultiChoiceDropdown("ctl00_cph_ddlCCCountry", LocateBy.Id);
        public Input BillingInfo_AddressLineOne = new Input("ctl00_cph_txtCCAddress", LocateBy.Id);
        public Input BillingInfo_AddressLineTwo = new Input("ctl00_cph_txtCCAddress2", LocateBy.Id);
        public Input BillingInfo_City = new Input("ctl00_cph_txtCCCity", LocateBy.Id);
        public Input BillingInfo_State = new Input("ctl00_cph_txtCCState", LocateBy.Id);
        public Input BillingInfo_Zip = new Input("ctl00_cph_txtCCZip", LocateBy.Id);

        public Label FeeSummary_Total = new Label(
            "//table[@class='columnHeader dataTable multiColumn']//td[text()='Total:']/following-sibling::td[@class='currency']", 
            LocateBy.XPath);

        public void Finish_Click()
        {
            this.Finish.WaitForDisplay();
            this.Finish.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        // 'AA' means ActiveAdvantage
        public void AANoThanks_Click()
        {
            this.AANoThanks.WaitForDisplay();
            this.AANoThanks.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void BillingInfo_Type(DataCollection.BillingInfo info)
        {
            this.BillingInfo_CCType.WaitForDisplay();
            this.BillingInfo_CCType.SelectWithText(Utilities.CustomStringAttribute.GetCustomString(info.CC_Type));
            this.BillingInfo_CCNumber.Type(info.CC_Number);
            this.BillingInfo_CVV.Type(info.SecurityCode);
            this.BillingInfo_ExpirationDate_Month.WaitForDisplay();
            this.BillingInfo_ExpirationDate_Month.SelectWithValue(info.ExpirationDate.Month.ToString());
            this.BillingInfo_ExpirationDate_Year.SelectWithValue(info.ExpirationDate.Year.ToString());
            this.BillingInfo_CardholderName.Type(info.CardholderName);
            this.BillingInfo_Country.WaitForDisplay();
            this.BillingInfo_Country.SelectWithText(Utilities.CustomStringAttribute.GetCustomString(info.CC_Country));
            this.BillingInfo_AddressLineOne.Type(info.BillingAddressLineOne);
            this.BillingInfo_AddressLineTwo.Type(info.BillingAddressLineTwo);
            this.BillingInfo_City.Type(info.BillingCity);
            this.BillingInfo_State.Type(info.BillingStateOrProvince);
            this.BillingInfo_Zip.Type(info.BillingZip);
        }

        public void FailTestWithErrorMessages()
        {
            int messages_Count = UIUtility.UIUtil.DefaultProvider.GetXPathCountByXPath("//div[@id='ctl00_valSummary']/ul/li");
            StringBuilder errorMessage = new StringBuilder();
            
            for (int cnt = 1; cnt <= messages_Count; cnt++)
            {
                errorMessage.Append(string.Format("Error{0}:'", cnt));
                errorMessage.Append(UIUtility.UIUtil.DefaultProvider.GetText(string.Format("//div[@id='ctl00_valSummary']/ul/li[{0}]", cnt), LocateBy.XPath));
                errorMessage.Append("';");
            }

            errorMessage.Replace(';', '.', errorMessage.Length - 1, 1);

            UIUtility.UIUtil.DefaultProvider.FailTest(errorMessage.ToString());
        }
    }
}
