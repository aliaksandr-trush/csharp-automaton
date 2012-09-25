namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.Utilities;
    using NUnit.Framework;
    using Managers.Builder;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Attributes;

    public partial class RegisterManager : ManagerBase
    {
        private const string Locator_Id_XAuthUserId = "ctl00_cph_txtXAuthUserID";
        private const string Locator_Id_XAuthEmaiAddressTextboxInLoginPage = "ctl00_cph_ctlEmailMemID_txtEmail";
        private const string Locator_Id_XAuthUserIdTextboxInLoginPage = "ctl00_cph_txtXAuthUserID";
        private const string LocatorFormat_XPath_PIfield = "//div[@id='pageContent']/fieldset/ol/li/div/span[text()='{0}']/../following-sibling::div";
        private const string LocatorFormat_XPath_FormSection = "//div[@id='pageContent']/fieldset/legend[text()='{0}']";
        private const string LocatorFormat_XPath_MessageToRegistration = "//div[@id='pageContent']/div[@class='pageInstructions']/p[last()]";
        private const string DEFAULT_XAUTH_PASSWORD = "testPassword";
        
        #region DefaultTestAccounts

        private Dictionary<TestAccountType, XAuthPersonalInfo> testAccounts;
        public Dictionary<TestAccountType, XAuthPersonalInfo> TestAccounts
        {
            get
            {
                if (testAccounts == null)
                {
                    testAccounts = new Dictionary<TestAccountType, XAuthPersonalInfo>();
                    
                    XAuthPersonalInfo PI1 = new XAuthPersonalInfo();
                    PI1.Email = "test@user.com";
                    PI1.UserName = "testUserName";
                    PI1.XAuthPassword = "testPassword";
                    PI1.FirstName = "Test";
                    PI1.MiddleName = "Middle Name";
                    PI1.LastName = "User";
                    PI1.JobTitle = "Job Title";
                    PI1.Company = "Company Name";
                    PI1.Address1 = "Address1";
                    PI1.Address2 = "Address2";
                    PI1.City = "Tinytown";
                    PI1.State = "Colorado";
                    PI1.Zip = "80005";
                    PI1.Phone = "123-123-1233";
                    PI1.Extension = "113";
                    PI1.Fax = "303-987-3524";
                    PI1.Country = "USA";
                    PI1.Password = "123456pwd";
                    testAccounts.Add(TestAccountType.DefaultAccount1, PI1);

                    XAuthPersonalInfo PI2 = new XAuthPersonalInfo();
                    PI2.Email = "qa@tester.com";
                    PI2.UserName = "user123";
                    PI2.XAuthPassword = "testPassword";
                    PI2.FirstName = "Regonline";
                    PI2.MiddleName = "J";
                    PI2.LastName = "Tester";
                    PI2.JobTitle = "Test Master";
                    PI2.Company = "Regonline";
                    PI2.Address1 = "4750 Walnut St";
                    PI2.Address2 = "Suite 100";
                    PI2.City = "Boulder";
                    PI2.State = "Colorado";
                    PI2.Zip = "80003";
                    PI2.Phone = "303-333-3333";
                    PI2.Extension = "101";
                    PI2.Fax = "303-777-7777";
                    PI2.Country = "United States";
                    PI2.Password = "123456pwd";
                    testAccounts.Add(TestAccountType.DefaultAccount2, PI2);

                    XAuthPersonalInfo PI3 = new XAuthPersonalInfo();
                    PI3.Email = "first@xauth.com";
                    PI3.UserName = "firstxauth";
                    PI3.XAuthPassword = "testPassword";
                    PI3.FirstName = "First";
                    PI3.MiddleName = "A";
                    PI3.LastName = "Xauth";
                    PI3.JobTitle = "First Xauth Tester";
                    PI3.Company = "Regonline One";
                    PI3.Address1 = "4751 Walnut St";
                    PI3.Address2 = "Suite 100";
                    PI3.City = "Boulder";
                    PI3.State = "Colorado";
                    PI3.Zip = "80301";
                    PI3.Phone = "303-333-3333";
                    PI3.Extension = "101";
                    PI3.Fax = "303-777-7777";
                    PI3.Country = "United States";
                    PI3.Password = "123456pwd";
                    testAccounts.Add(TestAccountType.DefaultAccount3, PI3);

                    XAuthPersonalInfo PI4 = new XAuthPersonalInfo();
                    PI4.Email = "second@xauth.com";
                    PI4.UserName = "secondxauth";
                    PI4.XAuthPassword = "testPassword";
                    PI4.FirstName = "Second";
                    PI4.MiddleName = "B";
                    PI4.LastName = "Xauth";
                    PI4.JobTitle = "Second Xauth Tester";
                    PI4.Company = "Regonline Two";
                    PI4.Address1 = "4752 Walnut St";
                    PI4.Address2 = "Suite 100";
                    PI4.City = "Boulder";
                    PI4.State = "Colorado";
                    PI4.Zip = "80302";
                    PI4.Phone = "303-333-3333";
                    PI4.Extension = "101";
                    PI4.Fax = "303-777-7777";
                    PI4.Country = "United States";
                    PI4.Password = "123456pwd";
                    testAccounts.Add(TestAccountType.DefaultAccount4, PI4);

                    return testAccounts;
                }
                else
                    return testAccounts;
            }
            set { ;}
        }

        private PersonalInfo normalAccount = null;
        public PersonalInfo NormalAccount
        {
            get
            {
                if (normalAccount == null)
                {
                    normalAccount = new PersonalInfo();
                    normalAccount.Email = "normal@user.com";
                    normalAccount.FirstName = "Second";
                    normalAccount.MiddleName = "B";
                    normalAccount.LastName = "Xauth";
                    normalAccount.JobTitle = "Second Xauth Tester";
                    normalAccount.Company = "Regonline Two";
                    normalAccount.Address1 = "4752 Walnut St";
                    normalAccount.Address2 = "Suite 100";
                    normalAccount.City = "Boulder";
                    normalAccount.State = "Colorado";
                    normalAccount.Zip = "80302";
                    normalAccount.Phone = "303-333-3333";
                    normalAccount.Extension = "101";
                    normalAccount.Fax = "303-777-7777";
                    normalAccount.Country = "United States";
                    normalAccount.Password = "123456pwd";
                }
                return normalAccount;
            }
        }

        public enum TestAccountType
        {
            DefaultAccount1,
            DefaultAccount2,
            DefaultAccount3,
            DefaultAccount4
        }

        public class XAuthPersonalInfo : PersonalInfo 
        {
            public string UserName { get; set; }
            public string XAuthPassword { get; set; }

            public XAuthPersonalInfo() { }

            // this List matches the personal info section on confirmation page
            public List<string> ToPersonalInfoSectionList()
            {
                List<string> List = new List<string>();
                string strFormat = "{0} {1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}\r\n{6}, {7} {8}{9}";
                List.Add(string.Format(strFormat, FirstName, LastName,
                    JobTitle,
                    Company,
                    Address1,
                    Address2,
                    City, State, Zip,
                    string.IsNullOrEmpty(Country) ? "" : "\r\n" + Country)
                    .Replace("Colorado", "CO") //Testcase 18
                    .Replace("Alaska", "AK")); //Testcase 19

                List.Add(Phone + (string.IsNullOrEmpty(Extension) ? "" : "  x " + Extension));
                List.Add(Fax);
                List.Add(Email);
                return List;
            }

            // Clone
            public override object Clone()
            {
                XAuthPersonalInfo newPi = (XAuthPersonalInfo)this.MemberwiseClone();
                return newPi;
            }
        }

        public class PersonalInfo : ICloneable
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string JobTitle { get; set; }
            public string Company { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string Phone { get; set; }
            public string Extension { get; set; }
            public string Fax { get; set; }
            public string Country { get; set; }

            // Clone
            public virtual object Clone()
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        [Verify]
        public void XAuth_VerifyMessageToRegistrantPresent(bool present)
        {
            Assert.AreEqual(present, UIUtil.DefaultProvider.IsElementPresent(LocatorFormat_XPath_MessageToRegistration, LocateBy.XPath));
        }
        
        [Verify]
        public void XAuth_VerifyMessageToRegistrant(string expectedMessage)
        {
            XAuth_VerifyMessageToRegistrantPresent(true);
            string actualMessage = UIUtil.DefaultProvider.GetText(LocatorFormat_XPath_MessageToRegistration, LocateBy.XPath);

            if (!expectedMessage.Equals(actualMessage))
            {
                VerifyTool.VerifyValue(expectedMessage, actualMessage, "Message to registrant: {0}");
            }
        }

        public void XAuth_VerifyEmailAddressTextboxContentInLoginPage(string expectedText)
        {
            string actualText = UIUtil.DefaultProvider.GetValue(Locator_Id_XAuthEmaiAddressTextboxInLoginPage, LocateBy.Id);

            if (!expectedText.Equals(actualText))
            {
                VerifyTool.VerifyValue(expectedText, actualText, "Email Address Textbox Content In Login Page: {0}");
            }
        }

        [Verify]
        public void XAuth_VerifyUserIdLabel(string expectedText)
        { 
            XAuth_VerifyUserIdLabelVisibility(true);
            XAuth_VerifyUserIdLabelText(expectedText);
        }

        public void XAuth_VerifyUserIdLabelVisibility(bool isVisble)
        {
            string locator_XPath = string.Format("//label[@for='{0}']", Locator_Id_XAuthUserId);
            VerifyTool.VerifyValue(isVisble, UIUtil.DefaultProvider.IsElementDisplay(locator_XPath, LocateBy.XPath), "UserId label Display: {0}");
        }

        public void XAuth_VerifyUserIdLabelText(string expectedText)
        {
            string locator_XPath = string.Format("//label[@for='{0}']", Locator_Id_XAuthUserId);
            string actualUserIdLabel = UIUtil.DefaultProvider.GetText(locator_XPath, LocateBy.XPath);

            if (!expectedText.Equals(actualUserIdLabel))
            {
                VerifyTool.VerifyValue(expectedText, actualUserIdLabel, "UserId label: {0}");
            }
        }

        public void XAuth_VerifyUserIDTextboxVisibility(bool isVisble)
        {
            VerifyTool.VerifyValue(isVisble, UIUtil.DefaultProvider.IsElementDisplay(Locator_Id_XAuthUserIdTextboxInLoginPage, LocateBy.Id), "UserId TextBox Display: {0}");
        }

        [Step]
        public void XAuth_TypeUserId(string userId)
        {
            UIUtil.DefaultProvider.Type(Locator_Id_XAuthUserId, userId, LocateBy.Id);
        }

        public void XAuth_VerifyPasswordLabelVisibilityInLoginPage(bool isVisble)
        {
            string locator_XPath = string.Format("//label[@for='{0}']", PasswordTextboxOnLoginPage);
            VerifyTool.VerifyValue(isVisble, UIUtil.DefaultProvider.IsElementDisplay(locator_XPath, LocateBy.XPath), "Password Label Display: {0}");
        }

        public void XAuth_VerifyPasswordTextboxVisibilityInLoginPage(bool isVisble)
        {
            VerifyTool.VerifyValue(isVisble, UIUtil.DefaultProvider.IsElementDisplay(PasswordTextboxOnLoginPage, LocateBy.Id), "Password Textbox Display: {0}");
        }

        [Verify]
        public void XAuth_VerifyPasswordSectionPresent(bool present)
        {
            Assert.AreEqual(present, UIUtil.DefaultProvider.IsElementPresent(string.Format(LocatorFormat_XPath_FormSection, "Password"), LocateBy.XPath));
        }

        public void XAuth_VerifySubsituteVisibility(bool isVisble)
        {
            VerifyTool.VerifyValue(isVisble, VerifyHasSubstituteLink(), "Subsitute Display: {0}");
        }

        [Verify]
        public void XAuth_VerifyPI(XAuthPersonalInfo PI)
        {          
            string locator_XPath_EmailSpan=string.Format(LocatorFormat_XPath_PIfield, "Email:");
            string locator_XPath_FirstNameSpan = string.Format(LocatorFormat_XPath_PIfield, "First Name:");
            string locator_XPath_MiddleName = string.Format(LocatorFormat_XPath_PIfield, "Middle Name:");
            string locator_XPath_LastName = string.Format(LocatorFormat_XPath_PIfield, "Last Name:");

            if (!UIUtil.DefaultProvider.IsElementPresent(locator_XPath_EmailSpan, LocateBy.XPath))
                Assert.AreEqual(PI.Email, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl02_sf_txtResponse", LocateBy.Id));
            else
                Assert.AreEqual(PI.Email, UIUtil.DefaultProvider.GetText(locator_XPath_EmailSpan, LocateBy.XPath).Trim());

            if (!UIUtil.DefaultProvider.IsElementPresent(locator_XPath_FirstNameSpan, LocateBy.XPath))
                Assert.AreEqual(PI.FirstName, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl06_sf_txtResponse", LocateBy.Id));
            else
                Assert.AreEqual(PI.FirstName, UIUtil.DefaultProvider.GetText(locator_XPath_FirstNameSpan, LocateBy.XPath).Trim());

            if (!UIUtil.DefaultProvider.IsElementPresent(locator_XPath_MiddleName, LocateBy.XPath))
                Assert.AreEqual(PI.MiddleName, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl07_sf_txtResponse", LocateBy.Id));
            else
                Assert.AreEqual(PI.MiddleName, UIUtil.DefaultProvider.GetText(locator_XPath_MiddleName, LocateBy.XPath).Trim());

            if (!UIUtil.DefaultProvider.IsElementPresent(locator_XPath_LastName, LocateBy.XPath))
                Assert.AreEqual(PI.LastName, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl08_sf_txtResponse", LocateBy.Id));
            else
                Assert.AreEqual(PI.LastName, UIUtil.DefaultProvider.GetText(locator_XPath_LastName, LocateBy.XPath).Trim());


            Assert.AreEqual(PI.Address1, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl14_sf_txtResponse", LocateBy.Id));
            Assert.AreEqual(PI.Address2, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl15_sf_txtResponse", LocateBy.Id));
            Assert.AreEqual(PI.City, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl16_sf_txtResponse", LocateBy.Id));
            Assert.AreEqual(PI.Company, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl12_sf_txtResponse", LocateBy.Id));
            Assert.AreEqual(PI.Extension, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl22_sf_txtResponse", LocateBy.Id));
            Assert.AreEqual(PI.Fax, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl23_sf_txtResponse", LocateBy.Id));
            Assert.AreEqual(PI.JobTitle, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl10_sf_txtResponse", LocateBy.Id));
            Assert.AreEqual(PI.Phone, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl21_sf_txtResponse", LocateBy.Id));
            Assert.AreEqual(PI.State, UIUtil.DefaultProvider.GetSelectedLabel("ctl00_cph_personalInfoStandardFields_rptFields_ctl17_sf_ddlResponse", LocateBy.Id));
            Assert.AreEqual(PI.Zip, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl19_sf_txtResponse", LocateBy.Id));
        }

        public void XAuth_VerifyPINotEqual(XAuthPersonalInfo PI)
        {            
            string locator_XPath_FirstNameSpan = string.Format(LocatorFormat_XPath_PIfield, "First Name:");
            string locator_XPath_MiddleName = string.Format(LocatorFormat_XPath_PIfield, "Middle Name:");
            string locator_XPath_LastName = string.Format(LocatorFormat_XPath_PIfield, "Last Name:");            

            if (!UIUtil.DefaultProvider.IsElementPresent(locator_XPath_FirstNameSpan, LocateBy.XPath))
                Assert.AreNotEqual(PI.FirstName, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl06_sf_txtResponse", LocateBy.Id));
            else
                Assert.AreNotEqual(PI.FirstName, UIUtil.DefaultProvider.GetText(locator_XPath_FirstNameSpan, LocateBy.XPath).Trim());

            if (!UIUtil.DefaultProvider.IsElementPresent(locator_XPath_MiddleName, LocateBy.XPath))
                Assert.AreNotEqual(PI.MiddleName, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl07_sf_txtResponse", LocateBy.Id));
            else
                Assert.AreNotEqual(PI.MiddleName, UIUtil.DefaultProvider.GetText(locator_XPath_MiddleName, LocateBy.XPath).Trim());

            if (!UIUtil.DefaultProvider.IsElementPresent(locator_XPath_LastName, LocateBy.XPath))
                Assert.AreNotEqual(PI.LastName, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl08_sf_txtResponse", LocateBy.Id));
            else
                Assert.AreNotEqual(PI.LastName, UIUtil.DefaultProvider.GetText(locator_XPath_LastName, LocateBy.XPath).Trim());

            Assert.AreNotEqual(PI.Address1, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl14_sf_txtResponse", LocateBy.Id));
            Assert.AreNotEqual(PI.Address2, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl15_sf_txtResponse", LocateBy.Id));
            Assert.AreNotEqual(PI.City, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl16_sf_txtResponse", LocateBy.Id));
            Assert.AreNotEqual(PI.Company, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl12_sf_txtResponse", LocateBy.Id));
            Assert.AreNotEqual(PI.Extension, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl22_sf_txtResponse", LocateBy.Id));
            Assert.AreNotEqual(PI.Fax, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl23_sf_txtResponse", LocateBy.Id));
            Assert.AreNotEqual(PI.JobTitle, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl10_sf_txtResponse", LocateBy.Id));
            Assert.AreNotEqual(PI.Phone, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl21_sf_txtResponse", LocateBy.Id));
            Assert.AreNotEqual(PI.State, UIUtil.DefaultProvider.GetSelectedLabel("ctl00_cph_personalInfoStandardFields_rptFields_ctl17_sf_ddlResponse", LocateBy.Id));
            Assert.AreNotEqual(PI.Zip, UIUtil.DefaultProvider.GetValue("ctl00_cph_personalInfoStandardFields_rptFields_ctl19_sf_txtResponse", LocateBy.Id));
        }

        [Verify]
        public void XAuth_VerifyRegisteredBeforeNoteIsDisplayed(bool displayed)
        {
            Assert.AreEqual(displayed, UIUtil.DefaultProvider.IsElementDisplay("//div[@id='pageContent']/div[@class='pageInstructions']/p[text()='You have registered before with our system. To automatically recall your personal information, enter your password.']", LocateBy.XPath));
        }

        [Verify]
        public void XAuth_VerifyAlreadyBeenUsedErrorMessageIsDisplayed(bool displayed)
        {
            Assert.AreEqual(displayed, UIUtil.DefaultProvider.IsElementDisplay("//div[@id='ctl00_valSummary']/ul/li[text()='Our records show that this email address has already been used to register for this event. Each registrant must use a unique email address.']", LocateBy.XPath));
        }

        [Verify]
        public void XAuth_VerifyStartNewRegistrationLinkIsDisplay(bool displayed)
        {
            Assert.AreEqual(displayed, UIUtil.DefaultProvider.IsElementDisplay("ctl00_cph_lnkNotRegistered", LocateBy.Id));
        }

        [Verify]
        public void XAuth_VerifyEmailAddressFieldValue(string email)
        {
            Assert.AreEqual(email, UIUtil.DefaultProvider.GetValue("ctl00_cph_ctlEmailMemID_txtEmail", LocateBy.Id));
        }

        [Verify]
        public void XAuth_VerifyEmailAddressFieldIsEditable(bool editable)
        {
            Assert.AreEqual(editable, UIUtil.DefaultProvider.IsEditable("ctl00_cph_ctlEmailMemID_txtEmail", LocateBy.Id));
        }

        [Verify]
        public void XAuth_VerifyEmailAddressFieldIsDisplayed(bool displayed)
        {
            Assert.AreEqual(displayed, UIUtil.DefaultProvider.IsElementDisplay("ctl00_cph_ctlEmailMemID_txtEmail", LocateBy.Id));
        }

        [Verify]
        public void XAuth_VerifyPasswordFieldIsDisplayed(bool displayed)
        {
            Assert.AreEqual(displayed, UIUtil.DefaultProvider.IsElementDisplay("ctl00_cph_txtPassword", LocateBy.Id));
        }

        [Verify]
        public void XAuth_VerifyUserNameFieldIsDisplayed(bool displayed)
        {
            Assert.AreEqual(displayed, UIUtil.DefaultProvider.IsElementDisplay("ctl00_cph_txtXAuthUserID", LocateBy.Id));
        }

        [Step]
        public void XAuth_EnterUserName(string userName)
        {
            UIUtil.DefaultProvider.Type("ctl00_cph_txtXAuthUserID", userName, LocateBy.Id);
        }

        /// <summary>
        /// Only type the JobTitle,Company,AddressLineOne,AddressLineTwo,City,State,ZipCode,WorkPhone,Extension,Fax
        /// </summary>
        public void XAuth_SetDefaultStandardPersonalInfoFields(RegisterManager.XAuthPersonalInfo personalInfo,bool isAttendeeExisted)
        {
            SetDefaultStandardPersonalInfoFields((PersonalInfo)personalInfo, isAttendeeExisted);
        }

        public void SetDefaultStandardPersonalInfoFields(PersonalInfo personalInfo, bool isAttendeeExisted)
        {
            if (!isAttendeeExisted)
            {
                if (IsEmailFieldPresent())
                {
                    this.TypePersonalInfoEmail(personalInfo.Email);
                }
                if (IsFirstNameFieldPresent())
                {
                    this.TypePersonalInfoFirstName(personalInfo.FirstName);
                }
                if (IsMiddleNameFieldPresent())
                {
                    this.TypePersonalInfoMiddleName(personalInfo.MiddleName);
                }
                if (IsLastNameFieldPresent())
                {
                    this.TypePersonalInfoLastName(personalInfo.LastName);
                }
            }
            this.TypePersonalInfoJobTitle(personalInfo.JobTitle);
            this.TypePersonalInfoCompany(personalInfo.Company);
            this.TypePersonalInfoAddressLineOne(personalInfo.Address1);
            this.TypePersonalInfoAddressLineTwo(personalInfo.Address2);
            this.TypePersonalInfoCity(personalInfo.City);
            this.SelectPersonalInfoState(personalInfo.State);
            this.TypePersonalInfoZipCode(personalInfo.Zip);
            this.TypePersonalInfoWorkPhone(personalInfo.Phone);
            this.TypePersonalInfoExtension(personalInfo.Extension);
            this.TypePersonalInfoFax(personalInfo.Fax);
        }
        public delegate void SelectRegTypeFunc(FormData.XAuthType xAuthType);
        public void ProcessRegistration(PersonalInfo personalInfo,
            int eventID, FormData.XAuthType xAuthType, SelectRegTypeFunc regTypeFunc = null)
        {
            OpenRegisterPage(eventID);
            CheckinWithEmail(personalInfo.Email);

            if (regTypeFunc != null)
            {
                regTypeFunc(xAuthType);    
            }

            Continue();

            if (OnLoginPage())
            {
                switch (xAuthType)
                {
                    case FormData.XAuthType.ByEmail:
                        break;
                    case FormData.XAuthType.ByEmailPassword:
                        TypeLoginPagePassword(((XAuthPersonalInfo)personalInfo).XAuthPassword);
                        break;
                    case FormData.XAuthType.ByUserName:
                        XAuth_TypeUserId(((XAuthPersonalInfo)personalInfo).UserName);
                        break;
                    case FormData.XAuthType.ByUserNamePassword:
                        XAuth_TypeUserId(((XAuthPersonalInfo)personalInfo).UserName);
                        TypeLoginPagePassword(((XAuthPersonalInfo)personalInfo).XAuthPassword);
                        break;
                    case FormData.XAuthType.NotUse:
                        TypeLoginPagePassword(personalInfo.Password);
                        break;
                    default: break;
                }
                Continue();
            }
            //PI  
            if (xAuthType != FormData.XAuthType.NotUse)
            {
                XAuth_SetDefaultStandardPersonalInfoFields((XAuthPersonalInfo)personalInfo, false);
            }
            else
            {
                SetDefaultStandardPersonalInfoFields(personalInfo, false);
            }
            switch (xAuthType)
            {
                case FormData.XAuthType.ByEmail:
                case FormData.XAuthType.ByUserName:
                    TypePersonalInfoPassword(((XAuthPersonalInfo)personalInfo).XAuthPassword);
                    TypePersonalInfoVerifyPassword(((XAuthPersonalInfo)personalInfo).XAuthPassword);
                    break;
                case FormData.XAuthType.NotUse:
                    TypePersonalInfoPassword(personalInfo.Password);
                    TypePersonalInfoVerifyPassword(personalInfo.Password);
                    break;
                case FormData.XAuthType.ByEmailPassword:
                case FormData.XAuthType.ByUserNamePassword:
                    break;
                default: break;
            }
            Continue();
            
            //Checkout
            //SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            FinishRegistration();
            //ConfirmRegistration();
        }
    }
}
