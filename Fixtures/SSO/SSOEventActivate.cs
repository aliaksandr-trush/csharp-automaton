namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;
   
    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class SSOEventActivate : ExternalAuthenticationFixtureBase
    {
        [Test]
        public void ActivateSSOEvent()
        {
            RegOnline.RegressionTest.DataAccess.AccessData.ApprovedXAuthRoleForCustomer(true);

            Event evt = new Event("ActivateSSOEvent");
            RegType regType = new RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.SSO, evt);

            Registrant reg = new Registrant(evt, ExternalAuthenticationData.SSOTestEmail);
            reg.Password = ExternalAuthenticationData.SSOPassword;
            reg.EventFee_Response = new EventFeeResponse(regType);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.SSO);
            KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Title);
            PageObject.PageObjectProvider.Manager.Dashboard.Activate_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.SelectByName();
            Assert.True(PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.RemoveTestReg.IsChecked);
            Assert.True(PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.RemoveTestReg.HasAttribute("disabled"));
            PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.Cancel_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdParty_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.ExternalAuthentication_Click();
            PageObject.PageObjectProvider.Manager.SSOBase.EndpointURL.Type(ExternalAuthenticationData.SSOEndpointURL + "add");
            PageObject.PageObjectProvider.Manager.SSOBase.SaveAndClose_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.Refresh();
            Assert.AreEqual(Convert.ToInt32(PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.TotalRegs.Text), 0);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdParty_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.ExternalAuthentication_Click();
            PageObject.PageObjectProvider.Manager.SSOBase.EndpointURL.Type(ExternalAuthenticationData.SSOEndpointURL);
            PageObject.PageObjectProvider.Manager.SSOBase.SaveAndClose_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.Activate_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.Activate_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.DashboardTab_Click(FormData.DashboardTab.EventDetails);
            PageObject.PageObjectProvider.Manager.Dashboard.ReturnToList_Click();

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.SSO);
            KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Title);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdParty_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.ExternalAuthentication_Click();
            Assert.True(PageObject.PageObjectProvider.Manager.SSOBase.EndpointURL.HasAttribute("disabled"));
        }
    }
}
