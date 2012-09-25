namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;

    public class RegType
    {
        public int Id;
        public string Name;
        public string FeeName;
        public double? Price;
        public List<CustomFieldCode> AllCustomCodes = new List<CustomFieldCode>();
        public bool? RequireDC;
        public EarlyPrice EarlyPrice;
        public LatePrice LatePrice;
        public bool? ApplyTaxOne;
        public bool? ApplyTaxTwo;
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
            this.Name = regTypeName;
        }
    }

    public class RegTypeLimit
    {
        public FormData.RegLimitType LimitType;
        public int LimitTo;
        public string SoldOutMessage;
    }
}
