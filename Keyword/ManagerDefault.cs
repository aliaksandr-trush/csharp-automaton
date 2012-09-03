namespace RegOnline.RegressionTest.Keyword
{
    using System.Collections.Generic;
    using RegOnline.RegressionTest.PageObject.Manager;
    using RegOnline.RegressionTest.UIUtility;

    public class ManagerDefault
    {
        public bool DoesEventExist(string eventName)
        {
            bool exist = false;

            try
            {
                List<EventList_EventRow> eventRows = EventList_EventRow.GetEventRows(eventName);
                exist = !eventRows.Count.Equals(0);
            }
            catch
            {
                exist = false;
            }

            return exist;
        }

        public int GetLatestEventId(string eventName)
        {
            List<EventList_EventRow> eventRows = EventList_EventRow.GetEventRows(eventName);
            List<int> eventIds = new List<int>();

            foreach (EventList_EventRow row in eventRows)
            {
                eventIds.Add(row.EventId);
            }

            eventIds.Sort();
            eventIds.Reverse();
            return eventIds[0];
        }

        public void DeleteEvent(string eventName)
        {
            List<EventList_EventRow> eventRows = EventList_EventRow.GetEventRows(eventName);

            foreach (EventList_EventRow eventRow in eventRows)
            {
                eventRow.DeleteEvent();
            }
        }

        public void OpenFormDashboard(int eventId)
        {
            EventList_EventRow eventRow = new EventList_EventRow(eventId);
            eventRow.ClickTitleToOpenDashboard();
        }

        public void OpenFormDashboard(string eventName)
        {
            this.OpenFormDashboard(this.GetLatestEventId(eventName));
        }

        public bool HasErrorMessage(string errorMessage)
        {
            bool found = false;

            int count = PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaErrorMessage.Count;
            string[] errorList = new string[count];

            for (int i = 1; i <= count; i++)
            {
                errorList[i - 1] = UIUtilityProvider.UIHelper.GetText(string.Format(
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaErrorMessage.Locator + "[{0}]", i), LocateBy.XPath);
            }

            foreach (string error in errorList)
            {
                if (error.Contains(errorMessage))
                    found = true;
            }

            return found;
        }
    }
}
