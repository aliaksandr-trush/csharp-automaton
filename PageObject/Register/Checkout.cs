﻿namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class Checkout : Window
    {
        public MultiChoiceDropdown PaymentMethodList = new MultiChoiceDropdown("ctl00_cph_ddlPaymentMethods", LocateBy.Id);
        public ButtonOrLink Finish = new ButtonOrLink("//div[@class='buttonGroup']//button[@type='submit']", LocateBy.XPath);
        public ButtonOrLink AANoThanks = new ButtonOrLink("//div[@class='offerSubmit']//span[text()='No thanks']/..", LocateBy.XPath);
        public CheckBox AggreementToWaiver = new CheckBox("ctl00_cph_chkAITerms", LocateBy.Id);

        public Label FeeSummary_Total = new Label("//table[@class='columnHeader dataTable multiColumn']//td[text()='Total:']/following-sibling::td[@class='currency']", LocateBy.XPath);

        public void Finish_Click()
        {
            this.Finish.WaitForDisplay();
            this.Finish.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AANoThanks_Click()
        {
            this.AANoThanks.WaitForDisplay();
            this.AANoThanks.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
