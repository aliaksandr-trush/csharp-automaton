namespace RegOnline.RegressionTest.Managers.Emails
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Managers.Builder;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public partial class EmailManager : ManagerBase
    {
        public abstract class DefaultConfirmationEmailHtmlContent
        {
            public const string Complete = @"<table cellpadding=""15"" style=""font-size: 12px;"">" +
            @"<tbody><tr>" +
            @"<td valign=""top"">" +
            @"<p>Thank you for registering. You are confirmed for /*Merge: EventTitle*/.</p>" +
            @"<p>Name: /*Merge: FirstName*/ /*Merge: LastName*/" +
            @"<br />" +
            @"Registration ID: /*Merge: RegistrationId*/</p>" +
            @"<p>/*AttendeeRecord: Review, change, or update your registration*/.</p>" +
            @"<p>We look forward to seeing you at the event!</p>" +
            @"</td>" +
            @"<td width=""175"" valign=""top"">" +
            @"<table style=""margin-bottom: 10px; font-size: 12px;"">" +
            @"<tbody><tr>" +
            @"<td style=""font-size: 14px; font-weight: bold;"">" +
            @"When" +
            @"</td>" +
            @"</tr>" +
            @"<tr>" +
            @"<td>" +
            @"/*Merge: EventDate*/" +
            @"</td>" +
            @"</tr>" +
            @"<tr>" +
            @"<td>" +
            @"/*Merge: AddToCalendar*/" +
            @"</td>" +
            @"</tr>" +
            @"</tbody></table>" +
            @"<table style=""margin-bottom: 10px; font-size: 12px;"">" +
            @"<tbody><tr>" +
            @"<td style=""font-size: 14px; font-weight: bold;"">" +
            @"Where" +
            @"</td>" +
            @"</tr>" +
            @"<tr>" +
            @"<td>" +
            @"/*Merge: LocationName*/<br />" +
            @"/*Merge: LocationAddress1*/<br />" +
            @"/*Merge: LocationCity*/, /*Merge: LocationRegion*/ /*Merge: LocationPostalCode*/<br />" +
            @"/*Merge: LocationPhone*/" +
            @"</td>" +
            @"</tr>" +
            @"<tr>" +
            @"<td>" +
            @"/*EventMap*/" +
            @"</td>" +
            @"</tr>" +
            @"</tbody></table>" +
            @"<table style=""margin-bottom: 10px; font-size: 12px;"">" +
            @"<tbody><tr>" +
            @"<td style=""font-size: 14px; font-weight: bold;"">" +
            @"Contact" +
            @"</td>" +
            @"</tr>" +
            @"<tr>" +
            @"<td>" +
            @"/*Merge: EventContactInfo*/" +
            @"</td>" +
            @"</tr>" +
            @"</tbody></table>" +
            @"</td>" +
            @"</tr>" +
            @"</tbody></table>";

            public const string Update = @"<table cellpadding=""15"" style=""font-size: 12px;"">" +
            @"<tbody><tr>" +
            @"<td valign=""top"">" +
            @"<p>This is to confirm that we have received an update to your registration for <strong>/*Merge: EventTitle*/</strong>.</p>" +
            @"<p>Name: /*Merge: FirstName*/ /*Merge: LastName*/" +
            @"<br />" +
            @"Registration ID: /*Merge: RegistrationId*/</p>" +
            @"<p>/*AttendeeRecord: Review, change, or update your registration*/.</p>" +
            @"</td>" +
            @"<td width=""175"" valign=""top"">" +
            @"<table style=""margin-bottom: 10px; font-size: 12px;"">" +
            @"<tbody><tr>" +
            @"<td style=""font-size: 14px; font-weight: bold;"">" +
            @"When" +
            @"</td>" +
            @"</tr>" +
            @"<tr>" +
            @"<td>" +
            @"/*Merge: EventDate*/" +
            @"</td>" +
            @"</tr>" +
            @"<tr>" +
            @"<td>" +
            @"/*Merge: AddToCalendar*/" +
            @"</td>" +
            @"</tr>" +
            @"</tbody></table>" +
            @"<table style=""margin-bottom: 10px; font-size: 12px;"">" +
            @"<tbody><tr>" +
            @"<td style=""font-size: 14px; font-weight: bold;"">" +
            @"Where" +
            @"</td>" +
            @"</tr>" +
            @"<tr>" +
            @"<td>" +
            @"/*Merge: LocationName*/<br />" +
            @"/*Merge: LocationAddress1*/<br />" +
            @"/*Merge: LocationCity*/, /*Merge: LocationRegion*/ /*Merge: LocationPostalCode*/<br />" +
            @"/*Merge: LocationPhone*/" +
            @"</td>" +
            @"</tr>" +
            @"<tr>" +
            @"<td>" +
            @"/*EventMap*/" +
            @"</td>" +
            @"</tr>" +
            @"</tbody></table>" +
            @"<table style=""margin-bottom: 10px; font-size: 12px;"">" +
            @"<tbody><tr>" +
            @"<td style=""font-size: 14px; font-weight: bold;"">" +
            @"Contact" +
            @"</td>" +
            @"</tr>" +
            @"<tr>" +
            @"<td>" +
            @"/*Merge: EventContactInfo*/" +
            @"</td>" +
            @"</tr>" + 
            @"</tbody></table>" +
            @"</td>" +
            @"</tr>" +
            @"</tbody></table>";

            public const string Incomplete = 
                @"We noticed that you started but did not complete your registration for <strong>/*Merge: EventTitle*/</strong>." +
                @" If you want to continue your registration," +
                @" <a href=""https://beta.regonline.com/?eventID=/*Merge:%20EventID*/&amp;rTypeID=/*Merge:%20RegTypeID*/"">click here</a>.<br>" +
                @"<br>" +
                @"If you have questions about your registration, reply to this email.";
        }

        public abstract class DefaultConfirmationemailBodyTextFormat
        {
            public const string Complete = "Thank you for registering. You are confirmed for {0}.\r\nName: {1} {2}\r\nRegistration ID: {3}\r\nReview, change, or update your registration.\r\nWe look forward to seeing you at the event!\r\nWhen\r\n{4} MDT - {5} MDT\r\nAdd to My Calendar\r\nWhere\r\n{6}\r\n{7}\r\n{8}, {9} {10}\r\nFor a map and directions to the event, click here.\r\nContact\r\nPhone: 303.577.5100";
            public const string Update = "This is to confirm that we have received an update to your registration for {0}.\r\nName: {1} {2}\r\nRegistration ID: {3}\r\nReview, change, or update your registration.\r\nWhen\r\n{4} MDT - {5} MDT\r\nAdd to My Calendar\r\nWhere\r\n{6}\r\n{7}\r\n{8}, {9} {10}\r\nFor a map and directions to the event, click here.\r\nContact\r\nPhone: 303.577.5100";
            public const string Incomplete = "We noticed that you started but did not complete your registration for {0}. If you want to continue your registration, click here.\r\n\r\nIf you have questions about your registration, reply to this email.";
        }

        protected const string WebEventJoinLinkLocator = "//a[contains(@href, 'WebEvent/join.aspx')]";
        protected const string HeaderTitleLocator = "emailHeaderText";
        protected const string HeaderImgLocator = "//div[@id='emailHeaderImgVail']/img";
        protected const string BodyTextLocator = "EmailContent";

        ////[Step]
        ////public void OpenConfirmationEmailUrl(EmailCategory category, int eventId, int registrationId)
        ////{
        ////    string url = ComposeConfirmationEmailURL(category, eventId, registrationId);
        ////    UIUtilityProvider.UIHelper.OpenUrl(url);
        ////}

        ////[Step]
        ////public void OpenEmailInvitationEmailUrl(int eventId, int attendeeId, string emailName)
        ////{
        ////    string emailId = FetchInvitationEmailId(emailName); 
        ////    string url = ComposeEmailInvitationURL(eventId, attendeeId.ToString(), emailId);
        ////    UIUtilityProvider.UIHelper.OpenUrl(url);
        ////}

        ////[Verify]
        ////public void OpenVirtualEventConfirmationLink()
        ////{
        ////    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(WebEventJoinLinkLocator, LocateBy.XPath);
        ////    UIUtilityProvider.UIHelper.WaitForPageToLoad();
        ////    Assert.True(OnWebEventJoinPage());
        ////}

        public bool OnWebEventJoinPage()
        {
            return WebDriverUtility.DefaultProvider.UrlContainsPath("register/WebEvent/join.aspx");
        }

        public string GetEmailHeaderTitle()
        {
            return WebDriverUtility.DefaultProvider.GetText(HeaderTitleLocator, LocateBy.Id);
        }

        public string GetHeaderImgPathAttribute()
        {
            return WebDriverUtility.DefaultProvider.GetAttribute(HeaderImgLocator, "src", LocateBy.XPath); 
        }

        public string GetBodyText()
        {
            return WebDriverUtility.DefaultProvider.GetText(BodyTextLocator, LocateBy.Id); 
        }

        public class ConfirmationEmailBody
        {
            public EmailCategory Category;
            public string AppendedBodyText;
            public string EventName;
            public string FirstName = RegisterManager.DefaultPersonalInfo.FirstName;
            public string LastName;
            public int RegistrationId;
            public DateTime StartDateTime = ManagerBase.DefaultEventStartDateTime;
            public DateTime EndDateTime = ManagerBase.DefaultEventEndDateTime;
            public string Location = FormDetailManager.StartPageDefaultInfo.Location;
            public string AddressLineOne = FormDetailManager.StartPageDefaultInfo.AddressLineOne;
            public string City = FormDetailManager.StartPageDefaultInfo.City;
            public string State = FormDetailManager.StartPageDefaultInfo.State;
            public string ZipCode = FormDetailManager.StartPageDefaultInfo.ZipCode;

            public ConfirmationEmailBody(EmailCategory category)
            {
                this.Category = category;
            }

            public void Fill(string eventName, string lastName, int registrationId, string appendedBodyText)
            {
                this.EventName = eventName;
                this.LastName = lastName;
                this.RegistrationId = registrationId;
                this.AppendedBodyText = appendedBodyText;
            }

            public string ComposeConfirmationEmailBodyText()
            {
                string formatString = DefaultConfirmationEmailBodyTextFormatAttribute.GetBodyTextFormat(this.Category);

                if (!string.IsNullOrEmpty(this.AppendedBodyText))
                {
                    formatString = formatString + "\r\n" + this.AppendedBodyText;
                }

                return string.Format(
                    formatString, 
                    this.EventName, 
                    this.FirstName, 
                    this.LastName, 
                    this.RegistrationId.ToString(), 
                    this.StartDateTime.ToString("M/d/yyyy h:mm tt"), 
                    this.EndDateTime.ToString("M/d/yyyy h:mm tt"), 
                    this.Location, 
                    this.AddressLineOne, 
                    this.City, 
                    this.State, 
                    this.ZipCode);
            }
        }

        public void VerifyConfirmationEmailBodyText(ConfirmationEmailBody bodyText)
        {
            VerifyTool.VerifyValue(
                bodyText.ComposeConfirmationEmailBodyText(),
                this.GetBodyText(),
                "Confirmation email body text: {0}");
        }

        public void ClickReviewChangeUpdateLinkInConfirmationEmail()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("Review, change, or update your registration", LocateBy.LinkText);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void ClickClickHereLinkInIncompleteConfirmationEmail()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("click here", LocateBy.LinkText);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void ModifyEmailHtmlContent(EmailCategory category, string appendedContentText)
        {
            string content = DefaultConfirmationEmailHtmlContentAttribute.GetHtmlContent(category);

            if (!string.IsNullOrEmpty(appendedContentText))
            {
                content = content + "<p>" + appendedContentText + "</p>";
            }

            this.TypeContentInHTMLView(content);
        }
    }
}
