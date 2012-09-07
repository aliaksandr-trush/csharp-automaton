namespace RegOnline.RegressionTest.Managers.Backend
{
    using System.Collections.Generic;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.UIUtility;

    public partial class BackendPaymentMethodManager : ManagerBase
    {
        #region consts
        private static Dictionary<PaymentMethodManager.PaymentMethod, int> entryByMethod =
            new Dictionary<PaymentMethod, int>()
            {
                {PaymentMethodManager.PaymentMethod.Cash, 4 },
                {PaymentMethodManager.PaymentMethod.Check, 2 },
                {PaymentMethodManager.PaymentMethod.CostCenter, 6 },
                {PaymentMethodManager.PaymentMethod.WireTransfer, 7 },
                {PaymentMethodManager.PaymentMethod.Custom, 13 },
                {PaymentMethodManager.PaymentMethod.PayAtTheEvent, 5 },
                {PaymentMethodManager.PaymentMethod.PayPal, 14 },
                {PaymentMethodManager.PaymentMethod.PurchaseOrder, 3 },
                {PaymentMethodManager.PaymentMethod.CreditCard, 1 }
               // {PaymentMethodManager.PaymentMethod.eCheck, 12 }
            };
        private const string CCTypeLocator = "//select[@id='creditCardType']";
        private const string CCNumberLocator = "ccNumber";
        private const string CCExpMonthLocator = "//select[@name='CCExpMonth']";
        private const string CCExpYearLocator = "//select[@name='CCExpYear']";
        private const string CCNameLocator = "CCName";
        private const string CCCountryLocator = "//select[@id='ccCountry']";
        private const string CCAddressLocator = "ccAddress";
        private const string CCAddress2Locator = "ccAddress2";
        private const string CCCityLocator = "ccCity";
        private const string CCStateLocator = "ccState";
        private const string CCZipLocator = "ccZip";
        
        //To Be Implemented.
        private const string CheckNumberLocator = "Name=Payment_Doc_No";
        private const string PONumberLocator = "Name=Payment_Doc_No";
        private const string CostCenterCodeLocator = "Name=Payment_Doc_No";
        private const string CustomCodeLocator = "Name=Payment_Doc_No";
        
        //...and all the eCheck stuff; however, this isn't supported, so implementing that would be a very low priority
        
        #endregion

        #region entry
        public void SelectPayBy(PaymentMethodManager.PaymentMethod selection)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(string.Format("//input[@value='{0}']", entryByMethod[selection]), LocateBy.XPath);
        }
        
        public void SelectCreditCardType(string selection)
        {
            WebDriverUtility.DefaultProvider.SelectWithText(CCTypeLocator, selection, LocateBy.XPath);
        }
        public void TypeCreditCardNumber(string selection)
        {
            WebDriverUtility.DefaultProvider.Type(CCNumberLocator, selection, LocateBy.Id);
        }
        public void SelectCreditCardExpirationMonth(string selection)
        {
            WebDriverUtility.DefaultProvider.SelectWithText(CCExpMonthLocator, selection, LocateBy.XPath);
        }
        public void SelectCreditCardExpirationYear(string selection)
        {
            WebDriverUtility.DefaultProvider.SelectWithText(CCExpYearLocator, selection, LocateBy.XPath);
        }
        public void TypeCreditCardName(string selection)
        {
            WebDriverUtility.DefaultProvider.Type(CCNameLocator, selection, LocateBy.Name);
        }
        public void SelectCreditCardCountry(string selection)
        {
            WebDriverUtility.DefaultProvider.SelectWithText(CCCountryLocator, selection, LocateBy.XPath);
        }
        public void TypeCreditCardAddress(string selection)
        {
            WebDriverUtility.DefaultProvider.Type(CCAddressLocator, selection, LocateBy.Name);
        }
        public void TypeCreditCardAddress2(string selection)
        {
            WebDriverUtility.DefaultProvider.Type(CCAddress2Locator, selection, LocateBy.Name);
        }
        public void TypeCreditCardCity(string selection)
        {
            WebDriverUtility.DefaultProvider.Type(CCCityLocator, selection, LocateBy.Name);
        }
        public void TypeCreditCardState(string selection)
        {
            WebDriverUtility.DefaultProvider.Type(CCStateLocator, selection, LocateBy.Name);
        }
        public void TypeCreditCardZip(string selection)
        {
            WebDriverUtility.DefaultProvider.Type(CCZipLocator, selection, LocateBy.Name);
        }

        public void TypeCheckNumber(string selection)
        {
            WebDriverUtility.DefaultProvider.Type(CheckNumberLocator, selection, LocateBy.Name);
        }
        public void TypePONumber(string selection)
        {
            WebDriverUtility.DefaultProvider.Type(PONumberLocator, selection, LocateBy.Name);
        }
        public void TypeCostCenterCode(string selection)
        {
            WebDriverUtility.DefaultProvider.Type(CostCenterCodeLocator, selection, LocateBy.Name);
        }
        public void TypeCustomCode(string selection)
        {
            WebDriverUtility.DefaultProvider.Type(CustomCodeLocator, selection, LocateBy.Name);
        }

        public void SaveAndClose()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//input[@type='submit']", LocateBy.XPath);
        }
        public void Close()
        {
            WebDriverUtility.DefaultProvider.ClosePopUpWindow();
        }
        #endregion

        #region verify
        //on the pattern of the BackendManagerVerify partial-class.
        //here, a #region.

        //public void VerifyCCInformation(C.CreditCardDetails ccDetail)
        //{
        //    string CardholderName = UIUtilityProvider.UIHelper.GetValue(CCNameLocator, LocateBy.Name);
        //    Assert.AreEqual(ccDetail.CardholderName, CardholderName);
        //    int ccExpMonth = int.Parse(UIUtilityProvider.UIHelper.GetText(CCExpMonthLocator + "/option[@selected=\"\"]", LocateBy.XPath).Trim());//gets text; no value here
        //    Assert.AreEqual(ccDetail.ExpirationDate.Month, ccExpMonth);
        //    int ccExpYear = int.Parse(UIUtilityProvider.UIHelper.GetText(CCExpYearLocator + "/option[@selected=\"\"]", LocateBy.XPath));//no value
        //    Assert.AreEqual(ccDetail.ExpirationDate.Year, ccExpYear);
        //    string ccType = UIUtilityProvider.UIHelper.GetText(CCTypeLocator + "/option[@selected=\"\"]", LocateBy.XPath);//some form of GetValue might also work: visa 1, mastercard 2.
        //    Assert.AreEqual(ccDetail.Type.ToString(), ccType);
        //    string ccNumber = UIUtilityProvider.UIHelper.GetValue(CCNumberLocator, LocateBy.Id);
        //    Assert.AreEqual((ccDetail.Number % 10000).ToString(), ccNumber.Substring(ccNumber.Length-4));//it appears as ***...1234

        //    C.Address address = ccDetail.Address;
        //    string ccCountry = UIUtilityProvider.UIHelper.GetText(CCCountryLocator + "/option[@selected=\"\"]", LocateBy.XPath);//text is value
        //    Assert.AreEqual(address.Country, ccCountry);
        //    string ccAddress = UIUtilityProvider.UIHelper.GetValue(CCAddressLocator, LocateBy.Name);
        //    Assert.AreEqual(address.Line1, ccAddress);
        //    if (UIUtilityProvider.UIHelper.IsElementPresent(CCAddress2Locator, LocateBy.Name))
        //    {
        //        //because it exists for ROL Gateway but not FirstDataGlobal Customer
        //        string ccAddress2 = UIUtilityProvider.UIHelper.GetValue(CCAddress2Locator, LocateBy.Name);
        //        Assert.AreEqual(address.Line2 ?? string.Empty, ccAddress2);
        //    }
        //    string ccCity = UIUtilityProvider.UIHelper.GetValue(CCCityLocator, LocateBy.Name);
        //    Assert.AreEqual(address.City, ccCity);
        //    string ccState = UIUtilityProvider.UIHelper.GetValue(CCStateLocator, LocateBy.Name);
        //    Assert.AreEqual(address.State ?? string.Empty, ccState);
        //    string ccZip = UIUtilityProvider.UIHelper.GetValue(CCZipLocator, LocateBy.Name);
        //    Assert.AreEqual(address.PostalCode ?? string.Empty, ccZip);
        //}

        /* To Be Implemented
        public void VerifyCheckInformation(C.Registration registration)
        {
            string code = UIUtilityProvider.UIHelper.GetText(CheckNumberLocator);
            Assert.AreEqual(registration.CheckNumber, code);
        }
        public void VerifyPOInformation(C.Registration registration)
        {
            string code = UIUtilityProvider.UIHelper.GetText(PONumberLocator);
            Assert.AreEqual(registration.PO, code);
        }
        public void VerifyCostCenterInformation(C.Registration registration)
        {
            string code = UIUtilityProvider.UIHelper.GetText(CostCenterCodeLocator);
            Assert.AreEqual(registration.Cost, code);
        }
        public void VerifyCustomInformation(C.Registration registration)
        {
            string code = UIUtilityProvider.UIHelper.GetText(CustomCodeLocator);
            Assert.AreEqual(registration.Custom, code);
        }
         */

        #endregion
    }
}
