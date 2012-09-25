namespace RegOnline.RegressionTest.Fixtures.New.FeeOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class TaxFixture : FixtureBase
    {
        private Event evt;
        private RegType regType;
        private AgendaItem_CheckBox agenda;
        private MerchandiseItem merch;
        private PaymentMethod paymentMethod;

        [Test]
        [Category(Priority.Two)]
        [Description("523")]
        public void RegWithTaxOne()
        {
            this.GenerateEventForTaxRate(true, false, null);
            Registrant reg = this.GenerateRegForTaxRate(null);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation),
                KeywordProvider.CalculateFee.CalculateTotalFee(reg));
        }

        [Test]
        [Category(Priority.Two)]
        [Description("524")]
        public void RegWithTaxTwo()
        {
            this.GenerateEventForTaxRate(false, true, null);
            Registrant reg = this.GenerateRegForTaxRate(null);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation),
                KeywordProvider.CalculateFee.CalculateTotalFee(reg));
        }

        [Test]
        [Category(Priority.Two)]
        [Description("525")]
        public void RegWithTaxOneandTaxTwo()
        {
            this.GenerateEventForTaxRate(true, true, null);
            Registrant reg = this.GenerateRegForTaxRate(null);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation),
                KeywordProvider.CalculateFee.CalculateTotalFee(reg));
        }

        [Test]
        [Category(Priority.Two)]
        [Description("522")]
        public void RegWithNoTax()
        {
            this.GenerateEventForTaxRate(false, false, null);
            Registrant reg = this.GenerateRegForTaxRate(null);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation),
                KeywordProvider.CalculateFee.CalculateTotalFee(reg));
        }

        [Test]
        [Category(Priority.Two)]
        [Description("526")]
        public void TaxOnlyInOneCountry()
        {
            this.GenerateEventForTaxRate(true, true, FormData.Countries.UnitedStates);
            Registrant reg1 = this.GenerateRegForTaxRate(FormData.Countries.UnitedStates);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation),
                            KeywordProvider.CalculateFee.CalculateTotalFee(reg1)); 
            Registrant reg2 = this.GenerateRegForTaxRate(FormData.Countries.Canada);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation),
                            KeywordProvider.CalculateFee.CalculateTotalFee(reg2));
        }

        [Test]
        [Category(Priority.Two)]
        [Description("527")]
        public void TaxVerifyEUCountry()
        {
            this.GenerateEventForTaxRate(true, true, FormData.Countries.EU);
            Registrant reg1 = this.GenerateRegForTaxRate(FormData.Countries.Austria);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation),
                            KeywordProvider.CalculateFee.CalculateTotalFee(reg1)); 
            Registrant reg2 = this.GenerateRegForTaxRate(FormData.Countries.UnitedStates);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation),
                            KeywordProvider.CalculateFee.CalculateTotalFee(reg2)); 
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
            this.agenda = new AgendaItem_CheckBox("agenda");
            agenda.Price = 60;
            agenda.ApplyTaxOne = applyTaxOne;
            agenda.ApplyTaxTwo = applyTaxTwo;
            evt.AgendaPage = new AgendaPage();
            evt.AgendaPage.AgendaItems.Add(agenda);
            this.merch = new MerchandiseItem("merch");
            merch.Type = FormData.MerchandiseType.Fixed;
            merch.Price = 70;
            merch.ApplyTaxOne = applyTaxOne;
            merch.ApplyTaxTwo = applyTaxTwo;
            evt.MerchandisePage = new MerchandisePage();
            evt.MerchandisePage.Merchandises.Add(merch);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);
        }

        private Registrant GenerateRegForTaxRate(FormData.Countries? country)
        {
            Registrant reg = new Registrant(evt);
            reg.EventFee_Response = new EventFeeResponse(regType);
            reg.EventFee_Response.Fee = regType.Price.Value;
            reg.Payment_Method = paymentMethod;
            if (country.HasValue)
            {
                reg.Country = country.Value;
            }
            AgendaResponse_Checkbox agResp = new AgendaResponse_Checkbox();
            agResp.AgendaItem = agenda;
            agResp.Checked = true;
            agResp.Fee = agenda.Price.Value;
            MerchResponse_FixedPrice merchResp = new MerchResponse_FixedPrice();
            merchResp.Merchandise_Item = merch;
            merchResp.Quantity = 2;
            merchResp.Fee = merch.Price.Value * merchResp.Quantity;
            reg.CustomField_Responses.Add(agResp);
            reg.Merchandise_Responses.Add(merchResp);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);

            return reg;
        }
    }
}
