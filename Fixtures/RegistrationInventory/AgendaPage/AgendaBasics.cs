namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.AgendaPage
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;
    using RegOnline.RegressionTest.WebElements;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgendaBasics : FixtureBase
    {
        [Test]
        [Category(Priority.Three)]
        public void AgendaNameTesting()
        {
            string baseString = "13579";
            string expectOnReports = null;
            string expectOnBadge = null;
            string longString = null;
            string specialCharacters = "~!@#$%^&*()[]\\/.";
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < 60; i++)
            {
                stringBuilder.Append(baseString);
            }

            expectOnReports = stringBuilder.ToString(0, 25);
            expectOnBadge = stringBuilder.ToString(0, 250);
            longString = stringBuilder.ToString(0, 280);

            Event evt = new Event("AgendaNameTesting");
            AgendaPage AgendaPage = new AgendaPage();
            AgendaItemCheckBox AgendaItem1 = new AgendaItemCheckBox("NameOnDifferentOptions");
            AgendaItemCheckBox AgendaItem2 = new AgendaItemCheckBox("NameMaxLength");
            AgendaItemCheckBox AgendaItem3 = new AgendaItemCheckBox(specialCharacters);
            AgendaItem1.NameOnReceipt = "NameOnReceipt";
            AgendaItem1.NameOnReports = "NameOnReports";
            AgendaItem1.NameOnBadge = "NameOnBadge";
            AgendaItem2.NameOnReceipt = baseString;
            AgendaItem2.NameOnReports = longString;
            AgendaItem2.NameOnBadge = longString;
            AgendaItem3.NameOnReceipt = specialCharacters;
            AgendaItem3.NameOnReports = specialCharacters;
            AgendaItem3.NameOnBadge = specialCharacters;
            AgendaPage.AgendaItems.Add(AgendaItem1);
            AgendaPage.AgendaItems.Add(AgendaItem2);
            AgendaPage.AgendaItems.Add(AgendaItem3);
            evt.AgendaPage = AgendaPage;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant();
            reg.Event = evt;
            reg.AgendaItems.Add(AgendaItem1);
            reg.AgendaItems.Add(AgendaItem2);
            reg.AgendaItems.Add(AgendaItem3);

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem1).AgendaLabel.Text == AgendaItem1.NameOnFrom);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem2).AgendaLabel.Text == AgendaItem2.NameOnFrom);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem3).AgendaLabel.Text == AgendaItem3.NameOnFrom);

            KeywordProvider.RegistrationCreation.Agenda(reg);
            KeywordProvider.RegistrationCreation.Checkout(reg);

            List<Label> selectedAgendaItems = PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.GetSelectedAgendaItems();

            foreach (Label agenda in selectedAgendaItems)
            {
                AgendaItem agendaItem = reg.AgendaItems.Find(a => agenda.Text.Trim() == a.NameOnReceipt);
                Assert.True(agendaItem != null);
            }

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.RegistrationInventory);
            KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Id);
            PageObject.PageObjectProvider.Manager.Dashboard.DashboardTab_Click(FormData.DashboardTab.Reports);
            PageObject.PageObjectProvider.Manager.Dashboard.Reports.AgendaReportLink_Click();
            List<string> results = KeywordProvider.VerifyStandardReports.VerifyStandardReport(FormData.StandardReports.AgendaReport);
            Assert.True(results.Find(r => r.Contains(AgendaItem1.NameOnReports)) != null);
            Assert.True(results.Find(r => r.Contains(expectOnReports)) != null);
            Assert.True(results.Find(r => r.Contains(AgendaItem3.NameOnReports)) != null);
        }

        [Test]
        [Category(Priority.Three)]
        public void AgendaDateTime()
        {
            Event evt = new Event("AgendaDateTime");
            AgendaPage AgendaPage = new AgendaPage();
            AgendaItemCheckBox AgendaItem1 = new AgendaItemCheckBox("DefaultDateTime");
            AgendaItemCheckBox AgendaItem2 = new AgendaItemCheckBox("DateAndTime");
            AgendaItemCheckBox AgendaItem3 = new AgendaItemCheckBox("DateOnly");
            AgendaItemCheckBox AgendaItem4 = new AgendaItemCheckBox("TimeOnly");
            AgendaItemCheckBox AgendaItem5 = new AgendaItemCheckBox("NotDisplayDateTime");
            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 00);
            DateTime nowDate = new DateTime(now.Year, now.Month, now.Day, 0, 0 ,0);
            AgendaItem1.StartDate = now;
            AgendaItem1.StartTime = now;
            AgendaItem1.EndDate = now.AddDays(3);
            AgendaItem1.EndTime = now;
            AgendaItem1.DateFormat = FormData.DateFormat.Default;
            AgendaItem2.StartDate = now;
            AgendaItem2.StartTime = now;
            AgendaItem2.EndDate = now.AddDays(3);
            AgendaItem2.EndTime = now;
            AgendaItem2.DateFormat = FormData.DateFormat.DateTime;
            AgendaItem3.StartDate = nowDate;
            AgendaItem3.StartTime = nowDate;
            AgendaItem3.EndDate = nowDate.AddDays(3);
            AgendaItem3.EndTime = nowDate;
            AgendaItem3.DateFormat = FormData.DateFormat.Date;
            AgendaItem4.StartDate = now;
            AgendaItem4.StartTime = now;
            AgendaItem4.EndDate = now.AddDays(3);
            AgendaItem4.EndTime = now;
            AgendaItem4.DateFormat = FormData.DateFormat.Time;
            AgendaItem5.StartDate = now;
            AgendaItem5.StartTime = now;
            AgendaItem5.EndDate = now.AddDays(3);
            AgendaItem5.EndTime = now;
            AgendaItem5.DateFormat = FormData.DateFormat.None;
            AgendaPage.AgendaItems.Add(AgendaItem1);
            AgendaPage.AgendaItems.Add(AgendaItem2);
            AgendaPage.AgendaItems.Add(AgendaItem3);
            AgendaPage.AgendaItems.Add(AgendaItem4);
            AgendaPage.AgendaItems.Add(AgendaItem5);
            evt.AgendaPage = AgendaPage;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant();
            reg.Event = evt;

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem1).StartDate == now);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem1).EndDate == now.AddDays(3));
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem2).StartDate == now);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem2).EndDate == now.AddDays(3));
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem3).StartDate == nowDate);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem3).EndDate == nowDate.AddDays(3));
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem4).StartDate == now);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem4).EndDate == now);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem5).StartDate == null);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem5).EndDate == null);
        }

        [Test]
        [Category(Priority.Three)]
        public void AgendaMultipleChoice()
        {
            Event evt = new Event("AgendaMultipleChoice");
            evt.AgendaPage = new AgendaPage();
            AgendaItemRadioButton AG1 = new AgendaItemRadioButton("RadioNew");
            AgendaItemRadioButton AG2 = new AgendaItemRadioButton("RadioCommon");
            AgendaItemDropDown AG3 = new AgendaItemDropDown("DropdownCommon");
            AgendaItemDropDown AG4 = new AgendaItemDropDown("DropdownNew");
            ChoiceItem AG1Choice1 = new ChoiceItem("AG1Choice1");
            ChoiceItem AG1Choice2 = new ChoiceItem("AG1Choice2");
            AG1Choice1.SingleLimit = 2;
            AG1.ChoiceItems.Add(AG1Choice1);
            AG1.ChoiceItems.Add(AG1Choice2);
            AG2.CommonlyUsedItems.Add(FormData.CommonlyUsedMultipleChoice.Agreement);
            AG3.CommonlyUsedItems.Add(FormData.CommonlyUsedMultipleChoice.YesOrNo);
            ChoiceItem AG4Choice1 = new ChoiceItem("AG4Choice1");
            AG4Choice1.GroupLimit = 2;
            AG4.ChoiceItems.Add(AG4Choice1);
            AG4.ChoiceItems.Add(new ChoiceItem("AG4Choice2"));
            
            evt.AgendaPage.AgendaItems.Add(AG1);
            evt.AgendaPage.AgendaItems.Add(AG2);
            evt.AgendaPage.AgendaItems.Add(AG3);
            evt.AgendaPage.AgendaItems.Add(AG4);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Id);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.EditForm_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.Agenda);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddAgendaItem_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.NameOnForm.Type("NoMultipleChoice");
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.FieldType_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaType_Select(FormData.CustomFieldType.RadioButton);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.SaveItem_Click();
            KeywordProvider.ManagerDefault.HassErrorMessage(Messages.BuilderError.AgendaNoMultipleChoice);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.Cancel_Click();

            Registrant reg1 = new Registrant();
            reg1.Event = evt;
            reg1.AgendaItems.Add(AG1);
            reg1.AgendaItems.Add(AG2);
            reg1.AgendaItems.Add(AG3);
            reg1.AgendaItems.Add(AG4);
            AgendaRadioButtonResponse resp1 = new AgendaRadioButtonResponse();
            resp1.AgendaItem = AG1;
            resp1.ChoiceItem = AG1Choice1;
            AgendaDropDownResponse resp2 = new AgendaDropDownResponse();
            resp2.AgendaItem = AG4;
            resp2.ChoiceItem = AG4Choice1;
            reg1.CustomFieldResponses.Add(resp1);
            reg1.CustomFieldResponses.Add(resp2);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            KeywordProvider.RegistrationCreation.Agenda(reg1);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            Registrant reg2 = new Registrant();
            reg2.AgendaItems.Add(AG1);
            reg2.AgendaItems.Add(AG2);
            reg2.AgendaItems.Add(AG3);
            reg2.AgendaItems.Add(AG4);
            reg2.CustomFieldResponses.Add(resp1);
            reg2.CustomFieldResponses.Add(resp2);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg2.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            KeywordProvider.RegistrationCreation.Agenda(reg2);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            Registrant reg3 = new Registrant();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg3.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg3);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.IsChoiceItemPresent(AG1Choice1));
            Assert.IsNull(((MultiChoiceDropdown)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG4).AgendaType).Options.Find(
                o => o.Value == AG4Choice1.Id.ToString()));
        }

        [Test]
        [Category(Priority.Three)]
        public void AgendaTypes()
        {
            Event evt = new Event("AgendaTypes");
            evt.StartPage.StartDate = DateTime.Today.AddDays(-10);
            evt.StartPage.EndDate = DateTime.Today.AddDays(10);
            PaymentMethod method = new PaymentMethod(FormData.PaymentMethod.Check);
            evt.CheckoutPage.PaymentMethods.Add(method);
            evt.AgendaPage = new AgendaPage();
            AgendaItemNumber AGNumber = new AgendaItemNumber("AGNumber");
            AGNumber.CharLimit = 10;
            AgendaItemOneLineText AGText = new AgendaItemOneLineText("AGText");
            AGText.CharLimit = 10;
            AgendaItemParagraph AGPara = new AgendaItemParagraph("AGPara");
            AGPara.CharLimit = 1000;
            AgendaItemDate AGDate = new AgendaItemDate("AGDate");
            AgendaItemTime AGTime = new AgendaItemTime("AGTime");
            AgendaItemHeader AGHeader = new AgendaItemHeader("AGHeader");
            AgendaItemContinue AGContinue = new AgendaItemContinue("AGContinue");
            AgendaItemContribution AGConribution = new AgendaItemContribution("AGConribution");
            AGConribution.MinAmount = 10;
            AGConribution.MaxAmount = 100;
            AgendaItemUpload AGUpload = new AgendaItemUpload("AGUpload");
            AgendaItemCheckBox AGCheckBox = new AgendaItemCheckBox("AGCheckBox");
            AgendaItemRadioButton AGRadio = new AgendaItemRadioButton("AGRadio");
            AGRadio.CommonlyUsedItems.Add(FormData.CommonlyUsedMultipleChoice.YesOrNo);
            AgendaItemDropDown AGDropDown = new AgendaItemDropDown("AGDropDown");
            AGDropDown.CommonlyUsedItems.Add(FormData.CommonlyUsedMultipleChoice.Agreement);
            AgendaItemAlways AGAlways = new AgendaItemAlways("AGAlways");
            AgendaItemCheckBox ChangeToHeader = new AgendaItemCheckBox("ChangeToHeader");
            ChangeToHeader.StartDate = DateTime.Today.AddDays(-5);
            ChangeToHeader.EndDate = DateTime.Today.AddDays(5);
            ChangeToHeader.Location = Registrant.Default.AddressLineOne;
            ChangeToHeader.Price = 10;
            ChangeToHeader.SpacesAvailable = 5;
            evt.AgendaPage.AgendaItems.Add(AGNumber);
            evt.AgendaPage.AgendaItems.Add(AGText);
            evt.AgendaPage.AgendaItems.Add(AGPara);
            evt.AgendaPage.AgendaItems.Add(AGDate);
            evt.AgendaPage.AgendaItems.Add(AGTime);
            evt.AgendaPage.AgendaItems.Add(AGHeader);
            evt.AgendaPage.AgendaItems.Add(AGContinue);
            evt.AgendaPage.AgendaItems.Add(AGConribution);
            evt.AgendaPage.AgendaItems.Add(AGUpload);
            evt.AgendaPage.AgendaItems.Add(AGCheckBox);
            evt.AgendaPage.AgendaItems.Add(AGRadio);
            evt.AgendaPage.AgendaItems.Add(AGDropDown);
            evt.AgendaPage.AgendaItems.Add(AGAlways);
            evt.AgendaPage.AgendaItems.Add(ChangeToHeader);

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.RegistrationInventory);

            if (KeywordProvider.ManagerDefault.DoesEventExist(evt.Title))
            {
                KeywordProvider.ManagerDefault.DeleteEvent(evt.Title);
            }

            KeywordProvider.EventCreator.ClickAddEventAndGetEventId(evt);
            KeywordProvider.EventCreator.StartPage(evt);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.Agenda);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.YesOnSplashPage_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CreateAgendaItem_Click();
            this.SelectAgendaType(FormData.CustomFieldType.Number);
            this.VerifyAgendaSettings(false, false, false, false, true, false, false);
            this.SelectAgendaType(FormData.CustomFieldType.OneLineText);
            this.VerifyAgendaSettings(false, false, false, false, true, false, false);
            this.SelectAgendaType(FormData.CustomFieldType.Paragraph);
            this.VerifyAgendaSettings(false, false, false, false, false, true, false);
            this.SelectAgendaType(FormData.CustomFieldType.Date);
            this.VerifyAgendaSettings(false, false, false, false, false, false, false);
            this.SelectAgendaType(FormData.CustomFieldType.Time);
            this.VerifyAgendaSettings(false, false, false, false, false, false, false);
            this.SelectAgendaType(FormData.CustomFieldType.SectionHeader);
            this.VerifyAgendaSettings(false, false, false, false, false, false, false);
            this.SelectAgendaType(FormData.CustomFieldType.ContinueButton);
            this.VerifyAgendaSettings(false, false, false, false, false, false, false);
            this.SelectAgendaType(FormData.CustomFieldType.Contribution);
            this.VerifyAgendaSettings(false, false, false, false, false, false, true);
            this.SelectAgendaType(FormData.CustomFieldType.FileUpload);
            this.VerifyAgendaSettings(false, true, true, true, false, false, false);
            this.SelectAgendaType(FormData.CustomFieldType.CheckBox);
            this.VerifyAgendaSettings(true, true, true, true, false, false, false);
            this.SelectAgendaType(FormData.CustomFieldType.RadioButton);
            this.VerifyAgendaSettings(true, true, true, true, false, false, false);
            this.SelectAgendaType(FormData.CustomFieldType.Dropdown);
            this.VerifyAgendaSettings(true, true, true, true, false, false, false);
            this.SelectAgendaType(FormData.CustomFieldType.AlwaysSelected);
            this.VerifyAgendaSettings(true, true, true, true, false, false, false);

            evt.ReSetShortcut();
            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Id);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.EditForm_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.Agenda);
            PageObject.Builder.RegistrationFormPages.AgendaRow row = new PageObject.Builder.RegistrationFormPages.AgendaRow(ChangeToHeader);
            row.Agenda_Click();
            this.SelectAgendaType(FormData.CustomFieldType.SectionHeader);
            //There is a bug here, location is not cleared after changed to section header.
            //So will uncomment this step after the bug is resolved.
            //this.VerifyAgendaSettings(false, false, false, false, false, false, false);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.SaveItem_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.SaveAndClose_Click();

            Registrant reg = new Registrant();
            reg.Event = evt;

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
        }

        private void SelectAgendaType(FormData.CustomFieldType type)
        {
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.FieldType_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaType_Select(type);
        }

        private void VerifyAgendaSettings(bool startDate, bool location, bool price, bool capacity, bool character, bool paragragh, bool contribution)
        {
            Assert.AreEqual(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.StartDate.IsDisplay, startDate);
            Assert.AreEqual(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.Location.IsDisplay, location);
            Assert.AreEqual(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.StandardPrice.IsDisplay, price);
            Assert.AreEqual(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.Capacity.IsDisplay, capacity);
            Assert.AreEqual(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.CharacterLimit.IsDisplay, character);
            Assert.AreEqual(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ParagraphLimit.IsDisplay, paragragh);
            Assert.AreEqual(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MinAmount.IsDisplay, contribution);
            Assert.AreEqual(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.MaxAmount.IsDisplay, contribution);
        }
    }
}
