﻿namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;

    public class BulkLoadCodesDefine : Frame
    {
        public BulkLoadCodesDefine(string name) : base(name) {}

        public TextBox CodesDefine = new TextBox("ctl00_cphDialog_txtBulkCodes", LocateBy.Id);

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        public void SaveAndStay_Click()
        {
            popupFrameHelper.SaveAndStay_Click();
            SwitchToMain();
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
    }
}
