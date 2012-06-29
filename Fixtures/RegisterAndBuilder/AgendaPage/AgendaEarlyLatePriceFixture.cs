namespace RegOnline.RegressionTest.Fixtures.RegisterAndBuilder.AgendaPage
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgendaEarlyLatePriceFixture : FixtureBase
    {
        #region Constants
        private const string EventName = "AgendaEarlyLatePriceTest";
        private const string AgendaItemIndexPrefix = "CF";
        private const double StandardPrice = 100;
        private const double EarlyPrice = 80;
        private const double LatePrice = 120;
        private const double PricePlus = 10;
        private const int FiveDaysBefore = -5;
        private const int FiveDaysAfter = 5;
        private const int TenDaysBefore = -10;
        private const int TenDaysAfter = 10;
        private const int TwoMinutesAfter = 2;
        private const int ThreeMinutesAfter = 3;
        private const int RegLimit = 1;
        private const int InvalidEventID = -1;
        #endregion

        #region Enum
        private enum AgendaItemType
        {
            EarlyPriceTime = 0,
            EarlyPriceRegistration = 1,
            LatePriceTime = 2,
            StandardPriceTime = 3,
            PriceSwitch = 4
        }

        private enum PriceSwitchType
        {
            EarlierThanEarlyPriceEndTime,
            BetweenEarlyPriceEndTimeAndLatePriceStartTime,
            LaterThanLatePriceStartTime
        }
        #endregion

        #region Private fields
        private int eventID = InvalidEventID;
        private string sessionID = string.Empty;
        private string agendaPageUrl = string.Empty;
        private Dictionary<int, int> cfIDs = new Dictionary<int, int>();
        private DateTime priceSwitchItemSettingTime;
        private DateTime priceSwitchItemRegTime;
        #endregion

        #region Test cases
        [Test]
        [Category(Priority.Two)]
        [Description("191")]
        public void AgendaEarlyLatePriceTest()
        {
            this.CreateNewEvent();
            this.TestReg();
        }
        #endregion

        #region Event setting
        private void LoginAndGetSessionID()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.sessionID = BuilderMgr.GetEventSessionId();
        }

        [Step]
        private void CreateNewEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.sessionID = BuilderMgr.GetEventSessionId();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            this.SetEventStartPage();
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
            this.SetAgendaPage();
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            this.SetCheckoutPage();
            BuilderMgr.Next();
            this.SetConfirmationPage();
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void SetEventStartPage()
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName);
            BuilderMgr.SaveAndStay();
        }

        [Step]
        private void SetAgendaPage()
        {
            DateTime earlyDatetime = DateTime.Now.AddDays(FiveDaysAfter);
            DateTime lateDatetime = DateTime.Now.AddDays(TenDaysAfter);

            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.VerifyEventAgendaPage();

            // Add the 1st agenda item: EarlyPriceTime
            this.AddAgendaItemWithPricintOptions(AgendaItemType.EarlyPriceTime, null, earlyDatetime, lateDatetime);
            this.VerifyPriceHistory(AgendaItemType.EarlyPriceTime, StandardPrice, EarlyPrice, null, earlyDatetime, LatePrice, lateDatetime);

            // Add the 2nd agenda item: EarlyPriceRegistration
            this.AddAgendaItemWithPricintOptions(AgendaItemType.EarlyPriceRegistration, RegLimit, null, lateDatetime);
            this.VerifyPriceHistory(AgendaItemType.EarlyPriceRegistration, StandardPrice, EarlyPrice, RegLimit, earlyDatetime, LatePrice, lateDatetime);

            // Add the 3rd agenda item: LatePriceTime
            earlyDatetime = DateTime.Now.AddDays(TenDaysBefore);
            lateDatetime = DateTime.Now.AddDays(FiveDaysBefore);
            this.AddAgendaItemWithPricintOptions(AgendaItemType.LatePriceTime, null, earlyDatetime, lateDatetime);
            this.VerifyPriceHistory(AgendaItemType.LatePriceTime, StandardPrice, EarlyPrice, null, earlyDatetime, LatePrice, lateDatetime);

            // Add the 4th agenda item: StandardPriceTime
            earlyDatetime = DateTime.Now.AddDays(TenDaysBefore);
            lateDatetime = DateTime.Now.AddDays(TenDaysAfter);
            this.AddAgendaItemWithPricintOptions(AgendaItemType.StandardPriceTime, null, earlyDatetime, lateDatetime);
            this.VerifyPriceHistory(AgendaItemType.StandardPriceTime, StandardPrice, EarlyPrice, null, earlyDatetime, LatePrice, lateDatetime);

            // Add the 5th agenda item: PriceSwitch
            earlyDatetime = DateTime.Now.AddMinutes(TwoMinutesAfter);
            lateDatetime = DateTime.Now.AddMinutes(ThreeMinutesAfter);
            this.AddAgendaItemWithPricintOptions(AgendaItemType.PriceSwitch, null, earlyDatetime, lateDatetime);
            this.VerifyPriceHistory(AgendaItemType.PriceSwitch, StandardPrice, EarlyPrice, null, earlyDatetime, LatePrice, lateDatetime);
        }

        [Step]
        private void SetCheckoutPage()
        {
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndStay();
        }

        [Step]
        private void SetConfirmationPage()
        {
            BuilderMgr.SetEventConfirmationPage();
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventConfirmationPage();
        }

        private void AddAgendaItemWithPricintOptions(AgendaItemType type, int? earlyRegLimit, DateTime? earlyDatetime, DateTime lateDatetime)
        {
            BuilderMgr.ClickAddAgendaItem();

            BuilderMgr.AGMgr.SetName(type.ToString());
            BuilderMgr.AGMgr.SetTypeWithDefaults(AgendaItemManager.AgendaItemType.CheckBox);

            BuilderMgr.AGMgr.FeeMgr.ExpandOption();
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(StandardPrice);

            switch (type)
            {
                case AgendaItemType.EarlyPriceTime:
                case AgendaItemType.LatePriceTime:
                case AgendaItemType.StandardPriceTime:
                    BuilderMgr.AGMgr.FeeMgr.Pricing.SetEarlyPricingSchedule(EarlyPrice, earlyDatetime.Value);
                    break;

                case AgendaItemType.PriceSwitch:
                    BuilderMgr.AGMgr.FeeMgr.Pricing.SetEarlyPricingSchedule(EarlyPrice, earlyDatetime.Value);
                    this.priceSwitchItemSettingTime = DateTime.Now;
                    break;

                case AgendaItemType.EarlyPriceRegistration:
                    BuilderMgr.AGMgr.FeeMgr.Pricing.SetEarlyPricingSchedule(EarlyPrice, earlyRegLimit.Value);
                    break;

                default:
                    break;
            }

            BuilderMgr.AGMgr.FeeMgr.Pricing.SetLatePricingSchedule(LatePrice, lateDatetime);

            BuilderMgr.AGMgr.ClickSaveItem();
            this.GetAgendaCFid((int)type);
        }

        private void ChangeAgendaItemPricingOptions(AgendaItemType type, double standardPrice, double earlyPrice, DateTime earlyDatetime, double latePrice, DateTime lateDatetime)
        {
            BuilderMgr.AGMgr.OpenAgendaInListByOrder((int)type);
            BuilderMgr.AGMgr.FeeMgr.ExpandOption();
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(standardPrice);
            BuilderMgr.AGMgr.FeeMgr.Pricing.SetEarlyPricingSchedule(earlyPrice, earlyDatetime);
            BuilderMgr.AGMgr.FeeMgr.Pricing.SetLatePricingSchedule(latePrice, lateDatetime);
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void ChangeAgendaEarlyLateDatetime(AgendaItemType type, DateTime earlyDate, DateTime lateDate)
        {
            BuilderMgr.AGMgr.OpenAgendaInListByOrder((int)type);
            BuilderMgr.AGMgr.FeeMgr.ExpandOption();
            BuilderMgr.AGMgr.FeeMgr.Pricing.SetEarlyDateTime(earlyDate);
            BuilderMgr.AGMgr.FeeMgr.Pricing.SetLateDateTime(lateDate);
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void ChangeAgendaPrice(double priceChangeAmount)
        {
            foreach (AgendaItemType type in Enum.GetValues(typeof(AgendaItemType)))
            {
                BuilderMgr.AGMgr.OpenAgendaInListByOrder((int)type);
                BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(BuilderMgr.AGMgr.FeeMgr.GetStandardPrice() + priceChangeAmount);
                BuilderMgr.AGMgr.FeeMgr.ExpandOption();
                BuilderMgr.AGMgr.FeeMgr.Pricing.SetEarlyPrice(BuilderMgr.AGMgr.FeeMgr.Pricing.GetEarlyPrice() + priceChangeAmount);
                BuilderMgr.AGMgr.FeeMgr.Pricing.SetLatePrice(BuilderMgr.AGMgr.FeeMgr.Pricing.GetLatePrice() + priceChangeAmount);
                BuilderMgr.AGMgr.ClickSaveItem();
            }
        }

        private void DeleteAgendaEarlyLateDatetime(AgendaItemType type)
        {
            BuilderMgr.AGMgr.OpenAgendaInListByOrder((int)type);
            BuilderMgr.AGMgr.FeeMgr.ExpandOption();
            BuilderMgr.AGMgr.FeeMgr.Pricing.ClearEarlyPricingSchedule();
            BuilderMgr.AGMgr.FeeMgr.Pricing.ClearLatePricingSchedule();
            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void VerifyPriceHistory(AgendaItemType type, double standardPrice, double earlyPrice, int? regLimit, DateTime? earlyDatetime, double latePrice, DateTime lateDatetime)
        {
            BuilderMgr.AGMgr.OpenAgendaInListByOrder((int)type);
            BuilderMgr.AGMgr.FeeMgr.ExpandOption();
            BuilderMgr.AGMgr.FeeMgr.VerifyStandardPrice(standardPrice);

            if (type == AgendaItemType.EarlyPriceRegistration)
            {
                BuilderMgr.AGMgr.FeeMgr.Pricing.VerifyEarlyPricingOptions(earlyPrice, regLimit.Value);
            }
            else
            {
                BuilderMgr.AGMgr.FeeMgr.Pricing.VerifyEarlyPricingOptions(earlyPrice, earlyDatetime.Value);
            }

            BuilderMgr.AGMgr.FeeMgr.Pricing.VerifyLatePricingOptions(latePrice, lateDatetime);
            BuilderMgr.AGMgr.ClickCancel();
        }
        #endregion

        #region Registration
        [Step]
        private void TestReg()
        {
            if (this.eventID == InvalidEventID)
            {
                this.CreateNewEvent();
            }

            DateTime earlyDatetime;
            DateTime lateDatetime;

            this.CreateRegistrationForPriceSwitch();
            this.CreateRegistration(AgendaItemType.EarlyPriceTime, EarlyPrice);
            this.CreateRegistration(AgendaItemType.EarlyPriceRegistration, EarlyPrice);
            this.CreateRegistration(AgendaItemType.EarlyPriceRegistration, StandardPrice);
            this.CreateRegistration(AgendaItemType.LatePriceTime, LatePrice);
            this.CreateRegistration(AgendaItemType.StandardPriceTime, StandardPrice);

            this.OpenEventBuilderAgendaPage();

            // Change agenda item #1 to standard price and verify price history
            earlyDatetime = DateTime.Now.AddDays(TenDaysBefore);
            lateDatetime = DateTime.Now.AddDays(TenDaysAfter);
            this.ChangeAgendaEarlyLateDatetime(AgendaItemType.EarlyPriceTime, earlyDatetime, lateDatetime);
            this.VerifyPriceHistory(AgendaItemType.EarlyPriceTime, StandardPrice, EarlyPrice, null, earlyDatetime, LatePrice, lateDatetime);

            // Change agenda item #3 to early price and verify price history
            earlyDatetime = DateTime.Now.AddDays(FiveDaysAfter);
            lateDatetime = DateTime.Now.AddDays(TenDaysAfter);
            this.ChangeAgendaEarlyLateDatetime(AgendaItemType.LatePriceTime, earlyDatetime, lateDatetime);
            this.VerifyPriceHistory(AgendaItemType.LatePriceTime, StandardPrice, EarlyPrice, null, earlyDatetime, LatePrice, lateDatetime);

            // Change agenda item #4 to late price and verify price history
            earlyDatetime = DateTime.Now.AddDays(TenDaysBefore);
            lateDatetime = DateTime.Now.AddDays(FiveDaysBefore);
            this.ChangeAgendaEarlyLateDatetime(AgendaItemType.StandardPriceTime, earlyDatetime, lateDatetime);
            this.VerifyPriceHistory(AgendaItemType.StandardPriceTime, StandardPrice, EarlyPrice, null, earlyDatetime, LatePrice, lateDatetime);

            BuilderMgr.SaveAndClose();

            this.CreateRegistration(AgendaItemType.EarlyPriceTime, StandardPrice);
            this.CreateRegistration(AgendaItemType.LatePriceTime, EarlyPrice);
            this.CreateRegistration(AgendaItemType.StandardPriceTime, LatePrice);

            // Add $10 to each price
            this.OpenEventBuilderAgendaPage();
            this.ChangeAgendaPrice(PricePlus);
            BuilderMgr.SaveAndClose();

            this.CreateRegistration(AgendaItemType.EarlyPriceTime, StandardPrice + PricePlus);
            this.CreateRegistration(AgendaItemType.LatePriceTime, EarlyPrice + PricePlus);
            this.CreateRegistration(AgendaItemType.StandardPriceTime, LatePrice + PricePlus);

            // Delete early price and late price
            this.OpenEventBuilderAgendaPage();
            this.DeleteAgendaEarlyLateDatetime(AgendaItemType.EarlyPriceTime);
            this.DeleteAgendaEarlyLateDatetime(AgendaItemType.LatePriceTime);
            this.DeleteAgendaEarlyLateDatetime(AgendaItemType.StandardPriceTime);
            BuilderMgr.SaveAndClose();

            this.CreateRegistration(AgendaItemType.EarlyPriceTime, StandardPrice + PricePlus);
            this.CreateRegistration(AgendaItemType.LatePriceTime, StandardPrice + PricePlus);
            this.CreateRegistration(AgendaItemType.StandardPriceTime, StandardPrice + PricePlus);

            // Add early/late price once again
            this.OpenEventBuilderAgendaPage();

            // Add early/late price to agenda item #1
            earlyDatetime = DateTime.Now.AddDays(FiveDaysAfter);
            lateDatetime = DateTime.Now.AddDays(TenDaysAfter);
            this.ChangeAgendaItemPricingOptions(AgendaItemType.EarlyPriceTime, StandardPrice + PricePlus, EarlyPrice + PricePlus, earlyDatetime, LatePrice + PricePlus, lateDatetime);
            this.VerifyPriceHistory(AgendaItemType.EarlyPriceTime, StandardPrice + PricePlus, EarlyPrice + PricePlus, null, earlyDatetime, LatePrice + PricePlus, lateDatetime);

            // Add early/late price to agenda item #3
            earlyDatetime = DateTime.Now.AddDays(TenDaysBefore);
            lateDatetime = DateTime.Now.AddDays(FiveDaysBefore);
            this.ChangeAgendaItemPricingOptions(AgendaItemType.LatePriceTime, StandardPrice + PricePlus, EarlyPrice + PricePlus, earlyDatetime, LatePrice + PricePlus, lateDatetime);
            this.VerifyPriceHistory(AgendaItemType.LatePriceTime, StandardPrice + PricePlus, EarlyPrice + PricePlus, null, earlyDatetime, LatePrice + PricePlus, lateDatetime);

            // Add early/late price to agenda item #4
            earlyDatetime = DateTime.Now.AddDays(TenDaysBefore);
            lateDatetime = DateTime.Now.AddDays(TenDaysAfter);
            this.ChangeAgendaItemPricingOptions(AgendaItemType.StandardPriceTime, StandardPrice + PricePlus, EarlyPrice + PricePlus, earlyDatetime, LatePrice + PricePlus, lateDatetime);
            this.VerifyPriceHistory(AgendaItemType.StandardPriceTime, StandardPrice + PricePlus, EarlyPrice + PricePlus, null, earlyDatetime, LatePrice + PricePlus, lateDatetime);

            // Minus $10 to each price
            this.ChangeAgendaPrice(-PricePlus);

            BuilderMgr.SaveAndClose();

            this.CreateRegistration(AgendaItemType.EarlyPriceTime, EarlyPrice);
            this.CreateRegistration(AgendaItemType.LatePriceTime, LatePrice);
            this.CreateRegistration(AgendaItemType.StandardPriceTime, StandardPrice);
        }

        private void CreateRegistration(AgendaItemType type, double agendaFeeToVerify)
        {
            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(this.cfIDs[(int)type]);
            RegisterMgr.Continue();
            RegisterMgr.PayMoneyAndVerify(agendaFeeToVerify, RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void CreateRegistrationForPriceSwitch()
        {
            this.priceSwitchItemRegTime = DateTime.Now;
            TimeSpan timeSpanOne = new TimeSpan(this.priceSwitchItemSettingTime.Ticks);
            TimeSpan timeSpanTwo = new TimeSpan(this.priceSwitchItemRegTime.Ticks);
            TimeSpan duration = timeSpanOne.Subtract(timeSpanTwo).Duration();
            int durationSeconds = duration.Minutes * 60 + duration.Seconds;

            if (durationSeconds < TwoMinutesAfter * 60)
            {
                this.CreateRegistration(AgendaItemType.PriceSwitch, EarlyPrice);
            }
            else if (durationSeconds >= TwoMinutesAfter * 60 && durationSeconds < ThreeMinutesAfter * 60)
            {
                this.CreateRegistration(AgendaItemType.PriceSwitch, StandardPrice);
            }
            else
            {
                this.CreateRegistration(AgendaItemType.PriceSwitch, LatePrice);
            }
        }
        #endregion

        #region Helper methods
        private void GetAgendaCFid(int order)
        {
            this.AddToDictionary(this.cfIDs, order, BuilderMgr.AGMgr.GetAgendaItemID(order));
        }

        public void AddToDictionary(Dictionary<int, int> dic, int key, int val)
        {
            if (dic.ContainsKey(key))
            {
                dic.Remove(key);
            }

            dic.Add(key, val);
        }

        private void OpenEventBuilderAgendaPage()
        {
            this.LoginAndGetSessionID();
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventID, this.sessionID);
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
        }

        private long CalculateTimespanSeconds(DateTime dateTimeOne, DateTime dateTimeTwo)
        {
            TimeSpan timeSpanOne = new TimeSpan(dateTimeOne.Ticks);
            TimeSpan timeSpanTwo = new TimeSpan(dateTimeTwo.Ticks);
            TimeSpan duration = timeSpanOne.Subtract(timeSpanTwo).Duration();
            return duration.Minutes * 60 + duration.Seconds;
        }
        #endregion
    }
}
