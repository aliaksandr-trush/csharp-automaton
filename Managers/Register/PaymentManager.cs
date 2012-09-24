namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public class PaymentManager : ManagerBase
    {
        public abstract class DefaultPaymentInfo
        {
            public const string CCType = "Visa";
            public const string CCNumber = "4444444444444448";
            public readonly static string CCNumber_Encrypted = Utility.GetEncryptedCCNumber(Managers.Register.PaymentManager.DefaultPaymentInfo.CCNumber);
            public const string CCNumberAlternative = "4012888888881881";
            public const string CVV = "123";// CVV is the namely 'Security Code'
            public const string ExpirationMonth = "12 - Dec";
            public const string ExpirationYear = "2019";
            public const string HolderName = "test test";
            public const string HolderCountry = "United States";
            public const string BillingPhone = "3035775100";
            public const string BillingAddressLineOne = "4750 Walnut Street";
            public const string BillingAddressLineTwo = "suite 100";
            public const string BillingCity = "Boulder";
            public const string BillingState = "CO";
            public const string ZipCode = "99701";
            public const string Country = "United States";
        }

        public enum CCType
        {
            Visa,
            Mastercard,
            Amex,
            Discover,
            Diners,
            Switch
        }

        public enum CCField
        {
            CCType,
            CC,
            CVV,
            CCExpYear,
            CCExpMonth,
            CCName,
            CCCountry,
            CCPhone,
            CCAddress,
            CCAddress2,
            CCCity,
            CCState,
            CCZip
        }

        public enum AdminOnlyField
        {
            ApplyPayment,
            CheckNumber,
            SendConfirmation
        }

        #region Input Types
        protected override InputType GetInputType<TEnum>(TEnum fieldEnum)
        {
            InputType inputType = new InputType();
            string enumString = fieldEnum.ToString();
            Type enumType = fieldEnum.GetType();

            if (enumType == typeof(CCField))
            {
                int enumInt = (int)Enum.Parse(typeof(CCField), enumString);
                inputType = GetInputType((CCField)enumInt);
            }
            else if (enumType == typeof(AdminOnlyField))
            {
                int enumInt = (int)Enum.Parse(typeof(AdminOnlyField), enumString);
                inputType = GetInputType((AdminOnlyField)enumInt);
            }

            return inputType;
        }

        private InputType GetInputType(CCField fieldType)
        {
            InputType inputType = new InputType();
            switch (fieldType)
            {
                case CCField.CCType:
                case CCField.CCExpYear:
                case CCField.CCExpMonth:
                case CCField.CCCountry:
                    inputType = InputType.Dropdown;
                    break;
                case CCField.CC:
                case CCField.CVV:
                case CCField.CCName:
                case CCField.CCAddress:
                case CCField.CCAddress2:
                case CCField.CCCity:
                case CCField.CCState:
                case CCField.CCZip:
                case CCField.CCPhone:
                    inputType = InputType.Textbox;
                    break;
            }
            return inputType;
        }

        private InputType GetInputType(AdminOnlyField fieldType)
        {
            InputType inputType = new InputType();
            switch (fieldType)
            {
                case AdminOnlyField.ApplyPayment:
                case AdminOnlyField.SendConfirmation:
                    inputType = InputType.Checkbox;
                    break;
                case AdminOnlyField.CheckNumber:
                    inputType = InputType.Textbox;
                    break;
            }
            return inputType;
        }
        #endregion

        #region Locators
        protected override string GetLocator<TEnum>(TEnum fieldEnum, LocatorType locatorType)
        {
            string locator = string.Empty;
            string enumString = fieldEnum.ToString();
            Type enumType = fieldEnum.GetType();

            if (enumType == typeof(CCField))
            {
                int enumInt = (int)Enum.Parse(typeof(CCField), enumString);
                locator = GetLocator((CCField)enumInt, locatorType);
            }
            else if (enumType == typeof(AdminOnlyField))
            {
                int enumInt = (int)Enum.Parse(typeof(AdminOnlyField), enumString);
                locator = GetLocator((AdminOnlyField)enumInt, locatorType);
            }

            return locator;
        }

        protected string GetLocator(CCField fieldType, LocatorType locatorType)
        {
            string locator = string.Empty;
            string baseLocator = "//div[@id='wrpCreditCard']//{0}[@id='ctl00_cph_{1}{2}']";
            string forField = fieldType.ToString();
            string forType = string.Empty;
            InputType inputType = GetInputType(fieldType);

            switch (inputType)
            {
                case InputType.Textbox:
                    locator = string.Format(baseLocator,"input","txt",forField);
                    break;
                case InputType.Dropdown:
                    locator = string.Format(baseLocator,"select","ddl",forField);
                    break;
            }
            
            if (locatorType == LocatorType.Label)
            {
                locator = locator + "/../label";
            }

            return locator;
        }

        protected string GetLocator(AdminOnlyField fieldType, LocatorType locatorType)
        {
            string locator = string.Empty;
            string baseLocator = "//input[@id='{0}']";
            string forField = fieldType.ToString();

            switch (fieldType)
            {
                case AdminOnlyField.ApplyPayment:
                    forField = "ctl00_cph_chkApplyPayment";
                    break;
                case AdminOnlyField.CheckNumber:
                    forField = "ctl00_cph_txtCheckNumber";
                    break;
                case AdminOnlyField.SendConfirmation:
                    forField = "ctl00_cph_chkSendConfirmation";
                    break;
            }

            locator = string.Format(baseLocator, forField);

            if (locatorType == LocatorType.Label)
            {
                locator = locator + "/../label";
            }

            return locator;
        }
        #endregion

        [Step]
        public void EnterCreditCardNumberInfo(string cardNumber, string cardCVV, string expMonth, string expYear)
        {
            if (!string.IsNullOrEmpty(cardNumber))
                SetFieldValue(CCField.CC, cardNumber);
            if (!string.IsNullOrEmpty(cardCVV))
            {
                if (UIUtil.DefaultProvider.IsElementPresent(GetLocator(CCField.CVV, LocatorType.Edit), LocateBy.XPath))
                {
                    SetFieldValue(CCField.CVV, cardCVV);
                }
            }
            if (!string.IsNullOrEmpty(expMonth))
                SetFieldValue(CCField.CCExpMonth, expMonth);
            if (!string.IsNullOrEmpty(expYear))
                SetFieldValue(CCField.CCExpYear, expYear);
        }

        [Step]
        public void EnterCreditCardNameCountryType(string holderName, string holderCountry, string cardType)
        {
            if (!string.IsNullOrEmpty(holderName))
                SetFieldValue(CCField.CCName, holderName);
            if (!string.IsNullOrEmpty(holderCountry))
                SetFieldValue(CCField.CCCountry, holderCountry);
            if (!string.IsNullOrEmpty(cardType))
                SetFieldValue(CCField.CCType, cardType);
        }

        [Step]
        public void EnterCreditCardAddressInfo(string address1, string address2, string city, string stateProvince, string zipPostalCode, string CCPhone = null)
        {
            if (!string.IsNullOrEmpty(CCPhone))
            {
                if (UIUtil.DefaultProvider.IsElementPresent(GetLocator(CCField.CCPhone, LocatorType.Edit), LocateBy.XPath))
                {
                    SetFieldValue(CCField.CCPhone, CCPhone);
                }
            }
            if (!string.IsNullOrEmpty(address1))
                SetFieldValue(CCField.CCAddress, address1);
            if (!string.IsNullOrEmpty(address2))
                SetFieldValue(CCField.CCAddress2, address2);
            if (!string.IsNullOrEmpty(city))
                SetFieldValue(CCField.CCCity, city);
            if (!string.IsNullOrEmpty(stateProvince))
                SetFieldValue(CCField.CCState, stateProvince);
            if (!string.IsNullOrEmpty(zipPostalCode))
                SetFieldValue(CCField.CCZip, zipPostalCode);
        }

        [Step]
        public void ClickApplyPaymentNow()
        {
            ClickField(AdminOnlyField.ApplyPayment);
        }

        [Step]
        public void EnterCheckNumber(string checkNumber)
        {
            SetFieldValue(AdminOnlyField.CheckNumber, checkNumber);
        }

        public void ClickSendConfirmation()
        {
            ClickField(AdminOnlyField.SendConfirmation);
        }
    }

    public class CreditCardDetails
    {
        public string Number;
        public DateTime ExpirationDate;
        public string CardholderName;
        public string CvvCode;
        public string Country;
        public string Line1;
        public string Line2;
        public string City;
        public string State;
        public string PostalCode;
        public PaymentManager.CCType Type;
    }
}
