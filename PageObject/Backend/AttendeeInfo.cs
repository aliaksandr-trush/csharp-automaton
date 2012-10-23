namespace RegOnline.RegressionTest.PageObject.Backend
{
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;
    using System;

    public class AttendeeInfo : Window
    {
        public Clickable AgendaEdit = new Clickable("//div[@id='agenda']//a/img", LocateBy.XPath);
        private Clickable ChargeRemainingBalance = new Clickable("inButCharge", LocateBy.Id);
        public Label TransactionTotal = new Label("//td[@id='tdTotalTransactions']/b", LocateBy.XPath);

        public void OpenUrl(int registerId)
        {
            string url = string.Format(
                "{0}reports/Attendee.aspx?EventSessionId={1}&registerId={2}",
                ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl,
                FormData.EventSessionId,
                registerId);

            UIUtil.DefaultProvider.OpenUrl(url);
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

        public void ChargeRemainingBalance_Click()
        {
            this.ChargeRemainingBalance.WaitForDisplay();
            this.ChargeRemainingBalance.Click();
            Utilities.Utility.ThreadSleep(0.5);
            UIUtility.UIUtil.DefaultProvider.GetConfirmation();
            Utilities.Utility.ThreadSleep(1.5);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class AttendeeInfoTransactionRow
    {
        public int Id;
        public DateTime Date;
        public Label Type;
        public double Amount;

        public AttendeeInfoTransactionRow()
        {
            Label transactionRowTr = new Label("//div[@id='transactions']//tbody/tr", LocateBy.XPath);
            Label latestTransactionRowTr = new Label(string.Format(
                "//div[@id='transactions']//tbody/tr[{0}]", transactionRowTr.Count - 1), LocateBy.XPath);
            Label transactionId = new Label(latestTransactionRowTr.Locator + "/td[1]", LocateBy.XPath);
            Label transactionDate = new Label(latestTransactionRowTr.Locator + "/td[2]/nobr", LocateBy.XPath);
            Label transactionAmount = new Label(latestTransactionRowTr.Locator + "/td[6]/nobr", LocateBy.XPath);

            this.Id = Convert.ToInt32(transactionId.Text.Trim());
            this.Date = Convert.ToDateTime(transactionDate.Text);
            this.Type = new Label(latestTransactionRowTr.Locator + "/td[3]", LocateBy.XPath);

            this.Amount = Utilities.MoneyTool.RemoveMoneyCurrencyCode(transactionAmount.Text.Trim());
        }
    }
}
