namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;

    public class Merchandise : Window
    {
        public TextBox MerchInputField(DataCollection.Merchandise merch)
        {
            return new TextBox(merch.Id.ToString(), LocateBy.Id);
        }
    }
}
