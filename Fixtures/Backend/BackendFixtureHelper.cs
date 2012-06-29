namespace RegOnline.RegressionTest.Fixtures.Backend
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    internal class BackendFixtureHelper : FixtureBase
    {
        /// <summary>
        /// This class contains all the stuff for AttendeeInfoRegressionChecklist
        /// </summary>
        public class AttendeeInfoEvent
        {
            public const string EventName = "Attendee info test extravaganza";
            public const double EventFee = 1;
            public const double AgendaCheckboxFee = 1;
            public const double AgendaAlwaysSelectedFee = 1;
            public const double MerchandiseFixedPrice = 1;

            public const string CFParagraphText = "Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah"
                + " Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah";

            public const string AgendaParagraphText = "This is a paragraph field!!! YAY!!!"
                + " This is a paragraph field!!! YAY!!! This is a paragraph field!!! YAY!!!"
                + " This is a paragraph field!!! YAY!!! This is a paragraph field!!! YAY!!!"
                + " This is a paragraph field!!! YAY!!!";

            public const string RegTypeFeeSuffix = "_Event_Fee";

            public enum RegType
            {
                [StringValue("reg type one")]
                One,

                [StringValue("reg type two")]
                Two
            }

            public enum CustomField
            {
                [StringValue("Personal information checkbox")]
                Checkbox,

                [StringValue("Personal info radio buttons")]
                RadioButton,

                [StringValue("Personal info drop down")]
                Dropdown,

                [StringValue("Personal info number")]
                Number,

                [StringValue("Personal info 1 line text")]
                OneLineText,

                [StringValue("Personal info time")]
                Time,

                [StringValue("Personal info paragraph")]
                Paragraph,

                [StringValue("Personal Info date")]
                Date,

                [StringValue("Personal info always selected")]
                AlwaysSelected
            }

            public enum AgendaItem
            {
                [StringValue("Agenda checkbox no fee")]
                CheckboxWithoutFee,

                [StringValue("Agenda checkbox w/fee")]
                CheckboxWithFee,

                [StringValue("Agenda Radio Buttons no fee")]
                RadioButtonsWithoutFee,

                [StringValue("Agenda Radio Buttons w/fee")]
                RadioButtonsWithFee,

                [StringValue("Agenda dropdown no fee")]
                DropdownWithoutFee,

                [StringValue("Agenda dropdown with fee")]
                DropdownWithFee,

                [StringValue("Agenda number")]
                Number,

                [StringValue("Agenda Time")]
                Time,

                [StringValue("Agenda File Upload")]
                Upload,

                [StringValue("Agenda paragraph")]
                Paragraph,

                [StringValue("Agenda Date")]
                Date,

                [StringValue("Agenda Contribution")]
                Contribution,

                [StringValue("Agenda Always Selected no fee")]
                AlwaysSelectedWithoutFee,

                [StringValue("Agenda always selected w/fee")]
                AlwaysSelectedWithFee
            }

            public enum MerchandiseItem
            {
                [StringValue("Fixed price")]
                FixedPrice,

                [StringValue("Fixed price w/MC items")]
                FixedPriceWithMCItems,

                [StringValue("Variable amount")]
                VariableAmount,

                [StringValue("Variable amount w/MC items")]
                VariableAmountWithMCItems,
            }

            public Dictionary<AttendeeInfoEvent.RegType, double> regTypeFee;
            public Dictionary<AttendeeInfoEvent.CustomField, int> customFieldsIds;
            public Dictionary<AttendeeInfoEvent.AgendaItem, int> agendaItemsIds;
            public Dictionary<AttendeeInfoEvent.MerchandiseItem, int> merchandiseItemsIds;

            public AttendeeInfoEvent()
            {
                this.regTypeFee = new Dictionary<AttendeeInfoEvent.RegType, double>();
                this.regTypeFee.Add(AttendeeInfoEvent.RegType.One, 1);
                this.regTypeFee.Add(AttendeeInfoEvent.RegType.Two, 2);

                this.customFieldsIds = new Dictionary<AttendeeInfoEvent.CustomField, int>();
                this.agendaItemsIds = new Dictionary<AttendeeInfoEvent.AgendaItem, int>();
                this.merchandiseItemsIds = new Dictionary<AttendeeInfoEvent.MerchandiseItem, int>();
            }
        }

        public class PersonalInfoFields
        {
            private string _email;
            public string Email
            {
                get
                {
                    if (string.IsNullOrEmpty(this._email))
                    {
                        this._email = string.Format("this{0}@isatest.com", DateTime.Now.Ticks.ToString());
                    }

                    return this._email;
                }
            }

            public string Status;
            public string RegType;
            public string Prefix;
            public string FirstName;
            public string MiddleName;
            public string LastName;
            public string Suffix;

            public string FullName
            {
                get
                {
                    return string.Format("{0}{1}{2}{3}{4}(Test)",
                        (string.IsNullOrEmpty(this.Prefix) ? string.Empty : string.Format("{0} ", this.Prefix)),
                        (string.IsNullOrEmpty(this.FirstName) ? string.Empty : string.Format("{0} ", this.FirstName)),
                        (string.IsNullOrEmpty(this.MiddleName) ? string.Empty : string.Format("{0} ", this.MiddleName)),
                        (string.IsNullOrEmpty(this.LastName) ? string.Empty : string.Format("{0} ", this.LastName)),
                        (string.IsNullOrEmpty(this.Suffix) ? string.Empty : string.Format("{0} ", this.Suffix)));
                }
            }

            public string JobTitle;
            public string Company;
            public string Country;
            public string AddressLineOne;
            public string AddressLineTwo;
            public string City;
            public string State;
            public string StateShortForm;
            public string ZipCode;

            public string CityStateZip
            {
                get
                {
                    return string.Format(
                        "{0}{1}{2}",
                        (string.IsNullOrEmpty(this.City) ? string.Empty : string.Format("{0}, ", this.City)),
                        (string.IsNullOrEmpty(this.StateShortForm) ? string.Empty : string.Format("{0} ", this.StateShortForm)),
                        (string.IsNullOrEmpty(this.ZipCode) ? string.Empty : string.Format("{0}", this.ZipCode)));
                }
            }

            public string HomePhone;
            public string WorkPhone;
            public string Extension;
            public string Fax;
            public string CellPhone;
            public string BadgeName;
            public string MembershipNumber;
            public string CustomerNumber;
            public string SecondaryEmail;
            public string SocialSecurityNumber;
            public string DateOfBirth_Month;
            public string DateOfBirth_Day;
            public string DateOfBirth_Year;

            public string DateOfBirth
            {
                get
                {
                    if (string.IsNullOrEmpty(this.DateOfBirth_Year) ||
                        string.IsNullOrEmpty(this.DateOfBirth_Month) ||
                        string.IsNullOrEmpty(this.DateOfBirth_Day))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        DateTime birthday = new DateTime(
                            Convert.ToInt32(this.DateOfBirth_Year),
                            Convert.ToInt32(this.DateOfBirth_Month),
                            Convert.ToInt32(this.DateOfBirth_Day));

                        return string.Format("{0}({1})", birthday.ToString("D"), DateTime.Now.Year - birthday.Year);
                    }
                }
            }

            public string EmergencyContactName;
            public string EmergencyContactPhone;
            public string ContactName;
            public string ContactPhone;
            public string ContactEmail;
            public string OptOutDirectory;

            public static PersonalInfoFields Default = PersonalInfoFields.CreateDefaultPersonalInfoFields();

            public static PersonalInfoFields CreateDefaultPersonalInfoFields()
            {
                return new PersonalInfoFields
                {
                    Status = "Confirmed",
                    RegType = StringEnum.GetStringValue(AttendeeInfoEvent.RegType.One),
                    Prefix = "Dr.",
                    FirstName = "Test",
                    MiddleName = "T",
                    LastName = "McTester",
                    Suffix = "Esq.",
                    JobTitle = "QA Tester",
                    Company = "RegOnline",
                    Country = "United States",
                    AddressLineOne = "4750 Walnut st.",
                    AddressLineTwo = "Suite 100",
                    City = "Boulder",
                    State = "Colorado",
                    StateShortForm = "CO",
                    ZipCode = "80301",
                    HomePhone = "3035555555",
                    WorkPhone = "3035775100",
                    Extension = "5188",
                    Fax = "3035775101",
                    CellPhone = "3035775556",
                    BadgeName = "Big T",
                    MembershipNumber = "123456789",
                    CustomerNumber = "321654987",
                    SecondaryEmail = "TEst@auto.test",
                    SocialSecurityNumber = "555555555",
                    DateOfBirth_Month = "02",
                    DateOfBirth_Day = "18",
                    DateOfBirth_Year = "1986",
                    EmergencyContactName = "My Mom",
                    EmergencyContactPhone = "1-800-The-force",
                    ContactName = "Mrs. McTester",
                    ContactPhone = "3335556666",
                    ContactEmail = "test@automatic.xx",
                    OptOutDirectory = "No"
                };
            }
        }

        [Step]
        public void EnterPersonalInfoDuringRegistration(string firstName, string middleName, string lastName)
        {
            RegisterMgr.EnterPersonalInfoNamePrefixSuffix(null, firstName, middleName, lastName, null);

            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry(
                PersonalInfoFields.Default.JobTitle,
                null,
                PersonalInfoFields.Default.Company,
                null);

            RegisterMgr.EnterPersonalInfoAddress(
                PersonalInfoFields.Default.AddressLineOne,
                null,
                PersonalInfoFields.Default.City,
                PersonalInfoFields.Default.State,
                null,
                PersonalInfoFields.Default.ZipCode);

            RegisterMgr.EnterPersonalInfoPhoneNumbers(null, PersonalInfoFields.Default.WorkPhone, null, null, null);

            if (RegisterMgr.HasPasswordTextbox())
            {
                RegisterMgr.EnterPersonalInfoPassword("321321");
            }
        }

        [Step]
        public void EnterPersonalInfoDuringRegistration()
        {
            this.EnterPersonalInfoDuringRegistration(
                PersonalInfoFields.Default.FirstName,
                PersonalInfoFields.Default.MiddleName,
                PersonalInfoFields.Default.LastName);
        }

        [Step]
        public string LoginAndGetEventSessionId()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            return ManagerSiteMgr.GetEventSessionId();
        }
    }
}
