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
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radEdit", LocateBy.Id);
                    UIUtilityProvider.UIHelper.WaitForAJAXRequest();
                    break;
                case Mode.HTML:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
                    UIUtilityProvider.UIHelper.WaitForAJAXRequest();
                    break;
                case Mode.Preview:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radPreview", LocateBy.Id);
                    UIUtilityProvider.UIHelper.WaitForAJAXRequest();
                    break;
                default:
                    break;
            }
        }

        public void TypeHTMLTextarea(string content)
        {
            UIUtilityProvider.UIHelper.Type("//textarea", content, LocateBy.XPath);
        }
    }
}
