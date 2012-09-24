namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Reflection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class CustomFieldManager : CFManagerBase
    {
        public const string FrameID = "dialog";

        public enum CustomFieldType
        {
            [StringValue("1-Line Text")]
            OneLineText = 2,

            [StringValue("Check Box")]
            CheckBox = 3,

            [StringValue("Paragraph")]
            Paragraph = 4,

            [StringValue("Date")]
            Date = 5,

            [StringValue("Time")]
            Time = 6,

            [StringValue("Always Selected")]
            AlwaysSelected = 7,

            [StringValue("Section Header")]
            SectionHeader = 8,

            [StringValue("Drop Down")]
            Dropdown = 9,

            [StringValue("Radio Button")]
            RadioButton = 10,

            [StringValue("Continue Button")]
            ContinueButton = 11,

            [StringValue("Number")]
            Number = 13,

            [StringValue("File Upload")]
            FileUpload = 14
        }

        public enum CustomFieldCategory
        {
            Invalid = 0,
            CustomField = 1,
            Agenda = 2,
            EventFee = 3
        }

        public enum CustomFieldLocation
        {
            Agenda = 0,
            PI = 1,
            LT_Lodging = 2,
            LT_Travel = 3,
            LT_Preferences = 4,
            EventFee = 5
        }

        public enum LimitReachedOption
        {
            HideThisItem,
            ShowThisMessage
        }

        #region Properties

        #region Locators
        protected override string NameOnFormTxtboxId
        {
            get
            {
                return "ctl00_cphDialog_cfCF_mipNam_elDesc_TextArea";
            }
        }

        protected override string TypeLocator_Id
        {
            get
            {
                return "ctl00_cphDialog_cfCF_selectedFieldTypeToggleImageSpan";
            }
        }


        protected override string AllRegTypesVisibilityDIVLocator
        {
            get
            {
                return "ctl00_cphDialog_cfCF_phCFAllRegTypeGrid";
            }
        }

        protected override string ConditionalLogicDIVLocator
        {
            get
            {
                return "ctl00_cphDialog_cfCF_CFParentTree";
            }
        }

        protected override string FieldPositionLocator
        {
            get
            {
                return "ctl00_cphDialog_cfCF_ddlPosition";
            }
        }

        protected override string OneLineLengthLocator
        {
            get
            {
                return "ctl00_cphDialog_cfCF_rntLn";
            }
        }

        protected override string ParagraphCharacterLimitLocator
        {
            get
            {
                return "ctl00_cphDialog_cfCF_rntMultipleLineLn";
            }
        }

        protected override string GroupNameLocator
        {
            get
            {
                return "ctl00_cphDialog_cfCF_txtGroupName";
            }
        }

        protected override string RegTypeRowLocatorPrefix
        {
            get
            {
                return "ctl00_cphDialog_cfCF_CustomFieldRegType";
            }
        }

        protected override string VisibleToAllLocator
        {
            get
            {
                return "ctl00_cphDialog_cfCF_chkActive";
            }
        }

        protected override string RequiredByAllLocator
        {
            get
            {
                return "ctl00_cphDialog_cfCF_chkRequired";
            }
        }

        protected override string AdminOnlyToAllLocator
        {
            get
            {
                return "ctl00_cphDialog_cfCF_chkAdminOnly";
            }
        }

        protected override string VisibleToRegTypePrefix
        {
            get
            {
                return "ctl00_cphDialog_cfCF_chkCFRegTypeActive";
            }
        }

        protected override string RequiredByRegTypePrefix
        {
            get
            {
                return "ctl00_cphDialog_cfCF_chkCFRegTypeRequired";
            }
        }

        protected override string AdminOnlyToRegTypePrefix
        {
            get
            {
                return "ctl00_cphDialog_cfCF_chkCFRegTypeAdminOnly";
            }
        }

        protected override string ShowDateLocator
        {
            get
            {
                return "ctl00_cphDialog_cfCF_dtpShowDate_tbDate";
            }
        }

        protected override string HideDateLocator
        {
            get
            {
                return "ctl00_cphDialog_cfCF_dtpHideDate_tbDate";
            }
        }
        #endregion

        public CFMultiChoiceItemManager MultiChoiceItemMgr { get; set; }

        public CFPredefinedMultiChoiceItemManager PredefinedMultiChoiceItemMgr { get; set; }
        #endregion

        public CustomFieldManager() 
        {
            this.MultiChoiceItemMgr = new CFMultiChoiceItemManager();
            this.PredefinedMultiChoiceItemMgr = new CFPredefinedMultiChoiceItemManager();
        }

        private void SelectThisFrame()
        {
            try
            {
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(FrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndClose()
        {
            SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SaveAndStay()
        {
            SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        public void Cancel()
        {
            SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
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
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(FrameID);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void ClickAddMultiChoiceItem()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(AddMultiChoiceItemLinkLocator, LocateBy.LinkText);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(this.MultiChoiceItemMgr.FrameID);
        }

        public void AddMultiChoiceItem(string name)
        {
            this.ClickAddMultiChoiceItem();
            this.MultiChoiceItemMgr.SetName(name);
            this.MultiChoiceItemMgr.SaveAndClose();
        }

        public void SetType(CustomFieldType type)
        {
            this.SelectType(StringEnum.GetStringValue(type));
        }

        public void SetTypeWithDefaults(CustomFieldType type)
        {
            this.SetType(type);

            switch (type)
            {
                case CustomFieldType.RadioButton:
                    this.AddPredefinedMultiChoiceItem(CFPredefinedMultiChoiceItemManager.PredefinedItemType.YesOrNo);
                    break;

                case CustomFieldType.Dropdown:
                    this.AddPredefinedMultiChoiceItem(CFPredefinedMultiChoiceItemManager.PredefinedItemType.Agreement);
                    break;

                case CustomFieldType.Number:
                    this.SetOneLineLength(DefaultOneLineNumericLength);
                    break;

                case CustomFieldType.OneLineText:
                    this.SetOneLineLength(DefaultOneLineTextLength);
                    break;

                case CustomFieldType.Paragraph:
                    this.SetParagraphCharacterLimit(DefaultParagraphCharacterLimit);
                    break;

                default:
                    break;
            }
        }
        public void PrePopulateGroupSelections(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox(PrePopulateGroupSelectionLocator, check, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void AllowGroupSelectionEditing(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox(AllowGroupSelectionEditingLocator, check, LocateBy.Id); 
        }

        public void ClickOptionsLink()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(OptionsLinkLocator, LocateBy.Id);
        }

        public void SetShowRemainingCapacity(bool show)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox(ShowRemainingCapacityLocator, show, LocateBy.Id);
        }

        public void SetLimitReachOption(LimitReachedOption option)
        {
            this.ClickOptionsLink();

            if (option == LimitReachedOption.HideThisItem)
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(HideItemWhenLimitReachedLocator, LocateBy.Id);
            }

            if (option == LimitReachedOption.ShowThisMessage)
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(ShowMessageWhenLimitReachedLocator, LocateBy.Id);
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(AddLimitReachedMessageLocator, LocateBy.Id);
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog2");
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
                WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                WebDriverUtility.DefaultProvider.SelectIFrame(1);
                WebDriverUtility.DefaultProvider.Type("//textarea", LimitReachedMessage + "<br>", LocateBy.XPath);
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog2");
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_btnSaveClose", LocateBy.Id);
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
            }
        }

        public void SetSpacesAvailable(int spaces)
        {
            WebDriverUtility.DefaultProvider.Type(SpacesAvailableLocator, spaces, LocateBy.Id);
        }
    }
}
