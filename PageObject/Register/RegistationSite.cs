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
        public PageObject.Register.LodgingAndTravel LodgingAndTravel = new PageObject.Register.LodgingAndTravel();
        #endregion

        #region WebElements
        public Clickable Trustwave = new Clickable("logo1", LocateBy.ClassName);
        public Window TrustwaveWeb = new Window();
        public Clickable EventHome = new Clickable("ctl00_lnkHome", LocateBy.Id);
        public Window EventHomeWindow = new Window();
        public Clickable EventContactInfo = new Clickable("ctl00_lnkContactInfo", LocateBy.Id);
        public ContactInfo PopupContactInfo = new ContactInfo(0);
        public Clickable Facebook = new Clickable("social1", LocateBy.ClassName);
        public Window FacebookWeb = new Window();
        public Clickable Twitter = new Clickable("social2", LocateBy.ClassName);
        public Window TwitterWeb = new Window();
        public Clickable Linkedin = new Clickable("social3", LocateBy.ClassName);
        public Window LinkedinWeb = new Window();
        public Label Copyright = new Label("activeCopyright", LocateBy.Id);
        public Clickable TermsOfUse = new Clickable("//*[@id='activeLinks']/li/a[text()='Terms of Use']", LocateBy.XPath);
        public Window TermsOfUseWindow = new Window();
        public Clickable PrivacyPolicy = new Clickable("//*[@id='activeLinks']/li/a[text()='Privacy Policy']", LocateBy.XPath);
        public Window PrivacyPolicyWindow = new Window();
        public Clickable CookiePolicy = new Clickable("//*[@id='activeLinks']/li/a[text()='Cookie Policy']", LocateBy.XPath);
        public Window CookiePolicyWindow = new Window();
        public Clickable About = new Clickable("//*[@id='activeLinks']/li/a[text()='About Active.com']", LocateBy.XPath);
        public Window AboutWindow = new Window();
        public Clickable ActiveCom = new Clickable("//*[@id='activeLogo']/a", LocateBy.XPath);
        public Window ActiveComWeb = new Window();
        public Clickable EventDetails = new Clickable("//*[text()='(View Details)']", LocateBy.XPath);
        public Label Location = new Label("//*[@id='fullEventDetails']//span[@class='fn org']", LocateBy.XPath);
        public Label AddressOne = new Label("//*[@id='fullEventDetails']//span[@class='street-address']", LocateBy.XPath);
        public Label AddressTwo = new Label("//*[@id='fullEventDetails']//span[@class='extended-address']", LocateBy.XPath);
        public Label City = new Label("//*[@id='fullEventDetails']//span[@class='locality']", LocateBy.XPath);
        public Label State = new Label("//*[@id='fullEventDetails']//span[@class='region']", LocateBy.XPath);
        public Label Zip = new Label("//*[@id='fullEventDetails']//span[@class='postal-code']", LocateBy.XPath);
        public Label Country = new Label("//*[@id='fullEventDetails']//span[@class='country-name']", LocateBy.XPath);
        public Label Phone = new Label("//*[@id='fullEventDetails']//span[@class='tel']", LocateBy.XPath);
        public Label Contact = new Label("//*[@id='fullEventDetails']//*[@class='contact']", LocateBy.XPath);
        public Clickable PopupContactInfoClose = new Clickable("//*[text()='close']", LocateBy.XPath);
        public Label EventTitle = new Label("summary", LocateBy.ClassName);
        public Label PageHeader = new Label("//*[@id='pageHeader']", LocateBy.XPath);
        public Label PageFooter = new Label("//*[@id='pageFooter']", LocateBy.XPath);
        public Clickable Continue = new Clickable("//div[@class='buttonGroup']/button[text()='Continue']", LocateBy.XPath);
        public Clickable AddAnotherPerson = new Clickable("//*[text()='Add Another Person']", LocateBy.XPath);
        public Clickable AddPersonToWaitlist = new Clickable("//*[text()='Add a Person to the Waitlist']", LocateBy.XPath);
        public Label ErrorMessages = new Label("//div[@id='ctl00_valSummary']/ul/li", LocateBy.XPath);
        #endregion

        public string[] GetErrorMessages()
        {
            int count = this.ErrorMessages.Count;
            string[] errorList = new string[count];

            for (int i = 1; i <= count; i++)
            {
                errorList[i - 1] = UIUtil.DefaultProvider.GetText(
                    string.Format("{0}[{1}]", PageObject.PageObjectProvider.Register.RegistationSite.ErrorMessages.Locator, i), 
                    LocateBy.XPath);
            }

            return errorList;
        }

        #region Basic Actions
        public void GoToPage(DataCollection.EventData_Common.RegisterPage registerPage)
        {
            Clickable pageLink = new Clickable(
                string.Format("//a[contains(@id,'StepBar')][text()='{0}']",
                CustomStringAttribute.GetCustomString(registerPage)), LocateBy.XPath);

            pageLink.WaitForDisplay();
            pageLink.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public Label Text(string text)
        {
            return new Label(string.Format("//*[contains(text(),'{0}')]", text), LocateBy.XPath);
        }

        public void Trustwave_Click()
        {
            this.Trustwave.WaitForDisplay();
            this.Trustwave.Click();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void EventHome_Click()
        {
            this.EventHome.WaitForDisplay();
            this.EventHome.Click();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
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
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void Twitter_Click()
        {
            this.Twitter.WaitForDisplay();
            this.Twitter.Click();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void Linkedin_Click()
        {
            this.Linkedin.WaitForDisplay();
            this.Linkedin.Click();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void TermsOfUse_Click()
        {
            this.TermsOfUse.WaitForDisplay();
            this.TermsOfUse.Click();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void PrivacyPolicy_Click()
        {
            this.PrivacyPolicy.WaitForDisplay();
            this.PrivacyPolicy.Click();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void CookiePolicy_Click()
        {
            this.CookiePolicy.WaitForDisplay();
            this.CookiePolicy.Click();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void About_Click()
        {
            this.About.WaitForDisplay();
            this.About.Click();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void ActiveCom_Click()
        {
            this.ActiveCom.WaitForDisplay();
            this.ActiveCom.Click();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
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
        public bool IsOnPage(DataCollection.EventData_Common.RegisterPage page)
        {
            switch (page)
            {
                case DataCollection.EventData_Common.RegisterPage.Login:
                    return UIUtil.DefaultProvider.UrlContainsPath("register/login.aspx");
                case DataCollection.EventData_Common.RegisterPage.Checkin:
                    return UIUtil.DefaultProvider.UrlContainsPath("register/checkin.aspx");
                case DataCollection.EventData_Common.RegisterPage.SSOLogin:
                    return UIUtil.DefaultProvider.UrlContainsPath("SSO/ssologin.aspx");
                case DataCollection.EventData_Common.RegisterPage.PersonalInfo:
                    return UIUtil.DefaultProvider.UrlContainsPath("register/PersonalInfo.aspx");
                case DataCollection.EventData_Common.RegisterPage.AttendeeCheck:
                    return UIUtil.DefaultProvider.UrlContainsPath("Register/AttendeeCheck.aspx");
                case DataCollection.EventData_Common.RegisterPage.Agenda:
                    return UIUtil.DefaultProvider.UrlContainsPath("register/agenda.aspx");
                case DataCollection.EventData_Common.RegisterPage.Checkout:
                    return UIUtil.DefaultProvider.UrlContainsPath("register/checkout.aspx");
                case DataCollection.EventData_Common.RegisterPage.Confirmation:
                    return UIUtil.DefaultProvider.UrlContainsPath("register/confirmation.aspx");
                case DataCollection.EventData_Common.RegisterPage.ConfirmationRedirect:
                    return (UIUtil.DefaultProvider.UrlContainsPath("regonline.com/register/ConfirmationRedirector.aspx")) && (UIUtil.DefaultProvider.IsTextPresent("Active Advantage"));
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
