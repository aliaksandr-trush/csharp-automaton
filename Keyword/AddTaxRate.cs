namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddTaxRate
    {
        public void AddTaxRates(TaxRate taxRateOne, TaxRate taxRateTwo, FormData.Location location)
        {
            switch (location)
            {
                case FormData.Location.RegType:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.AddTaxRate_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.TaxRateDefine.SelectByName();

                    if (taxRateOne != null)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.TaxRateDefine.TaxOneCaption.Type(taxRateOne.TaxRateCaption);
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.TaxRateDefine.TaxOneRate.Type(taxRateOne.Rate);
                    }

                    if (taxRateTwo != null)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.TaxRateDefine.TaxTwoCaption.Type(taxRateTwo.TaxRateCaption);
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.TaxRateDefine.TaxTwoRate.Type(taxRateTwo.Rate);
                    }

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.EventFeeDefine.TaxRateDefine.SaveAndClose_Click();
                    break;
                case FormData.Location.Agenda:
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddTaxRate_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.SelectByName();

                    if (taxRateOne != null)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.TaxOneCaption.Type(taxRateOne.TaxRateCaption);
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.TaxOneRate.Type(taxRateOne.Rate);
                    }

                    if (taxRateTwo != null)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.TaxTwoCaption.Type(taxRateTwo.TaxRateCaption);
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.TaxTwoRate.Type(taxRateTwo.Rate);
                    }

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.SaveAndClose_Click();
                    break;
                default:
                    break;
            }
        }
    }
}
