namespace RegOnline.RegressionTest.Fixtures.Transaction
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Backend;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Managers.Report;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class TransactionTestFixture : FixtureBase
    {
        private int eventId = 0;
        private int regId = 0;
        private string sessionId = string.Empty;
        private string EventName = "TransactionTest";
        private string managerURL = string.Empty;
        private BackendManager.TransactionResponse transaction = new BackendManager.TransactionResponse();
        
        private string regTypeName = "RegType1";
        private string merchandiseItemName = "MerchItem1";
        private string agendaItemName = "AgendaItem1";
        private double regTypeFee = 10;
        private double agendaItemFee = 10;
        private double merchandiseItemFee = 10;
        private double expectedTotalFee = 30.00;
        
        #region Test Methods
        [Test]
        [Category(Priority.Two)]
        [Description("519")]
        public void TransactionTestRegWithCheck()
        {
            //Create Event and Reg With Check
            CreateNewTransactionEvent();
            
            //string totalToVerify = U.MoneyTools.FormatMoney(eventId, expectedTotalFee, CurrentConfig.ClientDbConnection);
            RegForTransactionTest(expectedTotalFee, RegisterManager.PaymentMethod.Check);

            //Transaction Test In Attendee Page
            AttendeePageTransactionTest(RegisterManager.PaymentMethod.Check);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("520")]
        public void TransactionTestRegWithCreditCard()
        {
            //Creat Event and Reg With Credit Card
            CreateNewTransactionEvent();
            
            //string totalToVerify = U.MoneyTools.FormatMoney(eventId, expectedTotalFee, CurrentConfig.ClientDbConnection);
            RegForTransactionTest(expectedTotalFee, RegisterManager.PaymentMethod.CreditCard);

            //Transaction Test In Attendee Page
            AttendeePageTransactionTest(RegisterManager.PaymentMethod.CreditCard);
        }
        #endregion

        #region Create Event Methods
        [Step]
        private void CreateNewTransactionEvent()
        {
            LoginAndGoToEventTab();

            // (always build the event for now)
            ManagerSiteMgr.DeleteEventByName(EventName);

            // get sessionId
            this.sessionId = BuilderMgr.GetEventSessionId();

            // get manager URL
            this.managerURL = ManagerSiteMgr.GetManagerURL(this.sessionId);

            // select "Pro Event" from drop down
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

            // get event id
            eventId = BuilderMgr.GetEventId();

            // test start page
            BuildEventStartPage(EventName, regTypeName, regTypeFee);

            // go to next page
            BuilderMgr.Next();

            // go to next page
            BuilderMgr.Next();

            // test agenda page
            BuildAgendaPage(agendaItemName, agendaItemFee);

            // go to next page
            BuilderMgr.Next();

            // go to next page
            BuilderMgr.Next();

            // test merchandise page
            BuildMerchandisePage(merchandiseItemName, merchandiseItemFee);

            // go to next page
            BuilderMgr.Next();

            // test checkout page
            BuildCheckoutPage();

            // go to next page
            BuilderMgr.Next();
                                
            // save and close
            BuilderMgr.SaveAndClose();
        }

        private void BuildEventStartPage(string eventName, string regtypeName, double eventFee)
        {
            //string fee = U.ConversionTools.ConvertToString(eventFee);

            // verify initial defaults
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);

            //enter event start page info
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventName);

            // save and stay
            BuilderMgr.SaveAndStay();

            //Add registrant type
            BuilderMgr.AddRegTypeWithEventFee(regtypeName, eventFee);
        }

        private void BuildAgendaPage(string agendaItemName, double agendaItemFee)
        {
            // verify splash page
            BuilderMgr.VerifySplashPage();

            // continue to agenda page
            BuilderMgr.ClickYesOnSplashPage();

            // verify agenda page
            BuilderMgr.VerifyEventAgendaPage();

            //add agenda item
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(
                AgendaItemManager.AgendaItemType.CheckBox, 
                agendaItemName, 
                agendaItemFee);
        }

        private void BuildMerchandisePage(string merchName, double merchandiseItemFee)
        {
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Fixed, merchName, merchandiseItemFee, null, null);
        }

        private void BuildCheckoutPage()
        {
            // enter event checkout page info
            BuilderMgr.EnterEventCheckoutPage();

            // save and stay
            BuilderMgr.SaveAndStay();
        }
        #endregion

        #region Registrations
        //Register For Transaction Test
        [Step]
        private void RegForTransactionTest(double totalToVerify, RegisterManager.PaymentMethod method)
        {
            //start new registration
            RegisterMgr.OpenRegisterPage(eventId);

            //check in
            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            //enter profile info
            RegisterMgr.EnterProfileInfo();
            regId = RegisterMgr.GetRegIdFromSession();
            RegisterMgr.CurrentRegistrationId = regId;
            RegisterMgr.Continue();

            //select all agenda items
            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();

            //select merchandise
            RegisterMgr.SelectMerchandise(1);
            RegisterMgr.Continue();

            //TODO: need to abstract calculating the total
            RegisterMgr.PayMoneyAndVerify(totalToVerify, method);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
        #endregion

        #region Transactions
        [Step]
        private void AttendeePageTransactionTest(RegisterManager.PaymentMethod method)
        {
            ManagerSiteMgr.OpenLogin();
            this.sessionId = ManagerSiteMgr.Login();
            this.managerURL = ManagerSiteMgr.GetManagerURL(this.sessionId);
            BackendMgr.OpenAttendeeInfoURL(sessionId, regId);

            if (method == RegisterManager.PaymentMethod.Check)
            {
                EventRegTypeCostTest();
                AgendaCostTest();
                MerchandiseCostTest();
                ManagerSiteMgr.OpenLogin();
                this.sessionId = ManagerSiteMgr.Login();
                this.managerURL = ManagerSiteMgr.GetManagerURL(this.sessionId);
                BackendMgr.OpenAttendeeInfoURL(sessionId, regId);
                TransactionCancelTest();
                ManagerSiteMgr.OpenLogin();
                this.sessionId = ManagerSiteMgr.Login();
                this.managerURL = ManagerSiteMgr.GetManagerURL(this.sessionId);
                BackendMgr.OpenAttendeeInfoURL(sessionId, regId);
                NewTransactionTest();
                VerifyAttendeeReport(EventName,eventId,RegisterManager.PaymentMethod.Check);
            }
            else
            {
                CreditCardTest();
                VerifyAttendeeReport(EventName,eventId,RegisterManager.PaymentMethod.CreditCard);
            }
        }

        private void EventRegTypeCostTest()
        {
            string feeName = regTypeName + "_Event_Fee";
            string totalBeforeTransaction = MoneyTool.FormatMoney(expectedTotalFee);

            //remove event cost
            BackendMgr.SetAndSaveOneEventCostItem(feeName, false);
            VerifyTransaction(totalBeforeTransaction, totalBeforeTransaction, 1);

            //verify amount    
            double total = 0;

            total = expectedTotalFee - regTypeFee;
            VerifyAttendeeInfoTransaction(total, -regTypeFee, 2);

            //add event cost
            BackendMgr.SetAndSaveOneEventCostItem(feeName, true);

            //verify amount
            total = total + regTypeFee;
            VerifyAttendeeInfoTransaction(total, regTypeFee, 3);
        }

        private void AgendaCostTest()
        {
            //remove agenda item
            BackendMgr.SetAndSaveOneAgendaCheckboxItem(agendaItemName,false);
            
            //verify amount
            double total = 0;
            total = expectedTotalFee - agendaItemFee;
            VerifyAttendeeInfoTransaction(total, -agendaItemFee, 4);

            //add agenda item
            BackendMgr.SetAndSaveOneAgendaCheckboxItem(agendaItemName, true);
            
            //verify amount
            total = total + agendaItemFee;
            VerifyAttendeeInfoTransaction(total, agendaItemFee, 5);
        }

        private void MerchandiseCostTest()
        {
            //remove merchandise item
            BackendMgr.SetAndSaveOneMerchandiseItem(merchandiseItemName, null);

            //verify amount
            double total = 0;
            total = expectedTotalFee - merchandiseItemFee;
            VerifyAttendeeInfoTransaction(total, -merchandiseItemFee, 6);

            //add merchandise item
            BackendMgr.SetAndSaveOneMerchandiseItem(merchandiseItemName, 1);

            //verify amount
            total = total + merchandiseItemFee;
            VerifyAttendeeInfoTransaction(total, merchandiseItemFee, 7);
        }

        private void TransactionCancelTest()
        {
            //cancel registration
            BackendMgr.CancelUncancelRegistration(true);

            //verify amount
            double total = 0;
            total = expectedTotalFee - expectedTotalFee;
            VerifyAttendeeInfoTransaction(total, -expectedTotalFee, 8);

            //uncancel registration
            BackendMgr.CancelUncancelRegistration(false);

            //verify amount
            total = total + expectedTotalFee;
            VerifyAttendeeInfoTransaction(total, expectedTotalFee, 9);
        }

        private void NewTransactionTest()
        {
            BackendMgr.OpenAttendeeSubPage(BackendManager.AttendeeSubPage.Transactions);
            
            // New transaction 1
            BackendMgr.OpenNewTransaction();
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.RevenueAdjustments);
            BackendMgr.EnterRevenueAdjustmentsInfo(BackendManager.NewTransactionPayMethod.OtherCharges, 10);
            BackendMgr.SaveAndCloseTransaction();
            //BackendMgr.SelectAttendeeInfoWindow();

            VerifyAttendeeInfoTransaction(40, 10, 10);

            // New transaction 2
            BackendMgr.OpenNewTransaction();
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.RevenueAdjustments);
            BackendMgr.EnterRevenueAdjustmentsInfo(BackendManager.NewTransactionPayMethod.OtherCredits, 10);
            BackendMgr.SaveAndCloseTransaction();
            //BackendMgr.SelectAttendeeInfoWindow();

            VerifyAttendeeInfoTransaction(30, -10, 11);

            // New transaction 3
            BackendMgr.OpenNewTransaction();
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.ManualOfflinePayment);
            BackendMgr.EnterRevenueAdjustmentsInfo(BackendManager.NewTransactionPayMethod.CheckPayment, 10);
            BackendMgr.SaveAndCloseTransaction();
            //BackendMgr.SelectAttendeeInfoWindow();

            VerifyAttendeeInfoTransaction(20, -10, 12);
        }

        private void CreditCardTest()
        {
            // New transaction test
            BackendMgr.OpenNewTransaction();
            BackendMgr.SelectTransactionTypeAndNext(BackendManager.TransactionType.OnlineCCPayment);
            BackendMgr.EnterOnlineCCPaymentInfo(PaymentManager.DefaultPaymentInfo.CCNumber, 10);
            BackendMgr.SaveAndCloseTransaction();
            //BackendMgr.SelectAttendeeInfoWindow();

            VerifyAttendeeInfoTransaction(-10, -10, 3);

            // Void this new transaction
            BackendMgr.VoidTransaction(3);

            VerifyAttendeeInfoTransaction(0, 0, 3);
        }

        private void VerifyAttendeeReport(string eventName,int eventId,RegisterManager.PaymentMethod method)
        {
            OpenTransactionReport(eventName);

            string tableName = "Table1"; //table id

            string thirty = MoneyTool.FormatMoney(30);
            string minusThirty = MoneyTool.FormatMoney(-30);
            string zero = MoneyTool.FormatMoney(0);
            string twenty = MoneyTool.FormatMoney(20);
            string ten = MoneyTool.FormatMoney(10);
            string minusTen = MoneyTool.FormatMoney(-10);

            if (method == RegisterManager.PaymentMethod.Check)
            {
                ReportMgr.VerifyTableReport(tableName, 1, 6, thirty);

                ReportMgr.VerifyTableReport(tableName, 1, 7, thirty);

                ReportMgr.VerifyTableReport(tableName, 2, 6, minusTen);
                ReportMgr.VerifyTableReport(tableName, 2, 7, twenty);

                ReportMgr.VerifyTableReport(tableName, 3, 6, ten);
                ReportMgr.VerifyTableReport(tableName, 3, 7, thirty);

                ReportMgr.VerifyTableReport(tableName, 4, 6, minusTen);
                ReportMgr.VerifyTableReport(tableName, 4, 7, twenty);

                ReportMgr.VerifyTableReport(tableName, 5, 6, ten);
                ReportMgr.VerifyTableReport(tableName, 5, 7, thirty);

                ReportMgr.VerifyTableReport(tableName, 6, 6, minusTen);
                ReportMgr.VerifyTableReport(tableName, 6, 7, twenty);

                ReportMgr.VerifyTableReport(tableName, 7, 6, ten);
                ReportMgr.VerifyTableReport(tableName, 7, 7, thirty);

                ReportMgr.VerifyTableReport(tableName, 8, 6, minusThirty);
                ReportMgr.VerifyTableReport(tableName, 8, 7, zero);

                ReportMgr.VerifyTableReport(tableName, 9, 6, thirty);
                ReportMgr.VerifyTableReport(tableName, 9, 7, thirty);

                ReportMgr.VerifyTableReport(tableName, 10, 6, ten);
                ReportMgr.VerifyTableReport(tableName, 10, 7, MoneyTool.FormatMoney(40));

                ReportMgr.VerifyTableReport(tableName, 11, 6, minusTen);
                ReportMgr.VerifyTableReport(tableName, 11, 7, thirty);

                ReportMgr.VerifyTableReport(tableName, 12, 6, minusTen);
                ReportMgr.VerifyTableReport(tableName, 12, 7, twenty);
            }
            else
            {
                ReportMgr.VerifyTableReport(tableName, 1, 6, thirty);
                ReportMgr.VerifyTableReport(tableName, 1, 7, thirty);

                ReportMgr.VerifyTableReport(tableName, 2, 6, minusThirty);
                ReportMgr.VerifyTableReport(tableName, 2, 7, zero);

                ReportMgr.VerifyTableReport(tableName, 3, 6, zero);
                ReportMgr.VerifyTableReport(tableName, 3, 7, zero);
            }
        }

        private void VerifyTransaction(string amount,string subTotal,int order)
        {
            BackendMgr.SelectAttendeeInfoWindow();
            BackendMgr.OpenAttendeeSubPage(BackendManager.AttendeeSubPage.Transactions);
            transaction = new BackendManager.TransactionResponse();
            transaction.Amount = amount;
            transaction.SubTotal = subTotal;
            BackendMgr.VerifyTransactionsByOrder(transaction, order);
        }

        private void VerifyAttendeeInfoTransaction(double total, double fee, int order)
        {
            string expectAmount = MoneyTool.FormatMoney(fee);
            string expectTotal = MoneyTool.FormatMoney(total);

            VerifyTransaction(expectAmount, expectTotal, order);
            BackendMgr.VerifyTotalTransactions(expectTotal);
        }

        private void OpenTransactionReport(string eventName)
        {
            UIUtil.DefaultProvider.OpenUrl(managerURL);
            ManagerSiteMgr.OpenEventDashboard(eventName);
            ManagerSiteMgr.DashboardMgr.ChooseTabAndVerify(DashboardManager.DashboardTab.Reports);
            ////M3.Dashboard.ClickOption(DashboardManager.ReportsFinancialReport.Transactions);
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(ReportManager.CommonReportType.Transaction);
        }

        private void LoginAndGoToEventTab()
        {
            ManagerSiteMgr.OpenLogin();

            // login to regression testing account
            ManagerSiteMgr.Login();

            // make sure we're on the Events tab
            ManagerSiteMgr.GoToEventsTabIfNeeded();

            // click "Regression" folder
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount(); 
        }
        #endregion
    }
}