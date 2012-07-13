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
            AG1Choice2.Select = true;
            AG1.ChoiceItems.Add(AG1Choice1);
            AG1.ChoiceItems.Add(AG1Choice2);
            AG2.CommonlyUsedItems.Add(FormData.CommonlyUsedMultipleChoice.Agreement);
            AG3.CommonlyUsedItems.Add(FormData.CommonlyUsedMultipleChoice.YesOrNo);
            ChoiceItem AG4Choice1 = new ChoiceItem("AG4Choice1");
            AG4Choice1.GroupLimit = 2;
            AG4Choice1.Select = true;
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

            Registrant reg = new Registrant();
            reg.Event = evt;
            reg.AgendaItems.Add(AG1);
            reg.AgendaItems.Add(AG2);
            reg.AgendaItems.Add(AG3);
            reg.AgendaItems.Add(AG4);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);
        }
    }
}
