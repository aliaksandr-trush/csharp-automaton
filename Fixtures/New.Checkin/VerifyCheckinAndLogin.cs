namespace RegOnline.RegressionTest.Fixtures.New.Checkin
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

            Registrant reg = new Registrant(evt, "abc");

            KeywordProvider.Registration_Creation.Checkin(reg);

            Assert.True(KeywordProvider.Register_Common.HasErrorMessage(Messages.RegisterError.YouMustEnterValidEmailAddress));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1292")]
        public void InvalidPassword()
        {
            Event evt = new Event("RI-InvalidPassword");
            string emailAddress = string.Format("selenium{0}@regonline.com", System.DateTime.Now.Ticks.ToString());

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant reg = new Registrant(evt, emailAddress);

            KeywordProvider.Registration_Creation.CreateRegistration(reg);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();

            PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.Type("abcdefg");
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();

            Assert.True(KeywordProvider.Register_Common.HasErrorMessage(Messages.RegisterError.InvalidPassword));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1310")]
        public void GroupInvalidEmail()
        {
            Event evt = new Event("RI-GroupInvalidEmail");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant reg = new Registrant(evt);
            Registrant reg1 = new Registrant(evt, "abc");

            KeywordProvider.Registration_Creation.Checkin(reg);
            KeywordProvider.Registration_Creation.PersonalInfo(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            KeywordProvider.Registration_Creation.Checkin(reg1);

            Assert.True(KeywordProvider.Register_Common.HasErrorMessage(Messages.RegisterError.YouMustEnterValidEmailAddress));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1311")]
        public void GroupInvalidPassword()
        {
            Event evt = new Event("RI-GroupInvalidPassword");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant reg1 = new Registrant(evt);
            Registrant reg2 = new Registrant(evt);
            Group group = new Group();
            group.Primary = reg1;
            group.Secondaries.Add(reg2);

            KeywordProvider.Registration_Creation.GroupRegistration(group);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();

            PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.Type("abcdefg");
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();

            Assert.True(KeywordProvider.Register_Common.HasErrorMessage(Messages.RegisterError.InvalidPassword));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1315")]
        public void ForgetPasswordLink()
        {
            Event evt = new Event("RI-ForgetPassword");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant reg = new Registrant(evt);

            KeywordProvider.Registration_Creation.CreateRegistration(reg);

            KeywordProvider.Registration_Creation.Checkin(reg);
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

            Registrant reg = new Registrant(evt);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.VerifyEmailAddress.IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type("a@b.com");
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.VerifyEmailAddress.Type("a@c.com");
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Assert.True(KeywordProvider.Register_Common.HasErrorMessage(Messages.RegisterError.EmailAddressesDoNotMatch));
        }
    }
}
