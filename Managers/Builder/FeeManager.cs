namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public class FeeManager : ManagerBase
    {
        #region Constants
        private readonly string StandardPriceTxtboxLocatorOnStartPage;
        private const string OptionLinkLocatorOnStartPage = "ctl00_cphDialog_cfCF_mipPrc_MoreInfoButtonPricing1_optionsLink";
        private const string PricingOptionsDIVLocatorOnStartPage = "ctl00_cphDialog_cfCF_mipPrc_ctl02";
        private readonly string StandardPriceTxtboxLocatorOnAgendaPage;
        private const string OptionLinkLocatorOnAgendaPage = "ctl00_cph_ucCF_mipPrc_MoreInfoButtonPricing1_optionsLink";
        private const string PricingOptionsDIVLocatorOnAgendaPage = "ctl00_cph_ucCF_mipPrc_ctl02";
        #endregion

        #region Properties
        public PricingScheduleManager Pricing { get; set; }
        public DiscountCodeManager DC { get; set; }
        public FeeTaxManager Tax { get; set; }
        #endregion

        #region Locator variables needed to be initialized
        protected string standardPriceTxtboxLocator;
        protected string optionLinkLocator;
        protected string pricingScheduleDIVLocator;
        #endregion

        #region Constructor
        public FeeManager(FormDetailManager.Page page) 
        {
            this.StandardPriceTxtboxLocatorOnStartPage = "ctl00_cphDialog_cfCF_mipPrc_rntAmount";
            this.StandardPriceTxtboxLocatorOnAgendaPage = "ctl00_cph_ucCF_mipPrc_rntAmount";

            InitializeLocators(page);

            Pricing = new PricingScheduleManager(page);
            DC = new DiscountCodeManager(page);
            Tax = new FeeTaxManager(page);
        }
        #endregion

        #region Initialize locators
        public void InitializeLocators(FormDetailManager.Page page)
        {
            switch(page)
            {
                case FormDetailManager.Page.Start:
                    standardPriceTxtboxLocator = StandardPriceTxtboxLocatorOnStartPage;
                    optionLinkLocator = OptionLinkLocatorOnStartPage;
                    pricingScheduleDIVLocator = PricingOptionsDIVLocatorOnStartPage;
                    break;
                case FormDetailManager.Page.Agenda:
                    standardPriceTxtboxLocator = StandardPriceTxtboxLocatorOnAgendaPage;
                    optionLinkLocator = OptionLinkLocatorOnAgendaPage;
                    pricingScheduleDIVLocator = PricingOptionsDIVLocatorOnAgendaPage;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Public methods
        // Notion: you cannot set the price to null if you already have set the pricing schedule options or discount code option or tax option
        // However you can set it to 0.00
        public void SetStandardPrice(double? price)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById(this.standardPriceTxtboxLocator, price);
        }

        public double GetStandardPrice()
        {
            return Convert.ToDouble(WebDriverUtility.DefaultProvider.GetValue(this.standardPriceTxtboxLocator, LocateBy.Id));
        }

        protected void ClickOptionLink()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(this.optionLinkLocator, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            System.Threading.Thread.Sleep(3000);
        }

        [Step]
        public void ExpandOption()
        {
            // Some Expand options - like with pricing - do an AJAX thing. So it might not even have rendered to HTML yet.
            if (!WebDriverUtility.DefaultProvider.IsElementPresent(this.pricingScheduleDIVLocator, LocateBy.Id) || WebDriverUtility.DefaultProvider.IsElementHidden(this.pricingScheduleDIVLocator, LocateBy.Id))
            {
                this.ClickOptionLink();
            }
        }

        public void CollapseOption()
        {
            if (!WebDriverUtility.DefaultProvider.IsElementHidden(this.pricingScheduleDIVLocator, LocateBy.Id))
            {
                this.ClickOptionLink();
            }
        }

        #region Verify
        public void VerifyStandardPrice(double? price)
        {
            VerifyTool.VerifyValue(price, this.GetStandardPrice(), "Agenda item standard price: {0}");
        }
        #endregion
        #endregion
    }
}
