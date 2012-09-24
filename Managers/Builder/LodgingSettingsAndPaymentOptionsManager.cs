namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public class LodgingSettingsAndPaymentOptionsManager
    {
        public enum PaymentOption
        {
            DoNotChargeOrCollect,
            ChargeForLodging,
            CollectCCInfo
        }

        public void SetAllowAlternativeHotel(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkAlternateHotel", check, LocateBy.Id);
        }

        public void SetShowHotelOnStartPage(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkShowOnStartPage", check, LocateBy.Id);
        }

        public void SetAssignRoomToRegistrant(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkSeparateRooms", check, LocateBy.Id);
        }

        public void SetHotelRequired(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkRequireHotel", check, LocateBy.Id);
        }

        public void CollectLodgingInfoForRegType(string regType, bool collect)
        {
            string regTypeLocator = "//span/input[following-sibling::*[contains(text(),'{0}')]][contains(@name,'Lodging')]";
            WebDriverUtility.DefaultProvider.SetCheckbox(string.Format(regTypeLocator, regType), collect, LocateBy.XPath);
        }

        [Step]
        public void ChoosePaymentOption(PaymentOption option)
        {
            switch (option)
            {
                case PaymentOption.DoNotChargeOrCollect:
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cph_radDoNotCharge", LocateBy.Id);
                    break;
                case PaymentOption.ChargeForLodging:
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cph_radCharge", LocateBy.Id);
                    break;
                case PaymentOption.CollectCCInfo:
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cph_radCollect", LocateBy.Id);
                    break;
                default:
                    throw new InvalidOperationException("No such payment option!");
            }
        }

        public void SetHotelBookingFee(double fee)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cph_txtBookingFee_text", fee, LocateBy.Id);
        }
    }
}