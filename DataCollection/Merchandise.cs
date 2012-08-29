namespace RegOnline.RegressionTest.DataCollection
{
    using System.Collections.Generic;

    public class MerchandiseItem
    {
        public int Id;
        public FormData.MerchandiseType Type;
        public double? Price;
        public double? MinPrice;
        public double? MaxPrice;
        public string Name;
        public bool? ApplyTaxOne;
        public bool? ApplyTaxTwo;
        public List<DiscountCode> DiscountCodes = new List<DiscountCode>();

        public MerchandiseItem(string merchandiseName)
        {
            this.Name = merchandiseName;
        }
    }
}
