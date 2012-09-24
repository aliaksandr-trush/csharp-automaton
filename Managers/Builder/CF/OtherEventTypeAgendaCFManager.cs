namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    
    public class OtherEventTypeAgendaAndCFManager : ManagerBase
    {
        public const string FrameId = "dialog";
        public const string FrameId2 = "dialog2";
        protected const int DefaultOneLineNumericLength = 5;
        protected const int DefaultOneLineTextLength = 50;

        public FeeManager FeeMgr { get; set; }
        public AgendaMultiChoiceItemManager MultiChoiceItemMgr { get; set; }

        public OtherEventTypeAgendaAndCFManager() 
        {
            this.FeeMgr = new FeeManager(FormDetailManager.Page.Agenda);
            this.MultiChoiceItemMgr = new AgendaMultiChoiceItemManager();
        }

        public enum FieldType
        {
            [StringValue("1-Line Text")]
            OneLineText = 2,

            [StringValue("Check Box")]
            CheckBox = 3,

            [StringValue("Multi-Line Text")]
            Paragraph = 4,

            [StringValue("Date")]
            Date = 5,

            [StringValue("Time")]
            Time = 6,

            [StringValue("Always Selected")]
            AlwaysSelected = 7,

            [StringValue("Section Header")]
            SectionHeader = 8,

            [StringValue("Multiple Choice (Dropdown List)")]
            Dropdown = 9,

            [StringValue("Multiple Choice (Radio Buttons)")]
            RadioButton = 10,

            [StringValue("Continue Button")]
            ContinueButton = 11,

            [StringValue("Variable Amount")]
            VariableAmount = 12,

            [StringValue("1-Line Numeric Only")]
            Number = 13,

            [StringValue("File Upload")]
            FileUpload = 14
        }

        public enum FeeType
        {
            [StringValue("One-Time Fee")]
            OneTimeFee,

            [StringValue("Recurring Membership Fee")]
            ReccuringMembershipFee
        }

        public enum PredefinedOptionsOld
        {
            YesOrNo,
            Agreement
        }

        public struct Locator
        {
            public const string AddOptionLinkLocatorPrefix = "ctl00_cph_grdAgendaItems_";
            public const string TypeLocator = "ctl00_cphDialog_ddlTypeId";
            public const string DescriptionLocator = "ctl00_cphDialog_elDescription_TextArea";
            public const string RegularPriceLocator = "ctl00_cphDialog_rntAmount";
            public const string MinAmountLocator = "ctl00_cphDialog_rntMinVarAmount";
            public const string MaxAmountLocator = "ctl00_cphDialog_rntMaxVarAmount";
            public const string LineTextLengthLocator = "ctl00_cphDialog_rntLength";
            public const string FeeTypeLocator = "ctl00_cphDialog_rblMembershipFeeType_";
            public const string AdvancedPriceOptionsDIV = "bsDiscountOptions_ADV";
            public const string AdvancedPriceOptions = "//span[text()='Advanced Price Options']";
            public const string DiscountCodeLocator = "ctl00_cphDialog_txtPassword";
            public const string DiscountDescriptionLocator = "ctl00_cphDialog_txtDiscountCodeTitle";
            public const string DiscountRequiredLocator = "ctl00_cphDialog_chkPasswordRequired";
            public const string ApplyOnceLocator = "ctl00_cphDialog_chkDiscountOnce";
            public const string AddMultipleChoiceItemLocator = "ctl00_cphDialog_lbnNewListItem";
            public const string FeeTypeTableLocator = "ctl00_cphDialog_rblMembershipFeeType";
            public const string TitleLocator = "ctl00_cphDialog_elReportDescription_TextArea";
            public const string NameOnReportsLocator = "ctl00_cphDialog_txtFieldName";
        }

        public void SelectType(FieldType type)
        {
            WebDriverUtility.DefaultProvider.SelectWithText(Locator.TypeLocator, StringEnum.GetStringValue(type), LocateBy.Id);
        }

        public void SetQuestionDescription(string name)
        {
            WebDriverUtility.DefaultProvider.Type(Locator.DescriptionLocator, name, LocateBy.Id);
        }

        public void SetTitle(string title)
        {
            WebDriverUtility.DefaultProvider.Type(Locator.TitleLocator, title, LocateBy.Id);
        }

        public void SetNameOnReports(string nameOnReports)
        {
            WebDriverUtility.DefaultProvider.Type(Locator.NameOnReportsLocator, nameOnReports, LocateBy.Id);
        }

        public void SetLineTextItemLength(int length)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById(Locator.LineTextLengthLocator, length);
        }

        public void SetFieldVisibility(bool isVisible)
        {
            string locator = "ctl00_cphDialog_chkActive";
            WebDriverUtility.DefaultProvider.SetCheckbox(locator, isVisible, LocateBy.Id);
        }

        public void SaveAndCloseAgendaOrCFItem()
        {
            WebDriverUtility.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetRegularPrice(double price)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById(Locator.RegularPriceLocator, price);
        }

        public void SetVariableAmount(double min, double max)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById(Locator.MinAmountLocator, min);
            WebDriverUtility.DefaultProvider.TypeRADNumericById(Locator.MaxAmountLocator, max);
        }

        public void SetMembershipFeeType(FeeType feeType)
        {
            switch (feeType)
            {
                case FeeType.OneTimeFee:
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(Locator.FeeTypeLocator + "0", LocateBy.Id);
                    break;
                case FeeType.ReccuringMembershipFee:
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(Locator.FeeTypeLocator + "1", LocateBy.Id);
                    break;
            }
        }

        public void ExpandAdvancedPriceOption()
        {
            if (WebDriverUtility.DefaultProvider.IsElementHidden(Locator.AdvancedPriceOptionsDIV, LocateBy.Id))
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(Locator.AdvancedPriceOptions, LocateBy.XPath);
            }
        }

        public void AddMembershipDiscountCodes(string description, string formattedCodes, bool requied)
        {
            WebDriverUtility.DefaultProvider.Type(Locator.DiscountDescriptionLocator, description, LocateBy.Id);
            WebDriverUtility.DefaultProvider.Type(Locator.DiscountCodeLocator, formattedCodes, LocateBy.Id);
            WebDriverUtility.DefaultProvider.SetCheckbox(Locator.DiscountRequiredLocator, requied, LocateBy.Id);
        }

        public void ApplyMembershipDiscountCodeOnce(bool applyOnce)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox(Locator.ApplyOnceLocator, applyOnce, LocateBy.Id);
        }

        public void SetTypeWithDefaultsOld(FieldType type)
        {
            this.SelectType(type);

            switch (type)
            {
                case FieldType.RadioButton:
                    this.AddPredefinedMultipleChoiceItemsOld(PredefinedOptionsOld.YesOrNo);
                    break;

                case FieldType.Dropdown:
                    this.AddPredefinedMultipleChoiceItemsOld(PredefinedOptionsOld.Agreement);
                    break;

                case FieldType.Number:
                    this.SetLineTextItemLength(DefaultOneLineNumericLength);
                    break;

                case FieldType.OneLineText:
                    this.SetLineTextItemLength(DefaultOneLineTextLength);
                    break;

                case FieldType.VariableAmount:
                    this.SetVariableAmount(
                        (int)Math.Round(new Random((int)DateTime.Now.Ticks).NextDouble() * 50 + 1, 2),
                        (int)Math.Round(new Random((int)DateTime.Now.Ticks).NextDouble() * 1000 + 50 + 1, 2));
                    break;

                default:
                    break;
            }
        }

        public void ClickAddMultipleChoiceItemsOld()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(Locator.AddMultipleChoiceItemLocator, LocateBy.Id);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(FrameId2);
        }

        public void AddPredefinedMultipleChoiceItemsOld(PredefinedOptionsOld choice)
        {
            switch (choice)
            {
                case PredefinedOptionsOld.Agreement:
                    this.ClickAddMultipleChoiceItemsOld();
                    MultiChoiceItemMgr.SetMultiChoiceItem(StringEnum.GetStringValue(PredifinedMultiChoiceItemManagerBase.Agreement.Agree), null);
                    SaveAndNewMCItem();
                    MultiChoiceItemMgr.SetMultiChoiceItem(StringEnum.GetStringValue(PredifinedMultiChoiceItemManagerBase.Agreement.Disagree), null);
                    SaveAndNewMCItem();
                    MultiChoiceItemMgr.SetMultiChoiceItem(StringEnum.GetStringValue(PredifinedMultiChoiceItemManagerBase.Agreement.Neutral), null);
                    SaveAndNewMCItem();
                    MultiChoiceItemMgr.SetMultiChoiceItem(StringEnum.GetStringValue(PredifinedMultiChoiceItemManagerBase.Agreement.NotApplicable), null);
                    SaveAndNewMCItem();
                    MultiChoiceItemMgr.SetMultiChoiceItem(StringEnum.GetStringValue(PredifinedMultiChoiceItemManagerBase.Agreement.StronglyAgree), null);
                    SaveAndNewMCItem();
                    MultiChoiceItemMgr.SetMultiChoiceItem(StringEnum.GetStringValue(PredifinedMultiChoiceItemManagerBase.Agreement.StronglyDisagree), null);
                    SaveAndCloseMCItem();
                    //UIUtilityProvider.UIHelper.SelectPopUpFrame(OldAgendaAndCFManager.FrameId);
                    break;
                case PredefinedOptionsOld.YesOrNo:
                    this.ClickAddMultipleChoiceItemsOld();
                    MultiChoiceItemMgr.SetMultiChoiceItem(StringEnum.GetStringValue(PredifinedMultiChoiceItemManagerBase.YesOrNo.No), null);
                    SaveAndNewMCItem();
                    MultiChoiceItemMgr.SetMultiChoiceItem(StringEnum.GetStringValue(PredifinedMultiChoiceItemManagerBase.YesOrNo.Yes), null);
                    SaveAndCloseMCItem();
                    //UIUtilityProvider.UIHelper.SelectPopUpFrame(OldAgendaAndCFManager.FrameId);
                    break;
            }
        }

        private void SaveAndCloseMCItem()
        {
            WebDriverUtility.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(FrameId);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        private void SaveAndNewMCItem()
        {
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(FrameId2);
            WebDriverUtility.DefaultProvider.ClickSaveAndNew();
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }
    }
}
