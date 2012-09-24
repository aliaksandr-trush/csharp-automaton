namespace RegOnline.RegressionTest.Managers.Report
{
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public class OnSiteCheckinManager : ManagerBase
    {
        #region locators
        private const string RunReportButton = "Action";
        private const string WhichNamesToViewLocator = "//td[text()='{0}']/..//input";
        private const string SortByDropDownLocator = "//select[@name='SortId']";
        private const string OrderByDropDownLocator = "//select[@name='OrderId']";
        private const string SearchByNameOrCoLocator = "//td[text()='{0}']/preceding-sibling::td[1]/input";
        private const string AlphabetFilterLocator = "//input[@name='alphabet'][@value='{0}']";
        #endregion

        public enum OnSiteNameFilter
        {
            [StringValue("Not Checked In")]
            NotCheckedIn,

            [StringValue("Checked In")]
            CheckedIn,

            [StringValue("Both")]
            Both,
        }

        public enum OnSiteSortByOptions
        {
            [StringValue("Last Name")]
            LastName,

            [StringValue("Reference #")]
            ReferenceNum,

            [StringValue("Company")]
            Company,

            [StringValue("Registration Date")]
            RegistrationDate,

            [StringValue("Cancellation Date")]
            CancellationDate
        }

        public enum OnSiteOrderByOptions
        {
            [StringValue("Ascending")]
            Ascending,

            [StringValue("Descending")]
            Descending
        }

        public enum OnSiteSearchByNameOrCompany
        {
            [StringValue("Search By Name")]
            Name,

            [StringValue("Search By Company")]
            Company
        }

        public void SelectWhichNamesYouWishToView(OnSiteNameFilter filter)
        {
            string locator_XPath = string.Format(WhichNamesToViewLocator, StringEnum.GetStringValue(filter));
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(locator_XPath, LocateBy.XPath);
        }

        public void SortNames(OnSiteSortByOptions sortOption, OnSiteOrderByOptions orderOptions)
        {
            WebDriverUtility.DefaultProvider.SelectWithText(SortByDropDownLocator, StringEnum.GetStringValue(sortOption), LocateBy.XPath);
            WebDriverUtility.DefaultProvider.SelectWithText(OrderByDropDownLocator, StringEnum.GetStringValue(orderOptions), LocateBy.XPath);
        }

        public void SelectSearchByNameOrCompany(OnSiteSearchByNameOrCompany nameOrCompany)
        {
            string locator_XPath = string.Format(SearchByNameOrCoLocator, StringEnum.GetStringValue(nameOrCompany));
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(locator_XPath, LocateBy.XPath);
        }

        public void FilterReportByCharacter(string characterOrNullToSelectAll)
        {
            if (characterOrNullToSelectAll == null)
            {
                string locator_XPath = string.Format(AlphabetFilterLocator, 0);
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(locator_XPath, LocateBy.XPath);
            }
            else
            {
                string locator_XPath = string.Format(AlphabetFilterLocator, characterOrNullToSelectAll);
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(locator_XPath, LocateBy.XPath);
            }
        }

        public void RunReport()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(RunReportButton, LocateBy.Name);
        }
    }
}
