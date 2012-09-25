namespace RegOnline.RegressionTest.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.UIUtility;

    public class RADContentEditorHelper
    {
        public enum Mode
        {
            Edit,
            HTML,
            Preview
        }

        public void SwitchMode(Mode mode)
        {
            switch (mode)
            {
                case Mode.Edit:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radEdit", LocateBy.Id);
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                case Mode.HTML:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                case Mode.Preview:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radPreview", LocateBy.Id);
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                default:
                    break;
            }
        }

        public void TypeHTMLTextarea(string content)
        {
            UIUtil.DefaultProvider.Type("//textarea", content, LocateBy.XPath);
        }
    }
}
