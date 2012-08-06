namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class SSOSetup : ExternalAuthenticationFixtureBase
    {
        [Test]
        public void SetupSSO()
        {
            string attri2 = "disabled";
            string wrongFormatURL1 = "http://www.xx.com";
            string wrongFormatURL2 = "www.xx.com";

            RegType regType1 = new RegType("SSORegType");
            regType1.IsSSO = true;
            RegType regType2 = new RegType("NoneSSORegType");
            regType2.IsSSO = true;
            Event evt = new Event("SetupSSO");

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.SSO);

            if (KeywordProvider.ManagerDefault.DoesEventExist(evt.Title))
            {
                KeywordProvider.ManagerDefault.DeleteEvent(evt.Title);
            }

            KeywordProvider.EventCreator.ClickAddEventAndGetEventId(evt);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EmptyAddRegType_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SelectByName();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.NameOnForm.Type(regType1.RegTypeName);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.NameOnReports.Type(regType1.RegTypeName);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthentication_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.SelectByName();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.SSORadio.Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.EndpointURL.Type(wrongFormatURL1);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.LoginURL.Type(ExternalAuthenticationData.SSOLoginURL);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.SaveAndClose_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.SelectByName();
            Assert.True(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.ErrorMessage(Messages.BuilderError.ServiceEndPointFormatError).IsPresent);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.EndpointURL.Type(wrongFormatURL2);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.SaveAndClose_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.SelectByName();
            Assert.True(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.ErrorMessage(Messages.BuilderError.ServiceEndPointFormatError).IsPresent);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.EndpointURL.Type(ExternalAuthenticationData.SSOEndpointURL);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.SaveAndClose_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EnableExternalAuthentication.Set(true);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SaveAndClose_Click();
            PageObject.Builder.RegistrationFormPages.RegTypeRow row1 = new PageObject.Builder.RegistrationFormPages.RegTypeRow(regType1.RegTypeName);
            regType1.RegTypeId = row1.RegTypeId;
            KeywordProvider.AddRegType.AddRegTypes(regType2);
            PageObject.Builder.RegistrationFormPages.RegTypeRow row2 = new PageObject.Builder.RegistrationFormPages.RegTypeRow(regType2.RegTypeName);
            regType2.RegTypeId = row2.RegTypeId;
            PageObject.PageObjectProvider.Builder.EventDetails.SaveAndClose_Click();
            KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Title);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdParty_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.ThirdPartyIntegrations.ExternalAuthentication_Click();
            Assert.True(PageObject.PageObjectProvider.Manager.SSOBase.SSORadio.IsChecked);
            Assert.AreEqual(PageObject.PageObjectProvider.Manager.SSOBase.EndpointURL.Value, ExternalAuthenticationData.SSOEndpointURL);
            Assert.AreEqual(PageObject.PageObjectProvider.Manager.SSOBase.LoginURL.Value, ExternalAuthenticationData.SSOLoginURL);
            Assert.True(PageObject.PageObjectProvider.Manager.SSOBase.RegTypeEnabled(regType1).IsChecked);
            Assert.True(PageObject.PageObjectProvider.Manager.SSOBase.RegTypeEnabled(regType2).IsChecked);
            PageObject.PageObjectProvider.Manager.SSOBase.EndpointURL.Type(wrongFormatURL1);
            PageObject.PageObjectProvider.Manager.SSOBase.SaveAndClose_Click();
            PageObject.PageObjectProvider.Manager.SSOBase.SelectByName();
            Assert.True(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.ErrorMessage(Messages.BuilderError.ServiceEndPointFormatError).IsPresent);
            PageObject.PageObjectProvider.Manager.SSOBase.EndpointURL.Type(wrongFormatURL2);
            PageObject.PageObjectProvider.Manager.SSOBase.SaveAndClose_Click();
            PageObject.PageObjectProvider.Manager.SSOBase.SelectByName();
            Assert.True(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.ExternalAuthenticationSetup.ErrorMessage(Messages.BuilderError.ServiceEndPointFormatError).IsPresent);
            PageObject.PageObjectProvider.Manager.SSOBase.EndpointURL.Type(ExternalAuthenticationData.SSOEndpointURL);
            PageObject.PageObjectProvider.Manager.SSOBase.RegTypeEnabled(regType2).Set(false);
            regType2.IsSSO = false;
            PageObject.PageObjectProvider.Manager.SSOBase.SaveAndClose_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.EditForm_Click();
            row2.Title_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SelectByName();
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EnableExternalAuthentication.IsChecked);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.Cancel_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.SaveAndClose_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.ReturnToList_Click();

            Registrant reg1 = new Registrant(evt, ExternalAuthenticationData.SSOTestEmail);
            reg1.Password = ExternalAuthenticationData.SSOPassword;
            reg1.RegType = regType1;
            Registrant reg2 = new Registrant(evt);
            reg2.RegType = regType2;

            KeywordProvider.RegistrationCreation.CreateRegistration(reg1);
            KeywordProvider.RegistrationCreation.CreateRegistration(reg2);

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.SSO);
            KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Title);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.EditForm_Click();
            row1.Title_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SelectByName();
            Assert.True(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EnableExternalAuthentication.HasAttribute(attri2));
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.Cancel_Click();
            row2.Title_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.SelectByName();
            Assert.True(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EnableExternalAuthentication.HasAttribute(attri2));
        }
    }
}
