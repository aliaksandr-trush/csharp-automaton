namespace RegOnline.RegressionTest.PageObject.Manager
{
    public class PageObjectProvider
    {
        public SignIn SignIn = new SignIn();
        public Events Events = new Events();
        public Dashboard.Dashboard Dashboard = new Dashboard.Dashboard();
        public XAuth XAuth = new XAuth();
        public SSOBase SSOBase = new SSOBase("plain");
    }
}
