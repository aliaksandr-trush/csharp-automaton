namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.PageObject.Register;
    using RegOnline.RegressionTest.Utilities;
    using UIUtility;

    public class VerifyDisplay
    {
        public VerifyFooterResults VerifyCompanyFooter()
        {
            VerifyFooterResults results = new VerifyFooterResults();

            PageObject.PageObjectProvider.Register.RegistationSite.Trustwave_Click();
            results.Trustwave = PageObject.PageObjectProvider.Register.RegistationSite.TrustwaveWeb.URLContains("sealserver.trustwave.com");
            PageObject.PageObjectProvider.Register.RegistationSite.TrustwaveWeb.CloseAndBackToPrevious();

            PageObject.PageObjectProvider.Register.RegistationSite.Facebook_Click();
            results.Facebook = PageObject.PageObjectProvider.Register.RegistationSite.FacebookWeb.URLContains("facebook.com");
            PageObject.PageObjectProvider.Register.RegistationSite.FacebookWeb.CloseAndBackToPrevious();

            PageObject.PageObjectProvider.Register.RegistationSite.Twitter_Click();
            results.Twitter = PageObject.PageObjectProvider.Register.RegistationSite.TwitterWeb.URLContains("twitter.com");
            PageObject.PageObjectProvider.Register.RegistationSite.TwitterWeb.CloseAndBackToPrevious();

            PageObject.PageObjectProvider.Register.RegistationSite.Linkedin_Click();
            results.Linkedin = PageObject.PageObjectProvider.Register.RegistationSite.LinkedinWeb.URLContains("linkedin.com");
            PageObject.PageObjectProvider.Register.RegistationSite.LinkedinWeb.CloseAndBackToPrevious();

            results.Copyright = PageObject.PageObjectProvider.Register.RegistationSite.Copyright.Text;

            PageObject.PageObjectProvider.Register.RegistationSite.TermsOfUse_Click();
            results.TermsOfUse = PageObject.PageObjectProvider.Register.RegistationSite.TermsOfUseWindow.URLContains("www.activenetwork.com/information/terms-of-use");
            PageObject.PageObjectProvider.Register.RegistationSite.TermsOfUseWindow.CloseAndBackToPrevious();

            PageObject.PageObjectProvider.Register.RegistationSite.PrivacyPolicy_Click();
            results.PrivacyPolicy = PageObject.PageObjectProvider.Register.RegistationSite.PrivacyPolicyWindow.URLContains("regonline.com/privacy");
            PageObject.PageObjectProvider.Register.RegistationSite.PrivacyPolicyWindow.CloseAndBackToPrevious();

            PageObject.PageObjectProvider.Register.RegistationSite.CookiePolicy_Click();
            results.CookiePolicy = PageObject.PageObjectProvider.Register.RegistationSite.CookiePolicyWindow.URLContains("activenetwork.com/information/cookie-policy");
            PageObject.PageObjectProvider.Register.RegistationSite.CookiePolicyWindow.CloseAndBackToPrevious();

            string Href = PageObject.PageObjectProvider.Register.RegistationSite.About.GetAttribute("href");
            PageObject.PageObjectProvider.Register.RegistationSite.About_Click();
            results.About = (PageObject.PageObjectProvider.Register.RegistationSite.AboutWindow.URLContains("activenetwork.com/software/solutions"))
                && (Href == "http://www.activenetwork.com/technology/overview.htm");
            PageObject.PageObjectProvider.Register.RegistationSite.AboutWindow.CloseAndBackToPrevious();

            PageObject.PageObjectProvider.Register.RegistationSite.ActiveCom_Click();
            results.ActiveCom = PageObject.PageObjectProvider.Register.RegistationSite.ActiveComWeb.URLContains("www.active.com");
            PageObject.PageObjectProvider.Register.RegistationSite.ActiveComWeb.CloseAndBackToPrevious();

            return results;
        }

        public VerifyEventDetailsResult VerifyEventDetails()
        {
            VerifyEventDetailsResult results = new VerifyEventDetailsResult();

            if (PageObject.PageObjectProvider.Register.RegistationSite.EventDetails.IsPresent)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.EventDetails.Click();
            }

            if (PageObject.PageObjectProvider.Register.RegistationSite.Location.IsPresent)
            {
                results.Location = PageObject.PageObjectProvider.Register.RegistationSite.Location.Text;
            }
            if (PageObject.PageObjectProvider.Register.RegistationSite.AddressOne.IsPresent)
            {
                results.Address1 = PageObject.PageObjectProvider.Register.RegistationSite.AddressOne.Text;
            }
            if (PageObject.PageObjectProvider.Register.RegistationSite.AddressTwo.IsPresent)
            {
                results.Address2 = PageObject.PageObjectProvider.Register.RegistationSite.AddressTwo.Text;
            }
            if (PageObject.PageObjectProvider.Register.RegistationSite.City.IsPresent)
            {
                results.City = PageObject.PageObjectProvider.Register.RegistationSite.City.Text;
            }
            if (PageObject.PageObjectProvider.Register.RegistationSite.State.IsPresent)
            {
                results.State = PageObject.PageObjectProvider.Register.RegistationSite.State.Text;
            }
            if (PageObject.PageObjectProvider.Register.RegistationSite.Zip.IsPresent)
            {
                results.Zip = PageObject.PageObjectProvider.Register.RegistationSite.Zip.Text;
            }
            if (PageObject.PageObjectProvider.Register.RegistationSite.Country.IsPresent)
            {
                results.Country = PageObject.PageObjectProvider.Register.RegistationSite.Country.Text;
            }
            if (PageObject.PageObjectProvider.Register.RegistationSite.Phone.IsPresent)
            {
                results.Phone = PageObject.PageObjectProvider.Register.RegistationSite.Phone.Text;
            }
            if (PageObject.PageObjectProvider.Register.RegistationSite.Contact.IsPresent)
            {
                results.Contact = PageObject.PageObjectProvider.Register.RegistationSite.Contact.Text;
            }

            PageObject.PageObjectProvider.Register.RegistationSite.EventHome_Click();
            results.EventHome = PageObject.PageObjectProvider.Register.RegistationSite.EventHomeWindow.CurrentURL;
            PageObject.PageObjectProvider.Register.RegistationSite.EventHomeWindow.CloseAndBackToPrevious();

            PageObject.PageObjectProvider.Register.RegistationSite.EventContactInfo_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.PopupContactInfo.SelectByIndex();
            results.EventContactInfo = PageObject.PageObjectProvider.Register.RegistationSite.PopupContactInfo.PopupContactInfo.Text;
            UIUtil.DefaultProvider.SwitchToMainContent();
            PageObject.PageObjectProvider.Register.RegistationSite.PopupContactInfoClose.WaitForPresent();
            Utility.ThreadSleep(1);
            PageObject.PageObjectProvider.Register.RegistationSite.PopupContactInfoClose.Click();

            results.EventTitle = PageObject.PageObjectProvider.Register.RegistationSite.EventTitle.Text;

            return results;
        }

        public VerifyPageResults VerifyPage()
        {
            VerifyPageResults results = new VerifyPageResults();

            results.PageHeader = PageObject.PageObjectProvider.Register.RegistationSite.PageHeader.Text;
            results.PageFooter = PageObject.PageObjectProvider.Register.RegistationSite.PageFooter.Text;

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
