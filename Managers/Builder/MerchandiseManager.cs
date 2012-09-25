namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class MerchandiseManager : ManagerBase
    {
        #region Constants
        public const string MerchItemDetailDialogID = "dialog";
        private const string MerchItemAdvancedDIVLocator = "bsAdvanced_ADV";
        private const string MerchItemAdvancedButton = "//span[text()='Advanced']";
        private const string AddMultiChoiceItemLinkLocator = "ctl00_cphDialog_lbnNewListItem";
        private const string NameOnFormLocator = "ctl00_cphDialog_elDescription_TextArea";
        private const string NameOnReceiptLocator = "ctl00_cphDialog_elReportDescription_TextArea";
        private const string NameOnReportsLocator = "ctl00_cphDialog_feeFieldName";
        #endregion

        public enum MerchandiseType
        {
            Fixed = 0,
            Variable = 1,
            Header = 9,
        }

        #region Constructor
        public MerchandiseManager()
        {
            this.TaxMgr = new FeeTaxManager(FormDetailManager.Page.Merchandise);
            this.DiscountCodeMgr = new DiscountCodeManager(FormDetailManager.Page.Merchandise);
            this.MultiChoiceItemMgr = new MerchandiseMultiChoiceItemManager();
        }
        #endregion

        #region Properties
        public FeeTaxManager TaxMgr { get; private set; }
        public DiscountCodeManager DiscountCodeMgr { get; private set; }
        public MerchandiseMultiChoiceItemManager MultiChoiceItemMgr { get; private set; }
        #endregion

        private void SelectThisFrame()
        {
            try
            {
                UIUtil.DefaultProvider.SelectPopUpFrameByName(MerchItemDetailDialogID);
            }
            catch
            {
                // Ignore
            }
        }

        #region Public methods
        public void SaveAndStay()
        {
            this.SelectThisFrame();
            UIUtil.DefaultProvider.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        public void SaveAndClose()
        {
            this.SelectThisFrame();
            UIUtil.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            this.SelectThisFrame();
            UIUtil.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void ExpandAdvanced()
        {
            if (UIUtil.DefaultProvider.IsElementHidden(MerchItemAdvancedDIVLocator, LocateBy.Id))
            {
                this.ClickAdvancedHeader();
            }
        }

        public void CollapseAdvanced()
        {
            if (!UIUtil.DefaultProvider.IsElementHidden(MerchItemAdvancedDIVLocator, LocateBy.Id))
            {
                this.ClickAdvancedHeader();
            }
        }

        private void ClickAdvancedHeader()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(MerchItemAdvancedButton, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetMinAndMaxLimit(int? minimum, int? maximum)
        {
            this.SetMinimumLimit(minimum);
            this.SetMaximumLimit(maximum);
        }

        public void SetMinimumLimit(int? minimum)
        {
            UIUtil.DefaultProvider.TypeRADNumericById("ctl00_cphDialog_feeMinAllowed", minimum);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetMaximumLimit(int? maximum)
        {
            UIUtil.DefaultProvider.TypeRADNumericById("ctl00_cphDialog_feeMaxAllowed", maximum);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetName(string name)
        {
            this.SetNameOnForm(name);
            this.SetNameOnReceipt(name);
            this.SetNameOnReports(name);
        }

        public void SetNameOnForm(string name)
        {
            UIUtil.DefaultProvider.Type(NameOnFormLocator, name, LocateBy.Id);
        }

        public void SetNameOnReceipt(string name)
        {
            UIUtil.DefaultProvider.Type(NameOnReceiptLocator, name, LocateBy.Id);
        }

        public void SetNameOnReports(string name)
        {
            UIUtil.DefaultProvider.Type(NameOnReportsLocator, name, LocateBy.Id);
        }

        public void SetType(MerchandiseType merchType)
        {
            string typeLabel = string.Empty;

            switch (merchType)
            {
                case MerchandiseType.Fixed:
                    typeLabel = "Fixed price";
                    break;
                case MerchandiseType.Variable:
                    typeLabel = "Variable amount";
                    break;
                case MerchandiseType.Header:
                    typeLabel = "Section header";
                    break;
            }

            UIUtil.DefaultProvider.SelectWithText("ctl00_cphDialog_feeAmountTypeId", typeLabel, LocateBy.Id);
        }

        public void SetMerchItemPrice(MerchandiseType type, double? fee, double? minFee, double? maxFee)
        {
            switch (type)
            {
                case MerchandiseType.Fixed:
                    this.SetFixedPrice(fee);
                    break;

                case MerchandiseType.Variable:
                    this.SetVariableMinMax(minFee, maxFee);
                    break;

                default:
                    break;
            }
        }

        public void SetFixedPrice(double? price)
        {
            string feeTxtboxLocator = "ctl00_cphDialog_feeAmount";
            UIUtil.DefaultProvider.TypeRADNumericById(feeTxtboxLocator, Convert.ToString(price));
        }

        public void SetVariableMinMax(double? min, double? max)
        {
            this.SetVariableMinimum(min);
            this.SetVariableMaximum(max);
        }

        public void SetVariableMinimum(double? min)
        {
            string minFeeTxtboxLocator = "ctl00_cphDialog_feeMinVarAmount";
            UIUtil.DefaultProvider.TypeRADNumericById(minFeeTxtboxLocator, Convert.ToString(min));
        }

        public void SetVariableMaximum(double? max)
        {
            string maxFeeTxtboxLocator = "ctl00_cphDialog_feeMaxVarAmount";
            UIUtil.DefaultProvider.TypeRADNumericById(maxFeeTxtboxLocator, Convert.ToString(max));
        }

        public void SetPercentage(double? percent)
        {
            string feePercentageTxtboxLocator = "ctl00_cphDialog_feePct";
            UIUtil.DefaultProvider.TypeRADNumericById(feePercentageTxtboxLocator, Convert.ToString(percent));
        }

        public void ClickAddMerchMultiChoiceItemLink()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(AddMultiChoiceItemLinkLocator, LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectPopUpFrameByName(MerchandiseMultiChoiceItemManager.FrameID);
        }

        public void AddMerchandiseMultipleChoiceItem(string name, int? limit)
        {
            this.ClickAddMerchMultiChoiceItemLink();
            this.MultiChoiceItemMgr.SetName(name);
            if (limit != null)
            {
                this.MultiChoiceItemMgr.SetLimit(limit);
            }
            this.MultiChoiceItemMgr.SaveAndClose();
        }

        public void SetShowDate(DateTime showDate)
        {
            UIUtil.DefaultProvider.SetDateTimeById("ctl00_cphDialog_feeShowDate", showDate);
        }

        public void SetHideDate(DateTime hideDate)
        {
            UIUtil.DefaultProvider.SetDateTimeById("ctl00_cphDialog_feeHideDate", hideDate);
        }

        public void SetShowAndHideDate(DateTime showDate, DateTime hideDate)
        {
            this.SetShowDate(showDate);
            this.SetHideDate(hideDate);
        }

        public void SetMerchVisibility(bool isVisible)
        {
            UIUtil.DefaultProvider.SetCheckbox("ctl00_cphDialog_feeVisible", isVisible, LocateBy.Id);
        }

        #endregion
    }
}
