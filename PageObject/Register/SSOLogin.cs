namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class SSOLogin : Window
    {
        public MultiChoiceDropdown Email = new MultiChoiceDropdown("emailList", LocateBy.Id);
        public MultiChoiceDropdown Password = new MultiChoiceDropdown("ddlPasswords", LocateBy.Id);
        public Clickable Login = new Clickable("btnLogin", LocateBy.Id);

        public void Login_Click()
        {
            this.Login.WaitForDisplay();
            this.Login.Click();
            Utility.ThreadSleep(2);
            WaitForLoad();
        }
    }
}
