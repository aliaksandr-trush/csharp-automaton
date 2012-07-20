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

            KeywordProvider.AddAgendaItem.AddAgendaItems(AG2);
            AG2.Id = Convert.ToInt32(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaItemId.Value);
            KeywordProvider.AddAgendaItem.AddAgendaItems(AG3);
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
            KeywordProvider.AddAgendaItem.AddAgendaItems(AG4);
            AG4.Id = Convert.ToInt32(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaItemId.Value);

            Registrant reg1 = new Registrant();
            reg1.Event = evt;
            CFCheckboxResponse resp = new CFCheckboxResponse();
            resp.CustomField = PICustomField;
            resp.Checked = true;
            reg1.CustomFieldResponses.Add(resp);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG1).AgendaType.IsPresent);
            ((CheckBox)(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG3).AgendaType)).Set(true);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG4).AgendaType.IsPresent);

            Registrant reg2 = new Registrant();
            reg2.Event = evt;

            KeywordProvider.RegistrationCreation.Checkin(reg2);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(AG1).AgendaType.IsPresent);
        }
    }
}
