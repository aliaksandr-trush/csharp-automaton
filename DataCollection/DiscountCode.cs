namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;

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
                return originalPrice;
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

        public static string GenerateBulkCodes(List<DiscountCode> codes)
        {
            string discountCodeString = string.Empty;

            for (int i = 0; i < codes.Count; i++)
            {
                discountCodeString += codes[i].Code;

                if (codes[i].CodeType != FormData.DiscountCodeType.AccessCode)
                {
                    discountCodeString += "=";

                    if (codes[i].CodeDirection == FormData.ChangePriceDirection.Decrease)
                    {
                        discountCodeString += "-";
                    }

                    if (codes[i].Amount < 0)
                    {
                        discountCodeString += 0.ToString();
                    }
                    else
                    {
                        discountCodeString += codes[i].Amount.ToString();
                    }

                    if (codes[i].CodeKind == FormData.ChangeType.Percent)
                    {
                        discountCodeString += "%";
                    }
                }

                if (codes[i].Limit.HasValue)
                {
                    discountCodeString += "(" + codes[i].Limit.Value.ToString() + ")";
                }

                if (i != codes.Count - 1)
                {
                    discountCodeString += ",";
                }
            }

            return discountCodeString;
        }
    }
}
