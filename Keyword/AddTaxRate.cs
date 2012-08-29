namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddTaxRate
    {
        public void AddTaxRates(TaxRate taxRateOne, TaxRate taxRateTwo, FormData.Location location)
        {
            if (((taxRateOne != null) && taxRateOne.ToBeAdded ) || ((taxRateTwo != null) && taxRateTwo.ToBeAdded))
            {
                switch (location)
                {
                    case FormData.Location.RegType:
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.AddTaxRate_Click();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.TaxRate_Define.SelectByName();

                        if (taxRateOne != null)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.TaxRate_Define.TaxOneCaption.Type(taxRateOne.TaxRateCaption);
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.TaxRate_Define.TaxOneRate.Type(taxRateOne.Rate);
                            taxRateOne.ToBeAdded = false;
                        }

                        if (taxRateTwo != null)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.TaxRate_Define.TaxTwoCaption.Type(taxRateTwo.TaxRateCaption);
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.TaxRate_Define.TaxTwoRate.Type(taxRateTwo.Rate);
                            taxRateTwo.ToBeAdded = false;
                        }

                        if (((taxRateOne != null) && taxRateOne.Country.HasValue) || ((taxRateTwo != null) && taxRateTwo.Country.HasValue))
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.TaxRate_Define.ApplyToSelectedCountry_Set(true);

                            if ((taxRateOne != null) && taxRateOne.Country.HasValue)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.TaxRate_Define.ApplyToCountry(taxRateOne.Country.Value).Set(true);
                            }

                            if ((taxRateTwo != null) && taxRateTwo.Country.HasValue)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.TaxRate_Define.ApplyToCountry(taxRateTwo.Country.Value).Set(true);
                            }
                        }

                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.TaxRate_Define.SaveAndClose_Click();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDefine.RegTypeFee_Define.SelectByName();
                        break;
                    case FormData.Location.Agenda:
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AddTaxRate_Click();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.SelectByName();

                        if (taxRateOne != null)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.TaxOneCaption.Type(taxRateOne.TaxRateCaption);
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.TaxOneRate.Type(taxRateOne.Rate);
                            taxRateOne.ToBeAdded = false;
                        }

                        if (taxRateTwo != null)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.TaxTwoCaption.Type(taxRateTwo.TaxRateCaption);
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.TaxTwoRate.Type(taxRateTwo.Rate);
                            taxRateTwo.ToBeAdded = false;
                        }

                        if (((taxRateOne != null) && taxRateOne.Country.HasValue) || ((taxRateTwo != null) && taxRateTwo.Country.HasValue))
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.ApplyToSelectedCountry_Set(true);

                            if ((taxRateOne != null) && taxRateOne.Country.HasValue)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.ApplyToCountry(taxRateOne.Country.Value).Set(true);
                            }

                            if ((taxRateTwo != null) && taxRateTwo.Country.HasValue)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.ApplyToCountry(taxRateTwo.Country.Value).Set(true);
                            }
                        }

                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.TaxRateDefine.SaveAndClose_Click();
                        break;
                    case FormData.Location.Merchandise:
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.AddTaxRate_Click();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.TaxRate_Define.SelectByName();

                        if (taxRateOne != null)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.TaxRate_Define.TaxOneCaption.Type(taxRateOne.TaxRateCaption);
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.TaxRate_Define.TaxOneRate.Type(taxRateOne.Rate);
                            taxRateOne.ToBeAdded = false;
                        }

                        if (taxRateTwo != null)
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.TaxRate_Define.TaxTwoCaption.Type(taxRateTwo.TaxRateCaption);
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.TaxRate_Define.TaxTwoRate.Type(taxRateTwo.Rate);
                            taxRateTwo.ToBeAdded = false;
                        }

                        if (((taxRateOne != null) && taxRateOne.Country.HasValue) || ((taxRateTwo != null) && taxRateTwo.Country.HasValue))
                        {
                            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.TaxRate_Define.ApplyToSelectedCountry_Set(true);

                            if ((taxRateOne != null) && taxRateOne.Country.HasValue)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.TaxRate_Define.ApplyToCountry(taxRateOne.Country.Value).Set(true);
                            }

                            if ((taxRateTwo != null) && taxRateTwo.Country.HasValue)
                            {
                                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.TaxRate_Define.ApplyToCountry(taxRateTwo.Country.Value).Set(true);
                            }
                        }

                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.TaxRate_Define.SaveAndClose_Click();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandiseDefine.SelectByName();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
