namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;

    public class Merchandise : Window
    {
        private TextBox PageContent_Div = new TextBox("pageContent", LocateBy.Id);

        public TextBox MerchInputField(DataCollection.MerchandiseItem merch)
        {
            return new TextBox(merch.Id.ToString(), LocateBy.Id);
        }

        public TextBox MerchDiscountCode(DataCollection.MerchandiseItem merch)
        {
            return new TextBox("dc" + merch.Id.ToString(), LocateBy.Id);
        }

        public void ClickPageContentDivToRefresh()
        {
            this.PageContent_Div.WaitForPresent();
            this.PageContent_Div.Click();
            WaitForAJAX();
        }
    }
}
