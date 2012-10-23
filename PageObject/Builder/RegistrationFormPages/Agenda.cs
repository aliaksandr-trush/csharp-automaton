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
        public Clickable CreateAgendaItem = new Clickable("Create Agenda Item", LocateBy.LinkText);
        public Clickable AddAgendaItem = new Clickable("Add Agenda Item", LocateBy.LinkText);
        public Clickable CreateActivities = new Clickable("Create Activities", LocateBy.LinkText);
        public Clickable AddActivities = new Clickable("Add Activities", LocateBy.LinkText);
        public Input NameOnForm = new Input("ctl00_cph_ucCF_mipNam_elDesc_TextArea", LocateBy.Id);
        public Clickable NameOptions = new Clickable("ctl00_cph_ucCF_mipNam_bccMoreInfoNaming_optionsLink", LocateBy.Id);
        public Input NameOnReceipt = new Input("ctl00_cph_ucCF_mipNam_ip1_elRptDesc_TextArea", LocateBy.Id);
        public Input NameOnReports = new Input("ctl00_cph_ucCF_mipNam_ip1_txtFieldName", LocateBy.Id);
        public Input NameOnBadge = new Input("ctl00_cph_ucCF_mipNam_ip1_txtBadgeCaption", LocateBy.Id);
        public Clickable FieldType = new Clickable("ctl00_cph_ucCF_selectedFieldTypeToggleImageSpan", LocateBy.Id);
        public Clickable SaveItem = new Clickable("Save Item", LocateBy.LinkText);
        public Clickable SaveAndNew = new Clickable("Save & New", LocateBy.LinkText);
        public Clickable Cancel = new Clickable("Cancel", LocateBy.LinkText);
        public Clickable AgendaPageHeader = new Clickable("//*[text()='Add Agenda Page Header']", LocateBy.XPath);
        public HtmlEditor AgendaPageHeaderEditor = new HtmlEditor("dialog");
        public Clickable AgendaPageFooter = new Clickable("//*[text()='Add Agenda Page Footer']", LocateBy.XPath);
        public HtmlEditor AgendaPageFooterEditor = new HtmlEditor("dialog");
        public Input StartDate = new Input("ctl00_cph_ucCF_dtpSD_dateInput_text", LocateBy.Id);
        public Input StartTime = new Input("ctl00_cph_ucCF_dtpST_dateInput_text", LocateBy.Id);
        public Input EndDate = new Input("ctl00_cph_ucCF_dtpED_dateInput_text", LocateBy.Id);
        public Input EndTime = new Input("ctl00_cph_ucCF_dtpET_dateInput_text", LocateBy.Id);
        public Input Location = new Input("ctl00_cph_ucCF_txtRoom", LocateBy.Id);
        public Input StandardPrice = new Input("ctl00_cph_ucCF_mipPrc_rntAmount_text", LocateBy.Id);
        public Clickable PriceOptionsLink = new Clickable("ctl00_cph_ucCF_mipPrc_MoreInfoButtonPricing1_optionsLink", LocateBy.Id);
        public Clickable AddEarlyPrice = new Clickable("//*[@id='ctl00_cph_ucCF_mipPrc_ip3_trEarlyPriceLink']/td/a", LocateBy.XPath);
        public Input EarlyPrice = new Input("ctl00_cph_ucCF_mipPrc_ip3_rntElyAmt_text", LocateBy.Id);
        public RadioButton EarlyPriceDateTime = new RadioButton("ctl00_cph_ucCF_mipPrc_ip3_rbEarlyPriceOptionDate", LocateBy.Id);
        public Input EarlyPriceDate = new Input("ctl00_cph_ucCF_mipPrc_ip3_dtpElyDt_tbDate", LocateBy.Id);
        public Input EarlyPriceTime = new Input("ctl00_cph_ucCF_mipPrc_ip3_tpElyEdT_dateInput_text", LocateBy.Id);
        public RadioButton EarlyPriceRegLimit = new RadioButton("ctl00_cph_ucCF_mipPrc_ip3_rbEarlyPriceOptionQuantity", LocateBy.Id);
        public Input EarlyPriceRegistrations = new Input("ctl00_cph_ucCF_mipPrc_ip3_rntEarlyPriceQuantity_text", LocateBy.Id);
        public Clickable AddLatePrice = new Clickable("//*[@id='ctl00_cph_ucCF_mipPrc_ip3_trLatePriceLink']/td/a", LocateBy.XPath);
        public Input LatePrice = new Input("ctl00_cph_ucCF_mipPrc_ip3_rntLtAmt_text", LocateBy.Id);
        public Input LatePriceDate = new Input("ctl00_cph_ucCF_mipPrc_ip3_dtpLtDt_tbDate", LocateBy.Id);
        public Input LatePriceTime = new Input("ctl00_cph_ucCF_mipPrc_ip3_tpLtSD_dateInput_text", LocateBy.Id);
        public Clickable AddDiscountCode = new Clickable("ctl00_cph_ucCF_mipPrc_ip4_hrefOpenCodeWindow", LocateBy.Id);
        public Clickable AddBulkCodes = new Clickable("ctl00_cph_ucCF_mipPrc_ip4_hrefOpenBulkCodesWindow", LocateBy.Id);
        public CodeDefine CodeDefine = new CodeDefine("dialog");
        public BulkLoadCodesDefine BulkLoadCodesDefine = new BulkLoadCodesDefine("dialog");
        public CheckBox RequireCode = new CheckBox("ctl00_cph_ucCF_mipPrc_ip4_chkPasswordRequired", LocateBy.Id);
        public Clickable AddTaxRate = new Clickable("//div[@id='ctl00_cph_ucCF_mipPrc_ip6_dvTxWarn']/a", LocateBy.XPath);
        public TaxRateDefine TaxRateDefine = new TaxRateDefine("dialog");
        public CheckBox ApplyTaxOne = new CheckBox("ctl00_cph_ucCF_mipPrc_ip6_chkListTaxRates_0", LocateBy.Id);
        public CheckBox ApplyTaxTwo = new CheckBox("ctl00_cph_ucCF_mipPrc_ip6_chkListTaxRates_1", LocateBy.Id);
        public Input Capacity = new Input("ctl00_cph_ucCF_mipCap_rntMaxQuantity_text", LocateBy.Id);
        public Clickable CapacityOptionsLink = new Clickable("ctl00_cph_ucCF_mipCap_MoreInfoButtonCapacity_optionsLink", LocateBy.Id);
        public CheckBox ShowRemainingCapacity = new CheckBox("ctl00_cph_ucCF_mipCap_ip7_cbShowRemainingCapacity", LocateBy.Id);
        public RadioButton HideWhenLimitReached = new RadioButton("ctl00_cph_ucCF_mipCap_ip7_rbHideThisItem", LocateBy.Id);
        public RadioButton ShowMessageWhenLimitReached = new RadioButton("ctl00_cph_ucCF_mipCap_ip7_rbDisplayThisMessage", LocateBy.Id);
        public Clickable AddLimitReachedMessage = new Clickable("ctl00_cph_ucCF_mipCap_ip7_elSoldOutMessage_linkCheckmarkCustomField0SoldOutMessage", LocateBy.Id);
        public HtmlEditor LimitReachedMessageEditor = new HtmlEditor("dialog");
        public RadioButton WaitlistWhenLimitReached = new RadioButton("ctl00_cph_ucCF_mipCap_ip7_rbActivateWaitlist", LocateBy.Id);
        public Clickable AddTextToConfirmation = new Clickable("ctl00_cph_ucCF_mipCap_ip7_elWaitlistConfirmation_linkCheckmarkCustomField0", LocateBy.Id);
        public EmailEditor TextToConfirmationEditor = new EmailEditor("dialog");
        public Clickable ShowAllRegTypes = new Clickable("//*[@id='tblRegTypesLink']//td[@class='fieldCaption']/a", LocateBy.XPath);
        public CheckBox VisibleToAll = new CheckBox("ctl00_cph_ucCF_chkActive", LocateBy.Id);
        public CheckBox RequiredByAll = new CheckBox("ctl00_cph_ucCF_chkRequired", LocateBy.Id);
        public CheckBox AdminOnlyToAll = new CheckBox("ctl00_cph_ucCF_chkAdminOnly", LocateBy.Id);
        public Clickable ExpandConditionalLogic = new Clickable("//div[@id='ctl00_cph_ucCF_CFParentTree']//span[@class='rtPlus']", LocateBy.XPath);
        public CheckBox AddToCalendar = new CheckBox("ctl00_cph_ucCF_chkDisplayCalendar", LocateBy.Id);
        public CheckBox IncludeOnEventWeb = new CheckBox("ctl00_cph_ucCF_chkIncludeOnAgenda", LocateBy.Id);
        public Input ShowStarting = new Input("ctl00_cph_ucCF_dtpShowDate_tbDate", LocateBy.Id);
        public Input HideStarting = new Input("ctl00_cph_ucCF_dtpHideDate_tbDate", LocateBy.Id);
        public MultiChoiceDropdown Gender = new MultiChoiceDropdown("ctl00_cph_ucCF_ddlVisibleForGender", LocateBy.Id);
        public Input AgeGreaterThan = new Input("ctl00_cph_ucCF_rntVisibleIfAgeGreater_text", LocateBy.Id);
        public Input AgeGreaterThanDate = new Input("ctl00_cph_ucCF_dtpVisibleIfAgeGreater_tbDate", LocateBy.Id);
        public Input AgeLessThan = new Input("ctl00_cph_ucCF_rntVisibleIfAgeLess_text", LocateBy.Id);
        public Input AgeLessThanDate = new Input("ctl00_cph_ucCF_dtpVisibleIfAgeLess_tbDate", LocateBy.Id);
        public MultiChoiceDropdown DateFormat = new MultiChoiceDropdown("ctl00_cph_ucCF_ddlDateFormats", LocateBy.Id);
        public Clickable AddDetails = new Clickable("ctl00_cph_ucCF_elSessionInfo_linkCheckmarktext_elSessionInfo", LocateBy.Id);
        public HtmlEditor DetailsEditor = new HtmlEditor("dialog");
        public Input DetailsURL = new Input("ctl00_cph_ucCF_txtSessionLink", LocateBy.Id);
        public Input GroupName = new Input("ctl00_cph_ucCF_txtGroupName", LocateBy.Id);
        public CheckBox ForceGroupToMatch = new CheckBox("ctl00_cph_ucCF_chkEnablePrePopulate", LocateBy.Id);
        public MultiChoiceDropdown InitialStatus = new MultiChoiceDropdown("ctl00_cph_ucCF_ddlDefaultStatusID", LocateBy.Id);
        public Clickable AddConfirmationAddendum = new Clickable("ctl00_cph_ucCF_elNonWaitlistConfirmation_linkCheckmarktext_elNonWaitlistConfirmation", LocateBy.Id);
        public EmailEditor ConfirmationAddendumEditor = new EmailEditor("dialog");
        public Label AgendaItemId = new Label("ctl00_cph_ucCF_currentCustomFieldId", LocateBy.Id);
        public Clickable AddMultipleChoiceItem = new Clickable("Add Multiple Choice Item", LocateBy.LinkText);
        public MultipleChoiceDefine MultipleChoiceDefine = new MultipleChoiceDefine("dialog");
        public Clickable AddCommonlyUsedItem = new Clickable("Add Commonly-Used Items", LocateBy.LinkText);
        public CommonlyUsedItemsDefine CommonlyUsedItemsDefine = new CommonlyUsedItemsDefine("dialog");
        public Input CharacterLimit = new Input("ctl00_cph_ucCF_rntLn_text", LocateBy.Id);
        public Input ParagraphLimit = new Input("ctl00_cph_ucCF_rntMultipleLineLn_text", LocateBy.Id);
        public Label AgendaChoiceItemCount = new Label("//*[@id='ctl00_cph_ucCF_grdLI_tblGrid']/tbody/tr[@class='dragTR']", LocateBy.XPath);
        public Input MinAmount = new Input("ctl00_cph_ucCF_mipPrc_rntMinVarAmount_text", LocateBy.Id);
        public Input MaxAmount = new Input("ctl00_cph_ucCF_mipPrc_rntMaxVarAmount_text", LocateBy.Id);
        public Input CopyAgendaAmount = new Input("//input[@class='rwDialogInput'][@value='1']", LocateBy.XPath);
        public Clickable OK = new Clickable("//span[@class='rwInnerSpan'][text()='OK']", LocateBy.XPath);
        public Clickable CancelCopy = new Clickable("//span[@class='rwInnerSpan'][text()='Cancel']", LocateBy.XPath);
        public CheckBox DoNotAllowOverlapping = new CheckBox("ctl00_cph_chkEnableScheduleConflictChecking", LocateBy.Id);
        public CheckBox IsShoppingCart = new CheckBox("ctl00_cph_chkEventsIsCart", LocateBy.Id);
        #endregion

        #region Generate Some Elements
        public CheckBox VisibleToRegType(RegType regType)
        {
            return new CheckBox(string.Format("//td[text()='{0}']/following-sibling::td/input[contains(@id,'Active')]", regType.Name), LocateBy.XPath);
        }

        public CheckBox RequiredByRegType(RegType regType)
        {
            return new CheckBox(string.Format("//td[text()='{0}']/following-sibling::td/input[contains(@id,'Required')]", regType.Name), LocateBy.XPath);
        }

        public CheckBox AdminOnlyToRegType(RegType regType)
        {
            return new CheckBox(string.Format("//td[text()='{0}']/following-sibling::td/input[contains(@id,'AdminOnly')]", regType.Name), LocateBy.XPath);
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

        public void AgendaType_Select(DataCollection.EventData_Common.CustomFieldType type)
        {
            Clickable Type = new Clickable(
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

        public void OK_Click()
        {
            this.OK.WaitForDisplay();
            this.OK.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
        #endregion
    }

    public class MultipleChoiceDefine : Frame
    {
        public MultipleChoiceDefine(string name) : base(name) { }

        public Input NameOnForm = new Input("ctl00_cphDialog_txtDescription", LocateBy.Id);
        public Input NameOnReports = new Input("ctl00_cphDialog_txtFieldName", LocateBy.Id);
        public Input Price = new Input("ctl00_cphDialog_rntAmount_text", LocateBy.Id);
        public Input Limit = new Input("ctl00_cphDialog_rntMaxQuantity_text", LocateBy.Id);
        public Input GroupLimit = new Input("ctl00_cphDialog_rntPerGroupLimit_text", LocateBy.Id);
        public CheckBox Visible = new CheckBox("ctl00_cphDialog_chkActive", LocateBy.Id);

        public void SaveAndNew_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndNew_Click();
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
    }

    public class CommonlyUsedItemsDefine : Frame
    {
        public CommonlyUsedItemsDefine(string name) : base(name) { }

        public void CommonlyUsedItem_Click(DataCollection.EventData_Common.CommonlyUsedMultipleChoice choiceItem)
        {
            Clickable item = new Clickable(string.Format("//div[@id='divPredefinedTypes']/div[text()='{0}']",
                CustomStringAttribute.GetCustomString(choiceItem)), LocateBy.XPath);

            item.WaitForDisplay();
            item.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
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
    }

    public class AgendaRow
    {
        public Clickable Agenda;
        public Clickable Delete;
        public Clickable Copy;

        public AgendaRow(DataCollection.AgendaItem agendaItem)
        {
            Label agendaNameTd = new Label(string.Format("//span[@class='bold'][text()='{0}']/..", agendaItem.NameOnForm), LocateBy.XPath);
            string agendaNameOnclickAttriText = agendaNameTd.GetAttribute("onclick");
            int agendaItemId = Convert.ToInt32(agendaNameOnclickAttriText.Split(new string[] { "'" }, StringSplitOptions.RemoveEmptyEntries)[1]);

            this.Agenda = new Clickable(string.Format("//*[@id='listGridTD{0}2']", agendaItemId), LocateBy.XPath);
            this.Delete = new Clickable(string.Format("//*[@id='listGridTD{0}5']//img[@title='Copy']/../../following-sibling::*//img", agendaItemId), LocateBy.XPath);
            this.Copy = new Clickable(string.Format("//*[@id='listGridTD{0}5']//img[@title='Copy']", agendaItemId), LocateBy.XPath);
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
            UIUtil.DefaultProvider.GetConfirmation();
            Utility.ThreadSleep(3);
            UIUtil.DefaultProvider.SelectTopWindow();
        }

        public void Copy_Click()
        {
            this.Copy.WaitForDisplay();
            this.Copy.Click();
            Utility.ThreadSleep(1);
        }
    }

    public class CodeRow
    {
        public Clickable Code;
        public int CodeId;

        public CodeRow(CustomFieldCode code)
        {
            this.Code = new Clickable(string.Format("//table[@id='tblCodes']//*[contains(text(),'{0}')]", code.CodeString), LocateBy.XPath);

            string OnClickAttributeOfCode = this.Code.GetAttribute("onclick");

            string tmp = OnClickAttributeOfCode.Split(new string[] { "'" }, StringSplitOptions.RemoveEmptyEntries)[1];

            this.CodeId = Convert.ToInt32(tmp);
        }
    }
}
