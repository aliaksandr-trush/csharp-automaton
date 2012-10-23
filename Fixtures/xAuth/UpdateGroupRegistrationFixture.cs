namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;

    [TestFixture]
    public class UpdateGroupRegistrationFixture : ExternalAuthenticationFixtureBase
    {
        private const string EventName = "UpdateGroupRegistration";
        private int eventID;
        private string eventSessionId;

        [Test]
        public void UpdateGroupRegistrationTest()
        {
            string xAuthTestEmail = "joe@rrr.com";

            #region create event

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.SelectFolder_xAuth();
            ManagerSiteMgr.DeleteEventByName(EventName);

            //create an event and add reg type 1
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            // Get sessionId
            this.eventSessionId = BuilderMgr.GetEventSessionId();
            // Get event id
            this.eventID = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype1");
            BuilderMgr.RegTypeMgr.ClickOpenXAuthSetup();

            //update xAuth type as EmailPassword
            Managers.ManagerProvider.XAuthMgr.SetXAuthType(DataCollection.EventData_Common.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.SetDefaultAccount(DataCollection.EventData_Common.XAuthType.ByEmailPassword);
            Managers.ManagerProvider.XAuthMgr.ClickTestButton();
            Managers.ManagerProvider.XAuthMgr.VerifyErrorMessages(DataCollection.EventData_Common.TestAccountResult.Success);
            Managers.ManagerProvider.XAuthMgr.ClickOKButton();
            BuilderMgr.RegTypeMgr.SelectRegTypeFrame();
            Utilities.Utility.ThreadSleep(2);
            BuilderMgr.RegTypeMgr.EnableXAuth(true);
            BuilderMgr.RegTypeMgr.SaveAndClose();

            //create reg type 2
            BuilderMgr.ClickAddRegType();
            BuilderMgr.RegTypeMgr.SetName("regtype2");
            BuilderMgr.RegTypeMgr.SaveAndClose();
            BuilderMgr.SaveAndClose();

            #endregion

            #region do a group registration with a xAuth type as the primary

            //register regtype1:xAuth regtype
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            //add another xAuth regtype registration
            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(xAuthTestEmail);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            //add another no-xAuth regtype registration
            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //As the primary, log in to Update your registration
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            //make persion info changes
            RegisterMgr.ClickEditPersonalInformationLink(0);
            RegisterMgr.Continue();
            RegisterMgr.ClickEditPersonalInformationLink(1);
            RegisterMgr.Continue();
            RegisterMgr.ClickEditPersonalInformationLink(2);
            RegisterMgr.Continue();
            //Make Substitutions for secondary registrations
            RegisterMgr.VerifyHasSubstituteLink(0, false);
            RegisterMgr.VerifyHasSubstituteLink(1, false);
            RegisterMgr.ClickSubstituteLink(2);
            RegisterMgr.TypePersonalInfoEmail(RegisterMgr.ComposeUniqueEmailAddress());
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //cancel secondary registrations
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.ClickCancelLink(1, true);
            RegisterMgr.ClickCancelLink(2, true);
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //add another xAuth regtype registration
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(xAuthTestEmail);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            //add another no-xAuth regtype registration
            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //cancel entire group
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.ClickCancelEntireGroup(true);

            #endregion

            #region do a group registration with a NonxAuth type as the primary

            string noXAuthTypeAsPrimaryEmail = string.Empty;

            //register regtype1:xAuth regtype
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype2");
            noXAuthTypeAsPrimaryEmail = RegisterMgr.ComposeUniqueEmailAddress();
            RegisterMgr.CheckinWithEmail(noXAuthTypeAsPrimaryEmail);
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            //add another xAuth regtype registration
            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Email);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            //add another no-xAuth regtype registration
            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //As the secondary, log in to Update your registration
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.Checkin(noXAuthTypeAsPrimaryEmail);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword();
            RegisterMgr.Continue();
            //make persion info changes
            RegisterMgr.ClickEditPersonalInformationLink(0);
            RegisterMgr.Continue();
            RegisterMgr.ClickEditPersonalInformationLink(1);
            RegisterMgr.Continue();
            RegisterMgr.ClickEditPersonalInformationLink(2);
            RegisterMgr.Continue();
            //Make Substitutions for secondary registrations
            RegisterMgr.VerifyHasSubstituteLink(0, true);
            RegisterMgr.VerifyHasSubstituteLink(1, false);
            RegisterMgr.ClickSubstituteLink(2);
            RegisterMgr.TypePersonalInfoEmail(RegisterMgr.ComposeUniqueEmailAddress());
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //cancel secondary registrations
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.Checkin(noXAuthTypeAsPrimaryEmail);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword();
            RegisterMgr.Continue();
            RegisterMgr.ClickCancelLink(1, true);
            RegisterMgr.ClickCancelLink(2, true);
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //add another xAuth regtype registration
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.Checkin(noXAuthTypeAsPrimaryEmail);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword();
            RegisterMgr.Continue();
            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.SelectRegType("regtype1");
            RegisterMgr.Checkin(xAuthTestEmail);
            RegisterMgr.Continue();
            RegisterMgr.TypeLoginPagePassword(Managers.ManagerProvider.XAuthMgr.DefaultAccount_Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            //add another no-xAuth regtype registration
            RegisterMgr.ClickAddAnotherPerson();
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();

            //cancel entire group
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.SelectRegType("regtype2");
            RegisterMgr.Checkin(noXAuthTypeAsPrimaryEmail);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.TypeLoginPagePassword();
            RegisterMgr.Continue();
            RegisterMgr.ClickCancelEntireGroup(true);

            #endregion
        }
    }
}
