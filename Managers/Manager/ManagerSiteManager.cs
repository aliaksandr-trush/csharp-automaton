namespace RegOnline.RegressionTest.Managers.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager.Account;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class ManagerSiteManager : ManagerBase
    {
        private const int EventExpiredTimeSpanInMinutes = 30;
        private const string TryRegonlineButton = "ctl00_cphMaster_hlCreateAccount";
        private const string FolderName_xAuth = "xAuth";
        private const string LoginUserNameLocator = "ctl00_cphMaster_txtLogin";
        private const string LoginPasswordLocator = "ctl00_cphMaster_txtPassword";
        private const string LoginButtonLocator = "//span[@class='BiggerButtonBase']/a";
        private const string LogoutLinkLocator = "ctl00_ctl00_hplLogout";
        private const string TabLocatorFormat = "//div[@id='ctl00_ctl00_cphDialog_RadMainManu']//span[text()='{0}']";
        private const string FolderLocatorFormat = "//div[@id='tree']//span[text()='{0}']";
        private const string AddEventDropDown = "//div[@id='createNewEvent']//img[@class='rmLeftImage'][1]";
        private const string AddEventDropDownType = "//div[@id='createNewEvent']//span[text()='{0}']";
        private const string EventExistsLocator = "//*[text()='{0}']/..";
        private const string HeaderAccountLocator = "ctl00_ctl00_hplAccount";
        private const string TemplateNameLocator = "//option[contains(text(),'{0}')]";
        
        private const string FirstEventLocator = 
            "//tr[@id='ctl00_ctl00_cphDialog_cpMgrMain_rdgrdgrdForms_ctl00__0']//a[@class='listEventTile']";

        private const string Locator_Div_EventListGridHeader = "ctl00_ctl00_cphDialog_cpMgrMain_rdgrdgrdForms_GridHeader";
        private const string Locator_Div_EventListGridData = "ctl00_ctl00_cphDialog_cpMgrMain_rdgrdgrdForms_GridData";

        private readonly string locatorFormat_A_EventIdColumnHeader_ClickToSort =
            string.Format("//div[@id='{0}']/table/thead/tr/th/", Locator_Div_EventListGridHeader) +
            "a[@title='Click here to sort' and text()='{0}']";

        private string ManagerSiteUrlFormat
        {
            get
            {
                return ConfigReader.DefaultProvider.AccountConfiguration.BaseUrlWithHttps + "manager/Forms/?EventSessionID={0}";
            }
        }

        public struct ErrorMessage
        {
            public const string ForgotLoginEmailSent =
                "An email was sent to you containing information about your login.";

            public const string ForgotLoginEmailNotFound =
                "A login associated with that email address was not found.";

            public const string ForgotPasswordEmailSent =
                "An email has been sent to your account with instructions to reset your password.";

            public const string ForgotPasswordProblemOccur = "There was a problem resetting your password.";
        }

        public enum EventListGridColumnHeader
        {
            ID,
            Title
        }

        public enum SortEventList
        {
            Asc,
            Desc
        }

        public enum EventType
        {
            [StringValue("Event")]
            ActiveEuropeEvent,

            [StringValue("Pro Event")]
            ProEvent,

            [StringValue("Express Event")]
            ExpressEvent,

            [StringValue("Lite Event")]
            LiteEvent,

            [StringValue("Membership")]
            Membership,

            [StringValue("Web Event")]
            WebEvent,

            [StringValue("Survey")]
            Survey,

            [StringValue("Donation Form")]
            DonationForm,

            [StringValue("Create from Template")]
            CreateFromTemplate
        }

        public enum Tab
        {
            [StringValue("Events")]
            Events,

            [StringValue("Email Invitations")]
            EmailInvitations,

            [StringValue("Account")]
            Account
        }

        private AccountManager _accountMgr;
        public AccountManager AccountMgr
        {
            get { return _accountMgr; }
            set { _accountMgr = value; }
        }

        private DashboardManager _dashboardMgr;
        public DashboardManager DashboardMgr
        {
            get { return _dashboardMgr; }
            set { _dashboardMgr = value; }
        }

        private GetStartedManager _getStartedMgr;
        public GetStartedManager GetStartedMgr
        {
            get { return _getStartedMgr; }
            set { _getStartedMgr = value; }
        }

        private CreateNewManager _createNewMgr;
        public CreateNewManager CreateNewMgr
        {
            get { return _createNewMgr; }
            set { _createNewMgr = value; }
        }

        public SearchManager SearchMgr
        {
            get;
            private set;
        }

        public ManagerSiteManager()
        {
            this._accountMgr = new AccountManager();
            this._dashboardMgr = new DashboardManager();
            this.BibNumberTool = new BibNumberingToolManager();
            this._getStartedMgr = new GetStartedManager();
            this._createNewMgr = new CreateNewManager();
            this.SearchMgr = new SearchManager();
        }

        [Step]
        public void OpenLogin()
        {
            UIUtil.DefaultProvider.OpenUrl(string.Format("{0}manager/login.aspx", ConfigReader.DefaultProvider.AccountConfiguration.BaseUrlWithHttps));
            AllowCookies();
        }

        [Step]
        public string Login()
        {
            this.Login(ConfigReader.DefaultProvider.AccountConfiguration.Login, ConfigReader.DefaultProvider.AccountConfiguration.Password);
            string eventSessionId = this.GetEventSessionId();
            this.OpenManagerURL(eventSessionId);
            return eventSessionId;
        }

        [Step]
        public void Login(string username, string password)
        {
            UIUtil.DefaultProvider.Type(LoginUserNameLocator, username, LocateBy.Id);
            UIUtil.DefaultProvider.Type(LoginPasswordLocator, password, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(LoginButtonLocator, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public void Logout()
        {
           UIUtil.DefaultProvider.WaitForDisplayAndClick(LogoutLinkLocator, LocateBy.Id);
        }

        [Step]
        public void ClickEditUserLink()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_ctl00_hpEditUser", LocateBy.Id);
            Utility.ThreadSleep(2);
            UIUtil.DefaultProvider.SelectPopUpFrameByName("plain");
        }

        [Step]
        public void SetSuperadmin(string eventSessionId, bool enable)
        {
            ROMasterDataContext db = new ROMasterDataContext();
            Session session = (from s in db.Sessions where s.Id == eventSessionId select s).Single();
            session.RegonlineSuperAdmin = enable;
            db.SubmitChanges();
        }

        [Verify]
        public void VerifyInvalidLogin()
        {
            Assert.That(UIUtil.DefaultProvider.IsElementPresent("//div[@class='warningMessageFluid']", LocateBy.XPath));
        }

        [Verify]
        public void VerifyCaptchaVisible()
        {
            Assert.That(UIUtil.DefaultProvider.IsElementPresent("//div[@class='captcha']", LocateBy.XPath));
        }

        [Step]
        public void SelectAndEnterForgotPassword(string username)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//a[text()='Password']", LocateBy.XPath);
            UIUtil.DefaultProvider.Type("//label[@for='txtUsername']/input", username, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//div[@id='password']//span[@class='BiggerButtonBase']/a/span[text()='Submit']", LocateBy.XPath);
        }

        [Step]
        public void ClickOnFirstEvent()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(FirstEventLocator, LocateBy.XPath);
        }

        [Step]
        public void SelectAndEnterForgotLogin(string email)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//a[text()='Username']", LocateBy.XPath);
            UIUtil.DefaultProvider.Type("//label[@for='txtEmail']/input", email, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//div[@id='username']//span[@class='BiggerButtonBase']/a/span[text()='Submit']", LocateBy.XPath);
        }

        public void OpenManagerURL(string eventSessionId)
        {
            UIUtil.DefaultProvider.OpenUrl(this.GetManagerURL(eventSessionId));
        }

        [Step]
        public void OpenEventBuilderStartPage(int eventId, string eventSessionId)
        {
            UIUtil.DefaultProvider.OpenUrl(string.Format(
                "{0}Builder/default.aspx?EventId={1}&EventSessionID={2}",
                ConfigReader.DefaultProvider.AccountConfiguration.BaseUrlWithHttps,
                eventId,
                eventSessionId));
        }

        [Verify]
        public void VerifyErrorMessage(string errorMessage)
        {
            VerifyTool.VerifyValue(errorMessage, UIUtil.DefaultProvider.GetText("ctl00_cphMaster_warning", LocateBy.Id), "Error message: {0}");
        }

        public string GetManagerURL(string eventSessionID)
        {
            return string.Format(ManagerSiteUrlFormat, eventSessionID);
        }

        [Verify]
        public void VerifyManager()
        {
            Assert.That(UIUtil.DefaultProvider.IsElementPresent("//td[@id='mgrHeader']", LocateBy.XPath) || UIUtil.DefaultProvider.IsElementPresent("//div[@class='m3logo']", LocateBy.XPath));
        }

        public void VerifyTextPresent(string text)
        {
            Assert.That(UIUtil.DefaultProvider.IsTextPresent(text));
        }

        public void SwitchToSubAccount(int customerId)
        {
            string otherAccountDropdown = "ctl00_cphDialog_ddOtherAccounts";
            string optionLocator = string.Format("value={0}", customerId);
            string accountText = string.Format("Account: {0}", customerId);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(HeaderAccountLocator, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForElementPresent(otherAccountDropdown, LocateBy.Id);
            UIUtil.DefaultProvider.SelectWithText(otherAccountDropdown, optionLocator, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForDisplayAndClick("OK", LocateBy.LinkText);
            Utility.ThreadSleep(2);
            Assert.AreEqual(accountText, UIUtil.DefaultProvider.GetText(HeaderAccountLocator, LocateBy.Id));
        }

        /// <summary>
        /// Extract the current [Customers].[ID] from the header
        /// </summary>
        /// <returns>Customer ID</returns>
        public int GetCurrentAccountId()
        {
            string accountHeader = string.Empty;

            if (UIUtil.DefaultProvider.IsElementDisplay(HeaderAccountLocator, LocateBy.Id))
            {
                accountHeader = UIUtil.DefaultProvider.GetText(HeaderAccountLocator, LocateBy.Id);
            }
            else
            {
                string tmp = UIUtil.DefaultProvider.GetText("//div[@class='titleBar']/ul/li", LocateBy.XPath).Split(
                    new char[] { ':' })[1];

                accountHeader = tmp.Split(new char[] { ')' })[0].Trim();
            }

            int accountId;

            if (!int.TryParse(accountHeader, out accountId))
            {
                throw new System.Exception("Problem parsing Account number from Header");
            }

            return accountId;
        }

        [Step]
        public void SelectFolder_xAuth()
        {
            this.SelectFolder(FolderName_xAuth);
        }

        [Step]
        public void SelectFolder(string folderName)
        {
            ////UIUtilityProvider.UIHelper.WaitForPageToLoad();
            string folderLocator = string.Format(FolderLocatorFormat, folderName);
            UIUtil.DefaultProvider.WaitForElementDisplay(folderLocator, LocateBy.XPath);
            string folderDivClassAttribute = UIUtil.DefaultProvider.GetAttribute(string.Format("{0}/parent::div", folderLocator), "class", LocateBy.XPath);

            if (!folderDivClassAttribute.Equals("rtMid rtSelected"))
            {
                UIUtil.DefaultProvider.WaitForDisplayAndClick(folderLocator, LocateBy.XPath);
                UIUtil.DefaultProvider.WaitForAJAXRequest();
                Utility.ThreadSleep(1);
            }
        }

        [Step]
        public void SelectFolder()
        {
            this.SelectFolder(ConfigReader.DefaultProvider.AccountConfiguration.Folder);
        }

        public void SelectTemplatesFolder()
        {
            this.SelectFolder("Templates");
        }

        public void SelectDeletedEventsFolder()
        {
            this.SelectFolder("Deleted Events");
        }

        [Step]
        public void ClickAddEvent(EventType eventType)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(AddEventDropDown, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForAJAXRequest();

            switch (eventType)
            {
                case EventType.ActiveEuropeEvent:
                case EventType.ProEvent:
                case EventType.DonationForm:
                case EventType.ExpressEvent:
                case EventType.LiteEvent:
                case EventType.Membership:
                case EventType.Survey:
                case EventType.WebEvent:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format(AddEventDropDownType, StringEnum.GetStringValue(eventType)), LocateBy.XPath);
                    UIUtil.DefaultProvider.WaitForPageToLoad();
                    break;
                case EventType.CreateFromTemplate:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format(AddEventDropDownType, StringEnum.GetStringValue(eventType)), LocateBy.XPath);
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
            }

            UIUtil.DefaultProvider.HideActiveSpecificFooter(true);
        }

        public void SelectTemplateToCreate(string eventName)
        {
            string option = UIUtil.DefaultProvider.GetAttribute(string.Format(TemplateNameLocator, eventName), "value", LocateBy.XPath);
            UIUtil.DefaultProvider.SelectWithText("//select[@id='lbTemplates']", eventName + " (" + option + ") - Account #" + Convert.ToInt32(ConfigReader.DefaultProvider.AccountConfiguration.Id), LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//a/span[text()='OK']/..", LocateBy.XPath);
        }

        [Step]
        public void GotoTab(Tab tab)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(this.ComposeTabLocator(tab), LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForPageToLoad();
            UIUtil.DefaultProvider.HideActiveSpecificFooter(true);
        }

        private bool IsTabSelected(Tab tab)
        {
            string tabLinkLocator = string.Format("{0}/parent::span/parent::span/parent::a", this.ComposeTabLocator(tab));
            string tabLinkClassAttribute = UIUtil.DefaultProvider.GetAttribute(tabLinkLocator, "class", LocateBy.XPath);

            if (tabLinkClassAttribute.Equals("rtsLink rtsSelected"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string ComposeTabLocator(Tab tab)
        {
            return string.Format(TabLocatorFormat, StringEnum.GetStringValue(tab));
        }

        [Step]
        public void GoToEventsTabIfNeeded()
        {
            if (!this.IsTabSelected(Tab.Events))
            {
                this.GotoTab(Tab.Events);
            }
        }

        [Step]
        public void GoToEmailTabIfNeeded()
        {
            if (!this.IsTabSelected(Tab.EmailInvitations))
            {
                this.GotoTab(Tab.EmailInvitations);
            }
        }

        [Step]
        public void GoToAccountTabIfNeeded()
        {
            if (!this.IsTabSelected(Tab.Account))
            {
                this.GotoTab(Tab.Account);
            }
        }

        [Step]
        public bool EventExists(string name)
        {
            bool result = false;

            result = UIUtil.DefaultProvider.IsElementPresent(string.Format(EventExistsLocator, name), LocateBy.XPath);

            return result;
        }

        [Step]
        public List<int> GetEventIds(string name)
        {
            // Sort by Title column so that events with the same title are listed sequentially
            this.EnsureSortByColumnHeader(EventListGridColumnHeader.Title, SortEventList.Asc);

            List<int> ids = new List<int>();

            string locator_A_EventTitle = string.Format(
                "//div[@id='{0}']/table/tbody/tr/td/a[text()='{1}']",
                Locator_Div_EventListGridData,
                name);

            if (UIUtil.DefaultProvider.IsElementPresent(locator_A_EventTitle, LocateBy.XPath))
            {
                int eventCount = UIUtil.DefaultProvider.GetXPathCountByXPath(locator_A_EventTitle);

                // Retrieve the first id
                string locator_Tr_EventRow = string.Format("{0}/parent::td/parent::tr", locator_A_EventTitle);
                string eventId = UIUtil.DefaultProvider.GetAttribute(locator_Tr_EventRow, "data-id", LocateBy.XPath);
                ids.Add(Convert.ToInt32(eventId));

                for (int cnt = 0; cnt < eventCount - 1; cnt++)
                {
                    string locator_Tr_EventRow_Following = string.Format(
                        "{0}/following-sibling::tr[{1}]",
                        locator_Tr_EventRow,
                        cnt + 1);

                    eventId = UIUtil.DefaultProvider.GetAttribute(locator_Tr_EventRow_Following, "data-id", LocateBy.XPath);
                    ids.Add(Convert.ToInt32(eventId));
                }
            }

            return ids;
        }

        private DateTime GetEventCreatedTime(int id)
        {
            //get created time
            //S.EventsService ets = new S.EventsService();
            //E.Events et = ets.GetById(id);
            ClientDataContext db = new ClientDataContext();
            Event et = (from e in db.Events where e.Id == id select e).Single();
            return et.Add_Date;
        }

        public int GetEventRegCount(int eventId)
        {
            string locatorFormat =
                "//table[@id='ctl00_ctl00_cphDialog_cpMgrMain_rdgrdgrdForms_ctl00']/tbody/tr[@data-id='{0}']/td/a[contains(@id,'aRegs') and contains(@href,'{0}')]";

            return Convert.ToInt32(UIUtil.DefaultProvider.GetText(string.Format(locatorFormat, eventId), LocateBy.XPath));
        }

        [Step]
        public void DeleteExpiredDuplicateEvents(string eventName)
        {
            this.DeleteExpiredDuplicateEvents(eventName, EventExpiredTimeSpanInMinutes);
        }

        [Step]
        public void DeleteExpiredDuplicateEvents(string name, int expireMinutes)
        {
            DateTime createdTime = new DateTime();

            if (string.IsNullOrEmpty(name) || expireMinutes < 0)
            {
                throw new ArgumentException("The name can't be null or empty. Time span can't be negative.");
            }

            List<int> ids = GetEventIds(name);

            if (ids.Count > 0)
            {
                int maxid = 0;

                foreach (int id in ids)
                {
                    if (id > maxid)
                    {
                        DeleteEventById(maxid);
                        maxid = Convert.ToInt32(id);
                    }
                    else if (Convert.ToInt32(id) < maxid)
                    {
                        DeleteEventById(Convert.ToInt32(id));
                    }
                }

                if (expireMinutes >= 0)
                {
                    createdTime = GetEventCreatedTime(Convert.ToInt32(maxid));

                    if (((int)(DateTimeTool.ConvertToRegOnlineTime(DateTime.Now) - createdTime).TotalMinutes) > expireMinutes)
                    {
                        DeleteEventById(maxid);
                    }
                }
            }

            // Refresh the page to avoid dirty data,
            // cause we don't drag the event to Delete folder, but move the event to that folder directly
            UIUtil.DefaultProvider.RefreshPage();
        }

        [Step]
        private void DeleteEventById(int id)
        {
            if (id <= 0)
            {
                return;
            }

            this.ClickDeleteEventInActionsColumn(id);
            DataHelper helper = new DataHelper();
            helper.HideEventsInDeletedFolder();
        }

        private void ClickDeleteEventInActionsColumn(int eventId)
        {
            string locator_A_DeleteLinkInActionsColumn = string.Format(
                "//div[@id='{0}']/table/tbody/tr[@data-id='{1}']/td/div[@class='actions']/a[contains(@onclick, 'deleteEvent')]",
                Locator_Div_EventListGridData,
                eventId);

            UIUtil.DefaultProvider.WaitForDisplayAndClick(locator_A_DeleteLinkInActionsColumn, LocateBy.XPath);
            Utility.ThreadSleep(0.5);
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//span[text()='OK']", LocateBy.XPath);
            UIUtil.DefaultProvider.SelectOriginalWindow();
            Utility.ThreadSleep(2);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        [Step]
        public int GetFirstEventId(string name)
        {
            int idAsInt = 0;

            if (EventExists(name))
            {
                this.EnsureSortByColumnHeader(EventListGridColumnHeader.ID, SortEventList.Desc);

                string eventIdAttributeLocator = "//*[@title='{0} - Details']/../..";
                string idAsString = UIUtil.DefaultProvider.GetAttribute(string.Format(eventIdAttributeLocator, name), "data-id", LocateBy.XPath);

                int.TryParse(idAsString, out idAsInt);
            }
            else
            {
                throw new Exception(string.Format("No such event: '{0}'", name));
            }

            return idAsInt;
        }

        private void EnsureSortByColumnHeader(EventListGridColumnHeader columnHeader, SortEventList sort)
        {
            string locator_Input_SortArrow = string.Format(
                "{0}/following-sibling::input",
                this.ComposeEventListGridColumnHeader_ClickToSortLink(columnHeader));

            if (!UIUtil.DefaultProvider.IsElementDisplay(locator_Input_SortArrow, LocateBy.XPath))
            {
                this.ClickEventListGridColumnHeaderToSort(columnHeader);
            }

            if (!UIUtil.DefaultProvider.GetAttribute(locator_Input_SortArrow, "class", LocateBy.XPath).Equals(
                string.Format("rgSort{0}", sort.ToString())))
            {
                this.ClickEventListGridColumnHeaderToSort(columnHeader);
            }
        }

        private void ClickEventListGridColumnHeaderToSort(EventListGridColumnHeader columnHeader)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(
                this.ComposeEventListGridColumnHeader_ClickToSortLink(columnHeader),
                LocateBy.XPath);

            Utility.ThreadSleep(2);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        private string ComposeEventListGridColumnHeader_ClickToSortLink(EventListGridColumnHeader columnHeader)
        {
            return string.Format(locatorFormat_A_EventIdColumnHeader_ClickToSort, columnHeader.ToString());
        }

        [Step]
        public void DeleteEventByName(string name)
        {
            if (!EventExists(name))
            {
                return;
            }

            List<int> ids = this.GetEventIds(name);

            for (int i = 0; i < ids.Count; i++)
            {
                this.DeleteEventById(ids[i]);
            }

            UIUtil.DefaultProvider.RefreshPage();
        }

        [Step]
        public void CopyEventByName(string name)
        {
            string copyEventLinkLocator = string.Format("//td/a[text()='{0}']/../..//a[@title='Copy event']", name);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(copyEventLinkLocator, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//span[text()='OK']/../..", LocateBy.XPath);
            UIUtil.DefaultProvider.SwitchToMainContent();
            Utility.ThreadSleep(6);
        }

        [Step]
        public void CopyEventById(int id)
        {
            string copyEventLinkLocator = string.Format("//td[text()='{0}']/..//a[@title='Copy event']", id.ToString());
            UIUtil.DefaultProvider.WaitForDisplayAndClick(copyEventLinkLocator, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//span[text()='OK']/../..", LocateBy.XPath);
            UIUtil.DefaultProvider.SwitchToMainContent();
            Utility.ThreadSleep(6);
        }

        public void ClickEditRegistrationForm(string name)
        {
            string editEventLinkLocator = string.Format("//td/a[text()='{0}']/../..//a[@title='Edit registration form']", name);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(editEventLinkLocator, LocateBy.XPath);
        }

        public void ClickEditRegistrationForm(int id)
        {
            string editEventLinkLocator = string.Format("//td[text()='{0}']/..//a[@title='Edit registration form']", id.ToString());
            UIUtil.DefaultProvider.WaitForDisplayAndClick(editEventLinkLocator, LocateBy.XPath);
        }
        /// <summary>
        /// Opens dashboard for first form with this name in grid
        /// Assumes form of that name exists
        /// </summary>
        /// <param name="formName"></param>
        [Step]
        public void OpenEventDashboard(string formName)
        {
            string locator = string.Format("//td[a='{0}']/a", formName);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(locator, LocateBy.XPath);
        }

        [Step]
        public void OpenEventDashboard(int eventID)
        {
            //string dashboardLinkLocator = string.Format(
            //"table[@id='ctl00_ctl00_cphDialog_cpMgrMain_rdgrdgrdForms_ctl00']/tbody/tr[@data-id='{0}']/td[@class='rgSorted']/a", 
            //eventID.ToString());
            string dashboardLinkLocator = string.Format("//a[contains(@href, 'eventID={0}')]", eventID.ToString());

            UIUtil.DefaultProvider.WaitForDisplayAndClick(dashboardLinkLocator, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        [Step]
        public void OpenEventDashboardUrl(int eventID, string sessionID)
        {
            string dashboardUrl = string.Format(
                ConfigReader.DefaultProvider.AccountConfiguration.BaseUrlWithHttps + "manager/Forms/details.aspx?EventSessionID={0}&eventID={1}",
                sessionID,
                eventID);

            UIUtil.DefaultProvider.OpenUrl(dashboardUrl);
        }

        public void OpenAttendeeReportFromManagerEventList(int eventID)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(this.ComposeAttendeeReportLink(eventID), LocateBy.XPath);
            UIUtil.DefaultProvider.SelectTopWindow();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        // Get an event id to open the attendee report on the manager page
        private string ComposeANonZeroAttendeeReportLink()
        {
            //string locator = "//a[contains(@href, javascript:AttendeeReportWithoutCancel)]";
            //int eventCount = U.ConversionTools.ConvertToInt(GetXPathCount(locator));
            //int count = 1;

            //while (count <= eventCount)
            //{
            //    if (GetText(locator + "[" + count + "]") != "0")
            //    {
            //        break;
            //    }

            //    count++;
            //}

            //this.currentEventID = U.ConversionTools.ConvertToInt(GetText(locator + "[" + count + "]/../../td[1]"));
            //return locator + "[" + count + "]";

            string locator = "//a[contains(@href, \"javascript:AttendeeReportWithoutCancel\") and text()!='0']";
            ////this.CurrentEventID = U.ConversionTools.ConvertToInt(GetText(locator + "/../../td[1]"));
            return locator;
        }

        private string ComposeAttendeeReportLink(int eventID)
        {
            ////this.CurrentEventID = eventID;
            string locatorFormat = "//a[contains(@href, \"javascript:AttendeeReportWithoutCancel({0},'{1}')\")]";
            return string.Format(locatorFormat, eventID, GetEventSessionId());
        }

        public void OpenJobHistoryReport()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Jobs History Report", LocateBy.LinkText);
        }

        public void OpenUnsubscribedReport()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Unsubscribed Report", LocateBy.LinkText);
        }

        /// <summary>
        /// delete all the test reg from the step "Login Manager"
        /// </summary>
        [Step]
        public string LoginAndDeleteTestRegsReturnToManagerScreen(int eventId, string folderName)
        {
            OpenLogin();
            Login();
            GoToEventsTabIfNeeded();
            SelectFolder(folderName);
            string sessionId = GetEventSessionId();
            OpenEventDashboardUrl(eventId, sessionId);
            DashboardMgr.ChooseTabAndVerify(DashboardManager.DashboardTab.EventDetails);
            DashboardMgr.ClickOption(DashboardManager.EventRegistrationFunction.DeleteTestRegistrations);
            UIUtil.DefaultProvider.SelectPopUpFrameByName("plain");
            DashboardMgr.DeleteTestReg_ClickDelete();
            UIUtil.DefaultProvider.SwitchToMainContent();
            DashboardMgr.ReturnToList();
            return sessionId;
        }

        public void OpenEventDashboardAndDeleteTestRegsAndReturnToManagerScreen(int eventId)
        {
            this.OpenEventDashboard(eventId);
            this.DashboardMgr.DeleteTestRegs();
            this.DashboardMgr.ReturnToList();
        }

        public void OpenEventDashboardUrlAndDeleteTestRegsAndReturnToManagerScreen(int eventId, string sessionId)
        {
            this.OpenEventDashboardUrl(eventId, sessionId);
            this.DashboardMgr.DeleteTestRegs();
            this.DashboardMgr.ReturnToList();
        }

        public void CreateNewAccount(string username, string password, string currency)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(TryRegonlineButton, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
            GetStartedMgr.CreateNewAccount(username, password, currency);
        }

        public void SkipEmailValidation()
        {
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectPopUpFrameByName("plain");
            UIUtil.DefaultProvider.WaitForDisplayAndClick("//span[text()='Skip']", LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SwitchToMainContent();
        }

        #region Helper
        private void LoginIfNecessary()
        {
            if (UIUtil.DefaultProvider.UrlContainsPath("manager/login.aspx"))
            {
                this.Login();
            }
        }
        #endregion

        #region BibTool
        //This region is the caller of a popup dialog, the BibNumberingTool; and this caller is based in Manager.
        //In Builder, such regions would be partial-classes.
        //BNT is, itself, a Builder form; given that it alters the state of an Event and its RegTypes.

        public BibNumberingToolManager BibNumberTool;

        public void ChangeBibNumberingOption(BibNumberingToolManager.AssignNumberToMember which, IEnumerable<BibNumberingToolManager.TeamWithRegTypes> teams)
        {
            //step 2
            //Select Bib Numbering Tool from the Event Details dashboard, Additional Functions
            //that means this should be up: https://activeeuropebeta.regonline.com/builder/dialogs/BibTool.aspx?
            this.DashboardMgr.ClickOption(DashboardManager.EventAdditionalFunction.BibNumberingTool);
            this.SelectBibNumberWindow();

            //step 3
            //[Enter starting bib number for some of the reg types and leave some blank to use the default number: this is done for you.]
            int zeroBasedSequenceOfRegTypes = 0;

            foreach (BibNumberingToolManager.TeamWithRegTypes team in teams)
            {
                if (team.StartingNumber > -1)
                {
                    this.BibNumberTool.SetStartingNumber(zeroBasedSequenceOfRegTypes, team.StartingNumber);
                }

                zeroBasedSequenceOfRegTypes++;
            }

            this.BibNumberTool.SetStartingNumberDefault(101);

            //Select the option to assign everyone a unique number, click assign and then exit
            this.BibNumberTool.SetTeamNumbers(which);

            //if there are any registrants with bib numbers, this is where we need to hit the prompt btnOK

            this.BibNumberTool.SaveAndClose();
            UIUtil.DefaultProvider.SelectOriginalWindow();
        }

        public void SelectBibNumberWindow()
        {
            UIUtil.DefaultProvider.SelectWindowByTitle("Bib Numbering Tool");
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }
        #endregion
    }
}
