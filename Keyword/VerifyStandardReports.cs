namespace RegOnline.RegressionTest.Keyword
{
    using System.Collections.Generic;
    using RegOnline.RegressionTest.DataCollection;

    public class VerifyStandardReports
    {
        public PageObject.Reports.StandardReports standardReport;

        public List<string> VerifyStandardReport(FormData.StandardReports reportType)
        {
            this.standardReport = new PageObject.Reports.StandardReports(reportType);
            List<string> results = new List<string>();

            switch (reportType)
            {
                case FormData.StandardReports.AgendaReport:
                    foreach (PageObject.Reports.AgendaReportRow agendarow in standardReport.AgendaReportRows)
                    {
                        results.Add(agendarow.AgendaName.Text);
                    }
                    break;
                case FormData.StandardReports.AttendeeReport:
                    break;
                default:
                    break;
            }

            return results;
        }
    }
}
