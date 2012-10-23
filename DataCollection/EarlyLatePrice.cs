namespace RegOnline.RegressionTest.DataCollection
{
    using System;

    public class EarlyPrice
    {
        public double earlyPrice;
        public EventData_Common.EarlyPriceType EarlyPriceType;
        public DateTime? EarlyPriceDate;
        public DateTime? EarlyPriceTime;
        public int? FirstNRegistrants;
    }

    public class LatePrice
    {
        public double latePrice;
        public DateTime LatePriceDate;
        public DateTime LatePriceTime;
    }
}
