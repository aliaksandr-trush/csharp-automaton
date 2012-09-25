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
            UIUtil.DefaultProvider.WaitForDisplayAndClick("btnSave", LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void Cancel()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("btnCancel", LocateBy.Id);
            SelectManagerWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetName(string name)
        {
            UIUtil.DefaultProvider.Type("crGeneral_tbReportName", name, LocateBy.Id);
        }

        public void ChooseTab(DirectoryTab tab)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format("Tabimg_M{0}", (int)tab), LocateBy.Id);
        }

        public void PutItOnListOfAvailableDirectories()
        {
            UIUtil.DefaultProvider.SetCheckbox("crAdvanced_cbShareReport", true, LocateBy.Id);
        }

        public void EnablePassword()
        {
            UIUtil.DefaultProvider.SetCheckbox("crAdvanced_cbRequireLogin", true, LocateBy.Id);
        }
    }
}
