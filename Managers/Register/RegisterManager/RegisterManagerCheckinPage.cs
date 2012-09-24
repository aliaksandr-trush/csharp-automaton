﻿namespace RegOnline.RegressionTest.Managers.Register
{
	using System;
	using NUnit.Framework;
	using RegOnline.RegressionTest.Utilities;
	//using RegOnline.RegressionTest.Fixtures;
	using RegOnline.RegressionTest.UIUtility;
	using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Attributes;

	public partial class RegisterManager : ManagerBase
	{
		#region Constants
		private const string CheckinErrorMessageDivLocator = "ctl00_valSummary";
		private const string CheckinErrorMessageLocator = "//div[@id='ctl00_valSummary']/ul/li";
		public const string CheckinEmailAddress = "ctl00_cph_ctlEmailMemID_txtEmail";
		public const string CheckinEmailAddressVerify = "//input[@id='ctl00_cph_ctlEmailMemID_txtVerifyEmail']";
		public const string CheckinMembershipId = "//input[@id='ctl00_cph_ctlEmailMemID_txtMemID']";
		public const string CheckinAlreadyRegistered = "ctl00_cph_lnkLogin";
		public const string CheckinEventFeeDiscountCode = "//input[@id='ctl00_cph_ctlRegType_txtDiscountCode']";
		public const string CheckinInvitationCode = "//input[@id='ctl00_cph_txtInvitationCode']";
		private const string CheckinRegTypesListLocator = "radRegTypes";
		private const string CheckinRegTypeInputLocatorFormat = "radRegType_{0}";
		public const string CheckinRegTypeDropDown = "//select[@id='ctl00_cph_ctlRegType_ddlRegistrantTypes']";
		public const string CheckinRegTypeDropDownFindByName = "/option[contains(text(),'{0}')]";
		public const string CheckinRegTypeRadioButton = "//li[@class='radioRight']";
		public const string CheckinRegTypeRadioButtonRows = "//ol[@id='radRegTypes']/li";
		public const string CheckinRegTypeRadioButtonRowsSelector = "//ol[@id='radRegTypes']/li[{0}]/input";
		public const string CheckinRegTypeRadioButtonFindByName = "/label[contains(text(),'{0}')]/..";
		private const string CheckinRegTypeLabelLocatorFormat = "//label[@for='radRegType_{0}']";
		public const string CheckinRegTypeRadioButtonInput = "/input";
		public const string CheckinRegTypeRadioButtonLabel = "/label";
		public const string CheckinRegTypeRadioButtonGroupSizeMssg = "/p";
		private const string CheckinRegTypeLimitReachedMessageLocatorFormat = "//input[@id='" + CheckinRegTypeInputLocatorFormat + "']/../p";
		public const string CheckinRegTypeGroupSizeMssgFormatMinAndMax = "You must register a group of at least {0} and no more than {1}.";
		public const string CheckinRegTypeGroupSizeMssgFormatJustMin = "You must register a group of {0} or more.";
		public const string CheckinRegTypeGroupSizeMssgFormatJustMax = "You must register a group of {0} or less.";
		private const string CheckinEventLimitReachedMessageLocator = "//div[@id = 'pageContent']/p";
		private const string CheckinEventLimitReachedMessageForRegTypeDirectLinkLocator = "//div[@id = 'pageContent']/p/strong";
		public const string CheckinEventAvailableMessage = "To get started, enter your email address.";
		public const string StartNewRegistration = "ctl00_cph_lnkNotRegistered";
		public const string ArchivedMessageOne = "You have reached this webpage using an incorrect address (URL).";
		public const string ArchivedMessageTwo = "Obtain the correct URL or contact the event organizer.";
		public const string RegisterNow = "aRegBtn";
		private const string AddedToWaitlistOfEvent = "//*[text()='You have been added to the waitlist for this event.']";
		private const string PasswordTextboxOnLoginPage = "ctl00_cph_txtPassword";
		private const string ForgetYourPasswordLinkLocator = "//input[@id='" + PasswordTextboxOnLoginPage + "']/following-sibling::a[text()='Forgot Your Password?']";
		private const string LocatorFormat_RegtypeRadioItem = "//ol[@id='radRegTypes']/li/label[contains(text(),'{0}')]/../input[@name='radRegType']";
		private const string RegTypeDetailsLocator = "//label[contains(text(),'{0}')]/span/a";
		#endregion

		#region Checkin Page
		public void VerifyOnCheckinPage(bool expected)
		{
			bool actual = this.OnCheckinPage();

			if (expected != actual)
			{
				if (expected)
				{
					UIUtil.DefaultProvider.FailTest("NOT on checkin page!");
				}
				else
				{
					UIUtil.DefaultProvider.FailTest("ON checkin page!");
				}
			}
		}

		[Step]
		public bool OnCheckinPage()
		{
			bool onCheckin = UIUtil.DefaultProvider.UrlContainsPath("checkin.aspx");
			return onCheckin;
		}

		public bool OnEventWebsite()
		{
			bool onEventWebsite = UIUtil.DefaultProvider.UrlContainsPath("/builder/site/Default.aspx");
			return onEventWebsite; 
		}

		public void ClickRegisterNowButton()
		{
			UIUtil.DefaultProvider.WaitForDisplayAndClick(RegisterNow, LocateBy.Id);
			UIUtil.DefaultProvider.WaitForPageToLoad();
		}
		[Step]
		public string ComposeUniqueEmailAddress()
		{
			string uniqueEmail = "selenium{0}@regonline.com";
			CurrentTicks = System.DateTime.Now.Ticks;
			return string.Format(uniqueEmail, CurrentTicks.ToString());
		}

		/// <summary>
		/// For when you don't care what the email is until after you check in
		/// </summary>
		/// <returns>Unique email for re-checking-in</returns>
		[Step]
		public string Checkin()
		{
			string email = this.ComposeUniqueEmailAddress();
			Checkin(email);
			return email;
		}

		[Step]
		public void Checkin(string email)
		{
			CheckinWithEmail(email);
		}

		public bool DoesCheckinAcceptEmail()
		{
			bool hasEmail = false;
			Assert.IsTrue(OnCheckinPage());

			if (UIUtil.DefaultProvider.IsElementPresent(CheckinEmailAddress, LocateBy.Id))
			{
				hasEmail = true;
			}

			return hasEmail;
		}

		public bool IsCheckinEmailFieldPresent()
		{
			bool hasEmail = false;
			
			if (UIUtil.DefaultProvider.IsElementPresent(CheckinEmailAddress, LocateBy.Id))
			{
				hasEmail = true;
			}

			return hasEmail;
		}

		public bool DoesCheckinAcceptMembershipId()
		{
			bool hasMemId = false;
			Assert.IsTrue(OnCheckinPage());

			if (UIUtil.DefaultProvider.IsElementPresent(CheckinMembershipId, LocateBy.XPath))
			{
				hasMemId = true;
			}

			return hasMemId;
		}

		[Step]
		public void CheckinWithEmail(string email)
		{
			ClickStartNewRegistration();
			EnterEmailAddress(email);
			CurrentTicks = System.DateTime.Now.Ticks;
			this.CurrentEmail = email;

			if (UIUtil.DefaultProvider.IsElementDisplay(CheckinEmailAddressVerify, LocateBy.XPath))
			{
				EnterVerifyEmailAddress(email);
			}
		}

		[Step]
		public void EnterEmailAddress(string email)
		{
			UIUtil.DefaultProvider.Type(CheckinEmailAddress, email, LocateBy.Id);
		}

		public bool IsVerifyEmailAddressPresent()
		{
			return UIUtil.DefaultProvider.IsElementPresent(CheckinEmailAddressVerify, LocateBy.XPath);
		}

		[Step]
		public void EnterVerifyEmailAddress(string email)
		{
			UIUtil.DefaultProvider.Type(CheckinEmailAddressVerify, email, LocateBy.XPath);
		}

		[Step]
		public void TypeLoginPagePassword()
		{
			TypeLoginPagePassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
		}

		[Step]
		public void TypeLoginPagePassword(string password)
		{
			UIUtil.DefaultProvider.Type(PasswordTextboxOnLoginPage, password, LocateBy.Id);
		}

		public void CheckinWithMembershipId(string memId)
		{
			UIUtil.DefaultProvider.Type(CheckinMembershipId, memId, LocateBy.XPath);
			CurrentEmail = string.Empty;
		}

		[Step]
		public void ClickStartNewRegistration()
		{
			if (OnLoginPage())
			{
				UIUtil.DefaultProvider.WaitForDisplayAndClick(StartNewRegistration, LocateBy.Id);
				UIUtil.DefaultProvider.WaitForPageToLoad();
			}

			AllowCookies();
		}

		public bool HasEventDiscountCodeField()
		{
			return UIUtil.DefaultProvider.IsElementPresent(CheckinEventFeeDiscountCode, LocateBy.XPath);
		}

		public void EnterEventDiscoutCode(string discountCode)
		{
			UIUtil.DefaultProvider.Type(CheckinEventFeeDiscountCode, discountCode, LocateBy.XPath);
		}

		public bool IsCodeRequired()
		{
			return UIUtil.DefaultProvider.IsElementPresent("//img[@alt='Required']/..[following-sibling::*[text()='Enter a discount code:']]", LocateBy.XPath);
		}

		public void TypeInvitationCode(string invitationCode)
		{
			UIUtil.DefaultProvider.Type(CheckinInvitationCode, invitationCode, LocateBy.XPath);
		}

		[Step]
		public void ClickCheckinAlreadyRegistered()
		{
			UIUtil.DefaultProvider.WaitForDisplayAndClick(CheckinAlreadyRegistered, LocateBy.Id);
			UIUtil.DefaultProvider.WaitForPageToLoad(TimeSpan.FromMinutes(1));
		}

		public bool HasCheckinErrorMessage()
		{
			return UIUtil.DefaultProvider.IsElementPresent(CheckinErrorMessageLocator, LocateBy.XPath);
		}

		public void VerifyHasCheckinErrorMessage(bool expected)
		{
			////VerifyValue(expected, this.HasCheckinErrorMessage(), "Checkin error message exist : {0}");

			if (expected != this.HasCheckinErrorMessage())
			{
				if (expected)
				{
					UIUtil.DefaultProvider.FailTest("There is no checkin error message!");
				}
				else
				{
					UIUtil.DefaultProvider.FailTest(string.Format("Got checkin error message: {0}", this.GetCheckinErrorMessage()));
				}
			}
		}

		public void VerifyCheckinErrorMessage(string expectedMessage)
		{
			VerifyTool.VerifyValue(expectedMessage, GetCheckinErrorMessage(), "Checkin error message: {0}");
		}

		[Verify]
		public void VerifyForgotYourPasswordLinkVisibility(bool isVisible)
		{
			VerifyTool.VerifyValue(isVisible, UIUtil.DefaultProvider.IsElementDisplay(ForgetYourPasswordLinkLocator, LocateBy.XPath), "Has Forgot Your Password Link: {0}");
		}

		public void VerifyLoginPageForgotYourPasswordLinkURL(string expURL)
		{
			VerifyTool.VerifyValue(expURL, UIUtil.DefaultProvider.GetAttribute(ForgetYourPasswordLinkLocator, "href", LocateBy.XPath), "Forgot Your Password Link URL: {0}");
		}

		public void ClickLoginPageForgotYourPasswordLinkAndVerify(string expectedURL)
		{
			UIUtil.DefaultProvider.WaitForDisplayAndClick(ForgetYourPasswordLinkLocator, LocateBy.XPath);
			UIUtil.DefaultProvider.SelectTopWindow();
			UIUtil.DefaultProvider.WaitForPageToLoad();
			VerifyTool.VerifyValue(expectedURL, UIUtil.DefaultProvider.GetLocation(), "Opened window's URL: {0}");
			UIUtil.DefaultProvider.ClosePopUpWindow();
			UIUtil.DefaultProvider.SelectTopWindow();
		}

		public string ClickLoginPageForgotYourPasswordLinkAndGetEmail()
		{
			UIUtil.DefaultProvider.WaitForDisplayAndClick(ForgetYourPasswordLinkLocator, LocateBy.XPath);
			UIUtil.DefaultProvider.WaitForPageToLoad();
			UIUtil.DefaultProvider.SelectIFrame(0);
			string email = UIUtil.DefaultProvider.GetAttribute(CheckinEmailAddress, "value", LocateBy.Id);
			UIUtil.DefaultProvider.Click("//*[@id='ctl00_cph_wrpEmailMembershipID']/div/a", LocateBy.XPath);

			return email;
		}

		public string GetCheckinErrorMessage()
		{
			return UIUtil.DefaultProvider.GetText(CheckinErrorMessageLocator, LocateBy.XPath);
		}

		public void VerifyEventLimitReachedMessage(string expMessage)
		{
			string actMessage = GetEventLimitReachedMessage();
			VerifyTool.VerifyValue(expMessage, actMessage, "The event full message is: {0}");
		}

		public void VerifyHasEventLimitReachedMessage(bool expected)
		{
			VerifyTool.VerifyValue(expected, this.HasEventLimitReachedMessage(), "The event limit reached message exists: {0}");
		}

		public bool HasEventLimitReachedMessage()
		{
			return UIUtil.DefaultProvider.IsElementPresent(CheckinEventLimitReachedMessageLocator, LocateBy.XPath);
		}

		public bool IsAddedToWaitlist()
		{
			return UIUtil.DefaultProvider.IsElementPresent(AddedToWaitlistOfEvent, LocateBy.XPath);
		}

		public string GetEventLimitReachedMessage()
		{
			UIUtil.DefaultProvider.WaitForElementPresent(CheckinEventLimitReachedMessageLocator, LocateBy.XPath);

			if (UIUtil.DefaultProvider.IsElementPresent(CheckinEventLimitReachedMessageForRegTypeDirectLinkLocator, LocateBy.XPath))
			{
				return UIUtil.DefaultProvider.GetText(CheckinEventLimitReachedMessageForRegTypeDirectLinkLocator, LocateBy.XPath);
			}
			else
			{
				return UIUtil.DefaultProvider.GetText(CheckinEventLimitReachedMessageLocator, LocateBy.XPath);
			}
		}

		public bool VerifyEventIsArchivedStatus()
		{
			UIUtil.DefaultProvider.WaitForPageToLoad();

			if (ArchivedMessageOne.Equals(UIUtil.DefaultProvider.GetText("//p[2]", LocateBy.XPath)) && ArchivedMessageTwo.Equals(UIUtil.DefaultProvider.GetText("//p[4]", LocateBy.XPath)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		#endregion

		#region Checkin RegTypes
		public bool IsRegTypeAvailable(int regTypeID)
		{
			return UIUtil.DefaultProvider.IsElementPresent(this.Compose_XPath_CheckinRegTypeGroupSizeMessageLocator(regTypeID), LocateBy.XPath);
		}

		public void VerifyRegTypeAvailability(int regTypeID, bool available)
		{
			if (this.IsRegTypeAvailable(regTypeID) != available)
			{
				Assert.Fail(string.Format(
					"RegType '{0}:{1}' is " + (available ? "un" : string.Empty) + "available!",
					regTypeID,
					this.GetRegTypeLabel(regTypeID)));
			}
		}

		public bool HasRegTypeList()
		{
			return UIUtil.DefaultProvider.IsElementPresent(CheckinRegTypesListLocator, LocateBy.Id);
		}

		public bool HasRegType(int regTypeID)
		{
			return UIUtil.DefaultProvider.IsElementPresent(this.ComposeRegTypeInputLocator(regTypeID), LocateBy.Id);
		}

		public string ComposeRegTypeInputLocator(int regTypeID)
		{
			return string.Format(CheckinRegTypeInputLocatorFormat, regTypeID);
		}

		public string ComposeRegTypeLabelLocator(int regTypeID)
		{
			return string.Format(CheckinRegTypeLabelLocatorFormat, regTypeID);
		}

		public string GetRegTypeLabel(int regTypeID)
		{
			return UIUtil.DefaultProvider.GetText(this.ComposeRegTypeLabelLocator(regTypeID), LocateBy.XPath);
		}

		public int CountRegTypes()
		{
			int count = 0;

			if (HasRegTypeRadioButton())
			{
				count = UIUtil.DefaultProvider.GetXPathCountByXPath(CheckinRegTypeRadioButton + CheckinRegTypeRadioButtonRows);
			}
			else if (HasRegTypeDropDown())
			{
				count = UIUtil.DefaultProvider.GetXPathCountByXPath(CheckinRegTypeDropDown + "/option") - 1;
			}

			return count;
		}

		public void SelectRegTypeByIndex(int index)
		{
			UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format(CheckinRegTypeRadioButtonRowsSelector, index), LocateBy.XPath);
		}

		public void VerifyHasRegTypes(bool hasRegTypes)
		{
			if (this.HasRegTypes() != hasRegTypes)
			{
				Assert.Fail((hasRegTypes ? "No " : string.Empty) + "RegTypes found!");
			}
		}

		public bool HasRegTypes()
		{
			bool regTypes = false;

			if ((this.HasRegTypeDropDown()) || (this.HasRegTypeRadioButton()))
			{
				regTypes = true;
			}

			return regTypes;
		}

		public bool HasRegTypeRadioButton()
		{
			bool hasRadioButton = false;

			hasRadioButton = UIUtil.DefaultProvider.IsElementPresent(CheckinRegTypeRadioButton, LocateBy.XPath);

			return hasRadioButton;
		}

		public bool HasRegTypeDropDown()
		{
			bool hasDropDown = false;

			hasDropDown = UIUtil.DefaultProvider.IsElementPresent(CheckinRegTypeDropDown, LocateBy.XPath);

			return hasDropDown;
		}

		public void SelectRegTypeRadioButton(string regTypeName)
		{
			////string locator = CheckinRegTypeRadioButton + CheckinRegTypeRadioButtonRows;
			////locator += string.Format(CheckinRegTypeRadioButtonFindByName, regTypeName);
			////locator += CheckinRegTypeRadioButtonInput;

			string locator = string.Format(RegisterSiteLocator.CheckinRegTypeLabelFormat, regTypeName);

			if (UIUtil.DefaultProvider.IsElementPresent(locator, LocateBy.XPath))
			{
				UIUtil.DefaultProvider.WaitForDisplayAndClick(locator, LocateBy.XPath);
			}
			else
			{
				UIUtil.DefaultProvider.FailTest("RegType '" + regTypeName + "' not found!");
			}
		}

		public void SelectRegTypeDropDown(string regTypeName)
		{
			string locator = string.Format(CheckinRegTypeDropDown + CheckinRegTypeDropDownFindByName, regTypeName);

			if (UIUtil.DefaultProvider.IsElementPresent(locator, LocateBy.XPath))
			{
				UIUtil.DefaultProvider.SelectWithText(CheckinRegTypeDropDown, regTypeName, LocateBy.XPath);
			}
			else
			{
				Assert.Fail("RegType '" + regTypeName + "' not found!");
			}
		}

		[Step]
		public void SelectRegType(string regTypeName)
		{
			if (this.HasRegTypeRadioButton())
			{
				this.SelectRegTypeRadioButton(regTypeName);
			}
			else if (this.HasRegTypeDropDown())
			{
				this.SelectRegTypeDropDown(regTypeName);
			}
		}

		public void VerifyRegTypeDetails(string regTypeName, string details)
		{
			UIUtil.DefaultProvider.WaitForDisplayAndClick(string.Format(RegTypeDetailsLocator, regTypeName), LocateBy.XPath);
			
			string detailsString = UIUtil.DefaultProvider.GetText(
				"//*[@class='tooltipWrapper tooltipLightbox ui-dialog-content ui-widget-content'][last()]//*[@class='tooltipWrapperContent']", 
				LocateBy.XPath);

			Assert.True(detailsString.Contains(details));
			UIUtil.DefaultProvider.Click("//*[text()='close']", LocateBy.XPath);
		}

		public void VerifyCountOfRegTypes(int expectQuantity)
		{
			VerifyTool.VerifyValue(expectQuantity, CountRegTypes(), "Count of Reg Types: {0}");
		}

		private string Compose_XPath_CheckinRegTypeGroupSizeMessageLocator(string regType)
		{
			//string locator;

			//locator = CheckinRegTypeRadioButton + CheckinRegTypeRadioButtonRows;
			//locator += string.Format(CheckinRegTypeRadioButtonFindByName, regType);
			//locator += CheckinRegTypeRadioButtonGroupSizeMssg;

			int regTypeID = Fetch_RegTypeID(this.CurrentEventId, regType);
			string locator = string.Format(CheckinRegTypeLabelLocatorFormat, regTypeID);
			locator += "/following-sibling::p";

			return locator;
		}

		private string Compose_XPath_CheckinRegTypeGroupSizeMessageLocator(int regTypeID)
		{
			return string.Format(CheckinRegTypeLimitReachedMessageLocatorFormat, regTypeID);
		}

		public bool HasCheckinRegTypeGroupSizeMessage(string regType)
		{
			string locator = this.Compose_XPath_CheckinRegTypeGroupSizeMessageLocator(regType);

			return UIUtil.DefaultProvider.IsElementPresent(locator, LocateBy.XPath);
		}

		public string GetCheckinRegTypeGroupSizeMessage(string regType)
		{
			string locator = this.Compose_XPath_CheckinRegTypeGroupSizeMessageLocator(regType);

			return UIUtil.DefaultProvider.GetText(locator, LocateBy.XPath);
		}

		public void VerifyCheckinRegTypeGroupSizeMessage(string regType, int? min, int? max)
		{
			string actualMssg = string.Empty;
			string expectedMssg = string.Empty;

			if (this.HasCheckinRegTypeGroupSizeMessage(regType))
			{
				actualMssg = this.GetCheckinRegTypeGroupSizeMessage(regType);
			}

			if (min != null)
			{
				if (max != null)
				{
					//Expect both min and max
					expectedMssg = string.Format(CheckinRegTypeGroupSizeMssgFormatMinAndMax, min, max);
				}
				else
				{
					//Expect min but no max
					expectedMssg = string.Format(CheckinRegTypeGroupSizeMssgFormatJustMin, min);
				}
			}
			else
			{
				if (max != null)
				{
					//Expect max but no min
					expectedMssg = string.Format(CheckinRegTypeGroupSizeMssgFormatJustMax, max);
				}
				else
				{
					//Expect no min and no max, so leave as empty string.
				}
			}

			VerifyTool.VerifyValue(expectedMssg, actualMssg, "Group size message for " + regType + ": {0}");
		}

		public bool IsRegTypeEditable(string regTypeName)
		{
			return UIUtil.DefaultProvider.IsEditable(string.Format(LocatorFormat_RegtypeRadioItem, regTypeName), LocateBy.XPath);
		}
		#endregion
	}
}
