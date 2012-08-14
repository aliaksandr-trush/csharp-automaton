namespace RegOnline.RegressionTest.Managers.Builder
{
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public partial class FormDetailManager : ManagerBase
    {
        #region Constants
        private const string PartialPaymentsDIVLocator = "bs_48_ADV";
        private const string PartialPaymentsCheckboxLocator = "ctl00_cph_chkEventsAllowPartialPayment";
        private const string AdvancedPaymentOptionsLinkLocatorOnCheckoutPage = "//span[text()='Advanced Payment Options']";
        private const string AutoRenewalLocator = "ctl00_cph_ddlEventAutoRenew";
        private const string RenewalFrequencyLocator = "ctl00_cph_ddlEventRenewalRate";
        private const string FixedRenewalLocator = "ctl00_cph_rdoFixedRenewal";
        private const string PaymentFrequencyLocator = "ctl00_cph_ddlEventDefaultPayRate";
        private const string ProratePaymentsLocator = "ctl00_cph_chkProrateMembershipUpdates";
        #endregion

        #region enums
        public enum AutoRenewals
        {
            [StringValue("No Auto-Renewals")]
            NoAutoRenewals,

            [StringValue("Member Selection")]
            MemberSelection,

            [StringValue("Force Auto-Renewals")]
            ForceAutoRenewals
        }

        public enum Frequency
        {
            [StringValue("Monthly")]
            Monthly,

            [StringValue("Quarterly")]
            Quarterly,

            [StringValue("Semi-annually")]
            SemiAnnually,

            [StringValue("Annually")]
            Annually
        }
        #endregion

        #region Properties
        public TaxManager TaxMgr { get; set; }
        public PaymentMethodManager PaymentMethodMgr { get; set; }
        #endregion

        [Step]
        public void EnterEventCheckoutPage()
        {
            this.PaymentMethodMgr.SetPaymentMethodVisibility(PaymentMethodManager.PaymentMethod.CreditCard, true, null, null);
            this.PaymentMethodMgr.SetPaymentMethodVisibility(PaymentMethodManager.PaymentMethod.Check, true, null, null);

            this.PaymentMethodMgr.ClickCreditCardLink();

            this.PaymentMethodMgr.CreditCardOptionsMgr.SelectPaymentGateway(
                CreditCardOptionsManager.PaymentGateway.RegOnlineGateway);

            // Bruce - 2012-8-13
            // Sleep for 2 seconds is necessary:
            // In case you'll meet the error 'Element is no longer attached to the DOM'.
            // The default gateway has changed from RegOnline gateway to AMS,
            // so every time we change it from AMS to RegOnline gateway, some related elements will be refreshed.
            // See http://stackoverflow.com/questions/5709204/random-element-is-no-longer-attached-to-the-dom-staleelementreferenceexception as a reference,
            // those elements are 'removed and re-added'.
            Utility.ThreadSleep(2);

            ////this.PaymentMethodMgr.CreditCardOptionsMgr.SetCreditCardStatementDescription(
            ////    CreditCardOptionsManager.DefaultCreditCardStatementDescription);

            this.PaymentMethodMgr.CreditCardOptionsMgr.SetAcceptedCreditCard(
                CreditCardOptionsManager.AcceptedCreditCard.Discover, 
                true, 
                null, 
                null);

            this.PaymentMethodMgr.CreditCardOptionsMgr.SetAcceptedCreditCard(
                CreditCardOptionsManager.AcceptedCreditCard.DinersClub, 
                true, 
                null, 
                null);

            this.PaymentMethodMgr.CreditCardOptionsMgr.SetAcceptedCreditCard(
                CreditCardOptionsManager.AcceptedCreditCard.AmericanExpress, 
                true, 
                null, 
                null);

            this.PaymentMethodMgr.CreditCardOptionsMgr.SaveAndClose();
        }

        [Step]
        public void EnterEventCheckoutPageFull()
        {
            this.PaymentMethodMgr.SetPaymentMethodVisibility(PaymentMethodManager.PaymentMethod.CreditCard, true, null, null);
            this.PaymentMethodMgr.SetPaymentMethodVisibility(PaymentMethodManager.PaymentMethod.Check, true, null, null);
            this.PaymentMethodMgr.SetPaymentMethodVisibility(PaymentMethodManager.PaymentMethod.PurchaseOrder, true, null, null);
            this.PaymentMethodMgr.SetPaymentMethodVisibility(PaymentMethodManager.PaymentMethod.Cash, null, true, null);
            this.PaymentMethodMgr.SetPaymentMethodVisibility(PaymentMethodManager.PaymentMethod.PayAtTheEvent, true, null, null);
            this.PaymentMethodMgr.SetPaymentMethodVisibility(PaymentMethodManager.PaymentMethod.CostCenter, true, null, null);
            this.PaymentMethodMgr.SetPaymentMethodVisibility(PaymentMethodManager.PaymentMethod.WireTransfer, true, null, null);
            this.PaymentMethodMgr.SetPaymentMethodVisibility(PaymentMethodManager.PaymentMethod.PayPal, true, null, null);
            this.PaymentMethodMgr.SetPaymentMethodVisibility(PaymentMethodManager.PaymentMethod.Custom, true, null, null);

            this.PaymentMethodMgr.SetCustomPaymentMethodDescription(
                PaymentMethodManager.DefaultCustomPaymentMethodDescription);

            this.PaymentMethodMgr.SetCustomPaymentMethodIncludeInputField(true);

            this.PaymentMethodMgr.ClickCreditCardLink();

            this.PaymentMethodMgr.CreditCardOptionsMgr.SelectPaymentGateway(
                CreditCardOptionsManager.PaymentGateway.RegOnlineGateway);

            // Bruce - 2012-8-13
            // Sleep for 2 seconds is necessary:
            // In case you'll meet the error 'Element is no longer attached to the DOM'.
            // The default gateway has changed from RegOnline gateway to AMS,
            // so every time we change it from AMS to RegOnline gateway, some related elements will be refreshed.
            // See http://stackoverflow.com/questions/5709204/random-element-is-no-longer-attached-to-the-dom-staleelementreferenceexception as a reference,
            // those elements are 'removed and re-added'.
            Utility.ThreadSleep(2);

            ////this.PaymentMethodMgr.CreditCardOptionsMgr.SetCreditCardStatementDescription(
            ////    CreditCardOptionsManager.DefaultCreditCardStatementDescription);

            this.PaymentMethodMgr.CreditCardOptionsMgr.SetAcceptedCreditCard(
                CreditCardOptionsManager.AcceptedCreditCard.Discover, 
                true, 
                null, 
                null);

            this.PaymentMethodMgr.CreditCardOptionsMgr.SetAcceptedCreditCard(
                CreditCardOptionsManager.AcceptedCreditCard.DinersClub, 
                true, 
                null, 
                null);

            this.PaymentMethodMgr.CreditCardOptionsMgr.SetAcceptedCreditCard(
                CreditCardOptionsManager.AcceptedCreditCard.AmericanExpress, 
                true, 
                null, 
                null);

            this.PaymentMethodMgr.CreditCardOptionsMgr.SaveAndClose();
        }

        [Verify]
        public void VerifyEventCheckoutPage()
        {
            ////this.ReloadEvent();
            ClientDataContext db = new ClientDataContext();

            Event = (from e in db.Events where e.Id == EventId select e).Single();

            // verify payment options
            Assert.That(Event.Capture_Payment);
            Assert.IsFalse(Event.CapturePaymentOnZero ?? false);
            VerifyTool.VerifyValue("USD", Event.CurrencyCode, "Event currency code: {0}");
            VerifyTool.VerifyValue("Active Events AKA RegOnline", Event.InvoiceCompany, "Event invoice company: {0}");
            Assert.That(Event.GenerateInvoice ?? false);

            // verify cc options
            Assert.That(Event.Charge_Online);
            VerifyTool.VerifyValue(1218, Event.customerMerchantId, "CustomerMerchantId: {0}");
            VerifyTool.VerifyValue("Active Events AKA RegOnli", Event.DynamicDescriptor, "Event dynamic descriptor: {0}");
            Assert.IsFalse(Event.DontChargeCC ?? false);
            Assert.That(Event.storeCCNumbers);
            Assert.That(Event.EventCollectionField.cCVVCode ?? false);
            Assert.That(Event.EventCollectionField.rCVVCode ?? false);

            // verify payment methods
            for (int i = 1; i <= 14; i++)
            {
                // skip e-check
                if (i == 12)
                {
                    continue;
                }

                // "Cash" payment method not available to public
                bool publicReg = i != 8;

                Event_Payment_Method method = null;

                ClientDataContext clientDb = new ClientDataContext();
                method = (from p in clientDb.Event_Payment_Methods where p.Event == Event && p.Payment_Method_Id == i select p).Single();
                //E.Event method = Event.Event_Payment_Methods.Find(
                //    delegate(E.EventPaymentMethods methodInner)
                //    {
                //        return methodInner.PaymentMethodId == i &&
                //            (methodInner.PublicReg ?? false) == publicReg &&
                //            (methodInner.AdminReg ?? false);
                //    }
                //);

                Assert.That(method != null);

                // additional verification for "Custom" payment method
                if (i == 13)
                {
                    Assert.That(method.customDescription == "custom");
                    Assert.That(method.DisplayTextbox);
                }
            }
        }
        public void SetMembershipRenewalOptions(AutoRenewals renewal, Frequency frequency, bool fixedDate, bool prorate)
        {
            UIUtilityProvider.UIHelper.SelectWithText(AutoRenewalLocator, StringEnum.GetStringValue(renewal), LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectWithText(RenewalFrequencyLocator, StringEnum.GetStringValue(frequency), LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectWithText(PaymentFrequencyLocator, StringEnum.GetStringValue(frequency), LocateBy.Id);
            
            //there is a lot of logic around this and the renewal/payment frequency, more work is needed here
            //but for now this should suffice for our basic needs, I would recommend leaving fixedDate false
            if (fixedDate)
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(FixedRenewalLocator, LocateBy.Id);
            }

            UIUtilityProvider.UIHelper.SetCheckbox(ProratePaymentsLocator, prorate, LocateBy.Id);
        }
    }
}
