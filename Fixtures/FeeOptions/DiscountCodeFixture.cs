namespace RegOnline.RegressionTest.Fixtures.FeeOptions
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category (FixtureCategory.Regression)]
    public class DiscountCodeFixture : FixtureBase
    {
        #region Constants
        private const double EventFee = 100.0;
        private const double RegTypeFeeOne = 400;
        private const double RegTypeFeeTwo = 200;
        private const double FreePriceFee = 0.0;
        private const double FreePricePercentage = 100.0;
        private const double HalfPricePercentage = 50.0;
        private const double FixedAmount = 25.0;
        private const double AgendaItemPrice = 50.0;
        private const double AgendaMultiChoiceItemPrice = 5.0;
        private const int MerchQuantity = 1;
        private const double MerchPrice = 25.0;
        private const string MerchDiscountCodeString = "Half=-50%,FixedAmount=-25,Free=-100%,Enter";
        private const int DiscountCodeLimit = 5;
        #endregion

        #region Enum
        private enum EventType
        {
            DiscountCodeFixture,
            DiscountCodeFixtureWithRegType,
            DiscountCodeLimitTest
        }

        private enum AgendaItem
        {
            CheckBox,
            AlwaysSelect,
            CheckBoxAndRequire,
            MultipleChoiceRadioButton,
        }

        //private enum AgendaItemNumber
        //{
        //    CF0,
        //    CF1,
        //    CF2,
        //    CF3
        //}

        private enum AgendaMultiChoiceItemName
        {
            MCRBMC1,
            MCRBMC2
        }

        private enum DiscountCodeType
        {
            [StringValue("")]
            Empty,

            [StringValue("Free")]
            Free,

            [StringValue("FixedAmount")]
            FixedAmount,

            [StringValue("Half")]
            Half,

            [StringValue("Enter")]
            Enter
        }

        private enum MerchandiseName
        {
            MerchandiseWithFixedPrice
        }

        private enum RegTypeName
        {
            RegTypeOne,
            RegTypeTwo
        }
        #endregion

        #region Fee variables definitions
        private DiscountFee feeWithNoDiscount;
        private DiscountFee feeWithAccessCode;
        private DiscountFee feeWithFreePrice;
        private DiscountFee feeWithHalfDiscount;
        private DiscountFee feeWithFixedAmountDiscount;
        private Dictionary<DiscountCodeType, DiscountFee> discountFee;

        private class DiscountFee
        {
            public double EventFee { get; set; }

            public double AgendaFee { get; set; }

            public double MerchandiseFee { get; set; }

            public double TotalFee { get; set; }

            public double TotalSaving { get; set; }
        }
        #endregion

        #region Private fields
        private int eventID = -1;
        private EventType eventType;
        private Dictionary<EventType, string> sessionIDs;
        private Dictionary<string, int> agendaItemIDs;
        //private Dictionary<AgendaItemNumber, int> cfIDsForCodeFixture;
        //private Dictionary<AgendaItemNumber, int> cfIDsForCodeFixtureWithRegType;
        //private Dictionary<AgendaItemNumber, int> cfIDsForCodeLimitTest;
        #endregion

        public DiscountCodeFixture()
        {
            this.agendaItemIDs = new Dictionary<string, int>();
            //this.cfIDsForCodeFixture = new Dictionary<AgendaItemNumber, int>();
            //this.cfIDsForCodeFixtureWithRegType = new Dictionary<AgendaItemNumber, int>();
            //this.cfIDsForCodeLimitTest = new Dictionary<AgendaItemNumber, int>();

            this.InitializeDiscountFee();
            this.InitializeEventSessionIDs();
        }

        #region Initializer
        private void InitializeDiscountFee()
        {
            this.feeWithNoDiscount = new DiscountFee();
            this.feeWithNoDiscount.EventFee = EventFee;

            // '3' here represents the quantity of non-MultiChoiceItem
            this.feeWithNoDiscount.AgendaFee = 3 * AgendaItemPrice + AgendaMultiChoiceItemPrice;

            this.feeWithNoDiscount.MerchandiseFee = MerchPrice * MerchQuantity;
            this.feeWithNoDiscount.TotalFee = this.feeWithNoDiscount.EventFee + this.feeWithNoDiscount.AgendaFee + this.feeWithNoDiscount.MerchandiseFee;
            this.feeWithNoDiscount.TotalSaving = FreePriceFee;

            this.feeWithAccessCode = new DiscountFee();
            this.feeWithAccessCode = this.feeWithNoDiscount;

            this.feeWithFreePrice = new DiscountFee();
            this.feeWithFreePrice.EventFee = FreePriceFee;
            this.feeWithFreePrice.AgendaFee = FreePriceFee;
            this.feeWithFreePrice.MerchandiseFee = FreePriceFee;
            this.feeWithFreePrice.TotalFee = this.feeWithFreePrice.EventFee + this.feeWithFreePrice.AgendaFee + this.feeWithFreePrice.MerchandiseFee;
            this.feeWithFreePrice.TotalSaving = this.feeWithNoDiscount.TotalFee - this.feeWithFreePrice.TotalFee;

            this.feeWithHalfDiscount = new DiscountFee();
            this.feeWithHalfDiscount.EventFee = EventFee * HalfPricePercentage / 100;
            this.feeWithHalfDiscount.AgendaFee = this.feeWithNoDiscount.AgendaFee * HalfPricePercentage / 100;
            this.feeWithHalfDiscount.MerchandiseFee = this.feeWithNoDiscount.MerchandiseFee * HalfPricePercentage / 100;
            this.feeWithHalfDiscount.TotalFee = this.feeWithHalfDiscount.EventFee + this.feeWithHalfDiscount.AgendaFee + this.feeWithHalfDiscount.MerchandiseFee;
            this.feeWithHalfDiscount.TotalSaving = this.feeWithNoDiscount.TotalFee - this.feeWithHalfDiscount.TotalFee;

            this.feeWithFixedAmountDiscount = new DiscountFee();
            this.feeWithFixedAmountDiscount.EventFee = EventFee - FixedAmount;

            // '4' here represents the quantity of agenda items
            this.feeWithFixedAmountDiscount.AgendaFee = this.feeWithNoDiscount.AgendaFee - 4 * FixedAmount;

            this.feeWithFixedAmountDiscount.MerchandiseFee = this.feeWithNoDiscount.MerchandiseFee - FixedAmount * MerchQuantity;
            this.feeWithFixedAmountDiscount.TotalFee = this.feeWithFixedAmountDiscount.EventFee + this.feeWithFixedAmountDiscount.AgendaFee + this.feeWithFixedAmountDiscount.MerchandiseFee;
            this.feeWithFixedAmountDiscount.TotalSaving = this.feeWithNoDiscount.TotalFee - this.feeWithFixedAmountDiscount.TotalFee;

            this.discountFee = new Dictionary<DiscountCodeType, DiscountFee>();
            this.discountFee.Add(DiscountCodeType.Empty, this.feeWithNoDiscount);
            this.discountFee.Add(DiscountCodeType.Enter, this.feeWithAccessCode);
            this.discountFee.Add(DiscountCodeType.Free, this.feeWithFreePrice);
            this.discountFee.Add(DiscountCodeType.Half, this.feeWithHalfDiscount);
            this.discountFee.Add(DiscountCodeType.FixedAmount, this.feeWithFixedAmountDiscount);
        }

        private void InitializeEventSessionIDs()
        {
            this.sessionIDs = new Dictionary<EventType, string>();
            this.sessionIDs.Add(EventType.DiscountCodeFixture, string.Empty);
            this.sessionIDs.Add(EventType.DiscountCodeFixtureWithRegType, string.Empty);
            this.sessionIDs.Add(EventType.DiscountCodeLimitTest, string.Empty);
        }
        #endregion

        #region Test Methods      
        [Test]
        [Category(Priority.Two)]
        [Description("691")]
        public void SameDiscountCode()
        {
            this.eventType = EventType.DiscountCodeFixture;

            this.InitializeEvent(false);
            
            // No Discount
            this.RegWithSameDiscountCode(DiscountCodeType.Empty);

            // Half Discount
            this.RegWithSameDiscountCode(DiscountCodeType.Half);

            // Free Discount
            this.RegWithSameDiscountCode(DiscountCodeType.Free);

            // Fixed Amount Discount
            this.RegWithSameDiscountCode(DiscountCodeType.FixedAmount);

            // Access Code
            this.RegWithSameDiscountCode(DiscountCodeType.Enter);
        }

        // Group registration (3 attendees). Use discount code “Half” for all items
        [Test]
        [Category(Priority.Two)]
        [Description("539")]
        public void GroupRegWithHalfDiscount()
        {
            this.eventType = EventType.DiscountCodeFixture;

            this.InitializeEvent(false);
            
            int groupSize = 3;
            this.GroupRegWithDiscountCode(groupSize);
        }

        // Use different discount codes for all items
        [Test]
        [Category(Priority.Two)]
        [Description("540")]
        public void DifferentDiscountCode()
        {
            this.eventType = EventType.DiscountCodeFixture;

            this.InitializeEvent(false);
            
            this.RegWithDifferentDiscountCode();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("541")]
        public void DiscountCodeLimitTest()
        {
            this.eventType = EventType.DiscountCodeLimitTest;

            this.InitializeEvent(false);
            
            // Add the 1st registration with free discount code
            this.RegWithSameDiscountCode(DiscountCodeType.Free);

            // Add the 2nd registration with free discount code
            this.RegWithSameDiscountCode(DiscountCodeType.Free);

            // Add the 3rd registration with free discount code
            this.RegWithSameDiscountCode(DiscountCodeType.Free);

            // Add the 4th registration with free discount code
            this.RegWithSameDiscountCode(DiscountCodeType.Free);

            // Add the 5th registration with free discount code
            this.RegWithSameDiscountCode(DiscountCodeType.Free);

            // Add the 6th registration with free discount code and use access code when code limit has been reached
            this.RegisterWhenDiscountCodeLimitHasBeenReached(DiscountCodeType.Free);
        }
        #endregion

        #region Private Methods
        [Step]
        private void InitializeEvent(bool withRegTypes)
        {
            this.LoginAndGoToEventTab();
            ManagerSiteMgr.DeleteEventByName(this.eventType.ToString());
            this.CreateEvent(this.eventType, withRegTypes);
            this.eventID = ManagerSiteMgr.GetFirstEventId(this.eventType.ToString());
        }    

        [Step]
        private void LoginAndGoToEventTab()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
        }

        [Step]
        private void CreateEvent(EventType eventType, bool withRegTypes)
        {
            LoginAndGoToEventTab();

            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

            if (withRegTypes)
            {
                this.SetEventStartPageWithRegType(eventType);
            }
            else
            {
                this.SetEventStartPage(eventType);
            }

            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);

            if (!withRegTypes)
            {
                this.SetAgendaPage();
            }

            BuilderMgr.GotoPage(FormDetailManager.Page.Merchandise);

            if (!withRegTypes)
            {
                this.SetMerchandisePage();
            }

            BuilderMgr.Next();
            this.SetCheckoutPage();
            BuilderMgr.Next();
            this.SetConfirmationPage();

            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SaveAndClose();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        private void SetEventStartPage(EventType eventType)
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventType.ToString());
            this.AddFeeAndDiscountCodeOnStartPage(EventFee);
            BuilderMgr.SaveAndStay();
        }

        private void SetEventStartPageWithRegType(EventType eventType)
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventType.ToString());
            BuilderMgr.SaveAndStay();
            this.AddFeeAndDiscountCodeOnStartPage(RegTypeName.RegTypeOne, RegTypeFeeOne);
            BuilderMgr.SaveAndStay();
            this.AddFeeAndDiscountCodeOnStartPage(RegTypeName.RegTypeTwo, RegTypeFeeTwo);
            BuilderMgr.SaveAndStay();
        }

        private void SetAgendaPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.VerifyEventAgendaPage();

            // Add the check-box agenda with discount code
            this.AddAgendaItemWithDiscountCode(
                AgendaItem.CheckBox, 
                AgendaItem.CheckBox.ToString(), 
                AgendaItemPrice);

            // Add the always select agenda with discount code
            this.AddAgendaItemWithDiscountCode(
                AgendaItem.AlwaysSelect, 
                AgendaItem.AlwaysSelect.ToString(), 
                AgendaItemPrice);

            // Add the check-box and required agenda with discount code
            this.AddAgendaItemWithDiscountCode(
                AgendaItem.CheckBoxAndRequire, 
                AgendaItem.CheckBoxAndRequire.ToString(), 
                AgendaItemPrice);

            // Add the multiple choice radio button agenda with discount code
            this.AddAgendaItemWithDiscountCode(
                AgendaItem.MultipleChoiceRadioButton, 
                AgendaItem.MultipleChoiceRadioButton.ToString(), 
                AgendaItemPrice);
        }

        private void SetMerchandisePage()
        {
            this.AddMerchandiseItem(MerchandiseName.MerchandiseWithFixedPrice, MerchPrice);
        }
        
        private void SetCheckoutPage()
        {
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndStay();
        }

        private void SetConfirmationPage()
        {
            BuilderMgr.SetEventConfirmationPage();
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventConfirmationPage();
        }

        private void AddAgendaItemWithDiscountCode(AgendaItem agendaType, string agendaName, double price)
        {
            switch (agendaType)
            {
                case AgendaItem.CheckBox:
                    BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.CheckBox, agendaName, price);
                    break;

                case AgendaItem.AlwaysSelect:
                    BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.AlwaysSelected, agendaName, price);
                    break;

                case AgendaItem.CheckBoxAndRequire:
                    BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.CheckBox, agendaName, price);
                    BuilderMgr.AGMgr.FeeMgr.ExpandOption();
                    BuilderMgr.AGMgr.FeeMgr.DC.SetCodeRequired(true);
                    break;

                case AgendaItem.MultipleChoiceRadioButton:
                    BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.RadioButton, agendaName, price);
                    UIUtilityProvider.UIHelper.RefreshPage();
                    
                    int order = BuilderMgr.AGMgr.GetLastAgendaItemOrder();
                    BuilderMgr.AGMgr.OpenAgendaInListByOrder(order);

                    BuilderMgr.AGMgr.ClickDeleteRadioButtonAllItems();                    
                    BuilderMgr.AGMgr.ClickOkOnDeleteRadioButtonAllMultipleChoiceItemsConfirmation();
                    
                    BuilderMgr.AGMgr.ClickAddMultiChoiceItem();
                    
                    BuilderMgr.AGMgr.MultiChoiceItemMgr.SetMultiChoiceItem(
                        AgendaMultiChoiceItemName.MCRBMC1.ToString(), 
                        AgendaMultiChoiceItemPrice);

                    BuilderMgr.AGMgr.MultiChoiceItemMgr.SaveAndNew();

                    BuilderMgr.AGMgr.MultiChoiceItemMgr.SetMultiChoiceItem(
                        AgendaMultiChoiceItemName.MCRBMC2.ToString(), 
                        AgendaMultiChoiceItemPrice);

                    BuilderMgr.AGMgr.MultiChoiceItemMgr.SaveAndClose();
                    BuilderMgr.AGMgr.ClickSaveItem();

                    UIUtilityProvider.UIHelper.RefreshPage();
                    BuilderMgr.AGMgr.OpenAgendaInListByOrder(order);
                    break;

                default:
                    break;
            }

            this.AddDiscountCodeOnAgendaPage();
            BuilderMgr.AGMgr.ClickSaveItem();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        private void AddMerchandiseItem(MerchandiseName merchName, double price)
        {
            if (merchName == MerchandiseName.MerchandiseWithFixedPrice)
            {
                // Add merchandise item
                BuilderMgr.AddMerchandiseItemWithFeeAmount(
                    global::RegOnline.RegressionTest.Managers.Builder.MerchandiseManager.MerchandiseType.Fixed,
                    merchName.ToString(), 
                    price, 
                    null, 
                    null);

                // Add discount code
                BuilderMgr.OpenMerchandiseItem(merchName.ToString());
                BuilderMgr.MerchMgr.ExpandAdvanced();
                BuilderMgr.MerchMgr.DiscountCodeMgr.SetMerchandiseDiscountCode(MerchDiscountCodeString);
            }

            BuilderMgr.MerchMgr.SaveAndClose();
        }

        private void AddFeeAndDiscountCodeOnStartPage(double fee)
        {
            BuilderMgr.SetEventFee(fee);
            BuilderMgr.ClickEventFeeAdvanced();
            BuilderMgr.EventFeeMgr.ExpandOption();
            BuilderMgr.EventFeeMgr.DC.ClickAddCode();

            // Add half discount code
            BuilderMgr.EventFeeMgr.DC.SetDiscountCodeAndAddAnother(
                StringEnum.GetStringValue(DiscountCodeType.Half),
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangePriceDirection.Decrease, 
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangeType.Percent, 
                HalfPricePercentage, 
                null);

            // Add fixed amount discount code
            BuilderMgr.EventFeeMgr.DC.SetDiscountCodeAndAddAnother(
                StringEnum.GetStringValue(DiscountCodeType.FixedAmount),
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangePriceDirection.Decrease, 
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangeType.FixedAmount, 
                FixedAmount, 
                null);

            // Add access code
            BuilderMgr.EventFeeMgr.DC.SetAccessCodeAndAddAnother(StringEnum.GetStringValue(DiscountCodeType.Enter), null);

            // Add free discount code
            BuilderMgr.EventFeeMgr.DC.SetDiscountCodeValues(
                StringEnum.GetStringValue(DiscountCodeType.Free),
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangePriceDirection.Decrease, 
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangeType.Percent, 
                FreePricePercentage, 
                DiscountCodeLimit);

            BuilderMgr.EventFeeMgr.DC.SaveAndCloseDiscountCode();

            BuilderMgr.EventFeeMgr.SaveAndClose();
        }

        // Add discount code in reg type fee advanced dialog
        private void AddFeeAndDiscountCodeOnStartPage(RegTypeName regTypeName, double fee)
        {
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(regTypeName.ToString());
            BuilderMgr.RegTypeMgr.ExpandAdvancedSection();
            BuilderMgr.RegTypeMgr.SetFee(fee);
            BuilderMgr.RegTypeMgr.ClickFeeAdvancedLink();
            BuilderMgr.RegTypeMgr.FeeMgr.ExpandOption();
            BuilderMgr.RegTypeMgr.FeeMgr.DC.ClickAddCode();

            BuilderMgr.RegTypeMgr.FeeMgr.DC.SetDiscountCodeAndAddAnother(
                StringEnum.GetStringValue(DiscountCodeType.Half),
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangePriceDirection.Decrease, 
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangeType.Percent, 
                HalfPricePercentage, 
                null);

            BuilderMgr.RegTypeMgr.FeeMgr.DC.SetDiscountCodeAndAddAnother(
                StringEnum.GetStringValue(DiscountCodeType.FixedAmount),
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangePriceDirection.Decrease, 
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangeType.FixedAmount, 
                FixedAmount, 
                null);

            BuilderMgr.RegTypeMgr.FeeMgr.DC.SetAccessCodeAndAddAnother(
                StringEnum.GetStringValue(DiscountCodeType.Enter), 
                null);

            BuilderMgr.RegTypeMgr.FeeMgr.DC.SetDiscountCodeValues(
                StringEnum.GetStringValue(DiscountCodeType.Free),
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangePriceDirection.Decrease, 
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangeType.Percent, 
                FreePricePercentage, 
                DiscountCodeLimit);

            BuilderMgr.RegTypeMgr.FeeMgr.DC.SaveAndCloseDiscountCode();
            BuilderMgr.RegTypeMgr.FeeMgr.SaveAndClose();
            BuilderMgr.RegTypeMgr.SaveAndClose();
        }

        private void AddDiscountCodeOnAgendaPage()
        {
            BuilderMgr.AGMgr.FeeMgr.ExpandOption();
            BuilderMgr.AGMgr.FeeMgr.DC.ClickAddCode();

            BuilderMgr.AGMgr.FeeMgr.DC.SetDiscountCodeAndAddAnother(
                StringEnum.GetStringValue(DiscountCodeType.Half),
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangePriceDirection.Decrease, 
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangeType.Percent, 
                HalfPricePercentage, 
                null);

            BuilderMgr.AGMgr.FeeMgr.DC.SetDiscountCodeAndAddAnother(
                StringEnum.GetStringValue(DiscountCodeType.FixedAmount),
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangePriceDirection.Decrease, 
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangeType.FixedAmount, 
                FixedAmount, 
                null);

            BuilderMgr.AGMgr.FeeMgr.DC.SetAccessCodeAndAddAnother(
                StringEnum.GetStringValue(DiscountCodeType.Enter), 
                null);

            BuilderMgr.AGMgr.FeeMgr.DC.SetDiscountCodeValues(
                StringEnum.GetStringValue(DiscountCodeType.Free),
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangePriceDirection.Decrease, 
                RegOnline.RegressionTest.Managers.Builder.DiscountCodeManager.ChangeType.Percent, 
                FreePricePercentage, 
                DiscountCodeLimit);

            BuilderMgr.AGMgr.FeeMgr.DC.SaveAndCloseDiscountCode();
        }

        #region Registration
        // Register with same discount code
        [Step]
        private void RegWithSameDiscountCode(DiscountCodeType codeType)
        {
            this.InitializeRegistration();
            this.EnterRegDiscountCode(codeType);
            this.VerifyCheckoutAndPay(this.discountFee[codeType].TotalFee);
            this.VerifySaving(this.discountFee[codeType].TotalSaving);
            this.FinishAndConfirmRegistration();
        }

        // Register with different discount code
        [Step]
        private void RegWithDifferentDiscountCode()
        {
            double eventFee = FreePriceFee;
            double agendaTotalToVerify = FreePriceFee;
            double merchTotalToVerify = FreePriceFee;
            double totalToVerify = FreePriceFee;
            double totalSaving = FreePriceFee;

            this.InitializeRegistration();

            this.EnterCheckinInfoAndContinue(DiscountCodeType.Half);
            eventFee += this.discountFee[DiscountCodeType.Half].EventFee;
            this.EnterProfileInfoAndContinue();
            this.EnterDifferentAgendaDiscountInfo(ref agendaTotalToVerify);
            this.VerifyAgendaTotalFeeAndContinue(agendaTotalToVerify);
            this.EnterDifferentMerchandiseDiscountInfo(ref merchTotalToVerify);
            this.VerifyMerchandiseTotalFeeAndContinue(merchTotalToVerify);

            totalToVerify = eventFee + agendaTotalToVerify + merchTotalToVerify;
            totalSaving = this.discountFee[DiscountCodeType.Empty].TotalFee - totalToVerify;

            this.VerifySaving(totalSaving);
            this.VerifyCheckoutAndPay(totalToVerify);
            this.FinishAndConfirmRegistration();
        }

        // Group Register With Half Discount Code 
        private void GroupRegWithDiscountCode(int groupSize)
        {
            string primaryEmail = string.Empty;

            // Initialize registration
            this.InitializeRegistration();

            // Add the frist attendee
            this.EnterRegDiscountCode(DiscountCodeType.Half);
            primaryEmail = RegisterMgr.CurrentEmail;

            // Add the rest attendees into current group
            for (int count = 0; count < groupSize - 1; count++)
            {
                RegisterMgr.ClickAddAnotherPerson();
                this.EnterGroupRegDiscountCode(DiscountCodeType.Half);
            }

            RegisterMgr.CurrentEmail = primaryEmail;

            double groupTotalFeeWithNoDiscount = 
                (this.discountFee[DiscountCodeType.Empty].EventFee + this.discountFee[DiscountCodeType.Empty].AgendaFee) * groupSize
                + this.discountFee[DiscountCodeType.Empty].MerchandiseFee * MerchQuantity;

            double groupTotalFeeWithHalfDiscount = 
                (this.discountFee[DiscountCodeType.Half].EventFee + this.discountFee[DiscountCodeType.Half].AgendaFee) * groupSize
                + this.discountFee[DiscountCodeType.Half].MerchandiseFee * MerchQuantity;

            double groupSaving = groupTotalFeeWithNoDiscount - groupTotalFeeWithHalfDiscount;

            // Verify
            this.VerifyCheckoutAndPay(groupTotalFeeWithHalfDiscount);
            this.VerifySaving(groupSaving);
            RegisterMgr.PayMoney(RegisterManager.PaymentMethod.Check);
            this.FinishAndConfirmRegistration();
        }

        // Group Register With different Event Regtype and Different Discount Code
        [Step]
        private void GroupRegWithRegType()
        {
            string primaryEmail = string.Empty;
            double groupTotalFee = FreePriceFee;
            double groupTotalSaving = FreePriceFee;

            // Initialize registration
            this.InitializeRegistration();

            // The frist registration: regtype1 and free discount code
            this.RegWithDiscountCodeAndRegType(RegTypeName.RegTypeOne, DiscountCodeType.Free, ref groupTotalFee, ref groupTotalSaving);
            primaryEmail = RegisterMgr.CurrentEmail;

            // Add the second registration: regtype2 and enter discount code
            RegisterMgr.ClickAddAnotherPerson();
            this.RegWithDiscountCodeAndRegType(RegTypeName.RegTypeTwo, DiscountCodeType.Enter, ref groupTotalFee, ref groupTotalSaving);

            // Add the third registration: regtype1 and quarter discount code
            RegisterMgr.ClickAddAnotherPerson();
            this.RegWithDiscountCodeAndRegType(RegTypeName.RegTypeOne, DiscountCodeType.FixedAmount, ref groupTotalFee, ref groupTotalSaving);

            // Add the fourth registration: regtype2 and half discount code
            RegisterMgr.ClickAddAnotherPerson();
            this.RegWithDiscountCodeAndRegType(RegTypeName.RegTypeTwo, DiscountCodeType.Half, ref groupTotalFee, ref groupTotalSaving);

            // Add the fifth registration: regtype1 and no discount code
            RegisterMgr.ClickAddAnotherPerson();
            this.RegWithDiscountCodeAndRegType(RegTypeName.RegTypeOne, DiscountCodeType.Empty, ref groupTotalFee, ref groupTotalSaving);

            RegisterMgr.CurrentEmail = primaryEmail;

            // Pay money and verify
            this.VerifyCheckoutAndPay(groupTotalFee);
            this.VerifySaving(groupTotalSaving);
            RegisterMgr.PayMoney(RegisterManager.PaymentMethod.Check);
            this.FinishAndConfirmRegistration();
        }

        private void RegWithDiscountCodeAndRegType(RegTypeName regTypeName, DiscountCodeType codeType, ref double groupTotalFee, ref double groupTotalSaving)
        {
            // Select reg type
            RegisterMgr.SelectRegType(regTypeName.ToString());

            this.EnterCheckinInfoAndContinue(codeType);
            this.EnterProfileInfoAndContinue();

            double totalFee = FreePriceFee;
            double totalSaving = FreePriceFee;

            this.GetRegTypeDiscountFee(regTypeName, codeType, ref totalFee, ref totalSaving);

            groupTotalFee += totalFee;
            groupTotalSaving += totalSaving;
        }

        // Register when discount code limit has been reached
        [Step]
        private void RegisterWhenDiscountCodeLimitHasBeenReached(DiscountCodeType codeType)
        {
            this.InitializeRegistration();
            this.EnterCheckinInfoAndContinue(DiscountCodeType.Free);

            string checkinErrorMessage = string.Format(
                "No more \"{0}\" codes are being accepted for this event. Enter another code or leave the field blank.", 
                StringEnum.GetStringValue(codeType));

            RegisterMgr.VerifyErrorMessage(checkinErrorMessage);
            RegisterMgr.EnterEventDiscoutCode(StringEnum.GetStringValue(DiscountCodeType.Enter));
            RegisterMgr.Continue();

            this.EnterProfileInfoAndContinue();

            // Select all agenda items
            this.EnterAgendaDiscountInfo(codeType);

            string agendaErrorMessage = 
                "The limit for the discount code you entered has been reached and is no longer being accepted.";

            // Verify error messages
            string[] agendaErrorMessages = new string[]
            {
                agendaErrorMessage,
                agendaErrorMessage,
                agendaErrorMessage,
                agendaErrorMessage
            };

            RegisterMgr.ClickRecalculateTotal();
            RegisterMgr.VerifyErrorMessage(agendaErrorMessages);

            // Enter agenda discount code
            RegisterMgr.EnterAgendaItemDiscountCode(
                this.agendaItemIDs[AgendaItem.CheckBox.ToString()], 
                StringEnum.GetStringValue(DiscountCodeType.Enter));

            RegisterMgr.EnterAgendaItemDiscountCode(
                this.agendaItemIDs[AgendaItem.AlwaysSelect.ToString()], 
                StringEnum.GetStringValue(DiscountCodeType.Enter));

            RegisterMgr.EnterAgendaItemDiscountCode(
                this.agendaItemIDs[AgendaItem.CheckBoxAndRequire.ToString()], 
                StringEnum.GetStringValue(DiscountCodeType.Enter));

            RegisterMgr.EnterAgendaItemDiscountCode(
                this.agendaItemIDs[AgendaItem.MultipleChoiceRadioButton.ToString()], 
                StringEnum.GetStringValue(DiscountCodeType.Enter));

            // Verify agenda total amount
            RegisterMgr.ClickRecalculateTotal();
            RegisterMgr.VerifyAgendaPageTotalAmount(this.discountFee[DiscountCodeType.Enter].AgendaFee);
            RegisterMgr.Continue();

            // Select merchandise
            this.EnterMerchandiseDiscountInfo(codeType);

            // Verify merchandise total amount
            RegisterMgr.VerifyMerchandisePageTotal(this.discountFee[codeType].MerchandiseFee);
            RegisterMgr.Continue();

            // Verify total amount and saving amount
            this.VerifyCheckoutAndPay(
                this.discountFee[DiscountCodeType.Enter].TotalFee - this.discountFee[DiscountCodeType.Enter].MerchandiseFee);

            this.VerifySaving(this.discountFee[DiscountCodeType.Enter].MerchandiseFee);

            this.FinishAndConfirmRegistration();
        }

        private void InitializeRegistration()
        {
            RegisterMgr.CurrentEventId = this.eventID;
            this.GetAgendaItems();
            RegisterMgr.OpenRegisterPage(this.eventID);
        }

        private void FinishAndConfirmRegistration()
        {
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void EnterRegDiscountCode(DiscountCodeType codeType)
        {
            this.EnterCheckinInfoAndContinue(codeType);
            this.EnterProfileInfoAndContinue();
            this.EnterAgendaDiscountInfo(codeType);
            this.VerifyAgendaTotalFeeAndContinue(this.discountFee[codeType].AgendaFee);
            this.EnterMerchandiseDiscountInfo(codeType);
            this.VerifyMerchandiseTotalFeeAndContinue(this.discountFee[codeType].MerchandiseFee);
        }

        // Added this method as after you add another person the disc code field on merch is disabled
        // In selenium 1 it would just try and type in a disabled field, this one won't... 
        private void EnterGroupRegDiscountCode(DiscountCodeType codeType)
        {
            this.EnterCheckinInfoAndContinue(codeType);
            this.EnterProfileInfoAndContinue();
            this.EnterAgendaDiscountInfo(codeType);
            this.VerifyAgendaTotalFeeAndContinue(this.discountFee[codeType].AgendaFee);
            this.VerifyMerchandiseTotalFeeAndContinue(this.discountFee[codeType].MerchandiseFee);
        }

        private void EnterCheckinInfoAndContinue(DiscountCodeType codeType)
        {
            RegisterMgr.Checkin();
            RegisterMgr.EnterEventDiscoutCode(StringEnum.GetStringValue(codeType));
            RegisterMgr.Continue();
        }

        private void EnterProfileInfoAndContinue()
        {
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
        }

        private void EnterAgendaDiscountInfo(DiscountCodeType codeType)
        {
            string code = StringEnum.GetStringValue(codeType);

            RegisterMgr.SelectAgendaItems();
            RegisterMgr.EnterAgendaItemDiscountCode(this.agendaItemIDs[AgendaItem.CheckBox.ToString()], code);
            RegisterMgr.EnterAgendaItemDiscountCode(this.agendaItemIDs[AgendaItem.AlwaysSelect.ToString()], code);

            if (codeType == DiscountCodeType.Empty)
            {
                RegisterMgr.EnterAgendaItemDiscountCode(
                    this.agendaItemIDs[AgendaItem.CheckBoxAndRequire.ToString()], 
                    StringEnum.GetStringValue(DiscountCodeType.Enter));
            }
            else
            {
                RegisterMgr.EnterAgendaItemDiscountCode(this.agendaItemIDs[AgendaItem.CheckBoxAndRequire.ToString()], code);
            }

            RegisterMgr.EnterAgendaItemDiscountCode(this.agendaItemIDs[AgendaItem.MultipleChoiceRadioButton.ToString()], code);
        }

        private void EnterDifferentAgendaDiscountInfo(ref double agendaTotalFee)
        {
            agendaTotalFee = FreePriceFee;

            RegisterMgr.SelectAgendaItems();

            RegisterMgr.EnterAgendaItemDiscountCode(
                this.agendaItemIDs[AgendaItem.CheckBox.ToString()], 
                StringEnum.GetStringValue(DiscountCodeType.FixedAmount));

            agendaTotalFee += AgendaItemPrice - FixedAmount;

            RegisterMgr.EnterAgendaItemDiscountCode(
                this.agendaItemIDs[AgendaItem.AlwaysSelect.ToString()], 
                StringEnum.GetStringValue(DiscountCodeType.Enter));

            agendaTotalFee += AgendaItemPrice;

            RegisterMgr.EnterAgendaItemDiscountCode(
                this.agendaItemIDs[AgendaItem.CheckBoxAndRequire.ToString()], 
                StringEnum.GetStringValue(DiscountCodeType.Free));
            
            RegisterMgr.EnterAgendaItemDiscountCode(
                this.agendaItemIDs[AgendaItem.MultipleChoiceRadioButton.ToString()], 
                StringEnum.GetStringValue(DiscountCodeType.Half));

            agendaTotalFee += AgendaMultiChoiceItemPrice * HalfPricePercentage / 100;
        }

        private void VerifyAgendaTotalFeeAndContinue(double fee)
        {
            RegisterMgr.ClickRecalculateTotal();
            RegisterMgr.VerifyAgendaPageTotalAmount(fee);
            RegisterMgr.Continue();
        }

        private void EnterMerchandiseDiscountInfo(DiscountCodeType codeType)
        {
            RegisterMgr.SelectMerchandiseQuantityByName(MerchandiseName.MerchandiseWithFixedPrice.ToString(), MerchQuantity);
            RegisterMgr.EnterMerchandiseDiscountCodeByName(MerchandiseName.MerchandiseWithFixedPrice.ToString(), StringEnum.GetStringValue(codeType));
        }

        private void EnterDifferentMerchandiseDiscountInfo(ref double merchTotalFee)
        {
            merchTotalFee = FreePriceFee;

            RegisterMgr.SelectMerchandiseQuantityByName(MerchandiseName.MerchandiseWithFixedPrice.ToString(), MerchQuantity);
            RegisterMgr.EnterMerchandiseDiscountCodeByName(MerchandiseName.MerchandiseWithFixedPrice.ToString(), StringEnum.GetStringValue(DiscountCodeType.FixedAmount));
            merchTotalFee += MerchPrice - FixedAmount;
        }

        private void VerifyMerchandiseTotalFeeAndContinue(double fee)
        {
            RegisterMgr.VerifyMerchandisePageTotal(fee);
            RegisterMgr.Continue();
        }

        private void VerifySaving(double totalSaving)
        {
            if (totalSaving > FreePriceFee)
            {
                RegisterMgr.VerifyCheckoutSaving(totalSaving);
            }
        }

        private void VerifyCheckoutAndPay(double totalToVerify)
        {
            if (totalToVerify == FreePriceFee)
            {
                RegisterMgr.VerifyCheckoutTotal(totalToVerify);
            }
            else
            {
                RegisterMgr.PayMoneyAndVerify(totalToVerify, RegisterManager.PaymentMethod.Check);
            }
        }
        #endregion

        #endregion

        #region Helper methods
        private void GetAgendaItems()
        {
            List<Custom_Field> agendaItems = RegisterMgr.DataTool.GetAgendaItems(RegisterMgr.CurrentEventId);

            foreach (Custom_Field agendaItem in agendaItems)
            {
                this.AddToCollection(this.agendaItemIDs, agendaItem.Description, agendaItem.Id);
            }
        }

        private void AddToCollection(Dictionary<string, int> cfIDs, string agendaItemName, int agendaItemID)
        {
            if (cfIDs.ContainsKey(agendaItemName))
            {
                cfIDs[agendaItemName] = agendaItemID;
            }
            else
            {
                cfIDs.Add(agendaItemName, agendaItemID);
            }
        }

        private void GetRegTypeDiscountFee(RegTypeName regTypeName, DiscountCodeType codeType, ref double totalFee, ref double totalSaving)
        {
            double standardTotalFee = FreePriceFee;

            switch (regTypeName)
            {
                case RegTypeName.RegTypeOne:
                    standardTotalFee = RegTypeFeeOne;
                    break;
                case RegTypeName.RegTypeTwo:
                    standardTotalFee = RegTypeFeeTwo;
                    break;
                default:
                    break;
            }

            switch (codeType)
            {
                case DiscountCodeType.Empty:
                case DiscountCodeType.Enter:
                    totalFee = standardTotalFee;
                    totalSaving = FreePriceFee;
                    break;

                case DiscountCodeType.Half:
                    totalFee = standardTotalFee * HalfPricePercentage / 100;
                    totalSaving = standardTotalFee - totalFee;
                    break;

                case DiscountCodeType.Free:
                    totalFee = FreePriceFee;
                    totalSaving = standardTotalFee - totalFee;
                    break;

                case DiscountCodeType.FixedAmount:
                    totalFee = standardTotalFee - FixedAmount;
                    totalSaving = standardTotalFee - totalFee;
                    break;
            }
        }
        #endregion
    }
}
