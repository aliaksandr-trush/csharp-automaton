namespace RegOnline.RegressionTest.PageObject.Builder
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class EmailEditor : Frame
    {
        public EmailEditor(string name) : base(name) { }
        public EmailEditor(int index) : base(index) { }

        public ButtonOrLink HtmlMode = new ButtonOrLink("ctl00_cphDialog_ucEmailEditor_ucContent_radHtml", LocateBy.Id);
        public TextBox Content = new TextBox("//textarea", LocateBy.XPath);

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
