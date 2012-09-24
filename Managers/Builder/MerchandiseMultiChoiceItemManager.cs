namespace RegOnline.RegressionTest.Managers.Builder
{
    using RegOnline.RegressionTest.Managers;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class MerchandiseMultiChoiceItemManager : ManagerBase
    {
        public const string FrameID = "dialog2";

        private void SelectThisFrame()
        {
            try
            {
                UIUtil.DefaultProvider.SelectPopUpFrameByName(FrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndStay()
        {
            this.SelectThisFrame();
            UIUtil.DefaultProvider.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        public void SaveAndClose()
        {
            this.SelectThisFrame();
            UIUtil.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.SelectPopUpFrameByName(MerchandiseManager.MerchItemDetailDialogID);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            this.SelectThisFrame();
            UIUtil.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.SelectPopUpFrameByName(MerchandiseManager.MerchItemDetailDialogID);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetName(string name)
        {
            this.SetFieldName(name);
            this.SetDescription(name);
        }

        public void SetFieldName(string name)
        {
            UIUtil.DefaultProvider.Type("ctl00_cphDialog_feeListItemFieldName", name, LocateBy.Id);
        }

        public void SetDescription(string name)
        {
            UIUtil.DefaultProvider.Type("ctl00_cphDialog_feeListItemDescription", name, LocateBy.Id);
        }

        public void SetLimit(int? limit)
        {
            UIUtil.DefaultProvider.TypeRADNumericById("ctl00_cphDialog_feeListItemMaxQuantity", limit);
        }

        public void SetVisibility(bool check)
        {
            UIUtil.DefaultProvider.SetCheckbox("ctl00_cphDialog_feeListItemVisible", check, LocateBy.Id);
        }
    }
}
