namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;
   
    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class SSOEventActivate : SSOFixtureBase
    {
        [Test]
        public void ActivateSSOEvent()
        {
            RegOnline.RegressionTest.DataAccess.AccessData.ApprovedXAuthRoleForCurrentCustomer(true);

            Event evt = new Event("ActivateSSOEvent");
            RegType ssoRegType = new RegType("SSORegType");
            ssoRegType.IsSSO = true;
            RegType nonSSORegType = new RegType("NonSSORegType");
            evt.StartPage.RegTypes.Add(ssoRegType);
            evt.StartPage.RegTypes.Add(nonSSORegType);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.SSO, evt);

            Registrant reg = new Registrant(evt, SSOData.SSOTestEmail);
            reg.Password = SSOData.SSOPassword;
            reg.EventFee_Response = new EventFeeResponse(ssoRegType);

            KeywordProvider.Registration_Creation.CreateRegistration(reg);

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.SSO);
            KeywordProvider.Manager_Common.OpenFormDashboard(evt.Title);
            PageObject.PageObjectProvider.Manager.Dashboard.Activate_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.SelectByName();
            Assert.True(PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.RemoveTestReg.IsChecked);
            Assert.True(PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.RemoveTestReg.HasAttribute("disabled"));
            PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.Cancel_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdParty_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.ExternalAuthentication_Click();
            PageObject.PageObjectProvider.Manager.SSOBase.EndpointURL.Type(SSOData.SSOEndpointURL + "add");
            PageObject.PageObjectProvider.Manager.SSOBase.SaveAndClose_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.Refresh();
            Assert.AreEqual(0, Convert.ToInt32(PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.TotalRegs.Text));
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdParty_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.ExternalAuthentication_Click();
            PageObject.PageObjectProvider.Manager.SSOBase.EndpointURL.Type(SSOData.SSOEndpointURL);
            PageObject.PageObjectProvider.Manager.SSOBase.SaveAndClose_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.Activate_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.Activate_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.DashboardTab_Click(FormData.DashboardTab.EventDetails);
            PageObject.PageObjectProvider.Manager.Dashboard.ReturnToList_Click();

            KeywordProvider.Registration_Creation.CreateRegistration(reg);

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.SSO);
            KeywordProvider.Manager_Common.OpenFormDashboard(evt.Title);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdParty_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.ExternalAuthentication_Click();
            Assert.True(PageObject.PageObjectProvider.Manager.SSOBase.EndpointURL.HasAttribute("disabled"));
        }
    }
}
