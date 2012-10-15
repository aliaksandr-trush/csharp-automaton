namespace RegOnline.RegressionTest.Fixtures.Manager.EventDashboard.OnSite
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Managers.Manager.SelfRegistrationKiosk;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Managers.Report;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class SelfRegKioskFixture : FixtureBase
    {
        private const string EventName = "Self-Registration Kiosk";
        private const string PaidInFullMessage = "You must be paid in full";

        private enum KioskType
        {
            SearchReg,
            RequirePassword,
            UpdateProfile,
            RequirePaidInFull,
            AllowOnSiteReg,
            AllowGroupCheckin
        }

        private List<OnSiteFixtureHelper.RegistrationInfo> regs = new List<OnSiteFixtureHelper.RegistrationInfo>();
        private OnSiteFixtureHelper helper = new OnSiteFixtureHelper();
        private int eventId;
        private string eventSessionId;

        [Test]
        [Category(Priority.One)]
        [Description("340")]
        public void SelfRegKiosk_SearchReg()
        {
            this.PrepareEvent();
            this.CreateRegistrations(KioskType.SearchReg);
            this.KioskTest(KioskType.SearchReg);
            this.VerifyStatusInAttendeeReport(KioskType.SearchReg);
        }

        [Test]
        [Category(Priority.One)]
        [Description("1336")]
        public void SelfRegKiosk_RequirePassword()
        {
            this.PrepareEvent();
            this.CreateRegistrations(KioskType.RequirePassword);
            this.KioskTest(KioskType.RequirePassword);
            this.VerifyStatusInAttendeeReport(KioskType.RequirePassword);
        }

        [Test]
        [Category(Priority.One)]
        [Description("1337")]
        public void SelfRegKiosk_UpdateProfile()
        {
            this.PrepareEvent();
            this.CreateRegistrations(KioskType.UpdateProfile);
            this.KioskTest(KioskType.UpdateProfile);
            this.VerifyStatusInAttendeeReport(KioskType.UpdateProfile);
        }

        [Test]
        [Category(Priority.One)]
        [Description("1339")]
        public void SelfRegKiosk_RequirePaidInFull()
        {
            this.PrepareEvent();
            this.CreateRegistrations(KioskType.RequirePaidInFull);
            this.KioskTest(KioskType.RequirePaidInFull);
            this.VerifyStatusInAttendeeReport(KioskType.RequirePaidInFull);
        }

        [Test]
        [Category(Priority.One)]
        [Description("1340")]
        public void SelfRegKiosk_AllowOnSiteReg()
        {
            this.PrepareEvent();
            this.CreateRegistrations(KioskType.AllowOnSiteReg);
            this.KioskTest(KioskType.AllowOnSiteReg);
            this.VerifyStatusInAttendeeReport(KioskType.AllowOnSiteReg);
        }

        [Test]
        [Category(Priority.One)]
        [Description("1341")]
        public void SelfRegKiosk_AllowGroupCheckin()
        {
            this.PrepareEvent();
            this.CreateRegistrations(KioskType.AllowGroupCheckin);
            this.KioskTest(KioskType.AllowGroupCheckin);
            this.VerifyStatusInAttendeeReport(KioskType.AllowGroupCheckin);
        }

        private void PrepareEvent()
        {
            this.eventSessionId = this.helper.Login();

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.CreateEvent();
            }

            this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);

            ManagerSiteMgr.OpenEventDashboard(this.eventId);
            DataHelperTool.ChangeAllRegsToTestForEvent(this.eventId);
            ManagerSiteMgr.DashboardMgr.DeleteTestRegs();
            ManagerSiteMgr.DashboardMgr.ChangeStatusIfNecessary(DashboardManager.EventStatus.Active);
        }

        private void KioskTest(KioskType type)
        {
            this.eventSessionId = this.helper.Login();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);
            ManagerSiteMgr.DashboardMgr.ChangeStatusIfNecessary(DashboardManager.EventStatus.OnSite);

            switch (type)
            {
                /// <summary>
                /// Enable barcode scanning / search by Registration ID      : true
                /// Require registrants to be paid in full prior to check-in : false
                /// Require paid in full - AllowCC/DisplayMessage            : false
                /// Require authentication                                   : false
                /// Allow on-site registrations                              : false
                /// Allow registrations to update profile                    : false
                /// Allow group check-in                                     : true
                /// Allow on-site badge printing                             : false
                /// 
                /// We're not able to handle badge for the moment, so do not allow to print badge
                /// </summary>
                case KioskType.SearchReg:
                    ManagerSiteMgr.DashboardMgr.LaunchKiosk(true, false, false, null, false, false, false, true, false);

                    RegisterMgr.KioskSearch(this.regs[0].emailAddress);
                    RegisterMgr.KioskClickCheckinReg();
                    RegisterMgr.FinishCheckin();

                    // If searched with registration id, the registrant will be checked in immediately,
                    // you don't have to call 'RegisterMgr.KioskClickCheckinReg();'
                    RegisterMgr.KioskSearch(this.regs[1].regId.ToString());

                    RegisterMgr.FinishCheckin();
                    RegisterMgr.KioskSearch(this.regs[2].fullName);
                    RegisterMgr.KioskClickNewSearch();
                    RegisterMgr.KioskSearch(this.regs[3].fullName);
                    RegisterMgr.KioskClickCheckinReg();
                    RegisterMgr.FinishCheckin();

                    UIUtil.DefaultProvider.ClosePopUpWindow();
                    break;

                /// <summary>
                /// Enable barcode scanning / search by Registration ID      : true
                /// Require registrants to be paid in full prior to check-in : false
                /// Require paid in full - AllowCC/DisplayMessage            : false
                /// Require authentication                                   : true
                /// Allow on-site registrations                              : false
                /// Allow registrations to update profile                    : false
                /// Allow group check-in                                     : true
                /// Allow on-site badge printing                             : false
                /// </summary>
                case KioskType.RequirePassword:
                    ManagerSiteMgr.DashboardMgr.LaunchKiosk(true, false, false, null, true, false, false, true, false);

                    RegisterMgr.KioskSearch(this.regs[0].emailAddress);
                    RegisterMgr.KioskEnterPassword();
                    RegisterMgr.KioskClickNewSearch();
                    RegisterMgr.KioskSearch(this.regs[0].emailAddress);
                    RegisterMgr.KioskEnterPassword();
                    RegisterMgr.KioskClickCheckinReg();
                    RegisterMgr.FinishCheckin();

                    UIUtil.DefaultProvider.ClosePopUpWindow();
                    break;

                /// <summary>
                /// Enable barcode scanning / search by Registration ID      : true
                /// Require registrants to be paid in full prior to check-in : false
                /// Require paid in full - AllowCC/DisplayMessage            : false
                /// Require authentication                                   : true
                /// Allow on-site registrations                              : false
                /// Allow registrations to update profile                    : true
                /// Allow group check-in                                     : true
                /// Allow on-site badge printing                             : false
                /// </summary>
                case KioskType.UpdateProfile:
                    ManagerSiteMgr.DashboardMgr.LaunchKiosk(true, false, false, null, true, false, true, true, false);

                    RegisterMgr.KioskSearch(this.regs[0].regId.ToString());
                    RegisterMgr.KioskEnterPassword();
                    RegisterMgr.KioskClickChangeIndividualRegistrationInfo(this.regs[0].emailAddress);
                    RegisterMgr.KioskUpdateRegistrationInfo(RegisterManager.KioskPersonalInfo.FirstName, "First Name");
                    RegisterMgr.KioskUpdateRegistrationInfo(RegisterManager.KioskPersonalInfo.LastName, "Last Name");
                    RegisterMgr.KioskUpdateRegistrationInfo(RegisterManager.KioskPersonalInfo.Address1, "Address1");
                    RegisterMgr.KioskUpdateRegistrationInfo(RegisterManager.KioskPersonalInfo.City, "City");
                    RegisterMgr.KioskUpdateRegistrationInfo(RegisterManager.KioskPersonalInfo.State, "Idaho");
                    RegisterMgr.KioskUpdateRegistrationInfo(RegisterManager.KioskPersonalInfo.Zip, "83712");
                    RegisterMgr.KioskUpdateRegistrationInfo(RegisterManager.KioskPersonalInfo.Company, "Company");
                    RegisterMgr.KioskSaveUpdate();
                    RegisterMgr.KioskClickCheckinReg();
                    RegisterMgr.FinishCheckin();

                    UIUtil.DefaultProvider.ClosePopUpWindow();
                    break;

                /// <summary>
                /// Enable barcode scanning / search by Registration ID      : true
                /// Require registrants to be paid in full prior to check-in : true
                /// Require paid in full - AllowCC/DisplayMessage            : false/true
                /// Require authentication                                   : true
                /// Allow on-site registrations                              : false
                /// Allow registrations to update profile                    : true
                /// Allow group check-in                                     : true
                /// Allow on-site badge printing                             : false
                /// </summary>
                case KioskType.RequirePaidInFull:
                    ManagerSiteMgr.DashboardMgr.LaunchKiosk(true, true, false, PaidInFullMessage, true, false, true, true, false);

                    RegisterMgr.KioskSearch(this.regs[0].fullName);
                    RegisterMgr.KioskEnterPassword();
                    RegisterMgr.GetPaidInFullMessage();

                    UIUtil.DefaultProvider.ClosePopUpWindow();

                    this.eventSessionId = this.helper.Login();
                    ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);

                    ManagerSiteMgr.DashboardMgr.LaunchKiosk(true, true, true, null, true, true, true, true, false);
                    RegisterMgr.KioskSearch(this.regs[0].fullName);
                    RegisterMgr.KioskEnterPassword();
                    RegisterMgr.KioskEnterCCInfoToPayInFull();
                    RegisterMgr.KioskClickMakePaymentButton();
                    RegisterMgr.KioskClickCheckinReg();
                    RegisterMgr.FinishCheckin();

                    UIUtil.DefaultProvider.ClosePopUpWindow();
                    break;

                /// <summary>
                /// Enable barcode scanning / search by Registration ID      : true
                /// Require registrants to be paid in full prior to check-in : true
                /// Require paid in full - AllowCC/DisplayMessage            : true
                /// Require authentication                                   : true
                /// Allow on-site registrations                              : true
                /// Allow registrations to update profile                    : true
                /// Allow group check-in                                     : true
                /// Allow on-site badge printing                             : false
                /// </summary>
                case KioskType.AllowOnSiteReg:
                    ManagerSiteMgr.DashboardMgr.LaunchKiosk(true, true, true, PaidInFullMessage, true, true, true, true, false);

                    RegisterMgr.KioskStartOnsiteRegistration();
                    this.KioskUpdateWithStartOver();
                    RegisterMgr.KioskStartOnsiteRegistration();
                    this.KioskUpdateReg();
                    RegisterMgr.FinishCheckin();

                    UIUtil.DefaultProvider.ClosePopUpWindow();
                    break;

                /// <summary>
                /// Enable barcode scanning / search by Registration ID      : true
                /// Require registrants to be paid in full prior to check-in : true
                /// Require paid in full - AllowCC/DisplayMessage            : true
                /// Require authentication                                   : true
                /// Allow on-site registrations                              : true
                /// Allow registrations to update profile                    : true
                /// Allow group check-in                                     : true
                /// Allow on-site badge printing                             : false
                /// </summary>
                case KioskType.AllowGroupCheckin:
                    ManagerSiteMgr.DashboardMgr.LaunchKiosk(true, true, true, PaidInFullMessage, true, true, true, true, false);

                    RegisterMgr.KioskSearch(this.regs[0].regId.ToString());
                    RegisterMgr.KioskEnterPassword();
                    RegisterMgr.KioskUnselectGroupMemberToCheckin(this.regs[2].emailAddress);
                    RegisterMgr.KioskClickCheckinReg();
                    RegisterMgr.FinishCheckin();

                    RegisterMgr.KioskSearch(this.regs[2].regId.ToString());
                    RegisterMgr.KioskEnterPassword();
                    RegisterMgr.KioskClickCheckinReg();
                    RegisterMgr.FinishCheckin();

                    UIUtil.DefaultProvider.ClosePopUpWindow();
                    break;

                default:
                    break;
            }
        }

        private void CreateRegistrations(KioskType type)
        {
            this.regs = new List<OnSiteFixtureHelper.RegistrationInfo>();

            switch (type)
            {
                case KioskType.SearchReg:

                    for (int i = 0; i < 4; i++)
                    {
                        this.SingleRegistration(RegisterManager.PaymentMethod.CreditCard, 20.00);
                    }

                    break;

                case KioskType.RequirePassword:
                    this.SingleRegistration(RegisterManager.PaymentMethod.CreditCard, 20.00);
                    break;

                case KioskType.UpdateProfile:
                    this.SingleRegistration(RegisterManager.PaymentMethod.CreditCard, 20.00);
                    break;

                case KioskType.RequirePaidInFull:
                    this.SingleRegistration(RegisterManager.PaymentMethod.Check, 20.00);
                    break;

                case KioskType.AllowOnSiteReg:
                    this.SingleRegistration(RegisterManager.PaymentMethod.CreditCard, 20.00);
                    break;

                case KioskType.AllowGroupCheckin:
                    this.GroupRegistration();
                    break;

                default:
                    break;
            }
        }

        public void SingleRegistration(RegisterManager.PaymentMethod paymentMethod, double subTotal)
        {
            OnSiteFixtureHelper.RegistrationInfo regInfo = new OnSiteFixtureHelper.RegistrationInfo();
            this.CheckinPage("One");
            regInfo.emailAddress = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            this.PersonalInfoPage();
            regInfo.fullName = RegisterMgr.CurrentRegistrantFullName;
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            this.helper.CheckoutPage(paymentMethod, subTotal);
            RegisterMgr.ConfirmRegistration();

            regInfo.regId = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.RegistrationId));

            this.regs.Add(regInfo);
        }

        public void GroupRegistration()
        {
            string primaryEmail = string.Empty;
            RegisterMgr.OpenRegisterPage(this.eventId);
            this.IndividualGroupMember();
            primaryEmail = RegisterMgr.CurrentEmail;
            RegisterMgr.ClickAddAnotherPerson();
            this.IndividualGroupMember();
            RegisterMgr.ClickAddAnotherPerson();
            this.IndividualGroupMember();
            RegisterMgr.Continue();
            this.MerchandisePage();
            RegisterMgr.Continue();
            RegisterMgr.CurrentEmail = primaryEmail;
            this.helper.CheckoutPage(RegisterManager.PaymentMethod.CreditCard, 180.00);
            RegisterMgr.ConfirmRegistration();
            Utility.ThreadSleep(2);

            this.regs[0].regId = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.RegistrationId));

            RegisterMgr.UnfoldGroupMember(0);

            this.regs[1].regId = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForGroupMembers(RegisterManager.ConfirmationPageField.RegistrationId, 0));

            RegisterMgr.UnfoldGroupMember(1);

            this.regs[2].regId = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForGroupMembers(RegisterManager.ConfirmationPageField.RegistrationId, 1));
        }

        public void IndividualGroupMember()
        {
            OnSiteFixtureHelper.RegistrationInfo regInfo = new OnSiteFixtureHelper.RegistrationInfo();

            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType("One");
            regInfo.emailAddress = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            this.PersonalInfoPage();
            regInfo.fullName = RegisterMgr.CurrentRegistrantFullName;
            RegisterMgr.Continue();
            this.AgendaPage();
            this.regs.Add(regInfo);
        }

        public void KioskUpdateWithStartOver()
        {
            RegisterMgr.Checkin(this.regs[0].emailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterEmailAddress(this.regs[0].emailAddress);
            RegisterMgr.EnterPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
            RegisterMgr.Continue();
            RegisterMgr.ClickEditPersonalInformationLink(0);
            RegisterMgr.SetCustomFieldCheckBox("CF-Checkbox", true);
            RegisterMgr.SelectCustomFieldRadioButtons("CF-Radio", "Yes");
            RegisterMgr.SelectCustomFieldDropDown("CF-DropDown", "Agree");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("CF-Numeric", "55");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("CF-Text", "Some Text");
            RegisterMgr.FillOutCustomTimeField("CF-Time", "12:00 PM");
            RegisterMgr.FillOutCustomDateField("CF-Date", "02/18/2011");
            RegisterMgr.FillOutCustomParagraphField("CF-Paragraph", "Some more information that is in a paragraph field");
            RegisterMgr.Continue();
            RegisterMgr.ClickEditAgendaLink(0);
            this.AgendaPage();
            RegisterMgr.Continue();
            RegisterMgr.KioskStartOverButton();
        }

        public void KioskUpdateReg()
        {
            RegisterMgr.Checkin(this.regs[0].emailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterEmailAddress(this.regs[0].emailAddress);
            RegisterMgr.EnterPassword();
            RegisterMgr.Continue();
            RegisterMgr.ClickEditPersonalInformationLink(0);
            RegisterMgr.SetCustomFieldCheckBox("CF-Checkbox", true);
            RegisterMgr.SelectCustomFieldRadioButtons("CF-Radio", "Yes");
            RegisterMgr.SelectCustomFieldDropDown("CF-DropDown", "Agree");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("CF-Numeric", "55");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("CF-Text", "Some Text");
            RegisterMgr.FillOutCustomTimeField("CF-Time", "12:00 PM");
            RegisterMgr.FillOutCustomDateField("CF-Date", "02/18/2011");
            RegisterMgr.FillOutCustomParagraphField("CF-Paragraph", "Some more information that is in a paragraph field");
            RegisterMgr.Continue();
            RegisterMgr.ClickEditAgendaLink(0);
            this.AgendaPage();
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            this.MerchandisePage();
            RegisterMgr.Continue();

            RegisterMgr.PaymentMgr.EnterCreditCardNumberInfo(
                PaymentManager.DefaultPaymentInfo.CCNumber,
                PaymentManager.DefaultPaymentInfo.CVV,
                PaymentManager.DefaultPaymentInfo.ExpirationMonth,
                PaymentManager.DefaultPaymentInfo.ExpirationYear);

            RegisterMgr.PaymentMgr.EnterCreditCardNameCountryType(
                PaymentManager.DefaultPaymentInfo.HolderName,
                PaymentManager.DefaultPaymentInfo.HolderCountry,
                null);

            RegisterMgr.PaymentMgr.EnterCreditCardAddressInfo(
                PaymentManager.DefaultPaymentInfo.BillingAddressLineOne,
                null,
                PaymentManager.DefaultPaymentInfo.BillingCity,
                PaymentManager.DefaultPaymentInfo.BillingState,
                PaymentManager.DefaultPaymentInfo.ZipCode);

            RegisterMgr.FinishRegistration();
        }

        public void CheckinPage(string regTypeName)
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(regTypeName);
        }

        public void PersonalInfoPage()
        {
            RegisterMgr.SetDefaultStandardPersonalInfoFields();
            RegisterMgr.EnterPersonalInfoPassword();
        }

        public void AgendaPage()
        {
            RegisterMgr.SetCustomFieldCheckBox("AI-Checkbox", true);
            RegisterMgr.SelectCustomFieldRadioButtons("AI-Radio", "Yes");
            RegisterMgr.SelectCustomFieldDropDown("AI-DropDown", "Agree");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("AI-Numeric", "55");
            RegisterMgr.FillOutCustomTimeField("AI-Time", "12:00 PM");
            RegisterMgr.FillOutCustomDateField("AI-Date", "02/18/2011");
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution("AI-Text", "Some Text");
            RegisterMgr.FillOutCustomParagraphField("AI-Paragraph", "Some more information that is in a paragraph field");
        }

        public void MerchandisePage()
        {
            RegisterMgr.SelectMerchandiseQuantityByName("FEE-Fixed", 2);
            RegisterMgr.EnterMerchandiseVariableAmountByName("FEE-Variable", 10.00);
        }

        [Verify]
        private void VerifyStatusInAttendeeReport(KioskType type)
        {
            ManagerSiteMgr.SelectManagerWindow();
            this.eventSessionId = ManagerSiteMgr.Login();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);
            ManagerSiteMgr.DashboardMgr.OpenAttendeeReportFromEventDashboard();

            switch (type)
            {
                case KioskType.SearchReg:

                    for (int index = 0; index < this.regs.Count; index++)
                    {
                        if (index == 2)
                        {
                            ReportMgr.VerifyRegistrationStatusOnAttendeeReport(
                                this.regs[index].regId,
                                ReportManager.AttendeeStatus.Confirmed);
                        }
                        else
                        {
                            ReportMgr.VerifyRegistrationStatusOnAttendeeReport(
                                this.regs[index].regId,
                                ReportManager.AttendeeStatus.Attended);
                        }
                    }

                    break;

                case KioskType.RequirePassword:
                case KioskType.UpdateProfile:
                case KioskType.RequirePaidInFull:
                case KioskType.AllowOnSiteReg:

                    ReportMgr.VerifyRegistrationStatusOnAttendeeReport(
                        this.regs[0].regId,
                        ReportManager.AttendeeStatus.Attended);

                    break;

                case KioskType.AllowGroupCheckin:

                    for (int index = 0; index < this.regs.Count; index++)
                    {
                        ReportMgr.VerifyRegistrationStatusOnAttendeeReport(
                            this.regs[index].regId,
                            ReportManager.AttendeeStatus.Attended);
                    }

                    break;

                default:
                    break;
            }

            ReportMgr.CloseReportPopupWindow();
            ManagerSiteMgr.SelectManagerWindow();
            ManagerSiteMgr.DashboardMgr.ReturnToManagerScreenEventList();
        }

        [Step]
        public void CreateEvent()
        {
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.helper.SetupStartPage(EventName);
            BuilderMgr.Next();
            this.helper.SetupPersonalInfoPage();
            BuilderMgr.Next();
            this.helper.SetupAgendaPage();
            BuilderMgr.Next();
            BuilderMgr.Next();
            this.helper.SetupMerchandisePage();
            BuilderMgr.Next();
            this.helper.SetupCheckoutPage();
            BuilderMgr.Next();
            this.helper.SetupConfirmationPage();
            BuilderMgr.SaveAndClose();
            this.helper.Login();
            this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            ManagerSiteMgr.OpenEventDashboard(eventId);
            ManagerSiteMgr.DashboardMgr.ClickManagerActivateEventButton();
            ManagerSiteMgr.DashboardMgr.ActivateEventMgr.ActivateEvent();
            ////throw new Exception("The Event Is Built, but a badge needs to be manually added, please do so then re-run test");
        }
    }
}
