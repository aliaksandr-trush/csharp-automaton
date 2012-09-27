namespace RegOnline.RegressionTest.UIUtility
{
    public enum LocateBy
    {
        XPath,
        Id,
        LinkText,
        PartialLinkText,
        CssSelector,
        ClassName,
        Name,
        TagName
    }

    internal class UIUtilHelper
    {
        internal const string ScreenshotFileNameFormat = "Screenshot_{0}.png";
        internal const string SaveAndNewButtonLocator = "ctl00_btnSaveNew";
        internal const string SaveAndStayButtonLocator = "ctl00_btnSaveStay";
        internal const string SaveAndCloseButtonLocator = "ctl00_btnSaveClose";
        internal const string CancelButtonLocator = "ctl00_btnCancel";
        internal const string AdvancedHeaderLocator = "togglebsAdvanced";
        internal const string AdvancedSectionDIVLocator = "bsAdvanced_ADV";
        internal const string RADNumericLocatorSuffix_text = "_text";
        internal const string RADNumericLocatorSuffix_value = "_Value";
        internal const int DefaultTimeoutMilliSeconds = 30000;

        internal enum UnexpectedAlertException
        {
            Sys_WebForms_PageRequestManagerServerErrorException
        }
    }
}
