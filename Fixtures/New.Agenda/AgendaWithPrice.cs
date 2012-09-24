﻿namespace RegOnline.RegressionTest.Fixtures.New.Agenda
{
    using System.Collections.Generic;
    using Fixtures.Base;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgendaWithPrice : FixtureBase
    {
        [Test]
        [Category(Priority.Three)]
        [Description("1362")]
        public void AgendaPrice()
        {
            DataCollection.Event evt = new DataCollection.Event("AgendaPrice");
            evt.AgendaPage = new DataCollection.AgendaPage();
            DataCollection.AgendaItem_CheckBox aGStandardPrice = new DataCollection.AgendaItem_CheckBox("AGStandardPrice");
            aGStandardPrice.Price = 50;
            DataCollection.AgendaItem_CheckBox aGEarlyPriceDateTime = new DataCollection.AgendaItem_CheckBox("AGEarlyPriceDateTime");
            aGEarlyPriceDateTime.Price = 110;
            DataCollection.EarlyPrice earlyPrice1 = new DataCollection.EarlyPrice();
            earlyPrice1.earlyPrice = 90;
            earlyPrice1.EarlyPriceType = DataCollection.FormData.EarlyPriceType.DateAndTime;
            earlyPrice1.EarlyPriceDate = System.DateTime.Today.AddDays(3);
            earlyPrice1.EarlyPriceTime = System.DateTime.Now;
            aGEarlyPriceDateTime.EarlyPrice = earlyPrice1;
            DataCollection.AgendaItem_CheckBox aGEarlyPriceRegs = new DataCollection.AgendaItem_CheckBox("AGEarlyPriceRegs");
            aGEarlyPriceRegs.Price = 150;
            DataCollection.EarlyPrice earlyPrice2 = new DataCollection.EarlyPrice();
            earlyPrice2.earlyPrice = 140;
            earlyPrice2.EarlyPriceType = DataCollection.FormData.EarlyPriceType.Registrants;
            earlyPrice2.FirstNRegistrants = 1;
            aGEarlyPriceRegs.EarlyPrice = earlyPrice2;
            DataCollection.AgendaItem_CheckBox aGLatePrice = new DataCollection.AgendaItem_CheckBox("AGLatePrice");
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

            DataCollection.Registrant reg1 = new DataCollection.Registrant(evt);
            DataCollection.AgendaResponse_Checkbox resp1 = new DataCollection.AgendaResponse_Checkbox();
            resp1.AgendaItem = aGStandardPrice;
            resp1.Checked = true;
            DataCollection.AgendaResponse_Checkbox resp2 = new DataCollection.AgendaResponse_Checkbox();
            resp2.AgendaItem = aGEarlyPriceDateTime;
            resp2.Checked = true;
            DataCollection.AgendaResponse_Checkbox resp3 = new DataCollection.AgendaResponse_Checkbox();
            resp3.AgendaItem = aGEarlyPriceRegs;
            resp3.Checked = true;
            DataCollection.AgendaResponse_Checkbox resp4 = new DataCollection.AgendaResponse_Checkbox();
            resp4.AgendaItem = aGLatePrice;
            resp4.Checked = true;
            reg1.CustomField_Responses.Add(resp1);
            reg1.CustomField_Responses.Add(resp2);
            reg1.CustomField_Responses.Add(resp3);
            reg1.CustomField_Responses.Add(resp4);
            reg1.Payment_Method = paymentMethod;

            KeywordProvider.RegistrationCreation.CreateRegistration(reg1);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation),
                KeywordProvider.CalculateFee.CalculateTotalFee(reg1));

            DataCollection.Registrant reg2 = new DataCollection.Registrant(evt);
            reg2.CustomField_Responses.Add(resp1);
            reg2.CustomField_Responses.Add(resp2);
            reg2.CustomField_Responses.Add(resp3);
            reg2.CustomField_Responses.Add(resp4);
            reg2.Payment_Method = paymentMethod;

            KeywordProvider.RegistrationCreation.CreateRegistration(reg2);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation),
                KeywordProvider.CalculateFee.CalculateTotalFee(reg1));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1363")]
        public void AgendaDiscountCode()
        {
            DataCollection.Event evt = new DataCollection.Event("AgendaDiscountCode");
            DataCollection.PaymentMethod paymentMethod = new DataCollection.PaymentMethod(DataCollection.FormData.PaymentMethod.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            evt.AgendaPage = new DataCollection.AgendaPage();
            DataCollection.AgendaItem_CheckBox agendaDCPercent = new DataCollection.AgendaItem_CheckBox("AgendaDCPercent");
            agendaDCPercent.Price = 50;
            DataCollection.DiscountCode dCPercent = new DataCollection.DiscountCode("DCPercent");
            dCPercent.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCPercent.Amount = 10;
            dCPercent.CodeKind = DataCollection.FormData.ChangeType.Percent;
            agendaDCPercent.DiscountCodes.Add(dCPercent);
            DataCollection.AgendaItem_CheckBox agendaDCFix = new DataCollection.AgendaItem_CheckBox("AgendaDCFix");
            agendaDCFix.Price = 50;
            DataCollection.DiscountCode dCFix = new DataCollection.DiscountCode("DCFix");
            dCFix.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCFix.Amount = 10;
            dCFix.CodeKind = DataCollection.FormData.ChangeType.FixedAmount;
            agendaDCFix.DiscountCodes.Add(dCFix);
            DataCollection.AgendaItem_CheckBox agendaDCPercentUnderZero = new DataCollection.AgendaItem_CheckBox("AgendaDCPercentUnderZero");
            agendaDCPercentUnderZero.Price = 50;
            DataCollection.DiscountCode dCPercentUnderZero = new DataCollection.DiscountCode("DCPercentUnderZero");
            dCPercentUnderZero.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCPercentUnderZero.Amount = -10;
            dCPercentUnderZero.CodeKind = DataCollection.FormData.ChangeType.Percent;
            agendaDCPercentUnderZero.DiscountCodes.Add(dCPercentUnderZero);
            DataCollection.AgendaItem_CheckBox agendaDCFixUnderZero = new DataCollection.AgendaItem_CheckBox("AgendaDCFixUnderZero");
            agendaDCFixUnderZero.Price = 50;
            DataCollection.DiscountCode dCFixUnderZero = new DataCollection.DiscountCode("DCFixUnderZero");
            dCFixUnderZero.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCFixUnderZero.Amount = -10;
            dCFixUnderZero.CodeKind = DataCollection.FormData.ChangeType.FixedAmount;
            agendaDCFixUnderZero.DiscountCodes.Add(dCFixUnderZero);
            DataCollection.AgendaItem_CheckBox agendaDCPercentIncreace = new DataCollection.AgendaItem_CheckBox("AgendaDCPercentIncreace");
            agendaDCPercentIncreace.Price = 50;
            DataCollection.DiscountCode dCPercentIncreace = new DataCollection.DiscountCode("DCPercentIncreace");
            dCPercentIncreace.CodeDirection = DataCollection.FormData.ChangePriceDirection.Increase;
            dCPercentIncreace.Amount = 10;
            dCPercentIncreace.CodeKind = DataCollection.FormData.ChangeType.Percent;
            agendaDCPercentIncreace.DiscountCodes.Add(dCPercentIncreace);
            DataCollection.AgendaItem_CheckBox agendaDCFixIncreace = new DataCollection.AgendaItem_CheckBox("AgendaDCFixIncreace");
            agendaDCFixIncreace.Price = 50;
            DataCollection.DiscountCode dCFixIncreace = new DataCollection.DiscountCode("DCFixIncreace");
            dCFixIncreace.CodeDirection = DataCollection.FormData.ChangePriceDirection.Increase;
            dCFixIncreace.Amount = 10;
            dCFixIncreace.CodeKind = DataCollection.FormData.ChangeType.FixedAmount;
            agendaDCFixIncreace.DiscountCodes.Add(dCFixIncreace);
            DataCollection.AgendaItem_CheckBox agendaDCPercentIncreaceUnderZero = new DataCollection.AgendaItem_CheckBox("AgendaDCPercentIncreaceUnderZero");
            agendaDCPercentIncreaceUnderZero.Price = 50;
            DataCollection.DiscountCode dCPercentIncreaceUnderZero = new DataCollection.DiscountCode("PercentIncreace0");
            dCPercentIncreaceUnderZero.CodeDirection = DataCollection.FormData.ChangePriceDirection.Increase;
            dCPercentIncreaceUnderZero.Amount = -10;
            dCPercentIncreaceUnderZero.CodeKind = DataCollection.FormData.ChangeType.Percent;
            agendaDCPercentIncreaceUnderZero.DiscountCodes.Add(dCPercentIncreaceUnderZero);
            DataCollection.AgendaItem_CheckBox agendaDCFixIncreaceUnderZero = new DataCollection.AgendaItem_CheckBox("AgendaDCFixIncreaceUnderZero");
            agendaDCFixIncreaceUnderZero.Price = 50;
            DataCollection.DiscountCode dCFixIncreaceUnderZero = new DataCollection.DiscountCode("FixIncreace0");
            dCFixIncreaceUnderZero.CodeDirection = DataCollection.FormData.ChangePriceDirection.Increase;
            dCFixIncreaceUnderZero.Amount = -10;
            dCFixIncreaceUnderZero.CodeKind = DataCollection.FormData.ChangeType.FixedAmount;
            agendaDCFixIncreaceUnderZero.DiscountCodes.Add(dCFixIncreaceUnderZero);
            DataCollection.AgendaItem_CheckBox agendaDCWithLimit = new DataCollection.AgendaItem_CheckBox("AgendaDCWithLimit");
            agendaDCWithLimit.Price = 50;
            DataCollection.DiscountCode dCWithLimit = new DataCollection.DiscountCode("DCWithLimit");
            dCWithLimit.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCWithLimit.Amount = 10;
            dCWithLimit.CodeKind = DataCollection.FormData.ChangeType.Percent;
            dCWithLimit.Limit = 1;
            agendaDCWithLimit.DiscountCodes.Add(dCWithLimit);
            DataCollection.AgendaItem_CheckBox agendaDCRequired = new DataCollection.AgendaItem_CheckBox("AgendaDCRequired");
            agendaDCRequired.Price = 50;
            DataCollection.DiscountCode dCRequired = new DataCollection.DiscountCode("DCRequired");
            dCRequired.CodeDirection = DataCollection.FormData.ChangePriceDirection.Decrease;
            dCRequired.Amount = 10;
            dCRequired.CodeKind = DataCollection.FormData.ChangeType.Percent;
            agendaDCRequired.DiscountCodes.Add(dCRequired);
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

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.RegistrationInventory, evt, false, true);

            DataCollection.Registrant reg = new DataCollection.Registrant(evt);
            reg.Payment_Method = paymentMethod;
            DataCollection.AgendaResponse_Checkbox resp1 = new DataCollection.AgendaResponse_Checkbox();
            resp1.AgendaItem = agendaDCPercent;
            resp1.Checked = true;
            resp1.Code = dCPercent;
            DataCollection.AgendaResponse_Checkbox resp2 = new DataCollection.AgendaResponse_Checkbox();
            resp2.AgendaItem = agendaDCFix;
            resp2.Checked = true;
            resp2.Code = dCFix;
            DataCollection.AgendaResponse_Checkbox resp3 = new DataCollection.AgendaResponse_Checkbox();
            resp3.AgendaItem = agendaDCPercentUnderZero;
            resp3.Checked = true;
            resp3.Code = dCPercentUnderZero;
            DataCollection.AgendaResponse_Checkbox resp4 = new DataCollection.AgendaResponse_Checkbox();
            resp4.AgendaItem = agendaDCFixUnderZero;
            resp4.Checked = true;
            resp4.Code = dCFixUnderZero;
            DataCollection.AgendaResponse_Checkbox resp5 = new DataCollection.AgendaResponse_Checkbox();
            resp5.AgendaItem = agendaDCPercentIncreace;
            resp5.Checked = true;
            resp5.Code = dCPercentIncreace;
            DataCollection.AgendaResponse_Checkbox resp6 = new DataCollection.AgendaResponse_Checkbox();
            resp6.AgendaItem = agendaDCFixIncreace;
            resp6.Checked = true;
            resp6.Code = dCFixIncreace;
            DataCollection.AgendaResponse_Checkbox resp7 = new DataCollection.AgendaResponse_Checkbox();
            resp7.AgendaItem = agendaDCPercentIncreaceUnderZero;
            resp7.Checked = true;
            resp7.Code = dCPercentIncreaceUnderZero;
            DataCollection.AgendaResponse_Checkbox resp8 = new DataCollection.AgendaResponse_Checkbox();
            resp8.AgendaItem = agendaDCFixIncreaceUnderZero;
            resp8.Checked = true;
            resp8.Code = dCFixIncreaceUnderZero;
            DataCollection.AgendaResponse_Checkbox resp9 = new DataCollection.AgendaResponse_Checkbox();
            resp9.AgendaItem = agendaDCWithLimit;
            resp9.Checked = true;
            resp9.Code = dCWithLimit;
            DataCollection.AgendaResponse_Checkbox resp10 = new DataCollection.AgendaResponse_Checkbox();
            resp10.AgendaItem = agendaDCRequired;
            resp10.Checked = true;
            reg.CustomField_Responses.Add(resp1);
            reg.CustomField_Responses.Add(resp2);
            reg.CustomField_Responses.Add(resp3);
            reg.CustomField_Responses.Add(resp4);
            reg.CustomField_Responses.Add(resp5);
            reg.CustomField_Responses.Add(resp6);
            reg.CustomField_Responses.Add(resp7);
            reg.CustomField_Responses.Add(resp8);
            reg.CustomField_Responses.Add(resp9);
            reg.CustomField_Responses.Add(resp10);

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
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(DataCollection.FormData.RegisterPage.Confirmation), expectedTotal);

            DataCollection.Registrant reg1 = new DataCollection.Registrant(evt);
            DataCollection.AgendaResponse_Checkbox resp11 = new DataCollection.AgendaResponse_Checkbox();
            resp11.AgendaItem = agendaDCWithLimit;
            resp11.Checked = true;
            resp11.Code = dCWithLimit;
            reg1.CustomField_Responses.Add(resp11);

            KeywordProvider.RegistrationCreation.Checkin(reg1);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg1);
            KeywordProvider.RegistrationCreation.Agenda(reg1);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(DataCollection.Messages.RegisterError.AgendaCodeLimitReached));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1364")]
        public void AgendaAccessCode()
        {
            DataCollection.Event evt = new DataCollection.Event("AgendaAccessCode");
            evt.AgendaPage = new DataCollection.AgendaPage();
            DataCollection.AgendaItem_CheckBox accessNoLimit = new DataCollection.AgendaItem_CheckBox("AccessNoLimit");
            DataCollection.DiscountCode accessCodeNoLimit = new DataCollection.DiscountCode("AccessNoLimit");
            accessCodeNoLimit.CodeType = DataCollection.FormData.DiscountCodeType.AccessCode;
            accessNoLimit.DiscountCodes.Add(accessCodeNoLimit);
            DataCollection.AgendaItem_CheckBox accessWithLimit = new DataCollection.AgendaItem_CheckBox("AccessWithLimit");
            DataCollection.DiscountCode accessCodeWithLimit = new DataCollection.DiscountCode("AccessWithLimit");
            accessCodeWithLimit.CodeType = DataCollection.FormData.DiscountCodeType.AccessCode;
            accessCodeWithLimit.Limit = 1;
            accessWithLimit.DiscountCodes.Add(accessCodeWithLimit);
            DataCollection.AgendaItem_CheckBox accessBulkCode = new DataCollection.AgendaItem_CheckBox("AccessBulkCode");
            accessBulkCode.BulkCodes = "Bulk";
            evt.AgendaPage.AgendaItems.Add(accessNoLimit);
            evt.AgendaPage.AgendaItems.Add(accessWithLimit);
            evt.AgendaPage.AgendaItems.Add(accessBulkCode);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.RegistrationInventory, evt, false, true);

            DataCollection.Registrant reg1 = new DataCollection.Registrant(evt);
            DataCollection.AgendaResponse_Checkbox resp1 = new DataCollection.AgendaResponse_Checkbox();
            resp1.AgendaItem = accessNoLimit;
            resp1.Checked = true;
            resp1.Code = accessCodeNoLimit;
            DataCollection.AgendaResponse_Checkbox resp2 = new DataCollection.AgendaResponse_Checkbox();
            resp2.AgendaItem = accessWithLimit;
            resp2.Checked = true;
            resp2.Code = accessCodeWithLimit;
            reg1.CustomField_Responses.Add(resp1);
            reg1.CustomField_Responses.Add(resp2);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg1);

            DataCollection.Registrant reg2 = new DataCollection.Registrant(evt);
            reg2.CustomField_Responses.Add(resp1);
            reg2.CustomField_Responses.Add(resp2);

            KeywordProvider.RegistrationCreation.Checkin(reg2);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg2);
            KeywordProvider.RegistrationCreation.Agenda(reg2);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(DataCollection.Messages.RegisterError.AgendaCodeLimitReached));
            PageObject.Register.AgendaRow row = new PageObject.Register.AgendaRow(accessBulkCode);
            Assert.True(row.DiscountCodeInput.IsPresent);
        }
    }
}
