namespace RegOnline.RegressionTest.Managers.Builder
{
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public partial class FormDetailManager : ManagerBase
    {
        public const string AddNewHotelLink = "//a[text()='Add New Hotel']";
        private const string EditHotelLocator = "//*[text()='{0}']";
        private const string LTPageAddLodgingCFLinkLocatorPrefix = "ctl00_cph_grdLdgCustomFields_";
        private const string LTPageAddTravelCFLinkLocatorPrefix = "ctl00_cph_grdTrvCustomFields_";
        private const string LTPageAddPreferenceCFLinkLocatorPrefix = "ctl00_cph_grdCFPreference_";
        private const string HotelStandardFieldLink = "//input[contains(@id,'{0}{1}')]";
        private const string DontPreferLodgingLocator = "ctl00_cph_lsf_radDontCollectLodging";
        private const string PreferLodgingLocator = "ctl00_cph_lsf_radCollectLodging";

        public enum CommonHotel
        {
            [StringValue("Boulder Marriott")]
            BoulderMarriott,

            [StringValue("Hotel Boulderado")]
            HotelBoulderado
        }

        public enum HotelStandardFields
        {
            [StringValue("RoomType")]
            RoomType,

            [StringValue("BedType")]
            BedType,

            [StringValue("SmokingPreference")]
            SmokingPreference,

            [StringValue("SharingWith")]
            SharingWith,

            [StringValue("AdjoiningWith")]
            AdjoiningWith,

            [StringValue("CheckInOutDate")]
            CheckInOutDate,

            [StringValue("AdditionalInfo")]
            AdditionalInfo
        }

        public enum PreferLodging
        {
            Yes,
            No
        }

        private HotelManager _hotelMgr;
        public HotelManager HotelMgr
        {
            get
            {
                return this._hotelMgr;
            }
            private set
            {
                this._hotelMgr = value;
            }
        }

        private LodgingStandardFieldsManager _lodgingStandardFieldsMgr;
        public LodgingStandardFieldsManager LodgingStandardFieldsMgr
        {
            get
            {
                return this._lodgingStandardFieldsMgr;
            }
            private set
            {
                this._lodgingStandardFieldsMgr = value;
            }
        }

        private LodgingSettingsAndPaymentOptionsManager _lodgingSettingsAndPaymentOptionsMgr;
        public LodgingSettingsAndPaymentOptionsManager LodgingSettingsAndPaymentOptionsMgr
        {
            get
            {
                return this._lodgingSettingsAndPaymentOptionsMgr;
            }
            private set
            {
                this._lodgingSettingsAndPaymentOptionsMgr = value;
            }
        }

        private TravelStandardAdditionalFieldsManager _travelStandardAdditionalFieldsMgr;
        public TravelStandardAdditionalFieldsManager TravelStandardAdditionalFieldsMgr
        {
            get
            {
                return this._travelStandardAdditionalFieldsMgr;
            }
            private set
            {
                this._travelStandardAdditionalFieldsMgr = value;
            }
        }

        private TravelSettingsAndPaymentOptionsManager _travelSettingsAndPaymentOptionsMgr;
        public TravelSettingsAndPaymentOptionsManager TravelSettingsAndPaymentOptionsMgr
        {
            get
            {
                return this._travelSettingsAndPaymentOptionsMgr;
            }
            private set
            {
                this._travelSettingsAndPaymentOptionsMgr = value;
            }
        }

        [Step]
        public void EnterEventLodgingTravelPage()
        {
            // TO DO:  read values from external source?

            // Select all standard lodging fields
            this.AddHotel(StringEnum.GetStringValue(CommonHotel.BoulderMarriott));
            this.AddHotel(StringEnum.GetStringValue(CommonHotel.HotelBoulderado));
            this._lodgingSettingsAndPaymentOptionsMgr.SetAllowAlternativeHotel(true);
            this._lodgingSettingsAndPaymentOptionsMgr.SetShowHotelOnStartPage(true);
            this._lodgingSettingsAndPaymentOptionsMgr.SetAssignRoomToRegistrant(true);
            this._lodgingStandardFieldsMgr.SetRoomType(true, false);
            this._lodgingStandardFieldsMgr.SetBedType(true, false);
            this._lodgingStandardFieldsMgr.SetSmokingPreference(true, false);
            this._lodgingStandardFieldsMgr.SetSharingWith(true, false);
            this._lodgingStandardFieldsMgr.SetAdjoiningWith(true, false);
            this._lodgingStandardFieldsMgr.SetCheckInOutDate(true, false);
            this._lodgingStandardFieldsMgr.SetValidDateRangeForCheckInOutDefault();
            this._lodgingSettingsAndPaymentOptionsMgr.ChoosePaymentOption(LodgingSettingsAndPaymentOptionsManager.PaymentOption.CollectCCInfo);
            this._lodgingStandardFieldsMgr.SetAdditionalInfo(true, false);

            // Select all standard travel fields
            this._travelStandardAdditionalFieldsMgr.SetAirline(true, false, true, false);
            this._travelStandardAdditionalFieldsMgr.SetFlightNumber(true, false, true, false);
            this._travelStandardAdditionalFieldsMgr.SetAirport(true, false, true, false);
            this._travelStandardAdditionalFieldsMgr.SetCity(true, false, true, false);
            this._travelStandardAdditionalFieldsMgr.SetDateTime(true, false, true, false);
            this._travelStandardAdditionalFieldsMgr.SetConnectionInfo(true, false, true, false);
            this._travelSettingsAndPaymentOptionsMgr.SetDateRangeForArrivalAndDepartureDefault();
            this._travelSettingsAndPaymentOptionsMgr.SetPaymentOption(TravelSettingsAndPaymentOptionsManager.PaymentOption.CollectCCInfo);
            this._travelStandardAdditionalFieldsMgr.SetGroundTransportPreference(true, false);
            this._travelStandardAdditionalFieldsMgr.SetAdditionalInfo(true, false);
        }

        [Verify]
        public void VerifyEventLodgingTravelPage()
        {
            ////ReloadEvent();

            this.VerifyEventHasHotel(StringEnum.GetStringValue(CommonHotel.BoulderMarriott));
            this.VerifyEventHasHotel(StringEnum.GetStringValue(CommonHotel.HotelBoulderado));

            ClientDataContext db = new ClientDataContext();

            Event = (from e in db.Events where e.Id == EventId select e).Single(); 

            // assert that selected fields are visible/required in DB
            Assert.That(Event.Capture_Lodging);
            Assert.That(Event.EventCollectionField.cLdgLocation1 ?? false);
            Assert.That(Event.EventCollectionField.cLdgLocation2 ?? false);
            Assert.That(Event.ShowLocationsOnStart ?? false);
            Assert.That(Event.AssignSeparateRoom);
            Assert.That(Event.EventCollectionField.cLdgRoomType ?? false);
            Assert.That(Event.EventCollectionField.cLdgBedType ?? false);
            Assert.That(Event.EventCollectionField.cLdgSmoking ?? false);
            Assert.That(Event.EventCollectionField.cLdgSharing ?? false);
            Assert.That(Event.EventCollectionField.cLdgAdjoining ?? false);
            Assert.That(Event.EventCollectionField.cLdgArrDate ?? false);
            Assert.That(Event.LdgDateValidFrom.Value.Equals(LodgingStandardFieldsManager.DefaultCheckInOutDateFrom));
            Assert.That(Event.LdgDateValidTo.Value.Equals(LodgingStandardFieldsManager.DefaultCheckInOutDateTo));
            Assert.That(Event.EventCollectionField.cLdgAdditionalInfo ?? false);
            Assert.That(Event.EventCollectionField.cLdgccInfo ?? false);
            Assert.That(Event.Capture_Travel);      
            Assert.That(Event.EventCollectionField.cTrvArrAirline ?? false);
            Assert.That(Event.EventCollectionField.cTrvArrFlightNumber ?? false);
            Assert.That(Event.EventCollectionField.cTrvArrAirport ?? false);
            Assert.That(Event.EventCollectionField.cTrvArrAirportCity ?? false);
            Assert.That(Event.EventCollectionField.cTrvArrDate ?? false);
            Assert.That(Event.EventCollectionField.cTrvArrConnectInfo ?? false);
            Assert.That(Event.EventCollectionField.cTrvDptAirline ?? false);
            Assert.That(Event.EventCollectionField.cTrvDptFlightNumber ?? false);
            Assert.That(Event.EventCollectionField.cTrvDptAirport ?? false);
            Assert.That(Event.EventCollectionField.cTrvDptAirportCity ?? false);
            Assert.That(Event.EventCollectionField.cTrvDptDate ?? false);
            Assert.That(Event.EventCollectionField.cTrvDptConnectInfo ?? false);
            Assert.That(Event.TrvDateValidFrom.Value.Equals(TravelSettingsAndPaymentOptionsManager.DefaultArrivalDate));
            Assert.That(Event.TrvDateValidTo.Value.Equals(TravelSettingsAndPaymentOptionsManager.DefaultDepartureDate));
            Assert.That(Event.EventCollectionField.cTrvccinfo ?? false);
            Assert.That(Event.EventCollectionField.cTrvGroundTrans ?? false);
            Assert.That(Event.EventCollectionField.cTrvAdditionalInfo ?? false);
        }


        #region Lodging
        public void VerifyEventHasHotel(string hotelName)
        {
            if (!this.EventHasHotel(hotelName))
            {
                Assert.Fail("Current event has no such hotel '{0}'", hotelName);
            }
        }

        public bool EventHasHotel(int locationID)
        {
            EventLocation location = null;

            ClientDataContext db = new ClientDataContext();
            location = (from l in db.EventLocations where l.LocationId == locationID && l.Event == Event select l).Single();

            return location != null;
        }

        public bool EventHasHotel(string hotelName)
        {
            EventLocation location = null;

            ClientDataContext db = new ClientDataContext();
                location = (from el in db.EventLocations where el.Location.Loc_Name == hotelName &&
                            el.Event == Event select el).Single();

            return location != null;
        }

        [Step]
        public void ClickAddHotel()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddNewHotelLink, LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(HotelManager.FrameID);
        }

        public void ClickEditHotel(string hotelName)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(EditHotelLocator, hotelName), LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(HotelManager.FrameID);
        }

        public void ClickAddLodgingCustomField()
        {
            this.ClickAddCustomField(CustomFieldManager.CustomFieldLocation.LT_Lodging);
        }

        [Step]
        public void AddLodgingCustomField(CustomFieldManager.CustomFieldType type, string name)
        {
            this.AddCustomField(CustomFieldManager.CustomFieldLocation.LT_Lodging, type, name);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Verify]
        public void VerifyLodgingCustomFieldInDatabase(CustomFieldManager.CustomFieldType type, string name)
        {
            this.VerifyCustomFieldInDatabase(type, name, 2);
        }

        public void AddHotel(string hotel)
        {
            int eventId = this.GetEventId();
            this.ClickAddHotel();
            this.HotelMgr.SelectHotelTemplate(hotel);
            this.HotelMgr.SetDisable(false);
            this.HotelMgr.SaveAndClose();
            this.SaveAndStay();
        }

        public void SetHotelStandardFieldsVisibilityAndRequired(HotelStandardFields hsf, bool? visible, bool? required)
        {
            if (visible.HasValue)
            {
                UIUtilityProvider.UIHelper.SetCheckbox(string.Format(HotelStandardFieldLink, hsf.ToString(), "V"), visible.Value, LocateBy.XPath);
            }

            if (required.HasValue)
            {
                UIUtilityProvider.UIHelper.SetCheckbox(string.Format(HotelStandardFieldLink, hsf.ToString(), "R"), required.Value, LocateBy.XPath);
            }
        }
        #endregion


        #region Travel
        public void ClickAddTravelCustomField()
        {
            this.ClickAddCustomField(CustomFieldManager.CustomFieldLocation.LT_Travel);
        }

        [Step]
        public void AddTravelCustomField(CustomFieldManager.CustomFieldType type, string name)
        {
            AddCustomField(CustomFieldManager.CustomFieldLocation.LT_Travel, type, name);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Verify]
        public void VerifyTravelCustomFieldInDatabase(CustomFieldManager.CustomFieldType type, string name)
        {
            VerifyCustomFieldInDatabase(type, name, 3);
        }
        #endregion


        #region Preferences
        public void ClickAddPreferencesCustomField()
        {
            this.ClickAddCustomField(CustomFieldManager.CustomFieldLocation.LT_Preferences);
        }

        public void AddPreferencesCustomField(CustomFieldManager.CustomFieldType type, string name)
        {
            this.AddCustomField(CustomFieldManager.CustomFieldLocation.LT_Preferences, type, name);
        }

        public void VerifyPreferencesCustomFieldInDatabase(CustomFieldManager.CustomFieldType type, string name)
        {
            VerifyCustomFieldInDatabase(type, name, 4);
        }

        #endregion
    }
}