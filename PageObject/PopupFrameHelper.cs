namespace RegOnline.RegressionTest.PageObject
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class PopupFrameHelper : Window
    {
        public static ButtonOrLink SaveAndStay = new ButtonOrLink("ctl00_btnSaveStay", LocateBy.Id);
        public static ButtonOrLink SaveAndNew = new ButtonOrLink("ctl00_btnSaveNew", LocateBy.Id);
        public static ButtonOrLink SaveAndClose = new ButtonOrLink("ctl00_btnSaveClose", LocateBy.Id);
        public static ButtonOrLink Cancel = new ButtonOrLink("ctl00_btnCancel", LocateBy.Id);

        public void SaveAndStay_Click()
        {
            SaveAndClose.WaitForDisplay();
            SaveAndStay.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void SaveAndNew_Click()
        {
            SaveAndNew.WaitForDisplay();
            SaveAndNew.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX(); 
            WaitForLoad();
        }

        public void SaveAndClose_Click()
        {
            SaveAndClose.WaitForDisplay();
            SaveAndClose.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void Cancel_Click()
        {
            Cancel.WaitForDisplay();
            Cancel.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad(); 
        }
    }
}
