namespace RegOnline.RegressionTest.Managers.Builder
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
            UIUtil.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
            UIUtil.DefaultProvider.SelectPopUpFrameByName(CustomFieldManager.FrameID);
        }

        public override void Cancel()
        {
            SelectThisFrame();
            UIUtil.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
            UIUtil.DefaultProvider.SelectPopUpFrameByName(CustomFieldManager.FrameID);
        }
    }
}
