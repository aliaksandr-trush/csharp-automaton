namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class OnsiteKiosk : ExternalAuthenticationFixtureBase
    {
        [Test]
        public void SSOOnsiteKiosk()
        {
            Event evt = new Event("SSOOnsiteKiosk");
            evt.IsActive = true;
            RegType regType1 = new RegType("SSORegType");
            regType1.IsSSO = true;
            RegType regType2 = new RegType("NoneSSORegType");
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.SSO, evt);

            Registrant reg1 = new Registrant(evt, ExternalAuthenticationData.SSOTestEmail);
            reg1.Password = ExternalAuthenticationData.SSOPassword;
            reg1.RegType_Response = new RegTypeResponse(regType1);
            Registrant reg2 = new Registrant(evt);
            reg2.RegType_Response = new RegTypeResponse(regType2);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg1);
            KeywordProvider.RegistrationCreation.CreateRegistration(reg2);

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.SSO);
            KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Id);
            KeywordProvider.ChangeEventStatus.EventStatusChange(evt, FormData.EventStatus.OnSite);

            DataCollection.OnsiteKiosk kiosk = new DataCollection.OnsiteKiosk();
            kiosk.RequireAuthentication = true;
            kiosk.AllowOnsiteReg = true;

            KeywordProvider.LaunchKiosk.LaunchOnsiteKiosk(kiosk);

            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.SearchCondition.Type(reg1.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.Enter_Click();
            AssertHelper.VerifyOnPage(FormData.RegisterPage.SSOLogin, true);
            KeywordProvider.RegistrationCreation.SSOLogin(reg1);
            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.NewSearch_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.SearchCondition.Type(reg2.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.Enter_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.Password.Type(DataCollection.DefaultPersonalInfo.Password);
            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.SubmitPassword_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.NewSearch_Click();

            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.OnsiteRegister_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(ExternalAuthenticationData.SSOJustNameEmail);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(regType1);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            AssertHelper.VerifyOnPage(FormData.RegisterPage.SSOLogin, true);
            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.GoBackToPreviousPage();
            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.GoBackToPreviousPage();
            PageObject.PageObjectProvider.Register.RegistationSite.OnsiteKiosk.OnsiteRegister_Click();
            Registrant reg3 = new Registrant(evt);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg3.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(regType2);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);

            DataAccess.AccessData.RemoveLiveRegForEvent(evt.Id);
        }
    }
}
