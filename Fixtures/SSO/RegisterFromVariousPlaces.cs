namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using System;

    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class RegisterFromVariousPlacesFixture : SSOFixtureBase
    {
        [Test]
        public void RegTypeDirectUrl()
        {
            DataCollection.Event evt = new DataCollection.Event("SSO_RegisterFromVariousPlaces_RegTypeDirectUrl");
            DataCollection.RegType regType = new DataCollection.RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);

            DataCollection.Registrant reg = new DataCollection.Registrant(evt, DataCollection.SSOData.SSOTestEmail);
            reg.Password = DataCollection.SSOData.SSOPassword;
            reg.EventFee_Response = new DataCollection.EventFeeResponse(regType);
            reg.Register_Method = DataCollection.RegisterMethod.RegTypeDirectUrl;

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            Keyword.KeywordProvider.Registration_Creation.SSOLogin(reg);            
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.Registration_Creation.CheckoutAndConfirmation(reg);
        }

        [Test]
        public void EventId()
        {
            DataCollection.Event evt = new DataCollection.Event("SSO_RegisterFromVariousPlaces_EventId");
            DataCollection.RegType regType = new DataCollection.RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);

            DataCollection.Registrant reg = new DataCollection.Registrant(evt, DataCollection.SSOData.SSOTestEmail);
            reg.Password = DataCollection.SSOData.SSOPassword;
            reg.EventFee_Response = new DataCollection.EventFeeResponse(regType);

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            Keyword.KeywordProvider.Registration_Creation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.Registration_Creation.CheckoutAndConfirmation(reg);
        }

        [Test]
        public void EventShortcut()
        {
            DataCollection.Event evt = new DataCollection.Event("SSO_RegisterFromVariousPlaces_EventShortcut");
            DataCollection.RegType regType = new DataCollection.RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);

            DataCollection.Registrant reg = new DataCollection.Registrant(evt, DataCollection.SSOData.SSOTestEmail);
            reg.Password = DataCollection.SSOData.SSOPassword;
            reg.EventFee_Response = new DataCollection.EventFeeResponse(regType);
            reg.Register_Method = DataCollection.RegisterMethod.Shortcut;

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            Keyword.KeywordProvider.Registration_Creation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg.EventFee_Response.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.Registration_Creation.CheckoutAndConfirmation(reg);
        }

        [Test]
        public void EventWebSite()
        {
            DataCollection.Event evt = new DataCollection.Event("SSO_RegisterFromVariousPlaces_EventWebsite");
            DataCollection.RegType regType = new DataCollection.RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);
            evt.EventWebsite = new DataCollection.EventWebsite();
            evt.EventWebsite.UseEventWebsiteAsTheStartingPageForEvent = true;

            DataCollection.Registrant reg = new DataCollection.Registrant(evt, DataCollection.SSOData.SSOTestEmail);
            reg.Password = DataCollection.SSOData.SSOPassword;
            reg.EventFee_Response = new DataCollection.EventFeeResponse(regType);
            reg.Register_Method = DataCollection.RegisterMethod.Shortcut;

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.EventWebsite.RegisterNow_Click();
            Keyword.KeywordProvider.Registration_Creation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg.EventFee_Response.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.Registration_Creation.CheckoutAndConfirmation(reg);
        }

        [Test]
        public void ParentChildEvent()
        {
            DataCollection.Event evt_Parent = new DataCollection.Event("SSO_RegisterFromVariousPlaces_ParentChildEvent_Parent");
            evt_Parent.StartPage.AdvancedSettings = new DataCollection.EventAdvancedSettings();
            evt_Parent.StartPage.AdvancedSettings.ThisIsAParentEvent = true;

            DataCollection.Event evt_Child = new DataCollection.Event("SSO_RegisterFromVariousPlaces_ParentChildEvent_Child");
            evt_Child.StartPage.StartDate = DataCollection.CrossSiteData.DefaultStartDate;
            evt_Child.StartPage.EndDate = DataCollection.CrossSiteData.DefaultEndDate;
            evt_Child.StartPage.AdvancedSettings = new DataCollection.EventAdvancedSettings();
            evt_Child.StartPage.AdvancedSettings.ThisIsAChildEvent = true;
            evt_Child.StartPage.AdvancedSettings.ParentEvent = evt_Parent;

            DataCollection.RegType regType = new DataCollection.RegType("SSORegType");
            regType.IsSSO = true;
            evt_Child.StartPage.RegTypes.Add(regType);

            DataCollection.Registrant reg = new DataCollection.Registrant(evt_Child, DataCollection.SSOData.SSOTestEmail);
            reg.Password = DataCollection.SSOData.SSOPassword;
            reg.EventFee_Response = new DataCollection.EventFeeResponse(regType);

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt_Parent);
            Keyword.KeywordProvider.SignIn.RecreateEventAndGetEventId(evt_Child);

            DataCollection.Registrant reg2 = new DataCollection.Registrant(evt_Parent);
            reg2.Register_Method = DataCollection.RegisterMethod.Shortcut;

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg2);
            PageObject.PageObjectProvider.Register.RegistationSite.EventCalendar.SelectView(DataCollection.EventData_Common.EventCalendarView.Day);
            PageObject.PageObjectProvider.Register.RegistationSite.EventCalendar.ClickToRegister(evt_Child);
            PageObject.PageObjectHelper.SelectTopWindow();
            Keyword.KeywordProvider.Registration_Creation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg.EventFee_Response.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.Registration_Creation.CheckoutAndConfirmation(reg);
        }
    }
}
