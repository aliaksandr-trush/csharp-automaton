namespace RegOnline.RegressionTest.Managers.Register
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Attributes;

    public partial class RegisterManager : ManagerBase
    {
        private const string LodgingTravelPagePathOld = "Registrations/Preferences/LdgTrvPref.asp";
        private const string LodgingTravelPagePathNew = "register/lodgingTravel.aspx";
        private const string ArrivalHeaderLocator = "//h4[text()='Travel Information - Arrival']";
        private const string DepartureHeaderLocator = "//h4[text()='Travel Information - Return']";
        private const string DefaultAirline = "UA";
        private const string DefaultArrivalFlightNumber = "789";
        private const string DefaultDepartureFlightNumber = "987";
        private const string NeedAccommodationsLocator = "ctl00_cph_lsf_radCollectLodging";
        private const string NeedNoAccommodationsLocator = "ctl00_cph_lsf_radDontCollectLodging";
        private const string LodgingCustomFieldLocator = "//h4[text()='Lodging Information - Other']/following-sibling::ol/li";
        private const string TravelCustomFieldLocator = "//h4[text()='Travel Information - Other']/following-sibling::ol/li";
        private const string CheckInDateLocator = "ctl00_cph_lsf_rptLodgingFields_ctl00_sf_Hotel_valStart";
        private const string CheckOutDateLocator = "ctl00_cph_lsf_rptLodgingFields_ctl01_sf_Hotel_valEnd";
        private const string CheckInDateOtherLocator = "ctl00_cph_lsf_rptLodgingFields_ctl00_sf_dpResponse";
        private const string CheckOutDateOtherLocator = "ctl00_cph_lsf_rptLodgingFields_ctl01_sf_dpResponse";
        private const string BedPreferenceLocator = "//*[text()='Bed Preference:']/../../div[2]/select";
        private const string HotelPreferenceLocator = "//label[contains(text(),\"{0}\")]";

        public enum RoomPreference
        {
            [StringValue("1 King w/ sofa bed")]
            OneKing,

            [StringValue("2 Queens w/ sofa bed")]
            TwoQueens,

            [StringValue("King Suite w/ sofa bed")]
            KingSuite,

            [StringValue("2 Doubles Suite w/ sofabed")]
            TwoDoubles,

            [StringValue("Single")]
            Single,

            [StringValue("Double")]
            Double,

            [StringValue("No Preference")]
            NoPreference
        }

        public enum BedPreference
        {
            [StringValue("King")]
            King,

            [StringValue("Queen")]
            Queen,

            [StringValue("Two Doubles")]
            TwoDoubles,

            [StringValue("No Preference")]
            NoPreference
        }

        public enum SmokingPreference
        {
            [StringValue("Smoking")]
            Smoking,

            [StringValue("Non-Smoking")]
            NonSmoking,

            [StringValue("No Preference")]
            NoPreference
        }

        public enum Section
        {
            Lodging,
            Travel
        }

        public enum FieldPositions
        {
            Left,
            Right,
            Below,
            Above
        }

        #region Methods for new page

        public bool OnLodgingTravelPage()
        {
            return UIUtilityProvider.UIHelper.UrlContainsAbsolutePath(LodgingTravelPagePathNew);
        }

        public void VerifyOnLodgingTravelPage()
        {
            //if (!this.OnLodgingTravelPageNew())
            //{
            //    Assert.Fail("Not on L&T page!");
            //}

            UIUtilityProvider.UIHelper.VerifyOnPage(this.OnLodgingTravelPage(), "L&T");
        }

        public List<string> GetLAndTCustomFieldNames(Section section)
        {
            int count;
            List<string> names = new List<string>();
            switch (section)
            {
                case Section.Lodging:
                    count = UIUtilityProvider.UIHelper.GetXPathCountByXPath(LodgingCustomFieldLocator);

                    for (int i = 0; i < count; i++)
                    {
                        names.Add(UIUtilityProvider.UIHelper.GetText(string.Format(LodgingCustomFieldLocator + "[{0}]", i + 1), LocateBy.XPath));
                    }

                    break;

                case Section.Travel:
                    count = UIUtilityProvider.UIHelper.GetXPathCountByXPath(TravelCustomFieldLocator);

                    for (int i = 0; i < count; i++)
                    {
                        names.Add(UIUtilityProvider.UIHelper.GetText(string.Format(TravelCustomFieldLocator + "[{0}]", i + 1), LocateBy.XPath));
                    }

                    break;
            }

            return names;
        }

        #region Lodging
        public void VerifyCustomFieldPosition(FieldPositions fieldPosition, int fieldId)
        {
            switch(fieldPosition)
            {
                case FieldPositions.Below:
                    UIUtilityProvider.UIHelper.VerifyElementDisplay(string.Format("//*[@for='{0}']/following-sibling::*[@id='{0}']", fieldId.ToString()), true, LocateBy.XPath);
                    break;
                case FieldPositions.Above:
                    UIUtilityProvider.UIHelper.VerifyElementDisplay(string.Format("//*[@id='{0}']/following-sibling::*[@for='{0}']", fieldId.ToString()), true, LocateBy.XPath);
                    break;
                case FieldPositions.Left:
                    UIUtilityProvider.UIHelper.VerifyElementDisplay(string.Format("//*[@id='{0}']/../following-sibling::*//*[@for='{0}']", fieldId.ToString()), true, LocateBy.XPath);
                    break;
                case FieldPositions.Right:
                    UIUtilityProvider.UIHelper.VerifyElementDisplay(string.Format("//*[@for='{0}']/../following-sibling::*//*[@id='{0}']", fieldId.ToString()), true, LocateBy.XPath);
                    break;
                default:
                    break;
            }
        }

        public void VerifyHotelDetailInfo(string info, bool present)
        {
            VerifyTool.VerifyValue(present, UIUtilityProvider.UIHelper.GetText("//p/a[@target='_blank']/../following-sibling::*[following-sibling::*[contains(text(),'Phone')]]", LocateBy.XPath).Contains("United States"), "United States present: {0}");
        }

        public void VerifySeparatorLineBelowField(int fieldId)
        {
            UIUtilityProvider.UIHelper.VerifyElementDisplay(string.Format("//*[@for='{0}']/../following-sibling::hr", fieldId.ToString()), true, LocateBy.XPath);
        }

        public void VerifyDetailsMessage(string field, string message)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//*[text()='{0}']/..//*[text()='Details']", field), LocateBy.XPath);
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format("//*[contains(text(),'{0}')]", message), true, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[@class='ui-icon ui-icon-closethick']", LocateBy.XPath);
        }

        public void VerifyDetailsURL(string field, string url)
        {
            string URLLocator = "//*[text()='{0}']/following-sibling::*/a[@href='{1}']";
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format(URLLocator, field, url), true, LocateBy.XPath);
        }

        public void VerifyFieldInGroup(string field, string groupName)
        {
            string groupLocator = "//*[text()='{0}']/following-sibling::*[contains(text(),'{1}')]";
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format(groupLocator, field, groupName.ToUpper()), true, LocateBy.XPath);
        }

        public void ClickHotelDetailInfo(string hotelName)
        {
            string detailInfoLocator = string.Format("//*[contains(text(),'{0}')]/a", hotelName);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(detailInfoLocator, LocateBy.XPath);
        }

        public void SelectHotelDetailInfoFrame()
        {
            UIUtilityProvider.UIHelper.SelectIFrame(0);
        }

        public void EnterLodgingInfo()
        {
            this.VerifyOnLodgingTravelPage();

            this.lodgingResponses.RoomType = StringEnum.GetStringValue(RoomPreference.OneKing);
            this.lodgingResponses.BedType = StringEnum.GetStringValue(BedPreference.King);
            this.lodgingResponses.SmokingPreference = StringEnum.GetStringValue(SmokingPreference.NoPreference);

            this.ClickNeedAccommodations();
            this.SelectRoomPreference(RoomPreference.OneKing);
            this.SelectBedPreference(BedPreference.King);
            this.SelectSmokingPreference(SmokingPreference.NoPreference);
        }

        public void ClickNeedAccommodations()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(NeedAccommodationsLocator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void ClickNeedNoAccommodations()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(NeedNoAccommodationsLocator, LocateBy.Id);
            Utility.ThreadSleep(0.5);
        }

        public void SelectHotel(string hotelName)
        {
            string id = UIUtilityProvider.UIHelper.GetAttribute(string.Format(HotelPreferenceLocator, hotelName), "for", LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(id, LocateBy.Id);
        }

        //If this is failing, look at the method below, there are two different locators depending on setup. 
        public void FillOutCheckInOutDates(string CheckInDate, string CheckOutDate)
        {
            UIUtilityProvider.UIHelper.Type(CheckInDateLocator, CheckInDate, LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(CheckOutDateLocator, CheckOutDate, LocateBy.Id);
            //Clears Calendar pop-up that is blocking it from continuing. 
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//ol[@class='fieldList'][1]/li[1]", LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void FillOutCheckInOutDatesWithOpenValidDates(string CheckInDate, string CheckOutDate)
        {
            UIUtilityProvider.UIHelper.Type(CheckInDateOtherLocator, CheckInDate, LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(CheckOutDateOtherLocator, CheckOutDate, LocateBy.Id);
            //Clears Calendar pop-up that is blocking it from continuing. 
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//ol[@class='fieldList'][1]/li[1]", LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SelectRoomPreference(RoomPreference room)
        {
            SelectRoomPreference(StringEnum.GetStringValue(room));
        }

        public void SelectRoomPreference(string RoomPreference)
        {
            UIUtilityProvider.UIHelper.SelectWithText(
                "//*[text()='Room Preference:']/../../div[2]/select",
                RoomPreference, 
                LocateBy.XPath);
        }

        public void SelectBedPreference(BedPreference bed)
        {
            UIUtilityProvider.UIHelper.SelectWithText(BedPreferenceLocator, StringEnum.GetStringValue(bed), LocateBy.XPath);
        }
        public void VerifyBedPreference(BedPreference bed)
        {
            Utilities.VerifyTool.VerifyValue(
                StringEnum.GetStringValue(bed),
                UIUtilityProvider.UIHelper.GetSelectedOptionFromDropdownByXPath(BedPreferenceLocator),                
                "Bed preference text : {0}");
        }

        public void SelectSmokingPreference(SmokingPreference smoking)
        {
            UIUtilityProvider.UIHelper.SelectWithText(
                "//*[text()='Smoking Preference:']/../../div[2]/select",
                StringEnum.GetStringValue(smoking), 
                LocateBy.XPath);
        }

        public void FillOutSharingWith(string sharingWith)
        {
            UIUtilityProvider.UIHelper.Type("//*[text()='Sharing With:']/../..//input", sharingWith, LocateBy.XPath);
        }

        public void FillOutAdjoiningWith(string adjoiningWith)
        {
            UIUtilityProvider.UIHelper.Type("//*[text()='Adjoining With:']/../..//input", adjoiningWith, LocateBy.XPath);
        }

        public void FillOutLodgingAdditionalInfo(string additionalInfo)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cph_lsf_rptLodgingFields_ctl08_sf_txtResponse", additionalInfo, LocateBy.Id);
        }

        [Step]
        public void FillOutLodgingCCInfo_Default()
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cph_lsf_rptLodgingFields_ctl10_sf_txtResponse", PaymentManager.DefaultPaymentInfo.HolderName, LocateBy.Id);

            UIUtilityProvider.UIHelper.Type("ctl00_cph_lsf_rptLodgingFields_ctl11_sf_txtResponse", PaymentManager.DefaultPaymentInfo.CCNumber, LocateBy.Id);

            UIUtilityProvider.UIHelper.SelectWithText("ctl00_cph_lsf_rptLodgingFields_ctl12_sf_ddlMonth", PaymentManager.DefaultPaymentInfo.ExpirationMonth, LocateBy.Id);

            UIUtilityProvider.UIHelper.SelectWithText("ctl00_cph_lsf_rptLodgingFields_ctl12_sf_ddlYear", PaymentManager.DefaultPaymentInfo.ExpirationYear, LocateBy.Id);
        }

        public void VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields fields, bool present)
        {
            string LodginStandardFieldsLocator = "//*[contains(@id,'LodgingFields')][contains(text(),'{0}')]";

            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format(LodginStandardFieldsLocator, StringEnum.GetStringValue(fields)), present, LocateBy.XPath);
        }

        #endregion

        #region Travel
        public void EnterTravelInfo()
        {
            this.VerifyOnLodgingTravelPage();

            this.travelResponses.ArrivalAirline = DefaultAirline;
            this.travelResponses.ArrivalFlightNumber = DefaultArrivalFlightNumber;
            this.travelResponses.DepartureAirline = DefaultAirline;
            this.travelResponses.DepartureFlightNumber = DefaultDepartureFlightNumber;

            this.EnterArrivalDate();
            this.EnterDepartureDate();
            this.EnterArrivalAirline();
            this.EnterDepartureAirline();
            this.EnterArrivalFlightNumber();
            this.EnterDepartureFlightNumber();
        }

        public void EnterArrivalDate()
        {
            lodgingResponses.ArrivalDate = DefaultEventStartDate;

            UIUtilityProvider.UIHelper.Type("//*[text()='Arrival Date:']/../following-sibling::div//input[1]", lodgingResponses.ArrivalDate.ToString("MM/dd/yyyy"), LocateBy.XPath);
        }

        public void EnterDepartureDate()
        {
            lodgingResponses.DepartureDate = DefaultEventEndDate;

            UIUtilityProvider.UIHelper.Type("//*[text()='Departure Date:']/../following-sibling::div//input[1]", lodgingResponses.DepartureDate.ToString("MM/dd/yyyy"), LocateBy.XPath);
        }

        public void EnterArrivalAirline()
        {
            UIUtilityProvider.UIHelper.Type(ArrivalHeaderLocator + "/following-sibling::ol//*[text()='Airline:']/../../div[2]/input", DefaultAirline, LocateBy.XPath);
        }

        public void EnterDepartureAirline()
        {
            UIUtilityProvider.UIHelper.Type(DepartureHeaderLocator + "/following-sibling::ol//*[text()='Airline:']/../../div[2]/input", DefaultAirline, LocateBy.XPath);
        }

        public void EnterArrivalFlightNumber()
        {
            UIUtilityProvider.UIHelper.Type(ArrivalHeaderLocator + "/following-sibling::ol//*[text()='Flight Number:']/../../div[2]/input", DefaultArrivalFlightNumber, LocateBy.XPath);
        }

        public void EnterDepartureFlightNumber()
        {
            UIUtilityProvider.UIHelper.Type(DepartureHeaderLocator + "/following-sibling::ol//*[text()='Flight Number:']/../../div[2]/input", DefaultDepartureFlightNumber, LocateBy.XPath);
        }

        public void FillOutTravelCCInfo_Default()
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cph_tsf_rptOtherFields_ctl02_sf_txtResponse", PaymentManager.DefaultPaymentInfo.HolderName, LocateBy.Id);

            UIUtilityProvider.UIHelper.Type("ctl00_cph_tsf_rptOtherFields_ctl03_sf_txtResponse", PaymentManager.DefaultPaymentInfo.CCNumber, LocateBy.Id);

            UIUtilityProvider.UIHelper.SelectWithText("ctl00_cph_tsf_rptOtherFields_ctl04_sf_ddlMonth", PaymentManager.DefaultPaymentInfo.ExpirationMonth, LocateBy.Id);

            UIUtilityProvider.UIHelper.SelectWithText("ctl00_cph_tsf_rptOtherFields_ctl04_sf_ddlYear", PaymentManager.DefaultPaymentInfo.ExpirationYear, LocateBy.Id);
        }

        public void FillOutTravelBookingCCInfo_Default()
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cph_tsf_rptOtherFields_ctl05_sf_txtResponse", PaymentManager.DefaultPaymentInfo.HolderName, LocateBy.Id);

            UIUtilityProvider.UIHelper.Type("ctl00_cph_tsf_rptOtherFields_ctl06_sf_txtResponse", PaymentManager.DefaultPaymentInfo.CCNumber, LocateBy.Id);

            UIUtilityProvider.UIHelper.SelectWithText("ctl00_cph_tsf_rptOtherFields_ctl07_sf_ddlMonth", PaymentManager.DefaultPaymentInfo.ExpirationMonth, LocateBy.Id);

            UIUtilityProvider.UIHelper.SelectWithText("ctl00_cph_tsf_rptOtherFields_ctl07_sf_ddlYear", PaymentManager.DefaultPaymentInfo.ExpirationYear, LocateBy.Id);
        }

        public void VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes? type,TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields field, bool present)
        {
            string TravelArrivalLocator = "//*[contains(@id,'ArrivalFields')][contains(text(),'{0}')]";
            string TravelDepartureLocator = "//*[contains(@id,'DepartureFields')][contains(text(),'{0}')]";
            string TravelAdditionalInfoLocator = "//*[contains(@id,'OtherFields')][contains(text(),'{0}')]";
            string TravelGroundTransportLocator = "//*[contains(@id,'GroundTransportation')][contains(text(),'{0}')]";

            switch(field)
            {
                case TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.Airline:
                case TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.Airport:
                case TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.City:
                case TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.ConnectionInfo:
                case TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.DateTime:
                case TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.FlightNumber:
                    if (type == TravelStandardAdditionalFieldsManager.FieldTypes.Arrival)
                    {
                        UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format(TravelArrivalLocator, StringEnum.GetStringValue(field)), present, LocateBy.XPath);
                    }
                    if (type == TravelStandardAdditionalFieldsManager.FieldTypes.Departure)
                    {
                        UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format(TravelDepartureLocator, StringEnum.GetStringValue(field)), present, LocateBy.XPath);
                    }
                    break;
                case TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.AdditionalInfo:
                    UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format(TravelAdditionalInfoLocator, StringEnum.GetStringValue(field)), present, LocateBy.XPath);
                    break;
                case TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.GroundTransportationPreference:
                    UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format(TravelGroundTransportLocator, StringEnum.GetStringValue(field)), present, LocateBy.XPath);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion
    }
}
