namespace RegOnline.RegressionTest.Fixtures.Backend
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Utilities;
    using AttendeeInfoSubPage = RegOnline.RegressionTest.Managers.Backend.BackendManager.AttendeeSubPage;
    using CFStatus = RegOnline.RegressionTest.Managers.Backend.BackendManager.CustomFieldStatus;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class MembershipInfoEditFixture : FixtureBase
    {
        /// <summary>
        /// This class contains all the stuff for editing membership
        /// </summary>
        private class MembershipEvent
        {
            public const string EventName = "Membership for editing Member info";

            public enum MembershipFee
            {
                [StringValue("Membership fee 1")]
                MembershipFeeOne,

                [StringValue("Membership fee 2")]
                MembershipFeeTwo,

                [StringValue("One time fee 1")]
                OneTimeFeeOne,

                [StringValue("One time fee 2")]
                OneTimeFeeTwo
            }

            public Dictionary<MembershipEvent.MembershipFee, int> membershipFeeIds;

            public MembershipEvent()
            {
                this.membershipFeeIds = new Dictionary<MembershipFee, int>();
            }
        }

        private MembershipEvent membershipEvent = new MembershipEvent();
        private BackendFixtureHelper helper = new BackendFixtureHelper();

        private int eventId;
        private string eventSessionId;
        private int registrationId;

        [Test]
        [Category(Priority.One)]
        [Description("382")]
        public void EditMemberInfo()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            this.eventId = ManagerSiteMgr.GetFirstEventId(MembershipEvent.EventName);
            RegisterMgr.CurrentEventId = this.eventId;
            RegisterMgr.OpenRegisterPage();
            RegisterMgr.CheckinWithEmail("this" + DateTime.Now.Ticks.ToString() + "@isatest.com");
            RegisterMgr.Continue();
            this.helper.EnterPersonalInfoDuringRegistration("Test", "T", "McTester");
            RegisterMgr.EnterPersonalInfoPassword();
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItem(StringEnum.GetStringValue(MembershipEvent.MembershipFee.MembershipFeeOne), false);

            this.membershipEvent.membershipFeeIds.Add(MembershipEvent.MembershipFee.MembershipFeeOne,
                RegisterMgr.GetCustomFieldIDForCheckboxItem(StringEnum.GetStringValue(MembershipEvent.MembershipFee.MembershipFeeOne)));

            this.membershipEvent.membershipFeeIds.Add(MembershipEvent.MembershipFee.MembershipFeeTwo,
                RegisterMgr.GetCustomFieldIDForCheckboxItem(StringEnum.GetStringValue(MembershipEvent.MembershipFee.MembershipFeeTwo)));

            RegisterMgr.SelectAgendaItem(StringEnum.GetStringValue(MembershipEvent.MembershipFee.OneTimeFeeTwo), false);

            this.membershipEvent.membershipFeeIds.Add(MembershipEvent.MembershipFee.OneTimeFeeOne,
                RegisterMgr.GetCustomFieldIDForCheckboxItem(StringEnum.GetStringValue(MembershipEvent.MembershipFee.OneTimeFeeOne)));

            this.membershipEvent.membershipFeeIds.Add(MembershipEvent.MembershipFee.OneTimeFeeTwo,
                RegisterMgr.GetCustomFieldIDForCheckboxItem(StringEnum.GetStringValue(MembershipEvent.MembershipFee.OneTimeFeeTwo)));

            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();

            this.registrationId = Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.RegistrationId));

            RegisterMgr.ConfirmRegistration();

            this.eventSessionId = this.helper.LoginAndGetEventSessionId();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registrationId);
            BackendMgr.OpenEditAttendeeSubPage(AttendeeInfoSubPage.MembershipFees);

            BackendMgr.SetCheckboxCFItem(this.membershipEvent.membershipFeeIds[MembershipEvent.MembershipFee.MembershipFeeOne], false);
            BackendMgr.SetCheckboxCFItem(this.membershipEvent.membershipFeeIds[MembershipEvent.MembershipFee.OneTimeFeeTwo], false);

            BackendMgr.SetCheckboxCFItem(this.membershipEvent.membershipFeeIds[MembershipEvent.MembershipFee.MembershipFeeTwo], true);
            BackendMgr.SelectCFStatus(this.membershipEvent.membershipFeeIds[MembershipEvent.MembershipFee.MembershipFeeTwo], CFStatus.Confirmed);
            BackendMgr.SetCheckboxCFItem(this.membershipEvent.membershipFeeIds[MembershipEvent.MembershipFee.OneTimeFeeOne], true);
            BackendMgr.SelectCFStatus(this.membershipEvent.membershipFeeIds[MembershipEvent.MembershipFee.OneTimeFeeOne], CFStatus.Confirmed);

            BackendMgr.SaveAndCloseEditMembershipFee();
            BackendMgr.SaveAndBypassTransaction();
            BackendMgr.SelectAttendeeInfoWindow();

            BackendMgr.VerifyCFCheckboxItem(
                this.membershipEvent.membershipFeeIds[MembershipEvent.MembershipFee.MembershipFeeTwo],
                StringEnum.GetStringValue(MembershipEvent.MembershipFee.MembershipFeeTwo),
                true, CFStatus.Confirmed,
                75);

            BackendMgr.VerifyCFCheckboxItem(
                this.membershipEvent.membershipFeeIds[MembershipEvent.MembershipFee.OneTimeFeeOne],
                StringEnum.GetStringValue(MembershipEvent.MembershipFee.OneTimeFeeOne),
                true, CFStatus.Confirmed,
                25);

            BackendMgr.VerifyAttendeeFees(25.00, 100.00, 25.00, 100.00);
            BackendMgr.VerifyTotalRecurringFees(75);

            // Change renewal date
            DateTime newRenewalDate = new DateTime(2013, 12, 22);
            BackendMgr.ChangeRenewalDate(newRenewalDate);
            BackendMgr.VerifyNextRenewDate(newRenewalDate);
            BackendMgr.VerifyNextPayDate(newRenewalDate);
        }
    }
}
