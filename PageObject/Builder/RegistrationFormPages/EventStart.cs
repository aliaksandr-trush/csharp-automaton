namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class EventStart : Window
    {
        #region WebElements
        public TextBox Title = new TextBox("ctl00_cph_txtEventsEventTitle", LocateBy.Id);
        public TextBox Shortcut = new TextBox("ctl00_cph_txtEventsShortcutDescription", LocateBy.Id);
        public MultiChoiceDropdown EventType = new MultiChoiceDropdown("ctl00_cph_ddlChannels", LocateBy.Id);
        public TextBox EventFee = new TextBox("ctl00_cph_txtEventCost_text", LocateBy.Id);
        public ButtonOrLink EventFeeAdvanced = new ButtonOrLink("ctl00_cph_mdDefineCost", LocateBy.Id);
        public EventFeeDefine EventFeeDefine = new EventFeeDefine("dialog");
        public ButtonOrLink AdvancedSettings = new ButtonOrLink("AdvSetting", LocateBy.Id);
        public EventAdvancedSettings EventAdvancedSettings = new EventAdvancedSettings("dialog");
        public TextBox StartDate = new TextBox("ctl00_cph_dtpEventsStartDate_dateInput_text", LocateBy.Id);
        public TextBox StartTime = new TextBox("ctl00_cph_dtpEventsStartTime_dateInput_text", LocateBy.Id);
        public TextBox EndDate = new TextBox("ctl00_cph_dtpEventsEndDate_dateInput_text", LocateBy.Id);
        public TextBox EndTime = new TextBox("ctl00_cph_dtpEventsEndTime_dateInput_text", LocateBy.Id);
        public TextBox LocationName = new TextBox("ctl00_cph_txtEventsLocName", LocateBy.Id);
        public TextBox LocationPhone = new TextBox("ctl00_cph_txtEventsLocPhone", LocateBy.Id);
        public MultiChoiceDropdown Country = new MultiChoiceDropdown("ctl00_cph_ddlEventsLocCountry", LocateBy.Id);
        public TextBox AddressLineOne = new TextBox("ctl00_cph_txtEventsLocAddress1", LocateBy.Id);
        public TextBox AddressLineTwo = new TextBox("ctl00_cph_txtEventsLocAddress2", LocateBy.Id);
        public TextBox City = new TextBox("ctl00_cph_txtEventsLocCity", LocateBy.Id);
        public MultiChoiceDropdown State = new MultiChoiceDropdown("ctl00_cph_ctl00_cph_ddlRegion_ddlRegion", LocateBy.Id);
        public TextBox Province = new TextBox("ctl00_cph_ctl00_cph_ddlRegion_txtRegion", LocateBy.Id);
        public TextBox PostalCode = new TextBox("ctl00_cph_txtEventsLocPostalCode", LocateBy.Id);
        public ButtonOrLink EditContactInfo = new ButtonOrLink("ctl00_cph_elHomeContactInfo_linkCheckmarkfrmEventTextsHomeContactInfo", LocateBy.Id);
        public HtmlEditor ContactInfo = new HtmlEditor("dialog");
        public ButtonOrLink AddRegType = new ButtonOrLink("ctl00_cph_grdRegTypes_hlAddNew", LocateBy.Id);
        public ButtonOrLink EmptyAddRegType = new ButtonOrLink("ctl00_cph_grdRegTypes_lnkEmptyAdd", LocateBy.Id);
        public RegTypeDefine RegTypeDefine = new RegTypeDefine("dialog");
        public CheckBox RegTypeDisplayOption = new CheckBox("ctl00_cph_chkShowRegTypeOptions", LocateBy.Id);
        public MultiChoiceDropdown RegTypeDisplayFormat = new MultiChoiceDropdown("ctl00_cph_ddlEventsRegTypeTypeID", LocateBy.Id);
        public CheckBox AllGroupReg = new CheckBox("ctl00_cph_chkAllowGroups", LocateBy.Id);
        public CheckBox ForceSameGroupType = new CheckBox("ctl00_cph_chkEventsForceSameRegTypes", LocateBy.Id);
        public ButtonOrLink AddGroupDiscount = new ButtonOrLink("//*[@id='ctl00_cph_mdDiscountRules']/a", LocateBy.XPath);
        public GroupDiscountDefine GroupDiscountDefine = new GroupDiscountDefine("dialog");
        public CheckBox LimitRegs = new CheckBox("ctl00_cph_chkLimit", LocateBy.Id);
        public TextBox RegSpaces = new TextBox("ctl00_cph_txtEventCapacity_text", LocateBy.Id);
        public CheckBox EnableWaitlist = new CheckBox("ctl00_cph_chkEnableEventWaitlist", LocateBy.Id);
        public TextBox EventHome = new TextBox("ctl00_cph_txtHomeLink", LocateBy.Id);
        public ButtonOrLink StartPageHeader = new ButtonOrLink("//*[text()='Add Start Page Header']", LocateBy.XPath);
        public HtmlEditor StartPageHeaderEditor = new HtmlEditor("dialog");
        public ButtonOrLink StartPageFooter = new ButtonOrLink("//*[text()='Add Start Page Footer']", LocateBy.XPath);
        public HtmlEditor StartPageFooterEditor = new HtmlEditor("dialog");
        public Label EventId = new Label("eventId", LocateBy.Name);
        public CheckBox AllowChangeRegType = new CheckBox("ctl00_cph_chkEventsAllowRegTypeUpdates", LocateBy.Id);
        #endregion

        #region Basic Actions
        public void EventFee_Type(double price)
        {
            this.EventFee.WaitForDisplay();
            this.EventFee.Type(price);
        }

        public void EventFeeAdvanced_Click()
        {
            this.EventFeeAdvanced.WaitForDisplay();
            this.EventFeeAdvanced.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void StartDate_Type(DateTime date)
        {
            string dateFormat = "{0}/{1}/{2}";
            string dateString = string.Format(dateFormat, date.Month, date.Day, date.Year);
            this.StartDate.Type(dateString);
        }

        public void StartTime_Type(DateTime time)
        {
            string timeFormat = "{0}:{1}:{2}";
            string timeString = string.Format(timeFormat, time.Hour, time.Minute, time.Second);
            this.StartTime.Type(timeString);
        }

        public void EndDate_Type(DateTime date)
        {
            string dateFormat = "{0}/{1}/{2}";
            string dateString = string.Format(dateFormat, date.Month, date.Day, date.Year);
            this.EndDate.Type(dateString);
        }

        public void EndTime_Type(DateTime time)
        {
            string timeFormat = "{0}:{1}:{2}";
            string timeString = string.Format(timeFormat, time.Hour, time.Minute, time.Second);
            this.EndTime.Type(timeString);
        }

        public void Country_Select(string country)
        {
            this.Country.SelectWithText(country);
            WaitForAJAX();
        }

        public void EmptyAddRegType_Click()
        {
            this.EmptyAddRegType.WaitForDisplay();
            this.EmptyAddRegType.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddRegType_Click()
        {
            this.AddRegType.WaitForDisplay();
            this.AddRegType.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void RegTypeDisplayOption_Set(bool check)
        {
            this.RegTypeDisplayOption.Set(check);
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void AllGroupReg_Set(bool check)
        {
            this.AllGroupReg.Set(check);
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void AddGroupDiscount_Click()
        {
            this.AddGroupDiscount.WaitForDisplay();
            this.AddGroupDiscount.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AdvancedSettings_Click()
        {
            this.AdvancedSettings.WaitForDisplay();
            this.AdvancedSettings.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void LimitRegs_Set(bool check)
        {
            this.LimitRegs.Set(check);
            Utility.ThreadSleep(1.5);
            WaitForAJAX();
        }

        public void StartPageHeader_Click()
        {
            this.StartPageHeader.WaitForDisplay();
            this.StartPageHeader.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void StartPageFooter_Click()
        {
            this.StartPageFooter.WaitForDisplay();
            this.StartPageFooter.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void EditContactInfo_Click()
        {
            this.EditContactInfo.WaitForDisplay();
            this.EditContactInfo.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
        #endregion
    }

    public class RegTypeRow
    {
        public int RegTypeId;
        public string RegTypeName;
        public ButtonOrLink Title;

        public RegTypeRow(string regTypeName)
        {
            this.RegTypeName = regTypeName;

            this.Title = new ButtonOrLink(
                string.Format("//table[@id='ctl00_cph_grdRegTypes_tblGrid']/tbody/tr/td/a[text()='{0}']", this.RegTypeName),
                LocateBy.XPath);

            string regTypeHrefAttributeText = this.Title.GetAttribute("href");

            string tmp = regTypeHrefAttributeText.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)[1];
            tmp = tmp.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[1];
            tmp = tmp.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1];
            tmp = tmp.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries)[2];
            tmp = tmp.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1];

            this.RegTypeId = Convert.ToInt32(tmp);
        }

        public void Title_Click()
        {
            this.Title.WaitForDisplay();
            this.Title.Click();
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }
    }
}
