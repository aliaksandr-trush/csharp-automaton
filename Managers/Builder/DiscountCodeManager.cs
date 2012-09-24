namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Text;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class DiscountCodeManager : ManagerBase
    {
        #region Constants
        private const string CodeDetailDialogID = "dialog3";
        private const string BulkCodesTxtareaLocator = "ctl00_cphDialog_txtBulkCodes";
        private const string CodeLinkLocatorFormat = "//table[@id='tblCodes']//a[contains(text(), '{0}')]";
        private const string DeleteCodeLinkLocatorFormat = "//a[contains(text(), '{0}')]/../../td[5]/a";

        private const string EventfeeCodeLabelOnFormTxtboxLocator = "ctl00_cphDialog_cfCF_mipPrc_ip4_txtDiscountCodeTitle";
        private const string EventfeeCodeRequiredCheckboxLocator = "ctl00_cphDialog_cfCF_mipPrc_ip4_chkPasswordRequired";
        private const string EventfeeAddCodeLinkLocator = "ctl00_cphDialog_cfCF_mipPrc_ip4_hrefOpenCodeWindow";
        private const string EventfeeCodeDialogID = "dialog3";
        private const string EventfeeBulkLoadCodesLinkLocator = "ctl00_cphDialog_cfCF_mipPrc_ip4_hrefOpenBulkCodesWindow";
        private const string EventfeeBulkLoadCodesDialogID = "dialog3";

        private const string AgendaCodeLabelOnFormTxtboxLocator = "ctl00_cph_ucCF_mipPrc_ip4_txtDiscountCodeTitle";
        private const string AgendaCodeRequiredCheckboxLocator = "ctl00_cph_ucCF_mipPrc_ip4_chkPasswordRequired";
        private const string AgendaAddCodeLinkLocator = "ctl00_cph_ucCF_mipPrc_ip4_hrefOpenCodeWindow";
        private const string AgendaCodeDialogID = "dialog";
        private const string AgendaBulkLoadCodesLinkLocator = "ctl00_cph_ucCF_mipPrc_ip4_hrefOpenBulkCodesWindow";
        private const string AgendaBulkLoadCodesDialogID = "dialog";
        #endregion

        #region Locator variables needed to be initialized
        private string codeLabelOnFormTxtboxLocator;
        private string codeRequiredCheckboxLocator;
        private string addCodeLinkLocator;
        private string codeDialogID;
        private string bulkLoadCodesLinkLocator;
        private string bulkLoadCodesDialogID;
        #endregion

        private FormDetailManager.Page builderPage;
        public FormDetailManager.FeeLocation FeeLocationOfStartPage;

        #region Constructor
        public DiscountCodeManager(FormDetailManager.Page page) 
        {
            this.builderPage = page;
            InitializeLocators();
        }
        #endregion

        #region Initialize locators
        private void InitializeLocators()
        {
            switch (this.builderPage)
            {
                case FormDetailManager.Page.Start:
                    codeLabelOnFormTxtboxLocator = EventfeeCodeLabelOnFormTxtboxLocator;
                    codeRequiredCheckboxLocator = EventfeeCodeRequiredCheckboxLocator;
                    addCodeLinkLocator = EventfeeAddCodeLinkLocator;
                    codeDialogID = EventfeeCodeDialogID;
                    bulkLoadCodesLinkLocator = EventfeeBulkLoadCodesLinkLocator;
                    bulkLoadCodesDialogID = EventfeeBulkLoadCodesDialogID;
                    break;

                case FormDetailManager.Page.Agenda:
                    codeLabelOnFormTxtboxLocator = AgendaCodeLabelOnFormTxtboxLocator;
                    codeRequiredCheckboxLocator = AgendaCodeRequiredCheckboxLocator;
                    addCodeLinkLocator = AgendaAddCodeLinkLocator;
                    codeDialogID = AgendaCodeDialogID;
                    bulkLoadCodesLinkLocator = AgendaBulkLoadCodesLinkLocator;
                    bulkLoadCodesDialogID = AgendaBulkLoadCodesDialogID;
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Enum
        public enum DiscountCodeType
        {
            DiscountCode,
            AccessCode
        }

        public enum ChangePriceDirection
        {
            Decrease,
            Increase
        }

        public enum ChangeType
        {
            Percent,
            FixedAmount
        }
        #endregion

        #region Public methods

        #region Methods on the start and agenda page
        public bool HasDiscountCode(string codeName)
        {
            return UIUtil.DefaultProvider.IsElementPresent(ComposeCodeLinkLocator(codeName), LocateBy.XPath);
        }

        public bool HasDiscountCodeErrors()
        {
            try
            {
                UIUtil.DefaultProvider.WaitForElementPresent("//div[@id = 'ctl00_valSummary']/ul/li", LocateBy.XPath);
            }
            catch { /*ignore*/ }

            return UIUtil.DefaultProvider.IsElementPresent("//div[@id = 'ctl00_valSummary']/ul/li", LocateBy.XPath);
        }

        [Step]
        public void ClickAddCode()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(addCodeLinkLocator, LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.WaitForAJAXRequest();

            if(builderPage == FormDetailManager.Page.Start)
            {
                SelectBuilderWindow();
            }

            UIUtil.DefaultProvider.SelectPopUpFrameByName(codeDialogID);
        }

        private void SelectCodeFrame()
        {
            try
            {
                UIUtil.DefaultProvider.SelectPopUpFrameByName(codeDialogID);
            }
            catch
            {
                // Ignore
            }
        }

        public void OpenCode(string codeName)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(ComposeCodeLinkLocator(codeName), LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectUpperFrame();
            UIUtil.DefaultProvider.SelectPopUpFrameByName(codeDialogID); ;
        }

        [Step]
        public void SaveAndNewDiscountCode()
        {
            this.SelectCodeFrame();
            UIUtil.DefaultProvider.ClickSaveAndNew();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
            UIUtil.DefaultProvider.SwitchToMainContent();
            this.SelectCodeFrame();
        }

        [Step]
        public void SaveAndCloseDiscountCode()
        {
            switch (this.builderPage)
            {
                case FormDetailManager.Page.Start:
                    SelectCodeFrame();
                    UIUtil.DefaultProvider.ClickSaveAndClose();
                    Utility.ThreadSleep(1.5);
                    UIUtil.DefaultProvider.SelectOriginalWindow();
                    UIUtil.DefaultProvider.SwitchToMainContent();

                    if (this.FeeLocationOfStartPage == FormDetailManager.FeeLocation.Event)
                    {
                        UIUtil.DefaultProvider.SelectPopUpFrameByName(EventFeeManager.FeeAdvancedFrameIDInEventFee);
                    }
                    else
                    {
                        UIUtil.DefaultProvider.SelectPopUpFrameByName(EventFeeManager.FeeAdvancedFrameIDInRegType);
                    }

                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                case FormDetailManager.Page.Agenda:
                    SelectCodeFrame();
                    UIUtil.DefaultProvider.ClickSaveAndClose();
                    Utility.ThreadSleep(1.5);
                    SelectBuilderWindow();
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                default:
                    break;
            }
        }

        public void CancelDiscountCode()
        {
            switch (this.builderPage)
            {
                case FormDetailManager.Page.Start:
                    SelectCodeFrame();
                    UIUtil.DefaultProvider.ClickCancel();
                    Utility.ThreadSleep(1.5);
                    UIUtil.DefaultProvider.SelectOriginalWindow();
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                case FormDetailManager.Page.Agenda:
                    SelectCodeFrame();
                    UIUtil.DefaultProvider.ClickCancel();
                    Utility.ThreadSleep(1.5);
                    SelectBuilderWindow();
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                default:
                    break;
            }
        }

        private void SelectBulkLoadCodesFrame()
        {
            try
            {
                UIUtil.DefaultProvider.SelectPopUpFrameByName(bulkLoadCodesDialogID);
            }
            catch
            {
                // Ignore
            }
        }

        public void ClickBulkLoadCodes()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(bulkLoadCodesLinkLocator, LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SaveAndStayBulkLoadCodes()
        {
            SelectBulkLoadCodesFrame();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.ClickSaveAndStay();
        }

        public void SaveAndCloseBulkLoadCodes()
        {
            switch (this.builderPage)
            {
                case FormDetailManager.Page.Start:
                    SelectBulkLoadCodesFrame();
                    UIUtil.DefaultProvider.ClickSaveAndClose();
                    Utility.ThreadSleep(1);
                    UIUtil.DefaultProvider.SelectOriginalWindow();
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                case FormDetailManager.Page.Agenda:
                    SelectBulkLoadCodesFrame();
                    UIUtil.DefaultProvider.ClickSaveAndClose();
                    Utility.ThreadSleep(1);
                    SelectBuilderWindow();
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                default:
                    break;
            }
        }

        public void CancelBulkLoadCodes()
        {
            switch (this.builderPage)
            {
                case FormDetailManager.Page.Start:
                    SelectBulkLoadCodesFrame();
                    UIUtil.DefaultProvider.ClickCancel();
                    Utility.ThreadSleep(1);
                    UIUtil.DefaultProvider.SelectOriginalWindow();
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                case FormDetailManager.Page.Agenda:
                    SelectBulkLoadCodesFrame();
                    UIUtil.DefaultProvider.ClickCancel();
                    Utility.ThreadSleep(1);
                    SelectBuilderWindow();
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                default:
                    break;
            }
        }

        public void DeleteCode(string codeName, bool doCancel)
        {
            if (doCancel)
            {
                UIUtil.DefaultProvider.ChooseCancelOnNextConfirmation();
            }

            UIUtil.DefaultProvider.WaitForDisplayAndClick(ComposeDeleteCodeLinkLocator(codeName), LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.GetConfirmation();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void DeleteCode(string codeName)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(ComposeDeleteCodeLinkLocator(codeName), LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.GetConfirmation();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public double CalculateDiscountPrice(double standardPrice, 
            ChangePriceDirection changePriceDirection, ChangeType changeType, double discountAmount)
        {
            switch (changePriceDirection)
            {
                case ChangePriceDirection.Increase:
                    break;

                case ChangePriceDirection.Decrease:
                    discountAmount = -discountAmount;
                    break;

                default:
                    break;
            }

            double discountPrice = standardPrice;

            switch (changeType)
            {
                case ChangeType.FixedAmount:
                    discountPrice += discountAmount;
                    break;

                case ChangeType.Percent:
                    discountPrice = discountPrice * (100 + discountAmount) / 100;
                    break;

                default:
                    break;
            }

            return discountPrice;
        }

        [Step]
        public void SetDiscountCodeValues(string code, ChangePriceDirection changePriceDirection, 
            ChangeType changeType, double discountPrice, int? limitNumber)
        {
            this.SetCodeType(DiscountCodeType.DiscountCode);

            // Set Change Price By Direction: Decrease or Increase
            this.SetChangeDirection(changePriceDirection);

            // Set Change Type : percent or fixed amount
            this.SetChangeType(changeType);

            // Set Change Price
            this.SetDiscountChangePrice(discountPrice);

            // Set Code Name
            this.SetCodeName(code);

            // Set Limit Number
            this.SetCodeLimitNumber(limitNumber);
        }

        public void SetDiscountCodeAndAddAnother(string code, ChangePriceDirection changePriceDirection, 
            ChangeType changeType, double discountPrice, int? limitNumber)
        {
            this.SetDiscountCodeValues(code, changePriceDirection, changeType, discountPrice, limitNumber);
            this.SaveAndNewDiscountCode();
        }

        [Step]
        public void SetAccessCodeValues(string code, int? limitNumber)
        {
            this.SetCodeType(DiscountCodeType.AccessCode);
            this.SetCodeName(code);
            this.SetCodeLimitNumber(limitNumber);
        }

        public void SetAccessCodeAndAddAnother(string code, int? limitNumber)
        {
            this.SetAccessCodeValues(code, limitNumber);
            this.SaveAndNewDiscountCode();
        }

        public void SetCodeAndAddAnother(DiscountCodeType discountCodeType, string code, 
            ChangePriceDirection changePriceDirection, ChangeType changeType, 
            double discountPrice, int? limitNumber)
        {
            if (discountCodeType == DiscountCodeType.DiscountCode)
            {
                this.SetDiscountCodeAndAddAnother(code, changePriceDirection, changeType, discountPrice, limitNumber);
            }
            else
            {
                this.SetAccessCodeAndAddAnother(code, limitNumber);
            }
        }

        public void SetCodeType(DiscountCodeType type)
        {
            switch (type)
            {
                case DiscountCodeType.DiscountCode:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_rblCodeTypes_0", LocateBy.Id);
                    break;
                case DiscountCodeType.AccessCode:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_rblCodeTypes_1", LocateBy.Id);
                    break;
                default:
                    break;
            }
        }

        public void SetChangeDirection(ChangePriceDirection changePriceDirection)
        {
            UIUtil.DefaultProvider.SelectWithText("ctl00_cphDialog_ddlChangePriceByDirection", changePriceDirection.ToString(), LocateBy.Id);
        }

        public void SetChangeType(ChangeType changeType)
        {
            if (changeType == ChangeType.Percent)
            {
                UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_rblChangeTypePercent", LocateBy.Id);
            }
            else
            {
                UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_rblChangeTypeFixed", LocateBy.Id);
            }
        }

        public void SetDiscountChangePrice(double discountPrice)
        {
            string priceTxtboxLocator = "ctl00_cphDialog_radNChangePriceBy";
            UIUtil.DefaultProvider.TypeRADNumericById(priceTxtboxLocator, Convert.ToString(discountPrice));
        }

        public void SetCodeName(string code)
        {
            string nameTxtboxLocator = "ctl00_cphDialog_txtDiscountCodeTitle";
            UIUtil.DefaultProvider.Type(nameTxtboxLocator, code, LocateBy.Id);
        }

        public void SetCodeLimitNumber(int? limitNumber)
        {
            string limitTxtboxLocator = "ctl00_cphDialog_radNCodeUseLimit";
            UIUtil.DefaultProvider.TypeRADNumericById(limitTxtboxLocator, Convert.ToString(limitNumber));
        }

        public void SetCodeLabelOnForm(string label)
        {
            UIUtil.DefaultProvider.Type(codeLabelOnFormTxtboxLocator, label, LocateBy.Id);
        }

        public void SetCodeRequired(bool check)
        {
            UIUtil.DefaultProvider.SetCheckbox(codeRequiredCheckboxLocator, check, LocateBy.Id);
        }

        public void AppendCodeToBulkLoadCodes(string codeString)
        {
            // Get existing code string
            // Be aware! Do not use GetText() here, or we cannot get the current code string in the textarea!
            string existingCodeString = UIUtil.DefaultProvider.GetValue(BulkCodesTxtareaLocator, LocateBy.Id);
            UIUtil.DefaultProvider.Type(BulkCodesTxtareaLocator, existingCodeString + "," + codeString, LocateBy.Id);
        }

        public void SetBulkLoadCodes(string codeString)
        {
            UIUtil.DefaultProvider.Type(BulkCodesTxtareaLocator, codeString, LocateBy.Id);
        }

        public void ClearBulkLoadCodes()
        {
            UIUtil.DefaultProvider.Type(BulkCodesTxtareaLocator, string.Empty, LocateBy.Id);
        }

        public string GetBulkLoadCodeStringForDiscountCode(string code, ChangePriceDirection changePriceDirection,
            ChangeType changeType, double discountPrice, int? limitNumber)
        {
            StringBuilder codeString = new StringBuilder();
            codeString.Append(code);
            codeString.Append("=");

            if (changePriceDirection == ChangePriceDirection.Decrease)
            {
                codeString.Append("-");
            }

            codeString.Append(Convert.ToString(discountPrice));

            if (changeType == ChangeType.Percent)
            {
                codeString.Append("%");
            }

            if (limitNumber.HasValue)
            {
                codeString.Append("(" + limitNumber.Value.ToString() + ")");
            }

            return codeString.ToString();
        }

        public string GetBulkLoadCodeStringForAccessCode(string code, int? limitNumber)
        {
            StringBuilder codeString = new StringBuilder();
            codeString.Append(code);

            if (limitNumber.HasValue)
            {
                codeString.Append("(" + limitNumber.Value.ToString() + ")");
            }

            return codeString.ToString();
        }

        #region Verify methods
        public void VerifyHasErrors(bool hasErrors)
        {
            VerifyTool.VerifyValue(hasErrors, HasDiscountCodeErrors(), "HasDiscountCodeErrors = {0}");
        }

        public void VerifyDiscountCodeInList(bool hasCode, string codeName, ChangePriceDirection changePriceDirection, ChangeType changeType, 
            double discountPrice, int? limitNumber)
            // We must enter the discount price, however, limit number can be null
        {
            string codeLinkLocator = ComposeCodeLinkLocator(codeName);

            if (hasCode != UIUtil.DefaultProvider.IsElementPresent(codeLinkLocator, LocateBy.XPath))
            {
                Assert.Fail(string.Format("DiscountCode '{0}' was{1} found in the code list!", codeName, hasCode ? " NOT" : string.Empty));
            }

            VerifyCodeTypeAndName(DiscountCodeType.DiscountCode, codeName);
            VerifyCodeChangeAmount(codeName, changePriceDirection, changeType, discountPrice);
            VerifyCodeLimitNumber(codeName, limitNumber);
        }

        public void VerifyAccessCodeInList(bool hasCode, string codeName, int? limitNumber)
        {
            string codeLinkLocator = ComposeCodeLinkLocator(codeName);

            if (hasCode != UIUtil.DefaultProvider.IsElementPresent(codeLinkLocator, LocateBy.XPath))
            {
                Assert.Fail(string.Format("AccessCode '{0}' was{1} found in the code list!", codeName, hasCode ? " NOT" : string.Empty));
            }

            VerifyCodeTypeAndName(DiscountCodeType.AccessCode, codeName);
            VerifyCodeLimitNumber(codeName, limitNumber);
        }

        public void VerifyDiscountCodeInBulkLoadCodes(bool hasCode, string codeName, 
            ChangePriceDirection changePriceDirection, ChangeType changeType, 
            double discountPrice, int? limitNumber)
        {
            UIUtil.DefaultProvider.WaitForElementPresent(BulkCodesTxtareaLocator, LocateBy.Id);
            bool actual = UIUtil.DefaultProvider.GetText(BulkCodesTxtareaLocator, LocateBy.Id).Contains(
                GetBulkLoadCodeStringForDiscountCode(codeName, changePriceDirection, changeType, discountPrice, limitNumber));

            if (hasCode != actual)
            {
                Assert.Fail(string.Format("DiscountCode '{0}' was{1} found in bulk codes!", codeName, hasCode ? " NOT" : string.Empty));
            }
        }

        public void VerifyAccessCodeInBulkLoadCodes(bool hasCode, string codeName, int? limitNumber)
        {
            UIUtil.DefaultProvider.WaitForElementPresent(BulkCodesTxtareaLocator, LocateBy.Id);
            bool actual = UIUtil.DefaultProvider.GetText(BulkCodesTxtareaLocator, LocateBy.Id).Contains(GetBulkLoadCodeStringForAccessCode(codeName, limitNumber));

            if (hasCode != actual)
            {
                Assert.Fail(string.Format("AccessCode '{0}' was{1} found in bulk codes!", codeName, hasCode ? " NOT" : string.Empty));
            }
        }

        public void VerifyCodeTypeAndName(DiscountCodeType codeType, string codeName)
        {
            string codeLinkLocator = ComposeCodeLinkLocator(codeName);

            //Get the code type title attribute locator
            string codeTypeTitleLocatorFormat = "{0}/../../td[1]";
            string codeTypeTitleLocator = string.Format(codeTypeTitleLocatorFormat, codeLinkLocator);

            //Get the code type title attribute and verify whether it matches the right type
            string codeTypeTitle = UIUtil.DefaultProvider.GetAttribute(codeTypeTitleLocator, "title", LocateBy.XPath);
            if (codeType == DiscountCodeType.DiscountCode)
            {
                VerifyTool.VerifyValue("Discount code", codeTypeTitle, "The code type is : {0}");
            }
            else
            {
                VerifyTool.VerifyValue("Access code", codeTypeTitle, "The code type is : {0}");
            }

            VerifyTool.VerifyValue(codeName, UIUtil.DefaultProvider.GetText(codeLinkLocator, LocateBy.XPath), "The code name is : {0}");
        }

        public void VerifyCodeChangeAmount(string codeName, ChangePriceDirection changePriceDirection, ChangeType changeType, double discountPrice)
        {
            string codeLinkLocator = ComposeCodeLinkLocator(codeName);

            //Verify code ChangePriceDirection(decrease or increase) and DiscountPrice and ChangeType(Percentage or FixedAmount)
            string codeChangeAmountLocator = string.Format("{0}/../../td[3]", codeLinkLocator);
            StringBuilder expectedAmount = new StringBuilder();

            if (changePriceDirection == ChangePriceDirection.Decrease)
            {
                expectedAmount.Append("-");
            }

            expectedAmount.Append(Convert.ToString(discountPrice));

            if (changeType == ChangeType.Percent)
            {
                expectedAmount.Append("%");
            }
            VerifyTool.VerifyValue(expectedAmount.ToString(), UIUtil.DefaultProvider.GetText(codeChangeAmountLocator, LocateBy.XPath), "The change amount is : {0}");
        }

        public void VerifyCodeLimitNumber(string codeName, int? limitNumber)
        {
            string codeLinkLocator = ComposeCodeLinkLocator(codeName);
            string codeLimitNumberLocator = string.Format("{0}/../../td[4]", codeLinkLocator);
            VerifyTool.VerifyValue(Convert.ToString(limitNumber), UIUtil.DefaultProvider.GetText(codeLimitNumberLocator, LocateBy.XPath), "The code limit is : {0}");
        }
        #endregion

        #endregion

        #region Methods on merchandise page
        public void SetMerchandiseDiscountCode(string discountCodeRule)
        {
            UIUtil.DefaultProvider.Type("ctl00_cphDialog_feepassword", discountCodeRule, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void ClickDiscountCodeRequired()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_feePasswordRequired", LocateBy.Id);
        }

        public void VerifyMerchandiseDiscountCodeAvailable(bool available)
        {
            string style = UIUtil.DefaultProvider.GetAttribute("ctl00_cphDialog_divDiscountOptions", "style", LocateBy.Id);
            bool actual = (style != "display: none;");

            if (actual != available)
            {
                VerifyTool.VerifyValue(available, actual, "“The discount code available state is: {0}");
            }
        }

        public void VerifyMerchandiseDiscountCodeErrorMessage(string errorMessage)
        {
            if (HasDiscountCodeErrors())
            {
                string actMessage = UIUtil.DefaultProvider.GetText("//div[@id = 'ctl00_valSummary']/ul/li", LocateBy.XPath);
                if (actMessage != errorMessage)
                {
                    VerifyTool.VerifyValue(errorMessage, actMessage, "The discount code error message is: {0}");
                }
            }
            else
            {
                Assert.Fail("There is no discount code errors");
            }
        }
        #endregion

        #endregion

        #region Helper methods
        private string ComposeCodeLinkLocator(string codeName)
        {
            return string.Format(CodeLinkLocatorFormat, codeName);
        }

        private string ComposeDeleteCodeLinkLocator(string codeName)
        {
            return string.Format(DeleteCodeLinkLocatorFormat, codeName);
        }
        #endregion
    }
}
