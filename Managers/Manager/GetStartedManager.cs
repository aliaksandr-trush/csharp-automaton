﻿namespace RegOnline.RegressionTest.Managers.Manager
{
    using System;
    using RegOnline.RegressionTest.UIUtility;

    public class GetStartedManager
    {
        private const string FirstNameLocator = "ctl00_site_content_body_uclOpenAccount_txtFirstName";
        private const string LastNameLocator = "ctl00_site_content_body_uclOpenAccount_txtLastName";
        private const string CompanyLocator = "ctl00_site_content_body_txtCompany";
        private const string PhoneLocator = "ctl00_site_content_body_txtPhone";
        private const string EmailLocator = "ctl00_site_content_body_uclOpenAccount_txtEmail";
        private const string WebsiteLocator = "ctl00_site_content_body_uclOpenAccount_website";
        private const string UsernameLocator = "ctl00_site_content_body_txtUsername";
        private const string PasswordLocator = "ctl00_site_content_body_txtPassword";
        private const string ConfirmedPasswordLocator = "ctl00_site_content_body_txtConfirmPassword";
        private const string RegPerYearLocator = "ctl00_site_content_body_uclOpenAccount_registrants";
        private const string AccountCurrencyLocator = "ctl00_site_content_body_uclOpenAccount_ddlCurrency";
        private const string HowDidYouFindUsLocator = "ctl00_site_content_body_uclOpenAccount_findUs";
        private const string HowDidYouFindUs_OtherInpuLocatort = "ctl00_site_content_body_uclOpenAccount_other";
        private const string TermsOfServiceLocator = "ctl00_site_content_body_chkTerms";
        private const string TryRegOnlineFreeButtonLocator = "ctl00_site_content_body_uclOpenAccount_btnSubmit";
        private const string GoToMyAccount = "ctl00_site_content_body_btnSubmit";
        private const string EstimatedTotalAttendeesAnnually = "ctl00_site_content_body_ddlEventSize";

        public void CreateNewAccount(string username, string password, string currency)
        {
            UIUtil.DefaultProvider.Type(FirstNameLocator, "Selenium", LocateBy.Id);
            UIUtil.DefaultProvider.Type(LastNameLocator, "Regression", LocateBy.Id);
            
            UIUtil.DefaultProvider.Type(EmailLocator, string.Format("selenium{0}@regonline.com", DateTime.Now.Ticks), LocateBy.Id);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(TryRegOnlineFreeButtonLocator, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();

            UIUtil.DefaultProvider.Type(UsernameLocator, username, LocateBy.Id);
            UIUtil.DefaultProvider.Type(PasswordLocator, password, LocateBy.Id);
            UIUtil.DefaultProvider.Type(ConfirmedPasswordLocator, password, LocateBy.Id);
            UIUtil.DefaultProvider.Type(CompanyLocator, "RegOnline", LocateBy.Id);
            UIUtil.DefaultProvider.Type(PhoneLocator, "303-577-5100", LocateBy.Id);

            UIUtil.DefaultProvider.SelectWithText(EstimatedTotalAttendeesAnnually, "0-50 Total Attendees", LocateBy.Id);
            ////UIUtilityProvider.UIHelper.Type(RegPerYearLocator, 50, LocateBy.Id);
            ////UIUtilityProvider.UIHelper.SelectWithText(AccountCurrencyLocator, currency, LocateBy.Id);
            ////UIUtilityProvider.UIHelper.SelectWithText(HowDidYouFindUsLocator, "Other", LocateBy.Id);
            ////UIUtilityProvider.UIHelper.Type(HowDidYouFindUs_OtherInpuLocatort, "Internal testing", LocateBy.Id);
            UIUtil.DefaultProvider.SetCheckbox(TermsOfServiceLocator, true, LocateBy.Id);

            UIUtil.DefaultProvider.WaitForDisplayAndClick(GoToMyAccount, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();

            if (UIUtil.DefaultProvider.UrlContainsAbsolutePath("__lab/track.aspx"))
            {
                // Click 'Start Building'
                UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_site_content_body_lnkEnter", LocateBy.Id);
                UIUtil.DefaultProvider.WaitForPageToLoad();
            }
        }
    }
}
