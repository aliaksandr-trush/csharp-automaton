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
            UIUtilityProvider.UIHelper.Type(this.NameOnFormTxtboxId, name, LocateBy.Id);
        }

        public void SelectType(string type)
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent(TypeLocator_Id, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(TypeLocator_Id, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(TypeDropDown, type), LocateBy.XPath);
        }

        public void SetOneLineLength(int length)
        {
            UIUtilityProvider.UIHelper.TypeRADNumericById(this.OneLineLengthLocator, length);
            ClearDatePopUp();
        }

        public void SetParagraphCharacterLimit(int charLimit)
        {
            UIUtilityProvider.UIHelper.TypeRADNumericById(this.ParagraphCharacterLimitLocator, charLimit.ToString());
        }

        public void SetGroupName(string name)
        {
            UIUtilityProvider.UIHelper.Type(this.GroupNameLocator, name, LocateBy.Id);
        }

        public void SetShowDate(DateTime showDate)
        {
            string date = string.Format("{0}/{1}/{2}", showDate.Month, showDate.Day, showDate.Year);

            // Set start date
            UIUtilityProvider.UIHelper.Type(this.ShowDateLocator, date, LocateBy.Id);
            this.ClearDatePopUp();
        }

        public void SetHideDate(DateTime hideDate)
        {
            string date = string.Format("{0}/{1}/{2}", hideDate.Month, hideDate.Day, hideDate.Year);

            // Set end date
            UIUtilityProvider.UIHelper.Type(this.HideDateLocator, date, LocateBy.Id);
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
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ClearDatePopUpLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void ExpandAdvanced()
        {
            UIUtilityProvider.UIHelper.ExpandAdvanced();
        }

        public void ExpandConditionalLogic()
        {
            while (UIUtilityProvider.UIHelper.IsElementPresent(ExpandConditionalLogicButtonLocator, LocateBy.XPath))
            {
                this.ClickExpandConditionalLogic();
            }
        }

        protected void ClickExpandConditionalLogic()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ExpandConditionalLogicButtonLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SetConditionalLogic(bool check, string conditionalItem)
        {
            string checkBox = string.Empty;
            string forLocator = ("//div[@id='" + this.ConditionalLogicDIVLocator + "']//*[text()='{0}']/../input");
            checkBox = string.Format(forLocator, conditionalItem);

            while (!UIUtilityProvider.UIHelper.IsElementPresent(checkBox, LocateBy.XPath))
            {
                this.ClickExpandConditionalLogic();
            }

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(checkBox, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            Utility.ThreadSleep(1.5);
        }

        protected void ClickShowHideAllRegTypes()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ShowHideAllRegTypesLinkLocator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void ShowAllRegTypes()
        {

            if (UIUtilityProvider.UIHelper.GetText(ShowHideAllRegTypesLinkLocator, LocateBy.Id).Equals("Show"))
            {
                this.ClickShowHideAllRegTypes();
            }
        }

        public void HideAllRegTypes()
        {

            if (UIUtilityProvider.UIHelper.GetText(ShowHideAllRegTypesLinkLocator, LocateBy.Id).Equals("Hide"))
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

            UIUtilityProvider.UIHelper.SetCheckbox(locator, check, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void VerifyVisibilityRegTypesVisible(bool expValue)
        {
            bool actValue;
            string errMssgFormat = "RegTypes visible = {0}";
            string styleFound = string.Empty;
            string locator = "//div[@id='" + this.AllRegTypesVisibilityDIVLocator + "']";

            actValue = !UIUtilityProvider.UIHelper.IsElementHidden(locator, LocateBy.XPath);

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
            UIUtilityProvider.UIHelper.VerifyElementPresent(locator, true, LocateBy.XPath);

            isChecked = UIUtilityProvider.UIHelper.IsChecked(locator, LocateBy.XPath);

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
            UIUtilityProvider.UIHelper.VerifyElementPresent(locator, true, LocateBy.XPath);

            isEnabled = UIUtilityProvider.UIHelper.IsEditable(locator, LocateBy.XPath);

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

            if (UIUtilityProvider.UIHelper.IsElementPresent(this.AllRegTypesVisibilityDIVLocator, LocateBy.Id))
            {
                regTypes = true;
            }

            return regTypes;
        }

        public void SelectFieldPosition(FieldPosition position)
        {
            UIUtilityProvider.UIHelper.SelectWithText(this.FieldPositionLocator, StringEnum.GetStringValue(position), LocateBy.Id);
        }

        public void CheckSeparatorLine(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cphDialog_cfCF_chkSeparator", check, LocateBy.Id);
        }

        public void AddDetailMessage(string message)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(CFDetailsLinkLocator, LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog2");
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(1);
            UIUtilityProvider.UIHelper.Type("//textarea", message + "<br>", LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog2");
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_btnSaveClose", LocateBy.Id);
            Utility.ThreadSleep(3);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
        }

        public void AddDetailURL(string url)
        {
            UIUtilityProvider.UIHelper.Type(CFDetailsURLLocator, url, LocateBy.Id);
        }

        public void EnterAgeGenderValidation(Register.RegisterManager.Gender? gender, string ageGreaterThan, 
            string ageGreaterThanDate, string ageLessThan, string ageLessThanDate)
        {
            UIUtilityProvider.UIHelper.ExpandAdvanced();
            if (gender != null)
            {
                UIUtilityProvider.UIHelper.SelectWithText("ctl00_cph_ucCF_ddlVisibleForGender", gender.ToString(), LocateBy.Id);
            }
            if (!string.IsNullOrEmpty(ageGreaterThan))
            {
                UIUtilityProvider.UIHelper.Type("ctl00_cph_ucCF_rntVisibleIfAgeGreater_text", ageGreaterThan, LocateBy.Id);
                UIUtilityProvider.UIHelper.Type("ctl00_cph_ucCF_dtpVisibleIfAgeGreater_tbDate", ageGreaterThanDate, LocateBy.Id);
            }
            if (!string.IsNullOrEmpty(ageLessThan))
            {
                UIUtilityProvider.UIHelper.Type("ctl00_cph_ucCF_rntVisibleIfAgeLess_text", ageLessThan, LocateBy.Id);
                UIUtilityProvider.UIHelper.Type("ctl00_cph_ucCF_dtpVisibleIfAgeLess_tbDate", ageLessThanDate, LocateBy.Id);

            }
        }
    }
}
