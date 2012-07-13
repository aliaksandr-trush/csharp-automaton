namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;

    public class LaunchKiosk
    {
        public void LaunchOnsiteKiosk(OnsiteKiosk kiosk)
        {
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.SelfKiosk_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.LaunchSelfKiosk.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.LaunchSelfKiosk.RequireAuthentication.Set(kiosk.RequireAuthentication);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.LaunchSelfKiosk.AllowOnsiteReg.Set(kiosk.AllowOnsiteReg);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.LaunchSelfKiosk.LaunchKiosk_Click();

            PageObject.PageObjectHelper.SelectTopWindow();
        }
    }
}
