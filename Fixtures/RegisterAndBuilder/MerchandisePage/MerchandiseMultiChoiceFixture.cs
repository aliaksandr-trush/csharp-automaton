namespace RegOnline.RegressionTest.Fixtures.RegisterAndBuilder.MerchandisePage
{
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
    class MerchandiseMultiChoiceFixture : FixtureBase
    {
        private const string EventFeeTitle = "Event Fee ";
        private const string EventName = "MerchandiseMultiChoice";
        private const double EventFee = 10;
        private const int TimeSpan = 30;
        private const string MerchName = "MCHWithMultiChoice";
        private const double MerchPrice = 20;
        private const int MerchItemQuantity = 4;
        private const int FeeQuantity = 1;
        private const string MultiChoiceItemNamePrefix = "Multiple";
        private const int MultiChoiceItemLimit = 5;

        private int eventID = ManagerBase.InvalidId;
        private RegisterManager.FeeResponse[] expFees;
        
        #region Test Methods
        [Test]
        [Category(Priority.Two)]
        [Description("658")]
        public void MerchandiseMultiChoiceTest()
        {
            this.CreateNewEvent();
            this.RegisterChooseMultiChoiceMerchandise();
            this.RegisterNotChooseMultiChoiceMerchandise();
            this.RegisterWithNoMerchandise();
        }
        #endregion

        #region Create Event Helper Methods
        [Step]
        private void CreateNewEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName, 0);

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
            BuilderMgr.ClickAddMerchandiseItem();
            BuilderMgr.MerchMgr.SetName(MerchName);
            BuilderMgr.MerchMgr.SetType(MerchandiseManager.MerchandiseType.Fixed);
            BuilderMgr.MerchMgr.SetFixedPrice(MerchPrice);
            BuilderMgr.MerchMgr.ExpandAdvanced();

            for (int i = 1; i <= MerchItemQuantity; i++)
            {
                BuilderMgr.MerchMgr.AddMerchandiseMultipleChoiceItem(MultiChoiceItemNamePrefix + i.ToString(), MultiChoiceItemLimit);
            }

            BuilderMgr.MerchMgr.SaveAndClose();
            BuilderMgr.SaveAndStay();
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

        private void AddMerchandiseMultipleChoice(string name, int number)
        {
            BuilderMgr.OpenMerchandiseItem(MerchName);
            BuilderMgr.ClickAdvancedOnFrame();
            BuilderMgr.MerchMgr.AddMerchandiseMultipleChoiceItem(name, number);
            BuilderMgr.MerchMgr.SaveAndClose();
            BuilderMgr.SaveAndStay();
        }
        #endregion

        #region Registrations
        [Step]
        private void RegisterChooseMultiChoiceMerchandise()
        {
            int multiChoiceItemIndex = 2;
            string multiChoiceItemName = MultiChoiceItemNamePrefix + multiChoiceItemIndex.ToString();

            this.StartRegistrationAndEnterProfileInfo();

            // Select merchandise and enter a quantity and select multiple item
            RegisterMgr.SelectMerchandiseQuantityByName(MerchName, FeeQuantity);
            RegisterMgr.SelectMerchandiseMultipleChoiceByName(MerchName, multiChoiceItemName);
            RegisterMgr.Continue();

            // Pay money and verify - should pay 30 and the merchandise fee name should be "MCHWithMultiChoice (Multiple2)"
            this.expFees = new RegisterManager.FeeResponse[2];

            this.expFees[0] = new RegisterManager.FeeResponse();
            this.expFees[0].FeeName = EventFeeTitle;
            this.expFees[0].FeeQuantity = FeeQuantity.ToString();
            this.expFees[0].FeeUnitPrice = MoneyTool.FormatMoney(EventFee);
            this.expFees[0].FeeAmount = MoneyTool.FormatMoney(EventFee * FeeQuantity);

            this.expFees[1] = new RegisterManager.FeeResponse();
            this.expFees[1].FeeName = MerchName + " (" + multiChoiceItemName + ") ";
            this.expFees[1].FeeQuantity = FeeQuantity.ToString();
            this.expFees[1].FeeUnitPrice = MoneyTool.FormatMoney(MerchPrice);
            this.expFees[1].FeeAmount = MoneyTool.FormatMoney(MerchPrice * FeeQuantity);

            this.PayMoneyAndVerify(this.expFees, EventFee + MerchPrice);
            this.FinishRegistrationAndVerify(this.expFees);
        }

        [Step]
        private void RegisterNotChooseMultiChoiceMerchandise()
        {
            int multiChoiceItemIndex = 1;
            string multiChoiceItemName = MultiChoiceItemNamePrefix + multiChoiceItemIndex.ToString();

            this.StartRegistrationAndEnterProfileInfo();
            RegisterMgr.SelectMerchandiseQuantityByName(MerchName, FeeQuantity);
            RegisterMgr.Continue();

            // Pay money and verify - should pay 30 and the merchandise fee name should be "MCHWithMultiChoice (Multiple1)"
            this.expFees[1].FeeName = MerchName + " (" + multiChoiceItemName + ") ";

            this.PayMoneyAndVerify(this.expFees, EventFee + MerchPrice);
            this.FinishRegistrationAndVerify(this.expFees);
        }

        [Step]
        private void RegisterWithNoMerchandise()
        {
            StartRegistrationAndEnterProfileInfo();

            // Don't select merchandise
            RegisterMgr.Continue();

            // Pay money and verify - should pay 10 and no merchandise item fee name
            this.expFees = new RegisterManager.FeeResponse[1];
            this.expFees[0] = new RegisterManager.FeeResponse();
            this.expFees[0].FeeName = EventFeeTitle;
            this.expFees[0].FeeQuantity = FeeQuantity.ToString();
            this.expFees[0].FeeUnitPrice = MoneyTool.FormatMoney(EventFee);
            this.expFees[0].FeeAmount = MoneyTool.FormatMoney(EventFee * FeeQuantity);

            this.PayMoneyAndVerify(this.expFees, EventFee);
            this.FinishRegistrationAndVerify(this.expFees);
        }
        #endregion

        #region Registration Helper Methods
        private void StartRegistrationAndEnterProfileInfo()
        {
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
        }

        private void PayMoneyAndVerify(RegisterManager.FeeResponse[] expFees, double amount)
        {
            RegisterMgr.VerifyRegistrationFees(expFees);
            RegisterMgr.PayMoneyAndVerify(amount, RegisterManager.PaymentMethod.Check);
        }

        private void FinishRegistrationAndVerify(RegisterManager.FeeResponse[] expFees)
        {
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            RegisterMgr.VerifyRegistrationFees(expFees);            
        }
        #endregion
    }
}
