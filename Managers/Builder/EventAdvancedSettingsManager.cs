namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public class EventAdvancedSettingsManager : ManagerBase
    {
        private const string DialogButtonLocator = "//div[@class='dialogButtonRow']/input[@id='{0}']";

        public enum AuthenticationField
        {
            AuthMethod,
            InvitationOnly,
            InvitationPassword,
            InvitationList,
            NoDupEmails,
            EnableAutoUpdate,
            RequirePassword,
            EnableXAccountRecall
        }

        public enum AuthenicationMothedType
        {
            [StringValue("Email Address Only")]
            EmailAddressOnly,

            [StringValue("Membership ID Only")]
            MembershipIDOnly,

            [StringValue("Email Address and Membership ID (One is required)")]
            EmailAddressAndMembershipID,

            [StringValue("Email Address and External Authentication")]
            EmailAddressAndExternalAuthentication,
        }

        public void ClickCancel()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(DialogButtonLocator, "ctl00_btnCancel"), LocateBy.XPath);
            Utility.ThreadSleep(2);
        }

        [Step]
        public void ClickSaveAndClose()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(DialogButtonLocator, "ctl00_btnSaveClose"), LocateBy.XPath);
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        public void ClickSaveAndStay()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(DialogButtonLocator, "ctl00_btnSaveStay"), LocateBy.XPath);
            Utility.ThreadSleep(2);
        }

        [Step]
        public void SetInternalCode(string code)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_txtEventsClientEventID", code, LocateBy.Id);
        }

        [Verify]
        public void VerifyAuthernticationMethodDropDownListIsEditable(bool enabled)
        {
            Assert.AreEqual(enabled, UIUtilityProvider.UIHelper.IsEditable("ctl00_cphDialog_ddlEventsAuthenticationTypeID", LocateBy.Id));
        }

        [Verify]
        public void VerifyAuthernticationMethodType(AuthenicationMothedType type)
        {
            Assert.AreEqual(StringEnum.GetStringValue(type), UIUtilityProvider.UIHelper.GetSelectedLabel("ctl00_cphDialog_ddlEventsAuthenticationTypeID", LocateBy.Id));
        }

        //public void VerifyEventsRequirePasswordOnRecallIsEnable(bool enabled)
        //{
        //    //Assert.AreEqual(enabled, UIUtilityProvider.UIHelper.IsChecked(UIManager.LocatorPrefix.Id + "ctl00_cphDialog_chkEventsRequirePassword"));
        //}

        //[Verify]
        //public void VerifyEventsRequirePasswordOnRecallIsEditable(bool enabled)
        //{
        //    //Assert.AreEqual(enabled, UIUtilityProvider.UIHelper.IsEditable(UIManager.LocatorPrefix.Id + "ctl00_cphDialog_chkEventsRequirePassword"));
        //}

        //[Verify]
        //public void VerifyEventsRequirePasswordOnRecallIsDisplayed(bool displayed)
        //{
        //    //Assert.AreEqual(displayed, UIUtilityProvider.UIHelper.IsElementDisplay(UIManager.LocatorPrefix.Id + "ctl00_cphDialog_chkEventsRequirePassword"));
        //}

        public void VerifyEventsEnableCrossAccountOnRecallIsEnable(bool enabled)
        {
            Assert.AreEqual(enabled, UIUtilityProvider.UIHelper.IsChecked("ctl00_cphDialog_chkEventsEnableCrossAccountRecall", LocateBy.Id));
        }

        [Verify]
        public void VerifyEventsEnableCrossAccountOnRecallIsEditable(bool enabled)
        {
            Assert.AreEqual(enabled, UIUtilityProvider.UIHelper.IsEditable("ctl00_cphDialog_chkEventsEnableCrossAccountRecall", LocateBy.Id));
        }

        [Verify]
        public void VerifyEventsEnableCrossAccountOnRecallIsDisplayed(bool displayed)
        {
            Assert.AreEqual(displayed, UIUtilityProvider.UIHelper.IsElementDisplay("ctl00_cphDialog_chkEventsEnableCrossAccountRecall", LocateBy.Id));
        }

        //public void CheckEventsRequirePasswordOnRecall(bool enable)
        //{
        //    //UIUtilityProvider.UIHelper.SetCheckbox(UIManager.LocatorPrefix.Id + "ctl00_cphDialog_chkEventsRequirePassword", enable);
        //}

        [Step]
        public void CheckEventsEnableCrossAccountOnRecall(bool enable)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cphDialog_chkEventsEnableCrossAccountRecall", enable, LocateBy.Id);
        }

        [Step]
        public void EnableRecall(bool enable)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cphDialog_chkEventsEnableRecall", enable, LocateBy.Id);
        }

        [Step]
        public void SetInvitationCodeAndList(string code, string list)
        {
            if (!UIUtilityProvider.UIHelper.IsChecked("ctl00_cphDialog_chkEventsInvitationOnly", LocateBy.Id))
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_chkEventsInvitationOnly", LocateBy.Id);
            }

            if (string.IsNullOrEmpty(code))
            {
                UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_txtEventsInvitationPassword", string.Empty, LocateBy.Id);
            }
            else
            {
                UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_txtEventsInvitationPassword", code, LocateBy.Id);
            }

            UIUtilityProvider.UIHelper.SelectWithText("ctl00_cphDialog_ddlEventsInvitationListId", list, LocateBy.Id);
        }

        #region Locators

        protected override string GetLocator<T>(T fieldEnum, LocatorType locatorType)
        {
            string locator = string.Empty;
            string enumString = fieldEnum.ToString();
            Type enumType = fieldEnum.GetType();

            if (enumType == typeof(AuthenticationField))
            {
                int enumInt = (int)Enum.Parse(typeof(AuthenticationField), enumString);
                locator = GetLocator((AuthenticationField)enumInt, locatorType);
            }

            return locator;
        }

        protected string GetLocator(AuthenticationField fieldType, LocatorType locatorType)
        {
            string locator = string.Empty;
            string baseLocator = "//div[@id='{0}']{1}";
            string baseField = "ct100_cphDialog_div{0}";
            string forField = string.Empty;
            string forType = string.Empty;
            InputType inputType = GetInputType(fieldType);

            switch (fieldType)
            {
                case AuthenticationField.InvitationOnly:
                case AuthenticationField.InvitationPassword:
                case AuthenticationField.InvitationList:
                    forField = string.Format(baseField, fieldType.ToString());
                    break;
                case AuthenticationField.RequirePassword:
                    forField = "ctl00_cphDialog_divRequirePassword";
                    break;
                case AuthenticationField.EnableXAccountRecall:
                    forField = "ctl00_cphDialog_divEnableXAccountRecall";
                    break;
                case AuthenticationField.AuthMethod:
                    forField = "ctl00_cphDialog_lblAuthMethod";
                    break;
                case AuthenticationField.NoDupEmails:
                    forField = "divNoDupEmails";
                    break;
                case AuthenticationField.EnableAutoUpdate:
                    forField = "divEnableAutoUpdate";
                    break;
            }

            switch (locatorType)
            {
                case LocatorType.Edit:
                    //Dropdowns are different!
                    if (inputType == InputType.Dropdown)
                        forType = "//select";
                    else
                        forType = "//input";
                    break;
                case LocatorType.Label:
                    //Checkbox labels are in different place
                    if (inputType == InputType.Checkbox)
                        forType = "";
                    else
                        forType = "/input";
                    break;
            }

            locator = string.Format(baseLocator, forField, forType);
            return locator;
        }
        #endregion

        #region Input types
        protected override InputType GetInputType<T>(T fieldEnum)
        {
            InputType inputType = new InputType();
            string enumString = fieldEnum.ToString();
            Type enumType = fieldEnum.GetType();

            if (enumType == typeof(AuthenticationField))
            {
                int enumInt = (int)Enum.Parse(typeof(AuthenticationField), enumString);
                inputType = GetInputType((AuthenticationField)enumInt);
            }

            return inputType;
        }

        private InputType GetInputType(AuthenticationField fieldType)
        {
            InputType inputType = new InputType();
            switch (fieldType)
            {
                case AuthenticationField.InvitationPassword:
                    inputType = InputType.Textbox;
                    break;
                case AuthenticationField.InvitationList:
                case AuthenticationField.AuthMethod:
                    inputType = InputType.Dropdown;
                    break;
                case AuthenticationField.InvitationOnly:
                case AuthenticationField.NoDupEmails:
                case AuthenticationField.EnableAutoUpdate:
                case AuthenticationField.RequirePassword:
                case AuthenticationField.EnableXAccountRecall:
                    inputType = InputType.Checkbox;
                    break;
            }

            return inputType;
        }
        #endregion
    }
}
