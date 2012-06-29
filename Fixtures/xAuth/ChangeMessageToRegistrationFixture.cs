namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    public class ChangeMessageToRegistrationFixture : FixtureBase
    {
        string eventName = "XAuth_Change_MessageToRegistration";
        string customizedMessageToRegistration = "This is modified message with GUID: {0}";

        [Test]
        public void ModifyMessageToRegistration()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder("xAuth");
            ManagerSiteMgr.DeleteEventByName(eventName);
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            int eventID = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(eventName);

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regType1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.TypeForgetPasswordUrl("http://www.regonline.com");
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByEmailPassword);
            // Change the MessageToRegistration
            string modifiedMessage = string.Format(customizedMessageToRegistration, Guid.NewGuid().ToString());
            Managers.ManagerProvider.XAuthMgr.TypeMessageToRegistration(modifiedMessage);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();

            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyMessageToRegistrant(modifiedMessage);
        }
    }
}
