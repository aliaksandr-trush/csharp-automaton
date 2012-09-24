namespace RegOnline.RegressionTest.Fixtures.Transaction
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Backend;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class TransactionsAttendeeInfoFixture : FixtureBase
    {
        private const string EventName = "TransactionsAttendeeInfoFixture";
        private const double EventFee = 100;

        private int eventId;
        private string eventSessionId;
        private int registrationId;

        [Test]
        [Category(Priority.One)]
        [Description("451")]
        public void ChargeRefundRemainingBalance()
        {
            double otherChargeAmount = 55.00;
            double otherCreditAmount = 37.75;

            // Step #1
            this.SetupEventAndRegistrationToTest();

            // Step #2
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);

            // Step #3
            BackendMgr.OpenNewTransaction();

            // Step #4
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.RevenueAdjustments);

            // Step #5
            BackendMgr.EnterRevenueAdjustmentsInfo(
                BackendManager.NewTransactionPayMethod.OtherCharges,
                otherChargeAmount);

            BackendMgr.SaveAndCloseTransaction();
            UIUtil.DefaultProvider.SelectOriginalWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
            System.Threading.Thread.Sleep(700);
            BackendMgr.VerifyTotalBalanceDue(otherChargeAmount);

            // Step #6, #7
            BackendMgr.ChargeRemainingBalance(otherChargeAmount);
            BackendMgr.SelectAttendeeInfoWindow();
            UIUtil.DefaultProvider.SelectOriginalWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
            System.Threading.Thread.Sleep(700);
            BackendMgr.VerifyTotalBalanceDue(0);
            System.Threading.Thread.Sleep(700);

            // Step #8, #9, verify transaction details
            BackendMgr.OpenTransactionDetails(null);

            BackendMgr.VerifyTransactionDetails(
                BackendManager.TransactionTypeString.OnlineCCPayment,
                otherChargeAmount,
                PaymentManager.CCType.Visa,
                PaymentManager.DefaultPaymentInfo.CCNumber,
                PaymentManager.DefaultPaymentInfo.HolderName,
                this.eventId);

            BackendMgr.CloseTransactionDetails();
            BackendMgr.SelectAttendeeInfoWindow();

            // Step #10
            BackendMgr.OpenNewTransaction();

            // Step #11
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.RevenueAdjustments);

            // Step #12
            BackendMgr.EnterRevenueAdjustmentsInfo(
                BackendManager.NewTransactionPayMethod.OtherCredits,
                otherCreditAmount);

            BackendMgr.SaveAndCloseTransaction();
            UIUtil.DefaultProvider.SelectOriginalWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
            System.Threading.Thread.Sleep(700);
            BackendMgr.VerifyTotalBalanceDue(0 - otherCreditAmount);

            // Step #13, #14, #15
            BackendMgr.ChangeTransactionDateToSettle(this.registrationId);
            BackendMgr.RefundCreditDue(otherCreditAmount);
            BackendMgr.SelectAttendeeInfoWindow();
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);
            BackendMgr.VerifyTotalBalanceDue(0);

            // Step #16
            BackendMgr.OpenTransactionDetails(null);

            BackendMgr.VerifyTransactionDetails(
                BackendManager.TransactionTypeString.OnlineCCRefund,
                otherCreditAmount,
                PaymentManager.CCType.Visa,
                PaymentManager.DefaultPaymentInfo.CCNumber,
                PaymentManager.DefaultPaymentInfo.HolderName,
                this.eventId);

            // Step #17
            BackendMgr.CloseTransactionDetails();
            BackendMgr.SelectAttendeeInfoWindow();
        }

        [Test]
        [Category(Priority.One)]
        [Description("453")]
        public void RefundTransaction()
        {
            double ccCharge = 50;
            double totalBalanceDue = 0;
            double transactionAmount = 0;

            // Step #1
            this.SetupEventAndRegistrationToTest();

            // Step #2
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);

            // Step #3
            BackendMgr.OpenNewTransaction();

            // Step #4
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.OnlineCCPayment);

            // Step #5
            transactionAmount = ccCharge;
            BackendMgr.EnterOnlineCCPaymentInfo(PaymentManager.DefaultPaymentInfo.CCNumberAlternative, transactionAmount);
            BackendMgr.SaveAndCloseTransaction();
            System.Threading.Thread.Sleep(5000);

            BackendMgr.VerifyTransactionHistory(
                null,
                BackendManager.TransactionTypeString.OnlineCCPayment,
                PaymentManager.DefaultPaymentInfo.CCNumberAlternative,
                0 - transactionAmount,
                this.eventId);

            // Should be: -$50.00
            totalBalanceDue += 0 - ccCharge;

            BackendMgr.VerifyTotalBalanceDue(totalBalanceDue);
            BackendMgr.ChangeTransactionDateToSettle(this.registrationId);

            // Step #6
            BackendMgr.OpenNewTransaction();

            // Step #7
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.OnlineCCRefund);

            BackendMgr.VerifyCCNumberSelection(0, true);

            BackendMgr.VerifyCCNumberAndMaxRefundable(
                PaymentManager.DefaultPaymentInfo.CCNumber,
                EventFee,
                this.eventId);

            BackendMgr.VerifyCCNumberAndMaxRefundable(
                PaymentManager.DefaultPaymentInfo.CCNumberAlternative,
                ccCharge,
                this.eventId);

            BackendMgr.VerifyCCAmount(EventFee);
            
            BackendMgr.VerifyTransactionDefaultCCInfo(this.eventId);

            // Step #8
            // This step is invalid: the amount will not change and this is WAD
            ////BackendMgr.EnterCCAmount(EventFee + ccCharge);
            ////BackendMgr.EnterDefaultHolderName();
            ////System.Threading.Thread.Sleep(1000);
            ////BackendMgr.VerifyCCAmount(EventFee);

            // Step #9
            double firstRefundAmount = 5;
            transactionAmount = firstRefundAmount;
            BackendMgr.EnterCCAmount(transactionAmount);
            //BackendMgr.ConfirmCCRefund();
            //AttendeeInfo.ConfirmCCRefund();
            //UIUtilityProvider.UIHelper.Click("Id=actionYes");
            BackendMgr.SaveAndCloseTransaction();
            //BackendMgr.SelectAttendeeInfoWindow();
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
            System.Threading.Thread.Sleep(5000);

            BackendMgr.VerifyTransactionHistory(
                null,
                BackendManager.TransactionTypeString.OnlineCCRefund,
                PaymentManager.DefaultPaymentInfo.CCNumber,
                transactionAmount,
                this.eventId);

            // Should be: -$45.00
            totalBalanceDue += firstRefundAmount;

            BackendMgr.VerifyTotalBalanceDue(totalBalanceDue);

            // Step #10
            BackendMgr.OpenNewTransaction();

            // Step #11
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.OnlineCCRefund);

            BackendMgr.VerifyCCNumberSelection(0, true);

            BackendMgr.VerifyCCNumberAndMaxRefundable(
                PaymentManager.DefaultPaymentInfo.CCNumber,
                EventFee - firstRefundAmount,
                this.eventId);

            BackendMgr.VerifyCCNumberAndMaxRefundable(
                PaymentManager.DefaultPaymentInfo.CCNumberAlternative,
                ccCharge,
                this.eventId);

            // Step #12
            BackendMgr.SelectCCNumber(1);
            //System.Threading.Thread.Sleep(1000);
            //BackendMgr.VerifyCCAmount(ccCharge);

            // Step #13
            // I have to ignore this step with the same reason as step #8

            // Step #14
            transactionAmount = ccCharge / 2;
            BackendMgr.EnterCCAmount(transactionAmount);
            //BackendMgr.ConfirmCCRefund();
            BackendMgr.SaveAndCloseTransaction();
            //BackendMgr.SelectAttendeeInfoWindow();
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
            System.Threading.Thread.Sleep(5000);

            BackendMgr.VerifyTransactionHistory(
                null,
                BackendManager.TransactionTypeString.OnlineCCRefund,
                PaymentManager.DefaultPaymentInfo.CCNumberAlternative,
                transactionAmount,
                this.eventId);

            // Should be: -$20.00
            totalBalanceDue += ccCharge / 2;

            BackendMgr.VerifyTotalBalanceDue(totalBalanceDue);

            // Step #15
            BackendMgr.OpenNewTransaction();

            // Step #16
            BackendMgr.SelectTransactionTypeAndNext(
                BackendManager.TransactionType.ManualOfflineRefund);

            // Step #17, note that total balance due is a minus number now, so charge for (0 - totalBalanceDue)
            transactionAmount = 0 - totalBalanceDue;

            BackendMgr.EnterRevenueAdjustmentsInfo(
                BackendManager.NewTransactionPayMethod.OfflineCCRefund,
                transactionAmount);

            BackendMgr.SaveAndCloseTransaction();
            //BackendMgr.SelectAttendeeInfoWindow();
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
            System.Threading.Thread.Sleep(5000);

            BackendMgr.VerifyTransactionHistory(
                null,
                BackendManager.TransactionTypeString.OfflineCCRefund,
                null,
                transactionAmount,
                this.eventId);

            BackendMgr.VerifyTotalBalanceDue(0);
        }

        [Test]
        [Category(Priority.One)]
        [Description("456")]
        public void PerformAllActionsInTransactionTypeMenu()
        {
            double firstCCCharge = 25;
            double secondCCCharge = 21;
            double offlinePayment = 14;
            double transactionAmount = 0;
            double totalBalanceDue = 0;

            // Step #1
            this.SetupEventAndRegistrationToTest();

            // Step #2
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);

            // Step #3
            BackendMgr.OpenNewTransaction();

            // Step #4
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.OnlineCCPayment);

            // Step #5
            transactionAmount = firstCCCharge;
            BackendMgr.EnterOnlineCCPaymentInfo(null, transactionAmount);
            BackendMgr.SaveAndCloseTransaction();
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.VerifyTransactionHistory(
                null,
                BackendManager.TransactionTypeString.OnlineCCPayment,
                PaymentManager.DefaultPaymentInfo.CCNumber,
                0 - transactionAmount,
                this.eventId);

            // Should be: -$25.00
            totalBalanceDue += 0 - firstCCCharge;

            BackendMgr.VerifyTotalBalanceDue(totalBalanceDue);

            // Step #6
            BackendMgr.OpenNewTransaction();

            // Step #7
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.OnlineCCPayment);

            // Step #8
            transactionAmount = secondCCCharge;
            BackendMgr.EnterOnlineCCPaymentInfo(PaymentManager.DefaultPaymentInfo.CCNumberAlternative, transactionAmount);
            BackendMgr.SaveAndCloseTransaction();
            BackendMgr.SelectAttendeeInfoWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
            System.Threading.Thread.Sleep(2000);

            BackendMgr.VerifyTransactionHistory(
                null,
                BackendManager.TransactionTypeString.OnlineCCPayment,
                PaymentManager.DefaultPaymentInfo.CCNumberAlternative,
                0 - transactionAmount,
                this.eventId);

            // Should be: -$46.00
            totalBalanceDue += 0 - secondCCCharge;

            BackendMgr.VerifyTotalBalanceDue(totalBalanceDue);

            // Step #9
            BackendMgr.OpenNewTransaction();

            // Step #10
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.ManualOfflinePayment);

            // Step #11
            transactionAmount = offlinePayment;

            BackendMgr.EnterRevenueAdjustmentsInfo(
                BackendManager.NewTransactionPayMethod.OtherPayments,
                transactionAmount);

            BackendMgr.SaveAndCloseTransaction();
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.VerifyTransactionHistory(
                null,
                BackendManager.TransactionTypeString.OtherPayments,
                null,
                0 - transactionAmount,
                this.eventId);

            // Should be: -$60.00
            totalBalanceDue += 0 - offlinePayment;

            BackendMgr.VerifyTotalBalanceDue(totalBalanceDue);

            // Step #12
            BackendMgr.OpenNewTransaction();

            // Step #13
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.RevenueAdjustments);

            // Step #14
            transactionAmount = 0 - totalBalanceDue;

            BackendMgr.EnterRevenueAdjustmentsInfo(
                BackendManager.NewTransactionPayMethod.OtherCharges,
                transactionAmount);

            BackendMgr.SaveAndCloseTransaction();
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.VerifyTransactionHistory(
                null,
                BackendManager.TransactionTypeString.OtherCharges,
                null,
                transactionAmount,
                this.eventId);

            // Should be: $0.00
            totalBalanceDue = 0;

            BackendMgr.VerifyTotalBalanceDue(totalBalanceDue);
        }

        [Step]
        public void SetupEventAndRegistrationToTest()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            this.SetEventStartPage();
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndClose();

            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.PayMoney(RegisterManager.PaymentMethod.CreditCard);
            RegisterMgr.FinishRegistration();

            this.registrationId = Convert.ToInt32(RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(
                RegisterManager.ConfirmationPageField.RegistrationId));

            RegisterMgr.ConfirmRegistration();
        }

        public void SetEventStartPage()
        {
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName);
            BuilderMgr.SetEventFee(EventFee);
            BuilderMgr.SaveAndStay();
        }

        public void SetEventCheckoutPage()
        {
            BuilderMgr.PaymentMethodMgr.AddPaymentMethod(PaymentMethodManager.PaymentMethod.Check);
            BuilderMgr.PaymentMethodMgr.ClickCreditCardLink();
            
            // Use First Data Global gateway so that we don't have to wait another day
            // till the transactions to be 'settled' to refund
            BuilderMgr.PaymentMethodMgr.CreditCardOptionsMgr.SelectPaymentGateway(
                CreditCardOptionsManager.PaymentGateway.FirstDataGlobalCustomer);
            
            BuilderMgr.PaymentMethodMgr.CreditCardOptionsMgr.SaveAndClose();
            BuilderMgr.SaveAndStay();
        }
    }
}
