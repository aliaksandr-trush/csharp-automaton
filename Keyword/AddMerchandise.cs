namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddMerchandise
    {
        public void AddMerchandises(DataCollection.Merchandise merch)
        {
            if (PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.EmptyAddMerchandise.IsPresent)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.EmptyAddMerchandise_Click();
            }
            else
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.AddMerchandise_Click();
            }

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.SelectByName();

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.MerchandiseType_Select(((int)merch.MerchandiseType).ToString());

            switch (merch.MerchandiseType)
            {
                case FormData.MerchandiseType.Fixed:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.FeeAmount.Type(merch.MerchandiseFee);
                    break;
                case FormData.MerchandiseType.Variable:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.VariableFeeMinAmount.Type(merch.MinMerchandiseFee);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.VariableFeeMaxAmount.Type(merch.MaxMerchandiseFee);
                    break;
                case FormData.MerchandiseType.Header:
                    break;
                default:
                    break;
            }

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.NameOnForm.Type(merch.MerchandiseName);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.NameOnReceipt.Type(merch.MerchandiseName);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.NameOnReports.Type(merch.MerchandiseName);

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.SaveAndClose_Click();
        }
    }
}
