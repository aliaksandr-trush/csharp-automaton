namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.AgendaPage
{
    using System;
    using System.Collections.Generic;
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
        [Description("1351")]
        public void AgendaEmailAddendum()
        {
            Event evt = new Event("AgendaEmailAddendum");
            evt.AgendaPage = new AgendaPage();
            AgendaItemCheckBox agendaWithEmailAddendum = new AgendaItemCheckBox("EmailAddendum");
            agendaWithEmailAddendum.ConfirmationAddendum = "ConfirmationAddendum";
            evt.AgendaPage.AgendaItems.Add(agendaWithEmailAddendum);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant(evt);
            AgendaCheckboxResponse resp = new AgendaCheckboxResponse();
            resp.AgendaItem = agendaWithEmailAddendum;
            resp.Checked = true;
            reg.CustomField_Responses.Add(resp);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);

            PageObject.PageObjectProvider.Builder.EmailViewer.OpenURL(evt.Id, reg.Id);
            Assert.True(PageObject.PageObjectHelper.IsTextPresent(agendaWithEmailAddendum.ConfirmationAddendum));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1352")]
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

            Registrant reg = new Registrant(evt);
            reg.Register_Method = RegisterMethod.EventWebsite;
            AgendaCheckboxResponse resp1 = new AgendaCheckboxResponse();
            resp1.AgendaItem = agendaAddToCalenda;
            resp1.Checked = true;
            AgendaCheckboxResponse resp2 = new AgendaCheckboxResponse();
            resp2.AgendaItem = includeOnEventWeb;
            resp2.Checked = true;
            reg.CustomField_Responses.Add(resp1);
            reg.CustomField_Responses.Add(resp2);

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
        [Description("1353")]
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

            Registrant reg = new Registrant(evt);
            AgendaCheckboxResponse resp1 = new AgendaCheckboxResponse();
            resp1.AgendaItem = agendaGroup1;
            resp1.Checked = true;
            AgendaCheckboxResponse resp2 = new AgendaCheckboxResponse();
            resp2.AgendaItem = agendaGroup2;
            resp2.Checked = true;
            reg.CustomField_Responses.Add(resp1);
            reg.CustomField_Responses.Add(resp2);

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            PageObject.Register.AgendaRow row1 = new PageObject.Register.AgendaRow(agendaDetails);
            PageObject.Register.AgendaRow row2 = new PageObject.Register.AgendaRow(agendaDetailsURL);
            row1.Details.Click();
            Assert.AreEqual(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.AgendaDetailsPopup.Text,
                agendaDetails.DetailsPopup);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.CloseDetailsPopup_Click();
            row2.Details.Click();
            System.Threading.Thread.Sleep(1500);
            PageObject.PageObjectHelper.SelectTopWindow();
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.AgendaDetailsWindow.URLContains(agendaDetailsURL.DetailsURL));
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.AgendaDetailsWindow.CloseAndBackToPrevious();
            KeywordProvider.RegistrationCreation.Agenda(reg);
            KeywordProvider.RegistrationCreation.Checkout(reg);
            List<WebElements.Label> selectedAgenda = PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.GetSelectedAgendaItems();
            Assert.IsNull(selectedAgenda.Find(a => a.Text == agendaGroup1.NameOnForm));
            Assert.IsNotNull(selectedAgenda.Find(a => a.Text == agendaGroup2.NameOnForm));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1354")]
        public void AgendaForceGroup()
        {
            Event evt = new Event("AgendaForceGroup");
            evt.AgendaPage = new AgendaPage();
            AgendaItemCheckBox ag1 = new AgendaItemCheckBox("ag1");
            ag1.ForceGroupToMatch = true;
            AgendaItemCheckBox ag2 = new AgendaItemCheckBox("ag2");
            ag2.ForceGroupToMatch = true;
            ag2.Gender = FormData.Gender.Male;
            AgendaItemCheckBox ag3 = new AgendaItemCheckBox("ag3");
            ag3.ForceGroupToMatch = true;
            ag3.AgeGreaterThan = 20;
            ag3.AgeGreaterThanDate = DateTime.Today;
            AgendaItemCheckBox ag4 = new AgendaItemCheckBox("ag4");
            ag4.ForceGroupToMatch = true;
            AgendaItemCheckBox ag5 = new AgendaItemCheckBox("ag5");
            ag5.ForceGroupToMatch = true;
            ag5.ConditionalLogic.Add(ag4.NameOnForm);
            AgendaItemCheckBox ag6 = new AgendaItemCheckBox("ag6");
            ag6.ForceGroupToMatch = true;
            AgendaItemCheckBox ag7 = new AgendaItemCheckBox("ag7");
            ag7.ConditionalLogic.Add(ag6.NameOnForm);
            evt.AgendaPage.AgendaItems.Add(ag1);
            evt.AgendaPage.AgendaItems.Add(ag2);
            evt.AgendaPage.AgendaItems.Add(ag3);
            evt.AgendaPage.AgendaItems.Add(ag4);
            evt.AgendaPage.AgendaItems.Add(ag5);
            evt.AgendaPage.AgendaItems.Add(ag6);
            evt.AgendaPage.AgendaItems.Add(ag7);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            AgendaCheckboxResponse resp1 = new AgendaCheckboxResponse();
            resp1.AgendaItem = ag1;
            resp1.Checked = true;
            AgendaCheckboxResponse resp2 = new AgendaCheckboxResponse();
            resp2.AgendaItem = ag2;
            resp2.Checked = true;
            AgendaCheckboxResponse resp3 = new AgendaCheckboxResponse();
            resp3.AgendaItem = ag3;
            resp3.Checked = true;
            AgendaCheckboxResponse resp4 = new AgendaCheckboxResponse();
            resp4.AgendaItem = ag4;
            resp4.Checked = true;
            AgendaCheckboxResponse resp5 = new AgendaCheckboxResponse();
            resp5.AgendaItem = ag5;
            resp5.Checked = true;
            AgendaCheckboxResponse resp6 = new AgendaCheckboxResponse();
            resp6.AgendaItem = ag6;
            resp6.Checked = true;
            AgendaCheckboxResponse resp7 = new AgendaCheckboxResponse();
            resp7.AgendaItem = ag7;
            resp7.Checked = true;

            Registrant reg1 = new Registrant(evt);
            reg1.Gender = FormData.Gender.Male;
            reg1.BirthDate = DateTime.Today.AddYears(-22);
            reg1.CustomField_Responses.Add(resp1);
            reg1.CustomField_Responses.Add(resp2);
            reg1.CustomField_Responses.Add(resp3);
            reg1.CustomField_Responses.Add(resp4);
            reg1.CustomField_Responses.Add(resp5);
            reg1.CustomField_Responses.Add(resp6);
            reg1.CustomField_Responses.Add(resp7);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            KeywordProvider.RegistrationCreation.Agenda(reg1);

            PageObject.Register.AgendaRow row1 = new PageObject.Register.AgendaRow(ag1);
            PageObject.Register.AgendaRow row2 = new PageObject.Register.AgendaRow(ag2);
            PageObject.Register.AgendaRow row3 = new PageObject.Register.AgendaRow(ag3);
            PageObject.Register.AgendaRow row4 = new PageObject.Register.AgendaRow(ag4);
            PageObject.Register.AgendaRow row5 = new PageObject.Register.AgendaRow(ag5);
            PageObject.Register.AgendaRow row6 = new PageObject.Register.AgendaRow(ag6);
            PageObject.Register.AgendaRow row7 = new PageObject.Register.AgendaRow(ag7);

            Registrant reg2 = new Registrant(evt);
            reg2.Gender = FormData.Gender.Male;
            reg2.BirthDate = DateTime.Today.AddYears(-18);

            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg2.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            Assert.True(((WebElements.CheckBox)(row1.AgendaType)).IsChecked);
            Assert.True(((WebElements.CheckBox)(row1.AgendaType)).HasAttribute("disabled"));
            Assert.True(((WebElements.CheckBox)(row2.AgendaType)).IsChecked);
            Assert.True(((WebElements.CheckBox)(row2.AgendaType)).HasAttribute("disabled"));
            Assert.False(((WebElements.CheckBox)(row3.AgendaType)).IsPresent);
            Assert.True(((WebElements.CheckBox)(row4.AgendaType)).IsChecked);
            Assert.True(((WebElements.CheckBox)(row4.AgendaType)).HasAttribute("disabled"));
            Assert.True(((WebElements.CheckBox)(row5.AgendaType)).IsChecked);
            Assert.True(((WebElements.CheckBox)(row5.AgendaType)).HasAttribute("disabled"));
            Assert.True(((WebElements.CheckBox)(row6.AgendaType)).IsChecked);
            Assert.True(((WebElements.CheckBox)(row6.AgendaType)).HasAttribute("disabled"));
            Assert.False(((WebElements.CheckBox)(row7.AgendaType)).IsChecked);
            Assert.False(((WebElements.CheckBox)(row7.AgendaType)).HasAttribute("disabled"));
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();

            Registrant reg3 = new Registrant(evt);
            reg3.Gender = FormData.Gender.Female;
            reg3.BirthDate = DateTime.Today.AddYears(-22);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg3.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg3);
            Assert.False(((WebElements.CheckBox)(row2.AgendaType)).IsPresent);
            Assert.True(((WebElements.CheckBox)(row3.AgendaType)).IsChecked);
            Assert.True(((WebElements.CheckBox)(row3.AgendaType)).HasAttribute("disabled"));

            Registrant reg4 = new Registrant(evt);
            reg4.Gender = FormData.Gender.Male;
            reg4.BirthDate = DateTime.Today.AddYears(-22);
            reg4.CustomField_Responses.Add(resp4);

            KeywordProvider.RegistrationCreation.Checkin(reg4);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg4);
            KeywordProvider.RegistrationCreation.Agenda(reg4);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();

            Registrant reg5 = new Registrant(evt);
            reg5.Gender = FormData.Gender.Male;
            reg5.BirthDate = DateTime.Today.AddYears(-22);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg5.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg5);
            Assert.True(((WebElements.CheckBox)(row4.AgendaType)).IsChecked);
            Assert.True(((WebElements.CheckBox)(row4.AgendaType)).HasAttribute("disabled"));
            Assert.False(((WebElements.CheckBox)(row5.AgendaType)).IsChecked);
            Assert.True(((WebElements.CheckBox)(row5.AgendaType)).HasAttribute("disabled"));
        }

        [Test]
        [Category(Priority.Four)]
        [Description("1355")]
        public void AgendaStatus()
        {
            Event evt = new Event("AgendaStatus");
            evt.AgendaPage = new AgendaPage();
            AgendaItemCheckBox ag1 = new AgendaItemCheckBox("ag1");
            AgendaItemCheckBox ag2 = new AgendaItemCheckBox("ag2");
            ag2.InitialStatus = FormData.AgendaInitialStatus.Pending;
            AgendaItemCheckBox ag3 = new AgendaItemCheckBox("ag3");
            ag3.InitialStatus = FormData.AgendaInitialStatus.Confirmed;
            AgendaItemCheckBox ag4 = new AgendaItemCheckBox("ag4");
            ag4.InitialStatus = FormData.AgendaInitialStatus.Approved;
            AgendaItemCheckBox ag5 = new AgendaItemCheckBox("ag5");
            ag5.InitialStatus = FormData.AgendaInitialStatus.Declined;
            AgendaItemCheckBox ag6 = new AgendaItemCheckBox("ag6");
            ag6.InitialStatus = FormData.AgendaInitialStatus.NoShow;
            AgendaItemCheckBox ag7 = new AgendaItemCheckBox("ag7");
            ag7.InitialStatus = FormData.AgendaInitialStatus.FollowUp;
            evt.AgendaPage.AgendaItems.Add(ag1);
            evt.AgendaPage.AgendaItems.Add(ag2);
            evt.AgendaPage.AgendaItems.Add(ag3);
            evt.AgendaPage.AgendaItems.Add(ag4);
            evt.AgendaPage.AgendaItems.Add(ag5);
            evt.AgendaPage.AgendaItems.Add(ag6);
            evt.AgendaPage.AgendaItems.Add(ag7);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            AgendaCheckboxResponse resp1 = new AgendaCheckboxResponse();
            resp1.AgendaItem = ag1;
            resp1.Checked = true;
            AgendaCheckboxResponse resp2 = new AgendaCheckboxResponse();
            resp2.AgendaItem = ag2;
            resp2.Checked = true;
            AgendaCheckboxResponse resp3 = new AgendaCheckboxResponse();
            resp3.AgendaItem = ag3;
            resp3.Checked = true;
            AgendaCheckboxResponse resp4 = new AgendaCheckboxResponse();
            resp4.AgendaItem = ag4;
            resp4.Checked = true;
            AgendaCheckboxResponse resp5 = new AgendaCheckboxResponse();
            resp5.AgendaItem = ag5;
            resp5.Checked = true;
            AgendaCheckboxResponse resp6 = new AgendaCheckboxResponse();
            resp6.AgendaItem = ag6;
            resp6.Checked = true;
            AgendaCheckboxResponse resp7 = new AgendaCheckboxResponse();
            resp7.AgendaItem = ag7;
            resp7.Checked = true;

            Registrant reg = new Registrant(evt);
            reg.CustomField_Responses.Add(resp1);
            reg.CustomField_Responses.Add(resp2);
            reg.CustomField_Responses.Add(resp3);
            reg.CustomField_Responses.Add(resp4);
            reg.CustomField_Responses.Add(resp5);
            reg.CustomField_Responses.Add(resp6);
            reg.CustomField_Responses.Add(resp7);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.RegistrationInventory);
            KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Id);
            PageObject.PageObjectProvider.Manager.Dashboard.DashboardTab_Click(FormData.DashboardTab.Reports);
            PageObject.PageObjectProvider.Manager.Dashboard.Reports.AgendaReportLink_Click();

            PageObject.Reports.StandardReports agendaReport = new PageObject.Reports.StandardReports(FormData.StandardReports.AgendaReport);

            Assert.AreEqual(agendaReport.AgendaReportRows.Find(r => r.AgendaName.Text.Contains(ag1.NameOnForm)).Attendees[0].AgendaStatus.Text.Trim(), "");
            Assert.AreEqual(agendaReport.AgendaReportRows.Find(r => r.AgendaName.Text.Contains(ag2.NameOnForm)).Attendees[0].AgendaStatus.Text.Trim(), "Pending");
            Assert.AreEqual(agendaReport.AgendaReportRows.Find(r => r.AgendaName.Text.Contains(ag3.NameOnForm)).Attendees[0].AgendaStatus.Text.Trim(), "Confirmed");
            Assert.AreEqual(agendaReport.AgendaReportRows.Find(r => r.AgendaName.Text.Contains(ag4.NameOnForm)).Attendees[0].AgendaStatus.Text.Trim(), "Approved");
            Assert.AreEqual(agendaReport.AgendaReportRows.Find(r => r.AgendaName.Text.Contains(ag5.NameOnForm)).Attendees[0].AgendaStatus.Text.Trim(), "Declined");
            Assert.AreEqual(agendaReport.AgendaReportRows.Find(r => r.AgendaName.Text.Contains(ag6.NameOnForm)).Attendees[0].AgendaStatus.Text.Trim(), "No-show");
            Assert.AreEqual(agendaReport.AgendaReportRows.Find(r => r.AgendaName.Text.Contains(ag7.NameOnForm)).Attendees[0].AgendaStatus.Text.Trim(), "Follow-up");

            agendaReport.CloseAndBackToPrevious();
        }
    }
}
