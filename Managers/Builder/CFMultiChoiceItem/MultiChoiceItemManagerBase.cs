namespace RegOnline.RegressionTest.Managers.Builder
{
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public abstract class MultiChoiceItemManagerBase : ManagerBase
    {
        protected const string PerGroupLimitTxtboxLocator = "ctl00_cphDialog_rntPerGroupLimit";
        protected const string NameOnFormTxtboxLocator = "ctl00_cphDialog_txtDescription";
        protected const string NameOnReportTxtboxLocator = "ctl00_cphDialog_txtFieldName";
        protected const string LimitTxtboxLocator = "ctl00_cphDialog_rntMaxQuantity";
        protected const string VisibilityCheckboxLocator = "ctl00_cphDialog_chkActive";

        public abstract string FrameID { get; }

        protected void SelectThisFrame()
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

        public void SaveAndNew()
        {
            this.SelectThisFrame();
            UIUtil.DefaultProvider.ClickSaveAndNew();
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public abstract void SaveAndClose();

        public abstract void Cancel();

        public void SetName(string name)
        {
            this.SetNameOnForm(name);
            this.SetNameOnReport(name);
        }

        public void SetNameOnForm(string name)
        {
            UIUtil.DefaultProvider.Type(NameOnFormTxtboxLocator, name, LocateBy.Id);
        }

        public void SetNameOnReport(string name)
        {
            UIUtil.DefaultProvider.Type(NameOnReportTxtboxLocator, name, LocateBy.Id);
        }

        public void SetLimit(int? limit)
        {
            UIUtil.DefaultProvider.TypeRADNumericById(LimitTxtboxLocator, limit);
        }

        public void SetLimitPerGroup(int? limit)
        {
            UIUtil.DefaultProvider.TypeRADNumericById(PerGroupLimitTxtboxLocator, limit);
        }

        public void SetVisibility(bool check)
        {
            UIUtil.DefaultProvider.SetCheckbox(VisibilityCheckboxLocator, check, LocateBy.Id);
        }
    }
}
