namespace RegOnline.RegressionTest.Fixtures.SpecialOptions
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegistrationLimitsFixture : FixtureBase
    {
        #region Constants
        private const double EventFee = 50;
        private const int EventLimit = 3;
        private const string RegTypeNameFormat = "RegType{0}";
        private const double RegTypeFee = 20;
        private const int RegTypeQuantity = 2;
        private const int RegLimitForRegType = 2;
        private const int SelectRandom = -1;
        #endregion

        #region Private variables
        private int eventID = -1;
        private string sessionID;
        private string primaryEmail;
        private Dictionary<int, string> regTypes;
        private Dictionary<int, string> directLinks;
        private Dictionary<int, RegistrationMethod> registrationMethods;
        #endregion

        #region Enum
        private enum EventType
        {
            EventWithLimitWithoutRegTypes,
            EventWithLimitWithRegTypes,
            EventWithLimitWithRegTypesForceSameRegType,
            EventWithoutLimitWithRegTypesEachHavingLimits,
            EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType
        }
        #endregion

        public RegistrationLimitsFixture()
        {
            this.regTypes = new Dictionary<int, string>();

            for (int count = 1; count <= RegTypeQuantity; count++)
            {
                this.regTypes.Add(count, string.Format(RegTypeNameFormat, count.ToString()));
            }

            this.directLinks = new Dictionary<int, string>();

            this.registrationMethods = new Dictionary<int, RegistrationMethod>();
            this.registrationMethods.Add(1, RegistrationMethod.Registrant);
            this.registrationMethods.Add(2, RegistrationMethod.EventWebsite);
            this.registrationMethods.Add(3, RegistrationMethod.RegTypeDirectLink);
        }

        #region Test Methods

        [Test]
        [Category(Priority.Two)]
        [Description("717")]
        public void EventWithLimitWithoutRegTypesTest()
        {
            // Step #1
            this.InitializeAndGetEventId(EventType.EventWithLimitWithoutRegTypes);

            this.DeleteTestRegistrations();

            // Step #2
            this.CreateIndividualRegistrationsUpToLimit(EventType.EventWithLimitWithoutRegTypes);

            // Step #3
            this.CreateIndividualRegistrationWhenLimitReached(RegistrationMethod.EventWebsite, EventType.EventWithLimitWithoutRegTypes);

            // Step #4
            this.CreateIndividualRegistrationWhenLimitReached(RegistrationMethod.Admin, EventType.EventWithLimitWithoutRegTypes);

            // Step #5
            this.CreateIndividualRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithLimitWithoutRegTypes);

            // Step #6
            this.DeleteTestRegistrations();

            // Step #7
            this.CreateGroupRegistrationsUpToLimit(RegistrationMethod.Registrant, EventType.EventWithLimitWithoutRegTypes);

            // Step #8
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithLimitWithoutRegTypes);

            // Step #9
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Admin, EventType.EventWithLimitWithoutRegTypes);

            // Step #10
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithLimitWithoutRegTypes);

            // Step #11
            this.DeleteTestRegistrations();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("722")]
        public void EventWithLimitWithRegTypesTest()
        {
            // Step #1
            this.InitializeAndGetEventId(EventType.EventWithLimitWithRegTypes);
            this.GetRegTypeDirectLinks();

            this.DeleteTestRegistrations();

            // Step #2
            this.CreateIndividualRegistrationsUpToLimit(EventType.EventWithLimitWithRegTypes);

            // Step #3
            this.CreateIndividualRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithLimitWithRegTypes);

            // Step #4
            this.CreateIndividualRegistrationWhenLimitReached(RegistrationMethod.RegTypeDirectLink, EventType.EventWithLimitWithRegTypes);

            // Step #5
            this.CreateIndividualRegistrationWhenLimitReached(RegistrationMethod.Admin, EventType.EventWithLimitWithRegTypes);

            // Step #6
            this.CreateIndividualRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithLimitWithRegTypes);

            // Step #7
            this.DeleteTestRegistrations();

            // Step #8
            this.CreateGroupRegistrationsUpToLimit(RegistrationMethod.RegTypeDirectLink, EventType.EventWithLimitWithRegTypes);

            // Step #9
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithLimitWithRegTypes);

            // Step #10
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Admin, EventType.EventWithLimitWithRegTypes);

            // Step #11
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithLimitWithRegTypes);

            // Step #12
            this.DeleteTestRegistrations();

            // Step #13
            this.SetForceSameRegType(true);

            // Step #14
            this.CreateGroupRegistrationsUpToLimit(this.GetRandomRegistrationMethodExceptAdmin(), EventType.EventWithLimitWithRegTypesForceSameRegType);

            // Step #15
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithLimitWithRegTypesForceSameRegType);

            // Step #16
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Admin, EventType.EventWithLimitWithRegTypesForceSameRegType);

            // Step #17
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithLimitWithRegTypesForceSameRegType);

            // Step #18
            this.DeleteTestRegistrations();

            // Step #19
            this.SetForceSameRegType(false);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("723")]
        public void EventWithoutLimitWithRegTypesEachHavingLimitsTest()
        {
            // Step #1
            this.InitializeAndGetEventId(EventType.EventWithoutLimitWithRegTypesEachHavingLimits);
            this.GetRegTypeDirectLinks();

            this.DeleteTestRegistrations();

            // Step #2
            this.CreateGroupRegistrationsUpToLimit(RegistrationMethod.Registrant, EventType.EventWithoutLimitWithRegTypesEachHavingLimits);
            string emailForGroupReg = this.primaryEmail;

            // Step #3
            this.CreateIndividualRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithoutLimitWithRegTypesEachHavingLimits);

            // Step #4
            this.CreateIndividualRegistrationWhenLimitReached(RegistrationMethod.RegTypeDirectLink, EventType.EventWithoutLimitWithRegTypesEachHavingLimits);

            // Step #5
            this.CreateGroupRegistrationsUpToLimit(RegistrationMethod.Admin, EventType.EventWithoutLimitWithRegTypesEachHavingLimits);

            // Step #6
            this.CreateIndividualRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithoutLimitWithRegTypesEachHavingLimits);

            // Step #7
            this.primaryEmail = emailForGroupReg;
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithoutLimitWithRegTypesEachHavingLimits);

            // Step #8
            this.DeleteTestRegistrations();

            // Step #9
            this.SetForceSameRegType(true);

            // Step #10
            this.CreateGroupRegistrationsUpToLimit(this.GetRandomRegistrationMethodExceptAdmin(), EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType);
            emailForGroupReg = this.primaryEmail;

            // Step #11
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Registrant, EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType);

            // Step #12
            this.CreateIndividualRegistration(true, RegistrationMethod.Registrant, EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType, SelectRandom);
            string emailForIndividualReg = RegisterMgr.CurrentEmail;

            // Step #13
            this.primaryEmail = emailForGroupReg;
            this.UpdateGroupRegistrationWhenLimitReached(RegistrationMethod.Admin, EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType);

            // Step #14
            this.primaryEmail = emailForIndividualReg;
            this.UpdateRegistrationsUpToLimit(RegistrationMethod.Registrant, EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType);

            // Step #15
            this.DeleteTestRegistrations();

            // Step #16
            this.SetForceSameRegType(false);
        }
        #endregion

        #region Create Event Methods
        [Step]
        private void CreateNewEvent(EventType eventType)
        {
            string eventName = eventType.ToString();

            // Delete old events
            ManagerSiteMgr.DeleteEventByName(eventName);

            // Always created new event
            // Select "Pro Event" from drop down
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);

            // Get sessionId
            this.sessionID = BuilderMgr.GetEventSessionId();

            // Get event id
            this.eventID = BuilderMgr.GetEventId();

            // Set start page
            this.SetEventStartPage(eventType);

            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);

            // Set checkout page
            this.SetCheckoutPage();

            // Go to Confirmation page
            BuilderMgr.Next();

            // Set confirmation page
            this.SetConfirmationPage();

            BuilderMgr.SaveAndClose();
        }

        private void SetEventStartPage(EventType eventType)
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventType.ToString());

            BuilderMgr.SetEventFee(EventFee);
            BuilderMgr.SaveAndStay();

            if (eventType == EventType.EventWithLimitWithoutRegTypes || eventType == EventType.EventWithLimitWithRegTypes)
            {
                BuilderMgr.SetEventLimit(true, EventLimit);
                BuilderMgr.SetEventLimitReachedMessage(this.eventID, RegisterManager.Error.CheckinEventSoldOut);
            }

            if (eventType == EventType.EventWithLimitWithRegTypes 
                || eventType == EventType.EventWithoutLimitWithRegTypesEachHavingLimits)
            {
                int? regTypeLimit = null;
                if (eventType == EventType.EventWithoutLimitWithRegTypesEachHavingLimits)
                {
                    regTypeLimit = RegLimitForRegType;
                }

                for (int count = 1; count <= RegTypeQuantity; count++)
                {
                    this.CreateRegType(this.regTypes[count], regTypeLimit);
                }
            }

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
        #endregion

        #region Builder Helper Methods
        [Step]
        private void LoginAndGoToEventTab()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.sessionID = BuilderMgr.GetEventSessionId();
        }

        private void CreateRegType(string regTypeName, int? regTypeLimit)
        {
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName(regTypeName);
            BuilderMgr.RegTypeMgr.ExpandAdvancedSection();
            BuilderMgr.RegTypeMgr.SetFee(RegTypeFee);

            if (regTypeLimit.HasValue)
            {
                BuilderMgr.RegTypeMgr.SetRegLimitOptions(
                    RegTypeManager.RegLimitType.Individual,
                    regTypeLimit.Value, 
                    string.Format(RegisterManager.Error.RegTypeLimitReachedFormat, regTypeName));
            }

            BuilderMgr.RegTypeMgr.SaveAndClose();
        }

        [Step]
        private void DeleteTestRegistrations()
        {
            this.LoginAndGoToEventTab();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTabAndVerify(DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ClickOption(DashboardManager.EventRegistrationFunction.DeleteTestRegistrations);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("plain");
            ManagerSiteMgr.DashboardMgr.DeleteTestReg_ClickDelete();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            ManagerSiteMgr.DashboardMgr.ReturnToList();
        }

        [Step]
        private void SetForceSameRegType(bool check)
        {
            this.LoginAndGoToEventTab();
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventID, this.sessionID);
            BuilderMgr.SetEventsForceSameRegTypes(check);
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void GetRegTypeDirectLinks()
        {
            List<string> regTypesDirectLinks = BuilderMgr.RegTypeMgr.GetRegTypeDirectLinks(this.eventID);

            int count = 1;

            foreach (string link in regTypesDirectLinks)
            {
                if (this.directLinks.ContainsKey(count))
                {
                    this.directLinks[count] = link;
                }
                else
                {
                    this.directLinks.Add(count, link);
                }

                count++;

                if (count > RegTypeQuantity)
                {
                    break;
                }
            }
        }
        #endregion

        #region Registrations
        [Step]
        private void InitializeAndGetEventId(EventType eventType)
        {
            string eventName = eventType.ToString();
            this.LoginAndGoToEventTab();
            ManagerSiteMgr.DeleteExpiredDuplicateEvents(eventName);

            if (!ManagerSiteMgr.EventExists(eventName))
            {
                switch (eventType)
                {
                    case EventType.EventWithLimitWithoutRegTypes:
                        this.CreateNewEvent(EventType.EventWithLimitWithoutRegTypes);
                        break;

                    case EventType.EventWithLimitWithRegTypes:
                        this.CreateNewEvent(EventType.EventWithLimitWithRegTypes);
                        break;

                    case EventType.EventWithoutLimitWithRegTypesEachHavingLimits:
                        this.CreateNewEvent(EventType.EventWithoutLimitWithRegTypesEachHavingLimits);
                        break;
                }
            }
            else
            {
                this.eventID = ManagerSiteMgr.GetFirstEventId(eventName);
            }
        }

        [Step]
        private void CreateIndividualRegistrationsUpToLimit(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.EventWithLimitWithoutRegTypes:
                    for (int regCount = 1; regCount <= EventLimit; regCount++)
                    {
                        this.CreateIndividualRegistration(
                            regCount < EventLimit,
                            RegistrationMethod.Registrant,
                            EventType.EventWithLimitWithoutRegTypes,
                            null);
                    }
                    break;

                case EventType.EventWithLimitWithRegTypes:
                    for (int regCount = 1; regCount <= EventLimit; regCount++)
                    {
                        int regTypeKey = this.GetRandomRegTypeKey();

                        this.CreateIndividualRegistration(
                            regCount < EventLimit,
                            this.GetRandomRegistrationMethodExceptAdmin(),
                            EventType.EventWithLimitWithRegTypes,
                            regTypeKey);
                    }
                    break;

                case EventType.EventWithoutLimitWithRegTypesEachHavingLimits:
                    for (int regTypeCount = 1; regTypeCount <= RegTypeQuantity; regTypeCount++)
                    {
                        for (int regCount = 1; regCount <= RegLimitForRegType; regCount++)
                        {
                            this.CreateIndividualRegistration(
                                (regTypeCount - 1) * RegLimitForRegType + regCount < RegTypeQuantity * RegLimitForRegType,
                                this.GetRandomRegistrationMethodExceptAdmin(),
                                EventType.EventWithoutLimitWithRegTypesEachHavingLimits,
                                regTypeCount);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        [Step]
        private void CreateIndividualRegistration(bool isAbleToAddAnotherPerson,
            RegistrationMethod registrationMethod, EventType eventType, int? regTypeKey)
        {
            if (regTypeKey == SelectRandom)
            {
                regTypeKey = this.GetRandomRegTypeKey();
            }

            switch (registrationMethod)
            {
                case RegistrationMethod.Registrant:
                    RegisterMgr.OpenRegisterPage(this.eventID);
                    break;

                case RegistrationMethod.RegTypeDirectLink:
                    RegisterMgr.OpenRegisterPage(this.eventID, this.directLinks[regTypeKey.Value]);
                    break;

                case RegistrationMethod.EventWebsite:
                    RegisterMgr.OpenRegisterPage(this.eventID, RegisterMgr.Fetch_EventWebsiteUrl(this.eventID));
                    break;

                default:
                    break;
            }

            double totalFee = 0;
            string message = string.Empty;

            switch (eventType)
            {
                case EventType.EventWithLimitWithoutRegTypes:
                    this.EnterCheckinAndProfileInfo(registrationMethod, null);
                    totalFee = EventFee;
                    message = RegisterManager.Error.EventLimitReachedAndContinue;
                    break;

                case EventType.EventWithLimitWithRegTypes:
                case EventType.EventWithoutLimitWithRegTypesEachHavingLimits:
                    string regType = this.regTypes[regTypeKey.Value];
                    this.EnterCheckinAndProfileInfo(registrationMethod, regType);
                    totalFee = RegTypeFee;
                    message = string.Format(RegisterManager.Error.RegTypeLimitReachedAndContinueFormat, regType);
                    break;

                case EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType:
                    string unavailableRegType = null;
                    List<EventRegType> regTypes = BuilderMgr.RegTypeMgr.Fetch_RegTypes(this.eventID);

                    foreach (EventRegType registrantType in regTypes)
                    {
                        if (!RegisterMgr.IsRegTypeAvailable(registrantType.Id))
                        {
                            regTypes.Remove(registrantType);
                            int randomKey = new Random((int)DateTime.Now.Ticks).Next(0, regTypes.Count);
                            unavailableRegType = regTypes[randomKey].Description;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(unavailableRegType))
                    {
                        Assert.Fail("All reg types are available!");
                    }

                    this.EnterCheckinAndProfileInfo(registrationMethod, unavailableRegType);
                    totalFee = RegTypeFee;
                    message = string.Format(RegisterManager.Error.RegTypeLimitReachedAndContinueFormat, unavailableRegType);
                    break;

                default:
                    break;
            }

            RegisterMgr.VerifyHasAddAnotherPersonButton(isAbleToAddAnotherPerson);

            if (!isAbleToAddAnotherPerson)
            {
                RegisterMgr.VerifyEventLimitReachedAndContinueMessage(message);
            }

            RegisterMgr.Continue();
            this.PayAndVerifyAmountAndFinish(totalFee);
        }

        [Step]
        private void UpdateRegistrationsUpToLimit(RegistrationMethod registrationMethod, EventType eventType)
        {
            if (registrationMethod == RegistrationMethod.Admin)
            {
                RegisterMgr.OpenAdminRegisterPage(this.eventID, this.sessionID);
            }
            else
            {
                RegisterMgr.OpenRegisterPage(this.eventID);
            }

            RegisterMgr.CheckinWithEmail(this.primaryEmail);
            RegisterMgr.Continue();
            RegisterMgr.EnterPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
            RegisterMgr.Continue();

            string regType = string.Empty;
            string message = string.Empty;

            switch (eventType)
            {
                case EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType:
                    int index = 1;
                    regType = RegisterMgr.GetRegTypeText(index);

                    int regQuantityForPrimaryAttendee = 1;

                    for (int regCount = 1; regCount <= RegLimitForRegType - regQuantityForPrimaryAttendee; regCount++)
                    {
                        this.AddAnotherPerson(registrationMethod, null);
                    }

                    RegisterMgr.CurrentEmail = this.primaryEmail;
                    message = string.Format(RegisterManager.Error.RegTypeLimitReachedAndContinueFormat, regType);
                    break;

                default:
                    break;
            }

            RegisterMgr.VerifyEventLimitReachedAndContinueMessage(message);
            RegisterMgr.Continue();
            RegisterMgr.VerifyHasAddAnotherPersonButton(false);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        [Step]
        private void CreateIndividualRegistrationWhenLimitReached(RegistrationMethod registrationMethod, EventType eventType)
        {
            int regTypeKey = this.GetRandomRegTypeKey();

            double totalFee = 0;

            switch (eventType)
            {
                case EventType.EventWithLimitWithoutRegTypes:
                    switch (registrationMethod)
                    {
                        case RegistrationMethod.Registrant:
                            RegisterMgr.OpenRegisterPage(this.eventID);
                            RegisterMgr.VerifyEventLimitReachedMessage(RegisterManager.Error.CheckinEventSoldOut);
                            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(false);
                            break;

                        case RegistrationMethod.Admin:
                            RegisterMgr.OpenAdminRegisterPage(this.eventID, this.sessionID);
                            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(true);
                            this.EnterCheckinAndProfileInfo(RegistrationMethod.Admin, null);
                            totalFee = EventFee;
                            RegisterMgr.Continue();
                            RegisterMgr.VerifyHasAddAnotherPersonButton(true);
                            this.PayAndVerifyAmountAndFinish(totalFee);
                            break;

                        case RegistrationMethod.EventWebsite:
                            RegisterMgr.OpenRegisterPage(this.eventID, RegisterMgr.Fetch_EventWebsiteUrl(this.eventID));
                            RegisterMgr.VerifyEventLimitReachedMessage(RegisterManager.Error.CheckinEventSoldOut);
                            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(false);
                            break;

                        default:
                            break;
                    }

                    break;

                case EventType.EventWithLimitWithRegTypes:
                    switch (registrationMethod)
                    {
                        case RegistrationMethod.Registrant:
                            RegisterMgr.OpenRegisterPage(this.eventID);
                            RegisterMgr.VerifyEventLimitReachedMessage(RegisterManager.Error.CheckinEventSoldOut);
                            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(false);
                            break;

                        case RegistrationMethod.EventWebsite:
                            RegisterMgr.OpenRegisterPage(this.eventID, RegisterMgr.Fetch_EventWebsiteUrl(this.eventID));
                            RegisterMgr.VerifyEventLimitReachedMessage(RegisterManager.Error.CheckinEventSoldOut);
                            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(false);
                            break;

                        case RegistrationMethod.RegTypeDirectLink:
                            RegisterMgr.OpenRegisterPage(this.eventID, this.directLinks[regTypeKey]);
                            RegisterMgr.VerifyEventLimitReachedMessage(RegisterManager.Error.CheckinEventSoldOut);
                            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(false);
                            break;

                        case RegistrationMethod.Admin:
                            RegisterMgr.OpenAdminRegisterPage(this.eventID, this.sessionID);
                            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(true);
                            this.EnterCheckinAndProfileInfo(RegistrationMethod.Admin, this.regTypes[regTypeKey]);
                            totalFee = RegTypeFee;
                            RegisterMgr.Continue();
                            RegisterMgr.VerifyHasAddAnotherPersonButton(true);
                            this.PayAndVerifyAmountAndFinish(totalFee);
                            break;

                        default:
                            break;
                    }

                    break;

                case EventType.EventWithoutLimitWithRegTypesEachHavingLimits:
                    switch (registrationMethod)
                    {
                        case RegistrationMethod.Registrant:
                            RegisterMgr.OpenRegisterPage(this.eventID);
                            //Registration.VerifyEventLimitReachedMessage(RegistrationManager.CheckinEventAvailableMessage);
                            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(true);
                            RegisterMgr.Checkin();
                            RegisterMgr.ContinueWithErrors();
                            RegisterMgr.VerifyHasCheckinErrorMessage(true);
                            RegisterMgr.VerifyCheckinErrorMessage(RegisterManager.Error.CheckinRequiredFieldsError);
                            break;

                        case RegistrationMethod.EventWebsite:
                            RegisterMgr.OpenRegisterPage(this.eventID, RegisterMgr.Fetch_EventWebsiteUrl(this.eventID));
                            //Registration.VerifyEventLimitReachedMessage(RegistrationManager.CheckinEventAvailableMessage);
                            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(true);
                            RegisterMgr.Checkin();
                            RegisterMgr.ContinueWithErrors();
                            RegisterMgr.VerifyHasCheckinErrorMessage(true);
                            RegisterMgr.VerifyCheckinErrorMessage(RegisterManager.Error.CheckinRequiredFieldsError);
                            break;

                        case RegistrationMethod.RegTypeDirectLink:
                            RegisterMgr.OpenRegisterPage(this.eventID, this.directLinks[regTypeKey]);
                            
                            RegisterMgr.VerifyEventLimitReachedMessage(string.Format(
                                RegisterManager.Error.RegTypeLimitReachedFormat, 
                                this.regTypes[regTypeKey]));

                            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(false);
                            break;

                        case RegistrationMethod.Admin:
                            RegisterMgr.OpenAdminRegisterPage(this.eventID, this.sessionID);
                            RegisterMgr.VerifyHasContinueOrContinueNextStepButton(true);

                            // Admin reg can go over limit, either event limit, or reg type limit
                            // So using random number as reg type key is OK here
                            this.EnterCheckinAndProfileInfo(RegistrationMethod.Admin, this.regTypes[regTypeKey]);
                            totalFee = RegTypeFee;
                            RegisterMgr.Continue();
                            RegisterMgr.VerifyHasAddAnotherPersonButton(true);
                            this.PayAndVerifyAmountAndFinish(totalFee);
                            break;

                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }
        }

        [Step]
        private void CreateGroupRegistrationsUpToLimit(RegistrationMethod registrationMethod, EventType eventType)
        {
            int regTypeKey = SelectRandom;

            switch (registrationMethod)
            {
                case RegistrationMethod.Registrant:
                    RegisterMgr.OpenRegisterPage(this.eventID);
                    break;

                case RegistrationMethod.EventWebsite:
                    RegisterMgr.OpenRegisterPage(this.eventID, RegisterMgr.Fetch_EventWebsiteUrl(this.eventID));
                    break;

                case RegistrationMethod.RegTypeDirectLink:
                    regTypeKey = this.GetRandomRegTypeKey();
                    RegisterMgr.OpenRegisterPage(this.eventID, this.directLinks[regTypeKey]);
                    break;

                case RegistrationMethod.Admin:
                    RegisterMgr.OpenAdminRegisterPage(this.eventID, this.sessionID);
                    break;

                default:
                    break;
            }

            // "1" represents the registration of the primary attendee
            int regQuantityForPrimaryAttendee = 1;

            string message = string.Empty;

            switch (eventType)
            {
                case EventType.EventWithLimitWithoutRegTypes:
                    this.EnterCheckinAndProfileInfo(registrationMethod, null);
                    this.primaryEmail = RegisterMgr.CurrentEmail;

                    for (int count = 1; count <= EventLimit - regQuantityForPrimaryAttendee; count++)
                    {
                        this.AddAnotherPerson(registrationMethod, null);
                    }

                    message = RegisterManager.Error.EventLimitReachedAndContinue;
                    break;

                case EventType.EventWithLimitWithRegTypes:
                    if (regTypeKey == SelectRandom)
                    {
                        this.EnterCheckinAndProfileInfo(registrationMethod, this.regTypes[this.GetRandomRegTypeKey()]);
                    }
                    // 'regTypeKey != SelectRandom' means the current registration is using reg type direct link,
                    // under this situation, all registrations in the same group are forced to one reg type
                    else
                    {
                        this.EnterCheckinAndProfileInfo(registrationMethod, null);
                    }

                    this.primaryEmail = RegisterMgr.CurrentEmail;
                    string lastRegType = string.Empty;

                    for (int count = 1; count <= EventLimit - regQuantityForPrimaryAttendee; count++)
                    {
                        int key = this.GetRandomRegTypeKey();

                        if (regTypeKey == SelectRandom)
                        {
                            this.AddAnotherPerson(registrationMethod, this.regTypes[key]);
                        }
                        else
                        {
                            this.AddAnotherPerson(registrationMethod, null);
                        }

                        if (count == EventLimit - regQuantityForPrimaryAttendee)
                        {
                            lastRegType = this.regTypes[key];
                        }
                    }

                    if (regTypeKey == SelectRandom)
                    {
                        message = string.Format(
                            RegisterManager.Error.RegTypeLimitReachedAndContinueFormat, 
                            lastRegType);
                    }
                    else
                    {
                        message = string.Format(
                            RegisterManager.Error.RegTypeLimitReachedAndContinueFormat, 
                            this.regTypes[regTypeKey]);
                    }

                    break;

                case EventType.EventWithLimitWithRegTypesForceSameRegType:
                    if (regTypeKey == SelectRandom)
                    {
                        regTypeKey = this.GetRandomRegTypeKey();
                    }

                    string regType = this.regTypes[regTypeKey];
                    this.EnterCheckinAndProfileInfo(registrationMethod, regType);
                    this.primaryEmail = RegisterMgr.CurrentEmail;

                    for (int count = 1; count <= EventLimit - regQuantityForPrimaryAttendee; count++)
                    {
                        this.AddAnotherPerson(registrationMethod, null);
                    }

                    message = string.Format(RegisterManager.Error.RegTypeLimitReachedAndContinueFormat, regType);
                    break;

                case EventType.EventWithoutLimitWithRegTypesEachHavingLimits:
                    if (regTypeKey == SelectRandom)
                    {
                        regTypeKey = this.GetRandomRegTypeKey();
                    }

                    int lastRegTypeKey = regTypeKey;
                    bool firstRegForStartingRegType = true;

                    this.EnterCheckinAndProfileInfo(registrationMethod, this.regTypes[regTypeKey]);
                    this.primaryEmail = RegisterMgr.CurrentEmail;

                    for (int regTypeCount = 1; regTypeCount <= RegTypeQuantity; regTypeCount++)
                    {
                        for (int regCount = 1; regCount <= RegLimitForRegType; regCount++)
                        {
                            if (firstRegForStartingRegType && regTypeCount == regTypeKey)
                            {
                                firstRegForStartingRegType = false;
                                continue;
                            }
                            else
                            {
                                this.AddAnotherPerson(registrationMethod, this.regTypes[regTypeCount]);
                            }

                            lastRegTypeKey = regTypeCount;
                        }
                    }

                    message = string.Format(RegisterManager.Error.RegTypeLimitReachedAndContinueFormat, this.regTypes[lastRegTypeKey]);
                    break;

                case EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType:
                    if (regTypeKey == SelectRandom)
                    {
                        regTypeKey = this.GetRandomRegTypeKey();
                    }

                    regType = this.regTypes[regTypeKey];
                    this.EnterCheckinAndProfileInfo(registrationMethod, regType);
                    this.primaryEmail = RegisterMgr.CurrentEmail;

                    for (int count = 1; count <= RegLimitForRegType - regQuantityForPrimaryAttendee; count++)
                    {
                        this.AddAnotherPerson(registrationMethod, null);
                    }

                    message = string.Format(RegisterManager.Error.RegTypeLimitReachedAndContinueFormat, regType);
                    break;

                default:
                    break;
            }

            RegisterMgr.VerifyHasAddAnotherPersonButton(registrationMethod == RegistrationMethod.Admin ? true : false);

            if (registrationMethod != RegistrationMethod.Admin)
            {
                RegisterMgr.VerifyEventLimitReachedAndContinueMessage(message);
            }

            RegisterMgr.CurrentEmail = this.primaryEmail;
            RegisterMgr.Continue();
            RegisterMgr.VerifyHasAddAnotherPersonButton(registrationMethod == RegistrationMethod.Admin ? true : false);
            this.PayAndVerifyAmountAndFinish(this.GetGroupRegTotalFee(eventType));
        }

        [Step]
        private void UpdateGroupRegistrationWhenLimitReached(RegistrationMethod registrationMethod, EventType eventType)
        {
            double totalFee = 0;

            if (registrationMethod == RegistrationMethod.Admin)
            {
                RegisterMgr.OpenAdminRegisterPage(this.eventID, this.sessionID);
            }
            else
            {
                RegisterMgr.OpenRegisterPage(this.eventID);
            }

            RegisterMgr.CheckinWithEmail(this.primaryEmail);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterEmailAddress(this.primaryEmail);

            if (registrationMethod == RegistrationMethod.Admin)
            {
                RegisterMgr.Continue();
                int regTypeKey = this.GetRandomRegTypeKey();
                string regType = this.regTypes[regTypeKey];

                RegisterMgr.VerifyHasAddAnotherPersonButton(true);

                switch (eventType)
                {
                    case EventType.EventWithLimitWithoutRegTypes:
                        totalFee = EventFee * (EventLimit + 1);
                        break;

                    case EventType.EventWithLimitWithRegTypes:
                    case EventType.EventWithLimitWithRegTypesForceSameRegType:
                        totalFee = RegTypeFee * (EventLimit + 1);
                        break;

                    case EventType.EventWithoutLimitWithRegTypesEachHavingLimits:
                        totalFee = RegTypeFee * (RegLimitForRegType * RegTypeQuantity + 1);
                        break;

                    case EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType:
                        totalFee = RegTypeFee * (RegLimitForRegType + 1);
                        break;

                    default:
                        break;
                }

                this.AddAnotherPerson(registrationMethod, regType);

                RegisterMgr.Continue();
                RegisterMgr.VerifyHasAddAnotherPersonButton(true);
                RegisterMgr.CurrentEmail = this.primaryEmail;
                this.PayAndVerifyAmountAndFinish(totalFee);
            }
            else
            {
                RegisterMgr.EnterPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
                RegisterMgr.Continue();

                RegisterMgr.VerifyHasAddAnotherPersonButton(false);

                int index = 1;
                string regType = string.Empty;

                switch (eventType)
                {
                    case EventType.EventWithLimitWithRegTypes:
                    case EventType.EventWithLimitWithRegTypesForceSameRegType:
                    case EventType.EventWithoutLimitWithRegTypesEachHavingLimits:
                    case EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType:
                        regType = RegisterMgr.GetRegTypeText(index);
                        break;

                    default:
                        break;
                }

                RegisterMgr.ClickEditPersonalInformationLink(index - 1);
                RegisterMgr.EnterPersonalInfoAddress(null, "Update", null, null, null, null);
                string message = string.Empty;

                switch (eventType)
                {
                    case EventType.EventWithLimitWithoutRegTypes:
                        message = RegisterManager.Error.EventLimitReachedAndContinue;
                        break;

                    case EventType.EventWithLimitWithRegTypes:
                    case EventType.EventWithLimitWithRegTypesForceSameRegType:
                    case EventType.EventWithoutLimitWithRegTypesEachHavingLimits:
                    case EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType:
                        message = string.Format(RegisterManager.Error.RegTypeLimitReachedAndContinueFormat, regType);
                        break;

                    default:
                        break;
                }

                RegisterMgr.VerifyEventLimitReachedAndContinueMessage(message);
                RegisterMgr.Continue();
                RegisterMgr.VerifyHasAddAnotherPersonButton(false);
                RegisterMgr.ClickFinalizeButton();
                RegisterMgr.FinishRegistration();
                RegisterMgr.ConfirmRegistration();
            }
        }
        #endregion

        #region Registration Helper Methods
        private void EnterCheckinAndProfileInfo(RegistrationMethod registrationType, string regType)
        {
            RegisterMgr.Checkin();

            if (regType != null)
            {
                RegisterMgr.SelectRegType(regType);
            }

            RegisterMgr.Continue();

            if (RegisterMgr.HasCheckinErrorMessage())
            {
                Assert.Fail(string.Format("Checkin error message: {0}", RegisterMgr.GetCheckinErrorMessage()));
            }

            if (registrationType == RegistrationMethod.Admin)
            {
                RegisterMgr.EnterProfileInfoWithoutPassword();
            }
            else
            {
                RegisterMgr.EnterProfileInfo();
            }
        }

        private void AddAnotherPerson(RegistrationMethod registrationType, string regType)
        {
            RegisterMgr.VerifyHasAddAnotherPersonButton(true);
            RegisterMgr.ClickAddAnotherPerson();
            this.EnterCheckinAndProfileInfo(registrationType, regType);
        }

        private void PayAndVerifyAmountAndFinish(double totalAmount)
        {
            RegisterMgr.PayMoneyAndVerify(totalAmount, RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        // Generate a random number used as key of the regTypes Dictionary
        private int GetRandomRegTypeKey()
        {
            // The random number cannot reach the upperbound in 'Next(minValue, maxValue)' method
            // So the upperbound must be 'RegTypeQuantity + 1'
            return new Random((int)DateTime.Now.Ticks).Next(1, RegTypeQuantity + 1);
        }

        private RegistrationMethod GetRandomRegistrationMethodExceptAdmin()
        {
            return this.registrationMethods[new Random((int)DateTime.Now.Ticks).Next(1, 4)];
        }

        private double GetGroupRegTotalFee(EventType eventType)
        {
            double totalFee = 0;
            switch (eventType)
            {
                case EventType.EventWithLimitWithoutRegTypes:
                    totalFee = EventFee * EventLimit;
                    break;

                case EventType.EventWithLimitWithRegTypes:
                case EventType.EventWithLimitWithRegTypesForceSameRegType:
                    totalFee = RegTypeFee * EventLimit;
                    break;

                case EventType.EventWithoutLimitWithRegTypesEachHavingLimits:
                    totalFee = RegTypeFee * RegLimitForRegType * RegTypeQuantity;
                    break;

                case EventType.EventWithoutLimitWithRegTypesEachHavingLimitsForceSameRegType:
                    totalFee = RegTypeFee * RegLimitForRegType;
                    break;

                default:
                    break;
            }
            return totalFee;
        }
        #endregion
    }
}
