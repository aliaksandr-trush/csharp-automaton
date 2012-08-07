namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.AgendaPage
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgendaUpdate : FixtureBase
    {
        [Test]
        [Category(Priority.Three)]
        public void UpdateReg()
        {
            Event evt = new Event("AgendaUpdateReg");
            evt.AgendaPage = new AgendaPage();
            AgendaItemCheckBox agenda1 = new AgendaItemCheckBox("Agenda1");
            AgendaItemCheckBox agenda2 = new AgendaItemCheckBox("Agenda2");
            AgendaItemCheckBox agenda3 = new AgendaItemCheckBox("Agenda3");
            evt.AgendaPage.AgendaItems.Add(agenda1);
            evt.AgendaPage.AgendaItems.Add(agenda2);
            evt.AgendaPage.AgendaItems.Add(agenda3);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant(evt);
            AgendaCheckboxResponse resp1 = new AgendaCheckboxResponse();
            resp1.AgendaItem = agenda1;
            resp1.Checked = true;
            AgendaCheckboxResponse resp2 = new AgendaCheckboxResponse();
            resp2.AgendaItem = agenda2;
            resp2.Checked = true;
            AgendaCheckboxResponse resp3 = new AgendaCheckboxResponse();
            resp3.AgendaItem = agenda3;
            resp3.Checked = true;
            reg.CustomFieldResponses.Add(resp1);
            reg.CustomFieldResponses.Add(resp2);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.Agenda_Click(0);
            resp2.IsUpdate = true;
            resp2.Checked = false;
            reg.CustomFieldResponses.Add(resp3);
            KeywordProvider.RegistrationCreation.Agenda(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.Checkout(reg);

            List<WebElements.Label> selectedAgendas = PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.GetSelectedAgendaItems();
            Assert.IsNotNull(selectedAgendas.Find(s => s.Text == agenda1.NameOnForm));
            Assert.IsNull(selectedAgendas.Find(s => s.Text == agenda2.NameOnForm));
            Assert.IsNotNull(selectedAgendas.Find(s => s.Text == agenda3.NameOnForm));
        }

        [Test]
        [Category(Priority.Three)]
        public void BackendUpdate()
        {
            Event evt = new Event("AgendaBackendUpdate");
            evt.AgendaPage = new AgendaPage();
            AgendaItemCheckBox agenda1 = new AgendaItemCheckBox("Agenda1");
            AgendaItemCheckBox agenda2 = new AgendaItemCheckBox("Agenda2");
            AgendaItemCheckBox agenda3 = new AgendaItemCheckBox("Agenda3");
            AgendaItemCheckBox agenda4 = new AgendaItemCheckBox("Agenda4");
            agenda4.SpacesAvailable = 1;
            AgendaItemCheckBox agenda5 = new AgendaItemCheckBox("Agenda5");
            agenda5.Gender = FormData.Gender.Male;
            evt.AgendaPage.AgendaItems.Add(agenda1);
            evt.AgendaPage.AgendaItems.Add(agenda2);
            evt.AgendaPage.AgendaItems.Add(agenda3);
            evt.AgendaPage.AgendaItems.Add(agenda4);
            evt.AgendaPage.AgendaItems.Add(agenda5);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg1 = new Registrant(evt);
            reg1.Gender = FormData.Gender.Male;
            AgendaCheckboxResponse resp1 = new AgendaCheckboxResponse();
            resp1.AgendaItem = agenda1;
            resp1.Checked = true;
            AgendaCheckboxResponse resp2 = new AgendaCheckboxResponse();
            resp2.AgendaItem = agenda2;
            resp2.Checked = true;
            AgendaCheckboxResponse resp3 = new AgendaCheckboxResponse();
            resp3.AgendaItem = agenda3;
            resp3.Checked = true;
            AgendaCheckboxResponse resp4 = new AgendaCheckboxResponse();
            resp4.AgendaItem = agenda4;
            resp4.Checked = true;
            AgendaCheckboxResponse resp5 = new AgendaCheckboxResponse();
            resp5.AgendaItem = agenda5;
            resp5.Checked = true;
            reg1.CustomFieldResponses.Add(resp1);
            reg1.CustomFieldResponses.Add(resp2);
            reg1.CustomFieldResponses.Add(resp3);
            reg1.CustomFieldResponses.Add(resp4);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg1);

            Registrant reg2 = new Registrant(evt);
            reg2.Gender = FormData.Gender.Female;
            resp1.IsUpdate = true;
            resp2.IsUpdate = true;
            reg2.CustomFieldResponses.Add(resp1);
            reg2.CustomFieldResponses.Add(resp2);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg2);

            PageObject.Backend.AttendeeInfo attendeeInfo = new PageObject.Backend.AttendeeInfo();
            attendeeInfo.OpenUrl(reg2.Id);
            resp2.IsUpdate = true;
            resp3.IsUpdate = true;
            resp4.IsUpdate = true;
            resp5.IsUpdate = true;
            resp2.Checked = false;
            reg2.CustomFieldResponses.Add(resp3);
            reg2.CustomFieldResponses.Add(resp4);
            reg2.CustomFieldResponses.Add(resp5);
            KeywordProvider.BackendUpdate.UpdateCustomField(reg2);
            Assert.True(attendeeInfo.AgendaLable(agenda1).IsPresent);
            Assert.False(attendeeInfo.AgendaLable(agenda2).IsPresent);
            Assert.True(attendeeInfo.AgendaLable(agenda3).IsPresent);
            Assert.True(attendeeInfo.AgendaLable(agenda4).IsPresent);
            Assert.True(attendeeInfo.AgendaLable(agenda5).IsPresent);
        }
    }
}
