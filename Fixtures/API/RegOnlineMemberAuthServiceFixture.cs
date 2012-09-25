namespace RegOnline.RegressionTest.Fixtures.API
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnlineMemberAuthService;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegOnlineMemberAuthServiceFixture : APIFixtureBase
    {
        private const string EventName = "WebService - RegOnlineMemberAuthServiceFixture";
        private const string MembershipItemName = "Recurring Checkbox";
        private const double MembershipFee = 10;

        private memberAuthorizationSoapClient service;
        private int membershipNumber;
        private string emailAddress;
        private int eventId;
        private string eventSessionId;
        private int registerId;
        private string xmlDoc;

        protected override Uri RemoteAddressUri { get; set; }

        public RegOnlineMemberAuthServiceFixture()
            : base(ConfigReader.WebServiceEnum.MemberAuthService)
        {
            RequiresBrowser = true;

            this.service = new memberAuthorizationSoapClient(
                CurrentWebServiceConfig.EndpointConfigName,
                RemoteAddressUri.ToString());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("858")]
        public void AuthorizeMember()
        {
            this.PrepareEvent();
            this.CreateRegistration();

            this.xmlDoc = this.service.authorizeMember(
                ConfigReader.DefaultProvider.AccountConfiguration.Login, 
                ConfigReader.DefaultProvider.AccountConfiguration.Password,
                this.eventId, 
                this.membershipNumber.ToString(), 
                ConfigReader.DefaultProvider.AccountConfiguration.Password);

            this.Verify();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("859")]
        public void AuthorizeMemberWithEmailAddress()
        {
            this.PrepareEvent();
            this.CreateRegistration();

            this.xmlDoc = this.service.authorizeMemberWithEmailAddress(
                ConfigReader.DefaultProvider.AccountConfiguration.Login, 
                ConfigReader.DefaultProvider.AccountConfiguration.Password,
                this.eventId,
                this.emailAddress, 
                ConfigReader.DefaultProvider.AccountConfiguration.Password);

            this.Verify();
        }

        [Step]
        private void PrepareEvent()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder();

            ManagerSiteMgr.DeleteExpiredDuplicateEvents(EventName);

            if (!ManagerSiteMgr.EventExists(EventName))
            {
                this.AddEvent();
            }
            else
            {
                this.eventId = ManagerSiteMgr.GetFirstEventId(EventName);
            }
        }

        private void AddEvent()
        {
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.Membership);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventName);
            BuilderMgr.GotoPage(Managers.Builder.FormDetailManager.Page.Agenda);
            this.AddRecurringFeeItem(OtherEventTypeAgendaAndCFManager.FieldType.CheckBox, MembershipItemName, MembershipFee);
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndClose();
        }

        public void AddRecurringFeeItem(OtherEventTypeAgendaAndCFManager.FieldType type, string name, double price)
        {
            BuilderMgr.ClickAddAgendaItemOld();
            BuilderMgr.OldAGAndCFMgr.SetQuestionDescription(name);
            BuilderMgr.OldAGAndCFMgr.SelectType(type);
            BuilderMgr.OldAGAndCFMgr.SetRegularPrice(price);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
            BuilderMgr.OldAGAndCFMgr.SaveAndCloseAgendaOrCFItem();
            UIUtil.DefaultProvider.SwitchToMainContent();
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        private void CreateRegistration()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            this.emailAddress = RegisterMgr.ComposeUniqueEmailAddress();
            RegisterMgr.Checkin(this.emailAddress);
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.Continue();
            RegisterMgr.SetCustomFieldCheckBox(MembershipItemName, true);
            RegisterMgr.Continue();
            RegisterMgr.SelectPaymentMethod(Managers.ManagerBase.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
            this.registerId = RegisterMgr.GetRegistrationIdOnConfirmationPage();
            BackendMgr.OpenAttendeeInfoURL(this.eventSessionId, this.registerId);
            this.membershipNumber = BackendMgr.GetMembershipNumber();
        }

        [Verify]
        private void Verify()
        {
            Assert.That(this.xmlDoc.Contains("<userNameValid>True</userNameValid>"));
            Assert.That(this.xmlDoc.Contains("<passwordValid>True</passwordValid>"));
            Assert.That(this.xmlDoc.Contains(string.Format("<BalanceDue>{0}</BalanceDue>", MembershipFee.ToString("0.0000"))));
            Assert.That(this.xmlDoc.Contains("<Status>Confirmed</Status>"));
            Assert.That(this.xmlDoc.Contains("<CancelDate />"));
        }
    }
}