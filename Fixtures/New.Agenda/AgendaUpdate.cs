﻿namespace RegOnline.RegressionTest.Fixtures.New.Agenda
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
        [Description("1360")]
        public void UpdateReg()
        {
            Event evt = new Event("AgendaUpdateReg");
            evt.AgendaPage = new AgendaPage();
            AgendaItem_CheckBox agenda1 = new AgendaItem_CheckBox("Agenda1");
            AgendaItem_CheckBox agenda2 = new AgendaItem_CheckBox("Agenda2");
            AgendaItem_CheckBox agenda3 = new AgendaItem_CheckBox("Agenda3");
            evt.AgendaPage.AgendaItems.Add(agenda1);
            evt.AgendaPage.AgendaItems.Add(agenda2);
            evt.AgendaPage.AgendaItems.Add(agenda3);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false);

            Registrant reg = new Registrant(evt);
            AgendaResponse_Checkbox resp1 = new AgendaResponse_Checkbox();
            resp1.AgendaItem = agenda1;
            resp1.Checked = true;
            AgendaResponse_Checkbox resp2 = new AgendaResponse_Checkbox();
            resp2.AgendaItem = agenda2;
            resp2.Checked = true;
            AgendaResponse_Checkbox resp3 = new AgendaResponse_Checkbox();
            resp3.AgendaItem = agenda3;
            resp3.Checked = true;
            reg.CustomField_Responses.Add(resp1);
            reg.CustomField_Responses.Add(resp2);

            KeywordProvider.Registration_Creation.CreateRegistration(reg);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.Registration_Creation.Login(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.Agenda_Click(0);
            resp2.Checked = false;
            reg.CustomField_Responses.Add(resp2);
            reg.CustomField_Responses.Add(resp3);
            KeywordProvider.Registration_Creation.Agenda(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.Registration_Creation.CheckoutAndConfirmation(reg);

            List<WebElements.Label> selectedAgendas = PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.GetSelectedAgendaItems();
            Assert.IsNotNull(selectedAgendas.Find(s => s.Text == agenda1.NameOnForm));
            Assert.IsNull(selectedAgendas.Find(s => s.Text == agenda2.NameOnForm));
            Assert.IsNotNull(selectedAgendas.Find(s => s.Text == agenda3.NameOnForm));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1361")]
        public void BackendUpdate()
        {
            Event evt = new Event("AgendaBackendUpdate");
            evt.AgendaPage = new AgendaPage();
            AgendaItem_CheckBox agenda1 = new AgendaItem_CheckBox("Agenda1");
            AgendaItem_CheckBox agenda2 = new AgendaItem_CheckBox("Agenda2");
            AgendaItem_CheckBox agenda3 = new AgendaItem_CheckBox("Agenda3");
            AgendaItem_CheckBox agenda4 = new AgendaItem_CheckBox("Agenda4");
            agenda4.SpacesAvailable = 1;
            AgendaItem_CheckBox agenda5 = new AgendaItem_CheckBox("Agenda5");
            agenda5.Gender = DataCollection.EventData_Common.Gender.Male;
            evt.AgendaPage.AgendaItems.Add(agenda1);
            evt.AgendaPage.AgendaItems.Add(agenda2);
            evt.AgendaPage.AgendaItems.Add(agenda3);
            evt.AgendaPage.AgendaItems.Add(agenda4);
            evt.AgendaPage.AgendaItems.Add(agenda5);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, true);

            Registrant reg1 = new Registrant(evt);
            reg1.Gender = DataCollection.EventData_Common.Gender.Male;
            AgendaResponse_Checkbox resp1 = new AgendaResponse_Checkbox();
            resp1.AgendaItem = agenda1;
            resp1.Checked = true;
            AgendaResponse_Checkbox resp2 = new AgendaResponse_Checkbox();
            resp2.AgendaItem = agenda2;
            resp2.Checked = true;
            AgendaResponse_Checkbox resp3 = new AgendaResponse_Checkbox();
            resp3.AgendaItem = agenda3;
            resp3.Checked = true;
            AgendaResponse_Checkbox resp4 = new AgendaResponse_Checkbox();
            resp4.AgendaItem = agenda4;
            resp4.Checked = true;
            AgendaResponse_Checkbox resp5 = new AgendaResponse_Checkbox();
            resp5.AgendaItem = agenda5;
            resp5.Checked = false;
            reg1.CustomField_Responses.Add(resp1);
            reg1.CustomField_Responses.Add(resp2);
            reg1.CustomField_Responses.Add(resp3);
            reg1.CustomField_Responses.Add(resp4);
            reg1.CustomField_Responses.Add(resp5);

            KeywordProvider.Registration_Creation.CreateRegistration(reg1);

            Registrant reg2 = new Registrant(evt);
            reg2.Gender = DataCollection.EventData_Common.Gender.Female;
            reg2.CustomField_Responses.Add(resp1);
            reg2.CustomField_Responses.Add(resp2);

            KeywordProvider.Registration_Creation.CreateRegistration(reg2);

            PageObject.PageObjectProvider.Backend.AttendeeInfo.OpenUrl(reg2.Id);
            resp2.Checked = false;
            reg2.CustomField_Responses.Add(resp2);
            reg2.CustomField_Responses.Add(resp3);
            reg2.CustomField_Responses.Add(resp4);
            resp5.Checked = true;
            reg2.CustomField_Responses.Add(resp5);
            KeywordProvider.Backend_Update.UpdateCustomField(reg2);
            Assert.True(PageObject.PageObjectProvider.Backend.AttendeeInfo.AgendaLable(agenda1).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Backend.AttendeeInfo.AgendaLable(agenda2).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Backend.AttendeeInfo.AgendaLable(agenda3).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Backend.AttendeeInfo.AgendaLable(agenda4).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Backend.AttendeeInfo.AgendaLable(agenda5).IsPresent);
        }
    }
}
