namespace RegOnline.RegressionTest.Managers.Manager.Account
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Managers.Manager.Account.PaymentGateway;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public class AccountManager : ManagerBase
    {
        public const string AccountTabLocator = "//div[@id='ctl00_ctl00_cphDialog_cpMgrMain_radTAccountTabs']//span[text()='{0}']";

        private PaymentGatewayManager gatewayMgr;
        public PaymentGatewayManager GatewayMgr
        {
            get
            {
                return this.gatewayMgr;
            }
            set
            {
                this.gatewayMgr = value;
            }
        }

        private EditUserManager editUserMgr;
        public EditUserManager EditUserMgr
        {
            get
            {
                return this.editUserMgr;
            }
            private set
            {
                this.editUserMgr = value;
            }
        }

        public AccountManager()
        {
            this.gatewayMgr = new PaymentGatewayManager();
            this.editUserMgr = new EditUserManager();
        }

        public enum AccountTab
        {
            Info,
            Reports,
            XEventReports,
            Hotels,
            Users,
            Roles,
            Gateways
        }

        /// <summary>
        /// Return the string name of an Account Tab
        /// </summary>
        /// <param name="accountTab"></param>
        /// <returns></returns>
        private string GetAccountTabName(AccountTab accountTab)
        {
            string tabName = accountTab.ToString();

            switch (accountTab)
            {
                case AccountTab.Info:
                    tabName = "Account Information";
                    break;
                case AccountTab.Reports:
                    tabName = "Account Reports";
                    break;
                case AccountTab.XEventReports:
                    tabName = "Cross-Event Reports";
                    break;
                case AccountTab.Gateways:
                    tabName = "Payment Gateways";
                    break;
            }

            return tabName;
        }

        [Step]
        public void ChooseTab(AccountTab accountTab)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(AccountTabLocator, GetAccountTabName(accountTab)), LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        [Step]
        public void ChooseTabAndVerify(AccountTab accountTab)
        {
            ChooseTab(accountTab);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            VerifyTab(accountTab);
        }

        public void VerifyTab(AccountTab accountTab)
        {
            bool isCorrect = false;

            switch (accountTab)
            {
                case AccountTab.Info:
                    //Verify presence of contact, billing, and defaults sections
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_uclMA_pnlContactInfo", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_uclMA_pnlBillingInfo", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_uclMA_pnlDefaults", LocateBy.Id))
                        isCorrect = true;
                    break;
                case AccountTab.Reports:
                    //Verify presence of account activity and payment sections
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlAcctActivity", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlPayments", LocateBy.Id))
                        isCorrect = true;
                    break;
                case AccountTab.XEventReports:
                    //Verify presence of standard and custom XEvent Report list
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_listXEventReports_pnlStandardXReports", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_listXEventReports_pnlCustomXEventReports", LocateBy.Id))
                        isCorrect = true;
                    break;
                case AccountTab.Hotels:
                    //Verify presence of hotel templates list
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlHotels", LocateBy.Id))
                        isCorrect = true;
                    break;
                case AccountTab.Users:
                    //Verify presence of user list
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlUsers", LocateBy.Id))
                        isCorrect = true;
                    break;
                case AccountTab.Roles:
                    //Verify presence of system roles list and custom roles list
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlSystemRoles", LocateBy.Id) &&
                        UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlCustomRoles", LocateBy.Id))
                        isCorrect = true;
                    break;
                case AccountTab.Gateways:
                    //Verify presence of merchant gateway list
                    if (UIUtilityProvider.UIHelper.IsElementPresent("ctl00_ctl00_cphDialog_cpMgrMain_pnlCustomerMerchants", LocateBy.Id))
                        isCorrect = true;
                    break;
            }

            Assert.That(isCorrect);
        }

	}
}
