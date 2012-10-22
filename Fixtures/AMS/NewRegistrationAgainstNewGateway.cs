namespace RegOnline.RegressionTest.Fixtures.AMS
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

            DataCollection.Registrant reg = new DataCollection.Registrant(evt);
            reg.EventFee_Response = new DataCollection.EventFeeResponse();
            reg.EventFee_Response.Fee = fee;
            reg.Payment_Method = new DataCollection.PaymentMethod(DataCollection.FormData.PaymentMethodEnum.CreditCard);
            reg.Billing_Info = new DataCollection.BillingInfo();
            reg.Billing_Info.SetWithDefaultValue();
            reg.Billing_Info.CC_Number = DataCollection.DefaultPaymentInfo.CCNumber_AMS_Visa.Replace('3', '4');

            Keyword.KeywordProvider.SignIn.SignIn("sprint08", "sprint08", DataCollection.EventFolders.Folders.AMS);
            Keyword.KeywordProvider.Manager_Common.DeleteEvent(eventName);
            Keyword.KeywordProvider.Event_Creator.CreateEvent(evt);
            Keyword.KeywordProvider.Registration_Creation.Checkin(reg);
            Keyword.KeywordProvider.Registration_Creation.PersonalInfo(reg);
            Keyword.KeywordProvider.Registration_Creation.Checkout(reg, true);
            Keyword.KeywordProvider.Register_Common.VerifyErrorMessages(new string[] { DataCollection.Messages.RegisterError.MustEnterValidCC });
            PageObject.PageObjectProvider.Register.RegistationSite.Checkout.BillingInfo_CCNumber.Type(DataCollection.DefaultPaymentInfo.CCNumber_AMS_Visa);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkout.Finish_Click();
            Keyword.KeywordProvider.Registration_Creation.Confirmation(reg);
        }
    }
}
