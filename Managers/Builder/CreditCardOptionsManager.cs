namespace RegOnline.RegressionTest.Managers.Builder
{
    using System.Collections.Generic;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class CreditCardOptionsManager : ManagerBase
    {
        #region Contants
        public const string FrameID = "dialog";
        public const string DefaultCreditCardStatementDescription = "ActiveEvents";
        private const string AcceptedCreditCardTableLocator = "tblCreditCards";
        private const string CreditCardTitleLocatorFormat = "//table[@id='" + AcceptedCreditCardTableLocator + "']/tbody/tr/td[1]/input[@value='{0}']";
        private const string CreditCardPublicCheckboxLocatorFormat = "{0}/../../td[2]/input";
        private const string CreditCardAdminCheckboxLocatorFormat = "{0}/../../td[3]/input";
        private const string CreditCardOnSiteCheckboxLocatorFormat = "{0}/../../td[4]/input";

        private const int VisaID = 1;
        private const int MastercardID = 2;
        private const int DiscoverID = 3;
        private const int DinersClubID = 4;
        private const int AmericanExpressID = 5;
        private readonly Dictionary<AcceptedCreditCard, int> CCIDs;
        #endregion

        #region Enum
        public enum PaymentGateway
        {
            [StringValue("RegOnline Gateway")]
            RegOnlineGateway,

            [StringValue("FirstDataGlobal Customer")]
            FirstDataGlobalCustomer
        }

        public enum AcceptedCreditCard
        {
            [StringValue("American Express")]
            AmericanExpress,

            [StringValue("Diners Club")]
            DinersClub,

            [StringValue("Discover")]
            Discover,

            [StringValue("Mastercard")]
            Mastercard,

            [StringValue("Visa")]
            Visa
        }
        #endregion

        public CreditCardOptionsManager()
        {
            this.CCIDs = new Dictionary<AcceptedCreditCard, int>();
            this.CCIDs.Add(AcceptedCreditCard.Visa, VisaID);
            this.CCIDs.Add(AcceptedCreditCard.Mastercard, MastercardID);
            this.CCIDs.Add(AcceptedCreditCard.Discover, DiscoverID);
            this.CCIDs.Add(AcceptedCreditCard.DinersClub, DinersClubID);
            this.CCIDs.Add(AcceptedCreditCard.AmericanExpress, AmericanExpressID);
        }

        private void SelectThisFrame()
        {
            try
            {
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName(FrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndClose()
        {
            SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SelectPaymentGateway(PaymentGateway paymentGateway)
        {
            UIUtilityProvider.UIHelper.SelectWithText("ctl00_cphDialog_ddlEventsCustomerMerchantID", StringEnum.GetStringValue(paymentGateway), LocateBy.Id);
        }

        public void SetCreditCardStatementDescription(string description)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_txtEventsDynamicDescriptor", description, LocateBy.Id);
        }

        public void SetAcceptedCreditCard(AcceptedCreditCard creditCard, bool? checkPublic, bool? checkAdmin, bool? checkOnSite)
        {
            string creditCardTitleLocator = string.Format(CreditCardTitleLocatorFormat, this.CCIDs[creditCard]);

            if (checkPublic.HasValue)
            {
                string creditCardPublicCheckboxLocator = string.Format(CreditCardPublicCheckboxLocatorFormat, creditCardTitleLocator);

                UIUtilityProvider.UIHelper.SetCheckbox(creditCardPublicCheckboxLocator, checkPublic.Value, LocateBy.XPath);
            }

            if (checkAdmin.HasValue)
            {
                string creditCardAdminCheckboxLocator = string.Format(CreditCardAdminCheckboxLocatorFormat, creditCardTitleLocator);

                UIUtilityProvider.UIHelper.SetCheckbox(creditCardAdminCheckboxLocator, checkAdmin.Value, LocateBy.XPath);
            }

            if (checkOnSite.HasValue)
            {
                string creditCardOnSiteCheckboxLocator = string.Format(CreditCardOnSiteCheckboxLocatorFormat, creditCardTitleLocator);

                UIUtilityProvider.UIHelper.SetCheckbox(creditCardOnSiteCheckboxLocator, checkOnSite.Value, LocateBy.XPath);
            }
        }

        public void SetDeleteCCNumber(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cphDialog_chkDeleteCreditCard", check, LocateBy.Id);
        }

        public void SetNotChargeCC(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cphDialog_chkDoNotCharge", check, LocateBy.Id);
        }
    }
}
