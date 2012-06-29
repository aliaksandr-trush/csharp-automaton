namespace RegOnline.RegressionTest.Managers.Report
{
    using System;
    using NUnit.Framework;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public class DirectoryReportManager : ManagerBase
    {
        private const string EmailAddressTextboxID = "ctl00_cphDirectory_loginDirectory_UserName";
        private const string UserIDTextboxID = "ctl00_cphDirectory_loginDirectory_UserID";
        private const string PasswordTextboxID = "ctl00_cphDirectory_loginDirectory_Password";
        private const string ContinueBtnID = "ctl00_cphDirectory_loginDirectory_LoginButton";
        private const string XAuthForgotPasswordLinkID = "ctl00_cphDirectory_xauthForgotPassword";
        private const string NonXAuthForgotPasswordLinkID = "forgotPassword";
        private const string FirstNameTextBoxID = "ctl00_cphDirectory_tbFirstName";
        private const string LastNameTextBoxID = "ctl00_cphDirectory_tbLastName";
        private const string PasswordEmailTextBoxID = "ctl00_cphDirectory_tbEmailAddress";
        private const string SubmitBtnID = "ctl00_cphDirectory_btnPasswordReminder";
        private const string SuccessSendMsgID = "ctl00_cphDirectory_lblPasswordReminder";

        public enum DirectoryElements
        { 
            EmailAddress,
            UserID,
            Password,
            XAuthForgotPasswordLink,
            NonXAuthForgotPasswordLink,
            SuccessSendtEmail
        }
        
        public void EnterEmailAddressOnLogin(string email)
        {
            UIUtilityProvider.UIHelper.Type(EmailAddressTextboxID, email, LocateBy.Id);
        }

        public void EnterUserIDOnLogin(string userID)
        {
            UIUtilityProvider.UIHelper.Type(UserIDTextboxID, userID, LocateBy.Id);
        }

        public void EnterPasswordOnLogin(string password)
        {
            UIUtilityProvider.UIHelper.Type(PasswordTextboxID, password, LocateBy.Id);
        }

        public void EnterFirstName(string firstName)
        {
            UIUtilityProvider.UIHelper.Type(FirstNameTextBoxID, firstName, LocateBy.Id);
        }

        public void EnterLastName(string lastName)
        {
            UIUtilityProvider.UIHelper.Type(LastNameTextBoxID, lastName, LocateBy.Id);
        }

        public void EnterForgotPasswordEmail(string email)
        {
            UIUtilityProvider.UIHelper.Type(PasswordEmailTextBoxID, email, LocateBy.Id);
        }

        public void Submit()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(SubmitBtnID, LocateBy.Id);
        }

        public void Continue()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ContinueBtnID, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void ClickNonXauthForgotPasswordLink()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(NonXAuthForgotPasswordLinkID, LocateBy.Id);
        }

        public void VerifyIsOnReportPage()
        {
            Assert.IsTrue(UIUtilityProvider.UIHelper.UrlContainsPath("DirectoryReport.aspx"));
        }

        public void VerifyIsOnDirectoryLoginPage()
        {
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            Assert.IsTrue(UIUtilityProvider.UIHelper.UrlContainsPath("DirectoryUserLogin.aspx"));   
        }

        public void VerifyErrorMessage(string errorMsg)
        {
            Assert.IsTrue(UIUtilityProvider.UIHelper.IsElementPresent(string.Format("//table//td[text()[contains(.,'{0}')]]", errorMsg), LocateBy.XPath));            
        }
        public void VerifyUserIDTextboxIsPresent(bool present = true)
        {
            VerifyFieldPresent<DirectoryElements>(DirectoryElements.UserID, present);
        }

        public void VerifyPasswordTextboxIsPresent(bool present = true)
        {
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            VerifyFieldPresent<DirectoryElements>(DirectoryElements.Password, present);
        }

        public void VerifyEmailTextboxIsPresent(bool present = true)
        {
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            VerifyFieldPresent<DirectoryElements>(DirectoryElements.EmailAddress, present);
        }

        public void VerifyXAuthForgotPasswordLinkIsPresent(bool present = true)
        {
            VerifyFieldPresent<DirectoryElements>(DirectoryElements.XAuthForgotPasswordLink, present);
        }

        public void VerifyNonXAuthForgotPasswordLinkIsPresent(bool present = true)
        {
            VerifyFieldPresent<DirectoryElements>(DirectoryElements.NonXAuthForgotPasswordLink, present);
        }

        public void VerifySuccessSentMsgIsPresent(bool present = true)
        {
            VerifyFieldPresent<DirectoryElements>(DirectoryElements.SuccessSendtEmail, present);
        }

        protected override string GetLocator<TEnum>(TEnum fieldEnum, ManagerBase.LocatorType locatorType)
        {
            Type enumType = fieldEnum.GetType();

            if(enumType == typeof(DirectoryElements))
            {
                string fieldString = fieldEnum.ToString();

                if(Enum.IsDefined(typeof(DirectoryElements), fieldString))
                {
                    DirectoryElements de = (DirectoryElements)Enum.Parse(typeof(DirectoryElements), fieldString);
                    return ComposeLocator(de, locatorType);
                }
            }

            return string.Empty;
        }

        private string ComposeLocator(DirectoryElements fieldEnum, ManagerBase.LocatorType locatorType)
        {
            string locator_Id = string.Empty;

            switch(fieldEnum)
            {
                case DirectoryElements.EmailAddress:
                    locator_Id = EmailAddressTextboxID;
                    break;
                case DirectoryElements.UserID:
                    locator_Id = UserIDTextboxID;
                    break;
                case DirectoryElements.Password:
                    locator_Id = PasswordTextboxID;
                    break;
                case DirectoryElements.XAuthForgotPasswordLink:
                    locator_Id = XAuthForgotPasswordLinkID;
                    break;
                case DirectoryElements.NonXAuthForgotPasswordLink:
                    locator_Id = NonXAuthForgotPasswordLinkID;
                    break;
                case DirectoryElements.SuccessSendtEmail:
                    locator_Id = SuccessSendMsgID;
                    break;
                default: break;
            }

            return locator_Id;
        }
    }
}
