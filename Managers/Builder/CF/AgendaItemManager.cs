namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class AgendaItemManager : CFManagerBase
    {
        public const string AgendaItemButtonLocatorFormat = "//div[@id='ctl00_cph_ucCF_pnlButtons']//span[text()='{0}']";
        public const string OverlappingAgendaItemsLocator = "ctl00_cph_chkEnableScheduleConflictChecking";
        private const string CapacityOptionDIVLocator = "ctl00_cph_ucCF_mipCap_ctl00";
        public const string LimitOptionsLocator = "ctl00_cph_ucCF_mipCap_MoreInfoButtonCapacity_optionsLink";
        public const string ActivateWaitlist = "ctl00_cph_ucCF_mipCap_ip7_rbActivateWaitlist";
        public const string PrePopAgendItemLocator = "ctl00_cph_ucCF_chkEnablePrePopulate";
        public const string AllGroupEditingLocator = "ctl00_cph_ucCF_chkEnableEditing";
        new public readonly string SpacesAvailableLocator;

        public enum AgendaItemType
        {
            [StringValue("1-Line Text")]
            [AgendaItemTypeInNetTiersEntityAttribute(2)]
            OneLineText = 2,

            [StringValue("Check Box")]
            [AgendaItemTypeInNetTiersEntityAttribute(3)]
            CheckBox = 3,

            [StringValue("Paragraph")]
            [AgendaItemTypeInNetTiersEntityAttribute(4)]
            Paragraph = 4,

            [StringValue("Date")]
            [AgendaItemTypeInNetTiersEntityAttribute(5)]
            Date = 5,

            [StringValue("Time")]
            [AgendaItemTypeInNetTiersEntityAttribute(6)]
            Time = 6,

            [StringValue("Always Selected")]
            [AgendaItemTypeInNetTiersEntityAttribute(7)]
            AlwaysSelected = 7,

            [StringValue("Section Header")]
            [AgendaItemTypeInNetTiersEntityAttribute(8)]
            SectionHeader = 8,

            [StringValue("Drop Down")]
            [AgendaItemTypeInNetTiersEntityAttribute(9)]
            Dropdown = 9,

            [StringValue("Radio Button")]
            [AgendaItemTypeInNetTiersEntityAttribute(10)]
            RadioButton = 10,

            [StringValue("Continue Button")]
            [AgendaItemTypeInNetTiersEntityAttribute(11)]
            ContinueButton = 11,

            [StringValue("Contribution")]
            [AgendaItemTypeInNetTiersEntityAttribute(12)]
            Contribution = 12,

            [StringValue("Number")]
            [AgendaItemTypeInNetTiersEntityAttribute(13)]
            Number = 13,

            [StringValue("File Upload")]
            [AgendaItemTypeInNetTiersEntityAttribute(14)]
            FileUpload = 14
        }

        public class AgendaItemTypeInNetTiersEntityAttribute : Attribute
        {
            public int typeInNetTiersEntity
            {
                get;
                set;
            }

            public AgendaItemTypeInNetTiersEntityAttribute(int customFieldTypeId)
            {
                this.typeInNetTiersEntity = customFieldTypeId;
            }

            public static int GetTypeInNetTiersEntity(AgendaItemType category)
            {
                Type type = category.GetType();
                FieldInfo fi = type.GetField(category.ToString());
                AgendaItemTypeInNetTiersEntityAttribute[] attrs = fi.GetCustomAttributes(typeof(AgendaItemTypeInNetTiersEntityAttribute), false) as AgendaItemTypeInNetTiersEntityAttribute[];
                return attrs[0].typeInNetTiersEntity;
            }
        }

        #region Properties

        #region Locators
        protected override string NameOnFormTxtboxId
        {
            get
            {
                return "ctl00_cph_ucCF_mipNam_elDesc_TextArea";
            }
        }

        protected override string TypeLocator_Id
        {
            get
            {
                return "ctl00_cph_ucCF_selectedFieldTypeToggleImageSpan";
            }
        }

        protected override string AllRegTypesVisibilityDIVLocator
        {
            get
            {
                return "ctl00_cph_ucCF_phCFAllRegTypeGrid";
            }
        }

        protected override string RegTypeRowLocatorPrefix
        {
            get
            {
                return "ctl00_cph_ucCF_CustomFieldRegType";
            }
        }

        protected override string VisibleToAllLocator
        {
            get
            {
                return "ctl00_cph_ucCF_chkActive";
            }
        }

        protected override string RequiredByAllLocator
        {
            get
            {
                return "ctl00_cph_ucCF_chkRequired";
            }
        }

        protected override string AdminOnlyToAllLocator
        {
            get
            {
                return "ctl00_cph_ucCF_chkAdminOnly";
            }
        }

        protected override string VisibleToRegTypePrefix
        {
            get
            {
                return "ctl00_cph_ucCF_chkCFRegTypeActive";
            }
        }

        protected override string RequiredByRegTypePrefix
        {
            get
            {
                return "ctl00_cph_ucCF_chkCFRegTypeRequired";
            }
        }

        protected override string AdminOnlyToRegTypePrefix
        {
            get
            {
                return "ctl00_cph_ucCF_chkCFRegTypeAdminOnly";
            }
        }

        protected override string ConditionalLogicDIVLocator
        {
            get
            {
                return "ctl00_cph_ucCF_CFParentTree";
            }
        }

        protected override string FieldPositionLocator
        {
            get
            {
                return "ctl00_cph_ucCF_ddlPosition";
            }
        }

        protected override string OneLineLengthLocator
        {
            get
            {
                return "ctl00_cph_ucCF_rntLn";
            }
        }

        protected override string ParagraphCharacterLimitLocator
        {
            get
            {
                return "ctl00_cph_ucCF_rntMultipleLineLn";
            }
        }

        protected override string GroupNameLocator
        {
            get
            {
                return "ctl00_cph_ucCF_txtGroupName";
            }
        }

        protected override string ShowDateLocator
        {
            get
            {
                return "ctl00_cph_ucCF_dtpShowDate_tbDate";
            }
        }

        protected override string HideDateLocator
        {
            get
            {
                return "ctl00_cph_ucCF_dtpHideDate_tbDate";
            }
        }
        #endregion

        public FeeManager FeeMgr { get; set; }

        public AgendaMultiChoiceItemManager MultiChoiceItemMgr { get; set; }

        public AgendaPredefinedMultiChoiceItemManager PredefinedMultiChoiceItemMgr { get; set; }
        #endregion

        public AgendaItemManager() 
        {
            this.FeeMgr = new FeeManager(FormDetailManager.Page.Agenda);
            this.MultiChoiceItemMgr = new AgendaMultiChoiceItemManager();
            this.PredefinedMultiChoiceItemMgr = new AgendaPredefinedMultiChoiceItemManager();

            this.SpacesAvailableLocator = "ctl00_cph_ucCF_mipCap_rntMaxQuantity";
        }

        public void ClickSaveItem()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("Save Item", LocateBy.LinkText);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void ClickSaveAndNew()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("Save & New", LocateBy.LinkText);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void ClickCancel()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("Cancel", LocateBy.LinkText);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void ClickAddPredefinedMultiChoiceItem()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(AddCommonlyUsedItemsLinkLocator, LocateBy.LinkText);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(this.PredefinedMultiChoiceItemMgr.FrameID);
        }

        public void AddPredefinedMultiChoiceItem(PredifinedMultiChoiceItemManagerBase.PredefinedItemType type)
        {
            this.ClickAddPredefinedMultiChoiceItem();
            this.PredefinedMultiChoiceItemMgr.ClickPredefinedItem(type);
            this.PredefinedMultiChoiceItemMgr.SaveAndClose();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void ClickAddMultiChoiceItem()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(AddMultiChoiceItemLinkLocator, LocateBy.LinkText);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(this.MultiChoiceItemMgr.FrameID);
        }

        public void AddMultiChoiceItems(List<Custom_Field_List_Item> lst)
        {
            ClickAddMultiChoiceItem();
            foreach (Custom_Field_List_Item cfli in lst)
            {
                if (cfli.Description == cfli.Fieldname || string.IsNullOrEmpty(cfli.Fieldname))
                    MultiChoiceItemMgr.SetName(cfli.Description);//description
                else
                {
                    MultiChoiceItemMgr.SetNameOnForm(cfli.Description);
                    MultiChoiceItemMgr.SetNameOnReport(cfli.Fieldname);
                }
                if(cfli.Amount.HasValue)
                    MultiChoiceItemMgr.SetPrice(double.Parse(cfli.Amount.ToString()));
                if (cfli.MaxQuantity.HasValue)
                    MultiChoiceItemMgr.SetLimit(cfli.MaxQuantity);
                if (cfli.PerGroupLimit.HasValue)
                    MultiChoiceItemMgr.SetLimitPerGroup(cfli.PerGroupLimit);
                if (cfli.Active.HasValue)
                    MultiChoiceItemMgr.SetVisibility(cfli.Active.Value);

                MultiChoiceItemMgr.SaveAndNew();
            }
            MultiChoiceItemMgr.Cancel();
        }

        public void AddMultiChoiceItem(string name, double? price)
        {
            this.ClickAddMultiChoiceItem();
            this.MultiChoiceItemMgr.SetMultiChoiceItem(name, price);
            this.MultiChoiceItemMgr.SaveAndClose();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetType(AgendaItemType type)
        {
            this.SelectType(StringEnum.GetStringValue(type));
        }

        public void SetTypeWithDefaults(AgendaItemType type)
        {
            this.SetType(type);

            switch (type)
            {
                case AgendaItemType.RadioButton:
                    this.AddPredefinedMultiChoiceItem(AgendaPredefinedMultiChoiceItemManager.PredefinedItemType.YesOrNo);
                    break;

                case AgendaItemType.Dropdown:
                    this.AddPredefinedMultiChoiceItem(AgendaPredefinedMultiChoiceItemManager.PredefinedItemType.Agreement);
                    break;

                case AgendaItemType.Number:
                    this.SetOneLineLength(DefaultOneLineNumericLength);
                    break;

                case AgendaItemType.OneLineText:
                    this.SetOneLineLength(DefaultOneLineTextLength);
                    break;

                case AgendaItemType.Paragraph:
                    this.SetParagraphCharacterLimit(DefaultParagraphCharacterLimit);
                    break;

                case AgendaItemType.Contribution:

                    this.SetVariableMinMax(
                        (int)Math.Round(new Random((int)DateTime.Now.Ticks).NextDouble() * 50 + 1, 2), 
                        (int)Math.Round(new Random((int)DateTime.Now.Ticks).NextDouble() * 1000 + 50 + 1, 2));

                    break;

                default:
                    break;
            }
        }

        #region Methods only used in Agenda
        public void SetVariableMinMax(int minValue, int maxValue)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById("ctl00_cph_ucCF_mipPrc_rntMinVarAmount", minValue);
            WebDriverUtility.DefaultProvider.TypeRADNumericById("ctl00_cph_ucCF_mipPrc_rntMaxVarAmount", maxValue);
        }

        public void SetStartEndDateTime(DateTime startDate, DateTime endDate)
        {
            WebDriverUtility.DefaultProvider.WaitForElementPresent("ctl00_cph_ucCF_dtpSD_dateInput_text", LocateBy.Id);
            WebDriverUtility.DefaultProvider.SetDatesTimesForDatePickerWrapper(startDate, endDate);
        }

        public void SetLocation(string location)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cph_ucCF_txtRoom", location, LocateBy.Id);
        }

        public void OpenAgendaInListByOrder(int order)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//tr[@id='sdgr_" + order.ToString() + "']/td[1]", LocateBy.XPath);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void OpenAgendaByName(string name)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(string.Format("//span[text()='{0}']", name), LocateBy.XPath);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        /// <summary>
        /// Get agenda item ID in the list on the left, based on its order in the list.
        /// </summary>
        /// <param name="orderInList">The order in the list, starts from zero!</param>
        /// <returns></returns>
        public int GetAgendaItemID(int orderInList)
        {
            string id = WebDriverUtility.DefaultProvider.GetAttribute("//tr[@id='sdgr_" + orderInList.ToString() + "']/td[1]", "onclick", LocateBy.XPath).Split(new char[] { '\'' }, 3)[1];
            return Convert.ToInt32(id);
        }

        public int GetAgendaItemID(string name)
        {
            string id = WebDriverUtility.DefaultProvider.GetAttribute("//table[@id='listGrid']/tbody/tr/td/span[text()='" + name + "']/..", "onclick", LocateBy.XPath).Split(new char[] { '\'' }, 3)[1];
            return Convert.ToInt32(id);
        }

        public int GetLastAgendaItemOrder()
        {
            int order = -1;
            order = order + WebDriverUtility.DefaultProvider.GetXPathCountByXPath("//table[@id = 'listGrid']/tbody/tr");
            return order;
        }

        public void ClickDeleteRadioButtonAllItems()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("aDeleteAll", LocateBy.Id);
            WebDriverUtility.DefaultProvider.VerifyConfirmation("Are you sure you want to delete all list items?\r\n( Any items already associated with registrations WILL NOT be deleted.)");
        }

        public void ClickOkOnDeleteRadioButtonAllMultipleChoiceItemsConfirmation()
        {
            WebDriverUtility.DefaultProvider.GetConfirmation();
        }

        public void DoNotAllowSelectionOverlappingAgendaItems(bool allow)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox(OverlappingAgendaItemsLocator, allow, LocateBy.Id);
        }

        public void SetLimit(int limit)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById(SpacesAvailableLocator, limit);
        }

        public void UpdateLimit(int limit)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById(SpacesAvailableLocator, limit);
        }

        public void SetWaitlist(int limit)
        {
            this.SetLimit(limit);
            this.ExpandCapacityOptions();
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(ActivateWaitlist, LocateBy.Id);
            Utility.ThreadSleep(0.5);
        }

        private void ClickCapacityOptions()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(LimitOptionsLocator, LocateBy.Id);
            Utility.ThreadSleep(0.5);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void ExpandCapacityOptions()
        {
            if (WebDriverUtility.DefaultProvider.IsElementHidden(CapacityOptionDIVLocator, LocateBy.Id))
            {
                this.ClickCapacityOptions();
            }
        }

        public void CollapseCapacityOptions()
        {
            if (!WebDriverUtility.DefaultProvider.IsElementHidden(CapacityOptionDIVLocator, LocateBy.Id))
            {
                this.ClickCapacityOptions();
            }
        }

        public void PrePopulateAgendaGroupSelections(bool check)
        {
            WebDriverUtility.DefaultProvider.ExpandAdvanced();
            WebDriverUtility.DefaultProvider.SetCheckbox(PrePopAgendItemLocator, check, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void AllowAgendaGroupSelectionEditing(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox(AllGroupEditingLocator, check, LocateBy.Id);
        }
        #endregion
    }
}
