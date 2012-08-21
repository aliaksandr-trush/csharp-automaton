namespace RegOnline.RegressionTest.DataCollection
{
    using System.Collections.Generic;

    public class Merchandise
    {
        public int Id;
        public FormData.MerchandiseType MerchandiseType;
        public double? MerchandiseFee;
        public double? MinMerchandiseFee;
        public double? MaxMerchandiseFee;
        public string MerchandiseName;
        public bool? ApplyTaxOne;
        public bool? ApplyTaxTwo;
        public List<DiscountCode> DiscountCodes = new List<DiscountCode>();

        public Merchandise(string merchandiseName)
        {
            this.MerchandiseName = merchandiseName;
        }
    }
}
