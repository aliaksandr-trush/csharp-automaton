﻿namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddMerchandise
    {
        public void AddMerchandises(DataCollection.MerchandiseItem merch, Event evt)
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

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.MerchandiseType_Select(((int)merch.Type).ToString());

            switch (merch.Type)
            {
                case DataCollection.EventData_Common.MerchandiseType.Fixed:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.FeeAmount.Type(merch.Price.Value);
                    break;
                case DataCollection.EventData_Common.MerchandiseType.Variable:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.VariableFeeMinAmount.Type(merch.MinPrice.Value);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.VariableFeeMaxAmount.Type(merch.MaxPrice.Value);
                    break;
                case DataCollection.EventData_Common.MerchandiseType.Header:
                    break;
                default:
                    break;
            }

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.NameOnForm.Type(merch.Name);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.NameOnReceipt.Type(merch.Name);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.NameOnReports.Type(merch.Name);

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.Advanced_Click();

            if ((evt.TaxRateOne != null) || (evt.TaxRateTwo != null))
            {
                KeywordProvider.Add_TaxRate.AddTaxRates(evt.TaxRateOne, evt.TaxRateTwo, DataCollection.EventData_Common.Location.Merchandise);
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
                string discountCodeString = CustomFieldCode.GenerateBulkCodes(merch.DiscountCodes);

                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.DiscountCodes.Type(discountCodeString);
            }

            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.SaveAndClose_Click();
        }
    }
}
