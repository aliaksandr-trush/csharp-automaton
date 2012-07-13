namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.Fixtures.Base;
    using NUnit.Framework;

    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class AdminRegistrationFixture : ExternalAuthenticationFixtureBase
    {
        [Test]
        public void AdminRegister()
        {
            DataCollection.Event evt = new DataCollection.Event("SSO_AdminRegister");
            DataCollection.RegType regType = new DataCollection.RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);

            DataCollection.Registrant reg = new DataCollection.Registrant(DataCollection.ExternalAuthenticationData.SSOTestEmail);
            reg.Password = DataCollection.ExternalAuthenticationData.SSOPassword;
            reg.Event = evt;
            reg.RegType = regType;

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(DataCollection.RegisterMethod.Admin, reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.Checkout(reg);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            AssertHelper.VerifyOnPage(DataCollection.FormData.RegisterPage.AttendeeCheck, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.Checkout(reg);
        }
    }
}
