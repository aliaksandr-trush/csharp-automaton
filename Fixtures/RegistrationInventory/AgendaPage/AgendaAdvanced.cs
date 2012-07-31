namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.AgendaPage
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgendaAdvanced : FixtureBase
    {
        [Test]
        [Category(Priority.Four)]
        public void AgendaEmailAddendum()
        {
            Event evt = new Event("AgendaEmailAddendum");
            evt.AgendaPage = new AgendaPage();
            AgendaItemCheckBox agendaWithEmailAddendum = new AgendaItemCheckBox("EmailAddendum");
            agendaWithEmailAddendum.ConfirmationAddendum = "ConfirmationAddendum";
            evt.AgendaPage.AgendaItems.Add(agendaWithEmailAddendum);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant();
            reg.Event = evt;
            AgendaCheckboxResponse resp = new AgendaCheckboxResponse();
            resp.AgendaItem = agendaWithEmailAddendum;
            resp.Checked = true;
            reg.CustomFieldResponses.Add(resp);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);

            PageObject.PageObjectProvider.Builder.EmailViewer.OpenURL(evt.Id, reg.Id);
            Assert.True(PageObject.PageObjectHelper.IsTextPresent(agendaWithEmailAddendum.ConfirmationAddendum));
        }

        [Test]
        [Category(Priority.Three)]
        public void AgendaCalendaAndEventWeb()
        {
            Event evt = new Event("AgendaCalendaAndEventWeb");
            evt.AgendaPage = new AgendaPage();
            evt.EventWebsite = new EventWebsite();
            evt.EventWebsite.UseEventWebsiteAsTheStartingPageForEvent = true;
            evt.EventWebsite.ShowNavigation = true;
            AgendaItemCheckBox agendaAddToCalenda = new AgendaItemCheckBox("AddToCalenda");
            agendaAddToCalenda.StartDate = DateTime.Today.AddDays(-3);
            agendaAddToCalenda.EndDate = DateTime.Today.AddDays(3);
            agendaAddToCalenda.AddToCalendar = true;
            agendaAddToCalenda.IncludeOnEventWeb = false;
            AgendaItemCheckBox includeOnEventWeb = new AgendaItemCheckBox("OnEventWeb");
            includeOnEventWeb.StartDate = DateTime.Today.AddDays(4);
            includeOnEventWeb.EndDate = DateTime.Today.AddDays(7);
            includeOnEventWeb.AddToCalendar = false;
            includeOnEventWeb.IncludeOnEventWeb = true;
            evt.AgendaPage.AgendaItems.Add(agendaAddToCalenda);
            evt.AgendaPage.AgendaItems.Add(includeOnEventWeb);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant();
            reg.Event = evt;
            reg.RegisterMethod = RegisterMethod.EventWebsite;
            AgendaCheckboxResponse resp1 = new AgendaCheckboxResponse();
            resp1.AgendaItem = agendaAddToCalenda;
            resp1.Checked = true;
            AgendaCheckboxResponse resp2 = new AgendaCheckboxResponse();
            resp2.AgendaItem = includeOnEventWeb;
            resp2.Checked = true;
            reg.CustomFieldResponses.Add(resp1);
            reg.CustomFieldResponses.Add(resp2);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.EventWebsite.Agenda_Click();
            Assert.True(PageObject.PageObjectHelper.IsTextPresent(includeOnEventWeb.NameOnForm));
            Assert.False(PageObject.PageObjectHelper.IsTextPresent(agendaAddToCalenda.NameOnForm));
            PageObject.PageObjectProvider.Register.RegistationSite.EventWebsite.RegisterNow_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            KeywordProvider.RegistrationCreation.Agenda(reg);
            KeywordProvider.RegistrationCreation.Checkout(reg);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.AddToCalendar(agendaAddToCalenda).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.AddToCalendar(includeOnEventWeb).IsPresent);
        }

        [Test]
        [Category(Priority.Four)]
        public void AgendaDetailsAndGroupName()
        {
            Event evt = new Event("AgendaDetailsAndGroupName");
            evt.AgendaPage = new AgendaPage();
            AgendaItemCheckBox agendaDetails = new AgendaItemCheckBox("AgendaDetails");
            agendaDetails.DetailsPopup = "DetailsPopup";
            AgendaItemCheckBox agendaDetailsURL = new AgendaItemCheckBox("AgendaDetailsURL");
            agendaDetailsURL.DetailsURL = "www.baidu.com";
            AgendaItemCheckBox agendaGroup1 = new AgendaItemCheckBox("AgendaGroup1");
            agendaGroup1.GroupName = "nameOfGroup";
            AgendaItemCheckBox agendaGroup2 = new AgendaItemCheckBox("AgendaGroup2");
            agendaGroup2.GroupName = "nameOfGroup";
            evt.AgendaPage.AgendaItems.Add(agendaDetails);
            evt.AgendaPage.AgendaItems.Add(agendaDetailsURL);
            evt.AgendaPage.AgendaItems.Add(agendaGroup1);
            evt.AgendaPage.AgendaItems.Add(agendaGroup2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);


        }
    }
}
