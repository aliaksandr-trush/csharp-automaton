namespace RegOnline.RegressionTest.Fixtures.FeeOptions
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class EventFeeFixture : FixtureBase
    {
        private const double EventFee = 10.24;
        private const string EventFeeSuffix = "Event Fee ";
        private const string RegTypeFeeSuffix = " Event Fee ";
        private const double RegTypeFee = 20;

        // Code limit for the DiscountCode and AccessCode
        private const string DiscountCode = "123";
        private const string AccessCode = "321";
        private const double DiscountPercentage = 50.0;

        private enum EventName
        {
            EventFeeFixture,
            RegTypeFeeFixture
        }

        private enum TypeOfRegType
        {
            RegTypeWithFee,
            RegTypeWithoutFee,
            
            // DC represent the DiscountCode, and AC represent the AccessCode
            RegTypeWithFeeAndDCAndAC
        }

        private int eventID = ManagerBase.InvalidId;

        [Test]
        [Category(Priority.Two)]
        [Description("425")]
        public void EventFeeTest()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(EventName.EventFeeFixture.ToString());
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

            // Event start page
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName.EventFeeFixture.ToString());
            BuilderMgr.SetEventFee(EventFee);
            BuilderMgr.ClickEventFeeAdvanced();
            BuilderMgr.EventFeeMgr.ExpandOption();

            // Add discount and access codes
            BuilderMgr.EventFeeMgr.DC.ClickAddCode();

            BuilderMgr.EventFeeMgr.DC.SetDiscountCodeValues(
                DiscountCode, 
                DiscountCodeManager.ChangePriceDirection.Decrease, 
                DiscountCodeManager.ChangeType.Percent, 
                DiscountPercentage, 
                null);

            ////BuilderMgr.EventFeeMgr.DC.SaveAndNewDiscountCode();
            BuilderMgr.EventFeeMgr.DC.SaveAndCloseDiscountCode();
            BuilderMgr.EventFeeMgr.DC.ClickAddCode();
            BuilderMgr.EventFeeMgr.DC.SetAccessCodeValues(AccessCode, null);
            BuilderMgr.EventFeeMgr.DC.SaveAndCloseDiscountCode();

            BuilderMgr.EventFeeMgr.SaveAndClose();
            BuilderMgr.SaveAndStay();

            // Event checkout and confirmation pages
            BuilderMgr.GotoPage(RegOnline.RegressionTest.Managers.Builder.FormDetailManager.Page.Checkout);
            this.SetCheckoutPage();

            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SaveAndClose();

            // Start registering
            this.Register(EventName.EventFeeFixture, null, EventFee, null, null);

            this.Register(
                EventName.EventFeeFixture,
                null, 
                EventFee * DiscountPercentage / 100, 
                DiscountCodeManager.DiscountCodeType.DiscountCode, 
                DiscountCode);

            this.Register(
                EventName.EventFeeFixture,
                null, 
                EventFee,
                DiscountCodeManager.DiscountCodeType.AccessCode, 
                AccessCode);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("426")]
        public void RegTypeFeeTest()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(EventName.RegTypeFeeFixture.ToString());
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, EventName.RegTypeFeeFixture.ToString());

            this.CreateRegType(TypeOfRegType.RegTypeWithFee, RegTypeFee);
            this.CreateRegType(TypeOfRegType.RegTypeWithoutFee, RegTypeFee);
            this.CreateRegType(TypeOfRegType.RegTypeWithFeeAndDCAndAC, RegTypeFee);

            BuilderMgr.GotoPage(RegOnline.RegressionTest.Managers.Builder.FormDetailManager.Page.Checkout);
            this.SetCheckoutPage();

            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SaveAndClose();

            this.Register(EventName.RegTypeFeeFixture, TypeOfRegType.RegTypeWithFee, RegTypeFee, null, null);

            this.Register(EventName.RegTypeFeeFixture, TypeOfRegType.RegTypeWithoutFee, null, null, null);

            this.Register(
                EventName.RegTypeFeeFixture,
                TypeOfRegType.RegTypeWithFeeAndDCAndAC, 
                RegTypeFee * DiscountPercentage / 100,
                DiscountCodeManager.DiscountCodeType.DiscountCode, 
                DiscountCode);

            this.Register(
                EventName.RegTypeFeeFixture,
                TypeOfRegType.RegTypeWithFeeAndDCAndAC, 
                RegTypeFee,
                DiscountCodeManager.DiscountCodeType.AccessCode, 
                AccessCode);
        }

        [Step]
        private void CreateRegType(TypeOfRegType type, double? fee)
        {
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(type.ToString());
            BuilderMgr.RegTypeMgr.ExpandAdvancedSection();
            BuilderMgr.RegTypeMgr.SetFee(fee);
            BuilderMgr.RegTypeMgr.VerifyFee(fee);

            switch (type)
            {
                // Delete the event fee for the second RegType and verify it
                case TypeOfRegType.RegTypeWithoutFee:
                    BuilderMgr.RegTypeMgr.SetFee(null);
                    BuilderMgr.RegTypeMgr.VerifyFee(null);
                    break;

                // Create a DiscountCode and a AccessCode for the third RegType
                case TypeOfRegType.RegTypeWithFeeAndDCAndAC:
                    BuilderMgr.RegTypeMgr.ClickFeeAdvancedLink();
                    BuilderMgr.RegTypeMgr.FeeMgr.ExpandOption();
                    BuilderMgr.RegTypeMgr.FeeMgr.DC.ClickAddCode();

                    BuilderMgr.RegTypeMgr.FeeMgr.DC.SetDiscountCodeValues(
                        DiscountCode, 
                        DiscountCodeManager.ChangePriceDirection.Decrease,
                        DiscountCodeManager.ChangeType.Percent,
                        DiscountPercentage, 
                        null);

                    ////BuilderMgr.RegTypeMgr.FeeMgr.DC.SaveAndNewDiscountCode();
                    BuilderMgr.RegTypeMgr.FeeMgr.DC.SaveAndCloseDiscountCode();
                    BuilderMgr.RegTypeMgr.FeeMgr.DC.ClickAddCode();
                    BuilderMgr.RegTypeMgr.FeeMgr.DC.SetAccessCodeValues(AccessCode, null);
                    BuilderMgr.RegTypeMgr.FeeMgr.DC.SaveAndCloseDiscountCode();
                    BuilderMgr.RegTypeMgr.FeeMgr.SaveAndClose();
                    break;

                default:
                    break;
            }

            BuilderMgr.RegTypeMgr.SaveAndClose();
        }

        [Step]
        private void SetCheckoutPage()
        {
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndStay();
        }

        [Step]
        private void Register(
            EventName eventType,
            TypeOfRegType? regType, 
            double? feeNeedToPay, 
            DiscountCodeManager.DiscountCodeType? codeType, 
            string code)
        {
            string registrantType = string.Empty;

            if (eventType == EventName.RegTypeFeeFixture)
            {
                registrantType = regType.ToString();
            }

            this.StartRegAndCheckin();

            if (eventType == EventName.RegTypeFeeFixture)
            {
                RegisterMgr.SelectRegType(regType.Value.ToString());
            }

            if (!string.IsNullOrEmpty(code))
            {
                RegisterMgr.EnterEventDiscoutCode(code);
            }

            RegisterMgr.Continue();

            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            RegisterManager.FeeResponse[] expectedFees = null;

            if (feeNeedToPay.HasValue)
            {
                int feeQuantity = 1;

                // Set the expected fees
                expectedFees = new RegisterManager.FeeResponse[feeQuantity];

                expectedFees[0] = new RegisterManager.FeeResponse();

                switch (eventType)
                {
                    case EventName.EventFeeFixture:
                        expectedFees[0].FeeName = registrantType + EventFeeSuffix;
                        break;
                    case EventName.RegTypeFeeFixture:
                        expectedFees[0].FeeName = registrantType + RegTypeFeeSuffix;
                        break;
                    default:
                        break;
                }

                expectedFees[0].FeeQuantity = feeQuantity.ToString();

                string discountPrice = this.GetDiscountPrice(eventType);
                string fullPrice = this.GetFullPrice(eventType);

                expectedFees[0].FeeUnitPrice = fullPrice;

                if (codeType.HasValue && codeType == DiscountCodeManager.DiscountCodeType.DiscountCode)
                {
                    expectedFees[0].FeeUnitPrice = discountPrice;
                }

                expectedFees[0].FeeAmount = expectedFees[0].FeeUnitPrice;

                // Pay and verify, then finish the registration and confirm it, then verify the fee again
                this.PayAndVerify(feeNeedToPay.Value, expectedFees);
            }

            this.FinishRegAndConfirm();

            if (feeNeedToPay.HasValue)
            {
                RegisterMgr.VerifyRegistrationFees(expectedFees);
            }
        }

        private void StartRegAndCheckin()
        {
            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin();
        }

        private void PayAndVerify(double feeNeedToPay, RegisterManager.FeeResponse[] expectedFees)
        {
            RegisterMgr.VerifyRegistrationFees(expectedFees);
            RegisterMgr.PayMoneyAndVerify(feeNeedToPay, RegisterManager.PaymentMethod.Check);
        }

        private void FinishRegAndConfirm()
        {
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private string GetDiscountPrice(EventName eventType)
        {
            switch (eventType)
            {
                case EventName.EventFeeFixture:
                    return MoneyTool.FormatMoney(EventFee * DiscountPercentage / 100);

                case EventName.RegTypeFeeFixture:
                    return MoneyTool.FormatMoney(RegTypeFee * DiscountPercentage / 100);

                default:
                    throw new ArgumentException("Invalid event type");
            }
        }

        private string GetFullPrice(EventName eventType)
        {
            switch (eventType)
            {
                case EventName.EventFeeFixture:
                    return MoneyTool.FormatMoney(EventFee);

                case EventName.RegTypeFeeFixture:
                    return MoneyTool.FormatMoney(RegTypeFee);

                default:
                    throw new ArgumentException("Invalid event type");
            }
        }
    }
}
