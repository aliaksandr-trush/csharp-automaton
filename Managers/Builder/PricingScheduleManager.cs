﻿namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Globalization;
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public class PricingScheduleManager : ManagerBase
    {
        #region Constants
        private const string AgendaAddEarlyPriceLinkLocator = "//tr[@id='ctl00_cph_ucCF_mipPrc_ip3_trEarlyPriceLink']/td/a";
        private const string AgendaAddLatePriceLinkLocator = "//tr[@id='ctl00_cph_ucCF_mipPrc_ip3_trLatePriceLink']/td/a";
        private const string AgendaEarlyPriceTxtbox = "ctl00_cph_ucCF_mipPrc_ip3_rntElyAmt";
        private const string AgendaEarlyPriceRegLimitRadioButton = "ctl00_cph_ucCF_mipPrc_ip3_rbEarlyPriceOptionQuantity";
        private const string AgendaEarlyPriceRegLimitTxtbox = "ctl00_cph_ucCF_mipPrc_ip3_rntEarlyPriceQuantity";
        private const string AgendaEarlyPriceDatetimeRadioButton = "ctl00_cph_ucCF_mipPrc_ip3_rbEarlyPriceOptionDate";
        private const string AgendaEarlyDateBox = "ctl00_cph_ucCF_mipPrc_ip3_dtpElyDt_tbDate";
        private const string AgendaEarlyTimeBox = "ctl00_cph_ucCF_mipPrc_ip3_tpElyEdT";
        private const string AgendaLatePriceTxtbox = "ctl00_cph_ucCF_mipPrc_ip3_rntLtAmt";
        private const string AgendaLateDateBox = "ctl00_cph_ucCF_mipPrc_ip3_dtpLtDt_tbDate";
        private const string AgendaLateTimeBox = "ctl00_cph_ucCF_mipPrc_ip3_tpLtSD";

        private const string EventfeeAddEarlyPriceLinkLocator = "//tr[@id='ctl00_cphDialog_cfCF_mipPrc_ip3_trEarlyPriceLink']/td/a";
        private const string EventfeeAddLatePriceLinkLocator = "//tr[@id='ctl00_cphDialog_cfCF_mipPrc_ip3_trLatePriceLink']/td/a";
        private const string EventfeeEarlyPriceTxtbox = "ctl00_cphDialog_cfCF_mipPrc_ip3_rntElyAmt";
        private const string EventfeeEarlyPriceRegLimitRadioButton = "ctl00_cphDialog_cfCF_mipPrc_ip3_rbEarlyPriceOptionQuantity";
        private const string EventfeeEarlyPriceRegLimitTxtbox = "ctl00_cphDialog_cfCF_mipPrc_ip3_rntEarlyPriceQuantity";
        private const string EventfeeEarlyPriceDatetimeRadioButton = "ctl00_cphDialog_cfCF_mipPrc_ip3_rbEarlyPriceOptionDate";
        private const string EventfeeEarlyDateBox = "ctl00_cphDialog_cfCF_mipPrc_ip3_dtpElyDt_tbDate";
        private const string EventfeeEarlyTimeBox = "ctl00_cphDialog_cfCF_mipPrc_ip3_tpElyEdT";
        private const string EventfeeLatePriceTxtbox = "ctl00_cphDialog_cfCF_mipPrc_ip3_rntLtAmt";
        private const string EventfeeLateDateBox = "ctl00_cphDialog_cfCF_mipPrc_ip3_dtpLtDt_tbDate";
        private const string EventfeeLateTimeBox = "ctl00_cphDialog_cfCF_mipPrc_ip3_tpLtSD";
        #endregion

        #region Locator variables needed to be initialized
        private string addEarlyPriceLinkLocator;
        private string addLatePriceLinkLocator;
        private string earlyPriceTxtbox;
        private string earlyPriceRegLimitRadioButton;
        private string earlyPriceRegLimitTxtbox;
        private string earlyPriceDatetimeRadioButton;
        private string earlyDateBox;
        private string earlyTimeBox;
        private string latePriceTxtbox;
        private string lateDateBox;
        private string lateTimeBox;
        #endregion

        #region Constructor
        public PricingScheduleManager(FormDetailManager.Page page) 
        {
            this.InitializeLocators(page);
        }
        #endregion

        #region Initialize locators
        private void InitializeLocators(FormDetailManager.Page page)
        {
            switch (page)
            {
                case FormDetailManager.Page.Agenda:
                    addEarlyPriceLinkLocator = AgendaAddEarlyPriceLinkLocator;
                    addLatePriceLinkLocator = AgendaAddLatePriceLinkLocator;
                    earlyPriceTxtbox = AgendaEarlyPriceTxtbox;
                    earlyPriceRegLimitRadioButton = AgendaEarlyPriceRegLimitRadioButton;
                    earlyPriceRegLimitTxtbox = AgendaEarlyPriceRegLimitTxtbox;
                    earlyPriceDatetimeRadioButton = AgendaEarlyPriceDatetimeRadioButton;
                    earlyDateBox = AgendaEarlyDateBox;
                    earlyTimeBox = AgendaEarlyTimeBox;
                    latePriceTxtbox = AgendaLatePriceTxtbox;
                    lateDateBox = AgendaLateDateBox;
                    lateTimeBox = AgendaLateTimeBox;
                    break;

                case FormDetailManager.Page.Start:
                    addEarlyPriceLinkLocator = EventfeeAddEarlyPriceLinkLocator;
                    addLatePriceLinkLocator = EventfeeAddLatePriceLinkLocator;
                    earlyPriceTxtbox = EventfeeEarlyPriceTxtbox;
                    earlyPriceRegLimitRadioButton = EventfeeEarlyPriceRegLimitRadioButton;
                    earlyPriceRegLimitTxtbox = EventfeeEarlyPriceRegLimitTxtbox;
                    earlyPriceDatetimeRadioButton = EventfeeEarlyPriceDatetimeRadioButton;
                    earlyDateBox = EventfeeEarlyDateBox;
                    earlyTimeBox = EventfeeEarlyTimeBox;
                    latePriceTxtbox = EventfeeLatePriceTxtbox;
                    lateDateBox = EventfeeLateDateBox;
                    lateTimeBox = EventfeeLateTimeBox;
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Set/Clear early/late pricing options
        public void ClickAddEarlyPriceButton()
        {
            // Click add agenda early price button if it's present
            if (UIUtil.DefaultProvider.IsElementPresent(addEarlyPriceLinkLocator, LocateBy.XPath))
            {
                UIUtil.DefaultProvider.WaitForDisplayAndClick(addEarlyPriceLinkLocator, LocateBy.XPath);
                UIUtil.DefaultProvider.WaitForAJAXRequest();
            }
        }

        public void ClickAddLatePriceButton()
        {
            // Click add agenda late price button if it's present
            if (UIUtil.DefaultProvider.IsElementPresent(addLatePriceLinkLocator, LocateBy.XPath))
            {
                UIUtil.DefaultProvider.WaitForDisplayAndClick(addLatePriceLinkLocator, LocateBy.XPath);
            }
        }

        public void ClickEarlyRegLimitRadioButton()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(earlyPriceRegLimitRadioButton, LocateBy.Id);
        }

        public void ClickEarlyDatetimeRadioButton()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(earlyPriceDatetimeRadioButton, LocateBy.Id);
        }

        public void ClearEarlyPricingSchedule()
        {
            ClearEarlyDate();
            ClearEarlyPrice();
            ClearEarlyRegLimit();
        }

        public void ClearLatePricingSchedule()
        {
            ClearLateDate();
            ClearLatePrice();
        }

        public void SetEarlyPricingSchedule(double price, int limit)
        {
            ClickAddEarlyPriceButton();
            SetEarlyPrice(price);
            ClickEarlyRegLimitRadioButton();
            SetEarlyRegLimit(limit);
        }

        public void SetEarlyPricingSchedule(double price, DateTime dateTime)
        {
            ClickAddEarlyPriceButton();
            SetEarlyPrice(price);
            ClickEarlyDatetimeRadioButton();
            SetEarlyDateTime(dateTime);
        }

        public void SetLatePricingSchedule(double price, DateTime dateTime)
        {
            ClickAddLatePriceButton();
            SetLatePrice(price);
            SetLateDateTime(dateTime);
        }

        public void SetEarlyRegLimit(int? limitNum)
        {
            UIUtil.DefaultProvider.TypeRADNumericById(this.earlyPriceRegLimitTxtbox, Convert.ToString(limitNum));
        }

        public int GetEarlyRegLimit()
        {
            return Convert.ToInt32(UIUtil.DefaultProvider.GetValue(this.earlyPriceRegLimitTxtbox, LocateBy.Id));
        }

        public void SetEarlyPrice(double? price)
        {
            UIUtil.DefaultProvider.WaitForElementPresent(earlyPriceTxtbox, LocateBy.Id);
            UIUtil.DefaultProvider.TypeRADNumericById(earlyPriceTxtbox, Convert.ToString(price));
            UIUtil.DefaultProvider.WaitForElementPresent(this.earlyPriceTxtbox, LocateBy.Id);
            UIUtil.DefaultProvider.TypeRADNumericById(this.earlyPriceTxtbox, Convert.ToString(price));
        }

        public double GetEarlyPrice()
        {
            return Convert.ToDouble(UIUtil.DefaultProvider.GetValue(this.earlyPriceTxtbox, LocateBy.Id));
        }

        public void SetLatePrice(double? price)
        {
            UIUtil.DefaultProvider.WaitForElementPresent(latePriceTxtbox, LocateBy.Id);
            UIUtil.DefaultProvider.TypeRADNumericById(latePriceTxtbox, Convert.ToString(price));
        }

        public double GetLatePrice()
        {
            return Convert.ToDouble(UIUtil.DefaultProvider.GetValue(this.latePriceTxtbox, LocateBy.Id));
        }

        public void SetEarlyDateTime(DateTime dateTime)
        {
            SetEarlyDate(dateTime);
            SetEarlyTime(dateTime);
        }

        public void SetEarlyDate(DateTime date)
        {
            string edate = string.Format("{0}/{1}/{2}", date.Month, date.Day, date.Year);

            //set early date
            UIUtil.DefaultProvider.Type(earlyDateBox, edate, LocateBy.Id);
        }

        public void SetEarlyTime(DateTime time)
        {
            string earlytime = string.Format("{0:yyyy-MM-dd-HH-mm-ss}", time);
            string eTime = string.Format("{0:h:mm tt}", time);

            //set early time   
            UIUtil.DefaultProvider.Type(earlyTimeBox + "_dateInput_text", eTime, LocateBy.Id);
            //Type(earlyTimeBox + "_dateInput", earlytime);
            //Type(earlyTimeBox, earlytime);
        }

        public void SetLateDateTime(DateTime dateTime)
        {
            SetLateDate(dateTime);
            SetLateTime(dateTime);
        }

        public void SetLateDate(DateTime date)
        {
            string ldate = string.Format("{0}/{1}/{2}", date.Month, date.Day, date.Year);

            //set early date
            UIUtil.DefaultProvider.Type(lateDateBox, ldate, LocateBy.Id);
        }

        public void SetLateTime(DateTime time)
        {
            string latetime = string.Format("{0:yyyy-MM-dd-HH-mm-ss}", time);
            string lTime = string.Format("{0:h:mm tt}", time);

            //set early time
            UIUtil.DefaultProvider.Type(lateTimeBox + "_dateInput_text", lTime, LocateBy.Id);
            //Type(lateTimeBox + "_dateInput", latetime);
            //Type(lateTimeBox, latetime);
        }

        public void ClearEarlyPrice()
        {
            SetEarlyPrice(null);
        }

        public void ClearEarlyDateTime()
        {
            ClearEarlyDate();
            ClearEarlyTime();
        }

        public void ClearEarlyDate()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(earlyDateBox, LocateBy.Id);
            UIUtil.DefaultProvider.Type(earlyDateBox, "", LocateBy.Id);
        }

        public void ClearEarlyTime()
        {
            ////UIUtilityProvider.UIHelper.Type(earlyTimeBox + "_dateInput_text", Keys.Delete, LocateBy.Id);
            UIUtil.DefaultProvider.Type(earlyTimeBox + "_dateInput_text", string.Empty, LocateBy.Id);
            //Type(earlyTimeBox + "_dateInput", "");
            //Type(earlyTimeBox, "");
        }

        public void ClearEarlyRegLimit()
        {
            SetEarlyRegLimit(null);
        }

        public void ClearLatePrice()
        {
            SetLatePrice(null);
        }

        public void ClearLateDateTime()
        {
            ClearLateDate();
            ClearLateTime();
        }

        public void ClearLateDate()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(lateDateBox, LocateBy.Id);
            UIUtil.DefaultProvider.Type(lateDateBox, string.Empty, LocateBy.Id);
        }

        public void ClearLateTime()
        {
            UIUtil.DefaultProvider.Type(lateTimeBox + "_dateInput_text", string.Empty, LocateBy.Id);
            //Type(lateTimeBox + "_dateInput", "");
            //Type(lateTimeBox, "");
        }
        #endregion

        #region Verify methods
        public void VerifyStandardPriceText(double price)
        {
            string standardPriceText = UIUtil.DefaultProvider.GetText("liStdPrice", LocateBy.Id);

            VerifyTool.VerifyValue(
                string.Format("Standard price is $ {0}", Convert.ToString(price)),
                standardPriceText, 
                "The standard price text is : {0}");
        }

        #region Verify early price options
        public void VerifyAddEarlyPriceOptionsLink()
        {
            if (UIUtil.DefaultProvider.IsElementHidden("ctl00_cphDialog_cfCF_mipPrc_ip3_trEarlyPriceLink", LocateBy.Id))
            {
                UIUtil.DefaultProvider.FailTest("The add early price link is not displayed!");
            }
        }

        public void VerifyEarlyPriceText(double price, int limit)
        {
            string earlyPriceText = UIUtil.DefaultProvider.GetText("liEarlyPrice", LocateBy.Id);

            VerifyTool.VerifyValue(
                string.Format("Early price of $ {0} is effective for the first {1} registrants", Convert.ToString(price), Convert.ToString(limit)),
                earlyPriceText, 
                "The early price with RegLimit text is : {0}");
        }

        public void VerifyEarlyPriceText(double price, DateTime dateTime)
        {
            string earlyDate = dateTime.ToString("M/d/yyyy", DateTimeFormatInfo.InvariantInfo);
            string earlyTime = dateTime.ToString("h:mm tt", DateTimeFormatInfo.InvariantInfo);

            string earlyPriceText = UIUtil.DefaultProvider.GetText("liEarlyPrice", LocateBy.Id);
            
            VerifyTool.VerifyValue(
                string.Format("Early price of $ {0} ends on {1} {2}", Convert.ToString(price), Convert.ToString(earlyDate), Convert.ToString(earlyTime)),
                earlyPriceText, 
                "The early price with DateTime text is : {0}");
        }

        public void VerifyEarlyPricingOptions(double price, int limit)
        {
            VerifyEarlyPrice(price);
            VerifyEarlyRegLimit(limit);
        }

        public void VerifyEarlyPricingOptions(double price, DateTime dateTime)
        {
            VerifyEarlyPrice(price);
            VerifyEarlyDateTime(dateTime);
        }

        public void VerifyEarlyPrice(double price)
        {
            VerifyTool.VerifyValue(Convert.ToString(price), UIUtil.DefaultProvider.GetValue(earlyPriceTxtbox, LocateBy.Id), "The early price amount is : {0}");
        }

        public void VerifyEarlyRegLimit(int limit)
        {
            VerifyTool.VerifyValue(true, UIUtil.DefaultProvider.IsChecked(earlyPriceRegLimitRadioButton, LocateBy.Id), "The early price reg limit radio button is checked : {0}");
            VerifyTool.VerifyValue(Convert.ToString(limit), UIUtil.DefaultProvider.GetValue(earlyPriceRegLimitTxtbox, LocateBy.Id), "The early reg limit is : {0}");
        }

        public void VerifyEarlyDateTime(DateTime dateTime)
        {
            VerifyTool.VerifyValue(true, UIUtil.DefaultProvider.IsChecked(earlyPriceDatetimeRadioButton, LocateBy.Id), "The early price datetime radio button is checked : {0}");
            VerifyEarlyDate(dateTime);
            VerifyEarlyTime(dateTime);
        }

        public void VerifyEarlyDate(DateTime date)
        {
            string earlyDate = string.Format("{0}/{1}/{2}", date.Month, date.Day, date.Year);
            VerifyTool.VerifyValue(earlyDate, UIUtil.DefaultProvider.GetValue(earlyDateBox, LocateBy.Id), "The early date is : {0}");
        }

        public void VerifyEarlyTime(DateTime time)
        {
            string earlyTime = string.Format("{0:h:mm tt}", time);
            VerifyTool.VerifyValue(earlyTime, UIUtil.DefaultProvider.GetValue(earlyTimeBox + "_dateInput_text", LocateBy.Id), "The early time is: {0}");
        }
        #endregion

        #region Verify late price options
        public void VerifyAddLatePriceOptionsLink()
        {
            if (UIUtil.DefaultProvider.IsElementHidden("ctl00_cphDialog_cfCF_mipPrc_ip3_trLatePriceLink", LocateBy.Id))
            {
                UIUtil.DefaultProvider.FailTest("The add late price link is not displayed!");
            }
        }

        public void VerifyLatePriceText(double price, DateTime dateTime)
        {
            string lateDate = dateTime.ToString("M/d/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string lateTime = dateTime.ToString("h:mm tt", System.Globalization.DateTimeFormatInfo.InvariantInfo);

            string latePriceText = UIUtil.DefaultProvider.GetText("liLatePrice", LocateBy.Id);

            VerifyTool.VerifyValue(
                string.Format("Late/Onsite price of $ {0} starts on {1} {2}", Convert.ToString(price), Convert.ToString(lateDate), Convert.ToString(lateTime)),
                latePriceText, 
                "The late price text is : {0}");
        }

        public void VerifyLatePricingOptions(double price, DateTime dateTime)
        {
            VerifyLatePrice(price);
            VerifyLateDateTime(dateTime);
        }

        public void VerifyLatePrice(double price)
        {
            UIUtil.DefaultProvider.WaitForElementPresent(latePriceTxtbox, LocateBy.Id);
            VerifyTool.VerifyValue(Convert.ToString(price), UIUtil.DefaultProvider.GetValue(latePriceTxtbox, LocateBy.Id), "The late price amount is : {0}");
        }

        public void VerifyLateDateTime(DateTime dateTime)
        {
            VerifyLateDate(dateTime);
            VerifyLateTime(dateTime);
        }

        public void VerifyLateDate(DateTime date)
        {
            string lateDate = string.Format("{0}/{1}/{2}", date.Month, date.Day, date.Year);
            VerifyTool.VerifyValue(lateDate, UIUtil.DefaultProvider.GetValue(lateDateBox, LocateBy.Id), "The late date is : {0}");
        }

        public void VerifyLateTime(DateTime time)
        {
            string lateTime = string.Format("{0:h:mm tt}", time);
            VerifyTool.VerifyValue(lateTime, UIUtil.DefaultProvider.GetValue(lateTimeBox + "_dateInput_text", LocateBy.Id), "The late time is: {0}");
        }
        #endregion

        #endregion
    }
}
