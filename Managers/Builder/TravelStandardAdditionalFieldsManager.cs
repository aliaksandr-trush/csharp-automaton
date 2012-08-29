namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class TravelStandardAdditionalFieldsManager
    {
        public enum TravelStandardAdditionalFields
        {
            [StringValue("Airline")]
            Airline,

            [StringValue("Flight Number")]
            FlightNumber,

            [StringValue("Airport")]
            Airport,

            [StringValue("City")]
            City,

            [StringValue("Date")]
            DateTime,

            [StringValue("Connection Info")]
            ConnectionInfo,

            [StringValue("Ground Transportation Preference")]
            GroundTransportationPreference,

            [StringValue("Additional Info")]
            AdditionalInfo
        }

        public enum FieldTypes
        {
            Arrival,
            Departure
        }

        public enum TravelInfo
        {
            PurposeArrive,
            PurposeBooking
        }

        public void SetAirline(bool arrivalVisible, bool arrivalRequired, bool departureVisible, bool departureRequired)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkAirlineDepartV", arrivalVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkAirlineDepartR", arrivalRequired, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkAirlineReturnV", departureVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkAirlineReturnR", departureRequired, LocateBy.Id);
        }

        public void SetFlightNumber(bool arrivalVisible, bool arrivalRequired, bool departureVisible, bool departureRequired)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkFlightNumberDepartV", arrivalVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkFlightNumberDepartR", arrivalRequired, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkFlightNumberReturnV", departureVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkFlightNumberReturnR", departureRequired, LocateBy.Id);
        }

        public void SetAirport(bool arrivalVisible, bool arrivalRequired, bool departureVisible, bool departureRequired)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkAirportDepartV", arrivalVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkAirportDepartR", arrivalRequired, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkAirportReturnV", departureVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkAirportReturnR", departureRequired, LocateBy.Id);
        }

        public void SetCity(bool arrivalVisible, bool arrivalRequired, bool departureVisible, bool departureRequired)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkCityDepartV", arrivalVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkCityDepartR", arrivalRequired, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkCityReturnV", departureVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkCityReturnR", departureRequired, LocateBy.Id);
        }

        public void SetDateTime(bool arrivalVisible, bool arrivalRequired, bool departureVisible, bool departureRequired)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkDateAndTimeDepartV", arrivalVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkDateAndTimeDepartR", arrivalRequired, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkDateAndTimeReturnV", departureVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkDateAndTimeReturnR", departureRequired, LocateBy.Id);
        }

        public void SetConnectionInfo(bool arrivalVisible, bool arrivalRequired, bool departureVisible, bool departureRequired)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkConnectionInfoDepartV", arrivalVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkConnectionInfoDepartR", arrivalRequired, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkConnectionInfoReturnV", departureVisible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkConnectionInfoReturnR", departureRequired, LocateBy.Id);
        }

        public void SetGroundTransportPreference(bool visible, bool required)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkGroundTransportationV", visible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkGroundTransportationR", required, LocateBy.Id);
        }

        public void SetAdditionalInfo(bool visible, bool required)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkAdditionalInfoTravelV", visible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkAdditionalInfoTravelR", required, LocateBy.Id);
        }

        public void SetFrequentFlyerNumber(bool visible, bool required)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkFrequentFlyerNumberV", visible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkFrequentFlyerNumberR", required, LocateBy.Id); 
        }

        public void SetSeatingPreference(bool visible, bool required)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkSeatingPreferenceV", visible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkSeatingPreferenceR", required, LocateBy.Id);
        }

        public void SetPassportNumber(bool visible, bool required)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkPassportNumberV", visible, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkPassportNumberR", required, LocateBy.Id);
        }

        public void ChoicePurposeForCollectingTravelInfo(TravelInfo travelinfo)
        {
            switch (travelinfo)
            {
                case TravelInfo.PurposeArrive:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cph_radPurposeArrive", LocateBy.Id);
                    break;

                case TravelInfo.PurposeBooking:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cph_radPurposeBooking", LocateBy.Id);
                    break;
            }
        }
    }
}