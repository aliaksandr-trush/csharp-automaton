namespace RegOnline.RegressionTest.Fixtures.RegisterAndBuilder.CheckinPage
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegTypeMinMaxGroupSizeFixture : FixtureBase
    {
        private int eventID = ManagerBase.InvalidId;
        private string eventName = "RegTypeMinMaxGroupSize";
        
        #region Test Methods
        [Test]
        [Category(Priority.Two)]
        [Description("434")]
        public void MinMaxGroupSizeForceToOneRegTypeTest()
        {
            CreateNewEvent(eventName, 2, 3, 3, 4);
            RegistrationWithRegTypeMinMaxGroupSize("RegType1", 2, 3, 30);
            RegistrationWithRegTypeMinMaxGroupSize("RegType2", 3, 4, 40);
        }
        #endregion

        #region Create Event Methods
        //Creat New Test Event
        [Step]
        private void CreateNewEvent(string eventName, int regType1MinSize, int regType1MaxSize, int regType2MinSize, int regType2MaxSize)
        {
            ManagerSiteMgr.OpenLogin();

            // login to regression testing account
            ManagerSiteMgr.Login();

            // make sure we're on the Events tab
            ManagerSiteMgr.GoToEventsTabIfNeeded();

            // click "Regression" folder
            ManagerSiteMgr.SelectFolder();

            //do not need build event if event exists and been created in the last a few minutes
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(eventName, 0);
            
            //if event exist get latest event id
            if (ManagerSiteMgr.EventExists(eventName))
            {
                eventID = ManagerSiteMgr.GetFirstEventId(eventName);
            }
            //if event not exist create new event
            else if (eventID == FormDetailManager.InvalidId)
            {
                //create pro event
                //select "Pro Event" from drop down
                ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

                //get event id
                eventID = BuilderMgr.GetEventId();

                //test start page
                TestEventStartPage(eventName,regType1MinSize,regType1MaxSize,regType2MinSize,regType2MaxSize);

                BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);

                //test checkout page
                TestCheckoutPage();

                //go to next page
                BuilderMgr.Next();

                //test confirmation page
                TestConfirmationPage();                

                //save and close
                BuilderMgr.SaveAndClose();
            }
        }

        private void TestEventStartPage(string eventName, int regType1MinSize, int regType1MaxSize, int regType2MinSize, int regType2MaxSize)
        {
            //verify initial defaults
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);

            //enter event start page info
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventName);

            //add event fee
            BuilderMgr.SetEventFee(10);

            //save and stay
            BuilderMgr.SaveAndStay();

            //add regtypes
            BuilderMgr.AddRegType("RegType1");
            BuilderMgr.AddRegType("RegType2");

            //make sure members of the same group must select the same registrant type 
            BuilderMgr.SaveAndStay();
            BuilderMgr.GotoPage(FormDetailManager.Page.PI);
            BuilderMgr.GotoPage(FormDetailManager.Page.Start);
            BuilderMgr.SetEventsForceSameRegTypes(true);

            //update regtypes,add min/max group size and minimum message
            UpdateRegType("RegType1", regType1MinSize, regType1MaxSize);
            UpdateRegType("RegType2", regType2MinSize, regType2MaxSize);

            //save and stay
            BuilderMgr.SaveAndStay();
        }

        private void TestCheckoutPage()
        {
            // enter event checkout page info
            BuilderMgr.EnterEventCheckoutPage();

            // save and stay
            BuilderMgr.SaveAndStay();
        }

        private void TestConfirmationPage()
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
        private void UpdateRegType(string name,int minSize,int maxSize)
        {
            // Open regtype
            BuilderMgr.OpenRegType(name);
            BuilderMgr.RegTypeMgr.ExpandAdvancedSection();
            
            // Set minimum message
            string message = string.Format("The minimum group size is {0} and the maximum group size is {1}.",minSize,maxSize);
            BuilderMgr.RegTypeMgr.SetMinGroupSize(minSize);
            BuilderMgr.RegTypeMgr.SetMaxGroupSize(maxSize);
            BuilderMgr.RegTypeMgr.SetMinimumRegistrantMessageInDatabase(this.eventID, name, message);

            // Save and close
            BuilderMgr.RegTypeMgr.SaveAndClose();
        }
        #endregion

        #region Registrations
        [Step]
        private void RegistrationWithRegTypeMinMaxGroupSize(string regtypeName, int minSize, int maxSize, double amount)
        {
            string primaryEmail = string.Empty;

            // Start new registration
            RegisterMgr.OpenRegisterPage(eventID);

            // Register check in
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(regtypeName);
            RegisterMgr.VerifyCheckinRegTypeGroupSizeMessage(regtypeName, minSize, maxSize);
            RegisterMgr.Continue();

            // Enter profile info and verify
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(false);
            RegisterMgr.VerifyHasAddAnotherPersonButton(true);
            VerifyPersonalInfoPageMinMessage(minSize, maxSize);
            primaryEmail = RegisterMgr.CurrentEmail;

            for (int i = 1; i < minSize-1; i++)
            {
                AddAnotherPersonWithSameRegType(false, false);
            }

            for (int i = minSize; i < maxSize; i++)
            {
                //add another person and verify
                AddAnotherPersonWithSameRegType(true,false);
            }

            //add another person and verify
            AddAnotherPersonWithSameRegType(true,true);
            RegisterMgr.Continue();
            RegisterMgr.CurrentEmail = primaryEmail;
            
            //pay money and verify
            RegisterManager.FeeResponse[] expFees = new RegisterManager.FeeResponse[1];

            expFees[0] = new RegisterManager.FeeResponse();
            expFees[0].FeeName = string.Format("{0} Event Fee ",regtypeName);
            expFees[0].FeeQuantity = maxSize.ToString();
            expFees[0].FeeUnitPrice = "$10.00";
            expFees[0].FeeAmount = string.Format("${0}0.00",maxSize);

            RegisterMgr.VerifyRegistrationFees(expFees);
            RegisterMgr.PayMoneyAndVerify(amount, PaymentManager.PaymentMethod.Check);

            //finish registration and check the registration details
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void AddAnotherPersonWithSameRegType(bool isRegTypeGroupMinSizeReached,bool isRegTypeGroupMaxSizeReached)
        {
            //add another person
            RegisterMgr.ClickAddAnotherPerson();

            //register check in
            RegisterMgr.Checkin();
            RegisterMgr.Continue();

            //enter profile info and verify
            RegisterMgr.EnterProfileInfo();
            
            if (isRegTypeGroupMinSizeReached)
            {
                RegisterMgr.VerifyHasContinueOrContinueNextStepButton(true);
            }
            else
            {
                RegisterMgr.VerifyHasContinueOrContinueNextStepButton(false);
            }

            if (isRegTypeGroupMaxSizeReached)
            {
                RegisterMgr.VerifyHasAddAnotherPersonButton(false);
            }
            else
            {
                RegisterMgr.VerifyHasAddAnotherPersonButton(true);
            }
        }
        
        private void VerifyPersonalInfoPageMinMessage(int minSize, int maxSize)
        {
            string expMessage = string.Format("The minimum group size is {0} and the maximum group size is {1}.",minSize,maxSize);
            RegisterMgr.VerifyRegTypeMinimumMessage(expMessage);            
        }
        #endregion        
    }
}
