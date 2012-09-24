namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class EventFeeDefine : Frame
    {
        public EventFeeDefine(string name) : base(name) { }

        #region WebElements
        public Input NameOnReceipt = new Input("ctl00_cphDialog_cfCF_mipNam_ip1_elRptDesc_TextArea", LocateBy.Id);
        public Input NameOnReports = new Input("ctl00_cphDialog_cfCF_mipNam_ip1_txtFieldName", LocateBy.Id);
        public Input StandardPrice = new Input("ctl00_cphDialog_cfCF_mipPrc_rntAmount_text", LocateBy.Id);
        public Clickable Options = new Clickable("ctl00_cphDialog_cfCF_mipPrc_MoreInfoButtonPricing1_optionsLink", LocateBy.Id);
        public Clickable AddEarlyPrice = new Clickable("//*[@id='ctl00_cphDialog_cfCF_mipPrc_ip3_trEarlyPriceLink']/td/a", LocateBy.XPath);
        public Input EarlyPrice = new Input("ctl00_cphDialog_cfCF_mipPrc_ip3_rntElyAmt_text", LocateBy.Id);
        public RadioButton EarlyPriceDateTime = new RadioButton("ctl00_cphDialog_cfCF_mipPrc_ip3_rbEarlyPriceOptionDate", LocateBy.Id);
        public Input EarlyPriceDate = new Input("ctl00_cphDialog_cfCF_mipPrc_ip3_dtpElyDt_tbDate", LocateBy.Id);
        public Input EarlyPriceTime = new Input("ctl00_cphDialog_cfCF_mipPrc_ip3_tpElyEdT_dateInput_text", LocateBy.Id);
        public RadioButton EarlyPriceRegLimit = new RadioButton("ctl00_cphDialog_cfCF_mipPrc_ip3_rbEarlyPriceOptionQuantity", LocateBy.Id);
        public Input EarlyPriceRegistrations = new Input("ctl00_cphDialog_cfCF_mipPrc_ip3_rntEarlyPriceQuantity_text", LocateBy.Id);
        public Clickable AddLatePrice = new Clickable("//*[@id='ctl00_cphDialog_cfCF_mipPrc_ip3_trLatePriceLink']/td/a", LocateBy.XPath);
        public Input LatePrice = new Input("ctl00_cphDialog_cfCF_mipPrc_ip3_rntLtAmt_text", LocateBy.Id);
        public Input LatePriceDate = new Input("ctl00_cphDialog_cfCF_mipPrc_ip3_dtpLtDt_tbDate", LocateBy.Id);
        public Input LatePriceTime = new Input("ctl00_cphDialog_cfCF_mipPrc_ip3_tpLtSD_dateInput_text", LocateBy.Id);
        public Clickable AddDiscountCode = new Clickable("ctl00_cphDialog_cfCF_mipPrc_ip4_hrefOpenCodeWindow", LocateBy.Id);
        public Clickable AddBulkCodes = new Clickable("ctl00_cphDialog_cfCF_mipPrc_ip4_hrefOpenBulkCodesWindow", LocateBy.Id);
        public CodeDefine Code_Define = new CodeDefine("dialog3");
        public CheckBox RequireCode = new CheckBox("ctl00_cphDialog_cfCF_mipPrc_ip4_chkPasswordRequired", LocateBy.Id);
        public Clickable AddTaxRate = new Clickable("//div[@id='ctl00_cphDialog_cfCF_mipPrc_ip6_dvTxWarn']/a", LocateBy.XPath);
        public TaxRateDefine TaxRate_Define = new TaxRateDefine("dialog3");
        public CheckBox ApplyTaxOne = new CheckBox("ctl00_cphDialog_cfCF_mipPrc_ip6_chkListTaxRates_0", LocateBy.Id);
        public CheckBox ApplyTaxTwo = new CheckBox("ctl00_cphDialog_cfCF_mipPrc_ip6_chkListTaxRates_1", LocateBy.Id);
        private ElementBase Options_Div = new ElementBase("ctl00_cphDialog_cfCF_mipPrc_ctl02", LocateBy.Id);
        #endregion

        #region Basic Actions
        public void Options_Expand()
        {
            this.Options_Div.WaitForPresent();

            if (this.Options_Div.GetAttribute("style").Contains("display: none;"))
            {
                this.Options_Click();
            }
        }

        public void Options_Collapse()
        {
            this.Options_Div.WaitForPresent();

            if (!this.Options_Div.GetAttribute("style").Contains("display: none;"))
            {
                this.Options_Click();
            }
        }

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