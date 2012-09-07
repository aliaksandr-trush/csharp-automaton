﻿namespace RegOnline.RegressionTest.Managers.Builder
{
    ////using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.UIUtility;

    public class CFMultiChoiceItemManager : MultiChoiceItemManagerBase
    {
        public override string FrameID
        {
            get
            {
                return "dialog2";
            }
        }

        public override void SaveAndClose()
        {
            SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(CustomFieldManager.FrameID);
        }

        public override void Cancel()
        {
            SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(CustomFieldManager.FrameID);
        }
    }
}
