namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public enum RegistrationMethod
    {
        Registrant,
        Admin,
        EventWebsite,
        RegTypeDirectLink
    }

    public enum SeleniumAction
    {
        Click,
        Select,
        Type
    }

    public partial class RegisterManager : ManagerBase
    {
        #region constants
        private const string AddAnotherPersonLocator = "//*[text()='Add Another Person']";
        private const string AddPersonToWaitlistLocator = "//*[text()='Add a Person to the Waitlist']";
        private const string ContinueButton = "//div[@class='buttonGroup']//button[@type='submit']";
        private const string ContinueButton_Old = "//span[text()='Continue']";
        private const string ContinueToNextStepButton = "//span[text()='Continue to Next Step']";
        private const string FinishButton = "//div[@class='buttonGroup']//button[@type='submit']";
        private const string FinalizeButton = "//span[text()='Finalize']";
        private const string ErrorDIVLocator = "//div[@id='ctl00_valSummary']";
        private const string ErrorLocator = ErrorDIVLocator + "/ul";
        private const string PreFillDropDown = ".preFill select";

        public const string TrustwaveLocator = "logo1";
        public const string EventHomeLocator = "ctl00_lnkHome";
        public const string EventContactInfoLocator = "ctl00_lnkContactInfo";
        public const string FacebookLocator = "social1";
        public const string TwitterLocator = "social2";
        public const string LinkedinLocator = "social3";
        public const string CopyrightLocator = "activeCopyright";
        public const string TermsOfUseLocator = "//*[@id='activeLinks']/li[1]/a";
        public const string PrivacyPolicyLocator = "//*[@id='activeLinks']/li[2]/a";
        public const string AboutLocator = "//*[@id='activeLinks']/li[3]/a";
        public const string ActiveComLocator = "//*[@id='activeLogo']/a";
        public const string EventDetailsLocator = "//*[text()='(View Details)']";
        public const string LocationLocator = "//*[@id='fullEventDetails']//span[@class='fn org']";
        public const string Address1Locator = "//*[@id='fullEventDetails']//span[@class='street-address']";
        public const string Address2Locator = "//*[@id='fullEventDetails']//span[@class='extended-address']";
        public const string CityLocator = "//*[@id='fullEventDetails']//span[@class='locality']";
        public const string StateLocator = "//*[@id='fullEventDetails']//span[@class='region']";
        public const string ZipLocator = "//*[@id='fullEventDetails']//span[@class='postal-code']";
        public const string CountryLocator = "//*[@id='fullEventDetails']//span[@class='country-name']";
        public const string PhoneLocator = "//*[@id='fullEventDetails']//span[@class='tel']";
        public const string ContactLocator = "//*[@id='fullEventDetails']//*[@class='contact']";
        public const string PopupContactInfoLocator = "dialogPadding";
        public const string PopupContactInfoClose = "//*[text()='close']";
        public const string EventTitleLocator = "summary";
        public const string PageHeaderLocator = "//*[@id='pageHeader']";
        public const string PageFooterLocator = "//*[@id='pageFooter']";
        #endregion

        #region Enum
        public enum CheckoutVerifyColumn
        {
            Name = 1,
            Email,
            Type
        }

        public enum AMorPM
        {
            am,
            pm
        }

        public enum RegistrationStatus
        {
            [StringValue("Pending")]
            Pending,

            [StringValue("Confirmed")]
            Confirmed,

            [StringValue("Approved")]
            Approved,

            [StringValue("Declined")]
            Declined,

            [StringValue("Standby")]
            Standby,

            [StringValue("No-show")]
            NoShow,

            [StringValue("Follow-up")]
            FollowUp
        }

        public enum PricingSchedule
        {
            [StringValue("(Early)")]
            Early,

            [StringValue("(Standard)")]
            Standard,

            [StringValue("(Late)")]
            Late
        }

        public enum RegisterPages
        {
            Checkin,
            PersonalInfo,
            Agenda,
            LodgingTravel,
            Merchandise,
            Checkout,
            Confirmation
        }
        #endregion

        #region Public/private fields
        public string CurrentEmail;
        public string CurrentRegistrantLastName;
        public int CurrentEventId;
        public int CurrentRegistrationId;
        public int CurrentRegistrationTypeID = InvalidId;
        public long CurrentTicks;
        public string CurrentTotal;
        public List<CustomFieldResponses> customFieldResponses;
        public List<MerchandiseResponses> merchandiseResponses;
        public LodgingResponses lodgingResponses;
        public TravelResponses travelResponses;
        #endregion

        #region Properties
        public string CurrentRegistrantFullName
        {
            get
            {
                return DefaultPersonalInfo.FirstName + " " + DefaultPersonalInfo.MiddleName + " " + CurrentRegistrantLastName;
            }
        }

        public DataHelper DataTool { private set; get; }
        public EventCalendarManager EventCalendarMgr { private set; get; }
        #endregion

        public RegisterManager()
        {
            this.CurrentEmail = String.Empty;
            this.CurrentRegistrantLastName = String.Empty;

            this.CurrentEventId = InvalidId;
            this.CurrentRegistrationId = InvalidId;
            this.CurrentRegistrationTypeID = InvalidId;
            this.CurrentTicks = 0;
            this.CurrentTotal = String.Empty;

            this.customFieldResponses = new List<CustomFieldResponses>();
            this.merchandiseResponses = new List<MerchandiseResponses>();

            this.lodgingResponses = new LodgingResponses();
            this.travelResponses = new TravelResponses();

            //this.customFieldsService = new S.CustomFieldsService();
            //this.customFieldListItemsService = new S.CustomFieldListItemsService();
            this.DataTool = new DataHelper();

            this.PaymentMgr = new PaymentManager();
            this.EventCalendarMgr = new EventCalendarManager();
        }

        #region General

        public int GetRegID()
        {
            int regId = InvalidId;
            bool result = int.TryParse(WebDriverUtility.DefaultProvider.GetValue("//input[@name='RegisterId']", LocateBy.XPath), out regId);

            if (!result)
            {
                WebDriverUtility.DefaultProvider.FailTest("Invalid reg ID!");
            }

            return regId;
        }

        public string GetSessionId()
        {
            return WebDriverUtility.DefaultProvider.GetQueryStringValue("SessionId");
        }

        public int GetEventId()
        {
            return Convert.ToInt32(
                WebDriverUtility.DefaultProvider.GetQueryStringValue("EventId"));
        }

        /// <summary>
        /// Verifies you are on a current page
        /// </summary>
        /// <param name="pagePath">Should be formaatted like: "register/PersonalInfo.aspx"</param>
        /// <returns></returns>
        public bool OnPage(string pagePath)
        {
            return WebDriverUtility.DefaultProvider.UrlContainsAbsolutePath(pagePath);
        }

        [Step]
        public int GetRegIdFromSession()
        {
            string sessionID = this.GetSessionId();
            int eventId = this.GetEventId();
            int regId = 0;

            var db = new ClientDataContext();
            var register = (from r in db.Registrations where r.Event_Id == eventId && r.SessionId == sessionID orderby r.Register_Id ascending select r).ToList().Last();
            regId = register.Register_Id;

            if (regId == 0)
            {
                WebDriverUtility.DefaultProvider.FailTest("Invalid reg ID!");
            }

            return regId;
        }

        public int GetCurrentRegIDFromQueryString()
        {
            int regId = Convert.ToInt32(WebDriverUtility.DefaultProvider.GetQueryStringValue("RegisterId"));

            if (regId == InvalidId)
            {
                WebDriverUtility.DefaultProvider.FailTest("Invalid reg ID!");
            }

            return regId;
        }

        public void SelectPreFillDropDown(string name)
        {
            WebDriverUtility.DefaultProvider.SelectWithText(PreFillDropDown, name, LocateBy.CssSelector);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }
        #endregion


        #region Cardinal Verification

        public bool OnCardinalVerificationPage()
        {
            bool onCardinal = false;

            Utility.ThreadSleep(3);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            
            if (WebDriverUtility.DefaultProvider.UrlContainsPath("regonline.com/register/cc/enrollment.aspx?"))
            {
                onCardinal = true;
            }

            return onCardinal;
        }

        [Step]
        public void SubmitCardinalPassword(string cardinalPassword)
        {
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.SelectUpperFrame();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.Type("external.field.password", cardinalPassword, LocateBy.Name);
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("UsernamePasswordEntry", LocateBy.Name);
            Utility.ThreadSleep(3);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }
        #endregion
    }
}
