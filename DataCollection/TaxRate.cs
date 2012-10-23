namespace RegOnline.RegressionTest.DataCollection
{
    public class TaxRate
    {
        public string TaxRateCaption;
        public double Rate;
        public EventData_Common.Country? Country;
        public bool ToBeAdded = true;

        public TaxRate(string caption)
        {
            this.TaxRateCaption = caption;
        }
    }
}
