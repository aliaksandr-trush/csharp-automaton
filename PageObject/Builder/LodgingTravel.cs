namespace RegOnline.RegressionTest.PageObject.Builder
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class LodgingTravel : Window
    {
        public ButtonOrLink LTPageHeader = new ButtonOrLink("//*[text()='Add Lodging & Travel Page Header']", LocateBy.XPath);
        public HtmlEditor LTPageHeaderEditor = new HtmlEditor("dialog");
        public ButtonOrLink LTPageFooter = new ButtonOrLink("//*[text()='Add Lodging & Travel Page Footer']", LocateBy.XPath);
        public HtmlEditor LTPageFooterEditor = new HtmlEditor("dialog");

        public void SetLodgingStandardFieldVisible(FormData.LodgingStandardFields field, bool visible)
        {
            CheckBox LodgingStandardFieldVisible = new CheckBox(string.Format("ctl00_cph_chk{0}V", field.ToString()), LocateBy.Id);
            LodgingStandardFieldVisible.Set(visible);
        }

        public void SetLodgingStandardFieldRequired(FormData.LodgingStandardFields field, bool requied)
        {
            CheckBox LodgingStandardFieldRequired = new CheckBox(string.Format("ctl00_cph_chk{0}R", field.ToString()), LocateBy.Id);
            LodgingStandardFieldRequired.Set(requied);
        }

        public void LTPageHeader_Click()
        {
            this.LTPageHeader.WaitForDisplay();
            this.LTPageHeader.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void LTPageFooter_Click()
        {
            this.LTPageFooter.WaitForDisplay();
            this.LTPageFooter.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
