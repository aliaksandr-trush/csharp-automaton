namespace RegOnline.RegressionTest.Fixtures.New.FeeOptions
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
            paymentMethod = new PaymentMethod(FormData.PaymentMethodEnum.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            regType = new RegType("RegType");
            regType.Price = 50;
            CustomFieldCode ac1 = new CustomFieldCode("ac1");
            ac1.CodeType = FormData.CustomFieldCodeType.AccessCode;
            ac1.Limit = 2;
            regType.AllCustomCodes.Add(ac1);
            CustomFieldCode ac2 = new CustomFieldCode("ac2");
            ac2.CodeType = FormData.CustomFieldCodeType.AccessCode;
            regType.AllCustomCodes.Add(ac2);
            evt.StartPage.RegTypes.Add(regType);

            agenda = new AgendaItem_CheckBox("Agenda");
            agenda.Price = 80;
            CustomFieldCode ac3 = new CustomFieldCode("ac3");
            ac3.CodeType = FormData.CustomFieldCodeType.AccessCode;
            ac3.Limit = 2;
            CustomFieldCode ac4 = new CustomFieldCode("ac4");
            ac4.CodeType = FormData.CustomFieldCodeType.AccessCode;
            agenda.DiscountCodes.Add(ac3);
            agenda.DiscountCodes.Add(ac4);
            agenda.BulkCodes = CustomFieldCode.GenerateBulkCodes(agenda.DiscountCodes);
            evt.AgendaPage = new AgendaPage();
            evt.AgendaPage.AgendaItems.Add(agenda);

            merch = new MerchandiseItem("Merch");
            merch.Type = FormData.MerchandiseType.Fixed;
            merch.Price = 110;
            CustomFieldCode ac5 = new CustomFieldCode("ac5");
            ac5.CodeType = FormData.CustomFieldCodeType.AccessCode;
            merch.DiscountCodes.Add(ac5);
            evt.MerchandisePage = new MerchandisePage();
            evt.MerchandisePage.Merchandises.Add(merch);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, true);

            Registrant reg1 = this.GenerateReg(ac1, ac3, ac5);
            KeywordProvider.Registration_Creation.CreateRegistration(reg1);
            Assert.True(KeywordProvider.Register_Common.GetTotal(FormData.RegisterPage.Confirmation) == 240);
            Registrant reg2 = this.GenerateReg(ac1, ac3, ac5);
            KeywordProvider.Registration_Creation.CreateRegistration(reg2);
            Assert.True(KeywordProvider.Register_Common.GetTotal(FormData.RegisterPage.Confirmation) == 240);
            Registrant reg3 = this.GenerateReg(ac1, ac4, ac5);
            KeywordProvider.Registration_Creation.Checkin(reg3);
            Assert.True(KeywordProvider.Register_Common.HasErrorMessage(string.Format(Messages.RegisterError.RegTypeCodeLimitHasReached, ac1.CodeString)));
            Registrant reg4 = this.GenerateReg(ac2, ac3, ac5);
            KeywordProvider.Registration_Creation.Checkin(reg4);
            KeywordProvider.Registration_Creation.PersonalInfo(reg4);
            KeywordProvider.Registration_Creation.Agenda(reg4);
            Assert.True(KeywordProvider.Register_Common.HasErrorMessage(Messages.RegisterError.AgendaCodeLimitReached));
            Registrant reg5 = this.GenerateReg(ac2, ac4, ac5);
            KeywordProvider.Registration_Creation.CreateRegistration(reg5);
            Assert.True(KeywordProvider.Register_Common.GetTotal(FormData.RegisterPage.Confirmation) == 240);
        }

        private Registrant GenerateReg(CustomFieldCode regTypeDc, CustomFieldCode agendaDc, CustomFieldCode merchDc)
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
