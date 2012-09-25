namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;

    public class BulkLoadCodesDefine : Frame
    {
        public BulkLoadCodesDefine(string name) : base(name) {}

        public Input CodesDefine = new Input("ctl00_cphDialog_txtBulkCodes", LocateBy.Id);

        public void SaveAndStay_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndStay_Click();
            SwitchToMain();
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
    }
}
