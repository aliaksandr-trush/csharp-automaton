namespace RegOnline.RegressionTest.Fixtures.Gateway
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager.Account;
    using RegOnline.RegressionTest.Managers.Manager.Account.PaymentGateway;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class GatewaySetupFixture : FixtureBase
    {
        /// <summary>
        /// Checklist- Build an express event
        /// </summary>
        [Test]
        [Category(Priority.Three)]
        [Description("705")]
        public void PayflowProSetupCustomer()
        {
            string gatewayName = "PayFlowPro Customer";

            // open login page on BETA
            ManagerSiteMgr.OpenLogin();

            // login to regression testing account
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToAccountTabIfNeeded();
            ManagerSiteMgr.AccountMgr.ChooseTabAndVerify(AccountManager.AccountTab.Gateways);

            // Since gateway list is not sorted by add date descendingly, we must delete previously added gateways as a cleanup
            ManagerSiteMgr.AccountMgr.GatewayMgr.DeletePaymentGateway(gatewayName);

            ManagerSiteMgr.AccountMgr.GatewayMgr.ClickAddPaymentGateway();

            //Default values
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.Description, true, "*Description:", "");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.Gateway, true, "Gateway:", "PayFlowPro");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.StatementName, true, "*Statement Name:", "");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.MerchantID, true, "*User:", "");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.GatewayLogin, true, "*Payflow Pro Login:", "");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.GatewayPassword, true, "*Payflow Pro Password:", "");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.Subprocessor, true, "*Payflow Partner:", "");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.BaseCurrencyCode, true, "Payment Gateway Currency:", "US Dollar");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.DefaultMerchantAccount, true, "Set as default payment gateway for all new events", true);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Visa, true, " Visa", false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Mastercard, true, " Mastercard", false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Amex, true, " American Express", false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Discover, true, " Discover", false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Diners, true, " Diners", false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Switch, false, " Switch / Maestro", false);

            //Set values
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.Description, gatewayName);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.StatementName, "StatementName");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.MerchantID, "User");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.GatewayLogin, "Login");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.GatewayPassword, "Password");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.Subprocessor, "Partner");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.DefaultMerchantAccount, false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentManager.CCType.Visa, "true");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentManager.CCType.Mastercard, "true");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentManager.CCType.Amex, "true");
            ManagerSiteMgr.AccountMgr.GatewayMgr.ClickOK();

            EnumValueSet<PaymentManager.CCType, bool?> ccOptionSet = new EnumValueSet<PaymentManager.CCType, bool?>(null);
            ccOptionSet.SetValue(PaymentManager.CCType.Visa, true);
            ccOptionSet.SetValue(PaymentManager.CCType.Mastercard, true);
            ccOptionSet.SetValue(PaymentManager.CCType.Amex, true);

            //Verify in Grid
            PaymentGatewayManager.CreditCardOptions ccOptions_old = new PaymentGatewayManager.CreditCardOptions();
            ccOptions_old.Visa = true;
            ccOptions_old.Mastercard = true;
            ccOptions_old.Amex = true;
            PaymentGatewayManager.GridActionOptions actionOptions = new PaymentGatewayManager.GridActionOptions();
            actionOptions.VerifyCC = true;
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            ManagerSiteMgr.AccountMgr.GatewayMgr.VerifyGridRow(gatewayName, "False", "PayFlowPro", ccOptions_old.ToString(), "USD", null, null, actionOptions.ToString());

            //Open and re-verify values
            ManagerSiteMgr.AccountMgr.GatewayMgr.OpenPaymentGateway(gatewayName);

            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.Description, true, "*Description:", gatewayName);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.Gateway, false, "Gateway:", "PayFlowPro");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.StatementName, true, "*Statement Name:", "StatementName");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.MerchantID, true, "*User:", "User");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.GatewayLogin, true, "*Payflow Pro Login:", "Login");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.GatewayPassword, true, "*Payflow Pro Password:", "Password");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.Subprocessor, true, "*Payflow Partner:", "Partner");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.BaseCurrencyCode, true, "Payment Gateway Currency:", "US Dollar");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyGatewayCCFields(ccOptionSet);
            ManagerSiteMgr.AccountMgr.GatewayMgr.ClickOK();

            //Clean up
            ManagerSiteMgr.AccountMgr.GatewayMgr.DeletePaymentGateway(gatewayName);

            ManagerSiteMgr.AccountMgr.ChooseTab(AccountManager.AccountTab.Info);
            ManagerSiteMgr.GotoTab(Managers.Manager.ManagerSiteManager.Tab.Events);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("706")]
        public void FirstDataGlobalSetupCustomer()
        {
            string gatewayName = "FirstDataGlobal Customer Test";

            // open login page on BETA
            ManagerSiteMgr.OpenLogin();

            // login to regression testing account
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToAccountTabIfNeeded();
            ManagerSiteMgr.AccountMgr.ChooseTabAndVerify(AccountManager.AccountTab.Gateways);

            // Since gateway list is not sorted by add date descendingly, we must delete previously added gateways as a cleanup
            ManagerSiteMgr.AccountMgr.GatewayMgr.DeletePaymentGateway(gatewayName);

            ManagerSiteMgr.AccountMgr.GatewayMgr.ClickAddPaymentGateway();

            //Verify default values
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.Gateway, true, "Gateway:", "PayFlowPro");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.Gateway, "First Data Global Gateway (WS)");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.Description, true, "*Description:", "");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.Gateway, true, "Gateway:", "First Data Global Gateway (WS)");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.StatementName, true, "*Statement Name:", "");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.GatewayLogin, true, "*Store #:", "");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.CertUpload, true, "*Upload Certificate (pem):", "");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.CertP12Upload, true, "*Upload Certificate (p12):", "");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.BaseCurrencyCode, true, "Payment Gateway Currency:", "US Dollar");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.DefaultMerchantAccount, true, "Set as default payment gateway for all new events", true);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Visa, true, " Visa", false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Mastercard, true, " Mastercard", false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Amex, true, " American Express", false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Discover, true, " Discover", false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Diners, true, " Diners", false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentManager.CCType.Switch, false, " Switch / Maestro", false);

            EnumValueSet<PaymentManager.CCType, bool?> ccOptionSet = new EnumValueSet<PaymentManager.CCType, bool?>(null);
            ccOptionSet.SetValue(PaymentManager.CCType.Visa, true);
            ccOptionSet.SetValue(PaymentManager.CCType.Amex, true);
            ccOptionSet.SetValue(PaymentManager.CCType.Diners, true);

            //Set CC and Action options
            PaymentGatewayManager.CreditCardOptions ccOptions_old = new PaymentGatewayManager.CreditCardOptions();
            ccOptions_old.Visa = true;
            ccOptions_old.Amex = true;
            ccOptions_old.Diners = true;
            PaymentGatewayManager.GridActionOptions actionOptions = new PaymentGatewayManager.GridActionOptions();
            actionOptions.VerifyCC = true;

            //Set values
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.Description, gatewayName);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.StatementName, "StatementName");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.GatewayLogin, "Store");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.GatewayPassword, "Password");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.P12GatewayPassword, "Password");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.CertUpload, @"C:\QA\MerchantGateways\1128072.pem");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.CertP12Upload, @"C:\QA\MerchantGateways\1128072.p12");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentGatewayManager.SetupFields.DefaultMerchantAccount, false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentManager.CCType.Visa, "true");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentManager.CCType.Amex, "true");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.SetFieldValue(PaymentManager.CCType.Diners, "true");

            ManagerSiteMgr.AccountMgr.GatewayMgr.ClickOK();

            //Verify in Grid
            ManagerSiteMgr.AccountMgr.GatewayMgr.VerifyGridRow(gatewayName, "False", "First Data Global Gateway (WS)", ccOptions_old.ToString(), "USD", null, null, actionOptions.ToString());

            //Open and re-verify values
            ManagerSiteMgr.AccountMgr.GatewayMgr.OpenPaymentGateway(gatewayName);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.Description, true, "*Description:", gatewayName);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.Gateway, false, "Gateway:", "First Data Global Gateway (WS)");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.StatementName, true, "*Statement Name:", "StatementName");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.GatewayLogin, true, "*Store #:", "Store");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.CertUpload, false, "*Upload New Certificate (pem):", (string)null);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.CertP12Upload, false, "*Upload new certificate (p12):", (string)null);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.BaseCurrencyCode, true, "Payment Gateway Currency:", "US Dollar");
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyFieldSettings(PaymentGatewayManager.SetupFields.DefaultMerchantAccount, true, "Set as default payment gateway for all new events", false);
            ManagerSiteMgr.AccountMgr.GatewayMgr.GatewaySetupMgr.VerifyGatewayCCFields(ccOptionSet);
            ManagerSiteMgr.AccountMgr.GatewayMgr.ClickOK();

            //Clean up
            ManagerSiteMgr.AccountMgr.GatewayMgr.DeletePaymentGateway(gatewayName);

            ManagerSiteMgr.AccountMgr.ChooseTab(AccountManager.AccountTab.Info);
            ManagerSiteMgr.GotoTab(Managers.Manager.ManagerSiteManager.Tab.Events);
        }
    }
}
