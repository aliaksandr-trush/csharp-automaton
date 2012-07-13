namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class OnsiteKiosk : Window
    {
        public TextBox SearchCondition = new TextBox("ctl00_cph_txtSearch", LocateBy.Id);
        public ButtonOrLink Enter = new ButtonOrLink("ctl00_cph_btnSearch", LocateBy.Id);
        public ButtonOrLink OnsiteRegister = new ButtonOrLink("ctl00_cph_lnkRegister", LocateBy.Id);
        public ButtonOrLink NewSearch = new ButtonOrLink("ctl00_cph_btnReset", LocateBy.Id);
        public TextBox Password = new TextBox("ctl00_cph_txtPassword", LocateBy.Id);
        public ButtonOrLink SubmitPassword = new ButtonOrLink("ctl00_cph_btnPassword", LocateBy.Id);

        public void Enter_Click()
        {
            this.Enter.WaitForDisplay();
            this.Enter.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void OnsiteRegister_Click()
        {
            this.OnsiteRegister.WaitForDisplay();
            this.OnsiteRegister.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void NewSearch_Click()
        {
            this.NewSearch.WaitForDisplay();
            this.NewSearch.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void SubmitPassword_Click()
        {
            this.SubmitPassword.WaitForDisplay();
            this.SubmitPassword.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
