namespace RegOnline.RegressionTest.Fixtures.Emails.RegMail
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Managers.Emails;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class ContactListFixture : FixtureBase
    {
        private const int BigListContactTotals = 16383;
        private const int CSVErrorListSize = 98;
        private const int XLSTestingBadListSize = 10;
        private const int XLSXEmailListSize = 6;
        private string listName;

        private struct ContactListFileName
        {
            public const string BigListCSV = "BigList-16384.csv";
            public const string BigListXLS = "BigList-16384.xls";
            public const string BigListXLSX = "BigList-16384.xlsx";
            public const string ErrorsCSV = "99Guys2errors.csv";
            public const string ErrorsXLS = "Testing14291Bad.xls";
            public const string ErrorsXLSX = "EmailList2007.xlsx";
        }

        [Test]
        [Category(Priority.Two)]
        [Description("272")]
        public void ContactListUploadCSVWithErrors()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEmailTabIfNeeded();
            this.UploadCSVListWithExpectedErrors(ContactListFileName.ErrorsCSV, 96, CSVErrorListSize);
            EmailMgr.DeleteContactList();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("1331")]
        public void ContactListUploadXLSWithErrors()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEmailTabIfNeeded();
            this.UploadXLSListWithExpectedErrors(ContactListFileName.ErrorsXLS, 10, 2, XLSTestingBadListSize);
            EmailMgr.DeleteContactList();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("1332")]
        public void ContactListUploadXLSXWithErrors()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEmailTabIfNeeded();
            this.UploadXLSXListWithExpectedErrors(ContactListFileName.ErrorsXLSX, 5, XLSXEmailListSize);
            EmailMgr.DeleteContactList();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("1333")]
        public void ContactListUploadCSVLarge()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEmailTabIfNeeded();
            this.UploadListNoExpectedErrors(ContactListFileName.BigListCSV, BigListContactTotals);
            EmailMgr.DeleteContactList();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("1334")]
        public void ContactListUploadXLSLarge()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEmailTabIfNeeded();
            this.UploadListNoExpectedErrors(ContactListFileName.BigListXLS, BigListContactTotals);
            EmailMgr.DeleteContactList();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("1335")]
        public void ContactListUploadXLSXLarge()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEmailTabIfNeeded();
            this.UploadListNoExpectedErrors(ContactListFileName.BigListXLSX, BigListContactTotals);
            EmailMgr.DeleteContactList();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("273")]
        public void ManualAddToContactList()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEmailTabIfNeeded();
            this.UploadCSVListWithExpectedErrors(ContactListFileName.ErrorsCSV, 96, CSVErrorListSize);
            EmailMgr.ReturnToEmailAndContactLists();
            EmailMgr.VerifyListUploaded(listName, "Kosta, Patricia", "admin@americandriversalliance.com", CSVErrorListSize);
            ManuallyAddContacts();
            EmailMgr.ReturnToEmailAndContactLists();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
        }

        [Step]
        private void UploadListNoExpectedErrors(string listToUpload, int exepectedContacts)
        {
            EmailMgr.ClickCreateNewContactList();
            listName = "ContactList" + DateTime.Now.Ticks;
            EmailMgr.CreateContactListSetup(listName, true, EmailManager.ContactMethod.Import);
            EmailMgr.ChooseContactListToUpload(ConfigReader.DefaultProvider.EnvironmentConfiguration.DataPath + listToUpload);
            EmailMgr.NextStep();
            EmailMgr.ConfirmContactListUpload(exepectedContacts, exepectedContacts);
            EmailMgr.CloseContactListImport();
            EmailMgr.VerifyListUploaded(listName, "Cantle, Allan", "2a.cantle@gmail.com", BigListContactTotals);
            //EmailMgr.ReturnToEmailAndContactLists();
        }

        [Step]
        private void UploadCSVListWithExpectedErrors(string listToUpload, int expectedContacts, int ListSize)
        {
            EmailMgr.ClickCreateNewContactList();
            listName = "ContactList" + DateTime.Now.Ticks;
            EmailMgr.CreateContactListSetup(listName, true, EmailManager.ContactMethod.Import);
            EmailMgr.ChooseContactListToUpload(ConfigReader.DefaultProvider.EnvironmentConfiguration.DataPath + listToUpload);
            EmailMgr.NextStep();
            EmailMgr.CorrectUploadErrors("laura.wisen@gmail.com", 0, expectedContacts, expectedContacts, 1);
            EmailMgr.CorrectUploadErrors("something@wulfogf.org", 0, expectedContacts + 1, expectedContacts + 1, 0); 
            EmailMgr.CloseContactListImport();
            EmailMgr.VerifyListUploaded(listName, "Kosta, Patricia", "admin@americandriversalliance.com", ListSize);
        }

        [Step]
        private void UploadXLSListWithExpectedErrors(string listToUpload, int expectedContacts, int errors, int ListSize)
        {
            EmailMgr.ClickCreateNewContactList();
            listName = "ContactList" + DateTime.Now.Ticks;
            EmailMgr.CreateContactListSetup(listName, true, EmailManager.ContactMethod.Import);
            EmailMgr.ChooseContactListToUpload(ConfigReader.DefaultProvider.EnvironmentConfiguration.DataPath + listToUpload);
            EmailMgr.MapFields(2, EmailManager.FieldMappingOptions.LastName);
            EmailMgr.MapFields(3, EmailManager.FieldMappingOptions.Company);
            EmailMgr.MapFields(6, EmailManager.FieldMappingOptions.AdditionalField1);
            EmailMgr.NextStep();
            EmailMgr.ConfirmContactListUploadWithErrorReport(expectedContacts, expectedContacts, errors);
            EmailMgr.CloseContactListImport();
            EmailMgr.VerifyListUploaded(listName, "Budiardjo, Anto", "antob@clasma.com", ListSize);
        }

        [Step]
        private void UploadXLSXListWithExpectedErrors(string listToUpload, int expectedContacts, int ListSize)
        {
            EmailMgr.ClickCreateNewContactList();
            listName = "ContactList" + DateTime.Now.Ticks;
            EmailMgr.CreateContactListSetup(listName, true, EmailManager.ContactMethod.Import);
            EmailMgr.ChooseContactListToUpload(ConfigReader.DefaultProvider.EnvironmentConfiguration.DataPath + listToUpload);
            EmailMgr.MapFields(2, EmailManager.FieldMappingOptions.Email);
            EmailMgr.NextStep();
            EmailMgr.CorrectUploadErrors("asdfassdf@asdf.fds", 0, expectedContacts, expectedContacts, 1);
            EmailMgr.RemoveContactWithError(expectedContacts + 1, expectedContacts + 1, 0, 0);
            EmailMgr.CloseContactListImport();
            EmailMgr.VerifyListUploaded(listName, "Error, rs", "asdfassdf@asdf.fds", ListSize);
        }

        [Step]
        private void ManuallyAddContacts()
        {
            AddFirstContactVerifyErrorMessages();
            AddSecondContacts();
            AddContactAndCancel();
            AddContactWithAdditionalFields();
        }

        private void AddFirstContactVerifyErrorMessages()
        {
            EmailMgr.ClickAddContact();
            EmailMgr.SaveAndClose();
            string error = EmailMgr.GetAddManualContactError(1);
            Utilities.VerifyTool.VerifyValue("You must enter an 'email address'", error, "Error: {0}");
            EnterDefaultContactInfo("asdf.com", "AddedManually", "AddedManually", "ManualCompany");
            EmailMgr.SaveAndClose();
            error = EmailMgr.GetAddManualContactError(1);
            Utilities.VerifyTool.VerifyValue("You must enter a valid email address", error, "Error: {0}");
            EmailMgr.EnterNewContactInfo(EmailManager.ManualFieldOptions.EmailAddress, "asdf@asdf.com");
            EmailMgr.SaveAndClose();
            EmailMgr.VerifyNameInList("AddedManually, AddedManually", "asdf@asdf.com", 7);
            EmailMgr.OpenContactDetails("AddedManually, AddedManually", 7);
            VerifyContactDetails("AddedManually AddedManually", "asdf@asdf.com", "ManualCompany");
            EmailMgr.ReturnToContactListDetails();
        }

        private void AddSecondContacts()
        {
            EmailMgr.ClickAddContact();
            EnterDefaultContactInfo("aassddff@aassddff.com", "AnotherManual", "AnotherManual", "New Company");
            EmailMgr.SaveAndNew();
            EnterDefaultContactInfo("assddff@aassddff.com", "ManualAgain", "ManualAgain", "New Company");
            EmailMgr.SaveAndClose();
            EmailMgr.VerifyNameInList("AnotherManual, AnotherManual", "aassddff@aassddff.com", 0);
            EmailMgr.OpenContactDetails("AnotherManual, AnotherManual", 0);
            VerifyContactDetails("AnotherManual AnotherManual", "aassddff@aassddff.com", "New Company");
            EmailMgr.ReturnToContactListDetails();
            EmailMgr.VerifyNameInList("ManualAgain, ManualAgain", "assddff@aassddff.com", 9);
            EmailMgr.OpenContactDetails("ManualAgain, ManualAgain", 9);
            VerifyContactDetails("ManualAgain ManualAgain", "assddff@aassddff.com", "New Company");
            EmailMgr.ReturnToContactListDetails();
        }

        private void AddContactAndCancel()
        {
            EmailMgr.ClickAddContact();
            EmailMgr.Cancel();
            EmailMgr.VerifyNameInList("ManualAgain, ManualAgain", "assddff@aassddff.com", 9);
            EmailMgr.OpenContactDetails("ManualAgain, ManualAgain", 9);
            VerifyContactDetails("ManualAgain ManualAgain", "assddff@aassddff.com", "New Company");
            EmailMgr.ReturnToContactListDetails();
        }

        private void AddContactWithAdditionalFields()
        {
            EmailMgr.ClickAddContact();
            EmailMgr.OpenEditFields();
            foreach (EmailManager.ManualFieldOptions option in Enum.GetValues(typeof(EmailManager.ManualFieldOptions)))
            {
                if (option != EmailManager.ManualFieldOptions.EmailAddress)
                {
                    EmailMgr.ChooseFieldToDisplay(option, true, false);
                }
            }
            EmailMgr.ChooseFieldToDisplay(EmailManager.ManualFieldOptions.WorkPhone, true, true);
            EmailMgr.ChooseFieldToDisplay(EmailManager.ManualFieldOptions.City, true, true);
            EmailMgr.ChooseFieldToDisplay(EmailManager.ManualFieldOptions.State, true, true);
            EmailMgr.CloseEditFields();
            EnterDefaultContactInfo("asdf@asdfasdf.com", "LastContact", "LastContact", "Finally");
            EmailMgr.SaveAndClose();
            string error1 = EmailMgr.GetAddManualContactError(1);
            string error2 = EmailMgr.GetAddManualContactError(2);
            string error3 = EmailMgr.GetAddManualContactError(3);
            Utilities.VerifyTool.VerifyValue("Please enter a valid city.", error1, "Error: {0}");
            Utilities.VerifyTool.VerifyValue("You must enter 'state (province)'", error2, "Error: {0}");
            Utilities.VerifyTool.VerifyValue("You must enter 'work phone'", error3, "Error: {0}");

            foreach (EmailManager.ManualFieldOptions option in Enum.GetValues(typeof(EmailManager.ManualFieldOptions)))
            {
                if (option != EmailManager.ManualFieldOptions.EmailAddress)
                {
                    if (option == EmailManager.ManualFieldOptions.CC_Email)
                    {
                        EmailMgr.EnterNewContactInfo(option, "ccemail@something.com");
                    }
                    else
                    {
                        EmailMgr.EnterNewContactInfo(option, StringEnum.GetStringValue(option));
                    }
                }
            }
            EmailMgr.SaveAndClose();
            EmailMgr.VerifyNameInList("Last Name, First Name", "asdf@asdfasdf.com", 9);
            EmailMgr.OpenContactDetails("Last Name, First Name", 9);

            foreach (EmailManager.ManualFieldOptions option in Enum.GetValues(typeof(EmailManager.ManualFieldOptions)))
            {
                if (option != EmailManager.ManualFieldOptions.EmailAddress)
                {
                    if (option == EmailManager.ManualFieldOptions.CC_Email)
                    {
                        EmailMgr.VerifyContactDetailInfo(option, "ccemail@something.com");
                        return;
                    }
                    if (option == EmailManager.ManualFieldOptions.Password
                            || option == EmailManager.ManualFieldOptions.Prefix
                            || option == EmailManager.ManualFieldOptions.Suffix
                            || option == EmailManager.ManualFieldOptions.MiddleName
                            || option == EmailManager.ManualFieldOptions.AddressLine3
                            || option == EmailManager.ManualFieldOptions.WorkExtension)
                    {
                        return;
                    }
                    if (option == EmailManager.ManualFieldOptions.FirstName
                            || option == EmailManager.ManualFieldOptions.LastName)
                    {
                        EmailMgr.VerifyContactDetailInfo(option, "First Name Last Name");
                    }
                    if (option == EmailManager.ManualFieldOptions.WorkPhone)
                    {
                        EmailMgr.VerifyContactDetailInfo(option, 
                            StringEnum.GetStringValue(EmailManager.ManualFieldOptions.WorkPhone) 
                            + ", " + StringEnum.GetStringValue(EmailManager.ManualFieldOptions.WorkExtension));
                    }
                    else
                    {
                        EmailMgr.VerifyContactDetailInfo(option, StringEnum.GetStringValue(option));
                    }
                }
            }
            EmailMgr.ReturnToContactListDetails();
        }

        private void EnterDefaultContactInfo(string email, string firstName, string lastName, string company)
        {
            EmailMgr.EnterNewContactInfo(EmailManager.ManualFieldOptions.EmailAddress, email);
            EmailMgr.EnterNewContactInfo(EmailManager.ManualFieldOptions.FirstName, firstName);
            EmailMgr.EnterNewContactInfo(EmailManager.ManualFieldOptions.LastName, lastName);
            EmailMgr.EnterNewContactInfo(EmailManager.ManualFieldOptions.Company, company);
        }

        private void VerifyContactDetails(string fullName, string emailAddress, string company)
        {
            EmailMgr.VerifyContactDetailInfo(EmailManager.ManualFieldOptions.FirstName, fullName);
            EmailMgr.VerifyContactDetailInfo(EmailManager.ManualFieldOptions.EmailAddress, emailAddress);
            EmailMgr.VerifyContactDetailInfo(EmailManager.ManualFieldOptions.Company, company);
        }
    }
}
