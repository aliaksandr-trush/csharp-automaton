namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddMerchandise
    {
        public void AddMerchandises(DataCollection.Merchandise merch, Event evt)
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
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.FeeAmount.Type(merch.MerchandiseFee.Value);
                    break;
                case FormData.MerchandiseType.Variable:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.VariableFeeMinAmount.Type(merch.MinMerchandiseFee.Value);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.VariableFeeMaxAmount.Type(merch.MaxMerchandiseFee.Value);
                    break;
                case FormData.MerchandiseType.Header:
                    break;
                default:
                    break;
            }

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.NameOnForm.Type(merch.MerchandiseName);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.NameOnReceipt.Type(merch.MerchandiseName);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.NameOnReports.Type(merch.MerchandiseName);

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.Advanced_Click();

            if ((evt.TaxRateOne != null) || (evt.TaxRateTwo != null))
            {
                KeywordProvider.AddTaxRate.AddTaxRates(evt.TaxRateOne, evt.TaxRateTwo, FormData.Location.Merchandise);
            }

            if ((evt.TaxRateOne != null) && (merch.ApplyTaxOne.HasValue))
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.ApplyTaxOne.Set(merch.ApplyTaxOne.Value);
            }

            if ((evt.TaxRateTwo != null) && (merch.ApplyTaxTwo.HasValue))
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.ApplyTaxTwo.Set(merch.ApplyTaxTwo.Value);
            }

            if (merch.DiscountCodes.Count != 0)
            {
                string discountCodeString = string.Empty;

                for (int i = 0; i < merch.DiscountCodes.Count; i++)
                {
                    discountCodeString += merch.DiscountCodes[i].Code;

                    if (merch.DiscountCodes[i].CodeType != FormData.DiscountCodeType.AccessCode)
                    {
                        discountCodeString += "=";

                        if (merch.DiscountCodes[i].CodeDirection == FormData.ChangePriceDirection.Decrease)
                        {
                            discountCodeString += "-";
                        }

                        discountCodeString += merch.DiscountCodes[i].Amount.ToString();

                        if (merch.DiscountCodes[i].CodeKind == FormData.ChangeType.Percent)
                        {
                            discountCodeString += "%";
                        }
                    }

                    if (i != merch.DiscountCodes.Count - 1)
                    {
                        discountCodeString += ",";
                    }
                }

                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.DiscountCodes.Type(discountCodeString);
            }

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.SaveAndClose_Click();
        }
    }
}
