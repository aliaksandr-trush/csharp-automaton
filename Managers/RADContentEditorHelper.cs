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
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radEdit", LocateBy.Id);
                    WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                    break;
                case Mode.HTML:
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
                    WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                    break;
                case Mode.Preview:
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radPreview", LocateBy.Id);
                    WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                    break;
                default:
                    break;
            }
        }

        public void TypeHTMLTextarea(string content)
        {
            WebDriverUtility.DefaultProvider.Type("//textarea", content, LocateBy.XPath);
        }
    }
}
