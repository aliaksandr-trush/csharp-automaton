namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.CheckinPage
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class RegTypeCreation : FixtureBase
    {
        [Test]
        [Category(Priority.Three)]
        [Description("1322")]
        public void RegTypeWithFee()
        {
            Event evt = new Event("RI-RegTypeWithFee");

            RegType regType1 = new RegType("EarlyRegistrantsLateDate");
            EarlyPrice earlyPrice1 = new EarlyPrice();
            earlyPrice1.earlyPrice = 40;
            earlyPrice1.EarlyPriceType = FormData.EarlyPriceType.Registrants;
            earlyPrice1.FirstNRegistrants = 1;
            LatePrice latePrice1 = new LatePrice();
            latePrice1.latePrice = 60;
            latePrice1.LatePriceDate = DateTime.Today.AddDays(2);
            latePrice1.LatePriceTime = DateTime.Now;
            regType1.Price = 50;
            regType1.EarlyPrice = earlyPrice1;
            regType1.LatePrice = latePrice1;

            RegType regType2 = new RegType("EarlyLateFeeDCDollar");
            EarlyPrice earlyPrice2 = new EarlyPrice();
            earlyPrice2.earlyPrice = 40;
            earlyPrice2.EarlyPriceType = FormData.EarlyPriceType.DateAndTime;
            earlyPrice2.EarlyPriceDate = DateTime.Today.AddDays(-2);
            earlyPrice2.EarlyPriceTime = DateTime.Now;
            LatePrice latePrice2 = new LatePrice();
            latePrice2.latePrice = 60;
            latePrice2.LatePriceDate = DateTime.Today.AddDays(2);
            latePrice2.LatePriceTime = DateTime.Now;
            DiscountCode discountCode2 = new DiscountCode("code2");
            discountCode2.Amount = 5;
            discountCode2.CodeDirection = FormData.ChangePriceDirection.Decrease;
            discountCode2.CodeKind = FormData.ChangeType.FixedAmount;
            discountCode2.CodeType = FormData.DiscountCodeType.DiscountCode;
            regType2.Price = 50;
            regType2.EarlyPrice = earlyPrice2;
            regType2.LatePrice = latePrice2;
            regType2.DiscountCode.Add(discountCode2);

            RegType regType3 = new RegType("EarlyLateFeeDCPercent");
            EarlyPrice earlyPrice3 = new EarlyPrice();
            earlyPrice3.earlyPrice = 40;
            earlyPrice3.EarlyPriceType = FormData.EarlyPriceType.DateAndTime;
            earlyPrice3.EarlyPriceDate = DateTime.Today.AddDays(-2);
            earlyPrice3.EarlyPriceTime = DateTime.Now;
            LatePrice latePrice3 = new LatePrice();
            latePrice3.latePrice = 60;
            latePrice3.LatePriceDate = DateTime.Today.AddDays(2);
            latePrice3.LatePriceTime = DateTime.Now;
            DiscountCode discountCode3 = new DiscountCode("code3");
            discountCode3.Amount = 10;
            discountCode3.CodeDirection = FormData.ChangePriceDirection.Decrease;
            discountCode3.CodeKind = FormData.ChangeType.Percent;
            discountCode3.CodeType = FormData.DiscountCodeType.DiscountCode;
            regType3.Price = 50;
            regType3.EarlyPrice = earlyPrice3;
            regType3.LatePrice = latePrice3;
            regType3.DiscountCode.Add(discountCode3);

            RegType regType4 = new RegType("EarlyLateFeeDCPositiveDollar");
            EarlyPrice earlyPrice4 = new EarlyPrice();
            earlyPrice4.earlyPrice = 40;
            earlyPrice4.EarlyPriceType = FormData.EarlyPriceType.DateAndTime;
            earlyPrice4.EarlyPriceDate = DateTime.Today.AddDays(-2);
            earlyPrice4.EarlyPriceTime = DateTime.Now;
            LatePrice latePrice4 = new LatePrice();
            latePrice4.latePrice = 60;
            latePrice4.LatePriceDate = DateTime.Today.AddDays(2);
            latePrice4.LatePriceTime = DateTime.Now;
            DiscountCode discountCode4 = new DiscountCode("code4");
            discountCode4.Amount = -5;
            discountCode4.CodeDirection = FormData.ChangePriceDirection.Decrease;
            discountCode4.CodeKind = FormData.ChangeType.FixedAmount;
            discountCode4.CodeType = FormData.DiscountCodeType.DiscountCode;
            regType4.Price = 50;
            regType4.EarlyPrice = earlyPrice4;
            regType4.LatePrice = latePrice4;
            regType4.DiscountCode.Add(discountCode4);

            RegType regType5 = new RegType("EarlyLateFeeDCRequired");
            EarlyPrice earlyPrice5 = new EarlyPrice();
            earlyPrice5.earlyPrice = 40;
            earlyPrice5.EarlyPriceType = FormData.EarlyPriceType.DateAndTime;
            earlyPrice5.EarlyPriceDate = DateTime.Today.AddDays(-2);
            earlyPrice5.EarlyPriceTime = DateTime.Now;
            LatePrice latePrice5 = new LatePrice();
            latePrice5.latePrice = 60;
            latePrice5.LatePriceDate = DateTime.Today.AddDays(2);
            latePrice5.LatePriceTime = DateTime.Now;
            DiscountCode discountCode5 = new DiscountCode("code5");
            discountCode5.Amount = 5;
            discountCode5.CodeDirection = FormData.ChangePriceDirection.Decrease;
            discountCode5.CodeKind = FormData.ChangeType.FixedAmount;
            discountCode5.CodeType = FormData.DiscountCodeType.DiscountCode;
            regType5.Price = 50;
            regType5.EarlyPrice = earlyPrice5;
            regType5.LatePrice = latePrice5;
            regType5.DiscountCode.Add(discountCode5);
            regType5.RequireDC = true;

            RegType regType6 = new RegType("EarlyLateFeeAC");
            EarlyPrice earlyPrice6 = new EarlyPrice();
            earlyPrice6.earlyPrice = 40;
            earlyPrice6.EarlyPriceType = FormData.EarlyPriceType.DateAndTime;
            earlyPrice6.EarlyPriceDate = DateTime.Today.AddDays(-2);
            earlyPrice6.EarlyPriceTime = DateTime.Now;
            LatePrice latePrice6 = new LatePrice();
            latePrice6.latePrice = 60;
            latePrice6.LatePriceDate = DateTime.Today.AddDays(2);
            latePrice6.LatePriceTime = DateTime.Now;
            DiscountCode discountCode6 = new DiscountCode("code6");
            discountCode6.CodeType = FormData.DiscountCodeType.AccessCode;
            regType6.Price = 50;
            regType6.EarlyPrice = earlyPrice6;
            regType6.LatePrice = latePrice6;
            regType6.DiscountCode.Add(discountCode6);

            EarlyPrice earlyPrice7 = new EarlyPrice();
            earlyPrice7.earlyPrice = 40;
            earlyPrice7.EarlyPriceType = FormData.EarlyPriceType.DateAndTime;
            earlyPrice7.EarlyPriceDate = DateTime.Today.AddDays(-2);
            earlyPrice7.EarlyPriceTime = DateTime.Now;
            LatePrice latePrice7 = new LatePrice();
            latePrice7.latePrice = 60;
            latePrice7.LatePriceDate = DateTime.Today.AddDays(2);
            latePrice7.LatePriceTime = DateTime.Now;
            DiscountCode discountCode7 = new DiscountCode("code7");
            discountCode7.Amount = 5;
            discountCode7.CodeDirection = FormData.ChangePriceDirection.Decrease;
            discountCode7.CodeKind = FormData.ChangeType.FixedAmount;
            discountCode7.CodeType = FormData.DiscountCodeType.DiscountCode;

            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);

            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);
            evt.StartPage.RegTypes.Add(regType3);
            evt.StartPage.RegTypes.Add(regType4);
            evt.StartPage.RegTypes.Add(regType5);
            evt.StartPage.RegTypes.Add(regType6);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg1 = new Registrant(evt);
            reg1.EventFee_Response = new EventFeeResponse(regType1);
            reg1.Payment_Method = paymentMethod;

            KeywordProvider.RegistrationCreation.CreateRegistration(reg1);
            Assert.True(reg1.EventFee_Response.RegType.EarlyPrice.earlyPrice.Equals(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation)));

            Registrant reg2 = new Registrant(evt);
            reg2.EventFee_Response = new EventFeeResponse(regType1);
            reg2.Payment_Method = paymentMethod;

            KeywordProvider.RegistrationCreation.CreateRegistration(reg2);
            Assert.True(reg2.EventFee_Response.RegType.Price.Value.Equals(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation)));

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg2);
            PageObject.PageObjectProvider.Register.RegistationSite.Login.StartNewRegistration_Click();
            KeywordProvider.RegisterDefault.SelectRegType(regType2);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EventFeeDiscountCode.IsPresent);
            KeywordProvider.RegisterDefault.SelectRegType(regType3);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EventFeeDiscountCode.IsPresent);
            KeywordProvider.RegisterDefault.SelectRegType(regType4);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EventFeeDiscountCode.IsPresent);
            KeywordProvider.RegisterDefault.SelectRegType(regType5);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EventFeeDiscountCode.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.DiscountCodeRequired.IsPresent);
            KeywordProvider.RegisterDefault.SelectRegType(regType6);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EventFeeDiscountCode.IsPresent);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1323")]
        public void DisableGroupReg()
        {
            Event evt = new Event("RI-DisableGroupReg");
            evt.StartPage.AllowGroupReg = false;
            RegType regType = new RegType("AdditionalDetails");
            regType.AdditionalDetails = "This is additional details.";
            evt.StartPage.RegTypes.Add(regType);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false);

            Registrant reg = new Registrant(evt);
            reg.EventFee_Response = new EventFeeResponse(regType);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeDetails_Click(reg.EventFee_Response.RegType);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.AdditionalDetails.Text.Trim().Equals(regType.AdditionalDetails));
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.AdditionalDetailsClose_Click();
            KeywordProvider.RegistrationCreation.Checkin(reg);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson.IsPresent);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson.IsPresent);
            KeywordProvider.RegistrationCreation.Checkout(reg);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1324")]
        public void RegTypeOptions()
        {
            Event evt1 = new Event("RI-RegTypeOptions-Radio");

            RegType regType1 = new RegType("MinMaxGroupSize");
            regType1.MinGroupSize = 3;
            regType1.MinRegistrantMessage = "This is minimum registrant message";
            regType1.MaxGroupSize = 5;

            RegType regType2 = new RegType("ShowHideDate");
            regType2.ShowStarting = DateTime.Today.AddDays(-2);
            regType2.HideStarting = DateTime.Today.AddDays(2);

            RegType regType3 = new RegType("VisibleToAll");
            regType3.IsPublic = true;
            regType3.IsAdmin = true;
            regType3.IsOnSite = true;

            RegType regType4 = new RegType("PublicAndAdmin");
            regType4.IsPublic = true;
            regType4.IsAdmin = true;
            regType4.IsOnSite = false;

            RegType regType5 = new RegType("AdminAndOnSite");
            regType5.IsPublic = false;
            regType5.IsAdmin = true;
            regType5.IsOnSite = true;

            RegType regType6 = new RegType("IndividualLimit");
            RegTypeLimit limit1 = new RegTypeLimit();
            limit1.LimitType = FormData.RegLimitType.Individual;
            limit1.LimitTo = 2;
            limit1.SoldOutMessage = "Sold out!";
            regType6.RegTypeLimit = limit1;

            RegType regType7 = new RegType("GroupLimit");
            RegTypeLimit limit2 = new RegTypeLimit();
            limit2.LimitType = FormData.RegLimitType.Group;
            limit2.LimitTo = 2;
            limit2.SoldOutMessage = "Sold out!";
            regType7.RegTypeLimit = limit2;

            RegType regType8 = new RegType("VisibleToNone");
            regType8.IsPublic = false;
            regType8.IsAdmin = false;
            regType8.IsOnSite = false;

            evt1.StartPage.RegTypes.Add(regType1);
            evt1.StartPage.RegTypes.Add(regType2);
            evt1.StartPage.RegTypes.Add(regType3);
            evt1.StartPage.RegTypes.Add(regType4);
            evt1.StartPage.RegTypes.Add(regType5);
            evt1.StartPage.RegTypes.Add(regType6);
            evt1.StartPage.RegTypes.Add(regType7);
            evt1.StartPage.RegTypes.Add(regType8);
            evt1.StartPage.RegTypeDisplayOption = FormData.RegTypeDisplayOption.RadioButton;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt1);

            Registrant reg = new Registrant(evt1);

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadioButton.IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType1).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType2).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType3).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType4).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType5).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType6).IsPresent);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType7).IsPresent);
            Assert.False(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeRadio(regType8).IsPresent);

            Event evt2 = new Event("RI-RegTypeOptions-DropDown");

            evt2.StartPage.RegTypes.Add(regType1);
            evt2.StartPage.RegTypes.Add(regType2);
            evt2.StartPage.RegTypes.Add(regType3);
            evt2.StartPage.RegTypes.Add(regType4);
            evt2.StartPage.RegTypes.Add(regType5);
            evt2.StartPage.RegTypes.Add(regType6);
            evt2.StartPage.RegTypes.Add(regType7);
            evt2.StartPage.RegTypes.Add(regType8);
            evt2.StartPage.RegTypeDisplayOption = FormData.RegTypeDisplayOption.DropDownList;

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt2);
            reg.Event = evt2;
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            Assert.True(PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeDropDown.IsPresent);
        }
    }
}
