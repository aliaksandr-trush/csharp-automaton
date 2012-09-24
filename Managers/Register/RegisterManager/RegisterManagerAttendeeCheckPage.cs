namespace RegOnline.RegressionTest.Managers.Register
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public partial class RegisterManager : ManagerBase
    {
        #region Constants
        // Password Page
        public const string PasswordEnterPassword = "//input[@id='ctl00_cph_txtPassword']";
        public const string PasswordAutoRecallBeginNew = "//p[contains(text(),'To begin a new registration')]/a";
        public const string PasswordDupEmailBeginNew = "ctl00_cph_lnkNotRegistered";
        public const string PasswordOnAlreadyReg = "//p[contains(text(),'To manage your current registration')]";
        public const string PasswordOnDupEmail = "//p[contains(text(),'Our records show that this email address was used to register for this event')]";
        public const string PasswordOnAutoRecall = "//p[contains(text(),'To automatically recall your personal information')]";
        public const string RegisterLoginPath = "register/login.aspx";
        public const string SubstituteLinkLocator = "//a[@id='ctl00_cph_grdMembers_ctl01_lnkSubst']";
        #endregion

        #region Password / AttendeeCheck

        [Step]
        public bool OnLoginPage()
        {
            bool onLogin = false;

            if (WebDriverUtility.DefaultProvider.UrlContainsPath(RegisterLoginPath))
            {
                onLogin = true;
            }

            return onLogin;
        }


        public bool OnPasswordPage()
        {
            bool onPassword = false;

            if (WebDriverUtility.DefaultProvider.IsElementPresent(PasswordEnterPassword, LocateBy.XPath))
            {
                onPassword = true;
            }

            return onPassword;
        }

        public bool OnPasswordUpdatePage()
        {
            bool onPasswordUpdate = false;

            if ((OnPasswordPage()) && (WebDriverUtility.DefaultProvider.IsElementPresent(PasswordOnDupEmail, LocateBy.XPath)))
            {
                onPasswordUpdate = true;
            }

            return onPasswordUpdate;
        }

        public bool OnPasswordDupEmailPage()
        {
            bool onPasswordDupEmail = false;

            if ((OnPasswordPage()) && (WebDriverUtility.DefaultProvider.IsElementPresent(PasswordOnDupEmail, LocateBy.XPath)))
            {
                onPasswordDupEmail = true;
            }

            return onPasswordDupEmail;
        }

        public bool OnPasswordAlreadyRegPage()
        {
            bool onPasswordAlreadyReg = false;

            if ((OnPasswordPage()) && (WebDriverUtility.DefaultProvider.IsElementPresent(PasswordOnAlreadyReg, LocateBy.XPath)))
            {
                onPasswordAlreadyReg = true;
            }

            return onPasswordAlreadyReg;
        }

        public bool OnPasswordAutoRecallPage()
        {
            bool onPasswordAutoRecallPage = false;

            if ((OnPasswordPage()) && (WebDriverUtility.DefaultProvider.IsElementPresent(PasswordOnAutoRecall, LocateBy.XPath)))
            {
                onPasswordAutoRecallPage = true;
            }

            return onPasswordAutoRecallPage;
        }

        public void EnterPassword()
        {
            WebDriverUtility.DefaultProvider.Type(
                PasswordEnterPassword, 
                Configuration.ConfigReader.DefaultProvider.AccountConfiguration.Password, 
                LocateBy.XPath);
        }

        [Step]
        public void EnterPassword(string password)
        {
            WebDriverUtility.DefaultProvider.Type(PasswordEnterPassword, password, LocateBy.XPath);
        }

        public void ClickPasswordBeginNewReg()
        {
            if (OnPasswordDupEmailPage())
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(PasswordDupEmailBeginNew, LocateBy.Id);
            }
            else
            {
                WebDriverUtility.DefaultProvider.FailTest("Not on expected page!");
            }

            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public bool OnAttendeeCheckPage()
        {
            return WebDriverUtility.DefaultProvider.UrlContainsPath("Register/AttendeeCheck.aspx");
        }

        public bool OnUseProfilePage()
        {
            bool onUseProfile = false;

            if ((OnAttendeeCheckPage()) && (WebDriverUtility.DefaultProvider.IsElementPresent("//label[@for='rTypeYes']/../../td[contains(label,'Use this profile')]", LocateBy.XPath)))
            {
                onUseProfile = true;
            }

            return onUseProfile;
        }

        [Step]
        public void AttendeeCheckEditPersonalInfo()
        {
            Assert.IsTrue(OnAttendeeCheckPage());
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//a[@id='ctl00_cph_grdMembers_ctl01_lnkPersInfo']", LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void AttendeeCheckEditAgenda()
        {
            Assert.IsTrue(OnAttendeeCheckPage());
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cph_grdMembers_ctl01_lnkAgenda", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public bool VerifyHasSubstituteLink()
        {
            return WebDriverUtility.DefaultProvider.IsElementDisplay(SubstituteLinkLocator, LocateBy.XPath);
        }

        public void ClickViewPrintOrEmailLink()
        {
            string locator = "//div[@id='attendeeLinks']/ul/li/a[text()='View, Print, or Email Registration Record and Invoice']";
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(locator, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void ClickCancelLinkAndVerifyDialogVisibility()
        {
            string locator = "ctl00_cph_lnkGrpCancel";
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(locator, LocateBy.Id);
            string dialogLocator = "//div[@role='dialog']/div/p[text()='Are you sure you want to cancel this person? Once you click OK, you CANNOT reverse this action.']";
            VerifyTool.VerifyValue(true, WebDriverUtility.DefaultProvider.IsElementDisplay(dialogLocator, LocateBy.XPath), "Cancel Dialog Display: {0}");
        }

        public void ClickCancelEntireGroup(bool isCancel)
        {
            string locator = "ctl00_cph_lnkGrpCancel";
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(locator, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();

            if (isCancel)
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//div[@class='confirmDialog ui-dialog-content ui-widget-content']/div[@class='buttonGroup']/a[text()='OK']", LocateBy.XPath);
            }
            else
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//div[@class='confirmDialog ui-dialog-content ui-widget-content']/div[@class='buttonGroup']/a[text()='Cancel']", LocateBy.XPath);
            }
        }

        public bool CancleRegistrationAndGetSuccessful()
        {
            string dialogLocator = "//div[@role='dialog']/div/p[text()='Are you sure you want to cancel this person? Once you click OK, you CANNOT reverse this action.']";

            if (WebDriverUtility.DefaultProvider.IsElementDisplay(dialogLocator, LocateBy.XPath))
            {
                string buttonLocator = "//div[@role='dialog']/div/div[@class='buttonGroup']/a[@class='okButton button']";
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(buttonLocator, LocateBy.XPath);
                WebDriverUtility.DefaultProvider.WaitForPageToLoad();
                string cancelledLabelLocator = "//table[@id='tblAttendeCheckRegistrantList']/tbody/tr/td/span[@class='warningText']/strong";
                VerifyTool.VerifyValue("Cancelled", WebDriverUtility.DefaultProvider.GetText(cancelledLabelLocator, LocateBy.XPath), "Cancle Registration State: {0}");
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}