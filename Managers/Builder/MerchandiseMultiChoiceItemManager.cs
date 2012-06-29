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
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName(FrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndStay()
        {
            this.SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        public void SaveAndClose()
        {
            this.SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(MerchandiseManager.MerchItemDetailDialogID);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            this.SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(MerchandiseManager.MerchItemDetailDialogID);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SetName(string name)
        {
            this.SetFieldName(name);
            this.SetDescription(name);
        }

        public void SetFieldName(string name)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_feeListItemFieldName", name, LocateBy.Id);
        }

        public void SetDescription(string name)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_feeListItemDescription", name, LocateBy.Id);
        }

        public void SetLimit(int? limit)
        {
            UIUtilityProvider.UIHelper.TypeRADNumericById("ctl00_cphDialog_feeListItemMaxQuantity", limit);
        }

        public void SetVisibility(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cphDialog_feeListItemVisible", check, LocateBy.Id);
        }
    }
}
