namespace RegOnline.RegressionTest.DataCollection
{
    public class TaxRate
    {
        public string TaxRateCaption;
        public double Rate;
        public bool? Apply;
        public FormData.Countries? Country;

        public TaxRate(string caption)
        {
            this.TaxRateCaption = caption;
        }
    }
}
