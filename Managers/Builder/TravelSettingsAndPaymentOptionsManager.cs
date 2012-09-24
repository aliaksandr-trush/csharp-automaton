namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public class TravelSettingsAndPaymentOptionsManager
    {
        internal static readonly DateTime DefaultArrivalDate = ManagerBase.DefaultEventStartDate.AddDays(-2);
        internal static readonly DateTime DefaultDepartureDate = ManagerBase.DefaultEventEndDate.AddDays(2);

        public enum PaymentOption
        {
            DoNotChargeOrCollect,
            CollectCCInfo
        }

        public void SetDateRangeForArrivalAndDeparture(DateTime arrival, DateTime departure)
        {
            UIUtil.DefaultProvider.SetDateTimeById("ctl00_cph_dtpTravelDepartureDate", arrival);
            UIUtil.DefaultProvider.SetDateTimeById("ctl00_cph_dtpTravelReturnDate", departure);
            UIUtil.DefaultProvider.WaitForDisplayAndClick("divTravelPurposeText", LocateBy.Id);
        }

        public void SetDateRangeForArrivalAndDepartureDefault()
        {
            this.SetDateRangeForArrivalAndDeparture(DefaultArrivalDate, DefaultDepartureDate);
        }

        public void SetPaymentOption(PaymentOption option)
        {
            switch (option)
            {
                case PaymentOption.DoNotChargeOrCollect:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cph_radDoNotChargeTravel", LocateBy.Id);
                    break;
                case PaymentOption.CollectCCInfo:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cph_radCollectTravel", LocateBy.Id);
                    break;
                default:
                    throw new InvalidOperationException("No such payment option!");
            }
        }
    }
}