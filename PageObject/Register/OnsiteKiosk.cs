﻿namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class OnsiteKiosk : Window
    {
        public Input SearchCondition = new Input("ctl00_cph_txtSearch", LocateBy.Id);
        public Clickable Enter = new Clickable("ctl00_cph_btnSearch", LocateBy.Id);
        public Clickable OnsiteRegister = new Clickable("ctl00_cph_lnkRegister", LocateBy.Id);
        public Clickable NewSearch = new Clickable("ctl00_cph_btnReset", LocateBy.Id);
        public Input Password = new Input("ctl00_cph_txtPassword", LocateBy.Id);
        public Clickable SubmitPassword = new Clickable("ctl00_cph_btnPassword", LocateBy.Id);

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
