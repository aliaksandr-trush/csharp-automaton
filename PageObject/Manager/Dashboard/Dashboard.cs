namespace RegOnline.RegressionTest.PageObject.Manager.Dashboard
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class Dashboard : Window
    {
        public EventDetails EventDetails = new EventDetails();
        public Reports Reports = new Reports();
        public AttendeeDirectory AttendeeDirectory = new AttendeeDirectory();

        public ButtonOrLink Activate = new ButtonOrLink("ctl00_ctl00_cphDialog_cpMgrMain_btnStatusChange", LocateBy.Id);
        public ActivateEvent ActivateEvent = new ActivateEvent("plain");
        public ButtonOrLink ChangeStatus = new ButtonOrLink("//*[@id='ctl00_ctl00_cphDialog_cpMgrMain_wrpStatusChange']/a", LocateBy.XPath);
        public ChangeStatusFrame ChangeStatusFrame = new ChangeStatusFrame("plain");
        public ButtonOrLink ReturnToList = new ButtonOrLink("ctl00_ctl00_cphDialog_cpMgrMain_hplBack", LocateBy.Id);

        public void ReturnToList_Click()
        {
            this.ReturnToList.WaitForDisplay();
            this.ReturnToList.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void Activate_Click()
        {
            this.Activate.WaitForDisplay();
            this.Activate.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void ChangeStatus_Click()
        {
            this.ChangeStatus.WaitForDisplay();
            this.ChangeStatus.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void DashboardTab_Click(FormData.DashboardTab tab)
        {
            ButtonOrLink targetTab = new ButtonOrLink(CustomStringAttribute.GetCustomString(tab), LocateBy.LinkText);
            targetTab.WaitForDisplay();
            targetTab.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class ActivateEvent : Frame
    {
        public ActivateEvent(string name) : base(name) { }

        public CheckBox RemoveTestReg = new CheckBox("chkDeleteTestRegs", LocateBy.Id);
        public ButtonOrLink Activate = new ButtonOrLink("//span[@class='BiggerButtonBaseBlue']/a", LocateBy.XPath);
        public ButtonOrLink Cancel = new ButtonOrLink("//span[@class='BiggerButtonBase']/a", LocateBy.XPath);

        public void Activate_Click()
        {
            this.Activate.WaitForDisplay();
            this.Activate.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        public void Cancel_Click()
        {
            this.Cancel.WaitForDisplay();
            this.Cancel.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }
    }

    public class ChangeStatusFrame : Frame
    {
        public ChangeStatusFrame(string name) : base(name) { }

        public CheckBox ChangeStatusNow = new CheckBox("chkChangeStatusNow", LocateBy.Id);
        public MultiChoiceDropdown Status = new MultiChoiceDropdown("ddlStatusesCurrent", LocateBy.Id);
        public ButtonOrLink OK = new ButtonOrLink("btnOK", LocateBy.Id);
        public ButtonOrLink Cancel = new ButtonOrLink("btnCancel", LocateBy.Id);

        public void ChangeStatusNow_Set(bool check)
        {
            this.ChangeStatusNow.WaitForDisplay();
            this.ChangeStatusNow.Set(check);
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void OK_Click()
        {
            this.OK.WaitForDisplay();
            this.OK.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        public void Cancel_Click()
        {
            this.Cancel.WaitForDisplay();
            this.Cancel.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }
    }
}
