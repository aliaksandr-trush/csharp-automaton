namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;

    public class RegType
    {
        public int RegTypeId;
        public string RegTypeName;
        public string FeeName;
        public double? Price;
        public List<DiscountCode> DiscountCode = new List<DiscountCode>();
        public bool? RequireDC;
        public EarlyPrice EarlyPrice;
        public LatePrice LatePrice;
        public TaxRate TaxRateOne;
        public TaxRate TaxRateTwo;
        public int? MinGroupSize;
        public int? MaxGroupSize;
        public string MinRegistrantMessage;
        public DateTime? ShowStarting;
        public DateTime? HideStarting;
        public bool? IsPublic;
        public bool? IsAdmin;
        public bool? IsOnSite;
        public RegTypeLimit RegTypeLimit;
        public string AdditionalDetails;
        public bool IsSSO;

        public static RegType Default = RegType.GetDefault();

        public static RegType GetDefault()
        {
            return new RegType
            {
                FeeName = "Event_Fee"
            };
        }

        public RegType(){}

        public RegType(string regTypeName)
        {
            this.RegTypeName = regTypeName;
        }
    }

    public class RegTypeLimit
    {
        public FormData.RegLimitType LimitType;
        public int LimitTo;
        public string SoldOutMessage;
    }
}
