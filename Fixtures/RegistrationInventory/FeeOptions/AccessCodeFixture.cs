namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.FeeOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AccessCodeFixture : FixtureBase
    {
        private Event evt;
        private PaymentMethod paymentMethod;
        private RegType regType;
        private AgendaItem_CheckBox agenda;
        private MerchandiseItem merch;

        [Test]
        [Category(Priority.Three)]
        [Description("1372")]
        public void TestAccessCode()
        {
            evt = new Event("AccessCodeFixture");
            paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            regType = new RegType("RegType");
            regType.Price = 50;
            DiscountCode ac1 = new DiscountCode("ac1");
            ac1.CodeType = FormData.DiscountCodeType.AccessCode;
            ac1.Limit = 2;
            regType.DiscountCode.Add(ac1);
            DiscountCode ac2 = new DiscountCode("ac2");
            ac2.CodeType = FormData.DiscountCodeType.AccessCode;
            regType.DiscountCode.Add(ac2);
            evt.StartPage.RegTypes.Add(regType);

            agenda = new AgendaItem_CheckBox("Agenda");
            agenda.Price = 80;
            DiscountCode ac3 = new DiscountCode("ac3");
            ac3.CodeType = FormData.DiscountCodeType.AccessCode;
            ac3.Limit = 2;
            DiscountCode ac4 = new DiscountCode("ac4");
            ac4.CodeType = FormData.DiscountCodeType.AccessCode;
            agenda.DiscountCodes.Add(ac3);
            agenda.DiscountCodes.Add(ac4);
            agenda.BulkCodes = DiscountCode.GenerateBulkCodes(agenda.DiscountCodes);
            evt.AgendaPage = new AgendaPage();
            evt.AgendaPage.AgendaItems.Add(agenda);

            merch = new MerchandiseItem("Merch");
            merch.Type = FormData.MerchandiseType.Fixed;
            merch.Price = 110;
            DiscountCode ac5 = new DiscountCode("ac5");
            ac5.CodeType = FormData.DiscountCodeType.AccessCode;
            merch.DiscountCodes.Add(ac5);
            evt.MerchandisePage = new MerchandisePage();
            evt.MerchandisePage.Merchandises.Add(merch);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, true);

            Registrant reg1 = this.GenerateReg(ac1, ac3, ac5);
            KeywordProvider.RegistrationCreation.CreateRegistration(reg1);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 240);
            Registrant reg2 = this.GenerateReg(ac1, ac3, ac5);
            KeywordProvider.RegistrationCreation.CreateRegistration(reg2);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 240);
            Registrant reg3 = this.GenerateReg(ac1, ac4, ac5);
            KeywordProvider.RegistrationCreation.Checkin(reg3);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(string.Format(Messages.RegisterError.RegTypeCodeLimitHasReached, ac1.Code)));
            Registrant reg4 = this.GenerateReg(ac2, ac3, ac5);
            KeywordProvider.RegistrationCreation.Checkin(reg4);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg4);
            KeywordProvider.RegistrationCreation.Agenda(reg4);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.AgendaCodeLimitReached));
            Registrant reg5 = this.GenerateReg(ac2, ac4, ac5);
            KeywordProvider.RegistrationCreation.CreateRegistration(reg5);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 240);
        }

        private Registrant GenerateReg(DiscountCode regTypeDc, DiscountCode agendaDc, DiscountCode merchDc)
        {
            Registrant reg = new Registrant(evt);
            reg.Payment_Method = paymentMethod;
            reg.EventFee_Response = new EventFeeResponse(regType);
            reg.EventFee_Response.Code = regTypeDc;
            AgendaResponse_Checkbox resp1 = new AgendaResponse_Checkbox();
            resp1.AgendaItem = agenda;
            resp1.Checked = true;
            resp1.Code = agendaDc;
            reg.CustomField_Responses.Add(resp1);
            MerchResponse_FixedPrice resp2 = new MerchResponse_FixedPrice();
            resp2.Merchandise_Item = merch;
            resp2.Quantity = 1;
            resp2.Discount_Code = merchDc;
            reg.Merchandise_Responses.Add(resp2);

            return reg;
        }
    }
}
