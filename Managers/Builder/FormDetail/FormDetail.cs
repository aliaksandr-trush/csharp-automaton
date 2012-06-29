namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Linq;
    using System.Reflection;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Linq;
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
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.HideActiveSpecificFooter(true);
        }

        [Step]
        public void SaveAndStay()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_btnSaveStay", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.HideActiveSpecificFooter(true);
        }

        [Step]
        public void SaveAndClose()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_btnSaveClose", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void Close()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_hplReturnToManger", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void Next()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_btnNext", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.HideActiveSpecificFooter(true);
        }

        public void Previous()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_btnPrev", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.HideActiveSpecificFooter(true);
        }

        [Verify]
        public void VerifySplashPage()
        {
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format(SplashButton, "Yes"), true, LocateBy.XPath);
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format(SplashButton, "No"), true, LocateBy.XPath);
        }

        [Step]
        public int GetEventId()
        {
            UIUtilityProvider.UIHelper.WaitForElementPresent("eventId", LocateBy.Name);
            return Convert.ToInt32(UIUtilityProvider.UIHelper.GetValue("eventId", LocateBy.Name));
        }

        public string GetEventWebsiteUrl()
        {
            string eventWebsite;
            string shortCut = UIUtilityProvider.UIHelper.GetValue("ctl00_cph_txtEventsShortcutDescription", LocateBy.Id);
            eventWebsite = ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrl + shortCut;
            return eventWebsite;
        }

        public void ClickAdvancedOnFrame()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='Advanced']", LocateBy.XPath);
            Utility.ThreadSleep(0.5);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public bool OnPage(Page page)
        {
            string accessKeyAttribute = PageAccessKeyAttribute.GetAccessKey(page);
            string accessKeyAttributeString = UIUtilityProvider.UIHelper.GetAttribute(string.Format(PageLinkLocatorFormat, accessKeyAttribute), "class", LocateBy.XPath);

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

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(PageLinkLocatorFormat, accesskey), LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.HideActiveSpecificFooter(true);
        }

        public void GotoTab(Tab tab)
        {
            string tabLocatorFormat = "//div[@id='ctl00_mainTabs']//span[text()='{0}']";
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(tabLocatorFormat, StringEnum.GetStringValue(tab)), LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.HideActiveSpecificFooter(true);
        }

        [Step]
        public void TogglePreviewAndEditMode()
        {
            string previewButtonLocator = "ctl00_btnPreview";
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(previewButtonLocator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.HideActiveSpecificFooter(true);
        }

        [Step]
        public void SelectRegTypeWhenPreview(string regTypeName)
        {
            string regTypeDropdownLocator = "ctl00_cph_ddlRegTypes";
            string selectedRegType = UIUtilityProvider.UIHelper.GetSelectedLabel(regTypeDropdownLocator, LocateBy.Id);

            if (!selectedRegType.Equals(regTypeName))
            {
                UIUtilityProvider.UIHelper.SelectWithText("ctl00_cph_ddlRegTypes", regTypeName, LocateBy.Id);
                UIUtilityProvider.UIHelper.WaitForPageToLoad();
            }
        }

        public string GetAddGridItemLocator(string prefix)
        {
            string addLocator = prefix + "hlAddNew";

            if (!UIUtilityProvider.UIHelper.IsElementPresent(addLocator, LocateBy.Id))
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
                        UIUtilityProvider.UIHelper.Click(StartPageHeaderLocator, LocateBy.XPath);
                        break;
                    case Page.PI:
                        UIUtilityProvider.UIHelper.Click(PersonalInfoPageHeaderLocator, LocateBy.XPath);
                        break;
                    case Page.Agenda:
                        UIUtilityProvider.UIHelper.Click(AgendaPageHeaderLocator, LocateBy.XPath);
                        break;
                    case Page.LodgingTravel:
                        UIUtilityProvider.UIHelper.Click(LodgingTravelPageHeaderLocator, LocateBy.XPath);
                        break;
                    case Page.Merchandise:
                        UIUtilityProvider.UIHelper.Click(MerchandisePageHeaderLocator, LocateBy.XPath);
                        break;
                    case Page.Checkout:
                        UIUtilityProvider.UIHelper.Click(CheckoutPageHeaderLocator, LocateBy.XPath);
                        break;
                    default:
                        break;
                }

                UIUtilityProvider.UIHelper.WaitForPageToLoad();
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
                UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(1);
                UIUtilityProvider.UIHelper.Type("//textarea", pageHeader, LocateBy.XPath);
                UIUtilityProvider.UIHelper.SwitchToMainContent();
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
                UIUtilityProvider.UIHelper.Click("ctl00_btnSaveClose", LocateBy.Id);
                UIUtilityProvider.UIHelper.WaitForPageToLoad();
                UIUtilityProvider.UIHelper.SwitchToMainContent();
                Utility.ThreadSleep(2);
            }

            if (pageFooter != null)
            {
                switch (page)
                {
                    case Page.Start:
                        UIUtilityProvider.UIHelper.Click(StartPageFooterLocator, LocateBy.XPath);
                        break;
                    case Page.PI:
                        UIUtilityProvider.UIHelper.Click(PersonalInfoPageFooterLocator, LocateBy.XPath);
                        break;
                    case Page.Agenda:
                        UIUtilityProvider.UIHelper.Click(AgendaPageFooterLocator, LocateBy.XPath);
                        break;
                    case Page.LodgingTravel:
                        UIUtilityProvider.UIHelper.Click(LodgingTravelPageFooterLocator, LocateBy.XPath);
                        break;
                    case Page.Merchandise:
                        UIUtilityProvider.UIHelper.Click(MerchandisePageFooterLocator, LocateBy.XPath);
                        break;
                    case Page.Checkout:
                        UIUtilityProvider.UIHelper.Click(CheckoutPageFooterLocator, LocateBy.XPath);
                        break;
                    default:
                        break;
                }

                UIUtilityProvider.UIHelper.WaitForPageToLoad();
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
                UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(1);
                UIUtilityProvider.UIHelper.Type("//textarea", pageFooter, LocateBy.XPath);
                UIUtilityProvider.UIHelper.SwitchToMainContent();
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
                UIUtilityProvider.UIHelper.Click("ctl00_btnSaveClose", LocateBy.Id);
                UIUtilityProvider.UIHelper.WaitForPageToLoad();
                UIUtilityProvider.UIHelper.SwitchToMainContent();
                Utility.ThreadSleep(2);
            }
        }
    }
}
