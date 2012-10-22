namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class SingleRegistration : SSOFixtureBase
    {
        [Test]
        public void SingleRegisterSSO()
        {
            Event evt = new Event("SingleRegisterSSO");
            RegType regType = new RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.SSO, evt, false, true);

            Registrant reg = new Registrant(evt, SSOData.SSOTestEmail);
            reg.Password = SSOData.SSOPassword;
            reg.EventFee_Response = new EventFeeResponse(regType);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.SSOLogin, true);
            KeywordProvider.Registration_Creation.SSOLogin(SSOData.SSOJustNameEmail, SSOData.SSOPassword);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.SSOLogin, true);
            KeywordProvider.Registration_Creation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Password.IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(FormData.PersonalInfoField.Email).HasAttribute("value"));
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(FormData.PersonalInfoField.FirstName).HasAttribute("value"));
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(FormData.PersonalInfoField.MiddleName).HasAttribute("value"));
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(FormData.PersonalInfoField.LastName).HasAttribute("value"));
            PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.JobTitle.Type("Automation");
            reg.JobTitle = "Automation";
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.Registration_Creation.CheckoutAndConfirmation(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            AssertHelper.VerifyOnPage(FormData.RegisterPage.SSOLogin, true);
            KeywordProvider.Registration_Creation.SSOLogin(reg);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.AttendeeCheck, true);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.Substitute(0).IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.PersonalInfoLink_Click(0);
            Assert.True(reg.JobTitle == PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.JobTitle.Value);
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

            Registrant reg1 = new Registrant(evt);
            reg1.EventFee_Response = new EventFeeResponse(regType2);

            Registrant reg2 = new Registrant(evt, SSOData.SSOTestEmail);
            reg2.Password = SSOData.SSOPassword;
            reg2.EventFee_Response = new EventFeeResponse(regType2);

            KeywordProvider.Registration_Creation.Checkin(reg1);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
            KeywordProvider.Registration_Creation.PersonalInfo(reg1);
            KeywordProvider.Registration_Creation.CheckoutAndConfirmation(reg1);

            KeywordProvider.Registration_Creation.Checkin(reg2);
            if (PageObject.PageObjectProvider.Register.RegistationSite.IsOnPage(FormData.RegisterPage.Login))
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Login.StartNewRegistration_Click();
            }
            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);

            KeywordProvider.Registration_Creation.Checkin(reg1);
            Assert.True(KeywordProvider.Register_Common.HasErrorMessage(Messages.RegisterError.EmailAlreadyUsed));
        }
    }
}
