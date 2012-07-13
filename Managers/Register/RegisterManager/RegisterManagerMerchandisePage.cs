namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class RegisterManager : ManagerBase
    {
        #region Constants
        private const string MerchandiseRowByName = "//table[@class='merchandise dataTable multiColumn']//label[text()='{0}']/../../..";
        private const string MerchandiseMultipleChoiceInRow = "/td[1]/select";
        private const string MerchandiseAmountInRow = "/td[2]";
        private const string MerchandiseQuantityInRow = "/td[3]";
        private const string MerchandiseDiscountCodeInRow = "/td[4]";
        private const string MerchandiseSubTotalInRow = "/td[5]";
        
        private const string MerchandiseRowByName_Old = "//table[@class='regFormgridInnerTable']//label[contains(@for,'FeeQuantity')][contains(text(),'{0}')]/../..";
        private const string MerchandiseMultipleChoiceInRow_Old = "/td/select[contains(@name,'FeeDDValue')]";
        private const string MerchandiseAmountInRow_Old = "//td[3]";
        private const string MerchandiseQuantityInRow_Old = "/td/input[contains(@id,'FeeQuantity')]";
        private const string MerchandiseDiscountCodeInRow_Old = "/td/input[contains(@id,'FeePassword')]";
        private const string MerchandiseSubTotalInRow_Old = "/td[6]";
        #endregion

        #region Merchandise Page

        public bool OnMerchandisePage()
        {
            bool onMerchandise = false;

            if (UIUtilityProvider.UIHelper.UrlContainsPath("Registrations/Fees/Fees.asp") || UIUtilityProvider.UIHelper.UrlContainsPath("register/merchandise.aspx"))
            {
                onMerchandise = true;
            }

            return onMerchandise;
        }

        [Step]
        public int GetMerchandiseItemId(string itemName)
        {
            return Convert.ToInt32(UIUtilityProvider.UIHelper.GetAttribute(string.Format("//label[text()='{0}']", itemName), "for", LocateBy.XPath));
        }
        
        [Step]
        public void SelectMerchandise(int quantity)
        {
            UIUtilityProvider.UIHelper.VerifyOnPage(OnMerchandisePage(), "Merchandise");

            List<Fee> merchandise = Fetch_Merchandise(CurrentEventId);

            foreach (Fee m in merchandise)
            {
                if (UIUtilityProvider.UIHelper.IsElementPresent(m.Id.ToString(), LocateBy.Id)
                    || UIUtilityProvider.UIHelper.IsElementPresent(m.Id.ToString(), LocateBy.Name))
                {
                    DoSeleniumActionForMerchandise(m, quantity);
                }
            }
        }

        public void SelectMerchandiseQuantityByName(string merchName, int quantity)
        {
            string xPath = string.Format(MerchandiseRowByName, merchName) + MerchandiseQuantityInRow + "//input";

            UIUtilityProvider.UIHelper.Type(xPath, quantity.ToString(), LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("pageContent", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void EnterMerchandiseVariableAmountByName(string merchName, double amount)
        {
            string xPath = string.Format(MerchandiseRowByName, merchName) + MerchandiseAmountInRow + "//input";

            UIUtilityProvider.UIHelper.Type(xPath, amount.ToString(), LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("pageContent", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SelectMerchandiseMultipleChoiceByName(string merchName, string multipleItemName)
        {
            string xPath = string.Format(MerchandiseRowByName, merchName) + MerchandiseMultipleChoiceInRow;

            UIUtilityProvider.UIHelper.SelectWithText(xPath, multipleItemName, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void EnterMerchandiseDiscountCodeByName(string merchName, string discountCode)
        {
            string xPath = string.Format(MerchandiseRowByName, merchName) + MerchandiseDiscountCodeInRow + "//input";

            UIUtilityProvider.UIHelper.Type(xPath, discountCode, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("pageContent", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void EnterMerchandiseDiscountCodeById(int id, string discountCode)
        {
            string locator_Id = "dc" + id; 

            UIUtilityProvider.UIHelper.Type(locator_Id, discountCode, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("pageContent", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public bool HasMerchandiseDiscountCodeByName(string merchName)
        {
            bool hasDC = false;

            string xPath = string.Format(MerchandiseRowByName, merchName) + MerchandiseDiscountCodeInRow + "//input";

            hasDC = UIUtilityProvider.UIHelper.IsElementPresent(xPath, LocateBy.XPath);

            return hasDC;
        }

        public void VerifyMerchandisePageTotal(double expectedTotal)
        {
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            Utility.ThreadSleep(1);
            string amount = UIUtilityProvider.UIHelper.GetText("//*[@id='totalAmt']", LocateBy.XPath);
            double actualTotal = ConversionTools.CurrencyToDouble(amount);

            if (expectedTotal != actualTotal)
            {
                UIUtilityProvider.UIHelper.FailTest("The expected merchandise total of " + expectedTotal + " does not match the actual: " + actualTotal);
            }
        }

        public void VerifyMerchandisePageShippingFee(double expectedTotal)
        {
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            Utilities.Utility.ThreadSleep(1);
            string amount = UIUtilityProvider.UIHelper.GetText("divShippingTotal", LocateBy.Id);
            double actualTotal = ConversionTools.CurrencyToDouble(amount);

            if (expectedTotal != actualTotal)
            {
                UIUtilityProvider.UIHelper.FailTest("The expected shipping fee of " + expectedTotal + " does not match the actual: " + actualTotal);
            }
        }

        public string GetMerchandiseFeeSubtotalsByName(string merchName)
        {
            string xPath = string.Format(MerchandiseRowByName, merchName) + MerchandiseSubTotalInRow;

            return UIUtilityProvider.UIHelper.GetText(xPath, LocateBy.XPath);
        }

        public void VerifyMerchandiseItemPresent(string name, bool present)
        {
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format("//*[text()='{0}']", name), present, LocateBy.XPath);
        }
        #endregion
    }
}
