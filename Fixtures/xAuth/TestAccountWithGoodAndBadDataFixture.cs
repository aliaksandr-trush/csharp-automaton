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

        [TestCase(FormData.XAuthType.ByEmail)]
        [TestCase(FormData.XAuthType.ByEmailPassword)]
        [TestCase(FormData.XAuthType.ByUserName)]
        [TestCase(FormData.XAuthType.ByUserNamePassword)]
        [Test]
        public void TestAccountWithBadAndGoodData(FormData.XAuthType type)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder("xAuth");
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
                case FormData.XAuthType.ByEmail:

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("");
                    ClickTestAndVerify(FormData.TestAccountResult.RequiredEmail);
                    
                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("test@user.com12345");
                    ClickTestAndVerify(FormData.TestAccountResult.InvalidEmail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("bad@user.com");
                    ClickTestAndVerify(FormData.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(type);
                    ClickTestAndVerify(FormData.TestAccountResult.Success);
                    break;

                case FormData.XAuthType.ByEmailPassword:

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword(DefaultPassword);
                    ClickTestAndVerify(FormData.TestAccountResult.RequiredEmail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail(DefaultEmail);
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("");
                    ClickTestAndVerify(FormData.TestAccountResult.RequiredPassword);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("");
                    ClickTestAndVerify(FormData.TestAccountResult.RequiredEmailPassword);
                    
                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("test@user.com12345");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword(DefaultPassword);
                    ClickTestAndVerify(FormData.TestAccountResult.InvalidEmail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("test@user.com12345");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("badPassword");
                    ClickTestAndVerify(FormData.TestAccountResult.InvalidEmail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("bad@user.com");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword(DefaultPassword);
                    ClickTestAndVerify(FormData.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail("bad@user.com");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("badPassword");
                    ClickTestAndVerify(FormData.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestEmail(DefaultEmail);
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("badPassword");
                    ClickTestAndVerify(FormData.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(type);
                    ClickTestAndVerify(FormData.TestAccountResult.Success);
                    break;

                case FormData.XAuthType.ByUserName:

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("");
                    ClickTestAndVerify(FormData.TestAccountResult.RequiredUsername);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("badUserName");
                    ClickTestAndVerify(FormData.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(type);
                    ClickTestAndVerify(FormData.TestAccountResult.Success);
                    break;

                case FormData.XAuthType.ByUserNamePassword:

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword(DefaultPassword);
                    ClickTestAndVerify(FormData.TestAccountResult.RequiredUsername);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName(DefaultUserName);
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("");
                    ClickTestAndVerify(FormData.TestAccountResult.RequiredPassword);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("");
                    ClickTestAndVerify(FormData.TestAccountResult.RequiredUsernamePassword);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("badUserName");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword(DefaultPassword);
                    ClickTestAndVerify(FormData.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName("badUserName");
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("badPassword");
                    ClickTestAndVerify(FormData.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.TypeTestUserName(DefaultUserName);
                    Managers.ManagerProvider.XAuthMgr.TypeTestPassword("badPassword");
                    ClickTestAndVerify(FormData.TestAccountResult.AuthenticateFail);

                    Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(type);
                    ClickTestAndVerify(FormData.TestAccountResult.Success);
                    break;
            }
        }

        [Verify]
        private void ClickTestAndVerify(FormData.TestAccountResult result)
        {
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyErrorMessages(result);
        }
    }
}
