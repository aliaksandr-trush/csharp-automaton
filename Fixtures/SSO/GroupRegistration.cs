﻿namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class GroupRegistration : ExternalAuthenticationFixtureBase
    {
        [Test]
        public void GroupRegisterSSO()
        {
            Event evt = new Event("GroupRegisterSSO");
            RegType regType1 = new RegType("SSORegistrationType1");
            regType1.IsSSO = true;
            RegType regType2 = new RegType("SSORegistrationType2");
            regType2.IsSSO = true;
            RegType regType3 = new RegType("NoneSSORegType1");
            RegType regType4 = new RegType("NoneSSORegType2");
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);
            evt.StartPage.RegTypes.Add(regType3);
            evt.StartPage.RegTypes.Add(regType4);
            evt.StartPage.AllowChangeRegType = true;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.SSO, evt, true, false);

            Registrant reg1 = new Registrant(ExternalAuthenticationData.SSOTestEmail);
            reg1.Event = evt;
            reg1.Password = ExternalAuthenticationData.SSOPassword;
            reg1.RegType = regType1;

            Registrant reg2 = new Registrant();
            reg2.Event = evt;
            reg2.RegType = regType3;

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.SSOLogin(reg1);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType1).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType2).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType3).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType4).IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg2.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg2.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Password.Value == "");
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            KeywordProvider.RegistrationCreation.Checkout(reg2);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            AssertHelper.VerifyOnPage(FormData.RegisterPage.SSOLogin, true);
            KeywordProvider.RegistrationCreation.SSOLogin(reg1);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.AttendeeCheck, true);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.Substitute(0).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.Substitute(1).IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.PersonalInfoLink_Click(0);
            PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.ChangeRegType_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.RegTypeList.SelectByIndex();
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.RegTypeList.RegTypeRadio(regType1).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.RegTypeList.RegTypeRadio(regType2).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.RegTypeList.RegTypeRadio(regType3).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.RegTypeList.RegTypeRadio(regType4).IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.RegTypeList.Cancel_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();

            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.PersonalInfoLink_Click(1);
            PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.ChangeRegType_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.RegTypeList.SelectByIndex();
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.RegTypeList.RegTypeRadio(regType1).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.RegTypeList.RegTypeRadio(regType2).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.RegTypeList.RegTypeRadio(regType3).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.RegTypeList.RegTypeRadio(regType4).IsPresent);
        }

        [Test]
        public void GroupRegisterNoneSSO()
        {
            Event evt = new Event("GroupRegisterNoneSSO");
            RegType regType1 = new RegType("SSORegType1");
            regType1.IsSSO = true;
            RegType regType2 = new RegType("SSORegType2");
            regType2.IsSSO = true;
            RegType regType3 = new RegType("NoneSSORegType1");
            RegType regType4 = new RegType("NoneSSORegType2");
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);
            evt.StartPage.RegTypes.Add(regType3);
            evt.StartPage.RegTypes.Add(regType4);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.SSO, evt, true, false);

            Registrant reg1 = new Registrant();
            reg1.Event = evt;
            reg1.RegType = regType3;

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);

            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType1).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType2).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType3).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType4).IsPresent);

            Registrant reg2 = new Registrant();
            reg2.Event = evt;
            reg2.RegType = regType4;

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg2.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg2.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            KeywordProvider.RegistrationCreation.Checkout(reg2);
            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            AssertHelper.VerifyOnPage(FormData.RegisterPage.Login, true);
            KeywordProvider.RegistrationCreation.Login(reg1);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.AttendeeCheck, true);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.Substitute(0).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.Substitute(1).IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.Checkout(reg2);
        }

        [Test]
        public void GroupRegisterWithNoNoneSSO()
        {
            RegType regType1 = new RegType("SSORegType");
            regType1.IsSSO = true;
            Event evt1 = new Event("EventWithOnlySSO");
            evt1.StartPage.RegTypes.Add(regType1);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.SSO, evt1, true, false);

            Registrant reg1 = new Registrant(ExternalAuthenticationData.SSOTestEmail);
            reg1.Password = ExternalAuthenticationData.SSOPassword;
            reg1.Event = evt1;
            reg1.RegType = regType1;

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.SSOLogin(reg1);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson.IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson.IsPresent);
            KeywordProvider.RegistrationCreation.Checkout(reg1);

            RegType regType3 = new RegType("SSORegType");
            regType3.IsSSO = true;
            Event evt2 = new Event("EventWithOnlySSOAndHideNoneSSO");
            evt2.StartPage.RegTypes.Add(regType3);
            RegType regType2 = new RegType("NoneSSORegType");
            regType2.IsPublic = false;
            evt2.StartPage.RegTypes.Add(regType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.SSO, evt2, true, false);

            Registrant reg2 = new Registrant(ExternalAuthenticationData.SSOTestEmail);
            reg2.Password = ExternalAuthenticationData.SSOPassword;
            reg2.Event = evt2;
            reg2.RegType = regType3;

            KeywordProvider.RegistrationCreation.Checkin(reg2);
            KeywordProvider.RegistrationCreation.SSOLogin(reg2);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson.IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson.IsPresent);
            KeywordProvider.RegistrationCreation.Checkout(reg2);
        }
    }
}
