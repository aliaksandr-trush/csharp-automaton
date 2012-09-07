namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using NUnit.Framework; 
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public abstract class CFManagerBase : ManagerBase
    {
        #region Constants
        protected const string AddMultiChoiceItemLinkLocator = "Add Multiple Choice Item";
        protected const string AddCommonlyUsedItemsLinkLocator = "Add Commonly-Used Items";
        protected const string PredefinedType = "//div[@id='divPredefinedTypes']//div[text()='{0}']";
        protected const string TypeDropDown = "//div[@id='divMoreFormats']//span[text()='{0}']";
        protected const string ShowHideAllRegTypesLinkLocator = "spanDisplayAll";
        public const string VisibilityAllRegTypes = "All";
        protected const string VisibilityErrorExpectedMessage = "Option {0} for RegType {1}";
        protected const string VisibilityErrorActualMessage = "Did not find option for that reg type!";
        protected const string LimitReachedMessage = "Sorry, but the custom field have no more capacity!";
        protected const string ClearDatePopUpLocator = "//*[@id='dvFieldType']/..";
        protected const int DefaultOneLineNumericLength = 5;
        protected const int DefaultOneLineTextLength = 50;
        protected const int DefaultParagraphCharacterLimit = 20000;
        protected readonly string ExpandConditionalLogicButtonLocator;
        protected const string PrePopulateGroupSelectionLocator = "ctl00_cphDialog_cfCF_chkEnablePrePopulate";
        protected const string AllowGroupSelectionEditingLocator = "ctl00_cphDialog_cfCF_chkEnableEditing";
        protected const string OptionsLinkLocator = "ctl00_cphDialog_cfCF_mipCap_MoreInfoButtonCapacity_optionsLink";
        protected const string ShowRemainingCapacityLocator = "ctl00_cphDialog_cfCF_mipCap_ip7_cbShowRemainingCapacity";
        protected const string HideItemWhenLimitReachedLocator = "ctl00_cphDialog_cfCF_mipCap_ip7_rbHideThisItem";
        protected const string ShowMessageWhenLimitReachedLocator = "ctl00_cphDialog_cfCF_mipCap_ip7_rbDisplayThisMessage";
        protected const string AddLimitReachedMessageLocator = "ctl00_cphDialog_cfCF_mipCap_ip7_elSoldOutMessage_linkCheckmarkCustomField0SoldOutMessage";
        protected const string SpacesAvailableLocator = "ctl00_cphDialog_cfCF_mipCap_rntMaxQuantity_text";
        protected const string CFDetailsLinkLocator = "ctl00_cphDialog_cfCF_elSessionInfo_linkCheckmarktext_elSessionInfo";
        protected const string CFDetailsURLLocator = "ctl00_cphDialog_cfCF_txtSessionLink";
        #endregion

        #region Enum
        public enum VisibilityOption
        {
            Visible,
            Required,
            Admin
        }

        public enum FieldPosition
        {
            [StringValue("Below Name")]
            BelowName,

            [StringValue("Right of Name")]
            RightOfName,

            [StringValue("Left of Name")]
            LeftOfName,

            [StringValue("Above Name")]
            AboveName,
        }
        #endregion

        #region Properties

        #region Locators definitions
        protected abstract string NameOnFormTxtboxId { get; }

        protected abstract string TypeLocator_Id { get; }

        protected abstract string AllRegTypesVisibilityDIVLocator { get; }

        protected abstract string ConditionalLogicDIVLocator { get; }

        protected abstract string FieldPositionLocator { get; }

        protected abstract string OneLineLengthLocator { get; }

        protected abstract string ParagraphCharacterLimitLocator { get; }

        protected abstract string GroupNameLocator { get; }

        protected abstract string RegTypeRowLocatorPrefix { get; }

        protected abstract string VisibleToAllLocator { get; }

        protected abstract string RequiredByAllLocator { get; }

        protected abstract string AdminOnlyToAllLocator { get; }

        protected abstract string VisibleToRegTypePrefix { get; }

        protected abstract string RequiredByRegTypePrefix { get; }

        protected abstract string AdminOnlyToRegTypePrefix { get; }

        protected abstract string ShowDateLocator { get; }

        protected abstract string HideDateLocator { get; }
        #endregion

        #endregion

        public CFManagerBase()
        {
            this.ExpandConditionalLogicButtonLocator = "//div[@id='" + this.ConditionalLogicDIVLocator + "']//span[@class='rtPlus']";
        }

        public void SetName(string name)
        {
            WebDriverUtility.DefaultProvider.Type(this.NameOnFormTxtboxId, name, LocateBy.Id);
        }

        public void SelectType(string type)
        {
            WebDriverUtility.DefaultProvider.WaitForElementPresent(TypeLocator_Id, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(TypeLocator_Id, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(string.Format(TypeDropDown, type), LocateBy.XPath);
        }

        public void SetOneLineLength(int length)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById(this.OneLineLengthLocator, length);
            ClearDatePopUp();
        }

        public void SetParagraphCharacterLimit(int charLimit)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById(this.ParagraphCharacterLimitLocator, charLimit.ToString());
        }

        public void SetGroupName(string name)
        {
            WebDriverUtility.DefaultProvider.Type(this.GroupNameLocator, name, LocateBy.Id);
        }

        public void SetShowDate(DateTime showDate)
        {
            string date = string.Format("{0}/{1}/{2}", showDate.Month, showDate.Day, showDate.Year);

            // Set start date
            WebDriverUtility.DefaultProvider.Type(this.ShowDateLocator, date, LocateBy.Id);
            this.ClearDatePopUp();
        }

        public void SetHideDate(DateTime hideDate)
        {
            string date = string.Format("{0}/{1}/{2}", hideDate.Month, hideDate.Day, hideDate.Year);

            // Set end date
            WebDriverUtility.DefaultProvider.Type(this.HideDateLocator, date, LocateBy.Id);
            this.ClearDatePopUp();
        }

        public void SetShowHideDate(DateTime showDate, DateTime hideDate)
        {
            this.SetShowDate(showDate);
            this.SetHideDate(hideDate);
            this.ClearDatePopUp();
        }

        public void ClearDatePopUp()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(ClearDatePopUpLocator, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void ExpandAdvanced()
        {
            WebDriverUtility.DefaultProvider.ExpandAdvanced();
        }

        public void ExpandConditionalLogic()
        {
            while (WebDriverUtility.DefaultProvider.IsElementPresent(ExpandConditionalLogicButtonLocator, LocateBy.XPath))
            {
                this.ClickExpandConditionalLogic();
            }
        }

        protected void ClickExpandConditionalLogic()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(ExpandConditionalLogicButtonLocator, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetConditionalLogic(bool check, string conditionalItem)
        {
            string checkBox = string.Empty;
            string forLocator = ("//div[@id='" + this.ConditionalLogicDIVLocator + "']//*[text()='{0}']/../input");
            checkBox = string.Format(forLocator, conditionalItem);

            while (!WebDriverUtility.DefaultProvider.IsElementPresent(checkBox, LocateBy.XPath))
            {
                this.ClickExpandConditionalLogic();
            }

            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(checkBox, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            Utility.ThreadSleep(1.5);
        }

        protected void ClickShowHideAllRegTypes()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(ShowHideAllRegTypesLinkLocator, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void ShowAllRegTypes()
        {

            if (WebDriverUtility.DefaultProvider.GetText(ShowHideAllRegTypesLinkLocator, LocateBy.Id).Equals("Show"))
            {
                this.ClickShowHideAllRegTypes();
            }
        }

        public void HideAllRegTypes()
        {

            if (WebDriverUtility.DefaultProvider.GetText(ShowHideAllRegTypesLinkLocator, LocateBy.Id).Equals("Hide"))
            {
                this.ClickShowHideAllRegTypes();
            }
        }

        public void SetVisibilityOption(bool check, VisibilityOption option)
        {
            this.SetVisibilityOption(check, option, VisibilityAllRegTypes);
        }

        public void SetVisibilityOption(bool check, VisibilityOption option, string regTypeName)
        {
            string locator = this.GetLocatorVisibilityOption(option, regTypeName);

            WebDriverUtility.DefaultProvider.SetCheckbox(locator, check, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void VerifyVisibilityRegTypesVisible(bool expValue)
        {
            bool actValue;
            string errMssgFormat = "RegTypes visible = {0}";
            string styleFound = string.Empty;
            string locator = "//div[@id='" + this.AllRegTypesVisibilityDIVLocator + "']";

            actValue = !WebDriverUtility.DefaultProvider.IsElementHidden(locator, LocateBy.XPath);

            Assert.AreEqual(string.Format(errMssgFormat, expValue), string.Format(errMssgFormat, actValue));
        }

        public void VerifyValueVisibilityOption(VisibilityOption option, bool expValue)
        {
            this.VerifyValueVisibilityOption(option, VisibilityAllRegTypes, expValue);
        }

        public void VerifyValueVisibilityOption(VisibilityOption option, string regTypeName, bool expValue)
        {
            bool actValue = this.GetValueVisibilityOption(option, regTypeName);
            string mssgFormat = "{0} checked = {1}";
            string expMssg = string.Format(mssgFormat, option.ToString(), expValue.ToString());
            string actMssg = string.Format(mssgFormat, option.ToString(), actValue.ToString());

            Assert.AreEqual(expMssg, actMssg);
        }

        public void VerifyEnabledVisibilityOption(VisibilityOption option, bool expValue)
        {
            this.VerifyEnabledVisibilityOption(option, VisibilityAllRegTypes, expValue);
        }

        public void VerifyEnabledVisibilityOption(VisibilityOption option, string regTypeName, bool expValue)
        {
            bool actValue = this.GetEnabledVisibilityOption(option, regTypeName);
            string mssgFormat = "{0} checked = {1}";
            string expMssg = string.Format(mssgFormat, option.ToString(), expValue.ToString());
            string actMssg = string.Format(mssgFormat, option.ToString(), actValue.ToString());

            Assert.AreEqual(expMssg, actMssg);
        }

        public bool GetValueVisibilityOption(VisibilityOption option)
        {
            return this.GetValueVisibilityOption(option, VisibilityAllRegTypes);
        }

        public bool GetValueVisibilityOption(VisibilityOption option, string regTypeName)
        {
            bool isChecked = false;
            string locator = this.GetLocatorVisibilityOption(option, regTypeName);

            ////UIUtilityProvider.UIHelper.VerifyElementPresent(locator, VisibilityErrorExpectedMessage, VisibilityErrorActualMessage);
            WebDriverUtility.DefaultProvider.VerifyElementPresent(locator, true, LocateBy.XPath);

            isChecked = WebDriverUtility.DefaultProvider.IsChecked(locator, LocateBy.XPath);

            return isChecked;
        }

        public bool GetEnabledVisibilityOption(VisibilityOption option)
        {
            return this.GetEnabledVisibilityOption(option, VisibilityAllRegTypes);
        }

        public bool GetEnabledVisibilityOption(VisibilityOption option, string regTypeName)
        {
            bool isEnabled = false;
            string locator = this.GetLocatorVisibilityOption(option, regTypeName);

            ////UIUtilityProvider.UIHelper.VerifyElementPresent(locator, VisibilityErrorExpectedMessage, VisibilityErrorActualMessage);
            WebDriverUtility.DefaultProvider.VerifyElementPresent(locator, true, LocateBy.XPath);

            isEnabled = WebDriverUtility.DefaultProvider.IsEditable(locator, LocateBy.XPath);

            return isEnabled;
        }

        protected string GetLocatorVisibilityOption(VisibilityOption option)
        {
            return this.GetLocatorVisibilityOption(option, VisibilityAllRegTypes);
        }

        protected string GetLocatorVisibilityOption(VisibilityOption option, string regTypeName)
        {
            string locator = string.Empty;
            string allLoc = "//input[@id='{0}']";
            string regTypeLoc = "//tr[contains(@id,'" + this.RegTypeRowLocatorPrefix + "')]/td[text()='{0}']/../td/input[contains(@id,'{1}')]";
            string inputID = string.Empty;

            if (regTypeName == VisibilityAllRegTypes)
            {
                switch (option)
                {
                    case VisibilityOption.Visible:
                        inputID = this.VisibleToAllLocator;
                        break;
                    case VisibilityOption.Required:
                        inputID = this.RequiredByAllLocator;
                        break;
                    case VisibilityOption.Admin:
                        inputID = this.AdminOnlyToAllLocator;
                        break;
                }

                locator = string.Format(allLoc, inputID);
            }
            else
            {
                switch (option)
                {
                    case VisibilityOption.Visible:
                        inputID = this.VisibleToRegTypePrefix;
                        break;
                    case VisibilityOption.Required:
                        inputID = this.RequiredByRegTypePrefix;
                        break;
                    case VisibilityOption.Admin:
                        inputID = this.AdminOnlyToRegTypePrefix;
                        break;
                }

                locator = string.Format(regTypeLoc, regTypeName, inputID);
            }

            return locator;
        }

        public bool HasVisibilityRegTypes()
        {
            bool regTypes = false;

            if (WebDriverUtility.DefaultProvider.IsElementPresent(this.AllRegTypesVisibilityDIVLocator, LocateBy.Id))
            {
                regTypes = true;
            }

            return regTypes;
        }

        public void SelectFieldPosition(FieldPosition position)
        {
            WebDriverUtility.DefaultProvider.SelectWithText(this.FieldPositionLocator, StringEnum.GetStringValue(position), LocateBy.Id);
        }

        public void CheckSeparatorLine(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cphDialog_cfCF_chkSeparator", check, LocateBy.Id);
        }

        public void AddDetailMessage(string message)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(CFDetailsLinkLocator, LocateBy.Id);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog2");
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SelectIFrameOnCurrentIFrame(1);
            WebDriverUtility.DefaultProvider.Type("//textarea", message + "<br>", LocateBy.XPath);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog2");
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_btnSaveClose", LocateBy.Id);
            Utility.ThreadSleep(3);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
        }

        public void AddDetailURL(string url)
        {
            WebDriverUtility.DefaultProvider.Type(CFDetailsURLLocator, url, LocateBy.Id);
        }

        public void EnterAgeGenderValidation(Register.RegisterManager.Gender? gender, string ageGreaterThan, 
            string ageGreaterThanDate, string ageLessThan, string ageLessThanDate)
        {
            WebDriverUtility.DefaultProvider.ExpandAdvanced();
            if (gender != null)
            {
                WebDriverUtility.DefaultProvider.SelectWithText("ctl00_cph_ucCF_ddlVisibleForGender", gender.ToString(), LocateBy.Id);
            }
            if (!string.IsNullOrEmpty(ageGreaterThan))
            {
                WebDriverUtility.DefaultProvider.Type("ctl00_cph_ucCF_rntVisibleIfAgeGreater_text", ageGreaterThan, LocateBy.Id);
                WebDriverUtility.DefaultProvider.Type("ctl00_cph_ucCF_dtpVisibleIfAgeGreater_tbDate", ageGreaterThanDate, LocateBy.Id);
            }
            if (!string.IsNullOrEmpty(ageLessThan))
            {
                WebDriverUtility.DefaultProvider.Type("ctl00_cph_ucCF_rntVisibleIfAgeLess_text", ageLessThan, LocateBy.Id);
                WebDriverUtility.DefaultProvider.Type("ctl00_cph_ucCF_dtpVisibleIfAgeLess_tbDate", ageLessThanDate, LocateBy.Id);

            }
        }
    }
}
