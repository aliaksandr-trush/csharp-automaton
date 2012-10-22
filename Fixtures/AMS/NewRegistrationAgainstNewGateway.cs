﻿namespace RegOnline.RegressionTest.Fixtures.AMS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.Fixtures.Base;
    using NUnit.Framework;

    [TestFixture]
    public class NewRegistrationAgainstNewGateway : FixtureBase
    {
        [Test]
        public void ValidCCReg()
        {
            string eventName = "AMS_USD_ValidCCReg";
            double fee = 100.0;
            DataCollection.Event evt = new DataCollection.Event(eventName);
            evt.StartPage.Event_Fee = new DataCollection.EventFee();
            evt.StartPage.Event_Fee.StandardPrice = fee;
            evt.CheckoutPage.ApplySettings_AMS_USD();
            evt.IsActive = true;

            DataCollection.Registrant reg = new DataCollection.Registrant(evt);
            reg.EventFee_Response = new DataCollection.EventFeeResponse();
            reg.EventFee_Response.Fee = fee;
            reg.Payment_Method = new DataCollection.PaymentMethod(DataCollection.FormData.PaymentMethodEnum.CreditCard);
            reg.Billing_Info = new DataCollection.BillingInfo();
            reg.Billing_Info.SetWithDefaultValue();

            Keyword.KeywordProvider.SignIn.SignIn("sprint08", "sprint08", DataCollection.EventFolders.Folders.AMS);
            Keyword.KeywordProvider.Manager_Common.DeleteEvent(eventName);
            Keyword.KeywordProvider.Event_Creator.CreateEvent(evt);
            Keyword.KeywordProvider.Registration_Creation.CreateRegistration(reg);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.IsOnPage(DataCollection.FormData.RegisterPage.Confirmation));
        }

        [Test]
        public void InvalidCCReg()
        {
            string eventName = "AMS_USD_ValidCCReg";
            double fee = 100.0;
            DataCollection.Event evt = new DataCollection.Event(eventName);
            evt.StartPage.Event_Fee = new DataCollection.EventFee();
            evt.StartPage.Event_Fee.StandardPrice = fee;
            evt.CheckoutPage.ApplySettings_AMS_USD();
            evt.IsActive = true;

            DataCollection.Registrant reg1 = new DataCollection.Registrant(evt);
            reg1.EventFee_Response = new DataCollection.EventFeeResponse();
            reg1.EventFee_Response.Fee = fee;
            reg1.Payment_Method = new DataCollection.PaymentMethod(DataCollection.FormData.PaymentMethodEnum.CreditCard);
            reg1.Billing_Info = new DataCollection.BillingInfo();
            reg1.Billing_Info.SetWithDefaultValue();
            reg1.Billing_Info.CC_Number = "";

            Keyword.KeywordProvider.SignIn.SignIn("sprint08", "sprint08", DataCollection.EventFolders.Folders.AMS);
            Keyword.KeywordProvider.Manager_Common.DeleteEvent(eventName);
            Keyword.KeywordProvider.Event_Creator.CreateEvent(evt);
            Keyword.KeywordProvider.Registration_Creation.Checkin(reg1);
            Keyword.KeywordProvider.Registration_Creation.PersonalInfo(reg1);
            Keyword.KeywordProvider.Registration_Creation.Checkout(reg1);

            DataCollection.Registrant reg2 = new DataCollection.Registrant(evt);
            reg2.EventFee_Response = new DataCollection.EventFeeResponse();
            reg2.EventFee_Response.Fee = fee;
            reg2.Payment_Method = new DataCollection.PaymentMethod(DataCollection.FormData.PaymentMethodEnum.CreditCard);
            reg2.Billing_Info = new DataCollection.BillingInfo();
            reg2.Billing_Info.SetWithDefaultValue();
            reg2.Billing_Info.ExpirationDate = DateTime.Today.AddDays(-3);

            Keyword.KeywordProvider.Registration_Creation.Checkin(reg2);
            Keyword.KeywordProvider.Registration_Creation.PersonalInfo(reg2);
            Keyword.KeywordProvider.Registration_Creation.Checkout(reg2);
        }
    }
}
