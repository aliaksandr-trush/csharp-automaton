namespace RegOnline.RegressionTest.Fixtures.Reports.StandardReports
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Managers.Backend;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class CCVisibilityFixture : FixtureBase
    {
        private const string EventName = "CCVisibilityFixture";
        private const double EventFee = 10;

        private int eventId;
        private string eventSessionId;
        private int registrationId;

        [Test]
        [Category(Priority.Two)]
        [Description("397")]
        public void UserWithAccess()
        {
            this.PrepareEventAndRegistrations(true);
            this.Verify(true);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("398")]
        public void UserWithoutAccess()
        {
            this.PrepareEventAndRegistrations(false);
            this.Verify(false);
        }

        [Step]
        private void PrepareEventAndRegistrations(bool withAccess)
        {
            ManagerSiteMgr.OpenLogin();
            this.LoginAndSelectFolder(withAccess);
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);

            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.SetEventFee(EventFee);
            BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.LodgingTravel);
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.ChoosePaymentOption(Managers.Builder.LodgingSettingsAndPaymentOptionsManager.PaymentOption.CollectCCInfo);
            BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Checkout);
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndClose();

            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.FillOutLodgingCCInfo_Default();
            RegisterMgr.Continue();
            RegisterMgr.PayMoney(Managers.ManagerBase.PaymentMethod.CreditCard);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            this.registrationId = RegisterMgr.GetRegistrationIdOnConfirmationPage();
        }

        [Verify]
        private void Verify(bool withAccess)
        {
            ManagerSiteMgr.OpenLogin();
            this.LoginAndSelectFolder(withAccess);
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);

            BackendMgr.OpenAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.PersonalInformation);

            // CC number in Payment Info is still not visible even if current user has access to CC number
            BackendMgr.VerifyPaymentInfoCCNumber(Managers.Register.PaymentManager.DefaultPaymentInfo.CCNumber_Encrypted);

            BackendMgr.OpenAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.Transactions);

            // CC number in Transaction is still not visible even if current user has access to CC number
            BackendMgr.VerifyTransactionHistory(
                null,
                BackendManager.TransactionTypeString.OnlineCCPayment,
                Managers.Register.PaymentManager.DefaultPaymentInfo.CCNumber_Encrypted,
                0 - EventFee,
                this.eventId);

            BackendMgr.OpenAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.LodgingAndTravel);
            Utility.ThreadSleep(10);

            if (withAccess)
            {
                BackendMgr.VerifyLodgingField(
                    Managers.Backend.BackendManager.LodgingViewField.CreditCardNumber,
                    Managers.Register.PaymentManager.DefaultPaymentInfo.CCNumber);
            }
            else
            {
                BackendMgr.VerifyLodgingField(
                    Managers.Backend.BackendManager.LodgingViewField.CreditCardNumber,
                    Managers.Register.PaymentManager.DefaultPaymentInfo.CCNumber_Encrypted);
            }

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.Reports);
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(Managers.Report.ReportManager.CommonReportType.RoomingList);
            ManagerSiteMgr.DashboardMgr.SelectReportPopupWindow();

            if (withAccess)
            {
                ReportMgr.VerifyDecryptedCCNumberPresent();
            }
            else
            {
                ReportMgr.VerifyEncryptedCCNumberPresent();
            }

            ReportMgr.CloseReportPopupWindow();

            string customReportName = "CCVisibility";
            ManagerSiteMgr.DashboardMgr.CreateCustomReport(customReportName);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(customReportName);

            if (withAccess)
            {
                ReportMgr.VerifyDecryptedCCNumberPresent();
            }
            else
            {
                ReportMgr.VerifyEncryptedCCNumberPresent();
            }

            ReportMgr.CloseReportPopupWindow();
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ReturnToList();
        }

        private void LoginAndSelectFolder(bool withAccess)
        {
            if (withAccess)
            {
                ManagerSiteMgr.Login();
                ManagerSiteMgr.SelectFolder();
            }
            else
            {
                ManagerSiteMgr.Login("internaluser", "Abcd1234");
                ManagerSiteMgr.SelectFolder("InternalTest");
            }
        }
    }
}