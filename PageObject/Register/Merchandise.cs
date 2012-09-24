namespace RegOnline.RegressionTest.PageObject.Register
{
    using System;
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;

    public class Merchandise : Window
    {
        private TextBox PageContent_Div = new TextBox("pageContent", LocateBy.Id);

        public TextBox MerchInputField(DataCollection.MerchandiseItem merch)
        {
            Label merchNameLabel = new Label(string.Format("//label[text()='{0}']", merch.Name), LocateBy.XPath);
            merch.Id = Convert.ToInt32(merchNameLabel.GetAttribute("for"));
            return new TextBox(merch.Id.ToString(), LocateBy.Id);
        }

        public TextBox MerchDiscountCode(DataCollection.MerchandiseItem merch)
        {
            Label merchNameLabel = new Label(string.Format("//label[text()='{0}']", merch.Name), LocateBy.XPath);
            merch.Id = Convert.ToInt32(merchNameLabel.GetAttribute("for"));
            return new TextBox("dc" + merch.Id.ToString(), LocateBy.Id);
        }

        public void ClickPageContentDivToRefresh()
        {
            this.PageContent_Div.WaitForPresent();
            this.PageContent_Div.Click();
            Utilities.Utility.ThreadSleep(3);
            WaitForAJAX();
        }
    }
}
