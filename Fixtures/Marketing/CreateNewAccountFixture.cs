namespace RegOnline.RegressionTest.Fixtures.Marketing
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class CreateNewAccountFixture : FixtureBase
    {
        [Test]
        [Category(Priority.One)]
        [Description("400")]
        //[Ignore("Marketing changes so frequently that it is meaningless to run automated test against it.")]
        public void CreateNewAccount()
        {
            string username = System.Guid.NewGuid().ToString();
            username = username.Replace("-", "");
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.CreateNewAccount(username, ConfigurationProvider.XmlConfig.AccountConfiguration.Password, "US Dollar");
            
            string EventTitle = "New Account Event";
            BuilderMgr.SetEventNameAndShortcut(EventTitle);
            BuilderMgr.SaveAndClose();

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login(username, ConfigurationProvider.XmlConfig.AccountConfiguration.Password);
            ManagerSiteMgr.GetFirstEventId(EventTitle);
        }
    }
}
