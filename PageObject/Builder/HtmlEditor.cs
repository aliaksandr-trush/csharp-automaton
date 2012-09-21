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
        public Clickable HtmlMode = new Clickable("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
        public Input Content = new Input("//textarea", LocateBy.XPath);
        #endregion

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
            PageObjectHelper.PopupFrame_Helper.SaveAndStay_Click();
        }

        public void SaveAndClose_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndClose_Click();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            PageObjectHelper.PopupFrame_Helper.Cancel_Click();
            SwitchToMain();
        }
        #endregion
    }
}
