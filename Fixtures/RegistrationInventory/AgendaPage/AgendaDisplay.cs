namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.AgendaPage
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
            AgendaItemCheckBox AG1 = new AgendaItemCheckBox("AG1");
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

            AgendaItemCheckBox AG2 = new AgendaItemCheckBox("AG2");
            AgendaItemCheckBox AG3 = new AgendaItemCheckBox("AG3");
            AG3.ConditionalLogic.Add(AG2.NameOnForm);

            KeywordProvider.AddAgendaItem.AddAgendaItems(AG2, evt);
            AG2.Id = Convert.ToInt32(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaItemId.Value);
            KeywordProvider.AddAgendaItem.AddAgendaItems(AG3, evt);
            AG3.Id = Convert.ToInt32(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaItemId.Value);
            PageObject.Builder.RegistrationFormPages.AgendaRow row1 = new PageObject.Builder.RegistrationFormPages.AgendaRow(AG2);
            row1.Delete_Click();
            PageObject.Builder.RegistrationFormPages.AgendaRow row2 = new PageObject.Builder.RegistrationFormPages.AgendaRow(AG1);
            row2.Agenda_Click();
            PageObject.Builder.RegistrationFormPages.AgendaRow row3 = new PageObject.Builder.RegistrationFormPages.AgendaRow(AG3);
            row3.Agenda_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ExpandConditionalLogic_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ExpandConditionalLogic_Click();
            Assert.False(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.ConditionalLogicParent(AG2.NameOnForm).IsPresent);

            AgendaItemCheckBox AG4 = new AgendaItemCheckBox("AG4");
            AG4.ConditionalLogic.Add(AG3.NameOnForm);
            KeywordProvider.AddAgendaItem.AddAgendaItems(AG4, evt);
            AG4.Id = Convert.ToInt32(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaItemId.Value);

            Registrant reg1 = new Registrant(evt);
            CFCheckboxResponse resp = new CFCheckboxResponse();
            resp.CustomField = PICustomField;
            resp.Checked = true;
            reg1.CustomField_Responses.Add(resp);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG1).AgendaType.IsPresent);
            ((CheckBox)(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG3).AgendaType)).Set(true);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG4).AgendaType.IsPresent);

            Registrant reg2 = new Registrant(evt);

            KeywordProvider.RegistrationCreation.Checkin(reg2);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG1).AgendaType.IsPresent);
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
            AgendaItemCheckBox visToAll = new AgendaItemCheckBox("VisToAll");
            AgendaItemCheckBox visToType1 = new AgendaItemCheckBox("VisToType1");
            CustomFieldVisibleOption visibleOption1 = new CustomFieldVisibleOption(regType1);
            visibleOption1.Visible = true;
            visToType1.CustomFieldVisibleOption.Add(visibleOption1);
            AgendaItemCheckBox reqByType2 = new AgendaItemCheckBox("ReqByType2");
            CustomFieldVisibleOption visibleOption2 = new CustomFieldVisibleOption(regType2);
            visibleOption2.Required = true;
            reqByType2.CustomFieldVisibleOption.Add(visibleOption2);
            AgendaItemCheckBox adminOnly = new AgendaItemCheckBox("AdminOnly");
            CustomFieldVisibleOption visibleOption3 = new CustomFieldVisibleOption();
            visibleOption3.AdminOnly = true;
            adminOnly.CustomFieldVisibleOption.Add(visibleOption3);
            AgendaItemCheckBox adminAndReq = new AgendaItemCheckBox("AdminAndReq");
            CustomFieldVisibleOption visibleOption4 = new CustomFieldVisibleOption();
            visibleOption4.AdminOnly = true;
            visibleOption4.Required = true;
            adminAndReq.CustomFieldVisibleOption.Add(visibleOption4);
            evt.AgendaPage.AgendaItems.Add(visToAll);
            evt.AgendaPage.AgendaItems.Add(visToType1);
            evt.AgendaPage.AgendaItems.Add(reqByType2);
            evt.AgendaPage.AgendaItems.Add(adminOnly);
            evt.AgendaPage.AgendaItems.Add(adminAndReq);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg1 = new Registrant(evt);
            reg1.RegType_Response = new RegTypeResponse(regType1);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            PageObject.Register.AgendaRow row1 = new PageObject.Register.AgendaRow(visToAll);
            PageObject.Register.AgendaRow row2 = new PageObject.Register.AgendaRow(visToType1);
            PageObject.Register.AgendaRow row3 = new PageObject.Register.AgendaRow(reqByType2);
            PageObject.Register.AgendaRow row4 = new PageObject.Register.AgendaRow(adminOnly);
            PageObject.Register.AgendaRow row5 = new PageObject.Register.AgendaRow(adminAndReq);
            Assert.True(row1.AgendaType.IsPresent);
            Assert.True(row2.AgendaType.IsPresent);
            Assert.False(row3.AgendaType.IsPresent);
            Assert.False(row4.AgendaType.IsPresent);
            Assert.False(row5.AgendaType.IsPresent);

            Registrant reg2 = new Registrant(evt);
            reg2.RegType_Response = new RegTypeResponse(regType2);

            KeywordProvider.RegistrationCreation.Checkin(reg2);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            Assert.True(row1.AgendaType.IsPresent);
            Assert.False(row2.AgendaType.IsPresent);
            Assert.True(row3.AgendaType.IsPresent);
            Assert.False(row4.AgendaType.IsPresent);
            Assert.False(row5.AgendaType.IsPresent);
            KeywordProvider.RegistrationCreation.Agenda(reg2);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.RequiredCheckBoxNotChecked));

            Registrant reg3 = new Registrant(evt);
            reg3.RegType_Response = new RegTypeResponse(regType1);
            reg3.Register_Method = RegisterMethod.Admin;

            KeywordProvider.RegistrationCreation.Checkin(reg3);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg3);
            Assert.True(row1.AgendaType.IsPresent);
            Assert.True(row2.AgendaType.IsPresent);
            Assert.False(row3.AgendaType.IsPresent);
            Assert.True(row4.AgendaType.IsPresent);
            Assert.True(row5.AgendaType.IsPresent);
            KeywordProvider.RegistrationCreation.Agenda(reg3);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.RequiredCheckBoxNotChecked));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1358")]
        public void AgendaShowHideDate()
        {
            Event evt = new Event("AgendaShowHideDate");
            evt.AgendaPage = new AgendaPage();
            AgendaItemCheckBox showInPast = new AgendaItemCheckBox("ShowInPast");
            showInPast.ShowStarting = DateTime.Today.AddDays(-3);
            AgendaItemCheckBox showInFuture = new AgendaItemCheckBox("ShowInFuture");
            showInFuture.ShowStarting = DateTime.Today.AddDays(3);
            AgendaItemCheckBox hideInPast = new AgendaItemCheckBox("HideInPast");
            hideInPast.HideStarting = DateTime.Today.AddDays(-3);
            AgendaItemCheckBox hideInFuture = new AgendaItemCheckBox("HideInFuture");
            hideInFuture.HideStarting = DateTime.Today.AddDays(3);
            AgendaItemCheckBox sIPHIF = new AgendaItemCheckBox("SIPHIF");
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
            PageObject.Register.AgendaRow row1 = new PageObject.Register.AgendaRow(showInPast);
            PageObject.Register.AgendaRow row2 = new PageObject.Register.AgendaRow(showInFuture);
            PageObject.Register.AgendaRow row3 = new PageObject.Register.AgendaRow(hideInPast);
            PageObject.Register.AgendaRow row4 = new PageObject.Register.AgendaRow(hideInFuture);
            PageObject.Register.AgendaRow row5 = new PageObject.Register.AgendaRow(sIPHIF);
            Assert.True(row1.AgendaLabel.IsPresent);
            Assert.False(row2.AgendaLabel.IsPresent);
            Assert.False(row3.AgendaLabel.IsPresent);
            Assert.True(row4.AgendaLabel.IsPresent);
            Assert.True(row5.AgendaLabel.IsPresent);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1359")]
        public void AgendaShowIf()
        {
            Event evt = new Event("AgendaShowIf");
            evt.AgendaPage = new AgendaPage();
            AgendaItemCheckBox showToMale = new AgendaItemCheckBox("ShowToMale");
            showToMale.Gender = FormData.Gender.Male;
            AgendaItemCheckBox showToFemale = new AgendaItemCheckBox("ShowToFemale");
            showToFemale.Gender = FormData.Gender.Female;
            AgendaItemCheckBox showOver20 = new AgendaItemCheckBox("ShowOver20");
            showOver20.AgeGreaterThan = 20;
            showOver20.AgeGreaterThanDate = DateTime.Today;
            AgendaItemCheckBox showLT20 = new AgendaItemCheckBox("ShowLT20");
            showLT20.AgeLessThan = 20;
            showLT20.AgeLessThanDate = DateTime.Today;
            evt.AgendaPage.AgendaItems.Add(showToMale);
            evt.AgendaPage.AgendaItems.Add(showToFemale);
            evt.AgendaPage.AgendaItems.Add(showOver20);
            evt.AgendaPage.AgendaItems.Add(showLT20);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg1 = new Registrant(evt);
            reg1.Gender = FormData.Gender.Male;
            reg1.BirthDate = DateTime.Today.AddYears(-22);

            PageObject.Register.AgendaRow row1 = new PageObject.Register.AgendaRow(showToMale);
            PageObject.Register.AgendaRow row2 = new PageObject.Register.AgendaRow(showToFemale);
            PageObject.Register.AgendaRow row3 = new PageObject.Register.AgendaRow(showOver20);
            PageObject.Register.AgendaRow row4 = new PageObject.Register.AgendaRow(showLT20);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            Assert.True(row1.AgendaLabel.IsPresent);
            Assert.False(row2.AgendaLabel.IsPresent);
            Assert.True(row3.AgendaLabel.IsPresent);
            Assert.False(row4.AgendaLabel.IsPresent);

            Registrant reg2 = new Registrant(evt);
            reg2.Gender = FormData.Gender.Female;
            reg2.BirthDate = DateTime.Today.AddYears(-18);

            KeywordProvider.RegistrationCreation.Checkin(reg2);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            Assert.False(row1.AgendaLabel.IsPresent);
            Assert.True(row2.AgendaLabel.IsPresent);
            Assert.False(row3.AgendaLabel.IsPresent);
            Assert.True(row4.AgendaLabel.IsPresent);
        }
    }
}
