namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    public class UpdatesFixture : ExternalAuthenticationFixtureBase
    {
        private const string EventName = "UpdatesFixture";
        private const string DescriptionForIdentifer = "User ID";
        private const string AgendaName = "agengda1";
        private const string MerchandiseName = "merchandise1";
        private const double MerchandiseFee = 10.0;
        private const int MerchandiseQuantity1 = 1;
        private const int MerchandiseQuantity2 = 2;
        private const string HotelName = "XAuth Updates Hotel";
        private const string RoomType1 = "RoomType1";
        private const double RoomType1Rate = 100.0;
        private const string RoomType2 = "RoomType2";
        private const double RoomType2Rate = 200.0;
        private const string JobTitle1 = "Job Title 1";
        private const string AddressLineOne1 = "Address Line One 1";
        private const string JobTitle2 = "Job Title 2";
        private const string AddressLineOne2 = "Address Line One 2";
        private const string WrongUserID = "WrongUserID";
        private const string WrongPassword = "654321";
        private const string InputPassword = "123456";
        
        private List<String> errorMessageList = new List<string>();
        private int eventId;
        private FormData.XAuthType currentType = FormData.XAuthType.NotUse;

        [Test]
        public void UpdatesTest()
        {
            //remove current customer's xAuth registers first
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();

            RegisterManager.XAuthPersonalInfo personalInfo1 = RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1];
            RegisterManager.XAuthPersonalInfo personalInfo2 = (RegisterManager.XAuthPersonalInfo)personalInfo1.Clone();
            personalInfo2.JobTitle = RegisterManager.DefaultPersonalInfo.JobTitle + " New";
            personalInfo2.Address1 = RegisterManager.DefaultPersonalInfo.AddressLineOne + " New";
            personalInfo2.Address2 = RegisterManager.DefaultPersonalInfo.AddressLineTwo + " New";
            personalInfo2.City = personalInfo2.City + " New";
            personalInfo2.Company = RegisterManager.DefaultPersonalInfo.Company + " New";
            personalInfo2.State = "Alaska";
            personalInfo2.Phone = "322.222.2222";
            personalInfo2.Zip = "22222";
            personalInfo2.Extension = "222";
            personalInfo2.Fax = "322.222.2222";
           

            this.errorMessageList.Add("You must sign in with the correct values.");

            //Test Case 19     
            this.currentType = FormData.XAuthType.ByUserName;

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_UserDefined("xAuth");
            ManagerSiteMgr.DeleteEventByName(EventName);

            //create existing email address event
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(this.currentType);
            Managers.ManagerProvider.XAuthMgr.TypeDescriptionForIdentifer(DescriptionForIdentifer);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(this.currentType);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyPassTest();
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
            BuilderMgr.ClickYesOnSplashPage();
            //Agenda
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.CheckBox, AgendaName);
            BuilderMgr.GotoPage(FormDetailManager.Page.LodgingTravel);
            BuilderMgr.ClickYesOnSplashPage();
            //Lodging & Travel
            BuilderMgr.ClickAddHotel();
            BuilderMgr.HotelMgr.ClickHotelTemplateLink();
            BuilderMgr.HotelMgr.HotelTemplateMgr.TypeName(HotelName);
            BuilderMgr.HotelMgr.HotelTemplateMgr.AddNewRoomType(RoomType1, RoomType1Rate);
            BuilderMgr.HotelMgr.HotelTemplateMgr.AddNewRoomType(RoomType2, RoomType2Rate);
            BuilderMgr.HotelMgr.HotelTemplateMgr.SaveAndClose();
            Utility.ThreadSleep(3);
            BuilderMgr.HotelMgr.SaveAndClose();
            BuilderMgr.LodgingStandardFieldsMgr.SetRoomType(true, false);
            BuilderMgr.Next();
            //Merchandise
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Fixed, MerchandiseName, MerchandiseFee, null, null);
            BuilderMgr.Next();
            //Checkout
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndClose();

            //19.1             
            CreateRegistration(personalInfo1);

            //19.2 19.3
            GoToAttendeeCheckPageAndVerify();

            //19.4 19.5
            UpdateRegistrationAndVerify(personalInfo2);

            //19.6
            VerifyViewPrintOrEmailLink();

            //19.7
            VerifyCancelLink();

            //19.8
            this.currentType = FormData.XAuthType.ByUserNamePassword;
            SetXAuthType();

            //19.9
            CreateRegistration(personalInfo1);

            //19.10
            this.errorMessageList[0] = "Authentication failure. Please try again.";
            GoToAttendeeCheckPageAndVerify();
            UpdateRegistrationAndVerify(personalInfo2);
            this.errorMessageList[0] = "Authentication failure. Please try again.";
            VerifyViewPrintOrEmailLink();
            this.errorMessageList[0] = "Authentication failure. Please try again.";
            VerifyCancelLink();

            //19.11
            this.currentType = FormData.XAuthType.ByEmail;
            SetXAuthType();

            //19.12
            CreateRegistration(personalInfo1);

            //19.13
            GoToAttendeeCheckPageAndVerify();
            UpdateRegistrationAndVerify(personalInfo2);
            VerifyViewPrintOrEmailLink();
            this.errorMessageList[0] = "You must sign in with the correct values.";
            VerifyCancelLink();

            //19.14
            this.currentType = FormData.XAuthType.ByEmailPassword;
            SetXAuthType();

            //19.15
            CreateRegistration(personalInfo1);

            //19.16
            this.errorMessageList[0] = "Authentication failure. Please try again.";
            GoToAttendeeCheckPageAndVerify();
            UpdateRegistrationAndVerify(personalInfo2);
            VerifyViewPrintOrEmailLink();
            VerifyCancelLink();
        }

        [Verify]
        public void VerifyFailWithWrongInfoInLoginPage()
        {
            switch (this.currentType)
            {
                case FormData.XAuthType.ByUserName:
                case FormData.XAuthType.ByUserNamePassword: 
                    RegisterMgr.XAuth_TypeUserId(Managers.ManagerProvider.XAuthMgr.DefaultAccount_UserName);
                    RegisterMgr.TypeLoginPagePassword(WrongPassword);
                    RegisterMgr.Continue();
                    RegisterMgr.VerifyErrorMessages(this.errorMessageList);
                    RegisterMgr.XAuth_TypeUserId(WrongUserID);
                    RegisterMgr.TypeLoginPagePassword(InputPassword);
                    RegisterMgr.Continue();
                    this.errorMessageList[0] = "You must sign in with the correct values.";
                    RegisterMgr.VerifyErrorMessages(this.errorMessageList);
                    break;
                case FormData.XAuthType.ByEmail:
                case FormData.XAuthType.ByEmailPassword:
                    RegisterMgr.TypeLoginPagePassword(WrongPassword);
                    RegisterMgr.Continue();
                    RegisterMgr.VerifyErrorMessages(this.errorMessageList);
                    break;
                default: break;
            }
        }

        [Verify]
        private void GoToAttendeeCheckPageAndVerify()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.EnterEmailAddress(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.XAuth_VerifyEmailAddressTextboxContentInLoginPage(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.XAuth_VerifyPasswordLabelVisibilityInLoginPage(true);
            RegisterMgr.XAuth_VerifyPasswordTextboxVisibilityInLoginPage(true);
            if (!RegisterMgr.OnLoginPage())
            {
                Assert.Fail("Not on login page!");
            }
            VerifyFailWithWrongInfoInLoginPage();
            switch (this.currentType)
            {
                case FormData.XAuthType.ByUserName:
                case FormData.XAuthType.ByUserNamePassword:
                    RegisterMgr.XAuth_VerifyUserIdLabel(DescriptionForIdentifer + ":");
                    RegisterMgr.XAuth_VerifyUserIDTextboxVisibility(true);
                    RegisterMgr.XAuth_TypeUserId(Managers.ManagerProvider.XAuthMgr.DefaultAccount_UserName);
                    break;
                case FormData.XAuthType.ByEmail:
                case FormData.XAuthType.ByEmailPassword:
                    RegisterMgr.XAuth_VerifyUserIDTextboxVisibility(false);
                    break;
                default: break;
            }
            switch (this.currentType)
            {
                case FormData.XAuthType.ByEmail:
                case FormData.XAuthType.ByUserName:
                    RegisterMgr.TypeLoginPagePassword(InputPassword);
                    break;
                case FormData.XAuthType.ByEmailPassword:
                case FormData.XAuthType.ByUserNamePassword:
                    RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
                    break;
                default: break;
            }            
            RegisterMgr.Continue();
            if (!RegisterMgr.OnAttendeeCheckPage())
            {
                Assert.Fail("Not on AttendeeCheck page!");
            }
            RegisterMgr.XAuth_VerifySubsituteVisibility(false);
        }

        [Step]
        private void SetXAuthType()
        {
            string sessionId = ManagerSiteMgr.LoginAndDeleteTestRegsReturnToManagerScreen(this.eventId, "xAuth");
            ManagerSiteMgr.SelectFolder_UserDefined("xAuth");
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventId, sessionId);

            //create existing email address event
            BuilderMgr.OpenRegType("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            Managers.ManagerProvider.XAuthMgr.SetXAuthType(this.currentType);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(this.currentType);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyPassTest();
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utility.ThreadSleep(3);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void CreateRegistration(RegisterManager.XAuthPersonalInfo personalInfo)
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.CheckinWithEmail(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            switch (this.currentType)
            {
                case FormData.XAuthType.ByEmail:
                    break;
                case FormData.XAuthType.ByEmailPassword:
                    RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
                    break;
                case FormData.XAuthType.ByUserName:
                    RegisterMgr.XAuth_TypeUserId(Managers.ManagerProvider.XAuthMgr.DefaultAccount_UserName);
                    break;
                case FormData.XAuthType.ByUserNamePassword:
                    RegisterMgr.XAuth_TypeUserId(Managers.ManagerProvider.XAuthMgr.DefaultAccount_UserName);
                    RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
                    break;
                default: break;
            }
            RegisterMgr.Continue();
            //PI            
            RegisterMgr.XAuth_SetDefaultStandardPersonalInfoFields(personalInfo, false);
            switch (this.currentType)
            {
                case FormData.XAuthType.ByEmail:
                    personalInfo.Country = string.Empty;
                    RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
                    RegisterMgr.TypePersonalInfoPassword(InputPassword);
                    RegisterMgr.TypePersonalInfoVerifyPassword(InputPassword);
                    break;
                case FormData.XAuthType.ByEmailPassword:
                    personalInfo.Country = "USA";
                    RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
                    break;
                case FormData.XAuthType.ByUserName:
                    personalInfo.Country = string.Empty;
                    RegisterMgr.XAuth_VerifyPasswordSectionPresent(true);
                    RegisterMgr.TypePersonalInfoPassword(InputPassword);
                    RegisterMgr.TypePersonalInfoVerifyPassword(InputPassword);
                    break;
                case FormData.XAuthType.ByUserNamePassword:
                    personalInfo.Country = "USA";
                    RegisterMgr.XAuth_VerifyPasswordSectionPresent(false);
                    break;
                default: break;
            }
            RegisterMgr.Continue();
            //Agenda
            RegisterMgr.SetCustomFieldCheckBox(AgendaName, false);
            RegisterMgr.Continue();
            //Lodging
            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.SelectRoomPreference(RoomType1);
            RegisterMgr.Continue();
            //Merchandise
            RegisterMgr.SelectMerchandiseQuantityByName(MerchandiseName, MerchandiseQuantity1);
            RegisterMgr.Continue();
            //Checkout
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        [Verify]
        private void UpdateRegistrationAndVerify(RegisterManager.XAuthPersonalInfo personalInfo)
        {
            RegisterMgr.ClickEditPersonalInformationLink(0);
            if (!RegisterMgr.OnPersonalInfoPage())
            {
                Assert.Fail("Not on PersonalInfo page!");
            }
            //PI
            switch (this.currentType)
            {
                case FormData.XAuthType.ByEmail:
                case FormData.XAuthType.ByUserName:
                    personalInfo.Country = string.Empty;                    
                    break;
                case FormData.XAuthType.ByEmailPassword:
                case FormData.XAuthType.ByUserNamePassword:
                    personalInfo.Country = "USA";                    
                    break;
                default: break;
            }
            RegisterMgr.XAuth_SetDefaultStandardPersonalInfoFields(personalInfo, true);
            RegisterMgr.Continue();
            //Agenda
            RegisterMgr.ClickEditAgendaLink(0);
            RegisterMgr.SetCustomFieldCheckBox(AgendaName, true);
            RegisterMgr.Continue();
            //Lodging
            RegisterMgr.ClickEditLodgingAndTravelLink(0);
            RegisterMgr.ClickNeedAccommodations();
            RegisterMgr.SelectRoomPreference(RoomType2);
            RegisterMgr.Continue();
            //AttendCheck
            RegisterMgr.Continue();
            //Mechandise
            //19.5
            RegisterMgr.SelectMerchandiseQuantityByName(MerchandiseName, MerchandiseQuantity2);
            RegisterMgr.Continue();
            //Checkout
            RegisterMgr.VerifyCheckoutTotal(MerchandiseFee * MerchandiseQuantity2);
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            RegisterMgr.VerifyPrimaryRegistrationPersonalInfo(personalInfo);
            RegisterMgr.VerifyConfirmationAgenda(0, AgendaName);
            RegisterMgr.VerifyConfirmationRoomType(RoomType2);
            RegisterMgr.VerifyConfirmationMerchandise(0, MerchandiseName, MerchandiseQuantity2.ToString(), "$" + MerchandiseFee.ToString("F2"), "$" + (MerchandiseFee * MerchandiseQuantity2).ToString("F2"));
        }

        [Verify]
        private void VerifyViewPrintOrEmailLink()
        {
            GoToAttendeeCheckPageAndVerify();
            RegisterMgr.ClickViewPrintOrEmailLink();
            if (!RegisterMgr.OnConfirmationPage())
            {
                Assert.Fail("Not on ConfirmationPage page!");
            }
        }

        [Verify]
        private void VerifyCancelLink()
        {
            GoToAttendeeCheckPageAndVerify();
            RegisterMgr.ClickCancelLinkAndVerifyDialogVisibility();
            VerifyTool.VerifyValue(true, RegisterMgr.CancleRegistrationAndGetSuccessful(), "Cancle Registration And Get Successful: {0}");
        }
    }
}
