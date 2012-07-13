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
        private const string AttendeeSearchButton = "//a[@id='ctl00_ctl00_cphDialog_uclSearch_btnSearchGo']/span";
        private const string AttendeeSearchNextPageClick = "//a[text()='Next']";
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
                return ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrlWithHttps + "manager/Forms/?EventSessionID={0}";
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

        public enum SearchModes
        {
            Attendee,
            Event,
            Transaction,
            Help
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

        public ManagerSiteManager()
        {
            this._accountMgr = new AccountManager();
            this._dashboardMgr = new DashboardManager();
            this.BibNumberTool = new BibNumberingToolManager();
            this._getStartedMgr = new GetStartedManager();
            this._createNewMgr = new CreateNewManager();
        }

        [Step]
        public void OpenLogin()
        {
            UIUtilityProvider.UIHelper.OpenUrl(string.Format("{0}manager/login.aspx", ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrlWithHttps));
            AllowCookies();
        }

        [Step]
        public string Login()
        {
            this.Login(ConfigurationProvider.XmlConfig.AccountConfiguration.Login, ConfigurationProvider.XmlConfig.AccountConfiguration.Password);
            string eventSessionId = this.GetEventSessionId();
            this.OpenManagerURL(eventSessionId);
            return eventSessionId;
        }

        [Step]
        public void Login(string username, string password)
        {
            UIUtilityProvider.UIHelper.Type(LoginUserNameLocator, username, LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(LoginPasswordLocator, password, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(LoginButtonLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void Logout()
        {
           UIUtilityProvider.UIHelper.WaitForDisplayAndClick(LogoutLinkLocator, LocateBy.Id);
        }

        [Step]
        public void ClickEditUserLink()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_ctl00_hpEditUser", LocateBy.Id);
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("plain");
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
            Assert.That(UIUtilityProvider.UIHelper.IsElementPresent("//div[@class='warningMessageFluid']", LocateBy.XPath));
        }

        [Verify]
        public void VerifyCaptchaVisible()
        {
            Assert.That(UIUtilityProvider.UIHelper.IsElementPresent("//div[@class='captcha']", LocateBy.XPath));
        }

        [Step]
        public void SelectAndEnterForgotPassword(string username)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//a[text()='Password']", LocateBy.XPath);
            UIUtilityProvider.UIHelper.Type("//label[@for='txtUsername']/input", username, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//div[@id='password']//span[@class='BiggerButtonBase']/a/span[text()='Submit']", LocateBy.XPath);
        }

        [Step]
        public void ClickOnFirstEvent()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(FirstEventLocator, LocateBy.XPath);
        }

        [Step]
        public void SelectAndEnterForgotLogin(string email)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//a[text()='Username']", LocateBy.XPath);
            UIUtilityProvider.UIHelper.Type("//label[@for='txtEmail']/input", email, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//div[@id='username']//span[@class='BiggerButtonBase']/a/span[text()='Submit']", LocateBy.XPath);
        }

        public void OpenManagerURL(string eventSessionId)
        {
            UIUtilityProvider.UIHelper.OpenUrl(this.GetManagerURL(eventSessionId));
        }

        [Step]
        public void OpenEventBuilderStartPage(int eventId, string eventSessionId)
        {
            UIUtilityProvider.UIHelper.OpenUrl(string.Format(
                "{0}Builder/default.aspx?EventId={1}&EventSessionID={2}",
                ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrlWithHttps,
                eventId,
                eventSessionId));
        }

        [Verify]
        public void VerifyErrorMessage(string errorMessage)
        {
            VerifyTool.VerifyValue(errorMessage, UIUtilityProvider.UIHelper.GetText("ctl00_cphMaster_warning", LocateBy.Id), "Error message: {0}");
        }

        public string GetManagerURL(string eventSessionID)
        {
            return string.Format(ManagerSiteUrlFormat, eventSessionID);
        }

        [Verify]
        public void VerifyManager()
        {
            Assert.That(UIUtilityProvider.UIHelper.IsElementPresent("//td[@id='mgrHeader']", LocateBy.XPath) || UIUtilityProvider.UIHelper.IsElementPresent("//div[@class='m3logo']", LocateBy.XPath));
        }

        public void VerifyTextPresent(string text)
        {
            Assert.That(UIUtilityProvider.UIHelper.IsTextPresent(text));
        }

        public void SwitchToSubAccount(int customerId)
        {
            string otherAccountDropdown = "ctl00_cphDialog_ddOtherAccounts";
            string optionLocator = string.Format("value={0}", customerId);
            string accountText = string.Format("Account: {0}", customerId);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(HeaderAccountLocator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForElementPresent(otherAccountDropdown, LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectWithText(otherAccountDropdown, optionLocator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("OK", LocateBy.LinkText);
            Utility.ThreadSleep(2);
            Assert.AreEqual(accountText, UIUtilityProvider.UIHelper.GetText(HeaderAccountLocator, LocateBy.Id));
        }

        /// <summary>
        /// Extract the current [Customers].[ID] from the header
        /// </summary>
        /// <returns>Customer ID</returns>
        public int GetCurrentAccountId()
        {
            string accountHeader = string.Empty;

            if (UIUtilityProvider.UIHelper.IsElementDisplay(HeaderAccountLocator, LocateBy.Id))
            {
                accountHeader = UIUtilityProvider.UIHelper.GetText(HeaderAccountLocator, LocateBy.Id);
            }
            else
            {
                string tmp = UIUtilityProvider.UIHelper.GetText("//div[@class='titleBar']/ul/li", LocateBy.XPath).Split(
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
            UIUtilityProvider.UIHelper.WaitForElementDisplay(folderLocator, LocateBy.XPath);
            string folderDivClassAttribute = UIUtilityProvider.UIHelper.GetAttribute(string.Format("{0}/parent::div", folderLocator), "class", LocateBy.XPath);

            if (!folderDivClassAttribute.Equals("rtMid rtSelected"))
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(folderLocator, LocateBy.XPath);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
                Utility.ThreadSleep(1);
            }
        }

        [Step]
        public void SelectFolder()
        {
            this.SelectFolder(ConfigurationProvider.XmlConfig.AccountConfiguration.Folder);
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
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddEventDropDown, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();

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
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(AddEventDropDownType, StringEnum.GetStringValue(eventType)), LocateBy.XPath);
                    UIUtilityProvider.UIHelper.WaitForPageToLoad();
                    break;
                case EventType.CreateFromTemplate:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(AddEventDropDownType, StringEnum.GetStringValue(eventType)), LocateBy.XPath);
                    UIUtilityProvider.UIHelper.WaitForAJAXRequest();
                    break;
            }

            UIUtilityProvider.UIHelper.HideActiveSpecificFooter(true);
        }

        public void SelectTemplateToCreate(string eventName)
        {
            string option = UIUtilityProvider.UIHelper.GetAttribute(string.Format(TemplateNameLocator, eventName), "value", LocateBy.XPath);
            UIUtilityProvider.UIHelper.SelectWithText("//select[@id='lbTemplates']", eventName + " (" + option + ") - Account #" + Convert.ToInt32(ConfigurationProvider.XmlConfig.AccountConfiguration.Id), LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//a/span[text()='OK']/..", LocateBy.XPath);
        }

        [Step]
        public void GotoTab(Tab tab)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(this.ComposeTabLocator(tab), LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.HideActiveSpecificFooter(true);
        }

        private bool IsTabSelected(Tab tab)
        {
            string tabLinkLocator = string.Format("{0}/parent::span/parent::span/parent::a", this.ComposeTabLocator(tab));
            string tabLinkClassAttribute = UIUtilityProvider.UIHelper.GetAttribute(tabLinkLocator, "class", LocateBy.XPath);

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

            result = UIUtilityProvider.UIHelper.IsElementPresent(string.Format(EventExistsLocator, name), LocateBy.XPath);

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

            if (UIUtilityProvider.UIHelper.IsElementPresent(locator_A_EventTitle, LocateBy.XPath))
            {
                int eventCount = UIUtilityProvider.UIHelper.GetXPathCountByXPath(locator_A_EventTitle);

                // Retrieve the first id
                string locator_Tr_EventRow = string.Format("{0}/parent::td/parent::tr", locator_A_EventTitle);
                string eventId = UIUtilityProvider.UIHelper.GetAttribute(locator_Tr_EventRow, "data-id", LocateBy.XPath);
                ids.Add(Convert.ToInt32(eventId));

                for (int cnt = 0; cnt < eventCount - 1; cnt++)
                {
                    string locator_Tr_EventRow_Following = string.Format(
                        "{0}/following-sibling::tr[{1}]",
                        locator_Tr_EventRow,
                        cnt + 1);

                    eventId = UIUtilityProvider.UIHelper.GetAttribute(locator_Tr_EventRow_Following, "data-id", LocateBy.XPath);
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

            return Convert.ToInt32(UIUtilityProvider.UIHelper.GetText(string.Format(locatorFormat, eventId), LocateBy.XPath));
        }

        /// <summary>
        /// This is just for china test to handle the time difference
        /// </summary>
        /// <param name="now">china time now</param>
        /// <returns>RegOnline time</returns>
        public DateTime ConvertToRegOnlineTime(DateTime dateTime)
        {
            if (TimeZone.CurrentTimeZone.StandardName == "China Standard Time")
            {
                int diff = Convert.ToInt32(ConfigurationProvider.XmlConfig.AllConfiguration.TimeZoneDifference) * -1;
                dateTime = dateTime.AddHours(diff);
            }

            return dateTime;
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

                    if (((int)(ConvertToRegOnlineTime(DateTime.Now) - createdTime).TotalMinutes) > expireMinutes)
                    {
                        DeleteEventById(maxid);
                    }
                }
            }

            // Refresh the page to avoid dirty data,
            // cause we don't drag the event to Delete folder, but move the event to that folder directly
            UIUtilityProvider.UIHelper.RefreshPage();
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

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator_A_DeleteLinkInActionsColumn, LocateBy.XPath);
            Utility.ThreadSleep(0.5);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='OK']", LocateBy.XPath);
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        [Step]
        public int GetFirstEventId(string name)
        {
            int idAsInt = 0;

            if (EventExists(name))
            {
                this.EnsureSortByColumnHeader(EventListGridColumnHeader.ID, SortEventList.Desc);

                string eventIdAttributeLocator = "//*[@title='{0} - Details']/../..";
                string idAsString = UIUtilityProvider.UIHelper.GetAttribute(string.Format(eventIdAttributeLocator, name), "data-id", LocateBy.XPath);

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

            if (!UIUtilityProvider.UIHelper.IsElementDisplay(locator_Input_SortArrow, LocateBy.XPath))
            {
                this.ClickEventListGridColumnHeaderToSort(columnHeader);
            }

            if (!UIUtilityProvider.UIHelper.GetAttribute(locator_Input_SortArrow, "class", LocateBy.XPath).Equals(
                string.Format("rgSort{0}", sort.ToString())))
            {
                this.ClickEventListGridColumnHeaderToSort(columnHeader);
            }
        }

        private void ClickEventListGridColumnHeaderToSort(EventListGridColumnHeader columnHeader)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(
                this.ComposeEventListGridColumnHeader_ClickToSortLink(columnHeader),
                LocateBy.XPath);

            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
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

            UIUtilityProvider.UIHelper.RefreshPage();
        }

        [Step]
        public void CopyEventByName(string name)
        {
            string copyEventLinkLocator = string.Format("//td/a[text()='{0}']/../..//a[@title='Copy event']", name);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(copyEventLinkLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='OK']/../..", LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            Utility.ThreadSleep(6);
        }

        [Step]
        public void CopyEventById(int id)
        {
            string copyEventLinkLocator = string.Format("//td[text()='{0}']/..//a[@title='Copy event']", id.ToString());
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(copyEventLinkLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='OK']/../..", LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            Utility.ThreadSleep(6);
        }

        public void ClickEditRegistrationForm(string name)
        {
            string editEventLinkLocator = string.Format("//td/a[text()='{0}']/../..//a[@title='Edit registration form']", name);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(editEventLinkLocator, LocateBy.XPath);
        }

        public void ClickEditRegistrationForm(int id)
        {
            string editEventLinkLocator = string.Format("//td[text()='{0}']/..//a[@title='Edit registration form']", id.ToString());
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(editEventLinkLocator, LocateBy.XPath);
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
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.XPath);
        }

        [Step]
        public void OpenEventDashboard(int eventID)
        {
            //string dashboardLinkLocator = string.Format(
            //"table[@id='ctl00_ctl00_cphDialog_cpMgrMain_rdgrdgrdForms_ctl00']/tbody/tr[@data-id='{0}']/td[@class='rgSorted']/a", 
            //eventID.ToString());
            string dashboardLinkLocator = string.Format("//a[contains(@href, 'eventID={0}')]", eventID.ToString());

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(dashboardLinkLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        [Step]
        public void OpenEventDashboardUrl(int eventID, string sessionID)
        {
            string dashboardUrl = string.Format(
                ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrlWithHttps + "manager/Forms/details.aspx?EventSessionID={0}&eventID={1}",
                sessionID,
                eventID);

            UIUtilityProvider.UIHelper.OpenUrl(dashboardUrl);
        }

        /// <summary>
        /// This enters attendee's first name and last name to do a search.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public void EnterSearchAttendeeCriteria(string firstName, string lastName)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_ctl00_cphDialog_uclSearch_txtSearchQ", firstName + " " + lastName, LocateBy.Id);
        }

        public void EnterSearchAttendeeCriteria(int attendeeID)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_ctl00_cphDialog_uclSearch_txtSearchQ", attendeeID.ToString(), LocateBy.Id);
        }

        /// <summary>
        /// This clicks the search button at the top right of M3 to perform a search.
        /// </summary>
        public void PerformAttendeeSearch()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AttendeeSearchButton, LocateBy.XPath);
        }

        /// <summary>
        /// This method will click the link to the attendee info page from the search results page.
        /// If the link is not available, it will see if the "Next" button is there and click to advance to the next search results page
        /// It will do this recursively until it finds the record.  If it doesn't find it, then Assert.Fail is called.
        /// </summary>
        /// <param name="attendeeId"></param>
        public void SelectAttendeeOnSearchResults(int attendeeId)
        {
            string attendeeInfoLinkSelector_LinkText = attendeeId.ToString();// "css=a[href=\"javascript:AttendeeInfo('20091006141429050q5jtjn55lnf3j355uv',11267264)\"]";
            string lastPageSelector = "//input[@title='Last Page']";

            //to save time let's look for the Next Pages link so we can skip ahead 10 pages instead of individual pages.
            if (UIUtilityProvider.UIHelper.IsElementPresent(lastPageSelector, LocateBy.XPath))
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(lastPageSelector, LocateBy.XPath);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            }

            try
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(attendeeInfoLinkSelector_LinkText, LocateBy.LinkText);
            }
            catch
            {
                Assert.Fail("Could not find link on Attendee search results page for attendeeId: " + attendeeId);
            }
        }

        /// <summary>
        /// Selects the search mode in the quick search at the top of M3
        /// </summary>
        /// <param name="searchMode"></param>
        [Step]
        public void SelectQuickSearchMode(SearchModes searchMode)
        {
            switch (searchMode)
            {
                case SearchModes.Attendee:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_ctl00_cphDialog_uclSearch_lbSearchAttendees", LocateBy.Id);
                    break;
                case SearchModes.Event:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_ctl00_cphDialog_uclSearch_lbSearchEvents", LocateBy.Id);
                    break;
                case SearchModes.Transaction:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_ctl00_cphDialog_uclSearch_lbSearchTransactions", LocateBy.Id);
                    break;
                case SearchModes.Help:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_ctl00_cphDialog_uclSearch_lbSearchHelp", LocateBy.Id);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Enters search text into the quick search at the top of M3
        /// </summary>
        /// <param name="text"></param>
        [Step]
        public void EnterSearchText(string text)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_ctl00_cphDialog_uclSearch_txtSearchQ", text, LocateBy.Id);
        }

        /// <summary>
        /// Clicks the search button in the quick search in M3
        /// </summary>
        [Step]
        public void ClickQuickSearchGoButton()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AttendeeSearchButton, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void OpenAttendeeReportFromManagerEventList(int eventID)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(this.ComposeAttendeeReportLink(eventID), LocateBy.XPath);
            UIUtilityProvider.UIHelper.SelectTopWindow();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
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
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Jobs History Report", LocateBy.LinkText);
        }

        public void OpenUnsubscribedReport()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Unsubscribed Report", LocateBy.LinkText);
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
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("plain");
            DashboardMgr.DeleteTestReg_ClickDelete();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
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
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(TryRegonlineButton, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            GetStartedMgr.CreateNewAccount(username, password, currency);
        }

        public void SkipEmailValidation()
        {
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("plain");
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='Skip']", LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        #region Helper
        private void LoginIfNecessary()
        {
            if (UIUtilityProvider.UIHelper.UrlContainsPath("manager/login.aspx"))
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
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
        }

        public void SelectBibNumberWindow()
        {
            UIUtilityProvider.UIHelper.SelectWindowByTitle("Bib Numbering Tool");
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }
        #endregion
    }
}
