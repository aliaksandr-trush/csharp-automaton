namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class Agenda : Window
    {
        #region WebElements
        public ButtonOrLink CreateAgendaItem = new ButtonOrLink("Create Agenda Item", LocateBy.LinkText);
        public ButtonOrLink AddAgendaItem = new ButtonOrLink("Add Agenda Item", LocateBy.LinkText);
        public ButtonOrLink CreateActivities = new ButtonOrLink("Create Activities", LocateBy.LinkText);
        public ButtonOrLink AddActivities = new ButtonOrLink("Add Activities", LocateBy.LinkText);
        public TextBox NameOnForm = new TextBox("ctl00_cph_ucCF_mipNam_elDesc_TextArea", LocateBy.Id);
        public ButtonOrLink NameOptions = new ButtonOrLink("ctl00_cph_ucCF_mipNam_bccMoreInfoNaming_optionsLink", LocateBy.Id);
        public TextBox NameOnReceipt = new TextBox("ctl00_cph_ucCF_mipNam_ip1_elRptDesc_TextArea", LocateBy.Id);
        public TextBox NameOnReports = new TextBox("ctl00_cph_ucCF_mipNam_ip1_txtFieldName", LocateBy.Id);
        public TextBox NameOnBadge = new TextBox("//*[@id='divBadgeCaption']//input", LocateBy.XPath);
        public ButtonOrLink FieldType = new ButtonOrLink("ctl00_cph_ucCF_selectedFieldTypeToggleImageSpan", LocateBy.Id);
        public ButtonOrLink SaveItem = new ButtonOrLink("Save Item", LocateBy.LinkText);
        public ButtonOrLink SaveAndNew = new ButtonOrLink("Save & New", LocateBy.LinkText);
        public ButtonOrLink Cancel = new ButtonOrLink("Cancel", LocateBy.LinkText);
        public ButtonOrLink AgendaPageHeader = new ButtonOrLink("//*[text()='Add Agenda Page Header']", LocateBy.XPath);
        public HtmlEditor AgendaPageHeaderEditor = new HtmlEditor("dialog");
        public ButtonOrLink AgendaPageFooter = new ButtonOrLink("//*[text()='Add Agenda Page Footer']", LocateBy.XPath);
        public HtmlEditor AgendaPageFooterEditor = new HtmlEditor("dialog");
        public TextBox StartDate = new TextBox("ctl00_cph_ucCF_dtpSD_dateInput_text", LocateBy.Id);
        public TextBox StartTime = new TextBox("ctl00_cph_ucCF_dtpST_dateInput_text", LocateBy.Id);
        public TextBox EndDate = new TextBox("ctl00_cph_ucCF_dtpED_dateInput_text", LocateBy.Id);
        public TextBox EndTime = new TextBox("ctl00_cph_ucCF_dtpET_dateInput_text", LocateBy.Id);
        public TextBox Location = new TextBox("ctl00_cph_ucCF_txtRoom", LocateBy.Id);
        public TextBox StandardPrice = new TextBox("ctl00_cph_ucCF_mipPrc_rntAmount_text", LocateBy.Id);
        public ButtonOrLink PriceOptionsLink = new ButtonOrLink("ctl00_cph_ucCF_mipPrc_MoreInfoButtonPricing1_optionsLink", LocateBy.Id);
        public ButtonOrLink AddEarlyPrice = new ButtonOrLink("//*[@id='ctl00_cph_ucCF_mipPrc_ip3_trEarlyPriceLink']/td/a", LocateBy.XPath);
        public TextBox EarlyPrice = new TextBox("ctl00_cph_ucCF_mipPrc_ip3_rntElyAmt_text", LocateBy.Id);
        public RadioButton EarlyPriceDateTime = new RadioButton("ctl00_cph_ucCF_mipPrc_ip3_rbEarlyPriceOptionDate", LocateBy.Id);
        public TextBox EarlyPriceDate = new TextBox("ctl00_cph_ucCF_mipPrc_ip3_dtpElyDt_tbDate", LocateBy.Id);
        public TextBox EarlyPriceTime = new TextBox("ctl00_cph_ucCF_mipPrc_ip3_tpElyEdT_dateInput_text", LocateBy.Id);
        public RadioButton EarlyPriceRegLimit = new RadioButton("ctl00_cph_ucCF_mipPrc_ip3_rbEarlyPriceOptionQuantity", LocateBy.Id);
        public TextBox EarlyPriceRegistrations = new TextBox("ctl00_cph_ucCF_mipPrc_ip3_rntEarlyPriceQuantity_text", LocateBy.Id);
        public ButtonOrLink AddLatePrice = new ButtonOrLink("//*[@id='ctl00_cph_ucCF_mipPrc_ip3_trLatePriceLink']/td/a", LocateBy.XPath);
        public TextBox LatePrice = new TextBox("ctl00_cph_ucCF_mipPrc_ip3_rntLtAmt_text", LocateBy.Id);
        public TextBox LatePriceDate = new TextBox("ctl00_cph_ucCF_mipPrc_ip3_dtpLtDt_tbDate", LocateBy.Id);
        public TextBox LatePriceTime = new TextBox("ctl00_cph_ucCF_mipPrc_ip3_tpLtSD_dateInput_text", LocateBy.Id);
        public ButtonOrLink AddDiscountCode = new ButtonOrLink("ctl00_cph_ucCF_mipPrc_ip4_hrefOpenCodeWindow", LocateBy.Id);
        public ButtonOrLink AddBulkCodes = new ButtonOrLink("ctl00_cph_ucCF_mipPrc_ip4_hrefOpenBulkCodesWindow", LocateBy.Id);
        public CodeDefine CodeDefine = new CodeDefine("dialog");
        public BulkLoadCodesDefine BulkLoadCodesDefine = new BulkLoadCodesDefine("dialog");
        public CheckBox RequireCode = new CheckBox("ctl00_cph_ucCF_mipPrc_ip4_chkPasswordRequired", LocateBy.Id);
        public ButtonOrLink AddTaxRate = new ButtonOrLink("//div[@id='ctl00_cph_ucCF_mipPrc_ip6_dvTxWarn']/a", LocateBy.XPath);
        public TaxRateDefine TaxRateDefine = new TaxRateDefine("dialog");
        public CheckBox ApplyTaxOne = new CheckBox("ctl00_cph_ucCF_mipPrc_ip6_chkListTaxRates_0", LocateBy.Id);
        public CheckBox ApplyTaxTwo = new CheckBox("ctl00_cph_ucCF_mipPrc_ip6_chkListTaxRates_1", LocateBy.Id);
        public TextBox Capacity = new TextBox("ctl00_cph_ucCF_mipCap_rntMaxQuantity_text", LocateBy.Id);
        public ButtonOrLink CapacityOptionsLink = new ButtonOrLink("ctl00_cph_ucCF_mipCap_MoreInfoButtonCapacity_optionsLink", LocateBy.Id);
        public CheckBox ShowRemainingCapacity = new CheckBox("ctl00_cph_ucCF_mipCap_ip7_cbShowRemainingCapacity", LocateBy.Id);
        public RadioButton HideWhenLimitReached = new RadioButton("ctl00_cph_ucCF_mipCap_ip7_rbHideThisItem", LocateBy.Id);
        public RadioButton ShowMessageWhenLimitReached = new RadioButton("ctl00_cph_ucCF_mipCap_ip7_rbDisplayThisMessage", LocateBy.Id);
        public ButtonOrLink AddLimitReachedMessage = new ButtonOrLink("ctl00_cph_ucCF_mipCap_ip7_elSoldOutMessage_linkCheckmarkCustomField0SoldOutMessage", LocateBy.Id);
        public HtmlEditor LimitReachedMessageEditor = new HtmlEditor("dialog");
        public RadioButton WaitlistWhenLimitReached = new RadioButton("ctl00_cph_ucCF_mipCap_ip7_rbActivateWaitlist", LocateBy.Id);
        public ButtonOrLink AddTextToConfirmation = new ButtonOrLink("ctl00_cph_ucCF_mipCap_ip7_elWaitlistConfirmation_linkCheckmarkCustomField0", LocateBy.Id);
        public EmailEditor TextToConfirmationEditor = new EmailEditor("dialog");
        public ButtonOrLink ShowAllRegTypes = new ButtonOrLink("//*[@id='tblRegTypesLink']//td[@class='fieldCaption']/a", LocateBy.XPath);
        public CheckBox VisibleToAll = new CheckBox("ctl00_cph_ucCF_chkActive", LocateBy.Id);
        public CheckBox RequiredByAll = new CheckBox("ctl00_cph_ucCF_chkRequired", LocateBy.Id);
        public CheckBox AdminOnlyToAll = new CheckBox("ctl00_cph_ucCF_chkAdminOnly", LocateBy.Id);
        public ButtonOrLink ExpandConditionalLogic = new ButtonOrLink("//div[@id='ctl00_cph_ucCF_CFParentTree']//span[@class='rtPlus']", LocateBy.XPath);
        public CheckBox AddToCalendar = new CheckBox("ctl00_cph_ucCF_chkDisplayCalendar", LocateBy.Id);
        public CheckBox IncludeOnEventWeb = new CheckBox("ctl00_cph_ucCF_chkIncludeOnAgenda", LocateBy.Id);
        public TextBox ShowStarting = new TextBox("ctl00_cph_ucCF_dtpShowDate_tbDate", LocateBy.Id);
        public TextBox HideStarting = new TextBox("ctl00_cph_ucCF_dtpHideDate_tbDate", LocateBy.Id);
        public MultiChoiceDropdown Gender = new MultiChoiceDropdown("ctl00_cph_ucCF_ddlVisibleForGender", LocateBy.Id);
        public TextBox AgeGreaterThan = new TextBox("ctl00_cph_ucCF_rntVisibleIfAgeGreater_text", LocateBy.Id);
        public TextBox AgeGreaterThanDate = new TextBox("ctl00_cph_ucCF_dtpVisibleIfAgeGreater_tbDate", LocateBy.Id);
        public TextBox AgeLessThan = new TextBox("ctl00_cph_ucCF_rntVisibleIfAgeLess_text", LocateBy.Id);
        public TextBox AgeLessThanDate = new TextBox("ctl00_cph_ucCF_dtpVisibleIfAgeLess_tbDate", LocateBy.Id);
        public MultiChoiceDropdown DateFormat = new MultiChoiceDropdown("ctl00_cph_ucCF_ddlDateFormats", LocateBy.Id);
        public ButtonOrLink AddDetails = new ButtonOrLink("ctl00_cph_ucCF_elSessionInfo_linkCheckmarktext_elSessionInfo", LocateBy.Id);
        public HtmlEditor DetailsEditor = new HtmlEditor("dialog");
        public TextBox DetailsURL = new TextBox("ctl00_cph_ucCF_txtSessionLink", LocateBy.Id);
        public TextBox GroupName = new TextBox("ctl00_cph_ucCF_txtGroupName", LocateBy.Id);
        public CheckBox ForceGroupToMatch = new CheckBox("ctl00_cph_ucCF_chkEnablePrePopulate", LocateBy.Id);
        public MultiChoiceDropdown InitialStatus = new MultiChoiceDropdown("ctl00_cph_ucCF_ddlDefaultStatusID", LocateBy.Id);
        public ButtonOrLink AddConfirmationAddendum = new ButtonOrLink("ctl00_cph_ucCF_elNonWaitlistConfirmation_linkCheckmarktext_elNonWaitlistConfirmation", LocateBy.Id);
        public EmailEditor ConfirmationAddendumEditor = new EmailEditor("dialog");
        public Label AgendaItemId = new Label("ctl00_cph_ucCF_currentCustomFieldId", LocateBy.Id);
        public ButtonOrLink AddMultipleChoiceItem = new ButtonOrLink("Add Multiple Choice Item", LocateBy.LinkText);
        public MultipleChoiceDefine MultipleChoiceDefine = new MultipleChoiceDefine("dialog");
        public ButtonOrLink AddCommonlyUsedItem = new ButtonOrLink("Add Commonly-Used Items", LocateBy.LinkText);
        public CommonlyUsedItemsDefine CommonlyUsedItemsDefine = new CommonlyUsedItemsDefine("dialog");
        public TextBox CharacterLimit = new TextBox("ctl00_cph_ucCF_rntLn_text", LocateBy.Id);
        public TextBox ParagraphLimit = new TextBox("ctl00_cph_ucCF_rntMultipleLineLn_text", LocateBy.Id);
        public Label AgendaChoiceItemCount = new Label("//*[@id='ctl00_cph_ucCF_grdLI_tblGrid']/tbody/tr[@class='dragTR']", LocateBy.XPath);
        public TextBox MinAmount = new TextBox("ctl00_cph_ucCF_mipPrc_rntMinVarAmount_text", LocateBy.Id);
        public TextBox MaxAmount = new TextBox("ctl00_cph_ucCF_mipPrc_rntMaxVarAmount_text", LocateBy.Id);
        #endregion

        #region Generate Some Elements
        public CheckBox VisibleToRegType(int regTypeId)
        {
            return new CheckBox(string.Format("ctl00_cph_ucCF_chkCFRegTypeActive{0}", regTypeId), LocateBy.Id);
        }

        public CheckBox RequiredByRegType(int regTypeId)
        {
            return new CheckBox(string.Format("ctl00_cph_ucCF_chkCFRegTypeRequired{0}", regTypeId), LocateBy.Id);
        }

        public CheckBox AdminOnlyToRegType(int regTypeId)
        {
            return new CheckBox(string.Format("ctl00_cph_ucCF_chkCFRegTypeAdminOnly{0}", regTypeId), LocateBy.Id);
        }

        public CheckBox ConditionalLogicParent(string name)
        {
            return new CheckBox(string.Format("//div[@id='ctl00_cph_ucCF_CFParentTree']//*[text()='{0}']/../input", name), LocateBy.XPath);
        }
        #endregion

        #region Basic Actions
        public void CreateAgendaItem_Click()
        {
            this.CreateAgendaItem.WaitForDisplay();
            this.CreateAgendaItem.Click();
            Utility.ThreadSleep(3);
            WaitForAJAX();
        }

        public void AddAgendaItem_Click()
        {
            this.AddAgendaItem.WaitForDisplay();
            this.AddAgendaItem.Click();
            Utility.ThreadSleep(3);
            WaitForAJAX();
        }

        public void CreateActivities_Click()
        {
            this.CreateActivities.WaitForDisplay();
            this.CreateActivities.Click();
            Utility.ThreadSleep(3);
            WaitForAJAX();
        }

        public void AddActivities_Click()
        {
            this.AddActivities.WaitForDisplay();
            this.AddActivities.Click();
            Utility.ThreadSleep(3);
            WaitForAJAX();
        }

        public void NameOptions_Click()
        {
            this.NameOptions.WaitForDisplay();
            this.NameOptions.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void FieldType_Click()
        {
            this.FieldType.WaitForDisplay();
            this.FieldType.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void AgendaType_Select(DataCollection.FormData.CustomFieldType type)
        {
            ButtonOrLink Type = new ButtonOrLink(
                string.Format("//div[@id='divMoreFormats']//span[text()='{0}']", CustomStringAttribute.GetCustomString(type)), 
                LocateBy.XPath);

            Type.WaitForDisplay();
            Type.Click();
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

        public void PriceOptionsLink_Click()
        {
            this.PriceOptionsLink.WaitForDisplay();
            this.PriceOptionsLink.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
        }

        public void AddEarlyPrice_Click()
        {
            this.AddEarlyPrice.WaitForDisplay();
            this.AddEarlyPrice.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
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

        public void CapacityOptionsLink_Click()
        {
            this.CapacityOptionsLink.WaitForDisplay();
            this.CapacityOptionsLink.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
        }

        public void AddLimitReachedMessage_Click()
        {
            this.AddLimitReachedMessage.WaitForDisplay();
            this.AddLimitReachedMessage.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddTextToConfirmation_Click()
        {
            this.AddTextToConfirmation.WaitForDisplay();
            this.AddTextToConfirmation.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void ShowAllRegTypes_Click()
        {
            this.ShowAllRegTypes.WaitForDisplay();
            this.ShowAllRegTypes.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
        }

        public void ExpandConditionalLogic_Click()
        {
            this.ExpandConditionalLogic.WaitForDisplay();
            this.ExpandConditionalLogic.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
        }

        public void ShowStarting_Type(DateTime date)
        {
            string dateFormat = "{0}/{1}/{2}";
            string dateString = string.Format(dateFormat, date.Month, date.Day, date.Year);
            this.ShowStarting.Type(dateString);
        }

        public void HideStarting_Type(DateTime date)
        {
            string dateFormat = "{0}/{1}/{2}";
            string dateString = string.Format(dateFormat, date.Month, date.Day, date.Year);
            this.HideStarting.Type(dateString);
        }

        public void AgeGreaterThanDate_Type(DateTime date)
        {
            string dateFormat = "{0}/{1}/{2}";
            string dateString = string.Format(dateFormat, date.Month, date.Day, date.Year);
            this.AgeGreaterThanDate.Type(dateString);
        }

        public void AgeLessThanDate_Type(DateTime date)
        {
            string dateFormat = "{0}/{1}/{2}";
            string dateString = string.Format(dateFormat, date.Month, date.Day, date.Year);
            this.AgeLessThanDate.Type(dateString);
        }

        public void AddDetails_Click()
        {
            this.AddDetails.WaitForDisplay();
            this.AddDetails.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddConfirmationAddendum_Click()
        {
            this.AddConfirmationAddendum.WaitForDisplay();
            this.AddConfirmationAddendum.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void SaveItem_Click()
        {
            this.SaveItem.WaitForDisplay();
            this.SaveItem.Click();
            Utility.ThreadSleep(3);
            WaitForAJAX();
        }

        public void SaveAndNew_Click()
        {
            this.SaveAndNew.WaitForDisplay();
            this.SaveAndNew.Click();
            Utility.ThreadSleep(3);
            WaitForAJAX();
        }

        public void Cancel_Click()
        {
            this.Cancel.WaitForDisplay();
            this.Cancel.Click();
            Utility.ThreadSleep(3);
            WaitForAJAX();
        }

        public void AgendaPageHeader_Click()
        {
            this.AgendaPageHeader.WaitForDisplay();
            this.AgendaPageHeader.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AgendaPageFooter_Click()
        {
            this.AgendaPageFooter.WaitForDisplay();
            this.AgendaPageFooter.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddMultipleChoiceItem_Click()
        {
            this.AddMultipleChoiceItem.WaitForDisplay();
            this.AddMultipleChoiceItem.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddCommonlyUsedItem_Click()
        {
            this.AddCommonlyUsedItem.WaitForDisplay();
            this.AddCommonlyUsedItem.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
        #endregion
    }

    public class MultipleChoiceDefine : Frame
    {
        public MultipleChoiceDefine(string name) : base(name) { }

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        public TextBox NameOnForm = new TextBox("ctl00_cphDialog_txtDescription", LocateBy.Id);
        public TextBox NameOnReports = new TextBox("ctl00_cphDialog_txtFieldName", LocateBy.Id);
        public TextBox Price = new TextBox("ctl00_cphDialog_rntAmount_text", LocateBy.Id);
        public TextBox Limit = new TextBox("ctl00_cphDialog_rntMaxQuantity_text", LocateBy.Id);
        public TextBox GroupLimit = new TextBox("ctl00_cphDialog_rntPerGroupLimit_text", LocateBy.Id);
        public CheckBox Visible = new CheckBox("ctl00_cphDialog_chkActive", LocateBy.Id);

        public void SaveAndNew_Click()
        {
            popupFrameHelper.SaveAndNew_Click();
        }

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            popupFrameHelper.Cancel_Click();
            SwitchToMain();
        }
    }

    public class CommonlyUsedItemsDefine : Frame
    {
        public CommonlyUsedItemsDefine(string name) : base(name) { }

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        public void CommonlyUsedItem_Click(FormData.CommonlyUsedMultipleChoice choiceItem)
        {
            ButtonOrLink item = new ButtonOrLink(string.Format("//div[@id='divPredefinedTypes']/div[text()='{0}']",
                CustomStringAttribute.GetCustomString(choiceItem)), LocateBy.XPath);

            item.WaitForDisplay();
            item.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            popupFrameHelper.Cancel_Click();
            SwitchToMain();
        }
    }

    public class AgendaRow
    {
        public ButtonOrLink Agenda;
        public ButtonOrLink Delete;

        public AgendaRow(DataCollection.AgendaItem agendaItem)
        {
            this.Agenda = new ButtonOrLink(string.Format("//*[@class='r1 colwidth1'][@id='listGridTD{0}2']", agendaItem.Id), LocateBy.XPath);
            this.Delete = new ButtonOrLink(string.Format("//*[@class='r1 colwidth4'][@id='listGridTD{0}5']//img[@title='Delete']", agendaItem.Id), LocateBy.XPath);
        }

        public void Agenda_Click()
        {
            this.Agenda.WaitForDisplay();
            this.Agenda.Click();
            Utility.ThreadSleep(2);
        }

        public void Delete_Click()
        {
            this.Delete.WaitForDisplay();
            this.Delete.Click();
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.GetConfirmation();
            Utility.ThreadSleep(3);
            UIUtilityProvider.UIHelper.SelectTopWindow();
        }
    }

    public class CodeRow
    {
        public ButtonOrLink Code;
        public int CodeId;

        public CodeRow(DiscountCode code)
        {
            this.Code = new ButtonOrLink(string.Format("//table[@id='tblCodes']//*[contains(text(),'{0}')]", code.Code), LocateBy.XPath);

            string OnClickAttributeOfCode = this.Code.GetAttribute("onclick");

            string tmp = OnClickAttributeOfCode.Split(new string[] { "'" }, StringSplitOptions.RemoveEmptyEntries)[1];

            this.CodeId = Convert.ToInt32(tmp);
        }
    }
}
