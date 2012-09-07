namespace RegOnline.RegressionTest.Managers.Manager.Account.PaymentGateway
{
    using System.Text;
    using NUnit.Framework;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public class PaymentGatewayManager : ManagerBase
    {
        #region Classes
        public static class Error
        {
            public const string MissingRequiredFields = "An error has occured saving this gateway merchant configuration. Please correct the fields marked '*' and click 'Save' when completed.";
        }

        public class CreditCardOptions
        {
            public bool Visa = false;
            public bool Mastercard = false;
            public bool Amex = false;
            public bool Discover = false;
            public bool Diners = false;
            public bool Switch = false;

            public override string ToString()
            {
                StringBuilder s = new StringBuilder();
                s.Append(StringTool.Left(Visa.ToString(), 1));
                s.Append(StringTool.Left(Mastercard.ToString(), 1));
                s.Append(StringTool.Left(Amex.ToString(), 1));
                s.Append(StringTool.Left(Discover.ToString(), 1));
                s.Append(StringTool.Left(Diners.ToString(), 1));
                s.Append(StringTool.Left(Switch.ToString(), 1));
                return s.ToString();
            }
        }

        public class GridActionOptions
        {
            public bool VerifyCC = false;
            public bool Delete = false;
            public bool AdminDelete = false;

            public override string ToString()
            {
                StringBuilder s = new StringBuilder();
                s.Append(StringTool.Left(VerifyCC.ToString(), 1));
                s.Append(StringTool.Left(Delete.ToString(), 1));
                s.Append(StringTool.Left(AdminDelete.ToString(), 1));
                return s.ToString();
            }
        }
        #endregion

        #region enums

        /// <summary>
        /// Columns in the merchant gateway grid view.
        /// </summary>
        public enum GridColumns
        {
            Default = 1,
            Description = 2,
            Gateway = 3,
            CardTypes = 4,
            Currency = 5,
            ModifiedBy = 6,
            LastModified = 7,
            Actions = 8
        }

        /// <summary>
        /// Actions in the action column of the merchant gateway grid.
        /// </summary>
        public enum GridActions
        {
            VerifyCC,
            Delete,
            AdminDelete
        }

        /// <summary>
        /// Setup fields in the edit gateway window (not cc).
        /// </summary>
        public enum SetupFields
        {
            Description,
            Gateway,
            StatementName,
            Prefixes,
            MerchantID,
            GatewayLogin,
            GatewayPassword,
            GatewayUrl,
            Subprocessor,
            P12GatewayPassword,
            CertUpload,
            CertP12Upload,
            BaseCurrencyCode,
            DefaultMerchantAccount,
            CsiMerchant
        }

        public enum Gateway
        {
            [StringValue("PayFlowPro")]
            PayFlowPro,

            [StringValue("Authorize.Net")]
            AuthorizeDotNet,

            [StringValue("Authorize.Net")]
            Touchnet,

            [StringValue("Moneris USA")]
            MonerisUSA,

            [StringValue("First Data Global Gateway (WS)")]
            FirstDataGlobalGatewayWS
        }

        /// <summary>
        /// Credit card types.
        /// </summary>
        public enum CCTypes
        {
            Visa,
            Mastercard,
            Amex,
            Discover,
            Diners,
            Switch
        }
        #endregion

        #region Locators
        private string ComposeGridLocator(GridColumns gridCol, string mgName)
        {
            string locator = string.Format("//td[a='{0}']/../td[{1}]", mgName, (int)gridCol);

            return locator;
        }

        private string ComposeGridLocator(GridActions gridAction, string mgName)
        {
            string locator = string.Empty;
            string baseLocator = ComposeGridLocator(GridColumns.Actions, mgName) + "/div/a[contains(@id,'{0}')]";
            string forAction = string.Empty;

            switch (gridAction)
            {
                case GridActions.VerifyCC:
                    forAction = "lnkTestCC";
                    break;
                case GridActions.Delete:
                    forAction = "lnkDelete";
                    break;
                case GridActions.AdminDelete:
                    forAction = "lnkAdminDelete";
                    break;
            }

            locator = string.Format(baseLocator, forAction);
            return locator;
        }

        private string ComposeGridLocator(CCTypes ccType, string mgName)
        {
            string forField = ccType.ToString().ToLower();
            string baseLocator = ComposeGridLocator(GridColumns.CardTypes, mgName) + "/img[contains(@id,'_{0}')]";
            string locator = string.Format(baseLocator, forField);

            return locator;
        }

        #endregion

        public PaymentGatewayManager()
		{
            ccVerifMgr = new CCVerificationManager();
            gatewaySetupMgr = new GatewaySetupManager();
        }

        private CCVerificationManager ccVerifMgr;
        public CCVerificationManager CCVerifMgr
        {
            get { return ccVerifMgr; }
            set { ccVerifMgr = value; }
        }

        private GatewaySetupManager gatewaySetupMgr;
        public GatewaySetupManager GatewaySetupMgr
        {
            get { return gatewaySetupMgr; }
            set { gatewaySetupMgr = value; }
        }

        #region Buttons
        
        /// <summary>
        /// Add Merchant Gateway- opens edit gateway window with new gateway
        /// </summary>
        [Step]
        public void ClickAddPaymentGateway()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("Add Payment Gateway", LocateBy.LinkText);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("plain");
        }

        /// <summary>
        /// Click OK on a popup window (Edit or Delete confirmation)
        /// </summary>
        [Step]
        public void ClickOK()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//span[text()='OK']/parent::a", LocateBy.XPath);
            Utility.ThreadSleep(5);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        /// <summary>
        /// Click Cancel on a popup window (Edit or Delete confirmation)
        /// </summary>
        public void ClickCancel()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//span[text()='Cancel']/parent::a", LocateBy.XPath);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }
        #endregion

        #region Merchant Gateways Grid
        /// <summary>
        /// Open Merchant Gateway from Merchant tab in Account, by the name.
        /// Assumes merchant gateway of that name is present.
        /// If there are duplicate names, opens the first.
        /// </summary>
        /// <param name="name"></param>
        [Step]
        public void OpenPaymentGateway(string name)
        {
            string linkText = name;

            if (!WebDriverUtility.DefaultProvider.IsElementPresent(linkText, LocateBy.LinkText))
            {
                Assert.AreEqual(linkText, "Not in list!");
            }

            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(linkText, LocateBy.LinkText);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("plain");
        }

        /// <summary>
        /// Returns true if gateway of that name exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasPaymentGateway(string name)
        {
            return WebDriverUtility.DefaultProvider.IsElementPresent(this.LocatorComposer_XPath_GatewayLinkText(name), LocateBy.XPath);
        }

        /// <summary>
        /// Delete all gateways of the same name in list
        /// </summary>
        /// <param name="gatewayName">The link text in description column of the gateway list</param>
        public void DeletePaymentGateway(string gatewayName)
        {
            ManagerSiteManager managerSiteManager = new ManagerSiteManager();
            string eventSessionId = managerSiteManager.GetEventSessionId();
            managerSiteManager.SetSuperadmin(eventSessionId, true);
            WebDriverUtility.DefaultProvider.RefreshPage();

            while (WebDriverUtility.DefaultProvider.IsElementDisplay(this.LocatorComposer_XPath_GatewayLinkText(gatewayName), LocateBy.XPath))
            {
                managerSiteManager.AccountMgr.GatewayMgr.ClickGridAction(PaymentGatewayManager.GridActions.AdminDelete, gatewayName);
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("plain");
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//span[text()='OK']/parent::a", LocateBy.XPath);
                Utility.ThreadSleep(2);
                WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                WebDriverUtility.DefaultProvider.SwitchToMainContent();
                WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            }

            managerSiteManager.SetSuperadmin(eventSessionId, false);
            WebDriverUtility.DefaultProvider.RefreshPage();
        }

        private string LocatorComposer_XPath_GatewayLinkText(string gatewayName)
        {
            return string.Format("//table[@id='ctl00_ctl00_cphDialog_cpMgrMain_grdMAs_ctl00']//a[text()='{0}']", gatewayName);
        }

        /// <summary>
        /// Clicks one of the possible grid actions, assumes that action is present.
        /// </summary>
        /// <param name="gridAction"></param>
        /// <param name="name"></param>
        [Step]
        public void ClickGridAction(GridActions gridAction, string name)
        {
            string xPath = ComposeGridLocator(gridAction, name);
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(xPath, LocateBy.XPath);
        }

        /// <summary>
        /// Gets the value displayed in one of the grid columns.
        /// Default returns boolean as text, CardTypes and Actions return option lists.
        /// </summary>
        /// <param name="gridCol"></param>
        /// <param name="name">Name of merchant gateway</param>
        /// <returns></returns>
        public string GetGridValue(GridColumns gridCol, string name)
        {
            string value = string.Empty;
            string xPath = ComposeGridLocator(gridCol, name);

            switch (gridCol)
            {
                case GridColumns.Description:
                case GridColumns.Gateway:
                case GridColumns.Currency:
                case GridColumns.ModifiedBy:
                case GridColumns.LastModified:
                    value = WebDriverUtility.DefaultProvider.GetText(xPath, LocateBy.XPath);
                    break;
                case GridColumns.Default:
                    value = WebDriverUtility.DefaultProvider.IsElementPresent(xPath + "/img[@id='imgDefaultGateway']", LocateBy.XPath).ToString();
                    break;
                case GridColumns.CardTypes:
                    CreditCardOptions ccOptions = new CreditCardOptions();
                    ccOptions.Visa = IsVerifiedCC(CCTypes.Visa, name);
                    ccOptions.Mastercard = IsVerifiedCC(CCTypes.Mastercard, name);
                    ccOptions.Amex = IsVerifiedCC(CCTypes.Amex, name);
                    ccOptions.Discover = IsVerifiedCC(CCTypes.Discover, name);
                    ccOptions.Diners = IsVerifiedCC(CCTypes.Diners, name);
                    ccOptions.Switch = IsVerifiedCC(CCTypes.Switch, name);
                    value = ccOptions.ToString();
                    break;
                case GridColumns.Actions:
                    GridActionOptions actionOptions = new GridActionOptions();
                    actionOptions.VerifyCC = IsActionPresent(GridActions.VerifyCC, name);
                    actionOptions.Delete = IsActionPresent(GridActions.Delete, name);
                    actionOptions.AdminDelete = IsActionPresent(GridActions.AdminDelete, name);
                    value = actionOptions.ToString();
                    break;
            }

            return value;
        }

        /// <summary>
        /// Verifies (asserts) that a particular grid value is present.
        /// </summary>
        /// <param name="gridCol"></param>
        /// <param name="name">Name of merchant gateway</param>
        /// <param name="expected"></param>
        public void VerifyGridValue(GridColumns gridCol, string name, string expected)
        {
            string actual = GetGridValue(gridCol, name);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifies the values in all columns of specified gateway grid row.
        /// Skips verification expected value is null.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isDefault"></param>
        /// <param name="gateway"></param>
        /// <param name="optionListCCs"></param>
        /// <param name="modifiedBy"></param>
        /// <param name="lastModified"></param>
        /// <param name="optionListActions"></param>
        [Verify]
        public void VerifyGridRow(string name, string isDefault, string gateway, string optionListCCs, string currency, string modifiedBy, string lastModified, string optionListActions)
        {
            if (isDefault != null)
            {
                VerifyGridValue(GridColumns.Default, name, isDefault);
            }

            if (gateway != null)
            {
                VerifyGridValue(GridColumns.Gateway, name, gateway);
            }

            if (optionListCCs != null)
            {
                VerifyGridValue(GridColumns.CardTypes, name, optionListCCs);
            }

            if (currency != null)
            {
                VerifyGridValue(GridColumns.Currency, name, currency);
            }

            if (modifiedBy != null)
            {
                VerifyGridValue(GridColumns.ModifiedBy, name, modifiedBy);
            }

            if (lastModified != null)
            {
                VerifyGridValue(GridColumns.LastModified, name, lastModified);
            }

            if (optionListActions != null)
            {
                VerifyGridValue(GridColumns.Actions, name, optionListActions);
            }
        }

        /// <summary>
        /// Checks to see if the credit card type has been verified.
        /// </summary>
        /// <param name="ccType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsVerifiedCC(CCTypes ccType, string name)
        {
            string xPath = ComposeGridLocator(ccType, name) + "[contains(@src,'sm_True.gif')]";
            return WebDriverUtility.DefaultProvider.IsElementPresent(xPath, LocateBy.XPath);
        }

        /// <summary>
        /// Checks to see if a particular action is present.
        /// </summary>
        /// <param name="gridAction"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsActionPresent(GridActions gridAction, string name)
        {
            string xPath = ComposeGridLocator(gridAction, name);

            return WebDriverUtility.DefaultProvider.IsElementPresent(xPath, LocateBy.XPath);
        }
        #endregion
	}
}
