namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class RegTypeDefine : Frame
    {
        public RegTypeDefine(string name)
            : base(name)
        { }

        #region WebElements
        public Input NameOnForm = new Input("ctl00_cphDialog_txtDescription", LocateBy.Id);
        public Input NameOnReports = new Input("ctl00_cphDialog_txtReportDescription", LocateBy.Id);
        public CheckBox RegTypeLimit = new CheckBox("ctl00_cphDialog_nonEnduranceLimiting_chkLimitRegs", LocateBy.Id);
        public RadioButton LimitToIndividual = new RadioButton("ctl00_cphDialog_nonEnduranceLimiting_rbInvidividualLimit", LocateBy.Id);
        public Input IndividualLimit = new Input("ctl00_cphDialog_nonEnduranceLimiting_txtLimit0_text", LocateBy.Id);
        public RadioButton LimitToGroup = new RadioButton("ctl00_cphDialog_nonEnduranceLimiting_rbGroupLimit", LocateBy.Id);
        public Input GroupLimit = new Input("ctl00_cphDialog_nonEnduranceLimiting_txtLimitGroups_text", LocateBy.Id);
        public Input SoldOutMessage = new Input("ctl00_cphDialog_nonEnduranceLimiting_LimitMessage", LocateBy.Id);
        public Clickable AdditionalDetails = new Clickable("ctl00_cphDialog_elRegTypeInfo_linkCheckmarktext_elRegTypeInfo", LocateBy.Id);
        public HtmlEditor AdditionalDetailsEditor = new HtmlEditor("dialog2");
        public CheckBox Public = new CheckBox("ctl00_cphDialog_chkPublic", LocateBy.Id);
        public CheckBox Admin = new CheckBox("ctl00_cphDialog_chkAdmin", LocateBy.Id);
        public CheckBox OnSite = new CheckBox("ctl00_cphDialog_chkOnsite", LocateBy.Id);
        public Input EventFee = new Input("ctl00_cphDialog_txtCost_text", LocateBy.Id);
        public Clickable EventFeeAdvanced = new Clickable("ctl00_cphDialog_mdCostLink", LocateBy.Id);
        public RegTypeFeeDefine RegTypeFee_Define = new RegTypeFeeDefine("dialog2");
        public Input ShowDate = new Input("ctl00_cphDialog_dtpShowDate_tbDate", LocateBy.Id);
        public Input HideDate = new Input("ctl00_cphDialog_dtpHideDate_tbDate", LocateBy.Id);
        public Input MinGroupSize = new Input("ctl00_cphDialog_MinRegs_text", LocateBy.Id);
        public Input MaxGroupSize = new Input("ctl00_cphDialog_MaxRegs_text", LocateBy.Id);
        public Clickable AddMinRegMessage = new Clickable("ctl00_cphDialog_elMinRegsMessage_linkCheckmarktext_elMinRegsMessage", LocateBy.Id);
        public HtmlEditor MinRegMessageEditor = new HtmlEditor("dialog2");
        public Clickable ExternalAuthentication = new Clickable("ctl00_cphDialog_btnSetupExAuth", LocateBy.Id);
        public Manager.SSOBase ExternalAuthenticationSetup = new Manager.SSOBase("dialog2");
        public CheckBox EnableExternalAuthentication = new CheckBox("ctl00_cphDialog_chkXAuthEnable", LocateBy.Id);
        #endregion

        #region Basic Actions
        public void RegTypeLimit_Set(bool check)
        {
            this.RegTypeLimit.Set(check);
            WaitForAJAX();
        }

        public void LimitToIndividual_Click()
        {
            this.LimitToIndividual.WaitForDisplay();
            this.LimitToIndividual.Click();
            WaitForAJAX();
        }

        public void LimitToGroup_Click()
        {
            this.LimitToGroup.WaitForDisplay();
            this.LimitToGroup.Click();
            WaitForAJAX();
        }

        public void AdditionalDetails_Click()
        {
            this.AdditionalDetails.WaitForDisplay();
            this.AdditionalDetails.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void EventFeeAdvanced_Click()
        {
            this.EventFeeAdvanced.WaitForDisplay();
            this.EventFeeAdvanced.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void ShowDate_Type(DateTime date)
        {
            string dateFormat = "{0}/{1}/{2}";
            string dateString = string.Format(dateFormat, date.Month, date.Day, date.Year);
            this.ShowDate.Type(dateString);
        }

        public void HideDate_Type(DateTime date)
        {
            string dateFormat = "{0}/{1}/{2}";
            string dateString = string.Format(dateFormat, date.Month, date.Day, date.Year);
            this.HideDate.Type(dateString);
        }

        public void AddMinRegMessage_Click()
        {
            this.AddMinRegMessage.WaitForDisplay();
            this.AddMinRegMessage.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void ExternalAuthentication_Click()
        {
            this.ExternalAuthentication.WaitForDisplay();
            this.ExternalAuthentication.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void SaveAndStay_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndStay_Click();
        }

        public void SaveAndClose_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndClose_Click();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            PageObjectHelper.PopupFrame_Helper.Cancel_Click();
            SwitchToMain();
        }
        #endregion

        public void AdjustRADWindowPositionAndResize()
        {
            PageObject.PageObjectHelper.AdjustRADWindowPosition("RadWindowWrapper_ctl00_dialog", 20, 20);
            PageObject.PageObjectHelper.ResizeRADWindow(this.Name, 800, 1000);
        }
    }
}
