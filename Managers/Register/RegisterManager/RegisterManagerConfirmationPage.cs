﻿namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Linq;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class RegisterManager : ManagerBase
    {
        #region Constants
        public const string ConfirmationSubTotal = "//tr[@class='summaryRow']/td[text()='Subtotal:']/following-sibling::td[@class='currency']";
        public const string ConfirmationTotal = "//td[text()='Total:']/following-sibling::td[@class='currency']";
        public const string ConfirmationShippingFee = "//tr[@class='summaryRow'][2]/td[2][@class='currency']";
        public const string ConfirmationLodgingFeeSubTotal = "//th[text()='Lodging Fee']/../../following-sibling::tbody//td[text()='Subtotal:']/following-sibling::td[@class='currency']";
        public const string ConfirmationLodgingBookingFee = "//tr[@class='summaryRow']/td[text()='Lodging Booking Fee:']/following-sibling::td";
        public const string ConfirmationVerifyTemplate = "//table/caption[contains(text(),'Verify')]/..//tr[{0}]/td[{1}]";
        public const string ConfirmationPaymentMethod = "ctl00_cph_ddlPaymentMethods";
        public const string ConfirmationDiscountMessage = "//td[@class='discountMessage']";
        public const string ConfirmationGroupDiscountSaving = "Your total includes a group discount savings of {0}.";
        public const string ConfirmationServiceFee = "//td[text()='Service Fee:']/following-sibling::td";
        public const string TaxLocatorConfirmation = "//td[text()='{0}:']/following-sibling::td[1]";
        public const string ConfirmationRecurringSubtotal = "//legend[text()='Recurring Fees']/..//td[text()='Subtotal:']/following-sibling::td[@class='currency']";
        public const string ConfirmationRecurringTax = "//legend[text()='Recurring Fees']/..//td[text()='{0}:']/following-sibling::td[@class='currency']";
        public const string ConfirmationMembershipYearlyFees = "//legend[text()='Recurring Fees']/..//td[text()='Membership Fees for This Year:']/following-sibling::td[@class='currency']";
        public const string ConfirmationMembershipDiscountMessage = "//legend[text()='Recurring Fees']/..//td[@class='discountMessage']";
        #endregion

        public enum ConfirmationPageField
        {
            [StringValue("Registration ID:")]
            RegistrationId,

            [StringValue("Status:")]
            Status, 

            [StringValue("Renewal Date:")]
            RenewalDate,

            [StringValue("Work Phone:")]
            WorkPhone,

            [StringValue("Fax:")]
            Fax,

            [StringValue("Email:")]
            Email,

            [StringValue("Registrant Type:")]
            RegistrantType,

            [StringValue("Registrant:")]
            RegistrantBasicInfo
        }

        #region Confirmation Page

        public bool OnConfirmationPage()
        {
            return UIUtilityProvider.UIHelper.UrlContainsPath("register/confirmation.aspx");
        }

        [Verify]
        public void VerifyOnConfirmationPage()
        {
            if (!this.OnConfirmationPage())
            {
                UIUtilityProvider.UIHelper.FailTest("Not on Confirmation page!");
            }
        }

        [Step]
        public int GetRegistrationIdOnConfirmationPage()
        {
            return Convert.ToInt32(this.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.RegistrationId));
        }

        [Step]
        public object GetConfirmationPageValueForPrimaryAttendee(ConfirmationPageField fieldName)
        {
            string primaryAttendeeDIVLocator = 
                "//div[@id='ctl00_cphNoForm_primaryInfo_registraionRepeater_ctl00_personalInfo_barCodeWrapper']";

            string forLocator = 
                primaryAttendeeDIVLocator 
                + "/following-sibling::div[@class='detailContentWrapper']//th[text()='{0}']/following-sibling::td[1]";

            return UIUtilityProvider.UIHelper.GetText(string.Format(forLocator, StringEnum.GetStringValue(fieldName)), LocateBy.XPath);
        }

        public object GetConfirmationPageValueForGroupMembers(ConfirmationPageField fieldName, int regIndex)
        {
            string groupMemberDIVLocator = string.Format(
                "//div[@id='ctl00_cphNoForm_membersRepeater_ctl{0}_panelRegDetail']//div[2]", 
                ConversionTools.ConvertGroupMemberIndexToTwoDigitsString(regIndex));

            string forLocator = 
                groupMemberDIVLocator 
                + "[@class='detailContentWrapper']//th[text()='{0}']/following-sibling::td[1]";

            object valueFound = UIUtilityProvider.UIHelper.GetText(string.Format(forLocator, StringEnum.GetStringValue(fieldName)), LocateBy.XPath);

            return valueFound;
        }


        /// <summary>
        /// Make the transactions for this registrant, the start of yesterday, so the Void button goes away
        /// </summary>
        /// <param name="registrationId"></param>
        public void PullBackTransaction(int registrationId)
        {
            var db = new ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            db.ExecuteCommand(string.Format("update Transactions set transdate='{0}' where RegisterId={1}",
                DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy"),
                registrationId));
        }

        [Step]
        public void ConfirmRegistration()
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent("//div[@class='successAlert']", LocateBy.XPath);
            this.VerifyOnConfirmationPage();
        }

        [Verify]
        public void VerifyRegistrationType(string typeName)
        {
            VerifyRegType(typeName, -1);
        }

        public void VerifyRegistrationType(string typeName, int regIndex)
        {
            VerifyRegType(typeName, regIndex);
        }

        private void VerifyRegType(string typeName, int regIndex)
        {
            string regType = Convert.ToString(
                regIndex == -1 ? GetConfirmationPageValueForPrimaryAttendee(ConfirmationPageField.RegistrantType) : GetConfirmationPageValueForGroupMembers(ConfirmationPageField.RegistrantType, regIndex));

            Assert.AreEqual(typeName, regType);
        }

        [Verify]
        public void VerifyPrimaryRegistrationPersonalInfo(XAuthPersonalInfo personalInfo)
        {
            List<string> personalInfos = new List<string>();
            personalInfos.Add(Convert.ToString(GetConfirmationPageValueForPrimaryAttendee(ConfirmationPageField.RegistrantBasicInfo)));
            personalInfos.Add(Convert.ToString(GetConfirmationPageValueForPrimaryAttendee(ConfirmationPageField.WorkPhone)));
            personalInfos.Add(Convert.ToString(GetConfirmationPageValueForPrimaryAttendee(ConfirmationPageField.Fax)));
            personalInfos.Add(Convert.ToString(GetConfirmationPageValueForPrimaryAttendee(ConfirmationPageField.Email)));
            VerifyTool.VerifyList(personalInfo.ToPersonalInfoSectionList(), personalInfos);
        }

        [Verify]
        public void VerifyGroupMemberPersonalInfo(XAuthPersonalInfo personalInfo, int regIndex)
        {
            List<string> personalInfos = new List<string>();
            personalInfos.Add(Convert.ToString(GetConfirmationPageValueForGroupMembers(ConfirmationPageField.RegistrantBasicInfo, regIndex)));
            personalInfos.Add(Convert.ToString(GetConfirmationPageValueForGroupMembers(ConfirmationPageField.WorkPhone, regIndex)));
            personalInfos.Add(Convert.ToString(GetConfirmationPageValueForGroupMembers(ConfirmationPageField.Fax, regIndex)));
            personalInfos.Add(Convert.ToString(GetConfirmationPageValueForGroupMembers(ConfirmationPageField.Email, regIndex)));
            VerifyTool.VerifyList(personalInfo.ToPersonalInfoSectionList(), personalInfos);
        }
        
        public void VerifyRegistrationFees(FeeResponse[] expectedResponse)
        {
            FeeResponse[] actualResponses = this.GetRegistrationFees();

            if (actualResponses.Length != expectedResponse.Length)
            {
                VerifyTool.VerifyValue(expectedResponse.Length, actualResponses.Length, "There are {0} fees");
            }
            else
            {
                for (int i = 0; i < actualResponses.Length; i++)
                {
                    if (!actualResponses.Equals(expectedResponse))
                    {
                        VerifyTool.VerifyValue(expectedResponse[i].FeeName, actualResponses[i].FeeName, "The fee name is {0}");
                        VerifyTool.VerifyValue(expectedResponse[i].FeeQuantity, actualResponses[i].FeeQuantity, "The fee quantity is {0}");
                        VerifyTool.VerifyValue(expectedResponse[i].FeeUnitPrice, actualResponses[i].FeeUnitPrice, "The fee unit price is {0}");
                        VerifyTool.VerifyValue(expectedResponse[i].FeeAmount, actualResponses[i].FeeAmount, "The fee amount is {0}");
                    }
                }
            }
        }

        public FeeResponse[] GetRegistrationFees()
        {
            string feeSummaryTableLocator = "//legend[text()='Fees']/../";
            List<FeeResponse> actResponse = new List<FeeResponse>();

            int count = UIUtilityProvider.UIHelper.GetXPathCountByXPath(feeSummaryTableLocator + "/tbody/tr");

            if (UIUtilityProvider.UIHelper.IsElementPresent(ConfirmationDiscountMessage, LocateBy.XPath))
            {
                count -= 3;//When there is a discount message, there are 3 summary rows
            }
            else
            {
                count -= 2;//When there is not a discount message, there are 2 summary rows
            }

            for (int i = 1; i < count; i++)
            {
                FeeResponse response = new FeeResponse();
                response.FeeName = UIUtilityProvider.UIHelper.GetText(string.Format(feeSummaryTableLocator + "/tbody/tr[{0}]/td", i + 1), LocateBy.XPath);
                response.FeeQuantity = UIUtilityProvider.UIHelper.GetText(string.Format(feeSummaryTableLocator + "/tbody/tr[{0}]/td[2]", i + 1), LocateBy.XPath);
                response.FeeUnitPrice = UIUtilityProvider.UIHelper.GetText(string.Format(feeSummaryTableLocator + "/tbody/tr[{0}]/td[3]", i + 1), LocateBy.XPath);
                response.FeeAmount = UIUtilityProvider.UIHelper.GetText(string.Format(feeSummaryTableLocator + "/tbody/tr[{0}]/td[4]", i + 1), LocateBy.XPath);
                actResponse.Add(response);
            }

            return actResponse.ToArray();
        }

        public void VerifyConfirmationTotal(double totalToVerify)
        {
            string expectedValue = MoneyTool.FormatMoney(totalToVerify);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationPage(), "Confirmation");

            actualValue = UIUtilityProvider.UIHelper.GetText(ConfirmationTotal, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "ConfirmationTotal : {0}");
        }

        public void VerifyConfirmationSubTotal(double subTotalToVerify)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationPage(), "Confirmation");

            actualValue = UIUtilityProvider.UIHelper.GetText(ConfirmationSubTotal, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "ConfirmationSubTotal : {0}");
        }

        public void VerifyConfirmationShippingFee(double subTotalToVerify)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify);
            string actualValue;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationPage(), "Confirmation");

            actualValue = UIUtilityProvider.UIHelper.GetText(ConfirmationShippingFee, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "Confirmation Shipping Fee : {0}");
        }

        public void VerifyConfirmationLodgingFeeTotal(double subTotalToVerify)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify);
            string actualValue;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationPage(), "Confirmation");

            actualValue = UIUtilityProvider.UIHelper.GetText(ConfirmationLodgingFeeSubTotal, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "Lodging Subtotal : {0}");
        }

        public void VerifyConfirmationLodgingBookingFeeTotal(double subTotalToVerify)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify);
            string actualValue;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationPage(), "Confirmation");

            actualValue = UIUtilityProvider.UIHelper.GetText(ConfirmationLodgingBookingFee, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "Lodging Booking Fee : {0}");
        }

        public void VerifyConfirmationSerivceFee(double subTotalToVerify)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify);
            string actualValue;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationPage(), "Confirmation");

            actualValue = UIUtilityProvider.UIHelper.GetText(ConfirmationServiceFee, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "Service Fee : {0}");
        }

        public void VerifyConfirmationSaving(double savingToVerify)
        {
            string amount = MoneyTool.FormatMoney(savingToVerify);
            string expectedValue = string.Format("By using a discount code, you have saved: {0}.", amount);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationPage(), "Confirmation");

            actualValue = UIUtilityProvider.UIHelper.GetText(ConfirmationDiscountMessage, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "ConfirmationSaving : {0}");
        }

        public void VerifyConfirmationGroupAndDiscountSaving(double groupSavingToVerify, double codeSavingToVerify)
        {
            string groupAmount = MoneyTool.FormatMoney(groupSavingToVerify);
            string codeAmount = MoneyTool.FormatMoney(codeSavingToVerify);
            string expectedValue = string.Format("Your total includes a group discount savings of {0} and a discount code savings of {1}.", groupAmount, codeAmount);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationPage(), "COnfirmation");

            actualValue = UIUtilityProvider.UIHelper.GetText(ConfirmationDiscountMessage, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "ConfirmationSaving : {0}");
        }

        public void VerifyConfirmationTax(double taxAmountToVerify, string taxLabel)
        {
            string expectedAmount = MoneyTool.FormatMoney(taxAmountToVerify);
            string actualAmount = UIUtilityProvider.UIHelper.GetText(string.Format(TaxLocatorConfirmation, taxLabel), LocateBy.XPath);
            VerifyTool.VerifyValue(expectedAmount, actualAmount, taxLabel + " = {0}");
        }

        public void VerifyConfirmationMembershipRecurringSubtotal(double subTotalToVerify)
        {
            string expectedValue = MoneyTool.FormatMoney(subTotalToVerify);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationPage(), "Confirmation");

            actualValue = UIUtilityProvider.UIHelper.GetText(ConfirmationRecurringSubtotal, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "Recurring Subtotal: {0}");
        }

        public void VerifyConfirmationMembershipReccuringTax(double taxAmountToVerify, string taxLabel)
        {
            string expectedAmount = MoneyTool.FormatMoney(taxAmountToVerify);
            string actualAmount = UIUtilityProvider.UIHelper.GetText(string.Format(ConfirmationRecurringTax, taxLabel), LocateBy.XPath);
            VerifyTool.VerifyValue(expectedAmount, actualAmount, taxLabel + " = {0}");
        }

        public void VerifyConfirmationMembershipRecurringTotal(double totalToVerify)
        {
            string expectedValue = MoneyTool.FormatMoney(totalToVerify);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationPage(), "Confirmation");

            actualValue = UIUtilityProvider.UIHelper.GetText(ConfirmationMembershipYearlyFees, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "MembershipYearlyFees : {0}");
        }

        public void VerifyConfirmationMembershipRecurringSaving(double savingToVerify)
        {
            string amount = MoneyTool.FormatMoney(savingToVerify);
            string expectedValue = string.Format("By using a discount code, you have saved: {0}.", amount);
            string actualValue = string.Empty;

            UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationPage(), "Confirmation");

            actualValue = UIUtilityProvider.UIHelper.GetText(ConfirmationMembershipDiscountMessage, LocateBy.XPath);

            VerifyTool.VerifyValue(expectedValue, actualValue, "ConfirmationSaving : {0}");
        }

        [Step]
        public void UnfoldGroupMember(int memberNumber)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("ctl00_cphNoForm_membersRepeater_ctl{0}_hlRegName", ConversionTools.ConvertGroupMemberIndexToTwoDigitsString(memberNumber)), LocateBy.Id);
        }

        [Verify]
        public void JoinAndVerifyWebEvent(string webEventURL)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//button[text()='Join the Web Event']", LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            VerifyTool.VerifyValue(Builder.FormDetailManager.StartPageDefaultInfo.ConferenceURL, UIUtilityProvider.UIHelper.GetLocation(), "URL = {0}");
        }

        public void VerifyConfirmationRoomType(string expectedRoomType)
        {
            string locator = "//fieldset[@class='registrantDetailSection']/legend[text()='Lodging & Travel']/following-sibling::table/tbody//th[text()='Room Preference:']/following-sibling::td";
            VerifyTool.VerifyValue(expectedRoomType, UIUtilityProvider.UIHelper.GetText(locator, LocateBy.XPath), "Confirmation RoomType: {0}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">the Merchandise index</param>
        /// <param name="expectedName"></param>
        /// <param name="expectedQuantity"></param>
        /// <param name="expectedPrice"></param>
        /// <param name="expectedAmount"></param>
        public void VerifyConfirmationMerchandise(int index, string expectedName, string expectedQuantity, string expectedPrice, string expectedAmount)
        {
            //in page, the index need to be +2, the first tr is the "gridTextOnlyShowInMobile".
            string locator = "//fieldset/legend[text()='Fees']/following-sibling::table/tbody/tr[" + (index + 2).ToString() + "]/td[{0}]";

            VerifyTool.VerifyValue(expectedName, UIUtilityProvider.UIHelper.GetText(string.Format(locator, 1), LocateBy.XPath).Trim(), "Merchandise Name: {0}"); //there is a space at the end of actual name, so Trim().
            VerifyTool.VerifyValue(expectedQuantity, UIUtilityProvider.UIHelper.GetText(string.Format(locator, 2), LocateBy.XPath), "Merchandise Quantity: {0}");
            VerifyTool.VerifyValue(expectedPrice, UIUtilityProvider.UIHelper.GetText(string.Format(locator, 3), LocateBy.XPath), "Merchandise Price: {0}");
            VerifyTool.VerifyValue(expectedAmount, UIUtilityProvider.UIHelper.GetText(string.Format(locator, 4), LocateBy.XPath), "Merchandise Amount: {0}");
        }

        public void VerifyConfirmationAgenda(int index,string expectedName)
        {
            string locator = "//fieldset[@class='registrantDetailSection']/legend[text()='Agenda']/following-sibling::div[@class='detailContentWrapper']/h4[{0}]";
            VerifyTool.VerifyValue(expectedName, UIUtilityProvider.UIHelper.GetText(string.Format(locator, index + 1), LocateBy.XPath), "Confirmation Agenda: {0}");
        }

        #endregion

        #region Confirmation Redirect

        [Step]
        public bool OnConfirmationRedirectPage()
        {
            bool onConfRedirect = false;

            if ((UIUtilityProvider.UIHelper.UrlContainsPath("regonline.com/register/ConfirmationRedirector.aspx")) && (UIUtilityProvider.UIHelper.IsTextPresent("Active Advantage")))
            {
                onConfRedirect = true;
            }

            return onConfRedirect;
        }

        [Step]
        public void ClickAdvantageNo()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//div[@class='offerSubmit']//span[text()='No thanks']/..", LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void ClickChangeMyRegistration()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphNoForm_btnRegEdit", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void ClickViewDirectory(string text)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//div[@class='actionsList']//a[text()='{0}']", text), LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }
        #endregion
    }
}