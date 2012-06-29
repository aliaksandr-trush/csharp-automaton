namespace RegOnline.RegressionTest.Fixtures.Manager
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.UIUtility;

	[TestFixture]
	[Category(FixtureCategory.Regression)]
	public class SearchFixture : FixtureBase
	{
		//TODO: actually make these worthwhile, right now they do almost nothing at all...

		[Test]
        [Category(Priority.Three)]
        [Description("364")]
		public void EventSearch()
		{
			if (!UIUtilityProvider.UIHelper.UrlContainsPath("localhost"))
			{
				System.Threading.Thread.Sleep(1000);
			}

			ManagerSiteMgr.OpenLogin();

			// Login
			ManagerSiteMgr.Login();

			// Click the event search
			ManagerSiteMgr.SelectQuickSearchMode(ManagerSiteManager.SearchModes.Event);

			// Type the search string
			ManagerSiteMgr.EnterSearchText("CreateRegistrationFixture - WithoutRegTypes");

			// Click search
			ManagerSiteMgr.ClickQuickSearchGoButton();

			Assert.IsFalse(UIUtilityProvider.UIHelper.IsTextPresent("No records to display."));
		}

		[Test]
        [Category(Priority.Three)]
        [Description("363")]
		public void AttendeeSearch()
		{
			ManagerSiteMgr.OpenLogin();

			// Login
			ManagerSiteMgr.Login("sprint08", "sprint08");

			// Click the event search
			ManagerSiteMgr.SelectQuickSearchMode(ManagerSiteManager.SearchModes.Attendee);

			// Type the search string
			// TODO: Make this less brittle
			ManagerSiteMgr.EnterSearchText("Ward");

			// Click search
			ManagerSiteMgr.ClickQuickSearchGoButton();

			Assert.IsFalse(UIUtilityProvider.UIHelper.IsTextPresent("No records to display."));
		}
	}
}