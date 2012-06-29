namespace RegOnline.RegressionTest.PageObject.Reports
{
    using System;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class StandardReports : Window
    {
        private string WindowNameFormat = "RolwinActiveReportsReportServer{0}EventSessionId{1}eventID{2}";

        public List<AgendaReportRow> AgendaReportRows = new List<AgendaReportRow>();

        public StandardReports(FormData.StandardReports reportType)
        {
            Name = string.Format(WindowNameFormat, CustomStringAttribute.GetCustomString(reportType), 
                GetQueryStringValue("EventSessionID"),
                Convert.ToInt32(GetQueryStringValue("eventID")));

            SelectByName();

            switch (reportType)
            {
                case FormData.StandardReports.AgendaReport:
                    this.GetAgendaReportRows();
                    break;
                case FormData.StandardReports.AttendeeReport:
                    break;
                default:
                    break;
            }
        }

        private void GetAgendaReportRows()
        {
            string agendaName = "//table[@id='rptParentTable']/tbody/tr[td/div[@class='section']]";
            Label AgendaName = new Label(agendaName, LocateBy.XPath);

            for (int i = 1; i <= AgendaName.Count; i++)
            {
                AgendaReportRow agendaReportRow = new AgendaReportRow(i);
                AgendaReportRows.Add(agendaReportRow);
            }
        }
    }

    public class AgendaReportRow
    {
        public Label AgendaName;
        public List<AttendeeRow> Attendees = new List<AttendeeRow>();

        private string agendaName = "//table[@id='rptParentTable']/tbody/tr[td/div[@class='section']][{0}]";
        private string agendaFollowingTr = "//table[@id='rptParentTable']/tbody/tr[td/div[@class='section']][{0}]/following-sibling::*";
        private string attendeeTr = "//table[@id='rptParentTable']/tbody/tr[td/div[@class='section']][1]/following-sibling::*[{0}]";
        private string attendeeFollowingTrIsAgenda = "//table[@id='rptParentTable']/tbody/tr[td/div[@class='section']][1]/following-sibling::*[{0}]/following-sibling::*[1]//div";

        public AgendaReportRow(int agendaIndex)
        {
            Label AgendaFollowingTr = new Label(string.Format(agendaFollowingTr, agendaIndex), LocateBy.XPath);
            AgendaName = new Label(string.Format(agendaName, agendaIndex), LocateBy.XPath);

            for (int i = 1; i <= AgendaFollowingTr.Count; i++)
            {
                Label AttendeeFollowingTrIsAgenda = new Label(string.Format(attendeeFollowingTrIsAgenda, i), LocateBy.XPath);
                AttendeeRow attendeeRow = new AttendeeRow();
                attendeeRow.CheckAttendee = new CheckBox(string.Format(attendeeTr, i) + "/td[1]/input", LocateBy.XPath);
                attendeeRow.AttendeeId = new ButtonOrLink(string.Format(attendeeTr, i) + "/td[2]/a", LocateBy.XPath);
                attendeeRow.AgendaStatus = new Label(string.Format(attendeeTr, i) + "/td[3]", LocateBy.XPath);
                attendeeRow.AttendeeName = new Label(string.Format(attendeeTr, i) + "/td[4]", LocateBy.XPath);
                attendeeRow.AttendeeCompany = new Label(string.Format(attendeeTr, i) + "/td[5]", LocateBy.XPath);
                attendeeRow.AttendeeEmail = new Label(string.Format(attendeeTr, i) + "/td[6]", LocateBy.XPath);
                attendeeRow.AttendeeWorkPhone = new Label(string.Format(attendeeTr, i) + "/td[7]", LocateBy.XPath);
                attendeeRow.AttendeeHomePhone = new Label(string.Format(attendeeTr, i) + "/td[8]", LocateBy.XPath);
                attendeeRow.AttendeeRegisterDate = new Label(string.Format(attendeeTr, i) + "/td[9]", LocateBy.XPath);
                attendeeRow.AttendeeModifyDate = new Label(string.Format(attendeeTr, i) + "/td[10]", LocateBy.XPath);

                Attendees.Add(attendeeRow);

                if (AttendeeFollowingTrIsAgenda.IsPresent)
                {
                    break;
                }
            }
        }
    }

    public class AttendeeRow
    {
        public CheckBox CheckAttendee;
        public ButtonOrLink AttendeeId;
        public Label AgendaStatus;
        public Label AttendeeName;
        public Label AttendeeCompany;
        public Label AttendeeEmail;
        public Label AttendeeWorkPhone;
        public Label AttendeeHomePhone;
        public Label AttendeeRegisterDate;
        public Label AttendeeModifyDate;
    }
}
