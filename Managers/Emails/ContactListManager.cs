namespace RegOnline.RegressionTest.Managers.Emails
{
    using System;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public partial class EmailManager : ManagerBase
    {
        #region locators/helpers
        private const string CreateNewContactListLocator = "ctl00_ctl00_cphDialog_cpMgrMain_hlCreateNewList";
        private const string ListNameLocator = "ctl00_cphDialog_txtName";
        private const string ContactListTemsLocator = "ctl00_cphDialog_chkCertify";
        private const string MethodLocatorConstructor = "ctl00_cphDialog_rblMethod_{0}";
        private const string ContactListStartNextLocator = "ctl00_cphDialog_btnNext";
        private const string ImportFileLocator = "ctl00_cphDialog_wzImport_inptFile";
        private const string StartContactListWizardNextLocator = "ctl00_cphDialog_wzImport_StartNavigationTemplateContainerID_btnNext";
        private const string ContactListWizardNextLocator = "ctl00_cphDialog_wzImport_StepNavigationTemplateContainerID_btnNext";
        private const string TopRowHeadersLocator = "ctl00_cphDialog_wzImport_chkHasHeaders";
        private const string FieldMapConstructor = "map_{0}";
        private const string ContactListWizardCloseLocator = "Close";
        private const string ContactListLocator = "{0}";
        private const string EmailAndContactListReturnLocator = "//div[@id='divBreadCrumbs']//a[1]";
        private const string ContactListDetailsReturnLocator = "//div[@id='divBreadCrumbs']//a[2]";
        private const string FirstEmailLocator = "//a[@id='ctl00_ctl00_cphDialog_cpMgrMain_clAlpha_gvContacts_ctl{0}_hlDetails']/../following-sibling::td[1]";
        private const string FirstNameLocator = "ctl00_ctl00_cphDialog_cpMgrMain_clAlpha_gvContacts_ctl{0}_hlDetails";
        private const string TotalLocator = "//div[@id='divGridFooterLeft']";
        private const string CorrectUploadTextLocator = "ctl00_cphDialog_wzImport_rptRows_ctl0{0}_txtEmail";
        private const string AddCorrectedContactLocator = "ctl00_cphDialog_wzImport_rptRows_ctl0{0}_lnkAdd";
        private const string RemoveContactWithErrorLocator = "ctl00_cphDialog_wzImport_rptRows_ctl0{0}_lnkDelete";
        private const string ImportedTotalLocator = "cntImported";
        private const string TotalNewContactsLocator = "cntTotal";
        private const string ErrorCountLcoator = "cntErr";
        private const string CloseLocator = "Close";
        private const string DeleteLocator = "Delete";
        private const string DeleteListLocator = "Delete List";
        private const string FixErrorsLocator = "Fix Errors";
        private const string DownloadErrorReportLocator = "Download Error Report";
        private const string SuccessfulMessage = "Your contact list was successfully created.";
        private const string SuccessWithErrorMessage = "Your contact list was successfully created, but there were some contacts with errors.";
        private const string AddContactLocator = "Add Contact";
        private const string ContactDetailsLocator = "//span[text()='{0}:']/following-sibling::span";
        private const string EditFieldsLinkLocator = "Edit Fields";
        private const string FieldVisibleLocator = "//span/label[contains(text(),'{0}')]/following-sibling::input[1]";
        private const string FieldRequiredLocator = "//span/label[contains(text(),'{0}')]/following-sibling::input[2]";
        private const string AdditionalFieldVisibleLocator = "//input[@value='{0}']/following-sibling::input[1]";
        private const string AdditionalFieldRequiredLocator = "//input[@value='{0}']/following-sibling::input[2]";
        private const string CloseEditFieldsLocator = "Close";
        private const string ExpandContactListsLocator = "//*[@id='ctl00_ctl00_cphDialog_cpMgrMain_egvContactLists']//a[contains(text(),'Show')]";
        private const string ContactEmailLocator = "ctl00_cphDialog_tbrEmail_txtValue";
        private const string ContactFirstNameLocator = "ctl00_cphDialog_tbrFirstName_txtValue";
        private const string ContactLastNameLocator = "ctl00_cphDialog_tbrLastName_txtValue";
        private const string ContactCompanyLocator = "ctl00_cphDialog_tbrCompany_txtValue";
        private const string SaveCloseContactInfoLocator = "//span[contains(text(),'Save & Close')]/..";

        public enum ContactMethod
        {
            Import,
            Manual,
            Filtered
        }

        public enum FieldMappingOptions
        {
            [StringValue("-- Select Column Header --")]
            SelectHeader,
            [StringValue("-- Do Not Import --")]
            DoNotImport,
            [StringValue("Email")]
            Email,
            [StringValue("CCEmail")]
            CCEmail,
            [StringValue("OptOut")]
            OptOut,
            [StringValue("Password")]
            Password,
            [StringValue("Prefix")]
            Prefix,
            [StringValue("First Name")]
            FirstName,
            [StringValue("Middle Name")]
            MiddleName,
            [StringValue("Last Name")]
            LastName,
            [StringValue("Suffix")]
            Suffix,
            [StringValue("Title")]
            Title,
            [StringValue("Company")]
            Company,
            [StringValue("Address Line 1")]
            AddressLine1,
            [StringValue("Address Line 2")]
            AddressLine2,
            [StringValue("Address Line 3")]
            AddressLine3,
            [StringValue("City")]
            City,
            [StringValue("State")]
            State,
            [StringValue("Zip")]
            Zip,
            [StringValue("Country")]
            Country,
            [StringValue("Work Phone")]
            WorkPhone,
            [StringValue("Work Extension")]
            WorkExtension,
            [StringValue("Home Phone")]
            HomePhone,
            [StringValue("Cell Phone")]
            CellPhone,
            [StringValue("Fax")]
            Fax,
            [StringValue("SSN")]
            SSN,
            [StringValue("Membership Id")]
            MembershipId,
            [StringValue("Customer Id")]
            CustomerId,
            [StringValue("Badge Name")]
            BadgeName,
            [StringValue("Contact Name")]
            ContactName,
            [StringValue("Contact Phone")]
            ContactPhone,
            [StringValue("Contact Email")]
            ContactEmail,
            [StringValue("Emergency Contact Name")]
            EmergencyContactName,
            [StringValue("Emergency Contact Phone")]
            EmergencyContactPhone,
            [StringValue("Additional Field 1")]
            AdditionalField1,
            [StringValue("Additional Field 2")]
            AdditionalField2,
            [StringValue("Additional Field 3")]
            AdditionalField3,
            [StringValue("Additional Field 4")]
            AdditionalField4,
            [StringValue("Additional Field 5")]
            AdditionalField5,
            [StringValue("Additional Field 6")]
            AdditionalField6,
            [StringValue("Additional Field 7")]
            AdditionalField7,
            [StringValue("Additional Field 8")]
            AdditionalField8,
            [StringValue("Additional Field 9")]
            AdditionalField9,
            [StringValue("Additional Field 10")]
            AdditionalField10,
            [StringValue("Date of Birth")]
            DateOfBirth,
        }

        public enum ManualFieldOptions
        {
            [StringValue("Email Address")]
            EmailAddress,
            [StringValue("CC Email")]
            CC_Email,
            [StringValue("Password")]
            Password,
            [StringValue("Prefix")]
            Prefix,
            [StringValue("First Name")]
            FirstName,
            [StringValue("Middle Name")]
            MiddleName,
            [StringValue("Last Name")]
            LastName,
            [StringValue("Suffix")]
            Suffix,
            [StringValue("Title")]
            Title,
            [StringValue("Company")]
            Company,
            [StringValue("Address 1")]
            AddressLine1,
            [StringValue("Address 2")]
            AddressLine2,
            [StringValue("Address 3")]
            AddressLine3,
            [StringValue("City")]
            City,
            [StringValue("State (Province)")]
            State,
            [StringValue("Zip (Postal Code)")]
            Zip,
            [StringValue("Country")]
            Country,
            [StringValue("Work Phone")]
            WorkPhone,
            [StringValue("Extension")]
            WorkExtension,
            [StringValue("Home Phone")]
            HomePhone,
            [StringValue("Cell Phone")]
            CellPhone,
            [StringValue("Fax")]
            Fax,
            [StringValue("SSN")]
            SSN,
            [StringValue("Membership ID")]
            MembershipId,
            [StringValue("Customer ID")]
            CustomerId,
            [StringValue("Badge Name")]
            BadgeName,
            [StringValue("Contact Name")]
            ContactName,
            [StringValue("Contact Phone")]
            ContactPhone,
            [StringValue("Contact Email")]
            ContactEmail,
            [StringValue("Emergency Name")]
            EmergencyContactName,
            [StringValue("Emergency Phone")]
            EmergencyContactPhone,
            [StringValue("AF1")]
            AdditionalField1,
            [StringValue("AF2")]
            AdditionalField2,
            [StringValue("AF3")]
            AdditionalField3,
            [StringValue("AF4")]
            AdditionalField4,
            [StringValue("AF5")]
            AdditionalField5,
            [StringValue("AF6")]
            AdditionalField6,
            [StringValue("AF7")]
            AdditionalField7,
            [StringValue("AF8")]
            AdditionalField8,
            [StringValue("AF9")]
            AdditionalField9,
            [StringValue("AF10")]
            AdditionalField10,
            [StringValue("Date Of Birth")]
            DateOfBirth,
        }

        #endregion

        private const int TimeOutInMinutes_Longer = 3;

        #region Upload List Methods
        public void ClickCreateNewContactList()
        {
            if (OnEmailSplashPage())
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(EmailSplashPageLocator + "//*[span='Create Contact List']", LocateBy.XPath);
            }
            else
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(CreateNewContactListLocator, LocateBy.Id);
            }

            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("plain");
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            // TODO: Remove this and change the call to clickandwaitrad once we can check in changes to the JS for rad windows in M3
            Utility.ThreadSleep(2);
        }

        /// <summary>
        /// Chooses method of uploading a contact list
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="termsAgree"></param>
        /// <param name="method"></param>
        public void CreateContactListSetup(string listName, bool termsAgree, ContactMethod method)
        {
            //UIUtilityProvider.UIHelper.SelectPopUpFrame("plain");
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.Type(ListNameLocator, listName, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox(ContactListTemsLocator, termsAgree, LocateBy.Id);

            switch(method)
            {
                case ContactMethod.Import:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(MethodLocatorConstructor, 0), LocateBy.Id);
                    break;
                case ContactMethod.Manual:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(MethodLocatorConstructor, 1), LocateBy.Id);
                    break;
                case ContactMethod.Filtered:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(MethodLocatorConstructor, 2), LocateBy.Id);
                    break;
            }

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ContactListStartNextLocator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        /// <summary>
        /// Maps a field for uploading a contact list, it is done a zero based index
        /// </summary>
        /// <param name="locationIndex">zero based index left to right</param>
        /// <param name="option">Use RegOnline.RegressionTests.Manager.Emails.EmailManager.FieldMappingOptions</param>
        public void MapFields(int locationIndex, FieldMappingOptions fieldToMap)
        {
            UIUtilityProvider.UIHelper.SelectWithText(string.Format(FieldMapConstructor, locationIndex), StringEnum.GetStringValue(fieldToMap), LocateBy.Id);
        }

        /// <summary>
        /// Actually Uploads the list, uses sleep due to lack of consistency from WaitForPageToLoad and AJAX... 
        /// </summary>
        public void NextStep()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ContactListWizardNextLocator, LocateBy.Id);
            int i = 0;

            while (i < 210)
            {
                try
                {
                    UIUtilityProvider.UIHelper.WaitForElementPresent(CloseLocator, LocateBy.LinkText);

                    if (UIUtilityProvider.UIHelper.IsElementPresent(CloseLocator, LocateBy.LinkText))
                    {
                        i = 210;
                    }
                }
                catch
                {
                    //ignore
                    Utility.ThreadSleep(10);
                    i += 10;
                }
            }
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        /// <summary>
        /// Closes the wizard
        /// </summary>
        public void CloseContactListImport()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ContactListWizardCloseLocator, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        /// <summary>
        /// Clicks fix errors, if necessary, and corrects the field needed
        /// </summary>
        /// <param name="correctText"></param>
        /// <param name="index">zero-based</param>
        /// <param name="expectedNewContacts"></param>
        /// <param name="expectedTotalContacts"></param>
        /// <param name="errorsRemaining"></param>
        public void CorrectUploadErrors(string correctText, int index, int expectedNewContacts, int expectedTotalContacts, int errorsRemaining)
        {
            UIUtilityProvider.UIHelper.SetTimeoutSpan(TimeSpan.FromMinutes(TimeOutInMinutes_Longer));
            UIUtilityProvider.UIHelper.WaitForElementDisplay(CloseLocator, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SetTimeoutSpan();
            Assert.True(UIUtilityProvider.UIHelper.IsTextPresent(SuccessWithErrorMessage));
            VerifyUploadedContactCounts(expectedNewContacts, expectedTotalContacts, errorsRemaining + 1);

            if (UIUtilityProvider.UIHelper.IsElementDisplay(FixErrorsLocator, LocateBy.LinkText))
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(FixErrorsLocator, LocateBy.LinkText);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            }

            UIUtilityProvider.UIHelper.Type(string.Format(CorrectUploadTextLocator, index), correctText, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(AddCorrectedContactLocator, index), LocateBy.Id);
            Utility.ThreadSleep(1.5);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            VerifyUploadedContactCounts(expectedNewContacts + 1, expectedTotalContacts + 1, errorsRemaining); 
        }

        /// <summary>
        /// Removes a contact that had an error, must have already clicked fix errors
        /// </summary>
        /// <param name="expectedNewContacts"></param>
        /// <param name="expectedTotalContacts"></param>
        /// <param name="index">zero-based</param>
        /// <param name="errorsRemaining"></param>
        public void RemoveContactWithError(int expectedNewContacts, int expectedTotalContacts, int index, int errorsRemaining)
        {
            VerifyUploadedContactCounts(expectedNewContacts, expectedTotalContacts, (errorsRemaining + 1));
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(RemoveContactWithErrorLocator, index), LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            VerifyUploadedContactCounts(expectedNewContacts, expectedTotalContacts, errorsRemaining); 
        }

        /// <summary>
        /// Confrims list uploaded and that the error report exists. 
        /// </summary>
        /// <param name="expectedNewContacts"></param>
        /// <param name="expectedTotalContacts"></param>
        /// <param name="expectedErrors"></param>
        public void ConfirmContactListUploadWithErrorReport(int expectedNewContacts, int expectedTotalContacts, int expectedErrors)
        {
            UIUtilityProvider.UIHelper.SetTimeoutSpan(TimeSpan.FromMinutes(TimeOutInMinutes_Longer));
            UIUtilityProvider.UIHelper.WaitForElementDisplay(CloseLocator, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SetTimeoutSpan();
            Assert.True(UIUtilityProvider.UIHelper.IsTextPresent(SuccessWithErrorMessage));
            Assert.True(!UIUtilityProvider.UIHelper.IsElementHidden(DownloadErrorReportLocator, LocateBy.LinkText));
            VerifyUploadedContactCounts(expectedNewContacts, expectedTotalContacts, expectedErrors);
        }

        /// <summary>
        /// Verify list is uploaded as expected
        /// </summary>
        /// <param name="expectedNewContacts"></param>
        /// <param name="expectedTotalContacts"></param>
        public void ConfirmContactListUpload(int expectedNewContacts, int expectedTotalContacts)
        {
            UIUtilityProvider.UIHelper.SetTimeoutSpan(TimeSpan.FromMinutes(TimeOutInMinutes_Longer));
            UIUtilityProvider.UIHelper.WaitForElementDisplay(CloseLocator, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SetTimeoutSpan();
            Assert.True(UIUtilityProvider.UIHelper.IsTextPresent(SuccessfulMessage));
            VerifyUploadedContactCounts(expectedNewContacts, expectedTotalContacts, null); 
        }

        /// <summary>
        /// Verifies the count of uploaded contacts: new contacts, errors and total
        /// </summary>
        /// <param name="expectedNewContacts">First Count on page New Contacts</param>
        /// <param name="expectedTotalContacts">Errors, if any, nullable</param>
        /// <param name="expectedErrorCount">Total Contacts at the bottom</param>
        public void VerifyUploadedContactCounts(int expectedNewContacts, int expectedTotalContacts, int? expectedErrorCount)
        {
            Utilities.VerifyTool.VerifyValue(expectedNewContacts.ToString(), UIUtilityProvider.UIHelper.GetText(ImportedTotalLocator, LocateBy.Id), "New Contacts: {0}");

            if (expectedErrorCount.HasValue)
            {
                Utilities.VerifyTool.VerifyValue(expectedErrorCount.ToString(), UIUtilityProvider.UIHelper.GetText(ErrorCountLcoator, LocateBy.Id), "Total Errors: {0}"); 
            }

            Utilities.VerifyTool.VerifyValue(expectedTotalContacts.ToString(), UIUtilityProvider.UIHelper.GetText(TotalNewContactsLocator, LocateBy.Id), "New Contacts: {0}"); 
        }

        /// <summary>
        /// Enters and attempts to start importing a contact list
        /// </summary>
        /// <param name="filePath"></param>
        public void ChooseContactListToUpload(string filePath)
        {
            UIUtilityProvider.UIHelper.Type(ImportFileLocator, filePath, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(StartContactListWizardNextLocator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("plain");
            UIUtilityProvider.UIHelper.WaitForElementDisplay(TopRowHeadersLocator, LocateBy.Id);
        }
        #endregion

        /// <summary>
        /// Opens the list and verifies the first person in the list and the total at the bottom
        /// </summary>
        /// <param name="listName">Name of Contact List</param>
        /// <param name="expectedFirstName">Format of Last, First</param>
        /// <param name="expectedFirstEmail">Email Address</param>
        /// <param name="expectedTotal">Total</param>
        [Verify]
        public void VerifyListUploaded(string listName, string expectedFirstName, string expectedFirstEmail, int expectedTotal)
        {
            OpenContactList(listName);
            VerifyNameInList(expectedFirstName, expectedFirstEmail, 0);
            string total = UIUtilityProvider.UIHelper.GetText(TotalLocator, LocateBy.XPath);
            Utilities.VerifyTool.VerifyValue(expectedTotal.ToString() + "  ", Regex.Split(total, @"Total: |Date Created: ")[1], "Total: {0}");
        }

        public void VerifyNameInList(string expectedFullName, string expectedEmail, int index)
        {
            string firstNameInList = UIUtilityProvider.UIHelper.GetText(string.Format(FirstNameLocator, Utilities.ConversionTools.ConvertGroupMemberIndexToTwoDigitsString(index + 2)), LocateBy.Id);

            string firstEmail = UIUtilityProvider.UIHelper.GetText(string.Format(FirstEmailLocator, Utilities.ConversionTools.ConvertGroupMemberIndexToTwoDigitsString(index + 2)), LocateBy.XPath);

            Utilities.VerifyTool.VerifyValue(expectedFullName, firstNameInList, "Name In List: {0}");
            Utilities.VerifyTool.VerifyValue(expectedEmail, firstEmail, "Email in List: {0}");
        }

        [Step]
        public void ExpandContactLists()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ExpandContactListsLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        [Step]
        public int GetContactListIdFromLink(string contactListName)
        {
            string listId;
            listId = UIUtilityProvider.UIHelper.GetAttribute(contactListName, "href", LocateBy.LinkText);
            string[] testing = Regex.Split(listId, @"EmailListID=");
            listId = testing[1]; 
            return Convert.ToInt32(listId);
        }

        public void OpenContactList(string listName)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(ContactListLocator, listName), LocateBy.LinkText);
            UIUtilityProvider.UIHelper.WaitForPageToLoad(TimeSpan.FromMinutes(3));
        }

        [Step]
        public void DeleteContactList()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(DeleteListLocator, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(DeleteLocator, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void ReturnToEmailAndContactLists()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(EmailAndContactListReturnLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void ReturnToContactListDetails()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ContactListDetailsReturnLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void ClickAddContact()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddContactLocator, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("plain");
        }

        public void OpenContactDetails(string name, int index)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(FirstNameLocator, Utilities.ConversionTools.ConvertGroupMemberIndexToTwoDigitsString(index + 2)), LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        /// <summary>
        /// Verifies data present on the contact details page. This code is really not ideal, but I am frustrated and can't think of a better way
        /// </summary>
        /// <param name="fieldToVerify">Use FieldMappingOptions Note: to verify ANY NAME you need to use the full name (first last) or it will fail.</param>
        /// <param name="expectedData"></param>
        public void VerifyContactDetailInfo(ManualFieldOptions fieldToVerify, string expectedData)
        {
            string actualData;

            if (fieldToVerify == ManualFieldOptions.FirstName || fieldToVerify == ManualFieldOptions.LastName)
            {
                actualData = UIUtilityProvider.UIHelper.GetText("builderSectionHeaderText", LocateBy.ClassName);
                Utilities.VerifyTool.VerifyValue(expectedData, actualData, "Data: {0}");
                return;
            }

            if (fieldToVerify == ManualFieldOptions.EmailAddress || fieldToVerify == ManualFieldOptions.Company)
            {
                actualData = UIUtilityProvider.UIHelper.GetText(string.Format(ContactDetailsLocator, StringEnum.GetStringValue(fieldToVerify)), LocateBy.XPath);
                Utilities.VerifyTool.VerifyValue(expectedData, actualData, "Data: {0}");
                return;
            }

            if (fieldToVerify == ManualFieldOptions.WorkPhone)
            {
                actualData = UIUtilityProvider.UIHelper.GetText(string.Format(ContactDetailsLocator, "Phone Number"), LocateBy.XPath);
                Utilities.VerifyTool.VerifyValue(expectedData, actualData, "Data: {0}");
                return;
            }

            if (fieldToVerify == ManualFieldOptions.AddressLine1
                || fieldToVerify == ManualFieldOptions.AddressLine2
                || fieldToVerify == ManualFieldOptions.State
                || fieldToVerify == ManualFieldOptions.Zip
                || fieldToVerify == ManualFieldOptions.City)
            {
                actualData = UIUtilityProvider.UIHelper.GetText("//address", LocateBy.XPath); 
                Assert.True(actualData.Contains(expectedData), "Data: " + expectedData + ", was not found");
                return;
            }
            else
            {
                if(UIUtilityProvider.UIHelper.IsElementPresent("Show All Fields", LocateBy.LinkText))
                {
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Show All Fields", LocateBy.LinkText);
                    UIUtilityProvider.UIHelper.WaitForAJAXRequest();
                }

                actualData = UIUtilityProvider.UIHelper.GetText(string.Format(ContactDetailsLocator, StringEnum.GetStringValue(fieldToVerify)), LocateBy.XPath);
                Utilities.VerifyTool.VerifyValue(expectedData, actualData, "Data: {0}"); 
            }
        }

        /// <summary>
        /// Enters Info For New Contact
        /// </summary>
        /// <param name="field">Use FieldMappingOptions Class</param>
        /// <param name="textToEnter"></param>
        public void EnterNewContactInfo(ManualFieldOptions field, string textToEnter)
        {
            string fieldLocator = "//label[contains(text(),'{0}')][@for='txtValue']/following-sibling::input";
            UIUtilityProvider.UIHelper.Type(string.Format(fieldLocator, StringEnum.GetStringValue(field)), textToEnter, LocateBy.XPath);
        }

        public void TypeManulContactInfo(string email, string firstName, string lastName, string company)
        {
            this.TypeContactEmailAddress(email);
            this.TypeContactFirstName(firstName);
            this.TypeContactLastName(lastName);
            this.TypeContactCompany(company);
        }

        public void TypeContactEmailAddress(string email)
        {
            UIUtilityProvider.UIHelper.Type(ContactEmailLocator, email, LocateBy.Id);
        }

        public void TypeContactFirstName(string firstName)
        {
            UIUtilityProvider.UIHelper.Type(ContactFirstNameLocator, firstName, LocateBy.Id);
        }

        public void TypeContactLastName(string lastName)
        {
            UIUtilityProvider.UIHelper.Type(ContactLastNameLocator, lastName, LocateBy.Id);
        }

        public void TypeContactCompany(string company)
        {
            UIUtilityProvider.UIHelper.Type(ContactCompanyLocator, company, LocateBy.Id);
        }

        public void SaveCloseContactInfo()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(SaveCloseContactInfoLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
        }

        public void OpenEditFields()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(EditFieldsLinkLocator, LocateBy.LinkText);
        }

        public void ChooseFieldToDisplay(ManualFieldOptions field, bool visible, bool required)
        {
            if (field == ManualFieldOptions.AdditionalField1
                || field == ManualFieldOptions.AdditionalField2
                || field == ManualFieldOptions.AdditionalField3
                || field == ManualFieldOptions.AdditionalField4
                || field == ManualFieldOptions.AdditionalField5
                || field == ManualFieldOptions.AdditionalField6
                || field == ManualFieldOptions.AdditionalField7
                || field == ManualFieldOptions.AdditionalField8
                || field == ManualFieldOptions.AdditionalField9
                || field == ManualFieldOptions.AdditionalField10)
            {
                UIUtilityProvider.UIHelper.SetCheckbox(string.Format(AdditionalFieldVisibleLocator, StringEnum.GetStringValue(field)), visible, LocateBy.XPath);
                UIUtilityProvider.UIHelper.SetCheckbox(string.Format(AdditionalFieldRequiredLocator, StringEnum.GetStringValue(field)), required, LocateBy.XPath);
            }
            else
            {
                UIUtilityProvider.UIHelper.SetCheckbox(string.Format(FieldVisibleLocator, StringEnum.GetStringValue(field)), visible, LocateBy.XPath);
                UIUtilityProvider.UIHelper.SetCheckbox(string.Format(FieldRequiredLocator, StringEnum.GetStringValue(field)), required, LocateBy.XPath);
            }
        }

        public void CloseEditFields()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(CloseEditFieldsLocator, LocateBy.LinkText); 
        }

        public string GetAddManualContactError(int index)
        {
            string errorMessageLocator = "//div[@id='ctl00_cphDialog_vsValidationSummary']/ul/li[" + index + "]";

            if (!UIUtilityProvider.UIHelper.IsElementHidden(errorMessageLocator, LocateBy.XPath))
            {
                string error = UIUtilityProvider.UIHelper.GetText(errorMessageLocator, LocateBy.XPath);
                return error;
            }
            else
            {
                return string.Empty; 
            }
        }
    }
}
