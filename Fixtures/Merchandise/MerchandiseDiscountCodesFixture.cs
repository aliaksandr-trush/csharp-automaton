namespace RegOnline.RegressionTest.Fixtures.Merchandise
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class MerchandiseDiscountCodesFixture : FixtureBase
    {
        private const int InvalidEventID = -1;
        private const string EventName = "MerchandiseDiscountCodeFixture";
        private const double EventFee = 10;

        private const string MerchName = "MCHWithDisCode";
        private const double MerchItemPrice = 50;
        private const double MerchItemMinPrice = 25;
        private const int MerchQuantity = 2;

        private const string DiscountCodeString = "Half=-50%,FixedAmount=-25,Free=-100%,Access";
        private const double HalfDiscountPercentage = 50;
        private const double FixedAmount = 25;
        private const double FreePrice = 0;

        private int eventID = InvalidEventID;
        private Dictionary<DiscountCodeTypes, double> merchDiscountFee;

        private enum DiscountCodeTypes
        {
            Free,
            FixedAmount,
            Half,
            Access
        }

        public MerchandiseDiscountCodesFixture()
        {
            this.merchDiscountFee = new Dictionary<DiscountCodeTypes, double>();
            this.merchDiscountFee.Add(DiscountCodeTypes.Half, MerchItemPrice * MerchQuantity * HalfDiscountPercentage / 100);
            this.merchDiscountFee.Add(DiscountCodeTypes.Free, FreePrice * MerchQuantity);
            this.merchDiscountFee.Add(DiscountCodeTypes.FixedAmount, (MerchItemPrice - FixedAmount) * MerchQuantity);
            this.merchDiscountFee.Add(DiscountCodeTypes.Access, MerchItemPrice * MerchQuantity);
        }

        #region Test methods
        [Test]
        [Category(Priority.Two)]
        [Description("443")]
        public void MerchandiseDiscountCodeFixture()
        {
            this.CreateEvent();
            this.RegisterWithMerchandiseDiscountCode();
        }
        #endregion

        #region Create event helper methods
        [Step]
        private void CreateEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName, 0);

            if (ManagerSiteMgr.EventExists(EventName))
            {
                this.eventID = ManagerSiteMgr.GetFirstEventId(EventName);
            }
            else if (this.eventID == InvalidEventID)
            {
                ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
                this.SetEventStartPage();
                BuilderMgr.GotoPage(FormDetailManager.Page.Merchandise);
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

        private void SetMerchandisePage()
        {
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Fixed, MerchName, MerchItemPrice, null, null);
            BuilderMgr.OpenMerchandiseItem(MerchName);

            // Change merchandise type to Section Header and verify that the discount code is not available 
            BuilderMgr.MerchMgr.SetType(MerchandiseManager.MerchandiseType.Header);
            BuilderMgr.MerchMgr.DiscountCodeMgr.VerifyMerchandiseDiscountCodeAvailable(false);

            // Change merchandise type to Variable Amount and verify the error message
            BuilderMgr.MerchMgr.SetType(MerchandiseManager.MerchandiseType.Variable);
            BuilderMgr.MerchMgr.SetMerchItemPrice(MerchandiseManager.MerchandiseType.Variable, null, MerchItemMinPrice, MerchItemPrice);
            BuilderMgr.MerchMgr.ExpandAdvanced();
            BuilderMgr.MerchMgr.DiscountCodeMgr.SetMerchandiseDiscountCode(DiscountCodeString);
            BuilderMgr.MerchMgr.SaveAndStay();
            BuilderMgr.MerchMgr.DiscountCodeMgr.VerifyMerchandiseDiscountCodeErrorMessage("You cannot use \"Discount (Invitation) codes\" when \"Variable Amount\" is selected");

            // Add discount code
            BuilderMgr.MerchMgr.SetType(MerchandiseManager.MerchandiseType.Fixed);
            BuilderMgr.MerchMgr.SetMerchItemPrice(MerchandiseManager.MerchandiseType.Fixed, MerchItemPrice, null, null);
            BuilderMgr.MerchMgr.ExpandAdvanced();
            BuilderMgr.MerchMgr.DiscountCodeMgr.SetMerchandiseDiscountCode(DiscountCodeString);
            BuilderMgr.MerchMgr.DiscountCodeMgr.ClickDiscountCodeRequired();
            BuilderMgr.MerchMgr.SaveAndClose();
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
        #endregion

        #region Registrations
        [Step]
        private void RegisterWithMerchandiseDiscountCode()
        {
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.SelectMerchandise(MerchQuantity);

            // Do not enter discount code and verify error message
            RegisterMgr.EnterMerchandiseDiscountCodeByName(MerchName, string.Empty);
            RegisterMgr.Continue();

            string errorMessages = string.Format(RegisterManager.Error.MerchandiseDiscountCode, MerchName);

            RegisterMgr.VerifyErrorMessage(errorMessages);

            // Enter merchandise discount code and verify the discount price
            RegisterMgr.EnterMerchandiseDiscountCodeByName(MerchName, DiscountCodeTypes.FixedAmount.ToString());
            RegisterMgr.VerifyMerchandisePageTotal(this.merchDiscountFee[DiscountCodeTypes.FixedAmount]);

            RegisterMgr.EnterMerchandiseDiscountCodeByName(MerchName, DiscountCodeTypes.Free.ToString());
            RegisterMgr.VerifyMerchandisePageTotal(this.merchDiscountFee[DiscountCodeTypes.Free]);

            RegisterMgr.EnterMerchandiseDiscountCodeByName(MerchName, DiscountCodeTypes.Access.ToString());
            RegisterMgr.VerifyMerchandisePageTotal(this.merchDiscountFee[DiscountCodeTypes.Access]);

            RegisterMgr.EnterMerchandiseDiscountCodeByName(MerchName, DiscountCodeTypes.Half.ToString());
            RegisterMgr.VerifyMerchandisePageTotal(this.merchDiscountFee[DiscountCodeTypes.Half]);

            RegisterMgr.Continue();

            // Pay money and verify
            double totalFee = EventFee + this.merchDiscountFee[DiscountCodeTypes.Half];
            double totalSaving = this.merchDiscountFee[DiscountCodeTypes.Access] - this.merchDiscountFee[DiscountCodeTypes.Half];
            RegisterMgr.PayMoneyAndVerify(totalFee, ManagerBase.PaymentMethod.Check);
            RegisterMgr.VerifyCheckoutSaving(totalSaving);

            // Finish registration and check the registration details
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
        #endregion
    }
}
