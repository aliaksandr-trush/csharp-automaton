namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.UIUtility;

    public class FeeTaxManager : TaxManager
    {
        #region Constants
        private const string TaxOptionDialogIDInEventFeeOption = "dialog3";
        private const string TaxOptionLinkLocatorInEventFeeOption = "//div[@id='ctl00_cphDialog_cfCF_mipPrc_ip6_dvTxWarn']/a";
        private const string ApplyTaxRateOneCheckboxLocatorInEventFeeOption = "ctl00_cphDialog_cfCF_mipPrc_ip6_chkListTaxRates_0";
        private const string _applyTaxRateTwoCheckboxLocatorInEventFeeOption = "ctl00_cphDialog_cfCF_mipPrc_ip6_chkListTaxRates_1";

        private const string _taxOptionDialogIDInAgendaFeeOption = "dialog";
        private const string _taxOptionLinkLocatorInAgendaFeeOption = "//div[@id='ctl00_cph_ucCF_mipPrc_ip6_dvTxWarn']/a";
        private const string _applyTaxRateOneCheckboxLocatorInAgendaFeeOption = "ctl00_cph_ucCF_mipPrc_ip6_chkListTaxRates_0";
        private const string _applyTaxRateTwoCheckboxLocatorInAgendaFeeOption = "ctl00_cph_ucCF_mipPrc_ip6_chkListTaxRates_1";

        private const string _taxOptionDialogIDInMerch = "dialog2";
        private const string _taxOptionLinkLocatorInMerch = "//*[@id='ctl00_cphDialog_lbnTaxes']";
        private const string _applyTaxRateOneCheckboxLocatorInMerch = "ctl00_cphDialog_chkListTaxRates_0";
        private const string _applyTaxRateTwoCheckboxLocatorInMerch = "ctl00_cphDialog_chkListTaxRates_1";
        #endregion

        #region Locator variables needed to be initialized
        private string _taxOptionDialogID;
        private string _taxOptionLinkLocator;
        private string _applyTaxRateOneCheckboxLocator;
        private string _applyTaxRateTwoCheckboxLocator;
        #endregion

        private FormDetailManager.Page builderPage;

        #region Constructor
        public FeeTaxManager(FormDetailManager.Page page) 
            : base(page)
        {
            this.builderPage = page;
            InitializeLocators();
        }
        #endregion

        #region Initialize locators
        private void InitializeLocators()
        {
            switch (this.builderPage)
            {
                case FormDetailManager.Page.Start:
                    _taxOptionDialogID = TaxOptionDialogIDInEventFeeOption;
                    _taxOptionLinkLocator = TaxOptionLinkLocatorInEventFeeOption;
                    _applyTaxRateOneCheckboxLocator = ApplyTaxRateOneCheckboxLocatorInEventFeeOption;
                    _applyTaxRateTwoCheckboxLocator = _applyTaxRateTwoCheckboxLocatorInEventFeeOption;
                    break;

                case FormDetailManager.Page.Agenda:
                    _taxOptionDialogID = _taxOptionDialogIDInAgendaFeeOption;
                    _taxOptionLinkLocator = _taxOptionLinkLocatorInAgendaFeeOption;
                    _applyTaxRateOneCheckboxLocator = _applyTaxRateOneCheckboxLocatorInAgendaFeeOption;
                    _applyTaxRateTwoCheckboxLocator = _applyTaxRateTwoCheckboxLocatorInAgendaFeeOption;
                    break;

                case FormDetailManager.Page.Merchandise:
                    _taxOptionDialogID = _taxOptionDialogIDInMerch;
                    _taxOptionLinkLocator = _taxOptionLinkLocatorInMerch;
                    _applyTaxRateOneCheckboxLocator = _applyTaxRateOneCheckboxLocatorInMerch;
                    _applyTaxRateTwoCheckboxLocator = _applyTaxRateTwoCheckboxLocatorInMerch;
                    break;

                default:
                    break;
            }
        }
        #endregion

        private void SelectThisFrame()
        {
            try
            {
                UIUtil.DefaultProvider.SelectPopUpFrameByName(_taxOptionDialogID);
            }
            catch
            {
                // Ignore
            }
        }

        #region Public methods
        public void ClickAddTaxRatesLink()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(_taxOptionLinkLocator, LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
            ////UIUtilityProvider.UIHelper.SelectUpperFrame();
            UIUtil.DefaultProvider.SelectPopUpFrameByName(_taxOptionDialogID);
        }

        public void SaveAndStay()
        {
            this.SelectThisFrame();
            UIUtil.DefaultProvider.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        public void SaveAndClose()
        {
            switch (this.builderPage)
            {
                case FormDetailManager.Page.Start:
                case FormDetailManager.Page.Merchandise:
                    this.SelectThisFrame();
                    UIUtil.DefaultProvider.ClickSaveAndClose();
                    Utility.ThreadSleep(1);
                    UIUtil.DefaultProvider.SelectPopUpFrameByName("dialog");
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                case FormDetailManager.Page.Agenda:
                    this.SelectThisFrame();
                    UIUtil.DefaultProvider.ClickSaveAndClose();
                    Utility.ThreadSleep(1);
                    SelectBuilderWindow();
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                default:
                    break;
            }
        }

        public void Cancel()
        {
            switch (this.builderPage)
            {
                case FormDetailManager.Page.Start:
                case FormDetailManager.Page.Merchandise:
                    this.SelectThisFrame();
                    UIUtil.DefaultProvider.ClickCancel();
                    Utility.ThreadSleep(1);
                    UIUtil.DefaultProvider.SelectPopUpFrameByName("dialog");
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                case FormDetailManager.Page.Agenda:
                    this.SelectThisFrame();
                    UIUtil.DefaultProvider.ClickCancel();
                    Utility.ThreadSleep(1);
                    SelectBuilderWindow();
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                default:
                    break;
            }
        }

        #region Set the "Apply tax rate one/two checkboxes" after setting up the tax rate
        public void ApplyTaxRatesToFee(bool checkOne, bool checkTwo)
        {
            ApplyTaxOneToFee(checkOne);
            ApplyTaxTwoToFee(checkTwo);
        }

        public void ApplyTaxOneToFee(bool check)
        {
            UIUtil.DefaultProvider.SetCheckbox(_applyTaxRateOneCheckboxLocator, check, LocateBy.Id);
        }

        public void ApplyTaxTwoToFee(bool check)
        {
            UIUtil.DefaultProvider.SetCheckbox(_applyTaxRateTwoCheckboxLocator, check, LocateBy.Id);
        }
        #endregion

        #region Verifying methods
        public void VerifyTaxAvailability(bool hasTaxOne, bool hasTaxTwo)
        {
            bool actualHasTaxOne = UIUtil.DefaultProvider.IsElementPresent(_applyTaxRateOneCheckboxLocator, LocateBy.Id);
            bool actualHasTaxTwo = UIUtil.DefaultProvider.IsElementPresent(_applyTaxRateTwoCheckboxLocator, LocateBy.Id);

            if (hasTaxOne != actualHasTaxOne)
            {
                if (hasTaxOne)
                {
                    Assert.Fail(string.Format("Tax rate ONE is NOT available!"));
                }
                else
                {
                    Assert.Fail(string.Format("Tax rate ONE is available!"));
                }
            }

            if (hasTaxTwo != actualHasTaxTwo)
            {
                if (hasTaxTwo)
                {
                    Assert.Fail(string.Format("Tax rate TWO is NOT available!"));
                }
                else
                {
                    Assert.Fail(string.Format("Tax rate TWO is available!"));
                }
            }
        }

        public void VerifyTaxOneSettings(string captionOne, double percentageOne, bool check)
        {
            VerifyTool.VerifyValue(string.Format("Apply {0} ({1}%)", captionOne, Convert.ToString(percentageOne)),
                UIUtil.DefaultProvider.GetText(string.Format("//label[@for='{0}']", _applyTaxRateOneCheckboxLocator), LocateBy.XPath),
                "The caption and percentage of tax rate ONE : {0}");

            VerifyTool.VerifyValue(check, UIUtil.DefaultProvider.GetValue(_applyTaxRateOneCheckboxLocator, LocateBy.Id) == "on", "The checkbox for tax rate ONE is checked : {0}");
        }

        public void VerifyTaxTwoSettings(string captionTwo, double percentageTwo, bool check)
        {
            VerifyTool.VerifyValue(string.Format("Apply {0} ({1}%)", captionTwo, Convert.ToString(percentageTwo)),
                UIUtil.DefaultProvider.GetText(string.Format("//label[@for='{0}']", _applyTaxRateTwoCheckboxLocator), LocateBy.XPath),
                "The caption and percentage of tax rate TWO : {0}");

            VerifyTool.VerifyValue(check, UIUtil.DefaultProvider.GetValue(_applyTaxRateTwoCheckboxLocator, LocateBy.Id) == "on", "The checkbox for tax rate TWO is checked : {0}");
        }
        #endregion

        #endregion
    }
}
