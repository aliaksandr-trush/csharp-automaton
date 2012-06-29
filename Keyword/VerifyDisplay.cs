namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.PageObject.Register;
    using RegOnline.RegressionTest.Utilities;
    using UIUtility;

    public class VerifyDisplay
    {
        private PageObjectHelper RegisterHelper = new PageObjectHelper();

        public VerifyFooterResults VerifyCompanyFooter()
        {
            VerifyFooterResults results = new VerifyFooterResults();

            RegisterHelper.Trustwave_Click();
            results.Trustwave = RegisterHelper.TrustwaveWeb.URLContains("sealserver.trustwave.com");
            RegisterHelper.TrustwaveWeb.CloseAndBackToPrevious();

            RegisterHelper.Facebook_Click();
            results.Facebook = RegisterHelper.FacebookWeb.URLContains("facebook.com");
            RegisterHelper.FacebookWeb.CloseAndBackToPrevious();

            RegisterHelper.Twitter_Click();
            results.Twitter = RegisterHelper.TwitterWeb.URLContains("twitter.com");
            RegisterHelper.TwitterWeb.CloseAndBackToPrevious();

            RegisterHelper.Linkedin_Click();
            results.Linkedin = RegisterHelper.LinkedinWeb.URLContains("linkedin.com");
            RegisterHelper.LinkedinWeb.CloseAndBackToPrevious();

            results.Copyright = RegisterHelper.Copyright.Text;

            RegisterHelper.TermsOfUse_Click();
            results.TermsOfUse = RegisterHelper.TermsOfUseWindow.URLContains("www.activenetwork.com/information/terms-of-use");
            RegisterHelper.TermsOfUseWindow.CloseAndBackToPrevious();

            RegisterHelper.PrivacyPolicy_Click();
            results.PrivacyPolicy = RegisterHelper.PrivacyPolicyWindow.URLContains("regonline.com/privacy");
            RegisterHelper.PrivacyPolicyWindow.CloseAndBackToPrevious();

            RegisterHelper.CookiePolicy_Click();
            results.CookiePolicy = RegisterHelper.CookiePolicyWindow.URLContains("activenetwork.com/information/cookie-policy");
            RegisterHelper.CookiePolicyWindow.CloseAndBackToPrevious();

            string Href = RegisterHelper.About.GetAttribute("href");
            RegisterHelper.About_Click();
            results.About = (RegisterHelper.AboutWindow.URLContains("activenetwork.com/software/solutions"))
                && (Href == "http://www.activenetwork.com/technology/overview.htm");
            RegisterHelper.AboutWindow.CloseAndBackToPrevious();

            RegisterHelper.ActiveCom_Click();
            results.ActiveCom = RegisterHelper.ActiveComWeb.URLContains("www.active.com");
            RegisterHelper.ActiveComWeb.CloseAndBackToPrevious();

            return results;
        }

        public VerifyEventDetailsResult VerifyEventDetails()
        {
            VerifyEventDetailsResult results = new VerifyEventDetailsResult();

            if (RegisterHelper.EventDetails.IsPresent)
            {
                RegisterHelper.EventDetails.Click();
            }

            if (RegisterHelper.Location.IsPresent)
            {
                results.Location = RegisterHelper.Location.Text;
            }
            if (RegisterHelper.AddressOne.IsPresent)
            {
                results.Address1 = RegisterHelper.AddressOne.Text;
            }
            if (RegisterHelper.AddressTwo.IsPresent)
            {
                results.Address2 = RegisterHelper.AddressTwo.Text;
            }
            if (RegisterHelper.City.IsPresent)
            {
                results.City = RegisterHelper.City.Text;
            }
            if (RegisterHelper.State.IsPresent)
            {
                results.State = RegisterHelper.State.Text;
            }
            if (RegisterHelper.Zip.IsPresent)
            {
                results.Zip = RegisterHelper.Zip.Text;
            }
            if (RegisterHelper.Country.IsPresent)
            {
                results.Country = RegisterHelper.Country.Text;
            }
            if (RegisterHelper.Phone.IsPresent)
            {
                results.Phone = RegisterHelper.Phone.Text;
            }
            if (RegisterHelper.Contact.IsPresent)
            {
                results.Contact = RegisterHelper.Contact.Text;
            }

            RegisterHelper.EventHome_Click();
            results.EventHome = RegisterHelper.EventHomeWindow.URL;
            RegisterHelper.EventHomeWindow.CloseAndBackToPrevious();

            RegisterHelper.EventContactInfo_Click();
            RegisterHelper.PopupContactInfo.SelectByIndex();
            results.EventContactInfo = RegisterHelper.PopupContactInfo.PopupContactInfo.Text;
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            RegisterHelper.PopupContactInfoClose.WaitForPresent();
            Utility.ThreadSleep(1);
            RegisterHelper.PopupContactInfoClose.Click();

            results.EventTitle = RegisterHelper.EventTitle.Text;

            return results;
        }

        public VerifyPageResults VerifyPage()
        {
            VerifyPageResults results = new VerifyPageResults();

            results.PageHeader = RegisterHelper.PageHeader.Text;
            results.PageFooter = RegisterHelper.PageFooter.Text;

            return results;
        }
    }

    public class VerifyFooterResults
    {
        public bool Trustwave;
        public bool Facebook;
        public bool Twitter;
        public bool Linkedin;
        public string Copyright;
        public bool TermsOfUse;
        public bool PrivacyPolicy;
        public bool CookiePolicy;
        public bool About;
        public bool ActiveCom;
    }

    public class VerifyEventDetailsResult
    {
        public string EventTitle;
        public string Location;
        public string Address1;
        public string Address2;
        public string City;
        public string State;
        public string Zip;
        public string Country;
        public string Phone;
        public string Contact;
        public string EventHome;
        public string EventContactInfo;
    }

    public class VerifyPageResults
    {
        public string PageHeader;
        public string PageFooter;
    }
}
