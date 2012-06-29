namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Linq;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class FormDetailManager : ManagerBase
    {
        protected const string PIPageAddCFLinkLocatorPrefix = "ctl00_cph_grdCustomFieldPersonal_";

        // 'Verify Email' and 'Opt out' has no required option
        private struct PersonalInfoFieldLocator
        {
            public const string EmailVisibleOptionLocator = "ctl00_cph_chkcEmail";
            public const string EmailRequiredOptionLocator = "ctl00_cph_chkrEmail";
            public const string VerifyEmailVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscVerifyEmail";
            public const string PrefixVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscPrefix";
            public const string PrefixRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrPrefix";
            public const string FirstNameVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscFirstName";
            public const string FirstNameRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrFirstName";
            public const string MiddleNameVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscMiddleName";
            public const string MiddleNameRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrMiddleName";
            public const string LastNameVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscLastName";
            public const string LastNameRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrLastName";
            public const string SuffixVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscSuffix";
            public const string SuffixRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrSuffix";
            public const string JobTitleVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscTitle";
            public const string JobTitleRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrTitle";
            public const string NameOnBadgeVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscBadgeName";
            public const string NameOnBadgeRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrBadgeName";
            public const string CompanyVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscCompany";
            public const string CompanyRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrCompany";
            public const string CountryVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscCountry";
            public const string CountryRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrCountry";
            public const string AddressLineOneVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscAddress1";
            public const string AddressLineOneRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrAddress1";
            public const string AddressLineTwoVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscAddress2";
            public const string AddressLineTwoRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrAddress2";
            public const string CityVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscCity";
            public const string CityRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrCity";
            public const string StateVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscState";
            public const string StateRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrState";
            public const string NonUSStateVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscAddress3";
            public const string NonUSStateRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrAddress3";
            public const string ZipCodeVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscPostalCode";
            public const string ZipCodeRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrPostalCode";
            public const string HomePhoneVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscHomePhone";
            public const string HomePhoneRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrHomePhone";
            public const string WorkPhoneVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscWorkPhone";
            public const string WorkPhoneRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrWorkPhone";
            public const string ExtensionVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscExtension";
            public const string ExtensionRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrExtension";
            public const string FaxVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscFax";
            public const string FaxRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrFax";
            public const string CellVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscCellPhone";
            public const string CellRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrCellPhone";
            public const string DateOfBirthVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscDateOfBirth";
            public const string DateOfBirthRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrDateOfBirth";
            public const string GenderVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscGender";
            public const string GenderRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrGender";
            public const string EmergencyContactNameVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscEmergencyContactName";
            public const string EmergencyContactNameRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrEmergencyContactName";
            public const string EmergencyContactPhoneVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscEmergencyContactPhone";
            public const string EmergencyContactPhoneRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrEmergencyContactPhone";
            public const string SecondaryEmailAddressVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscCCEmail";
            public const string SecondaryEmailAddressRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrCCEmail";
            public const string UploadPhotoVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscPhoto";
            public const string UploadPhotoRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrPhoto";
            public const string MembershipNumberVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscMembershipID";
            public const string MembershipNumberRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrMembershipID";
            public const string CustomerNumberVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscCustID";
            public const string CustomerNumberRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrCustID";
            public const string SocialSecurityNumberVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscSSNID";
            public const string SocialSecurityNumberRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrSSNID";
            public const string TaxIdentificationNumberVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscTaxIdentificationNumber";
            public const string TaxIdentificationNumberRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrTaxIdentificationNumber";
            public const string OptOutVisibleOptionLocator = "ctl00_cph_chkEventsAllowDirectoryOptOut";
            public const string ContactInfoVisibleOptionLocator = "ctl00_cph_chkCollectionFieldscContactInfo";
            public const string ContactInfoRequiredOptionLocator = "ctl00_cph_chkCollectionFieldsrContactInfo";
        }

        public enum PersonalInfoField
        {
            [StringValue("Email")]
            Email,

            [StringValue("Verify Email")]
            VerifyEmail,

            [StringValue("Prefix (Mr., Mrs., etc.)")]
            Prefix,

            [StringValue("First Name")]
            FirstName,

            [StringValue("Middle Name")]
            MiddleName,

            [StringValue("Last Name")]
            LastName,

            [StringValue("Suffix")]
            Suffix,

            [StringValue("Job Title")]
            JobTitle,

            [StringValue("Name as it would appear on the badge")]
            NameOnBadge,

            [StringValue("Company/Organization")]
            Company,

            [StringValue("Country")]
            Country,

            [StringValue("Address Line 1")]
            AddressLineOne,

            [StringValue("Address Line 2")]
            AddressLineTwo,

            [StringValue("City")]
            City,

            [StringValue("US State/Canadian Province")]
            State,

            [StringValue("State/Province/Region (Non US/Canada)")]
            NonUSState,

            [StringValue("Zip (Postal Code)")]
            ZipCode,

            [StringValue("Home Phone")]
            HomePhone,

            [StringValue("Work Phone")]
            WorkPhone,

            [StringValue("Extension")]
            Extension,

            [StringValue("Fax")]
            Fax,

            [StringValue("Cell Phone")]
            Cell,

            [StringValue("Date of Birth")]
            DateOfBirth,

            [StringValue("Gender")]
            Gender,

            [StringValue("Emergency Contact Name")]
            EmergencyContactName,

            [StringValue("Emergency Contact Phone")]
            EmergencyContactPhone,

            [StringValue("Secondary Email Address (cc Email)")]
            SecondaryEmailAddress,

            [StringValue("Upload Photo")]
            UploadPhoto,

            [StringValue("Membership Number")]
            MembershipNumber,

            [StringValue("Customer Number")]
            CustomerNumber,

            [StringValue("Social Security Number")]
            SocialSecurityNumber,

            [StringValue("Tax Identification Number")]
            TaxIdentificationNumber,

            [StringValue("opt out")]
            OptOut,

            [StringValue("Contact Info")]
            ContactInfo
        }

        private Dictionary<PersonalInfoField, string> personalInfoFieldVisibleOptionLocator;
        private Dictionary<PersonalInfoField, string> personalInfoFieldRequiredOptionLocator;

        private void InitializePersonalInfoFieldVisibleOptionLocatorDictionary()
        {
            this.personalInfoFieldVisibleOptionLocator = new Dictionary<PersonalInfoField, string>();
            this.personalInfoFieldRequiredOptionLocator = new Dictionary<PersonalInfoField, string>();

            // Initialize visible option locator
            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.Email, 
                PersonalInfoFieldLocator.EmailVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.VerifyEmail, 
                PersonalInfoFieldLocator.VerifyEmailVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.Prefix,
                PersonalInfoFieldLocator.PrefixVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.FirstName,
                PersonalInfoFieldLocator.FirstNameVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.MiddleName,
                PersonalInfoFieldLocator.MiddleNameVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.LastName,
                PersonalInfoFieldLocator.LastNameVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.Suffix,
                PersonalInfoFieldLocator.SuffixVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.JobTitle,
                PersonalInfoFieldLocator.JobTitleVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.NameOnBadge,
                PersonalInfoFieldLocator.NameOnBadgeVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.Company,
                PersonalInfoFieldLocator.CompanyVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.Country,
                PersonalInfoFieldLocator.CountryVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.AddressLineOne,
                PersonalInfoFieldLocator.AddressLineOneVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.AddressLineTwo,
                PersonalInfoFieldLocator.AddressLineTwoVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.City,
                PersonalInfoFieldLocator.CityVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.State,
                PersonalInfoFieldLocator.StateVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.NonUSState,
                PersonalInfoFieldLocator.NonUSStateVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.ZipCode,
                PersonalInfoFieldLocator.ZipCodeVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.HomePhone,
                PersonalInfoFieldLocator.HomePhoneVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.WorkPhone,
                PersonalInfoFieldLocator.WorkPhoneVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.Extension,
                PersonalInfoFieldLocator.ExtensionVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.Fax,
                PersonalInfoFieldLocator.FaxVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.Cell,
                PersonalInfoFieldLocator.CellVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.DateOfBirth,
                PersonalInfoFieldLocator.DateOfBirthVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.Gender,
                PersonalInfoFieldLocator.GenderVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.EmergencyContactName,
                PersonalInfoFieldLocator.EmergencyContactNameVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.EmergencyContactPhone,
                PersonalInfoFieldLocator.EmergencyContactPhoneVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.SecondaryEmailAddress,
                PersonalInfoFieldLocator.SecondaryEmailAddressVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.UploadPhoto,
                PersonalInfoFieldLocator.UploadPhotoVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.MembershipNumber,
                PersonalInfoFieldLocator.MembershipNumberVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.CustomerNumber,
                PersonalInfoFieldLocator.CustomerNumberVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.SocialSecurityNumber,
                PersonalInfoFieldLocator.SocialSecurityNumberVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.TaxIdentificationNumber,
                PersonalInfoFieldLocator.TaxIdentificationNumberVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.OptOut,
                PersonalInfoFieldLocator.OptOutVisibleOptionLocator);

            this.personalInfoFieldVisibleOptionLocator.Add(
                PersonalInfoField.ContactInfo,
                PersonalInfoFieldLocator.ContactInfoVisibleOptionLocator);

            // Initialize required option locator
            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.Email,
                PersonalInfoFieldLocator.EmailRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.VerifyEmail,
                null);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.Prefix,
                PersonalInfoFieldLocator.PrefixRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.FirstName,
                PersonalInfoFieldLocator.FirstNameRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.MiddleName,
                PersonalInfoFieldLocator.MiddleNameRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.LastName,
                PersonalInfoFieldLocator.LastNameRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.Suffix,
                PersonalInfoFieldLocator.SuffixRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.JobTitle,
                PersonalInfoFieldLocator.JobTitleRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.NameOnBadge,
                PersonalInfoFieldLocator.NameOnBadgeRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.Company,
                PersonalInfoFieldLocator.CompanyRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.Country,
                PersonalInfoFieldLocator.CountryRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.AddressLineOne,
                PersonalInfoFieldLocator.AddressLineOneRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.AddressLineTwo,
                PersonalInfoFieldLocator.AddressLineTwoRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.City,
                PersonalInfoFieldLocator.CityRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.State,
                PersonalInfoFieldLocator.StateRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.NonUSState,
                PersonalInfoFieldLocator.NonUSStateRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.ZipCode,
                PersonalInfoFieldLocator.ZipCodeRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.HomePhone,
                PersonalInfoFieldLocator.HomePhoneRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.WorkPhone,
                PersonalInfoFieldLocator.WorkPhoneRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.Extension,
                PersonalInfoFieldLocator.ExtensionRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.Fax,
                PersonalInfoFieldLocator.FaxRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.Cell,
                PersonalInfoFieldLocator.CellRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.DateOfBirth,
                PersonalInfoFieldLocator.DateOfBirthRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.Gender,
                PersonalInfoFieldLocator.GenderRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.EmergencyContactName,
                PersonalInfoFieldLocator.EmergencyContactNameRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.EmergencyContactPhone,
                PersonalInfoFieldLocator.EmergencyContactPhoneRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.SecondaryEmailAddress,
                PersonalInfoFieldLocator.SecondaryEmailAddressRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.UploadPhoto,
                PersonalInfoFieldLocator.UploadPhotoRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.MembershipNumber,
                PersonalInfoFieldLocator.MembershipNumberRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.CustomerNumber,
                PersonalInfoFieldLocator.CustomerNumberRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.SocialSecurityNumber,
                PersonalInfoFieldLocator.SocialSecurityNumberRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.TaxIdentificationNumber,
                PersonalInfoFieldLocator.TaxIdentificationNumberRequiredOptionLocator);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.OptOut,
                null);

            this.personalInfoFieldRequiredOptionLocator.Add(
                PersonalInfoField.ContactInfo,
                PersonalInfoFieldLocator.ContactInfoRequiredOptionLocator);
        }

        [Step]
        public void SetPersonalInfoPage()
        {
            // TO DO:  read values from external source?

            // Make all standard fields visible

            this.SetRecommendedFieldsVisibility(true);

            // Make all optional fields visible
            this.SetOptionalFieldsVisibility(true);
        }

        public void SetRecommendedFieldsVisibility(bool visible)
        {
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.VerifyEmail, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Prefix, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Suffix, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.NameOnBadge, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Country, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.NonUSState, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.HomePhone, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Cell, visible, null);
        }

        public void SetOptionalFieldsVisibility(bool visible)
        {
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.DateOfBirth, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Gender, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.EmergencyContactName, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.EmergencyContactPhone, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.SecondaryEmailAddress, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.MembershipNumber, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.CustomerNumber, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.SocialSecurityNumber, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.TaxIdentificationNumber, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.UploadPhoto, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.OptOut, visible, null);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.ContactInfo, visible, null);
        }

        public void SetRecommendedFieldsRequired(bool required)
        {
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Prefix, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Suffix, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.NameOnBadge, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Country, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.NonUSState, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.HomePhone, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Cell, null, required);
        }

        public void SetOptionalFieldsRequired(bool required)
        {
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.DateOfBirth, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.Gender, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.EmergencyContactName, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.EmergencyContactPhone, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.SecondaryEmailAddress, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.MembershipNumber, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.CustomerNumber, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.SocialSecurityNumber, null, required);
            SetPersonalInfoFieldVisibilityOption(FormDetailManager.PersonalInfoField.TaxIdentificationNumber, null, required);
        }

        [Step]
        public void SetPersonalInfoFieldVisibilityOption(PersonalInfoField field, bool? checkVisibleOption, bool? checkRequiredOption)
        {
            string PIPageStandardFieldVisibleLocator = "//*[text()='{0}']/../../../..//td[2]/input";
            string PIPageStandardFieldRequiredLocator = "//*[text()='{0}']/../../../..//td[3]/input";
            string PIPageOptOutVisibleLocator = "//*[contains(text(),'{0}')]/../../td[2]/input";
            string PIPageSSNVisibleLocator = "ctl00_cph_chkCollectionFieldscSSNID";
            string PIPageSSNRequiredLocator = "ctl00_cph_chkCollectionFieldsrSSNID";
            string PIPageContactInfoVisibleLocator = "//*[contains(text(),'{0}')]/../td[2]/input";
            string PIPageContactInfoRequiredLocator = "//*[contains(text(),'{0}')]/../td[3]/input";

            switch(field)
            {
                case PersonalInfoField.OptOut:
                    UIUtilityProvider.UIHelper.SetCheckbox(string.Format(PIPageOptOutVisibleLocator, StringEnum.GetStringValue(field)), checkVisibleOption.Value, LocateBy.XPath);
                    break;

                case PersonalInfoField.SocialSecurityNumber:
                    if (checkVisibleOption.HasValue)
                    {
                        UIUtilityProvider.UIHelper.SetCheckbox(PIPageSSNVisibleLocator, checkVisibleOption.Value, LocateBy.Id);
                    }
                    if (checkRequiredOption.HasValue)
                    {
                        UIUtilityProvider.UIHelper.SetCheckbox(PIPageSSNRequiredLocator, checkRequiredOption.Value, LocateBy.Id);
                    }
                    break;

                case PersonalInfoField.ContactInfo:
                    if (checkVisibleOption.HasValue)
                    {
                        UIUtilityProvider.UIHelper.SetCheckbox(string.Format(PIPageContactInfoVisibleLocator, StringEnum.GetStringValue(field)), checkVisibleOption.Value, LocateBy.XPath);
                    }
                    if (checkRequiredOption.HasValue)
                    {
                        UIUtilityProvider.UIHelper.SetCheckbox(string.Format(PIPageContactInfoRequiredLocator, StringEnum.GetStringValue(field)), checkRequiredOption.Value, LocateBy.XPath);
                    }
                    break;

                default:
                    if (checkVisibleOption.HasValue)
                    {
                        UIUtilityProvider.UIHelper.SetCheckbox(string.Format(PIPageStandardFieldVisibleLocator, StringEnum.GetStringValue(field)), checkVisibleOption.Value, LocateBy.XPath);
                    }
                    if (checkRequiredOption.HasValue)
                    {
                        UIUtilityProvider.UIHelper.SetCheckbox(string.Format(PIPageStandardFieldRequiredLocator, StringEnum.GetStringValue(field)), checkRequiredOption.Value, LocateBy.XPath);
                    }
                    break;
            }
        }

        [Verify]
        public void VerifyPersonalInfoPageDefaults()
        {
            // Assert that:
            //   Email is always visible and required
            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.Email, 
                true, 
                true);

            //   Certain fields are required/visible
            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.FirstName,
                true,
                true);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.MiddleName,
                true,
                null);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.LastName,
                true,
                true);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.JobTitle,
                true,
                null);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.Company,
                true,
                true);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.AddressLineOne,
                true,
                true);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.AddressLineTwo,
                true,
                null);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.City,
                true,
                true);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.State,
                true,
                true);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.ZipCode,
                true,
                true);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.WorkPhone,
                true,
                true);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.Extension,
                true,
                null);

            this.VerifyPersonalInfoFieldVisibilityOption(
                PersonalInfoField.Fax,
                true,
                null);
        }

        public void VerifyPersonalInfoFieldVisibilityOption(
            PersonalInfoField field, 
            bool? checkVisibleOption,
            bool? checkRequiredOption)
        {
            if (field == PersonalInfoField.Email)
            {
                this.VerifyPersonalInfoEmailFieldVisibilityOption(checkVisibleOption, checkRequiredOption);
            }
            else
            {
                this.VerifyPersonalInfoFieldExceptEmailVisibilityOption(
                        field,
                        checkVisibleOption,
                        checkRequiredOption);
            }
        }

        private void VerifyPersonalInfoEmailFieldVisibilityOption(
            bool? checkVisibleOption,
            bool? checkRequiredOption)
        {
            ////string disabledAttribute = "@disabled";
            string disabledAttributeString = "disabled";
            string disabledAttributeTrueString = "true";

            if (checkVisibleOption.HasValue)
            {
                // Verify visible option when option parameter is set to 'Visible' or 'All'
                VerifyTool.VerifyValue(
                    disabledAttributeTrueString,
                    UIUtilityProvider.UIHelper.GetAttribute(PersonalInfoFieldLocator.EmailVisibleOptionLocator, disabledAttributeString, LocateBy.Id),
                    "'Email' field visible option disabled: {0}");

                VerifyTool.VerifyValue(
                    checkVisibleOption.Value,
                    UIUtilityProvider.UIHelper.IsChecked(PersonalInfoFieldLocator.EmailVisibleOptionLocator, LocateBy.Id), 
                    "'Email' field visible option checked: {0}");
            }

            if (checkRequiredOption.HasValue)
            {
                // Verify required option when option parameter is set to 'Required' or 'All'
                VerifyTool.VerifyValue(
                    disabledAttributeTrueString,
                    UIUtilityProvider.UIHelper.GetAttribute(PersonalInfoFieldLocator.EmailRequiredOptionLocator, disabledAttributeString, LocateBy.Id),
                    "'Email' field required option disabled: {0}");

                VerifyTool.VerifyValue(
                    checkRequiredOption.Value,
                    UIUtilityProvider.UIHelper.IsChecked(PersonalInfoFieldLocator.EmailRequiredOptionLocator, LocateBy.Id),
                    "'Email' field required option checked: {0}");
            }
        }

        private void VerifyPersonalInfoFieldExceptEmailVisibilityOption(
            PersonalInfoField field,
            bool? checkVisibleOption,
            bool? checkRequiredOption)
        {
            string fieldName = StringEnum.GetStringValue(field);

            if (checkVisibleOption.HasValue)
            {
                VerifyTool.VerifyValue(
                    checkVisibleOption.Value,
                    UIUtilityProvider.UIHelper.IsChecked(this.personalInfoFieldVisibleOptionLocator[field], LocateBy.Id),
                    "'" + fieldName + "' field visible option checked: {0}");
            }

            if (checkRequiredOption.HasValue)
            {
                VerifyTool.VerifyValue(
                    checkRequiredOption.Value,
                    UIUtilityProvider.UIHelper.IsChecked(this.personalInfoFieldRequiredOptionLocator[field], LocateBy.Id),
                    "'" + fieldName + "' field required option checked: {0}");
            }
        }

        [Verify]
        public void VerifyPersonalInfoPageSettingsAreSaved()
        {
            ////this.ReloadEvent();
            ClientDataContext db = new ClientDataContext();
            //Event.EventCollectionField = (from ec in db.EventCollectionFields where ec.EventId == Event.Id select ec).Single();
            Event = (from e in db.Events where e.Id == eventId join ec in db.EventCollectionFields on e.Id equals ec.EventId select e).Single();

            // assert that selected fields are visible/required in DB
            Assert.That(Event.EventCollectionField.cEmail ?? false);
            Assert.That(Event.EventCollectionField.rEmail ?? false);
            Assert.That(Event.EventCollectionField.cVerifyEmail ?? false);
            Assert.That(Event.EventCollectionField.cPrefix ?? false);
            Assert.That(Event.EventCollectionField.cFirstName ?? false);
            Assert.That(Event.EventCollectionField.rFirstName ?? false);
            Assert.That(Event.EventCollectionField.cMiddleName ?? false);
            Assert.That(Event.EventCollectionField.cLastName ?? false);
            Assert.That(Event.EventCollectionField.rLastName ?? false);
            Assert.That(Event.EventCollectionField.cSuffix ?? false);
            Assert.That(Event.EventCollectionField.cTitle ?? false);
            Assert.That(Event.EventCollectionField.cBadgeName ?? false);
            Assert.That(Event.EventCollectionField.cCompany ?? false);
            Assert.That(Event.EventCollectionField.rCompany ?? false);
            Assert.That(Event.EventCollectionField.cAddress1 ?? false);
            Assert.That(Event.EventCollectionField.rAddress1 ?? false);
            Assert.That(Event.EventCollectionField.cAddress2 ?? false);
            Assert.That(Event.EventCollectionField.cCity ?? false);
            Assert.That(Event.EventCollectionField.rCity ?? false);
            Assert.That(Event.EventCollectionField.cState ?? false);
            Assert.That(Event.EventCollectionField.rState ?? false);
            Assert.That(Event.EventCollectionField.cAddress3 ?? false);
            Assert.That(Event.EventCollectionField.cPostalCode ?? false);
            Assert.That(Event.EventCollectionField.rPostalCode ?? false);
            Assert.That(Event.EventCollectionField.cCountry ?? false);
            Assert.That(Event.EventCollectionField.cHomePhone ?? false);
            Assert.That(Event.EventCollectionField.cWorkPhone ?? false);
            Assert.That(Event.EventCollectionField.cExtension ?? false);
            Assert.That(Event.EventCollectionField.cFax ?? false);
            Assert.That(Event.EventCollectionField.cCellPhone ?? false);
            Assert.That(Event.EventCollectionField.cDateOfBirth ?? false);
            Assert.That(Event.EventCollectionField.cGender ?? false);
            Assert.That(Event.EventCollectionField.cEmergencyContactName ?? false);
            Assert.That(Event.EventCollectionField.cEmergencyContactPhone ?? false);
            Assert.That(Event.EventCollectionField.cCCEmail ?? false);
            Assert.That(Event.EventCollectionField.cPhoto);
            Assert.That(Event.EventCollectionField.cMembershipId ?? false);
            Assert.That(Event.EventCollectionField.cCustId ?? false);
            Assert.That(Event.EventCollectionField.cSSNId ?? false);
            Assert.That(Event.EventCollectionField.cTaxIdentificationNumber ?? false);
            Assert.That(Event.AllowDirectoryOptOut);
            Assert.That(Event.EventCollectionField.cContactInfo ?? false);
        }

        public void ClickAddPICustomField()
        {
            this.ClickAddCustomField(CustomFieldManager.CustomFieldLocation.PI);
        }

        [Step]
        public void AddPICustomField(CustomFieldManager.CustomFieldType type, string name)
        {
            this.AddCustomField(CustomFieldManager.CustomFieldLocation.PI, type, name);
        }

        [Verify]
        public void VerifyPersonalInfoCustomFieldInDatabase(CustomFieldManager.CustomFieldType type, string name)
        {
            VerifyCustomFieldInDatabase(type, name, 1);
        }

        public int GetCustomFieldID(string name)
        {
            return Convert.ToInt32(
                UIUtilityProvider.UIHelper.ExtractElementInQueryString(UIUtilityProvider.UIHelper.GetAttribute(string.Format("//a[text()='{0}']", name), "href", LocateBy.XPath), "cfId"));
        }
    }
}