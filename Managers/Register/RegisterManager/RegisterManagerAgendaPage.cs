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
        private const string AgendaItemsTableLocator = "//table[@class = 'FormContent']";

        public void TypeAgendaItem(int agendaID, string text)
        {
            UIUtilityProvider.UIHelper.Type("CF" + agendaID.ToString(), text, LocateBy.Id);
        }

        public void TypeAgendaItem(string name, string text)
        {
            string id = UIUtilityProvider.UIHelper.GetAttribute(string.Format("//label[text()='{0}']", name), "@for", LocateBy.XPath);
            UIUtilityProvider.UIHelper.Type(id, text, LocateBy.Id);
        }

        public bool OnAgendaPage()
        {
            return UIUtilityProvider.UIHelper.UrlContainsPath("register/agenda.aspx");
        }

        public void VerifyOnAgendaPage()
        {
            if (!this.OnAgendaPage())
            {
                UIUtilityProvider.UIHelper.FailTest("Not on Agenda page!");
            }
        }

        #region New methods for new register agenda page
        
        public string GetAgendaItemID(string name)
        {
            return UIUtilityProvider.UIHelper.GetAttribute(string.Format("//label[text() = '{0}']", name), "for", LocateBy.XPath);
        }

        /// <summary>
        /// Radio buttons are different. The id appears in the name fields of each radio option.
        /// Fortunately it's also in the data-id of the wrapper.
        /// </summary>
        /// <param name="name">The displayed name of the field from which you choose</param>
        /// <returns>agenda item ID as an int</returns>
        public int GetAgendaItemIDFromRadio(string name)
        {
            string id = UIUtilityProvider.UIHelper.GetAttribute(string.Format("//p[text()='{0}']/../..", name), "data-id", LocateBy.XPath);
            return int.Parse(id);
        }

        [Step]
        public void SelectAgendaItems()
        {
            List<Custom_Field> agendaItems = this.DataTool.GetAgendaItems(this.CurrentEventId);

            foreach (Custom_Field agendaItem in agendaItems)
            {
                if (UIUtilityProvider.UIHelper.IsElementPresent(agendaItem.Id.ToString(), LocateBy.Id)
                    || UIUtilityProvider.UIHelper.IsElementPresent(agendaItem.Id.ToString(), LocateBy.Name))
                {
                    this.DoSeleniumActionForCustomField(agendaItem);
                }
            }
        }

        public void SelectAgendaItem(int agendaID)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(agendaID.ToString(), LocateBy.Id);
            this.WaitForConditionalLogic();
        }

        public void SelectAgendaItem(int agendaId, bool withConditionalLogic)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(agendaId.ToString(), LocateBy.Id);

            if (withConditionalLogic)
            {
                this.WaitForConditionalLogic();
            }
        }

        [Step]
        public void SelectAgendaItem(string agendaName)
        {
            string id = this.GetAgendaItemID(agendaName);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(id, LocateBy.Id);
            this.WaitForConditionalLogic();
        }

        [Step]
        public void SelectAgendaItem(string agendaName, bool withConditionalLogic)
        {
            string id = this.GetAgendaItemID(agendaName);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(id, LocateBy.Id);

            if (withConditionalLogic)
            {
                this.WaitForConditionalLogic();
            }
        }

        public void EnterAgendaItemDiscountCode(int agendaCFid, string discountCode)
        {
            UIUtilityProvider.UIHelper.Type("dc" + agendaCFid.ToString(), discountCode, LocateBy.Id);
        }

        public void EnterAgendaItemDiscountCode(string itemName, string discountCode)
        {
            string locator = string.Format("//*[text()='{0}']/..//div[@class='codeInput']/input[@type='text']", itemName);
            UIUtilityProvider.UIHelper.Type(locator, discountCode, LocateBy.XPath);
        }

        public void VerifyAgendaPageTotalAmount(double totalAmount, Utilities.MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string expectedAmount = MoneyTool.FormatMoney(totalAmount, currency);
            string actualAmount = UIUtilityProvider.UIHelper.GetText("totalAmt", LocateBy.Id);
            VerifyTool.VerifyValue(expectedAmount, actualAmount, "Agenda total fee : {0}");
        }

        public void VerifyMerchandiseFeePageTotalAmount(double totalAmountOneTime, double totalAmountRecurring)
        {
            string expectedOneTimeAmount = MoneyTool.FormatMoney(totalAmountOneTime);
            string actualOneTimeAmount = UIUtilityProvider.UIHelper.GetText("//div[@class='sectionTotal']//div[1]/span", LocateBy.XPath);
            string expectedRecurringAmount = MoneyTool.FormatMoney(totalAmountRecurring);
            string actualRecurringAmount = UIUtilityProvider.UIHelper.GetText("//div[@class='sectionTotal']//div[2]/span", LocateBy.XPath);
            VerifyTool.VerifyValue(expectedOneTimeAmount, actualOneTimeAmount, "One Time total fee : {0}");
            VerifyTool.VerifyValue(expectedRecurringAmount, actualRecurringAmount, "Recurring total fee : {0}");
        }

        public void ClickRecalculateTotal()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//*[text()='Recalculate Total']", LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public double GetAgendaItemPrice(int agendaID)
        {
            return Convert.ToDouble(UIUtilityProvider.UIHelper.GetText(string.Format("//label[@for='{0}']/..//span[text()='Price:']/..", agendaID.ToString()), LocateBy.XPath).Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries)[1]);
        }

        public int CountAgendaItems()
        {
            return UIUtilityProvider.UIHelper.GetXPathCountByXPath("//fieldset/ol/li");
        }

        public void VerifyCountOfAgendaItems(int expectQuantity)
        {
            VerifyTool.VerifyValue(expectQuantity, this.CountAgendaItems(), "Count of Agenda Items: {0}");
        }

        public void VerifyAgendaItem(string name, bool exists)
        {
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format("//*[text()='{0}']", name), exists, LocateBy.XPath);
        }
        #endregion

        #region Methods for admin reg
        public void SelectPricingSchedule(int agendaItemID, PricingSchedule pricingSchedule)
        {
            string locatorFormat = "//label[@for='" + agendaItemID.ToString() + "']/..//span[text()='{0}']/../../input";
            string locator = string.Format(locatorFormat, StringEnum.GetStringValue(pricingSchedule));
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.XPath);
        }

        [Step]
        public void SelectPricingSchedule(string agendaItemName, PricingSchedule pricingSchedule)
        {
            string locatorFormat = "//label[text()='" + agendaItemName + "']/..//span[text()='{0}']/../../input";
            string locator = string.Format(locatorFormat, StringEnum.GetStringValue(pricingSchedule));
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.XPath);
        }

        public double GetPriceForPricingSchedule(int agendaItemID, PricingSchedule pricingSchedule)
        {
            string locatorFormat = "//label[@for='" + agendaItemID.ToString() + "']/..//span[text()='{0}']/parent::label";
            string locator = string.Format(locatorFormat, StringEnum.GetStringValue(pricingSchedule));
            string price = UIUtilityProvider.UIHelper.GetText(locator, LocateBy.XPath);
            price = price.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[0];
            price = price.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries)[0];

            return Convert.ToDouble(price);
        }

        public double GetPriceForPricingSchedule(string agendaItemName, PricingSchedule pricingSchedule)
        {
            string locatorFormat = "//label[text()='" + agendaItemName + "']/..//span[text()='{0}']/parent::label";
            string locator = string.Format(locatorFormat, StringEnum.GetStringValue(pricingSchedule));
            string price = UIUtilityProvider.UIHelper.GetText(locator, LocateBy.XPath);
            price = price.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[0];
            price = price.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries)[0];

            return Convert.ToDouble(price);
        }
        #endregion
    }
}
