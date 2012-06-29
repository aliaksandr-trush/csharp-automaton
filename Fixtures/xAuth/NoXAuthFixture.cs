namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;

    [TestFixture]
    public class NoXAuthFixture : FixtureBase
    {
        private const string EventName = "NoXAuthTest";
        private const string EventNameExistEmail = "NoXAuthTest-ExistingEmail";
        private int eventID;
        private string eventSessionId;

        [Test]
        public void NoXAuthTest()
        {
            string existingEmailAddress = null;
            string emailAddress = null;

            #region create a event, active the event and don't delete test regs

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder("xAuth");
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.DeleteEventByName(EventNameExistEmail);

            //create existing email address event
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventNameExistEmail);
            BuilderMgr.SaveAndClose();

            //register this event and save the email address as existingEmailAddress
            RegisterMgr.OpenRegisterPage(this.eventID);
            existingEmailAddress = RegisterMgr.ComposeUniqueEmailAddress();
            RegisterMgr.CheckinWithEmail(existingEmailAddress);
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            int regId1 = RegisterMgr.GetRegistrationIdOnConfirmationPage();

            //active event and don't delete test regs
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            this.eventSessionId = BuilderMgr.GetEventSessionId();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, eventSessionId);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.IsCheckRemoveTestRegs(true);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.RemoveTestRegs(false);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.SelectPricingOption(0);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.CheckAgreementForTermsOfService(true);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.TypeInitialing("YES");
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();

            //check the regs didn't delete
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.Reports);
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(Managers.Report.ReportManager.CommonReportType.RegistrantList);
            ReportMgr.VerifyIsTestRegistrationOnAttendeeReport(regId1, true);
            ReportMgr.CloseReportPopupWindow();

            #endregion

            #region create a event, active the event and delete test regs

            //create a new event
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            //ManagerSiteMgr.Dashboard.ReturnToList();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder("xAuth");
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.SaveAndClose();

            //register this event save the email address as emailAddress, later we will update it
            RegisterMgr.OpenRegisterPage(this.eventID);
            emailAddress = RegisterMgr.ComposeUniqueEmailAddress();
            RegisterMgr.CheckinWithEmail(emailAddress);
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            int regId2 = RegisterMgr.GetRegistrationIdOnConfirmationPage();

            //register this event using the existingEmailAddress
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.CheckinWithEmail(existingEmailAddress);
            RegisterMgr.Continue();
            if (!RegisterMgr.OnLoginPage())
            {
                Assert.Fail("Not on login page!");
            }
            RegisterMgr.VerifyForgotYourPasswordLinkVisibility(true);
            RegisterMgr.TypeLoginPagePassword();
            RegisterMgr.Continue();
            if (!RegisterMgr.OnPersonalInfoPage())
            {
                Assert.Fail("Not on Personal Info page!");
            }
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            int regId3 = RegisterMgr.GetRegistrationIdOnConfirmationPage();

            //register the event using the emailAddress (who registered the event as the first person), this time we will update it
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.CheckinWithEmail(emailAddress);
            RegisterMgr.Continue();
            if (!RegisterMgr.OnLoginPage())
            {
                Assert.Fail("Not on login page!");
            }
            RegisterMgr.VerifyForgotYourPasswordLinkVisibility(true);
            RegisterMgr.TypeLoginPagePassword();
            RegisterMgr.Continue();
            if (!RegisterMgr.OnAttendeeCheckPage())
            {
                Assert.Fail("Not on AttendeeCheck page!");
            }
            RegisterMgr.ClickEditPersonalInformationLink(0);
            RegisterMgr.Continue();
            RegisterMgr.VerifyHasSubstituteLink();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //active event and delete the regs
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            this.eventSessionId = BuilderMgr.GetEventSessionId();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, eventSessionId);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.RemoveTestRegs(true);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.SelectPricingOption(0);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.CheckAgreementForTermsOfService(true);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.TypeInitialing("YES");
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();

            //check the regs were deleted
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.Reports);
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(Managers.Report.ReportManager.CommonReportType.RegistrantList);
            ReportMgr.VerifyReportTableRowCount(0);
            ReportMgr.CloseReportPopupWindow();

            #endregion
        }
    }
}
