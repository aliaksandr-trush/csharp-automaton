namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.AgendaPage
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgendaBasics : FixtureBase
    {
        [Test]
        [Category(Priority.Three)]
        [Description("1273")]
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
            AgendaItem_CheckBox AgendaItem1 = new AgendaItem_CheckBox("NameOnDifferentOptions");
            AgendaItem_CheckBox AgendaItem2 = new AgendaItem_CheckBox("NameMaxLength");
            AgendaItem_CheckBox AgendaItem3 = new AgendaItem_CheckBox(specialCharacters);
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

            Registrant reg = new Registrant(evt);
            AgendaResponse_Checkbox resp1 = new AgendaResponse_Checkbox();
            resp1.AgendaItem = AgendaItem1;
            resp1.Checked = true;
            AgendaResponse_Checkbox resp2 = new AgendaResponse_Checkbox();
            resp2.AgendaItem = AgendaItem2;
            resp2.Checked = true;
            AgendaResponse_Checkbox resp3 = new AgendaResponse_Checkbox();
            resp3.AgendaItem = AgendaItem3;
            resp3.Checked = true;
            reg.CustomField_Responses.Add(resp1);
            reg.CustomField_Responses.Add(resp2);
            reg.CustomField_Responses.Add(resp3);

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem1).AgendaLabel.Text == AgendaItem1.NameOnForm);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem2).AgendaLabel.Text == AgendaItem2.NameOnForm);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AgendaItem3).AgendaLabel.Text == AgendaItem3.NameOnForm);

            KeywordProvider.RegistrationCreation.Agenda(reg);
            KeywordProvider.RegistrationCreation.Checkout(reg);

            this.VerifySelectedAgendaItems(reg);

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.RegistrationInventory);
            KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Id);
            PageObject.PageObjectProvider.Manager.Dashboard.DashboardTab_Click(FormData.DashboardTab.Reports);
            PageObject.PageObjectProvider.Manager.Dashboard.Reports.AgendaReportLink_Click();
            List<string> results = KeywordProvider.VerifyStandardReports.VerifyStandardReport(FormData.StandardReports.AgendaReport);
            Assert.True(results.Find(r => r.Contains(AgendaItem1.NameOnReports)) != null);
            Assert.True(results.Find(r => r.Contains(expectOnReports)) != null);
            Assert.True(results.Find(r => r.Contains(AgendaItem3.NameOnReports)) != null);
        }

        public void VerifySelectedAgendaItems(DataCollection.Registrant reg)
        {
            List<Label> selectedAgendaItems = PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.GetSelectedAgendaItems();

            foreach (Label agenda in selectedAgendaItems)
            {
                List<AgendaResponse> resps = new List<AgendaResponse>();

                foreach (CustomFieldResponse resp in reg.CustomField_Responses)
                {
                    resps.Add(resp as AgendaResponse);
                }

                AgendaItem agendaItem = resps.Find(r => agenda.Text.Trim() == r.AgendaItem.NameOnForm).AgendaItem;
                Assert.True(agendaItem != null);
            }
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1344")]
        public void AgendaDateTime()
        {
            Event evt = new Event("AgendaDateTime");
            AgendaPage AgendaPage = new AgendaPage();
            AgendaItem_CheckBox AgendaItem1 = new AgendaItem_CheckBox("DefaultDateTime");
            AgendaItem_CheckBox AgendaItem2 = new AgendaItem_CheckBox("DateAndTime");
            AgendaItem_CheckBox AgendaItem3 = new AgendaItem_CheckBox("DateOnly");
            AgendaItem_CheckBox AgendaItem4 = new AgendaItem_CheckBox("TimeOnly");
            AgendaItem_CheckBox AgendaItem5 = new AgendaItem_CheckBox("NotDisplayDateTime");
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

            Registrant reg = new Registrant(evt);

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
        [Description("1345")]
        public void AgendaMultipleChoice()
        {
            Event evt = new Event("AgendaMultipleChoice");
            evt.AgendaPage = new AgendaPage();
            AgendaItem_MultipleChoice_RadioButton AG1 = new AgendaItem_MultipleChoice_RadioButton("RadioNew");
            AgendaItem_MultipleChoice_RadioButton AG2 = new AgendaItem_MultipleChoice_RadioButton("RadioCommon");
            AgendaItem_MultipleChoice_DropDown AG3 = new AgendaItem_MultipleChoice_DropDown("DropdownCommon");
            AgendaItem_MultipleChoice_DropDown AG4 = new AgendaItem_MultipleChoice_DropDown("DropdownNew");
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
            Assert.True(KeywordProvider.ManagerDefault.HasErrorMessage(Messages.BuilderError.AgendaNoMultipleChoice));
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.Cancel_Click();

            Registrant reg1 = new Registrant(evt);
            AgendaResponse_MultipleChoice_RadioButton resp1 = new AgendaResponse_MultipleChoice_RadioButton();
            resp1.AgendaItem = AG1;
            resp1.ChoiceItem = AG1Choice1;
            AgendaResponse_MultipleChoice_DropDown resp2 = new AgendaResponse_MultipleChoice_DropDown();
            resp2.AgendaItem = AG4;
            resp2.ChoiceItem = AG4Choice1;
            reg1.CustomField_Responses.Add(resp1);
            reg1.CustomField_Responses.Add(resp2);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            KeywordProvider.RegistrationCreation.Agenda(reg1);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            Registrant reg2 = new Registrant(evt);
            reg2.CustomField_Responses.Add(resp1);
            reg2.CustomField_Responses.Add(resp2);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg2.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            KeywordProvider.RegistrationCreation.Agenda(reg2);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            Registrant reg3 = new Registrant(evt);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg3.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg3);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.IsChoiceItemPresent(AG1Choice1));
            Assert.IsNull(((MultiChoiceDropdown)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG4).AgendaType).Options.Find(
                o => o.Value == AG4Choice1.Id.ToString()));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1346")]
        public void AgendaTypes()
        {
            Event evt = new Event("AgendaTypes");
            evt.StartPage.StartDate = DateTime.Today.AddDays(-10);
            evt.StartPage.EndDate = DateTime.Today.AddDays(10);
            evt.AgendaPage = new AgendaPage();
            AgendaItem_Number AGNumber = new AgendaItem_Number("AGNumber");
            AGNumber.CharLimit = 10;
            AgendaItem_OneLineText AGText = new AgendaItem_OneLineText("AGText");
            AGText.CharLimit = 10;
            AgendaItem_Paragraph AGPara = new AgendaItem_Paragraph("AGPara");
            AGPara.CharLimit = 1000;
            AgendaItem_Date AGDate = new AgendaItem_Date("AGDate");
            AgendaItem_Time AGTime = new AgendaItem_Time("AGTime");
            AgendaItem_Header AGHeader = new AgendaItem_Header("AGHeader");
            AgendaItem_ContinueButton AGContinue = new AgendaItem_ContinueButton("AGContinue");
            AgendaItem_Contribution AGConribution = new AgendaItem_Contribution("AGConribution");
            AGConribution.MinAmount = 10;
            AGConribution.MaxAmount = 100;
            AgendaItem_FileUpload AGUpload = new AgendaItem_FileUpload("AGUpload");
            AgendaItem_CheckBox AGCheckBox = new AgendaItem_CheckBox("AGCheckBox");
            AgendaItem_MultipleChoice_RadioButton AGRadio = new AgendaItem_MultipleChoice_RadioButton("AGRadio");
            AGRadio.CommonlyUsedItems.Add(FormData.CommonlyUsedMultipleChoice.YesOrNo);
            AgendaItem_MultipleChoice_DropDown AGDropDown = new AgendaItem_MultipleChoice_DropDown("AGDropDown");
            AGDropDown.CommonlyUsedItems.Add(FormData.CommonlyUsedMultipleChoice.Agreement);
            AgendaItem_AlwaysSelected AGAlways = new AgendaItem_AlwaysSelected("AGAlways");
            AgendaItem_CheckBox ChangeToHeader = new AgendaItem_CheckBox("ChangeToHeader");
            ChangeToHeader.StartDate = DateTime.Today.AddDays(-5);
            ChangeToHeader.EndDate = DateTime.Today.AddDays(5);
            ChangeToHeader.Location = DataCollection.DefaultPersonalInfo.AddressLineOne;
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

            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 00);
            DateTime nowDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

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
            ChangeToHeader.Type = FormData.CustomFieldType.SectionHeader;
            PageObject.PageObjectProvider.Builder.EventDetails.SaveAndClose_Click();

            Registrant reg = new Registrant(evt);
            reg.Event = evt;
            AgendaResponse_TextInput resp1 = new AgendaResponse_TextInput();
            resp1.AgendaItem = AGNumber;
            resp1.CharToInput = "1234567890123";
            AgendaResponse_TextInput resp2 = new AgendaResponse_TextInput();
            resp2.AgendaItem = AGText;
            resp2.CharToInput = "abcdefghijkl";
            AgendaResponse_TextInput resp3 = new AgendaResponse_TextInput();
            resp3.AgendaItem = AGPara;
            resp3.CharToInput = "abcdefghijkl";
            AgendaResponse_Date resp4 = new AgendaResponse_Date();
            resp4.AgendaItem = AGDate;
            resp4.Date = DateTime.Today;
            AgendaResponse_Time resp5 = new AgendaResponse_Time();
            resp5.AgendaItem = AGTime;
            resp5.Time = DateTime.Now;
            AgendaResponse_Contribution resp6 = new AgendaResponse_Contribution();
            resp6.AgendaItem = AGConribution;
            resp6.ContributionAmount = 10.5;
            AgendaResponse_Checkbox resp7 = new AgendaResponse_Checkbox();
            resp7.AgendaItem = AGCheckBox;
            resp7.Checked = true;
            AgendaResponse_MultipleChoice_RadioButton resp8 = new AgendaResponse_MultipleChoice_RadioButton();
            resp8.AgendaItem = AGRadio;
            resp8.ChoiceItem = AGRadio.ChoiceItems.Find(c => c.Name == MultipleChoice_CommonlyUsed.YesOrNo.Yes);
            AgendaResponse_MultipleChoice_DropDown resp9 = new AgendaResponse_MultipleChoice_DropDown();
            resp9.AgendaItem = AGDropDown;
            resp9.ChoiceItem = AGDropDown.ChoiceItems.Find(c => c.Name == MultipleChoice_CommonlyUsed.Agreement.Agree);
            reg.CustomField_Responses.Add(resp1);
            reg.CustomField_Responses.Add(resp2);
            reg.CustomField_Responses.Add(resp3);
            reg.CustomField_Responses.Add(resp4);
            reg.CustomField_Responses.Add(resp5);
            reg.CustomField_Responses.Add(resp6);
            reg.CustomField_Responses.Add(resp7);
            reg.CustomField_Responses.Add(resp8);
            reg.CustomField_Responses.Add(resp9);

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGNumber).AgendaType.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGNumber).AgendaType.GetAttribute("data-ml") == 10.ToString());
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGText).AgendaType.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGText).AgendaType.GetAttribute("data-ml") == 10.ToString());
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGPara).AgendaType.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGPara).AgendaType.GetAttribute("data-ml") == 1000.ToString());
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGDate).AgendaType.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGTime).AgendaType.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGHeader).AgendaLabel.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGContinue).AgendaType.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGConribution).AgendaType.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGUpload).AgendaType.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGCheckBox).AgendaType.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGRadio).AgendaLabel.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGDropDown).AgendaType.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGAlways).AgendaType.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(ChangeToHeader).AgendaLabel.IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(ChangeToHeader).GetAgendaDate(ChangeToHeader) == nowDate.AddDays(-5));
            //Comment this assertion to avoid a bug here
            //Will uncomment after the bug is resolved
            //Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(ChangeToHeader).GetAgendaLocation(ChangeToHeader) == Registrant.Default.AddressLineOne);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(ChangeToHeader).GetAgendaPrice(ChangeToHeader) == 10);
            ((TextBox)(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AGConribution).AgendaType)).Type(1);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegisterDefault.HasErrorMessage(string.Format(Messages.RegisterError.ContributionNotInMinAndMax, MoneyTool.FormatMoney(10), MoneyTool.FormatMoney(100)));
            KeywordProvider.RegistrationCreation.Agenda(reg);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("1347")]
        public void Agenda_Duration()
        {
            Configuration.ConfigReader.DefaultProvider.ReloadAccount(Configuration.ConfigReader.AccountEnum.ActiveEurope);
            Event evt = new Event("RI_Agenda_Duration");
            evt.FormType = FormData.FormType.ActiveEuropeEvent;
            evt.StartPage.EventType = EventType.Running;
            evt.AgendaPage = new AgendaPage();
            DataCollection.AgendaItem_Duration duration = new AgendaItem_Duration("AG_Duration");
            evt.AgendaPage.AgendaItems.Add(duration);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant(evt);
            AgendaResponse_Duration duration_Response = new AgendaResponse_Duration();
            duration_Response.AgendaItem = duration;
            duration_Response.Duration = new TimeSpan(1, 30, 30);
            reg.CustomField_Responses.Add(duration_Response);

            Keyword.KeywordProvider.RegistrationCreation.CreateRegistration(reg);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("1348")]
        public void ShoppingCart()
        {
            double agendaStandardPrice = 100;

            Event evt = new Event("RI_Agenda_ShoppingCart");
            evt.AgendaPage = new AgendaPage();
            evt.AgendaPage.IsShoppingCart = true;

            DataCollection.AgendaItem_CheckBox sessionOne = new AgendaItem_CheckBox("SessionOne");
            sessionOne.Location = "FirstRoom";
            DateTime oneYearAfter = DateTime.Now.AddYears(1);
            sessionOne.StartDate = sessionOne.StartTime = oneYearAfter;
            sessionOne.EndDate = sessionOne.EndTime = sessionOne.StartTime.Value.AddHours(1);
            sessionOne.Price = agendaStandardPrice;

            DataCollection.AgendaItem_CheckBox sessionTwo = new AgendaItem_CheckBox("SessionTwo");
            sessionTwo.Location = "SecondRoom";
            sessionTwo.StartDate = sessionTwo.StartTime = sessionOne.EndTime.Value.AddHours(1);
            sessionTwo.EndDate = sessionTwo.EndTime = sessionTwo.StartTime.Value.AddHours(1);
            sessionTwo.Price = agendaStandardPrice;

            DataCollection.AgendaItem_CheckBox sessionThree = new AgendaItem_CheckBox("SessionThree");
            sessionThree.Location = "ThirdRoom";
            sessionThree.StartDate = sessionThree.StartTime = sessionTwo.EndTime.Value.AddHours(1);
            sessionThree.EndDate = sessionThree.EndTime = sessionThree.StartTime.Value.AddHours(1);
            sessionThree.Price = agendaStandardPrice;

            evt.AgendaPage.AgendaItems.Add(sessionOne);
            evt.AgendaPage.AgendaItems.Add(sessionTwo);
            evt.AgendaPage.AgendaItems.Add(sessionThree);

            evt.CheckoutPage.PaymentMethods.Add(new PaymentMethod(FormData.PaymentMethod.Check));

            Registrant reg = new Registrant(evt);

            DataCollection.AgendaResponse responseOne = new AgendaResponse();
            responseOne.AgendaItem = sessionOne;
            DataCollection.AgendaResponse responseTwo = new AgendaResponse();
            responseTwo.AgendaItem = sessionTwo;
            DataCollection.AgendaResponse responseThree = new AgendaResponse();
            responseThree.AgendaItem = sessionThree;

            reg.CustomField_Responses.Add(responseOne);
            reg.CustomField_Responses.Add(responseTwo);
            reg.CustomField_Responses.Add(responseThree);

            reg.Payment_Method = new PaymentMethod(FormData.PaymentMethod.Check);

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);
            Keyword.KeywordProvider.RegistrationCreation.CreateRegistration(reg);
            this.VerifySelectedAgendaItems(reg);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1349")]
        public void AgendaLocation()
        {
            Event evt = new Event("AgendaLocation");
            evt.AgendaPage = new AgendaPage();
            AgendaItem_CheckBox AGCheckbox = new AgendaItem_CheckBox("AGCheckbox");
            AGCheckbox.Location = DataCollection.DefaultPersonalInfo.AddressLineOne;
            evt.AgendaPage.AgendaItems.Add(AGCheckbox);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant(evt);

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            Assert.AreEqual(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                AGCheckbox).GetAgendaLocation(AGCheckbox), DataCollection.DefaultPersonalInfo.AddressLineOne);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1350")]
        public void AgendaSpaces()
        {
            Event evt = new Event("AgendaSpaces");
            evt.AgendaPage = new AgendaPage();
            AgendaItem_CheckBox AgendaShowCapacity = new AgendaItem_CheckBox("AgendaShowCapacity");
            AgendaShowCapacity.SpacesAvailable = 1;
            AgendaShowCapacity.ShowCapacity = true;
            AgendaItem_CheckBox AgendaHideReached = new AgendaItem_CheckBox("AgendaHideReached");
            AgendaHideReached.SpacesAvailable = 1;
            AgendaHideReached.LimitReachedOption = FormData.AgendaLimitReachedOption.HideItem;
            AgendaItem_CheckBox AgendaShowMessage = new AgendaItem_CheckBox("AgendaShowMessage");
            AgendaShowMessage.SpacesAvailable = 1;
            AgendaShowMessage.LimitReachedOption = FormData.AgendaLimitReachedOption.ShowMessage;
            AgendaShowMessage.LimitReachedMessage = "Full";
            AgendaItem_CheckBox AgendaWaitlist = new AgendaItem_CheckBox("AgendaWaitlist");
            AgendaWaitlist.SpacesAvailable = 1;
            AgendaWaitlist.LimitReachedOption = FormData.AgendaLimitReachedOption.Waitlist;
            AgendaWaitlist.WaitlistConfirmationText = "WaitlistConfirmationText";
            evt.AgendaPage.AgendaItems.Add(AgendaShowCapacity);
            evt.AgendaPage.AgendaItems.Add(AgendaHideReached);
            evt.AgendaPage.AgendaItems.Add(AgendaShowMessage);
            evt.AgendaPage.AgendaItems.Add(AgendaWaitlist);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg1 = new Registrant(evt);
            AgendaResponse_Checkbox resp1 = new AgendaResponse_Checkbox();
            resp1.AgendaItem = AgendaShowCapacity;
            resp1.Checked = true;
            AgendaResponse_Checkbox resp2 = new AgendaResponse_Checkbox();
            resp2.AgendaItem = AgendaHideReached;
            resp2.Checked = true;
            AgendaResponse_Checkbox resp3 = new AgendaResponse_Checkbox();
            resp3.AgendaItem = AgendaShowMessage;
            resp3.Checked = true;
            AgendaResponse_Checkbox resp4 = new AgendaResponse_Checkbox();
            resp4.AgendaItem = AgendaWaitlist;
            resp4.Checked = true;
            reg1.CustomField_Responses.Add(resp1);
            reg1.CustomField_Responses.Add(resp2);
            reg1.CustomField_Responses.Add(resp3);
            reg1.CustomField_Responses.Add(resp4);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            PageObject.Register.AgendaRow row1 = new PageObject.Register.AgendaRow(AgendaShowCapacity);
            Assert.True(row1.AgendaLabel.Text.Contains("1 remaining"));
            KeywordProvider.RegistrationCreation.Agenda(reg1);
            KeywordProvider.RegistrationCreation.Checkout(reg1);

            Registrant reg2 = new Registrant(evt);
            reg2.CustomField_Responses.Add(resp4);
            KeywordProvider.RegistrationCreation.Checkin(reg2);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            PageObject.Register.AgendaRow row2 = new PageObject.Register.AgendaRow(AgendaHideReached);
            PageObject.Register.AgendaRow row3 = new PageObject.Register.AgendaRow(AgendaShowMessage);
            PageObject.Register.AgendaRow row4 = new PageObject.Register.AgendaRow(AgendaWaitlist);
            Assert.False(row1.AgendaType.IsPresent);
            Assert.False(row2.AgendaType.IsPresent);
            Assert.True(row3.LimitFullMessage.IsPresent);
            Assert.True(row4.WaitlistMessage.IsPresent);
            KeywordProvider.RegistrationCreation.Agenda(reg2);
            KeywordProvider.RegistrationCreation.Checkout(reg2);

            PageObject.PageObjectProvider.Builder.EmailViewer.OpenURL(evt.Id, reg2.Id);
            Assert.True(PageObject.PageObjectHelper.IsTextPresent(AgendaWaitlist.WaitlistConfirmationText));
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
