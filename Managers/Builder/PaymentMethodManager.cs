namespace RegOnline.RegressionTest.Managers.Builder
{
    using System.Collections.Generic;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class PaymentMethodManager : ManagerBase
    {
        #region Constants
        private const string PaymentMethodTableLocator = "tblCheckoutPaymentMethods";
        private const string PaymentMethodTitleLocatorFormat = "//table[@id='" + PaymentMethodTableLocator + "']/tbody/tr/td[1]/input[@value='{0}']";
        private readonly string CCPaymentMethodLinkLocator;
        private const string PaymentMethodPublicCheckboxLocatorFormat = "{0}/../../td[2]/input";
        private const string CCPaymentMethodPublicCheckboxLocatorFormat = "{0}/../../td[2]/input";
        private const string PaymentMethodAdminCheckboxLocatorFormat = "{0}/../../td[3]/input";
        private const string CCPaymentMethodAdminCheckboxLocatorFormat = "{0}/../../td[3]/input";
        private const string PaymentMethodOnSiteCheckboxLocatorFormat = "{0}/../../td[4]/input";
        private const string CCPaymentMethodOnSiteCheckboxLocatorFormat = "{0}/../../td[4]/input";
        private const string PaymentMethodActionsLinkLocatorFormat = "{0}/../../td[5]/a";
        private const string EditCCPaymentMethodLinkLocatorFormat = "{0}/../../td[5]/a[1]";
        private const string DeleteCCPaymentMethodLinkLocatorFormat = "{0}/../../td[5]/a[2]";
        private const string PaypalEmailAddressLocator = "{0}/../../td[1]/span/input";
        private const string PaypalEmailAddress = "apiuser@regonline.com";

        public const string AddPaymentMethodFrameID = "dialog";
        public const string TestPayPalFrameID = "dialog";
        public const string DefaultCustomPaymentMethodDescription = "custom";

        private const int CheckPaymentMethodID = 6;
        private const int PurchaseOrderPaymentMethodID = 7;
        private const int CashPaymentMethodID = 8;
        private const int PayAtTheEventPaymentMethodID = 9;
        private const int CostCenterPaymentMethodID = 10;
        private const int WireTransferPaymentMethodID = 11;
        private const int CustomPaymentMethodID = 13;
        private const int PayPalPaymentMethodID = 14;
        private readonly Dictionary<PaymentMethod, int> PaymentMethodIDs;
        private readonly Dictionary<PaymentMethod, string> paymentMethodLabels;
        #endregion

        #region Properties
        public CreditCardOptionsManager CreditCardOptionsMgr { get; set; }
        #endregion

        private struct PaymentMethodLabel
        {
            public const string CreditCard = "Credit Cards";
            public const string Check = "Check";
            public const string PurchaseOrder = "Purchase Order";
            public const string Cash = "Cash (On-Site & Admin Only)";
            public const string PayAtTheEvent = "Pay at the Event";
            public const string CostCenter = "Cost Center";
            public const string WireTransfer = "Wire Transfer";
            public const string Custom = "Custom";
            public const string PayPal = "PayPal Express Checkout";
        }

        public enum Visibilities
        {
            Public,
            Admin,
            Onsite
        }

        #region Constructor
        public PaymentMethodManager()
        {
            this.paymentMethodLabels = new Dictionary<PaymentMethod, string>();
            this.paymentMethodLabels.Add(PaymentMethod.CreditCard, PaymentMethodLabel.CreditCard);
            this.paymentMethodLabels.Add(PaymentMethod.Check, PaymentMethodLabel.Check);
            this.paymentMethodLabels.Add(PaymentMethod.PurchaseOrder, PaymentMethodLabel.PurchaseOrder);
            this.paymentMethodLabels.Add(PaymentMethod.Cash, PaymentMethodLabel.Cash);
            this.paymentMethodLabels.Add(PaymentMethod.PayAtTheEvent, PaymentMethodLabel.PayAtTheEvent);
            this.paymentMethodLabels.Add(PaymentMethod.CostCenter, PaymentMethodLabel.CostCenter);
            this.paymentMethodLabels.Add(PaymentMethod.WireTransfer, PaymentMethodLabel.WireTransfer);
            this.paymentMethodLabels.Add(PaymentMethod.Custom, PaymentMethodLabel.Custom);
            this.paymentMethodLabels.Add(PaymentMethod.PayPal, PaymentMethodLabel.PayPal);

            this.CCPaymentMethodLinkLocator = string.Format(
                "//table[@id='{0}']/tbody/tr/td/a[text()='{1}']",
                PaymentMethodTableLocator,
                this.paymentMethodLabels[PaymentMethod.CreditCard]);

            this.PaymentMethodIDs = new Dictionary<PaymentMethod, int>();
            this.PaymentMethodIDs.Add(PaymentMethod.Check, CheckPaymentMethodID);
            this.PaymentMethodIDs.Add(PaymentMethod.PurchaseOrder, PurchaseOrderPaymentMethodID);
            this.PaymentMethodIDs.Add(PaymentMethod.Cash, CashPaymentMethodID);
            this.PaymentMethodIDs.Add(PaymentMethod.PayAtTheEvent, PayAtTheEventPaymentMethodID);
            this.PaymentMethodIDs.Add(PaymentMethod.CostCenter, CostCenterPaymentMethodID);
            this.PaymentMethodIDs.Add(PaymentMethod.WireTransfer, WireTransferPaymentMethodID);
            this.PaymentMethodIDs.Add(PaymentMethod.Custom, CustomPaymentMethodID);
            this.PaymentMethodIDs.Add(PaymentMethod.PayPal, PayPalPaymentMethodID);

            this.CreditCardOptionsMgr = new CreditCardOptionsManager();
        }
        #endregion

        public void SetPaymentMethodVisibility(PaymentMethod paymentMethod, bool? checkPublic, bool? checkAdmin, bool? checkOnSite)
        {
            switch (paymentMethod)
            {
                case PaymentMethod.CreditCard:
                    if (checkPublic.HasValue)
                    {
                        UIUtil.DefaultProvider.SetCheckbox(
                            string.Format(CCPaymentMethodPublicCheckboxLocatorFormat, this.CCPaymentMethodLinkLocator),
                            checkPublic.Value, 
                            LocateBy.XPath);
                    }

                    if (checkAdmin.HasValue)
                    {
                        UIUtil.DefaultProvider.SetCheckbox(
                            string.Format(CCPaymentMethodAdminCheckboxLocatorFormat, this.CCPaymentMethodLinkLocator),
                            checkAdmin.Value, 
                            LocateBy.XPath);
                    }

                    if (checkOnSite.HasValue)
                    {
                        UIUtil.DefaultProvider.SetCheckbox(
                            string.Format(CCPaymentMethodOnSiteCheckboxLocatorFormat, this.CCPaymentMethodLinkLocator),
                            checkOnSite.Value, 
                            LocateBy.XPath);
                    }
                    break;

                case PaymentMethod.Check:
                case PaymentMethod.PurchaseOrder:
                case PaymentMethod.Cash:
                case PaymentMethod.PayAtTheEvent:
                case PaymentMethod.CostCenter:
                case PaymentMethod.WireTransfer:
                case PaymentMethod.Custom:
                case PaymentMethod.PayPal:
                    string paymentMethodTitleLocator = string.Format(
                            PaymentMethodTitleLocatorFormat,
                            this.PaymentMethodIDs[paymentMethod]);

                    if (!UIUtil.DefaultProvider.IsElementPresent(paymentMethodTitleLocator, LocateBy.XPath))
                    {
                        this.AddPaymentMethod(paymentMethod);
                    }

                    if (checkPublic.HasValue && paymentMethod != PaymentMethod.Cash)
                    {
                        string paymentMethodPublicCheckboxLocator = string.Format(
                            PaymentMethodPublicCheckboxLocatorFormat,
                            paymentMethodTitleLocator);

                        UIUtil.DefaultProvider.SetCheckbox(paymentMethodPublicCheckboxLocator, checkPublic.Value, LocateBy.XPath);
                    }

                    if (checkAdmin.HasValue)
                    {
                        string paymentMethodAdminCheckboxLocator = string.Format(
                            PaymentMethodAdminCheckboxLocatorFormat,
                            paymentMethodTitleLocator);

                        UIUtil.DefaultProvider.SetCheckbox(paymentMethodAdminCheckboxLocator, checkAdmin.Value, LocateBy.XPath);
                    }

                    if (checkOnSite.HasValue)
                    {
                        string paymentMethodOnSiteCheckboxLocator = string.Format(
                            PaymentMethodOnSiteCheckboxLocatorFormat,
                            paymentMethodTitleLocator);

                        UIUtil.DefaultProvider.SetCheckbox(paymentMethodOnSiteCheckboxLocator, checkOnSite.Value, LocateBy.XPath);
                    }

                    if (paymentMethod == PaymentMethod.PayPal)
                    {
                        string paypalEmailExpressAddressLocator = string.Format(
                            PaypalEmailAddressLocator, 
                            paymentMethodTitleLocator);

                        UIUtil.DefaultProvider.Type(paypalEmailExpressAddressLocator, PaypalEmailAddress, LocateBy.XPath);
                    }
                    break;

                default:
                    break;
            }
        }

        public void ClickCreditCardLink()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(CCPaymentMethodLinkLocator, LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectPopUpFrameByName(CreditCardOptionsManager.FrameID);
        }

        public void ClickEditCreditCardPaymentMethod()
        {
            string actionLinkLocator = string.Format(EditCCPaymentMethodLinkLocatorFormat, this.CCPaymentMethodLinkLocator);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(actionLinkLocator, LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectPopUpFrameByName(CreditCardOptionsManager.FrameID);
        }

        public void AddPaymentMethod(PaymentMethod paymentMethod)
        {
            this.ClickAddPaymentMethod();
            this.SelectNewPaymentMethod(paymentMethod);
            this.SaveAndCloseAddPaymentMethod();
        }

        public void ClickAddPaymentMethod()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//a[text()='Add Payment Method']", LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectPopUpFrameByName(AddPaymentMethodFrameID);
        }

        private void SelectAddPaymentMethodFrame()
        {
            try
            {
                UIUtil.DefaultProvider.SelectPopUpFrameByName(AddPaymentMethodFrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndCloseAddPaymentMethod()
        {
            SelectAddPaymentMethodFrame();
            UIUtil.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void CancelAddPaymentMethod()
        {
            SelectAddPaymentMethodFrame();
            UIUtil.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SelectNewPaymentMethod(PaymentMethod paymentMethod)
        {
            UIUtil.DefaultProvider.SelectWithText("ctl00_cphDialog_ddlPaymentMethod", this.paymentMethodLabels[paymentMethod], LocateBy.Id);
        }

        public void DeletePaymentMethod(PaymentMethod paymentMethod, bool delete)
        {
            string deleteLinkLocator = string.Empty;

            switch (paymentMethod)
            {
                case PaymentMethod.CreditCard:
                    deleteLinkLocator = string.Format(
                        DeleteCCPaymentMethodLinkLocatorFormat, 
                        this.CCPaymentMethodLinkLocator);

                    break;

                case PaymentMethod.Check:
                case PaymentMethod.PurchaseOrder:
                case PaymentMethod.Cash:
                case PaymentMethod.PayAtTheEvent:
                case PaymentMethod.CostCenter:
                case PaymentMethod.WireTransfer:
                case PaymentMethod.Custom:
                case PaymentMethod.PayPal:
                    string paymentMethodTitleLocator = string.Format(
                            PaymentMethodTitleLocatorFormat,
                            this.paymentMethodLabels[paymentMethod]);

                    deleteLinkLocator = string.Format(PaymentMethodActionsLinkLocatorFormat, paymentMethodTitleLocator);
                    break;

                default:
                    break;
            }

            if (!delete)
            {
                UIUtil.DefaultProvider.ChooseCancelOnNextConfirmation();
            }

            UIUtil.DefaultProvider.WaitForDisplayAndClick(deleteLinkLocator, LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.GetConfirmation();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetCustomPaymentMethodDescription(string description)
        {
            UIUtil.DefaultProvider.Type("ctl00_cph_rptPaymentMethods_ctl06_txtDescription", description, LocateBy.Id);
        }

        public void SetCustomPaymentMethodIncludeInputField(bool check)
        {
            UIUtil.DefaultProvider.SetCheckbox("ctl00_cph_rptPaymentMethods_ctl06_chkIncludeInput", check, LocateBy.Id);
        }

        public void SetPayPalPaymentMethodDescription(string description)
        {
            UIUtil.DefaultProvider.Type("ctl00_cph_rptPaymentMethods_ctl07_txtDescription", description, LocateBy.Id);
        }

        public void ClickTestPayPalLink()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cph_rptPaymentMethods_ctl07_hlTestLink", LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectPopUpFrameByName(TestPayPalFrameID);
        }
    }
}
