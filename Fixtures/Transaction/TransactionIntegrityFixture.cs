namespace RegOnline.RegressionTest.Fixtures.Transaction.TransactionIntegrity
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Managers.Report;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class TransactionIntegrityFixture : FixtureBase
    {
        #region locators/helpers
        public enum EventType
        {
            ProEvent,
            ProEventUpdate,
            Membership,
            MembershipRenew,
            Endurance_USD,
            Endurance_Pound
        }

        public enum RegisterType
        {
            ProEvent_Single,
            ProEvent_Group,
            ProEvent_Simple,
            ProEvent_Update,
            Membership,
            EnduranceEvent_Single_USD,
            EnduranceEvent_Single_Pound
        }

        private int regId;
        private int eventId;
        private string eventSessionId;
        private string emailAddress;
        #endregion

        #region test methods

        /// <summary>
        /// Registers single person and verifies transactional data in all reports and the database. 
        /// The event is transactionally complicated, including discounts, many fee types, etc.
        /// Eventually once PT figures it out, we will verify the correct amount in PT via their API.
        /// </summary>
        [Test]
        [Category(Priority.One)]
        [Description("739")]
        public void ProEvent_SingleReg()
        {
            this.LoginAndGoToRegressionFolder();
            this.eventId = ManagerSiteMgr.GetFirstEventId(ProEvent.EventName);

            this.OpenEventDashboardAndCleanupTestRegAndReturnToManagerEventList();

            this.SingleRegisterForTransactionIntegrityEvent();

            this.LoginAndOpenEventDashboardAndSwitchToReportTab();

            this.VerifyReportData(EventType.ProEvent, ProEvent.FeeCalculation_SingleReg.Default);

            ////VerifyChargeInPT(MerchandId, "620451-11364828");

            ////VerifyTransactionDataInDB(
            ////    TxnIntegrityConstants.SingleRegTransactionAmount,
            ////    TxnIntegrityConstants.SingleRegBillableAmount,
            ////    TxnIntegrityConstants.AddAndModByAttendee,
            ////    TxnIntegrityConstants.SharedFeePercent, EventType.Event);

            this.ChooseEventsTabOnEventDashboardAndCleanupTestRegAndReturnToManagerEventList();
        }

        /// <summary>
        /// Same as above except a group registration.
        /// </summary>
        [Test]
        [Category(Priority.One)]
        [Description("740")]
        public void ProEvent_GroupReg()
        {
            this.LoginAndGoToRegressionFolder();
            this.eventId = ManagerSiteMgr.GetFirstEventId(ProEvent.EventName);

            this.OpenEventDashboardAndCleanupTestRegAndReturnToManagerEventList();

            this.GroupRegisterForTransactionIntegrityEvent();

            this.LoginAndOpenEventDashboardAndSwitchToReportTab();

            this.VerifyReportData(EventType.ProEvent, ProEvent.FeeCalculation_GroupReg.Default);

            ////VerifyTransactionDataInDB(
            ////    TxnIntegrityConstants.GroupRegTransactionAmount,
            ////    TxnIntegrityConstants.GroupRegBillableAmount,
            ////    TxnIntegrityConstants.AddAndModByAttendee,
            ////    TxnIntegrityConstants.SharedFeePercent, EventType.ProEvent);

            this.ChooseEventsTabOnEventDashboardAndCleanupTestRegAndReturnToManagerEventList();
        }

        /// <summary>
        /// Single registration and then charged on the backend, and verifies all reports and data in db.
        /// </summary>
        [Test]
        [Category(Priority.One)]
        [Description("741")]
        public void ProEvent_Backend()
        {
            this.LoginAndGoToRegressionFolder();
            this.eventId = ManagerSiteMgr.GetFirstEventId(ProEvent.EventName);

            this.OpenEventDashboardAndCleanupTestRegAndReturnToManagerEventList();

            this.RegisterNoAgendaItemsMerch(RegisterManager.PaymentMethod.Check);

            LoginAndGoToRegressionFolder();
            BackendMgr.OpenAttendeeInfoURL(eventSessionId, regId);
            BackendMgr.OpenPaymentMethod();
            Assert.True(UIUtilityProvider.UIHelper.IsElementPresent("//select[@id='creditCardType'][@disabled]", LocateBy.XPath));

            ////this.PerformBackendCharge();

            ////this.LoginAndOpenEventDashboardAndSwitchToReportTab();

            ////this.VerifyReportData(EventType.ProEvent, ProEvent.FeeCalculation_SimpleReg.Default);

            ////VerifyTransactionDataInDB(TxnIntegrityConstants.BackEndTransactionAmount,
            ////    TxnIntegrityConstants.BackEndBillableAmount,
            ////    TxnIntegrityConstants.AddAndModByRegression,
            ////    TxnIntegrityConstants.SharedFeePercent, EventType.ProEvent);

            ////this.ChooseEventsTabOnEventDashboardAndCleanupTestRegAndReturnToManagerEventList();
        }

        /// <summary>
        /// Udpate a registration, then verify charges on the backend/reports/data in db
        /// </summary>
        [Test]
        [Category(Priority.One)]
        [Description("744")]
        public void ProEvent_UpdateReg()
        {
            this.LoginAndGoToRegressionFolder();
            this.eventId = ManagerSiteMgr.GetFirstEventId(ProEvent.EventName);

            this.OpenEventDashboardAndCleanupTestRegAndReturnToManagerEventList();

            this.RegisterNoAgendaItemsMerch(RegisterManager.PaymentMethod.CreditCard);

            this.LoginAndGoToRegressionFolder();
            ManagerSiteMgr.OpenEventDashboard(this.eventId);
            ManagerSiteMgr.DashboardMgr.ChooseTabAndVerify(DashboardManager.DashboardTab.Reports);

            this.VerifyReportData(EventType.ProEvent, ProEvent.FeeCalculation_SimpleReg.Default);

            ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ReturnToList();

            ////VerifyTransactionDataInDB(
            ////    TxnIntegrityConstants.BackEndTransactionAmount,
            ////    TxnIntegrityConstants.BackEndBillableAmount,
            ////    TxnIntegrityConstants.AddAndModByAttendee,
            ////    TxnIntegrityConstants.SharedFeePercent, EventType.ProEvent);

            this.UpdateRegistration(this.emailAddress);

            this.LoginAndOpenEventDashboardAndSwitchToReportTab();

            this.VerifyReportData(EventType.ProEventUpdate, ProEvent.FeeCalculation_UpdateReg.Default);

            ////VerifyTransactionDataInDB(
            ////    TxnIntegrityConstants.UpdateTransactionAmounts,
            ////    TxnIntegrityConstants.UpdateBillableAmount,
            ////    TxnIntegrityConstants.AddAndModByAttendee2,
            ////    TxnIntegrityConstants.SharedFeePercent, EventType.Update);

            this.ChooseEventsTabOnEventDashboardAndCleanupTestRegAndReturnToManagerEventList();
        }

        /// <summary>
        /// Registers for a membership, verify charges and fees, then renews and re-checks everything
        /// </summary>
        [Test]
        [Category(Priority.One)]
        [Description("746")]
        public void Membership_SingleRegAndRenew()
        {
            this.LoginAndGoToRegressionFolder();
            this.eventId = ManagerSiteMgr.GetFirstEventId(Membership.EventName);

            this.OpenEventDashboardAndCleanupTestRegAndReturnToManagerEventList();

            this.RegisterForMembership();

            this.LoginAndGoToRegressionFolder();
            ManagerSiteMgr.OpenEventDashboard(this.eventId);
            ManagerSiteMgr.DashboardMgr.ChooseTabAndVerify(DashboardManager.DashboardTab.Reports);

            this.VerifyReportData(EventType.Membership, Membership.FeeCalculation_InitialReg.Default);

            ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ReturnToList();

            ////VerifyTransactionDataInDB(
            ////    TxnIntegrityConstants.MembershipTransactionAmounts,
            ////    TxnIntegrityConstants.MembershipBillableAmount,
            ////    TxnIntegrityConstants.AddAndModByMember,
            ////    TxnIntegrityConstants.NoSharedFeePercent,
            ////    EventType.Membership);

            this.RenewMembership();

            this.LoginAndOpenEventDashboardAndSwitchToReportTab();

            this.VerifyReportData(EventType.MembershipRenew, Membership.FeeCalculation_Renew.Default);

            ////VerifyTransactionDataInDB(
            ////    TxnIntegrityConstants.RenewedMemberTransactionAmount,
            ////    TxnIntegrityConstants.RenewedMemberBillableAmount,
            ////    TxnIntegrityConstants.AddAndModByMember2,
            ////    TxnIntegrityConstants.NoSharedFeePercent, EventType.Membership);

            this.ChooseEventsTabOnEventDashboardAndCleanupTestRegAndReturnToManagerEventList();
        }

        /// <summary>
        /// Registers for an active event with merch non merch fees and verifies txns and fees. 
        /// </summary>
        [Test]
        [Category(Priority.One)]
        [Description("752")]
        public void EnduranceEvent_SingleReg_USD()
        {
            ConfigurationProvider.XmlConfig.ReloadAccount(
                XmlConfiguration.AccountType.ActiveEurope);

            this.LoginAndGoToRegressionFolder();
            this.eventId = ManagerSiteMgr.GetFirstEventId(EnduranceEvent_USD.EventName);

            this.OpenEventDashboardAndCleanupTestRegAndReturnToManagerEventList();

            this.RegisterForEnduranceEvent_USD(RegisterType.EnduranceEvent_Single_USD);

            this.LoginAndOpenEventDashboardAndSwitchToReportTab();

            this.VerifyReportData(EventType.Endurance_USD, EnduranceEvent_USD.FeeCalculation_SingleReg.Default);

            ////VerifyTransactionDataInDB(
            ////    TxnIntegrityConstants.MerchNonMerchTransactionAmount,
            ////    TxnIntegrityConstants.MerchNonMerchBillableAmount,
            ////    TxnIntegrityConstants.AddAndModByParticpant,
            ////    TxnIntegrityConstants.SharedFeePercent, EventType.Endurance);

            this.ChooseEventsTabOnEventDashboardAndCleanupTestRegAndReturnToManagerEventList();
        }

        /// <summary>
        /// Verifies Registration and transaction amounts/fees for an event with a different currency than the gateway
        /// Note:The transaction fees and transaction report are commented out pending the resolution of BUG 22600
        /// </summary>
        [Test]
        [Category(Priority.One)]
        [Description("761")]
        public void EnduranceEvent_SingleReg_Pound()
        {
            ConfigurationProvider.XmlConfig.ReloadAccount(
                XmlConfiguration.AccountType.ActiveEurope);

            this.LoginAndGoToRegressionFolder();
            this.eventId = ManagerSiteMgr.GetFirstEventId(EnduranceEvent_Pound.EventName);

            this.OpenEventDashboardAndCleanupTestRegAndReturnToManagerEventList();

            this.RegisterForEnduranceEvent_Pound(RegisterType.EnduranceEvent_Single_Pound);

            this.LoginAndOpenEventDashboardAndSwitchToReportTab();

            this.VerifyReportData(EventType.Endurance_Pound, EnduranceEvent_Pound.FeeCalculation_SingleReg.Default);

            ////VerifyTransactionDataInDB(
            ////    TxnIntegrityConstants.DiffCurrenciesTransactionAmount,
            ////    TxnIntegrityConstants.MerchNonMerchBillableAmount,
            ////    TxnIntegrityConstants.AddAndModByParticpant,
            ////    TxnIntegrityConstants.SharedFeePercent, EventType.Endurance);

            this.ChooseEventsTabOnEventDashboardAndCleanupTestRegAndReturnToManagerEventList();
        }

        /// <summary>
        /// Runs BuildInvoiceItems and verifies InvoiceItems_AdHoc currently
        /// </summary>
        ////[Test]
        ////[Category(Priority.One)]
        ////[Description("745")]
        public void zzRunBillingAndVerification()
        {
            Billing();
            FinalizeBilling();

            ConfigurationProvider.XmlConfig.ReloadAccount(XmlConfiguration.AccountType.ActiveEurope);
            LoginAndGoToRegressionFolder();
            eventId = ManagerSiteMgr.GetFirstEventId(TxnIntegrityConstants.ActiveEventName);
            VerifyBilling(EventType.Endurance_USD);
            VerifyFinalizeBilling(EventType.Endurance_USD);
            LoginAndGoToRegressionFolder();
            eventId = ManagerSiteMgr.GetFirstEventId(TxnIntegrityConstants.DifferentCurrencies);
            VerifyBilling(EventType.Endurance_Pound);
            VerifyFinalizeBilling(EventType.Endurance_Pound);
            ConfigurationProvider.XmlConfig.ReloadAccount(XmlConfiguration.AccountType.Default);
            LoginAndGoToRegressionFolder();
            eventId = ManagerSiteMgr.GetFirstEventId(TxnIntegrityConstants.EventName);
            VerifyBilling(EventType.ProEvent);
            VerifyFinalizeBilling(EventType.ProEvent);
            LoginAndGoToRegressionFolder();
            eventId = ManagerSiteMgr.GetFirstEventId(TxnIntegrityConstants.MembershipName);
            VerifyBilling(EventType.Membership);
            VerifyFinalizeBilling(EventType.Membership);
        }

        /// <summary>
        /// This needs to be run before the whole suite is run in order for the billing verification to work. 
        /// It deletes all registrations in the events. 
        /// If one test fails and then the billing verification fails, then this needs to be run and the whole suite started over. 
        /// If only one test fails, but the registation was complete, and therefor zzRunBillingAndVerification passed, you do not need to run this again.
        /// </summary>
        ////[Test]
        public void AlwaysRunThisTestFirstInTheTxnIntegritySuite()
        {
            ////ChangeTodaysRegistrationsToTestAndDelete(TxnIntegrityConstants.ActiveEventName, XmlConfiguration.Environment.Beta, XmlConfiguration.PrivateLabel.ActiveEurope);
            ////ChangeTodaysRegistrationsToTestAndDelete(TxnIntegrityConstants.DifferentCurrencies, XmlConfiguration.Environment.Beta, XmlConfiguration.PrivateLabel.ActiveEurope);
            ////ChangeTodaysRegistrationsToTestAndDelete(TxnIntegrityConstants.EventName, XmlConfiguration.Environment.Beta, XmlConfiguration.PrivateLabel.RegOnline);
            ////ChangeTodaysRegistrationsToTestAndDelete(TxnIntegrityConstants.MembershipName, XmlConfiguration.Environment.Beta, XmlConfiguration.PrivateLabel.RegOnline);
        }

        #endregion

        #region helper methods

        [Step]
        private void LoginAndGoToRegressionFolder()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GotoTab(ManagerSiteManager.Tab.Events);
            ManagerSiteMgr.SelectFolder();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
        }

        private void OpenEventDashboardAndCleanupTestRegAndReturnToManagerEventList()
        {
            ManagerSiteMgr.OpenEventDashboard(this.eventId);
            this.ChooseEventsTabOnEventDashboardAndCleanupTestRegAndReturnToManagerEventList();
        }

        private void ChooseEventsTabOnEventDashboardAndCleanupTestRegAndReturnToManagerEventList()
        {
            ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);
            this.ChangeRegistrationsToTestStatusAndDelete();
            ManagerSiteMgr.DashboardMgr.ReturnToList();
        }

        private void LoginAndOpenEventDashboardAndSwitchToReportTab()
        {
            this.LoginAndGoToRegressionFolder();
            ManagerSiteMgr.OpenEventDashboard(this.eventId);
            ManagerSiteMgr.DashboardMgr.ChooseTabAndVerify(DashboardManager.DashboardTab.Reports);
        }

        #region Registration Methods

        // Registers single person for transaction integrity event
        [Step]
        private void SingleRegisterForTransactionIntegrityEvent()
        {
            this.CheckinPage();
            this.PersonalInfoPage();
            this.AgendaPage(EventType.ProEvent);
            this.LodgingAndTravelPage();
            RegisterMgr.Continue();
            this.MerchandisePage();
            this.CheckoutPage(RegisterType.ProEvent_Single, ProEvent.FeeCalculation_SingleReg.Default, RegisterManager.PaymentMethod.CreditCard);
            this.ConfirmationPage(RegisterType.ProEvent_Single, ProEvent.FeeCalculation_SingleReg.Default);
            this.regId = RegisterMgr.GetRegID();
        }

        // Group registrations for transaction integrity event
        [Step]
        private void GroupRegisterForTransactionIntegrityEvent()
        {
            string primaryEmail = string.Empty;
            this.CheckinPage();
            primaryEmail = RegisterMgr.CurrentEmail;
            this.PersonalInfoPage();
            this.AgendaPage(EventType.ProEvent);
            this.LodgingAndTravelPage();
            RegisterMgr.ClickAddAnotherPerson();
            this.SecondReg();
            RegisterMgr.ClickAddAnotherPerson();
            this.ThirdReg();
            RegisterMgr.Continue();
            this.MerchandisePage();
            RegisterMgr.CurrentEmail = primaryEmail;
            this.CheckoutPage(RegisterType.ProEvent_Group, ProEvent.FeeCalculation_GroupReg.Default, RegisterManager.PaymentMethod.CreditCard);
            this.ConfirmationPage(RegisterType.ProEvent_Group, ProEvent.FeeCalculation_GroupReg.Default);
            this.regId = RegisterMgr.GetRegID();
        }

        // second reg for group is sharing for L&T page
        private void SecondReg()
        {
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(ProEvent.RegType.Name.One);
            RegisterMgr.EnterEventDiscoutCode(ProEvent.DiscountCode);
            RegisterMgr.Continue();
            this.PersonalInfoPage();
            this.AgendaPage(EventType.ProEvent);
        }

        // Thrid and final group reg, has seperate Lodging from primary
        private void ThirdReg()
        {
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(ProEvent.RegType.Name.One);
            RegisterMgr.EnterEventDiscoutCode(ProEvent.DiscountCode);
            RegisterMgr.Continue();
            this.PersonalInfoPage();
            this.AgendaPage(EventType.ProEvent);
            this.LodgingAndTravelPageNoCheckInOutDates();
        }

        // Simple registration for backend charge
        [Step]
        private void RegisterNoAgendaItemsMerch(RegisterManager.PaymentMethod paymentMethod)
        {
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(ProEvent.RegType.Name.One);
            RegisterMgr.Continue();
            this.PersonalInfoPage();
            this.emailAddress = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            this.CheckoutPage(RegisterType.ProEvent_Simple, ProEvent.FeeCalculation_SimpleReg.Default, paymentMethod);
            this.ConfirmationPage(RegisterType.ProEvent_Simple, ProEvent.FeeCalculation_SimpleReg.Default);
            this.regId = RegisterMgr.GetRegID();
        }

        // Updates a basic registrations
        [Step]
        private void UpdateRegistration(string emailAddress)
        {
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin(emailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterPassword();
            RegisterMgr.Continue();
            RegisterMgr.ClickEditAgendaLink(0);
            this.AgendaPage(EventType.ProEvent);
            RegisterMgr.ClickEditLodgingAndTravelLink(0);
            this.LodgingAndTravelPage();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            this.MerchandisePage();
            this.CheckoutPage(RegisterType.ProEvent_Update, ProEvent.FeeCalculation_UpdateReg.Default, RegisterManager.PaymentMethod.CreditCard);
            this.ConfirmationPage(RegisterType.ProEvent_Update, ProEvent.FeeCalculation_UpdateReg.Default);
        }

        // Registers for Membership
        [Step]
        private void RegisterForMembership()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(Membership.MemberType.Name.One);
            this.emailAddress = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            this.PersonalInfoPage();
            this.MembershipFeePage();
            this.MerchandisePage();
            CheckoutPage(RegisterType.Membership, Membership.FeeCalculation_InitialReg.Default, RegisterManager.PaymentMethod.CreditCard);
            ConfirmationPage(RegisterType.Membership, Membership.FeeCalculation_InitialReg.Default);
            this.regId = RegisterMgr.GetRegID();
        }

        // Registers for events on ActiveEuropeBeta whose currency is USD
        [Step]
        private void RegisterForEnduranceEvent_USD(RegisterType testType)
        {
            this.CheckinPage();
            this.PersonalInfoPage();
            this.AgendaPage(EventType.Endurance_USD);
            this.LodgingAndTravelPage();
            RegisterMgr.Continue();
            this.MerchandisePage();
            this.CheckoutPage(testType, EnduranceEvent_USD.FeeCalculation_SingleReg.Default, RegisterManager.PaymentMethod.CreditCard);
            this.ConfirmationPage(testType, EnduranceEvent_USD.FeeCalculation_SingleReg.Default);
            this.regId = RegisterMgr.GetRegID();
        }

        // Registers for events on ActiveEuropeBeta whose currency is GBP
        private void RegisterForEnduranceEvent_Pound(RegisterType testType)
        {
            this.CheckinPage();
            this.PersonalInfoPage();
            this.AgendaPage(EventType.Endurance_Pound);
            this.LodgingAndTravelPage();
            RegisterMgr.Continue();
            this.MerchandisePage();
            this.CheckoutPage(testType, EnduranceEvent_Pound.FeeCalculation_SingleReg.Default, RegisterManager.PaymentMethod.CreditCard);
            this.ConfirmationPage(testType, EnduranceEvent_Pound.FeeCalculation_SingleReg.Default);
            this.regId = RegisterMgr.GetRegID();
        }

        #endregion

        #region individual registration pages

        // Checkin
        private void CheckinPage()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(ProEvent.RegType.Name.One);
            RegisterMgr.EnterEventDiscoutCode(ProEvent.DiscountCode);
            RegisterMgr.Continue();
        }

        // Enters basic PI 
        private void PersonalInfoPage()
        {
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
        }

        // Fills out all agenda items and discount codes based on event type
        private void AgendaPage(EventType Type)
        {
            RegisterMgr.SetCustomFieldCheckBox(ProEvent.AgendaItem.Name.CheckboxAgendaPage, true);
            
            RegisterMgr.EnterAgendaItemDiscountCode(
                ProEvent.AgendaItem.Name.CheckboxAgendaPage,
                ProEvent.DiscountCode);
            
            RegisterMgr.SelectCustomFieldRadioButtons(
                ProEvent.AgendaItem.Name.RadioButtonsParentPrice, 
                ProEvent.AgendaItem.Name.RadioButtonsParentPriceSelection);
            
            RegisterMgr.EnterAgendaItemDiscountCode(
                ProEvent.AgendaItem.Name.RadioButtonsParentPrice,
                ProEvent.DiscountCode);

            if (Type != EventType.Endurance_Pound)
            {
                RegisterMgr.SelectCustomFieldRadioButtons(
                    ProEvent.AgendaItem.Name.RadionButtonsPriceOnItem, 
                    ProEvent.AgendaItem.Name.RadioButtonsOnItemSelection);
            }
            else
            {
                RegisterMgr.SelectCustomFieldRadioButtons(
                    EnduranceEvent_Pound.AgendaItem.Name.RadionButtonsPriceOnItem,
                    EnduranceEvent_Pound.AgendaItem.Name.RadioButtonsOnItemSelection);
            }

            RegisterMgr.EnterAgendaItemDiscountCode(
                ProEvent.AgendaItem.Name.RadionButtonsPriceOnItem,
                ProEvent.DiscountCode);

            RegisterMgr.EnterAgendaItemDiscountCode(
                ProEvent.AgendaItem.Name.AlwaysSelectedAgenda,
                ProEvent.DiscountCode);

            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(
                ProEvent.AgendaItem.Name.ContributionAgendaPage, 
                ProEvent.AgendaItem.Fee.Contribution.ToString());

            RegisterMgr.SelectCustomFieldDropDown(
                ProEvent.AgendaItem.Name.DropDownParentPrice,
                ProEvent.AgendaItem.Name.DropDownParentSelection);

            RegisterMgr.EnterAgendaItemDiscountCode(
                ProEvent.AgendaItem.Name.DropDownParentPrice,
                ProEvent.DiscountCode);

            if (Type != EventType.Endurance_Pound)
            {
                RegisterMgr.SelectCustomFieldDropDown(
                    ProEvent.AgendaItem.Name.DropDownPriceOnItem,
                    ProEvent.AgendaItem.Name.DropDownOnItemSelection);
            }
            else
            {
                RegisterMgr.SelectCustomFieldDropDown(
                    EnduranceEvent_Pound.AgendaItem.Name.DropDownPriceOnItem,
                    EnduranceEvent_Pound.AgendaItem.Name.DropDownOnItemSelection);
            }

            RegisterMgr.EnterAgendaItemDiscountCode(
                ProEvent.AgendaItem.Name.DropDownPriceOnItem,
                ProEvent.DiscountCode);

            RegisterMgr.ClickRecalculateTotal();

            if (Type == EventType.Endurance_Pound)
            {
                RegisterMgr.VerifyAgendaPageTotalAmount(EnduranceEvent_Pound.FeeCalculation_SingleReg.Default.AgendaFeeTotal.Value, MoneyTool.CurrencyCode.GBP);
            }
            else
            {
                RegisterMgr.VerifyAgendaPageTotalAmount(ProEvent.FeeCalculation_SingleReg.Default.AgendaFeeTotal.Value);
            }

            RegisterMgr.Continue();
        }

        // Select items on the member fees page
        private void MembershipFeePage()
        {
            RegisterMgr.SetCustomFieldCheckBox(Membership.MembershipFees.Name.Recurring_AlwaysDiscount, true);
            
            RegisterMgr.EnterAgendaItemDiscountCode(
                Membership.MembershipFees.Name.Recurring_AlwaysDiscount,
                Membership.DiscountCode);

            RegisterMgr.SetCustomFieldCheckBox(Membership.MembershipFees.Name.Recurring_DiscountOnce, true);
            
            RegisterMgr.EnterAgendaItemDiscountCode(
                Membership.MembershipFees.Name.Recurring_DiscountOnce,
                Membership.DiscountCode);

            RegisterMgr.SetCustomFieldCheckBox(Membership.MembershipFees.Name.OneTime, true);
            
            RegisterMgr.EnterAgendaItemDiscountCode(
                Membership.MembershipFees.Name.OneTime,
                Membership.DiscountCode);

            RegisterMgr.ClickRecalculateTotal();
            RegisterMgr.VerifyMerchandiseFeePageTotalAmount(5.00, 10.00);
            RegisterMgr.Continue();
        }

        // Fill out L&T page info
        private void LodgingAndTravelPage()
        {
            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.FillOutCheckInOutDates(ProEvent.LT.CheckinDate, ProEvent.LT.CheckoutDate);
            RegisterMgr.SelectRoomPreference(ProEvent.LT.RoomPreference);
            RegisterMgr.SelectBedPreference(RegisterManager.BedPreference.King);
            RegisterMgr.SelectSmokingPreference(RegisterManager.SmokingPreference.Smoking);
        }

        private void LodgingAndTravelPageNoCheckInOutDates()
        {
            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.SelectRoomPreference(TxnIntegrityConstants.RoomPreference);
            RegisterMgr.SelectBedPreference(RegisterManager.BedPreference.King);
            RegisterMgr.SelectSmokingPreference(RegisterManager.SmokingPreference.Smoking);
        }

        // Selects merchandise items and enters discount codes
        private void MerchandisePage()
        {
            RegisterMgr.SelectMerchandiseQuantityByName(
                ProEvent.Merchandise.Name.MerchFixedPrice,
                ProEvent.Merchandise.Fee.Quantity);

            RegisterMgr.EnterMerchandiseDiscountCodeByName(
                ProEvent.Merchandise.Name.MerchFixedPrice, 
                ProEvent.DiscountCode);

            RegisterMgr.SelectMerchandiseMultipleChoiceByName(
                ProEvent.Merchandise.Name.MerchFixedPriceMC,
                ProEvent.Merchandise.Name.FixedPriceMCSelection);

            RegisterMgr.SelectMerchandiseQuantityByName(
                ProEvent.Merchandise.Name.MerchFixedPriceMC,
                ProEvent.Merchandise.Fee.Quantity);

            RegisterMgr.EnterMerchandiseDiscountCodeByName(
                ProEvent.Merchandise.Name.MerchFixedPriceMC,
                ProEvent.DiscountCode);

            RegisterMgr.EnterMerchandiseVariableAmountByName(
                ProEvent.Merchandise.Name.MerchVariableAmount,
                ProEvent.Merchandise.Fee.VariableAmount);

            RegisterMgr.SelectMerchandiseMultipleChoiceByName(
                ProEvent.Merchandise.Name.MerchVariableAmountMC,
                ProEvent.Merchandise.Name.VariableAmountMCSelection);

            RegisterMgr.EnterMerchandiseVariableAmountByName(
                ProEvent.Merchandise.Name.MerchVariableAmountMC,
                ProEvent.Merchandise.Fee.VariableAmount);

            RegisterMgr.VerifyMerchandisePageShippingFee(ProEvent.FeeCalculation_SingleReg.Default.ShippingFeeTotal.Value);
            RegisterMgr.VerifyMerchandisePageTotal(ProEvent.FeeCalculation_SingleReg.Default.MerchandiseFeeTotal.Value);
            RegisterMgr.Continue();
        }
        
        // Determines what data to verify on checkout page
        private void CheckoutPage(RegisterType type, IFeeCalculation feeCalculation, RegisterManager.PaymentMethod paymentMethod)
        {
            switch (type)
            {
                case RegisterType.ProEvent_Single:
                case RegisterType.ProEvent_Group:
                case RegisterType.Membership:
                case RegisterType.ProEvent_Simple:
                case RegisterType.ProEvent_Update:
                    this.VerifyCheckoutFeeTotals(feeCalculation);
                    RegisterMgr.PayMoney(paymentMethod);
                    RegisterMgr.FinishRegistration();
                    break;

                case RegisterType.EnduranceEvent_Single_USD:
                    this.VerifyCheckoutFeeTotals(feeCalculation);
                    RegisterMgr.ClickCheckoutActiveWaiver();
                    RegisterMgr.PayMoney(paymentMethod);
                    RegisterMgr.FinishRegistration();
                    break;

                case RegisterType.EnduranceEvent_Single_Pound:
                    this.VerifyCheckoutFeeTotals(feeCalculation, MoneyTool.CurrencyCode.GBP);
                    RegisterMgr.ClickCheckoutActiveWaiver();
                    RegisterMgr.PayMoney(paymentMethod);
                    RegisterMgr.FinishRegistration();
                    break;
            }
        }

        // Verify Fees on Checkout page, pulls data from TransactionIntegrityData
        private void VerifyCheckoutFeeTotals(
            IFeeCalculation feeCalculation, 
            Utilities.MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            RegisterMgr.VerifyCheckoutTotal(feeCalculation.TotalCharges.Value, currency);
            RegisterMgr.VerifyCheckoutSubTotal(feeCalculation.Subtotal.Value, currency);
            RegisterMgr.VerifyCheckoutTax(feeCalculation.Tax1Amount.Value, ProEvent.Tax.Name.TaxOne, currency);
            RegisterMgr.VerifyCheckoutTax(feeCalculation.Tax2Amount.Value, ProEvent.Tax.Name.TaxTwo, currency);

            if (feeCalculation.ServiceFeeTotal.HasValue)
            {
                RegisterMgr.VerifyCheckoutSerivceFee(feeCalculation.ServiceFeeTotal.Value, currency);
            }

            if (feeCalculation.ShippingFeeTotal.HasValue)
            {
                RegisterMgr.VerifyCheckoutShippingFee(feeCalculation.ShippingFeeTotal.Value, currency);
            }

            if (feeCalculation.LodgingSubtotal.HasValue)
            {
                RegisterMgr.VerifyCheckoutLodgingFeeTotal(feeCalculation.LodgingSubtotal.Value, currency);
            }

            if (feeCalculation.LodgingBookingFee.HasValue)
            {
                RegisterMgr.VerifyCheckoutLodgingBookingFeeTotal(feeCalculation.LodgingBookingFee.Value, currency);
            }

            if (feeCalculation.DiscountCodeSavings.HasValue)
            {
                if (feeCalculation.GroupDiscountSavings.HasValue)
                {
                    RegisterMgr.VerifyCheckoutGroupAndDiscountSaving(
                        feeCalculation.GroupDiscountSavings.Value,
                        feeCalculation.DiscountCodeSavings.Value, 
                        currency);
                }

                if (!feeCalculation.GroupDiscountSavings.HasValue)
                {
                    RegisterMgr.VerifyCheckoutSaving(feeCalculation.DiscountCodeSavings.Value, currency);
                }
            }

            if (feeCalculation.RecurringSubtotal.HasValue)
            {
                RegisterMgr.VerifyCheckoutMembershipRecurringSubtotal(feeCalculation.RecurringSubtotal.Value, currency);
            }

            if (feeCalculation.RecurringTax1Amount.HasValue)
            {
                RegisterMgr.VerifyCheckoutMembershipReccuringTax(feeCalculation.RecurringTax1Amount.Value, ProEvent.Tax.Name.TaxOne, currency);
            }

            if (feeCalculation.RecurringTax2Amount.HasValue)
            {
                RegisterMgr.VerifyCheckoutMembershipReccuringTax(feeCalculation.RecurringTax2Amount.Value, ProEvent.Tax.Name.TaxTwo, currency);
            }

            if (feeCalculation.YearlyFees.HasValue)
            {
                RegisterMgr.VerifyCheckoutMembershipRecurringTotal(feeCalculation.YearlyFees.Value, currency);
            }

            if (feeCalculation.YearlyFeesDiscount.HasValue)
            {
                RegisterMgr.VerifyCheckoutMembershipRecurringSaving(feeCalculation.YearlyFeesDiscount.Value, currency);
            }
        }

        // Determines what data to verify on confirmation page
        private void ConfirmationPage(RegisterType type, IFeeCalculation feeCalculation)
        {
            if (RegisterMgr.OnConfirmationRedirectPage())
            {
                RegisterMgr.ClickAdvantageNo();
            }

            switch (type)
            {
                case RegisterType.ProEvent_Single:
                case RegisterType.ProEvent_Update:
                case RegisterType.ProEvent_Group:
                case RegisterType.Membership:
                case RegisterType.ProEvent_Simple:
                case RegisterType.EnduranceEvent_Single_USD:
                    this.VerifyConfirmationFeeTotals(feeCalculation);
                    break;

                case RegisterType.EnduranceEvent_Single_Pound:
                    this.VerifyConfirmationFeeTotals(feeCalculation, MoneyTool.CurrencyCode.GBP);
                    break;
            }
        }

        // Verify Fees on Confirmation page, pulls data from TransactionIntegrityData
        private void VerifyConfirmationFeeTotals(
            IFeeCalculation feeCalculation, 
            MoneyTool.CurrencyCode currency = MoneyTool.CurrencyCode.USD)
        {
            RegisterMgr.ConfirmRegistration();
            RegisterMgr.VerifyConfirmationSubTotal(feeCalculation.Subtotal.Value, currency);
            RegisterMgr.VerifyConfirmationTax(feeCalculation.Tax1Amount.Value, ProEvent.Tax.Name.TaxOne, currency);
            RegisterMgr.VerifyConfirmationTax(feeCalculation.Tax2Amount.Value, ProEvent.Tax.Name.TaxTwo, currency);

            if (feeCalculation.ServiceFeeTotal.HasValue)
            {
                RegisterMgr.VerifyConfirmationSerivceFee(feeCalculation.ServiceFeeTotal.Value, currency);
            }

            if (feeCalculation.ShippingFeeTotal.HasValue)
            {
                RegisterMgr.VerifyConfirmationShippingFee(feeCalculation.ShippingFeeTotal.Value, currency);
            }

            if (feeCalculation.LodgingSubtotal.HasValue)
            {
                RegisterMgr.VerifyConfirmationLodgingFeeTotal(feeCalculation.LodgingSubtotal.Value, currency);
            }

            if (feeCalculation.LodgingBookingFee.HasValue)
            {
                RegisterMgr.VerifyConfirmationLodgingBookingFeeTotal(feeCalculation.LodgingBookingFee.Value, currency);
            }

            if (feeCalculation.DiscountCodeSavings.HasValue)
            {
                if (feeCalculation.GroupDiscountSavings.HasValue)
                {
                    RegisterMgr.VerifyConfirmationGroupAndDiscountSaving(
                    feeCalculation.GroupDiscountSavings.Value, 
                    feeCalculation.DiscountCodeSavings.Value,
                    currency);
                }
                else
                {
                    RegisterMgr.VerifyConfirmationSaving(feeCalculation.DiscountCodeSavings.Value, currency);
                }
            }

            if (feeCalculation.RecurringSubtotal.HasValue)
            {
                RegisterMgr.VerifyConfirmationMembershipRecurringSubtotal(feeCalculation.RecurringSubtotal.Value, currency);
            }

            if (feeCalculation.RecurringTax1Amount.HasValue)
            {
                RegisterMgr.VerifyConfirmationMembershipReccuringTax(
                    feeCalculation.RecurringTax1Amount.Value, 
                    ProEvent.Tax.Name.TaxOne,
                    currency);
            }

            if (feeCalculation.RecurringTax2Amount.HasValue)
            {
                RegisterMgr.VerifyConfirmationMembershipReccuringTax(
                    feeCalculation.RecurringTax2Amount.Value, 
                    ProEvent.Tax.Name.TaxTwo,
                    currency);
            }

            if (feeCalculation.YearlyFees.HasValue)
            {
                RegisterMgr.VerifyConfirmationMembershipRecurringTotal(feeCalculation.YearlyFees.Value, currency);
            }

            if (feeCalculation.YearlyFeesDiscount.HasValue)
            {
                RegisterMgr.VerifyConfirmationMembershipRecurringSaving(feeCalculation.YearlyFeesDiscount.Value, currency);
            }

            RegisterMgr.VerifyConfirmationTotal(feeCalculation.TotalCharges.Value, currency);
        }

        // Renews the membership 
        [Step]
        private void RenewMembership()
        {
            RegisterMgr.CheckinToExistingMembership(this.eventId, this.emailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.LoginToMembersip();
            RegisterMgr.ClickRenewNowButton();
            RegisterMgr.PayMoney(RegisterManager.PaymentMethod.CreditCard);
            RegisterMgr.FinishRegistration();
        }

        #endregion

        #region Report and DB Verification
        private void VerifyReportData(EventType type, IFeeCalculation feeCalculation)
        {
            if (type == EventType.Membership)
            {
                this.VerifyAttendeeReportTotals(feeCalculation.TotalCharges.Value + feeCalculation.YearlyFees.Value, type);
            }
            else
            {
                this.VerifyAttendeeReportTotals(feeCalculation.TotalCharges.Value, type);
            }

            this.VerifyTransactionFeesReport(feeCalculation.ExpectedTransactionFeesReportData);
            this.VerifyTransactionReport(type, feeCalculation.ExpectedTransactionReportData);
        }

        // Verifies total charges and balance due in the attendee report
        [Verify]
        private void VerifyAttendeeReportTotals(double totalCharge, EventType type)
        {
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(ReportManager.CommonReportType.RegistrantList);

            switch (type)
            {
                case EventType.Membership:
                case EventType.ProEvent:
                case EventType.ProEventUpdate:
                    ReportMgr.VerifyReportTotalChargeAndBalanceDueByRegId(regId, totalCharge, 0.00);
                    break;
                
                case EventType.Endurance_USD:
                    ReportMgr.VerifyEnduranceReportTotalChargeAndBalanceDueByRegId(eventId, regId, totalCharge, 0.00);
                    break;

                case EventType.Endurance_Pound:
                    ReportMgr.VerifyEnduranceReportTotalChargeAndBalanceDueByRegId(eventId, regId, totalCharge, 0.00, MoneyTool.CurrencyCode.GBP);
                    break;
            }

            UIUtilityProvider.UIHelper.ClosePopUpWindow();
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
        }

        // verifies pertinant data in transaction fees report
        [Verify]
        private void VerifyTransactionFeesReport(string[] expectedData)
        {
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(ReportManager.CommonReportType.TransactionFees);
            ReportMgr.ExpandTransactionFeesReportRow(regId);
            ReportMgr.VerifyExpandedTransactionFeeRowData(regId, expectedData);
            UIUtilityProvider.UIHelper.ClosePopUpWindow();
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
        }

        // Verifies all pertinant data and rows in the transactions report
        [Verify]
        private void VerifyTransactionReport(EventType type, string[] expectedData)
        {
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(ReportManager.CommonReportType.Transaction);

            if (type == EventType.ProEventUpdate)
            {
                expectedData[9] = (eventId + "-" + regId);
                expectedData[expectedData.Length - 1] = (eventId + "-" + regId + "-01");
            }
            else
            {
                expectedData[expectedData.Length - 1] = (eventId + "-" + regId);
            }

            ReportMgr.VerifyTransactionsReportData(regId, expectedData);
            UIUtilityProvider.UIHelper.ClosePopUpWindow();
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
        }

        // Method to charge somone on the attendee info page
        [Step]
        private void PerformBackendCharge()
        {
            LoginAndGoToRegressionFolder();
            BackendMgr.OpenAttendeeInfoURL(eventSessionId, regId);
            BackendMgr.EditPaymentMethodCreditCard();
            BackendMgr.ChargeRemainingBalance(ProEvent.FeeCalculation_SimpleReg.Default.TotalCharges.Value);
        }

        /// <summary>
        /// Using Linq, this method calls to the db to get the transaction data stored for a registrant. 
        /// Currently it is only looking at the type 2 (cc charge) row in the db, and eventually needs
        /// to be expanded to look at all transaction types. This whole method could probably be expanded
        /// into its own class. I will look further into it as I continue with transaction stuff. 
        /// </summary>
        /// <param name="TransactionAmount"></param>
        /// <param name="BillableAmount"></param>
        /// <param name="addAndModBy"></param>
        [Verify]
        public void VerifyTransactionDataInDB(string[] TransactionAmount, string[] BillableAmount, string[] addAndModBy, string SharedFeePct, EventType type)
        {
            var db = new DataAccess.ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            var transactions = (from t in db.Transactions where t.RegisterId == regId && t.TypeId == 2 select t).ToList();

            for (int i = 0; i < transactions.Count; i++)
            {
                VerifyTool.VerifyValue(2, transactions[i].TypeId, "Type Id = {0}");
                VerifyTool.VerifyValue(regId, transactions[i].RegisterId, "Register Id = {0}");
                VerifyTool.VerifyValue(TransactionAmount[i], transactions[i].TransAmount.ToString(), "Tansaction Amount = {0}");
                VerifyTool.VerifyValue(GetExchangeRate(eventId), transactions[i].ExchangeRate.ToString(), "Exchange Rate = {0}");

                Event evt = new Event();

                evt = (from e in db.Events where e.Id == eventId select e).Single();

                VerifyTool.VerifyValue(evt.CurrencyCode, transactions[i].CurrencyCode, "Currency Code = {0}");
                VerifyTool.VerifyValue("1", transactions[i].BaseExchangeRate.ToString(), "Base Exchange Rate = {0}");
                VerifyTool.VerifyValue("USD", transactions[i].BaseCurrencyCode, "Base Currency Code = {0}");
                
                if (i == 0)
                {
                    VerifyTool.VerifyValue(eventId + "-" + regId, transactions[i].TransDoc.ToString(), "Trans Doc = {0}");
                }
                else if (i > 0 && type == EventType.ProEventUpdate)
                {
                    VerifyTool.VerifyValue(eventId + "-" + regId + "-0" + (i), transactions[i].TransDoc.ToString(), "Trans Doc = {0}");
                }
                else
                {
                    VerifyTool.VerifyValue(eventId + "-" + regId + "-0" + i, transactions[i].TransDoc.ToString(), "Trans Doc = {0}");
                }

                // Comment this out: an decryption overflow error happened when trying to decrypt cc number
                ////VerifyTool.VerifyValue(
                ////    TxnIntegrityConstants.ccNumber, 
                ////    U.EncryptionTools.ccDecrypt(transactions[i].encryptedccNumber, regId.ToString()), 
                ////    "CCnumber = {0}");

                VerifyTool.VerifyValue(TxnIntegrityConstants.ccExpDate, transactions[i].ccExpDate.ToString(), "CC Exp date = {0}");
                VerifyTool.VerifyValue(TxnIntegrityConstants.ccName, transactions[i].ccName, "CC name = {0}");
                VerifyTool.VerifyValue(TxnIntegrityConstants.ccType, transactions[i].ccType, "CC type = {0}");
                VerifyTool.VerifyValue(TxnIntegrityConstants.MerchandId, transactions[i].MerchantId, "Merchant Id = {0}");
                VerifyTool.VerifyValue(addAndModBy[i], transactions[i].Add_By, "Add By = {0}");
                VerifyTool.VerifyValue(addAndModBy[i], transactions[i].Mod_By, "Mod By = {0}");
                VerifyTool.VerifyValue(TxnIntegrityConstants.LastFourCC, transactions[i].last4CCDigits.ToString(), "Last 4 CC digits {0}");
                VerifyTool.VerifyValue(true, transactions[i].authCode != null, "Auth code is not null = {0}");
                VerifyTool.VerifyValue(SharedFeePct, transactions[i].SharedFeePct.ToString(), "Shared Fee Percent = {0}");
                
                if (type != EventType.Endurance_USD)
                {
                    VerifyTool.VerifyValue(TxnIntegrityConstants.CustomerMerchantId, transactions[i].CustomerMerchantId.ToString(), "Customer Merchand Id = {0}");
                }
                else
                {
                    VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceCustomerMerchId, transactions[i].CustomerMerchantId.ToString(), "Customer Merchand Id = {0}");
                    VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceMerchAmount, transactions[i].MerchAmount.ToString(), "Merch Amount = {0}");
                    VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceNonMerchAmount, transactions[i].NonMerchAmount.ToString(), "Non-Merch Amount = {0}");
                }

                VerifyTool.VerifyValue(BillableAmount[i], transactions[i].BillableAmount.ToString(), "Billable Amount = {0}");
                VerifyTool.VerifyValue(true, transactions[i].GatewayTransactionId != null, "Gateway Transaction Id is not null = {0}");
            }
        }

        /// <summary>
        /// This can be ignored for now as Paymentech is not returning the amount, which is really what we need to verify
        /// and they are not being helpful in figuring out why it isn't working... Waiting on Brett. 
        /// </summary>
        /// <param name="merchId"></param>
        /// <param name="txnOrderId"></param>
        //private void VerifyChargeInPT(string merchId, string txnOrderId)
        //{
        //    C.Event e = C.Event.Get(620451);

        //    if (e.Gateway.Type == C.GatewayType.Paymentech)
        //    {
        //        C.PaymentechGateway gateway = e.Gateway as C.PaymentechGateway;
        //        C.OnlineTransactionResults results = gateway.Inquiry(merchId, txnOrderId);

        //        //using results
        //        string authCode = results.AuthCode;
        //        C.OnlineTransactionStatus status = results.Status;

        //        //use MostRecentInquiryResponse directly
        //        string expected = gateway.MostRecentInquiryResponse.RequestAmount;

        //        //...verify

        //    }
        //    else
        //    {
        //        //oops - error
        //    }
        //}

        //Runs Build Invoice Items, populating InvoiceItems_AdHoc
        [Step]
        private void Billing()
        {
            SqlConnection sqlConnection1 = new SqlConnection(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            SqlCommand spcmd = new SqlCommand();
            Int32 rowsAffeted;

            spcmd.CommandText = "BuildInvoiceItems";
            spcmd.CommandType = CommandType.StoredProcedure;
            spcmd.Connection = sqlConnection1;
            SqlParameter MyParm1 = spcmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
            MyParm1.Value = DateTimeTool.FirstDayOfMonth(DateTime.Now).AddMonths(-1);
            SqlParameter myParm2 = spcmd.Parameters.Add("@ToDate", SqlDbType.DateTime);
            myParm2.Value = DateTimeTool.LastDayOfMonth(DateTime.Now);
            sqlConnection1.Open();
            rowsAffeted = spcmd.ExecuteNonQuery();
            sqlConnection1.Close();
        }

        //Runs The Final Billing sproc
        [Step]
        private void FinalizeBilling()
        {
            ClearPreviousInvoiceData();
            SqlConnection sqlConnection1 = new SqlConnection(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ROMasterConnection);
            SqlCommand spcmd = new SqlCommand();
            Int32 rowsAffeted;

            spcmd.CommandText = "BillingFinalize";
            spcmd.CommandType = CommandType.StoredProcedure;
            spcmd.Connection = sqlConnection1;
            SqlParameter MyParm1 = spcmd.Parameters.Add("@startdate", SqlDbType.DateTime);
            MyParm1.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1); 
            SqlParameter myParm2 = spcmd.Parameters.Add("@enddate", SqlDbType.DateTime);
            myParm2.Value = DateTimeTool.LastDayOfMonth(DateTime.Now);
            SqlParameter myParm3 = spcmd.Parameters.Add("@billingcycleid", SqlDbType.TinyInt);
            myParm3.Value = 1;
            sqlConnection1.Open();
            rowsAffeted = spcmd.ExecuteNonQuery();
            sqlConnection1.Close();
        }

        //Verifies the data in InvoiceItems_AdHoc
        [Verify]
        private void VerifyBilling(EventType type)
        {
            var db = new DataAccess.ROWarehouseDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ROWarehouseConnection);
            var billing = (from i in db.InvoiceItems_AdHocs where i.StartDate == DateTime.Today && i.EventId == eventId select i).ToList();
            if (billing.Count == 0)
            {
                Assert.Fail("InvoiceItems_AdHoc did not return any values, there is no data to verify");
            }
            switch (type)
            {
                case EventType.ProEvent:
                    for (int i = 0; i < billing.Count; i++)
                    {
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EventAmount[i].ToString(), billing[i].Amount.ToString(), "Invoice Items AdHoc Event Amount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EventCCAmount[i].ToString(), billing[i].CCAmount.ToString(), "Invoice Items AdHoc Event CCAmount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EventCCCost.ToString(), billing[i].CCCost.ToString(), "Invoice Items AdHoc Event CCCost = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EventRegCount[i], billing[i].RegCount, "Invoice Items AdHoc Event RegCount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EventTypeId[i], billing[i].TypeId, "Invoice Items AdHoc Event TypeId = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EventCusomterId[i], billing[i].CustomerId, "Invoice Items AdHoc Event CustomerId = {0}");
                    }
                    break;
                case EventType.Membership:
                    for (int i = 0; i < billing.Count; i++)
                    {
                        VerifyTool.VerifyValue(TxnIntegrityConstants.MembershipAmount[i].ToString(), billing[i].Amount.ToString(), "Invoice Items AdHoc Memebership Amount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.MembershipCCAmount[i].ToString(), billing[i].CCAmount.ToString(), "Invoice Items AdHoc Memebership CCAmount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.MembershipCCCost.ToString(), billing[i].CCCost.ToString(), "Invoice Items AdHoc Memebership CCCost = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.MembershipRegCount[i], billing[i].RegCount, "Invoice Items AdHoc Memebership RegCount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.MembershipTypeId[i], billing[i].TypeId, "Invoice Items AdHoc Memebership TypeId = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.MembershipCusomterId[i], billing[i].CustomerId, "Invoice Items AdHoc Memebership CustomerId = {0}");
                    }
                    break;
                case EventType.Endurance_USD:
                    for (int i = 0; i < billing.Count; i++)
                    {
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceAmount[i].ToString(), billing[i].Amount.ToString(), "Invoice Items AdHoc MerchNonMerch Amount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceCCAmount[i].ToString(), billing[i].CCAmount.ToString(), "Invoice Items AdHoc MerchNonMerch CCAmount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceCCCost.ToString(), billing[i].CCCost.ToString(), "Invoice Items AdHoc MerchNonMerch CCCost = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceRegCount[i], billing[i].RegCount, "Invoice Items AdHoc MerchNonMerch RegCount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceTypeId[i], billing[i].TypeId, "Invoice Items AdHoc MerchNonMerch TypeId = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceCusomterId[i], billing[i].CustomerId, "Invoice Items AdHoc MerchNonMerch CustomerId = {0}");
                    }
                    break;
                case EventType.Endurance_Pound:
                    // Finds what the various billing amounts should be based on amount charged and exchange rate
                    TxnIntegrityConstants.DiffCurrencyAmount[1] =
                        (decimal)System.Data.SqlTypes.SqlDecimal.Round(-Convert.ToDecimal(
                        FeeTotals.getDifferentCurrencies().TotalCharges / Convert.ToDouble(GetExchangeRate(eventId))),2);
                    TxnIntegrityConstants.DiffCurrencyAmount[2] = 
                       (decimal)(System.Data.SqlTypes.SqlDecimal.Round( Convert.ToDecimal(
                        (Convert.ToDouble(GetDifferentCurrenciesServiceFee()) - 
                        (TxnIntegrityConstants.PerRegFee * 
                        Convert.ToDouble(GetExchangeRate(eventId)))) /
                        Convert.ToDouble(GetExchangeRate(eventId))), 2) + 0.01m);
                    TxnIntegrityConstants.DiffCurrencyCCAmount[1] = TxnIntegrityConstants.DiffCurrencyAmount[1];
                    for (int i = 0; i < billing.Count; i++)
                    {
                        VerifyTool.VerifyValue(Convert.ToDouble(
                            TxnIntegrityConstants.DiffCurrencyAmount[i]), 
                            Convert.ToDouble(billing[i].Amount), 
                            "Invoice Items AdHoc Amount = {0}");

                        VerifyTool.VerifyValue(
                            Convert.ToDouble(TxnIntegrityConstants.DiffCurrencyCCAmount[i]), 
                            Convert.ToDouble(billing[i].CCAmount), 
                            "Invoice Items AdHoc CCAmount = {0}");

                        VerifyTool.VerifyValue(TxnIntegrityConstants.DiffCurrencyCCCost.ToString(), billing[i].CCCost.ToString(), "Invoice Items AdHoc DiffCurrencies CCCost = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.DiffCurrencyRegCount[i], billing[i].RegCount, "Invoice Items AdHoc DiffCurrencies RegCount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.DiffCurrencyTypeId[i], billing[i].TypeId, "Invoice Items AdHoc DiffCurrencies TypeId = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.DiffCurrencyCusomterId[i], billing[i].CustomerId, "Invoice Items AdHoc DiffCurrencies CustomerId = {0}");
                    }
                    break;
            }
        }

        [Verify]
        public void VerifyFinalizeBilling(EventType type)
        {
            var db = new DataAccess.ROMasterDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ROMasterConnection);
            var finalBilling = (from i in db.CustomerInvoiceItems where i.EventId == eventId select i).ToList();
            if (finalBilling.Count == 0)
            {
                Assert.Fail("CustomerInvoiceItems did not return any values, there is no data to verify");
            }
            switch (type)
            {
                case EventType.ProEvent:
                    for (int i = 0; i < finalBilling.Count; i++)
                    {
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EventFinalAmount[i].ToString(), finalBilling[i].Amount.ToString(), "Customer Invoice Item Event Amount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EventFinalCCAmount[i].ToString(), finalBilling[i].CCAmount.ToString(), "Customer Invoice Item Event CCAmount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EventFinalCCCost.ToString(), finalBilling[i].CCCost.ToString(), "Customer Invoice Item Event CCCost = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EventFinalRegCount[i], finalBilling[i].RegCount, "Customer Invoice Item Event RegCount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EventFinalItemId[i], finalBilling[i].ItemId, "Customer Invoice Item Event TypeId = {0}");
                    }
                    break;
                case EventType.Membership:
                    for (int i = 0; i < finalBilling.Count; i++)
                    {
                        VerifyTool.VerifyValue(TxnIntegrityConstants.MembershipFinalAmount[i].ToString(), finalBilling[i].Amount.ToString(), "Customer Invoice Item Membership Amount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.MembershipFinalCCAmount[i].ToString(), finalBilling[i].CCAmount.ToString(), "Customer Invoice Item Membership CCAmount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.MembershipFinalCCCost.ToString(), finalBilling[i].CCCost.ToString(), "Customer Invoice Item Membership CCCost = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.MembershipFinalRegCount[i], finalBilling[i].RegCount, "Customer Invoice Item Membership RegCount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.MembershipFinalItemId[i], finalBilling[i].ItemId, "Customer Invoice Item Membership TypeId = {0}");
                    }
                    break;
                case EventType.Endurance_USD:
                    for (int i = 0; i < finalBilling.Count; i++)
                    {
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceFinalAmount[i].ToString(), finalBilling[i].Amount.ToString(), "Customer Invoice Item MerchNonMerch Amount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceFinalCCAmount[i].ToString(), finalBilling[i].CCAmount.ToString(), "Customer Invoice Item MerchNonMerch CCAmount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceFinalCCCost.ToString(), finalBilling[i].CCCost.ToString(), "Customer Invoice Item MerchNonMerch CCCost = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceFinalRegCount[i], finalBilling[i].RegCount, "Customer Invoice Item MerchNonMerch RegCount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.EnduranceFinalItemId[i], finalBilling[i].ItemId, "Customer Invoice Item MerchNonMerch TypeId = {0}");
                    }
                    break;
                case EventType.Endurance_Pound:
                    for (int i = 0; i < finalBilling.Count; i++)
                    {
                        if (i > 0)
                        {
                            VerifyTool.VerifyValue(((decimal)(System.Data.SqlTypes.SqlDecimal.Round((TxnIntegrityConstants.DiffCurrencyFinalAmount[i] /
                                Convert.ToDecimal(GetExchangeRate(eventId))), 2))).ToString("0.0000"),
                                finalBilling[i].Amount.ToString(),
                                "Customer Invoice Item DiffCurrencies Amount = {0}");
                        }
                        else
                        {
                            VerifyTool.VerifyValue(TxnIntegrityConstants.DiffCurrencyFinalAmount[i].ToString(), finalBilling[i].Amount.ToString(), "Customer Invoice Item DiffCurrencies Amount = {0}");
                        }

                        if (i == 1)
                        {
                            VerifyTool.VerifyValue(((decimal)System.Data.SqlTypes.SqlDecimal.Round((TxnIntegrityConstants.DiffCurrencyFinalCCAmount[i] /
                                Convert.ToDecimal(GetExchangeRate(eventId))),2)).ToString("0.0000"),
                                finalBilling[i].CCAmount.ToString(),
                                "Customer Invoice Item DiffCurrencies CCAmount = {0}");
                        }
                        else
                        {
                            VerifyTool.VerifyValue(TxnIntegrityConstants.DiffCurrencyFinalCCAmount[i].ToString(), finalBilling[i].CCAmount.ToString(), "Customer Invoice Item DiffCurrencies CCAmount = {0}");
                        }

                        VerifyTool.VerifyValue(TxnIntegrityConstants.DiffCurrencyFinalCCCost.ToString(), finalBilling[i].CCCost.ToString(), "Customer Invoice Item DiffCurrencies CCCost = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.DiffCurrencyFinalRegCount[i], finalBilling[i].RegCount, "Customer Invoice Item DiffCurrencies RegCount = {0}");
                        VerifyTool.VerifyValue(TxnIntegrityConstants.DiffCurrencyFinalItemId[i], finalBilling[i].ItemId, "Customer Invoice Item DiffCurrencies TypeId = {0}");
                    }
                    break;
            }
        }

        private void ChangeTodaysRegistrationsToTestAndDelete(string eventName)
        {
            LoginAndGoToRegressionFolder();
            eventId = ManagerSiteMgr.GetFirstEventId(eventName);
            DataHelperTool.ChangeAllRegsToTestForEvent(eventId);
            ManagerSiteMgr.OpenEventDashboardAndDeleteTestRegsAndReturnToManagerScreen(eventId);
        }

        private void ChangeRegistrationsToTestStatusAndDelete()
        {
            DataHelperTool.ChangeAllRegsToTestForEvent(this.eventId);
            ManagerSiteMgr.DashboardMgr.DeleteTestRegs();
        }

        public void ClearPreviousInvoiceData()
        {
            var db = new DataAccess.ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ROMasterConnection);
            int rowsAffected;
            string Command = string.Format(
                "DELETE FROM ct FROM dbo.CustomerTransactions ct INNER JOIN dbo.CustomerInvoices ci ON ci.EndDate = ct.TransDate AND ci.EndDate BETWEEN '{0}' AND '{1}'",
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1), DateTimeTool.LastDayOfMonth(DateTime.Now));
            rowsAffected = db.ExecuteCommand(Command);
            Command = string.Format(
                "DELETE  FROM ci FROM dbo.CustomerInvoiceItems cii INNER JOIN dbo.CustomerInvoices ci ON ci.Id = cii.InvoiceId WHERE ci.InvoiceDate BETWEEN '{0}' AND '{1}'",
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1), DateTimeTool.LastDayOfMonth(DateTime.Now));
            rowsAffected = db.ExecuteCommand(Command);
            Command = string.Format(
                "DELETE  FROM dbo.CustomerInvoices WHERE InvoiceDate BETWEEN '{0}' AND '{1}'",
                new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1), DateTimeTool.LastDayOfMonth(DateTime.Now));
            rowsAffected = db.ExecuteCommand(Command);
            Command = string.Format(
                @"UPDATE [VSQL2.RegOnline.com\vsql2].ROWarehouse.dbo.ALLDATES SET BUILDSTATUS = 0 WHERE DAYNOW <= '{0}' AND DAYNOW >= '{1}'",
                DateTime.Now, new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1));
            rowsAffected = db.ExecuteCommand(Command);
        }

        // Gets the exchange rate for an event. Keep an eye on it as eventually you might be able to do this from core
        public string GetExchangeRate(int eventID)
        {
            ClientDataContext db = new ClientDataContext();
            return (from c in db.Events where c.Id == eventID select c).Single().CurrencyCode;
        }

        public string GetDifferentCurrenciesServiceFee()
        {
            FeeTotals f = FeeTotals.getDifferentCurrencies();
            f.ServiceFeeTotal = f.ServiceFeeTotal + (TxnIntegrityConstants.PerRegFee * Convert.ToDouble(GetExchangeRate(eventId)));
            f.ServiceFeeTotal = Math.Round((double)f.ServiceFeeTotal, 2);
            return f.ServiceFeeTotal.ToString();
        }
        #endregion

        #endregion
    }
}
