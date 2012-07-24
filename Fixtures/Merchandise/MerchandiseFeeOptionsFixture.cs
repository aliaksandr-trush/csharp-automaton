namespace RegOnline.RegressionTest.Fixtures.Merchandise
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegError = RegOnline.RegressionTest.Managers.Register.RegisterManager.Error;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class MerchandiseFeeOptionsFixture : FixtureBase
    {
        private const string EventName = "MerchandiseFeeOption";
        private const double EventFee = 10;
        private const double MerchFixedPrice = 10;

        private enum MerchType
        {
            MerchWithMin,
            MerchWithMax,
            MerchWithMinAndMax
        }

        private int eventID = ManagerBase.InvalidId;

        #region Test Methods
        [Test]
        [Category(Priority.Two)]
        [Description("592")]
        public void MerchandiseFeeOptionTest()
        {
            this.CreateEvent();
            this.RegisterToVerifyMerchandiseFeeOption();
        }
        #endregion 

        #region Create Event Helper Methods
        [Step]
        private void CreateEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (ManagerSiteMgr.EventExists(EventName))
            {
                this.eventID = ManagerSiteMgr.GetFirstEventId(EventName);
            }
            else if (this.eventID == ManagerBase.InvalidId)
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
            this.AddMerchandiseItem(MerchType.MerchWithMin, 2, null);
            this.AddMerchandiseItem(MerchType.MerchWithMax, null, 5);
            this.AddMerchandiseItem(MerchType.MerchWithMinAndMax, 3, 4);
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

        private void AddMerchandiseItem(MerchType merchName, int? minLimit, int? maxLimit)
        {
            BuilderMgr.ClickAddMerchandiseItem();
            BuilderMgr.MerchMgr.SetName(merchName.ToString());
            BuilderMgr.MerchMgr.SetType(MerchandiseManager.MerchandiseType.Fixed);
            BuilderMgr.MerchMgr.SetFixedPrice(MerchFixedPrice);
            BuilderMgr.MerchMgr.ExpandAdvanced();
            BuilderMgr.MerchMgr.SetMinimumLimit(minLimit);
            BuilderMgr.MerchMgr.SetMaximumLimit(maxLimit);
            BuilderMgr.MerchMgr.SaveAndClose();
        }      
        #endregion

        #region Registrations
        [Step]
        private void RegisterToVerifyMerchandiseFeeOption()
        {
            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();

            // Select merchandise and enter out of limit quantity
            RegisterMgr.SelectMerchandiseQuantityByName(MerchType.MerchWithMin.ToString(), 1);
            RegisterMgr.SelectMerchandiseQuantityByName(MerchType.MerchWithMax.ToString(), 6);
            RegisterMgr.SelectMerchandiseQuantityByName(MerchType.MerchWithMinAndMax.ToString(), 2);
            RegisterMgr.ContinueWithErrors();

            // Verify error message
            string[] errorMessages = new string[] {
                string.Format(RegError.MerchandiseMinOnlyFormat_NameMin, MerchType.MerchWithMin.ToString(), 2),
                string.Format(RegError.MerchandiseMinMaxFormat_NameMinMax, MerchType.MerchWithMax.ToString(), RegError.MerchandiseMinLimitDefault, 5),
                string.Format(RegError.MerchandiseMinMaxFormat_NameMinMax, MerchType.MerchWithMinAndMax.ToString(), 3, 4) };
            
            RegisterMgr.VerifyErrorMessage(errorMessages);

            List<int> merchQuantity = new List<int>() { 2, 5, 3 };

            // Select merchandise and enter the correct quantity
            RegisterMgr.SelectMerchandiseQuantityByName(MerchType.MerchWithMin.ToString(), merchQuantity[0]);
            RegisterMgr.SelectMerchandiseQuantityByName(MerchType.MerchWithMax.ToString(), merchQuantity[1]);
            RegisterMgr.SelectMerchandiseQuantityByName(MerchType.MerchWithMinAndMax.ToString(), merchQuantity[2]);

            double totalFee = EventFee;

            foreach (int quantity in merchQuantity)
            {
                totalFee += quantity * MerchFixedPrice;
            }
            
            // Verify continue with no errors
            RegisterMgr.Continue();
            RegisterMgr.OnCheckoutPage();

            // Pay money and verify - should pay 110
            RegisterMgr.PayMoneyAndVerify(totalFee, RegisterManager.PaymentMethod.Check);

            // Finish or update registration
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }
        #endregion
    }
}
