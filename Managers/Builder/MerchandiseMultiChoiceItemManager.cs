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
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(FrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndStay()
        {
            this.SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        public void SaveAndClose()
        {
            this.SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(MerchandiseManager.MerchItemDetailDialogID);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            this.SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(MerchandiseManager.MerchItemDetailDialogID);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetName(string name)
        {
            this.SetFieldName(name);
            this.SetDescription(name);
        }

        public void SetFieldName(string name)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_feeListItemFieldName", name, LocateBy.Id);
        }

        public void SetDescription(string name)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_feeListItemDescription", name, LocateBy.Id);
        }

        public void SetLimit(int? limit)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById("ctl00_cphDialog_feeListItemMaxQuantity", limit);
        }

        public void SetVisibility(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cphDialog_feeListItemVisible", check, LocateBy.Id);
        }
    }
}
