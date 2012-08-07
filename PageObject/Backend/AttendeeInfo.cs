﻿namespace RegOnline.RegressionTest.PageObject.Backend
{
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public class AttendeeInfo : Window
    {
        public ButtonOrLink AgendaEdit = new ButtonOrLink("//div[@id='agenda']//a/img", LocateBy.XPath);

        public void OpenUrl(int registerId)
        {
            string url = string.Format(
                "{0}reports/Attendee.aspx?EventSessionId={1}&registerId={2}",
                ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrl,
                FormData.EventSessionId,
                registerId);

            UIUtilityProvider.UIHelper.OpenUrl(url);
        }

        public Label AgendaLable(AgendaItem agenda)
        {
            return new Label(string.Format("tdCustomField{0}", agenda.Id), UIUtility.LocateBy.Id);
        }

        public void AgendaEdit_Click()
        {
            this.AgendaEdit.WaitForDisplay();
            this.AgendaEdit.Click();
            Utilities.Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
