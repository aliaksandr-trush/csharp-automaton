namespace RegOnline.RegressionTest.Fixtures.SpecialOptions
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RequiredFieldsShowHideDateFixture : FixtureBase
    {
        private const string EventName = "RequiredFieldsShowHideDate";
        private const int InvalidEventID = ManagerBase.InvalidId;
        private const double EventFee = 10;
        private const double AgendaPrice = 10;
        private const double MerchandisePrice = 20;
        private const int EventFeeQuantity = 1;
        private const int AgendaFeeQuantity = 1;
        private const int AgendaItemQuantity = 3;
        private const int MerchandiseItemQuantity = 3;
        private const int MerchandiseFeeQuantity = 1;
        
        private const int TenDaysBefore = -10;
        private const int TenDaysAfter = 10;
        private const int FiveDaysBefore = -5;
        private const int FiveDaysAfter = 5;
        private const int TwoDaysBefore = -2;
        private const int TwoDaysAfter = 2;

        private readonly string EventFeeQuantityString = EventFeeQuantity.ToString();
        private readonly string AgendaFeeQuantityString = AgendaFeeQuantity.ToString();
        private readonly string MerchandiseFeeQuantityString = MerchandiseFeeQuantity.ToString();

        private enum PICF
        {
            PICFBefore,
            PICFNow,
            PICFAfter,
            PICFShow,
            PICFNotShow,
            PICFHide,
            PICFNotHide
        }

        private enum AgendaItem
        {
            AgendaItemBefore,
            AgendaItemNow,
            AgendaItemAfter,
            AgendaItemShow,
            AgendaItemNotShow,
            AgendaItemHide,
            AgendaItemNotHide
        }

        private enum LTCF
        {
            LTCFBefore,
            LTCFNow,
            LTCFAfter,
            LTCFShow,
            LTCFNotShow,
            LTCFHide,
            LTCFNotHide
        }

        private enum MerchandiseItem
        {
            MerchandiseItemBefore,
            MerchandiseItemNow,
            MerchandiseItemAfter,
            MerchandiseItemShow,
            MerchandiseItemNotShow,
            MerchandiseItemHide,
            MerchandiseItemNotHide
        }

        private int eventID = InvalidEventID;
        private DateTime nowDate = DateTime.Now;
        private string formatedEventFee;
        private string formatedAgendaFee;
        private string formatedMerchandiseFee;

        #region Test Methods
        [Test]
        [Category(Priority.Two)]
        [Description("715")]
        public void RequiredFieldsShowHideDateTest()
        {
            this.CreateNewEvent();
            this.RegistrationForRequiredFieldsShowHideDate();
        }
        #endregion

        #region Create Event Methods
        [Step]
        private void CreateNewEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (ManagerSiteMgr.EventExists(EventName))
            {
                this.eventID = ManagerSiteMgr.GetFirstEventId(EventName);
            }
            else if (this.eventID == InvalidEventID)
            {
                ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
                this.SetEventStartPage();
                BuilderMgr.Next();
                this.SetPersonalInfoPage();
                BuilderMgr.Next();
                this.SetAgendaItemPage();
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
        }

        private void SetEventStartPage()
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName);
            BuilderMgr.SetEventFee(EventFee);
            BuilderMgr.SaveAndStay();
        }

        private void SetPersonalInfoPage()
        {
            //verify initial defaults
            BuilderMgr.VerifyPersonalInfoPageDefaults();

            //add custom field with show/hide date
            BuilderMgr.ClickAddPICustomField();
            this.SetCustomFieldWithShowHideDate(PICF.PICFBefore.ToString(), TenDaysBefore, FiveDaysBefore);

            BuilderMgr.ClickAddPICustomField();
            this.SetCustomFieldWithShowHideDate(PICF.PICFNow.ToString(), FiveDaysBefore, FiveDaysAfter);

            BuilderMgr.ClickAddPICustomField();
            this.SetCustomFieldWithShowHideDate(PICF.PICFAfter.ToString(), FiveDaysAfter, TenDaysAfter);

            //add custom field only with show date
            BuilderMgr.ClickAddPICustomField();
            this.SetCustomFieldWithShowHideDate(PICF.PICFShow.ToString(), TwoDaysBefore, null);

            BuilderMgr.ClickAddPICustomField();
            this.SetCustomFieldWithShowHideDate(PICF.PICFNotShow.ToString(), TwoDaysAfter, null);

            //add custom field only hide date
            BuilderMgr.ClickAddPICustomField();
            this.SetCustomFieldWithShowHideDate(PICF.PICFHide.ToString(), null, TwoDaysBefore);

            BuilderMgr.ClickAddPICustomField();
            this.SetCustomFieldWithShowHideDate(PICF.PICFNotHide.ToString(), null, TwoDaysAfter);

            //save and stay
            BuilderMgr.SaveAndStay();
        }

        private void SetAgendaItemPage()
        {
            // verify splash page
            BuilderMgr.VerifySplashPage();

            // continue to agenda page
            BuilderMgr.ClickYesOnSplashPage();

            // verify agenda page
            BuilderMgr.VerifyEventAgendaPage();

            // add agenda item with show/hide date
            this.AddAgendaItemWithShowHideDate(AgendaItem.AgendaItemBefore.ToString(), TenDaysBefore, FiveDaysBefore);
            this.AddAgendaItemWithShowHideDate(AgendaItem.AgendaItemNow.ToString(), FiveDaysBefore, FiveDaysAfter);
            this.AddAgendaItemWithShowHideDate(AgendaItem.AgendaItemAfter.ToString(), FiveDaysAfter, TenDaysAfter);

            // add agenda item only with show date
            this.AddAgendaItemWithShowHideDate(AgendaItem.AgendaItemShow.ToString(), TwoDaysBefore, null);
            this.AddAgendaItemWithShowHideDate(AgendaItem.AgendaItemNotShow.ToString(), TwoDaysAfter, null);

            // add agenda item only with hide date
            this.AddAgendaItemWithShowHideDate(AgendaItem.AgendaItemHide.ToString(), null, TwoDaysBefore);
            this.AddAgendaItemWithShowHideDate(AgendaItem.AgendaItemNotHide.ToString(), null, TwoDaysAfter);

            BuilderMgr.SaveAndStay();
        }

        private void SetLodgingTravelPage()
        {
            // verify splash page
            BuilderMgr.VerifySplashPage();

            // continue to lodging page
            BuilderMgr.ClickYesOnSplashPage();

            // add custom field with show/hide date
            BuilderMgr.ClickAddLodgingCustomField();
            this.SetCustomFieldWithShowHideDate(LTCF.LTCFBefore.ToString(), TenDaysBefore, FiveDaysBefore);

            BuilderMgr.ClickAddLodgingCustomField();
            this.SetCustomFieldWithShowHideDate(LTCF.LTCFNow.ToString(), FiveDaysBefore, FiveDaysAfter);

            BuilderMgr.ClickAddLodgingCustomField();
            this.SetCustomFieldWithShowHideDate(LTCF.LTCFAfter.ToString(), FiveDaysAfter, TenDaysAfter);

            // add custom field only with show date
            BuilderMgr.ClickAddLodgingCustomField();
            this.SetCustomFieldWithShowHideDate(LTCF.LTCFShow.ToString(), TwoDaysBefore, null);

            BuilderMgr.ClickAddLodgingCustomField();
            this.SetCustomFieldWithShowHideDate(LTCF.LTCFNotShow.ToString(), TwoDaysAfter, null);

            // add custom field only hide date
            BuilderMgr.ClickAddLodgingCustomField();
            this.SetCustomFieldWithShowHideDate(LTCF.LTCFHide.ToString(), null, TwoDaysBefore);

            BuilderMgr.ClickAddLodgingCustomField();
            this.SetCustomFieldWithShowHideDate(LTCF.LTCFNotHide.ToString(), null, TwoDaysAfter);

            // save and stay
            BuilderMgr.SaveAndStay();
        }

        private void SetMerchandisePage()
        {
            // add merchandise item with show/hide date
            this.AddMerchandiseWithShowHideDate(MerchandiseItem.MerchandiseItemBefore.ToString(), TenDaysBefore, FiveDaysBefore);
            this.AddMerchandiseWithShowHideDate(MerchandiseItem.MerchandiseItemNow.ToString(), FiveDaysBefore, FiveDaysAfter);
            this.AddMerchandiseWithShowHideDate(MerchandiseItem.MerchandiseItemAfter.ToString(), FiveDaysAfter, TenDaysAfter);

            // add merchandise item only with show date
            this.AddMerchandiseWithShowHideDate(MerchandiseItem.MerchandiseItemShow.ToString(), TwoDaysBefore, null);
            this.AddMerchandiseWithShowHideDate(MerchandiseItem.MerchandiseItemNotShow.ToString(), TwoDaysAfter, null);

            // add merchandise item only with hide date
            this.AddMerchandiseWithShowHideDate(MerchandiseItem.MerchandiseItemHide.ToString(), null, TwoDaysBefore);
            this.AddMerchandiseWithShowHideDate(MerchandiseItem.MerchandiseItemNotHide.ToString(), null, TwoDaysAfter);
        }

        private void SetCheckoutPage()
        {
            // enter event checkout page info
            BuilderMgr.EnterEventCheckoutPage();

            // save and stay
            BuilderMgr.SaveAndStay();
        }

        private void SetConfirmationPage()
        {
            // enter event checkout page info
            BuilderMgr.SetEventConfirmationPage();

            // save and stay
            BuilderMgr.SaveAndStay();

            // verify event checkout page info
            BuilderMgr.VerifyEventConfirmationPage();
        }
        #endregion

        #region Create Event Helper Methods
        private void SetCustomFieldWithShowHideDate(string name, int? showOffset, int? hideOffset)
        {
            BuilderMgr.CFMgr.SetName(name);
            BuilderMgr.CFMgr.SetType(CustomFieldManager.CustomFieldType.CheckBox);
            UIUtil.DefaultProvider.ExpandAdvanced();
            if (showOffset != null)
            {
                BuilderMgr.CFMgr.SetShowDate(DateTime.Now.AddDays(Convert.ToDouble(showOffset)));
            }

            if (hideOffset != null)
            {
                BuilderMgr.CFMgr.SetHideDate(DateTime.Now.AddDays(Convert.ToDouble(hideOffset)));            
            }
            BuilderMgr.CFMgr.ClearDatePopUp();
            BuilderMgr.CFMgr.SaveAndClose();
        }

        private void AddAgendaItemWithShowHideDate(string name, int? showOffset, int? hideOffset)
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(name);
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(AgendaPrice);

            UIUtil.DefaultProvider.ExpandAdvanced();

            if (showOffset != null)
            {
                BuilderMgr.AGMgr.SetShowDate(DateTime.Now.AddDays(Convert.ToDouble(showOffset)));
            }

            if (hideOffset != null)
            {
                BuilderMgr.AGMgr.SetHideDate(DateTime.Now.AddDays(Convert.ToDouble(hideOffset)));
            }
            BuilderMgr.CFMgr.ClearDatePopUp();
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void AddMerchandiseWithShowHideDate(string name, int? showOffset, int? hideOffset)
        {
            //add merchandise item
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Fixed, name, MerchandisePrice, null, null);

            //add custom field with show/hide date
            BuilderMgr.OpenMerchandiseItem(name);
            BuilderMgr.ClickAdvancedOnFrame();

            if (showOffset != null)
            {
                BuilderMgr.MerchMgr.SetShowDate(DateTime.Now.AddDays(Convert.ToDouble(showOffset)));
            }

            if (hideOffset != null)
            {
                BuilderMgr.MerchMgr.SetHideDate(DateTime.Now.AddDays(Convert.ToDouble(hideOffset)));
            }

            //save and close
            BuilderMgr.MerchMgr.SaveAndClose();
        }
        #endregion

        #region Registrations
        [Step]
        private void RegistrationForRequiredFieldsShowHideDate()
        {
            // start registration
            RegisterMgr.OpenRegisterPage(this.eventID);

            // register check in
            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            // enter profile info
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.SelectPersonalInfoCustomFields();
            RegisterMgr.VerifyCustomFieldPresent(PICF.PICFNow.ToString(), true);
            RegisterMgr.VerifyCustomFieldPresent(PICF.PICFShow.ToString(), true);
            RegisterMgr.VerifyCustomFieldPresent(PICF.PICFNotHide.ToString(), true);
            //Registration.VerifyCustomFieldNamesOld(new string[3] { "PICFNow" ,"PICFShow","PICFNotHide"});
            RegisterMgr.Continue();

            // select agenda item
            RegisterMgr.SelectAgendaItems();
            RegisterMgr.VerifyCustomFieldPresent(AgendaItem.AgendaItemNow.ToString(), true);
            RegisterMgr.VerifyCustomFieldPresent(AgendaItem.AgendaItemShow.ToString(), true);
            RegisterMgr.VerifyCustomFieldPresent(AgendaItem.AgendaItemNotHide.ToString(), true);
            RegisterMgr.Continue();

            // select lodging and travel custom field
            RegisterMgr.SelectLodgingCustomFields();
            RegisterMgr.VerifyCustomFieldPresent(LTCF.LTCFNow.ToString(), true);
            RegisterMgr.VerifyCustomFieldPresent(LTCF.LTCFShow.ToString(), true);
            RegisterMgr.VerifyCustomFieldPresent(LTCF.LTCFNotHide.ToString(), true);
            //Registration.VerifyCustomFieldNamesOld(new string[3] { "LTCFNow" , "LTCFShow" ,"LTCFNotHide" });
            RegisterMgr.Continue();

            // select merchandise and enter a quantity
            RegisterMgr.SelectMerchandise(MerchandiseFeeQuantity);
            RegisterMgr.VerifyMerchandiseItemPresent(MerchandiseItem.MerchandiseItemNow.ToString(), true);
            RegisterMgr.VerifyMerchandiseItemPresent(MerchandiseItem.MerchandiseItemShow.ToString(), true);
            RegisterMgr.VerifyMerchandiseItemPresent(MerchandiseItem.MerchandiseItemNotHide.ToString(), true);
            RegisterMgr.Continue();

            // pay money and verify agemda item name and fee & merchandise item name and fee
            RegisterManager.FeeResponse[] expFees = new RegisterManager.FeeResponse[7];

            formatedEventFee = MoneyTool.FormatMoney(EventFee);

            formatedAgendaFee = MoneyTool.FormatMoney(AgendaPrice);

            formatedMerchandiseFee = MoneyTool.FormatMoney(MerchandisePrice);

            expFees[0] = new RegisterManager.FeeResponse();
            expFees[0].FeeName = "Event Fee ";
            expFees[0].FeeQuantity = EventFeeQuantityString;
            expFees[0].FeeUnitPrice = formatedEventFee;
            expFees[0].FeeAmount = formatedEventFee;

            expFees[1] = new RegisterManager.FeeResponse();
            expFees[1].FeeName = AgendaItem.AgendaItemNow.ToString() + " ";
            expFees[1].FeeQuantity = AgendaFeeQuantityString;
            expFees[1].FeeUnitPrice = formatedAgendaFee;
            expFees[1].FeeAmount = formatedAgendaFee;

            expFees[2] = new RegisterManager.FeeResponse();
            expFees[2].FeeName = AgendaItem.AgendaItemShow.ToString() + " ";
            expFees[2].FeeQuantity = AgendaFeeQuantityString;
            expFees[2].FeeUnitPrice = formatedAgendaFee;
            expFees[2].FeeAmount = formatedAgendaFee;

            expFees[3] = new RegisterManager.FeeResponse();
            expFees[3].FeeName = AgendaItem.AgendaItemNotHide.ToString() + " ";
            expFees[3].FeeQuantity = AgendaFeeQuantityString;
            expFees[3].FeeUnitPrice = formatedAgendaFee;
            expFees[3].FeeAmount = formatedAgendaFee;

            expFees[4] = new RegisterManager.FeeResponse();
            expFees[4].FeeName = MerchandiseItem.MerchandiseItemNow.ToString() + " ";
            expFees[4].FeeQuantity = MerchandiseFeeQuantityString;
            expFees[4].FeeUnitPrice = formatedMerchandiseFee;
            expFees[4].FeeAmount = formatedMerchandiseFee;

            expFees[5] = new RegisterManager.FeeResponse();
            expFees[5].FeeName = MerchandiseItem.MerchandiseItemShow.ToString() + " ";
            expFees[5].FeeQuantity = MerchandiseFeeQuantityString;
            expFees[5].FeeUnitPrice = formatedMerchandiseFee;
            expFees[5].FeeAmount = formatedMerchandiseFee;

            expFees[6] = new RegisterManager.FeeResponse();
            expFees[6].FeeName = MerchandiseItem.MerchandiseItemNotHide.ToString() + " ";
            expFees[6].FeeQuantity = MerchandiseFeeQuantityString;
            expFees[6].FeeUnitPrice = formatedMerchandiseFee;
            expFees[6].FeeAmount = formatedMerchandiseFee;

            double totalFee = EventFee + AgendaPrice *  AgendaItemQuantity + MerchandisePrice * MerchandiseItemQuantity;

            RegisterMgr.VerifyRegistrationFees(expFees);
            RegisterMgr.PayMoneyAndVerify(totalFee, RegisterManager.PaymentMethod.Check);
            
            //finish registration and check the registration details
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
        #endregion
    }
}
