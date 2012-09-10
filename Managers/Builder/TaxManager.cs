namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Linq;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public class TaxManager : ManagerBase
    {
        #region Constants
        private const string TaxOptionTitleTxtboxLocatorInFeeOption = "ctl00_cphDialog_ucTaxRates_txtEventsTaxIdentificationDesc";
        private const string TaxRateOneCaptionTxtboxLocatorInFeeOption = "ctl00_cphDialog_ucTaxRates_txtEventsTaxCaption1";
        private const string TaxRateOnePercentageTxtboxLocatorInFeeOption = "ctl00_cphDialog_ucTaxRates_txtEventsTaxRate1";
        private const string TaxRateTwoCaptionTxtboxLocatorInFeeOption = "ctl00_cphDialog_ucTaxRates_txtEventsTaxCaption2";
        private const string TaxRateTwoPercentageTxtboxLocatorInFeeOption = "ctl00_cphDialog_ucTaxRates_txtEventsTaxRate2";
        private const string TaxOptionApplyTaxToCountriesCheckboxLocatorInFeeOption = "ctl00_cph_ucTaxRates_chkEventsApplyTaxesCountries";

        private const string TaxOptionTitleTxtboxLocatorOnCheckoutPage = "ctl00_cph_ucTaxRates_txtEventsTaxIdentificationDesc";
        private const string TaxRateOneCaptionTxtboxLocatorOnCheckoutPage = "ctl00_cph_ucTaxRates_txtEventsTaxCaption1";
        private const string TaxRateOnePercentageTxtboxLocatorOnCheckoutPage = "ctl00_cph_ucTaxRates_txtEventsTaxRate1";
        private const string TaxRateTwoCaptionTxtboxLocatorOnCheckoutPage = "ctl00_cph_ucTaxRates_txtEventsTaxCaption2";
        private const string TaxRateTwoPercentageTxtboxLocatorOnCheckoutPage = "ctl00_cph_ucTaxRates_txtEventsTaxRate2";
        private const string TaxOptionApplyTaxToCountriesCheckboxLocatorOnCheckoutPage = "ctl00_cph_ucTaxRates_chkEventsApplyTaxesCountries";
        #endregion

        private string CheckoutPageUrlFormat
        {
            get
            {
                return ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "Builder/checkout.aspx?EventSessionID={0}&EventID={1}&pg=payment&previewmode=0&nodeID=&";
            }
        }

        //#region Private variables
        //private S.CountriesService countriesService;
        //#endregion

        #region Constructor
        public TaxManager(FormDetailManager.Page page) 
        {
            //Get all countries
            //this.countriesService = new S.CountriesService();

            this.InitializeLocators(page);
        }
        #endregion

        #region Initialize locators
        private void InitializeLocators(FormDetailManager.Page page)
        {
            switch (page)
            {
                case FormDetailManager.Page.Start:
                case FormDetailManager.Page.Agenda:
                    TaxOptionTitleTxtboxLocator = TaxOptionTitleTxtboxLocatorInFeeOption;
                    TaxRateOneCaptionTxtboxLocator = TaxRateOneCaptionTxtboxLocatorInFeeOption;
                    TaxRateOnePercentageTxtboxLocator = TaxRateOnePercentageTxtboxLocatorInFeeOption;
                    TaxRateTwoCaptionTxtboxLocator = TaxRateTwoCaptionTxtboxLocatorInFeeOption;
                    TaxRateTwoPercentageTxtboxLocator = TaxRateTwoPercentageTxtboxLocatorInFeeOption;
                    TaxOptionApplyTaxToCountriesCheckboxLocator = TaxOptionApplyTaxToCountriesCheckboxLocatorInFeeOption;
                    break;

                case FormDetailManager.Page.Checkout:
                    TaxOptionTitleTxtboxLocator = TaxOptionTitleTxtboxLocatorOnCheckoutPage;
                    TaxRateOneCaptionTxtboxLocator = TaxRateOneCaptionTxtboxLocatorOnCheckoutPage;
                    TaxRateOnePercentageTxtboxLocator = TaxRateOnePercentageTxtboxLocatorOnCheckoutPage;
                    TaxRateTwoCaptionTxtboxLocator = TaxRateTwoCaptionTxtboxLocatorOnCheckoutPage;
                    TaxRateTwoPercentageTxtboxLocator = TaxRateTwoPercentageTxtboxLocatorOnCheckoutPage;
                    TaxOptionApplyTaxToCountriesCheckboxLocator = TaxOptionApplyTaxToCountriesCheckboxLocatorOnCheckoutPage;
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Locator variables needed to be initialized
        protected string TaxOptionTitleTxtboxLocator;
        protected string TaxRateOneCaptionTxtboxLocator;
        protected string TaxRateOnePercentageTxtboxLocator;
        protected string TaxRateTwoCaptionTxtboxLocator;
        protected string TaxRateTwoPercentageTxtboxLocator;
        protected string TaxOptionApplyTaxToCountriesCheckboxLocator;
        #endregion

        #region Public methods
        [Verify]
        public void SetAndVerifyEUCountry(bool check, string sessionId, int eventId)
        {
            string CheckoutPageUrl = string.Format(CheckoutPageUrlFormat, sessionId, eventId.ToString());
            WebDriverUtility.DefaultProvider.OpenUrl(CheckoutPageUrl);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.SetCheckbox(TaxOptionApplyTaxToCountriesCheckboxLocator, check, LocateBy.Id);

            // Uncheck Australia
            WebDriverUtility.DefaultProvider.SetCheckbox("c_31", false, LocateBy.Id);

            // Check EU
            WebDriverUtility.DefaultProvider.SetCheckbox("c_1001", check, LocateBy.Id);

            //verify EU countries
            VerifyCountry(check, "Austria");//Austria
            VerifyCountry(check, "Belgium");//Belgium
            VerifyCountry(check, "Bulgaria");//Bulgaria
            VerifyCountry(check, "Cyprus");//Cyprus
            VerifyCountry(check, "Czech Republic");//Czech Republic
            VerifyCountry(check, "Denmark");//Denmark
            VerifyCountry(check, "Estonia");//Estonia
            VerifyCountry(check, "Finland");//Finland
            VerifyCountry(check, "France");//France
            VerifyCountry(check, "Germany");//Germany
            VerifyCountry(check, "Greece");//Greece
            VerifyCountry(check, "Hungary");//Hungary
            VerifyCountry(check, "Ireland");//Ireland
            VerifyCountry(check, "Italy");//Italy
            VerifyCountry(check, "Latvia");//Latvia
            VerifyCountry(check, "Lithuania");//Lithuania
            VerifyCountry(check, "Luxembourg");//Luxembourg
            VerifyCountry(check, "Malta");//Malta
            VerifyCountry(check, "Netherlands");//Netherlands
            VerifyCountry(check, "Poland");//Poland
            VerifyCountry(check, "Portugal");//Portugal
            VerifyCountry(check, "Romania");//Romania
            VerifyCountry(check, "Slovakia");//Slovakia
            VerifyCountry(check, "Slovenia");//Slovenia
            VerifyCountry(check, "Spain");//Spain
            VerifyCountry(check, "Sweden");//Sweden
            VerifyCountry(check, "United Kingdom");//United Kingdom
        }

        public void SetTaxRateOptions(
            string title,
            string captionOne, 
            double? percentageOne, 
            string captionTwo, 
            double? percentageTwo,
            params string[] countryNames)
        {

            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cph_chxDefineTaxRates", LocateBy.Id);
            SetTaxOptionTitle(title);
            SetTaxRateOneAndTwo(captionOne, percentageOne, captionTwo, percentageTwo);

            if (countryNames != null && countryNames.Length > 0)
            {
                this.SetApplyTaxToCountriesCheckbox(true);
                this.SelectCountries(true, countryNames);
            }
        }

        public void SetTaxOptionTitle(string title)
        {
            WebDriverUtility.DefaultProvider.Type(TaxOptionTitleTxtboxLocator, title, LocateBy.Id);
        }

        public void SetTaxRateOneAndTwo(string captionOne, double? percentageOne, string captionTwo, double? percentageTwo)
        {
            SetTaxRateOne(captionOne, percentageOne);
            SetTaxRateTwo(captionTwo, percentageTwo);
        }

        public void SetTaxRateOne(string captionOne, double? percentageOne)
        {
            SetCaptionOne(captionOne);
            SetPercentageOne(percentageOne);
        }

        public void SetTaxRateTwo(string captionTwo, double? percentageTwo)
        {
            SetCaptionTwo(captionTwo);
            SetPercentageTwo(percentageTwo);
        }

        [Step]
        public void SetApplyTaxToCountriesCheckbox(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox(TaxOptionApplyTaxToCountriesCheckboxLocator, check, LocateBy.Id);
        }

        // Check the checkboxes which match the names of the countries
        public void SelectCountries(bool checkEach, params string[] countryNames)
        {
            foreach (string countryName in countryNames)
            {
                this.SelectCountry(checkEach, countryName);
            }
        }

        [Step]
        public void SelectCountry(bool check, string countryName)
        {
            //E.Countries country = countriesService.GetByDescription(countryName);
            Country country = new Country();

            ClientDataContext db = new ClientDataContext();
            country = (from c in db.Countries where c.Description == countryName select c).Single();

            // The locator of the checkbox of a specific country is "c_" plus the country id
            WebDriverUtility.DefaultProvider.SetCheckbox("c_" + country.CountryId, check, LocateBy.Id);
        }

        #region Verify tax options
        public void VerifyCountries(bool checkEach, params string[] countryNames)
        {
            foreach (string countryName in countryNames)
            {
                VerifyCountry(checkEach, countryName);
            }
        }

        public void VerifyCountry(bool check, string countryName)
        {
            //E.Countries country = countriesService.GetByDescription(countryName);
            Country country = new Country();

            ClientDataContext db = new ClientDataContext();
            country = (from c in db.Countries where c.Description == countryName select c).Single();

            VerifyTool.VerifyValue(check, WebDriverUtility.DefaultProvider.IsChecked("c_" + country.CountryId.ToString(), LocateBy.Id), "The country " + countryName + " is selected : {0}");
        }
        #endregion

        #endregion

        #region Helper methods
        private void SetCaptionOne(string caption)
        {
            WebDriverUtility.DefaultProvider.Type(TaxRateOneCaptionTxtboxLocator, caption, LocateBy.Id);
        }

        private void SetCaptionTwo(string caption)
        {
            WebDriverUtility.DefaultProvider.Type(TaxRateTwoCaptionTxtboxLocator, caption, LocateBy.Id);
        }

        private void SetPercentageOne(double? percentage)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById(TaxRateOnePercentageTxtboxLocator, Convert.ToString(percentage));
        }

        private void SetPercentageTwo(double? percentage)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById(TaxRateTwoPercentageTxtboxLocator, Convert.ToString(percentage));
        }
        #endregion
    }
}
