﻿namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using System;

    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class RegisterFromVariousPlacesFixture : ExternalAuthenticationFixtureBase
    {
        [Test]
        public void RegTypeDirectUrl()
        {
            DataCollection.Event evt = new DataCollection.Event("SSO_RegisterFromVariousPlaces_RegTypeDirectUrl");
            DataCollection.RegType regType = new DataCollection.RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);

            DataCollection.Registrant reg = new DataCollection.Registrant(DataCollection.ExternalAuthenticationData.SSOTestEmail);
            reg.Password = DataCollection.ExternalAuthenticationData.SSOPassword;
            reg.Event = evt;
            reg.RegType = regType;

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt);
            Keyword.KeywordProvider.RegisterDefault.OpenRegTypeDirectUrl(evt.Id, reg.RegType.RegTypeId);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadioButton.IsDisplay);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.Checkout(reg);
        }

        [Test]
        public void EventId()
        {
            DataCollection.Event evt = new DataCollection.Event("SSO_RegisterFromVariousPlaces_EventId");
            DataCollection.RegType regType = new DataCollection.RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);

            DataCollection.Registrant reg = new DataCollection.Registrant(DataCollection.ExternalAuthenticationData.SSOTestEmail);
            reg.Password = DataCollection.ExternalAuthenticationData.SSOPassword;
            reg.Event = evt;
            reg.RegType = regType;

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt);
            Keyword.KeywordProvider.RegistrationCreation.Checkin(reg);
            Keyword.KeywordProvider.RegistrationCreation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.Checkout(reg);
        }

        [Test]
        public void EventShortcut()
        {
            DataCollection.Event evt = new DataCollection.Event("SSO_RegisterFromVariousPlaces_EventShortcut");
            DataCollection.RegType regType = new DataCollection.RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);

            DataCollection.Registrant reg = new DataCollection.Registrant(DataCollection.ExternalAuthenticationData.SSOTestEmail);
            reg.Password = DataCollection.ExternalAuthenticationData.SSOPassword;
            reg.Event = evt;
            reg.RegType = regType;

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt);
            Keyword.KeywordProvider.RegisterDefault.OpenRegisterPageUrl(evt.Shortcut);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.Checkout(reg);
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

            DataCollection.Registrant reg = new DataCollection.Registrant(DataCollection.ExternalAuthenticationData.SSOTestEmail);
            reg.Password = DataCollection.ExternalAuthenticationData.SSOPassword;
            reg.Event = evt;
            reg.RegType = regType;

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt);
            Keyword.KeywordProvider.RegisterDefault.OpenRegisterPageUrl(evt.Shortcut);
            PageObject.PageObjectProvider.Register.RegistationSite.EventWebsite.RegisterNow_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.Checkout(reg);
        }

        [Test]
        public void ParentChildEvent()
        {
            DataCollection.Event evt_Parent = new DataCollection.Event("SSO_RegisterFromVariousPlaces_ParentChildEvent_Parent");
            evt_Parent.StartPage.AdvancedSettings = new DataCollection.EventAdvancedSettings();
            evt_Parent.StartPage.AdvancedSettings.ThisIsAParentEvent = true;

            DataCollection.Event evt_Child = new DataCollection.Event("SSO_RegisterFromVariousPlaces_ParentChildEvent_Child");
            evt_Child.StartPage.StartDate = DataCollection.FormData.DefaultStartDate;
            evt_Child.StartPage.EndDate = DataCollection.FormData.DefaultEndDate;
            evt_Child.StartPage.AdvancedSettings = new DataCollection.EventAdvancedSettings();
            evt_Child.StartPage.AdvancedSettings.ThisIsAChildEvent = true;
            evt_Child.StartPage.AdvancedSettings.ParentEvent = evt_Parent;

            DataCollection.RegType regType = new DataCollection.RegType("SSORegType");
            regType.IsSSO = true;
            evt_Child.StartPage.RegTypes.Add(regType);

            DataCollection.Registrant reg = new DataCollection.Registrant(DataCollection.ExternalAuthenticationData.SSOTestEmail);
            reg.Password = DataCollection.ExternalAuthenticationData.SSOPassword;
            reg.Event = evt_Child;
            reg.RegType = regType;

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt_Parent);
            Keyword.KeywordProvider.EventCreator.CreateEvent(evt_Child);

            Keyword.KeywordProvider.RegisterDefault.OpenRegisterPageUrl(evt_Parent.Shortcut);
            PageObject.PageObjectProvider.Register.RegistationSite.EventCalendar.SelectView(DataCollection.FormData.EventCalendarView.Day);
            PageObject.PageObjectProvider.Register.RegistationSite.EventCalendar.ClickToRegister(evt_Child);
            PageObject.PageObjectHelper.SelectTopWindow();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.Checkout(reg);
        }
    }
}
