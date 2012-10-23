namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;

    public class CustomFieldCode
    {
        public int Id;
        public EventData_Common.CustomFieldCodeType CodeType;
        public EventData_Common.ChangePriceDirection? CodeDirection;
        public string CodeString;
        public double Amount;
        public EventData_Common.ChangeType CodeKind;
        public int? Limit;

        public CustomFieldCode(string code)
        {
            this.CodeString = code;
        }

        public double CalculateDiscountedPrice(double originalPrice)
        {
            if (this.CodeType == EventData_Common.CustomFieldCodeType.AccessCode)
            {
                return originalPrice;
            }
            else
            {
                double discountAmount = 0;

                if (this.Amount >= 0)
                {
                    discountAmount = this.Amount;
                }

                if (this.CodeDirection.Value == EventData_Common.ChangePriceDirection.Decrease)
                {
                    discountAmount = -discountAmount;
                }

                double discountedPrice = originalPrice;

                switch (CodeKind)
                {
                    case EventData_Common.ChangeType.FixedAmount:
                        discountedPrice += discountAmount;
                        break;

                    case EventData_Common.ChangeType.Percent:
                        discountedPrice = discountedPrice * (100 + discountAmount) / 100.0;
                        break;

                    default:
                        break;
                }

                return discountedPrice;
            }
        }

        public static string GenerateBulkCodes(List<CustomFieldCode> codes)
        {
            string discountCodeString = string.Empty;

            for (int i = 0; i < codes.Count; i++)
            {
                discountCodeString += codes[i].CodeString;

                if (codes[i].CodeType != EventData_Common.CustomFieldCodeType.AccessCode)
                {
                    discountCodeString += "=";

                    if (codes[i].CodeDirection == EventData_Common.ChangePriceDirection.Decrease)
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

                    if (codes[i].CodeKind == EventData_Common.ChangeType.Percent)
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
