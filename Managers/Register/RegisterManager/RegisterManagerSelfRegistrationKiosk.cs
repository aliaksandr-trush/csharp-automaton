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
            UIUtil.DefaultProvider.Type(SearchTextBox, emailNameOrRegId, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(SearchEnterButton, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void KioskEnterUserId(string userId)
        {
            UIUtil.DefaultProvider.Type(UserIdField, userId, LocateBy.Id);
        }

        public void KioskEnterPassword()
        {
            this.KioskEnterPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
        }

        public void KioskEnterPassword(string password)
        {
            UIUtil.DefaultProvider.Type(PasswordField, password, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(PassowrdEnterButton, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void KioskClickCheckinReg()
        {
            UIUtil.DefaultProvider.WaitForElementPresent(ProfileInfo, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(CheckIn, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }
       
        public void KioskUnselectGroupMemberToCheckin(string email)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format(GroupCheckInCheckBoxEmail, email), LocateBy.XPath);
        }

        public void KioskEditGroupMemberInfo(string email)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format(GroupMemberEditEmail, email), LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void KioskClickChangeIndividualRegistrationInfo(string email)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(UpdateIndividualRegistration, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void KioskUpdateRegistrationInfo(KioskPersonalInfo itemToUpdate, string newInfo)
        {
            string id = string.Empty;

            switch (itemToUpdate)
            {
                case KioskPersonalInfo.FirstName:
                    id = "ctl00_cph_txtProfileFirstName";
                    UIUtil.DefaultProvider.Type(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.LastName:
                    id = "ctl00_cph_txtProfileLastName";
                    UIUtil.DefaultProvider.Type(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.Address1:
                    id = "ctl00_cph_txtProfileAddress";
                    UIUtil.DefaultProvider.Type(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.City:
                    id = "ctl00_cph_txtProfileCity";
                    UIUtil.DefaultProvider.Type(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.State:
                    id = "ctl00_cph_ddRegion";
                    UIUtil.DefaultProvider.SelectWithText(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.Zip:
                    id = "ctl00_cph_txtProfilePostalCode";
                    UIUtil.DefaultProvider.Type(id, newInfo, LocateBy.Id);
                    break;
                case KioskPersonalInfo.Company:
                    id = "ctl00_cph_txtProfileCompany";
                    UIUtil.DefaultProvider.Type(id, newInfo, LocateBy.Id);
                    break;
            }
        }

        public void KioskEnterCCInfoToPayInFull()
        {
            UIUtil.DefaultProvider.WaitForElementPresent(StringEnum.GetStringValue(KioskCCLocators.CCNumberLocator), LocateBy.Id);

            UIUtil.DefaultProvider.Type(StringEnum.GetStringValue(KioskCCLocators.CCNumberLocator), PaymentManager.DefaultPaymentInfo.CCNumber, LocateBy.Id);

            UIUtil.DefaultProvider.Type(StringEnum.GetStringValue(KioskCCLocators.CCCVVLocator), PaymentManager.DefaultPaymentInfo.CVV, LocateBy.Id);

            UIUtil.DefaultProvider.SelectWithText(StringEnum.GetStringValue(KioskCCLocators.CCExpMonthLocator), PaymentManager.DefaultPaymentInfo.ExpirationMonth, LocateBy.Id);

            UIUtil.DefaultProvider.SelectWithText(StringEnum.GetStringValue(KioskCCLocators.CCExpYearLocator), PaymentManager.DefaultPaymentInfo.ExpirationYear, LocateBy.Id);

            UIUtil.DefaultProvider.Type(StringEnum.GetStringValue(KioskCCLocators.CCNameLocator), PaymentManager.DefaultPaymentInfo.HolderName, LocateBy.Id);

            UIUtil.DefaultProvider.Type(StringEnum.GetStringValue(KioskCCLocators.CCAddressLocator), PaymentManager.DefaultPaymentInfo.BillingAddressLineOne, LocateBy.Id);

            UIUtil.DefaultProvider.Type(StringEnum.GetStringValue(KioskCCLocators.CCCityLocator), PaymentManager.DefaultPaymentInfo.BillingCity, LocateBy.Id);

            UIUtil.DefaultProvider.Type(StringEnum.GetStringValue(KioskCCLocators.CCStateLocator), PaymentManager.DefaultPaymentInfo.BillingState, LocateBy.Id);

            UIUtil.DefaultProvider.SelectWithText(StringEnum.GetStringValue(KioskCCLocators.CCCountryLocator), PaymentManager.DefaultPaymentInfo.Country, LocateBy.Id);
        }

        public void KioskClickMakePaymentButton()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(MakePaymentButton, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void KioskStartOverButton()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(StartOverButton, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void KioskSaveUpdate()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(SaveUpdateLink, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void KioskExitCheckIn()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(ExitButton, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void KioskClickNewSearch()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(NewSearchButton, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void KioskVerifyBadgeGenerated()
        {
            Assert.True(UIUtil.DefaultProvider.IsElementPresent(BadgeLoadedLocator, LocateBy.XPath));
            UIUtil.DefaultProvider.SwitchToMainContent();
        }

        public void ClickPrintBadge()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(Locator_Id_PrintBadge, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();

            if (UIUtil.DefaultProvider.IsAlertPresent())
            {
                UIUtil.DefaultProvider.GetConfirmation();
            }
        }

        public void FinishCheckin()
        {
            UIUtil.DefaultProvider.WaitForAJAXRequest();

            if (!UIUtil.DefaultProvider.IsElementHidden(KioskFinishButton, LocateBy.Id))
            {
                UIUtil.DefaultProvider.WaitForDisplayAndClick(KioskFinishButton, LocateBy.Id);
                UIUtil.DefaultProvider.WaitForAJAXRequest();
            }
            else
            {
                UIUtil.DefaultProvider.WaitForDisplayAndClick(KioskFinishButtonTwo, LocateBy.XPath);
                UIUtil.DefaultProvider.WaitForAJAXRequest();
            }
        }

        public void KioskStartOnsiteRegistration()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(RegisterOnSiteLink, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public string GetPaidInFullMessage()
        {
            return UIUtil.DefaultProvider.GetText(MustBePaidInFullMessage, LocateBy.XPath);
        }
    }
}
