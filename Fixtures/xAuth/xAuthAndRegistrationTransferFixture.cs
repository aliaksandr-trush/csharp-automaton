namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.UIUtility;

    ////[TestFixture]
    public class xAuthAndRegistrationTransferFixture : FixtureBase
    {
        private const string EventTransferFrom = "xAuthAndRegistrationTransferFrom";
        private const string EventTransferTo = "xAuthAndRegistrationTransferTo";
        private const string NoXAuthEvent = "noXAuthEvent";
        private int EventIDTransferFrom, EventIDTransferTo, NoXAuthEventID;
        private string eventSessionId;

        // Transferring is no longer allowed for xAuth
        ////[Test]
        public void xAuthAndRegistrationTransferTest()
        {
            Managers.ManagerProvider.XAuthMgr.ApprovedXAuthRoleForCustomer(false);
            Managers.ManagerProvider.XAuthMgr.RemoveLiveXAuthEventForCustomer();

            #region create xAuth event will be transfered to another xauth event

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();

            List<int> eventIds = ManagerSiteMgr.GetEventIds(EventTransferFrom);

            foreach (int id in eventIds)
            {
                DataHelperTool.ChangeAllRegsToTestForEvent(id);
            }

            ManagerSiteMgr.DeleteEventByName(EventTransferFrom);

            //create an xauth event and add reg type 1
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            EventIDTransferFrom = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventTransferFrom);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            //update xAuth type as EmailPassword
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyErrorMessages(FormData.TestAccountResult.Success);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(2);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            //create reg type 2 no xauth
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype2");
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            Managers.ManagerProvider.XAuthMgr.ApprovedXAuthRoleForCustomer(true);

            //activate the event
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            this.eventSessionId = BuilderMgr.GetEventSessionId();
            ManagerSiteMgr.OpenEventDashboardUrl(EventIDTransferFrom, eventSessionId);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.CheckAgreementForTermsOfService(true);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.TypeInitialing("123");
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();

            #endregion

            #region register xauth event

            //register xauth event regtype1:xAuth regtype
            RegisterMgr.OpenRegisterPage(EventIDTransferFrom);
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            int regId1 = RegisterMgr.GetRegistrationIdOnConfirmationPage();

            //register regtype2:no xAuth regtype
            RegisterMgr.OpenRegisterPage(EventIDTransferFrom);
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.Continue();
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            int regId2 = RegisterMgr.GetRegistrationIdOnConfirmationPage();

            //register regtype2:no xAuth regtype
            RegisterMgr.OpenRegisterPage(EventIDTransferFrom);
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.Continue();
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            int regId3 = RegisterMgr.GetRegistrationIdOnConfirmationPage();

            //go to Attendee Info page
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            //Transfer is not visible
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, regId1);
            BackendMgr.VerifyMenuOptionPresent(Managers.Backend.BackendManager.MoreOption.Transfer, false);
            //Transfer is visible
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, regId2);
            BackendMgr.VerifyMenuOptionPresent(Managers.Backend.BackendManager.MoreOption.Transfer, true);
            //Transfer is visible
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, regId3);
            BackendMgr.VerifyMenuOptionPresent(Managers.Backend.BackendManager.MoreOption.Transfer, true);

            #endregion

            #region create another xauth event

            //create a no xauth event and add reg type 1
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();

            eventIds = ManagerSiteMgr.GetEventIds(EventTransferFrom);

            foreach (int id in eventIds)
            {
                DataHelperTool.ChangeAllRegsToTestForEvent(id);
            }

            ManagerSiteMgr.DeleteEventByName(EventTransferTo);
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            // Get event id
            EventIDTransferTo = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventTransferTo);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            //create reg type 2 no xauth
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype2");
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            //activate the event
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            this.eventSessionId = BuilderMgr.GetEventSessionId();
            ManagerSiteMgr.OpenEventDashboardUrl(EventIDTransferTo, eventSessionId);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.CheckAgreementForTermsOfService(true);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.TypeInitialing("123");
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();

            #endregion

            #region create no xAuth event

            //create a no xauth event and add reg type 1
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();

            eventIds = ManagerSiteMgr.GetEventIds(EventTransferFrom);

            foreach (int id in eventIds)
            {
                DataHelperTool.ChangeAllRegsToTestForEvent(id);
            }

            ManagerSiteMgr.DeleteEventByName(NoXAuthEvent);
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            // Get event id
            NoXAuthEventID = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(NoXAuthEvent);
            BuilderMgr.SaveAndClose();

            //activate the event
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            this.eventSessionId = BuilderMgr.GetEventSessionId();
            ManagerSiteMgr.OpenEventDashboardUrl(NoXAuthEventID, eventSessionId);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.CheckAgreementForTermsOfService(true);
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.TypeInitialing("123");
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();

            //register no xauth event regtype1:no xAuth regtype
            RegisterMgr.OpenRegisterPage(NoXAuthEventID);
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Continue();
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            int regId4 = RegisterMgr.GetRegistrationIdOnConfirmationPage();

            //go to Attendee Info page
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            //Transfer is visible
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, regId4);
            BackendMgr.VerifyMenuOptionPresent(Managers.Backend.BackendManager.MoreOption.Transfer, true);

            #endregion

            #region Transfer

            //transfer regId2 to xauth event
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, regId2);
            BackendMgr.VerifyRegTypeExistInTransferAttendee(EventIDTransferTo, EventTransferTo, "regtype1", false);

            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, regId2);
            int newRegistrationIdForXAuth = BackendMgr.TransferAttendee(EventIDTransferTo, EventTransferTo);
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
            Assert.Greater(newRegistrationIdForXAuth, 0);

            //transfer regId3 to no xauth event
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, regId3);
            int newRegistrationIdForNoXAuth = BackendMgr.TransferAttendee(NoXAuthEventID, NoXAuthEvent);
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
            Assert.Greater(newRegistrationIdForNoXAuth, 0);

            #endregion

            Managers.ManagerProvider.XAuthMgr.ApprovedXAuthRoleForCustomer(false);
            Managers.ManagerProvider.XAuthMgr.RemoveLiveXAuthEventForCustomer();
        }
    }
}
