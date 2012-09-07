namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class RegistationSite : Window
    {
        #region Pages
        public PageObject.Register.Agenda Agenda = new PageObject.Register.Agenda();
        public PageObject.Register.AttendeeCheck AttendeeCheck = new PageObject.Register.AttendeeCheck();
        public PageObject.Register.Checkin Checkin = new PageObject.Register.Checkin();
        public PageObject.Register.Checkout Checkout = new PageObject.Register.Checkout();
        public PageObject.Register.Confirmation Confirmation = new PageObject.Register.Confirmation();
        public PageObject.Register.EventCalendar EventCalendar = new PageObject.Register.EventCalendar();
        public PageObject.Register.EventWebsite EventWebsite = new PageObject.Register.EventWebsite();
        public PageObject.Register.Login Login = new PageObject.Register.Login();
        public PageObject.Register.OnsiteKiosk OnsiteKiosk = new PageObject.Register.OnsiteKiosk();
        public PageObject.Register.PersonalInfo PersonalInfo = new PageObject.Register.PersonalInfo();
        public PageObject.Register.SSOLogin SSOLogin = new PageObject.Register.SSOLogin();
        public PageObject.Register.Merchandise Merchandise = new PageObject.Register.Merchandise();
        #endregion

        #region WebElements
        public ButtonOrLink Trustwave = new ButtonOrLink("logo1", LocateBy.ClassName);
        public Window TrustwaveWeb = new Window();
        public ButtonOrLink EventHome = new ButtonOrLink("ctl00_lnkHome", LocateBy.Id);
        public Window EventHomeWindow = new Window();
        public ButtonOrLink EventContactInfo = new ButtonOrLink("ctl00_lnkContactInfo", LocateBy.Id);
        public ContactInfo PopupContactInfo = new ContactInfo(0);
        public ButtonOrLink Facebook = new ButtonOrLink("social1", LocateBy.ClassName);
        public Window FacebookWeb = new Window();
        public ButtonOrLink Twitter = new ButtonOrLink("social2", LocateBy.ClassName);
        public Window TwitterWeb = new Window();
        public ButtonOrLink Linkedin = new ButtonOrLink("social3", LocateBy.ClassName);
        public Window LinkedinWeb = new Window();
        public Label Copyright = new Label("activeCopyright", LocateBy.Id);
        public ButtonOrLink TermsOfUse = new ButtonOrLink("//*[@id='activeLinks']/li/a[text()='Terms of Use']", LocateBy.XPath);
        public Window TermsOfUseWindow = new Window();
        public ButtonOrLink PrivacyPolicy = new ButtonOrLink("//*[@id='activeLinks']/li/a[text()='Privacy Policy']", LocateBy.XPath);
        public Window PrivacyPolicyWindow = new Window();
        public ButtonOrLink CookiePolicy = new ButtonOrLink("//*[@id='activeLinks']/li/a[text()='Cookie Policy']", LocateBy.XPath);
        public Window CookiePolicyWindow = new Window();
        public ButtonOrLink About = new ButtonOrLink("//*[@id='activeLinks']/li/a[text()='About Active.com']", LocateBy.XPath);
        public Window AboutWindow = new Window();
        public ButtonOrLink ActiveCom = new ButtonOrLink("//*[@id='activeLogo']/a", LocateBy.XPath);
        public Window ActiveComWeb = new Window();
        public ButtonOrLink EventDetails = new ButtonOrLink("//*[text()='(View Details)']", LocateBy.XPath);
        public Label Location = new Label("//*[@id='fullEventDetails']//span[@class='fn org']", LocateBy.XPath);
        public Label AddressOne = new Label("//*[@id='fullEventDetails']//span[@class='street-address']", LocateBy.XPath);
        public Label AddressTwo = new Label("//*[@id='fullEventDetails']//span[@class='extended-address']", LocateBy.XPath);
        public Label City = new Label("//*[@id='fullEventDetails']//span[@class='locality']", LocateBy.XPath);
        public Label State = new Label("//*[@id='fullEventDetails']//span[@class='region']", LocateBy.XPath);
        public Label Zip = new Label("//*[@id='fullEventDetails']//span[@class='postal-code']", LocateBy.XPath);
        public Label Country = new Label("//*[@id='fullEventDetails']//span[@class='country-name']", LocateBy.XPath);
        public Label Phone = new Label("//*[@id='fullEventDetails']//span[@class='tel']", LocateBy.XPath);
        public Label Contact = new Label("//*[@id='fullEventDetails']//*[@class='contact']", LocateBy.XPath);
        public ButtonOrLink PopupContactInfoClose = new ButtonOrLink("//*[text()='close']", LocateBy.XPath);
        public Label EventTitle = new Label("summary", LocateBy.ClassName);
        public Label PageHeader = new Label("//*[@id='pageHeader']", LocateBy.XPath);
        public Label PageFooter = new Label("//*[@id='pageFooter']", LocateBy.XPath);
        public ButtonOrLink Continue = new ButtonOrLink("//div[@class='buttonGroup']/button[text()='Continue']", LocateBy.XPath);
        public ButtonOrLink AddAnotherPerson = new ButtonOrLink("//*[text()='Add Another Person']", LocateBy.XPath);
        public ButtonOrLink AddPersonToWaitlist = new ButtonOrLink("//*[text()='Add a Person to the Waitlist']", LocateBy.XPath);
        public Label ErrorMessages = new Label("//div[@id='ctl00_valSummary']/ul/li", LocateBy.XPath);
        #endregion

        #region Basic Actions
        public Label Text(string text)
        {
            return new Label(string.Format("//*[contains(text(),'{0}')]", text), LocateBy.XPath);
        }

        public void Trustwave_Click()
        {
            this.Trustwave.WaitForDisplay();
            this.Trustwave.Click();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void EventHome_Click()
        {
            this.EventHome.WaitForDisplay();
            this.EventHome.Click();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void EventContactInfo_Click()
        {
            this.EventContactInfo.WaitForDisplay();
            this.EventContactInfo.Click();
            Utility.ThreadSleep(1);
        }

        public void Facebook_Click()
        {
            this.Facebook.WaitForDisplay();
            this.Facebook.Click();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void Twitter_Click()
        {
            this.Twitter.WaitForDisplay();
            this.Twitter.Click();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void Linkedin_Click()
        {
            this.Linkedin.WaitForDisplay();
            this.Linkedin.Click();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void TermsOfUse_Click()
        {
            this.TermsOfUse.WaitForDisplay();
            this.TermsOfUse.Click();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void PrivacyPolicy_Click()
        {
            this.PrivacyPolicy.WaitForDisplay();
            this.PrivacyPolicy.Click();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void CookiePolicy_Click()
        {
            this.CookiePolicy.WaitForDisplay();
            this.CookiePolicy.Click();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void About_Click()
        {
            this.About.WaitForDisplay();
            this.About.Click();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void ActiveCom_Click()
        {
            this.ActiveCom.WaitForDisplay();
            this.ActiveCom.Click();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void EventDetails_Click()
        {
            this.EventDetails.WaitForDisplay();
            this.EventDetails.Click();
            Utility.ThreadSleep(1);
        }

        public void Continue_Click()
        {
            this.Continue.WaitForDisplay();
            this.Continue.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddAnotherPerson_Click()
        {
            this.AddAnotherPerson.WaitForDisplay();
            this.AddAnotherPerson.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddPersonToWaitlist_Click()
        {
            this.AddPersonToWaitlist.WaitForDisplay();
            this.AddPersonToWaitlist.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
        #endregion

        #region OnPageDistinguish
        public bool IsOnPage(DataCollection.FormData.RegisterPage page)
        {
            switch (page)
            {
                case DataCollection.FormData.RegisterPage.Login:
                    return WebDriverUtility.DefaultProvider.UrlContainsPath("register/login.aspx");
                case DataCollection.FormData.RegisterPage.Checkin:
                    return WebDriverUtility.DefaultProvider.UrlContainsPath("register/checkin.aspx");
                case DataCollection.FormData.RegisterPage.SSOLogin:
                    return WebDriverUtility.DefaultProvider.UrlContainsPath("SSO/ssologin.aspx");
                case DataCollection.FormData.RegisterPage.PersonalInfo:
                    return WebDriverUtility.DefaultProvider.UrlContainsPath("register/PersonalInfo.aspx");
                case DataCollection.FormData.RegisterPage.AttendeeCheck:
                    return WebDriverUtility.DefaultProvider.UrlContainsPath("Register/AttendeeCheck.aspx");
                case DataCollection.FormData.RegisterPage.Agenda:
                    return WebDriverUtility.DefaultProvider.UrlContainsPath("register/agenda.aspx");
                case DataCollection.FormData.RegisterPage.Checkout:
                    return WebDriverUtility.DefaultProvider.UrlContainsPath("register/checkout.aspx");
                case DataCollection.FormData.RegisterPage.Confirmation:
                    return WebDriverUtility.DefaultProvider.UrlContainsPath("register/confirmation.aspx");
                case DataCollection.FormData.RegisterPage.ConfirmationRedirect:
                    return (WebDriverUtility.DefaultProvider.UrlContainsPath("regonline.com/register/ConfirmationRedirector.aspx")) && (WebDriverUtility.DefaultProvider.IsTextPresent("Active Advantage"));
                default:
                    return false;
            }
        }
        #endregion
    }

    public class ContactInfo : Frame
    {
        public ContactInfo(int index) : base(index) { }

        public Label PopupContactInfo = new Label("dialogPadding", LocateBy.ClassName);
    }
}
