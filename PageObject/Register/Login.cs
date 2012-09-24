﻿namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class Login : Window
    {
        public Input EmailAddress = new Input("ctl00_cph_ctlEmailMemID_txtEmail", LocateBy.Id);
        public Input Password = new Input("ctl00_cph_txtPassword", LocateBy.Id);
        public Clickable StartNewRegistration = new Clickable("ctl00_cph_lnkNotRegistered", LocateBy.Id);
        public Clickable ForgotPassword = new Clickable("//input[@id='ctl00_cph_txtPassword']/following-sibling::a", LocateBy.XPath);
        public ForgotPasswordPopup ForgotPasswordPopup = new ForgotPasswordPopup(0);
        public Label PasswordOnDupEmail = new Label("//p[contains(text(),'Our records show that this email address was used to register for this event')]", LocateBy.XPath);
        public Label PasswordOnAutoRecall = new Label("//p[contains(text(),'To automatically recall your personal information')]", LocateBy.XPath);

        public void StartNewRegistration_Click()
        {
            this.StartNewRegistration.WaitForDisplay();
            this.StartNewRegistration.Click();
            Utility.ThreadSleep(2);
            WaitForLoad();
        }

        public void ForgotPassword_Click()
        {
            this.ForgotPassword.WaitForDisplay();
            this.ForgotPassword.Click();
            Utility.ThreadSleep(2);
            WaitForLoad();
        }
    }

    public class ForgotPasswordPopup : Frame
    {
        public ForgotPasswordPopup(int index) : base(index) { }

        public Clickable Close = new Clickable("//*[@id='ctl00_cph_wrpEmailMembershipID']/div/a", LocateBy.XPath);
        public Input EmailAddress = new Input("ctl00_cph_ctlEmailMemID_txtEmail", LocateBy.Id);

        public void Close_Click()
        {
            this.Close.WaitForDisplay();
            this.Close.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            WaitForLoad();
            SwitchToMain();
        }
    }
}
