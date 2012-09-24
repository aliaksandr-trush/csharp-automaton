namespace RegOnline.RegressionTest.Managers.Backend
{
    using System;
    using System.Web;
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Attributes;

    public partial class BackendManager : ManagerBase
    {
        private const string TotalRecurringChargesLocator = "//td[@id='tdRecurringFees']";
        private const string TotalChargesLocator = "//td[@id='tdTotalCharges']";
        private const string TotalTransactionsLocator = "//td[@id='tdTotalTransactions']";
        private const string TotalBalanceDueLocator = "//td[@id='tdBalanceDueFees']";
        private const string SubTotalLocator = "//td[@id='tdSubTotalOptions']";

        // Custom field response selenium paths
        public const string CustomFieldResponseText = "//td[@id='tdCustomField{0}']";
        public const string CustomFieldResponseAmountText = "//td[@id='tdCustomFieldAmount{0}']";
        public const string CustomFieldResponseCodeText = "//td[@id='tdCustomFieldCode{0}']";

        // Lodging
        public const string LodgingBookingStatus = "//td[@id='tdLodgingBookingStatus']";
        public const string LodgingHotelPreference1 = "//td[@id='tdHotelPreference1']";
        public const string LodgingHotelPreference2 = "//td[@id='tdHotelPreference2']";
        public const string LodgingConfirmationNumber = "//td[@id='tdLodgingConfirmationNumber']";
        public const string LodgingArrivalDate = "//td[@id='tdLodgingArrivalDate']";
        public const string LodgingDepartureDate = "//td[@id='tdLodgingDepartureDate']";
        public const string LodgingRoomPreference = "//td[@id='tdRoomPreference']";
        public const string LodgingBedPreference = "//td[@id='tdBedPreference']";
        public const string LodgingSmokingPreference = "//td[@id='tdSmokingPreference']";
        public const string LodgingSharePreference = "//td[@id='tdSharePreference']";
        public const string LodgingAdjoining = "//td[@id='tdAdjoining']";
        public const string LodgingSystemAssignedSharer = "//td[@id='tdSystemAssignedSharer']";
        public const string LodgingCreditCardNumber = "//td[@id='tdLodgingCCNumber']";
        public const string LodgingCreditCardExpiration = "//td[@id='tdLodgingCCExpiration']";
        public const string LodgingCreditCardHolder = "//td[@id='tdLodgingCCCardHolder']";
        public const string LodgingAdditionalInfo = "//td[@id='tdAdditionalInfo']";

        // Travel Arrival info
        public const string TravelBookingStatus = "//td[@id='tdTravelBookingStatus']";
        public const string TravelConfirmationNumber = "//td[@id='tdTravelConfirmationNumber']";
        public const string TravelArrivalAirline = "//td[@id='tdArrivalAirline']";
        public const string TravelArrivalFlightNumber = "//td[@id='tdArrivalFlightNumber']";
        public const string TravelArrivalCity = "//td[@id='tdArrivalCity']";
        public const string TravelArrivalAirport = "//td[@id='tdArrivalAirport']";
        public const string TravelArrivalDateTime = "//td[@id='tdTravelArrivalDateTime']";
        public const string TravelArrivalConnection = "//td[@id='tdArrivalConnection']";

        // Travel departure info
        public const string TravelDepartureAirline = "//td[@id='tdTravelDepartureAirline']";
        public const string TravelDepartureFlightNumber = "//td[@id='tdTravelDepartureFlightNumber']";
        public const string TravelDepartureCity = "//td[@id='tdDepartureCity']";
        public const string TravelDepartureAirport = "//td[@id='tdDepartureAirport']";
        public const string TravelDepartureDateTime = "//td[@id='tdDepartureDateTime']";
        public const string TravelDepartureConnection = "//td[@id='tdDepartureConnectionInfo']";

        // Travel miscellaneous
        public const string TravelSeatingPreference = "//td[@id='tdSeatingPreference']";
        public const string TravelFrequentFlyerNumber = "//td[@id='tdFrequentFlyerNumber']";
        public const string TravelPassportNumber = "//td[@id='tdPassportNumber']";
        public const string TravelCreditCardNumber = "//td[@id='tdTravelCCNumber']";
        public const string TravelCreditCardExpiration = "//td[@id='tdTravelCCExpiration']";
        public const string TravelCreditCardHolder = "//td[@id='tdTravelCCHolder']";
        public const string TravelAdditionalInfo = "//td[@id='tdTravelAdditionalInfo']";

        // Ground Transportation
        private const string TravelGroundTransportationPreference = "//td[@id='td1']";
        private const string TravelOtherInformation = "//td[@id='td2']";

        // Merchandise
        public const string MerchandiseResponseText = "//td[@id='tdFeeName{0}']";
        public const string MerchandiseQuantityText = "//td[@id='tdQuantity{0}']";
        public const string MerchandiseAmountText = "//td[@id='tdAmount{0}']";
        public const string MerchandiseSubTotalText = "//td[@id='tdSubTotal{0}']";
        public const string MerchandiseTotalText = "//td[@id='tdTotalFees']";

        // Transactions
        public const string TransactionRows = 
            "//div[@id = 'transactions']/div[@class = 'attendeeRecordSectionContent']/table/tbody/tr[@valign = 'middle']";

        // Membership
        public const string NextRenewDateLinkLocator = "//b[text()='Next Renew Date:']/../following-sibling::td/a";

        // Regex
        public const string NumericRegex = @"[^.0-9]";

        public enum AttendeeSubPage
        { 
            ViewAll,
            PersonalInformation,
            EventCost,
            CustomFields,
            Agenda,
            MembershipFees,
            Merchandise,
            Transactions,
            LodgingAndTravel,
            UpdateHistory,
            EmailHistory
        }

        public enum CustomFieldStatus
        {
            [StringValue("")]
            None,

            [StringValue("Pending")]
            Pending,

            [StringValue("Confirmed")]
            Confirmed,

            [StringValue("Approved")]
            Approved,

            [StringValue("Declined")]
            Declined,

            [StringValue("Canceled")]
            Canceled,

            [StringValue("Attended")]
            Attended,

            [StringValue("No-show")]
            Noshow,

            [StringValue("Follow-up")]
            Followup
        }

        public enum PricingOption
        {
            Early = 1,
            Standard = 3,
            Late = 2
        }

        public enum PersonalInfoEditField
        {
            Status,
            RegType,
            Prefix,
            First_Name,
            Middle_Name,
            Last_Name,
            Suffix,
            Title,
            Company,
            Country,
            Address_1,
            Address_2,
            City,
            StateUSCanada,
            StateNonUS,
            Postal_Code,
            HomePhone,
            WorkPhone,
            Extension,
            Fax,
            CellPhone,
            Email,
            OptOutDirectory,
            SecondaryEmail,
            BadgeName,
            //UploadPhoto,
            MembershipNumber,
            CustomerNumber,
            SocialSecurityNumber,
            DateOfBirth_Month,
            DateOfBirth_Day,
            DateOfBirth_Year,
            //Gender_ClearSelection,
            //Gender_Male,
            //Gender_Female,
            EmergencyContactName,
            EmergencyContactPhone,
            TaxId,
            ContactName,
            ContactPhone,
            ContactEmail,
            Notes
        }

        public enum Gender
        {
            [StringValue("rbGenderNoSelection")]
            ClearSelection,

            [StringValue("rbGenderMale")]
            Male,

            [StringValue("rbGenderFemale")]
            Female
        }

        public enum PersonalInfoViewField
        {
            Id,
            Status,
            RegType,
            FullName,
            JobTitle,
            Company,
            Country,
            AddressLine1,
            AddressLine2,
            CityStateZip,
            WorkPhone,
            Extension,
            HomePhone,
            Fax,
            CellPhone,
            Email,
            OptOutDirectory,
            SecondaryEmail,
            Photo,
            Password,
            RegisterDate,
            TotalCredits,
            NameOnBadge,
            MembershipNumber,
            CustomerNumber,
            SocialSecurityNumber,
            DateOfBirth,
            Gender,
            EmergencyContactName,
            EmergencyContactPhone,
            TaxId,
            ContactName,
            ContactPhone,
            ContactEmail,
            Notes
        }

        public enum LodgingViewField
        {
            BookingStatus,
            //BookingAgent,
            ConfirmationCode,
            HotelPrimary,
            HotelSecondary,
            ArrivalDate,
            //ArrivalTime,
            DepartureDate,
            //DepartureTime,
            RoomPreference,
            BedPreference,
            SmokingPreference,
            ShareWith,
            RoomSharerId,
            CreditCardNumber,
            AdjoinWith,
            //BookingFee,
            AdditionalInfo
        }

        public enum TravelViewField
        {
            BookingStatus,
            //BookingAgent,
            ConfirmationCode,
            ArrivalAirline,
            ArrivalCity,
            ArrivalAirport,
            ArrivalDateTime,
            //ArrivalTime,
            ArrivalConnection,
            DepartureCity,
            DepartureAirport,
            DepartureDateTime,
            //DepartureTime,
            DepartureAirline,
            DepartureConnection,
            SeatingPreference,
            FrequentFlyerNumber,
            PassportNumber,
            CreditCardNumber,
            CreditCardExpiration,
            CreditCardHolderName,
            CreditCardInfo,
            GroundTransportation,
            GroundTransportationInfo
        }

        public enum LodgingEditField
        {
            BookingStatus,
            BookingAgent,
            ConfirmationCode,
            HotelPrimary,
            HotelSecondary,
            ArrivalDate,
            ArrivalTime,
            DepartureDate,
            DepartureTime,
            RoomPreference,
            BedPreference,
            SmokingPreference,
            ShareWith,
            RoomSharerId,
            AdjoinWith,
            BookingFee,
            AdditionalInfo
        }

        public enum TravelEditField
        {
            BookingStatus,
            BookingAgent,
            ConfirmationCode,
            ArrivalAirline,
            ArrivalCity,
            ArrivalAirport,
            ArrivalDate,
            ArrivalTime,
            ArrivalConnection,
            DepartureCity,
            DepartureAirport,
            DepartureDate,
            DepartureTime,
            DepartureAirline,
            DepartureConnection,
            SeatingPreference,
            FrequentFlyerNumber,
            PassportNumber,
            CreditCardNumber,
            CreditCardExpiration,
            CreditCardHolderName,
            CreditCardInfo,
            GroundTransportation,
            GroundTransportationInfo
        }

        /// <summary>
        /// Purpose of this class is to verify transactions
        /// </summary>
        public class TransactionResponse
        {
            public string Id = null;
            public string Date = null;
            public string Type = null;
            public string Notes = null;
            public string Amount = null;
            public string SubTotal = null;
            public string AddBy = null;
            public string ModBy = null;
            public string Delete = null;

            public override bool Equals(object obj)
            {
                TransactionResponse response = obj as TransactionResponse;

                if (obj != null)
                {
                    return this.Id == response.Id &&
                        this.Date == response.Date &&
                        this.Type == response.Type &&
                        this.Notes == response.Notes &&
                        this.Amount == response.Amount &&
                        this.SubTotal == response.SubTotal &&
                        this.AddBy == response.AddBy &&
                        this.ModBy == response.ModBy &&
                        this.Delete == response.Delete;
                }

                return false;
            }
        }

        [Step]
        public void OpenAttendeeInfoURL(string sessionID, int regID)
        {
            UIUtil.DefaultProvider.OpenUrl(HttpUtility.UrlDecode(string.Format(
                "{0}reports/Attendee.aspx?EventSessionId={1}&registerId={2}", 
                ConfigReader.DefaultProvider.AccountConfiguration.BaseUrlWithHttps, 
                sessionID, 
                regID)));
        }

        public void CloseAttendeeInfo()
        {
            UIUtil.DefaultProvider.CloseWindow();
        }

        [Step]
        public void OpenAttendeeSubPage(AttendeeSubPage attendeeSubPage)
        {
            switch (attendeeSubPage)
            {
                case AttendeeSubPage.ViewAll:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("//input[@value=' View All ']", LocateBy.XPath);
                    break;
                case AttendeeSubPage.PersonalInformation:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("link_personal", LocateBy.Id);
                    break;
                case AttendeeSubPage.EventCost:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("lnkEventCost", LocateBy.Id);
                    break;
                case AttendeeSubPage.CustomFields:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("link_cf", LocateBy.Id);
                    break;
                case AttendeeSubPage.Agenda:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("link_agenda", LocateBy.Id);
                    break;
                case AttendeeSubPage.Merchandise:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("link_fees", LocateBy.Id);
                    break;
                case AttendeeSubPage.Transactions:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("link_transactions", LocateBy.Id);
                    break;
                case AttendeeSubPage.LodgingAndTravel:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("link_lodgingTravel", LocateBy.Id);
                    break;
                case AttendeeSubPage.UpdateHistory:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("link_update", LocateBy.Id);
                    break;
                case AttendeeSubPage.EmailHistory:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("link_email", LocateBy.Id);
                    break;
                default:
                    throw new ArgumentException(string.Format("No such attendee sub page: {0}", attendeeSubPage.ToString()));
            }

            Utility.ThreadSleep(0.5);
        }

        [Step]
        public int GetMembershipNumber()
        {
            return Convert.ToInt32(UIUtil.DefaultProvider.GetText("//b[text()='Membership Number:']/../following-sibling::td", LocateBy.XPath));
        }

        #region Locator helper methods
        protected override string GetLocator<TEnum>(TEnum fieldEnum, LocatorType locatorType)
        {
            string locator = string.Empty;
            string enumString = fieldEnum.ToString();
            Type enumType = fieldEnum.GetType();

            if (enumType == typeof(PersonalInfoEditField))
            {
                int enumInt = (int)Enum.Parse(typeof(PersonalInfoEditField), enumString);
                locator = GetLocator((PersonalInfoEditField)enumInt, locatorType);
            }
            else if (enumType == typeof(PersonalInfoViewField))
            {
                int enumInt = (int)Enum.Parse(typeof(PersonalInfoViewField), enumString);
                locator = GetLocator((PersonalInfoViewField)enumInt, locatorType);
            }
            else if (enumType == typeof(LodgingEditField))
            {
                int enumInt = (int)Enum.Parse(typeof(LodgingEditField), enumString);
                locator = GetLocator((LodgingEditField)enumInt, locatorType);
            }
            else if (enumType == typeof(TravelEditField))
            {
                int enumInt = (int)Enum.Parse(typeof(TravelEditField), enumString);
                locator = GetLocator((TravelEditField)enumInt, locatorType);
            }
            else
            {
                UIUtil.DefaultProvider.FailTest("Unknown enum type: " + enumString);
            }

            return locator;
        }

        protected string GetLocator(PersonalInfoViewField fieldType, LocatorType locatorType)
        {
            string locator = string.Empty;

            string baseLocator = "//div[@id='personal']//b[text()='{0}:']";
            string baseOtherContactLocator = "//div[@id='personal']//b[text()='Other Contact Info']/../../following-sibling::tr[{0}]";
            string forField = fieldType.ToString();
            string forType = string.Empty;
            InputType inputType = GetInputType(fieldType);

            switch (fieldType)
            {
                case PersonalInfoViewField.Id:
                    forField = "ID";
                    break;
                case PersonalInfoViewField.RegType:
                    forField = "Type";
                    break;
                case PersonalInfoViewField.FullName:
                    forField = "Attendee";
                    break;
                case PersonalInfoViewField.JobTitle:
                    forField = "Job Title";
                    break;
                case PersonalInfoViewField.Company:
                    forField = "Company/Organization";
                    break;
                case PersonalInfoViewField.AddressLine1:
                    forField = "Address Line 1";
                    break;
                case PersonalInfoViewField.AddressLine2:
                    forField = "Address Line 2";
                    break;
                case PersonalInfoViewField.CityStateZip:
                    forField = "City/State/ZIP";
                    break;
                case PersonalInfoViewField.WorkPhone:
                    forField = "Work Phone";
                    break;
                case PersonalInfoViewField.HomePhone:
                    forField = "Home Phone";
                    break;
                case PersonalInfoViewField.CellPhone:
                    forField = "Cell Phone";
                    break;
                case PersonalInfoViewField.OptOutDirectory:
                    forField = "Registration directory opt-out";
                    break;
                case PersonalInfoViewField.SecondaryEmail:
                    forField = "Secondary Email Address (cc Email)";
                    break;
                case PersonalInfoViewField.RegisterDate:
                    forField = "Registered";
                    break;
                case PersonalInfoViewField.TotalCredits:
                    forField = "Total Credits";
                    break;
                case PersonalInfoViewField.NameOnBadge:
                    forField = "Name as it would appear on the badge";
                    break;
                case PersonalInfoViewField.MembershipNumber:
                    forField = "Membership Number";
                    break;
                case PersonalInfoViewField.CustomerNumber:
                    forField = "Customer Number";
                    break;
                case PersonalInfoViewField.SocialSecurityNumber:
                    forField = "Social Security Number";
                    break;
                case PersonalInfoViewField.DateOfBirth:
                    forField = "Date of Birth";
                    break;
                case PersonalInfoViewField.Gender:
                    forField = "Gender";
                    break;
                case PersonalInfoViewField.EmergencyContactName:
                    forField = "Emergency Contact Name";
                    break;
                case PersonalInfoViewField.EmergencyContactPhone:
                    forField = "Emergency Contact Phone";
                    break;
                case PersonalInfoViewField.TaxId:
                    forField = "Tax Identification Number";
                    break;
                case PersonalInfoViewField.ContactName:
                    forField = "1";
                    break;
                case PersonalInfoViewField.ContactPhone:
                    forField = "2";
                    break;
                case PersonalInfoViewField.ContactEmail:
                    forField = "3";
                    break;
            }

            locator = string.Format(baseLocator, forField);

            if (locatorType == LocatorType.Edit)
            {
                locator += "/../../td[2]";
            }

            if ((fieldType == PersonalInfoViewField.ContactName) ||
                (fieldType == PersonalInfoViewField.ContactPhone) ||
                (fieldType == PersonalInfoViewField.ContactEmail))
            {
                locator = string.Format(baseOtherContactLocator, forField);
                switch (locatorType)
                {
                    case LocatorType.Edit:
                        locator += "/td[2]";
                        break;
                    case LocatorType.Label:
                        locator += "/td[1]";
                        break;
                }
            }

            return locator;
        }

        private InputType GetInputType(PersonalInfoViewField fieldType)
        {
            return InputType.Read;
        }

        protected override InputType GetInputType<TEnum>(TEnum fieldEnum)
        {
            InputType inputType = new InputType();
            string enumString = fieldEnum.ToString();
            Type enumType = fieldEnum.GetType();

            if (enumType == typeof(PersonalInfoEditField))
            {
                int enumInt = (int)Enum.Parse(typeof(PersonalInfoEditField), enumString);
                inputType = GetInputType((PersonalInfoEditField)enumInt);
            }
            else if (enumType == typeof(PersonalInfoViewField))
            {
                int enumInt = (int)Enum.Parse(typeof(PersonalInfoViewField), enumString);
                inputType = GetInputType((PersonalInfoViewField)enumInt);
            }
            else if (enumType == typeof(LodgingEditField))
            {
                int enumInt = (int)Enum.Parse(typeof(LodgingEditField), enumString);
                inputType = GetInputType((LodgingEditField)enumInt);
            }
            else if (enumType == typeof(TravelEditField))
            {
                int enumInt = (int)Enum.Parse(typeof(TravelEditField), enumString);
                inputType = GetInputType((TravelEditField)enumInt);
            }
            else
            {
                UIUtil.DefaultProvider.FailTest("Invalid enum '" + enumString + "' of type '" + enumType.ToString() + "'!");
            }

            return inputType;
        }

        private InputType GetInputType(PersonalInfoEditField fieldType)
        {
            InputType inputType = new InputType();

            switch (fieldType)
            {
                case PersonalInfoEditField.Status:
                case PersonalInfoEditField.RegType:
                case PersonalInfoEditField.Country:
                case PersonalInfoEditField.StateUSCanada:
                    inputType = InputType.Dropdown;
                    break;

                case PersonalInfoEditField.Prefix:
                case PersonalInfoEditField.First_Name:
                case PersonalInfoEditField.Middle_Name:
                case PersonalInfoEditField.Last_Name:
                case PersonalInfoEditField.Suffix:
                case PersonalInfoEditField.Title:
                case PersonalInfoEditField.Company:
                case PersonalInfoEditField.Address_1:
                case PersonalInfoEditField.Address_2:
                case PersonalInfoEditField.City:
                case PersonalInfoEditField.StateNonUS:
                case PersonalInfoEditField.Postal_Code:
                case PersonalInfoEditField.HomePhone:
                case PersonalInfoEditField.WorkPhone:
                case PersonalInfoEditField.Extension:
                case PersonalInfoEditField.Fax:
                case PersonalInfoEditField.CellPhone:
                case PersonalInfoEditField.Email:
                case PersonalInfoEditField.SecondaryEmail:
                case PersonalInfoEditField.BadgeName:
                case PersonalInfoEditField.MembershipNumber:
                case PersonalInfoEditField.CustomerNumber:
                case PersonalInfoEditField.SocialSecurityNumber:
                case PersonalInfoEditField.EmergencyContactName:
                case PersonalInfoEditField.EmergencyContactPhone:
                case PersonalInfoEditField.TaxId:
                case PersonalInfoEditField.ContactName:
                case PersonalInfoEditField.ContactPhone:
                case PersonalInfoEditField.ContactEmail:
                case PersonalInfoEditField.Notes:
                case PersonalInfoEditField.DateOfBirth_Year:
                case PersonalInfoEditField.DateOfBirth_Month:
                case PersonalInfoEditField.DateOfBirth_Day:
                    inputType = InputType.Textbox;
                    break;

                case PersonalInfoEditField.OptOutDirectory:
                    inputType = InputType.Checkbox;
                    break;

                default:
                    inputType = InputType.Other;
                    break;
            }

            return inputType;
        }

        protected string GetLocator(PersonalInfoEditField fieldType, LocatorType locatorType)
        {
            string locator = string.Empty;

            string baseLocator = "//{0}[@name='{1}']";
            string forField = fieldType.ToString();
            string forType = string.Empty;
            InputType inputType = GetInputType(fieldType);

            switch (fieldType)
            {
                case PersonalInfoEditField.Status:
                    forField = "StatusId";
                    break;
                case PersonalInfoEditField.RegType:
                    forField = "RegTypeId";
                    break;
                case PersonalInfoEditField.StateUSCanada:
                    forField = "Region";
                    break;
                case PersonalInfoEditField.StateNonUS:
                    forField = "Address_3";
                    break;
                case PersonalInfoEditField.WorkPhone:
                    forField = "Phone";
                    break;
                case PersonalInfoEditField.Email:
                    forField = "EMail_Address";
                    break;
                case PersonalInfoEditField.OptOutDirectory:
                    forField = "directoryOptOut";
                    break;
                case PersonalInfoEditField.SecondaryEmail:
                    forField = "ccEmail";
                    break;
                case PersonalInfoEditField.MembershipNumber:
                    forField = "MembershipId";
                    break;
                case PersonalInfoEditField.CustomerNumber:
                    forField = "CustId";
                    break;
                case PersonalInfoEditField.SocialSecurityNumber:
                    forField = "SSNId";
                    break;
                case PersonalInfoEditField.TaxId:
                    forField = "VATNumber";
                    break;
                case PersonalInfoEditField.ContactName:
                    forField = "contactname";
                    break;
                case PersonalInfoEditField.ContactPhone:
                    forField = "contactPhone";
                    break;
                case PersonalInfoEditField.ContactEmail:
                    forField = "contactEmail";
                    break;
                //case PersonalInfoEditField.DateOfBirth_Year:

                //    break;
                //case PersonalInfoEditField.DateOfBirth_Month:
                //    break;
                //case PersonalInfoEditField.DateOfBirth_Day:
                //    break;
            }

            switch (locatorType)
            {
                case LocatorType.Edit:
                    // Dropdowns are different!
                    if (inputType == InputType.Dropdown)
                        forType = "select";
                    else
                        forType = "input";
                    break;
                case LocatorType.Label:
                    // Checkbox labels are in different place
                    if (inputType == InputType.Checkbox)
                        forType = "/../../td[2]";
                    else
                        forType = "/../../td[1]";
                    break;
            }

            locator = string.Format(baseLocator, forType, forField);

            return locator;
        }

        protected string GetLocator(LodgingViewField field)
        {
            string locator = string.Empty;

            switch (field)
            {
                case LodgingViewField.BookingStatus:
                    locator = LodgingBookingStatus;
                    break;

                case LodgingViewField.ConfirmationCode:
                    locator = LodgingConfirmationNumber;
                    break;

                case LodgingViewField.HotelPrimary:
                    locator = LodgingHotelPreference1;
                    break;

                case LodgingViewField.HotelSecondary:
                    locator = LodgingHotelPreference2;
                    break;

                case LodgingViewField.ArrivalDate:
                    locator = LodgingArrivalDate;
                    break;

                case LodgingViewField.DepartureDate:
                    locator = LodgingDepartureDate;
                    break;

                case LodgingViewField.RoomPreference:
                    locator = LodgingRoomPreference;
                    break;

                case LodgingViewField.BedPreference:
                    locator = LodgingBedPreference;
                    break;

                case LodgingViewField.SmokingPreference:
                    locator = LodgingSmokingPreference;
                    break;

                case LodgingViewField.ShareWith:
                    locator = LodgingSharePreference;
                    break;

                case LodgingViewField.RoomSharerId:
                    locator = LodgingSystemAssignedSharer;
                    break;

                case LodgingViewField.CreditCardNumber:
                    locator = LodgingCreditCardNumber;
                    break;

                case LodgingViewField.AdjoinWith:
                    locator = LodgingAdjoining;
                    break;

                case LodgingViewField.AdditionalInfo:
                    locator = LodgingAdditionalInfo;
                    break;

                default:
                    break;
            }

            return locator;
        }

        protected string GetLocator(TravelViewField field)
        {
            string locator = string.Empty;

            switch (field)
            {
                case TravelViewField.BookingStatus:
                    locator = TravelBookingStatus;
                    break;
                case TravelViewField.ConfirmationCode:
                    locator = TravelConfirmationNumber;
                    break;
                case TravelViewField.ArrivalAirline:
                    locator = TravelArrivalAirline;
                    break;
                case TravelViewField.ArrivalCity:
                    locator = TravelArrivalCity;
                    break;
                case TravelViewField.ArrivalAirport:
                    locator = TravelArrivalAirport;
                    break;
                case TravelViewField.ArrivalDateTime:
                    locator = TravelArrivalDateTime;
                    break;
                case TravelViewField.ArrivalConnection:
                    locator = TravelArrivalConnection;
                    break;
                case TravelViewField.DepartureCity:
                    locator = TravelDepartureCity;
                    break;
                case TravelViewField.DepartureAirport:
                    locator = TravelDepartureAirport;
                    break;
                case TravelViewField.DepartureDateTime:
                    locator = TravelDepartureDateTime;
                    break;
                case TravelViewField.DepartureAirline:
                    locator = TravelDepartureAirline;
                    break;
                case TravelViewField.DepartureConnection:
                    locator = TravelDepartureConnection;
                    break;
                case TravelViewField.SeatingPreference:
                    locator = TravelSeatingPreference;
                    break;
                case TravelViewField.FrequentFlyerNumber:
                    locator = TravelFrequentFlyerNumber;
                    break;
                case TravelViewField.PassportNumber:
                    locator = TravelPassportNumber;
                    break;
                case TravelViewField.CreditCardNumber:
                    locator = TravelCreditCardNumber;
                    break;
                case TravelViewField.CreditCardExpiration:
                    locator = TravelCreditCardExpiration;
                    break;
                case TravelViewField.CreditCardHolderName:
                    locator = TravelCreditCardHolder;
                    break;
                case TravelViewField.CreditCardInfo:
                    locator = TravelAdditionalInfo;
                    break;
                case TravelViewField.GroundTransportation:
                    locator = TravelGroundTransportationPreference;
                    break;
                case TravelViewField.GroundTransportationInfo:
                    locator = TravelOtherInformation;
                    break;
                default:
                    break;
            }

            return locator;
        }

        protected string GetLocator(LodgingEditField field, LocatorType locatorType)
        {
            string locator = string.Empty;
            string locatorFormat = "//{0}[@name='{1}']";
            string name = field.ToString();
            string fieldType = "*";
            InputType inputType = GetInputType(field);

            switch (field)
            {
                case LodgingEditField.BookingStatus:
                    name = "Ldg_Booking_Status_Id";
                    break;

                case LodgingEditField.BookingAgent:
                    name = "Ldg_Booking_Agent_Id";
                    break;

                case LodgingEditField.ConfirmationCode:
                    name = "LdgConfirmNumber";
                    break;

                case LodgingEditField.HotelPrimary:
                    name = "LdgLocation1";
                    break;

                case LodgingEditField.HotelSecondary:
                    name = "LdgLocation2";
                    break;

                case LodgingEditField.ArrivalDate:
                    UIUtil.DefaultProvider.FailTest("Please call individual method to type lodging arrival date field!");
                    break;

                case LodgingEditField.ArrivalTime:
                    name = "LdgArrTime";
                    break;

                case LodgingEditField.DepartureDate:
                    UIUtil.DefaultProvider.FailTest("Please call individual method to type lodging departure date field!");
                    break;

                case LodgingEditField.DepartureTime:
                    name = "LdgDptTime";
                    break;

                case LodgingEditField.RoomPreference:
                    name = "Ldg_Room_Type_Id";
                    break;

                case LodgingEditField.BedPreference:
                    name = "Ldg_Bed_Type_Id";
                    break;

                case LodgingEditField.SmokingPreference:
                    name = "Ldg_Smoking_Pref_Id";
                    break;

                case LodgingEditField.ShareWith:
                    name = "LdgSharing";
                    break;

                case LodgingEditField.RoomSharerId:
                    name = "roomSharerID";
                    break;

                case LodgingEditField.AdjoinWith:
                    name = "LdgAdjoining";
                    break;

                case LodgingEditField.BookingFee:
                    name = "BookingFee";
                    break;

                case LodgingEditField.AdditionalInfo:
                    name = "LdgOtherInfo";
                    break;

                default:
                    UIUtil.DefaultProvider.FailTest("Invalid enum for '" + field.ToString() + "' of 'LodgingEditField'!");
                    break;
            }

            switch (locatorType)
            {
                case LocatorType.Edit:
                    // Dropdowns are different!
                    if (inputType == InputType.Dropdown)
                    {
                        fieldType = "select";
                    }
                    else if (inputType == InputType.Textbox)
                    {
                        fieldType = "input";
                    }
                    else if (inputType == InputType.Textarea)
                    {
                        fieldType = "textarea";
                    }

                    break;

                //    case LocatorType.Label:
                //        // Checkbox labels are in different place
                //        if (inputType == InputType.Checkbox)
                //            forType = "/../../td[2]";
                //        else
                //            forType = "/../../td[1]";
                //        break;
            }

            locator = string.Format(locatorFormat, fieldType, name);

            return locator;
        }

        protected string GetLocator(TravelEditField field, LocatorType locatorType)
        {
            string locator = string.Empty;
            string locatorFormat = "//{0}[@name='{1}']";
            string name = field.ToString();
            string fieldType = "*";
            InputType inputType = GetInputType(field);

            switch (field)
            {
                case TravelEditField.BookingStatus:
                    name = "Trv_Booking_Status_Id";
                    break;

                case TravelEditField.BookingAgent:
                    name = "Trv_Booking_Agent_Id";
                    break;

                case TravelEditField.ConfirmationCode:
                    name = "TrvConfirmNumber";
                    break;

                case TravelEditField.ArrivalCity:
                    name = "TrvArrAirportCity";
                    break;

                case TravelEditField.ArrivalAirport:
                    name = "TrvArrAirport";
                    break;

                case TravelEditField.ArrivalAirline:
                    name = "TrvArrAirline";
                    break;

                case TravelEditField.ArrivalConnection:
                    name = "TrvArrConnectInfo";
                    break;

                case TravelEditField.ArrivalDate:
                    UIUtil.DefaultProvider.FailTest("Please call individual method to type travel arrival date field!");
                    break;

                case TravelEditField.ArrivalTime:
                    name = "TrvArrTime";
                    break;

                case TravelEditField.DepartureCity:
                    name = "TrvDptAirportCity";
                    break;

                case TravelEditField.DepartureAirport:
                    name = "TrvDptAirport";
                    break;

                case TravelEditField.DepartureAirline:
                    name = "TrvDptAirline";
                    break;

                case TravelEditField.DepartureConnection:
                    name = "TrvDptConnectInfo";
                    break;

                case TravelEditField.DepartureDate:
                    UIUtil.DefaultProvider.FailTest("Please call individual method to type travel departure date field!");
                    break;

                case TravelEditField.DepartureTime:
                    name = "TrvDptTime";
                    break;

                case TravelEditField.SeatingPreference:
                    name = "Trv_Seating_Pref_Id";
                    break;

                case TravelEditField.FrequentFlyerNumber:
                    name = "Trv_Primary_FF_No";
                    break;

                case TravelEditField.PassportNumber:
                    name = "trv_passport_no";
                    break;

                case TravelEditField.CreditCardNumber:
                    name = "TrvccNumber";
                    break;

                case TravelEditField.CreditCardExpiration:
                    UIUtil.DefaultProvider.FailTest("Please call the method 'SelectTravelCCExpirationDate()'!");
                    break;

                case TravelEditField.CreditCardHolderName:
                    name = "TrvccName";
                    break;

                case TravelEditField.CreditCardInfo:
                    name = "TrvOtherInfo";
                    break;

                case TravelEditField.GroundTransportation:
                    name = "Grd_Type_Id";
                    break;

                case TravelEditField.GroundTransportationInfo:
                    name = "Grd_Other_Info";
                    break;

                default:
                    UIUtil.DefaultProvider.FailTest("Invalid enum for '" + field.ToString() + "' of 'TravelEditField'!");
                    break;
            }

            switch (locatorType)
            {
                case LocatorType.Edit:
                    // Dropdowns are different!
                    if (inputType == InputType.Dropdown)
                    {
                        fieldType = "select";
                    }
                    else if (inputType == InputType.Textbox)
                    {
                        fieldType = "input";
                    }
                    else if (inputType == InputType.Textarea)
                    {
                        fieldType = "textarea";
                    }

                    break;

                //    case LocatorType.Label:
                //        // Checkbox labels are in different place
                //        if (inputType == InputType.Checkbox)
                //            forType = "/../../td[2]";
                //        else
                //            forType = "/../../td[1]";
                //        break;
            }

            locator = string.Format(locatorFormat, fieldType, name);

            return locator;
        }

        private InputType GetInputType(LodgingEditField field)
        {
            InputType inputType = new InputType();

            switch (field)
            {
                case LodgingEditField.BookingStatus:
                case LodgingEditField.BookingAgent:
                case LodgingEditField.HotelPrimary:
                case LodgingEditField.HotelSecondary:
                case LodgingEditField.RoomPreference:
                case LodgingEditField.BedPreference:
                case LodgingEditField.SmokingPreference:
                    inputType = InputType.Dropdown;
                    break;

                case LodgingEditField.ConfirmationCode:
                case LodgingEditField.ShareWith:
                case LodgingEditField.RoomSharerId:
                case LodgingEditField.AdjoinWith:
                case LodgingEditField.BookingFee:
                case LodgingEditField.DepartureTime:
                case LodgingEditField.ArrivalTime:
                    inputType = InputType.Textbox;
                    break;

                case LodgingEditField.AdditionalInfo:
                    inputType = InputType.Textarea;
                    break;

                case LodgingEditField.ArrivalDate:
                case LodgingEditField.DepartureDate:
                    inputType = InputType.Other;
                    break;

                default:
                    break;
            }

            return inputType;
        }

        private InputType GetInputType(TravelEditField field)
        {
            InputType inputType = new InputType();

            switch (field)
            {
                case TravelEditField.BookingStatus:
                case TravelEditField.BookingAgent:
                case TravelEditField.SeatingPreference:
                case TravelEditField.GroundTransportation:
                    inputType = InputType.Dropdown;
                    break;

                case TravelEditField.ConfirmationCode:
                case TravelEditField.DepartureCity:
                case TravelEditField.DepartureAirport:
                case TravelEditField.DepartureAirline:
                case TravelEditField.ArrivalCity:
                case TravelEditField.ArrivalAirport:
                case TravelEditField.ArrivalAirline:
                case TravelEditField.FrequentFlyerNumber:
                case TravelEditField.PassportNumber:
                case TravelEditField.CreditCardNumber:
                case TravelEditField.CreditCardHolderName:
                case TravelEditField.ArrivalTime:
                case TravelEditField.DepartureTime:
                    inputType = InputType.Textbox;
                    break;

                case TravelEditField.ArrivalConnection:
                case TravelEditField.DepartureConnection:
                case TravelEditField.CreditCardInfo:
                case TravelEditField.GroundTransportationInfo:
                    inputType = InputType.Textarea;
                    break;

                case TravelEditField.ArrivalDate:
                case TravelEditField.DepartureDate:
                case TravelEditField.CreditCardExpiration:
                    inputType = InputType.Other;
                    break;

                default:
                    break;
            }

            return inputType;
        }
        #endregion
    }
}
