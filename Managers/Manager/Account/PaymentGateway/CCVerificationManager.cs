namespace RegOnline.RegressionTest.Managers.Manager.Account.PaymentGateway
{
    using NUnit.Framework;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public class CCVerificationManager : ManagerBase
    {
        #region enums
        /// <summary>
        /// Fields in CC Verification
        /// </summary>
        public enum Fields
        {
            CardType,
            CardNumber,
            CVV,
            Month,
            Year,
            Name,
            Address1,
            Address2,
            City,
            StateProvince,
            Zip,
            Country,
            Transaction,
            Currency,
            Test
        }
        #endregion

        #region Input Types
        private InputType GetInputType(Fields ccVerifType)
        {
            InputType inputType = new InputType();
            switch (ccVerifType)
            {
                case Fields.CardType:
                case Fields.Year:
                case Fields.Name:
                case Fields.Country:
                case Fields.Currency:
                    inputType = InputType.Dropdown;
                    break;
                case Fields.CardNumber:
                case Fields.CVV:
                case Fields.Month:
                case Fields.Address1:
                case Fields.Address2:
                case Fields.City:
                case Fields.StateProvince:
                case Fields.Zip:
                case Fields.Transaction:
                    inputType = InputType.Textbox;
                    break;
                case Fields.Test:
                    inputType = InputType.Checkbox;
                    break;
            }
            return inputType;
        }
        #endregion

        #region Locators
        protected string GetLocator(Fields ccVerifFields, LocatorType locatorType)
        {
            string locator = string.Empty;
            string baseLocator = "//*[@id='{0}{1}']{2}";
            string forType = string.Empty;
            string forField = ccVerifFields.ToString();
            InputType inputType = GetInputType(ccVerifFields);

            switch (inputType)
            {
                case InputType.Dropdown:
                    forType = "ddl";
                    break;
                case InputType.Textbox:
                    forType = "txt";
                    break;
                case InputType.Checkbox:
                    forType = "chk";
                    break;
            }

            switch (locatorType)
            {
                case LocatorType.Edit:
                    locator = string.Format(baseLocator, forType, forField, string.Empty);
                    break;
                case LocatorType.Label:
                    if (inputType == InputType.Checkbox)
                        locator = string.Format(baseLocator, forType, forField, "/../label");

                    else
                        locator = string.Format(baseLocator, forType, forField, "/../../td[1]");
                    break;
            }

            return locator;
        }
        #endregion

        #region Buttons
        public void ClickVerifyCard()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("Verify Card", LocateBy.LinkText);
        }

        public void ClickClose()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("Close", LocateBy.LinkText);
        }

        #endregion

        #region Verify Field Enabled
        public void VerifyFieldEnabled(Fields aField, bool isEnabled)
        {
            string locator = GetLocator(aField, LocatorType.Edit);
            Assert.AreEqual(isEnabled, WebDriverUtility.DefaultProvider.IsEditable(locator, LocateBy.XPath));
        }
        #endregion

        #region Verify Field Settings
        public void VerifyFieldSettings(Fields aField, string isEnabled, string expLabel, string expValue)
        {
            if (isEnabled != null)
            {
                VerifyFieldEnabled(aField, bool.Parse(isEnabled));
            }

            if (expLabel != null)
            {
                VerifyFieldLabel(aField, expLabel);
            }

            if (expValue != null)
            {
                VerifyFieldValue(aField, expValue);
            }
        }
        #endregion

        #region Set Field Value
        public void SetFieldValue(Fields aField, string aValue)
        {
            InputType inputType = GetInputType(aField);
            string locator = GetLocator(aField, LocatorType.Edit);

            SetFieldValue(inputType, locator, aValue);
        }
        #endregion

        #region Click Field
        public void ClickField(Fields aField)
        {
            ClickField(GetLocator(aField, LocatorType.Edit));
        }
        #endregion

        #region Get Field Value
        public string GetFieldValue(Fields aField)
        {
            InputType inputType = GetInputType(aField);
            string locator = GetLocator(aField, LocatorType.Edit);

            return GetFieldValue(inputType, locator);
        }
        #endregion

        #region Verify Field Value
        public void VerifyFieldValue(Fields aField, string value)
        {
            InputType inputType = GetInputType(aField);
            string locator = GetLocator(aField, LocatorType.Edit);

            VerifyFieldValue(inputType, locator, value);
        }
        #endregion

        #region Is Field Checked
        public bool IsFieldChecked(Fields aField)
        {
            return IsFieldChecked(GetLocator(aField, LocatorType.Edit));
        }
        #endregion

        #region Get Field label
        public string GetFieldLabel(Fields aField)
        {
            return GetFieldLabel(GetLocator(aField, LocatorType.Label));
        }
        #endregion

        #region Verify field label
        public void VerifyFieldLabel(Fields aField, string expected)
        {
            string locator = GetLocator(aField, LocatorType.Label);
            VerifyFieldLabel(locator, expected);
        }
        #endregion
    }
}
