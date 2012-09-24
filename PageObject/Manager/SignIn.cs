namespace RegOnline.RegressionTest.PageObject.Manager
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class SignIn : Window
    {
        public Input UserName = new Input("ctl00_cphMaster_txtLogin", LocateBy.Id);
        public Input Password = new Input("ctl00_cphMaster_txtPassword", LocateBy.Id);
        public Clickable SignInButton = new Clickable("//span[@class='BiggerButtonBase']/a", LocateBy.XPath);

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
