namespace RegOnline.RegressionTest.PageObject.Manager.Dashboard
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class EventDetails : Window
    {
        public ButtonOrLink DeleteTestReg = new ButtonOrLink("ctl00_ctl00_cphDialog_cpMgrMain_lnkDeleteTestRegs", LocateBy.Id);
        public DeleteTestRegFrame DeleteTestRegFrame = new DeleteTestRegFrame("plain");
        public ButtonOrLink EditForm = new ButtonOrLink("ctl00_ctl00_cphDialog_cpMgrMain_lnkEdit", LocateBy.Id);
        public ButtonOrLink ThirdParty = new ButtonOrLink("lnkIntegrations", LocateBy.Id);
        public ThirdParty ThirdPartyIntegrations = new ThirdParty("plain");
        public ButtonOrLink TotalRegs = new ButtonOrLink("ctl00_ctl00_cphDialog_cpMgrMain_lnkTotalRegs", LocateBy.Id);
        public ButtonOrLink SelfKiosk = new ButtonOrLink("//a[@class='frmDashLink kiosk']", LocateBy.XPath);
        public LaunchSelfKiosk LaunchSelfKiosk = new LaunchSelfKiosk("plain");

        public void SelfKiosk_Click()
        {
            this.SelfKiosk.WaitForDisplay();
            this.SelfKiosk.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void DeleteTestReg_Click()
        {
            this.DeleteTestReg.WaitForDisplay();
            this.DeleteTestReg.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void EditForm_Click()
        {
            this.EditForm.WaitForDisplay();
            this.EditForm.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void ThirdParty_Click()
        {
            this.ThirdParty.WaitForDisplay();
            this.ThirdParty.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class DeleteTestRegFrame : Frame
    {
        public DeleteTestRegFrame(string name) : base(name) { }

        public ButtonOrLink Delete = new ButtonOrLink("//a[contains(@onclick,'DeleteTestRegistrations')]/span", LocateBy.XPath);

        public void Delete_Click()
        {
            this.Delete.WaitForDisplay();
            this.Delete.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }
    }

    public class LaunchSelfKiosk : Frame
    {
        public LaunchSelfKiosk(string name) : base(name) { }

        public CheckBox RequireAuthentication = new CheckBox("ctl00_cphDialog_chkRequirePassword", LocateBy.Id);
        public CheckBox AllowOnsiteReg = new CheckBox("ctl00_cphDialog_chkAllowOnsiteRegistrations", LocateBy.Id);
        public ButtonOrLink LaunchKiosk = new ButtonOrLink("ctl00_cphDialog_btnContinue", LocateBy.Id);

        public void LaunchKiosk_Click()
        {
            this.LaunchKiosk.WaitForDisplay();
            this.LaunchKiosk.Click();
            Utility.ThreadSleep(5);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class ThirdParty : Frame
    {
        public ThirdParty(string name) : base(name) { }

        public ButtonOrLink ExternalAuthentication = new ButtonOrLink("//div[@id='ctl00_cphDialog_rtsIntegrations']//ul[@class='rtsUL']/li[5]/a[@class='rtsLink']", LocateBy.XPath);

        public void ExternalAuthentication_Click()
        {
            this.ExternalAuthentication.WaitForDisplay();
            this.ExternalAuthentication.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
        }
    }
}
