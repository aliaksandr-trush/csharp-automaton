namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Keyword;
    using RegOnline.RegressionTest.Fixtures.Base;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;

    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class SSOAttendeeDirectory : SSOFixtureBase
    {
       
        [Test]
        public void SSODirectory()
        {
            Event evt = new Event("AttendeeDirectory");
            RegType regType1 = new RegType("SSORegType");
            regType1.IsSSO = true;
            RegType regType2 = new RegType("NoneSSORegType");
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.SSO, evt);

            Registrant reg1 = new Registrant(evt, SSOData.SSOTestEmail);
            reg1.Password = SSOData.SSOPassword;
            reg1.EventFee_Response = new EventFeeResponse(regType1);
            Registrant reg2 = new Registrant(evt);
            reg2.EventFee_Response = new EventFeeResponse(regType2);

            KeywordProvider.Registration_Creation.CreateRegistration(reg1);
            KeywordProvider.Registration_Creation.CreateRegistration(reg2);

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.SSO);
            KeywordProvider.Manager_Common.OpenFormDashboard(evt.Title);
            PageObject.PageObjectProvider.Manager.Dashboard.DashboardTab_Click(FormData.DashboardTab.Directories);

            AttendeeDirectory attendeeDirectory = new AttendeeDirectory("SSODirectory");
            attendeeDirectory.ShareDirectory = true;
            attendeeDirectory.RequireLogin = true;

            string directoryURL = KeywordProvider.Add_AttendeeDirectory.CreateAttendeeDirectory(attendeeDirectory);
            PageObject.PageObjectProvider.Manager.Dashboard.DashboardTab_Click(FormData.DashboardTab.EventDetails);
            PageObject.PageObjectProvider.Manager.Dashboard.ReturnToList_Click();

            PageObject.PageObjectHelper.NavigateTo(directoryURL);
            PageObject.PageObjectProvider.Reports.AttendeeDirectory.EmailAddress.Type(reg1.Email);
            PageObject.PageObjectProvider.Reports.AttendeeDirectory.Continue_Click();
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.IsOnPage(FormData.RegisterPage.SSOLogin));
            KeywordProvider.Registration_Creation.SSOLogin(reg1);
            PageObject.PageObjectProvider.Reports.AttendeeDirectory.SignOut_Click();

            PageObject.PageObjectProvider.Reports.AttendeeDirectory.EmailAddress.Type(reg2.Email);
            PageObject.PageObjectProvider.Reports.AttendeeDirectory.Continue_Click();
            PageObject.PageObjectProvider.Reports.AttendeeDirectory.Password.Type(DataCollection.DefaultPersonalInfo.Password);
            PageObject.PageObjectProvider.Reports.AttendeeDirectory.Continue_Click();
            PageObject.PageObjectProvider.Reports.AttendeeDirectory.SignOut_Click();
        }
    }
}
