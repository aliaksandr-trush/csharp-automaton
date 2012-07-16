namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;

    public class AddAttendeeDirectory
    {
        public string CreateAttendeeDirectory(AttendeeDirectory directory)
        {
            PageObject.PageObjectProvider.Manager.Dashboard.AttendeeDirectory.AddDirectory_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.AttendeeDirectory.DirectoryDefine.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.AttendeeDirectory.DirectoryDefine.DirectoryName.Type(directory.DirectoryName);
            PageObject.PageObjectProvider.Manager.Dashboard.AttendeeDirectory.DirectoryDefine.LinksAndSecurity_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.AttendeeDirectory.DirectoryDefine.ShareDirectory.Set(directory.ShareDirectory);
            PageObject.PageObjectProvider.Manager.Dashboard.AttendeeDirectory.DirectoryDefine.RequireLogin.Set(directory.RequireLogin);
            PageObject.PageObjectProvider.Manager.Dashboard.AttendeeDirectory.DirectoryDefine.Apply_Click();
            string directoryURL = PageObject.PageObjectProvider.Manager.Dashboard.AttendeeDirectory.DirectoryDefine.DirectoryLink.Value;
            PageObject.PageObjectProvider.Manager.Dashboard.AttendeeDirectory.DirectoryDefine.Cancel_Click();

            return directoryURL;
        }
    }
}
