namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.AgendaPage
{
    using Fixtures.Base;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgendaWithPrice : FixtureBase
    {
        [Test]
        [Category(Priority.Three)]
        public void AgendaPrice()
        {
            DataCollection.Event evt = new DataCollection.Event("AgendaPrice");
            evt.AgendaPage = new DataCollection.AgendaPage();
            DataCollection.AgendaItemCheckBox aGStandardPrice = new DataCollection.AgendaItemCheckBox("AGStandardPrice");
            aGStandardPrice.Price = 50;
            DataCollection.AgendaItemCheckBox aGEarlyPriceDateTime = new DataCollection.AgendaItemCheckBox("AGEarlyPriceDateTime");
            aGEarlyPriceDateTime.Price = 110;
            DataCollection.EarlyPrice earlyPrice1 = new DataCollection.EarlyPrice();
            earlyPrice1.earlyPrice = 90;
            earlyPrice1.EarlyPriceType = DataCollection.FormData.EarlyPriceType.DateAndTime;
            earlyPrice1.EarlyPriceDate = System.DateTime.Today.AddDays(3);
            earlyPrice1.EarlyPriceTime = System.DateTime.Now;
            aGEarlyPriceDateTime.EarlyPrice = earlyPrice1;
            DataCollection.AgendaItemCheckBox aGEarlyPriceRegs = new DataCollection.AgendaItemCheckBox("AGEarlyPriceRegs");
            aGEarlyPriceRegs.Price = 150;
            DataCollection.EarlyPrice earlyPrice2 = new DataCollection.EarlyPrice();
            earlyPrice2.earlyPrice = 140;
            earlyPrice2.EarlyPriceType = DataCollection.FormData.EarlyPriceType.Registrants;
            earlyPrice2.FirstNRegistrants = 1;
            aGEarlyPriceRegs.EarlyPrice = earlyPrice2;
            DataCollection.AgendaItemCheckBox aGLatePrice = new DataCollection.AgendaItemCheckBox("AGLatePrice");
            aGLatePrice.Price = 200;
            DataCollection.LatePrice latePrice = new DataCollection.LatePrice();
            latePrice.latePrice = 210;
            latePrice.LatePriceDate = System.DateTime.Today.AddDays(-3);
            latePrice.LatePriceTime = System.DateTime.Now;
            aGLatePrice.LatePrice = latePrice;
            evt.AgendaPage.AgendaItems.Add(aGStandardPrice);
            evt.AgendaPage.AgendaItems.Add(aGEarlyPriceDateTime);
            evt.AgendaPage.AgendaItems.Add(aGEarlyPriceRegs);
            evt.AgendaPage.AgendaItems.Add(aGLatePrice);
            DataCollection.PaymentMethod paymentMethod = new DataCollection.PaymentMethod(DataCollection.FormData.PaymentMethod.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.RegistrationInventory, evt);

            DataCollection.Registrant reg1 = new DataCollection.Registrant();
            reg1.Event = evt;
            DataCollection.AgendaCheckboxResponse resp1 = new DataCollection.AgendaCheckboxResponse();
            resp1.AgendaItem = aGStandardPrice;
            resp1.Checked = true;
            DataCollection.AgendaCheckboxResponse resp2 = new DataCollection.AgendaCheckboxResponse();
            resp2.AgendaItem = aGEarlyPriceDateTime;
            resp2.Checked = true;
            DataCollection.AgendaCheckboxResponse resp3 = new DataCollection.AgendaCheckboxResponse();
            resp3.AgendaItem = aGEarlyPriceRegs;
            resp3.Checked = true;
            DataCollection.AgendaCheckboxResponse resp4 = new DataCollection.AgendaCheckboxResponse();
            resp4.AgendaItem = aGLatePrice;
            resp4.Checked = true;
            reg1.CustomFieldResponses.Add(resp1);
            reg1.CustomFieldResponses.Add(resp2);
            reg1.CustomFieldResponses.Add(resp3);
            reg1.CustomFieldResponses.Add(resp4);
            reg1.PaymentMethod = paymentMethod;

            KeywordProvider.RegistrationCreation.CreateRegistration(reg1);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetConfirmationTotal(),aGStandardPrice.Price
                + aGEarlyPriceDateTime.EarlyPrice.earlyPrice + aGEarlyPriceRegs.EarlyPrice.earlyPrice + aGLatePrice.LatePrice.latePrice);

            DataCollection.Registrant reg2 = new DataCollection.Registrant();
            reg2.Event = evt;
            reg2.CustomFieldResponses.Add(resp1);
            reg2.CustomFieldResponses.Add(resp2);
            reg2.CustomFieldResponses.Add(resp3);
            reg2.CustomFieldResponses.Add(resp4);
            reg2.PaymentMethod = paymentMethod;

            KeywordProvider.RegistrationCreation.CreateRegistration(reg2);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetConfirmationTotal(), aGStandardPrice.Price
                + aGEarlyPriceDateTime.EarlyPrice.earlyPrice + aGEarlyPriceRegs.Price + aGLatePrice.LatePrice.latePrice);
        }

        [Test]
        [Category(Priority.Three)]
        public void AgendaDiscountCode()
        {
            DataCollection.Event evt = new DataCollection.Event("AgendaDiscountCode");
            DataCollection.PaymentMethod paymentMethod = new DataCollection.PaymentMethod(DataCollection.FormData.PaymentMethod.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            evt.AgendaPage = new DataCollection.AgendaPage();
            DataCollection.AgendaItemCheckBox agendaDCPercent = new DataCollection.AgendaItemCheckBox("AgendaDCPercent");
            agendaDCPercent.Price = 50;
            DataCollection.DiscountCode dCPercent = new DataCollection.DiscountCode("DCPercent");
            dCPercent.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCPercent.Amount = 10;
            dCPercent.CodeKind = DataCollection.FormData.ChangeType.Percent;
            agendaDCPercent.DiscountCode.Add(dCPercent);
            DataCollection.AgendaItemCheckBox agendaDCFix = new DataCollection.AgendaItemCheckBox("AgendaDCFix");
            agendaDCFix.Price = 50;
            DataCollection.DiscountCode dCFix = new DataCollection.DiscountCode("DCFix");
            dCFix.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCFix.Amount = 10;
            dCFix.CodeKind = DataCollection.FormData.ChangeType.FixedAmount;
            agendaDCFix.DiscountCode.Add(dCFix);
            DataCollection.AgendaItemCheckBox agendaDCPercentUnderZero = new DataCollection.AgendaItemCheckBox("AgendaDCPercentUnderZero");
            agendaDCPercentUnderZero.Price = 50;
            DataCollection.DiscountCode dCPercentUnderZero = new DataCollection.DiscountCode("DCPercentUnderZero");
            dCPercentUnderZero.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCPercentUnderZero.Amount = -10;
            dCPercentUnderZero.CodeKind = DataCollection.FormData.ChangeType.Percent;
            agendaDCPercentUnderZero.DiscountCode.Add(dCPercentUnderZero);
            DataCollection.AgendaItemCheckBox agendaDCFixUnderZero = new DataCollection.AgendaItemCheckBox("AgendaDCFixUnderZero");
            agendaDCFixUnderZero.Price = 50;
            DataCollection.DiscountCode dCFixUnderZero = new DataCollection.DiscountCode("DCFixUnderZero");
            dCFixUnderZero.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCFixUnderZero.Amount = -10;
            dCFixUnderZero.CodeKind = DataCollection.FormData.ChangeType.FixedAmount;
            agendaDCFixUnderZero.DiscountCode.Add(dCFixUnderZero);
            DataCollection.AgendaItemCheckBox agendaDCPercentIncreace = new DataCollection.AgendaItemCheckBox("AgendaDCPercentIncreace");
            agendaDCPercentIncreace.Price = 50;
            DataCollection.DiscountCode dCPercentIncreace = new DataCollection.DiscountCode("DCPercentIncreace");
            dCPercentIncreace.CodeDirection = DataCollection.FormData.ChangePriceDirection.Increase;
            dCPercentIncreace.Amount = 10;
            dCPercentIncreace.CodeKind = DataCollection.FormData.ChangeType.Percent;
            agendaDCPercentIncreace.DiscountCode.Add(dCPercentIncreace);
            DataCollection.AgendaItemCheckBox agendaDCFixIncreace = new DataCollection.AgendaItemCheckBox("AgendaDCFixIncreace");
            agendaDCFixIncreace.Price = 50;
            DataCollection.DiscountCode dCFixIncreace = new DataCollection.DiscountCode("DCFixIncreace");
            dCFixIncreace.CodeDirection = DataCollection.FormData.ChangePriceDirection.Increase;
            dCFixIncreace.Amount = 10;
            dCFixIncreace.CodeKind = DataCollection.FormData.ChangeType.FixedAmount;
            agendaDCFixIncreace.DiscountCode.Add(dCFixIncreace);
            DataCollection.AgendaItemCheckBox agendaDCPercentIncreaceUnderZero = new DataCollection.AgendaItemCheckBox("AgendaDCPercentIncreaceUnderZero");
            agendaDCPercentIncreaceUnderZero.Price = 50;
            DataCollection.DiscountCode dCPercentIncreaceUnderZero = new DataCollection.DiscountCode("PercentIncreace0");
            dCPercentIncreaceUnderZero.CodeDirection = DataCollection.FormData.ChangePriceDirection.Increase;
            dCPercentIncreaceUnderZero.Amount = -10;
            dCPercentIncreaceUnderZero.CodeKind = DataCollection.FormData.ChangeType.Percent;
            agendaDCPercentIncreaceUnderZero.DiscountCode.Add(dCPercentIncreaceUnderZero);
            DataCollection.AgendaItemCheckBox agendaDCFixIncreaceUnderZero = new DataCollection.AgendaItemCheckBox("AgendaDCFixIncreaceUnderZero");
            agendaDCFixIncreaceUnderZero.Price = 50;
            DataCollection.DiscountCode dCFixIncreaceUnderZero = new DataCollection.DiscountCode("FixIncreace0");
            dCFixIncreaceUnderZero.CodeDirection = DataCollection.FormData.ChangePriceDirection.Increase;
            dCFixIncreaceUnderZero.Amount = -10;
            dCFixIncreaceUnderZero.CodeKind = DataCollection.FormData.ChangeType.FixedAmount;
            agendaDCFixIncreaceUnderZero.DiscountCode.Add(dCFixIncreaceUnderZero);
            DataCollection.AgendaItemCheckBox agendaDCWithLimit = new DataCollection.AgendaItemCheckBox("AgendaDCWithLimit");
            agendaDCWithLimit.Price = 50;
            DataCollection.DiscountCode dCWithLimit = new DataCollection.DiscountCode("DCWithLimit");
            dCWithLimit.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCWithLimit.Amount = 10;
            dCWithLimit.CodeKind = DataCollection.FormData.ChangeType.Percent;
            dCWithLimit.Limit = 1;
            agendaDCWithLimit.DiscountCode.Add(dCWithLimit);
            DataCollection.AgendaItemCheckBox agendaDCRequired = new DataCollection.AgendaItemCheckBox("AgendaDCRequired");
            agendaDCRequired.Price = 50;
            DataCollection.DiscountCode dCRequired = new DataCollection.DiscountCode("DCRequired");
            dCRequired.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCRequired.Amount = 10;
            dCRequired.CodeKind = DataCollection.FormData.ChangeType.Percent;
            agendaDCRequired.DiscountCode.Add(dCRequired);
            agendaDCRequired.RequireDC = true;
            evt.AgendaPage.AgendaItems.Add(agendaDCPercent);
            evt.AgendaPage.AgendaItems.Add(agendaDCFix);
            evt.AgendaPage.AgendaItems.Add(agendaDCPercentUnderZero);
            evt.AgendaPage.AgendaItems.Add(agendaDCFixUnderZero);
            evt.AgendaPage.AgendaItems.Add(agendaDCPercentIncreace);
            evt.AgendaPage.AgendaItems.Add(agendaDCFixIncreace);
            evt.AgendaPage.AgendaItems.Add(agendaDCPercentIncreaceUnderZero);
            evt.AgendaPage.AgendaItems.Add(agendaDCFixIncreaceUnderZero);
            evt.AgendaPage.AgendaItems.Add(agendaDCWithLimit);
            evt.AgendaPage.AgendaItems.Add(agendaDCRequired);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.RegistrationInventory, evt);

            DataCollection.Registrant reg = new DataCollection.Registrant();
            reg.Event = evt;
            reg.PaymentMethod = paymentMethod;
            DataCollection.AgendaCheckboxResponse resp1 = new DataCollection.AgendaCheckboxResponse();
            resp1.AgendaItem = agendaDCPercent;
            resp1.Checked = true;
            resp1.Code = dCPercent;
            DataCollection.AgendaCheckboxResponse resp2 = new DataCollection.AgendaCheckboxResponse();
            resp2.AgendaItem = agendaDCFix;
            resp2.Checked = true;
            resp2.Code = dCFix;
            DataCollection.AgendaCheckboxResponse resp3 = new DataCollection.AgendaCheckboxResponse();
            resp3.AgendaItem = agendaDCPercentUnderZero;
            resp3.Checked = true;
            resp3.Code = dCPercentUnderZero;
            DataCollection.AgendaCheckboxResponse resp4 = new DataCollection.AgendaCheckboxResponse();
            resp4.AgendaItem = agendaDCFixUnderZero;
            resp4.Checked = true;
            resp4.Code = dCFixUnderZero;
            DataCollection.AgendaCheckboxResponse resp5 = new DataCollection.AgendaCheckboxResponse();
            resp5.AgendaItem = agendaDCPercentIncreace;
            resp5.Checked = true;
            resp5.Code = dCPercentIncreace;
            DataCollection.AgendaCheckboxResponse resp6 = new DataCollection.AgendaCheckboxResponse();
            resp6.AgendaItem = agendaDCFixIncreace;
            resp6.Checked = true;
            resp6.Code = dCFixIncreace;
            DataCollection.AgendaCheckboxResponse resp7 = new DataCollection.AgendaCheckboxResponse();
            resp7.AgendaItem = agendaDCPercentIncreaceUnderZero;
            resp7.Checked = true;
            resp7.Code = dCPercentIncreaceUnderZero;
            DataCollection.AgendaCheckboxResponse resp8 = new DataCollection.AgendaCheckboxResponse();
            resp8.AgendaItem = agendaDCFixIncreaceUnderZero;
            resp8.Checked = true;
            resp8.Code = dCFixIncreaceUnderZero;
            DataCollection.AgendaCheckboxResponse resp9 = new DataCollection.AgendaCheckboxResponse();
            resp9.AgendaItem = agendaDCWithLimit;
            resp9.Checked = true;
            resp9.Code = dCWithLimit;
            DataCollection.AgendaCheckboxResponse resp10 = new DataCollection.AgendaCheckboxResponse();
            resp10.AgendaItem = agendaDCRequired;
            resp10.Checked = true;
            reg.CustomFieldResponses.Add(resp1);
            reg.CustomFieldResponses.Add(resp2);
            reg.CustomFieldResponses.Add(resp3);
            reg.CustomFieldResponses.Add(resp4);
            reg.CustomFieldResponses.Add(resp5);
            reg.CustomFieldResponses.Add(resp6);
            reg.CustomFieldResponses.Add(resp7);
            reg.CustomFieldResponses.Add(resp8);
            reg.CustomFieldResponses.Add(resp9);
            reg.CustomFieldResponses.Add(resp10);

            KeywordProvider.RegistrationCreation.Checkin(reg);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            KeywordProvider.RegistrationCreation.Agenda(reg);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(DataCollection.Messages.RegisterError.DiscountCodeNotFilled));
            PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(agendaDCRequired).DiscountCodeInput.Type(dCRequired.Code);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.Checkout(reg);
            double expectedTotal = agendaDCPercent.Price.Value * 0.9 + (agendaDCFix.Price.Value - 10) + agendaDCPercentUnderZero.Price.Value
                + agendaDCFixUnderZero.Price.Value + agendaDCPercentIncreace.Price.Value * 1.1 + (agendaDCFixIncreace.Price.Value + 10)
                + agendaDCPercentIncreaceUnderZero.Price.Value + agendaDCFixIncreaceUnderZero.Price.Value + agendaDCWithLimit.Price.Value * 0.9
                + agendaDCRequired.Price.Value * 0.9;
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetConfirmationTotal(), expectedTotal);

            DataCollection.Registrant reg1 = new DataCollection.Registrant();
            reg1.Event = evt;
            DataCollection.AgendaCheckboxResponse resp11 = new DataCollection.AgendaCheckboxResponse();
            resp11.AgendaItem = agendaDCWithLimit;
            resp11.Checked = true;
            resp11.Code = dCWithLimit;
            reg1.CustomFieldResponses.Add(resp11);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            KeywordProvider.RegistrationCreation.Agenda(reg1);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(DataCollection.Messages.RegisterError.AgendaCodeLimitReached));
        }                      
    }                          
}
