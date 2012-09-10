namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Linq;
    using System.Reflection;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class FormDetailManager : ManagerBase
    {
        #region Constants
        private const string PageLinkLocatorFormat = "//a[@accesskey='{0}']";
        private const string SplashButton = "//div[@id='splashChoicePage']//span[text()='{0}']";
        private const string StartPageHeaderLocator = "//*[text()='Add Start Page Header']";
        private const string StartPageFooterLocator = "//*[text()='Add Start Page Footer']";
        private const string PersonalInfoPageHeaderLocator = "//*[text()='Add Personal Information Page Header']";
        private const string PersonalInfoPageFooterLocator = "//*[text()='Add Personal Information Page Footer']";
        private const string AgendaPageHeaderLocator = "//*[text()='Add Agenda Page Header']";
        private const string AgendaPageFooterLocator = "//*[text()='Add Agenda Page Footer']";
        private const string LodgingTravelPageHeaderLocator = "//*[text()='Add Lodging & Travel Page Header']";
        private const string LodgingTravelPageFooterLocator = "//*[text()='Add Lodging & Travel Page Footer']";
        private const string MerchandisePageHeaderLocator = "//*[text()='Add Merchandise Page Header']";
        private const string MerchandisePageFooterLocator = "//*[text()='Add Merchandise Page Footer']";
        private const string CheckoutPageHeaderLocator = "//*[text()='Add Checkout Page Header']";
        private const string CheckoutPageFooterLocator = "//*[text()='Add Checkout Page Footer']";
        #endregion

        #region Enum
        public enum Tab
        {
            [StringValue("Registration Form Pages")]
            RegistrationFormPages,

            [StringValue("Theme Designer")]
            ThemeDesigner,

            [StringValue("Event Website")]
            EventWebsite,

            [StringValue("Emails")]
            Emails
        }

        public enum Page
        {
            [PageAccessKey("S")]
            [PageIndex(0)]
            Start,

            [PageAccessKey("I")]
            [PageIndex(1)]
            PI,

            [PageAccessKey("H")]
            [PageIndex(2)]
            Agenda,

            [PageAccessKey("L")]
            [PageIndex(3)]
            LodgingTravel,

            [PageAccessKey("M")]
            [PageIndex(4)]
            Merchandise,

            [PageAccessKey("K")]
            [PageIndex(5)]
            Checkout,

            [PageAccessKey("C")]
            [PageIndex(6)]
            Confirmation
        }
        #endregion

        public class PageAccessKeyAttribute : Attribute
        {
            public string AccessKey { get; set; }

            public PageAccessKeyAttribute(string key)
            {
                this.AccessKey = key;
            }

            public static string GetAccessKey(Page page)
            {
                Type type = page.GetType();
                FieldInfo fi = type.GetField(page.ToString());
                PageAccessKeyAttribute[] attrs = fi.GetCustomAttributes(typeof(PageAccessKeyAttribute), false) as PageAccessKeyAttribute[];
                return attrs[0].AccessKey;
            }
        }

        public class PageIndexAttribute : Attribute
        {
            public int PageIndex { get; set; }

            public PageIndexAttribute(int index)
            {
                this.PageIndex = index;
            }

            public static int GetPageIndex(Page page)
            {
                Type type = page.GetType();
                FieldInfo fi = type.GetField(page.ToString());
                PageIndexAttribute[] attrs = fi.GetCustomAttributes(typeof(PageIndexAttribute), false) as PageIndexAttribute[];
                return attrs[0].PageIndex;
            }
        }

        #region Configuration

        private int eventId = InvalidId;
        private int EventId
        {
            get
            {
                if (eventId == InvalidId)
                {
                    eventId = this.GetEventId();
                }
                return eventId;
            }
            set
            {
                eventId = value;
            }
        }

        private Event evt = null;
        public Event Event
        {
            get
            {
                if (evt == null)
                {
                    ClientDataContext db = new ClientDataContext();
                    evt = (from e in db.Events where e.Id == EventId select e).Single();
                }
                return evt;
            }
            set
            {
                evt = value;
            }
        }

        private DataHelper dataHelper;

        public FormDetailManager()
        {
            this.EventFeeMgr = new EventFeeManager(FeeLocation.Event);
            this.CFMgr = new CustomFieldManager();
            this.AGMgr = new AgendaItemManager();
            this.MerchMgr = new MerchandiseManager();
            this.AdvancedSettingsMgr = new EventAdvancedSettingsManager();
            this.RegTypeMgr = new RegTypeManager();
            this.GroupDiscountRuleMgr = new GroupDiscountRuleManager();
            this._hotelMgr = new HotelManager();
            this._lodgingStandardFieldsMgr = new LodgingStandardFieldsManager();
            this._lodgingSettingsAndPaymentOptionsMgr = new LodgingSettingsAndPaymentOptionsManager();
            this._travelStandardAdditionalFieldsMgr = new TravelStandardAdditionalFieldsManager();
            this._travelSettingsAndPaymentOptionsMgr = new TravelSettingsAndPaymentOptionsManager();
            this.TaxMgr = new TaxManager(Page.Checkout);
            this.PaymentMethodMgr = new PaymentMethodManager();
            this.dataHelper = new DataHelper();
            this.OldAGAndCFMgr = new OtherEventTypeAgendaAndCFManager();

            this.InitializePersonalInfoFieldVisibleOptionLocatorDictionary();
        }

        #endregion

        [Step]
        public void ClickYesOnSplashPage()
        {
            // Click Yes on splash page, if needed
            string locator = string.Format(SplashButton, "Yes");
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(locator, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.HideActiveSpecificFooter(true);
        }

        [Step]
        public void SaveAndStay()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_btnSaveStay", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.HideActiveSpecificFooter(true);
        }

        [Step]
        public void SaveAndClose()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_btnSaveClose", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void Close()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_hplReturnToManger", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public void Next()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_btnNext", LocateBy.Id);
            Utility.ThreadSleep(2);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.HideActiveSpecificFooter(true);
        }

        public void Previous()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_btnPrev", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.HideActiveSpecificFooter(true);
        }

        [Verify]
        public void VerifySplashPage()
        {
            WebDriverUtility.DefaultProvider.VerifyElementPresent(string.Format(SplashButton, "Yes"), true, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.VerifyElementPresent(string.Format(SplashButton, "No"), true, LocateBy.XPath);
        }

        [Step]
        public int GetEventId()
        {
            WebDriverUtility.DefaultProvider.WaitForElementPresent("eventId", LocateBy.Name);
            return Convert.ToInt32(WebDriverUtility.DefaultProvider.GetValue("eventId", LocateBy.Name));
        }

        public string GetEventWebsiteUrl()
        {
            string eventWebsite;
            string shortCut = WebDriverUtility.DefaultProvider.GetValue("ctl00_cph_txtEventsShortcutDescription", LocateBy.Id);
            eventWebsite = ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + shortCut;
            return eventWebsite;
        }

        public void ClickAdvancedOnFrame()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//span[text()='Advanced']", LocateBy.XPath);
            Utility.ThreadSleep(0.5);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public bool OnPage(Page page)
        {
            string accessKeyAttribute = PageAccessKeyAttribute.GetAccessKey(page);
            string accessKeyAttributeString = WebDriverUtility.DefaultProvider.GetAttribute(string.Format(PageLinkLocatorFormat, accessKeyAttribute), "class", LocateBy.XPath);

            if (accessKeyAttributeString.Equals("rtsLink rtsSelected"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Step]
        public void GotoPage(Page page)
        {
            string accesskey = string.Empty;

            switch (page)
            {
                case Page.Start:
                    accesskey = "S";
                    break;

                case Page.PI:
                    accesskey = "I";
                    break;

                case Page.Agenda:
                    accesskey = "H";
                    break;

                case Page.LodgingTravel:
                    accesskey = "L";
                    break;

                case Page.Merchandise:
                    accesskey = "M";
                    break;

                case Page.Checkout:
                    accesskey = "K";
                    break;

                case Page.Confirmation:
                    accesskey = "C";
                    break;

                default:
                    break;
            }

            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(string.Format(PageLinkLocatorFormat, accesskey), LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.HideActiveSpecificFooter(true);
        }

        public void GotoTab(Tab tab)
        {
            string tabLocatorFormat = "//div[@id='ctl00_mainTabs']//span[text()='{0}']";
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(string.Format(tabLocatorFormat, StringEnum.GetStringValue(tab)), LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.HideActiveSpecificFooter(true);
        }

        [Step]
        public void TogglePreviewAndEditMode()
        {
            string previewButtonLocator = "ctl00_btnPreview";
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(previewButtonLocator, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.HideActiveSpecificFooter(true);
        }

        [Step]
        public void SelectRegTypeWhenPreview(string regTypeName)
        {
            string regTypeDropdownLocator = "ctl00_cph_ddlRegTypes";
            string selectedRegType = WebDriverUtility.DefaultProvider.GetSelectedLabel(regTypeDropdownLocator, LocateBy.Id);

            if (!selectedRegType.Equals(regTypeName))
            {
                WebDriverUtility.DefaultProvider.SelectWithText("ctl00_cph_ddlRegTypes", regTypeName, LocateBy.Id);
                WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            }
        }

        public string GetAddGridItemLocator(string prefix)
        {
            string addLocator = prefix + "hlAddNew";

            if (!WebDriverUtility.DefaultProvider.IsElementPresent(addLocator, LocateBy.Id))
            {
                addLocator = prefix + "lnkEmptyAdd";
            }
            
            return addLocator;
        }

        public void AddPageHeaderFooter(Page page, string pageHeader, string pageFooter)
        {
            if (pageHeader != null)
            {
                switch (page)
                {
                    case Page.Start:
                        WebDriverUtility.DefaultProvider.Click(StartPageHeaderLocator, LocateBy.XPath);
                        break;
                    case Page.PI:
                        WebDriverUtility.DefaultProvider.Click(PersonalInfoPageHeaderLocator, LocateBy.XPath);
                        break;
                    case Page.Agenda:
                        WebDriverUtility.DefaultProvider.Click(AgendaPageHeaderLocator, LocateBy.XPath);
                        break;
                    case Page.LodgingTravel:
                        WebDriverUtility.DefaultProvider.Click(LodgingTravelPageHeaderLocator, LocateBy.XPath);
                        break;
                    case Page.Merchandise:
                        WebDriverUtility.DefaultProvider.Click(MerchandisePageHeaderLocator, LocateBy.XPath);
                        break;
                    case Page.Checkout:
                        WebDriverUtility.DefaultProvider.Click(CheckoutPageHeaderLocator, LocateBy.XPath);
                        break;
                    default:
                        break;
                }

                WebDriverUtility.DefaultProvider.WaitForPageToLoad();
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
                WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                WebDriverUtility.DefaultProvider.SelectIFrameOnCurrentIFrame(1);
                WebDriverUtility.DefaultProvider.Type("//textarea", pageHeader, LocateBy.XPath);
                WebDriverUtility.DefaultProvider.SwitchToMainContent();
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
                WebDriverUtility.DefaultProvider.Click("ctl00_btnSaveClose", LocateBy.Id);
                WebDriverUtility.DefaultProvider.WaitForPageToLoad();
                WebDriverUtility.DefaultProvider.SwitchToMainContent();
                Utility.ThreadSleep(2);
            }

            if (pageFooter != null)
            {
                switch (page)
                {
                    case Page.Start:
                        WebDriverUtility.DefaultProvider.Click(StartPageFooterLocator, LocateBy.XPath);
                        break;
                    case Page.PI:
                        WebDriverUtility.DefaultProvider.Click(PersonalInfoPageFooterLocator, LocateBy.XPath);
                        break;
                    case Page.Agenda:
                        WebDriverUtility.DefaultProvider.Click(AgendaPageFooterLocator, LocateBy.XPath);
                        break;
                    case Page.LodgingTravel:
                        WebDriverUtility.DefaultProvider.Click(LodgingTravelPageFooterLocator, LocateBy.XPath);
                        break;
                    case Page.Merchandise:
                        WebDriverUtility.DefaultProvider.Click(MerchandisePageFooterLocator, LocateBy.XPath);
                        break;
                    case Page.Checkout:
                        WebDriverUtility.DefaultProvider.Click(CheckoutPageFooterLocator, LocateBy.XPath);
                        break;
                    default:
                        break;
                }

                WebDriverUtility.DefaultProvider.WaitForPageToLoad();
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
                WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
                WebDriverUtility.DefaultProvider.SelectIFrameOnCurrentIFrame(1);
                WebDriverUtility.DefaultProvider.Type("//textarea", pageFooter, LocateBy.XPath);
                WebDriverUtility.DefaultProvider.SwitchToMainContent();
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
                WebDriverUtility.DefaultProvider.Click("ctl00_btnSaveClose", LocateBy.Id);
                WebDriverUtility.DefaultProvider.WaitForPageToLoad();
                WebDriverUtility.DefaultProvider.SwitchToMainContent();
                Utility.ThreadSleep(2);
            }
        }
    }
}
