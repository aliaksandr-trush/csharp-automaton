namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    public class TestAccountWithGoodAndBadDataFixture : ExternalAuthenticationFixtureBase
    {
        string eventName = "XAuth_TestAccountWithBadAndGoodData";

        [TestCase(DataCollection.EventData_Common.XAuthType.ByEmail)]
        [TestCase(DataCollection.EventData_Common.XAuthType.ByEmailPassword)]
        [TestCase(DataCollection.EventData_Common.XAuthType.ByUserName)]
        [TestCase(DataCollection.EventData_Common.XAuthType.ByUserNamePassword)]
        [Test]
        public void TestAccountWithBadAndGoodData(DataCollection.EventData_Common.XAuthType type)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_UserDefined("xAuth");
            ManagerSiteMgr.DeleteEventByName(eventName);
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);

            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regType1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(type);

            string DefaultUserName = Managers.ManagerProvider.XAuthMgr.DefaultAccount_UserName;
            string DefaultEmail = Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email;
            string DefaultPassword = Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password;

            switch (type)
            {
                case DataCollection.EventData_Common.XAuthType.ByEmail:

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.RequiredEmail);
                    
                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("test@user.com12345");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.InvalidEmail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("bad@user.com");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(type);
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.Success);
                    break;

                case DataCollection.EventData_Common.XAuthType.ByEmailPassword:

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword(DefaultPassword);
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.RequiredEmail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail(DefaultEmail);
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.RequiredPassword);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.RequiredEmailPassword);
                    
                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("test@user.com12345");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword(DefaultPassword);
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.InvalidEmail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("test@user.com12345");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("badPassword");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.InvalidEmail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("bad@user.com");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword(DefaultPassword);
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("bad@user.com");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("badPassword");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail(DefaultEmail);
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("badPassword");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(type);
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.Success);
                    break;

                case DataCollection.EventData_Common.XAuthType.ByUserName:

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.RequiredUsername);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("badUserName");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(type);
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.Success);
                    break;

                case DataCollection.EventData_Common.XAuthType.ByUserNamePassword:

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword(DefaultPassword);
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.RequiredUsername);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName(DefaultUserName);
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.RequiredPassword);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.RequiredUsernamePassword);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("badUserName");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword(DefaultPassword);
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("badUserName");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("badPassword");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName(DefaultUserName);
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("badPassword");
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(type);
                    ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult.Success);
                    break;
            }
        }

        [Verify]
        private void ClickTestAndVerify(DataCollection.EventData_Common.TestAccountResult result)
        {
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyErrorMessages(result);
        }
    }
}
