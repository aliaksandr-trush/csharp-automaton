namespace RegOnline.RegressionTest.Fixtures.New.Checkin
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

        public Registrant UniqueEmail()
        {
            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, this.evt, false);

            Registrant registrant = new Registrant(this.evt, string.Format("selenium{0}@regonline.com", System.DateTime.Now.Ticks.ToString()));

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);

            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);
            KeywordProvider.RegistrationCreation.Checkout(registrant);

            return registrant;
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1289")]
        public void UsedEmail()
        {
            Registrant registrant = this.UniqueEmail();

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
            Registrant registrant = this.UniqueEmail();

            Event diffEvent = new Event("RI-SingleRegistrationDiffEvent");

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, diffEvent);

            registrant.Event = diffEvent;

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
        [Description("1290")]
        public void RegisterWithEventLimitFull()
        {
            Event eventWithLimit = new Event("RI-SingleRegistrationEventWithLimit");
            EventLevelLimit eventLimit = new EventLevelLimit(1);
            eventLimit.EnableWaitList = true;
            eventWithLimit.StartPage.EventLimit = eventLimit;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, eventWithLimit);

            Registrant registrant = new Registrant(eventWithLimit);

            KeywordProvider.RegistrationCreation.Checkin(registrant);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);
            KeywordProvider.RegistrationCreation.Checkout(registrant);

            Registrant registrantWhenFull = new Registrant(eventWithLimit);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(registrantWhenFull);
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

        public Registrant CreateEventAndRegisterWithRegType()
        {
            Event eventWithRegType = new Event("RI-SingleRegistrationWithRegType");
            RegType regType1 = new RegType("First");
            RegType regType2 = new RegType("Second");
            eventWithRegType.StartPage.RegTypes.Add(regType1);
            eventWithRegType.StartPage.RegTypes.Add(regType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, eventWithRegType);

            Registrant registrant = new Registrant(eventWithRegType, string.Format("selenium{0}@regonline.com", System.DateTime.Now.Ticks.ToString()));
            registrant.EventFee_Response = new EventFeeResponse(eventWithRegType.StartPage.RegTypes[1]);

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);

            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);
            KeywordProvider.RegistrationCreation.Checkout(registrant);

            return registrant;
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1295")]
        public void RegistrationWithRegTypeDiffEvent()
        {
            Registrant registrant = this.CreateEventAndRegisterWithRegType();

            Event diffEventWithRegType = new Event("RI-SingleRegistrationWithRegTypeDiffEvent");
            RegType regType1 = new RegType("Third");
            RegType regType2 = new RegType("Fourth");
            diffEventWithRegType.StartPage.RegTypes.Add(regType1);
            diffEventWithRegType.StartPage.RegTypes.Add(regType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, diffEventWithRegType);

            registrant.Event = diffEventWithRegType;
            registrant.EventFee_Response = new EventFeeResponse(diffEventWithRegType.StartPage.RegTypes[1]);

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
        [Description("1296")]
        public void RegistrationWithRegTypeSameEvent()
        {
            Registrant registrant = this.CreateEventAndRegisterWithRegType();

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
            Event eventWithFeeGroupDiscount = new Event("RI-SingleRegistrationWithEventFeeGroupDiscount");
            RegType regType = new RegType("First");
            regType.Price = 50;
            eventWithFeeGroupDiscount.StartPage.RegTypes.Add(regType);
            GroupDiscount GroupDiscount = new GroupDiscount();
            GroupDiscount.GroupSize = 2;
            GroupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.SizeOrMore;
            GroupDiscount.DiscountAmount = 1;
            GroupDiscount.GroupDiscountType = GroupDiscount_DiscountType.USDollar;
            GroupDiscount.AddtionalRegOption = GroupDiscount_AdditionalRegOption.AnyAdditional;
            eventWithFeeGroupDiscount.StartPage.GroupDiscount = GroupDiscount;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, eventWithFeeGroupDiscount);

            Registrant registrant = new Registrant(eventWithFeeGroupDiscount);
            registrant.EventFee_Response = new EventFeeResponse(eventWithFeeGroupDiscount.StartPage.RegTypes[0]);

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1299")]
        public void RegistrationEventFeeDCDollar()
        {
            Event eventWithFeeDCDollar = new Event("RI-SingleRegistrationEventFeeDCDollar");
            RegType regType1 = new RegType("First");
            RegType regType2 = new RegType("Second");
            regType2.Price = 50;
            DiscountCode dc = new DiscountCode("CodeName");
            dc.CodeType = FormData.DiscountCodeType.DiscountCode;
            dc.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc.Amount = 10;
            dc.CodeKind = FormData.ChangeType.Percent;
            regType2.DiscountCode.Add(dc);
            eventWithFeeDCDollar.StartPage.RegTypes.Add(regType1);
            eventWithFeeDCDollar.StartPage.RegTypes.Add(regType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, eventWithFeeDCDollar);

            Registrant registrant = new Registrant(eventWithFeeDCDollar);
            registrant.EventFee_Response = new EventFeeResponse(eventWithFeeDCDollar.StartPage.RegTypes[1]);

            Registrant reg = new Registrant(eventWithFeeDCDollar, string.Format("test{0}@test.com", System.DateTime.Now.Ticks.ToString()));

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(eventWithFeeDCDollar.StartPage.RegTypes[1]);
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
            Event eventWithFeeDCDollar = new Event("RI-SingleRegistrationEventFeeDCPercent");
            RegType regType1 = new RegType("First");
            RegType regType2 = new RegType("Second");
            regType2.Price = 50;
            DiscountCode dc = new DiscountCode("CodeName");
            dc.CodeType = FormData.DiscountCodeType.DiscountCode;
            dc.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc.Amount = 10;
            dc.CodeKind = FormData.ChangeType.FixedAmount;
            regType2.DiscountCode.Add(dc);
            eventWithFeeDCDollar.StartPage.RegTypes.Add(regType1);
            eventWithFeeDCDollar.StartPage.RegTypes.Add(regType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, eventWithFeeDCDollar);

            Registrant registrant = new Registrant(eventWithFeeDCDollar);
            registrant.EventFee_Response = new EventFeeResponse(eventWithFeeDCDollar.StartPage.RegTypes[1]);

            KeywordProvider.RegistrationCreation.Checkin(registrant);

            AssertHelper.VerifyOnPage(FormData.RegisterPage.PersonalInfo, true);
        }

        public Registrant RegistrationEventFeeCodeRequired(FormData.DiscountCodeType type)
        {
            Event eventFeeDCRequired = new Event(string.Format("RI-SingleRegistrationEventFee{0}Required", type.ToString()));
            RegType regType1 = new RegType("First");
            RegType regType2 = new RegType("Second");
            regType2.Price = 50;
            DiscountCode dc = new DiscountCode("CodeName");
            dc.CodeType = type;
            dc.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc.Amount = 10;
            dc.Limit = 1;
            dc.CodeKind = FormData.ChangeType.FixedAmount;
            regType2.DiscountCode.Add(dc);
            regType2.RequireDC = true;
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            eventFeeDCRequired.StartPage.RegTypes.Add(regType1);
            eventFeeDCRequired.StartPage.RegTypes.Add(regType2);
            eventFeeDCRequired.CheckoutPage.PaymentMethods.Add(paymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, eventFeeDCRequired);

            Registrant registrant = new Registrant(eventFeeDCRequired);
            registrant.EventFee_Response = new EventFeeResponse(eventFeeDCRequired.StartPage.RegTypes[1]);
            registrant.EventFee_Response.Code = dc;
            registrant.Payment_Method = paymentMethod;

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(registrant);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(registrant.EventFee_Response.RegType);

            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.DiscountCodeRequired.IsPresent);

            KeywordProvider.RegistrationCreation.CreateRegistration(registrant);

            return registrant;
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1300")]
        public void RegistrationEventFeeDCRequiredLimit()
        {
            Registrant reg = this.RegistrationEventFeeCodeRequired(FormData.DiscountCodeType.DiscountCode);

            DiscountCode discountCode = new DataCollection.DiscountCode("CodeName");
            Registrant registrantWhenFull = new Registrant(reg.Event);
            registrantWhenFull.EventFee_Response = new EventFeeResponse(reg.Event.StartPage.RegTypes[1]);
            registrantWhenFull.EventFee_Response.Code = discountCode;

            KeywordProvider.RegistrationCreation.Checkin(registrantWhenFull);

            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(string.Format(Messages.RegisterError.RegTypeCodeLimitHasReachedAndRequired, discountCode.Code)));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1301")]
        public void RegistrationEventFeeACRequiredLimit()
        {
            Registrant reg = this.RegistrationEventFeeCodeRequired(FormData.DiscountCodeType.AccessCode);

            DiscountCode discountCode = new DataCollection.DiscountCode("CodeName");
            Registrant registrantWhenFull = new Registrant(reg.Event);
            registrantWhenFull.EventFee_Response = new EventFeeResponse(reg.Event.StartPage.RegTypes[1]);
            registrantWhenFull.EventFee_Response.Code = discountCode;

            KeywordProvider.RegistrationCreation.Checkin(registrantWhenFull);

            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(string.Format(Messages.RegisterError.RegTypeCodeLimitHasReachedAndRequired, discountCode.Code)));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1302")]
        public void EventFeeEaylyPrice_Registrants()
        {
            Event eventFeeEaylyPrice_Registrants = new Event("RI-EventFeeEaylyPrice-Registrants");
            RegType regType = new RegType("First");
            EarlyPrice earlyPrice = new EarlyPrice();
            earlyPrice.earlyPrice = 40;
            earlyPrice.EarlyPriceType = FormData.EarlyPriceType.Registrants;
            earlyPrice.FirstNRegistrants = 1;
            regType.EarlyPrice = earlyPrice;
            regType.Price = 50;
            eventFeeEaylyPrice_Registrants.StartPage.RegTypes.Add(regType);
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            eventFeeEaylyPrice_Registrants.CheckoutPage.PaymentMethods.Add(paymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, eventFeeEaylyPrice_Registrants);

            Registrant registrant = new Registrant(eventFeeEaylyPrice_Registrants);
            registrant.EventFee_Response = new EventFeeResponse(eventFeeEaylyPrice_Registrants.StartPage.RegTypes[0]);
            registrant.Payment_Method = paymentMethod;

            KeywordProvider.RegistrationCreation.Checkin(registrant);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);

            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == earlyPrice.earlyPrice);

            KeywordProvider.RegistrationCreation.Checkout(registrant);

            Registrant registrant1 = new Registrant(eventFeeEaylyPrice_Registrants);
            registrant1.EventFee_Response = new EventFeeResponse(eventFeeEaylyPrice_Registrants.StartPage.RegTypes[0]);
            registrant1.Payment_Method = paymentMethod;

            KeywordProvider.RegistrationCreation.Checkin(registrant1);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant1);

            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == regType.Price.Value);

            KeywordProvider.RegistrationCreation.Checkout(registrant1);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1303")]
        public void RegistrationEarlyPriceDate()
        {
            Event eventEarlyPrice = new Event("RI-EventEarlyPrice");
            RegType regType = new RegType("First");
            EarlyPrice earlyPrice = new EarlyPrice();
            earlyPrice.earlyPrice = 40;
            earlyPrice.EarlyPriceType = FormData.EarlyPriceType.DateAndTime;
            earlyPrice.EarlyPriceDate = System.DateTime.Today.AddDays(2);
            earlyPrice.EarlyPriceTime = System.DateTime.Now;
            regType.EarlyPrice = earlyPrice;
            regType.Price = 50;
            eventEarlyPrice.StartPage.RegTypes.Add(regType);
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            eventEarlyPrice.CheckoutPage.PaymentMethods.Add(paymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, eventEarlyPrice);

            Registrant registrant = new Registrant(eventEarlyPrice);
            registrant.EventFee_Response = new EventFeeResponse(eventEarlyPrice.StartPage.RegTypes[0]);
            registrant.Payment_Method = paymentMethod;

            KeywordProvider.RegistrationCreation.Checkin(registrant);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);

            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == earlyPrice.earlyPrice);

            KeywordProvider.RegistrationCreation.Checkout(registrant);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1304")]
        public void RegistrationLatePriceDate()
        {
            Event eventLatePrice = new Event("RI-EventLatePrice");
            RegType regType = new RegType("First");
            LatePrice latePrice = new LatePrice();
            latePrice.latePrice = 60;
            latePrice.LatePriceDate = System.DateTime.Today.AddDays(-2);
            latePrice.LatePriceTime = System.DateTime.Now;
            regType.LatePrice = latePrice;
            regType.Price = 50;
            eventLatePrice.StartPage.RegTypes.Add(regType);
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            eventLatePrice.CheckoutPage.PaymentMethods.Add(paymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, eventLatePrice);

            Registrant registrant = new Registrant(eventLatePrice);
            registrant.EventFee_Response = new EventFeeResponse(eventLatePrice.StartPage.RegTypes[0]);
            registrant.Payment_Method = paymentMethod;

            KeywordProvider.RegistrationCreation.Checkin(registrant);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);

            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == latePrice.latePrice);

            KeywordProvider.RegistrationCreation.Checkout(registrant);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1305")]
        public void RegistrationMiddlePriceDate()
        {
            Event eventMiddlePrice = new Event("RI-EventMiddlePrice");
            RegType regType = new RegType("First");
            EarlyPrice earlyPrice = new EarlyPrice();
            earlyPrice.earlyPrice = 40;
            earlyPrice.EarlyPriceType = FormData.EarlyPriceType.DateAndTime;
            earlyPrice.EarlyPriceDate = System.DateTime.Today.AddDays(-2);
            earlyPrice.EarlyPriceTime = System.DateTime.Now;
            LatePrice latePrice = new LatePrice();
            latePrice.latePrice = 60;
            latePrice.LatePriceDate = System.DateTime.Today.AddDays(2);
            latePrice.LatePriceTime = System.DateTime.Now;
            regType.EarlyPrice = earlyPrice;
            regType.LatePrice = latePrice;
            regType.Price = 50;
            eventMiddlePrice.StartPage.RegTypes.Add(regType);
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            eventMiddlePrice.CheckoutPage.PaymentMethods.Add(paymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, eventMiddlePrice);

            Registrant registrant = new Registrant(eventMiddlePrice);
            registrant.EventFee_Response = new EventFeeResponse(eventMiddlePrice.StartPage.RegTypes[0]);
            registrant.Payment_Method = paymentMethod;

            KeywordProvider.RegistrationCreation.Checkin(registrant);
            KeywordProvider.RegistrationCreation.PersonalInfo(registrant);

            Assert.True(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation) == regType.Price.Value);

            KeywordProvider.RegistrationCreation.Checkout(registrant);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1317")]
        public void UpdateRegAndSubstitute()
        {
            Registrant reg = this.UniqueEmail();

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.SubstituteLink_Click(0);

            PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Email.Type(
                string.Format("selenium{0}@regonline.com", System.DateTime.Now.Ticks.ToString()));

            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            AssertHelper.VerifyOnPage(FormData.RegisterPage.Confirmation, true);
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

            Registrant reg = new Registrant(evt);
            reg.EventFee_Response = new EventFeeResponse(regType1);
            reg.Register_Method = RegisterMethod.RegTypeDirectUrl;

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadioButton.IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeDropDown.IsPresent);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            KeywordProvider.RegistrationCreation.Checkout(reg);
        }
    }
}
