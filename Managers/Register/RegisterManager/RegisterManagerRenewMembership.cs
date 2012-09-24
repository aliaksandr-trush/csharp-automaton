namespace RegOnline.RegressionTest.Managers.Register
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class RegisterManager : ManagerBase
    {
        #region Constants

        private const string MemberLoginPagePath = "register/login.aspx";
        private const string MemberRenewNowPageTitle = "Member Renew";
        private const string MemberPasswordLocator = "ctl00_cph_txtPassword";
        private const string RenewNowButtonLocator = "ctl00_cph_lnkMembershipRenew";
        private const string RenewalPaymentOptionLocator = "ctl00_cphMemberships_pnlPayments";
        private const string UseExistingPaymentInfoLocator = "ctl00_cphMemberships_rbPaymentType_0";
        private const string UseNewPaymentInfoLocator = "ctl00_cphMemberships_rbPaymentType_1";
        private const string RenewalConfirmationMessageLocator = "ctl00_cphMemberships_stepThankYou";
        private const string ExpectedRenewalConfirmationMessage = "Your membership is complete.";
        private const string AttendeeCheckRenewalDateLocator = "//table[@id='tblAttendeCheckRegistrantList']//td[4]";

        public enum RenewalPaymentOptions
        {
            UseExistingPaymentInfo,
            UseNewPaymentInfo
        }

        #endregion

        public void CheckinToExistingMembership(int eventId, string email)
        {
            OpenRegisterPage(eventId);
            Checkin(email);
            //ClickCheckinAlreadyRegistered();
        }

        public void LoginToMembersip()
        {
            this.EnterMemberPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
            this.ClickLoginToMembership();
        }

        private void EnterMemberPassword(string password)
        {
            VerifyOnMemberLoginPage();
            UIUtil.DefaultProvider.Type(MemberPasswordLocator, password, LocateBy.Id);
        }

        public void ClickLoginToMembership()
        {
            Assert.True(OnMemberLoginPage(), "Not On Member Login Page");
            //WaitAndClickAndWait(MemberLoginLocator);
            Continue();
        }

        public void ClickRenewNowButton()
        {
            Assert.True(OnAttendeeCheckPage(), "Not on Attendee Check Page");
            UIUtil.DefaultProvider.WaitForDisplayAndClick(RenewNowButtonLocator, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void ChooseRenewPaymentOptions(RenewalPaymentOptions paymentOption)
        {
            UIUtil.DefaultProvider.VerifyElementPresent("//*[@id='ctl00_cphMemberships_stepVerify'][@class='rmpHiddenView']", false, LocateBy.XPath);

            switch (paymentOption)
            {
                case RenewalPaymentOptions.UseExistingPaymentInfo:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick(UseExistingPaymentInfoLocator, LocateBy.Id);
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                    
                case RenewalPaymentOptions.UseNewPaymentInfo:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick(UseNewPaymentInfoLocator, LocateBy.Id);
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    //TO DO: add new method to enter new payment info
                    break;
            }
        }

        public string GetCurrentRenewalDate()
        {
            if (!this.OnAttendeeCheckPage())
            {
                UIUtil.DefaultProvider.FailTest("Not on Attendee Check Page");
            }

            string renewalDate = UIUtil.DefaultProvider.GetText(AttendeeCheckRenewalDateLocator, LocateBy.XPath);
            return renewalDate;
        }

        public void VerifySuccessfulRenewal(string expectedNewRenewalDate)
        {
            ConfirmRegistration();
            string actualValue = (string)GetConfirmationPageValueForPrimaryAttendee(ConfirmationPageField.RenewalDate);
            VerifyTool.VerifyValue(expectedNewRenewalDate, actualValue, "Renewal Date: {0}");
            ////UIUtilityProvider.UIHelper.VerifyElementPresent("//*[@id='ctl00_cphMemberships_stepThankYou'][@class='rmpHiddenView']", false);
            ////VerifyTool.VerifyValue(ExpectedRenewalConfirmationMessage, UIUtilityProvider.UIHelper.GetText(RenewalConfirmationMessageLocator), "Renewal Confirmation: {0}");
        }

        public void VerifyOnMemberLoginPage()
        {
            if (!this.OnLoginPage())
            {
                UIUtil.DefaultProvider.FailTest("Not on member login page!");
            }
        }

        public void VerifyOnMemberAccountDetailsPage()
        {
            if (!this.OnAttendeeCheckPage())
            {
                UIUtil.DefaultProvider.FailTest("Not on member account details page!");
            }
        }

        public bool OnMemberLoginPage()
        {
            return UIUtil.DefaultProvider.UrlContainsAbsolutePath(MemberLoginPagePath);
        }
    }
}
