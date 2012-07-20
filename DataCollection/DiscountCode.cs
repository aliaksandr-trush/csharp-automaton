namespace RegOnline.RegressionTest.DataCollection
{
    public class DiscountCode
    {
        public int Id;
        public FormData.DiscountCodeType CodeType;
        public FormData.ChangePriceDirection? CodeDirection;
        public string Code;
        public double Amount;
        public FormData.ChangeType CodeKind;
        public int? Limit;

        public DiscountCode(string code)
        {
            this.Code = code;
        }
    }
}
