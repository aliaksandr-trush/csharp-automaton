namespace RegOnline.RegressionTest.PageObject.Register
{
    using System;
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;

    public class Merchandise : Window
    {
        private Input PageContent_Div = new Input("pageContent", LocateBy.Id);

        public Input MerchInputField(DataCollection.MerchandiseItem merch)
        {
            Label merchNameLabel = new Label(string.Format("//label[text()='{0}']", merch.Name), LocateBy.XPath);
            merch.Id = Convert.ToInt32(merchNameLabel.GetAttribute("for"));
            return new Input(merch.Id.ToString(), LocateBy.Id);
        }

        public Input MerchDiscountCode(DataCollection.MerchandiseItem merch)
        {
            Label merchNameLabel = new Label(string.Format("//label[text()='{0}']", merch.Name), LocateBy.XPath);
            merch.Id = Convert.ToInt32(merchNameLabel.GetAttribute("for"));
            return new Input("dc" + merch.Id.ToString(), LocateBy.Id);
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
