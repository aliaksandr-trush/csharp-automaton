namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;

    [TestFixture]
    public class InvitationsFixture : ExternalAuthenticationFixtureBase
    {
        private const string EventName = "InvitationsCodeTest";
        private const string EventNameListTest = "InvitationsListTest";
        private int eventID;

        [Test]
        public void InvitationsTest()
        {
            string invitationCode = "Invitation";
            string invitationList = "Invitation List";

            if (Configuration.ConfigurationProvider.XmlConfig.AccountConfiguration.XAuthVersion == "New")
            {
                invitationList = "Invitations";
            }

            #region test Invitation Code AND xAuth

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventName);

            //add Invitation Code
            BuilderMgr.ClickStartPageEventAdvancedSettings();
            BuilderMgr.AdvancedSettingsMgr.SetInvitationCodeAndList(invitationCode, "");
            BuilderMgr.AdvancedSettingsMgr.ClickSaveAndClose();

            //add regtype
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
            BuilderMgr.SaveAndClose();

            //a new register for the customer, register this event
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.TypeInvitationCode(invitationCode);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            #endregion

            #region test Invitation List AND xAuth

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            ManagerSiteMgr.DeleteEventByName(EventNameListTest);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(EventNameListTest);
            BuilderMgr.SaveAndStay();
            this.eventID = BuilderMgr.GetEventId();

            //add Invitation Code
            BuilderMgr.ClickStartPageEventAdvancedSettings();
            BuilderMgr.AdvancedSettingsMgr.SetInvitationCodeAndList("", invitationList);
            BuilderMgr.AdvancedSettingsMgr.ClickSaveAndClose();

            //add regtype
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
            BuilderMgr.SaveAndClose();

            //a new register for the customer, register this event
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            string[] err = RegisterMgr.GetErrorMessages();
            Utilities.VerifyTool.VerifyValue("The email address you entered is invalid for this event. If you feel that this is an error, click here to request further assistance.", 
                err[0], "Error: {0}"); 
            //RegisterMgr.TypeLoginPagePassword(BuilderMgr.RegType.XAuth.DefaultAccount_Password);
            //RegisterMgr.Continue();
            //RegisterMgr.Continue();
            //RegisterMgr.FinishRegistration();
            //RegisterMgr.ConfirmRegistration();

            #endregion
        }
    }
}
