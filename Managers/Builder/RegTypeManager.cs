﻿namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class RegTypeManager : ManagerBase
    {
        #region Constants
        public const string RegTypeDetailFrameID = "dialog";
        private const string FormNameLocator = "ctl00_cphDialog_txtDescription";
        private const string ReportsNameLocator = "ctl00_cphDialog_txtReportDescription";
        private const string FeeTxtboxLocator = "ctl00_cphDialog_txtCost";
        private const string VisibleToPublicCheckboxLocator = "ctl00_cphDialog_chkPublic";
        private const string VisibleToAdminCheckboxLocator = "ctl00_cphDialog_chkAdmin";
        private const string VisibleToOnsiteCheckboxLocator = "ctl00_cphDialog_chkOnsite";
        private const string AdvancedSectionDIVLocator = "bsAdvanced_ADV";
        private const string TxtboxLocatorSuffix = "_text";
        private const string MinGroupSizeTxtboxLocator = "ctl00_cphDialog_MinRegs";
        private const string MaxGroupSizeTxtboxLocator = "ctl00_cphDialog_MaxRegs";
        private const string RegTypeDirectLinkLocator = "ctl00_cphDialog_txtRegTypeLink";
        private const string XAuthWhatIsThisLocator = "//a[@id='ctl00_cphDialog_btnSetupExAuth']/following-sibling::a";
        #endregion

        private string RegTypeDirectLinkFormat
        {
            get
            {
                return ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "?eventID={0}&rTypeID={1}";
            }
        }

        #region Constructor
        public RegTypeManager() 
        {
            MembershipIntegrationMgr = new MembershipIntegrationManager();
            FeeMgr = new EventFeeManager(FormDetailManager.FeeLocation.RegType);
            //XAuthMgr = new Manager.XAuthManager();
        }
        #endregion

        #region Properties
        public MembershipIntegrationManager MembershipIntegrationMgr { get; set; }
        public EventFeeManager FeeMgr { get; set; }
        //public Manager.XAuthManager XAuthMgr { get; set; }
        #endregion

        #region Enum
        public enum InitialRegStatusType
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

            [StringValue("No-show")]
            NoShow,

            [StringValue("Follow-up")]
            FollowUp
        }

        public enum VisibilityOption
        {
            Public,
            Admin,
            Onsite
        }

        public enum RegLimitType
        {
            Individual,
            Group
        }
        #endregion

        #region RegType detail
        private void SelectThisFrame()
        {
            try
            {
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(RegTypeDetailFrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndStay()
        {
            this.SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        [Step]
        public void SaveAndClose()
        {
            this.SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            this.SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        [Step]
        public void SetName(string regTypeName)
        {
            SetNameOnForm(regTypeName);
            SetNameOnReports(regTypeName);
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_bsGeneral", LocateBy.Id);
            VerifyName(regTypeName);
        }

        public void SetNameOnForm(string name)
        {
            WebDriverUtility.DefaultProvider.Type(FormNameLocator, name, LocateBy.Id);
        }

        public void SetNameOnReports(string name)
        {
            WebDriverUtility.DefaultProvider.Type(ReportsNameLocator, name, LocateBy.Id);
        }

        private void ClickAdvancedHeader()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//span[text()='Advanced']", LocateBy.XPath);
            Utility.ThreadSleep(0.5);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void ExpandAdvancedSection()
        {
            if (WebDriverUtility.DefaultProvider.IsElementHidden(AdvancedSectionDIVLocator, LocateBy.Id))
            {
                ClickAdvancedHeader();
            }
        }

        public void CollapseAdvancedSection()
        {
            if (!WebDriverUtility.DefaultProvider.IsElementHidden(AdvancedSectionDIVLocator, LocateBy.Id))
            {
                ClickAdvancedHeader();
            }
        }

        public void SetFee(double? regTypeFee)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById(FeeTxtboxLocator, Convert.ToString(regTypeFee));
        }

        public void SetVisibilities(bool? isPublic, bool? isAdmin, bool? isOnsite)
        {
            if (isPublic.HasValue)
            {
                SetVisibility(VisibilityOption.Public, isPublic.Value);
            }

            if (isAdmin.HasValue)
            {
                SetVisibility(VisibilityOption.Admin, isAdmin.Value);
            }

            if (isOnsite.HasValue)
            {
                SetVisibility(VisibilityOption.Onsite, isOnsite.Value);
            }
        }

        public void SetVisibility(VisibilityOption option, bool check)
        {
            switch (option)
            {
                case VisibilityOption.Public:
                    WebDriverUtility.DefaultProvider.SetCheckbox(VisibleToPublicCheckboxLocator, check, LocateBy.Id);
                    break;
                case VisibilityOption.Admin:
                    WebDriverUtility.DefaultProvider.SetCheckbox(VisibleToAdminCheckboxLocator, check, LocateBy.Id);
                    break;
                case VisibilityOption.Onsite:
                    WebDriverUtility.DefaultProvider.SetCheckbox(VisibleToOnsiteCheckboxLocator, check, LocateBy.Id);
                    break;
                default:
                    break;
            }
        }

        // Get the value of a visibility option checkbox to see whether it is checked or not
        // Return true : checked
        // Return false : unchecked
        public bool GetVisibility(VisibilityOption option)
        {
            string checkboxValue = string.Empty;
            switch (option)
            {
                case VisibilityOption.Public:
                    checkboxValue = WebDriverUtility.DefaultProvider.GetValue(VisibleToPublicCheckboxLocator, LocateBy.Id);
                    break;
                case VisibilityOption.Admin:
                    checkboxValue = WebDriverUtility.DefaultProvider.GetValue(VisibleToAdminCheckboxLocator, LocateBy.Id);
                    break;
                case VisibilityOption.Onsite:
                    checkboxValue = WebDriverUtility.DefaultProvider.GetValue(VisibleToOnsiteCheckboxLocator, LocateBy.Id);
                    break;
                default:
                    break;
            }

            return checkboxValue == "on" ? true : false;
        }

        public void SetMinGroupSize(int? min)
        {
            WebDriverUtility.DefaultProvider.WaitForElementPresent(MinGroupSizeTxtboxLocator + TxtboxLocatorSuffix, LocateBy.Id);
            WebDriverUtility.DefaultProvider.TypeRADNumericById(MinGroupSizeTxtboxLocator, Convert.ToString(min));
        }

        public void SetMaxGroupSize(int? max)
        {
            WebDriverUtility.DefaultProvider.WaitForElementPresent(MaxGroupSizeTxtboxLocator + TxtboxLocatorSuffix, LocateBy.Id);
            WebDriverUtility.DefaultProvider.TypeRADNumericById(MaxGroupSizeTxtboxLocator, Convert.ToString(max));
        }

        public void SetDisableGroupReg(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cphDialog_chkDisableGroupReg", check, LocateBy.Id);
        }

        public void SetCollectTeamName(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cphDialog_chkCollectTeamName", check, LocateBy.Id);
        }

        /// <summary>
        /// Call database to set message
        /// </summary>
        /// <param name="message"></param>
        public void SetMinimumRegistrantMessageInDatabase(int eventId, string regTypeName, string message)
        {
            //S.EventRegTypesService regTypeService = new S.EventRegTypesService();
            //E.TList<E.EventRegTypes> regTypes = regTypeService.GetByEventId(eventId);
            List<EventRegType> regTypes = new List<EventRegType>();

            ClientDataContext db = new ClientDataContext();
            regTypes = (from rt in db.EventRegTypes where rt.EventId == eventId select rt).ToList();

            if (regTypes.Count > 0)
            {
                foreach (EventRegType regType in regTypes)
                {
                    if (regType.Description == regTypeName)
                    {
                        regType.MinRegsMessage = Convert.ToString(message);
                        db.SubmitChanges();
                        //regTypeService.Save(regType);
                        break;
                    }
                }
            }
        }

        public void SetMinimumRegistrantMessage(string message)
        {
            WebDriverUtility.DefaultProvider.Click("ctl00_cphDialog_elMinRegsMessage_linkCheckmarktext_elMinRegsMessage", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog2");
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SelectIFrameOnCurrentIFrame(1);
            WebDriverUtility.DefaultProvider.Type("//textarea", message + "<br>", LocateBy.XPath);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog2");
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_btnSaveClose", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            System.Threading.Thread.Sleep(1000);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
            System.Threading.Thread.Sleep(1000);
        }

        public void SetCompletionStatus(InitialRegStatusType statusType)
        {
            WebDriverUtility.DefaultProvider.SelectWithText("ctl00_cphDialog_RegStatusId", StringEnum.GetStringValue(statusType), LocateBy.Id);
        }

        // The two steps, SetRegLimit and SetRegLimitReachedMsg are linked in workflow
        public void SetRegLimitOptions(RegLimitType type, int limit, string message)
        {
            SetRegLimit(type, limit);
            SetRegLimitReachedMessage(message);
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_bsGeneral", LocateBy.Id);
        }

        public void SetRegLimit(RegLimitType type, int limit)
        {
            // Check 'Limit the number of registrants for this registrant type'
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_nonEnduranceLimiting_chkLimitRegs", LocateBy.Id);

            switch (type)
            {
                case RegLimitType.Individual:
                    // Limit the number of individual registrants
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_nonEnduranceLimiting_rbInvidividualLimit", LocateBy.Id);
                    WebDriverUtility.DefaultProvider.TypeRADNumericById("ctl00_cphDialog_nonEnduranceLimiting_txtLimit0", limit);
                    break;

                case RegLimitType.Group:
                    // Limit the number of groups
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_nonEnduranceLimiting_rbGroupLimit", LocateBy.Id);
                    WebDriverUtility.DefaultProvider.TypeRADNumericById("ctl00_cphDialog_nonEnduranceLimiting_txtLimitGroups", limit);
                    break;

                default:
                    break;
            }
        }

        public void SetRegLimitReachedMessage(string message)
        {
            string regLimitReachedMessageTxtboxLocator = "ctl00_cphDialog_nonEnduranceLimiting_LimitMessage";
            WebDriverUtility.DefaultProvider.Type(regLimitReachedMessageTxtboxLocator, message ?? string.Empty, LocateBy.Id);
        }

        public void SetAdditionalDetails(string details)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_elRegTypeInfo_linkCheckmarktext_elRegTypeInfo", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog2");
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SelectIFrameOnCurrentIFrame(1);
            WebDriverUtility.DefaultProvider.Type("//textarea", details + "<br>", LocateBy.XPath);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog2");
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_btnSaveClose", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
        }

        public void SetShareProcessingFeePercentage(int? percentage)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById("ctl00_cphDialog_txtProcessingFeePercent", Convert.ToString(percentage));
        }

        public void SetShowDate(DateTime date)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_dtpShowDate_tbDate", date.ToString("MM/dd/yyyy"), LocateBy.Id);
        }

        public void SetHideDate(DateTime date)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_dtpHideDate_tbDate", date.ToString("MM/dd/yyyy"), LocateBy.Id);
        }

        public void SetShowAndHideDates(DateTime showDate, DateTime hideDate)
        {
            SetShowDate(showDate);
            SetHideDate(hideDate);
        }

        public void ClickRegTypeDirectLink()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(RegTypeDirectLinkLocator, LocateBy.Id);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
        }

        public string GetRegTypeDirectLink()
        {
            return WebDriverUtility.DefaultProvider.GetText(RegTypeDirectLinkLocator, LocateBy.Id);
        }

        public string ComposeRegTypeDirectLink(int eventID, int regTypeID)
        {
            return string.Format(RegTypeDirectLinkFormat, eventID, regTypeID);
        }

        public string GetRegTypeDirectLink(int eventID, string regTypeName)
        {
            List<EventRegType> regTypes = this.Fetch_RegTypes(eventID);
            int regTypeID = InvalidId;

            EventRegType regType = regTypes.Find(x => x.Description == regTypeName);
            
            if (regType != null)
            {
                regTypeID = regType.Id;
            }

            if (regTypeID == InvalidId)
            {
                Assert.Fail(string.Format("No such reg type: {0}", regTypeName));
            }

            return this.ComposeRegTypeDirectLink(eventID, regTypeID);
        }

        public List<int> GetRegTypeIDs(int eventID)
        {
            List<EventRegType> regTypes = this.Fetch_RegTypes(eventID);
            List<int> regTypeIDs = new List<int>();

            foreach (EventRegType regType in regTypes)
            {
                regTypeIDs.Add(regType.Id);
            }

            return regTypeIDs;
        }

        public List<string> GetRegTypeDirectLinks(int eventID)
        {
            List<EventRegType> regTypes = this.Fetch_RegTypes(eventID);

            List<string> regTypeDirectLinks = new List<string>();
            
            foreach (EventRegType regType in regTypes)
            {
                regTypeDirectLinks.Add(ComposeRegTypeDirectLink(eventID, regType.Id));
            }

            return regTypeDirectLinks;
        }

        public void VerifyName(string regTypeName)
        {
            // Verify "name on reports" is copied
            VerifyTool.VerifyValue(regTypeName, WebDriverUtility.DefaultProvider.GetValue(ReportsNameLocator, LocateBy.Id), "RegType name on form/reports: {0}");
        }

        public void VerifyFee(double? regTypeFee)
        {
            string expectedValue = Convert.ToString(regTypeFee);
            string actualValue = WebDriverUtility.DefaultProvider.GetValue(FeeTxtboxLocator + TxtboxLocatorSuffix, LocateBy.Id);

            if (!actualValue.Contains(expectedValue))
            {
                WebDriverUtility.DefaultProvider.FailTest(string.Format("Expected RegTypeFee is '{0}' but actual value was '{1}'", expectedValue, actualValue));
            }
            
            //VerifyValue(U.ConversionTools.ConvertToString(regTypeFee),
            //    GetValue(FeeTxtboxLocator + TxtboxLocatorSuffix), "RegType Fee: {0}");
        }
        #endregion

        #region RegType fee advanced
        public void ClickFeeAdvancedLink()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_mdCostLink", LocateBy.Id);
            Utility.ThreadSleep(2);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();

            try
            {
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog2");
            }
            catch
            {
                // Ignore any error
            }
        }
        #endregion

        #region XAuth
        
        [Step]
        public void ClickOpenXAuthSetup()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_btnSetupExAuth", LocateBy.Id);
            Utility.ThreadSleep(2);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog2");

            if (ConfigReader.DefaultProvider.AccountConfiguration.XAuthVersion == "New")
            {
                ManagerProvider.XAuthMgr.SelectXAuthRadioButton();
            }
        }

        [Step]
        public void EnableXAuth(bool enable)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cphDialog_chkXAuthEnable", enable, LocateBy.Id);
        }

        [Verify]
        public void VerifyEnableXAuthIsChecked(bool isChecked)
        {
            Assert.AreEqual(isChecked, WebDriverUtility.DefaultProvider.IsChecked("ctl00_cphDialog_chkXAuthEnable", LocateBy.Id));
        }

        [Verify]
        public void VerifyEnableXAuthTextIsEnableExternalAuthentication(bool isEnable)
        {
            Assert.AreEqual(isEnable, WebDriverUtility.DefaultProvider.GetText("ctl00_cphDialog_btnSetupExAuth", LocateBy.Id).Equals("Enable External Authentication"));
        }

        [Step]
        public void SelectRegTypeFrame()
        {
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
        }

        [Step]
        public void ClickOpenAndCloseXAuthWhatIsThis()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(XAuthWhatIsThisLocator, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.ClosePopUpWindow();
        }

        [Verify]
        public void VerifyUnableToActivateEventWhenXAuthNotApprovedMessageShown(bool isShowMessage)
        {
            Assert.AreEqual(isShowMessage, WebDriverUtility.DefaultProvider.GetAttribute("wrpUnapprovedXAuth", "@style", LocateBy.Id) != "display: none;");
        }

        #endregion 
    }
}
