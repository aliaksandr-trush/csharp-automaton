namespace RegOnline.RegressionTest.Fixtures.New.DisplayVerification
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class Display : FixtureBase
    {
        [Test]
        [Category(Priority.Three)]
        [Description("1325")]
        public void VerifyGloableEventDisplay()
        {
            Event Event = new Event("RI-VerifyDisplay");
            Event.StartPage.Location = "Sichuan";
            Event.StartPage.Phone = "8888888888";
            Event.StartPage.Country = "United States";
            Event.StartPage.Address1 = "Fuxing Road No.1";
            Event.StartPage.Address2 = "Huamin Empire Plaza";
            Event.StartPage.City = "Chengdu";
            Event.StartPage.State = "Alaska";
            Event.StartPage.Zip = "12345";
            Event.StartPage.ContactInfo = "Contact us!";
            Event.StartPage.EventHome = "www.google.com";
            Event.StartPage.PageHeader = "This is checkin Page Header.";
            Event.StartPage.PageFooter = "This is checkin Page Footer.";
            Event.PersonalInfoPage.PageHeader = "This is personal info Page Header.";
            Event.PersonalInfoPage.PageFooter = "This is personal info Page Footer.";

            AgendaItem_CheckBox agendaItem = new AgendaItem_CheckBox(DateTime.Now.Ticks.ToString());
            DataCollection.AgendaPage agendaPage = new DataCollection.AgendaPage();
            agendaPage.AgendaItems.Add(agendaItem);
            agendaPage.PageHeader = "This is agenda Page Header.";
            agendaPage.PageFooter = "This is agenda Page Footer.";
            Event.AgendaPage = agendaPage;

            LodgingStandardFields field = new LodgingStandardFields();
            field.Field = DataCollection.EventData_Common.LodgingStandardFields.RoomType;
            field.Visible = true;
            Lodging lodging = new Lodging();
            lodging.StandardFields.Add(field);
            LodgingTravelPage lodgingTravelPage = new LodgingTravelPage();
            lodgingTravelPage.Lodging = lodging;
            lodgingTravelPage.PageHeader = "This is lodging travel Page Header.";
            lodgingTravelPage.PageFooter = "This is lodging travel Page Footer.";
            Event.LodgingTravelPage = lodgingTravelPage;

            MerchandiseItem merchandise = new MerchandiseItem(DateTime.Now.AddSeconds(1).Ticks.ToString());
            merchandise.Type = DataCollection.EventData_Common.MerchandiseType.Header;
            MerchandisePage merchandisePage = new MerchandisePage();
            merchandisePage.Merchandises.Add(merchandise);
            merchandisePage.PageHeader = "This is merchandise Page Header.";
            merchandisePage.PageFooter = "This is merchandise Page Footer.";
            Event.MerchandisePage = merchandisePage;

            Event.CheckoutPage.PageHeader = "This is checkout Page Header.";
            Event.CheckoutPage.PageFooter = "This is checkout Page Footer.";

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, Event, false, false);
            Registrant reg = new Registrant(Event);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            VerifyFooterResults footerResults = KeywordProvider.Verify_Display.VerifyCompanyFooter();
            VerifyEventDetailsResult eventDetailsResults = KeywordProvider.Verify_Display.VerifyEventDetails();
            VerifyPageResults pageResults = KeywordProvider.Verify_Display.VerifyPage();

            Assert.True(footerResults.Trustwave);
            Assert.True(footerResults.Facebook);
            Assert.True(footerResults.Twitter);
            Assert.True(footerResults.Linkedin);
            Assert.True(footerResults.Copyright == "Copyright © 2012 The Active Network, Inc.");
            Assert.True(footerResults.TermsOfUse);
            Assert.True(footerResults.PrivacyPolicy);
            Assert.True(footerResults.CookiePolicy);
            Assert.True(footerResults.About);
            Assert.True(footerResults.ActiveCom);

            Assert.True(eventDetailsResults.EventTitle == Event.Title);
            Assert.True((eventDetailsResults.Location == Event.StartPage.Location) || (eventDetailsResults.Location == null));
            Assert.True((eventDetailsResults.Phone == Event.StartPage.Phone) || (eventDetailsResults.Phone == null));
            Assert.True((eventDetailsResults.Address1 == Event.StartPage.Address1) || (eventDetailsResults.Address1 == null));
            Assert.True((eventDetailsResults.Address2 == Event.StartPage.Address2) || (eventDetailsResults.Address2 == null));
            Assert.True((eventDetailsResults.City == Event.StartPage.City) || (eventDetailsResults.City == null));
            Assert.True((eventDetailsResults.State == Event.StartPage.State) || (eventDetailsResults.State == null));
            Assert.True((eventDetailsResults.Zip == Event.StartPage.Zip) || (eventDetailsResults.Zip == null));
            Assert.True((eventDetailsResults.Country == Event.StartPage.Country) || (eventDetailsResults.Country == null));
            Assert.True(eventDetailsResults.EventContactInfo.Contains(Event.StartPage.ContactInfo) && eventDetailsResults.Contact.Contains(Event.StartPage.ContactInfo));

            Assert.True(pageResults.PageHeader.Contains(Event.StartPage.PageHeader));
            Assert.True(pageResults.PageFooter.Contains(Event.StartPage.PageFooter));

            KeywordProvider.Registration_Creation.Checkin(reg);
            pageResults = KeywordProvider.Verify_Display.VerifyPage();

            Assert.True(pageResults.PageHeader.Contains(Event.PersonalInfoPage.PageHeader));
            Assert.True(pageResults.PageFooter.Contains(Event.PersonalInfoPage.PageFooter));

            KeywordProvider.Registration_Creation.PersonalInfo(reg);
            pageResults = KeywordProvider.Verify_Display.VerifyPage();

            Assert.True(pageResults.PageHeader.Contains(Event.AgendaPage.PageHeader));
            Assert.True(pageResults.PageFooter.Contains(Event.AgendaPage.PageFooter));

            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            pageResults = KeywordProvider.Verify_Display.VerifyPage();

            Assert.True(pageResults.PageHeader.Contains(Event.LodgingTravelPage.PageHeader));
            Assert.True(pageResults.PageFooter.Contains(Event.LodgingTravelPage.PageFooter));

            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            pageResults = KeywordProvider.Verify_Display.VerifyPage();

            Assert.True(pageResults.PageHeader.Contains(Event.MerchandisePage.PageHeader));
            Assert.True(pageResults.PageFooter.Contains(Event.MerchandisePage.PageFooter));

            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            pageResults = KeywordProvider.Verify_Display.VerifyPage();

            Assert.True(pageResults.PageHeader.Contains(Event.CheckoutPage.PageHeader));
            Assert.True(pageResults.PageFooter.Contains(Event.CheckoutPage.PageFooter));
        }
    }
}
