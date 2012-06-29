namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject;
    using RegOnline.RegressionTest.PageObject.Manager;
    using RegOnline.RegressionTest.UIUtility;

    public class SignInToManager
    {
        private PageObject.PageObjectHelper PageObjectHelper = new PageObject.PageObjectHelper();
        private SignIn SignIntoManager = new SignIn();
        private Events Events = new Events();
        private PageObject.Manager.Dashboard.EventDtails Dashboard = new PageObject.Manager.Dashboard.EventDtails();

        public void SignIn(EventFolders.Folders folder)
        {
            this.SignIn(ConfigurationProvider.XmlConfig.AccountConfiguration.Login,
                ConfigurationProvider.XmlConfig.AccountConfiguration.Password, folder);
        }

        public void SignIn(string userName, string password, EventFolders.Folders folder)
        {
            UIUtilityProvider.UIHelper.OpenUrl(string.Format("{0}manager/login.aspx", ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrlWithHttps));
            PageObjectHelper.Allow_Click();
            SignIntoManager.UserName.Type(userName);
            SignIntoManager.Password.Type(password);
            SignIntoManager.SignInButton_Click();
            Events.Folder_Click(folder.ToString());
        }

        public void SignInAndRecreateEventAndGetEventId(EventFolders.Folders folder, DataCollection.Event evt, bool recreateEvent, bool deleteTestReg)
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
                        Dashboard.DeleteTestReg_Click();
                        Dashboard.DeleteTestRegFrame.SelectByName();
                        Dashboard.DeleteTestRegFrame.Delete_Click();
                        Dashboard.ReturnToList_Click();
                    }
                }
            }

            if (!KeywordProvider.ManagerDefault.DoesEventExist(evt.Title))
            {
                KeywordProvider.EventCreator.CreateEvent(evt);
            }

            evt.Id = KeywordProvider.ManagerDefault.GetLatestEventId(evt.Title);
        }
    }
}
