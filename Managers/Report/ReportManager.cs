namespace RegOnline.RegressionTest.Managers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class ReportManager : ManagerBase
    {
        private const string AttendeeReportColumnHeaderLocator = "//tr[@id='ctl00_Main_rptAttendees_ctl00_trItem']/td[{0}]/div[2]";
        private const string ReportTableID = "rptParentTable";
        public const string DefaultSmartLinkPassword = "password1";

        private const string TransactionFeesReportLocator = "ctl00_ctl00_cphDialog_cpMgrMain_lnkTransFees";
        private const string TransactionsReportLocator = "//a[text()='{0}']";
        private const string TransactionsFeesReportXPathCount = "//td[text()='{0}']/../following-sibling::tr[1]//tbody/tr[1]/td";
        private const string ExpandedTransactionFeesReportLocator = "//td[text()='{0}']/../following-sibling::tr[1]//tbody/tr";
        private const string VerifyAttendeeRowIndexLocator = "ctl00_Main_rptAttendees_ctl{0}_trItem";
        private const string GetAttendeeRowIndexLocator = "//b[text()='{0}']/../../..";
        private const string GroupByLocator = "//*[contains(@id,'GroupHeader')]/*[contains(text(),'{0}')]";

        private abstract class Locator
        {
            public const string NoRecordsTextLocator = "//table[@id='rptCell']//*[text()='No Records Found']";
            public const string ZeroRecordsTextLocator = "//*[@id='headerTable']/tbody/tr/td[@class='rptHead'][contains(text(),'0')]";
        }

        // There is another 'AttendeeStatus' enum in AttendeeInfoManager.cs, but that one doesn't contain 'Attended' while this one does
        public enum AttendeeStatus
        {
            [StringValue("Pending")]
            Pending,

            [StringValue("Confirmed")]
            Confirmed,

            [StringValue("Approved")]
            Approved,

            [StringValue("Declined")]
            Declined,

            [StringValue("Standby")]
            Standby,

            [StringValue("Attended")]
            Attended,

            [StringValue("No-show")]
            NoShow,

            [StringValue("Follow-up")]
            FollowUp,

            [StringValue("Canceled")]
            Canceled
        }

        public enum CommonReportType
        {
            [StringValue("Attendee Report")]
            RegistrantList,

            [StringValue("Agenda Item Report")]
            AgendaSelections,

            [StringValue("Transaction Report")]
            Transaction,

            [StringValue("Credit Card Reconciliation")]
            CreditCardTransaction,

            [StringValue("Transaction Fees Report")]
            TransactionFees,

            [StringValue("Rooming List")]
            RoomingList,

            [StringValue("Event Snapshot")]
            EventSnapshot,

            [StringValue("Registration Waitlist Report")]
            RegistrationWaitlist,

            [StringValue("Resource Report")]
            Resource,

            [StringValue("Travel Report")]
            Travel,

            [StringValue("Lodging Report")]
            Lodging,

            [StringValue("Group Discount Report")]
            GroupDiscount,

            [StringValue("Confirmed Registrants")]
            ConfirmedRegistrants
        }

        public enum BalanceTypes
        {
            Positive,
            Negative,
            Zero
        }

        public enum SortTypes
        {
            Ascending,
            Descending
        }

        public ReportManager() 
        {
            this.OnSiteCheckinMgr = new OnSiteCheckinManager();
        }

        public OnSiteCheckinManager OnSiteCheckinMgr
        {
            get;
            private set;
        }

        public bool ReportHasRecords()
        {
            Utility.ThreadSleep(1);
            return !(UIUtilityProvider.UIHelper.IsElementPresent(Locator.NoRecordsTextLocator, LocateBy.XPath))
                && !(UIUtilityProvider.UIHelper.IsElementPresent(Locator.ZeroRecordsTextLocator, LocateBy.XPath));
        }

        [Step]
        public void CloseReportPopupWindow()
        {
            SelectReportPopupWindow();
            UIUtilityProvider.UIHelper.CloseWindow();
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
        }

        public void SelectCommonReportType(CommonReportType type)
        {
            UIUtilityProvider.UIHelper.SelectWithText("ctl00_reportTools_ddlSelectReport", StringEnum.GetStringValue(type), LocateBy.Id);
        }

        [Verify]
        public void VerifyAttendeeReportRowCount(int eventId)
        {
            // Fetch expected count
            //S.RegistrationsService svcRegistrations = new S.RegistrationsService();
            //E.TList<E.Registrations> regs = svcRegistrations.GetByEventId(eventId);
            //regs.ApplyFilter(delegate(E.Registrations r) { return r.StatusId != (int)E.RegistrationStatusesList.Incomplete; });
            List<Registration> regs = new List<Registration>();

            ClientDataContext db = new ClientDataContext();
            regs = (from r in db.Registrations where r.Event_Id == eventId && r.StatusId != 7 select r).ToList();
            
            
            // Verify
            this.VerifyReportTableRowCount(regs.Count);
        }

        [Step]
        public void ClickReportsFilterButton()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_reportTools_linkCustomizeIcon", LocateBy.Id);

            // select the window
            UIUtilityProvider.UIHelper.SelectWindowByTitle("FiltersConsole"); 

            // make sure it is the a correct popup
            if (!UIUtilityProvider.UIHelper.IsTextPresent("Attendee Report Filter"))
            {
                UIUtilityProvider.UIHelper.FailTest("Not on Attendee Report Filter!");
            }
        }

        [Step]
        public void SelectFilterRegType(string regTypeCaption)
        {
            // set reg types filter
            UIUtilityProvider.UIHelper.SelectWithText("CC_ddlRegTypes", regTypeCaption, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnApply", LocateBy.Id);

            // go to the parent window
            SelectReportPopupWindow();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void SetFilterRegStatus(string regStatusCaption)
        {
            //set reg types filter
            UIUtilityProvider.UIHelper.SelectWithText("CC_ddlRegStatus", regStatusCaption, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnApply", LocateBy.Id);

            //go to the parent window
            SelectReportPopupWindow();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Verify]
        public void VerifyReportFilteredByRegType(int regTypeId)
        {
            //get expected count            
            //S.RegistrationsService svcRegistrations = new S.RegistrationsService();
            //E.TList<E.Registrations> regs = svcRegistrations.GetByRegTypeId(regTypeId);
            //regs.ApplyFilter(delegate(E.Registrations r) { return r.StatusId != (int)E.RegistrationStatusesList.Incomplete; });

            List<Registration> regs = new List<Registration>();

            ClientDataContext db = new ClientDataContext();
            regs = (from r in db.Registrations where r.RegTypeId == regTypeId && r.StatusId != 7 orderby r.Register_Id descending select r).ToList();

            //verify count
            this.VerifyReportTableRowCount(regs.Count);

            //verify attendee presence
            this.VerifyAttendeePresentOnTheReport(regs[0].Attendee.First_Name, regs[0].Attendee.Last_Name);
        }

        public void VerifyReportFilteredByRegType(string regType)
        {
            int columns = UIUtilityProvider.UIHelper.GetXPathCountByXPath("//table[@id='_rol_fixedHeader']//tr/td");
            string regTypeLocator = "//table[@id='_rol_fixedHeader']//tr/td[{0}]/div[contains(text(),'Type')]";
            int regTypeColumn = 0;
            int i = 1;

            while (i <= columns)
            {
                string regTypeColumnLocator = string.Format(regTypeLocator, i);
                if (UIUtilityProvider.UIHelper.IsElementPresent(regTypeColumnLocator, LocateBy.XPath))
                {
                    regTypeColumn = i;
                    break;
                }
                i += 1;
            }

            int regs = this.GetReportTableRowCount();

            for (int j = 1; j <= regs; j++)
            {
                string type = UIUtilityProvider.UIHelper.GetText(string.Format("//table[@id='rptParentTable']/tbody/tr[{0}]/td[{1}]", j, regTypeColumn), LocateBy.XPath);
                Assert.True(type == regType);
            }
        }

        [Verify]
        public void VerifyReportFilteredByRegStatus(int eventId, int regStatus)
        {
            //get expected count            
            //S.RegistrationsService svcRegistrations = new S.RegistrationsService();
            //E.TList<E.Registrations> regs = svcRegistrations.GetByEventId(eventId);
            //regs.ApplyFilter(delegate(E.Registrations r) { return r.StatusId == (int)regStatus; });
            List<Registration> regs = new List<Registration>();

            ClientDataContext db = new ClientDataContext();
            regs = (from r in db.Registrations where r.Event_Id == eventId && r.StatusId == regStatus orderby r.Register_Id descending select r).ToList();
            
            //verify count
            this.VerifyReportTableRowCount(regs.Count);

            //verify attendee presence
            this.VerifyAttendeePresentOnTheReport(regs[0].Attendee.First_Name, regs[0].Attendee.Last_Name);
        }

        public void VerifyReportFilteredByBalance(BalanceTypes balanceType)
        {
            int columns = UIUtilityProvider.UIHelper.GetXPathCountByXPath("//table[@id='_rol_fixedHeader']//tr/td");
            string balanceLocator = "//table[@id='_rol_fixedHeader']//tr/td[{0}]/div[contains(text(),'Balance Due')]";
            int balanceColumn = 0;
            int i = 1;

            while (i <= columns)
            {
                string balanceColumnLocator = string.Format(balanceLocator, i);
                if (UIUtilityProvider.UIHelper.IsElementPresent(balanceColumnLocator, LocateBy.XPath))
                {
                    balanceColumn = i;
                    break;
                }
                i += 1;
            }

            int regs = this.GetReportTableRowCount();

            for (int j = 1; j <= regs; j++)
            {
                string balance = UIUtilityProvider.UIHelper.GetText(string.Format("//table[@id='rptParentTable']/tbody/tr[{0}]/td[{1}]", j, balanceColumn), LocateBy.XPath);
                switch(balanceType)
                {
                    case BalanceTypes.Positive:
                        Assert.False(balance.Contains("("));
                        Assert.False(balance.Contains(")"));
                        Assert.False(balance.Contains("-"));
                        Assert.False(balance == "$0.00");
                        break;
                    case BalanceTypes.Negative:
                        Assert.True(balance.Contains("(") || balance.Contains("-"));
                        break;
                    case BalanceTypes.Zero:
                        Assert.True(balance == "$0.00");
                        break;
                    default:
                        break;
                }
            }
        }

        public void VerifyReportSortedByFullName(SortTypes sortType)
        {
            int columns = UIUtilityProvider.UIHelper.GetXPathCountByXPath("//table[@id='_rol_fixedHeader']//tr/td");
            string fullNameLocator = "//table[@id='_rol_fixedHeader']//tr/td[{0}]/div[contains(text(),'Full Name')]";
            int fullNameColumn = 0;
            int i = 1;

            while (i <= columns)
            {
                string fullNameColumnLocator = string.Format(fullNameLocator, i);
                if (UIUtilityProvider.UIHelper.IsElementPresent(fullNameColumnLocator, LocateBy.XPath))
                {
                    fullNameColumn = i;
                    break;
                }
                i += 1;
            }

            int regs = this.GetReportTableRowCount();
            List<double> fullNameSurfixNumbers = new List<double>();
            
            for (int j = 0; j < regs; j++)
            {
                string fullName = UIUtilityProvider.UIHelper.GetText(string.Format("//table[@id='rptParentTable']/tbody/tr[{0}]/td[{1}]", j + 1, fullNameColumn), LocateBy.XPath);
                fullName = Regex.Replace(fullName, @"[^\d]", "");
                fullNameSurfixNumbers.Add(Convert.ToDouble(fullName));
            }
            
            switch (sortType)
            {
                case SortTypes.Ascending:
                    int k = 0;

                    while (k < fullNameSurfixNumbers.Count - 1)
                    {
                        Assert.True(fullNameSurfixNumbers[k] <= fullNameSurfixNumbers[k + 1]);
                        k++;
                    }
                    break;
                case SortTypes.Descending:
                    int h = 0;

                    while (h < fullNameSurfixNumbers.Count - 1)
                    {
                        Assert.True(fullNameSurfixNumbers[h] >= fullNameSurfixNumbers[h + 1]);
                        h++;
                    }
                    break;
                default:
                    break;
            }
        }

        public void VerifyAllAttendeesStatus(AttendeeStatus attendeeStatus)
        {
            int columns = UIUtilityProvider.UIHelper.GetXPathCountByXPath("//table[@id='_rol_fixedHeader']//tr/td");
            string statusLocator = "//table[@id='_rol_fixedHeader']//tr/td[{0}]/div[contains(text(),'Status')]";
            int statusColumn = 0;
            int i = 1;

            while (i <= columns)
            {
                string statusColumnLocator = string.Format(statusLocator, i);
                if (UIUtilityProvider.UIHelper.IsElementPresent(statusColumnLocator, LocateBy.XPath))
                {
                    statusColumn = i;
                    break;
                }
                i += 1;
            }

            int regs = this.GetReportTableRowCount();

            for (int j = 1; j <= regs; j++)
            {
                string status = UIUtilityProvider.UIHelper.GetText(string.Format("//table[@id='rptParentTable']/tbody/tr[{0}]/td[{1}]", j, statusColumn), LocateBy.XPath);
                if (status != StringEnum.GetStringValue(AttendeeStatus.Canceled))
                    Assert.True(status == StringEnum.GetStringValue(attendeeStatus));
            }
        }

        [Step]
        public void ClickSmartLinkButton()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//div[@id='ctl00_reportTools_divSmartlink']/a", LocateBy.XPath);

            // select the window
            UIUtilityProvider.UIHelper.SelectWindowByTitle("SmartLink Options");
        }

        [Step]
        public string EnableSmartLinkAccess(bool withPassword)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("cbShareReport", true, LocateBy.Id);

            if (withPassword)
            {
                UIUtilityProvider.UIHelper.Type("tbPassword", DefaultSmartLinkPassword, LocateBy.Id);
            }

            string link = UIUtilityProvider.UIHelper.GetText("lblSmartLink", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnSave", LocateBy.Id);

            return link;
        }

        [Step]
        public void LoginToSmartLinkReport(string password)
        {
            UIUtilityProvider.UIHelper.Type("txtPassword", password, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("submit", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.WaitForElementPresent("//table[@id='" + ReportTableID + "']", LocateBy.XPath);
        }

        public void VerifyAttendeePresentOnTheReport(string FirstName, string LastName)
        {
            UIUtilityProvider.UIHelper.IsTextPresent(LastName + ", " + FirstName);
        }

        [Step]
        public void ClickCheckInButton()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Check In", LocateBy.LinkText);
        }

        [Step]
        public void SelectAttendeeReportRecord(int RegisterId)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//input[@id='chkAttendee' and @value='{0}']", RegisterId), LocateBy.XPath);
        }

        public void SelectAllAttendees()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("selectAll", LocateBy.Id);
        }

        //actually RegisterID. AttendeeID is the contact key
        public int GetAttendeeId(int rowIndex)
        {
            return Convert.ToInt32(Get(rowIndex, 2, "/a/b"));//also, it's within a bold within a hyperlink
        }

        private string Get(int rowIndex, int cellIndex)
        {
            return Get(rowIndex, cellIndex, "");
        }

        private string Get(int rowIndex, int cellIndex, string inside)
        {
            string regIdLocatorFormat = 
                "//table[@id='" + ReportTableID + "']/tbody/tr[@id='" + VerifyAttendeeRowIndexLocator + "']/td[{1}]" + inside;

            string index = rowIndex.ToString();
            
            // If row index is less than 10, add a '0' at the front
            if (rowIndex < 10)
            {
                index = index.Insert(0, "0");
            }

            return UIUtilityProvider.UIHelper.GetText(string.Format(regIdLocatorFormat, index, cellIndex), LocateBy.XPath);
        }
                
        private string GetRegType(int rowIndex)
        {
            //column / cell 7 for Active Europe non-Pro event
            return Get(rowIndex, 7);
        }

        private int FetchGroupPrimary(int rowIndex)
        {
            int regId = GetAttendeeId(rowIndex);//register id
            //S.RegistrationsService svcRegs = new S.RegistrationsService();
            //E.Registrations r = svcRegs.GetByRegisterId(regId);
            Registration reg = new Registration();

            ClientDataContext db = new ClientDataContext();
            reg = (from r in db.Registrations where r.Register_Id == regId select r).Single();
            

            return reg.GroupId;
        }

        //only for Active Europe non-Pro event
        public string GetTeamName(int rowIndex)
        {
            //column / cell 4
            return Get(rowIndex, 4);
        }

        //only for Active Europe non-Pro event
        public int GetBibId(int rowIndex)
        {
            //column / cell 8
            return Convert.ToInt32(Get(rowIndex, 8));
        }

        //This is when event is "Assign a unique number to each member of a team"
        /*1. Single registrations have unique bib numbers
        2. Verify that every registrant has a unique Bib number matching the reg type.
        3. All members of a group registration that did not require a Team Name have unique Bib numbers.
         basically everyone is unique.
         */
        [Step]
        public void VerifyUniquenessOnAttendeeReport(int eventId)
        {
            //SortedDictionary gets us O(log n) speed for ContainsKey.
            Dictionary<int, SortedDictionary<int, int>> bibsPerGroup = new Dictionary<int, SortedDictionary<int, int>>();
            SortedDictionary<int, int> bibsOverall = new SortedDictionary<int, int>();

            Dictionary<int, List<int>> groupsPerType = new Dictionary<int, List<int>>();
            List<Registration> regs = new List<Registration>();
            List<EventRegType> regTypes = new List<EventRegType>();

            ClientDataContext db = new ClientDataContext();
            regs = (from r in db.Registrations where r.Event_Id == eventId select r).ToList();
            regTypes = (from rt in db.EventRegTypes where rt.EventId == eventId select rt).ToList();


            //S.RegistrationsService svcRegistrations = new S.RegistrationsService();
            //E.TList<E.Registrations> regs = svcRegistrations.GetByEventId(eventId);

            //S.EventRegTypesService svcRegTypes = new S.EventRegTypesService();
            //E.TList<E.EventRegTypes> regTypes = svcRegTypes.GetByEventId(eventId);

            //for each line in the report, prepare directories: bibsPerGroup and groupsPerType
            for (int i = 1; i <= regs.Count; i++)
            {
                int bibIdFromReport = GetBibId(i);

                if (bibsOverall.ContainsKey(bibIdFromReport))
                {
                    Assert.Fail("Duplicate bib number");
                }
                else
                {
                    bibsOverall.Add(bibIdFromReport, 0);
                }

                string regTypeNameFromReport = GetRegType(i);
                EventRegType regTypeFromReport = regTypes.Find(r => r.ReportDescription == regTypeNameFromReport);
                int groupFromReport = FetchGroupPrimary(i);

                //we already tested duplicates for this event; there are no duplicates in this group
                SortedDictionary<int, int> sd;
                if (bibsPerGroup.ContainsKey(groupFromReport))
                {
                    sd = bibsPerGroup[groupFromReport];
                }
                else
                {
                    sd = new SortedDictionary<int, int>();
                    bibsPerGroup.Add(groupFromReport, sd);
                }

                sd.Add(bibIdFromReport, 0);
                AddGroupsPerType(groupFromReport, regTypeFromReport.Id, groupsPerType);
            }

            TestSequence(eventId, regTypes, bibsPerGroup, groupsPerType);
        }

        private void TestSequence(int eventId, List<EventRegType> regTypes, Dictionary<int, SortedDictionary<int, int>> bibsPerGroup, Dictionary<int, List<int>> groupsPerType)
        {
            //S.BibNumbersService svcBibs = new S.BibNumbersService();//this table is separate from regtypes, with 1-0 FK
            //E.TList<E.BibNumbers> bibNumbersAll = svcBibs.GetByEventId(eventId);

            //E.BibNumbers bibNumberEvent = bibNumbersAll.Find(b => b.RegTypeId == 0);
            List<BibNumber> bibNumbersAll = new List<BibNumber>();

            ClientDataContext db = new ClientDataContext();
            bibNumbersAll = (from b in db.BibNumbers where b.EventId == eventId select b).ToList();
            

            BibNumber bibNumberEvent = bibNumbersAll.Find(b => b.RegTypeId == 0);

            List<EventRegType> regTypesWithStartingNumbers = regTypes.FindAll(r =>
                (bibNumbersAll.Find(b => b.RegTypeId == r.Id) != null));

            List<EventRegType> regTypesWithDefaultStartingNumber = regTypes.FindAll(r =>
                (bibNumbersAll.Find(b => b.RegTypeId == r.Id) == null));

            foreach (EventRegType regType in regTypesWithStartingNumbers)
            {
                BibNumber bibNumbers = bibNumbersAll.Find(b => b.RegTypeId == regType.Id);
                int startingNumberPerRegType = bibNumbers.StartingNumber;
                IEnumerable<int> groups = groupsPerType[regType.Id];
                int[] bibs = CollectBibsOverGroups(bibsPerGroup, groups);

                //all the types grouped together?
                Array.Sort<int>(bibs);

                if (bibs[0] != startingNumberPerRegType)
                {
                    Assert.Fail(" starts with wrong value");
                }

                VerifyBibsAreGroupedTogether(bibs);
            }

            //now, the bibs which aren't in a special-numbered regtype
            int startingNumber = bibNumberEvent.StartingNumber;
            int[] bibsForEvent;
            List<int[]> bibCollection = new List<int[]>();
            int tally = 0;

            foreach (EventRegType regType in regTypesWithDefaultStartingNumber)
            {
                IEnumerable<int> groups = groupsPerType[regType.Id];
                int[] bibsPerType = CollectBibsOverGroups(bibsPerGroup, groups);
                tally += bibsPerType.Length;
                bibCollection.Add(bibsPerType);
            }

            //all the types grouped together?
            bibsForEvent = new int[tally];
            tally = 0;

            foreach (int[] bibsPerType in bibCollection)
            {
                bibsPerType.CopyTo(bibsForEvent, tally);
                tally += bibsPerType.Length;
            }

            Array.Sort<int>(bibsForEvent);

            if (bibsForEvent[0] != startingNumber)
            {
                Assert.Fail(" starts with wrong value");
            }

            VerifyBibsAreGroupedTogether(bibsForEvent);
        }

        private int[] CollectBibsOverGroups(Dictionary<int, SortedDictionary<int, int>> bibsPerGroup, IEnumerable<int> groups)
        {
            int tally = 0;

            foreach (int group in groups)
            {
                tally += bibsPerGroup[group].Count;
            }

            int[] bibs = new int[tally];
            tally = 0;

            //teams all grouped together?
            foreach (int group in groups)
            {
                SortedDictionary<int, int> bibsDictionary = bibsPerGroup[group];
                bibsDictionary.Keys.CopyTo(bibs, tally);

                int start = tally;
                tally += bibsPerGroup[group].Count;
                int end = tally;

                VerifyBibsAreGroupedTogether(bibs, start, end);
            }

            return bibs;
        }

        private void VerifyBibsAreGroupedTogether(int[] bibs)
        {
            VerifyBibsAreGroupedTogether(bibs, 0, bibs.Length);
        }

        private void VerifyBibsAreGroupedTogether(int[] bibs, int start, int end)
        {
            int lastNumberOverall = bibs[start];

            for (int i = start + 1; i < end; i++)
            {
                int thisNumber = bibs[i];

                if (lastNumberOverall != thisNumber - 1)
                {
                    Assert.Fail(string.Format(" skipped a value from {0} to {1}", lastNumberOverall, thisNumber));
                }

                lastNumberOverall = thisNumber;
            }
        }

        private void AddGroupsPerType(int groupFromReport, int regTypeFromReportId, Dictionary<int, List<int>> groupsPerType)
        {
            //add each team entry to a reg type
            if (groupsPerType.ContainsKey(regTypeFromReportId))
            {
                if (!groupsPerType[regTypeFromReportId].Contains(groupFromReport))
                {
                    groupsPerType[regTypeFromReportId].Add(groupFromReport);
                }
            }
            else
            {
                groupsPerType.Add(regTypeFromReportId, new List<int>() { groupFromReport });
            }
        }

        //Test event setting, "Assign the same number to every member of a team."
        /*Verify the following results:
        1. Single registrations still have unique bib numbers
        2. All the members of a group registration with a Team Name have the same Bib number.
        3. All members of a group registration that did not require a Team Name still have unique Bib numbers.*/
        [Step]
        public void VerifySameOnAttendeeReport(int eventId)
        {
            Dictionary<int, List<int>> groupsPerType = new Dictionary<int, List<int>>();
            Dictionary<string, int> bibPerTeam = new Dictionary<string, int>();
            Dictionary<int, SortedDictionary<int, int>> bibsPerGroup = new Dictionary<int, SortedDictionary<int, int>>();
            SortedDictionary<int, int> bibsOverall = new SortedDictionary<int, int>();

            //S.RegistrationsService svcRegistrations = new S.RegistrationsService();
            //E.TList<E.Registrations> regs = svcRegistrations.GetByEventId(eventId);

            //S.EventRegTypesService svcRegTypes = new S.EventRegTypesService();
            //E.TList<E.EventRegTypes> regTypes = svcRegTypes.GetByEventId(eventId);
            List<Registration> regs = new List<Registration>();
            List<EventRegType> regTypes = new List<EventRegType>();

            ClientDataContext db = new ClientDataContext();
            regs = (from r in db.Registrations where r.Event_Id == eventId select r).ToList();
            regTypes = (from rt in db.EventRegTypes where rt.EventId == eventId select rt).ToList();
            

            //for each line in the report, prepare directories: bibsPerGroup and groupsPerType
            for (int i = 1; i <= regs.Count; i++)
            {
                string teamFromReport = GetTeamName(i);
                int bibIdFromReport = GetBibId(i);
                
                if (!string.IsNullOrEmpty(teamFromReport))
                {
                    if (bibPerTeam.ContainsKey(teamFromReport))
                    {
                        if (bibPerTeam[teamFromReport] != bibIdFromReport)
                        {
                            Assert.Fail("Different bib number in team");
                        }
                        else
                        {
                            bibsOverall.Add(bibIdFromReport, 0);
                        }
                    }
                }
                else if (bibsOverall.ContainsKey(bibIdFromReport))
                {
                    Assert.Fail("Duplicate bib number");
                }
                else
                {
                    bibsOverall.Add(bibIdFromReport, 0);
                }

                //add each team entry to a reg type
                string regTypeNameFromReport = GetRegType(i);
                EventRegType regTypeFromReport = regTypes.Find(r => r.ReportDescription == regTypeNameFromReport);
                int groupFromReport = FetchGroupPrimary(i);

                SortedDictionary<int, int> sd;
                if (bibsPerGroup.ContainsKey(groupFromReport))
                {
                    sd = bibsPerGroup[groupFromReport];
                    if(!sd.ContainsKey(bibIdFromReport))//this sort of event is designed to make duplicates inside a group; don't repeat them.
                        sd.Add(bibIdFromReport, 0);
                }
                else
                {
                    sd = new SortedDictionary<int, int>();
                    bibsPerGroup.Add(groupFromReport, sd);
                    sd.Add(bibIdFromReport, 0);
                }
                AddGroupsPerType(groupFromReport, regTypeFromReport.Id, groupsPerType);
            }

            TestSequence(eventId, regTypes, bibsPerGroup, groupsPerType);
        }

        public void OpenAttendeeInfoByRegId(int regId)
        {
            string locator_XPath = string.Format("//b[text()='{0}']/..", regId);
            string here = UIUtilityProvider.UIHelper.GetLocation();
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator_XPath, LocateBy.XPath);
            Utility.ThreadSleep(3);
            UIUtilityProvider.UIHelper.SelectWindowByTitle("Attendee Info");
        }

        [Step]
        public void ClickOKOnCheckInConfirmationPopup()
        {
            UIUtilityProvider.UIHelper.GetConfirmation();
            Utility.ThreadSleep(3);
            ////UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void WaitForReportToLoad()
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent("//table[@id='" + ReportTableID + "']", LocateBy.XPath);
        }

        [Verify]
        public void VerifyIsTestRegistrationOnAttendeeReport(int RegisterId, bool isTest)
        {
            //get row count
            int totalRowsInReport = GetReportTableRowCount();
            string currentStatus = string.Empty;
            for (int i = 1; i <= totalRowsInReport; i++)
            {
                string regId = UIUtilityProvider.UIHelper.GetTable(ReportTableID, i, 2);
                //find the row first
                if (regId == RegisterId.ToString())
                {
                    currentStatus = UIUtilityProvider.UIHelper.GetTable(ReportTableID, i, 3);
                    break;
                }
            }

            Assert.AreEqual(currentStatus.ToLower().Contains("(test)"), isTest);
        }

        [Verify]
        public void VerifyRegistrationStatusOnAttendeeReport(int RegisterId, AttendeeStatus expectedStatus)
        {
            // Get row count
            int totalRowsInReport = this.GetReportTableRowCount();
            string currentStatus = string.Empty;

            for (int i = 1; i <= totalRowsInReport; i++)
            {
                string regId = UIUtilityProvider.UIHelper.GetTable(ReportTableID, i, 2);

                // Find the row first
                if (regId == RegisterId.ToString())
                {
                    currentStatus = UIUtilityProvider.UIHelper.GetTable(ReportTableID, i, 6);
                    break;
                }
            }

            Assert.AreEqual(StringEnum.GetStringValue(expectedStatus).ToLower(), currentStatus.ToLower());
        }

        [Step]
        public void OpenReportUrl(int reportTypeId, int eventId)
        {
            try
            {
                UIUtilityProvider.UIHelper.OpenUrl(String.Format(
                    "{0}activereports/ReportServer/attendeeReport.aspx?rptType={1}&EventId={2}&EventSessionId={3}",
                    ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrl, 
                    reportTypeId, 
                    eventId, 
                    this.GetEventSessionId()));
            }
            catch (Exception ex)
            {
                //ignore if alert("CheckIn successful") pops up.
                Trace.WriteLine(ex.Message);
            }
        }

        [Step]
        public void ClickChangeStatusButton()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Change Status", LocateBy.LinkText);
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.SelectWindowByName("GroupEmail");
        }

        public void ClickChangeStatusButtonOnCustomReport()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Change Status", LocateBy.LinkText);
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.SelectWindowByName("ChangeStatus");
        }

        [Step]
        public void ChangeStatus(AttendeeStatus fromStatus, AttendeeStatus toStatus)
        {
            UIUtilityProvider.UIHelper.SelectWithText("FromStatusId", StringEnum.GetStringValue(fromStatus), LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectWithText("ToStatusId", StringEnum.GetStringValue(toStatus), LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnChange", LocateBy.Id);
            Utility.ThreadSleep(2);
        }

        public void ChangeStatusOnCustomReport(AttendeeStatus fromStatus, AttendeeStatus toStatus)
        {
            UIUtilityProvider.UIHelper.SelectWithText("FromStatusId", StringEnum.GetStringValue(fromStatus), LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectWithText("ToStatusId", StringEnum.GetStringValue(toStatus), LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Submit1", LocateBy.Id);
            Utility.ThreadSleep(2);
        }

        [Step]
        public void ClickOKOnChangeStatusConfirmationPopup()
        {
            UIUtilityProvider.UIHelper.GetConfirmation();
        }

        public void ClickRunReport()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_reportTools_btnRun", LocateBy.Id);
            Utility.ThreadSleep(2);
        }

        [Step]
        public void ClickSendEmailButton()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Send Email", LocateBy.LinkText);
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.SelectWindowByName("GroupEmail");
        }

        [Step]
        public void SendGroupEmail(string emailSubject, string emailContent)
        {
            string subjectLocator = "txtSubject";

            // Click subject textbox first to set focus on it
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(subjectLocator, LocateBy.Id);

            UIUtilityProvider.UIHelper.Type(subjectLocator, emailSubject, LocateBy.Id);

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//*[@id='htmContent_ModesWrapper']//a[@class='reMode_html']", LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(1);
            UIUtilityProvider.UIHelper.Type("//textarea", emailContent, LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnSend", LocateBy.Id);
            Utility.ThreadSleep(1);
            SelectReportPopupWindow();
        }

        [Verify]
        public void VerifyGroupEmailJobWasCreated(int eventId, string emailSubject, int AttendeeId)
        {
            bool emailFound = false;
            //check if it is in EmailJobs table first
            //S.EmailJobsService svcEmailJobs = new S.EmailJobsService();
            //E.TList<E.EmailJobs> emailJobs = svcEmailJobs.GetByInviteEventId(eventId);

            ////check if emailSubject matches and email was created in the last 24 hours
            //emailJobs.ApplyFilter(delegate(E.EmailJobs e) { return e.Subject == emailSubject && DateTime.Now.AddDays(-1) < e.AddDate.GetValueOrDefault(); });

            List<EmailJob> emailJobs = new List<EmailJob>();

            ClientDataContext db = new ClientDataContext();
            emailJobs = (from e in db.EmailJobs where e.Event.Id == eventId &&
                                 e.Subject == emailSubject && DateTime.Now.AddDays(-1) < e.Add_Date.GetValueOrDefault() select e).ToList();
            

            emailFound = (emailJobs.Count > 0);

            //check EmailTransactions table in case it was already sent
            if (!emailFound)
            {
                //S.EmailTransactionsService svcEmailTransactions = new S.EmailTransactionsService();
                //E.TList<E.EmailTransactions> emailTransactions = svcEmailTransactions.GetByEventID(eventId);
                ////we can check that attendee id matches as well
                //emailTransactions.ApplyFilter(delegate(E.EmailTransactions e) { return e.Subject == emailSubject && DateTime.Now.AddDays(-1) < e.DateSent && e.MemberId == AttendeeId; });
                List<EmailTransaction> emailTransactions = new List<EmailTransaction>();
                emailTransactions = (from e in db.EmailTransactions where e.EventID == eventId && e.Subject == emailSubject &&
                                             DateTime.Now.AddDays(-1) < e.DateSent && e.MemberId == AttendeeId select e).ToList();

                emailFound = (emailTransactions.Count > 0);
            }

            Assert.AreEqual(true, emailFound);
        }

        /// <summary>
        /// columnIndex is 0-based 
        /// </summary>
        /// <param name="columnIndex"></param>
        [Step]
        public void ClickAttendeeReportColumnHeader(int columnIndex)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(AttendeeReportColumnHeaderLocator, columnIndex + 1), LocateBy.XPath);
        }

        /// <summary>
        /// both row and column indexes are 0 based
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="expectedValue"></param>
        [Verify]
        public void VerifyReportValue(int rowIndex, int columnIndex, string expectedValue)
        {
            VerifyTableReport(ReportTableID, rowIndex, columnIndex, expectedValue);
        }

        /// <summary>
        /// Verifies the Total Charges and Balance due of a registration in the registrant list report. 
        /// </summary>
        /// <param name="regId"></param>
        /// <param name="expectedTotalCharge"></param>
        /// <param name="expectedBalanceDue"></param>
        public void VerifyReportTotalChargeAndBalanceDueByRegId(int regId, double expectedTotalCharge, double expectedBalanceDue)
        {
            string locator_XPath = string.Format("//b[text()='{0}']/..", regId);
            UIUtilityProvider.UIHelper.WaitForElementPresent(locator_XPath, LocateBy.XPath);
            string actualValueTotalCharge = UIUtilityProvider.UIHelper.GetText(locator_XPath + "/../following-sibling::td[5]", LocateBy.XPath);
            VerifyTool.VerifyValue(MoneyTool.FormatMoney(expectedTotalCharge), actualValueTotalCharge, "Total Charge : {0}");
            string actualValueBalanceDue = UIUtilityProvider.UIHelper.GetText(locator_XPath + "/../following-sibling::td[6]", LocateBy.XPath);
            VerifyTool.VerifyValue(MoneyTool.FormatMoney(expectedBalanceDue), actualValueBalanceDue, "Balance Due: {0}");
        }

        public void VerifyEnduranceReportTotalChargeAndBalanceDueByRegId(
            int eventId, 
            int regId, 
            double expectedTotalCharge, 
            double expectedBalanceDue, 
            MoneyTool.CurrencyCode currencySymbol = MoneyTool.CurrencyCode.USD)
        {
            string locator_XPath = string.Format("//b[text()='{0}']/..", regId);
            UIUtilityProvider.UIHelper.WaitForElementPresent(locator_XPath, LocateBy.XPath);
            string actualValueTotalCharge = UIUtilityProvider.UIHelper.GetText(locator_XPath + "/../following-sibling::td[8]", LocateBy.XPath);

            VerifyTool.VerifyValue(MoneyTool.FormatMoney(expectedTotalCharge, currencySymbol), actualValueTotalCharge, "Total Charge : {0}");
            string actualValueBalanceDue = UIUtilityProvider.UIHelper.GetText(locator_XPath + "/../following-sibling::td[9]", LocateBy.XPath);
            VerifyTool.VerifyValue(MoneyTool.FormatMoney(expectedBalanceDue, currencySymbol), actualValueBalanceDue, "Balance Due: {0}");
        }

        /// <summary>
        /// both row and column indexes are 0 based
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="expectedValue"></param>
        public void VerifyTableReport(string tableId, int rowIndex, int columnIndex, string expectedValue)
        {
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            //make sure the report table exists
            Assert.IsTrue(UIUtilityProvider.UIHelper.IsElementPresent(string.Format("//table[@id='{0}']", tableId), LocateBy.XPath));
            
            //wait for the value to appear (it is important for the sorting)
            //string script = "return document.getElementById('{0}').getElementsByTagName('tr')[{1}].getElementsByTagName('td')[{2}].innerHTML.indexOf('{3}') > -1 ";
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TimeOutSpan));
            //wait.Until(d => (IJavaScriptExecutor)d).ExecuteScript(string.Format(script, tableId, rowIndex, columnIndex, expectedValue));

            Assert.AreEqual(expectedValue, UIUtilityProvider.UIHelper.GetTable(tableId, rowIndex, columnIndex));
        }

        /// <summary>
        /// This method expands the transaction fees report row to show more detailed info. 
        /// </summary>
        /// <param name="regId"></param>
        public void ExpandTransactionFeesReportRow(int regId)
        {
            string locator_XPath = string.Format("//td[text()='{0}']/..//input", regId);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator_XPath, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            Utility.ThreadSleep(1);
        }

        /// <summary>
        /// This verifies data in the expanded or second row in the transaction fees report. 
        /// transaction fees report the data it is looking is this: 
        /// Total Amount Collected, TransactionFees, Registrants in group, Total per Reg Fee,
        /// Net Amount Due/Owed, Transaction %, Transaction Fee, Per Reg Fee, Group Count, Total fees,
        /// % passed to participant, total paid by participant, fees due. 
        /// </summary>
        /// <param name="regId"></param>
        /// <param name="expectedFees"></param>

        public void VerifyExpandedTransactionFeeRowData(int regId, string[] expectedFees)
        {
            int f = 5;
            int columnCount = UIUtilityProvider.UIHelper.GetXPathCountByXPath(string.Format(TransactionsFeesReportXPathCount, regId));
            int rowCount = UIUtilityProvider.UIHelper.GetXPathCountByXPath(string.Format(ExpandedTransactionFeesReportLocator, regId));
            string[] actualData = new string[expectedFees.Length];

            for (int i = 0; i < 5; i++)
            {
                actualData[i] = GetTransactionFeesMainRowData(regId, (i + 4));
            }

            while (f < expectedFees.Length)
            {
                for (int q = 1; q <= rowCount; q++)
                {
                    for (int i = 3; i <= columnCount; i++)
                    {
                        actualData[f] = GetTransactionFeesExpandedRowData(regId, q, i);
                        f++;
                    }
                }
            }

            for (int i = 0; i < expectedFees.Length; i++)
            {
                VerifyTool.VerifyValue(expectedFees[i], actualData[i], "{0}");
            }
        }

        /// <summary>
        /// This gets the data in the main, or first row, in the tranasactions fees report.
        /// It returns one cell of data at a time, based on the locator i. 
        ///
        /// It will look at this data: 
        /// Total Amount Collected, TransactionFees, Registrants in group, Total per Reg Fee,
        /// Net Amount Due/Owed
        /// </summary>
        /// <param name="regId"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetTransactionFeesMainRowData(int regId, int i)
        {
            string locator_XPath = string.Format("//td[text()='{0}']/../td[" + i + "]", regId);
            string data = UIUtilityProvider.UIHelper.GetText(locator_XPath, LocateBy.XPath);
            return data;
        }

        /// <summary>
        /// This gets the data in the expanded, or second row, in the tranasactions fees report.
        /// It returns one cell of data at a time, based on the locator i. 
        ///
        /// It will look at this data: 
        /// Transaction %, Transaction Fee, Per Reg Fee, Group Count, Total fees,
        /// % passed to participant, total paid by participant, fees due.
        /// <param name="regId"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetTransactionFeesExpandedRowData(int regId, int q, int i)
        {
            string locator_XPath = string.Format("//td[text()='{0}']/../following-sibling::tr[1]//tbody/tr[" + q + "]/td[" + (i) + "]",regId);
            string data = UIUtilityProvider.UIHelper.GetText(locator_XPath, LocateBy.XPath);
            return data;
        }

        /// <summary>
        /// Gets and verifies Type, Amount, and Running Balance for all rows of data per registrant
        /// on the transactions report. 
        /// 
        /// Note: that because what shows on this report is so dependant on what you are doing,
        /// you will need to provide an array with the information you want to verify.
        /// </summary>
        /// <param name="regId"></param>
        /// <param name="expectedData"></param>
        public void VerifyTransactionsReportData(int regId, string[] expectedData)
        {
            string[] actualData = new string[expectedData.Length];
            int f = 0;
            int rowCount = UIUtilityProvider.UIHelper.GetXPathCountByXPath(string.Format(TransactionsReportLocator, regId));
            string systemNotesLocator = "//*[@id='Table1']/tbody/tr[td//text()='" + regId + "'][{0}]/td[{1}]";
            string convertedAmountLocator = "//*[@id='Table1']/tbody/tr[td//text()='" + regId + "'][{0}]/td[8]";
            string xPathCountLocator = "//*[@id='Table1']/tbody/tr[td//text()='" + regId + "'][1]/td";
            string xPathCount = UIUtilityProvider.UIHelper.GetXPathCountByXPath(xPathCountLocator).ToString();

            while (f < (actualData.Length - 1))
            {
                for (int q = 0; q < rowCount; q++)
                {
                    string watchMe = UIUtilityProvider.UIHelper.GetText(string.Format(systemNotesLocator, q + 1, xPathCount), LocateBy.XPath);
                    string convertedAmtWatch = UIUtilityProvider.UIHelper.GetText(string.Format(convertedAmountLocator, q + 1), LocateBy.XPath);

                    for (int i = 5; i < 8; i++)
                    {
                        actualData[f] = UIUtilityProvider.UIHelper.GetText("//*[@id='Table1']/tbody/tr[td//text()='" + regId + "'][" + (q + 1) + "]/td[" + i + "]", LocateBy.XPath);
                        f++;
                        if (xPathCount== "11" && i == 7 && convertedAmtWatch != "")
                        {
                            actualData[f] = convertedAmtWatch;
                            f++;
                        }
                        if (watchMe != "" && i == 7)
                        {
                            actualData[f] = watchMe;
                            f++;
                        }
                    }
                }
            }

            actualData[expectedData.Length - 1] = UIUtilityProvider.UIHelper.GetText(string.Format(systemNotesLocator, rowCount, xPathCount), LocateBy.XPath);

            for (f = 0; f < actualData.Length-1; f++)
            {
                VerifyTool.VerifyValue(expectedData[f], actualData[f], "Array element " + f + " = {0}");
            }
        }

        #region helpers
        private int GetReportTableRowCount()
        {
            string reportTableLocator = "//table[@id='" + ReportTableID + "']";

            // make sure the report table exists
            try
            {
                UIUtilityProvider.UIHelper.WaitForElementPresent(reportTableLocator, LocateBy.XPath);
            }
            catch
            {
                Assert.Fail("Report table not found!");
            }

            string locator_B_NoRecordsFound = "//*[@id='rptCell']//*[text()='No Records Found']";

            if (UIUtilityProvider.UIHelper.IsElementDisplay(locator_B_NoRecordsFound, LocateBy.XPath))
            {
                return 0;
            }

            return UIUtilityProvider.UIHelper.GetXPathCountByXPath(reportTableLocator + "/tbody/tr");
        }

        [Verify]
        public void VerifyReportTableRowCount(int expectedRowCount)
        {
            int numRows = this.GetReportTableRowCount();
            VerifyTool.VerifyValue(expectedRowCount, numRows, "Row count: {0}");
        }

        public void VerifyAttendeeRowIndex(int regId, int index)
        {
            string expectedLocation = FormatAttendeeRowIndexLocator(index);
            string actualLocation = GetAttendeeRowIndex(regId);
            Assert.AreEqual(expectedLocation, actualLocation);
        }

        public string FormatAttendeeRowIndexLocator(int index)
        {
            string locator = string.Empty;

            if (index < 10)
            {
                locator = string.Format(VerifyAttendeeRowIndexLocator, "0" + index);
            }
            else
            {
                locator = string.Format(VerifyAttendeeRowIndexLocator, "1" + index);
            }

            return locator;
        }

        public string GetAttendeeRowIndex(int regId)
        {
            return UIUtilityProvider.UIHelper.GetAttribute(string.Format(GetAttendeeRowIndexLocator, regId), "id", LocateBy.XPath);
        }

        [Step]
        public void UndoCheckIn(string connectionString, int RegisterId)
        {
            //B.Data.DataProvider.ExecuteNonQuery(connectionString, System.Data.CommandType.Text, "checkOutRegistrant " + RegisterId.ToString());
            ClientDataContext db = new ClientDataContext();
            Registration reg = (from r in db.Registrations where r.Register_Id == RegisterId select r).Single();
            reg.StatusId = 2;
            db.SubmitChanges();
        }

        [Step]
        public void SetRegistrationStatus(int RegisterId, int StatusId, Event evt)
        {
            ClientDataContext db = new ClientDataContext();
            Registration reg = (from r in db.Registrations where r.Event == evt && r.Register_Id == RegisterId select r).Single();
            reg.StatusId = StatusId;
            db.SubmitChanges();
            //C.Registration reg = C.Registration.GetByRegistrationIDEventID(RegisterId, evt);
            //reg.UpdateStatus(RegisterId, (C.RegistrationStatusType)StatusId);
        }

        #endregion

        [Verify]
        public void VerifyDecryptedCCNumberPresent()
        {
            if (!UIUtilityProvider.UIHelper.IsTextPresent(PaymentManager.DefaultPaymentInfo.CCNumber))
            {
                UIUtilityProvider.UIHelper.FailTest("Decrypted CC number not present!");
            }
        }

        [Verify]
        public void VerifyEncryptedCCNumberPresent()
        {
            if (!UIUtilityProvider.UIHelper.IsTextPresent(Managers.Register.PaymentManager.DefaultPaymentInfo.CCNumber_Encrypted))
            {
                UIUtilityProvider.UIHelper.FailTest("Encrypted CC number not present!");
            }
        }

        public void VerifyGroupBy(string groupBy)
        {
            string groupByLocator = string.Format(GroupByLocator, groupBy);
            UIUtilityProvider.UIHelper.IsElementPresent(groupByLocator, LocateBy.XPath);
        }
    }
}
