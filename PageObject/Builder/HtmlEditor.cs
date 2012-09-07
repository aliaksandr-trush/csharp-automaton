namespace RegOnline.RegressionTest.PageObject.Builder
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class HtmlEditor : Frame
    {
        public HtmlEditor(string name) : base(name) { }
        public HtmlEditor(int index) : base(index) { }

        #region WebElements
        public ButtonOrLink HtmlMode = new ButtonOrLink("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
        public TextBox Content = new TextBox("//textarea", LocateBy.XPath);
        #endregion

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        #region Basic Actions
        public void HtmlMode_Click()
        {
            this.HtmlMode.WaitForDisplay();
            this.HtmlMode.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            WebDriverUtility.DefaultProvider.SelectIFrameOnCurrentIFrame(1);
        }

        public void Content_Type(string content)
        {
            this.Content.Type(content);
            SwitchToMain();
            SelectByName();
        }

        public void SaveAndStay_Click()
        {
            popupFrameHelper.SaveAndStay_Click();
        }

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            popupFrameHelper.Cancel_Click();
            SwitchToMain();
        }
        #endregion
    }
}
