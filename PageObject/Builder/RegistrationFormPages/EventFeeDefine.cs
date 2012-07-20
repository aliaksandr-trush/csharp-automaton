namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class EventFeeDefine : Frame
    {
        public EventFeeDefine(string name) : base(name) { }
        public EventFeeDefine(string name, string parentFrame) : base(name, parentFrame) { }

        #region WebElements
        public TextBox NameOnReceipt = new TextBox("ctl00_cphDialog_cfCF_mipNam_ip1_elRptDesc_TextArea", LocateBy.Id);
        public TextBox NameOnReports = new TextBox("ctl00_cphDialog_cfCF_mipNam_ip1_txtFieldName", LocateBy.Id);
        public TextBox StandardPrice = new TextBox("ctl00_cphDialog_cfCF_mipPrc_rntAmount_text", LocateBy.Id);
        public ButtonOrLink Options = new ButtonOrLink("ctl00_cphDialog_cfCF_mipPrc_MoreInfoButtonPricing1_optionsLink", LocateBy.Id);
        public ButtonOrLink AddEarlyPrice = new ButtonOrLink("//*[@id='ctl00_cphDialog_cfCF_mipPrc_ip3_trEarlyPriceLink']/td/a", LocateBy.XPath);
        public TextBox EarlyPrice = new TextBox("ctl00_cphDialog_cfCF_mipPrc_ip3_rntElyAmt_text", LocateBy.Id);
        public RadioButton EarlyPriceDateTime = new RadioButton("ctl00_cphDialog_cfCF_mipPrc_ip3_rbEarlyPriceOptionDate", LocateBy.Id);
        public TextBox EarlyPriceDate = new TextBox("ctl00_cphDialog_cfCF_mipPrc_ip3_dtpElyDt_tbDate", LocateBy.Id);
        public TextBox EarlyPriceTime = new TextBox("ctl00_cphDialog_cfCF_mipPrc_ip3_tpElyEdT_dateInput_text", LocateBy.Id);
        public RadioButton EarlyPriceRegLimit = new RadioButton("ctl00_cphDialog_cfCF_mipPrc_ip3_rbEarlyPriceOptionQuantity", LocateBy.Id);
        public TextBox EarlyPriceRegistrations = new TextBox("ctl00_cphDialog_cfCF_mipPrc_ip3_rntEarlyPriceQuantity_text", LocateBy.Id);
        public ButtonOrLink AddLatePrice = new ButtonOrLink("//*[@id='ctl00_cphDialog_cfCF_mipPrc_ip3_trLatePriceLink']/td/a", LocateBy.XPath);
        public TextBox LatePrice = new TextBox("ctl00_cphDialog_cfCF_mipPrc_ip3_rntLtAmt_text", LocateBy.Id);
        public TextBox LatePriceDate = new TextBox("ctl00_cphDialog_cfCF_mipPrc_ip3_dtpLtDt_tbDate", LocateBy.Id);
        public TextBox LatePriceTime = new TextBox("ctl00_cphDialog_cfCF_mipPrc_ip3_tpLtSD_dateInput_text", LocateBy.Id);
        public ButtonOrLink AddDiscountCode = new ButtonOrLink("ctl00_cphDialog_cfCF_mipPrc_ip4_hrefOpenCodeWindow", LocateBy.Id);
        public ButtonOrLink AddBulkCodes = new ButtonOrLink("ctl00_cphDialog_cfCF_mipPrc_ip4_hrefOpenBulkCodesWindow", LocateBy.Id);
        public CodeDefine CodeDefine = new CodeDefine("dialog3", "dialog2");
        public CheckBox RequireCode = new CheckBox("ctl00_cphDialog_cfCF_mipPrc_ip4_chkPasswordRequired", LocateBy.Id);
        public ButtonOrLink AddTaxRate = new ButtonOrLink("//div[@id='ctl00_cphDialog_cfCF_mipPrc_ip6_dvTxWarn']/a", LocateBy.XPath);
        public TaxRateDefine TaxRateDefine = new TaxRateDefine("dialog3", "dialog2");
        public CheckBox ApplyTaxOne = new CheckBox("ctl00_cphDialog_cfCF_mipPrc_ip6_chkListTaxRates_0", LocateBy.Id);
        public CheckBox ApplyTaxTwo = new CheckBox("ctl00_cphDialog_cfCF_mipPrc_ip6_chkListTaxRates_1", LocateBy.Id);
        #endregion

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        #region Basic Actions
        public void Options_Click()
        {
            this.Options.WaitForDisplay();
            this.Options.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddEarlyPrice_Click()
        {
            this.AddEarlyPrice.WaitForDisplay();
            this.AddEarlyPrice.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            WaitForLoad();
        }

        public void EarlyPriceDate_Type(DateTime date)
        {
            string dateFormat = "{0}/{1}/{2}";
            string dateString = string.Format(dateFormat, date.Month, date.Day, date.Year);
            this.EarlyPriceDate.Type(dateString);
        }

        public void EarlyPriceTime_Type(DateTime time)
        {
            string timeFormat = "{0}:{1}:{2}";
            string timeString = string.Format(timeFormat, time.Hour, time.Minute, time.Second);
            this.EarlyPriceTime.Type(timeString);
        }

        public void AddLatePrice_Click()
        {
            this.AddLatePrice.WaitForDisplay();
            this.AddLatePrice.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            WaitForLoad();
        }

        public void LatePriceDate_Type(DateTime date)
        {
            string dateFormat = "{0}/{1}/{2}";
            string dateString = string.Format(dateFormat, date.Month, date.Day, date.Year);
            this.LatePriceDate.Type(dateString);
        }

        public void LatePriceTime_Type(DateTime time)
        {
            string timeFormat = "{0}:{1}:{2}";
            string timeString = string.Format(timeFormat, time.Hour, time.Minute, time.Second);
            this.LatePriceTime.Type(timeString);
        }

        public void AddDiscountCode_Click()
        {
            this.AddDiscountCode.WaitForDisplay();
            this.AddDiscountCode.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddBulkCodes_Click()
        {
            this.AddBulkCodes.WaitForDisplay();
            this.AddBulkCodes.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddTaxRate_Click()
        {
            this.AddTaxRate.WaitForDisplay();
            this.AddTaxRate.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void SaveAndStay_Click()
        {
            popupFrameHelper.SaveAndStay_Click();
        }

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            SelectParentFrame();
        }

        public void Cancel_Click()
        {
            popupFrameHelper.Cancel_Click();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            SelectParentFrame();
        }
        #endregion

        public void AdjustRADWindowPositionAndResize()
        {
            PageObject.PageObjectHelper.AdjustRADWindowPosition("RadWindowWrapper_ctl00_dialog2", 20, 20);
            PageObject.PageObjectHelper.ResizeRADWindow(this.Name, 800, 1000);
        }
    }
}
