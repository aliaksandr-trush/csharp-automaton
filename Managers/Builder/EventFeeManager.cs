namespace RegOnline.RegressionTest.Managers.Builder
{
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public class EventFeeManager : FeeManager
    {
        #region Constants
        public const string FeeAdvancedFrameIDInEventFee = "dialog";
        public const string FeeAdvancedFrameIDInRegType = "dialog2";
        #endregion

        #region Constructor
        public EventFeeManager(FormDetailManager.FeeLocation feeLocation) 
            : base(FormDetailManager.Page.Start)
        {
            this.StartPageFeeLocation = feeLocation;
            DC.FeeLocationOfStartPage = feeLocation;
            
            switch (this.StartPageFeeLocation)
            {
                case FormDetailManager.FeeLocation.Event:
                    this.FeeAdvancedDialogID = FeeAdvancedFrameIDInEventFee;
                    break;
                case FormDetailManager.FeeLocation.RegType:
                    this.FeeAdvancedDialogID = FeeAdvancedFrameIDInRegType;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Locators needed to be initialized
        private readonly FormDetailManager.FeeLocation StartPageFeeLocation;
        public readonly string FeeAdvancedDialogID;
        #endregion

        private void SelectThisFrame()
        {
            try
            {
                WebDriverUtility.DefaultProvider.SwitchToMainContent();
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(this.FeeAdvancedDialogID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndStay()
        {
            // When we click the 'advanced' link in the start page, the dialogID is 'dialog',
            // while, the dialogID changes to 'dialog2' when in regtype
            this.SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        [Step]
        public void SaveAndClose()
        {
            this.SelectThisFrame();

            switch (this.StartPageFeeLocation)
            {
                case FormDetailManager.FeeLocation.Event:
                    WebDriverUtility.DefaultProvider.ClickSaveAndClose();
                    Utility.ThreadSleep(1);
                    SelectBuilderWindow();
                    WebDriverUtility.DefaultProvider.WaitForPageToLoad();
                    WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                    break;
                case FormDetailManager.FeeLocation.RegType:
                    WebDriverUtility.DefaultProvider.ClickSaveAndClose();
                    Utility.ThreadSleep(1);
                    WebDriverUtility.DefaultProvider.SwitchToMainContent();
                    WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(RegTypeManager.RegTypeDetailFrameID);
                    WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                    break;
                default:
                    break;
            }
            
            Utility.ThreadSleep(1.5);
        }

        public void Cancel()
        {
            switch (this.StartPageFeeLocation)
            {
                case FormDetailManager.FeeLocation.Event:
                    this.SelectThisFrame();
                    WebDriverUtility.DefaultProvider.ClickCancel();
                    Utility.ThreadSleep(1);
                    WebDriverUtility.DefaultProvider.SelectOriginalWindow();
                    WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                    break;
                case FormDetailManager.FeeLocation.RegType:
                    this.SelectThisFrame();
                    WebDriverUtility.DefaultProvider.ClickCancel();
                    Utility.ThreadSleep(1);
                    WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(RegTypeManager.RegTypeDetailFrameID);
                    WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                    break;
                default:
                    break;
            }
        }

        public void SetName(string nameOnReceipt, string nameOnReport)
        {
            this.SetReceiptName(nameOnReceipt);
            this.SetReportName(nameOnReport);
        }

        public void SetReceiptName(string name)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_cfCF_mipNam_ip1_elRptDesc_TextArea", name, LocateBy.Id);

        }

        public void SetReportName(string name)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_cfCF_mipNam_ip1_txtFieldName", name, LocateBy.Id);
        }
    }
}
