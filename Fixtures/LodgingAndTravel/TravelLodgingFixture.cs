namespace RegOnline.RegressionTest.Fixtures.LodgingAndTravel
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class TravelLodgingFixture : FixtureBase
    {
        private const string EventName_StandardField = "LTStandardFieldsVisibility";
        private const string EventName_LodgingCF = "LodgingCustomFieldsVisibility";
        private const string EventName_TravelCF = "TravelCustomFieldsVisibility";
        private const string GroupName = "Group";
        private const string DetailsMessage = "This message is very very detail!";
        private const int Capacity = 1;
        private const string EventHotelName = "AddHotel";
        private const string HotelTemplateName1 = "different name";
        private const string HotelTemplateName2 = "Hotel Boulderado";
        private const string LodgingVisibleToRegType = "All";
        private const string DoNotNeedHotel = "I do not need accommodations";
        private const string AlternateHotel = "Lodging (Hotel) Preference (Secondary)";
        private const string MapAndDirection = "Map and Directions";
        private const double BookingFee = 50;

        private enum CFTypes
        {
            Lodging,
            Travel
        }

        private enum CFs
        {
            [StringValue("{0}CF-Checkbox")]
            CFCheckbox,

            [StringValue("{0}CF-Radio")]
            CFRadio,

            [StringValue("{0}CF-DropDown")]
            CFDropDown,

            [StringValue("{0}CF-Numeric")]
            CFNumeric,

            [StringValue("{0}CF-Text")]
            CFText,

            [StringValue("{0}CF-Time")]
            CFTime,

            [StringValue("{0}CF-Header")]
            CFHeader,

            [StringValue("{0}CF-Always")]
            CFAlways,

            [StringValue("{0}CF-Continue")]
            CFContinue,

            [StringValue("{0}CF-Paragraph")]
            CFParagraph,

            [StringValue("{0}CF-Date")]
            CFDate,

            [StringValue("{0}CF-File")]
            CFFile,

            [StringValue("{0}CF-AdminOnly")]
            CFAdminOnly,

            [StringValue("{0}CF-Capacity")]
            CFCapacity,

            [StringValue("{0}CF-Required")]
            CFRequired,

            [StringValue("{0}CF-ConditionalDisplay")]
            CFConditionalDisplay,

            [StringValue("{0}CF-Position")]
            CFPosition,

            [StringValue("{0}CF-Separator")]
            CFSeparator,

            [StringValue("{0}CF-ShowStarting")]
            CFShowStarting,

            [StringValue("{0}CF-HideStarting")]
            CFHideStarting,

            [StringValue("{0}CF-WithDetails")]
            CFWithDetails,

            [StringValue("{0}CF-DetailUrl")]
            CFDetailUrl,

            [StringValue("{0}CF-Group")]
            CFGroup
        }

        private enum RoomTypes
        {
            [StringValue("King")]
            King = 298,

            [StringValue("Queen")]
            Queen = 268,

            [StringValue("Jack")]
            Jack = 238
        }

        private enum NewHotel
        {
            [StringValue("DolphinHotel")]
            Name,

            [StringValue("www.baidu.com")]
            Website,

            [StringValue("United States")]
            Country,

            [StringValue("Fuxing Road No.1")]
            Address1,

            [StringValue("hotel@regonline.com")]
            Email,

            [StringValue("Moziqiao")]
            Address2,

            [StringValue("8888888888")]
            Phone,

            [StringValue("Chengdu")]
            City,

            [StringValue("1234567")]
            Fax,

            [StringValue("SC")]
            State,

            TaxRate = 50,

            [StringValue("12345")]
            ZipCode
        }

        private string eventSessionId;
        private int eventId;
        private int numberOfRequiredFields = 0;
        private int positionFieldId;
        private int separatorLineFieldId;
        private string roomBlockDate = string.Format("{0}/{1}/{2}", DateTime.Today.Month, DateTime.Today.Day, DateTime.Today.Year);
        private int blockSize = 60;
        private string DetailsURL = ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrlWithHttps;

        [Test]
        [Category(Priority.Three)]
        [Description("437")]
        public void LTStandardFieldVisibility()
        {
            this.LoginAndGoToLTPage(EventName_StandardField);
            this.SetLTStandardFieldsVisibility();
            this.RegisterAndCheckVisibility();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("438")]
        public void LodgingCFVisibility()
        {
            this.CreateLodgingCFs();
            this.RegisterAndCheckCFVisibility(CFTypes.Lodging);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1326")]
        public void TravelCFVisibility()
        {
            this.CreateTravelCFs();
            this.RegisterAndCheckCFVisibility(CFTypes.Travel);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("439")]
        public void AddNewHotel()
        {
            this.LoginAndGoToLTPage(EventHotelName);
            this.CreateAndRegisterNewHotel();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("440")]
        public void HotelOptions()
        {
            this.LoginAndGoToLTPage(EventHotelName);
            this.SetOptions();
            this.RegisterAndCheckHotel();
            this.UpdateHotelSettingAndCheckAgain();
        }

        private void SetOptions()
        {
            BuilderMgr.ClickAddHotel();
            BuilderMgr.HotelMgr.SelectHotelTemplate(HotelTemplateName1);
            BuilderMgr.HotelMgr.SaveAndClose();

            BuilderMgr.ClickAddHotel();
            BuilderMgr.HotelMgr.SelectHotelTemplate(HotelTemplateName2);
            BuilderMgr.HotelMgr.SaveAndClose();

            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.CollectLodgingInfoForRegType(LodgingVisibleToRegType, true);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetHotelRequired(true);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetAllowAlternativeHotel(true);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetShowHotelOnStartPage(true);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetAssignRoomToRegistrant(true);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.ChoosePaymentOption(LodgingSettingsAndPaymentOptionsManager.PaymentOption.DoNotChargeOrCollect);
            BuilderMgr.SaveAndStay();
        }

        private void CreateAndRegisterNewHotel()
        {
            BuilderMgr.ClickAddHotel();
            BuilderMgr.HotelMgr.ClickHotelTemplateLink();
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypeName(StringEnum.GetStringValue(NewHotel.Name));
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypeWebsite(StringEnum.GetStringValue(NewHotel.Website));
            BuilderMgr.HotelMgr.HotelTemplateMgr.SelectCountry(StringEnum.GetStringValue(NewHotel.Country));
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypeAddress1(StringEnum.GetStringValue(NewHotel.Address1));
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypeEmail(StringEnum.GetStringValue(NewHotel.Email));
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypeAddress2(StringEnum.GetStringValue(NewHotel.Address2));
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypePhone(StringEnum.GetStringValue(NewHotel.Phone));
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypeCity(StringEnum.GetStringValue(NewHotel.City));
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypeFax(StringEnum.GetStringValue(NewHotel.Fax));
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypeStateProvince(StringEnum.GetStringValue(NewHotel.State));
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypeDefaultTaxRate((double)(NewHotel.TaxRate));
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypeZipCode(StringEnum.GetStringValue(NewHotel.ZipCode));
            BuilderMgr.HotelMgr.HotelTemplateMgr.SetShowMap(true);
            BuilderMgr.HotelMgr.HotelTemplateMgr.AddNewRoomType(StringEnum.GetStringValue(RoomTypes.King), (double)(RoomTypes.King));
            BuilderMgr.HotelMgr.HotelTemplateMgr.AddNewRoomType(StringEnum.GetStringValue(RoomTypes.Queen), (double)(RoomTypes.Queen));
            BuilderMgr.HotelMgr.HotelTemplateMgr.AddNewRoomType(StringEnum.GetStringValue(RoomTypes.Jack), (double)(RoomTypes.Jack));
            BuilderMgr.HotelMgr.HotelTemplateMgr.SaveAndClose();
            BuilderMgr.HotelMgr.AddRoomBlock(roomBlockDate, StringEnum.GetStringValue(RoomTypes.King), blockSize);
            BuilderMgr.HotelMgr.SaveAndClose();
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.CollectLodgingInfoForRegType(LodgingVisibleToRegType, true);
            BuilderMgr.SaveAndStay();

            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(NewHotel.Name), true);
            RegisterMgr.ClickHotelDetailInfo(StringEnum.GetStringValue(NewHotel.Name));
            RegisterMgr.SelectHotelDetailInfoFrame();
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(NewHotel.Name), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(NewHotel.Website), true);
            RegisterMgr.VerifyHotelDetailInfo(StringEnum.GetStringValue(NewHotel.Country), true);
            RegisterMgr.VerifyHotelDetailInfo(StringEnum.GetStringValue(NewHotel.Address1), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(NewHotel.Email), true);
            RegisterMgr.VerifyHotelDetailInfo(StringEnum.GetStringValue(NewHotel.Address2), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(NewHotel.Phone), true);
            RegisterMgr.VerifyHotelDetailInfo(StringEnum.GetStringValue(NewHotel.City), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(NewHotel.Fax), true);
            RegisterMgr.VerifyHotelDetailInfo(StringEnum.GetStringValue(NewHotel.State), true);
            RegisterMgr.VerifyCustomFieldPresent(StringEnum.GetStringValue(NewHotel.TaxRate), true);
            RegisterMgr.VerifyHotelDetailInfo(StringEnum.GetStringValue(NewHotel.ZipCode), true);
            RegisterMgr.VerifyCustomFieldPresent(MapAndDirection, true);
            RegisterMgr.VerifyCustomFieldPresent(roomBlockDate, true);
        }

        private void RegisterAndCheckHotel()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();

            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName1, true);
            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName2, true);

            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            RegisterMgr.VerifyCustomFieldPresent(DoNotNeedHotel, false);
            RegisterMgr.VerifyCustomFieldPresent(AlternateHotel, true);
            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName1, true);
            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName2, true);
        }

        private void UpdateHotelSettingAndCheckAgain()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.ClickEditRegistrationForm(EventHotelName);

            BuilderMgr.GotoPage(FormDetailManager.Page.LodgingTravel);

            BuilderMgr.ClickEditHotel(HotelTemplateName1);
            BuilderMgr.HotelMgr.AddRoomBlock(roomBlockDate);
            BuilderMgr.HotelMgr.SaveAndClose();

            BuilderMgr.ClickEditHotel(HotelTemplateName2);
            BuilderMgr.HotelMgr.AddRoomBlock(roomBlockDate);
            BuilderMgr.HotelMgr.SaveAndClose();

            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.CollectLodgingInfoForRegType(LodgingVisibleToRegType, true);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetHotelRequired(false);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetAllowAlternativeHotel(false);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetShowHotelOnStartPage(false);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetAssignRoomToRegistrant(false);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.ChoosePaymentOption(LodgingSettingsAndPaymentOptionsManager.PaymentOption.ChargeForLodging);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetHotelBookingFee(BookingFee);
            BuilderMgr.SaveAndStay();

            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();

            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName1, false);
            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName2, false);

            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.VerifyCustomFieldPresent(DoNotNeedHotel, true);
            RegisterMgr.VerifyCustomFieldPresent(AlternateHotel, false);
            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName1, true);
            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName2, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.CheckInDate, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.CheckOutDate, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.RoomPreference, true);

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.ClickEditRegistrationForm(EventHotelName);

            BuilderMgr.GotoPage(FormDetailManager.Page.LodgingTravel);

            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.CollectLodgingInfoForRegType(LodgingVisibleToRegType, true);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetHotelRequired(false);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetAllowAlternativeHotel(false);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetShowHotelOnStartPage(false);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.SetAssignRoomToRegistrant(false);
            BuilderMgr.LodgingSettingsAndPaymentOptionsMgr.ChoosePaymentOption(LodgingSettingsAndPaymentOptionsManager.PaymentOption.CollectCCInfo);
            BuilderMgr.SaveAndStay();

            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();

            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName1, false);
            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName2, false);

            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.VerifyCustomFieldPresent(DoNotNeedHotel, true);
            RegisterMgr.VerifyCustomFieldPresent(AlternateHotel, false);
            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName1, true);
            RegisterMgr.VerifyCustomFieldPresent(HotelTemplateName2, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.CheckInDate, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.CheckOutDate, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.RoomPreference, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.CreditCardHoder, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.CreditCardNumber, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.ExpirationDate, true);
        }

        private void CreateLodgingCFs()
        {
            this.LoginAndGoToLTPage(EventName_LodgingCF);

            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.CheckBox, string.Format(StringEnum.GetStringValue(CFs.CFCheckbox), CFTypes.Lodging.ToString()));
            /*BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.RadioButton, string.Format(StringEnum.GetStringValue(CFs.CFRadio), CFTypes.Lodging.ToString()));
            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.Dropdown, string.Format(StringEnum.GetStringValue(CFs.CFDropDown), CFTypes.Lodging.ToString()));
            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.Number, string.Format(StringEnum.GetStringValue(CFs.CFNumeric), CFTypes.Lodging.ToString()));
            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.OneLineText, string.Format(StringEnum.GetStringValue(CFs.CFText), CFTypes.Lodging.ToString()));
            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.Time, string.Format(StringEnum.GetStringValue(CFs.CFTime), CFTypes.Lodging.ToString()));
            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.SectionHeader, string.Format(StringEnum.GetStringValue(CFs.CFHeader), CFTypes.Lodging.ToString()));
            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.AlwaysSelected, string.Format(StringEnum.GetStringValue(CFs.CFAlways), CFTypes.Lodging.ToString()));
            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.ContinueButton, string.Format(StringEnum.GetStringValue(CFs.CFContinue), CFTypes.Lodging.ToString()));
            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.Paragraph, string.Format(StringEnum.GetStringValue(CFs.CFParagraph), CFTypes.Lodging.ToString()));
            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.Date, string.Format(StringEnum.GetStringValue(CFs.CFDate), CFTypes.Lodging.ToString()));
            BuilderMgr.AddLodgingCustomField(CustomFieldManager.CustomFieldType.FileUpload, string.Format(StringEnum.GetStringValue(CFs.CFFile), CFTypes.Lodging.ToString()));*/
            
            BuilderMgr.ClickAddLodgingCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFAdminOnly), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Admin);
            BuilderMgr.CFMgr.SaveAndClose();
            
            BuilderMgr.ClickAddLodgingCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFCapacity), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.SetSpacesAvailable(Capacity);
            BuilderMgr.CFMgr.SaveAndClose();
            
            BuilderMgr.ClickAddLodgingCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFRequired), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Required);
            BuilderMgr.CFMgr.SaveAndClose();
            
            BuilderMgr.ClickAddLodgingCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFConditionalDisplay), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.SetConditionalLogic(true, string.Format(StringEnum.GetStringValue(CFs.CFCheckbox), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.SaveAndClose();
            
            BuilderMgr.ClickAddLodgingCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFPosition), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.SelectFieldPosition(CFManagerBase.FieldPosition.BelowName);
            BuilderMgr.CFMgr.SaveAndClose();
            this.positionFieldId = BuilderMgr.GetCustomFieldID(string.Format(StringEnum.GetStringValue(CFs.CFPosition), CFTypes.Lodging.ToString()));
            
            BuilderMgr.ClickAddLodgingCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFSeparator), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.CheckSeparatorLine(true);
            BuilderMgr.CFMgr.SaveAndClose();
            this.separatorLineFieldId = BuilderMgr.GetCustomFieldID(string.Format(StringEnum.GetStringValue(CFs.CFSeparator), CFTypes.Lodging.ToString()));
            
            BuilderMgr.ClickAddLodgingCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFShowStarting), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.SetShowDate(DateTime.Today.AddDays(2));
            BuilderMgr.CFMgr.SaveAndClose();
            
            BuilderMgr.ClickAddLodgingCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFHideStarting), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.SetHideDate(DateTime.Today.AddDays(2));
            BuilderMgr.CFMgr.SaveAndClose();
            
            BuilderMgr.ClickAddLodgingCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFWithDetails), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.AddDetailMessage(DetailsMessage);
            BuilderMgr.CFMgr.SaveAndClose();
            
            BuilderMgr.ClickAddLodgingCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFDetailUrl), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.AddDetailURL(DetailsURL);
            BuilderMgr.CFMgr.SaveAndClose();
            
            BuilderMgr.ClickAddLodgingCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFGroup), CFTypes.Lodging.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.SetGroupName(GroupName);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.SaveAndStay();
        }

        private void CreateTravelCFs()
        {
            this.LoginAndGoToLTPage(EventName_TravelCF);

            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.CheckBox, string.Format(StringEnum.GetStringValue(CFs.CFCheckbox), CFTypes.Travel.ToString()));
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.RadioButton, string.Format(StringEnum.GetStringValue(CFs.CFRadio), CFTypes.Travel.ToString()));
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.Dropdown, string.Format(StringEnum.GetStringValue(CFs.CFDropDown), CFTypes.Travel.ToString()));
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.Number, string.Format(StringEnum.GetStringValue(CFs.CFNumeric), CFTypes.Travel.ToString()));
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.OneLineText, string.Format(StringEnum.GetStringValue(CFs.CFText), CFTypes.Travel.ToString()));
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.Time, string.Format(StringEnum.GetStringValue(CFs.CFTime), CFTypes.Travel.ToString()));
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.SectionHeader, string.Format(StringEnum.GetStringValue(CFs.CFHeader), CFTypes.Travel.ToString()));
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.AlwaysSelected, string.Format(StringEnum.GetStringValue(CFs.CFAlways), CFTypes.Travel.ToString()));
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.ContinueButton, string.Format(StringEnum.GetStringValue(CFs.CFContinue), CFTypes.Travel.ToString()));
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.Paragraph, string.Format(StringEnum.GetStringValue(CFs.CFParagraph), CFTypes.Travel.ToString()));
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.Date, string.Format(StringEnum.GetStringValue(CFs.CFDate), CFTypes.Travel.ToString()));
            BuilderMgr.AddTravelCustomField(CustomFieldManager.CustomFieldType.FileUpload, string.Format(StringEnum.GetStringValue(CFs.CFFile), CFTypes.Travel.ToString()));

            /*BuilderMgr.ClickAddTravelCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFAdminOnly), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Admin);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddTravelCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFCapacity), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.SetSpacesAvailable(Capacity);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddTravelCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFRequired), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Required);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddTravelCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFConditionalDisplay), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.SetConditionalLogic(true, string.Format(StringEnum.GetStringValue(CFs.CFRadio), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddTravelCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFPosition), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.SelectFieldPosition(CFManagerBase.FieldPosition.BelowName);
            BuilderMgr.CFMgr.SaveAndClose();
            this.positionFieldId = BuilderMgr.GetCustomFieldID(string.Format(StringEnum.GetStringValue(CFs.CFPosition), CFTypes.Travel.ToString()));

            BuilderMgr.ClickAddTravelCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFSeparator), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.CheckSeparatorLine(true);
            BuilderMgr.CFMgr.SaveAndClose();
            this.separatorLineFieldId = BuilderMgr.GetCustomFieldID(string.Format(StringEnum.GetStringValue(CFs.CFSeparator), CFTypes.Travel.ToString()));

            BuilderMgr.ClickAddTravelCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFShowStarting), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.SetShowDate(DateTime.Today.AddDays(2));
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddTravelCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFHideStarting), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.SetHideDate(DateTime.Today.AddDays(2));
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddTravelCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFWithDetails), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.AddDetailMessage(DetailsMessage);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddTravelCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFDetailUrl), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.AddDetailURL(DetailsURL);
            BuilderMgr.CFMgr.SaveAndClose();

            BuilderMgr.ClickAddTravelCustomField();
            BuilderMgr.CFMgr.SetName(string.Format(StringEnum.GetStringValue(CFs.CFGroup), CFTypes.Travel.ToString()));
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.SetGroupName(GroupName);
            BuilderMgr.CFMgr.SaveAndClose();*/

            BuilderMgr.SaveAndStay();
        }

        private void RegisterAndCheckCFVisibility(CFTypes cfType)
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            if (cfType == CFTypes.Travel)
            {
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFCheckbox), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFRadio), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFDropDown), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFNumeric), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFText), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFTime), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFHeader), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFAlways), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFContinue), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFParagraph), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFDate), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFFile), cfType.ToString()), true);

                RegisterMgr.SelectCustomFieldRadioButtons(
                    string.Format(StringEnum.GetStringValue(CFs.CFRadio), cfType.ToString()), 
                    "Yes");
            }
            else if (cfType == CFTypes.Lodging)
            {
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFAdminOnly), cfType.ToString()), false);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFCapacity), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFRequired), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFConditionalDisplay), cfType.ToString()), false);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFPosition), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFSeparator), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFShowStarting), cfType.ToString()), false);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFHideStarting), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFWithDetails), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFDetailUrl), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFGroup), cfType.ToString()), true);

                RegisterMgr.VerifyCustomFieldRequired(string.Format(StringEnum.GetStringValue(CFs.CFRequired), cfType.ToString()), true);

                RegisterMgr.SetCustomFieldCheckbox(string.Format(StringEnum.GetStringValue(CFs.CFCheckbox), cfType.ToString()), true);
                RegisterMgr.SelectAgendaItem(string.Format(StringEnum.GetStringValue(CFs.CFRequired), cfType.ToString()));
                RegisterMgr.SelectAgendaItem(string.Format(StringEnum.GetStringValue(CFs.CFCapacity), cfType.ToString()));

                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFConditionalDisplay), cfType.ToString()), true);

                RegisterMgr.VerifyCustomFieldPosition(Managers.Register.RegisterManager.FieldPositions.Below, this.positionFieldId);
                RegisterMgr.VerifySeparatorLineBelowField(this.separatorLineFieldId);
                RegisterMgr.VerifyDetailsMessage(string.Format(StringEnum.GetStringValue(CFs.CFWithDetails), cfType.ToString()), DetailsMessage);
                RegisterMgr.VerifyDetailsURL(string.Format(StringEnum.GetStringValue(CFs.CFDetailUrl), cfType.ToString()), DetailsURL);
                RegisterMgr.VerifyFieldInGroup(string.Format(StringEnum.GetStringValue(CFs.CFGroup), cfType.ToString()), GroupName);
            }

            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();

            RegisterMgr.OpenAdminRegisterPage(this.eventId, this.eventSessionId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfoWithoutPassword();
            RegisterMgr.Continue();

            if (cfType == CFTypes.Lodging)
            {
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFAdminOnly), cfType.ToString()), true);
                RegisterMgr.VerifyCustomFieldPresent(string.Format(StringEnum.GetStringValue(CFs.CFCapacity), cfType.ToString()), false);
            }
       }

        private void LoginAndGoToLTPage(string eventName)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();

            if (ManagerSiteMgr.EventExists(eventName))
            {
                ManagerSiteMgr.DeleteEventByName(eventName);
            }
            
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventName);
            BuilderMgr.GotoPage(FormDetailManager.Page.LodgingTravel);
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.SelectBuilderWindow();
        }

        private void SetLTStandardFieldsVisibility()
        {
            BuilderMgr.LodgingStandardFieldsMgr.SetRoomType(true, true);
            this.numberOfRequiredFields += 1;
            BuilderMgr.LodgingStandardFieldsMgr.SetBedType(true, true);
            this.numberOfRequiredFields += 1;
            BuilderMgr.LodgingStandardFieldsMgr.SetSmokingPreference(true, true);
            this.numberOfRequiredFields += 1;
            BuilderMgr.LodgingStandardFieldsMgr.SetSharingWith(true, false);
            BuilderMgr.LodgingStandardFieldsMgr.SetAdjoiningWith(true, false);
            BuilderMgr.LodgingStandardFieldsMgr.SetCheckInOutDate(true, true);
            this.numberOfRequiredFields += 2;
            BuilderMgr.LodgingStandardFieldsMgr.SetAdditionalInfo(true, false);

            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetAirline(true, true, true, true);
            this.numberOfRequiredFields += 2;
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetFlightNumber(true, true, true, true);
            this.numberOfRequiredFields += 2;
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetAirport(true, true, true, true);
            this.numberOfRequiredFields += 2;
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetCity(true, false, true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetDateTime(true, false, true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetConnectionInfo(true, false, true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetGroundTransportPreference(true, false);
            BuilderMgr.TravelStandardAdditionalFieldsMgr.SetAdditionalInfo(true, false);

            BuilderMgr.SaveAndStay();
        }

        private void RegisterAndCheckVisibility()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.CheckInDate, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.CheckOutDate, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.RoomPreference, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.BedPreference, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.SmokingPreference, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.SharingWith, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.AdjoiningWith, true);
            RegisterMgr.VerifyLodgingStandardFieldsPresent(LodgingStandardFieldsManager.LodgingStandardFields.AdditionalInfo, true);

            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Arrival, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.Airline, true);
            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Arrival, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.FlightNumber, true);
            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Arrival, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.City, true);
            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Arrival, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.Airport, true);
            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Arrival, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.DateTime, true);
            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Arrival, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.ConnectionInfo, true);

            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Departure, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.Airline, true);
            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Departure, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.FlightNumber, true);
            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Departure, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.City, true);
            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Departure, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.Airport, true);
            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Departure, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.DateTime, true);
            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(TravelStandardAdditionalFieldsManager.FieldTypes.Departure, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.ConnectionInfo, true);

            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(null, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.AdditionalInfo, true);
            RegisterMgr.VerifyTravelStandardAdditionalFieldsPresent(null, TravelStandardAdditionalFieldsManager.TravelStandardAdditionalFields.GroundTransportationPreference, true);

            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.Continue();

            string[] errorList = RegisterMgr.GetErrorMessages();
            Assert.True(errorList.Length == numberOfRequiredFields);
        }
    }
}
