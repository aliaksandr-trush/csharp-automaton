namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.CheckinPage
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class GroupRegistration : FixtureBase
    {
        string emailAddress;
        Event GroupUniqueEmailEvent;

        private PageObject.Register.Checkin Checkin = new PageObject.Register.Checkin();
        private PageObject.Register.Login Login = new PageObject.Register.Login();
        private PageObject.Register.PersonalInfo PersonalInfo = new PageObject.Register.PersonalInfo();
        private PageObject.Register.Confirmation Confirmation = new PageObject.Register.Confirmation();
        private PageObject.Register.PageObjectHelper RegisterHelper = new PageObject.Register.PageObjectHelper();

        public void GroupUniqueEmail()
        {
            this.GroupUniqueEmailEvent = new Event("RI-GroupRegistration");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, GroupUniqueEmailEvent, false, false);

            Registrant reg1 = new Registrant();
            Registrant reg2 = new Registrant();
            this.emailAddress = reg1.Email;
            reg1.Event = GroupUniqueEmailEvent;
            reg2.Event = GroupUniqueEmailEvent;
            List<Registrant> regs = new List<Registrant>();
            regs.Add(reg1);
            regs.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(regs);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1307")]
        public void GroupUsedEmail()
        {
            this.GroupUniqueEmail();

            Event GroupUsedEmail = new Event("RI-GroupRegistration");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, GroupUsedEmail, false, false);

            Registrant registrant = new Registrant(this.emailAddress);
            registrant.Event = GroupUsedEmail;

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            Assert.True(Login.Password.IsPresent);
            Assert.True(Login.PasswordOnDupEmail.IsPresent);

            KeywordProvider.RegistrationCreation.Login(registrant);

            Assert.True(KeywordProvider.RegisterDefault.IsOnAttendeeCheckPage());

            RegisterHelper.Continue_Click();

            KeywordProvider.RegistrationCreation.Checkout(registrant);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1306")]
        public void GroupUsedEmailDiffEvent()
        {
            this.GroupUniqueEmail();

            Event GroupUsedEmailDiffEvent = new Event("RI-GroupRegistrationDiffEvent");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, GroupUsedEmailDiffEvent, false, false);

            Registrant registrant = new Registrant(this.emailAddress);
            registrant.Event = GroupUsedEmailDiffEvent;

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            Assert.True(Login.Password.IsPresent);
            Assert.True(Login.PasswordOnAutoRecall.IsPresent);

            KeywordProvider.RegistrationCreation.Login(registrant);

            Assert.True(KeywordProvider.RegisterDefault.IsOnPersonalInfoPage());
            Assert.True(PersonalInfo.PersonalInfoFields(
                FormData.PersonalInfoField.FirstName).Text.Trim().Equals(Registrant.Default.FirstName));
            Assert.True(PersonalInfo.PersonalInfoFields(
                FormData.PersonalInfoField.MiddleName).Text.Trim().Equals(Registrant.Default.MiddleName));
            Assert.True(PersonalInfo.PersonalInfoFields(
                FormData.PersonalInfoField.Password).Text != null);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1308")]
        public void GroupEventFeeGroupDiscount()
        {
            Event GroupEventFeeGroupDiscount = new Event("RI-GroupEventFeeGroupDiscount");
            GroupDiscount GroupDiscount = new GroupDiscount();
            GroupDiscount.GroupSize = 2;
            GroupDiscount.GroupSizeOption = FormData.GroupSizeOption.SizeOrMore;
            GroupDiscount.DiscountAmount = 1;
            GroupDiscount.GroupDiscountType = FormData.DiscountType.USDollar;
            GroupDiscount.AddtionalRegOption = FormData.AdditionalRegOption.AnyAdditional;
            GroupEventFeeGroupDiscount.StartPage.GroupDiscount = GroupDiscount;
            RegType RegType = new RegType("First");
            RegType.Price = 50;
            GroupEventFeeGroupDiscount.StartPage.RegTypes.Add(RegType);
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            GroupEventFeeGroupDiscount.CheckoutPage.PaymentMethods.Add(paymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, GroupEventFeeGroupDiscount, false, false);

            Registrant reg1 = new Registrant();
            reg1.Event = GroupEventFeeGroupDiscount;
            reg1.RegType = GroupEventFeeGroupDiscount.StartPage.RegTypes[0];
            reg1.PaymentMethod = paymentMethod;
            Registrant reg2 = new Registrant();
            reg1.Event = GroupEventFeeGroupDiscount;
            reg1.RegType = GroupEventFeeGroupDiscount.StartPage.RegTypes[0];
            List<Registrant> regs = new List<Registrant>();
            regs.Add(reg1);
            regs.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(regs);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1309")]
        public void GroupEventLimitReached()
        {
            Event GroupEventLimitReached = new Event("RI-GroupEventLimitReached");
            EventLevelLimit eventLimit = new EventLevelLimit(1);
            eventLimit.EnableWaitList = true;
            GroupEventLimitReached.StartPage.EventLimit = eventLimit;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, GroupEventLimitReached, false, true);

            Registrant reg1 = new Registrant();
            reg1.Event = GroupEventLimitReached;
            Registrant reg2 = new Registrant();
            reg2.Event = GroupEventLimitReached;

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);

            Assert.True(RegisterHelper.AddPersonToWaitlist.IsPresent);
            RegisterHelper.AddPersonToWaitlist_Click();

            Assert.True(Checkin.EventLimitReachedMessage.IsPresent);

            Checkin.EmailAddress.Type(reg2.Email);
            Checkin.AddToWaitlist_Click();
            KeywordProvider.RegistrationCreation.Checkout(reg1);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1312")]
        public void UpdateGroup_AddAPerson()
        {
            this.GroupUniqueEmail();

            Event UpdateGroup = new Event("RI-GroupRegistration");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, UpdateGroup, false, false);

            Registrant reg1 = new Registrant(emailAddress);
            reg1.Event = UpdateGroup;
            Registrant reg2 = new Registrant();
            reg2.Event = UpdateGroup;

            KeywordProvider.RegistrationCreation.Checkin(reg1);

            Assert.True(Login.Password.IsPresent);
            Assert.True(Login.PasswordOnDupEmail.IsPresent);

            KeywordProvider.RegistrationCreation.Login(reg1);

            Assert.True(KeywordProvider.RegisterDefault.IsOnAttendeeCheckPage());

            RegisterHelper.AddAnotherPerson_Click();

            Assert.True(KeywordProvider.RegisterDefault.IsOnCheckinPage());

            KeywordProvider.RegistrationCreation.Checkin(reg2);

            Assert.True(KeywordProvider.RegisterDefault.IsOnPersonalInfoPage());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1318")]
        public void UpdateGroup_AddAPerson_RegType()
        {
            Event evt = new Event("RI-GroupRegistrationRegType");
            RegType regType = new RegType("First");
            evt.StartPage.RegTypes.Add(regType);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant reg1 = new Registrant();
            Registrant reg2 = new Registrant();
            reg1.Event = evt;
            reg2.Event = evt;
            reg1.RegType = regType;
            reg2.RegType = regType;
            List<Registrant> regs = new List<Registrant>();
            regs.Add(reg1);
            regs.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(regs);

            Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(reg1);
            RegisterHelper.AddAnotherPerson_Click();

            Registrant reg3 = new Registrant();
            reg3.Event = evt;
            reg3.RegType = regType;

            KeywordProvider.RegistrationCreation.Checkin(reg3);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg3);
            KeywordProvider.RegistrationCreation.Checkout(reg3);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1319")]
        public void UpdateGroup_AddAPerson_ForceSameRegType()
        {
            Event evt = new Event("RI-GroupRegistrationForceSameRegType");
            RegType regType = new RegType("First");
            evt.StartPage.RegTypes.Add(regType);
            evt.StartPage.ForceSelectSameRegType = true;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant reg1 = new Registrant();
            Registrant reg2 = new Registrant();
            reg1.Event = evt;
            reg2.Event = evt;
            reg1.RegType = regType;
            List<Registrant> regs = new List<Registrant>();
            regs.Add(reg1);
            regs.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(regs);

            Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(reg1);
            RegisterHelper.AddAnotherPerson_Click();

            Registrant reg3 = new Registrant();
            reg3.Event = evt;

            Assert.False(Checkin.RegTypeRadioButton.IsPresent);
            Assert.False(Checkin.RegTypeDropDown.IsPresent);
            KeywordProvider.RegistrationCreation.CreateRegistration(reg3);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1313")]
        public void StartNewRegistrationLink()
        {
            this.GroupUniqueEmail();

            Confirmation.ChangeMyRegistration_Click();

            Assert.True(KeywordProvider.RegisterDefault.IsOnLoginPage());

            Registrant reg = new Registrant(emailAddress);
            reg.Event = this.GroupUniqueEmailEvent;

            KeywordProvider.RegistrationCreation.Checkin(reg);

            Login.StartNewRegistration_Click();

            Assert.True(KeywordProvider.RegisterDefault.IsOnPersonalInfoPage());
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1314")]
        public void GoBackLink()
        {
            Event GoBackLink = new Event("RI-GoBackLink");
            AgendaItemCheckBox agenda = new AgendaItemCheckBox("GroupAgenda");
            AgendaPage agendaPage = new AgendaPage();
            agendaPage.AgendaItems.Add(agenda);
            GoBackLink.AgendaPage = agendaPage;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, GoBackLink, false, false);

            Registrant reg = new Registrant();
            reg.Event = GoBackLink;

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            RegisterHelper.AddAnotherPerson_Click();
            Checkin.GoBack_Click();
            Assert.True(KeywordProvider.RegisterDefault.IsOnAgendaPage());
            RegisterHelper.Continue_Click();
            RegisterHelper.AddAnotherPerson_Click();
            Checkin.GoBack_Click();
            Assert.True(KeywordProvider.RegisterDefault.IsOnCheckoutPage());
            KeywordProvider.RegistrationCreation.Checkout(reg);
        }
    }
}
