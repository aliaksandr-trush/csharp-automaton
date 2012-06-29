namespace RegOnline.RegressionTest.Managers
{
    using RegOnline.RegressionTest.Managers.Backend;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Emails;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Managers.Report;

    public class ManagerProvider
    {
        public static ManagerSiteManager ManagerSiteMgr = new ManagerSiteManager();
        public static FormDetailManager BuilderMgr = new FormDetailManager();
        public static BackendManager BackendMgr = new BackendManager();
        public static RegisterManager RegisterMgr = new RegisterManager();
        public static ReportManager ReportMgr = new ReportManager();
        public static EmailManager EmailMgr = new EmailManager();
        public static DataHelper DataHelperTool = new DataHelper();
        public static XAuthManager XAuthMgr;

        public static void ResetManagers()
        {
            ManagerSiteMgr = new ManagerSiteManager();
            BuilderMgr = new FormDetailManager();
            BackendMgr = new BackendManager();
            RegisterMgr = new RegisterManager();
            ReportMgr = new ReportManager();
            EmailMgr = new EmailManager();
            DataHelperTool = new DataHelper();
        }
    }
}
