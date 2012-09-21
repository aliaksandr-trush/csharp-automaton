namespace RegOnline.RegressionTest.PageObject.Manager.Dashboard
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.Utilities;

    public class AttendeeDirectory : Window
    {
        public Clickable AddDirectory = new Clickable("ctl00_ctl00_cphDialog_cpMgrMain_aNewDirectory", LocateBy.Id);
        public DirectoryDefine DirectoryDefine = new DirectoryDefine("dialog");
        public Clickable DeleteFirstDirectory = new Clickable(
            "ctl00_ctl00_cphDialog_cpMgrMain_grdDirectories_ctl02_lnkDeleteDirectory", LocateBy.Id);
        private Clickable OKButton_DeletePopup = new Clickable("//span[text()='OK']", LocateBy.XPath);

        public void AddDirectory_Click()
        {
            this.AddDirectory.WaitForDisplay();
            this.AddDirectory.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void DeleteFirstDirectory_Click()
        {
            this.DeleteFirstDirectory.WaitForDisplay();
            this.DeleteFirstDirectory.Click();
            Utility.ThreadSleep(2);
        }

        public void OKButton_DeletePopup_Click()
        {
            this.OKButton_DeletePopup.WaitForDisplay();
            this.OKButton_DeletePopup.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class DirectoryDefine : Frame
    {
        public DirectoryDefine(string name) : base(name) { }

        public Input DirectoryName = new Input("crGeneral_tbReportName", LocateBy.Id);
        public Clickable LinksAndSecurity = new Clickable("Tabimg_M4", LocateBy.Id);
        public CheckBox RequireLogin = new CheckBox("crAdvanced_cbRequireLogin", LocateBy.Id);
        public Clickable Apply = new Clickable("btnSave", LocateBy.Id);
        public Clickable Cancel = new Clickable("btnCancel", LocateBy.Id);
        public Input DirectoryLink = new Input("crAdvanced_txtShareLink", LocateBy.Id);
        public CheckBox ShareDirectory = new CheckBox("crAdvanced_cbShareReport", LocateBy.Id);

        public void LinksAndSecurity_Click()
        {
            this.LinksAndSecurity.WaitForDisplay();
            this.LinksAndSecurity.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void Apply_Click()
        {
            this.Apply.WaitForDisplay();
            this.Apply.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void Cancel_Click()
        {
            this.Cancel.WaitForDisplay();
            this.Cancel.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            SwitchToMain();
        }
    }
}
