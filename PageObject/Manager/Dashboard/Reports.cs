namespace RegOnline.RegressionTest.PageObject.Manager.Dashboard
{
    using System;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Reports;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class Reports : Window
    {
        public Clickable AgendaReportLink = new Clickable("ctl00_ctl00_cphDialog_cpMgrMain_lnkAgendaReport", LocateBy.Id);

        public void AgendaReportLink_Click()
        {
            this.AgendaReportLink.WaitForDisplay();
            this.AgendaReportLink.Click();
            Utility.ThreadSleep(5);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
