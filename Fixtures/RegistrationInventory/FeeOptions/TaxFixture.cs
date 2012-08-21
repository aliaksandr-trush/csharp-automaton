namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.FeeOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    public class TaxFixture : FixtureBase
    {
        private Event evt;
        private RegType regType;
        private AgendaItemCheckBox agenda;
        private Merchandise merch;
        private PaymentMethod paymentMethod;

        [Test]
        [Category(Priority.Two)]
        [Description("523")]
        public void RegWithTaxOne()
        {
            this.GenerateEventForTaxRate(true, false, null);
            this.GenerateRegForTaxRate(null);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == 375);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("524")]
        public void RegWithTaxTwo()
        {
            this.GenerateEventForTaxRate(false, true, null);
            this.GenerateRegForTaxRate(null);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == 275);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("525")]
        public void RegWithTaxOneandTaxTwo()
        {
            this.GenerateEventForTaxRate(true, true, null);
            this.GenerateRegForTaxRate(null);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == 400);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("522")]
        public void RegWithNoTax()
        {
            this.GenerateEventForTaxRate(false, false, null);
            this.GenerateRegForTaxRate(null);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == 250);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("526")]
        public void TaxOnlyInOneCountry()
        {
            this.GenerateEventForTaxRate(true, true, FormData.Countries.UnitedStates);
            this.GenerateRegForTaxRate(FormData.Countries.UnitedStates);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == 400);
            this.GenerateRegForTaxRate(FormData.Countries.Canada);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == 250);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("527")]
        public void TaxVerifyEUCountry()
        {
            this.GenerateEventForTaxRate(true, true, FormData.Countries.EU);
            this.GenerateRegForTaxRate(FormData.Countries.Austria);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == 400);
            this.GenerateRegForTaxRate(FormData.Countries.UnitedStates);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == 250);
        }

        private void GenerateEventForTaxRate(bool applyTaxOne, bool applyTaxTwo, FormData.Countries? country)
        {
            this.evt = new Event("TaxFixture");
            this.regType = new RegType("regType");
            this.paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            TaxRate tax1 = new TaxRate("tax1");
            if (country.HasValue)
            {
                tax1.Country = country.Value;
            }
            tax1.Rate = 50;
            TaxRate tax2 = new TaxRate("tax2");
            tax2.Rate = 10;
            regType.Price = 50;
            regType.ApplyTaxOne = applyTaxOne;
            regType.ApplyTaxTwo = applyTaxTwo;
            evt.TaxRateOne = tax1;
            evt.TaxRateTwo = tax2;
            evt.StartPage.RegTypes.Add(regType);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            this.agenda = new AgendaItemCheckBox("agenda");
            agenda.Price = 60;
            agenda.ApplyTaxOne = applyTaxOne;
            agenda.ApplyTaxTwo = applyTaxTwo;
            evt.AgendaPage = new AgendaPage();
            evt.AgendaPage.AgendaItems.Add(agenda);
            this.merch = new Merchandise();
            merch.MerchandiseName = "merch";
            merch.MerchandiseType = FormData.MerchandiseType.Fixed;
            merch.MerchandiseFee = 70;
            merch.ApplyTaxOne = applyTaxOne;
            merch.ApplyTaxTwo = applyTaxTwo;
            evt.MerchandisePage = new MerchandisePage();
            evt.MerchandisePage.Merchandises.Add(merch);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);
        }

        private void GenerateRegForTaxRate(FormData.Countries? country)
        {
            Registrant reg = new Registrant(evt);
            reg.RegType_Response = new RegTypeResponse(regType);
            reg.Payment_Method = paymentMethod;
            if (country.HasValue)
            {
                reg.Country = country.Value;
            }
            AgendaCheckboxResponse agResp = new AgendaCheckboxResponse();
            agResp.AgendaItem = agenda;
            agResp.Checked = true;
            MerchFixedResponse merchResp = new MerchFixedResponse();
            merchResp.Merchandise = merch;
            merchResp.Quantity = 2;
            reg.CustomField_Responses.Add(agResp);
            reg.Merchandise_Responses.Add(merchResp);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);
        }
    }
}
