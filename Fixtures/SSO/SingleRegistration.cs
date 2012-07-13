namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class SingleRegistration : ExternalAuthenticationFixtureBase
    {
        [Test]
        public void SingleRegisterSSO()
        {
            Event evt = new Event("SingleRegisterSSO");
            RegType regType = new RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.SSO, evt, false, true);

            Registrant reg = new Registrant(ExternalAuthenticationData.SSOTestEmail);
            reg.Password = ExternalAuthenticationData.SSOPassword;
            reg.Event = evt;
            reg.RegType = regType;

            KeywordProvider.RegistrationCreation.Checkin(reg);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.SSOLogin, true);
            KeywordProvider.RegistrationCreation.SSOLogin(ExternalAuthenticationData.SSOJustNameEmail, ExternalAuthenticationData.SSOPassword);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.YouMustEnterValidEmailAddress));
            KeywordProvider.RegistrationCreation.Checkin(reg);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.SSOLogin, true);
            KeywordProvider.RegistrationCreation.SSOLogin(reg);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Password.IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(FormData.PersonalInfoField.Email).HasAttribute("value"));
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(FormData.PersonalInfoField.FirstName).HasAttribute("value"));
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(FormData.PersonalInfoField.MiddleName).HasAttribute("value"));
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(FormData.PersonalInfoField.LastName).HasAttribute("value"));
            PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.JobTitle.Type("Automation");
            reg.JobTitle = "Automation";
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.Checkout(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            AssertHelper.VerifyOnPage(FormData.RegisterPage.SSOLogin, true);
            KeywordProvider.RegistrationCreation.SSOLogin(reg);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.AttendeeCheck, true);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.Substitute(0).IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.PersonalInfoLink_Click(0);
            Assert.True(reg.JobTitle == PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.JobTitle.Value);

            KeywordProvider.RegistrationCreation.Checkin(reg);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.EmailAlreadyUsed));
        }

        [Test]
        public void SingleRegisterNoneSSO()
        {
            Event evt = new Event("SingleRegisterNoneSSO");
            RegType regType1 = new RegType("SSORegType");
            regType1.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType1);
            RegType regType2 = new RegType("NoneSSORegType");
            evt.StartPage.RegTypes.Add(regType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.SSO, evt, false, true);

            Registrant reg1 = new Registrant();
            reg1.Event = evt;
            reg1.RegType = regType2;

            Registrant reg2 = new Registrant(ExternalAuthenticationData.SSOTestEmail);
            reg2.Password = ExternalAuthenticationData.SSOPassword;
            reg2.Event = evt;
            reg2.RegType = regType2;

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            KeywordProvider.RegistrationCreation.Checkout(reg1);

            KeywordProvider.RegistrationCreation.Checkin(reg2);
            if (PageObject.PageObjectProvider.Register.RegistationSite.IsOnPage(FormData.RegisterPage.Login))
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Login.StartNewRegistration_Click();
            }
            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.EmailAlreadyUsed));
        }
    }
}
