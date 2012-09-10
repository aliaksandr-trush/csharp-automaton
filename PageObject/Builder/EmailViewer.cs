namespace RegOnline.RegressionTest.PageObject.Builder
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.Configuration;

    public class EmailViewer : Window
    {
        public void OpenURL(int eventId, int registrantId)
        {
            string ConfirmationEmailURLConstructor = "builder/site/emailviewer.aspx?eventid={0}&aid={1}&emailid={2}=&typeid=2&themeid=-1";

            string attendeeId = DataAccess.AccessData.FetchAttendeeId(registrantId).ToString();
            string emailId = DataAccess.AccessData.FetchConfirmationEmailId(eventId);

            string encryptAttendeeId = DataAccess.AccessData.GetEncryptString(attendeeId);
            string encryptEmailId = DataAccess.AccessData.GetEncryptString(emailId);

            string url = ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl +
                string.Format(ConfirmationEmailURLConstructor, eventId, encryptAttendeeId, encryptEmailId);

            WebDriverUtility.DefaultProvider.OpenUrl(url);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
        }
    }
}
