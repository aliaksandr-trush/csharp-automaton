namespace RegOnline.RegressionTest.Fixtures.Manager.AccountInfo
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Manager.Account;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class SuperadminFixture : FixtureBase
    {
        [Test]
        [Category(Priority.Two)]
        [Description("407")]
        public void CannotChangePasswordOrRole()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();

            ManagerSiteMgr.ClickEditUserLink();
            ManagerSiteMgr.AccountMgr.EditUserMgr.VerifyCurrentPasswordValidationVisible(true);
            ManagerSiteMgr.AccountMgr.EditUserMgr.VerifyIsPasswordEditable(false);
            ManagerSiteMgr.AccountMgr.EditUserMgr.VerifyIsUserRoleEditable(false);
            ManagerSiteMgr.AccountMgr.EditUserMgr.TypeCurrentPasswordToValidate();
            ManagerSiteMgr.AccountMgr.EditUserMgr.VerifyIsPasswordEditable(true);
            ManagerSiteMgr.AccountMgr.EditUserMgr.VerifyIsUserRoleEditable(true);
            ManagerSiteMgr.AccountMgr.EditUserMgr.SaveAndClose();

            this.SetSuperadmin(ManagerSiteMgr.GetEventSessionId(), true);

            ManagerSiteMgr.ClickEditUserLink();
            ManagerSiteMgr.AccountMgr.EditUserMgr.VerifyCurrentPasswordValidationVisible(false);
            ManagerSiteMgr.AccountMgr.EditUserMgr.VerifyIsPasswordEditable(false);
            ManagerSiteMgr.AccountMgr.EditUserMgr.VerifyIsUserRoleEditable(false);
            ManagerSiteMgr.AccountMgr.EditUserMgr.Cancel();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("408")]
        public void CannotEditBillingInfo()
        {
            AccountInformationManager accountInfoMgr = new AccountInformationManager();

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GotoTab(Managers.Manager.ManagerSiteManager.Tab.Account);
            ManagerSiteMgr.AccountMgr.ChooseTab(Managers.Manager.Account.AccountManager.AccountTab.Info);

            accountInfoMgr.ClickEditBillingInfo();
            accountInfoMgr.VerifyCanEditBillingInfo(true);
            accountInfoMgr.VerifyButtonVisible_SameAsPrimary(true);
            accountInfoMgr.VerifyButtonVisible_Save(true);
            accountInfoMgr.VerifyButtonVisible_Cancel(true);
            accountInfoMgr.ClickSaveBillingInfo();

            this.SetSuperadmin(ManagerSiteMgr.GetEventSessionId(), true);

            accountInfoMgr.ClickEditBillingInfo();
            accountInfoMgr.VerifyCanEditBillingInfo(true);
            accountInfoMgr.VerifyButtonVisible_SameAsPrimary(false);
            accountInfoMgr.VerifyButtonVisible_Save(false);
            accountInfoMgr.VerifyButtonVisible_Cancel(true);
            accountInfoMgr.ClickCancelBillingInfo();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("409")]
        public void CannotSeeCreditCard()
        {
            string eventName = "SuperadminFixture";

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(eventName);
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            int eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(eventName);
            BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.LodgingTravel);
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.ChoosePaymentOption(Managers.Builder.LodgingSettingsAndPaymentOptionsManager.PaymentOption.CollectCCInfo);
            BuilderMgr.SaveAndClose();

            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.FillOutLodgingCCInfo_Default();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            int registrationId = RegisterMgr.GetRegistrationIdOnConfirmationPage();

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            string eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(eventSessionId, registrationId);
            BackendMgr.OpenAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.LodgingAndTravel);

            BackendMgr.VerifyLodgingField(
                Managers.Backend.BackendManager.LodgingViewField.CreditCardNumber, 
                Managers.Register.PaymentManager.DefaultPaymentInfo.CCNumber);

            this.SetSuperadmin(eventSessionId, true);

            BackendMgr.OpenAttendeeSubPage(Managers.Backend.BackendManager.AttendeeSubPage.LodgingAndTravel);

            BackendMgr.VerifyLodgingField(
                Managers.Backend.BackendManager.LodgingViewField.CreditCardNumber,
                Managers.Register.PaymentManager.DefaultPaymentInfo.CCNumber_Encrypted);

            this.SetSuperadmin(eventSessionId, false);
            ManagerSiteMgr.OpenEventDashboardUrl(eventId, eventSessionId);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.Reports);
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(Managers.Report.ReportManager.CommonReportType.RoomingList);
            ReportMgr.VerifyDecryptedCCNumberPresent();
            this.SetSuperadmin(eventSessionId, true);
            ReportMgr.VerifyEncryptedCCNumberPresent();
            ReportMgr.CloseReportPopupWindow();

            string customReportName = "VerifyCCVisibility";
            this.SetSuperadmin(eventSessionId, false);
            ManagerSiteMgr.DashboardMgr.CreateCustomReport(customReportName);
            ManagerSiteMgr.DashboardMgr.OpenCustomReport(customReportName);
            ReportMgr.VerifyDecryptedCCNumberPresent();
            this.SetSuperadmin(eventSessionId, true);
            ReportMgr.VerifyEncryptedCCNumberPresent();
            ReportMgr.CloseReportPopupWindow();
            this.SetSuperadmin(eventSessionId, false);

            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ReturnToList();
        }

        [Step]
        private void SetSuperadmin(string eventSessionId, bool enable)
        {
            ManagerSiteMgr.SetSuperadmin(eventSessionId, enable);
            UIUtilityProvider.UIHelper.RefreshPage();
        }
    }
}