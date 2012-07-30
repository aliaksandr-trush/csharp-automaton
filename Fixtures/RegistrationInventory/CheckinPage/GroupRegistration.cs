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
        private string emailAddress;
        private Event groupUniqueEmailEvent;

        public void GroupUniqueEmail()
        {
            this.groupUniqueEmailEvent = new Event("RI-GroupRegistration");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(
                EventFolders.Folders.RegistrationInventory, 
                this.groupUniqueEmailEvent, 
                false);

            Registrant reg1 = new Registrant();
            Registrant reg2 = new Registrant();
            this.emailAddress = reg1.Email;
            reg1.Event = this.groupUniqueEmailEvent;
            reg2.Event = this.groupUniqueEmailEvent;
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

            Registrant registrant = new Registrant(this.emailAddress);
            registrant.Event = this.groupUniqueEmailEvent;

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Login.PasswordOnDupEmail.IsPresent);

            KeywordProvider.RegistrationCreation.Login(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.AttendeeCheck, true);

            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();

            KeywordProvider.RegistrationCreation.Checkout(registrant);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1306")]
        public void GroupUsedEmailDiffEvent()
        {
            this.GroupUniqueEmail();

            Event GroupUsedEmailDiffEvent = new Event("RI-GroupRegistrationDiffEvent");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, GroupUsedEmailDiffEvent);

            Registrant registrant = new Registrant(this.emailAddress);
            registrant.Event = GroupUsedEmailDiffEvent;

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Login.PasswordOnAutoRecall.IsPresent);

            KeywordProvider.RegistrationCreation.Login(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(
                FormData.PersonalInfoField.FirstName).Text.Trim().Equals(Registrant.Default.FirstName));

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(
                FormData.PersonalInfoField.MiddleName).Text.Trim().Equals(Registrant.Default.MiddleName));

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(
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
            GroupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.SizeOrMore;
            GroupDiscount.DiscountAmount = 1;
            GroupDiscount.GroupDiscountType = GroupDiscount_DiscountType.USDollar;
            GroupDiscount.AddtionalRegOption = GroupDiscount_AdditionalRegOption.AnyAdditional;
            GroupEventFeeGroupDiscount.StartPage.GroupDiscount = GroupDiscount;
            RegType RegType = new RegType("First");
            RegType.Price = 50;
            GroupEventFeeGroupDiscount.StartPage.RegTypes.Add(RegType);
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            GroupEventFeeGroupDiscount.CheckoutPage.PaymentMethods.Add(paymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, GroupEventFeeGroupDiscount);

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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, GroupEventLimitReached);

            Registrant reg1 = new Registrant();
            reg1.Event = GroupEventLimitReached;
            Registrant reg2 = new Registrant();
            reg2.Event = GroupEventLimitReached;

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.AddPersonToWaitlist.IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.AddPersonToWaitlist_Click();

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EventLimitReachedMessage.IsPresent);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg2.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.AddToWaitlist_Click();
            KeywordProvider.RegistrationCreation.Checkout(reg1);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1312")]
        public void UpdateGroup_AddAPerson()
        {
            this.GroupUniqueEmail();

            Registrant reg1 = new Registrant(this.emailAddress);
            reg1.Event = this.groupUniqueEmailEvent;
            Registrant reg2 = new Registrant();
            reg2.Event = this.groupUniqueEmailEvent;

            KeywordProvider.RegistrationCreation.Checkin(reg1);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Login.PasswordOnDupEmail.IsPresent);

            KeywordProvider.RegistrationCreation.Login(reg1);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.AttendeeCheck, true);

            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();

            AssertHelper.VerifyOnPage(FormData.RegisterPage.Checkin, true);

            KeywordProvider.RegistrationCreation.Checkin(reg2);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1318")]
        public void UpdateGroup_AddAPerson_RegType()
        {
            Event evt = new Event("RI-GroupRegistrationRegType");
            RegType regType = new RegType("First");
            evt.StartPage.RegTypes.Add(regType);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

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

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(reg1);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();

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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg1 = new Registrant();
            Registrant reg2 = new Registrant();
            reg1.Event = evt;
            reg2.Event = evt;
            reg1.RegType = regType;
            List<Registrant> regs = new List<Registrant>();
            regs.Add(reg1);
            regs.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(regs);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(reg1);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();

            Registrant reg3 = new Registrant();
            reg3.Event = evt;

            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadioButton.IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeDropDown.IsPresent);
            KeywordProvider.RegistrationCreation.CreateRegistration(reg3);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1313")]
        public void StartNewRegistrationLink()
        {
            this.GroupUniqueEmail();

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();

            AssertHelper.VerifyOnPage(FormData.RegisterPage.Login, true);

            Registrant reg = new Registrant(emailAddress);
            reg.Event = this.groupUniqueEmailEvent;

            KeywordProvider.RegistrationCreation.Checkin(reg);

            PageObject.PageObjectProvider.Register.RegistationSite.Login.StartNewRegistration_Click();

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, GoBackLink);

            Registrant reg = new Registrant();
            reg.Event = GoBackLink;

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.GoBack_Click();
            AssertHelper.VerifyOnPage(FormData.RegisterPage.Agenda, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.GoBack_Click();
            AssertHelper.VerifyOnPage(FormData.RegisterPage.Checkout, true);
            KeywordProvider.RegistrationCreation.Checkout(reg);
        }
    }
}
