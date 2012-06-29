namespace RegOnline.RegressionTest.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Linq;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public abstract class ManagerBase
    {
        public const string BuilderWindowId = "Builder";
        public const int InvalidId = -1;
        public const string InvalidSessionId = "";
        private const string PreviewFrameLocator = "ctl00_cph_ifrmPreview";
        private const string AllowCookiesLocator = "//button[@class='ui-button ui-widget ui-corner-all ui-button-text-only dialog-allow-button']";
                
        public class RegType
        {
            public string RegistantTypeName;

            public bool DisableGroupReg { get; set; }
        }

        protected enum DefaultStartDateTime
        {
            Year = 2020,
            Month = 8,
            Day = 1,
            Hour = 8,
            Minute = 0,
            Second = 0
        }

        protected enum DefaultEndDateTime
        {
            Year = 2020,
            Month = 8,
            Day = 5,
            Hour = 17,
            Minute = 0,
            Second = 0
        }

        protected static readonly DateTime _defaultEventStartDate = new DateTime(
            (int)DefaultStartDateTime.Year,
            (int)DefaultStartDateTime.Month,
            (int)DefaultStartDateTime.Day);

        public static DateTime DefaultEventStartDate
        {
            get
            {
                return _defaultEventStartDate;
            }
        }

        protected static readonly DateTime _defaultEventStartDateTime = new DateTime(
            (int)DefaultStartDateTime.Year,
            (int)DefaultStartDateTime.Month,
            (int)DefaultStartDateTime.Day,
            (int)DefaultStartDateTime.Hour,
            (int)DefaultStartDateTime.Minute,
            (int)DefaultStartDateTime.Second);

        public static DateTime DefaultEventStartDateTime
        {
            get
            {
                return _defaultEventStartDateTime;
            }
        }

        protected static readonly DateTime _defaultEventEndDate = new DateTime(
            (int)DefaultEndDateTime.Year,
            (int)DefaultEndDateTime.Month,
            (int)DefaultEndDateTime.Day);

        public static DateTime DefaultEventEndDate
        {
            get
            {
                return _defaultEventEndDate;
            }
        }

        protected static readonly DateTime _defaultEventEndDateTime = new DateTime(
            (int)DefaultEndDateTime.Year,
            (int)DefaultEndDateTime.Month,
            (int)DefaultEndDateTime.Day,
            (int)DefaultEndDateTime.Hour,
            (int)DefaultEndDateTime.Minute,
            (int)DefaultEndDateTime.Second);

        public static DateTime DefaultEventEndDateTime
        {
            get
            {
                return _defaultEventEndDateTime;
            }
        }

        public enum PaymentMethod
        {
            CreditCard,
            Check,
            PurchaseOrder,
            Cash,
            PayAtTheEvent,
            CostCenter,
            WireTransfer,
            Custom,
            PayPal
        }

        protected struct RegisterSiteLocator
        {
            public const string CheckinRegTypeLabelFormat = "//ol[@id='radRegTypes']/li/label[contains(text(),'{0}')]";
            public const string ContactInfoNameLocator = "ctl00_cph_personalInfoStandardFields_rptProxyFields_ctl00_sf_txtResponse";
            public const string ContactInfoPhoneLocator = "ctl00_cph_personalInfoStandardFields_rptProxyFields_ctl01_sf_txtResponse";
            public const string ContactInfoEmailLocator = "ctl00_cph_personalInfoStandardFields_rptProxyFields_ctl02_sf_txtResponse";
        }

        public void SelectBuilderWindow()
        {
            try
            {
                UIUtilityProvider.UIHelper.SelectWindowByName(BuilderWindowId);
            }
            catch
            {
                UIUtilityProvider.UIHelper.SelectOriginalWindow();
            }
        }

        public void SelectManagerWindow()
        {
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
        }

        protected void SelectPreviewFrame()
        {
            UIUtilityProvider.UIHelper.SelectPopUpFrameById(PreviewFrameLocator);
        }

        [Step]
        public void SelectReportPopupWindow()
        {
            Utility.ThreadSleep(1.5);
            UIUtilityProvider.UIHelper.SelectWindowByIndex(1);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public string GetEventSessionId()
        {
            return UIUtilityProvider.UIHelper.GetQueryStringValue("EventSessionId");
        }

        public string GetWindowID(string idString)
        {
            List<string> windowHandles = UIUtilityProvider.UIHelper.GetAllWindows();

            foreach (string s in windowHandles)
            {
                if (s.Contains(idString))
                {
                    return s;
                }
            }

            Assert.Fail("Can't find window that contains string: " + idString);
            return string.Empty;
        }

        public List<EventRegType> Fetch_RegTypes(int eventId)
        {
            ClientDataContext db = new ClientDataContext();
            return (from regType in db.EventRegTypes where regType.EventId == eventId select regType).ToList();
        }

        public int Fetch_RegTypeID(int eventID, string regTypeName)
        {
            List<EventRegType> regTypes = this.Fetch_RegTypes(eventID);

            foreach (EventRegType regType in regTypes)
            {
                if (regType.Description == regTypeName)
                {
                    return regType.Id;
                }
            }

            Assert.Fail(string.Format("No such reg type: {0}", regTypeName));
            return InvalidId;
        }

        [Verify]
        public void VerifyCustomFieldPresent(string name, bool present)
        {
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format("//*[contains(text(),'{0}')]", name), present, LocateBy.XPath);
        }

        public void VerifyCustomFieldRequired(string name, bool required)
        {
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format("//*[contains(text(),'{0}')]//img[@alt='Required']", name), required, LocateBy.XPath);
        }

        public void AllowCookies()
        {
            if (UIUtilityProvider.UIHelper.IsElementPresent(AllowCookiesLocator, LocateBy.XPath))
            {
                Utility.ThreadSleep(1);
                UIUtilityProvider.UIHelper.Click(AllowCookiesLocator, LocateBy.XPath);
            }
        }

        #region UI edit
        #region Enums

        /// <summary>
        /// Input types for fields.
        /// </summary>
        protected enum InputType
        {
            Read,
            Textbox,
            Textarea,
            Dropdown,
            Checkbox,
            Other
        }

        /// <summary>
        /// Types of locators needed.
        /// </summary>
        protected enum LocatorType
        {
            Label,
            Edit
        }
        #endregion

        #region Required virtuals

        protected virtual string GetLocator<TEnum>(TEnum fieldEnum, LocatorType locatorType)
        {
            Assert.AreEqual("Override of GetLocator", "No override!");
            return "Need to override GetLocator";
        }

        protected virtual InputType GetInputType<TEnum>(TEnum fieldEnum)
        {
            Assert.AreEqual("Override of GetInputType", "No override!");

            return 0;
        }

        #endregion

        #region Get label

        /// <summary>
        /// Get field labels with EnumValueSet
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="setToCheck"></param>
        /// <returns></returns>
        public virtual EnumValueSet<TEnum, string> GetFieldLabel<TEnum>(EnumValueSet<TEnum, bool> setToCheck)
        {
            EnumValueSet<TEnum, string> setToReturn = new EnumValueSet<TEnum, string>(null);

            foreach (TEnum enumField in setToCheck.Keys)
            {
                if (setToCheck.GetValue(enumField))
                {
                    setToReturn.SetValue(enumField, GetFieldLabel(enumField));
                }
            }
            return setToReturn;
        }

        /// <summary>
        /// Get label for field (specified by enum)
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <returns></returns>
        public virtual string GetFieldLabel<TEnum>(TEnum fieldEnum)
        {
            return GetFieldLabel(GetLocator(fieldEnum, LocatorType.Label));
        }

        /// <summary>
        /// Get field label with locator.
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        protected string GetFieldLabel(string locator)
        {
            return UIUtilityProvider.UIHelper.GetText(locator, LocateBy.XPath);
        }
        #endregion

        #region Verify label

        /// <summary>
        /// Verify labels by enum value set
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="setToVerify"></param>
        public virtual void VerifyFieldLabel<TEnum, TValue>(EnumValueSet<TEnum, TValue> setToVerify)
        {
            foreach (TEnum enumField in setToVerify.Keys)
            {
                TValue expected = setToVerify.GetValue(enumField);
                if (expected != null)
                {
                    VerifyFieldLabel(enumField, expected.ToString());
                }
            }
        }

        /// <summary>
        /// Verify label by enum
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <param name="expected"></param>
        public virtual void VerifyFieldLabel<TEnum, TValue>(TEnum fieldEnum, TValue expected)
        {
            string locator = GetLocator(fieldEnum, LocatorType.Label);
            VerifyFieldLabel(locator, expected.ToString());
        }

        /// <summary>
        /// Verify field label with xpath locator.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="expected"></param>
        protected void VerifyFieldLabel(string locator, string expected)
        {
            string actual = GetFieldLabel(locator);
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Verify field settings

        /// <summary>
        /// Verify by enum- enabled, label, and value
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <param name="isEnabled"></param>
        /// <param name="expLabel"></param>
        /// <param name="expValue"></param>
        [Verify]
        public virtual void VerifyFieldSettings<TEnum, TValue>(TEnum fieldEnum, bool? isEnabled, string expLabel, TValue expValue)
        {
            if (isEnabled != null)
                VerifyFieldEnabled(fieldEnum, (bool)isEnabled);
            if (expLabel != null)
                VerifyFieldLabel(fieldEnum, expLabel.ToString());
            if (expValue != null)
                VerifyFieldValue(fieldEnum, expValue);
        }

        /// <summary>
        /// Verify by enum- present, enabled, label, and value
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <param name="isPresent"></param>
        /// <param name="isEnabled"></param>
        /// <param name="expLabel"></param>
        /// <param name="expValue"></param>
        public virtual void VerifyFieldSettings<TEnum, TValue>(TEnum fieldEnum, bool? isPresent, bool? isEnabled, string expLabel, TValue expValue)
        {
            if (isPresent != null)
                VerifyFieldPresent(fieldEnum, (bool)isPresent);
            if (isEnabled != null)
                VerifyFieldEnabled(fieldEnum, (bool)isEnabled);
            if (expLabel != null)
                VerifyFieldLabel(fieldEnum, expLabel.ToString());
            if (expValue != null)
                VerifyFieldValue(fieldEnum, expValue);
        }

        #endregion

        #region Verify field present

        /// <summary>
        /// Verify that fields are present with EnumValueSet
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="setToVerify"></param>
        public virtual void VerifyFieldPresent<TEnum>(EnumValueSet<TEnum, bool?> setToVerify)
        {
            foreach (TEnum enumField in setToVerify.Keys)
            {
                bool? expected = setToVerify.GetValue(enumField);
                VerifyFieldPresent(enumField, expected);
            }
        }

        /// <summary>
        /// Verify that the field (specified by enum) is present
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <param name="isPresent"></param>
        public virtual void VerifyFieldPresent<TEnum>(TEnum fieldEnum, bool? isPresent)
        {
            string locator = GetLocator(fieldEnum, LocatorType.Edit);
            VerifyTool.VerifyValue(isPresent, UIUtilityProvider.UIHelper.IsElementPresent(locator, LocateBy.Id), fieldEnum.ToString() + " visible is {0}");
        }
        #endregion

        #region Verify field enabled

        /// <summary>
        /// Verify that fields are enabled with EnumValueSet
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="setToVerify"></param>
        public virtual void VerifyFieldEnabled<TEnum>(EnumValueSet<TEnum, bool?> setToVerify)
        {
            foreach (TEnum enumField in setToVerify.Keys)
            {
                bool? expected = setToVerify.GetValue(enumField);
                if (expected != null)
                {
                    VerifyFieldEnabled(enumField, (bool)expected);
                }
            }
        }

        /// <summary>
        /// Verify that the field (specified by enum) is enabled
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <param name="isEnabled"></param>
        public virtual void VerifyFieldEnabled<TEnum>(TEnum fieldEnum, bool? isEnabled)
        {
            string locator = GetLocator(fieldEnum, LocatorType.Edit);
            string assertText = string.Concat(fieldEnum.ToString(), " enabled is {0}");
            string expected = string.Format(assertText, isEnabled.ToString());
            string found = string.Format(assertText, UIUtilityProvider.UIHelper.IsEditable(locator, LocateBy.XPath).ToString());
            if (isEnabled != null)
            {
                Assert.AreEqual(expected, found);
            }
        }
        #endregion

        #region Is field checked
        /// <summary>
        /// Determines if checkbox field is checked.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <returns></returns>
        protected virtual bool IsFieldChecked<TEnum>(TEnum fieldEnum)
        {
            return IsFieldChecked(GetLocator(fieldEnum, LocatorType.Edit));
        }
        #endregion

        #region Set field value

        /// <summary>
        /// Set field value with EnumValueSet
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="setToAdd"></param>
        public virtual void SetFieldValue<TEnum, TValue>(EnumValueSet<TEnum, TValue> setToAdd)
        {
            foreach (TEnum enumField in setToAdd.Keys)
            {
                TValue setValue = setToAdd.GetValue(enumField);
                if (setValue != null)
                {
                    SetFieldValue(enumField, setValue);
                }
            }
        }

        /// <summary>
        /// Set value for field (specified by enum)
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <param name="value"></param>
        [Step]
        public virtual void SetFieldValue<TEnum, TValue>(TEnum fieldEnum, TValue value)
        {
            InputType inputType = GetInputType(fieldEnum);
            string locator = GetLocator(fieldEnum, LocatorType.Edit);
            SetFieldValue(inputType, locator, value);
        }

        /// <summary>
        /// Set a value with input type and xpath locator.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="inputType"></param>
        /// <param name="locator"></param>
        /// <param name="setValue"></param>
        protected void SetFieldValue<TValue>(InputType inputType, string locator, TValue setValue)
        {
            switch (inputType)
            {
                case InputType.Textbox:
                    SetTextValue(locator, setValue.ToString());
                    break;
                case InputType.Dropdown:
                    SetDropdownValue(locator, setValue.ToString());
                    break;
                case InputType.Checkbox:
                    if (setValue != null)
                    {
                        UIUtilityProvider.UIHelper.SetCheckbox(locator, Boolean.Parse(setValue.ToString()), LocateBy.XPath);
                    }
                    break;
                case InputType.Textarea:
                    UIUtilityProvider.UIHelper.Type(locator, setValue.ToString(), LocateBy.XPath);
                    Utility.ThreadSleep(0.5);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Click field
        /// <summary>
        /// Click field (specified by enum)
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="fieldEnum"></param>
        public virtual void ClickField<TEnum>(TEnum fieldEnum)
        {
            ClickField(GetLocator(fieldEnum, LocatorType.Edit));
        }

        /// <summary>
        /// Click on a field with locator.
        /// </summary>
        /// <param name="locator"></param>
        protected void ClickField(string locator)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.XPath);
            Utility.ThreadSleep(0.5);
        }
        #endregion

        #region Get field value

        /// <summary>
        /// Get field values with EnumValueSet
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="setToCheck"></param>
        /// <returns></returns>
        public virtual EnumValueSet<TEnum, string> GetFieldValue<TEnum>(EnumValueSet<TEnum, bool> setToCheck)
        {
            EnumValueSet<TEnum, string> setToReturn = new EnumValueSet<TEnum, string>(null);

            foreach (TEnum enumField in setToCheck.Keys)
            {
                if (setToCheck.GetValue(enumField))
                {
                    setToReturn.SetValue(enumField, GetFieldValue(enumField));
                }
            }
            return setToReturn;
        }

        /// <summary>
        /// Get value for field (specified by enum)
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <returns></returns>
        public virtual string GetFieldValue<TEnum>(TEnum fieldEnum)
        {
            InputType inputType = GetInputType(fieldEnum);
            string locator = GetLocator(fieldEnum, LocatorType.Edit);

            return GetFieldValue(inputType, locator);
        }

        /// <summary>
        /// Get a field value with input type and locator.
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="locator"></param>
        /// <returns></returns>
        protected string GetFieldValue(InputType inputType, string locator)
        {
            string value = string.Empty;

            switch (inputType)
            {
                case InputType.Textbox:
                    value = GetTextValue(locator);
                    break;
                case InputType.Dropdown:
                    value = GetDropdownValue(locator);
                    break;
                case InputType.Checkbox:
                    value = IsFieldChecked(locator).ToString();
                    break;
                case InputType.Read:
                    value = GetReadValue(locator);
                    break;
            }
            return value;
        }
        #endregion

        #region Verify field value

        /// <summary>
        /// Verify field values EnumValueSet
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="setToVerify"></param>
        public virtual void VerifyFieldValue<TEnum, TValue>(EnumValueSet<TEnum, TValue> setToVerify)
        {
            foreach (TEnum enumField in setToVerify.Keys)
            {
                TValue expected = setToVerify.GetValue(enumField);
                if (expected != null)
                {
                    VerifyFieldValue(enumField, expected);
                }
            }
        }

        /// <summary>
        /// Verify value for field (specified by enum)
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="fieldEnum"></param>
        /// <param name="value"></param>
        [Verify]
        public virtual void VerifyFieldValue<TEnum, TValue>(TEnum fieldEnum, TValue value)
        {
            if (value != null)
            {
                InputType inputType = GetInputType(fieldEnum);
                string locator = GetLocator(fieldEnum, LocatorType.Edit);

                VerifyFieldValue(inputType, locator, value);
            }
        }

        /// <summary>
        /// Verify a field value with inputType and locator.
        /// Skips verification if expected value is null.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="inputType"></param>
        /// <param name="locator"></param>
        /// <param name="value"></param>
        protected void VerifyFieldValue<TValue>(InputType inputType, string locator, TValue value)
        {
            if (value != null)
            {
                switch (inputType)
                {
                    case InputType.Textbox:
                        VerifyTextValue(locator, value.ToString());
                        break;
                    case InputType.Dropdown:
                        VerifyDropdownValue(locator, value.ToString());
                        break;
                    case InputType.Checkbox:
                        VerifyCheckbox(locator, Boolean.Parse(value.ToString()));
                        break;
                    case InputType.Read:
                        VerifyReadValue(locator, value.ToString());
                        break;
                }
            }
        }

        #endregion

        #region Text

        /// <summary>
        /// Sets textbox field with xpath locator.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="value"></param>
        protected void SetTextValue(string locator, string value)
        {
            VerifyIsTextField(locator);
            UIUtilityProvider.UIHelper.Type(locator, value, LocateBy.XPath);
            //System.Threading.Thread.Sleep(500);
        }

        /// <summary>
        /// Get text value with xpath locator.
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        protected string GetTextValue(string locator)
        {
            VerifyIsTextField(locator);
            return UIUtilityProvider.UIHelper.GetValue(locator, LocateBy.XPath);
        }

        /// <summary>
        /// Verify a text value with locator.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="value"></param>
        protected void VerifyTextValue(string locator, string value)
        {
            string valueText = GetTextValue(locator);
            Assert.AreEqual(value, valueText);
        }

        /// <summary>
        /// Verifies that field is a text field
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        private void VerifyIsTextField(string locator)
        {
            string assertTrue = string.Format("Type of text, password, or file: {0}", locator);
            string assertFalse = string.Format("Type not of text, password, or file: {0}", locator);
            string typeAttibute = UIUtilityProvider.UIHelper.GetAttribute(locator, "type", LocateBy.XPath).ToLower();

            if (typeAttibute == "text" ||
                typeAttibute == "password" ||
                typeAttibute == "file")
            {
                Assert.AreEqual(assertTrue, assertTrue);
            }
            else
            {
                Assert.AreEqual(assertTrue, assertFalse);
            }
        }
        #endregion

        #region Dropdowns

        /// <summary>
        /// Set dropdown value with xpath locator.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="value"></param>
        protected void SetDropdownValue(string locator, string value)
        {
            VerifyIsDropdownField(locator);
            UIUtilityProvider.UIHelper.SelectWithText(locator, /*"label="+*/ value, LocateBy.XPath);
            Utility.ThreadSleep(0.5);
        }

        /// <summary>
        /// Get dropdown value with xpath locator.
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        protected string GetDropdownValue(string locator)
        {
            VerifyIsDropdownField(locator);
            return UIUtilityProvider.UIHelper.GetSelectedLabel(locator, LocateBy.XPath);
        }

        /// <summary>
        /// Verify dropdown value with xpath locator.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="value"></param>
        protected void VerifyDropdownValue(string locator, string value)
        {
            string valueText = GetDropdownValue(locator);
            Assert.AreEqual(value, valueText);
        }

        /// <summary>
        /// Verify that field is a dropdown
        /// </summary>
        /// <param name="locator"></param>
        private void VerifyIsDropdownField(string locator)
        {
            string assertTrue = string.Format("Type of dropdown: {0}", locator);
            string assertFalse = string.Format("Type not of dropdown: {0}", locator);
            if (UIUtilityProvider.UIHelper.IsElementPresent(locator + "/option", LocateBy.XPath))
            {
                Assert.AreEqual(assertTrue, assertTrue);
            }
            else
            {
                Assert.AreEqual(assertTrue, assertFalse);
            }
        }
        #endregion

        #region Checkboxes

        /// <summary>
        /// Checks to see if checkbox field with xpath locator is checked.
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        protected bool IsFieldChecked(string locator)
        {
            VerifyIsCheckboxField(locator);
            return UIUtilityProvider.UIHelper.IsChecked(locator, LocateBy.XPath);
        }

        /// <summary>
        /// Verify a checkbox field value with xpath locator.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="toChecked"></param>
        protected void VerifyCheckbox(string locator, bool toChecked)
        {
            VerifyIsCheckboxField(locator);
            Assert.AreEqual(toChecked, UIUtilityProvider.UIHelper.IsChecked(locator, LocateBy.XPath));
        }

        /// <summary>
        /// Verify field is a checkbox.
        /// </summary>
        /// <param name="locator"></param>
        private void VerifyIsCheckboxField(string locator)
        {
            string assertTrue = string.Format("Type of checkbox, or radio: {0}", locator);
            string assertFalse = string.Format("Type not of checkbox, or radio: {0}", locator);
            if ((UIUtilityProvider.UIHelper.IsElementPresent(locator + "[@type='checkbox']", LocateBy.XPath) ||
                (UIUtilityProvider.UIHelper.IsElementPresent(locator + "[@type='radio']", LocateBy.XPath))))
            {
                Assert.AreEqual(assertTrue, assertTrue);
            }
            else
            {
                Assert.AreEqual(assertTrue, assertFalse);
            }
        }

        #endregion

        #region Read

        protected string GetReadValue(string locator)
        {
            return UIUtilityProvider.UIHelper.GetText(locator, LocateBy.XPath);
        }

        protected void VerifyReadValue(string locator, string value)
        {
            string valueText = GetReadValue(locator);
            Assert.AreEqual(value, valueText);
        }

        #endregion
        #endregion
    }
}
