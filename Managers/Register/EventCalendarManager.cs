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
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("viewList_0", LocateBy.Id);
                    break;

                case ViewBy.Location:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("viewList_1", LocateBy.Id);
                    break;

                case ViewBy.Month:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("viewList_2", LocateBy.Id);
                    break;

                case ViewBy.Day:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("viewList_3", LocateBy.Id);
                    break;

                case ViewBy.Category:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("viewList_4", LocateBy.Id);
                    break;

                default:
                    break;
            }
        }

        [Step]
        public void AddToCart(int agendaItemId)
        {
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//table[@id='dgCategory']//img[contains(@id, {0})]", agendaItemId), LocateBy.XPath);
            Utility.ThreadSleep(2);
        }

        [Step]
        public void OpenRegister()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnRegister2", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            AllowCookies();
        }
    }
}
