namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.FeeOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Keyword;

    public class DiscountCodeFixture : FixtureBase
    {
        [Test]
        [Category(Priority.Two)]
        [Description("692")]
        public void RegTypeDiscount()
        {
            Event evt = new Event("RegTypeDiscount");
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            RegType regType1 = new RegType("RegType1");
            regType1.Price = 80;
            RegType regType2 = new RegType("RegType2");
            regType2.Price = 90;
            DiscountCode dc1 = new DiscountCode("dc1");
            dc1.Amount = 10;
            dc1.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc1.CodeKind = FormData.ChangeType.Percent;
            dc1.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc2 = new DiscountCode("dc2");
            dc2.Amount = 20;
            dc2.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc2.CodeKind = FormData.ChangeType.Percent;
            dc2.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc3 = new DiscountCode("dc3");
            dc3.Amount = 30;
            dc3.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc3.CodeKind = FormData.ChangeType.Percent;
            dc3.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc4 = new DiscountCode("dc4");
            dc4.Amount = 40;
            dc4.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc4.CodeKind = FormData.ChangeType.Percent;
            dc4.CodeType = FormData.DiscountCodeType.DiscountCode;
            regType1.DiscountCode.Add(dc1);
            regType1.DiscountCode.Add(dc2);
            regType2.DiscountCode.Add(dc3);
            regType2.DiscountCode.Add(dc4);
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg1 = new Registrant(evt);
            reg1.Payment_Method = paymentMethod;
            reg1.RegType_Response = new RegTypeResponse(regType1);
            reg1.RegType_Response.DiscountCode = dc1;
            System.Threading.Thread.Sleep(10);
            Registrant reg2 = new Registrant(evt);
            reg2.RegType_Response = new RegTypeResponse(regType2);
            reg2.RegType_Response.DiscountCode = dc3;
            Group group1 = new Group();
            group1.Primary = reg1;
            group1.Secondaries.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(group1);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 135);

            Registrant reg3 = new Registrant(evt);
            reg3.Payment_Method = paymentMethod;
            reg3.RegType_Response = new RegTypeResponse(regType1);
            reg3.RegType_Response.DiscountCode = dc2;
            System.Threading.Thread.Sleep(10);
            Registrant reg4 = new Registrant(evt);
            reg4.RegType_Response = new RegTypeResponse(regType2);
            reg4.RegType_Response.DiscountCode = dc4;
            Group group2 = new Group();
            group2.Primary = reg3;
            group2.Secondaries.Add(reg4);

            KeywordProvider.RegistrationCreation.GroupRegistration(group2);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 118);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("539")]
        public void GroupRegWithHalfDiscount()
        {
            Event evt = new Event("GroupRegWithHalfDiscount");
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            RegType regType1 = new RegType("RegType1");
            regType1.Price = 50;
            RegType regType2 = new RegType("RegType2");
            regType2.Price = 60;
            DiscountCode dc1 = new DiscountCode("dc1");
            dc1.Amount = 50;
            dc1.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc1.CodeKind = FormData.ChangeType.Percent;
            dc1.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc2 = new DiscountCode("dc2");
            dc2.Amount = 50;
            dc2.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc2.CodeKind = FormData.ChangeType.Percent;
            dc2.CodeType = FormData.DiscountCodeType.DiscountCode;
            regType1.DiscountCode.Add(dc1);
            regType2.DiscountCode.Add(dc2);
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);
            AgendaItemCheckBox agenda1 = new AgendaItemCheckBox("Agenda1");
            agenda1.Price = 80;
            AgendaItemCheckBox agenda2 = new AgendaItemCheckBox("Agenda2");
            agenda2.Price = 100;
            DiscountCode dc3 = new DiscountCode("dc3");
            dc3.Amount = 50;
            dc3.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc3.CodeKind = FormData.ChangeType.Percent;
            dc3.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc4 = new DiscountCode("dc4");
            dc4.Amount = 50;
            dc4.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc4.CodeKind = FormData.ChangeType.Percent;
            dc4.CodeType = FormData.DiscountCodeType.DiscountCode;
            agenda1.DiscountCode.Add(dc3);
            agenda2.DiscountCode.Add(dc4);
            evt.AgendaPage = new AgendaPage();
            evt.AgendaPage.AgendaItems.Add(agenda1);
            evt.AgendaPage.AgendaItems.Add(agenda2);
            Merchandise merch1 = new Merchandise("Merch1");
            merch1.MerchandiseType = FormData.MerchandiseType.Fixed;
            merch1.MerchandiseFee = 110;
            Merchandise merch2 = new Merchandise("Merch2");
            merch2.MerchandiseType = FormData.MerchandiseType.Fixed;
            merch2.MerchandiseFee = 150;
            DiscountCode dc5 = new DiscountCode("dc5");
            dc5.Amount = 50;
            dc5.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc5.CodeKind = FormData.ChangeType.Percent;
            dc5.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc6 = new DiscountCode("dc6");
            dc6.Amount = 50;
            dc6.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc6.CodeKind = FormData.ChangeType.Percent;
            dc6.CodeType = FormData.DiscountCodeType.DiscountCode;
            merch1.DiscountCodes.Add(dc5);
            merch2.DiscountCodes.Add(dc6);
            evt.MerchandisePage = new MerchandisePage();
            evt.MerchandisePage.Merchandises.Add(merch1);
            evt.MerchandisePage.Merchandises.Add(merch2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            RegTypeResponse resp1 = new RegTypeResponse(regType1);
            resp1.DiscountCode = dc1;
            RegTypeResponse resp2 = new RegTypeResponse(regType2);
            resp2.DiscountCode = dc2;
            AgendaCheckboxResponse resp3 = new AgendaCheckboxResponse();
            resp3.AgendaItem = agenda1;
            resp3.Checked = true;
            resp3.Code = dc3;
            AgendaCheckboxResponse resp4 = new AgendaCheckboxResponse();
            resp4.AgendaItem = agenda2;
            resp4.Checked = true;
            resp4.Code = dc4;
            MerchFixedResponse resp5 = new MerchFixedResponse();
            resp5.Merchandise = merch1;
            resp5.Quantity = 1;
            resp5.Discount_Code = dc5;
            MerchFixedResponse resp6 = new MerchFixedResponse();
            resp6.Merchandise = merch2;
            resp6.Quantity = 1;
            resp6.Discount_Code = dc6;

            Registrant reg1 = new Registrant(evt);
            reg1.Payment_Method = paymentMethod;
            reg1.RegType_Response = resp1;
            reg1.CustomField_Responses.Add(resp3);
            reg1.CustomField_Responses.Add(resp4);
            reg1.Merchandise_Responses.Add(resp5);
            reg1.Merchandise_Responses.Add(resp6);
            System.Threading.Thread.Sleep(10);
            Registrant reg2 = new Registrant(evt);
            reg2.RegType_Response = resp2;
            reg2.CustomField_Responses.Add(resp3);
            reg2.CustomField_Responses.Add(resp4);
            System.Threading.Thread.Sleep(10);
            Registrant reg3 = new Registrant(evt);
            reg3.RegType_Response = resp1;
            reg3.CustomField_Responses.Add(resp3);
            Group group = new Group();
            group.Primary = reg1;
            group.Secondaries.Add(reg2);
            group.Secondaries.Add(reg3);

            KeywordProvider.RegistrationCreation.GroupRegistration(group);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 615);
        }
    }
}
