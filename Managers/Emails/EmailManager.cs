namespace RegOnline.RegressionTest.Managers.Emails
{
    using System;
    using System.Linq;
    using System.Reflection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public partial class EmailManager : ManagerBase
    {
        public enum EmailCategory
        {
            [RegMailTriggerId(1)]
            [DefaultConfirmationEmailHtmlContentAttribute(DefaultConfirmationEmailHtmlContent.Complete)]
            [DefaultConfirmationEmailBodyTextFormatAttribute(DefaultConfirmationemailBodyTextFormat.Complete)]
            Complete,

            [RegMailTriggerId(5)]
            [DefaultConfirmationEmailHtmlContentAttribute(DefaultConfirmationEmailHtmlContent.Incomplete)]
            [DefaultConfirmationEmailBodyTextFormatAttribute(DefaultConfirmationemailBodyTextFormat.Incomplete)]
            Incomplete,

            [RegMailTriggerId(2)]
            [DefaultConfirmationEmailHtmlContentAttribute(DefaultConfirmationEmailHtmlContent.Update)]
            [DefaultConfirmationEmailBodyTextFormatAttribute(DefaultConfirmationemailBodyTextFormat.Update)]
            Update,

            [RegMailTriggerId(3)]
            Substitute,

            [RegMailTriggerId(4)]
            Cancel
        }

        public class RegMailTriggerIdAttribute : Attribute
        {
            public int TriggerId
            {
                get;
                set;
            }

            public RegMailTriggerIdAttribute(int triggerId)
            {
                this.TriggerId = triggerId;
            }

            public static int GetRegMailTriggerId(EmailCategory category)
            {
                Type type = category.GetType();
                FieldInfo fi = type.GetField(category.ToString());
                RegMailTriggerIdAttribute[] attrs = fi.GetCustomAttributes(typeof(RegMailTriggerIdAttribute), false) as RegMailTriggerIdAttribute[];
                return attrs[0].TriggerId;
            }
        }

        public class DefaultConfirmationEmailHtmlContentAttribute : Attribute
        {
            public string HtmlContent { get; set; }

            public DefaultConfirmationEmailHtmlContentAttribute(string htmlContent)
            {
                this.HtmlContent = htmlContent;
            }

            public static string GetHtmlContent(EmailCategory category)
            {
                Type type = category.GetType();
                FieldInfo fi = type.GetField(category.ToString());
                DefaultConfirmationEmailHtmlContentAttribute[] attrs = fi.GetCustomAttributes(typeof(DefaultConfirmationEmailHtmlContentAttribute), false) as DefaultConfirmationEmailHtmlContentAttribute[];
                return attrs[0].HtmlContent;
            }
        }

        public class DefaultConfirmationEmailBodyTextFormatAttribute : Attribute
        {
            public string HtmlContent { get; set; }

            public DefaultConfirmationEmailBodyTextFormatAttribute(string htmlContent)
            {
                this.HtmlContent = htmlContent;
            }

            public static string GetBodyTextFormat(EmailCategory category)
            {
                Type type = category.GetType();
                FieldInfo fi = type.GetField(category.ToString());
                DefaultConfirmationEmailBodyTextFormatAttribute[] attrs = fi.GetCustomAttributes(typeof(DefaultConfirmationEmailBodyTextFormatAttribute), false) as DefaultConfirmationEmailBodyTextFormatAttribute[];
                return attrs[0].HtmlContent;
            }
        }

        private const string ConfirmationEmailURLConstructor = "builder/site/emailviewer.aspx?eventid={0}&aid={1}&emailid={2}=&typeid=2&themeid=-1";
        private const string EmailInvitationURLConstructor = "builder/site/emailviewer.aspx?eventid={0}&aid={1}&emailid={2}=&typeid=9&themeid=-1";

        ////public string ComposeConfirmationEmailURL(EmailCategory category, int eventId, int registrationId)
        ////{
        ////    string url = string.Empty;
        ////    string emailId = FetchConfirmationEmailId(category, eventId);
        ////    string attendeeId = FetchAttendeeId(registrationId);

        ////    url = ConfigurationProvider.XmlConfig.PrivateLabelConfiguration.BaseUrl + 
        ////        string.Format(ConfirmationEmailURLConstructor, eventId, U.EncryptionTools.Encrypt(attendeeId), U.EncryptionTools.Encrypt(emailId));

        ////    return url;
        ////}

        ////public string ComposeEmailInvitationURL(int eventId, string attendeeId, string emailId)
        ////{
        ////    string url = string.Empty;

        ////    url = ConfigurationProvider.XmlConfig.PrivateLabelConfiguration.BaseUrl + 
        ////        string.Format(EmailInvitationURLConstructor, eventId, U.EncryptionTools.Encrypt(attendeeId), U.EncryptionTools.Encrypt(emailId));
            
        ////    return url;
        ////}

        public string FetchConfirmationEmailId(EmailCategory category, int eventId)
        {
            int regMailTriggerId = RegMailTriggerIdAttribute.GetRegMailTriggerId(category);
            string emailId = string.Empty;
            var db = new DataAccess.ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            var id = (from i in db.RegMailResponders where i.EventId == eventId && i.RegMailTypeId == 2 && i.RegMailTriggerId == regMailTriggerId orderby i.Id descending select i.Id).ToList();
            emailId = id[0].ToString();
            return emailId;
        }

        public string FetchInvitationEmailId(string emailName)
        {
            string emailId = string.Empty;
            var db = new DataAccess.ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            var id = (from i in db.EmailJobs where i.CustomerId == Convert.ToInt32(ConfigurationProvider.XmlConfig.AccountConfiguration.Id) && i.Description == emailName select i.Id).ToList();
            emailId = id[0].ToString();
            return emailId;
        }

        public string FetchAttendeeId(int registrationId)
        {
            string attendeeID = string.Empty;
            var db = new DataAccess.ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            var id = (from i in db.Registrations where i.Register_Id == registrationId select i.Attendee_Id).ToList();
            attendeeID = id[0].ToString();
            return attendeeID;
        }

        [Step]
        public void SaveAndClose()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(@"Save & Close", LocateBy.LinkText);
            Utility.ThreadSleep(1);
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void SaveAndStay()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(@"Save & Stay", LocateBy.LinkText);
            Utility.ThreadSleep(1);
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void SaveAndNew()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(@"Save & New", LocateBy.LinkText);
            Utility.ThreadSleep(1);
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void Cancel()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(@"Cancel", LocateBy.LinkText);
            Utility.ThreadSleep(1);
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }
    }
}
