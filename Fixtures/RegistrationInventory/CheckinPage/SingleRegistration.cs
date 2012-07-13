﻿namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.CheckinPage
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class SingleRegistration : FixtureBase
    {
        private Event evt = new Event("RI-SingleRegistration");
        private string emailAddress;

        public void UniqueEmail()
        {
            string EmailAddress = string.Format("selenium{0}@regonline.com", System.DateTime.Now.Ticks.ToString());

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false);

            Registrant registrant = new Registrant(EmailAddress);
            registrant.Event = evt;

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);

            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);
            KeywordProvider.RegistrationCreation.Checkout(registrant);

            emailAddress = EmailAddress;
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1289")]
        public void UsedEmail()
        {
            this.UniqueEmail();

            Registrant registrant = new Registrant(emailAddress);
            registrant.Event = evt;

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
        [Description("1288")]
        public void UsedEmailDiffEvent()
        {
            this.UniqueEmail();

            Event DiffEvent = new Event("RI-SingleRegistrationDiffEvent");

            Registrant registrant = new Registrant(emailAddress);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, DiffEvent);

            registrant.Event = DiffEvent;

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
        [Description("1290")]
        public void RegisterWithEventLimitFull()
        {
            Event EventWithLimit = new Event("RI-SingleRegistrationEventWithLimit");
            EventLevelLimit eventLimit = new EventLevelLimit(1);
            eventLimit.EnableWaitList = true;
            EventWithLimit.StartPage.EventLimit = eventLimit;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventWithLimit);

            Registrant registrant = new Registrant();
            registrant.Event = EventWithLimit;

            KeywordProvider.RegistrationCreation.Checkin(registrant);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);
            KeywordProvider.RegistrationCreation.Checkout(registrant);

            Registrant registrantWhenFull = new Registrant();
            registrantWhenFull.Event = EventWithLimit;

            KeywordProvider.RegisterDefault.OpenRegisterPageUrl(EventWithLimit.Id);
            PageObject.PageObjectProvider.Register.RegistationSite.Login.StartNewRegistration_Click();

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EventLimitReachedMessage.IsPresent);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(registrantWhenFull.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.AddToWaitlist_Click();

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.AddedToWaitlistOfEvent.IsPresent);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1294")]
        public void UpdateSingleRegistration()
        {
            this.UniqueEmail();

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.IsPresent);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.Login, true);
        }

        public Event RegistrationWithRegType()
        {
            Event EventWithRegType = new Event("RI-SingleRegistrationWithRegType");
            RegType RegType1 = new RegType("First");
            RegType RegType2 = new RegType("Second");
            EventWithRegType.StartPage.RegTypes.Add(RegType1);
            EventWithRegType.StartPage.RegTypes.Add(RegType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventWithRegType);

            string EmailAddress = string.Format("selenium{0}@regonline.com", System.DateTime.Now.Ticks.ToString());
            Registrant registrant = new Registrant(EmailAddress);
            registrant.Event = EventWithRegType;
            registrant.RegType = EventWithRegType.StartPage.RegTypes[1];

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);

            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);
            KeywordProvider.RegistrationCreation.Checkout(registrant);

            emailAddress = EmailAddress;

            return EventWithRegType;
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1295")]
        public void RegistrationWithRegTypeDiffEvent()
        {
            this.RegistrationWithRegType();

            Event DiffEventWithRegType = new Event("RI-SingleRegistrationWithRegTypeDiffEvent");
            RegType RegType1 = new RegType("Third");
            RegType RegType2 = new RegType("Fourth");
            DiffEventWithRegType.StartPage.RegTypes.Add(RegType1);
            DiffEventWithRegType.StartPage.RegTypes.Add(RegType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, DiffEventWithRegType);

            Registrant registrant = new Registrant(emailAddress);
            registrant.Event = DiffEventWithRegType;
            registrant.RegType = DiffEventWithRegType.StartPage.RegTypes[1];

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
        [Description("1296")]
        public void RegistrationWithRegTypeSameEvent()
        {
            Event EventWithRegType = this.RegistrationWithRegType();

            Registrant registrant = new Registrant(emailAddress);
            registrant.Event = EventWithRegType;
            registrant.RegType = EventWithRegType.StartPage.RegTypes[0];

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
        [Description("1297")]
        public void RegistrationWithEventFeeGroupDiscount()
        {
            Event EventWithFeeGroupDiscount = new Event("RI-SingleRegistrationWithEventFeeGroupDiscount");
            RegType RegType = new RegType("First");
            RegType.Price = 50;
            EventWithFeeGroupDiscount.StartPage.RegTypes.Add(RegType);
            GroupDiscount GroupDiscount = new GroupDiscount();
            GroupDiscount.GroupSize = 2;
            GroupDiscount.GroupSizeOption = FormData.GroupSizeOption.SizeOrMore;
            GroupDiscount.DiscountAmount = 1;
            GroupDiscount.GroupDiscountType = FormData.DiscountType.USDollar;
            GroupDiscount.AddtionalRegOption = FormData.AdditionalRegOption.AnyAdditional;
            EventWithFeeGroupDiscount.StartPage.GroupDiscount = GroupDiscount;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventWithFeeGroupDiscount);

            Registrant registrant = new Registrant();
            registrant.Event = EventWithFeeGroupDiscount;
            registrant.RegType = EventWithFeeGroupDiscount.StartPage.RegTypes[0];

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1299")]
        public void RegistrationEventFeeDCDollar()
        {
            Event EventWithFeeDCDollar = new Event("RI-SingleRegistrationEventFeeDCDollar");
            RegType RegType1 = new RegType("First");
            RegType RegType2 = new RegType("Second");
            RegType2.Price = 50;
            DiscountCode DC = new DiscountCode("CodeName");
            DC.CodeType = FormData.DiscountCodeType.DiscountCode;
            DC.CodeDirection = FormData.ChangePriceDirection.Decrease;
            DC.Amount = 10;
            DC.CodeKind = FormData.ChangeType.Percent;
            RegType2.DiscountCode.Add(DC);
            EventWithFeeDCDollar.StartPage.RegTypes.Add(RegType1);
            EventWithFeeDCDollar.StartPage.RegTypes.Add(RegType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventWithFeeDCDollar);

            Registrant registrant = new Registrant();
            registrant.Event = EventWithFeeDCDollar;
            registrant.RegType = EventWithFeeDCDollar.StartPage.RegTypes[1];

            Registrant reg = new Registrant(string.Format("test{0}@test.com", System.DateTime.Now.Ticks.ToString()));

            KeywordProvider.RegisterDefault.OpenRegisterPageUrl(EventWithFeeDCDollar.Id);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(EventWithFeeDCDollar.StartPage.RegTypes[1]);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EventFeeDiscountCode.Type("abc");
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();

            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.InvalidCode));

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1298")]
        public void RegistrationEventFeeDCPercent()
        {
            Event EventWithFeeDCDollar = new Event("RI-SingleRegistrationEventFeeDCPercent");
            RegType RegType1 = new RegType("First");
            RegType RegType2 = new RegType("Second");
            RegType2.Price = 50;
            DiscountCode DC = new DiscountCode("CodeName");
            DC.CodeType = FormData.DiscountCodeType.DiscountCode;
            DC.CodeDirection = FormData.ChangePriceDirection.Decrease;
            DC.Amount = 10;
            DC.CodeKind = FormData.ChangeType.FixedAmount;
            RegType2.DiscountCode.Add(DC);
            EventWithFeeDCDollar.StartPage.RegTypes.Add(RegType1);
            EventWithFeeDCDollar.StartPage.RegTypes.Add(RegType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventWithFeeDCDollar);

            Registrant registrant = new Registrant();
            registrant.Event = EventWithFeeDCDollar;
            registrant.RegType = EventWithFeeDCDollar.StartPage.RegTypes[1];

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
        }

        public Event RegistrationEventFeeCodeRequired(FormData.DiscountCodeType type)
        {
            Event EventFeeDCRequired = new Event(string.Format("RI-SingleRegistrationEventFee{0}Required", type.ToString()));
            RegType RegType1 = new RegType("First");
            RegType RegType2 = new RegType("Second");
            RegType2.Price = 50;
            DiscountCode DC = new DiscountCode("CodeName");
            DC.CodeType = type;
            DC.CodeDirection = FormData.ChangePriceDirection.Decrease;
            DC.Amount = 10;
            DC.Limit = 1;
            DC.CodeKind = FormData.ChangeType.FixedAmount;
            RegType2.DiscountCode.Add(DC);
            RegType2.RequireDC = true;
            PaymentMethod PaymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            EventFeeDCRequired.StartPage.RegTypes.Add(RegType1);
            EventFeeDCRequired.StartPage.RegTypes.Add(RegType2);
            EventFeeDCRequired.CheckoutPage.PaymentMethods.Add(PaymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventFeeDCRequired);

            Registrant registrant = new Registrant();
            registrant.Event = EventFeeDCRequired;
            registrant.RegType = EventFeeDCRequired.StartPage.RegTypes[1];
            registrant.PaymentMethod = PaymentMethod;

            KeywordProvider.RegisterDefault.OpenRegisterPageUrl(registrant.Event.Id);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(registrant.RegType);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.DiscountCodeRequired.IsPresent);

            KeywordProvider.RegistrationCreation.CreateRegistration(registrant);

            return EventFeeDCRequired;
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1300")]
        public void RegistrationEventFeeDCRequiredLimit()
        {
            Event EventFeeDCRequiredLimit = this.RegistrationEventFeeCodeRequired(FormData.DiscountCodeType.DiscountCode);

            DiscountCode DiscountCode = new DataCollection.DiscountCode("CodeName");
            Registrant registrantWhenFull = new Registrant();
            registrantWhenFull.Event = EventFeeDCRequiredLimit;
            registrantWhenFull.RegType = EventFeeDCRequiredLimit.StartPage.RegTypes[1];

            KeywordProvider.RegistrationCreation.Checkin(registrantWhenFull);

            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(string.Format(Messages.RegisterError.CodeLimitHasReached, DiscountCode.Code)));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1301")]
        public void RegistrationEventFeeACRequiredLimit()
        {
            Event EventFeeACRequiredLimit = this.RegistrationEventFeeCodeRequired(FormData.DiscountCodeType.AccessCode);

            DiscountCode DiscountCode = new DataCollection.DiscountCode("CodeName");
            Registrant registrantWhenFull = new Registrant();
            registrantWhenFull.Event = EventFeeACRequiredLimit;
            registrantWhenFull.RegType = EventFeeACRequiredLimit.StartPage.RegTypes[1];

            KeywordProvider.RegistrationCreation.Checkin(registrantWhenFull);

            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(string.Format(Messages.RegisterError.CodeLimitHasReached, DiscountCode.Code)));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1302")]
        public void EventFeeEaylyPrice_Registrants()
        {
            Event EventFeeEaylyPrice_Registrants = new Event("RI-EventFeeEaylyPrice-Registrants");
            RegType RegType = new RegType("First");
            EarlyPrice EarlyPrice = new EarlyPrice();
            EarlyPrice.earlyPrice = 40;
            EarlyPrice.EarlyPriceType = FormData.EarlyPriceType.Registrants;
            EarlyPrice.FirstNRegistrants = 1;
            RegType.EarlyPrice = EarlyPrice;
            RegType.Price = 50;
            EventFeeEaylyPrice_Registrants.StartPage.RegTypes.Add(RegType);
            PaymentMethod PaymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            EventFeeEaylyPrice_Registrants.CheckoutPage.PaymentMethods.Add(PaymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventFeeEaylyPrice_Registrants);

            Registrant registrant = new Registrant();
            registrant.Event = EventFeeEaylyPrice_Registrants;
            registrant.RegType = EventFeeEaylyPrice_Registrants.StartPage.RegTypes[0];
            registrant.PaymentMethod = PaymentMethod;

            KeywordProvider.RegistrationCreation.Checkin(registrant);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);

            Assert.True(KeywordProvider.RegisterDefault.GetConfirmationTotal() == EarlyPrice.earlyPrice);

            KeywordProvider.RegistrationCreation.Checkout(registrant);

            Registrant registrant1 = new Registrant();
            registrant1.Event = EventFeeEaylyPrice_Registrants;
            registrant1.RegType = EventFeeEaylyPrice_Registrants.StartPage.RegTypes[0];
            registrant1.PaymentMethod = PaymentMethod;

            KeywordProvider.RegistrationCreation.Checkin(registrant1);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant1);

            Assert.True(KeywordProvider.RegisterDefault.GetConfirmationTotal() == RegType.Price.Value);

            KeywordProvider.RegistrationCreation.Checkout(registrant1);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1303")]
        public void RegistrationEarlyPriceDate()
        {
            Event EventEarlyPrice = new Event("RI-EventEarlyPrice");
            RegType RegType = new RegType("First");
            EarlyPrice EarlyPrice = new EarlyPrice();
            EarlyPrice.earlyPrice = 40;
            EarlyPrice.EarlyPriceType = FormData.EarlyPriceType.DateAndTime;
            EarlyPrice.EarlyPriceDate = System.DateTime.Today.AddDays(2);
            EarlyPrice.EarlyPriceTime = System.DateTime.Now;
            RegType.EarlyPrice = EarlyPrice;
            RegType.Price = 50;
            EventEarlyPrice.StartPage.RegTypes.Add(RegType);
            PaymentMethod PaymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            EventEarlyPrice.CheckoutPage.PaymentMethods.Add(PaymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventEarlyPrice);

            Registrant registrant = new Registrant();
            registrant.Event = EventEarlyPrice;
            registrant.RegType = EventEarlyPrice.StartPage.RegTypes[0];
            registrant.PaymentMethod = PaymentMethod;

            KeywordProvider.RegistrationCreation.Checkin(registrant);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);

            Assert.True(KeywordProvider.RegisterDefault.GetConfirmationTotal() == EarlyPrice.earlyPrice);

            KeywordProvider.RegistrationCreation.Checkout(registrant);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1304")]
        public void RegistrationLatePriceDate()
        {
            Event EventLatePrice = new Event("RI-EventLatePrice");
            RegType RegType = new RegType("First");
            LatePrice LatePrice = new LatePrice();
            LatePrice.latePrice = 60;
            LatePrice.LatePriceDate = System.DateTime.Today.AddDays(-2);
            LatePrice.LatePriceTime = System.DateTime.Now;
            RegType.LatePrice = LatePrice;
            RegType.Price = 50;
            EventLatePrice.StartPage.RegTypes.Add(RegType);
            PaymentMethod PaymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            EventLatePrice.CheckoutPage.PaymentMethods.Add(PaymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventLatePrice);

            Registrant registrant = new Registrant();
            registrant.Event = EventLatePrice;
            registrant.RegType = EventLatePrice.StartPage.RegTypes[0];
            registrant.PaymentMethod = PaymentMethod;

            KeywordProvider.RegistrationCreation.Checkin(registrant);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);

            Assert.True(KeywordProvider.RegisterDefault.GetConfirmationTotal() == LatePrice.latePrice);

            KeywordProvider.RegistrationCreation.Checkout(registrant);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1305")]
        public void RegistrationMiddlePriceDate()
        {
            Event EventMiddlePrice = new Event("RI-EventMiddlePrice");
            RegType RegType = new RegType("First");
            EarlyPrice EarlyPrice = new EarlyPrice();
            EarlyPrice.earlyPrice = 40;
            EarlyPrice.EarlyPriceType = FormData.EarlyPriceType.DateAndTime;
            EarlyPrice.EarlyPriceDate = System.DateTime.Today.AddDays(-2);
            EarlyPrice.EarlyPriceTime = System.DateTime.Now;
            LatePrice LatePrice = new LatePrice();
            LatePrice.latePrice = 60;
            LatePrice.LatePriceDate = System.DateTime.Today.AddDays(2);
            LatePrice.LatePriceTime = System.DateTime.Now;
            RegType.EarlyPrice = EarlyPrice;
            RegType.LatePrice = LatePrice;
            RegType.Price = 50;
            EventMiddlePrice.StartPage.RegTypes.Add(RegType);
            PaymentMethod PaymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            EventMiddlePrice.CheckoutPage.PaymentMethods.Add(PaymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventMiddlePrice);

            Registrant registrant = new Registrant();
            registrant.Event = EventMiddlePrice;
            registrant.RegType = EventMiddlePrice.StartPage.RegTypes[0];
            registrant.PaymentMethod = PaymentMethod;

            KeywordProvider.RegistrationCreation.Checkin(registrant);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);

            Assert.True(KeywordProvider.RegisterDefault.GetConfirmationTotal() == RegType.Price.Value);

            KeywordProvider.RegistrationCreation.Checkout(registrant);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1317")]
        public void UpdateRegAndSubstitute()
        {
            this.UniqueEmail();

            Registrant reg = new Registrant(emailAddress);
            reg.Event = evt;

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.SubstituteLink_Click(0);
            PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Email.Type(string.Format(
                "selenium{0}@regonline.com", System.DateTime.Now.Ticks.ToString()));
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.Confirmation, true);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1320")]
        public void EventFeeTax()
        {
            Event evt = new Event("RI-EventFeeTax");
            RegType regType = new RegType("First");
            TaxRate tax = new TaxRate("tax1");
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            tax.Rate = 50;
            tax.Apply = true;
            regType.Price = 50;
            regType.TaxRateOne = tax;
            evt.StartPage.RegTypes.Add(regType);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant();
            reg.Event = evt;
            reg.RegType = regType;
            reg.PaymentMethod = paymentMethod;

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);
            Assert.True(KeywordProvider.RegisterDefault.GetConfirmationTotal() == 75);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1321")]
        public void RegTypeDirectUrl()
        {
            Event evt = new Event("RI-RegTypeDirectUrl");
            RegType regType1 = new RegType("First");
            RegType regType2 = new RegType("Second");
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg = new Registrant();
            reg.Event = evt;

            KeywordProvider.RegisterDefault.OpenRegTypeDirectUrl(evt.Id, regType1.RegTypeId);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadioButton.IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeDropDown.IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            KeywordProvider.RegistrationCreation.Checkout(reg);
        }
    }
}
