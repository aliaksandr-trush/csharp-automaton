namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.AgendaPage
{
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
        public void AgendaEmailAddendum()
        {
            Event evt = new Event("AgendaEmailAddendum");
            evt.AgendaPage = new AgendaPage();
            AgendaItemCheckBox agendaWithEmailAddendum = new AgendaItemCheckBox("EmailAddendum");
            agendaWithEmailAddendum.ConfirmationAddendum = "ConfirmationAddendum";
            evt.AgendaPage.AgendaItems.Add(agendaWithEmailAddendum);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant();
            reg.Event = evt;
            AgendaCheckboxResponse resp = new AgendaCheckboxResponse();
            resp.AgendaItem = agendaWithEmailAddendum;
            resp.Checked = true;
            reg.CustomFieldResponses.Add(resp);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);

            //To implement.
            //Check the ConfirmationAddendum is added to confirmation email.
        }
    }
}
