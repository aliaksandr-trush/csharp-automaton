namespace RegOnline.RegressionTest.Fixtures.Emails.Registration
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Emails;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class ConfirmationAndUpdateAndIncompleteEmailFixture : FixtureBase
    {
        private const string AppendedConfirmationEmailBodyText = "Modified text here!";

        private int eventId;
        private string eventSessionId;
        private int registrationId;
        private string registrantLastName;
        private List<string> registrationSessionIds = new List<string>();

        private struct EventName
        {
            public const string Complete = "ConfirmationEmailFixture-Complete";
            public const string Update = "ConfirmationEmailFixture-Update";
            public const string Incomplete = "ConfirmationEmailFixture-Incomplete";
        }

        [Test]
        [Category(Priority.Two)]
        [Description("266")]
        public void CompleteConfirmationEmail()
        {
            EmailManager.ConfirmationEmailBody emailBody = new EmailManager.ConfirmationEmailBody(EmailManager.EmailCategory.Complete);

            // Step #1
            this.AddEvent(EventName.Complete, EmailManager.EmailCategory.Complete);

            // Step #2
            this.Register();
            emailBody.Fill(EventName.Complete, this.registrantLastName, this.registrationId, string.Empty);
            ////this.OpenEmailUrlAndVerify(emailBody);

            // Step #3
            this.LoginAndEditEmail(EmailManager.EmailCategory.Complete, AppendedConfirmationEmailBodyText);

            // Step #4
            this.Register();
            emailBody.Fill(EventName.Complete, this.registrantLastName, this.registrationId, AppendedConfirmationEmailBodyText);
            ////this.OpenEmailUrlAndVerify(emailBody);

            // Step #5,6: impossible?
        }

        [Test]
        [Category(Priority.Two)]
        [Description("267")]
        public void UpdateConfirmationEmail()
        {
            EmailManager.ConfirmationEmailBody emailBody = new EmailManager.ConfirmationEmailBody(EmailManager.EmailCategory.Update);

            // Step #1
            this.AddEvent(EventName.Update, EmailManager.EmailCategory.Update);

            // Step #2
            string email = this.Register();

            // Step #3
            this.UpdateRegister(email, 1);
            emailBody.Fill(EventName.Update, this.registrantLastName, this.registrationId, string.Empty);
            ////this.OpenEmailUrlAndVerify(emailBody);

            // Step #4
            this.LoginAndEditEmail(EmailManager.EmailCategory.Update, AppendedConfirmationEmailBodyText);

            // Step #5
            this.UpdateRegister(email, 2);
            emailBody.AppendedBodyText = AppendedConfirmationEmailBodyText;
            ////this.OpenEmailUrlAndVerify(emailBody);

            // Step #6: impossible?
        }

        [Test]
        [Category(Priority.Two)]
        [Description("269")]
        public void IncompleteConfirmationEmail()
        {
            EmailManager.ConfirmationEmailBody emailBody = new EmailManager.ConfirmationEmailBody(EmailManager.EmailCategory.Incomplete);

            // Step #1
            this.AddEvent(EventName.Incomplete, EmailManager.EmailCategory.Incomplete);

            // Step #2
            this.CreateIncompleteRegistration();
            emailBody.Fill(EventName.Incomplete, this.registrantLastName, this.registrationId, string.Empty);
            ////this.OpenEmailUrlAndVerify(emailBody);

            // Step #3
            this.LoginAndEditEmail(EmailManager.EmailCategory.Incomplete, AppendedConfirmationEmailBodyText);

            // Step #4
            this.CreateIncompleteRegistration();
            emailBody.AppendedBodyText = AppendedConfirmationEmailBodyText;
            ////this.OpenEmailUrlAndVerify(emailBody);

            // Step #5,6,7: impossible?
        }

        private void AddEvent(string eventName, EmailManager.EmailCategory category)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(eventName);
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetStartPage(Managers.Manager.ManagerSiteManager.EventType.ProEvent, eventName);
            BuilderMgr.SetStartEndDateTimeDefault();
            BuilderMgr.GotoTab(Managers.Builder.FormDetailManager.Tab.Emails);

            if (category == EmailManager.EmailCategory.Incomplete)
            {
                BuilderMgr.SetWhetherToSendIncompleteNotificationEmail(true);
            }

            this.EditEmail(category, null);
            BuilderMgr.SaveAndClose();
        }

        private string Register()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            string email = RegisterMgr.Checkin();
            RegisterMgr.Continue();
            this.registrantLastName = RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            this.registrationId = RegisterMgr.GetRegistrationIdOnConfirmationPage();

            return email;
        }

        private void CreateIncompleteRegistration()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            registrationSessionIds.Clear();
            this.registrationSessionIds.Add(RegisterMgr.GetSessionId());
            DataHelperTool.DeleteRegistrationSession(this.registrationSessionIds);
        }

        private void UpdateRegister(string email, int updateCount)
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin(email);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
            RegisterMgr.Continue();
            RegisterMgr.ClickEditPersonalInformationLink(0);
            string newAddressLineOne = RegisterManager.DefaultPersonalInfo.AddressLineOne + updateCount.ToString();
            RegisterMgr.EnterPersonalInfoAddress(newAddressLineOne, null, null, null, null, null);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        ////private void OpenEmailUrlAndVerify(EmailManager.ConfirmationEmailBody body)
        ////{
        ////    EmailMgr.OpenConfirmationEmailUrl(body.Category, this.eventId, this.registrationId);
        ////    EmailMgr.VerifyConfirmationEmailBodyText(body);

        ////    if (body.Category == EmailManager.EmailCategory.Complete || body.Category == EmailManager.EmailCategory.Update)
        ////    {
        ////        EmailMgr.ClickReviewChangeUpdateLinkInConfirmationEmail();
        ////        RegisterMgr.VerifyOnConfirmationPage();

        ////        VerifyTool.VerifyValue(
        ////        this.registrationId,
        ////        RegisterMgr.GetRegistrationIdOnConfirmationPage(),
        ////        "RegistrationId in confirmation email: {0}");
        ////    }
        ////    else if (body.Category == EmailManager.EmailCategory.Incomplete)
        ////    {
        ////        EmailMgr.ClickClickHereLinkInIncompleteConfirmationEmail();
        ////        RegisterMgr.VerifyOnCheckinPage(true);
        ////    }
        ////}

        private void LoginAndEditEmail(EmailManager.EmailCategory category, string appendedEmailBodyText)
        {
            ManagerSiteMgr.OpenLogin();
            this.eventSessionId = ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventId, this.eventSessionId);
            BuilderMgr.GotoTab(Managers.Builder.FormDetailManager.Tab.Emails);
            this.EditEmail(category, appendedEmailBodyText);
            BuilderMgr.SaveAndClose();
        }

        private void EditEmail(EmailManager.EmailCategory category, string appendedEmailBodyText)
        {
            BuilderMgr.OpenEditConfirmationEmail(category);
            BuilderMgr.SwitchModeInEmail(Managers.Builder.FormDetailManager.Mode.Html);
            EmailMgr.ModifyEmailHtmlContent(category, appendedEmailBodyText);
            BuilderMgr.SelectEmailEditFrame();
            BuilderMgr.SaveAndCloseEditEmail();
        }
    }
}