namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Backend;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Managers.Register;

    [TestFixture]
    public class CreateEventWithAndWithoutXauthRegTypesAndRelatedRegistrationFixture : ExternalAuthenticationFixtureBase
    {
        string folderName = "xAuth";
        private const string EventName = "XAuth_WithAndWithoutXauthRegTypesAndRelatedRegistration";
        string randomEmailAddressFormat = "test{0}@test.com";
        string password = "123abc123";
        string firstName = "Vincent";
        string lastName = "Guo";
        string middleName = "J";
        string jobTitle = "Developer";
        string addressLineTwo = "Chengdu";
        string fax = "303-987-3524";
        string CFName1 = "CF1_CheckBox";
        string CFName2 = "CF2_CheckBox";
        string CFName3 = "CF_XAuth1_Visible";
        string CFName4 = "CF_NonXAuth2_Visible";
        string CFName5 = "CF_XAuth2_Required";
        string CFName6 = "CF_NonXAuth1_Required";
        string AGName1 = "AG1_CheckBox";
        string AGName2 = "AG2_CheckBox";
        string AGName3 = "AG_XAuth2_Visile";
        string AGName4 = "AG_NonXAuth1_Visile";
        string AGName5 = "AG_XAuth1_Required";
        string AGName6 = "AG_NonXAuth2_Required";

        enum Regtypes
        {
            xAuthType1,
            xAuthType2,
            NonxAuthType1,
            NonxAuthType2
        }

        // TestCase 15
        [TestCase(false)]
        [Test]
        public void CreateEventWithAndWithoutXauthRegTypes(bool onSite)
        {
            CreateEvent(onSite);
        }

        // TestCase 16
        [Test]
        public void RegistrationWithNonXAuthRegtype()
        {
            List<string> IDs = CreateEvent(false);
            int eventID = Convert.ToInt32(IDs[1]);
            string eventSessionID = IDs[0];
            ChangeAndResetXAuthConfiguration(eventID, eventSessionID, Regtypes.xAuthType1.ToString());

            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin(string.Format(randomEmailAddressFormat, Guid.NewGuid().ToString().Replace("-", "")));
            RegisterMgr.SelectRegType(Regtypes.NonxAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(false);
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.EnterPersonalInfoPassword(password);
            RegisterMgr.SetCustomFieldCheckBox(CFName6, true);

            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();
            RegisterMgr.ClickChangeMyRegistration();
            Assert.IsTrue(RegisterMgr.OnLoginPage());
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(false);
            RegisterMgr.TypeLoginPagePassword(password);
            RegisterMgr.Continue();
            Assert.IsTrue(RegisterMgr.OnAttendeeCheckPage());
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
        }

        // TestCase 17
        [TestCase(false)]
        [Test]
        public void GroupRegistrationWithSameXAuthRegtype(bool onSite)
        {
            List<string> IDs = CreateEvent(onSite);
            int eventID = Convert.ToInt32(IDs[1]);
            string eventSessionID = IDs[0];
            ChangeAndResetXAuthConfiguration(eventID, eventSessionID, Regtypes.xAuthType1.ToString());

            if (onSite)
                RegisterMgr.OpenOnSiteRegisterPage(eventID);
            else
                RegisterMgr.OpenRegisterPage(eventID);

            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.SelectRegType(Regtypes.xAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(true);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
            RegisterMgr.XAuth_VerifyPI(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName5);
            RegisterMgr.Continue();
            RegisterMgr.Continue();

            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.Checkin(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount2].Email);
            RegisterMgr.SelectRegType(Regtypes.xAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(true);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
            RegisterMgr.XAuth_VerifyPI(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount2]);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName5);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();

            if (!onSite)
            {
                RegisterMgr.UnfoldGroupMember(0);
                RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
                RegisterMgr.VerifyGroupMemberPersonalInfo(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount2], 0);
            }
        }

        // TestCase 18, P1
        [TestCase(false)]
        [Test]
        public void GroupRegistrationWithDifferentXAuthRegTypes(bool onSite)
        {
            List<string> IDs = CreateEvent(onSite);
            int eventID = Convert.ToInt32(IDs[1]);
            string eventSessionID = IDs[0];
            ChangeAndResetXAuthConfiguration(eventID, eventSessionID, Regtypes.xAuthType1.ToString());


            // Part 1. Group Registration With Different XAuth Regtypes
            if (onSite)
                RegisterMgr.OpenOnSiteRegisterPage(eventID);
            else
                RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.SelectRegType(Regtypes.xAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(true);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
            RegisterMgr.XAuth_VerifyPI(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName5);
            RegisterMgr.Continue();

            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.Checkin(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount2].Email);
            RegisterMgr.SelectRegType(Regtypes.xAuthType2.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(true);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
            RegisterMgr.XAuth_VerifyPI(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount2]);
            RegisterMgr.SetCustomFieldCheckBox(CFName5, true);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();

            if (!onSite)
            {
                RegisterMgr.UnfoldGroupMember(0);
                RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
                RegisterMgr.VerifyGroupMemberPersonalInfo(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount2], 0);
            }

        }

        // TestCase 18, P2
        [TestCase(false)]
        [Test]
        public void GroupRegistrationWithXAuthAndNonXAuthRegTypes(bool onSite)
        {
            List<string> IDs = CreateEvent(onSite);
            int eventID = Convert.ToInt32(IDs[1]);
            string eventSessionID = IDs[0];
            ChangeAndResetXAuthConfiguration(eventID, eventSessionID, Regtypes.xAuthType1.ToString());

            // Part 2. Group Registration With XAuth And NonXAuth RegTypes
            if (onSite)
                RegisterMgr.OpenOnSiteRegisterPage(eventID);
            else
                RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.SelectRegType(Regtypes.xAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(true);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
            RegisterMgr.XAuth_VerifyPI(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName5);
            RegisterMgr.Continue();

            string randomEmail = string.Format(randomEmailAddressFormat, Guid.NewGuid().ToString().Replace("-", ""));
            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.Checkin(randomEmail);
            RegisterMgr.SelectRegType(Regtypes.NonxAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(false);
            RegisterManager.XAuthPersonalInfo primaryInfo = 
                (RegisterManager.XAuthPersonalInfo)RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1].Clone();
            primaryInfo.Email = randomEmail;
            primaryInfo.FirstName = "";
            primaryInfo.LastName = "";
            primaryInfo.MiddleName = "";
            primaryInfo.JobTitle = "";
            RegisterMgr.XAuth_VerifyPI(primaryInfo);
            RegisterMgr.TypePersonalInfoFirstName(firstName);
            RegisterMgr.TypePersonalInfoLastName(lastName);
            RegisterMgr.TypePersonalInfoMiddleName(middleName);
            RegisterMgr.TypePersonalInfoJobTitle(jobTitle);
            RegisterMgr.EnterPersonalInfoPassword(password);
            RegisterMgr.SetCustomFieldCheckBox(CFName6, true);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();

            if (!onSite)
            {
                RegisterMgr.UnfoldGroupMember(0);
                RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);

                primaryInfo.Email = randomEmail;
                primaryInfo.FirstName = firstName;
                primaryInfo.LastName = lastName;
                primaryInfo.MiddleName = middleName;
                primaryInfo.JobTitle = jobTitle;
                RegisterMgr.VerifyGroupMemberPersonalInfo(primaryInfo, 0);
            }
        }

        // TestCase 18, P3
        [TestCase(false)]
        [Test]
        public void GroupRegistrationWithNonXAuthAndXAuthRegTypes(bool onSite)
        {
            List<string> IDs = CreateEvent(onSite);
            int eventID = Convert.ToInt32(IDs[1]);
            string eventSessionID = IDs[0];
            ChangeAndResetXAuthConfiguration(eventID, eventSessionID, Regtypes.xAuthType1.ToString());

            // Part 3. Group Registration With NonXAuth And XAuth RegTypes
            if (onSite)
                RegisterMgr.OpenOnSiteRegisterPage(eventID);
            else
                RegisterMgr.OpenRegisterPage(eventID);
            string randomEmail = string.Format(randomEmailAddressFormat, Guid.NewGuid().ToString().Replace("-", ""));
            RegisterMgr.Checkin(randomEmail);
            RegisterMgr.SelectRegType(Regtypes.NonxAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(false);
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.TypePersonalInfoAddressLineTwo(addressLineTwo);
            RegisterMgr.TypePersonalInfoFax(fax);
            RegisterMgr.EnterPersonalInfoPassword(password);
            RegisterMgr.SetCustomFieldCheckBox(CFName6, true);
            RegisterMgr.Continue();
            RegisterMgr.Continue();

            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.SelectRegType(Regtypes.xAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(true);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
            RegisterMgr.XAuth_VerifyPI(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName5);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();


            if (!onSite)
            {
                RegisterMgr.UnfoldGroupMember(0);

                RegisterManager.XAuthPersonalInfo primaryInfo = new RegisterManager.XAuthPersonalInfo();
                primaryInfo.Email = randomEmail;
                primaryInfo.FirstName = RegisterManager.DefaultPersonalInfo.FirstName;
                primaryInfo.LastName = RegisterMgr.CurrentRegistrantLastName;
                primaryInfo.MiddleName = RegisterManager.DefaultPersonalInfo.MiddleName;
                primaryInfo.JobTitle = RegisterManager.DefaultPersonalInfo.JobTitle;
                primaryInfo.Address1 = RegisterManager.DefaultPersonalInfo.AddressLineOne;
                primaryInfo.Address2 = addressLineTwo;
                primaryInfo.City = RegisterManager.DefaultPersonalInfo.City;
                primaryInfo.Company = RegisterManager.DefaultPersonalInfo.Company;
                primaryInfo.State = RegisterManager.DefaultPersonalInfo.State;
                primaryInfo.Phone = RegisterManager.DefaultPersonalInfo.WorkPhone;
                primaryInfo.Zip = RegisterManager.DefaultPersonalInfo.ZipCode;
                primaryInfo.Fax = fax;
                RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(primaryInfo);
                RegisterMgr.VerifyGroupMemberPersonalInfo(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1], 0);
            }
        }

        // TestCase 18, P4
        [TestCase(false)]
        [Test]
        public void GroupRegistrationWithDifferentNonXAuthRegTypes(bool onSite)
        {
            List<string> IDs = CreateEvent(onSite);
            int eventID = Convert.ToInt32(IDs[1]);
            string eventSessionID = IDs[0];
            ChangeAndResetXAuthConfiguration(eventID, eventSessionID, Regtypes.xAuthType1.ToString());

            // Part 4. Group Registration With Different NonXAuth RegTypes
            if (onSite)
                RegisterMgr.OpenOnSiteRegisterPage(eventID);
            else
                RegisterMgr.OpenRegisterPage(eventID);
            string randomEmail = string.Format(randomEmailAddressFormat, Guid.NewGuid().ToString().Replace("-", ""));
            RegisterMgr.Checkin(randomEmail);
            RegisterMgr.SelectRegType(Regtypes.NonxAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(false);
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.TypePersonalInfoFax(fax);
            RegisterMgr.TypePersonalInfoAddressLineTwo(addressLineTwo);
            RegisterMgr.EnterPersonalInfoPassword(password);
            RegisterMgr.SetCustomFieldCheckBox(CFName6, true);
            RegisterMgr.Continue();
            RegisterMgr.Continue();

            RegisterMgr.ClickAddAnotherPerson();
            string randomEmail2 = string.Format(randomEmailAddressFormat, Guid.NewGuid().ToString().Replace("-", ""));
            RegisterMgr.Checkin(randomEmail2);
            RegisterMgr.SelectRegType(Regtypes.NonxAuthType2.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(false);
            RegisterManager.XAuthPersonalInfo secondaryInfo = new RegisterManager.XAuthPersonalInfo();
            secondaryInfo.Email = randomEmail2;
            secondaryInfo.FirstName = "";
            secondaryInfo.LastName = "";
            secondaryInfo.MiddleName = "";
            secondaryInfo.JobTitle = "";
            secondaryInfo.Address1 = RegisterManager.DefaultPersonalInfo.AddressLineOne;
            secondaryInfo.Address2 = addressLineTwo;
            secondaryInfo.City = RegisterManager.DefaultPersonalInfo.City;
            secondaryInfo.Company = RegisterManager.DefaultPersonalInfo.Company;
            secondaryInfo.State = RegisterManager.DefaultPersonalInfo.State;
            secondaryInfo.Phone = RegisterManager.DefaultPersonalInfo.WorkPhone;
            secondaryInfo.Extension = "";
            secondaryInfo.Zip = RegisterManager.DefaultPersonalInfo.ZipCode;
            secondaryInfo.Fax = fax;
            RegisterMgr.XAuth_VerifyPI(secondaryInfo);
            RegisterMgr.TypePersonalInfoFirstName(firstName);
            RegisterMgr.TypePersonalInfoLastName(lastName);
            RegisterMgr.TypePersonalInfoMiddleName(middleName);
            RegisterMgr.TypePersonalInfoJobTitle(jobTitle);
            secondaryInfo.FirstName = firstName;
            secondaryInfo.LastName = lastName;
            secondaryInfo.MiddleName = middleName;
            secondaryInfo.JobTitle = jobTitle;
            RegisterMgr.EnterPersonalInfoPassword(password);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName6);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            
            RegisterMgr.VerifyOnConfirmationPage();

            if (!onSite)
            {
                RegisterMgr.UnfoldGroupMember(0);
                RegisterManager.XAuthPersonalInfo primaryInfo = (RegisterManager.XAuthPersonalInfo)secondaryInfo.Clone();
                primaryInfo.Email = randomEmail;
                primaryInfo.FirstName = RegisterManager.DefaultPersonalInfo.FirstName;
                primaryInfo.LastName = RegisterMgr.CurrentRegistrantLastName;
                primaryInfo.MiddleName = RegisterManager.DefaultPersonalInfo.MiddleName;
                primaryInfo.JobTitle = RegisterManager.DefaultPersonalInfo.JobTitle;
                RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(primaryInfo);
                RegisterMgr.VerifyGroupMemberPersonalInfo(secondaryInfo, 0);
            }
        }

        [TestCase(false)]
        [Test]
        // TestCase 18, P5
        public void GroupRegistrationWithAllFourDifferentRegTypes(bool onSite)
        {
            List<string> IDs = CreateEvent(onSite);
            int eventID = Convert.ToInt32(IDs[1]);
            string eventSessionID = IDs[0];
            ChangeAndResetXAuthConfiguration(eventID, eventSessionID, Regtypes.xAuthType1.ToString());

            // Part 5. Group Registration With All Four Different RegTypes
            if (onSite)
                RegisterMgr.OpenOnSiteRegisterPage(eventID);
            else
                RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.SelectRegType(Regtypes.xAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(true);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
            RegisterMgr.XAuth_VerifyPI(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName5);
            RegisterMgr.Continue();

            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.Checkin(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount2].Email);
            RegisterMgr.SelectRegType(Regtypes.xAuthType2.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(true);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
            RegisterMgr.XAuth_VerifyPI(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount2]);
            RegisterMgr.SetCustomFieldCheckBox(CFName5, true);
            RegisterMgr.Continue();
            RegisterMgr.Continue();

            RegisterMgr.ClickAddAnotherPerson();
            string randomEmail = string.Format(randomEmailAddressFormat, Guid.NewGuid().ToString().Replace("-", ""));
            RegisterMgr.Checkin(randomEmail);
            RegisterMgr.SelectRegType(Regtypes.NonxAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(false);
            RegisterManager.XAuthPersonalInfo thirdMemberInfo =
                (RegisterManager.XAuthPersonalInfo)RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount2].Clone();
            thirdMemberInfo.Email = randomEmail;
            thirdMemberInfo.FirstName = "";
            thirdMemberInfo.LastName = "";
            thirdMemberInfo.MiddleName = "";
            thirdMemberInfo.JobTitle = "";
            RegisterMgr.XAuth_VerifyPI(thirdMemberInfo);
            RegisterMgr.TypePersonalInfoFirstName(firstName);
            RegisterMgr.TypePersonalInfoLastName(lastName);
            RegisterMgr.TypePersonalInfoMiddleName(middleName);
            RegisterMgr.TypePersonalInfoJobTitle(jobTitle);
            RegisterMgr.EnterPersonalInfoPassword(password);
            thirdMemberInfo.FirstName = firstName;
            thirdMemberInfo.LastName = lastName;
            thirdMemberInfo.MiddleName = middleName;
            thirdMemberInfo.JobTitle = jobTitle;
            RegisterMgr.SetCustomFieldCheckBox(CFName6, true);
            RegisterMgr.Continue();
            RegisterMgr.Continue();

            RegisterMgr.ClickAddAnotherPerson();
            string randomEmail2 = string.Format(randomEmailAddressFormat, Guid.NewGuid().ToString().Replace("-", ""));
            RegisterMgr.Checkin(randomEmail2);
            RegisterMgr.SelectRegType(Regtypes.NonxAuthType2.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(false);
            RegisterManager.XAuthPersonalInfo fourthMemberInfo =
                (RegisterManager.XAuthPersonalInfo)RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount2].Clone();
            fourthMemberInfo.Email = randomEmail2;
            fourthMemberInfo.FirstName = "";
            fourthMemberInfo.LastName = "";
            fourthMemberInfo.MiddleName = "";
            fourthMemberInfo.JobTitle = "";
            RegisterMgr.XAuth_VerifyPI(fourthMemberInfo);
            RegisterMgr.TypePersonalInfoFirstName(firstName + "2");
            RegisterMgr.TypePersonalInfoLastName(lastName + "2");
            RegisterMgr.TypePersonalInfoMiddleName(middleName + "2");
            RegisterMgr.TypePersonalInfoJobTitle(jobTitle + "2");
            RegisterMgr.EnterPersonalInfoPassword(password);
            fourthMemberInfo.FirstName = firstName + "2";
            fourthMemberInfo.LastName = lastName + "2";
            fourthMemberInfo.MiddleName = middleName + "2";
            fourthMemberInfo.JobTitle = jobTitle + "2";
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName6);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();

            if (!onSite)
            {
                RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
                RegisterMgr.UnfoldGroupMember(0);
                RegisterMgr.VerifyGroupMemberPersonalInfo(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount2], 0);
                RegisterMgr.UnfoldGroupMember(1);
                RegisterMgr.VerifyGroupMemberPersonalInfo(thirdMemberInfo, 1);
                RegisterMgr.UnfoldGroupMember(2);
                RegisterMgr.VerifyGroupMemberPersonalInfo(fourthMemberInfo, 2);
            }
        }

        [Test]
        // TestCase 20 P1
        public void ChangeXAuthRegistrationTypeDuringUpdate()
        {
            List<string> IDs = CreateEvent(false);
            int eventID = Convert.ToInt32(IDs[1]);
            string eventSessionID = IDs[0];
            ChangeAndResetXAuthConfiguration(eventID, eventSessionID, Regtypes.xAuthType1.ToString());
            ManagerSiteMgr.OpenEventBuilderStartPage(eventID, eventSessionID);
            BuilderMgr.SetEventAllowChangeRegistrationType(true);
            BuilderMgr.SetEventAllowUpdateRegistration(true);
            BuilderMgr.SaveAndClose();

            // Do a XAuth registration
            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.SelectRegType(Regtypes.xAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(true);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
            RegisterMgr.XAuth_VerifyPI(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName5);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();
            RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
            RegisterMgr.VerifyRegistrationType(Regtypes.xAuthType1.ToString());

            // Update Registration Type
            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterEmailAddress(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.ContinueWithErrors();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.AttendeeCheckEditPersonalInfo();
            RegisterMgr.ClickEditRegistrationType();
            RegisterMgr.VerifyRegistrationTypeOptions(new List<string> { Regtypes.xAuthType1.ToString(), Regtypes.xAuthType2.ToString() });
            RegisterMgr.ChangeRegistrationType(Regtypes.xAuthType2.ToString());
            RegisterMgr.ConfirmChangingRegistrationType();
            RegisterMgr.TypePersonalInfoFax(fax);
            RegisterMgr.SetCustomFieldCheckBox(CFName5, true);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();
            RegisterManager.XAuthPersonalInfo modifiedInfo = (RegisterManager.XAuthPersonalInfo)RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1].Clone();
            modifiedInfo.Fax = fax;
            RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(modifiedInfo);
            RegisterMgr.VerifyRegistrationType(Regtypes.xAuthType2.ToString());

        }

        [Test]
        // TestCase 20 P2
        public void ChangeNonXAuthRegistrationTypeDuringUpdate()
        {
            List<string> IDs = CreateEvent(false);
            int eventID = Convert.ToInt32(IDs[1]);
            string eventSessionID = IDs[0];
            ChangeAndResetXAuthConfiguration(eventID, eventSessionID, Regtypes.xAuthType1.ToString());
            ManagerSiteMgr.OpenEventBuilderStartPage(eventID, eventSessionID);
            BuilderMgr.SetEventAllowChangeRegistrationType(true);
            BuilderMgr.SetEventAllowUpdateRegistration(true);
            BuilderMgr.SaveAndClose();

            // Do a NonXAuth registration
            string randomEmail = string.Format(randomEmailAddressFormat, Guid.NewGuid().ToString().Replace("-", ""));
            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin(randomEmail);
            RegisterMgr.SelectRegType(Regtypes.NonxAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(false);
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.TypePersonalInfoFax(fax);
            RegisterMgr.TypePersonalInfoAddressLineTwo(addressLineTwo);
            RegisterMgr.EnterPersonalInfoPassword(password);
            RegisterMgr.SetCustomFieldCheckBox(CFName6, true);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();
            RegisterMgr.VerifyRegistrationType(Regtypes.NonxAuthType1.ToString());
            RegisterManager.XAuthPersonalInfo personalInfo = new RegisterManager.XAuthPersonalInfo();
            personalInfo.Email = randomEmail;
            personalInfo.FirstName = RegisterManager.DefaultPersonalInfo.FirstName;
            personalInfo.LastName = RegisterMgr.CurrentRegistrantLastName;
            personalInfo.MiddleName = RegisterManager.DefaultPersonalInfo.MiddleName;
            personalInfo.JobTitle = RegisterManager.DefaultPersonalInfo.JobTitle;
            personalInfo.Address1 = RegisterManager.DefaultPersonalInfo.AddressLineOne;
            personalInfo.Address2 = addressLineTwo;
            personalInfo.City = RegisterManager.DefaultPersonalInfo.City;
            personalInfo.Company = RegisterManager.DefaultPersonalInfo.Company;
            personalInfo.State = RegisterManager.DefaultPersonalInfo.State;
            personalInfo.Phone = RegisterManager.DefaultPersonalInfo.WorkPhone;
            personalInfo.Extension = "";
            personalInfo.Zip = RegisterManager.DefaultPersonalInfo.ZipCode;
            personalInfo.Fax = fax;
            RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(personalInfo);

            // Update Registration Type
            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterEmailAddress(randomEmail);
            RegisterMgr.ContinueWithErrors();
            RegisterMgr.TypeLoginPagePassword(password);
            RegisterMgr.Continue();
            RegisterMgr.AttendeeCheckEditPersonalInfo();
            RegisterMgr.ClickEditRegistrationType();
            RegisterMgr.VerifyRegistrationTypeOptions(new List<string> { Regtypes.NonxAuthType1.ToString(), Regtypes.NonxAuthType2.ToString() });
            RegisterMgr.ChangeRegistrationType(Regtypes.NonxAuthType2.ToString());
            RegisterMgr.ConfirmChangingRegistrationType();
            RegisterMgr.TypePersonalInfoJobTitle("QA Tester");
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName6);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();
            RegisterMgr.VerifyRegistrationType(Regtypes.NonxAuthType2.ToString());
            personalInfo.JobTitle = "QA Tester";
            RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(personalInfo);

        }

        [Test]
        //[SpiraTestCase(139)]
        // TestCase 23
        public void XAuthAndNonXAuthRegistrationsBackendUpdate()
        {
            List<string> IDs = CreateEvent(false);
            int eventID = Convert.ToInt32(IDs[1]);
            string eventSessionID = IDs[0];
            ChangeAndResetXAuthConfiguration(eventID, eventSessionID, Regtypes.xAuthType1.ToString());
            int regID1;
            int regID2;
            
            // Do registrations
            // Registration with XAuth
            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.SelectRegType(Regtypes.xAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(true);
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
            RegisterMgr.XAuth_VerifyPI(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
            RegisterMgr.SetCustomFieldCheckBox(CFName1, true);
            RegisterMgr.SetCustomFieldCheckBox(CFName2, true);
            RegisterMgr.SetCustomFieldCheckBox(CFName3, true);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName1);
            RegisterMgr.SelectAgendaItem(AGName2);
            RegisterMgr.SelectAgendaItem(AGName5);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();
            RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1]);
            regID1 = RegisterMgr.GetRegistrationIdOnConfirmationPage();
            // Two registrations with NonXAuth
            RegisterMgr.OpenRegisterPage(eventID);
            string randomEmail = string.Format(randomEmailAddressFormat, Guid.NewGuid().ToString().Replace("-", ""));
            RegisterMgr.Checkin(randomEmail);
            RegisterMgr.SelectRegType(Regtypes.NonxAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(false);
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.TypePersonalInfoAddressLineTwo(addressLineTwo);
            RegisterMgr.TypePersonalInfoFax(fax);
            RegisterMgr.EnterPersonalInfoPassword(password);
            RegisterMgr.SetCustomFieldCheckBox(CFName1, true);
            RegisterMgr.SetCustomFieldCheckBox(CFName2, true);
            RegisterMgr.SetCustomFieldCheckBox(CFName6, true);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName1);
            RegisterMgr.SelectAgendaItem(AGName2);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();
            RegisterManager.XAuthPersonalInfo personalInfo = new RegisterManager.XAuthPersonalInfo();
            personalInfo.Email = randomEmail;
            personalInfo.FirstName = RegisterManager.DefaultPersonalInfo.FirstName;
            personalInfo.LastName = RegisterMgr.CurrentRegistrantLastName;
            personalInfo.MiddleName = RegisterManager.DefaultPersonalInfo.MiddleName;
            personalInfo.JobTitle = RegisterManager.DefaultPersonalInfo.JobTitle;
            personalInfo.Address1 = RegisterManager.DefaultPersonalInfo.AddressLineOne;
            personalInfo.Address2 = addressLineTwo;
            personalInfo.City = RegisterManager.DefaultPersonalInfo.City;
            personalInfo.Company = RegisterManager.DefaultPersonalInfo.Company;
            personalInfo.State = RegisterManager.DefaultPersonalInfo.State;
            personalInfo.Phone = RegisterManager.DefaultPersonalInfo.WorkPhone;
            personalInfo.Extension = "";
            personalInfo.Zip = RegisterManager.DefaultPersonalInfo.ZipCode;
            personalInfo.Fax = fax;
            RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(personalInfo);
            regID2 = RegisterMgr.GetRegistrationIdOnConfirmationPage();

            RegisterMgr.OpenRegisterPage(eventID);
            string randomEmail2 = string.Format(randomEmailAddressFormat, Guid.NewGuid().ToString().Replace("-", ""));
            RegisterMgr.Checkin(randomEmail2);
            RegisterMgr.SelectRegType(Regtypes.NonxAuthType1.ToString());
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
            RegisterMgr.XAuth_VerifyMessageToRegistrantPresent(false);
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.TypePersonalInfoAddressLineTwo(addressLineTwo);
            RegisterMgr.TypePersonalInfoFax(fax);
            RegisterMgr.EnterPersonalInfoPassword(password);
            RegisterMgr.SetCustomFieldCheckBox(CFName6, true);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(AGName1);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.VerifyOnConfirmationPage();
            
            // Backend Update
            // Update registration with XAuth
            BackendMgr.OpenAttendeeInfoURL(eventSessionID, regID1);
            
            BackendMgr.OpenEditAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.PersonalInformation);
            BackendMgr.VerifyRegistrationTypeOptions(new List<string> { Regtypes.xAuthType1.ToString(), Regtypes.xAuthType2.ToString() });
            BackendMgr.SetFieldValue(BackendManager.PersonalInfoEditField.RegType, Regtypes.xAuthType2.ToString());
            BackendMgr.SaveAndCloseEditPersonalInformation();
            BackendMgr.SelectAttendeeInfoWindow();
            BackendMgr.VerifyCustomFieldPresent(CFName1, true);
            BackendMgr.VerifyCustomFieldPresent(CFName2, true);
            BackendMgr.VerifyCustomFieldPresent(CFName3, true);
            BackendMgr.VerifyCustomFieldPresent(AGName1, true);
            BackendMgr.VerifyCustomFieldPresent(AGName2, true);
            BackendMgr.VerifyCustomFieldPresent(AGName5, true);

            BackendMgr.OpenEditAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.PersonalInformation);
            BackendMgr.SetFieldValue(BackendManager.PersonalInfoEditField.Email, randomEmail);
            BackendMgr.SaveAndCloseEditPersonalInformation();
            BackendMgr.VerifyErrorMessage(Managers.Register.RegisterManager.Error.CheckinDisallowDuplicateEmail);
            BackendMgr.SetFieldValue(BackendManager.PersonalInfoEditField.Email, Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            BackendMgr.SaveAndCloseEditPersonalInformation();
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.OpenEditAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.PersonalInformation);
            BackendMgr.SetFieldValue(BackendManager.PersonalInfoEditField.Email, "dev1@regonline.com");
            BackendMgr.SetFieldValue(BackendManager.PersonalInfoEditField.First_Name, "Dev");
            BackendMgr.SetFieldValue(BackendManager.PersonalInfoEditField.Last_Name, "One");
            BackendMgr.SaveAndCloseEditPersonalInformation();
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.OpenEditAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.CustomFields);
            BackendMgr.SetCheckboxCFItem(CFName1, false);
            BackendMgr.SaveAndCloseEditCF();
            BackendMgr.SelectAttendeeInfoWindow();
            BackendMgr.VerifyCustomFieldPresent(CFName1, false);

            // Update registration with NonXAuth
            BackendMgr.OpenAttendeeInfoURL(eventSessionID, regID2);

            BackendMgr.OpenEditAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.PersonalInformation);
            BackendMgr.VerifyRegistrationTypeOptions(new List<string> { Regtypes.NonxAuthType1.ToString(), Regtypes.NonxAuthType2.ToString() });
            BackendMgr.SetFieldValue(BackendManager.PersonalInfoEditField.RegType, Regtypes.NonxAuthType2.ToString());
            BackendMgr.SaveAndCloseEditPersonalInformation();
            BackendMgr.SelectAttendeeInfoWindow();
            BackendMgr.VerifyCustomFieldPresent(CFName1, true);
            BackendMgr.VerifyCustomFieldPresent(CFName2, true);
            BackendMgr.VerifyCustomFieldPresent(CFName6, true);
            BackendMgr.VerifyCustomFieldPresent(AGName1, true);
            BackendMgr.VerifyCustomFieldPresent(AGName2, true);

            string randomEmail3 = string.Format(randomEmailAddressFormat, Guid.NewGuid().ToString().Replace("-", ""));
            BackendMgr.OpenEditAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.PersonalInformation);
            BackendMgr.SetFieldValue(BackendManager.PersonalInfoEditField.Email, randomEmail3);
            BackendMgr.SaveAndCloseEditPersonalInformation();
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.OpenEditAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.PersonalInformation);
            BackendMgr.SetFieldValue(BackendManager.PersonalInfoEditField.Email, "dev2@regonline.com");
            BackendMgr.SetFieldValue(BackendManager.PersonalInfoEditField.First_Name, "Dev");
            BackendMgr.SetFieldValue(BackendManager.PersonalInfoEditField.Last_Name, "Two");
            BackendMgr.SaveAndCloseEditPersonalInformation();
            BackendMgr.ConfirmWhenSaveAndCloseEditPI(BackendManager.ConfirmWhenSaveEditPI.Correction);
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.OpenEditAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.CustomFields);
            BackendMgr.SetCheckboxCFItem(CFName6, false);
            BackendMgr.SaveAndCloseEditCF();
            BackendMgr.SelectAttendeeInfoWindow();
            BackendMgr.VerifyCustomFieldPresent(CFName6, false);
        }

        private void SetupRegtypes(string regTypeName, bool enableXAuth, DataCollection.EventData_Common.XAuthType xAuthType)
        {
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(regTypeName);
            if (enableXAuth)
            {
                BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();
                Managers.ManagerProvider.XAuthMgr.SetXAuthType(xAuthType);
                Managers.ManagerProvider.XAuthMgr.TypeForgetPasswordUrl("http://www.regonline.com");
                Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(xAuthType);
                Managers.ManagerProvider.XAuthMgr.TypeMessageToRegistration("This is XAuth Event.");
                Managers.ManagerProvider.XAuthMgr.ClickOKButton();

                BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            }
            BuilderMgr.RegTypeMgr.EnableXAuth(enableXAuth);
            BuilderMgr.RegTypeMgr.SaveAndClose();
        }

        [Step]
        private List<string> CreateEvent(bool onSite)
        {
            List<string> ret = new List<string>();
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ret.Add(ManagerSiteMgr.GetEventSessionId());
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_UserDefined(folderName);
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (ManagerSiteMgr.EventExists(EventName))
            {
                int eventId = ManagerSiteMgr.GetFirstEventId(EventName);
                DataHelperTool.ChangeAllRegsToTestAndDelete(eventId);
                Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();
                ret.Add(eventId.ToString());
            }
            else
            {
                #region Setup Event

                ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
                int eventID = BuilderMgr.GetEventId();
                BuilderMgr.SetEventNameAndShortcut(EventName);

                // reg type 1 with xAuth
                SetupRegtypes(Regtypes.xAuthType1.ToString(), true, DataCollection.EventData_Common.XAuthType.ByEmailPassword);

                // reg type 2 with xAuth
                SetupRegtypes(Regtypes.xAuthType2.ToString(), true, DataCollection.EventData_Common.XAuthType.ByEmailPassword);

                // reg type 3 without xAuth
                SetupRegtypes(Regtypes.NonxAuthType1.ToString(), false, DataCollection.EventData_Common.XAuthType.NotUse);

                // reg type 4 without xAuth
                SetupRegtypes(Regtypes.NonxAuthType2.ToString(), false, DataCollection.EventData_Common.XAuthType.NotUse);

                BuilderMgr.SaveAndStay();
                BuilderMgr.Next();
                BuilderMgr.Previous();

                BuilderMgr.SetGroupRegistration(true);
                BuilderMgr.SetEventsForceSameRegTypes(false);            

                // Add some custom fields
                BuilderMgr.GotoPage(FormDetailManager.Page.PI);

                BuilderMgr.ClickAddPICustomField();
                BuilderMgr.CFMgr.SetName(CFName1);
                BuilderMgr.CFMgr.SaveAndClose();

                BuilderMgr.ClickAddPICustomField();
                BuilderMgr.CFMgr.SetName(CFName2);
                BuilderMgr.CFMgr.SaveAndClose();                
                
                BuilderMgr.ClickAddPICustomField();
                BuilderMgr.CFMgr.SetName(CFName3);
                BuilderMgr.CFMgr.ShowAllRegTypes();
                BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Visible, Regtypes.xAuthType1.ToString());
                BuilderMgr.CFMgr.SaveAndClose();

                BuilderMgr.ClickAddPICustomField();
                BuilderMgr.CFMgr.SetName(CFName4);
                BuilderMgr.CFMgr.ShowAllRegTypes();
                BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Visible, Regtypes.NonxAuthType2.ToString());
                BuilderMgr.CFMgr.SaveAndClose();

                BuilderMgr.ClickAddPICustomField();
                BuilderMgr.CFMgr.SetName(CFName5);
                BuilderMgr.CFMgr.ShowAllRegTypes();
                BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Required, Regtypes.xAuthType2.ToString());
                BuilderMgr.CFMgr.SaveAndClose();

                BuilderMgr.ClickAddPICustomField();
                BuilderMgr.CFMgr.SetName(CFName6);
                BuilderMgr.CFMgr.ShowAllRegTypes();
                BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Required, Regtypes.NonxAuthType1.ToString());
                BuilderMgr.CFMgr.SaveAndClose();

                // Add some agenda fields
                BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
                BuilderMgr.ClickYesOnSplashPage();

                BuilderMgr.ClickAddAgendaItem();
                BuilderMgr.AGMgr.SetName(AGName1);
                BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(10);
                BuilderMgr.AGMgr.ClickSaveItem();

                BuilderMgr.ClickAddAgendaItem();
                BuilderMgr.AGMgr.SetName(AGName2);
                BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(20);
                BuilderMgr.AGMgr.ClickSaveItem();

                BuilderMgr.ClickAddAgendaItem();
                BuilderMgr.AGMgr.SetName(AGName3);
                BuilderMgr.AGMgr.ShowAllRegTypes();
                BuilderMgr.AGMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Visible, Regtypes.xAuthType2.ToString());
                BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(30);
                BuilderMgr.AGMgr.ClickSaveItem();

                BuilderMgr.ClickAddAgendaItem();
                BuilderMgr.AGMgr.SetName(AGName4);
                BuilderMgr.AGMgr.ShowAllRegTypes();
                BuilderMgr.AGMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Visible, Regtypes.NonxAuthType1.ToString());
                BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(40);
                BuilderMgr.AGMgr.ClickSaveItem();

                BuilderMgr.ClickAddAgendaItem();
                BuilderMgr.AGMgr.SetName(AGName5);
                BuilderMgr.AGMgr.ShowAllRegTypes();
                BuilderMgr.AGMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Required, Regtypes.xAuthType1.ToString());
                BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(50);
                BuilderMgr.AGMgr.ClickSaveItem();

                BuilderMgr.ClickAddAgendaItem();
                BuilderMgr.AGMgr.SetName(AGName6);
                BuilderMgr.AGMgr.ShowAllRegTypes();
                BuilderMgr.AGMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Required, Regtypes.NonxAuthType2.ToString());
                BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(60);
                BuilderMgr.AGMgr.ClickSaveItem();

                // Add hotel and room blocks
                BuilderMgr.GotoPage(FormDetailManager.Page.LodgingTravel);
                BuilderMgr.ClickYesOnSplashPage();

                BuilderMgr.ClickAddHotel();
                BuilderMgr.HotelMgr.ClickHotelTemplateLink();
                BuilderMgr.HotelMgr.HotelTemplateMgr.TypeName("XAuth Hotel");
                BuilderMgr.HotelMgr.HotelTemplateMgr.AddNewRoomType("room1", 100);
                BuilderMgr.HotelMgr.HotelTemplateMgr.AddNewRoomType("room2", 150);
                BuilderMgr.HotelMgr.HotelTemplateMgr.SaveAndClose();
                BuilderMgr.HotelMgr.SaveAndClose();
                BuilderMgr.LodgingStandardFieldsMgr.SetRoomType(true, false);
                BuilderMgr.LodgingStandardFieldsMgr.SetCheckInOutDate(true, false);
                BuilderMgr.LodgingStandardFieldsMgr.SetValidDateRangeForCheckInOut(System.DateTime.Now, System.DateTime.Now.AddDays(5));

                // Set up payment method
                BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
                BuilderMgr.PaymentMethodMgr.ClickAddPaymentMethod();
                BuilderMgr.PaymentMethodMgr.SelectNewPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
                BuilderMgr.PaymentMethodMgr.SaveAndCloseAddPaymentMethod();
                Utilities.Utility.ThreadSleep(2);
                BuilderMgr.PaymentMethodMgr.SetPaymentMethodVisibility(Managers.ManagerBase.PaymentMethod.Check, true, true, true);
                
                
                BuilderMgr.SaveAndClose();

                if (onSite)
                {
                    ManagerSiteMgr.OpenEventDashboard(eventID);
                    ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
                    ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
                    ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();
                    ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);
                    ManagerSiteMgr.DashboardMgr.ChangeStatusIfNecessary(DashboardManager.EventStatus.OnSite);
                }

                ret.Add(eventID.ToString());

                #endregion
            }

            return ret;
        }

        // Use this function to clean up attendees from xauth for avoid using recall attendee info
        [Step]
        private void ChangeAndResetXAuthConfiguration(int eventID, string eventSessionID, string regTypeName)
        {
            ManagerSiteMgr.OpenEventBuilderStartPage(eventID, eventSessionID);
            BuilderMgr.OpenRegType(regTypeName);
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(DataCollection.EventData_Common.XAuthType.ByEmail);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(DataCollection.EventData_Common.XAuthType.ByEmail);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            BuilderMgr.RegTypeMgr.SaveAndStay();

            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(DataCollection.EventData_Common.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(DataCollection.EventData_Common.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();
        }
    }
}
