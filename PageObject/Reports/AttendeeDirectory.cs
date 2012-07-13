namespace RegOnline.RegressionTest.PageObject.Reports
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class AttendeeDirectory : Window
    {
        public TextBox EmailAddress = new TextBox("ctl00_cphDirectory_loginDirectory_UserName", LocateBy.Id);
        public TextBox Password = new TextBox("ctl00_cphDirectory_loginDirectory_Password", LocateBy.Id);
        public ButtonOrLink Continue = new ButtonOrLink("ctl00_cphDirectory_loginDirectory_LoginButton", LocateBy.Id);
        public ButtonOrLink SignOut = new ButtonOrLink("//a[contains(@onclick,'SignOut')]", LocateBy.XPath);

        public void Continue_Click()
        {
            this.Continue.WaitForDisplay();
            this.Continue.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void SignOut_Click()
        {
            this.SignOut.WaitForDisplay();
            this.SignOut.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
