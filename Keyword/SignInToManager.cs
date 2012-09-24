namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject;
    using RegOnline.RegressionTest.PageObject.Manager;
    using RegOnline.RegressionTest.UIUtility;

    public class SignInToManager
    {
        public void SignIn(EventFolders.Folders folder)
        {
            this.SignIn(ConfigReader.DefaultProvider.AccountConfiguration.Login,
                ConfigReader.DefaultProvider.AccountConfiguration.Password, folder);
        }

        public void SignIn(string userName, string password, EventFolders.Folders folder)
        {
            WebDriverUtility.DefaultProvider.OpenUrl(
                string.Format("{0}manager/login.aspx", 
                ConfigReader.DefaultProvider.AccountConfiguration.BaseUrlWithHttps));

            PageObject.PageObjectHelper.AllowCookie_Click();
            PageObject.PageObjectProvider.Manager.SignIn.UserName.Type(userName);
            PageObject.PageObjectProvider.Manager.SignIn.Password.Type(password);
            PageObject.PageObjectProvider.Manager.SignIn.SignInButton_Click();
            DataCollection.FormData.EventSessionId = PageObject.PageObjectProvider.Manager.SignIn.GetQueryStringValue("EventSessionId");
            PageObject.PageObjectProvider.Manager.Events.Folder_Click(folder.ToString());
        }

        public void SignInAndRecreateEventAndGetEventId(
            EventFolders.Folders folder, 
            DataCollection.Event evt, 
            bool recreateEvent = true, 
            bool deleteTestReg = false)
        {
            this.SignIn(folder);

            if (KeywordProvider.ManagerDefault.DoesEventExist(evt.Title))
            {
                if (recreateEvent)
                {
                    KeywordProvider.ManagerDefault.DeleteEvent(evt.Title);
                    KeywordProvider.EventCreator.CreateEvent(evt);
                }
                else
                {
                    if (deleteTestReg)
                    {
                        evt.Id = KeywordProvider.ManagerDefault.GetLatestEventId(evt.Title);
                        KeywordProvider.ManagerDefault.OpenFormDashboard(evt.Id);
                        PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.DeleteTestReg_Click();
                        PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.DeleteTestRegFrame.SelectByName();
                        PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.DeleteTestRegFrame.Delete_Click();
                        PageObject.PageObjectProvider.Manager.Dashboard.ReturnToList_Click();
                    }
                }
            }
            else
            {
                KeywordProvider.EventCreator.CreateEvent(evt);
            }

            evt.Id = KeywordProvider.ManagerDefault.GetLatestEventId(evt.Title);
        }
    }
}
