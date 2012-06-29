namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;

    public class AddTaxRate
    {
        private EventDetails EventDetails = new EventDetails();
        private Agenda Agenda = new Agenda();

        public void AddTaxRates(TaxRate taxRateOne, TaxRate taxRateTwo, FormData.Location location)
        {
            switch (location)
            {
                case FormData.Location.RegType:
                    EventDetails.RegTypeDefine.EventFeeDefine.AddTaxRate_Click();
                    EventDetails.RegTypeDefine.EventFeeDefine.TaxRateDefine.SelectByName();

                    if (taxRateOne != null)
                    {
                        EventDetails.RegTypeDefine.EventFeeDefine.TaxRateDefine.TaxOneCaption.Type(taxRateOne.TaxRateCaption);
                        EventDetails.RegTypeDefine.EventFeeDefine.TaxRateDefine.TaxOneRate.Type(taxRateOne.Rate);
                    }

                    if (taxRateTwo != null)
                    {
                        EventDetails.RegTypeDefine.EventFeeDefine.TaxRateDefine.TaxTwoCaption.Type(taxRateTwo.TaxRateCaption);
                        EventDetails.RegTypeDefine.EventFeeDefine.TaxRateDefine.TaxTwoRate.Type(taxRateTwo.Rate);
                    }

                    EventDetails.RegTypeDefine.EventFeeDefine.TaxRateDefine.SaveAndClose_Click();
                    break;
                case FormData.Location.Agenda:
                    Agenda.AddTaxRate_Click();
                    Agenda.TaxRateDefine.SelectByName();

                    if (taxRateOne != null)
                    {
                        Agenda.TaxRateDefine.TaxOneCaption.Type(taxRateOne.TaxRateCaption);
                        Agenda.TaxRateDefine.TaxOneRate.Type(taxRateOne.Rate);
                    }

                    if (taxRateTwo != null)
                    {
                        Agenda.TaxRateDefine.TaxTwoCaption.Type(taxRateTwo.TaxRateCaption);
                        Agenda.TaxRateDefine.TaxTwoRate.Type(taxRateTwo.Rate);
                    }

                    Agenda.TaxRateDefine.SaveAndClose_Click();
                    break;
                default:
                    break;
            }
        }
    }
}
