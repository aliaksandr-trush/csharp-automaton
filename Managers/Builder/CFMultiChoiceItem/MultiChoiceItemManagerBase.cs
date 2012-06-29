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
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName(FrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndNew()
        {
            this.SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickSaveAndNew();
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
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
            UIUtilityProvider.UIHelper.Type(NameOnFormTxtboxLocator, name, LocateBy.Id);
        }

        public void SetNameOnReport(string name)
        {
            UIUtilityProvider.UIHelper.Type(NameOnReportTxtboxLocator, name, LocateBy.Id);
        }

        public void SetLimit(int? limit)
        {
            UIUtilityProvider.UIHelper.TypeRADNumericById(LimitTxtboxLocator, limit);
        }

        public void SetLimitPerGroup(int? limit)
        {
            UIUtilityProvider.UIHelper.TypeRADNumericById(PerGroupLimitTxtboxLocator, limit);
        }

        public void SetVisibility(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(VisibilityCheckboxLocator, check, LocateBy.Id);
        }
    }
}
