namespace RegOnline.RegressionTest.PageObject.Manager.Dashboard
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class EventDtails : Window
    {
        public ButtonOrLink DeleteTestReg = new ButtonOrLink("ctl00_ctl00_cphDialog_cpMgrMain_lnkDeleteTestRegs", LocateBy.Id);
        public DeleteTestRegFrame DeleteTestRegFrame = new DeleteTestRegFrame("plain");
        public ButtonOrLink ReturnToList = new ButtonOrLink("ctl00_ctl00_cphDialog_cpMgrMain_hplBack", LocateBy.Id);
        public ButtonOrLink EditForm = new ButtonOrLink("ctl00_ctl00_cphDialog_cpMgrMain_lnkEdit", LocateBy.Id);

        public void DeleteTestReg_Click()
        {
            this.DeleteTestReg.WaitForDisplay();
            this.DeleteTestReg.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void ReturnToList_Click()
        {
            this.ReturnToList.WaitForDisplay();
            this.ReturnToList.Click();
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
}
