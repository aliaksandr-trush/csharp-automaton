namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Utilities;

    public class ChangeEventStatus
    {
        public void EventStatusChange(Event details, FormData.EventStatus status)
        {
            PageObject.PageObjectProvider.Manager.Dashboard.ChangeStatus_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.ChangeStatusFrame.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.ChangeStatusFrame.ChangeStatusNow_Set(true);
            PageObject.PageObjectProvider.Manager.Dashboard.ChangeStatusFrame.Status.SelectWithText(CustomStringAttribute.GetCustomString(status));
            PageObject.PageObjectProvider.Manager.Dashboard.ChangeStatusFrame.OK_Click();
        }
    }
}
