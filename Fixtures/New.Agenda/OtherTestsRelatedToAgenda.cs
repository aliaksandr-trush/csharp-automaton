namespace RegOnline.RegressionTest.Fixtures.New.Agenda
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class OtherTestsRelatedToAgenda : FixtureBase
    {
        [Test]
        [Category(Priority.Four)]
        [Description("1367")]
        public void ButtonTest()
        {
            Event evt = new Event("AgendaButtonTest");
            AgendaItem_CheckBox agenda1 = new AgendaItem_CheckBox("agenda1");
            AgendaItem_CheckBox agenda2 = new AgendaItem_CheckBox("agenda2");

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.RegistrationInventory);

            if (KeywordProvider.Manager_Common.DoesEventExist(evt.Title))
            {
                KeywordProvider.Manager_Common.DeleteEvent(evt.Title);
            }

            KeywordProvider.Event_Creator.ClickAddEventAndGetEventId(evt);
            KeywordProvider.Event_Creator.StartPage(evt);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(DataCollection.EventData_Common.Page.Agenda);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.YesOnSplashPage_Click();
            KeywordProvider.Add_AgendaItem.AddAgendaItems(agenda1, evt);
            int id = Convert.ToInt32(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaItemId.Value);
            PageObject.Builder.RegistrationFormPages.AgendaRow row1 = new PageObject.Builder.RegistrationFormPages.AgendaRow(agenda1);
            Assert.True(row1.Agenda.IsPresent);
            Assert.AreEqual(row1.Agenda.GetAttribute("class"), "hs colwidth1");
            KeywordProvider.Add_AgendaItem.AddAgendaItems(agenda2, evt);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.SaveAndNew_Click();
            Assert.AreNotEqual(id, Convert.ToInt32(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaItemId.Value));
            Assert.AreEqual(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnForm.Text, "");
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.Cancel_Click();
            Assert.AreEqual(id, Convert.ToInt32(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaItemId.Value));
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnForm.Type("NameChanged");
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.Cancel_Click();
            Assert.AreEqual(id, Convert.ToInt32(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaItemId.Value));
            Assert.AreEqual(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnForm.Text, agenda1.NameOnForm);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnForm.Type("NameChanged");
            PageObject.PageObjectProvider.Builder.EventDetails.SaveAndStay_Click();
            Assert.AreEqual(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnForm.Text, "NameChanged");
        }

        [Test]
        [Category(Priority.Four)]
        [Description("1368")]
        public void AgendaCopyDelete()
        {
            Event evt = new Event("AgendaCopyDelete");
            evt.AgendaPage = new AgendaPage();
            AgendaItem_CheckBox agenda1 = new AgendaItem_CheckBox("agenda1");
            AgendaItem_CheckBox agenda2 = new AgendaItem_CheckBox("agenda2");
            evt.AgendaPage.AgendaItems.Add(agenda1);
            evt.AgendaPage.AgendaItems.Add(agenda2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant(evt);
            AgendaResponse_Checkbox resp = new AgendaResponse_Checkbox();
            resp.AgendaItem = agenda1;
            resp.Checked = true;
            reg.CustomField_Responses.Add(resp);

            KeywordProvider.Registration_Creation.CreateRegistration(reg);
            KeywordProvider.SignIn.SignIn(EventFolders.Folders.RegistrationInventory);
            KeywordProvider.Manager_Common.OpenFormDashboard(evt.Id);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.EditForm_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(DataCollection.EventData_Common.Page.Agenda);

            PageObject.Builder.RegistrationFormPages.AgendaRow row1 = new PageObject.Builder.RegistrationFormPages.AgendaRow(agenda1);
            PageObject.Builder.RegistrationFormPages.AgendaRow row2 = new PageObject.Builder.RegistrationFormPages.AgendaRow(agenda2);
            Assert.True(row1.Delete.GetAttribute("src").Contains("deletex_Off.gif"));
            row2.Copy_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CopyAgendaAmount.Type(1);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.OK_Click();
            Assert.AreEqual(agenda2.NameOnForm + " Copy1",
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnReports.Value);
            row2.Delete_Click();
            Assert.False(row2.Agenda.IsPresent);
        }

        [Test]
        [Category(Priority.Four)]
        [Description("1369")]
        public void OverlappingAgenda()
        {
            Event evt = new Event("AgendaOverlapping");
            evt.AgendaPage = new AgendaPage();
            AgendaItem_CheckBox agenda1 = new AgendaItem_CheckBox("agenda1");
            agenda1.StartDate = DateTime.Today.AddDays(3);
            agenda1.EndDate = DateTime.Today.AddDays(9);
            AgendaItem_CheckBox agenda2 = new AgendaItem_CheckBox("agenda2");
            agenda2.StartDate = DateTime.Today.AddDays(6);
            agenda2.EndDate = DateTime.Today.AddDays(12);
            evt.AgendaPage.AgendaItems.Add(agenda1);
            evt.AgendaPage.AgendaItems.Add(agenda2);
            evt.AgendaPage.DoNotAllowOverlapping = true;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg1 = new Registrant(evt);
            AgendaResponse_Checkbox resp1 = new AgendaResponse_Checkbox();
            resp1.AgendaItem = agenda1;
            resp1.Checked = true;
            AgendaResponse_Checkbox resp2 = new AgendaResponse_Checkbox();
            resp2.AgendaItem = agenda2;
            resp2.Checked = true;
            reg1.CustomField_Responses.Add(resp1);
            reg1.CustomField_Responses.Add(resp2);

            KeywordProvider.Registration_Creation.Checkin(reg1);
            KeywordProvider.Registration_Creation.PersonalInfo(reg1);
            KeywordProvider.Registration_Creation.Agenda(reg1);
            Assert.True(KeywordProvider.Register_Common.HasErrorMessage(Messages.RegisterError.AgendaOverlapped));

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.RegistrationInventory);
            KeywordProvider.Manager_Common.OpenFormDashboard(evt.Id);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.EditForm_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(DataCollection.EventData_Common.Page.Agenda);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DoNotAllowOverlapping.Set(false);
            PageObject.PageObjectProvider.Builder.EventDetails.SaveAndClose_Click();

            Registrant reg2 = new Registrant(evt);
            reg2.CustomField_Responses.Add(resp1);
            reg2.CustomField_Responses.Add(resp2);

            KeywordProvider.Registration_Creation.CreateRegistration(reg2);
        }

        [Test]
        [Category(Priority.Four)]
        [Description("1370")]
        public void RecalculateTotal()
        {
            Event evt = new Event("AgendaRecalculate");
            evt.AgendaPage = new AgendaPage();
            AgendaItem_CheckBox agenda1 = new AgendaItem_CheckBox("agenda1");
            agenda1.Price = 50;
            AgendaItem_CheckBox agenda2 = new AgendaItem_CheckBox("agenda2");
            agenda2.Price = 60;
            CustomFieldCode discount = new CustomFieldCode("discount");
            discount.Amount = 10;
            discount.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            discount.CodeKind = DataCollection.EventData_Common.ChangeType.Percent;
            discount.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            agenda2.DiscountCodes.Add(discount);
            evt.AgendaPage.AgendaItems.Add(agenda1);
            evt.AgendaPage.AgendaItems.Add(agenda2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false);

            Registrant reg = new Registrant(evt);

            KeywordProvider.Registration_Creation.Checkin(reg);
            KeywordProvider.Registration_Creation.PersonalInfo(reg);
            PageObject.Register.AgendaRow row1 = new PageObject.Register.AgendaRow(agenda1);
            PageObject.Register.AgendaRow row2 = new PageObject.Register.AgendaRow(agenda2);
            ((WebElements.CheckBox)(row1.AgendaType)).Set(true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.RecalculateTotal_Click();
            Assert.AreEqual(agenda1.Price, KeywordProvider.Register_Common.GetTotal(DataCollection.EventData_Common.RegisterPage.Agenda));
            ((WebElements.CheckBox)(row2.AgendaType)).Set(true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.RecalculateTotal_Click();
            Assert.AreEqual(agenda1.Price + agenda2.Price, KeywordProvider.Register_Common.GetTotal(DataCollection.EventData_Common.RegisterPage.Agenda));
            ((WebElements.CheckBox)(row1.AgendaType)).Set(false);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.RecalculateTotal_Click();
            Assert.AreEqual(agenda2.Price, KeywordProvider.Register_Common.GetTotal(DataCollection.EventData_Common.RegisterPage.Agenda));
            row2.DiscountCodeInput.Type(discount.CodeString);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.RecalculateTotal_Click();
            Assert.AreEqual(54, KeywordProvider.Register_Common.GetTotal(DataCollection.EventData_Common.RegisterPage.Agenda));
        }
    }
}
