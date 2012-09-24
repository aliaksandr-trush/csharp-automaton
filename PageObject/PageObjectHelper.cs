namespace RegOnline.RegressionTest.PageObject
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class PageObjectHelper
    {
        public static PopupFrameHelper PopupFrame_Helper = new PopupFrameHelper();

        public static void AllowCookie_Click()
        {
            Clickable Allow = new Clickable("//button[@class='ui-button ui-widget ui-corner-all ui-button-text-only dialog-allow-button']", LocateBy.XPath);

            if (Allow.IsPresent)
            {
                Utility.ThreadSleep(1);
                Allow.WaitForDisplay();
                Allow.Click();
            }
        }

        public static void NavigateTo(string url)
        {
            UIUtil.DefaultProvider.OpenUrl(url);
        }

        public static void AdjustRADWindowPosition(string locator_Id_RADWindowDiv, int leftPx, int topPx)
        {
            UIUtil.DefaultProvider.SwitchToMainContent();

            string js = string.Format(
                "var div = document.getElementById('{0}');div.style.left='{1}px';div.style.top='{2}px';",
                locator_Id_RADWindowDiv,
                leftPx,
                topPx);

            UIUtil.DefaultProvider.ExecuteJavaScript(js);
            Utility.ThreadSleep(1);
        }

        public static void ResizeRADWindow(string frameName, int widthPx, int heightPx)
        {
            UIUtil.DefaultProvider.SwitchToMainContent();

            string js = string.Format(
                "var frame = document.getElementsByName('{0}')[0];frame.style.width='{1}px';frame.style.height='{2}px';",
                frameName,
                widthPx,
                heightPx);

            UIUtil.DefaultProvider.ExecuteJavaScript(js);
            Utility.ThreadSleep(1);
        }

        public static bool IsTextPresent(string text)
        {
            Label label = new Label(string.Format("//*[contains(text(),'{0}')]", text), LocateBy.XPath);
            return label.IsPresent;
        }

        public static void SelectTopWindow()
        {
            UIUtil.DefaultProvider.SelectTopWindow();
        }

        public static void ClickConfirmation(DataCollection.FormData.ConfirmationOptions option)
        {
            if (option == DataCollection.FormData.ConfirmationOptions.OK)
            {
                UIUtil.DefaultProvider.GetConfirmation();
                Utility.ThreadSleep(3);
                UIUtil.DefaultProvider.WaitForAJAXRequest();
                UIUtil.DefaultProvider.WaitForPageToLoad();
            }

            if (option == DataCollection.FormData.ConfirmationOptions.Cancel)
            {
                UIUtil.DefaultProvider.ChooseCancelOnNextConfirmation();
                Utility.ThreadSleep(3);
                UIUtil.DefaultProvider.WaitForAJAXRequest();
                UIUtil.DefaultProvider.WaitForPageToLoad();
            }
        }

        public static string GetSessionId()
        {
            return UIUtil.DefaultProvider.GetQueryStringValue("SessionId");
        }
    }
}
