namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public partial class RegisterManager : ManagerBase
    {
        #region Constants
        // Checkout Page
        public const string CheckoutSubTotal = "//tr[@class='summaryRow']/td[text()='Subtotal:']/following-sibling::td[@class='currency']";
        public const string CheckoutTotal = "//td[text()='Total:']/following-sibling::td[@class='currency']";
        public const string CheckoutShippingFee = "//tr[@class='summaryRow'][2]/td[2][@class='currency']";
        public const string CheckoutLodgingFeeSubTotal = "//th[text()='Lodging Fee']/../../following-sibling::tbody//td[text()='Subtotal:']/following-sibling::td[@class='currency']";
        public const string CheckoutLodgingBookingFee = "//tr[@class='summaryRow']/td[text()='Lodging Booking Fee:']/following-sibling::td";
        public const string CheckoutVerifyTemplate = "//table[@id='tblCheckoutRegistrantList']/tbody/tr[{0}]/td[{1}]";
        public const string CheckoutPaymentMethod = "ctl00_cph_ddlPaymentMethods";
        public const string CheckoutDiscountMessage = "//td[@class='discountMessage']";
        public const string CheckoutGroupDiscountSaving = "Your total includes a group discount savings of {0}.";
        public const string CheckoutServiceFee = "//td[text()='Service Fee:']/following-sibling::td";
        public const string TaxLocatorCheckout = "//td[text()='{0}:']/following-sibling::td[1]";
        public const string CheckoutRecurringSubtotal = "//legend[text()='Recurring Fees']/..//td[text()='Subtotal:']/following-sibling::td[@class='currency']";
        public const string CheckoutRecurringTax = "//legend[text()='Recurring Fees']/..//td[text()='{0}:']/following-sibling::td[@class='currency']";
        public const string CheckoutMembershipYearlyFees = "//table[@class='columnHeader dataTable multiColumn']//td[text()='Membership Fees for This Year:']/following-sibling::td[@class='currency']";
        public const string CheckoutMembershipDiscountMessage = "//legend[text()='Recurring Fees']/..//td[@class='discountMessage']";
        public const string CheckoutAgendaItemWaitlistLocator = "//td[contains(text(), '{0}')]/a";
        public const string CheckoutMembershipRenewalDate = "//td[@class='renewLabel']";
        public const string CheckoutMembershipAutoRenew = "ctl00_cph_chkRenewalMode";
        public const string CurentBalanceCheckoutLocator = "//tr[@class='summaryRow total']/td[text()='Current Balance:']/following-sibling::td[@class='currency']";
        public const string CheckoutTermsAndConditionsLocator = "ctl00_cph_ctlTerms_chkTerms";
        public const string CheckoutCCCountryLocator = "ctl00_cph_ddlCCCountry";

        public const string CCAmountLocatorNew = "ctl00_cph_txtPartialPayment";
        public const string CheckoutRequiredOptionLocatorBase = "//label[@for='ctl00_cph_{0}']/em/img";

        #endregion

        public PaymentManager PaymentMgr { set; get; }

        #region Checkout Page
        [Verify]
        public void VerifyCheckoutTotal(double totalToVerify, Utilities.MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string expectedValue = MoneyTool.FormatMoney(totalToVerify, currency);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutTotal, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "CheckoutTotal : {0}");
        }

        public void VerifyCheckoutSubTotal(double subTotalToVerify, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify, currency);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutSubTotal, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "CheckoutSubTotal : {0}");
        }

        public void VerifyCheckoutShippingFee(double subTotalToVerify, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify, currency);
            string actualValue;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutShippingFee, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "Checkout Shipping Fee : {0}");
        }

        public void VerifyCheckoutLodgingFeeTotal(double subTotalToVerify, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify, currency);
            string actualValue;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutLodgingFeeSubTotal, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "Lodging Subtotal : {0}");
        }

        public void VerifyCheckoutLodgingBookingFeeTotal(double subTotalToVerify, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify, currency);
            string actualValue;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutLodgingBookingFee, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "Lodging Booking Fee : {0}");
        }

        public void VerifyCheckoutSerivceFee(double subTotalToVerify, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify, currency);
            string actualValue;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutServiceFee, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "Service Fee : {0}");
        }

        public void VerifyCheckoutSaving(double savingToVerify, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string amount = MoneyTool.FormatMoney(savingToVerify, currency);
            string expectedValue = string.Format("By using a discount code, you have saved: {0}.", amount);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutDiscountMessage, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "CheckoutSaving : {0}");
        }

        public void VerifyCheckoutGroupAndDiscountSaving(double groupSavingToVerify, double codeSavingToVerify, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string groupAmount = MoneyTool.FormatMoney(groupSavingToVerify, currency);
            string codeAmount = MoneyTool.FormatMoney(codeSavingToVerify, currency);
            string expectedValue = string.Format("Your total includes a group discount savings of {0} and a discount code savings of {1}.", groupAmount, codeAmount);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutDiscountMessage, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "CheckoutSaving : {0}");
        }

        public void VerifyCheckoutTax(double taxAmountToVerify, string taxLabel, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string expectedAmount = MoneyTool.FormatMoney(taxAmountToVerify, currency);
            string actualAmount = UIUtilityProvider.UIHelper.GetText(string.Format(TaxLocatorCheckout, taxLabel), LocateBy.XPath);
            VerifyTool.VerifyValue(expectedAmount, actualAmount, taxLabel + " = {0}");
        }

        public void VerifyCheckoutMembershipRecurringSubtotal(double subTotalToVerify, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify, currency);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutRecurringSubtotal, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "Recurring Subtotal: {0}");
        }

        public void VerifyCheckoutMembershipReccuringTax(double taxAmountToVerify, string taxLabel, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string expectedAmount = MoneyTool.FormatMoney(taxAmountToVerify, currency);
            string actualAmount = UIUtilityProvider.UIHelper.GetText(string.Format(CheckoutRecurringTax, taxLabel), LocateBy.XPath);
            VerifyTool.VerifyValue(expectedAmount, actualAmount, taxLabel + " = {0}");
        }

        public void VerifyCheckoutMembershipRecurringTotal(double totalToVerify, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string expectedValue = MoneyTool.FormatMoney(totalToVerify, currency);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutMembershipYearlyFees, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "MembershipYearlyFees : {0}");
        }

        public void VerifyCheckoutMembershipRecurringSaving(double savingToVerify, MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            string amount = MoneyTool.FormatMoney(savingToVerify, currency);
            string expectedValue = string.Format("By using a discount code, you have saved: {0}.", amount);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutMembershipDiscountMessage, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "CheckoutSaving : {0}");
        }

        public void VerifyCheckoutMembershipRecurringDate(string date)
        {
            string expectedValue = string.Format("Renew your membership on {0}.", date);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CheckoutMembershipRenewalDate, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "Renewal Date: {0}");
        }

        public void VerifyCheckoutCurrentBalance(double expectedAmount)
        {
            string expectedValue = MoneyTool.FormatMoney(expectedAmount);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnCheckoutPage(), "Checkout");

            actualValue = UIUtilityProvider.UIHelper.GetText(CurentBalanceCheckoutLocator, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "CheckoutCurrentBalance : {0}");
        }

        public bool OnCheckoutPage()
        {
            bool onCheckout = false;

            if (UIUtilityProvider.UIHelper.UrlContainsPath("register/checkout.aspx"))
            {
                onCheckout = true;
            }

            return onCheckout;
        }

        public string CheckoutVerifyValue(CheckoutVerifyColumn column, int regNum)
        {
            string valueFound = string.Empty;
            string locator = string.Format(CheckoutVerifyTemplate, regNum, (int)column);

            if ((regNum >= 1) && (UIUtilityProvider.UIHelper.IsElementPresent(locator, LocateBy.XPath)))
            {
                valueFound = UIUtilityProvider.UIHelper.GetText(locator, LocateBy.XPath);
            }
            else
            {
                valueFound = null;
            }

            return valueFound;
        }

        public void MakeChangesToMerchandise()
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent("//strong[text() = 'Make Changes to Merchandise']", LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//strong[text() = 'Make Changes to Merchandise']", LocateBy.XPath);
        }

        [Step]
        public void ClickCheckoutActiveWaiver()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cph_chkAITerms", LocateBy.Id);
        }

        public void ClickTermsAndConditions()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(CheckoutTermsAndConditionsLocator, LocateBy.Id);
        }

        public void VerifyCheckoutGroupDiscountSavings(double savingToVerify)
        {
            string saving = MoneyTool.FormatMoney(savingToVerify);
            string expectedValue = string.Format(CheckoutGroupDiscountSaving, saving);
            string foundValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(this.OnCheckoutPage(), "Checkout");

            foundValue = UIUtilityProvider.UIHelper.GetText(CheckoutDiscountMessage, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, foundValue, "CheckoutGroupDiscountSavings : {0}");
        }

        public void VerifyHasDiscountMessage(bool hasDiscountMessage)
        {
            VerifyTool.VerifyValue(hasDiscountMessage, HasDiscountMessage(), "HasDiscountMessage : {0}");
        }

        public bool HasDiscountMessage()
        {
            return UIUtilityProvider.UIHelper.IsElementPresent(CheckoutDiscountMessage, LocateBy.XPath);
        }

        public void SetMembershipAutomaticRenewalCheckbox(bool automaticallyRenew)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(CheckoutMembershipAutoRenew, automaticallyRenew, LocateBy.Id);
        }

        #endregion

        #region Payments
        [Step]
        public void SelectPaymentMethod(PaymentMethod method)
        {
            string methodLabel = method.ToString();

            switch (method)
            {
                case PaymentMethod.CreditCard:
                    methodLabel = "Credit Card";
                    break;
                case PaymentMethod.PurchaseOrder:
                    methodLabel = "Purchase Order";
                    break;
                case PaymentMethod.PayAtTheEvent:
                    methodLabel = "Will Call (At The Event)";
                    break;
                case PaymentMethod.CostCenter:
                    methodLabel = "Cost Center";
                    break;
                case PaymentMethod.WireTransfer:
                    methodLabel = "Wire Transfer";
                    break;
            }

            SelectPaymentMethod(methodLabel);
        }

        public void SelectPaymentMethod(string customMethod)
        {
            if (UIUtilityProvider.UIHelper.IsElementDisplay(CheckoutPaymentMethod, LocateBy.Id))
            {
                UIUtilityProvider.UIHelper.SelectWithText(CheckoutPaymentMethod, customMethod, LocateBy.Id);
            }
        }

        [Verify]
        public void PayMoneyAndVerify(double total, PaymentMethod method)
        {
            PayMoneyAndVerify(total, total, method);
        }

        // Attention! On the checkout page of registration, the SubTotal will differ from Total if the event has service fee or tax rate!
        public void PayMoneyAndVerify(double total, double subTotal, PaymentMethod method)
        {
            this.CurrentTotal = MoneyTool.FormatMoney(total);

            this.VerifyCheckoutSubTotal(subTotal);
            this.VerifyCheckoutTotal(total);
            this.PayMoney(method);
        }

        private static Dictionary<int, string> ExpirationMonthEntry = new Dictionary<int, string>()
        {
            {1, "01 - Jan"},
            {2, "02 - Feb"},
            {3, "03 - Mar"},
            {4, "04 - Apr"},
            {5, "05 - May"},
            {6, "06 - Jun"},
            {7, "07 - Jul"},
            {8, "08 - Aug"},
            {9, "09 - Sep"},
            {10, "10 - Oct"},
            {11, "11 - Nov"},
            {12, "12 - Dec"}
        };

        public void PayCreditCard(CreditCardDetails ccDetails)
        {
            this.PaymentMgr.EnterCreditCardNumberInfo(
                ccDetails.Number.ToString(),
                ccDetails.CvvCode,
                ExpirationMonthEntry[ccDetails.ExpirationDate.Month],
                ccDetails.ExpirationDate.Year.ToString());

            this.PaymentMgr.EnterCreditCardNameCountryType(
                ccDetails.CardholderName,
                ccDetails.Country,
                ccDetails.Type.ToString());

            this.PaymentMgr.EnterCreditCardAddressInfo(
                ccDetails.Line1,
                ccDetails.Line2,
                ccDetails.City,
                ccDetails.State,
                ccDetails.PostalCode,
                PaymentManager.DefaultPaymentInfo.BillingPhone);
        }

        [Step]
        public void PayMoney(PaymentMethod method)
        {
            Assert.IsTrue(OnCheckoutPage());
            Assert.AreEqual(CurrentEmail, CheckoutVerifyValue(CheckoutVerifyColumn.Email, 1));

            switch (method)
            {
                case PaymentMethod.Check:
                    this.SelectPaymentMethod(PaymentMethod.Check);
                    break;

                case PaymentMethod.CreditCard:
                    this.SelectPaymentMethod(PaymentMethod.CreditCard);


                    CreditCardDetails ccDetails = new CreditCardDetails()
                    {
                        Number = PaymentManager.DefaultPaymentInfo.CCNumber,
                        ExpirationDate = DateTime.Parse("12/" + PaymentManager.DefaultPaymentInfo.ExpirationYear),//PaymentManager.DefaultPaymentInfo.ExpirationMonth is "12 - Dec"
                        CardholderName = PaymentManager.DefaultPaymentInfo.HolderName,
                        CvvCode = PaymentManager.DefaultPaymentInfo.CVV,
                        Country = PaymentManager.DefaultPaymentInfo.HolderCountry,
                        Line1 = PaymentManager.DefaultPaymentInfo.BillingAddressLineOne,
                        Line2 = null,
                        City = PaymentManager.DefaultPaymentInfo.BillingCity,
                        State = PaymentManager.DefaultPaymentInfo.BillingState,
                        PostalCode = PaymentManager.DefaultPaymentInfo.ZipCode,
                        Type = PaymentManager.CCType.Visa
                    };
                    PayCreditCard(ccDetails);
                    break;

                default:
                    break;
            }
        }

        public void VerifyPaymentWaitlistMessage()
        {
            string locator = "//div[@class='detailContentWrapper']/strong";

            VerifyTool.VerifyValue(
                "Your registration contains a waitlisted item(s) with a fee. If the waitlisted item becomes available, you will be charged for the waitlisted item.",
                UIUtilityProvider.UIHelper.GetText(locator, LocateBy.XPath), 
                "Waitlist Payment message: {0}");
        }

        public void VerifyAgendaItemWaitlistMessage(string agendaItemName)
        {
            string locator = string.Format(CheckoutAgendaItemWaitlistLocator, agendaItemName);
            VerifyTool.VerifyValue(UIUtilityProvider.UIHelper.GetText(locator, LocateBy.XPath), "Waitlisted", "Waitlist Message: {0}");
        }
        #endregion

        #region Credit Card Info
        #region From Backend
        [Step]
        public void ClickSaveAndClose()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//button[@name='ctl00$cph$btnFinalize']", LocateBy.XPath);
        }
        /// <summary>
        /// in Refund mode, the value can start with invalid characters like the dollar or euro sign
        /// </summary>
        public void FixAmount()
        {
            string actual = UIUtilityProvider.UIHelper.GetValue(CCAmountLocatorNew, LocateBy.Id);
            decimal ret;
            while (!string.IsNullOrEmpty(actual) && !decimal.TryParse(actual.Substring(0, 1), out ret))
                actual = actual.Substring(1);

            if (string.IsNullOrEmpty(actual))
                Assert.Fail("This is a bad amount.");

            if (UIUtilityProvider.UIHelper.GetValue(CCAmountLocatorNew, LocateBy.Id) != actual)
                UIUtilityProvider.UIHelper.Type(CCAmountLocatorNew, actual, LocateBy.Id);
        }
        #endregion

        public void ChangeCheckoutCCCountry(string country)
        {
            UIUtilityProvider.UIHelper.SelectWithText(CheckoutCCCountryLocator, country, LocateBy.Id);
        }

        public enum CheckoutField
        {
            [StringValue("Credit Card Type")]
            CCType,
            [StringValue("Credit Card Number")]
            CCNumber,
            [StringValue("Credit Card Security Code")]
            CCSecurityCode,
            [StringValue("Expiration Date")]
            CCExpirationDate,
            [StringValue("Cardholder Name")]
            CCCardholderName,
            [StringValue("Country")]
            CCCountry,
            [StringValue("Billing Address Line 1")]
            CCBillingAddress1,
            [StringValue("Billing Address Line 2")]
            CCBillingAddress2,
            [StringValue("Billing City")]
            CCBillingCity,
            [StringValue("Billing State / Province")]
            CCState,
            [StringValue("Billing Zip / Postal Code")]
            CCZip
        }
        private static readonly Dictionary<CheckoutField, string> checkoutFieldControl = new Dictionary<CheckoutField, string>()
            {
                {CheckoutField.CCType, "ddlCCType"},
                {CheckoutField.CCNumber, "txtCC"},
                {CheckoutField.CCSecurityCode, "txtCVV"},
                {CheckoutField.CCExpirationDate, "ddlCCExpYear"},
                {CheckoutField.CCCardholderName, "txtCCName"},
                {CheckoutField.CCCountry, "ddlCCCountry"},
                {CheckoutField.CCBillingAddress1, "txtCCAddress"},
                {CheckoutField.CCBillingAddress2, "txtCCAddress2"},
                {CheckoutField.CCBillingCity, "txtCCCity"},
                {CheckoutField.CCState, "txtCCState"},
                {CheckoutField.CCZip, "txtCCZip"}
            };

        /// <summary>
        /// These stars will always be rendered but may be marked hidden on client side.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="checkRequiredOption"></param>
        public void VerifyCheckoutFieldRequired(
            CheckoutField field,
            bool checkRequiredOption)
        {
            string loc = string.Format(CheckoutRequiredOptionLocatorBase, checkoutFieldControl[field]);
            string imgClass = UIUtilityProvider.UIHelper.GetAttribute(loc, "class", LocateBy.XPath);
            //based on UIUtilityProvider.UIHelper.VerifyElementContains(imgClass, "required");
            Assert.AreEqual(checkRequiredOption, !imgClass.Contains("hidden"));
        }
        #endregion
    }
}
