namespace RegOnline.RegressionTest.Fixtures.FormTypes.ProEvent
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.DataAccess;
    using System.Collections.Generic;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class FormOptionsFixture : FixtureBase
    {
        private const string GoLiveEventName = "GoLiveAndCheckDate";
        private const string EventName_ToCreateTemplateFrom = BuildEventFixture.EventName;
        private const string EventName_CreatedFromTemplate = BuildEventFixture.EventName + " (from template)";
        private const string TestModeLocator = "warningTestMode";
        private const string CopiedEventName = BuildEventFixture.EventName + " (Copy)";
        private const string ChangeStatusEventName = "FormOptionsFixture_ChangeStatus";

        private BuildEventFixtureHelper helper = new BuildEventFixtureHelper();
        
        private int eventID;
        private string sessionId;
        private string emailAddr;

        #region tests
        [Test]
        [Category(Priority.Two)]
        [Description("370")]
        public void ActivateEvent()
        {
            this.CreateNewEventToActivate();
            this.EventActivation();
            this.VerifyActiveDateInDB();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("369")]
        public void CreateFromTemplate()
        {
            this.LoginAndGoToRegressionFolder();
            ManagerSiteMgr.DeleteEventByName(EventName_CreatedFromTemplate);

            // Go to Templates folder and delete old template event
            ManagerSiteMgr.SelectFolder_PreBuilt(ManagerSiteManager.PreBuiltFolderName.Templates);
            ManagerSiteMgr.DeleteEventByName(EventName_ToCreateTemplateFrom);
            ManagerSiteMgr.DeleteEventByName(EventName_ToCreateTemplateFrom + " (Copy)");

            // Go back to default folder, drag and drop to create an event template
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();

            if (!ManagerSiteMgr.EventExists(EventName_ToCreateTemplateFrom))
            {
                BuildEventFixture buildEventFixture = new BuildEventFixture();
                buildEventFixture.FullEventBuild();
            }

            ManagerSiteMgr.DragAndDropToCreateTemplate(ManagerSiteMgr.GetFirstEventId(EventName_ToCreateTemplateFrom));

            // Remove the '(Copy)' suffix for the event template
            ManagerSiteMgr.SelectFolder_PreBuilt(ManagerSiteManager.PreBuiltFolderName.Templates);
            ManagerSiteMgr.OpenEventBuilderStartPage(ManagerSiteMgr.GetFirstEventId(EventName_ToCreateTemplateFrom + " (Copy)"), this.sessionId);
            BuilderMgr.SetEventNameAndShortcut(EventName_ToCreateTemplateFrom);
            BuilderMgr.SaveAndClose();

            // Create event from template, register, and verify
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            ManagerSiteMgr.AddEventFromTemplate(EventName_ToCreateTemplateFrom);
            BuilderMgr.SetEventNameAndShortcut(EventName_CreatedFromTemplate);
            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SaveAndClose();
            RegisterMgr.OpenRegisterPage(this.eventID);
            this.RegisterForCopiedOrTemplateEvent();
        }

        [Test]
        [Category(Priority.Two)]
        [Description("581")]
        public void CopyEvent()
        {
            LoginAndGoToRegressionFolder();
            ManagerSiteMgr.DeleteEventByName(CopiedEventName);
            eventID = ManagerSiteMgr.GetFirstEventId(BuildEventFixture.EventName);
            ManagerSiteMgr.OpenEventDashboard(eventID);
            ManagerSiteMgr.DashboardMgr.CopyEventFromDashboard(CopiedEventName);
            LoginAndGoToRegressionFolder();
            this.eventID = ManagerSiteMgr.GetFirstEventId(CopiedEventName);
            ManagerSiteMgr.OpenEventBuilderStartPage(eventID, sessionId);
            this.VerifyEvent(CopiedEventName);
            this.RegisterForCopiedOrTemplateEvent();
        }

        // Related to bug #23438
        [Test]
        [Category(Priority.Two)]
        [Description("371")]
        public void ChangeStatuses()
        {
            this.LoginAndGoToRegressionFolder();
            AccessData.SetLiveRegToTest(ManagerSiteMgr.GetEventIds(ChangeStatusEventName));
            ManagerSiteMgr.DeleteEventByName(ChangeStatusEventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            this.SetEventDetails(ChangeStatusEventName);
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionId);
            ManagerSiteMgr.DashboardMgr.ActiveEvent();
            ManagerSiteMgr.DashboardMgr.ReturnToManagerScreenEventList();

            this.ChangeStatus(DashboardManager.EventStatus.Cancelled);
            this.VerifyStatus(DashboardManager.EventStatus.Cancelled);

            this.ChangeStatus(DashboardManager.EventStatus.Archived);
            this.VerifyStatus(DashboardManager.EventStatus.Archived);

            this.ChangeStatus(DashboardManager.EventStatus.Active);
            this.VerifyStatus(DashboardManager.EventStatus.Active);

            this.ChangeStatus(DashboardManager.EventStatus.Inactive);
            this.VerifyStatus(DashboardManager.EventStatus.Inactive);

            this.ChangeStatus(DashboardManager.EventStatus.OnSite);
            this.VerifyStatus(DashboardManager.EventStatus.OnSite);

            this.ChangeStatus(DashboardManager.EventStatus.SoldOut);
            this.VerifyStatus(DashboardManager.EventStatus.SoldOut);

            this.ChangeStatus(DashboardManager.EventStatus.UpdateOnly);
            this.VerifyStatus(DashboardManager.EventStatus.UpdateOnly);

            this.ChangeStatus(DashboardManager.EventStatus.Testing);
            this.VerifyStatus(DashboardManager.EventStatus.Testing);
        }

        #endregion

        #region manager/event build methods
        [Step]
        private void LoginAndGoToRegressionFolder()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.sessionId = ManagerSiteMgr.GetEventSessionId();
        }

        [Step]
        private void CreateNewEventToActivate()
        {
            this.LoginAndGoToRegressionFolder();
            AccessData.SetLiveRegToTest(ManagerSiteMgr.GetEventIds(GoLiveEventName));
            ManagerSiteMgr.DeleteEventByName(GoLiveEventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            this.SetEventDetails(GoLiveEventName);
        }

        private void SetEventDetails(string eventName)
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(eventName);
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void EventActivation()
        {
            ManagerSiteMgr.OpenEventDashboard(eventID);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.SelectActivateEventFrame();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();
            ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ReturnToManagerScreenEventList();
        }

        [Verify]
        private void VerifyActiveDateInDB()
        {
            var db = new DataAccess.ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);
            var activeDate = (from e in db.Events where e.Id == eventID select e.ActiveDate).ToList();
            string date = activeDate[0].ToString();
            string[] dateAndTime = date.Split(new char[] { ' ' });
            VerifyTool.VerifyValue(DateTime.Today.ToString("d"), dateAndTime[0], "Active Date: {0}");
        }

        private void VerifyEvent(string eventName)
        {
            VerifyStartPage(eventName);
            BuilderMgr.Next();
            VerifyPersonalInfoPage();
            BuilderMgr.Next();
            VerifyAgedaPage();
            BuilderMgr.Next();
            VerifyLAndTPage();
            BuilderMgr.Next();
            VerifyMerchPage();
            BuilderMgr.Next();
            BuilderMgr.PaymentMethodMgr.ClickCreditCardLink();
            BuilderMgr.PaymentMethodMgr.CreditCardOptionsMgr.SelectPaymentGateway(CreditCardOptionsManager.PaymentGateway.RegOnlineGateway);
            BuilderMgr.PaymentMethodMgr.CreditCardOptionsMgr.SaveAndClose();
            VerifyCheckoutPage();
            BuilderMgr.Next();
            VerifyConfirmationPage();
            BuilderMgr.SaveAndClose();
        }

        private void VerifyStartPage(string eventName)
        {
            BuilderMgr.SetEventNameAndShortcut(eventName);
            BuilderMgr.SaveAndStay();
            BuilderMgr.SetStartEndDateTimeDefault();
            BuilderMgr.SaveAndStay();
            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.VerifyStartPageSettingsAreSaved(ManagerSiteManager.EventType.ProEvent, eventName);
            BuilderMgr.VerifyHasRegTypeInDatabase("First");
            BuilderMgr.VerifyHasRegTypeInDatabase("Second");
            BuilderMgr.VerifyHasRegTypeInDatabase("Third");
            BuilderMgr.VerifyHasRegTypeInDatabase("Fourth");
        }

        private void VerifyPersonalInfoPage()
        {
            BuilderMgr.VerifyPersonalInfoPageSettingsAreSaved();

            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.CheckBox, "CF-Checkbox");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.RadioButton, "CF-Radio");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Dropdown, "CF-DropDown");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Number, "CF-Numeric");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.OneLineText, "CF-Text");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Time, "CF-Time");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.SectionHeader, "CF-Header");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.AlwaysSelected, "CF-Always");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.ContinueButton, "CF-Continue");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Paragraph, "CF-Paragraph");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Date, "CF-Date");
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.FileUpload, "CF-File");

            //BuilderMgr.fie
        }

        private void VerifyAgedaPage()
        {
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.CheckBox, "AI-Checkbox");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.RadioButton, "AI-Radio");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Dropdown, "AI-DropDown");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Number, "AI-Numeric");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.OneLineText, "AI-Text");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Time, "AI-Time");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.SectionHeader, "AI-Header");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.AlwaysSelected, "AI-Always");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.ContinueButton, "AI-Continue");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Paragraph, "AI-Paragraph");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Date, "AI-Date");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.FileUpload, "AI-File");
            BuilderMgr.VerifyAgendaItemInDatabase(Managers.Builder.AgendaItemManager.AgendaItemType.Contribution, "AI-Contribution");

            BuilderMgr.VerifyFormView();
        }

        private void VerifyLAndTPage()
        {
            BuilderMgr.VerifyEventLodgingTravelPage();

            // verify lodging custom fields of each type
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.CheckBox, "LDG-Checkbox");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.RadioButton, "LDG-Radio");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Dropdown, "LDG-DropDown");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Number, "LDG-Numeric");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.OneLineText, "LDG-Text");
            BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Time, "LDG-Time");
            ////BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.SectionHeader, "LDG-Header");
            ////BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.AlwaysSelected, "LDG-Always");
            ////BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.ContinueButton, "LDG-Continue");
            ////BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Paragraph, "LDG-Paragraph");
            ////BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Date, "LDG-Date");
            ////BuilderMgr.VerifyLodgingCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.FileUpload, "LDG-File");

            // verify travel custom fields of each type
            ////BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.CheckBox, "TRV-Checkbox");
            ////BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.RadioButton, "TRV-Radio");
            ////BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Dropdown, "TRV-DropDown");
            ////BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Number, "TRV-Numeric");
            ////BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.OneLineText, "TRV-Text");
            ////BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Time, "TRV-Time");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.SectionHeader, "TRV-Header");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.AlwaysSelected, "TRV-Always");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.ContinueButton, "TRV-Continue");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Paragraph, "TRV-Paragraph");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.Date, "TRV-Date");
            BuilderMgr.VerifyTravelCustomFieldInDatabase(Managers.Builder.CustomFieldManager.CustomFieldType.FileUpload, "TRV-File");
        }

        private void VerifyMerchPage()
        {
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Header, "FEE-Header");
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Fixed, "FEE-Fixed");
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Variable, "FEE-Variable");
        }

        private void VerifyCheckoutPage()
        {
            BuilderMgr.VerifyEventCheckoutPage();
        }

        private void VerifyConfirmationPage()
        {
            BuilderMgr.VerifyEventConfirmationPage();
        }
        #endregion

        #region register methods
        private void RegisterForCopiedOrTemplateEvent()
        {
            this.helper.Checkin(eventID);
            this.helper.PersonalInfo();
            this.helper.Agenda();
            this.helper.LAndT();
            this.helper.Merchandise();
            this.helper.Finish();
        }

        private void RegisterForGoLiveEvent()
        {
            RegisterMgr.Checkin();
            emailAddr = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void NoNewRegs()
        {
            UIUtil.DefaultProvider.ClearCookiesAndRestart();
            RegisterMgr.OpenRegisterPage(eventID);
            RegisterMgr.EnterEmailAddress("no" + System.DateTime.Now.Ticks + "@bademail.com");
            RegisterMgr.Continue();
            RegisterMgr.CheckErrorMessage(RegisterManager.Error.IncorrectLogin, true); 
        }
        #endregion

        [Step]
        private void ChangeStatus(DashboardManager.EventStatus status)
        {
            ManagerSiteMgr.OpenEventDashboard(eventID);
            ManagerSiteMgr.DashboardMgr.ChooseTabAndVerify(DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ChangeStatusIfNecessary(status, status.ToString());
        }

        [Verify]
        private void VerifyStatus(DashboardManager.EventStatus status)
        {
            UIUtil.DefaultProvider.ClearCookiesAndRestart();
            LoginAndGoToRegressionFolder();
            ManagerSiteMgr.OpenEventDashboard(eventID);
            ManagerSiteMgr.DashboardMgr.ChooseTabAndVerify(DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.VerifyEventStatus(status);
            
            switch (status)
            {
                case DashboardManager.EventStatus.Active:
                    UIUtil.DefaultProvider.OpenUrl(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "Register/Checkin.aspx?EventID=" + eventID + "&O=" + Guid.NewGuid());
                    UIUtil.DefaultProvider.RefreshPage();
                    VerifyTool.VerifyValue(false, !UIUtil.DefaultProvider.IsElementHidden(TestModeLocator, LocateBy.Id), "In test mode");
                    ////VerifyTool.VerifyValue(false, !UIUtil.DefaultProvider.IsElementDisplay(TestModeLocator, LocateBy.Id), "Verify 'In test mode' display:{0}");
                    RegisterForGoLiveEvent();
                    break;
                case DashboardManager.EventStatus.Archived:
                    UIUtil.DefaultProvider.OpenUrl(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "Register/Checkin.aspx?EventID=" + eventID + "&P=" + Guid.NewGuid());
                    UIUtil.DefaultProvider.RefreshPage();
                    VerifyTool.VerifyValue(true, RegisterMgr.VerifyEventIsArchivedStatus(), "Archived Page is not displayed!");
                    break;
                case DashboardManager.EventStatus.Cancelled:
                    UIUtil.DefaultProvider.OpenUrl(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "Register/Checkin.aspx?EventID=" + eventID + "&Q=" + Guid.NewGuid());
                    UIUtil.DefaultProvider.RefreshPage();
                    VerifyTool.VerifyValue(false, RegisterMgr.DoesCheckinAcceptEmail(), "Email Address is visible!");
                    break;
                case DashboardManager.EventStatus.Inactive:
                    UIUtil.DefaultProvider.OpenUrl(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "Register/Checkin.aspx?EventID=" + eventID + "&R=" + Guid.NewGuid());
                    UIUtil.DefaultProvider.RefreshPage();
                    VerifyTool.VerifyValue(false, RegisterMgr.DoesCheckinAcceptEmail(), "Email Address is visible!");
                    break;
                case DashboardManager.EventStatus.OnSite:
                    UIUtil.DefaultProvider.OpenUrl(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "Register/Checkin.aspx?EventID=" + eventID + "&S=" + Guid.NewGuid());
                    UIUtil.DefaultProvider.RefreshPage();
                    VerifyTool.VerifyValue(false, RegisterMgr.DoesCheckinAcceptEmail(), "Email Address is visible!");
                    break;
                case DashboardManager.EventStatus.SoldOut:
                    UIUtil.DefaultProvider.OpenUrl(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "Register/Checkin.aspx?EventID=" + eventID + "&T=" + Guid.NewGuid());
                    UIUtil.DefaultProvider.RefreshPage();
                    VerifyTool.VerifyValue(false, RegisterMgr.DoesCheckinAcceptEmail(), "Email Address is visible!");
                    break;
                case DashboardManager.EventStatus.Testing:
                    UIUtil.DefaultProvider.OpenUrl(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "Register/Checkin.aspx?EventID=" + eventID + "&U=" + Guid.NewGuid());
                    UIUtil.DefaultProvider.RefreshPage();
                    VerifyTool.VerifyValue(true, !UIUtil.DefaultProvider.IsElementHidden(TestModeLocator, LocateBy.Id), "Not in test mode");
                    VerifyTool.VerifyValue(true, RegisterMgr.DoesCheckinAcceptEmail(), "Email Address is NOT visible!");
                    break;
                case DashboardManager.EventStatus.UpdateOnly:
                    UIUtil.DefaultProvider.OpenUrl(ConfigReader.DefaultProvider.AccountConfiguration.BaseUrl + "Register/Checkin.aspx?EventID=" + eventID + "&V=" + Guid.NewGuid());
                    int count = 0;
                    while (count < 5)
                    {
                        if (RegisterMgr.DoesCheckinAcceptEmail())
                        {
                            break;
                        }
                        else
                        {
                            UIUtil.DefaultProvider.RefreshPage();
                            count++;
                        }
                    }
                    VerifyTool.VerifyValue(true, RegisterMgr.DoesCheckinAcceptEmail(), "Email Address is NOT visible!");
                    this.helper.UpdateRegistration(emailAddr);
                    NoNewRegs();
                    break;
            }

            LoginAndGoToRegressionFolder();
        }
    }
}
