namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class RegTypeFeeDefine : EventFeeDefine
    {
        public RegTypeFeeDefine(string name) : base(name) { }

        new public void AdjustRADWindowPositionAndResize()
        {
            PageObject.PageObjectHelper.AdjustRADWindowPosition("RadWindowWrapper_ctl00_dialog2", 20, 20);
            PageObject.PageObjectHelper.ResizeRADWindow(this.Name, 800, 1000);
        }
    }
}
