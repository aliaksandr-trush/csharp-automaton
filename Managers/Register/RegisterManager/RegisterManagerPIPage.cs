namespace RegOnline.RegressionTest.Managers.Register
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using NUnit.Framework;
	using RegOnline.RegressionTest.Utilities;
	using RegOnline.RegressionTest.UIUtility;
	using RegOnline.RegressionTest.Configuration;
	using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Attributes;

	public partial class RegisterManager : ManagerBase
	{
		private const string PersonalInfoPageTitle = "Personal Info";
		private const string PersonalInfoPagePathOld = "Registrations/Attendee/Attendee.asp";
		private const string PersonalInfoPagePathNew = "register/PersonalInfo.aspx";
		private const string PersonalInfoLastNameLocator = "ctl00_cph_personalInfoStandardFields_rptFields_ctl08_sf_txtResponse";

		public enum Gender
		{
			Male,
			Female
		}

		public struct DefaultPersonalInfo
		{
			public const string FirstName = "Selenium";
			public const string MiddleName = "M";
			public const string PartialLastName = "Rgrssn-";
			public const string JobTitle = "Regression meister";
			public const string Company = "Regression, Inc";
			public const string AddressLineOne = PaymentManager.DefaultPaymentInfo.BillingAddressLineOne;
			public const string AddressLineTwo = PaymentManager.DefaultPaymentInfo.BillingAddressLineTwo;
			public const string City = PaymentManager.DefaultPaymentInfo.BillingCity;
			public const string State = "Colorado";
			public const string ZipCode = PaymentManager.DefaultPaymentInfo.ZipCode;
			public const string WorkPhone = "303.555.1212";
			public const string Extension = "113";
			public const string Fax = "303.987.3524";
		}

		#region Methods for old Personal Info Page

		public void EnterPersonalInfoFieldsEmailFirstMiddleLastCompanyMembershipId(string email,
			string first, string middle, string last, string company, string membershipId)
		{
			if (email != null) UIUtil.DefaultProvider.Type("Text4", email, LocateBy.Id);
			if (first != null) this.TypePersonalInfoFirstName(first);
			if (middle != null) this.TypePersonalInfoMiddleName(middle);
			if (last != null) this.TypePersonalInfoLastName(last);
			if (company != null) this.TypePersonalInfoCompany(company);
			if (membershipId != null) UIUtil.DefaultProvider.Type("Text25", membershipId, LocateBy.Id);
		}

		[Step]
		public void EnterPersonalInfoEnduranceNewsletterPartners(bool? eNewsletters, bool? partnerEmails)
		{
			if (eNewsletters != null) UIUtil.DefaultProvider.SetCheckbox("AllowNewsletter", (bool)eNewsletters, LocateBy.Id);
			if (partnerEmails != null) UIUtil.DefaultProvider.SetCheckbox("AllowPartnersCommunication", (bool)partnerEmails, LocateBy.Id);
		}

		public void VerifyPersonalInfoFieldsEmailFirstMiddleLastCompanyMembershipId(string email,
			string first, string middle, string last, string company, string membershipId)
		{
			if (email != null) Assert.AreEqual(email, UIUtil.DefaultProvider.GetValue("Text4", LocateBy.Id));
			if (first != null) Assert.AreEqual(first, UIUtil.DefaultProvider.GetValue("Text8", LocateBy.Id));
			if (middle != null) Assert.AreEqual(middle, UIUtil.DefaultProvider.GetValue("Text9", LocateBy.Id));
			if (last != null) Assert.AreEqual(last, UIUtil.DefaultProvider.GetValue("Text11", LocateBy.Id));
			if (company != null) Assert.AreEqual(company, UIUtil.DefaultProvider.GetValue("Text14", LocateBy.Id));
			if (membershipId != null) Assert.AreEqual(membershipId, UIUtil.DefaultProvider.GetValue("Text25", LocateBy.Id));
		}

		public void VerifyPersonalInfoFieldsUpdateHidden(string email,
			string first, string middle, string last, string membershipId)
		{
			if (email != null) Assert.AreEqual(email, UIUtil.DefaultProvider.GetValue("//input[@name='EMail_Address']", LocateBy.XPath));
			if (first != null) Assert.AreEqual(first, UIUtil.DefaultProvider.GetValue("//input[@name='First_Name']", LocateBy.XPath));
			if (middle != null) Assert.AreEqual(middle, UIUtil.DefaultProvider.GetValue("//input[@name='Middle_Name']", LocateBy.XPath));
			if (last != null) Assert.AreEqual(last, UIUtil.DefaultProvider.GetValue("//input[@name='Last_Name']", LocateBy.XPath));
			if (membershipId != null) Assert.AreEqual(membershipId, UIUtil.DefaultProvider.GetValue("//input[@name='MembershipId']", LocateBy.XPath));
		}

		public bool DoesPersonalInfoHavePassword()
		{
			bool hasPassword = false;
			//Assert.IsTrue(OnPersonalInfoPageOld());

			if (UIUtil.DefaultProvider.IsElementPresent("Password1", LocateBy.Id))
			{
				hasPassword = true;
			}

			return hasPassword;
		}
		#endregion

		#region Methods for new PI page
		[Step]
		public bool OnPersonalInfoPage()
		{
			return UIUtil.DefaultProvider.UrlContainsAbsolutePath(PersonalInfoPagePathNew);
		}

		[Step]
		public string EnterProfileInfo()
		{
			UIUtil.DefaultProvider.VerifyOnPage(OnPersonalInfoPage(), PersonalInfoPageTitle);
			string lastName = this.SetDefaultStandardPersonalInfoFields();
			this.EnterPersonalInfoPassword();

			return lastName;
		}

		public string EnterProfileInfoEnduranceNew()
		{
			UIUtil.DefaultProvider.VerifyOnPage(OnPersonalInfoPage(), PersonalInfoPageTitle);
			string lastName = this.SetDefaultStandardPersonalInfoEnduranceFields();
			this.EnterPersonalInfoPassword();

			return lastName;
		}

		[Step]
		public string EnterProfileInfoWithoutPassword()
		{
			UIUtil.DefaultProvider.VerifyOnPage(OnPersonalInfoPage(), PersonalInfoPageTitle);
			return this.SetDefaultStandardPersonalInfoFields();
		}

		public string SetDefaultStandardPersonalInfoFields()
		{
			string lastName = this.CurrentRegistrantLastName = this.GenerateCurrentRegistrantLastName();
			this.TypePersonalInfoFirstName(DefaultPersonalInfo.FirstName);
			this.TypePersonalInfoMiddleName(DefaultPersonalInfo.MiddleName);
			this.TypePersonalInfoLastName(lastName);
			this.TypePersonalInfoJobTitle(DefaultPersonalInfo.JobTitle);
			this.TypePersonalInfoCompany(DefaultPersonalInfo.Company);
			this.TypePersonalInfoAddressLineOne(DefaultPersonalInfo.AddressLineOne);
			this.TypePersonalInfoCity(DefaultPersonalInfo.City);
			this.SelectPersonalInfoState(DefaultPersonalInfo.State);
			this.TypePersonalInfoZipCode(DefaultPersonalInfo.ZipCode);
			this.TypePersonalInfoWorkPhone(DefaultPersonalInfo.WorkPhone);

			return lastName;
		}

		public string SetDefaultStandardPersonalInfoEnduranceFields()
		{
			string lastName = this.CurrentRegistrantLastName = this.GenerateCurrentRegistrantLastName();
			this.TypePersonalInfoEnduranceForename(DefaultPersonalInfo.FirstName);
			this.TypePersonalInfoMiddleName(DefaultPersonalInfo.MiddleName);
			this.TypePersonalInfoEnduranceSurname(lastName);
			this.TypePersonalInfoEnduranceCompany(DefaultPersonalInfo.Company);
			this.TypePersonalInfoAddressLineOne(DefaultPersonalInfo.AddressLineOne);
			this.TypePersonalInfoEnduranceCity(DefaultPersonalInfo.City);
			this.TypePersonalInfoZipCode(DefaultPersonalInfo.ZipCode);
			this.TypePersonalInfoEnduranceWorkPhone(DefaultPersonalInfo.WorkPhone);

			return lastName;
		}

		[Step]
		public string GenerateCurrentRegistrantLastName()
		{
			string lastName = string.Empty;

			if (CurrentTicks == 0)
			{
				CurrentTicks = System.DateTime.Now.Ticks;
			}

			int check = Convert.ToInt32(CurrentTicks.ToString().Substring(0, 5)) % 9;

			lastName = 
				DefaultPersonalInfo.PartialLastName 
				+ check.ToString() 
				+ CurrentTicks.ToString().Substring(6);

			this.CurrentRegistrantLastName = lastName;

			return lastName;
		}

		[Step]
		public void EnterPersonalInfoEnduranceTeamNameNew(string teamname)
		{
			this.TypePersonalInfoEnduranceTeamName(teamname);
		}

		[Step]
		public void EnterPersonalInfoEnduranceForename(string name)
		{
			this.TypePersonalInfoEnduranceForename(name);
		}

		[Step]
		public void EnterPersonalInfoEnduranceSurname(string name)
		{
			this.TypePersonalInfoEnduranceSurname(name);
		}

		[Step]
		public void EnterPersonalInfoPassword(string password)
		{
			this.TypePersonalInfoPassword(password);
			this.TypePersonalInfoVerifyPassword(password);
		}

        [Step]
        public void EnterPersonalInfoPassword()
        {
            this.EnterPersonalInfoPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
        }

		[Step]
		public void EnterPersonalInfoNamePrefixSuffix(string prefix, string first, string middle, string last, string suffix)
		{
			this.TypePersonalInfoPrefix(prefix);
			this.TypePersonalInfoFirstName(first);
			this.TypePersonalInfoMiddleName(middle);
			this.TypePersonalInfoLastName(last);
			this.TypePersonalInfoSuffix(suffix);
		}

		[Step]
		public void EnterPersonalInfoEndurancePrefix(string prefix)
		{
			this.TypePersonalInfoEndurancePrefix(prefix);
		}

		[Step]
		public void EnterPersonalInfoTitleBadgeCompanyCountry(string title, string nameOnBadge, string company, string country)
		{
			this.TypePersonalInfoJobTitle(title);
			this.TypePersonalInfoBadge(nameOnBadge);
			this.TypePersonalInfoCompany(company);
			this.SelectPersonalInfoCountry(country);
		}

		[Step]
		public void EnterPersonalInfoEnduranceTitleBadgeCompanyCountry(string title, string nameOnBadge, string company, string country)
		{
			this.TypePersonalInfoJobTitle(title);
			this.TypePersonalInfoBadge(nameOnBadge);
			this.TypePersonalInfoEnduranceCompany(company);
			this.SelectPersonalInfoCountry(country);
		}

		[Step]
		public void EnterPersonalInfoAddress(string addressOne, string addressTwo, string city, string stateUS, string stateNonUS, string zip)
		{
			this.TypePersonalInfoAddressLineOne(addressOne);
			this.TypePersonalInfoAddressLineTwo(addressTwo);
			this.TypePersonalInfoCity(city);
			this.SelectPersonalInfoState(stateUS);
			this.SelectPersonalInfoNonUSRegion(stateNonUS);
			this.TypePersonalInfoZipCode(zip);
		}

		[Step]
		public void EnterPersonalInfoEnduranceAddress(string addressOne, string addressTwo, string city, string stateUS, string stateNonUS, string zip)
		{
			this.TypePersonalInfoAddressLineOne(addressOne);
			this.TypePersonalInfoAddressLineTwo(addressTwo);
			this.TypePersonalInfoEnduranceCity(city);
			this.SelectPersonalInfoState(stateUS);
			this.SelectPersonalInfoEnduranceRegion(stateNonUS);
			this.TypePersonalInfoZipCode(zip);
		}

		[Step]
		public void EnterPersonalInfoPhoneNumbers(string homePhone, string workPhone, string workExtension, string faxNumber, string cellPhone)
		{
			this.TypePersonalInfoHomePhone(homePhone);
			this.TypePersonalInfoWorkPhone(workPhone);
			this.TypePersonalInfoExtension(workExtension);
			this.TypePersonalInfoFax(faxNumber);
			this.TypePersonalInfoCellPhone(cellPhone);
		}

		[Step]
		public void EnterPersonalInfoEndurancePhoneNumbers(string homePhone, string workPhone, string workExtension, string faxNumber, string cellPhone)
		{
			this.TypePersonalInfoEnduranceHomePhone(homePhone);
			this.TypePersonalInfoEnduranceWorkPhone(workPhone);
			this.TypePersonalInfoExtension(workExtension);
			this.TypePersonalInfoFax(faxNumber);
			this.TypePersonalInfoEnduranceMobilePhone(cellPhone);
		}

		public void EnterPersonalInfoDateOfBirthGender(DateTime dateOfBirth, Gender gender)
		{
			this.TypePersonalInfoDateOfBirth(dateOfBirth);
			this.SelectPersonalInfoGender(gender);
		}

		[Step]
		public void EnterPersonalInfoEnduranceNationality(string nationality)
		{
			this.SelectPersonalInfoEnduranceNationality(nationality);
		}

		[Step]
		public void EnterPersonalInfoTaxNumberMembershipNumberCustomerNumber(string taxNumber, string membershipNumber, string customerNumber)
		{
			this.TypePersonalInfoTaxNumber(taxNumber);
			this.TypePersonalInfoMembershipNumber(membershipNumber);
			this.TypePersonalInfoCustomerNumber(customerNumber);
		}

		[Step]
		public void TypePersonalInfoUniqueEmail()
		{
			this.TypePersonalInfoEmail(this.ComposeUniqueEmailAddress());
		}

		public void TypePersonalInfoEmail(string email)
		{
			if (!string.IsNullOrEmpty(email))
			{
				UIUtil.DefaultProvider.Type("ctl00_cph_personalInfoStandardFields_rptFields_ctl02_sf_txtResponse", email, LocateBy.Id);
			}
		}

		public void VerifyStandardFieldsPresent(bool recommended, bool optional)
		{
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.VerifyEmail), recommended);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Prefix), recommended);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Suffix), recommended);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.NameOnBadge), recommended);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Country), recommended);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.NonUSState), recommended);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.HomePhone), recommended);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Cell), recommended);

			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.DateOfBirth), optional);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Gender), optional);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.EmergencyContactName), optional);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.EmergencyContactPhone), optional);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.SecondaryEmailAddress), optional);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.MembershipNumber), optional);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.CustomerNumber), optional);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.SocialSecurityNumber), optional);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.TaxIdentificationNumber), optional);
			VerifyCustomFieldPresent(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.UploadPhoto), optional);
			this.VerifyContactInfoPresent(optional);
		}

		public void VerifyStandardFieldsRequired(bool recommended, bool optional)
		{
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Prefix), recommended);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Suffix), recommended);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.NameOnBadge), recommended);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Country), recommended);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.NonUSState), recommended);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.HomePhone), recommended);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Cell), recommended);
							 
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.DateOfBirth), optional);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Gender), optional);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.EmergencyContactName), optional);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.EmergencyContactPhone), optional);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.SecondaryEmailAddress), optional);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.MembershipNumber), optional);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.CustomerNumber), optional);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.SocialSecurityNumber), optional);
			VerifyCustomFieldRequired(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.TaxIdentificationNumber), optional);
		}

		public void VerifyContactInfoPresent(bool present)
		{
			UIUtil.DefaultProvider.VerifyElementPresent(RegisterSiteLocator.ContactInfoNameLocator, present, LocateBy.Id);
			UIUtil.DefaultProvider.VerifyElementPresent(RegisterSiteLocator.ContactInfoPhoneLocator, present, LocateBy.Id);
			UIUtil.DefaultProvider.VerifyElementPresent(RegisterSiteLocator.ContactInfoEmailLocator, present, LocateBy.Id);
		}

		public bool IsEmailFieldPresent()
		{
			return UIUtil.DefaultProvider.IsElementPresent("ctl00_cph_personalInfoStandardFields_rptFields_ctl02_sf_txtResponse", LocateBy.Id);
		}
		public void TypePersonalInfoVerifyEmail(string email)
		{
			if (!string.IsNullOrEmpty(email))
			{
				UIUtil.DefaultProvider.Type("ctl00_cph_personalInfoStandardFields_rptFields_ctl03_sf_txtResponse", email, LocateBy.Id);
			}
		}

		public void TypePersonalInfoSecondaryEmail(string email)
		{
			if (!string.IsNullOrEmpty(email))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Secondary Email Address (cc Email):']", "for", LocateBy.XPath), email, LocateBy.Id);
			}
		}

		public void TypePersonalInfoEnduranceTeamName(string teamname)
		{
			if (!string.IsNullOrEmpty(teamname))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Team Name:']", "for", LocateBy.XPath), teamname, LocateBy.Id);
			}
		}

		public void TypePersonalInfoPrefix(string prefix)
		{
			if (!string.IsNullOrEmpty(prefix))
			{
				UIUtil.DefaultProvider.Type("ctl00_cph_personalInfoStandardFields_rptFields_ctl05_sf_txtResponse", prefix, LocateBy.Id);
			}
		}

		public void TypePersonalInfoEndurancePrefix(string prefix)
		{
			if (!string.IsNullOrEmpty(prefix))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Prefix (Mr, Mrs, etc):']", "for", LocateBy.XPath), prefix, LocateBy.Id);
			}
		}

		public void TypePersonalInfoSuffix(string suffix)
		{
			if (!string.IsNullOrEmpty(suffix))
			{
				UIUtil.DefaultProvider.Type("ctl00_cph_personalInfoStandardFields_rptFields_ctl09_sf_txtResponse", suffix, LocateBy.Id);
			}
		}

		[Step]
		public void TypePersonalInfoFirstName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				UIUtil.DefaultProvider.Type("ctl00_cph_personalInfoStandardFields_rptFields_ctl06_sf_txtResponse", name, LocateBy.Id);
			}
		}

		public bool IsFirstNameFieldPresent()
		{
			return UIUtil.DefaultProvider.IsElementPresent("ctl00_cph_personalInfoStandardFields_rptFields_ctl06_sf_txtResponse", LocateBy.Id);
		}
		public void TypePersonalInfoEnduranceForename(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Forename:']", "for", LocateBy.XPath), name, LocateBy.Id);
			}
		}

		[Step]
		public void TypePersonalInfoMiddleName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				UIUtil.DefaultProvider.Type("ctl00_cph_personalInfoStandardFields_rptFields_ctl07_sf_txtResponse", name, LocateBy.Id);
			}
		}
		public bool IsMiddleNameFieldPresent()
		{
			return UIUtil.DefaultProvider.IsElementPresent("ctl00_cph_personalInfoStandardFields_rptFields_ctl07_sf_txtResponse", LocateBy.Id);
		}

		[Step]
		public void TypePersonalInfoLastName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				UIUtil.DefaultProvider.Type(PersonalInfoLastNameLocator, name, LocateBy.Id);
			}
		}
		public bool IsLastNameFieldPresent()
		{
			return UIUtil.DefaultProvider.IsElementPresent(PersonalInfoLastNameLocator, LocateBy.Id);
		}

		public void VerifyPersonalInfoLastName(string name)
		{
			string actualValue = UIUtil.DefaultProvider.GetValue(PersonalInfoLastNameLocator, LocateBy.Id);
			Utilities.VerifyTool.VerifyValue(name, actualValue, "Last name text : {0}");
		}

		public bool IsStatusFieldPresent()
		{
			return UIUtil.DefaultProvider.IsElementPresent("ctl00_cph_personalInfoStandardFields_rptFields_ctl39_sf_ddlResponse", LocateBy.Id);
		}

		public void TypePersonalInfoEnduranceSurname(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Surname:']", "for", LocateBy.XPath), name, LocateBy.Id);
			}
		}

		[Step]
		public void TypePersonalInfoJobTitle(string title)
		{
			if (!string.IsNullOrEmpty(title))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Job Title:']", "for", LocateBy.XPath), title, LocateBy.Id);
			}
		}
		public void VerifyPersonalInfoJobTitle(string title)
		{
			VerifyPersonalInfoStandardFieldText("Job Title", title);
		}

		public void TypePersonalInfoBadge(string badge)
		{
			if (!string.IsNullOrEmpty(badge))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Name as it would appear on the badge:']", "for", LocateBy.XPath), badge, LocateBy.Id);
			}
		}

		[Step]
		public void TypePersonalInfoCompany(string company)
		{
			if (!string.IsNullOrEmpty(company))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Company/Organization:']", "for", LocateBy.XPath), company, LocateBy.Id);
			}
		}

		public void VerifyPersonalInfoCompany(string company)
		{
			VerifyPersonalInfoStandardFieldText("Company/Organization", company);
		}

		public void TypePersonalInfoEnduranceCompany(string company)
		{
			if (!string.IsNullOrEmpty(company))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Company/Organisation:']", "for", LocateBy.XPath), company, LocateBy.Id);
			}
		}

		public void SelectPersonalInfoCountry(string country)
		{
			if (!string.IsNullOrEmpty(country))
			{
				UIUtil.DefaultProvider.SelectWithText(UIUtil.DefaultProvider.GetAttribute("//*[text()='Country:']", "for", LocateBy.XPath), country, LocateBy.Id);
			}
		}

		public void TypePersonalInfoAddressLineOne(string address)
		{
			if (!string.IsNullOrEmpty(address))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Address Line 1:']", "for", LocateBy.XPath), address, LocateBy.Id);
			}
		}

		[Step]
		public void TypePersonalInfoAddressLineTwo(string address)
		{
			if (!string.IsNullOrEmpty(address))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Address Line 2:']", "for", LocateBy.XPath), address, LocateBy.Id);
			}
		}

		[Step]
		public void TypePersonalInfoCity(string city)
		{
			if (!string.IsNullOrEmpty(city))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='City:']", "for", LocateBy.XPath), city, LocateBy.Id);
			}
		}

		public void TypePersonalInfoEnduranceCity(string city)
		{
			if (!string.IsNullOrEmpty(city))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Town/City:']", "for", LocateBy.XPath), city, LocateBy.Id);
			}
		}

		public void SelectPersonalInfoState(string region)
		{
			if (!string.IsNullOrEmpty(region))
			{
				UIUtil.DefaultProvider.SelectWithText(UIUtil.DefaultProvider.GetAttribute("//*[text()='US State/Canadian Province:']", "for", LocateBy.XPath), region, LocateBy.Id);
			}
		}

		public void SelectPersonalInfoNonUSRegion(string nonUSRegion)
		{
			if (!string.IsNullOrEmpty(nonUSRegion))
			{
				UIUtil.DefaultProvider.SelectWithText(UIUtil.DefaultProvider.GetAttribute("//*[text()='State/Province/Region (Non US/Canada):']", "for", LocateBy.XPath), nonUSRegion, LocateBy.Id);
			}
		}

		public void SelectPersonalInfoEnduranceRegion(string nonUSRegion)
		{
			if (!string.IsNullOrEmpty(nonUSRegion))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='State/Province/Region:']", "for", LocateBy.XPath), nonUSRegion, LocateBy.Id);
			}
		}

		public void TypePersonalInfoZipCode(string zip)
		{
			if (!string.IsNullOrEmpty(zip))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Zip (Postal Code):']", "for", LocateBy.XPath), zip, LocateBy.Id);
			}
		}

		public void TypePersonalInfoHomePhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Home Phone:']", "for", LocateBy.XPath), phone, LocateBy.Id);
			}
		}

		public void TypePersonalInfoEnduranceHomePhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Home Telephone:']", "for", LocateBy.XPath), phone, LocateBy.Id);
			}
		}

		public void TypePersonalInfoWorkPhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Work Phone:']", "for", LocateBy.XPath), phone, LocateBy.Id);
			}
		}

		public void TypePersonalInfoEnduranceWorkPhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Work Telephone:']", "for", LocateBy.XPath), phone, LocateBy.Id);
			}
		}

		public void TypePersonalInfoExtension(string extension)
		{
			if (!string.IsNullOrEmpty(extension))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Extension:']", "for", LocateBy.XPath), extension, LocateBy.Id);
			}
		}

		[Step]
		public void TypePersonalInfoFax(string fax)
		{
			if (!string.IsNullOrEmpty(fax))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Fax:']", "for", LocateBy.XPath), fax, LocateBy.Id);
			}
		}

		public void TypePersonalInfoCellPhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Cell Phone:']", "for", LocateBy.XPath), phone, LocateBy.Id);
			}
		}

		public void TypePersonalInfoEnduranceMobilePhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Mobile Phone:']", "for", LocateBy.XPath), phone, LocateBy.Id);
			}
		}

		public bool HasPasswordTextbox()
		{
			return UIUtil.DefaultProvider.IsElementPresent("ctl00_cph_ctlPassword_txtPassword", LocateBy.Id);
		}

		public void VerifyHasPersonalInfoPassword(bool expected)
		{
			VerifyTool.VerifyValue(expected, UIUtil.DefaultProvider.IsElementDisplay(PasswordEnterPassword, LocateBy.Id), "Need to enter password on PI page: {0}");
		}

		[Step]
		public void TypePersonalInfoPassword(string password)
		{
			if (!string.IsNullOrEmpty(password))
			{
				UIUtil.DefaultProvider.Type("ctl00_cph_ctlPassword_txtPassword", password, LocateBy.Id);
			}
		}

		[Verify]
		public void TypePersonalInfoVerifyPassword(string verifyPassword)
		{
			if (!string.IsNullOrEmpty(verifyPassword))
			{
				UIUtil.DefaultProvider.Type("ctl00_cph_ctlPassword_txtVerifyPassword", verifyPassword, LocateBy.Id);
			}
		}

		public void TypePersonalInfoDateOfBirth(DateTime dateOfBirth)
		{
			UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Date of Birth:']", "for", LocateBy.XPath), this.GetBirthDateString(dateOfBirth, false), LocateBy.Id);
		}

		public void TypePersonalInfoDateOfBirth_Endurance(DateTime dateOfBirth)
		{
			UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Date of Birth:']", "for", LocateBy.XPath), this.GetBirthDateString(dateOfBirth, true), LocateBy.Id);
		}

		private string GetBirthDateString(DateTime dateOfBirth, bool isEndurance)
		{
			StringBuilder dateString = new StringBuilder();

			if (isEndurance)
			{
				dateString.Append(dateOfBirth.Day.ToString());
				dateString.Append("/");
				dateString.Append(dateOfBirth.Month.ToString());
				dateString.Append("/");
			}
			else
			{
				dateString.Append(dateOfBirth.Month.ToString());
				dateString.Append("/");
				dateString.Append(dateOfBirth.Day.ToString());
				dateString.Append("/");
			}

			dateString.Append(dateOfBirth.Year.ToString());
			return dateString.ToString();
		}

		public void SelectPersonalInfoGender(Gender? gender)
		{
			if (gender.HasValue)
			{
				UIUtil.DefaultProvider.SelectWithText(UIUtil.DefaultProvider.GetAttribute("//*[text()='Gender:']", "for", LocateBy.XPath), gender.Value.ToString(), LocateBy.Id);
			}
		}

		public void SelectPersonalInfoEnduranceNationality(string nationality)
		{
			if (!string.IsNullOrEmpty(nationality))
			{
				UIUtil.DefaultProvider.SelectWithText(UIUtil.DefaultProvider.GetAttribute("//*[text()='Nationality:']", "for", LocateBy.XPath), nationality, LocateBy.Id);
			}
		}

		public void TypePersonalInfoTaxNumber(string taxNumber)
		{
			if (!string.IsNullOrEmpty(taxNumber))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Tax Identification Number:']", "for", LocateBy.XPath), taxNumber, LocateBy.Id);
			}
		}

		public void TypePersonalInfoSocialSecurityNumber(string ssn)
		{
			if (!string.IsNullOrEmpty(ssn))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Social Security Number:']", "for", LocateBy.XPath), ssn, LocateBy.Id);
			}
		}

		public void TypePersonalInfoContactName(string contactName)
		{
			if (!string.IsNullOrEmpty(contactName))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Emergency Contact Name:']", "for", LocateBy.XPath), contactName, LocateBy.Id);
			}
		}

		public void TypePersonalInfoContactPhone(string contactPhone)
		{
			if (!string.IsNullOrEmpty(contactPhone))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Emergency Contact Phone:']", "for", LocateBy.XPath), contactPhone, LocateBy.Id);
			}
		}

		public void TypePersonalInfoMembershipNumber(string membershipNumber)
		{
			if (!string.IsNullOrEmpty(membershipNumber))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Membership Number:']", "for", LocateBy.XPath), membershipNumber, LocateBy.Id);
			}
		}

		public void TypePersonalInfoCustomerNumber(string customerNumber)
		{
			if (!string.IsNullOrEmpty(customerNumber))
			{
				UIUtil.DefaultProvider.Type(UIUtil.DefaultProvider.GetAttribute("//*[text()='Customer Number:']", "for", LocateBy.XPath), customerNumber, LocateBy.Id);
			}
		}

		public void UploadPersonalInfoPhoto(string fileName)
		{
			if (!string.IsNullOrEmpty(fileName))
			{
				UIUtil.DefaultProvider.WaitForDisplayAndClick("auplPhoto", LocateBy.Id);
				UIUtil.DefaultProvider.SelectWindowByTitle("File Upload Page");
				UIUtil.DefaultProvider.WaitForPageToLoad();

				//WebDriverBase.WebDriverManager.driver.FindElement(By.Id("ctl00_cphBody_ruFilefile0")).SendKeys(
				//    Fixtures.ConfigurationProvider.XmlConfig.AllSettings.DataPath + fileName);

				UIUtil.DefaultProvider.SendKeys("ctl00_cphBody_ruFilefile0", ConfigReader.DefaultProvider.EnvironmentConfiguration.DataPath + fileName, LocateBy.Id);
				UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphBody_btnSubmit", LocateBy.Id);
				UIUtil.DefaultProvider.WaitForPageToLoad();
				UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphBody_btnCancel", LocateBy.Id);
				UIUtil.DefaultProvider.SelectOriginalWindow();
			}
		}

		public void TypePersonalInfoProxyName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				UIUtil.DefaultProvider.Type(RegisterSiteLocator.ContactInfoNameLocator, name, LocateBy.Id);
			}
		}
		
		public void TypePersonalInfoProxyPhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
			{
				UIUtil.DefaultProvider.Type(RegisterSiteLocator.ContactInfoPhoneLocator, phone, LocateBy.Id);
			}
		}
		
		public void TypePersonalInfoProxyEmail(string email)
		{
			if (!string.IsNullOrEmpty(email))
			{
				UIUtil.DefaultProvider.Type(RegisterSiteLocator.ContactInfoEmailLocator, email, LocateBy.Id);
			}
		}

		[Step]
		public void EnterPersonalInfoStatus(RegistrationStatus status)
		{
			UIUtil.DefaultProvider.SelectWithText(
				UIUtil.DefaultProvider.GetAttribute("//*[text()='Status:']", "for", LocateBy.XPath),
				StringEnum.GetStringValue(status), 
				LocateBy.Id);
		}

		[Step]
		public void ClickEditRegistrationType()
		{
			UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cph_lnkChangeRegType", LocateBy.Id);
			UIUtil.DefaultProvider.WaitForAJAXRequest();
			UIUtil.DefaultProvider.SelectIFrame(0);
		}

		[Verify]
		public void VerifyRegistrationTypeOptions(List<string> regTypes)
		{
			VerifyTool.VerifyList(regTypes, GetRegTypeOptions());
		}

		[Step]
		public void ChangeRegistrationType(string typeName)
		{
			UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format("//ol[@id='radRegTypes']/li/label[contains(text(),'{0}')]", typeName), LocateBy.XPath);
		}

		[Step]
		public void ConfirmChangingRegistrationType()
		{
			UIUtil.DefaultProvider.WaitForDisplayAndClick("//button[text()='OK']", LocateBy.XPath);
			UIUtil.DefaultProvider.SwitchToMainContent();
		}

		private List<string> GetRegTypeOptions()
		{
			List<string> types = new List<string>();
			string regTypeLocator = "//ol[@id='radRegTypes']";
			int count = UIUtil.DefaultProvider.GetXPathCountByXPath(regTypeLocator + "/li");
			string regTypeFormat = regTypeLocator + "/li[{0}]/label";

			for (int i = 1; i <= count; i++)
			{
				types.Add(UIUtil.DefaultProvider.GetText(string.Format(regTypeFormat, i), LocateBy.XPath).Trim());
			}

			return types;
		}

		private void VerifyPersonalInfoStandardFieldText(string label, string expectedValue)
		{
			string actualValue = UIUtil.DefaultProvider.GetValue(UIUtil.DefaultProvider.GetAttribute("//*[text()='" + label + ":']", "for", LocateBy.XPath), LocateBy.Id);
			Utilities.VerifyTool.VerifyValue(expectedValue, actualValue, label + " text : {0}");
		}

		public bool IsPersonalInfoRecalled(string firstName, string middleName)
		{
			string actualFirstName = UIUtil.DefaultProvider.GetText("//span[contains(text(),'First Name')]/../following-sibling::*", LocateBy.XPath);
			string actualMiddleName = UIUtil.DefaultProvider.GetText("//span[contains(text(),'Middle Name')]/../following-sibling::*", LocateBy.XPath);
			string password = UIUtil.DefaultProvider.GetAttribute("//*[@for='ctl00_cph_ctlPassword_txtPassword']/../following-sibling::*/input", "value", LocateBy.XPath);

            bool firstNameIsCorrect = actualFirstName.Contains(firstName);
            bool middleNameIsCorrect = actualMiddleName.Contains(middleName);
            bool hasPassword = (password != null);

            return firstNameIsCorrect && middleNameIsCorrect && hasPassword;
		}
		#endregion
	}
}
