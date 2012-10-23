namespace RegOnline.RegressionTest.Keyword
{
    using System.Collections.Generic;
    using RegOnline.RegressionTest.DataCollection;

    public class VerifyStandardReports
    {
        public PageObject.Reports.StandardReports standardReport;

        public List<string> VerifyStandardReport(DataCollection.EventData_Common.StandardReports reportType)
        {
            this.standardReport = new PageObject.Reports.StandardReports(reportType);
            List<string> results = new List<string>();

            switch (reportType)
            {
                case DataCollection.EventData_Common.StandardReports.AgendaReport:
                    foreach (PageObject.Reports.AgendaReportRow agendarow in standardReport.AgendaReportRows)
                    {
                        results.Add(agendarow.AgendaName.Text);
                    }
                    break;
                case DataCollection.EventData_Common.StandardReports.AttendeeReport:
                    break;
                default:
                    break;
            }

            return results;
        }
    }
}
