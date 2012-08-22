namespace RegOnline.RegressionTest.DataCollection
{
    using System;

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

        public double CalculateDiscountedPrice(double originalPrice)
        {
            if (this.CodeType == FormData.DiscountCodeType.AccessCode)
            {
                throw new InvalidOperationException(string.Format("Cannot calculate discounted price for access code: {0}", this.Code));
            }
            else
            {
                double discountAmount = this.Amount;

                if (this.CodeDirection.Value == FormData.ChangePriceDirection.Decrease)
                {
                    discountAmount = -discountAmount;
                }

                double discountedPrice = originalPrice;

                switch (CodeKind)
                {
                    case FormData.ChangeType.FixedAmount:
                        discountedPrice += discountAmount;
                        break;

                    case FormData.ChangeType.Percent:
                        discountedPrice = discountedPrice * (100 + discountAmount) / 100;
                        break;

                    default:
                        break;
                }

                return discountedPrice;
            }
        }
    }
}
