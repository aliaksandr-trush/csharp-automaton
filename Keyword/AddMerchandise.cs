namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddMerchandise
    {
        private PageObject.Builder.Merchandise Merchandise = new PageObject.Builder.Merchandise();

        public void AddMerchandises(DataCollection.Merchandise merch)
        {
            if (Merchandise.EmptyAddMerchandise.IsPresent)
            {
                Merchandise.EmptyAddMerchandise_Click();
            }
            else
            {
                Merchandise.AddMerchandise_Click();
            }

            Merchandise.MerchandiseDefine.SelectByName();

            Merchandise.MerchandiseDefine.MerchandiseType_Select(((int)merch.MerchandiseType).ToString());

            switch (merch.MerchandiseType)
            {
                case FormData.MerchandiseType.Fixed:
                    Merchandise.MerchandiseDefine.FeeAmount.Type(merch.MerchandiseFee);
                    break;
                case FormData.MerchandiseType.Variable:
                    Merchandise.MerchandiseDefine.VariableFeeMinAmount.Type(merch.MinMerchandiseFee);
                    Merchandise.MerchandiseDefine.VariableFeeMaxAmount.Type(merch.MaxMerchandiseFee);
                    break;
                case FormData.MerchandiseType.Header:
                    break;
                default:
                    break;
            }

            Merchandise.MerchandiseDefine.NameOnForm.Type(merch.MerchandiseName);
            Merchandise.MerchandiseDefine.NameOnReceipt.Type(merch.MerchandiseName);
            Merchandise.MerchandiseDefine.NameOnReports.Type(merch.MerchandiseName);

            Merchandise.MerchandiseDefine.SaveAndClose_Click();
        }
    }
}
