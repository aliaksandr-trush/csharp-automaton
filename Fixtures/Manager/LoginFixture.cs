namespace RegOnline.RegressionTest.Fixtures.Manager
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
	[Category(FixtureCategory.Regression)]
    public class LoginFixture : FixtureBase
	{
		[Test]
        [Category(Priority.Three)]
        [Description("702")]
        public void TestInvalidLogin()
		{
            ManagerSiteMgr.OpenLogin();

			// Try an unsuccessful login
			ManagerSiteMgr.Login("bad22", "bad23");

			// Verify we are still on the login page
			ManagerSiteMgr.VerifyInvalidLogin();

			// Now try for 2 more invalid logins to trigger the captcha
			for (int i = 0; i < 2; i++)
			{
				ManagerSiteMgr.Login("bad22", "bad23");
			}

			// Verify captcha is visible
			ManagerSiteMgr.VerifyCaptchaVisible();

            UIUtility.UIUtil.DefaultProvider.ClearCookiesAndRestart();
			// Would be nice to actually log in now with the captcha value but 
			// that's sort of the point of captcha is to prevent this from happening.
		}

		[Test]
        [Category(Priority.Three)]
        [Description("701")]
        public void TestSuccessfullLogin()
        {
            ManagerSiteMgr.OpenLogin();

            // Login
            ManagerSiteMgr.Login();

            // Verify we are in manager
            ManagerSiteMgr.VerifyManager();
        }

		[Test]
        [Category(Priority.Three)]
        [Description("703")]
        public void TestForgotPassword()
		{
            ManagerSiteMgr.OpenLogin();

			// Enter a valid username
			ManagerSiteMgr.SelectAndEnterForgotPassword("RegressionTester");

			// Verify message
            ManagerSiteMgr.VerifyErrorMessage(Managers.Manager.ManagerSiteManager.ErrorMessage.ForgotPasswordEmailSent);

			// Enter an invalid username
			ManagerSiteMgr.SelectAndEnterForgotPassword("321456789au");

			// Verify message
            ManagerSiteMgr.VerifyErrorMessage(Managers.Manager.ManagerSiteManager.ErrorMessage.ForgotPasswordEmailSent);
		}

		[Test]
        [Category(Priority.Three)]
        [Description("704")]
        public void TestForgotLogin()
		{
            ManagerSiteMgr.OpenLogin();

			// Enter a valid email address
			ManagerSiteMgr.SelectAndEnterForgotLogin("regression@regonline.com");
			
			// Verify message
            ManagerSiteMgr.VerifyErrorMessage(Managers.Manager.ManagerSiteManager.ErrorMessage.ForgotLoginEmailSent);

            // Enter an invalid email address
			ManagerSiteMgr.SelectAndEnterForgotLogin("regression_bad22@regonline.com");

			// Verify message
            ManagerSiteMgr.VerifyErrorMessage(Managers.Manager.ManagerSiteManager.ErrorMessage.ForgotLoginEmailSent);
		}

        [Test]
        [Category(Priority.Three)]
        [Description("1279")]
        public void TestLoginLogoutLogin()
        {
            ManagerSiteMgr.OpenLogin();

            // Login
            ManagerSiteMgr.Login();

            // Verify we are in manager
            ManagerSiteMgr.VerifyManager();

            // Logout
            ManagerSiteMgr.Logout();

            // Login
            ManagerSiteMgr.Login();

            // Verify we're still in manager.
            ManagerSiteMgr.VerifyManager();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1280")]
        public void TestLoginClickOnEventLogoutLogin()
        {
            ManagerSiteMgr.OpenLogin();

            // Login
            ManagerSiteMgr.Login();

            // Verify we are in manager
            ManagerSiteMgr.VerifyManager();

            // Click on the first event
            ManagerSiteMgr.ClickOnFirstEvent();

            // Logout
            ManagerSiteMgr.Logout();

            // Login
            ManagerSiteMgr.Login();

            // Verify we're still in manager.
            ManagerSiteMgr.VerifyManager();
        }
	}
}
