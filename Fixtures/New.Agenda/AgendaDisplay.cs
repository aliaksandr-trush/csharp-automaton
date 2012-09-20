namespace RegOnline.RegressionTest.Fixtures.New.Agenda
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;
    using RegOnline.RegressionTest.WebElements;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgendaDisplay : FixtureBase
    {
        [Test]
        [Category(Priority.Three)]
        [Description("1356")]
        public void AgendaConditionalLogic()
        {
            Event evt = new Event("AgendaConditionalLogic");
            CFCheckBox PICustomField = new CFCheckBox("PICustomField");
            evt.PersonalInfoPage.CustomFields.Add(PICustomField);
            evt.AgendaPage = new DataCollection.AgendaPage();
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
            AgendaItem_CheckBox AG1 = new AgendaItem_CheckBox("AG1");
            AG1.ConditionalLogic.Add(PICustomField.NameOnForm);
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
            evt.AgendaPage.AgendaItems.Add(AG1);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Id);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.EditForm_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.Agenda);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddAgendaItem_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ExpandConditionalLogic_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ExpandConditionalLogic_Click();
            Assert.True(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGCheckBox.NameOnForm).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGRadio.NameOnForm).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGDropDown.NameOnForm).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AG1.NameOnForm).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGNumber.NameOnForm).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGText.NameOnForm).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGPara.NameOnForm).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGDate.NameOnForm).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGTime.NameOnForm).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGHeader.NameOnForm).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGContinue.NameOnForm).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGConribution.NameOnForm).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGUpload.NameOnForm).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AGAlways.NameOnForm).IsPresent);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.Cancel_Click();

            AgendaItem_CheckBox AG2 = new AgendaItem_CheckBox("AG2");
            AgendaItem_CheckBox AG3 = new AgendaItem_CheckBox("AG3");
            AG3.ConditionalLogic.Add(AG2.NameOnForm);

            KeywordProvider.AddAgendaItem.AddAgendaItems(AG2, evt);
            KeywordProvider.AddAgendaItem.AddAgendaItems(AG3, evt);
            PageObject.Builder.RegistrationFormPages.AgendaRow row1 = new PageObject.Builder.RegistrationFormPages.AgendaRow(AG2);
            row1.Delete_Click();
            PageObject.Builder.RegistrationFormPages.AgendaRow row2 = new PageObject.Builder.RegistrationFormPages.AgendaRow(AG1);
            row2.Agenda_Click();
            PageObject.Builder.RegistrationFormPages.AgendaRow row3 = new PageObject.Builder.RegistrationFormPages.AgendaRow(AG3);
            row3.Agenda_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ExpandConditionalLogic_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ExpandConditionalLogic_Click();
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AG2.NameOnForm).IsPresent);

            AgendaItem_CheckBox AG4 = new AgendaItem_CheckBox("AG4");
            AG4.ConditionalLogic.Add(AG3.NameOnForm);
            KeywordProvider.AddAgendaItem.AddAgendaItems(AG4, evt);

            Registrant reg1 = new Registrant(evt);
            CFResponse_Checkbox resp = new CFResponse_Checkbox();
            resp.CustomField = PICustomField;
            resp.Checked = true;
            reg1.CustomField_Responses.Add(resp);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            PageObject.Register.AgendaRow row4 = PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG1);
            Assert.True(row4.AgendaType.IsPresent);
            ((CheckBox)(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG3).AgendaType)).Set(true);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG4).AgendaType.IsPresent);

            Registrant reg2 = new Registrant(evt);

            KeywordProvider.RegistrationCreation.Checkin(reg2);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            Assert.False(row4.AgendaType.IsPresent);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1357")]
        public void AgendaVisibility()
        {
            Event evt = new Event("AgendaVisibility");
            evt.AgendaPage = new AgendaPage();
            RegType regType1 = new RegType("RegType1");
            RegType regType2 = new RegType("RegType2");
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);
            AgendaItem_CheckBox visToAll = new AgendaItem_CheckBox("VisToAll");
            AgendaItem_CheckBox visToType1 = new AgendaItem_CheckBox("VisToType1");
            CustomFieldVisibleOption visibleOption1 = new CustomFieldVisibleOption(regType1);
            visibleOption1.Visible = true;
            visToType1.CustomFieldVisibleOption.Add(visibleOption1);
            AgendaItem_CheckBox reqByType2 = new AgendaItem_CheckBox("ReqByType2");
            CustomFieldVisibleOption visibleOption2 = new CustomFieldVisibleOption(regType2);
            visibleOption2.Required = true;
            reqByType2.CustomFieldVisibleOption.Add(visibleOption2);
            AgendaItem_CheckBox adminOnly = new AgendaItem_CheckBox("AdminOnly");
            CustomFieldVisibleOption visibleOption3 = new CustomFieldVisibleOption();
            visibleOption3.AdminOnly = true;
            adminOnly.CustomFieldVisibleOption.Add(visibleOption3);
            AgendaItem_CheckBox adminAndReq = new AgendaItem_CheckBox("AdminAndReq");
            CustomFieldVisibleOption visibleOption4 = new CustomFieldVisibleOption();
            visibleOption4.AdminOnly = true;
            visibleOption4.Required = true;
            adminAndReq.CustomFieldVisibleOption.Add(visibleOption4);
            evt.AgendaPage.AgendaItems.Add(visToAll);
            evt.AgendaPage.AgendaItems.Add(visToType1);
            evt.AgendaPage.AgendaItems.Add(reqByType2);
            evt.AgendaPage.AgendaItems.Add(adminOnly);
            evt.AgendaPage.AgendaItems.Add(adminAndReq);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false);

            Registrant reg3 = new Registrant(evt);
            reg3.EventFee_Response = new EventFeeResponse(regType1);
            reg3.Register_Method = RegisterMethod.Admin;

            KeywordProvider.RegistrationCreation.Checkin(reg3);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg3);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(visToAll, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(visToType1, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(reqByType2, false);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(adminOnly, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(adminAndReq, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.RequiredCheckBoxNotChecked));

            Registrant reg2 = new Registrant(evt);
            reg2.EventFee_Response = new EventFeeResponse(regType2);

            KeywordProvider.RegistrationCreation.Checkin(reg2);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(visToAll, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(visToType1, false);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(reqByType2, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(adminOnly, false);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(adminAndReq, false);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.RequiredCheckBoxNotChecked));

            Registrant reg1 = new Registrant(evt);
            reg1.EventFee_Response = new EventFeeResponse(regType1);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(visToAll, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(visToType1, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(reqByType2, false);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(adminOnly, false);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(adminAndReq, false);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1358")]
        public void AgendaShowHideDate()
        {
            Event evt = new Event("AgendaShowHideDate");
            evt.AgendaPage = new AgendaPage();
            AgendaItem_CheckBox showInPast = new AgendaItem_CheckBox("ShowInPast");
            showInPast.ShowStarting = DateTime.Today.AddDays(-3);
            AgendaItem_CheckBox showInFuture = new AgendaItem_CheckBox("ShowInFuture");
            showInFuture.ShowStarting = DateTime.Today.AddDays(3);
            AgendaItem_CheckBox hideInPast = new AgendaItem_CheckBox("HideInPast");
            hideInPast.HideStarting = DateTime.Today.AddDays(-3);
            AgendaItem_CheckBox hideInFuture = new AgendaItem_CheckBox("HideInFuture");
            hideInFuture.HideStarting = DateTime.Today.AddDays(3);
            AgendaItem_CheckBox sIPHIF = new AgendaItem_CheckBox("SIPHIF");
            sIPHIF.ShowStarting = DateTime.Today.AddDays(-3);
            sIPHIF.HideStarting = DateTime.Today.AddDays(3);
            evt.AgendaPage.AgendaItems.Add(showInPast);
            evt.AgendaPage.AgendaItems.Add(showInFuture);
            evt.AgendaPage.AgendaItems.Add(hideInPast);
            evt.AgendaPage.AgendaItems.Add(hideInFuture);
            evt.AgendaPage.AgendaItems.Add(sIPHIF);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant(evt);

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(showInPast, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(showInFuture, false);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(hideInPast, false);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(hideInFuture, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(sIPHIF, true);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1359")]
        public void AgendaShowIf()
        {
            Event evt = new Event("AgendaShowIf");
            evt.AgendaPage = new AgendaPage();
            AgendaItem_CheckBox showToMale = new AgendaItem_CheckBox("ShowToMale");
            showToMale.Gender = FormData.Gender.Male;
            AgendaItem_CheckBox showToFemale = new AgendaItem_CheckBox("ShowToFemale");
            showToFemale.Gender = FormData.Gender.Female;
            AgendaItem_CheckBox showOver20 = new AgendaItem_CheckBox("ShowOver20");
            showOver20.AgeGreaterThan = 20;
            showOver20.AgeGreaterThanDate = DateTime.Today;
            AgendaItem_CheckBox showLT20 = new AgendaItem_CheckBox("ShowLT20");
            showLT20.AgeLessThan = 20;
            showLT20.AgeLessThanDate = DateTime.Today;
            evt.AgendaPage.AgendaItems.Add(showToMale);
            evt.AgendaPage.AgendaItems.Add(showToFemale);
            evt.AgendaPage.AgendaItems.Add(showOver20);
            evt.AgendaPage.AgendaItems.Add(showLT20);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false);

            Registrant reg1 = new Registrant(evt);
            reg1.Gender = FormData.Gender.Male;
            reg1.BirthDate = DateTime.Today.AddYears(-22);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(showToMale, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(showToFemale, false);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(showOver20, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(showLT20, false);

            Registrant reg2 = new Registrant(evt);
            reg2.Gender = FormData.Gender.Female;
            reg2.BirthDate = DateTime.Today.AddYears(-18);

            KeywordProvider.RegistrationCreation.Checkin(reg2);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(showToMale, false);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(showToFemale, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(showOver20, false);
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.VerifyAgendaItemDisplay(showLT20, true);
        }
    }
}
