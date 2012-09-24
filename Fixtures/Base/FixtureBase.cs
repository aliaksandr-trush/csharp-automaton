namespace RegOnline.RegressionTest.Fixtures.Base
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Backend;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Emails;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Managers.Report;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public abstract class FixtureBase
    {
        protected struct Priority
        {
            public const string One = "P1";
            public const string Two = "P2";
            public const string Three = "P3";
            public const string Four = "P4";
            public const string Five = "P5";
        }

        protected struct FixtureCategory
        {
            public const string Regression = "Regression";
            public const string XAuth = "XAuth";
            public const string SSO = "SSO";
        }

        protected static ManagerSiteManager ManagerSiteMgr;
        protected static FormDetailManager BuilderMgr;
        protected static BackendManager BackendMgr;
        protected static RegisterManager RegisterMgr;
        protected static ReportManager ReportMgr;
        protected static EmailManager EmailMgr;
        protected static DataHelper DataHelperTool;

        /// <summary>
        /// Whether browser should be opened for a fixture
        /// </summary>
        protected bool RequiresBrowser { get; set; }

        public FixtureBase()
        {
            RequiresBrowser = true;
        }

        private void ResetManagers()
        {
            ManagerSiteMgr = new ManagerSiteManager();
            BuilderMgr = new FormDetailManager();
            BackendMgr = new BackendManager();
            RegisterMgr = new RegisterManager();
            ReportMgr = new ReportManager();
            EmailMgr = new EmailManager();
            DataHelperTool = new DataHelper();
        }

        [SetUp]
        public void SetUp()
        {
            Utility.ThreadSleep(1);

            ConfigReader.DefaultProvider.ReloadAllConfiguration();

            if (RequiresBrowser)
            {
                UIUtil.DefaultProvider.Initialize();
            }

            this.ResetManagers();

            if (ConfigReader.DefaultProvider.AccountConfiguration.XAuthVersion == "Old")
            {
                ManagerProvider.XAuthMgr = new Managers.Manager.XAuthManager(Managers.Manager.XAuthManager.XAuthVersion.Old);
            }
            else
            {
                ManagerProvider.XAuthMgr = new Managers.Manager.XAuthManager(Managers.Manager.XAuthManager.XAuthVersion.New);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (RequiresBrowser)
            {
                if (ConfigReader.DefaultProvider.AllConfiguration.Browsers.DirectStartup)
                {
                    UIUtil.DefaultProvider.CaptureScreenshot();
                }

                Utility.ThreadSleep(1);
                UIUtil.DefaultProvider.Exit();
            }
        }
    }
}
