namespace RegOnline.RegressionTest.Managers.Manager.Dashboard
{
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public class AttendeeDirectoryCreator : ManagerBase
    {
        public enum DirectoryTab
        {
            [StringValue("General")]
            General = 0,

            [StringValue("Fields")]
            Fields,

            [StringValue("Filters")]
            Filters,

            [StringValue("Sorting and Grouping")]
            SortingAndGrouping,

            [StringValue("Links and Security")]
            LinksSecurity
        }

        public void Apply()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnSave", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void Cancel()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnCancel", LocateBy.Id);
            SelectManagerWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SetName(string name)
        {
            UIUtilityProvider.UIHelper.Type("crGeneral_tbReportName", name, LocateBy.Id);
        }

        public void ChooseTab(DirectoryTab tab)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("Tabimg_M{0}", (int)tab), LocateBy.Id);
        }

        public void PutItOnListOfAvailableDirectories()
        {
            UIUtilityProvider.UIHelper.SetCheckbox("crAdvanced_cbShareReport", true, LocateBy.Id);
        }

        public void EnablePassword()
        {
            UIUtilityProvider.UIHelper.SetCheckbox("crAdvanced_cbRequireLogin", true, LocateBy.Id);
        }
    }
}
