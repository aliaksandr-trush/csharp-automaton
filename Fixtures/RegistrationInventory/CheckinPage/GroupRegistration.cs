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

            Registrant reg1 = new Registrant(this.groupUniqueEmailEvent);
            Registrant reg2 = new Registrant(this.groupUniqueEmailEvent);
            this.emailAddress = reg1.Email;
            Group group = new Group();
            group.Primary = reg1;
            group.Secondaries.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(group);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1307")]
        public void GroupUsedEmail()
        {
            this.GroupUniqueEmail();

            Registrant registrant = new Registrant(this.groupUniqueEmailEvent, this.emailAddress);

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

            Event groupUsedEmailDiffEvent = new Event("RI-GroupRegistrationDiffEvent");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, groupUsedEmailDiffEvent);

            Registrant registrant = new Registrant(groupUsedEmailDiffEvent, this.emailAddress);

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Login.PasswordOnAutoRecall.IsPresent);

            KeywordProvider.RegistrationCreation.Login(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(
                FormData.PersonalInfoField.FirstName).Text.Trim().Equals(DataCollection.DefaultPersonalInfo.FirstName));

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(
                FormData.PersonalInfoField.MiddleName).Text.Trim().Equals(DataCollection.DefaultPersonalInfo.MiddleName));

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PersonalInfoFields(
                FormData.PersonalInfoField.Password).Text != null);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1308")]
        public void GroupEventFeeGroupDiscount()
        {
            Event groupEventFeeGroupDiscount = new Event("RI-GroupEventFeeGroupDiscount");
            GroupDiscount groupDiscount = new GroupDiscount();
            groupDiscount.GroupSize = 2;
            groupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.SizeOrMore;
            groupDiscount.DiscountAmount = 1;
            groupDiscount.GroupDiscountType = GroupDiscount_DiscountType.USDollar;
            groupDiscount.AddtionalRegOption = GroupDiscount_AdditionalRegOption.AnyAdditional;
            groupEventFeeGroupDiscount.StartPage.GroupDiscount = groupDiscount;
            RegType regType = new RegType("First");
            regType.Price = 50;
            groupEventFeeGroupDiscount.StartPage.RegTypes.Add(regType);
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            groupEventFeeGroupDiscount.CheckoutPage.PaymentMethods.Add(paymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, groupEventFeeGroupDiscount);

            Registrant reg1 = new Registrant(groupEventFeeGroupDiscount);
            reg1.EventFee_Response = new EventFeeResponse(groupEventFeeGroupDiscount.StartPage.RegTypes[0]);
            reg1.Payment_Method = paymentMethod;
            Registrant reg2 = new Registrant(groupEventFeeGroupDiscount);
            reg1.EventFee_Response = new EventFeeResponse(groupEventFeeGroupDiscount.StartPage.RegTypes[0]);
            Group group = new Group();
            group.Primary = reg1;
            group.Secondaries.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(group);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1309")]
        public void GroupEventLimitReached()
        {
            Event groupEventLimitReached = new Event("RI-GroupEventLimitReached");
            EventLevelLimit eventLimit = new EventLevelLimit(1);
            eventLimit.EnableWaitList = true;
            groupEventLimitReached.StartPage.EventLimit = eventLimit;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, groupEventLimitReached);

            Registrant reg1 = new Registrant(groupEventLimitReached);
            Registrant reg2 = new Registrant(groupEventLimitReached);

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

            Registrant reg1 = new Registrant(this.groupUniqueEmailEvent, this.emailAddress);
            Registrant reg2 = new Registrant(this.groupUniqueEmailEvent);

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

            Registrant reg1 = new Registrant(evt);
            Registrant reg2 = new Registrant(evt);
            reg1.EventFee_Response = new EventFeeResponse(regType);
            reg2.EventFee_Response = new EventFeeResponse(regType);
            Group group = new Group();
            group.Primary = reg1;
            group.Secondaries.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(group);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(reg1);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();

            Registrant reg3 = new Registrant(evt);
            reg3.EventFee_Response = new EventFeeResponse(regType);

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

            Registrant reg1 = new Registrant(evt);
            Registrant reg2 = new Registrant(evt);
            reg1.EventFee_Response = new EventFeeResponse(regType);
            Group group = new Group();
            group.Primary = reg1;
            group.Secondaries.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(group);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(reg1);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();

            Registrant reg3 = new Registrant(evt);

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

            Registrant reg = new Registrant(this.groupUniqueEmailEvent, emailAddress);

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
            AgendaItem_CheckBox agenda = new AgendaItem_CheckBox("GroupAgenda");
            AgendaPage agendaPage = new AgendaPage();
            agendaPage.AgendaItems.Add(agenda);
            GoBackLink.AgendaPage = agendaPage;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, GoBackLink);

            Registrant reg = new Registrant(GoBackLink);

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
