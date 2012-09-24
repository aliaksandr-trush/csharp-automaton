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
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("btnSave", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void Cancel()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("btnCancel", LocateBy.Id);
            SelectManagerWindow();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetName(string name)
        {
            WebDriverUtility.DefaultProvider.Type("crGeneral_tbReportName", name, LocateBy.Id);
        }

        public void ChooseTab(DirectoryTab tab)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(string.Format("Tabimg_M{0}", (int)tab), LocateBy.Id);
        }

        public void PutItOnListOfAvailableDirectories()
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("crAdvanced_cbShareReport", true, LocateBy.Id);
        }

        public void EnablePassword()
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("crAdvanced_cbRequireLogin", true, LocateBy.Id);
        }
    }
}
