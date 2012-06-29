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
            UIUtilityProvider.UIHelper.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(CustomFieldManager.FrameID);
        }

        public override void Cancel()
        {
            SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(CustomFieldManager.FrameID);
        }
    }
}
