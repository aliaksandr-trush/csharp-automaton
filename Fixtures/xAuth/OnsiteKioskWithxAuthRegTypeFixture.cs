namespace RegOnline.RegressionTest.Fixtures.xAuth
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;

    [TestFixture]
    public class OnsiteKioskWithxAuthRegTypeFixture : ExternalAuthenticationFixtureBase
    {
        [TestCase(FormData.XAuthType.ByEmailPassword)]
        [TestCase(FormData.XAuthType.ByUserNamePassword)]
        [TestCase(FormData.XAuthType.ByEmail)]
        [TestCase(FormData.XAuthType.ByUserName)]
        [Test]
        public void TestOnsiteKioskWithxAuthRegTypeFixture(FormData.XAuthType xAuthType)
        {
            ChangeAdvancedSettingInxAuthEventFixture changeAdvancedSettingInxAuthEvent = 
                new ChangeAdvancedSettingInxAuthEventFixture();

            int eventId = 
                changeAdvancedSettingInxAuthEvent.ChangeAdvancedSettingInxAuthEvent(xAuthType, true);

            this.VerifyOnSiteCheckInProcess(
                true, 
                true, 
                true, 
                true, 
                true, 
                true, 
                true, 
                false, 
                xAuthType,
                eventId,
                RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1].Email,
                RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1].UserName,
                RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1].XAuthPassword,
                RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1].Password);

            this.VerifyOnSiteCheckInProcess(
                true, 
                true, 
                true, 
                false, 
                true, 
                true, 
                true, 
                false,
                xAuthType, 
                eventId,
                RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1].Email,
                RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1].UserName,
                RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1].XAuthPassword,
                RegisterMgr.TestAccounts[RegisterManager.TestAccountType.DefaultAccount1].Password);
        }

        [Test]
        public void TestOnsiteGroupRegistration()
        {
            CreateEventWithAndWithoutXauthRegTypesAndRelatedRegistrationFixture fixture = 
                new CreateEventWithAndWithoutXauthRegTypesAndRelatedRegistrationFixture();

            // Clear up
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();
            fixture.CreateEventWithAndWithoutXauthRegTypes(true);
            fixture.GroupRegistrationWithSameXAuthRegtype(true);

            // Clear up
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();
            fixture.GroupRegistrationWithDifferentXAuthRegTypes(true);

            // Clear up
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();
            fixture.GroupRegistrationWithXAuthAndNonXAuthRegTypes(true);

            // Clear up
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();
            fixture.GroupRegistrationWithNonXAuthAndXAuthRegTypes(true);

            // Clear up
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();
            fixture.GroupRegistrationWithDifferentNonXAuthRegTypes(true);

            // Clear up
            Managers.ManagerProvider.XAuthMgr.RemoveXAuthTestRegisterAndAttendeeForCustomer();
            fixture.GroupRegistrationWithAllFourDifferentRegTypes(true);
        }

        private void VerifyOnSiteCheckInProcess(
            bool enableBarcodeSearchByRegId,
            bool paidInFull,
            bool allowCC,
            bool requirePassword,
            bool allowOnSiteRegs,
            bool allowUpdates,
            bool allowGroupCheckIn,
            bool allowBadgePrinting,
            FormData.XAuthType xAuthType, 
            int eventId, 
            string email, 
            string userName, 
            string xAuthPassword, 
            string password)
        {

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.OpenEventDashboard(eventId);

            ManagerSiteMgr.DashboardMgr.LaunchKiosk(
                enableBarcodeSearchByRegId, 
                paidInFull,
                allowCC, 
                "text",
                requirePassword,
                allowOnSiteRegs,
                allowUpdates,
                allowGroupCheckIn,
                allowBadgePrinting);

            RegisterMgr.KioskSearch("WRONGID");
            VerifyNorecordsfoundShowUp();
            RegisterMgr.KioskClickNewSearch();

            RegisterMgr.KioskSearch(email);

            if (requirePassword)
            {
                if (xAuthType == FormData.XAuthType.ByUserName || xAuthType == FormData.XAuthType.ByUserNamePassword)
                {
                    RegisterMgr.KioskEnterUserId("WRONGID");
                    RegisterMgr.KioskEnterPassword("WRONG PASSWORD");
                    this.VerifyIncorrectMessageShowUp();
                    RegisterMgr.KioskClickNewSearch();

                    RegisterMgr.KioskSearch(email);
                    RegisterMgr.KioskEnterUserId(userName);
                    RegisterMgr.KioskEnterPassword("WRONG PASSWORD");
                    this.VerifyIncorrectMessageShowUp();
                    RegisterMgr.KioskClickNewSearch();

                    RegisterMgr.KioskSearch(email);
                    RegisterMgr.KioskEnterUserId(userName);


                    if (xAuthType == FormData.XAuthType.ByUserName)
                    {
                        RegisterMgr.KioskEnterPassword(password);
                    }

                    if (xAuthType == FormData.XAuthType.ByUserNamePassword)
                    {
                        RegisterMgr.KioskEnterPassword(xAuthPassword);
                    }
                }

                if (xAuthType == FormData.XAuthType.ByEmail || xAuthType == FormData.XAuthType.ByEmailPassword)
                {
                    RegisterMgr.KioskEnterPassword("WRONG PASSWORD");
                    VerifyIncorrectMessageShowUp();
                    RegisterMgr.KioskClickNewSearch();

                    RegisterMgr.KioskSearch(email);

                    if (xAuthType == FormData.XAuthType.ByEmail)
                    {
                        RegisterMgr.KioskEnterPassword(password);
                    }

                    if (xAuthType == FormData.XAuthType.ByEmailPassword)
                    {
                        RegisterMgr.KioskEnterPassword(xAuthPassword);
                    }
                }
            }

            this.VerifyCheckInSucess();
            RegisterMgr.KioskExitCheckIn();
        }

        private void VerifyNorecordsfoundShowUp()
        {
            Assert.AreEqual("0 record(s) found.", WebDriverUtility.DefaultProvider.GetText("//div[@class='regHeader']", LocateBy.XPath));
        }

        private void VerifyIncorrectMessageShowUp()
        {
            string errorMessage = "Authentication failure. Please try again.";
            string errorMessage_Alternative = "You must sign in with the correct values.";
            string actualMessage = WebDriverUtility.DefaultProvider.GetText("//div[@id='ctl00_cph_valSummary']/ul/li", LocateBy.XPath);

            try
            {
                Assert.AreEqual(errorMessage, actualMessage);
            }
            catch
            {
                Assert.AreEqual(errorMessage_Alternative, actualMessage);
            }
        }

        private void VerifyCheckInSucess()
        {
            Assert.AreEqual("You are already checked-in.", WebDriverUtility.DefaultProvider.GetText("//div[@id='ctl00_cph_divMessage']/span[@class='prompt']", LocateBy.XPath));
        }
    }
}
