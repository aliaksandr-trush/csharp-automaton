namespace RegOnline.RegressionTest.Managers.Manager
{
    using System;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public class GetStartedManager
    {
        private const string FirstNameLocator = "ctl00_site_content_body_uclOpenAccount_firstName";
        private const string LastNameLocator = "ctl00_site_content_body_uclOpenAccount_lastName";
        private const string OrganizationLocator = "ctl00_site_content_body_uclOpenAccount_company";
        private const string PhoneLocator = "ctl00_site_content_body_uclOpenAccount_phone";
        private const string EmailLocator = "ctl00_site_content_body_uclOpenAccount_email";
        private const string WebsiteLocator = "ctl00_site_content_body_uclOpenAccount_website";
        private const string UsernameLocator = "ctl00_site_content_body_uclOpenAccount_username";
        private const string Password1Locator = "ctl00_site_content_body_uclOpenAccount_password";
        private const string Password2Locator = "ctl00_site_content_body_uclOpenAccount_confirmPassword";
        private const string RegPerYearLocator = "ctl00_site_content_body_uclOpenAccount_registrants";
        private const string AccountCurrencyLocator = "ctl00_site_content_body_uclOpenAccount_ddlCurrency";
        private const string HowDidYouFindUsLocator = "ctl00_site_content_body_uclOpenAccount_findUs";
        private const string HowDidYouFindUs_OtherInpuLocatort = "ctl00_site_content_body_uclOpenAccount_other";
        private const string TermsOfServiceLocator = "ctl00_site_content_body_uclOpenAccount_terms";
        private const string GoToMyAccount = "ctl00_site_content_body_uclOpenAccount_btnGetStarted";

        public void CreateNewAccount(string username, string password, string currency)
        {
            UIUtilityProvider.UIHelper.Type(FirstNameLocator, "Selenium", LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(LastNameLocator, "Regression", LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(OrganizationLocator, "RegOnline", LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(PhoneLocator, "303-577-5100", LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(EmailLocator, string.Format("selenium{0}@regonline.com", DateTime.Now.Ticks), LocateBy.Id);
            
            UIUtilityProvider.UIHelper.Type(UsernameLocator, username, LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(Password1Locator, password, LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(Password2Locator, password, LocateBy.Id);

            UIUtilityProvider.UIHelper.Type(RegPerYearLocator, 50, LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectWithText(AccountCurrencyLocator, currency, LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectWithText(HowDidYouFindUsLocator, "Other", LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(HowDidYouFindUs_OtherInpuLocatort, "Internal testing", LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox(TermsOfServiceLocator, true, LocateBy.Id);

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(GoToMyAccount, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();

            if (UIUtilityProvider.UIHelper.UrlContainsAbsolutePath("__lab/track.aspx"))
            {
                // Click 'Start Building'
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_site_content_body_lnkEnter", LocateBy.Id);
                UIUtilityProvider.UIHelper.WaitForPageToLoad();
            }
        }
    }
}
