namespace RegOnline.RegressionTest.Managers.Register
{
    using System.Linq;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public partial class RegisterManager : ManagerBase
    {
        /// <summary>
        /// Open register page using specific event id
        /// </summary>
        /// <param name="eventID"></param>
        [Step]
        public void OpenRegisterPage(int eventId)
        {
            this.CurrentEventId = eventId;
            WebDriverUtility.DefaultProvider.OpenUrl(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + eventId);
            AllowCookies();
            ClickStartNewRegistration();
        }

        public void OpenRegTypeDirectUrl(int eventId, int regTypeId)
        {
            this.CurrentEventId = eventId;
            WebDriverUtility.DefaultProvider.OpenUrl(string.Format(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "?eventID={0}&rTypeID={1}", eventId, regTypeId));
            AllowCookies();
            ClickStartNewRegistration();
        }

        public void OpenOnSiteRegisterPage(int eventId)
        {
            this.CurrentEventId = eventId;
            WebDriverUtility.DefaultProvider.OpenUrl(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "register/checkin.aspx?MethodId=2&eventsessionId=&eventID=" + eventId);
            AllowCookies();
            ClickStartNewRegistration();
        }

        /// <summary>
        /// Open register page using current event id
        /// </summary>
        [Step]
        public void OpenRegisterPage()
        {
            OpenRegisterPage(this.CurrentEventId);
        }

        /// <summary>
        /// Open register page using specific url
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="url"></param>
        public void OpenRegisterPage(int eventId, string url)
        {
            this.CurrentEventId = eventId;
            WebDriverUtility.DefaultProvider.OpenUrl(url);
            AllowCookies();
            ClickStartNewRegistration();
        }

        /// <summary>
        /// Open admin register page using specific event id and event session id
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="sessionID"></param>
        [Step]
        public void OpenAdminRegisterPage(int eventID, string eventSessionId)
        {
            string adminRegTargetUrl = 
                ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl +
                "register/checkin.aspx?MethodId=1&eventsessionId={0}&eventID={1}&UseNewSecurity=true";

            string adminRegUrl = string.Format(adminRegTargetUrl, eventSessionId, eventID);

            this.CurrentEventId = eventID;
            WebDriverUtility.DefaultProvider.OpenUrl(adminRegUrl);
            AllowCookies();
        }

        /// <summary>
        /// Fetch event website url through database
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
        public string Fetch_EventWebsiteUrl(int eventID)
        {
            ROMasterDataContext db = new ROMasterDataContext();
            Shortcut shortcut = (from s in db.Shortcuts where s.EventId == eventID select s).Single();
            return ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + shortcut.Description;
        }
    }
}
