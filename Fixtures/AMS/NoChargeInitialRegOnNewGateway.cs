namespace RegOnline.RegressionTest.Fixtures.AMS
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    public class NoChargeInitialRegOnNewGateway : FixtureBase
    {
        [Test]
        public void InvalidCCReg()
        {
            string eventName = "AMS_USD_InvalidCCReg";
            double fee = 100.0;
            DataCollection.Event evt = new DataCollection.Event(eventName);
            evt.StartPage.Event_Fee = new DataCollection.EventFee();
            evt.StartPage.Event_Fee.StandardPrice = fee;
            evt.CheckoutPage.ApplySettings_AMS_USD();
            evt.CheckoutPage.CC_Options.ChargeInitialReg = true;
            evt.IsActive = true;

            Keyword.KeywordProvider.SignIn.SignIn("sprint08", "sprint08", DataCollection.EventFolders.Folders.AMS);
            Keyword.KeywordProvider.Manager_Common.DeleteEvent(eventName);
            Keyword.KeywordProvider.Event_Creator.CreateEvent(evt);

            DataCollection.Registrant reg1 = new DataCollection.Registrant(evt);
            reg1.EventFee_Response = new DataCollection.EventFeeResponse();
            reg1.EventFee_Response.Fee = fee;
            reg1.Payment_Method = new DataCollection.PaymentMethod(DataCollection.EventData_Common.PaymentMethodEnum.CreditCard);
            reg1.Billing_Info = new DataCollection.BillingInfo();
            reg1.Billing_Info.SetWithDefaultValue();
            reg1.Billing_Info.CC_Number = DataCollection.DefaultPaymentInfo.CCNumber_AMS_Visa.Replace('3', '4');

            Keyword.KeywordProvider.Registration_Creation.Checkin(reg1);
            Keyword.KeywordProvider.Registration_Creation.PersonalInfo(reg1);
            Keyword.KeywordProvider.Registration_Creation.Checkout(reg1, true);
            Keyword.KeywordProvider.Register_Common.VerifyErrorMessages(new string[] { DataCollection.Messages.RegisterError.MustEnterValidCC });
            PageObject.PageObjectProvider.Register.RegistationSite.Checkout.BillingInfo_CCNumber.Type(DataCollection.DefaultPaymentInfo.CCNumber_AMS_Visa);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkout.Finish_Click();
            Keyword.KeywordProvider.Registration_Creation.Confirmation(reg1);

            DataCollection.Registrant reg2 = new DataCollection.Registrant(evt);
            reg2.EventFee_Response = new DataCollection.EventFeeResponse();
            reg2.EventFee_Response.Fee = fee;
            reg2.Payment_Method = new DataCollection.PaymentMethod(DataCollection.EventData_Common.PaymentMethodEnum.CreditCard);
            reg2.Billing_Info = new DataCollection.BillingInfo();
            reg2.Billing_Info.SetWithDefaultValue();
            reg2.Billing_Info.ExpirationDate = new DateTime(2012, 1, 1);

            Keyword.KeywordProvider.Registration_Creation.Checkin(reg2);
            Keyword.KeywordProvider.Registration_Creation.PersonalInfo(reg2);
            Keyword.KeywordProvider.Registration_Creation.Checkout(reg2, true);
            Assert.True(Keyword.KeywordProvider.Register_Common.HasErrorMessage(DataCollection.Messages.RegisterError.MustEnterValidDate));
            PageObject.PageObjectProvider.Register.RegistationSite.Checkout.BillingInfo_ExpirationDate_Month.SelectWithValue(
                DataCollection.DefaultPaymentInfo.ExpirationDate_Month.ToString());
            PageObject.PageObjectProvider.Register.RegistationSite.Checkout.BillingInfo_ExpirationDate_Year.SelectWithValue(
                DataCollection.DefaultPaymentInfo.ExpirationDate_Year.ToString());
            PageObject.PageObjectProvider.Register.RegistationSite.Checkout.Finish_Click();
            Keyword.KeywordProvider.Registration_Creation.Confirmation(reg2);

            PageObject.PageObjectProvider.Backend.AttendeeInfo.OpenUrl(reg1.Id);
            PageObject.PageObjectProvider.Backend.AttendeeInfo.ChargeRemainingBalance_Click();
            PageObject.PageObjectProvider.Backend.Charge.SelectByName();
            Assert.AreEqual(PageObject.PageObjectProvider.Backend.Charge.ExistingCC(0).Text.Substring(12, 4),
                DataCollection.DefaultPaymentInfo.CCNumber_AMS_Visa.Substring(12, 4));
            PageObject.PageObjectProvider.Backend.Charge.SaveAndClose_Click();

            PageObject.PageObjectHelper.SelectTopWindow();
            Assert.AreEqual(Utilities.MoneyTool.RemoveMoneyCurrencyCode(
                PageObject.PageObjectProvider.Backend.AttendeeInfo.TransactionTotal.Text.Trim()), 0);
            PageObject.Backend.AttendeeInfoTransactionRow latestRow = new PageObject.Backend.AttendeeInfoTransactionRow();
        }
    }
}
