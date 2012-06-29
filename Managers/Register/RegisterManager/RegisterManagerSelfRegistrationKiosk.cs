namespace RegOnline.RegressionTest.Managers.Register
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Configuration;

    public partial class RegisterManager : ManagerBase
    {
        #region locators/helpers
        private const string SearchTextBox = "ctl00_cph_txtSearch";
        private const string SearchEnterButton = "ctl00_cph_btnSearch";
        private const string RegisterOnSiteLink = "ctl00_cph_lnkRegister";
        private const string UserIdField = "ctl00_cph_txtUserID";
        private const string PasswordField = "ctl00_cph_txtPassword";
        private const string PassowrdEnterButton = "ctl00_cph_btnPassword";
        private const string CheckIn = "ctl00_cph_btnCheckin";
        private const string ProfileInfo = "//div[@class='profileBox']";
        private const string ChangeInfo = "ctl00_cph_btnChangeProfile";
        private const string ExitButton = "ctl00_cph_btnExit";
        private const string NewSearchButton = "ctl00_cph_btnReset";
        private const string GroupCheckInCheckBoxEmail = "//span[text()='({0})']/../..//input";
        private const string GroupMemberEditEmail = "//span[text()='({0})']/../..//a";
        private const string UpdateIndividualRegistration = "ctl00_cph_btnChangeProfile";
        private const string KioskFinishButton = "ctl00_cph_btnExit";
        private const string KioskFinishButtonTwo = "//div[@id='wrpClose']/input";
        private const string BadgeLoadedLocator = "//html/body/embed";
        private const string iframeLocator = "ifrmBadge";
        private const string Locator_Id_PrintBadge = "btnPrint1";
        private const string SaveUpdateLink = "ctl00_cph_btnSaveProfile";
        private const string MustBePaidInFullMessage = "//div[@id='ctl00_cph_pnlPaymentMsg']/div";
        private const string MakePaymentButton = "//*[text()='Make a Payment']";
        private const string StartOverButton = "ctl00_cph_ctlOnsiteLnk_hlStartOver";

        public enum KioskCCLocators
        {
            [StringValue("ctl00_cph_txtCC")]
            CCNumberLocator,

            [StringValue("ctl00_cph_txtCVV")]
            CCCVVLocator,

            [StringValue("ctl00_cph_ddlCCExpMonth")]
            CCExpMonthLocator,

            [StringValue("ctl00_cph_ddlCCExpYear")]
            CCExpYearLocator,

            [StringValue("ctl00_cph_txtCCName")]
            CCNameLocator,

            [StringValue("ctl00_cph_ddlCCCountry")]
            CCCountryLocator,

            [StringValue("ctl00_cph_txtCCAddress")]
            CCAddressLocator,

            [StringValue("ctl00_cph_txtCCCity")]
            CCCityLocator,

            [StringValue("ctl00_cph_txtCCState")]
            CCStateLocator,

            [StringValue("ctl00_cph_txtCCZip")]
            CCZipLocator
        }

        public enum KioskPersonalInfo
        {
            FirstName,
            LastName,
            Address1,
            City,
            State,
            Zip,
            Company
        }
        #endregion

        public void KioskSearch(string emailNameOrRegId)
        {
            UIUtilityProvider.UIHelper.Type(SearchTextBox, emailNameOrRegId, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(SearchEnterButton, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void KioskEnterUserId(string userId)
        {
            UIUtilityProvider.UIHelper.Type(UserIdField, userId, LocateBy.Id);
        }

        public void KioskEnterPassword()
        {
            this.KioskEnterPassword(ConfigurationProvider.XmlConfig.AccountConfiguration.Password);
        }

        public void KioskEnterPassword(string password)
        {
            UIUtilityProvider.UIHelper.Type(PasswordField, password, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(PassowrdEnterButton, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void KioskClickCheckinReg()
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent(ProfileInfo, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(CheckIn, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }
       
        public void KioskUnselectGroupMemberToCheckin(string email)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(GroupCheckInCheckBoxEmail, email), LocateBy.XPath);
        }

        public void KioskEditGroupMemberInfo(string email)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(GroupMemberEditEmail, email), LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void KioskClickChangeIndividualRegistrationInfo(string email)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(UpdateIndividualRegistration, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void KioskUpdateRegistrationInfo(KioskPersonalInfo itemToUpdate, string newInfo)
        {
            string id = string.Empty;

            switch (itemToUpdate)
            {
                case KioskPersonalInfo.FirstName:
                    id = "ctl00_cph_txtProfileFirstName";
                    UIUtilityProvider.UIHelper.Type(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.LastName:
                    id = "ctl00_cph_txtProfileLastName";
                    UIUtilityProvider.UIHelper.Type(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.Address1:
                    id = "ctl00_cph_txtProfileAddress";
                    UIUtilityProvider.UIHelper.Type(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.City:
                    id = "ctl00_cph_txtProfileCity";
                    UIUtilityProvider.UIHelper.Type(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.State:
                    id = "ctl00_cph_ddRegion";
                    UIUtilityProvider.UIHelper.SelectWithText(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.Zip:
                    id = "ctl00_cph_txtProfilePostalCode";
                    UIUtilityProvider.UIHelper.Type(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.Company:
                    id = "ctl00_cph_txtProfileCompany";
                    UIUtilityProvider.UIHelper.Type(id, newInfo, LocateBy.Id);
                    break;
            }
        }

        public void KioskEnterCCInfoToPayInFull()
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent(StringEnum.GetStringValue(KioskCCLocators.CCNumberLocator), LocateBy.Id);

            UIUtilityProvider.UIHelper.Type(StringEnum.GetStringValue(KioskCCLocators.CCNumberLocator), PaymentManager.DefaultPaymentInfo.CCNumber, LocateBy.Id);

            UIUtilityProvider.UIHelper.Type(StringEnum.GetStringValue(KioskCCLocators.CCCVVLocator), PaymentManager.DefaultPaymentInfo.CVV, LocateBy.Id);

            UIUtilityProvider.UIHelper.SelectWithText(StringEnum.GetStringValue(KioskCCLocators.CCExpMonthLocator), PaymentManager.DefaultPaymentInfo.ExpirationMonth, LocateBy.Id);

            UIUtilityProvider.UIHelper.SelectWithText(StringEnum.GetStringValue(KioskCCLocators.CCExpYearLocator), PaymentManager.DefaultPaymentInfo.ExpirationYear, LocateBy.Id);

            UIUtilityProvider.UIHelper.Type(StringEnum.GetStringValue(KioskCCLocators.CCNameLocator), PaymentManager.DefaultPaymentInfo.HolderName, LocateBy.Id);

            UIUtilityProvider.UIHelper.Type(StringEnum.GetStringValue(KioskCCLocators.CCAddressLocator), PaymentManager.DefaultPaymentInfo.BillingAddressLineOne, LocateBy.Id);

            UIUtilityProvider.UIHelper.Type(StringEnum.GetStringValue(KioskCCLocators.CCCityLocator), PaymentManager.DefaultPaymentInfo.BillingCity, LocateBy.Id);

            UIUtilityProvider.UIHelper.Type(StringEnum.GetStringValue(KioskCCLocators.CCStateLocator), PaymentManager.DefaultPaymentInfo.BillingState, LocateBy.Id);

            UIUtilityProvider.UIHelper.SelectWithText(StringEnum.GetStringValue(KioskCCLocators.CCCountryLocator), PaymentManager.DefaultPaymentInfo.Country, LocateBy.Id);
        }

        public void KioskClickMakePaymentButton()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(MakePaymentButton, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void KioskStartOverButton()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(StartOverButton, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void KioskSaveUpdate()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(SaveUpdateLink, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void KioskExitCheckIn()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ExitButton, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void KioskClickNewSearch()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(NewSearchButton, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void KioskVerifyBadgeGenerated()
        {
            Assert.True(UIUtilityProvider.UIHelper.IsElementPresent(BadgeLoadedLocator, LocateBy.XPath));
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        public void ClickPrintBadge()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(Locator_Id_PrintBadge, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();

            if (UIUtilityProvider.UIHelper.IsAlertPresent())
            {
                UIUtilityProvider.UIHelper.GetConfirmation();
            }
        }

        public void FinishCheckin()
        {
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();

            if (!UIUtilityProvider.UIHelper.IsElementHidden(KioskFinishButton, LocateBy.Id))
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(KioskFinishButton, LocateBy.Id);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            }
            else
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(KioskFinishButtonTwo, LocateBy.XPath);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            }
        }

        public void KioskStartOnsiteRegistration()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(RegisterOnSiteLink, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public string GetPaidInFullMessage()
        {
            return UIUtilityProvider.UIHelper.GetText(MustBePaidInFullMessage, LocateBy.XPath);
        }
    }
}
