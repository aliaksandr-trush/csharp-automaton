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
        public void CreateNewAccount()
        {
            string username = System.Guid.NewGuid().ToString();
            username = username.Replace("-", "");
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.CreateNewAccount(username, ConfigReader.DefaultProvider.AccountConfiguration.Password, "US Dollar");
            
            string EventTitle = "New Account Event";
            BuilderMgr.SetEventNameAndShortcut(EventTitle);
            BuilderMgr.SaveAndClose();

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login(username, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            ManagerSiteMgr.SkipEmailValidation();
            ManagerSiteMgr.GetFirstEventId(EventTitle);
        }
    }
}
