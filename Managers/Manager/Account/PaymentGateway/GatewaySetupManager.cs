namespace RegOnline.RegressionTest.Managers.Manager.Account.PaymentGateway
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Managers.Register;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public class GatewaySetupManager : ManagerBase
    {
        #region Locators

        /// <summary>
        /// Override parent GetLocator to call with local enums
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <param name="locatorType"></param>
        /// <returns></returns>
        protected override string GetLocator<TEnum>(TEnum fieldEnum, LocatorType locatorType)
        {
            string locator = string.Empty;
            string enumString = fieldEnum.ToString();
            Type enumType = fieldEnum.GetType();

            if (enumType == typeof(PaymentGatewayManager.SetupFields))
            {
                int enumInt = (int)Enum.Parse(typeof(PaymentGatewayManager.SetupFields), enumString);
                locator = GetLocator((PaymentGatewayManager.SetupFields)enumInt, locatorType);
            }
            else if (enumType == typeof(RegOnline.RegressionTest.Managers.Register.PaymentManager.CCType))
            {
                int enumInt = (int)Enum.Parse(typeof(RegOnline.RegressionTest.Managers.Register.PaymentManager.CCType), enumString);
                locator = GetLocator((RegOnline.RegressionTest.Managers.Register.PaymentManager.CCType)enumInt, locatorType);
            }

            return locator;
        }

        /// <summary>
        /// Get selenium locators for setup fields
        /// </summary>
        /// <param name="locatorType"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        protected string GetLocator(PaymentGatewayManager.SetupFields fieldType, LocatorType locatorType)
        {
            string locator = string.Empty;
            string baseLocator = "//tr[@id='tr{0}']{1}";
            string forField = fieldType.ToString();
            string forType = string.Empty;
            InputType inputType = GetInputType(fieldType);

            switch (locatorType)
            {
                case LocatorType.Edit:
                    //Dropdowns are different!
                    if (inputType == InputType.Dropdown)
                        forType = "/td[2]//select";
                    else
                        forType = "/td[2]//input";
                    break;
                case LocatorType.Label:
                    //Checkbox labels are in different place
                    if (inputType == InputType.Checkbox)
                        forType = "/td[2]";
                    else
                        forType = "/td[1]";
                    break;
            }

            locator = string.Format(baseLocator, forField, forType);
            return locator;
        }

        /// <summary>
        /// Get selenium locators for cc types
        /// </summary>
        /// <param name="locatorType"></param>
        /// <param name="ccType"></param>
        /// <returns></returns>
        protected string GetLocator(
            PaymentManager.CCType ccType, 
            LocatorType locatorType)
        {
            string locator = string.Empty;
            string forField = string.Empty;

            switch (ccType)
            {
                case PaymentManager.CCType.Visa:
                case PaymentManager.CCType.Amex:
                case PaymentManager.CCType.Diners:
                case PaymentManager.CCType.Switch:
                    forField = ccType.ToString();
                    break;
                case PaymentManager.CCType.Mastercard:
                    forField = "Mc";
                    break;
                case PaymentManager.CCType.Discover:
                    forField = "Disc";
                    break;
            }

            switch (locatorType)
            {
                case LocatorType.Edit:
                    locator = string.Format("//td[@id='cc{0}2']//input", forField);
                    break;
                case LocatorType.Label:
                    locator = string.Format("//td[@id='cc{0}1']", forField);
                    break;
            }

            return locator;
        }

        #endregion

        #region Input Types

        /// <summary>
        /// Override parent GetInputType to call with local enums
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <returns></returns>
        protected override InputType GetInputType<TEnum>(TEnum fieldEnum)
        {
            InputType inputType = new InputType();
            string enumString = fieldEnum.ToString();
            Type enumType = fieldEnum.GetType();

            if (enumType == typeof(PaymentGatewayManager.SetupFields))
            {
                int enumInt = (int)Enum.Parse(typeof(PaymentGatewayManager.SetupFields), enumString);
                inputType = GetInputType((PaymentGatewayManager.SetupFields)enumInt);
            }
            else if (enumType == typeof(PaymentManager.CCType))
            {
                int enumInt = (int)Enum.Parse(typeof(PaymentManager.CCType), enumString);
                inputType = GetInputType((PaymentManager.CCType)enumInt);
            }

            return inputType;
        }

        /// <summary>
        /// Returns the input type for each gateway setup field.
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        private InputType GetInputType(PaymentGatewayManager.SetupFields fieldType)
        {
            InputType inputType = new InputType();

            switch (fieldType)
            {
                case PaymentGatewayManager.SetupFields.Description:
                case PaymentGatewayManager.SetupFields.StatementName:
                case PaymentGatewayManager.SetupFields.MerchantID:
                case PaymentGatewayManager.SetupFields.GatewayLogin:
                case PaymentGatewayManager.SetupFields.GatewayPassword:
                case PaymentGatewayManager.SetupFields.GatewayUrl:
                case PaymentGatewayManager.SetupFields.Subprocessor:
                case PaymentGatewayManager.SetupFields.P12GatewayPassword:
                case PaymentGatewayManager.SetupFields.CertUpload:
                case PaymentGatewayManager.SetupFields.CertP12Upload:
                    inputType = InputType.Textbox;
                    break;
                case PaymentGatewayManager.SetupFields.Gateway:
                case PaymentGatewayManager.SetupFields.Prefixes:
                case PaymentGatewayManager.SetupFields.BaseCurrencyCode:
                    inputType = InputType.Dropdown;
                    break;
                case PaymentGatewayManager.SetupFields.DefaultMerchantAccount:
                case PaymentGatewayManager.SetupFields.CsiMerchant:
                    inputType = InputType.Checkbox;
                    break;
            }

            return inputType;
        }

        /// <summary>
        /// Returns the input type for each gateway cc field.
        /// </summary>
        /// <param name="ccType"></param>
        /// <returns></returns>
        private InputType GetInputType(PaymentManager.CCType ccType)
        {
            return InputType.Checkbox;
        }

        #endregion

        #region Errors

        /// <summary>
        /// Class to handle GatewaySetup errors
        /// </summary>
        public static class Error
        {
            public const string MissingRequiredFields =
                "An error has occured saving this gateway merchant configuration. Please correct the fields marked '*' and click 'Save' when completed.";
        }

        /// <summary>
        /// Verifies expected error message at the top of edit page
        /// </summary>
        /// <param name="errorMessage"></param>
        public void VerifyErrorMessage(string errorMessage)
        {
            string errorText = UIUtilityProvider.UIHelper.GetText("//div[@id='bsMerchantGatewaySetup_ADV']/div/div[2]", LocateBy.XPath);
            Assert.AreEqual(errorMessage, errorText);
        }
        #endregion

        #region Verify field enabled

        public override void VerifyFieldEnabled<TEnum>(TEnum fieldEnum, bool? isEnabled)
        {
            Type t = fieldEnum.GetType();

            if ((isEnabled == false) && (t == typeof(PaymentGatewayManager.SetupFields)) &&
                (fieldEnum.ToString() == PaymentGatewayManager.SetupFields.CertUpload.ToString() || fieldEnum.ToString() == PaymentGatewayManager.SetupFields.CertP12Upload.ToString()))
            {
                string locator = string.Empty;

                if (fieldEnum.ToString() == PaymentGatewayManager.SetupFields.CertUpload.ToString())
                {
                    locator = GetLocator(PaymentGatewayManager.SetupFields.CertUpload, LocatorType.Edit);
                }
                else
                {
                    locator = GetLocator(PaymentGatewayManager.SetupFields.CertP12Upload, LocatorType.Edit);
                }

                Assert.IsFalse(UIUtilityProvider.UIHelper.IsElementPresent(locator, LocateBy.XPath));
            }
            else
            {
                base.VerifyFieldEnabled(fieldEnum, isEnabled);
            }
        }
        #endregion

        #region Set field values

        /// <summary>
        /// Set standard setup fields for merchant gateway.
        /// Skips fields whose value is set to null.
        /// </summary>
        /// <param name="forGateway"></param>
        /// <param name="forDescription"></param>
        /// <param name="forStatementName"></param>
        /// <param name="forPrefixes"></param>
        /// <param name="forMerchantID"></param>
        /// <param name="forGatewayLogin"></param>
        /// <param name="forGatewayPassword"></param>
        /// <param name="forGatewayUrl"></param>
        /// <param name="forSubprocessor"></param>
        /// <param name="forCertUpload"></param>
        /// <param name="forBaseCurrencyCode"></param>
        /// <param name="forDefaultMerchantAccount"></param>
        /// <param name="forCsiMerchant"></param>
        public void SetGatewaySetupFields(string forGateway, string forDescription,
            string forStatementName, string forPrefixes, string forMerchantID, string forGatewayLogin,
            string forGatewayPassword, string forGatewayUrl, string forSubprocessor, string forCertUpload,
            string forBaseCurrencyCode, string forDefaultMerchantAccount, string forCsiMerchant)
        {
            EnumValueSet<PaymentGatewayManager.SetupFields, string> setupFieldSet = new EnumValueSet<PaymentGatewayManager.SetupFields, string>(null);

            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.Gateway, forGateway);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.Description, forDescription);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.StatementName, forStatementName);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.Prefixes, forPrefixes);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.MerchantID, forMerchantID);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.GatewayLogin, forGatewayLogin);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.GatewayPassword, forGatewayPassword);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.GatewayUrl, forGatewayUrl);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.Subprocessor, forSubprocessor);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.CertUpload, forCertUpload);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.BaseCurrencyCode, forBaseCurrencyCode);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.DefaultMerchantAccount, forDefaultMerchantAccount);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.CsiMerchant, forCsiMerchant);

            SetFieldValue(setupFieldSet);
        }

        /// <summary>
        /// Set gateway setup fields with EnumValueSet
        /// </summary>
        /// <param name="setupFieldSet"></param>
        public void SetGatewaySetupFields(EnumValueSet<PaymentGatewayManager.SetupFields, string> setupFieldSet)
        {
            SetFieldValue(setupFieldSet);
        }

        /// <summary>
        /// Set standard cc fields for merchant gateway.
        /// Skips fields whose value is set to null.
        /// </summary>
        /// <param name="forVisa"></param>
        /// <param name="forMC"></param>
        /// <param name="forAmex"></param>
        /// <param name="forDiscover"></param>
        /// <param name="forDiners"></param>
        /// <param name="forSwitch"></param>
        public void SetGatewayCCFields(string forVisa, string forMC, string forAmex, string forDiscover, string forDiners, string forSwitch)
        {
            EnumValueSet<PaymentManager.CCType, string> ccFieldSet = new EnumValueSet<PaymentManager.CCType, string>(null);

            ccFieldSet.SetValue(PaymentManager.CCType.Visa, forVisa);
            ccFieldSet.SetValue(PaymentManager.CCType.Mastercard, forVisa);
            ccFieldSet.SetValue(PaymentManager.CCType.Amex, forVisa);
            ccFieldSet.SetValue(PaymentManager.CCType.Discover, forVisa);
            ccFieldSet.SetValue(PaymentManager.CCType.Diners, forVisa);
            ccFieldSet.SetValue(PaymentManager.CCType.Switch, forVisa);

            SetFieldValue(ccFieldSet);
        }

        /// <summary>
        /// Set gateway cc fields with EnumValueSet
        /// </summary>
        /// <param name="ccFieldSet"></param>
        public void SetGatewayCCFields(EnumValueSet<PaymentManager.CCType, bool?> ccFieldSet)
        {
            SetFieldValue(ccFieldSet);
        }

        #endregion

        #region Verify field values

        /// <summary>
        /// Verifies values for standard gateway setup fields.
        /// Skips validation when expected value == null.
        /// </summary>
        /// <param name="forGateway"></param>
        /// <param name="forDescription"></param>
        /// <param name="forStatementName"></param>
        /// <param name="forPrefixes"></param>
        /// <param name="forMerchantID"></param>
        /// <param name="forGatewayLogin"></param>
        /// <param name="forGatewayPassword"></param>
        /// <param name="forGatewayUrl"></param>
        /// <param name="forSubprocessor"></param>
        /// <param name="forCertUpload"></param>
        /// <param name="forBaseCurrencyCode"></param>
        /// <param name="forDefaultMerchantAccount"></param>
        /// <param name="forCsiMerchant"></param>
        public void VerifyGatewaySetupFields(string forGateway, string forDescription,
            string forStatementName, string forPrefixes, string forMerchantID, string forGatewayLogin,
            string forGatewayPassword, string forGatewayUrl, string forSubprocessor, string forCertUpload,
            string forBaseCurrencyCode, string forDefaultMerchantAccount, string forCsiMerchant)
        {
            EnumValueSet<PaymentGatewayManager.SetupFields, string> setupFieldSet = new EnumValueSet<PaymentGatewayManager.SetupFields, string>(null);

            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.Gateway, forGateway);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.Description, forDescription);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.StatementName, forStatementName);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.Prefixes, forPrefixes);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.MerchantID, forMerchantID);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.GatewayLogin, forGatewayLogin);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.GatewayPassword, forGatewayPassword);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.GatewayUrl, forGatewayUrl);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.Subprocessor, forSubprocessor);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.CertUpload, forCertUpload);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.BaseCurrencyCode, forBaseCurrencyCode);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.DefaultMerchantAccount, forDefaultMerchantAccount);
            setupFieldSet.SetValue(PaymentGatewayManager.SetupFields.CsiMerchant, forCsiMerchant);

            VerifyFieldValue(setupFieldSet);
        }

        /// <summary>
        /// Verify gateway setup fields with EnumValueSet
        /// </summary>
        /// <param name="setupFieldSet"></param>
        public void VerifyGatewaySetupFields(EnumValueSet<PaymentGatewayManager.SetupFields, string> setupFieldSet)
        {
            VerifyFieldValue(setupFieldSet);
        }

        /// <summary>
        /// Verifies values for standard gateway cc fields.
        /// Skips validation when expected value == null.
        /// </summary>
        /// <param name="forVisa"></param>
        /// <param name="forMC"></param>
        /// <param name="forAmex"></param>
        /// <param name="forDiscover"></param>
        /// <param name="forDiners"></param>
        /// <param name="forSwitch"></param>
        public void VerifyGatewayCCFields(string forVisa, string forMC, string forAmex, string forDiscover, string forDiners, string forSwitch)
        {
            EnumValueSet<PaymentManager.CCType, string> ccFieldSet = new EnumValueSet<PaymentManager.CCType, string>(null);

            ccFieldSet.SetValue(PaymentManager.CCType.Visa, forVisa);
            ccFieldSet.SetValue(PaymentManager.CCType.Mastercard, forVisa);
            ccFieldSet.SetValue(PaymentManager.CCType.Amex, forVisa);
            ccFieldSet.SetValue(PaymentManager.CCType.Discover, forVisa);
            ccFieldSet.SetValue(PaymentManager.CCType.Diners, forVisa);
            ccFieldSet.SetValue(PaymentManager.CCType.Switch, forVisa);

            VerifyFieldValue(ccFieldSet);
        }

        /// <summary>
        /// Verify gateway cc fields with EnumValueSet
        /// </summary>
        /// <param name="ccFieldSet"></param>
        [Verify]
        public void VerifyGatewayCCFields(EnumValueSet<PaymentManager.CCType, bool?> ccFieldSet)
        {
            VerifyFieldValue( ccFieldSet);
        }

        #endregion

    }
}
