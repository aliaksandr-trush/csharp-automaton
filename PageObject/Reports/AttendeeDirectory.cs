namespace RegOnline.RegressionTest.PageObject.Reports
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class AttendeeDirectory : Window
    {
        public Input EmailAddress = new Input("ctl00_cphDirectory_loginDirectory_UserName", LocateBy.Id);
        public Input Password = new Input("ctl00_cphDirectory_loginDirectory_Password", LocateBy.Id);
        public Clickable Continue = new Clickable("ctl00_cphDirectory_loginDirectory_LoginButton", LocateBy.Id);
        public Clickable SignOut = new Clickable("//a[contains(@onclick,'SignOut')]", LocateBy.XPath);

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
