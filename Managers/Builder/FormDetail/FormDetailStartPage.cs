namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Linq;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class FormDetailManager : ManagerBase
    {
        private const string RegTypeTableLocator = "ctl00_cph_grdRegTypes_tblGrid";
        private const string RegTypeLinkLocatorFormat = "//table[@id='ctl00_cph_grdRegTypes_tblGrid']/tbody/tr/td/a[text()='{0}']";
        private const string RegTypeDeleteLocatorFormat = RegTypeLinkLocatorFormat + "/../../td[7]/div/img[2]";
        private const string RegTypeDisplayOptionLocator = "ctl00_cph_chkShowRegTypeOptions";
        private const string RegTypeDisplayOptions = "ctl00_cph_ddlEventsRegTypeTypeID";

        private struct StartPageLocator
        {
            public const string EventName = "ctl00_cph_txtEventsEventTitle";
            public const string Shortcut = "ctl00_cph_txtEventsShortcutDescription";
            public const string StartDate = "ctl00_cph_dtpEventsStartDate";
            public const string StartTime = "ctl00_cph_dtpEventsStartTime";
            public const string EndDate = "ctl00_cph_dtpEventsEndDate";
            public const string EndTime = "ctl00_cph_dtpEventsEndTime";
            public const string Location = "ctl00_cph_txtEventsLocName";
            public const string Phone = "ctl00_cph_txtEventsLocPhone";
            public const string Country = "ctl00_cph_ddlEventsLocCountry";
            public const string AddressLineOne = "ctl00_cph_txtEventsLocAddress1";
            public const string AddressLineTwo = "ctl00_cph_txtEventsLocAddress2";
            public const string City = "ctl00_cph_txtEventsLocCity";
            public const string State = "ctl00_cph_ctl00_cph_ddlRegion_txtRegion";
            public const string ZipCode = "ctl00_cph_txtEventsLocPostalCode";
            public const string ContactEmail = "ctl00_cph_txtEventsHomeEmail";
            public const string ContactInfo = "ctl00_cph_elHomeContactInfo_linkCheckmarkfrmEventTextsHomeContactInfo";
            public const string Homepage = "ctl00_cph_txtHomeLink";
            public const string AllowGroupRegister = "ctl00_cph_chkAllowGroups";
            public const string AllowUpdate = "ctl00_cph_chkEventsAllowUpdates";
            public const string AllowSubstitution = "ctl00_cph_chkEventsAllowSubstitutions";
            public const string AllowCancel = "ctl00_cph_chkEventsAllowCancels";
            public const string AllowChangeRegType = "ctl00_cph_chkEventsAllowRegTypeUpdates";
            public const string ConferenceURL = "ctl00_cph_txtMeetingUrl";
            public const string EventType = "ctl00_cph_ddlChannels"; // This is for active european events
            public const string EventCategory = "ctl00_cph_ddlMediaTypes"; // This is for pro events
            public const string Industry = "ddlChannels"; // This is for pro events
        }

        public struct StartPageDefaultInfo
        {
            public const string Location = "Automation Test Room One";
            public const string Country = "United States";
            public const string AddressLineOne = "4750 Walnut Street";
            public const string City = "Boulder";
            public const string State = "CO";
            public const string ZipCode = "99701";
            public const string ConferenceURL = "https://www.readytalk.com/";
            public const string EventType = "Running";
        }

        public enum FeeLocation
        {
            Event,
            RegType
        }

        public enum ActiveEuropeEventType
        {
            Running
        }

        public enum EventCategory
        {
            [CustomStringAttribute("Class/Workshop")]
            ClassOrWorkshop,

            [CustomStringAttribute("Conference")]
            Conference,

            [CustomStringAttribute("Expo")]
            Expo,

            [CustomStringAttribute("Festival")]
            Festival,

            [CustomStringAttribute("Fundraiser")]
            Fundraiser,

            [CustomStringAttribute("Meeting")]
            Meeting,

            [CustomStringAttribute("Reunion")]
            Reunion,

            [CustomStringAttribute("Social / Networking")]
            SocialOrNetworking,

            [CustomStringAttribute("Trade Show")]
            TradeShow,

            [CustomStringAttribute("Virtual Event")]
            VirtualEvent,

            [CustomStringAttribute("Other")]
            Other
        }

        public enum EventIndustry
        {
            [CustomStringAttribute("Professional & Continuing Education")]
            ProfessionalAndContinuingEducation,

            [CustomStringAttribute("Entertainment")]
            Entertainment,

            [CustomStringAttribute("Clubs & Associations")]
            ClubsAndAssociations,

            [CustomStringAttribute("Corporate")]
            Corporate,

            [CustomStringAttribute("Community")]
            Community,

            [CustomStringAttribute("Family & Kids")]
            FamilyAndKids,

            [CustomStringAttribute("Health & Fitness")]
            HealthAndFitness,

            [CustomStringAttribute("Charity & Non-Profit")]
            CharityAndNonProfit,

            [CustomStringAttribute("Personal Development")]
            PersonalDevelopment,

            [CustomStringAttribute("Arts & Entertainment")]
            ArtsAndEntertainment,

            [CustomStringAttribute("Music")]
            Music,

            [CustomStringAttribute("Science")]
            Science,

            [CustomStringAttribute("Outdoors & recreation")]
            OutdoorsAndRecreation,

            [CustomStringAttribute("Politics & Activism")]
            PoliticsAndActivism,

            [CustomStringAttribute("Religious & Spiritual")]
            ReligiousAndSpiritual,

            [CustomStringAttribute("College & University")]
            CollegeAndUniversity,

            [CustomStringAttribute("Technology")]
            Technology,

            [CustomStringAttribute("Social & Networking")]
            SocialAndNetworking,

            [CustomStringAttribute("Sports")]
            Sports,

            [CustomStringAttribute("Expos & Tradeshows")]
            ExposAndTradeshows,

            [CustomStringAttribute("Other")]
            Other
        }

        public enum RegTypeDisplayOption
        {
            RadioButton,
            DropDownList
        }

        public EventFeeManager EventFeeMgr { get; private set; }
        public EventAdvancedSettingsManager AdvancedSettingsMgr { get; private set; }
        public RegTypeManager RegTypeMgr { get; private set; }
        public GroupDiscountRuleManager GroupDiscountRuleMgr { get; private set; }

        public void SetStartEndDateTimeDefault()
        {
            // Start date
            UIUtilityProvider.UIHelper.SetDateForDatePicker(
                StartPageLocator.StartDate,
                (int)(DefaultStartDateTime.Year),
                (int)(DefaultStartDateTime.Month),
                (int)(DefaultStartDateTime.Day));

            // Start time
            UIUtilityProvider.UIHelper.SetDateForDatePicker(
                StartPageLocator.StartTime,
                (int)(DefaultStartDateTime.Year),
                (int)(DefaultStartDateTime.Month),
                (int)(DefaultStartDateTime.Day),
                (int)(DefaultStartDateTime.Hour),
                (int)(DefaultStartDateTime.Minute),
                (int)(DefaultStartDateTime.Second));

            // End date
            UIUtilityProvider.UIHelper.SetDateForDatePicker(
                StartPageLocator.EndDate,
                (int)(DefaultEndDateTime.Year),
                (int)(DefaultEndDateTime.Month),
                (int)(DefaultEndDateTime.Day));

            // End time
            UIUtilityProvider.UIHelper.SetDateForDatePicker(
                StartPageLocator.EndTime,
                (int)(DefaultEndDateTime.Year),
                (int)(DefaultEndDateTime.Month),
                (int)(DefaultEndDateTime.Day),
                (int)(DefaultEndDateTime.Hour),
                (int)(DefaultEndDateTime.Minute),
                (int)(DefaultEndDateTime.Second));
        }

        public void SetStartDate(DateTime startDate)
        {
            UIUtilityProvider.UIHelper.SetDateForDatePicker(StartPageLocator.StartDate, startDate);
        }

        public void SetStartTime(DateTime startTime)
        {
            UIUtilityProvider.UIHelper.SetDateForDatePicker(StartPageLocator.StartTime, startTime);
        }

        public void SetEndDate(DateTime endDate)
        {
            UIUtilityProvider.UIHelper.SetDateForDatePicker(StartPageLocator.EndDate, endDate);
        }

        public void SetEndTime(DateTime endTime)
        {
            UIUtilityProvider.UIHelper.SetDateForDatePicker(StartPageLocator.EndTime, endTime);
        }

        public void TypeLocation(string location)
        {
            UIUtilityProvider.UIHelper.Type(StartPageLocator.Location, location, LocateBy.Id);
        }

        public void TypePhone(string phone)
        {
            UIUtilityProvider.UIHelper.Type(StartPageLocator.Phone, phone, LocateBy.Id);
        }

        public void SelectCountry(string country)
        {
            UIUtilityProvider.UIHelper.SelectWithText(StartPageLocator.Country, country, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void TypeAddressLineOne(string addressOne)
        {
            UIUtilityProvider.UIHelper.Type(StartPageLocator.AddressLineOne, addressOne, LocateBy.Id);
        }

        public void TypeAddressLineTwo(string addressTwo)
        {
            UIUtilityProvider.UIHelper.Type(StartPageLocator.AddressLineTwo, addressTwo, LocateBy.Id);
        }

        public void TypeCity(string city)
        {
            UIUtilityProvider.UIHelper.Type(StartPageLocator.City, city, LocateBy.Id);
        }

        public void SelectState(string state)
        {
            UIUtilityProvider.UIHelper.SelectWithText(StartPageLocator.State, state, LocateBy.Id);
        }

        public void TypeZip(string zip)
        {
            UIUtilityProvider.UIHelper.Type(StartPageLocator.ZipCode, zip, LocateBy.Id);
        }

        public void AddContactInfo(string contactInfo)
        {
            UIUtilityProvider.UIHelper.Click(StartPageLocator.ContactInfo, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(1);
            UIUtilityProvider.UIHelper.Type("//textarea", contactInfo, LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
            UIUtilityProvider.UIHelper.Click("ctl00_btnSaveClose", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        public void TypeHomepageUrl(string url)
        {
            UIUtilityProvider.UIHelper.Type(StartPageLocator.Homepage, url, LocateBy.Id);
        }

        public void SelectEventCategory(EventCategory category)
        {
            UIUtilityProvider.UIHelper.SelectWithText(
                StartPageLocator.EventCategory, 
                CustomStringAttribute.GetCustomString(category), 
                LocateBy.Id);

            // Wait for the 'Industry' dropdown to show up
            Utility.ThreadSleep(1);
        }

        public void SelectEventIndustry(EventIndustry industry)
        {
            UIUtilityProvider.UIHelper.SelectWithText(
                StartPageLocator.Industry,
                CustomStringAttribute.GetCustomString(industry),
                LocateBy.Id);
        }

        [Step]
        public void SetEventNameAndShortcut(string eventName)
        {
            this.SetEventNameAndShortcut(eventName, Guid.NewGuid().ToString());
        }

        public void SetEventNameAndShortcut(string eventName, string shortcut)
        {
            UIUtilityProvider.UIHelper.Type(StartPageLocator.EventName, eventName, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(StartPageLocator.Shortcut, LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(StartPageLocator.Shortcut, shortcut, LocateBy.Id);
        }

        [Step]
        public void SetStartPage(ManagerSiteManager.EventType eventType, string eventName)
        {
            this.SetEventNameAndShortcut(eventName);

            switch (eventType)
            {
                case ManagerSiteManager.EventType.ProEvent:
                    UIUtilityProvider.UIHelper.Type(StartPageLocator.Location, StartPageDefaultInfo.Location, LocateBy.Id);
                    UIUtilityProvider.UIHelper.Type(StartPageLocator.AddressLineOne, StartPageDefaultInfo.AddressLineOne, LocateBy.Id);
                    UIUtilityProvider.UIHelper.Type(StartPageLocator.City, StartPageDefaultInfo.City, LocateBy.Id);
                    UIUtilityProvider.UIHelper.Type(StartPageLocator.State, StartPageDefaultInfo.State, LocateBy.Id);
                    UIUtilityProvider.UIHelper.Type(StartPageLocator.ZipCode, StartPageDefaultInfo.ZipCode, LocateBy.Id);
                    break;

                case ManagerSiteManager.EventType.ExpressEvent:
                    break;

                case ManagerSiteManager.EventType.LiteEvent:
                    break;

                case ManagerSiteManager.EventType.Membership:
                    break;

                case ManagerSiteManager.EventType.WebEvent:
                    UIUtilityProvider.UIHelper.Type(StartPageLocator.ConferenceURL, StartPageDefaultInfo.ConferenceURL, LocateBy.Id);
                    break;

                case ManagerSiteManager.EventType.Survey:
                    break;

                case ManagerSiteManager.EventType.DonationForm:
                    break;

                case ManagerSiteManager.EventType.CreateFromTemplate:
                    break;
                case ManagerSiteManager.EventType.ActiveEuropeEvent:
                    UIUtilityProvider.UIHelper.SelectWithText(StartPageLocator.EventType, StartPageDefaultInfo.EventType, LocateBy.Id); 
                    UIUtilityProvider.UIHelper.Type(StartPageLocator.Location, StartPageDefaultInfo.Location, LocateBy.Id);
                    UIUtilityProvider.UIHelper.Type(StartPageLocator.AddressLineOne, StartPageDefaultInfo.AddressLineOne, LocateBy.Id);
                    UIUtilityProvider.UIHelper.Type(StartPageLocator.City, StartPageDefaultInfo.City, LocateBy.Id);
                    UIUtilityProvider.UIHelper.Type(StartPageLocator.State, StartPageDefaultInfo.State, LocateBy.Id);
                    UIUtilityProvider.UIHelper.Type(StartPageLocator.ZipCode, StartPageDefaultInfo.ZipCode, LocateBy.Id);
                    break;
                default:
                    break;
            }
        }

        public void SetRegTypeDisplayOption(RegTypeDisplayOption displayOption)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(RegTypeDisplayOptionLocator, true, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();

            switch (displayOption)
            {
                case RegTypeDisplayOption.RadioButton:
                    UIUtilityProvider.UIHelper.SelectWithValue(RegTypeDisplayOptions, "2", LocateBy.Id);
                    break;
                case RegTypeDisplayOption.DropDownList:
                    UIUtilityProvider.UIHelper.SelectWithValue(RegTypeDisplayOptions, "1", LocateBy.Id);
                    break;
                default:
                    break;
            }
        }

        protected void VerifyContactEmail(bool hasValue)
        {
            VerifyTool.VerifyValue(hasValue, UIUtilityProvider.UIHelper.GetValue(StartPageLocator.ContactEmail, LocateBy.Id).Trim().Length > 0, "Contact email has value: {0}");
        }

        protected void VerifyAllowGroupRegister(bool isChecked)
        {
            VerifyTool.VerifyValue(isChecked, UIUtilityProvider.UIHelper.IsChecked(StartPageLocator.AllowGroupRegister, LocateBy.Id), "Allow group registering is checked: {0}");
        }

        protected void VerifyAllowUpdate(bool isChecked)
        {
            VerifyTool.VerifyValue(isChecked, UIUtilityProvider.UIHelper.IsChecked(StartPageLocator.AllowUpdate, LocateBy.Id), "Allow updating is checked: {0}");
        }

        protected void VerifyAllowSubstitution(bool isChecked)
        {
            VerifyTool.VerifyValue(isChecked, UIUtilityProvider.UIHelper.IsChecked(StartPageLocator.AllowSubstitution, LocateBy.Id), "Allow substitution is checked: {0}");
        }

        protected void VerifyAllowCancel(bool isChecked)
        {
            VerifyTool.VerifyValue(isChecked, UIUtilityProvider.UIHelper.IsChecked(StartPageLocator.AllowCancel, LocateBy.Id), "Allow cancelling is checked: {0}");
        }

        [Verify]
        public void VerifyStartPageInitialDefaults(ManagerSiteManager.EventType eventType)
        {
            this.VerifyContactEmail(true);

            switch (eventType)
            {
                case ManagerSiteManager.EventType.ProEvent:
                    this.VerifyAllowGroupRegister(true);
                    this.VerifyAllowUpdate(true);
                    this.VerifyAllowSubstitution(true);
                    this.VerifyAllowCancel(true);
                    break;

                case ManagerSiteManager.EventType.ExpressEvent:
                    break;

                case ManagerSiteManager.EventType.LiteEvent:
                    break;

                case ManagerSiteManager.EventType.Membership:
                    this.VerifyAllowGroupRegister(true);
                    this.VerifyAllowUpdate(true);
                    break;

                case ManagerSiteManager.EventType.WebEvent:
                    this.VerifyAllowGroupRegister(true);
                    this.VerifyAllowCancel(true);
                    break;

                case ManagerSiteManager.EventType.Survey:
                    break;

                case ManagerSiteManager.EventType.DonationForm:
                    break;

                case ManagerSiteManager.EventType.CreateFromTemplate:
                    break;

                default:
                    break;
            }
        }

        [Verify]
        public void VerifyStartPageSettingsAreSaved(ManagerSiteManager.EventType eventType, string eventName)
        {
            Assert.That(Event != null);

            switch (eventType)
            {
                case ManagerSiteManager.EventType.ProEvent:
                    Assert.That(Event.Event_Title == eventName);
            
                    Assert.That(Event.Start_Date.Value.Equals(new DateTime(
                        (int)(DefaultStartDateTime.Year),
                        (int)(DefaultStartDateTime.Month),
                        (int)(DefaultStartDateTime.Day),
                        (int)(DefaultStartDateTime.Hour),
                        (int)(DefaultStartDateTime.Minute),
                        (int)(DefaultStartDateTime.Second))));
            
                    Assert.That(Event.End_Date.Value.Equals(new DateTime(
                        (int)(DefaultEndDateTime.Year),
                        (int)(DefaultEndDateTime.Month),
                        (int)(DefaultEndDateTime.Day),
                        (int)(DefaultEndDateTime.Hour),
                        (int)(DefaultEndDateTime.Minute),
                        (int)(DefaultEndDateTime.Second))));

                    Assert.That(Event.Loc_Name == StartPageDefaultInfo.Location);
                    Assert.That(Event.Loc_Address_1 == StartPageDefaultInfo.AddressLineOne);
                    Assert.That(Event.Loc_City == StartPageDefaultInfo.City);
                    Assert.That(Event.Loc_Region == StartPageDefaultInfo.State);
                    Assert.That(Event.Loc_Postal_Code == StartPageDefaultInfo.ZipCode);
                    Assert.That(Event.HomeEmail.Trim().Length > 0);
                    Assert.That(Event.GroupRegs);
                    Assert.That(Event.AllowUpdates);
                    Assert.That(Event.AllowSubstitutions);
                    Assert.That(Event.AllowCancels);
                    break;

                case ManagerSiteManager.EventType.ExpressEvent:
                    break;

                case ManagerSiteManager.EventType.LiteEvent:
                    break;

                case ManagerSiteManager.EventType.Membership:
                    break;

                case ManagerSiteManager.EventType.WebEvent:
                    Assert.That(Event.Event_Title == eventName);

                    Assert.That(Event.Start_Date.Value.Equals(new DateTime(
                        (int)(DefaultStartDateTime.Year),
                        (int)(DefaultStartDateTime.Month),
                        (int)(DefaultStartDateTime.Day),
                        (int)(DefaultStartDateTime.Hour),
                        (int)(DefaultStartDateTime.Minute),
                        (int)(DefaultStartDateTime.Second))));

                    Assert.That(Event.End_Date.Value.Equals(new DateTime(
                        (int)(DefaultEndDateTime.Year),
                        (int)(DefaultEndDateTime.Month),
                        (int)(DefaultEndDateTime.Day),
                        (int)(DefaultEndDateTime.Hour),
                        (int)(DefaultEndDateTime.Minute),
                        (int)(DefaultEndDateTime.Second))));

                    Assert.That(Event.HomeEmail.Trim().Length > 0);
                    Assert.That(Event.GroupRegs);
                    Assert.That(Event.AllowCancels);
                    break;

                case ManagerSiteManager.EventType.Survey:
                    break;

                case ManagerSiteManager.EventType.DonationForm:
                    break;

                case ManagerSiteManager.EventType.CreateFromTemplate:
                    break;

                default:
                    break;
            }
        }

        public void SetRegistrationTarget(int target)
        {
            UIUtilityProvider.UIHelper.TypeRADNumericById("ctl00_cph_txtTargetAttendance", target);
        }

        [Step]
        public void ClickStartPageEventAdvancedSettings()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("AdvSetting", LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.WaitForRADWindow();
            UIUtilityProvider.UIHelper.MaximizeRADWindow();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
        }

        public void SetGroupRegistration(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkAllowGroups", check, LocateBy.Id);
        }

        [Step]
        public void SetEventAllowUpdateRegistration(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(StartPageLocator.AllowUpdate, check, LocateBy.Id);
        }

        [Step]
        public void SetEventAllowChangeRegistrationType(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(StartPageLocator.AllowChangeRegType, check, LocateBy.Id);
        }

        public void OpenDiscountRule()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//*[@id='ctl00_cph_mdDiscountRules']/a", LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
        }

        public void SetEventLimit(bool check, int? limit)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkLimit", check, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();

            if (check)
            {
                string eventLimitTxtboxLocator = "ctl00_cph_txtEventCapacity";
                UIUtilityProvider.UIHelper.TypeRADNumericById(eventLimitTxtboxLocator, Convert.ToString(limit));
            }
        }

        public void SetEventLimitReachedMessage(int eventID, string message)
        {
            ClientDataContext db = new ClientDataContext();
            EventText eventText = (from e in db.EventTexts where e.EventId == eventID select e).Single();
            eventText.WaitListInfo = message;
            db.SubmitChanges();
        }

        public void SetEnableWaitlist(bool enable)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkEnableEventWaitlist", enable, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        #region RegTypes
        public string GetRegTypeLink(string regTypeName)
        {
            return string.Format(RegTypeLinkLocatorFormat, regTypeName);
        }

        public void AddRegTypes<T>(IEnumerable<T> regTypes) where T : ManagerBase.RegType
        {
            foreach (T rt in regTypes)
            {
                this.ClickAddRegType();
                UIUtilityProvider.UIHelper.WaitForPageToLoad();
                this.RegTypeMgr.SetName(rt.RegistantTypeName.ToString());
                if (rt.GetType() == typeof(BibNumberingToolManager.TeamWithRegTypes))
                {
                    BibNumberingToolManager.TeamWithRegTypes team = rt as BibNumberingToolManager.TeamWithRegTypes;
                    this.RegTypeMgr.SetCollectTeamName(team.CollectTeamName);
                }
                this.RegTypeMgr.SetDisableGroupReg(rt.DisableGroupReg);
                this.RegTypeMgr.SaveAndClose();
            }
        }

        public int GetRegTypeIdFromHref(string regTypeName)
        {
            // Here is a reg type href attribute fomat reference(two lines of js code):
            // javascript:Javascript:highlightRow('ctl00_cph_grdRegTypes_grd', 0);
            // OpenRadWindowByUrl( 'dialog', 'Dialogs/RegType.aspx?EventSessionId=38b2a6bfc30045948cbbd51f82a3cc7d&EventId=608931&regTypeId=10153&panelid=ctl00_cph_grdRegTypes_grd', 675, 660 )
            string regTypeLink = this.GetRegTypeLink(regTypeName);
            string regTypeHrefAttributeString = "href";
            string regTypeHrefAttributeText = UIUtilityProvider.UIHelper.GetAttribute(regTypeLink, regTypeHrefAttributeString, LocateBy.XPath);

            string tmp = regTypeHrefAttributeText.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)[1];
            tmp = tmp.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[1];
            tmp = tmp.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1];
            tmp = tmp.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries)[2];
            tmp = tmp.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1];

            int regTypeID = Convert.ToInt32(tmp);

            return regTypeID;
        }

        [Step]
        public void AddRegType(string name)
        {
            this.ClickAddRegType();
            this.RegTypeMgr.SetName(name);
            this.RegTypeMgr.SaveAndClose();
        }

        [Step]
        public void ClickAddRegType()
        {
            // Click "Add registrant type"
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(GetAddGridItemLocator("ctl00_cph_grdRegTypes_"), LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
        }

        // To decide whether a RegType alreadly exists
        public bool HasRegType(string regTypeName)
        {
            if (UIUtilityProvider.UIHelper.IsElementPresent("//table[@id='ctl00_cph_grdRegTypes_tblGrid']//a[text()='" + regTypeName + "']", LocateBy.XPath))
            {
                return true;
            }

            return false;
        }

        [Step]
        public void OpenRegType(string name)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(name, LocateBy.LinkText);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(RegTypeManager.RegTypeDetailFrameID);
        }

        [Step]
        public void AddRegTypeWithEventFee(string name, double? eventfee)
        {
            this.ClickAddRegType();
            this.RegTypeMgr.SetName(name);
            this.RegTypeMgr.ExpandAdvancedSection();
            this.RegTypeMgr.SetFee(eventfee);
            this.RegTypeMgr.SaveAndClose();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        [Verify]
        public void VerifyHasRegTypeInDatabase(string name)
        {
            ////ReloadEvent();

            EventRegType regType = null;

            ClientDataContext db = new ClientDataContext();
            regType = (from r in db.EventRegTypes where r.Description == name && r.Event == Event select r).Single();
            

            Assert.That(regType != null);

            //Assert.That(Event.EventRegTypesCollection.Find(
            //    delegate(E.EventRegTypes r)
            //    {
            //        return r.Description == name && r.ReportDescription == r.Description;
            //    }
            //) != null);
        }

        public void VerifyHasRegType(string name, bool present)
        {
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format(RegTypeLinkLocatorFormat, name), present, LocateBy.XPath);
        }

        public void DeleteRegType(string name)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(RegTypeDeleteLocatorFormat, name), LocateBy.XPath);
            UIUtilityProvider.UIHelper.GetConfirmation();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SetEventsForceSameRegTypes(bool forceSameRegType)
        {
            string forceSameRegTypeCheckboxLocator = "ctl00_cph_chkEventsForceSameRegTypes";
            UIUtilityProvider.UIHelper.SetCheckbox(forceSameRegTypeCheckboxLocator, forceSameRegType, LocateBy.Id);
        }
        #endregion

        #region Start Page Event/RegType Fee
        [Step]
        public void SetEventFee(double? eventFee)
        {
            string eventFeeTxtboxLocator = "ctl00_cph_txtEventCost";
            UIUtilityProvider.UIHelper.TypeRADNumericById(eventFeeTxtboxLocator, eventFee);
        }

        [Step]
        public void ClickEventFeeAdvanced()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cph_mdDefineCost", LocateBy.Id);
            Utility.ThreadSleep(1.5);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(EventFeeManager.FeeAdvancedFrameIDInEventFee);
        }
        #endregion

        public void SelectEventType(ActiveEuropeEventType type)
        {
            UIUtilityProvider.UIHelper.SelectWithText("ctl00_cph_ddlChannels", type.ToString(), LocateBy.Id);
        }
    }
}
