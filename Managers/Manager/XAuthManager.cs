namespace RegOnline.RegressionTest.Managers.Manager
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class XAuthManager : ManagerBase
    {
        public string DefaultAccount_UserName = "testUserName";
        public string DefaultAccount_Email = "test@user.com";
        public string DefaultAccount_Password = "testPassword";
        
        #region Constants

        private const string ErrorMsg_InvalidEmail = "\"Email Address\" is invalid";
        public const string ErrorMsg_AuthenticateFail = "Authentication failure. Please try again.";
        public const string ErrorMsg_MustSignInCorrectValue = "You must sign in with the correct values.";
        private const string ErrorMsg_UrlStartWithHttps = "\"Service Endpoint URL\" must start with \"https://\"";
        private const string ErrorMsg_RequiredUsername = "\"Username\" is required";
        private const string ErrorMsg_RequiredPassword = "\"Password\" is required";
        private const string ErrorMsg_RequiredEmail = "\"Email Address\" is required";
        ////private const string ErrorDIVLocator = "//div[@id='ctl00_valSummary']";
        private string ErrorLocator;

        private const string ServiceEndpointURL_By_UserName =          "https://beta.regonline.com/webServiceFakes/XAuth/MemberService.asmx/ValidateMemberByUserName";
        private const string ServiceEndpointURL_By_UserName_Password = "https://beta.regonline.com/webServiceFakes/XAuth/MemberService.asmx/ValidateMemberByUserNamePassword";
        private const string ServiceEndpointURL_By_Email =             "https://beta.regonline.com/webServiceFakes/XAuth/MemberService.asmx/ValidateMemberByEmail";
        private const string ServiceEndpointURL_By_Email_Password =    "https://beta.regonline.com/webServiceFakes/XAuth/MemberService.asmx/ValidateMemberByEmailPassword";
        
        #endregion

        public enum XAuthVersion
        {
            Old,
            New
        }

        private PageObject.Manager.XAuthLocator xAuth = null;
        private XAuthVersion defaultVersion = XAuthVersion.Old;

        public XAuthManager()
        {
            this.Initialize(defaultVersion);
        }

        public XAuthManager(XAuthVersion version)
        {
            this.Initialize(version);
        }

        private void Initialize(XAuthVersion version)
        {
            if (version == XAuthVersion.Old)
            {
                xAuth =  PageObject.PageObjectProvider.Builder.EventDetails.FormPages.XAuthOld;
            }
            else
            {
                xAuth = PageObject.PageObjectProvider.Manager.XAuth;
            }

            ErrorLocator = xAuth.ErrorDIVLocator.Locator + "/ul";
        }

        #region Public Methods

        [Step]
        public void RemoveXAuthRoleForCustomer()
        {
            RemoveLiveXAuthEventForCustomer();
    
            var db = new ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);
            db.ExecuteCommand("update Attendees set IsXAuth=0 where IsXAuth=1 and CustomerId=" + ConfigReader.DefaultProvider.AccountConfiguration.Id);
            db.ExecuteCommand("delete from XAuthConfiguration where CustomerId=" + ConfigReader.DefaultProvider.AccountConfiguration.Id);
        }

        [Step]
        public void ApprovedXAuthRoleForCustomer(bool isApproved)
        {
            var db = new ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);
            db.ExecuteCommand("update XAuthConfiguration set approved='" + isApproved + "' where CustomerId=" + ConfigReader.DefaultProvider.AccountConfiguration.Id);
        }

        [Step]
        public void RemoveLiveXAuthEventForCustomer()
        {
            var db = new ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);
            db.ExecuteCommand("update EventRegTypes set xauthenabled=0 where xauthenabled=1 and eventid in (select id from events where customer_id=" + ConfigReader.DefaultProvider.AccountConfiguration.Id + ")");
        }

        [Step]
        public void RemoveXAuthTestRegisterAndAttendeeForCustomer()
        {
            var db = new ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);
            db.ExecuteCommand("UPDATE Registrations Set GroupId=Register_Id,ResourceGroupId=Register_Id WHERE GroupId in (SELECT Registrations.Register_Id FROM Registrations WHERE Attendee_Id IN (SELECT id FROM Attendees WHERE IsXAuth=1 AND CustomerId=" + ConfigReader.DefaultProvider.AccountConfiguration.Id + ")) OR ResourceGroupId in (SELECT Registrations.Register_Id FROM Registrations WHERE Attendee_Id IN (SELECT id FROM Attendees WHERE IsXAuth=1 AND CustomerId=" + ConfigReader.DefaultProvider.AccountConfiguration.Id + "))");
            db.ExecuteCommand("DELETE FROM Registrations WHERE Attendee_Id IN (SELECT id FROM Attendees WHERE IsXAuth=1 AND CustomerId=" + ConfigReader.DefaultProvider.AccountConfiguration.Id + ")");
            db.ExecuteCommand("DELETE FROM Attendees WHERE IsXAuth=1 AND CustomerId=" + ConfigReader.DefaultProvider.AccountConfiguration.Id);
        }

        [Step]
        public void RemoveTestRegisterAndAttendeeByCustomerIdEmail(string email)
        {
            var db = new ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);
            db.ExecuteCommand("UPDATE Registrations Set GroupId=Register_Id,ResourceGroupId=Register_Id WHERE GroupId in (SELECT Registrations.Register_Id FROM Registrations WHERE Attendee_Id IN (SELECT id FROM Attendees WHERE CustomerId=" + ConfigReader.DefaultProvider.AccountConfiguration.Id + " AND Email_Address='" + email + "')) OR ResourceGroupId in (SELECT Registrations.Register_Id FROM Registrations WHERE Attendee_Id IN (SELECT id FROM Attendees WHERE CustomerId=" + ConfigReader.DefaultProvider.AccountConfiguration.Id + " AND Email_Address='" + email + "'))");
            db.ExecuteCommand("UPDATE Registrations SET GroupId = 0 WHERE Attendee_Id IN (SELECT id FROM Attendees WHERE CustomerId=" + ConfigReader.DefaultProvider.AccountConfiguration.Id + " AND Email_Address='" + email + "')");
            db.ExecuteCommand("DELETE FROM Registrations WHERE Attendee_Id IN (SELECT id FROM Attendees WHERE CustomerId= " + ConfigReader.DefaultProvider.AccountConfiguration.Id + " AND Email_Address='" + email + "')");
            db.ExecuteCommand("DELETE FROM Attendees WHERE CustomerId=" + ConfigReader.DefaultProvider.AccountConfiguration.Id + " AND Email_Address='" + email + "'");
        }

        [Step]
        public void ClickTestButton()
        {
            xAuth.Test.WaitForDisplay();
            xAuth.Test.Click();
        }

        [Step]
        public void ClickOKButton()
        {
            xAuth.OK.WaitForDisplay();
            xAuth.OK.Click();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(Builder.RegTypeManager.RegTypeDetailFrameID);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void ClickCancelButton()
        {
            WebDriverUtility.DefaultProvider.ClickCancel();
        }

        [Step]
        public void SetXAuthType(FormData.XAuthType type)
        {
            switch (type)
            {
                case FormData.XAuthType.ByUserName:
                    SetValidateMemberByUserName();
                    TypeDescriptionForIdentifer("User ID");
                    SetValidateMemberRequirePassword(false);
                    break;
                case FormData.XAuthType.ByUserNamePassword:
                    SetValidateMemberByUserName();
                    TypeDescriptionForIdentifer("User ID");
                    SetValidateMemberRequirePassword(true);
                    break;
                case FormData.XAuthType.ByEmail:
                    SetValidateMemberByEmail();
                    SetValidateMemberRequirePassword(false);
                    break;
                case FormData.XAuthType.ByEmailPassword:
                    SetValidateMemberByEmail();
                    SetValidateMemberRequirePassword(true);
                    break;
            }

            TypeServiceEndpointURL(type);
        }

        [Step]
        public void SetDefaultAccount(FormData.XAuthType type)
        {
            switch (type)
            {
                case FormData.XAuthType.ByUserName:
                    TypeTestUserName(DefaultAccount_UserName);
                    break;
                case FormData.XAuthType.ByUserNamePassword:
                    TypeTestUserName(DefaultAccount_UserName);
                    TypeTestPassword(DefaultAccount_Password);
                    break;
                case FormData.XAuthType.ByEmail:
                    TypeTestEmail(DefaultAccount_Email);
                    break;
                case FormData.XAuthType.ByEmailPassword:
                    TypeTestEmail(DefaultAccount_Email);
                    TypeTestPassword(DefaultAccount_Password);
                    break;
            }
        }

        [Step]
        public void TypeServiceEndpointURL(string url)
        {
            xAuth.ServiceEndpointURL.Type(url);
        }

        [Step]
        public void TypeMessageToRegistration(string msg)
        {
            xAuth.MessageToRegistration.Type(msg);
        }

        [Step]
        public void TypeTestEmail(string email)
        {
            xAuth.TestEmail.Type(email);
        }

        [Step]
        public void TypeTestPassword(string password)
        {
            xAuth.TestPassword.Type(password);
        }

        [Step]
        public void TypeTestUserName(string username)
        {
            xAuth.TestUserName.Type(username);
        }

        [Step]
        public void TypeDescriptionForIdentifer(string description)
        {
            xAuth.DescriptionForIdentifer.Type(description);
        }

        [Step]
        public void TypeForgetPasswordUrl(string forgetPasswordUrl)
        {
            xAuth.ForgetPasswordUrl.Type(forgetPasswordUrl);
        }

        // Verify Error Message Functions
        [Verify]
        public void VerifyErrorMessages(FormData.TestAccountResult errorType)
        {
            List<string> errors = new List<string>();
            switch (errorType)
            {
                case FormData.TestAccountResult.InvalidEmail:
                    errors.Add(ErrorMsg_InvalidEmail);
                    break;
                case FormData.TestAccountResult.AuthenticateFail:
                    errors.Add(ErrorMsg_AuthenticateFail);
                    break;
                case FormData.TestAccountResult.RequiredUsername:
                    errors.Add(ErrorMsg_RequiredUsername);
                    break;
                case FormData.TestAccountResult.RequiredEmail:
                    errors.Add(ErrorMsg_RequiredEmail);
                    break;
                case FormData.TestAccountResult.RequiredPassword:
                    errors.Add(ErrorMsg_RequiredPassword);
                    break;
                case FormData.TestAccountResult.RequiredUsernamePassword:
                    errors.Add(ErrorMsg_RequiredUsername);
                    errors.Add(ErrorMsg_RequiredPassword);
                    break;
                case FormData.TestAccountResult.RequiredEmailPassword:
                    errors.Add(ErrorMsg_RequiredEmail);
                    errors.Add(ErrorMsg_RequiredPassword);
                    break;
                case FormData.TestAccountResult.UrlStartWithHttps:
                    errors.Add(ErrorMsg_UrlStartWithHttps);
                    break;
                case FormData.TestAccountResult.RequiredUsernameAndUrlStartWithHttps:
                    errors.Add(ErrorMsg_UrlStartWithHttps);
                    errors.Add(ErrorMsg_RequiredUsername);
                    break;
                case FormData.TestAccountResult.RequiredEmailAndUrlStartWithHttps:
                    errors.Add(ErrorMsg_UrlStartWithHttps);
                    errors.Add(ErrorMsg_RequiredEmail);
                    break;
                case FormData.TestAccountResult.RequiredPasswordAndUrlStartWithHttps:
                    errors.Add(ErrorMsg_UrlStartWithHttps);
                    errors.Add(ErrorMsg_RequiredPassword);
                    break;
                case FormData.TestAccountResult.RequiredUsernamePasswordAndUrlStartWithHttps:
                    errors.Add(ErrorMsg_UrlStartWithHttps);
                    errors.Add(ErrorMsg_RequiredUsername);
                    errors.Add(ErrorMsg_RequiredPassword);
                    break;
                case FormData.TestAccountResult.RequiredEmailPasswordAndUrlStartWithHttps:
                    errors.Add(ErrorMsg_UrlStartWithHttps);
                    errors.Add(ErrorMsg_RequiredEmail);
                    errors.Add(ErrorMsg_RequiredPassword);
                    break;
                case FormData.TestAccountResult.Success:
                    break;
            }

            VerifyErrors(errors, errors.Count);
        }

        [Verify]
        public void VerifyPassTest()
        {
            xAuth.TestSuccessMessage.WaitForDisplay();
            Assert.AreEqual("Test was successful", xAuth.TestSuccessMessage.Text.Trim());
        }

        public void SetValidateMemberRequirePassword(bool require)
        {
            xAuth.ValidateMemberRequirePassword.Set(require);
        }

        public void SelectXAuthRadioButton()
        {
            xAuth.XAuthRadio.WaitForDisplay();
            xAuth.XAuthRadio.Click();
            Utility.ThreadSleep(1);
        }
        #endregion

        #region Private Methods

        private void SetValidateMemberByUserName()
        {
            xAuth.ValidateMemberByUserName.WaitForDisplay();
            xAuth.ValidateMemberByUserName.Click();
        }

        private void SetValidateMemberByEmail()
        {
            xAuth.ValidateMemberByEmail.WaitForDisplay();
            xAuth.ValidateMemberByEmail.Click();
        }

        private void TypeServiceEndpointURL(FormData.XAuthType type)
        {
            switch (type)
            {
                case FormData.XAuthType.ByUserName:
                    TypeServiceEndpointURL(ServiceEndpointURL_By_UserName);
                    break;
                case FormData.XAuthType.ByUserNamePassword:
                    TypeServiceEndpointURL(ServiceEndpointURL_By_UserName_Password);
                    break;
                case FormData.XAuthType.ByEmail:
                    TypeServiceEndpointURL(ServiceEndpointURL_By_Email);
                    break;
                case FormData.XAuthType.ByEmailPassword:
                    TypeServiceEndpointURL(ServiceEndpointURL_By_Email_Password);
                    break;
            }
        }

        private List<string> GetErrorMessages()
        {
            List<string> errorList = new List<string>();

            if (HasErrors())
            {
                int count = Convert.ToInt32(WebDriverUtility.DefaultProvider.GetXPathCountByXPath(ErrorLocator + "/li"));
                string errorFormat = ErrorLocator + "/li[{0}]";

                for (int i = 1; i <= count; i++)
                {
                    errorList.Add(WebDriverUtility.DefaultProvider.GetText(string.Format(errorFormat, i), LocateBy.XPath));
                }
            }

            return errorList;
        }

        private bool HasErrors()
        {
            return WebDriverUtility.DefaultProvider.GetAttribute(this.xAuth.ErrorDIVLocator.Locator, "@style", LocateBy.XPath) != "display:none;";
        }

        private void VerifyErrors(List<string> errorMessages, int errorsCount)
        {
            List<string> actualErrors = GetErrorMessages();
            int actualErrorsCount = actualErrors.Count;
            Assert.AreEqual(errorsCount, actualErrorsCount);

            foreach (string expectError in errorMessages)
            {
                string actualError = actualErrors[errorMessages.IndexOf(expectError)];
                Assert.AreEqual(expectError, actualError);
            }
        }

        #endregion
    }
}
