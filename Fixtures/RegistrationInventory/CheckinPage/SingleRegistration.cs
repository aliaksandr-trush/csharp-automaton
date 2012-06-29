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

        private PageObject.Register.Checkin Checkin = new PageObject.Register.Checkin();
        private PageObject.Register.Login Login = new PageObject.Register.Login();
        private PageObject.Register.PersonalInfo PersonalInfo = new PageObject.Register.PersonalInfo();
        private PageObject.Register.Confirmation Confirmation = new PageObject.Register.Confirmation();
        private PageObject.Register.AttendeeCheck AttendeeCheck = new PageObject.Register.AttendeeCheck();
        private PageObject.Register.PageObjectHelper RegisterHelper = new PageObject.Register.PageObjectHelper();

        public void UniqueEmail()
        {
            string EmailAddress = string.Format("selenium{0}@regonline.com", System.DateTime.Now.Ticks.ToString());

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

            Registrant registrant = new Registrant(EmailAddress);
            registrant.Event = evt;

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            Assert.True(KeywordProvider.RegisterDefault.IsOnPersonalInfoPage());

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

            Assert.True(Login.Password.IsPresent);
            Assert.True(Login.PasswordOnDupEmail.IsPresent);

            KeywordProvider.RegistrationCreation.Login(registrant);

            Assert.True(KeywordProvider.RegisterDefault.IsOnAttendeeCheckPage());

            RegisterHelper.Continue_Click();

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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, DiffEvent, false, false);

            registrant.Event = DiffEvent;

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
        [Description("1290")]
        public void RegisterWithEventLimitFull()
        {
            Event EventWithLimit = new Event("RI-SingleRegistrationEventWithLimit");
            EventLevelLimit eventLimit = new EventLevelLimit(1);
            eventLimit.EnableWaitList = true;
            EventWithLimit.StartPage.EventLimit = eventLimit;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventWithLimit, false, true);

            Registrant registrant = new Registrant();
            registrant.Event = EventWithLimit;

            KeywordProvider.RegistrationCreation.Checkin(registrant);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);
            KeywordProvider.RegistrationCreation.Checkout(registrant);

            Registrant registrantWhenFull = new Registrant();
            registrantWhenFull.Event = EventWithLimit;

            KeywordProvider.RegisterDefault.OpenRegisterPage(EventWithLimit.Id);
            Login.StartNewRegistration_Click();

            Assert.True(Checkin.EventLimitReachedMessage.IsPresent);

            Checkin.EmailAddress.Type(registrantWhenFull.Email);
            Checkin.AddToWaitlist_Click();

            Assert.True(Checkin.AddedToWaitlistOfEvent.IsPresent);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1294")]
        public void UpdateSingleRegistration()
        {
            this.UniqueEmail();

            Confirmation.ChangeMyRegistration_Click();

            Assert.True(Login.Password.IsPresent);
            Assert.True(KeywordProvider.RegisterDefault.IsOnLoginPage());
        }

        public void RegistrationWithRegType()
        {
            Event EventWithRegType = new Event("RI-SingleRegistrationWithRegType");
            RegType RegType1 = new RegType("First");
            RegType RegType2 = new RegType("Second");
            EventWithRegType.StartPage.RegTypes.Add(RegType1);
            EventWithRegType.StartPage.RegTypes.Add(RegType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventWithRegType, false, false);

            string EmailAddress = string.Format("selenium{0}@regonline.com", System.DateTime.Now.Ticks.ToString());
            Registrant registrant = new Registrant(EmailAddress);
            registrant.Event = EventWithRegType;
            registrant.RegType = EventWithRegType.StartPage.RegTypes[1];

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            Assert.True(KeywordProvider.RegisterDefault.IsOnPersonalInfoPage());

            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);
            KeywordProvider.RegistrationCreation.Checkout(registrant);

            emailAddress = EmailAddress;
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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, DiffEventWithRegType, false, false);

            Registrant registrant = new Registrant(emailAddress);
            registrant.Event = DiffEventWithRegType;
            registrant.RegType = DiffEventWithRegType.StartPage.RegTypes[1];

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
        [Description("1296")]
        public void RegistrationWithRegTypeSameEvent()
        {
            this.RegistrationWithRegType();

            Event EventWithRegType = new Event("RI-SingleRegistrationWithRegType");
            RegType RegType = new RegType("First");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventWithRegType, false, false);

            Registrant registrant = new Registrant(emailAddress);
            registrant.Event = EventWithRegType;
            registrant.RegType = RegType;

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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventWithFeeGroupDiscount, false, false);

            Registrant registrant = new Registrant();
            registrant.Event = EventWithFeeGroupDiscount;
            registrant.RegType = EventWithFeeGroupDiscount.StartPage.RegTypes[0];

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            Assert.True(KeywordProvider.RegisterDefault.IsOnPersonalInfoPage());
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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventWithFeeDCDollar, false, false);

            Registrant registrant = new Registrant();
            registrant.Event = EventWithFeeDCDollar;
            registrant.RegType = EventWithFeeDCDollar.StartPage.RegTypes[1];

            Registrant reg = new Registrant(string.Format("test{0}@test.com", System.DateTime.Now.Ticks.ToString()));

            KeywordProvider.RegisterDefault.OpenRegisterPage(EventWithFeeDCDollar.Id);

            Checkin.EmailAddress.Type(reg.Email);
            Checkin.SelectRegTypeRadioButton(EventWithFeeDCDollar.StartPage.RegTypes[1].RegTypeName);
            Checkin.EventFeeDiscountCode.Type("abc");
            RegisterHelper.Continue_Click();

            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.InvalidCode));

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            Assert.True(KeywordProvider.RegisterDefault.IsOnPersonalInfoPage());
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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventWithFeeDCDollar, false, false);

            Registrant registrant = new Registrant();
            registrant.Event = EventWithFeeDCDollar;
            registrant.RegType = EventWithFeeDCDollar.StartPage.RegTypes[1];

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            Assert.True(KeywordProvider.RegisterDefault.IsOnPersonalInfoPage());
        }

        public void RegistrationEventFeeCodeRequired(FormData.DiscountCodeType type)
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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventFeeDCRequired, false, true);

            Registrant registrant = new Registrant();
            registrant.Event = EventFeeDCRequired;
            registrant.RegType = EventFeeDCRequired.StartPage.RegTypes[1];
            registrant.PaymentMethod = PaymentMethod;

            KeywordProvider.RegisterDefault.OpenRegisterPage(registrant.Event.Id);
            Checkin.SelectRegTypeRadioButton(registrant.RegType.RegTypeName);

            Assert.True(Checkin.DiscountCodeRequired.IsPresent);

            KeywordProvider.RegistrationCreation.CreateRegistration(registrant);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1300")]
        public void RegistrationEventFeeDCRequiredLimit()
        {
            this.RegistrationEventFeeCodeRequired(FormData.DiscountCodeType.DiscountCode);

            Event EventFeeDCRequiredLimit = new Event(string.Format("RI-SingleRegistrationEventFee{0}Required",
                FormData.DiscountCodeType.DiscountCode.ToString()));

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventFeeDCRequiredLimit, false, false);

            DiscountCode DiscountCode = new DataCollection.DiscountCode("CodeName");
            RegType RegType = new RegType("Second");
            RegType.DiscountCode.Add(DiscountCode);
            Registrant registrantWhenFull = new Registrant();
            registrantWhenFull.Event = EventFeeDCRequiredLimit;
            registrantWhenFull.RegType = RegType;

            KeywordProvider.RegistrationCreation.Checkin(registrantWhenFull);

            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(string.Format(Messages.RegisterError.CodeLimitHasReached, DiscountCode.Code)));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1301")]
        public void RegistrationEventFeeACRequiredLimit()
        {
            this.RegistrationEventFeeCodeRequired(FormData.DiscountCodeType.AccessCode);

            Event EventFeeACRequiredLimit = new Event(string.Format("RI-SingleRegistrationEventFee{0}Required",
                FormData.DiscountCodeType.AccessCode.ToString()));

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventFeeACRequiredLimit, false, false);

            DiscountCode DiscountCode = new DataCollection.DiscountCode("CodeName");
            RegType RegType = new RegType("Second");
            RegType.DiscountCode.Add(DiscountCode);
            Registrant registrantWhenFull = new Registrant();
            registrantWhenFull.Event = EventFeeACRequiredLimit;
            registrantWhenFull.RegType = RegType;

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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventFeeEaylyPrice_Registrants, false, true);

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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventEarlyPrice, true, false);

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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventLatePrice, true, false);

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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, EventMiddlePrice, true, false);

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

            Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(reg);
            AttendeeCheck.SubstituteLink_Click(0);
            PersonalInfo.Email.Type(string.Format(
                "selenium{0}@regonline.com", System.DateTime.Now.Ticks.ToString()));
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            Assert.True(KeywordProvider.RegisterDefault.IsOnConfirmationPage());
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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false, false);

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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, true, false);

            Registrant reg = new Registrant();
            reg.Event = evt;

            KeywordProvider.RegisterDefault.OpenRegTypeDirectUrl(evt.Id, regType1.RegTypeId);
            Assert.False(Checkin.RegTypeRadioButton.IsPresent);
            Assert.False(Checkin.RegTypeDropDown.IsPresent);
            Checkin.EmailAddress.Type(reg.Email);
            RegisterHelper.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            KeywordProvider.RegistrationCreation.Checkout(reg);
        }
    }
}
