namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Reflection;
    using RegOnline.RegressionTest.Utilities;

    public class FormData
    {
        public static string EventSessionId;

        #region Enums
        public enum EarlyPriceType
        {
            DateAndTime,
            Registrants
        }

        public enum RegLimitType
        {
            Individual,
            Group
        }

        public enum DiscountCodeType
        {
            DiscountCode,
            AccessCode
        }

        public enum ChangePriceDirection
        {
            Decrease,
            Increase
        }

        public enum ChangeType
        {
            Percent,
            FixedAmount
        }

        public enum Location
        {
            EventFee,
            RegType,
            Agenda,
            Merchandise
        }

        public enum ConfirmationOptions
        {
            OK,
            Cancel
        }

        public enum RegTypeDisplayOption
        {
            [CustomString("1")]
            DropDownList,

            [CustomString("2")]
            RadioButton
        }

        public enum Page
        {
            Start,
            PI,
            Agenda,
            LodgingTravel,
            Merchandise,
            Checkout,
            Confirmation
        }

        public enum PersonalInfoField
        {
            [CustomString("Email")]
            Email,

            [CustomString("Verify Email")]
            VerifyEmail,

            [CustomString("Prefix (Mr., Mrs., etc.)")]
            Prefix,

            [CustomString("First Name")]
            FirstName,

            [CustomString("Middle Name")]
            MiddleName,

            [CustomString("Last Name")]
            LastName,

            [CustomString("Suffix")]
            Suffix,

            [CustomString("Job Title")]
            JobTitle,

            [CustomString("Name as it would appear on the badge")]
            NameOnBadge,

            [CustomString("Company/Organization")]
            Company,

            [CustomString("Country")]
            Country,

            [CustomString("Address Line 1")]
            AddressLineOne,

            [CustomString("Address Line 2")]
            AddressLineTwo,

            [CustomString("City")]
            City,

            [CustomString("US State/Canadian Province")]
            State,

            [CustomString("State/Province/Region (Non US/Canada)")]
            NonUSState,

            [CustomString("Zip (Postal Code)")]
            ZipCode,

            [CustomString("Home Phone")]
            HomePhone,

            [CustomString("Work Phone")]
            WorkPhone,

            [CustomString("Extension")]
            Extension,

            [CustomString("Fax")]
            Fax,

            [CustomString("Cell Phone")]
            Cell,

            [CustomString("Date of Birth")]
            DateOfBirth,

            [CustomString("Gender")]
            Gender,

            [CustomString("Emergency Contact Name")]
            EmergencyContactName,

            [CustomString("Emergency Contact Phone")]
            EmergencyContactPhone,

            [CustomString("Secondary Email Address (cc Email)")]
            SecondaryEmailAddress,

            [CustomString("Upload Photo")]
            UploadPhoto,

            [CustomString("Membership Number")]
            MembershipNumber,

            [CustomString("Customer Number")]
            CustomerNumber,

            [CustomString("Social Security Number")]
            SocialSecurityNumber,

            [CustomString("Tax Identification Number")]
            TaxIdentificationNumber,

            [CustomString("opt out")]
            OptOut,

            [CustomString("Contact Info")]
            ContactInfo,

            [CustomString("Password")]
            Password
        }

        public enum CustomFieldType
        {
            [CustomString("1-Line Text")]
            OneLineText = 2,

            [CustomString("Check Box")]
            CheckBox = 3,

            [CustomString("Paragraph")]
            Paragraph = 4,

            [CustomString("Date")]
            Date = 5,

            [CustomString("Time")]
            Time = 6,

            [CustomString("Always Selected")]
            AlwaysSelected = 7,

            [CustomString("Section Header")]
            SectionHeader = 8,

            [CustomString("Drop Down")]
            Dropdown = 9,

            [CustomString("Radio Button")]
            RadioButton = 10,

            [CustomString("Continue Button")]
            ContinueButton = 11,

            [CustomString("Contribution")]
            Contribution = 12,

            [CustomString("Number")]
            Number = 13,

            [CustomString("File Upload")]
            FileUpload = 14,

            [CustomString("Duration")]
            Duration = 15
        }

        public enum AgendaLimitReachedOption
        {
            HideItem,
            ShowMessage,
            Waitlist
        }

        public enum Gender
        {
            Male,
            Female
        }

        public enum DateFormat
        {
            Default = 1,
            DateTime = 2,
            Time = 3,
            Date = 4,
            None = 5
        }

        public enum AgendaInitialStatus
        {
            Pending = 1,
            Confirmed = 2,
            Approved = 3,
            Declined = 4,
            NoShow = 9,
            FollowUp = 10
        }

        public enum LodgingStandardFields
        {
            RoomType,
            BedType,
            SmokingPreference,
            SharingWith,
            AdjoiningWith,
            CheckInOutDate,
            AdditionalInfo
        }

        public enum MerchandiseType
        {
            Fixed = 0,
            Variable = 1,
            Header = 9,
        }

        public enum PaymentMethod
        {
            [CustomString("Credit Cards")]
            [PaymentMethodCheckouLabel("Credit Card")]
            CreditCard,

            [CustomString("Check")]
            [PaymentMethodCheckouLabel("Check")]
            Check,

            [CustomString("Purchase Order")]
            [PaymentMethodCheckouLabel("Purchase Order")]
            PurchaseOrder,

            [CustomString("Cash (On-Site & Admin Only)")]
            [PaymentMethodCheckouLabel("Cash")]
            Cash,

            [CustomString("Pay at the Event")]
            [PaymentMethodCheckouLabel("Will Call (At The Event)")]
            PayAtTheEvent,

            [CustomString("Cost Center")]
            [PaymentMethodCheckouLabel("Cost Center")]
            CostCenter,

            [CustomString("Wire Transfer")]
            [PaymentMethodCheckouLabel("Wire Transfer")]
            WireTransfer,

            [CustomString("Custom")]
            Custom,

            [CustomString("PayPal Express Checkout")]
            [PaymentMethodCheckouLabel("PayPal")]
            PayPal
        }

        public enum FormType
        {
            [CustomString("Event")]
            ActiveEuropeEvent,

            [CustomString("Pro Event")]
            ProEvent,

            [CustomString("Express Event")]
            ExpressEvent,

            [CustomString("Lite Event")]
            LiteEvent,

            [CustomString("Membership")]
            Membership,

            [CustomString("Web Event")]
            WebEvent,

            [CustomString("Survey")]
            Survey,

            [CustomString("Donation Form")]
            DonationForm,

            [CustomString("Create from Template")]
            CreateFromTemplate
        }

        public enum XAuthType
        {
            ByUserName,
            ByUserNamePassword,
            ByEmail,
            ByEmailPassword,
            NotUse
        }

        public enum TestAccountResult
        {
            Success,
            InvalidEmail,
            AuthenticateFail,
            RequiredUsername,
            RequiredEmail,
            RequiredPassword,
            RequiredUsernamePassword,
            RequiredEmailPassword,
            UrlStartWithHttps,
            RequiredUsernameAndUrlStartWithHttps,
            RequiredEmailAndUrlStartWithHttps,
            RequiredPasswordAndUrlStartWithHttps,
            RequiredUsernamePasswordAndUrlStartWithHttps,
            RequiredEmailPasswordAndUrlStartWithHttps
        }

        public enum DashboardTab
        {
            [CustomString("Event Details")]
            EventDetails,

            [CustomString("Reports")]
            Reports,

            [CustomString("Cross-Event Reports")]
            XEventReports,

            [CustomString("Labels and Badges")]
            LabelsBadges,

            [CustomString("Attendee Directories")]
            Directories
        }

        public enum StandardReports
        {
            [CustomString("AgendaReportaspxrptType70")]
            AgendaReport,

            [CustomString("attendeeReportaspxrptType40")]
            AttendeeReport
        }

        public enum CommonlyUsedMultipleChoice
        {
            [CustomString("Agreement")]
            Agreement,

            [CustomString("Yes/No")]
            YesOrNo
        }

        public enum RegisterPage
        {
            Checkin,
            Login,
            SSOLogin,
            PersonalInfo,
            Agenda,
            LodgingTravel,
            Merchandise,
            Checkout,
            ConfirmationRedirect,
            Confirmation,
            AttendeeCheck
        }

        public enum EventCalendarView
        {
            Calendar,
            Location,
            Month,
            Day,
            Category
        }

        public enum EventStatus
        {
            [CustomString("Inactive")]
            Inactive,

            [CustomString("Sold Out")]
            SoldOut,

            [CustomString("Cancelled")]
            Cancelled,

            [CustomString("Testing")]
            Testing,

            [CustomString("On-site")]
            OnSite,

            [CustomString("Update Only")]
            UpdateOnly,

            [CustomString("Archived")]
            Archived,

            [CustomString("Active")]
            Active
        }

        public enum Countries
        {
            [CustomString("United States")]
            UnitedStates,

            [CustomString("China")]
            China,

            [CustomString("Canada")]
            Canada,

            [CustomString("European Union (EU)")]
            EU,

            [CustomString("Austria")]
            Austria
        }
        #endregion

        public static DateTime DefaultStartDate = new DateTime(2020, 10, 10);
        public static DateTime DefaultEndDate = new DateTime(2020, 10, 15);

        public class PaymentMethodCheckouLabelAttribute : Attribute
        {
            public PaymentMethodCheckouLabelAttribute(string value)
            {
                this.PaymentMethodCheckouLabelValue = value;
            }

            public string PaymentMethodCheckouLabelValue
            {
                get;
                set;
            }

            public static string GetPaymentMethodCheckouLabel(PaymentMethod value)
            {
                string label = null;
                Type type = value.GetType();

                FieldInfo fi = type.GetField(value.ToString());
                PaymentMethodCheckouLabelAttribute[] attrs = fi.GetCustomAttributes(typeof(PaymentMethodCheckouLabelAttribute), false) as PaymentMethodCheckouLabelAttribute[];
                if (attrs.Length > 0)
                    label = attrs[0].PaymentMethodCheckouLabelValue;

                return label;
            }
        }
    }
}
