namespace RegOnline.RegressionTest.Fixtures.FormTypes.ProEvent
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class CreateRegistrationFixture : FixtureBase
    {
        #region Constants
        public const string EventWithoutRegType = "CreateRegistrationFixture - WithoutRegTypes";
        public const string EventWithRegType = "CreateRegistrationFixture - WithRegTypes";
        private const int InvalidEventID = FormDetailManager.InvalidId;
        private const int ExpireTime = 30;
        private const double EventFee = 10;

        private const double AgendaOrientationFee = 0;
        private const double AgendaDinnerFee = 90;
        private const double AgendaBasketballOneFee = 20;
        private const double AgendaBasketballTwoFee = 30;
        #endregion

        #region Private fields
        private int eventID;
        private string eventSessionId;
        private string eventName;
        private bool withRegType;
        private Dictionary<RegType, double?> regTypeFees;
        private Dictionary<FrisbeeLevel, double> frisbeeLevelFees;
        private Dictionary<BreakfastCereal, double> breakfastCerealFees;
        private Dictionary<MerchandiseItem, double> merchFees;
        private Dictionary<string, string> cfIDs;
        #endregion

        #region Enum
        private enum RegType
        {
            Student,
            Vendor,
            Speaker,
            VIP
        }

        private enum PICustomFieldName
        {
            [StringValue("Are you a Steelers fan?")]
            AreYouASteelersFan,

            [StringValue("Favorite Breakfast?")]
            FavoriteBreakfast,

            [StringValue("Favorite towns?")]
            FavoriteTowns
        }

        private enum FavoriteBreakfast
        {
            [StringValue("Sugary Cereal")]
            SugaryCereal,

            [StringValue("Eggs & Bacon")]
            EggsAndBacon,

            [StringValue("Deep Fried Grasshoppers")]
            DeepFriedGrasshoppers
        }

        private enum FavoriteTowns
        {
            Longmont,
            Lafayette,
            Louisville,
            Superior
        }

        private enum AgendaItemName
        {
            [StringValue("Orientation")]
            Orientation,

            [StringValue("Ultimate Frisbee level")]
            UltimateFrisbeeLevel,

            [StringValue("Post game dinner with raffle")]
            PostGameDinnerWithRaffle,

            [StringValue("Dinner")]
            Dinner,

            [StringValue("Breakfast cereal")]
            BreakfastCereal,

            [StringValue("Tracking keywords")]
            TrackingKeywords,

            [StringValue("Basketball 101")]
            Basketball101,

            [StringValue("Basketball 102")]
            Basketball102
        }

        private enum FrisbeeLevel
        {
            [StringValue("Never played")]
            NeverPlayed,

            [StringValue("Amateur")]
            Amateur,

            [StringValue("Semi-Pro")]
            SemiPro,

            [StringValue("Pro")]
            Pro
        }

        private enum BreakfastCereal
        {
            [StringValue("Cocoa Puffs")]
            CocoaPuffs,

            [StringValue("Rice Krispies")]
            RiceKrispies,

            [StringValue("Lucky Charms")]
            LuckyCharms
        }

        private enum Hotel
        {
            [StringValue("Hotel Boulderado")]
            HotelBoulderado,

            [StringValue("Boulder Marriott")]
            BoulderMarriott
        }

        private enum MerchandiseItem
        {
            Frisbee,
            Basketball,
            Shoes
        }
        #endregion

        public CreateRegistrationFixture()
            : base()
        {
            // Initialize RegTypes
            this.regTypeFees = new Dictionary<RegType, double?>();
            this.regTypeFees.Add(RegType.Student, 10);
            this.regTypeFees.Add(RegType.Vendor, 50);
            this.regTypeFees.Add(RegType.Speaker, null);
            this.regTypeFees.Add(RegType.VIP, 500);

            // Initialize Agenda
            this.frisbeeLevelFees = new Dictionary<FrisbeeLevel, double>();
            this.frisbeeLevelFees.Add(FrisbeeLevel.NeverPlayed, 0);
            this.frisbeeLevelFees.Add(FrisbeeLevel.Amateur, 5);
            this.frisbeeLevelFees.Add(FrisbeeLevel.SemiPro, 15);
            this.frisbeeLevelFees.Add(FrisbeeLevel.Pro, 20);

            this.breakfastCerealFees = new Dictionary<BreakfastCereal, double>();
            this.breakfastCerealFees.Add(BreakfastCereal.CocoaPuffs, 1);
            this.breakfastCerealFees.Add(BreakfastCereal.RiceKrispies, 1);
            this.breakfastCerealFees.Add(BreakfastCereal.LuckyCharms, 1);

            this.cfIDs = new Dictionary<string, string>();

            // Initialize Merchandise
            this.merchFees = new Dictionary<MerchandiseItem, double>();
            this.merchFees.Add(MerchandiseItem.Frisbee, 10);
            this.merchFees.Add(MerchandiseItem.Basketball, 20);
            this.merchFees.Add(MerchandiseItem.Shoes, 60);
        }

        ////[Test]
        ////[Category(Priority.One)]
        ////[Description("708")]
        public void CanCreateRegistrationForEventWithNoRegTypesAndPayWithCheck()
        {
            this.withRegType = false;
            
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventWithoutRegType/*, 0*/);

            if (ManagerSiteMgr.EventExists(EventWithoutRegType))
            {
                this.eventID = ManagerSiteMgr.GetFirstEventId(EventWithoutRegType);
            }
            else
            {
                this.CreateNewEvent();
            }

            RegisterMgr.CurrentEventId = this.eventID;
            this.CreateRegistrationAndPayWithCheck(2);
        }

        [Test]
        [Category(Priority.One)]
        [Description("335")]
        public void CanCreateRegistrationForEventWithRegTypesAndPayWithCheck()
        {
            this.withRegType = true;

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventWithRegType);

            if (ManagerSiteMgr.EventExists(EventWithRegType))
            {
                this.eventID = ManagerSiteMgr.GetFirstEventId(EventWithRegType);
            }
            else
            {
                this.CreateNewEvent();
            }

            RegisterMgr.CurrentEventId = this.eventID;
            this.CreateRegistrationAndPayWithCheck(4);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("748")]
        public void AttendeeInfoShowsCorrectHeader()
        {
            int registerID = 11351618;
            this.LoadAttendeeInfoPage(registerID, "selenium m Rgrssn-5775948560258");
            //BO.Registration.Attendee attendee = new BO.Registration.Attendee(registerID);
            //VerifyTool.VerifyValue(attendee.RegisterId, registerID, "RegisterID: {0}");
        }

        ////[Test]
        ////[Category(Priority.Three)]
        ////[Description("749")]
        public void CanCreateRegistrationWithoutRegTypesAndAttendeeInfoShowCorrectInformation()
        {
            this.CanCreateRegistrationForEventWithNoRegTypesAndPayWithCheck();

            this.LoadAttendeeInfoPage(RegisterMgr.CurrentRegistrationId, RegisterMgr.CurrentRegistrantFullName);
            BackendMgr.VerifyAttendeeInfoInformation(RegisterMgr);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("750")]
        public void CanCreateRegistrationWithoutRegTypesAndAttendeeInfoShowCorrectInformationAndMakeBackendEdits()
        {
            this.CanCreateRegistrationWithoutRegTypesAndAttendeeInfoShowCorrectInformation();
            BackendMgr.EditAttendeePersonalInformation();
        }

        [Step]
        private void LoadAttendeeInfoPage(int registrationId, string fullName)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            string eventSessionId = ManagerSiteMgr.GetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(eventSessionId, registrationId);
            BackendMgr.VerifyHeaderIsCorrect(String.Format("Attendee Info for {0}", fullName));
        }

        #region Event creation
        [Step]
        private void CreateNewEvent()
        {
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

            if (this.withRegType)
            {
                this.eventName = EventWithRegType;
            }
            else
            {
                this.eventName = EventWithoutRegType;
            }

            this.SetStartPage();
            BuilderMgr.Next();
            this.SetPIPage();
            BuilderMgr.Next();
            this.SetAgendaPage();
            BuilderMgr.Next();
            this.SetLodgingTravelPage();
            BuilderMgr.Next();
            this.SetMerchandisePage();
            BuilderMgr.Next();
            this.SetCheckoutPage();
            BuilderMgr.Next();
            this.SetConfirmationPage();

            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SaveAndClose();
        }

        private void SetStartPage()
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, this.eventName);
            BuilderMgr.SetEventFee(EventFee);

            if (this.withRegType)
            {
                foreach (KeyValuePair<RegType, double?> regTypeFee in this.regTypeFees)
                {
                    BuilderMgr.AddRegTypeWithEventFee(regTypeFee.Key.ToString(), regTypeFee.Value);
                }
            }

            BuilderMgr.SaveAndStay();
        }

        private void SetPIPage()
        {
            BuilderMgr.VerifyPersonalInfoPageDefaults();
            this.AddFirstPICF();
            this.AddSecondPICF();
            this.AddThirdPICF();
            BuilderMgr.SaveAndStay();
        }

        private void SetAgendaPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.VerifyEventAgendaPage();
            this.AddFirstAgendaItem();
            this.AddSecondAgendaItem();
            this.AddThirdAgendaItem();
            this.AddFourthAgendaItem();
            this.AddFifthAgendaItem();
            this.AddSixthAgendaItem();
            this.AddSeventhAgendaItem();
            this.AddEighthAgendaItem();

            BuilderMgr.SaveAndStay();
        }

        private void SetLodgingTravelPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.EnterEventLodgingTravelPage();
            BuilderMgr.SaveAndStay();
        }

        private void SetMerchandisePage()
        {
            foreach (KeyValuePair<MerchandiseItem, double> fee in this.merchFees)
            {
                BuilderMgr.AddMerchandiseItemWithFeeAmount(
                    MerchandiseManager.MerchandiseType.Fixed,
                    fee.Key.ToString(),
                    fee.Value,
                    null,
                    null);
            }
            
            BuilderMgr.SaveAndStay();
        }

        private void SetCheckoutPage()
        {
            BuilderMgr.EnterEventCheckoutPageFull();
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventCheckoutPage();
        }

        private void SetConfirmationPage()
        {
            BuilderMgr.SetEventConfirmationPage();
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventConfirmationPage();
        }

        #region Add PI CF
        private void AddFirstPICF()
        {
            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(PICustomFieldName.AreYouASteelersFan));
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.CheckBox);
            BuilderMgr.CFMgr.SelectFieldPosition(CFManagerBase.FieldPosition.LeftOfName);
            BuilderMgr.CFMgr.SaveAndClose();
        }

        private void AddSecondPICF()
        {
            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(PICustomFieldName.FavoriteBreakfast));
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.RadioButton);
            BuilderMgr.CFMgr.ClickAddMultiChoiceItem();
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SetName(StringEnum.GetStringValue(FavoriteBreakfast.SugaryCereal));
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SaveAndNew();
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SetName(StringEnum.GetStringValue(FavoriteBreakfast.EggsAndBacon));
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SaveAndNew();
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SetName(StringEnum.GetStringValue(FavoriteBreakfast.DeepFriedGrasshoppers));
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SaveAndClose();
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.SelectFieldPosition(CFManagerBase.FieldPosition.BelowName);
            BuilderMgr.CFMgr.SaveAndClose();
        }

        private void AddThirdPICF()
        {
            BuilderMgr.ClickAddPICustomField();
            BuilderMgr.CFMgr.SetName(StringEnum.GetStringValue(PICustomFieldName.FavoriteTowns));
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.Dropdown);
            BuilderMgr.CFMgr.ClickAddMultiChoiceItem();
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SetName(FavoriteTowns.Longmont.ToString());
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SaveAndNew();
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SetName(FavoriteTowns.Lafayette.ToString());
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SaveAndNew();
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SetName(FavoriteTowns.Louisville.ToString());
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SaveAndNew();
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SetName(FavoriteTowns.Superior.ToString());
            BuilderMgr.CFMgr.MultiChoiceItemMgr.SaveAndClose();
            BuilderMgr.CFMgr.ExpandAdvanced();
            BuilderMgr.CFMgr.SelectFieldPosition(CFManagerBase.FieldPosition.BelowName);
            BuilderMgr.CFMgr.SaveAndClose();
        }
        #endregion

        #region Add Agenda items
        private void AddFirstAgendaItem()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(StringEnum.GetStringValue(AgendaItemName.Orientation));
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.AlwaysSelected);
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(AgendaOrientationFee);

            // Set 'Visible' option
            if (this.withRegType)
            {
                // Because the 'Visible to all' option is checked as default, we must uncheck it first
                BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Visible);

                BuilderMgr.AGMgr.ShowAllRegTypes();
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, RegType.VIP.ToString());
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, RegType.Student.ToString());
            }
            else
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible);
            }

            // Set 'Required' option
            if (this.withRegType)
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required, RegType.VIP.ToString());
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required, RegType.Student.ToString());
            }
            else
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required);
            }

            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void AddSecondAgendaItem()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(StringEnum.GetStringValue(AgendaItemName.UltimateFrisbeeLevel));
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.RadioButton);

            foreach (KeyValuePair<FrisbeeLevel, double> frisbee in this.frisbeeLevelFees)
            {
                BuilderMgr.AGMgr.AddMultiChoiceItem(StringEnum.GetStringValue(frisbee.Key), frisbee.Value);
            }

            if (this.withRegType)
            {
                BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Visible);
                BuilderMgr.AGMgr.ShowAllRegTypes();
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, RegType.VIP.ToString());
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, RegType.Student.ToString());
            }
            else
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible);
            }

            if (this.withRegType)
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required, RegType.VIP.ToString());
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required, RegType.Student.ToString());
            }
            else
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required);
            }

            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void AddThirdAgendaItem()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(StringEnum.GetStringValue(AgendaItemName.PostGameDinnerWithRaffle));
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.SectionHeader);
            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible);
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void AddFourthAgendaItem()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(StringEnum.GetStringValue(AgendaItemName.Dinner));
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.CheckBox);
            BuilderMgr.AGMgr.SetLocation("Dinner hall");
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(AgendaDinnerFee);

            if (this.withRegType)
            {
                BuilderMgr.AGMgr.SetVisibilityOption(false, AgendaItemManager.VisibilityOption.Visible);
                BuilderMgr.AGMgr.ShowAllRegTypes();
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible, RegType.Vendor.ToString());
            }
            else
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible);
            }

            if (this.withRegType)
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required, RegType.Vendor.ToString());
            }
            else
            {
                BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Required);
            }

            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void AddFifthAgendaItem()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(StringEnum.GetStringValue(AgendaItemName.BreakfastCereal));
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.RadioButton);

            foreach (KeyValuePair<BreakfastCereal, double> breakfast in this.breakfastCerealFees)
            {
                BuilderMgr.AGMgr.AddMultiChoiceItem(StringEnum.GetStringValue(breakfast.Key), breakfast.Value);
            }

            BuilderMgr.AGMgr.SetVisibilityOption(true, AgendaItemManager.VisibilityOption.Visible);

            // Set the conditional logic
            BuilderMgr.AGMgr.SetConditionalLogic(true, StringEnum.GetStringValue(FavoriteBreakfast.SugaryCereal));

            BuilderMgr.AGMgr.SelectFieldPosition(CFManagerBase.FieldPosition.BelowName);
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void AddSixthAgendaItem()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(StringEnum.GetStringValue(AgendaItemName.TrackingKeywords));
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.OneLineText);
            BuilderMgr.AGMgr.SetOneLineLength(50);
            BuilderMgr.AGMgr.SetVisibilityOption(true, CFManagerBase.VisibilityOption.Admin);
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void AddSeventhAgendaItem()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(StringEnum.GetStringValue(AgendaItemName.Basketball101));
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.CheckBox);
            BuilderMgr.AGMgr.SetLocation("by the hoop");
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(AgendaBasketballOneFee);
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void AddEighthAgendaItem()
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(StringEnum.GetStringValue(AgendaItemName.Basketball102));
            BuilderMgr.AGMgr.SetType(AgendaItemManager.AgendaItemType.CheckBox);
            BuilderMgr.AGMgr.SetLocation("by the hoop");
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(AgendaBasketballTwoFee);
            BuilderMgr.AGMgr.SetConditionalLogic(true, StringEnum.GetStringValue(AgendaItemName.Basketball101));
            BuilderMgr.AGMgr.ClickSaveItem();
        }
        #endregion

        #endregion

        #region Registration
        [Step]
        private void CreateRegistrationAndPayWithCheck(int merchandiseQuantity)
        {
            RegType regType = RegType.Speaker;
            RegisterMgr.OpenRegisterPage();
            RegisterMgr.Checkin();

            if (this.withRegType)
            {
                RegisterMgr.SelectRegType(regType.ToString());
                RegisterMgr.CurrentRegistrationTypeID = BuilderMgr.RegTypeMgr.Fetch_RegTypeID(this.eventID, regType.ToString());
            }

            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();

            RegisterMgr.CurrentRegistrationId = RegisterMgr.GetRegIdFromSession();

            RegisterMgr.SelectPersonalInfoCustomFields();
            RegisterMgr.Continue();

            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();

            RegisterMgr.EnterLodgingInfo();
            RegisterMgr.EnterTravelInfo();
            RegisterMgr.Continue();

            RegisterMgr.SelectMerchandise(merchandiseQuantity);
            RegisterMgr.Continue();

            double totalFee = 0;

            if (this.withRegType)
            {
                totalFee =
                    (this.regTypeFees[regType] == null ? 0 : this.regTypeFees[regType].Value) +
                    this.breakfastCerealFees[BreakfastCereal.CocoaPuffs] +
                    AgendaBasketballOneFee +
                    AgendaBasketballTwoFee +
                    (this.merchFees[MerchandiseItem.Frisbee] +
                    this.merchFees[MerchandiseItem.Basketball] +
                    this.merchFees[MerchandiseItem.Shoes]) * merchandiseQuantity;
            }
            else
            {
                totalFee =
                    EventFee +
                    AgendaOrientationFee +
                    this.frisbeeLevelFees[FrisbeeLevel.NeverPlayed] +
                    AgendaDinnerFee +
                    this.breakfastCerealFees[BreakfastCereal.CocoaPuffs] +
                    AgendaBasketballOneFee +
                    AgendaBasketballTwoFee +
                    (this.merchFees[MerchandiseItem.Frisbee] +
                    this.merchFees[MerchandiseItem.Basketball] +
                    this.merchFees[MerchandiseItem.Shoes]) * merchandiseQuantity;
            }

            RegisterMgr.PayMoneyAndVerify(totalFee, totalFee, RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
        #endregion
    }
}
