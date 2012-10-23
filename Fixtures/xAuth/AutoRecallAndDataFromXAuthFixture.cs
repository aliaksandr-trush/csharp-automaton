namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    public class AutoRecallAndDataFromXAuthFixture : ExternalAuthenticationFixtureBase
    {
        private const string FirstEventNameWithXAuth = "AutoRecallAndDataFromXAuth1";
        private const string SecondEventNameWithoutXAuth = "AutoRecallAndDataFromXAuthWithoutXAuth2";
        private const string ThirdEventNameWithoutXAuth = "AutoRecallAndDataFromXAuthWithoutXAuth3";

        [Test]
        public void AutoRecallAndDataFromXAuthTest()
        {
            //remove current customer's all registers with the same email first
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

            RegisterManager.XAuthPersonalInfo personalInfo3 = (RegisterManager.XAuthPersonalInfo)personalInfo1.Clone();
            personalInfo3.FirstName = personalInfo3.FirstName + "3";
            personalInfo3.MiddleName = personalInfo3.MiddleName + "3";
            personalInfo3.LastName = personalInfo3.LastName + "3";
            personalInfo3.JobTitle = RegisterManager.DefaultPersonalInfo.JobTitle + " 3";
            personalInfo3.Address1 = RegisterManager.DefaultPersonalInfo.AddressLineOne + " 3";
            personalInfo3.Address2 = RegisterManager.DefaultPersonalInfo.AddressLineTwo + " 3";
            personalInfo3.City = personalInfo3.City + "3";
            personalInfo3.Company = RegisterManager.DefaultPersonalInfo.Company + " 3";
            personalInfo3.State = "Arizona";
            personalInfo3.Phone = "303.333.3333";
            personalInfo3.Zip = "33333";
            personalInfo3.Extension = "333";
            personalInfo3.Fax = "333.333.3333";
            
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_UserDefined("xAuth");
            ManagerSiteMgr.DeleteEventByName(FirstEventNameWithXAuth);
            ManagerSiteMgr.DeleteEventByName(SecondEventNameWithoutXAuth);
            ManagerSiteMgr.DeleteEventByName(ThirdEventNameWithoutXAuth);

            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            int firstEventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(FirstEventNameWithXAuth);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(DataCollection.EventData_Common.XAuthType.ByEmail);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(DataCollection.EventData_Common.XAuthType.ByEmail);
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
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.EnableXAuth(false);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            int thirdEventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(ThirdEventNameWithoutXAuth);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.EnableXAuth(false);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();


            //34.1
            RegisterMgr.OpenRegisterPage(firstEventId);
            RegisterMgr.CheckinWithEmail(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            //PI, becasue removing first, so the all textbox can be input.
            RegisterMgr.XAuth_SetDefaultStandardPersonalInfoFields(personalInfo1, false);
            RegisterMgr.TypePersonalInfoPassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.TypePersonalInfoVerifyPassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //34.2
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

            //34.3
            RegisterMgr.OpenRegisterPage(firstEventId);
            RegisterMgr.EnterEmailAddress(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.ClickEditPersonalInformationLink(0);            
            //PI
            RegisterMgr.XAuth_VerifyPI(personalInfo1);
            RegisterMgr.XAuth_VerifyPINotEqual(personalInfo2);
            RegisterMgr.XAuth_SetDefaultStandardPersonalInfoFields(personalInfo3, true);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //34.4
            RegisterMgr.OpenRegisterPage(thirdEventId);
            RegisterMgr.EnterEmailAddress(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword("123456");
            RegisterMgr.Continue();            
            //PI
            RegisterMgr.XAuth_VerifyPI(personalInfo2);
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
    }
}
