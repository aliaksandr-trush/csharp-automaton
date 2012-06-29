namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;

    [TestFixture]
    public class XAuthAndOtherRegTypeFeature : FixtureBase
    {
        private const string EventName = "XAuthFixture_XAuthAndOtherRegTypeFeature";
        private int eventId=0;
        private string eventSessionId;
       
        private void OpenEventBuilderPage()
        {
            if (eventId == 0 && eventSessionId == null)
            {
                ManagerSiteMgr.OpenLogin();
                ManagerSiteMgr.Login();
                ManagerSiteMgr.SelectFolder("xAuth");
                ManagerSiteMgr.DeleteEventByName(EventName);
                ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
                eventId = BuilderMgr.GetEventId();
                eventSessionId = BuilderMgr.GetEventSessionId();
                BuilderMgr.SetEventNameAndShortcut(EventName);
            }
            else
            {
                ManagerSiteMgr.OpenEventBuilderStartPage(eventId, eventSessionId);
            }
        }


        private void SetupAndEnableXauth()
        {
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(FormData.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            //Utilities.Utility.ThreadSleep(3);

            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();
        }

        private bool CreateRegistrant(RegisterManager.XAuthPersonalInfo user, string regTypeName)
        {
            RegisterMgr.OpenRegisterPage(eventId);
            RegisterMgr.Checkin(user.Email);
            if (RegisterMgr.IsRegTypeEditable(regTypeName))
                RegisterMgr.SelectRegType(regTypeName);
            else
                return false;
            RegisterMgr.Continue();
            RegisterMgr.EnterPassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.XAuth_VerifyPI(user);
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            return true;
        }

        [Verify]
        private void VerifyRegisterOnRegTypeWithLimitation(RegTypeManager.RegLimitType limitationType, string regTypeName, int limit)
        {
            //26
            int registerCount = 0;
            OpenEventBuilderPage();
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(regTypeName);
            BuilderMgr.RegTypeMgr.SetRegLimitOptions(limitationType, limit, "Sold Out");
            SetupAndEnableXauth();
            foreach (var user in RegisterMgr.TestAccounts.Values)
            {
                registerCount++;
                bool result = CreateRegistrant(user, regTypeName);

                if (registerCount > limit)
                {
                    Assert.AreEqual(false, result);
                    break;
                }
                else
                {
                    Assert.AreEqual(true, result);
                }
            }
        }

        [Test]
        public void TestXAuthAndOtherRegTypeFeature()
        {
            //26
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();
            VerifyRegisterOnRegTypeWithLimitation(RegTypeManager.RegLimitType.Individual, "LimitOnTotalNumberOfIndividuals", 1);

            //26.1
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();
            VerifyRegisterOnRegTypeWithLimitation(RegTypeManager.RegLimitType.Group, "LimitOnTotalNumberOfGroups", 1);
        }
    }
}
