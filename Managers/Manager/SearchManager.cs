namespace RegOnline.RegressionTest.Managers.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.UIUtility;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Utilities;

    public class SearchManager : ManagerBase
    {
        private const string SearchResult = "//a[@href='../forms/details.aspx?EventSessionID={0}&EventID={1}']";
        private const string AttendeeSearchButton = "//a[@id='ctl00_ctl00_cphDialog_uclSearch_btnSearchGo']/span";
        private const string AttendeeSearchNextPageClick = "//a[text()='Next']";
        private const string AdvancedSearchLinkLocator = "ctl00_cphDialog_lbAdvanced";

        public enum SearchMode
        {
            Attendee,
            Event,
            Transaction,
            Help
        }

        private struct SearchPageUrlPath
        {
            public const string Event = "manager/Search/Events.aspx";
            public const string Attendee = "manager/Search/Attendees.aspx";
        }

        public void SelectSearchMode(SearchMode searchMode)
        {
            switch (searchMode)
            {
                case SearchMode.Attendee:
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_ctl00_cphDialog_uclSearch_lbSearchAttendees", LocateBy.Id);
                    break;
                case SearchMode.Event:
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_ctl00_cphDialog_uclSearch_lbSearchEvents", LocateBy.Id);
                    break;
                case SearchMode.Transaction:
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_ctl00_cphDialog_uclSearch_lbSearchTransactions", LocateBy.Id);
                    break;
                case SearchMode.Help:
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_ctl00_cphDialog_uclSearch_lbSearchHelp", LocateBy.Id);
                    break;
                default:
                    break;
            }

            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void SearchEvent(string eventNameToSearch)
        {
            this.SelectSearchMode(SearchMode.Event);
            this.TypeSearchText(eventNameToSearch);
            this.ClickSearchButton();
        }

        public void SearchAttendee(string textToSearch)
        {
            this.SelectSearchMode(SearchMode.Attendee);
            this.TypeSearchText(textToSearch);
            this.ClickSearchButton();
        }

        public void VerifyOnSearchPage(SearchMode searchMode)
        {
            WebDriverUtility.DefaultProvider.WaitForElementDisplay(AdvancedSearchLinkLocator, LocateBy.Id);
            string url = string.Empty;

            switch (searchMode)
            {
                case SearchMode.Attendee:
                    url = SearchPageUrlPath.Attendee;
                    break;

                case SearchMode.Event:
                    url = SearchPageUrlPath.Event;
                    break;

                case SearchMode.Transaction:
                case SearchMode.Help:
                    throw new NotImplementedException();

                default:
                    break;
            }

            Assert.True(WebDriverUtility.DefaultProvider.UrlContainsAbsolutePath(url));
        }

        public void ClickEventLinkOnSearchResultPage(int eventId)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(
                string.Format("//a[@href='../forms/details.aspx?EventSessionID={0}&EventID={1}']", GetEventSessionId(), eventId), 
                LocateBy.XPath);

            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void ClickAttendeeLinkOnSearchResultPage(int registrationId)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(
                string.Format("//a[@href=\"javascript:AttendeeInfo('{0}',{1})\"]", GetEventSessionId(), registrationId), 
                LocateBy.XPath);

            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void TypeSearchText(string text)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_ctl00_cphDialog_uclSearch_txtSearchQ", text, LocateBy.Id);
        }

        public void ClickSearchButton()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(AttendeeSearchButton, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            Utility.ThreadSleep(3);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void Return()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_hplBack", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }
    }
}
