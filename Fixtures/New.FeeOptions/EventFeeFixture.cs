﻿namespace RegOnline.RegressionTest.Fixtures.New.FeeOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Keyword;
    using RegOnline.RegressionTest.PageObject;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class EventFeeFixture : FixtureBase
    {
        private Event evt;
        private PaymentMethod paymentMethod;

        [Test]
        [Category(Priority.Two)]
        [Description("426")]
        public void TestRegTypeWithDiscountCodeAccessCode()
        {
            evt = new Event("TestRegTypeWithDiscountCodeAccessCode");
            paymentMethod = new PaymentMethod(DataCollection.EventData_Common.PaymentMethodEnum.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            //regtype1 no price
            RegType regType1 = new RegType("RegType1");
            //regtype2 has price, no code
            RegType regType2 = new RegType("RegType2");
            regType2.Price = 10;
            //regtype2 has price, has code
            RegType regType3 = new RegType("RegType3");
            regType3.Price = 100;
            CustomFieldCode dc1 = new CustomFieldCode("dc1");
            dc1.Amount = 10;
            dc1.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            dc1.CodeKind = DataCollection.EventData_Common.ChangeType.FixedAmount;
            dc1.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            CustomFieldCode ac1 = new CustomFieldCode("ac1");
            ac1.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.AccessCode;
            regType3.AllCustomCodes.Add(dc1);
            regType3.AllCustomCodes.Add(ac1);
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);
            evt.StartPage.RegTypes.Add(regType3);
            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);
            Registrant reg1 = new Registrant(evt);
            reg1.Payment_Method = paymentMethod;
            reg1.EventFee_Response = new EventFeeResponse(regType1);
            KeywordProvider.Registration_Creation.CreateRegistration(reg1);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.Total.IsPresent);
            Registrant reg2 = new Registrant(evt);
            reg2.Payment_Method = paymentMethod;
            reg2.EventFee_Response = new EventFeeResponse(regType2);
            KeywordProvider.Registration_Creation.CreateRegistration(reg2);
            Assert.True(KeywordProvider.Register_Common.GetTotal(DataCollection.EventData_Common.RegisterPage.Confirmation) == 10);
            Registrant reg3 = new Registrant(evt);
            reg3.Payment_Method = paymentMethod;
            reg3.EventFee_Response = new EventFeeResponse(regType3);
            reg3.EventFee_Response.Code = dc1;
            KeywordProvider.Registration_Creation.CreateRegistration(reg3);
            Assert.True(KeywordProvider.Register_Common.GetTotal(DataCollection.EventData_Common.RegisterPage.Confirmation) == 90);
            Registrant reg4 = new Registrant(evt);
            reg4.Payment_Method = paymentMethod;
            reg4.EventFee_Response = new EventFeeResponse(regType3);
            reg4.EventFee_Response.Code = ac1;
            KeywordProvider.Registration_Creation.CreateRegistration(reg4);
            Assert.True(KeywordProvider.Register_Common.GetTotal(DataCollection.EventData_Common.RegisterPage.Confirmation) == 100);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("425")]
        public void TestEventFeeWithDiscountCodeAccessCode()
        {
            evt = new Event("TestEventFeeWithDiscountCodeAccessCode");
            paymentMethod = new PaymentMethod(DataCollection.EventData_Common.PaymentMethodEnum.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            EventFee ef = new EventFee();
            ef.StandardPrice = 100;
            CustomFieldCode dc1 = new CustomFieldCode("dc1");
            dc1.Amount = 10;
            dc1.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            dc1.CodeKind = DataCollection.EventData_Common.ChangeType.FixedAmount;
            dc1.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            CustomFieldCode ac1 = new CustomFieldCode("ac1");
            ac1.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.AccessCode;
            ef.AllCustomCodes.Add(dc1);
            ef.AllCustomCodes.Add(ac1);
            evt.StartPage.Event_Fee = ef;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);
            Registrant reg1 = new Registrant(evt);
            reg1.Payment_Method = paymentMethod;
            reg1.EventFee_Response = new EventFeeResponse();
            reg1.EventFee_Response.Code = dc1;
            KeywordProvider.Registration_Creation.CreateRegistration(reg1);
            Assert.True(KeywordProvider.Register_Common.GetTotal(DataCollection.EventData_Common.RegisterPage.Confirmation) == 90);
            Registrant reg2 = new Registrant(evt);
            reg2.Payment_Method = paymentMethod;
            reg2.EventFee_Response = new EventFeeResponse();
            reg2.EventFee_Response.Code = ac1;
            KeywordProvider.Registration_Creation.CreateRegistration(reg2);
            Assert.True(KeywordProvider.Register_Common.GetTotal(DataCollection.EventData_Common.RegisterPage.Confirmation) == 100);
        }
    }
}
