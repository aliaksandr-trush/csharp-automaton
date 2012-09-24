namespace RegOnline.RegressionTest.Managers.Register
{
    ////using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public class EventCalendarManager : ManagerBase
    {
        public enum ViewBy
        {
            Calendar,
            Location,
            Month,
            Day,
            Category
        }

        [Step]
        public void SelectViewBy(ViewBy viewBy)
        {
            switch (viewBy)
            {
                case ViewBy.Calendar:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("viewList_0", LocateBy.Id);
                    break;

                case ViewBy.Location:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("viewList_1", LocateBy.Id);
                    break;

                case ViewBy.Month:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("viewList_2", LocateBy.Id);
                    break;

                case ViewBy.Day:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("viewList_3", LocateBy.Id);
                    break;

                case ViewBy.Category:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("viewList_4", LocateBy.Id);
                    break;

                default:
                    break;
            }
        }

        [Step]
        public void AddToCart(int agendaItemId)
        {
            Utility.ThreadSleep(2);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format("//table[@id='dgCategory']//img[contains(@id, {0})]", agendaItemId), LocateBy.XPath);
            Utility.ThreadSleep(2);
        }

        [Step]
        public void OpenRegister()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("btnRegister2", LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
            AllowCookies();
        }
    }
}
