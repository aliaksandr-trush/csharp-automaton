namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.CheckinPage
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class VerifyCheckinAndLogin : FixtureBase
    {
        [Test]
        [Category(Priority.Three)]
        [Description("1291")]
        public void InvalidEmailAddress()
        {
            Event evt = new Event("RI-InvalidEmailAddress");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant reg = new Registrant("abc");
            reg.Event = evt;

            KeywordProvider.RegistrationCreation.Checkin(reg);

            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.YouMustEnterValidEmailAddress));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1292")]
        public void InvalidPassword()
        {
            Event evt = new Event("RI-InvalidPassword");
            string emailAddress = string.Format("selenium{0}@regonline.com", System.DateTime.Now.Ticks.ToString());

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant reg = new Registrant(emailAddress);
            reg.Event = evt;

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();

            PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.Type("abcdefg");
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();

            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.InvalidPassword));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1310")]
        public void GroupInvalidEmail()
        {
            Event evt = new Event("RI-GroupInvalidEmail");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant reg = new Registrant();
            reg.Event = evt;
            Registrant reg1 = new Registrant("abc");
            reg1.Event = evt;

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            KeywordProvider.RegistrationCreation.Checkin(reg1);

            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.YouMustEnterValidEmailAddress));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1311")]
        public void GroupInvalidPassword()
        {
            Event evt = new Event("RI-GroupInvalidPassword");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant reg1 = new Registrant();
            Registrant reg2 = new Registrant();
            reg1.Event = evt;
            reg2.Event = evt;
            List<Registrant> regs = new List<Registrant>();
            regs.Add(reg1);
            regs.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(regs);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();

            PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.Type("abcdefg");
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();

            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.InvalidPassword));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1315")]
        public void ForgetPasswordLink()
        {
            Event evt = new Event("RI-ForgetPassword");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant reg = new Registrant();
            reg.Event = evt;

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);

            KeywordProvider.RegistrationCreation.Checkin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Login.ForgotPassword_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Login.ForgotPasswordPopup.SelectByIndex();
            string email = PageObject.PageObjectProvider.Register.RegistationSite.Login.ForgotPasswordPopup.EmailAddress.GetAttribute("value");
            Assert.True(reg.Email == email);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1316")]
        public void AddVerifyEmailField()
        {
            Event evt = new Event("RI-VerifyEmail");
            PersonalInfoPageStandardField VerifyEmail = new PersonalInfoPageStandardField();
            VerifyEmail.StandardField = FormData.PersonalInfoField.VerifyEmail;
            VerifyEmail.Visible = true;
            evt.PersonalInfoPage.StandardFields.Add(VerifyEmail);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            KeywordProvider.RegisterDefault.OpenRegisterPageUrl(evt.Id);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.VerifyEmailAddress.IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type("a@b.com");
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.VerifyEmailAddress.Type("a@c.com");
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.EmailAddressesDoNotMatch));
        }
    }
}
