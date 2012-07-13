namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    public class RegistrantWithXAuthAndNonXauthProfilesFixture : ExternalAuthenticationFixtureBase
    {
        private const string FirstEventNameWithXAuth = "RegistrantWithXAuthAndNonXauthProfilesWithXAuthRegType";
        private const string SecondEventNameWithoutXAuth = "RegistrantWithXAuthAndNonXauthProfilesWithoutXAuthRegType";
        private const string ThirdEventNameWithoutXAuth = "RegistrantWithXAuthAndNonXauthProfilesBoth";

        [Test]
        public void RegistrantWithXAuthAndNonXauthProfilesTest()
        {
            Managers.ManagerProvider.XAuthMgr.RemoveTestRegisterAndAttendeeByCustomerIdEmail(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterManager.XAuthPersonalInfo personalInfo1 = RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1];
            RegisterManager.XAuthPersonalInfo personalInfo2 = (RegisterManager.XAuthPersonalInfo)personalInfo1.Clone();
            personalInfo2.FirstName = personalInfo2.FirstName + "New";
            personalInfo2.MiddleName = personalInfo2.MiddleName + "New";
            personalInfo2.LastName = personalInfo2.LastName + "New";
            personalInfo2.JobTitle = RegisterManager.DefaultPersonalInfo.JobTitle + " New";
            personalInfo2.Address1 = RegisterManager.DefaultPersonalInfo.AddressLineOne + " New";
            personalInfo2.Address2 = RegisterManager.DefaultPersonalInfo.AddressLineTwo + " New";
            personalInfo2.City = personalInfo2.City + " New";
            personalInfo2.Company = RegisterManager.DefaultPersonalInfo.Company + " New";
            personalInfo2.State = "Alabama";
            personalInfo2.Phone = "322.222.2222";
            personalInfo2.Zip = "82222";
            personalInfo2.Extension = "222";
            personalInfo2.Fax = "322.222.2222";

            List<String> errorMessageList = new List<string>();
            errorMessageList.Add("Authentication failure. Please try again.");

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder("xAuth");
            ManagerSiteMgr.DeleteEventByName(FirstEventNameWithXAuth);
            ManagerSiteMgr.DeleteEventByName(SecondEventNameWithoutXAuth);
            ManagerSiteMgr.DeleteEventByName(ThirdEventNameWithoutXAuth);

            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            int firstEventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(FirstEventNameWithXAuth);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1 With XAuth");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyPassTest();
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            int secondEventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(SecondEventNameWithoutXAuth);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype2 No XAuth");
            BuilderMgr.RegTypeMgr.EnableXAuth(false);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            int thirdEventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(ThirdEventNameWithoutXAuth);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype3 With XAuth");
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype3 No XAuth");
            BuilderMgr.RegTypeMgr.EnableXAuth(false);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            //39.2
            RegisterMgr.OpenRegisterPage(firstEventId);
            RegisterMgr.CheckinWithEmail(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            //PI, becasue removing first, so the all textbox can be input.
            RegisterMgr.XAuth_SetDefaultStandardPersonalInfoFields(personalInfo1, false);            
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //39.3,39.4
            RegisterMgr.OpenRegisterPage(secondEventId);
            RegisterMgr.CheckinWithEmail(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            //PI, becasue removing first, so the all textbox can be input.
            RegisterMgr.XAuth_VerifyPINotEqual(personalInfo1);
            RegisterMgr.XAuth_SetDefaultStandardPersonalInfoFields(personalInfo2, false);
            RegisterMgr.TypePersonalInfoPassword("123456");
            RegisterMgr.TypePersonalInfoVerifyPassword("123456");
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //39.5
            RegisterMgr.OpenRegisterPage(thirdEventId);
            RegisterMgr.CheckinWithEmail(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.SelectRegType("regtype3 With XAuth");
            RegisterMgr.Continue();
            if (!RegisterMgr.OnLoginPage())
            {
                Assert.Fail("Not on login page!");
            }
            RegisterMgr.TypeLoginPagePassword("123456");
            RegisterMgr.Continue();
            RegisterMgr.VerifyErrorMessages(errorMessageList);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPI(personalInfo1);
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            ManagerSiteMgr.LoginAndDeleteTestRegsReturnToManagerScreen(thirdEventId, "xAuth");

            //39.6
            RegisterMgr.OpenRegisterPage(thirdEventId);
            RegisterMgr.CheckinWithEmail(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.SelectRegType("regtype3 No XAuth");
            RegisterMgr.Continue();
            if (!RegisterMgr.OnLoginPage())
            {
                Assert.Fail("Not on login page!");
            }
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            errorMessageList.Clear();
            errorMessageList.Add("Your login or password is incorrect. Please try again.");
            RegisterMgr.VerifyErrorMessages(errorMessageList);
            RegisterMgr.TypeLoginPagePassword("123456");
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPI(personalInfo2);
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
    }
}
