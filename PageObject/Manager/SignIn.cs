namespace RegOnline.RegressionTest.PageObject.Manager
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class SignIn : Window
    {
        public TextBox UserName = new TextBox("ctl00_cphMaster_txtLogin", LocateBy.Id);
        public TextBox Password = new TextBox("ctl00_cphMaster_txtPassword", LocateBy.Id);
        public ButtonOrLink SignInButton = new ButtonOrLink("//span[@class='BiggerButtonBase']/a", LocateBy.XPath);

        public void SignInButton_Click()
        {
            this.SignInButton.WaitForDisplay();
            this.SignInButton.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
