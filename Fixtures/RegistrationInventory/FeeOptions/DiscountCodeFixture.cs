namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.FeeOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class DiscountCodeFixture : FixtureBase
    {
        private Event diffDcEvt;
        private PaymentMethod diffDcPaymentMethod;
        private RegType diffDcRegType;
        private AgendaItem_CheckBox diffDcAgenda;
        private MerchandiseItem diffDcMerch;

        [Test]
        [Category(Priority.Two)]
        [Description("692")]
        public void RegTypeDiscount()
        {
            Event evt = new Event("RegTypeDiscount");
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            RegType regType1 = new RegType("RegType1");
            regType1.Price = 80;
            RegType regType2 = new RegType("RegType2");
            regType2.Price = 90;
            DiscountCode dc1 = new DiscountCode("dc1");
            dc1.Amount = 10;
            dc1.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc1.CodeKind = FormData.ChangeType.Percent;
            dc1.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc2 = new DiscountCode("dc2");
            dc2.Amount = 20;
            dc2.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc2.CodeKind = FormData.ChangeType.Percent;
            dc2.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc3 = new DiscountCode("dc3");
            dc3.Amount = 30;
            dc3.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc3.CodeKind = FormData.ChangeType.Percent;
            dc3.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc4 = new DiscountCode("dc4");
            dc4.Amount = 40;
            dc4.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc4.CodeKind = FormData.ChangeType.Percent;
            dc4.CodeType = FormData.DiscountCodeType.DiscountCode;
            regType1.DiscountCode.Add(dc1);
            regType1.DiscountCode.Add(dc2);
            regType2.DiscountCode.Add(dc3);
            regType2.DiscountCode.Add(dc4);
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            Registrant reg1 = new Registrant(evt);
            reg1.Payment_Method = paymentMethod;
            reg1.EventFee_Response = new EventFeeResponse(regType1);
            reg1.EventFee_Response.Code = dc1;
            System.Threading.Thread.Sleep(10);
            Registrant reg2 = new Registrant(evt);
            reg2.EventFee_Response = new EventFeeResponse(regType2);
            reg2.EventFee_Response.Code = dc3;
            Group group1 = new Group();
            group1.Primary = reg1;
            group1.Secondaries.Add(reg2);

            KeywordProvider.RegistrationCreation.GroupRegistration(group1);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 135);

            Registrant reg3 = new Registrant(evt);
            reg3.Payment_Method = paymentMethod;
            reg3.EventFee_Response = new EventFeeResponse(regType1);
            reg3.EventFee_Response.Code = dc2;
            System.Threading.Thread.Sleep(10);
            Registrant reg4 = new Registrant(evt);
            reg4.EventFee_Response = new EventFeeResponse(regType2);
            reg4.EventFee_Response.Code = dc4;
            Group group2 = new Group();
            group2.Primary = reg3;
            group2.Secondaries.Add(reg4);

            KeywordProvider.RegistrationCreation.GroupRegistration(group2);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 118);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("539")]
        public void GroupRegWithHalfDiscount()
        {
            Event evt = new Event("GroupRegWithHalfDiscount");
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            RegType regType1 = new RegType("RegType1");
            regType1.Price = 50;
            RegType regType2 = new RegType("RegType2");
            regType2.Price = 60;
            DiscountCode dc1 = new DiscountCode("dc1");
            dc1.Amount = 50;
            dc1.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc1.CodeKind = FormData.ChangeType.Percent;
            dc1.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc2 = new DiscountCode("dc2");
            dc2.Amount = 50;
            dc2.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc2.CodeKind = FormData.ChangeType.Percent;
            dc2.CodeType = FormData.DiscountCodeType.DiscountCode;
            regType1.DiscountCode.Add(dc1);
            regType2.DiscountCode.Add(dc2);
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);
            AgendaItem_CheckBox agenda1 = new AgendaItem_CheckBox("Agenda1");
            agenda1.Price = 80;
            AgendaItem_CheckBox agenda2 = new AgendaItem_CheckBox("Agenda2");
            agenda2.Price = 100;
            DiscountCode dc3 = new DiscountCode("dc3");
            dc3.Amount = 50;
            dc3.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc3.CodeKind = FormData.ChangeType.Percent;
            dc3.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc4 = new DiscountCode("dc4");
            dc4.Amount = 50;
            dc4.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc4.CodeKind = FormData.ChangeType.Percent;
            dc4.CodeType = FormData.DiscountCodeType.DiscountCode;
            agenda1.DiscountCodes.Add(dc3);
            agenda2.DiscountCodes.Add(dc4);
            evt.AgendaPage = new AgendaPage();
            evt.AgendaPage.AgendaItems.Add(agenda1);
            evt.AgendaPage.AgendaItems.Add(agenda2);
            MerchandiseItem merch1 = new MerchandiseItem("Merch1");
            merch1.Type = FormData.MerchandiseType.Fixed;
            merch1.Price = 110;
            MerchandiseItem merch2 = new MerchandiseItem("Merch2");
            merch2.Type = FormData.MerchandiseType.Fixed;
            merch2.Price = 150;
            DiscountCode dc5 = new DiscountCode("dc5");
            dc5.Amount = 50;
            dc5.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc5.CodeKind = FormData.ChangeType.Percent;
            dc5.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc6 = new DiscountCode("dc6");
            dc6.Amount = 50;
            dc6.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc6.CodeKind = FormData.ChangeType.Percent;
            dc6.CodeType = FormData.DiscountCodeType.DiscountCode;
            merch1.DiscountCodes.Add(dc5);
            merch2.DiscountCodes.Add(dc6);
            evt.MerchandisePage = new MerchandisePage();
            evt.MerchandisePage.Merchandises.Add(merch1);
            evt.MerchandisePage.Merchandises.Add(merch2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            EventFeeResponse resp1 = new EventFeeResponse(regType1);
            resp1.Code = dc1;
            EventFeeResponse resp2 = new EventFeeResponse(regType2);
            resp2.Code = dc2;
            AgendaResponse_Checkbox resp3 = new AgendaResponse_Checkbox();
            resp3.AgendaItem = agenda1;
            resp3.Checked = true;
            resp3.Code = dc3;
            AgendaResponse_Checkbox resp4 = new AgendaResponse_Checkbox();
            resp4.AgendaItem = agenda2;
            resp4.Checked = true;
            resp4.Code = dc4;
            MerchResponse_FixedPrice resp5 = new MerchResponse_FixedPrice();
            resp5.Merchandise_Item = merch1;
            resp5.Quantity = 1;
            resp5.Discount_Code = dc5;
            MerchResponse_FixedPrice resp6 = new MerchResponse_FixedPrice();
            resp6.Merchandise_Item = merch2;
            resp6.Quantity = 1;
            resp6.Discount_Code = dc6;

            Registrant reg1 = new Registrant(evt);
            reg1.Payment_Method = paymentMethod;
            reg1.EventFee_Response = resp1;
            reg1.CustomField_Responses.Add(resp3);
            reg1.CustomField_Responses.Add(resp4);
            reg1.Merchandise_Responses.Add(resp5);
            reg1.Merchandise_Responses.Add(resp6);
            System.Threading.Thread.Sleep(10);
            Registrant reg2 = new Registrant(evt);
            reg2.EventFee_Response = resp2;
            reg2.CustomField_Responses.Add(resp3);
            reg2.CustomField_Responses.Add(resp4);
            System.Threading.Thread.Sleep(10);
            Registrant reg3 = new Registrant(evt);
            reg3.EventFee_Response = resp1;
            reg3.CustomField_Responses.Add(resp3);
            Group group = new Group();
            group.Primary = reg1;
            group.Secondaries.Add(reg2);
            group.Secondaries.Add(reg3);

            KeywordProvider.RegistrationCreation.GroupRegistration(group);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 430);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("540")]
        public void DifferentDiscountCode()
        {
            diffDcEvt = new Event("DifferentDiscountCode");
            diffDcPaymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            diffDcEvt.CheckoutPage.PaymentMethods.Add(diffDcPaymentMethod);
            diffDcRegType = new RegType("RegType");
            diffDcRegType.Price = 50;
            DiscountCode dc1 = new DiscountCode("free");
            dc1.Amount = 100;
            dc1.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc1.CodeKind = FormData.ChangeType.Percent;
            dc1.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc2 = new DiscountCode("half");
            dc2.Amount = 50;
            dc2.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc2.CodeKind = FormData.ChangeType.Percent;
            dc2.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc3 = new DiscountCode("fixAmount");
            dc3.Amount = 10;
            dc3.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc3.CodeKind = FormData.ChangeType.FixedAmount;
            dc3.CodeType = FormData.DiscountCodeType.DiscountCode;
            diffDcRegType.DiscountCode.Add(dc1);
            diffDcRegType.DiscountCode.Add(dc2);
            diffDcRegType.DiscountCode.Add(dc3);
            diffDcEvt.StartPage.RegTypes.Add(diffDcRegType);
            diffDcAgenda = new AgendaItem_CheckBox("Agenda");
            diffDcAgenda.Price = 80;
            DiscountCode dc4 = new DiscountCode("free");
            dc4.Amount = 100;
            dc4.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc4.CodeKind = FormData.ChangeType.Percent;
            dc4.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc5 = new DiscountCode("half");
            dc5.Amount = 50;
            dc5.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc5.CodeKind = FormData.ChangeType.Percent;
            dc5.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc6 = new DiscountCode("fixAmount");
            dc6.Amount = 10;
            dc6.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc6.CodeKind = FormData.ChangeType.FixedAmount;
            dc6.CodeType = FormData.DiscountCodeType.DiscountCode;
            diffDcAgenda.DiscountCodes.Add(dc4);
            diffDcAgenda.DiscountCodes.Add(dc5);
            diffDcAgenda.DiscountCodes.Add(dc6);
            diffDcEvt.AgendaPage = new AgendaPage();
            diffDcEvt.AgendaPage.AgendaItems.Add(diffDcAgenda);
            diffDcMerch = new MerchandiseItem("Merch");
            diffDcMerch.Price = 90;
            diffDcMerch.Type = FormData.MerchandiseType.Fixed;
            DiscountCode dc7 = new DiscountCode("free");
            dc7.Amount = 100;
            dc7.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc7.CodeKind = FormData.ChangeType.Percent;
            dc7.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc8 = new DiscountCode("half");
            dc8.Amount = 50;
            dc8.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc8.CodeKind = FormData.ChangeType.Percent;
            dc8.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc9 = new DiscountCode("fixAmount");
            dc9.Amount = 10;
            dc9.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc9.CodeKind = FormData.ChangeType.FixedAmount;
            dc9.CodeType = FormData.DiscountCodeType.DiscountCode;
            diffDcMerch.DiscountCodes.Add(dc7);
            diffDcMerch.DiscountCodes.Add(dc8);
            diffDcMerch.DiscountCodes.Add(dc9);
            diffDcEvt.MerchandisePage = new MerchandisePage();
            diffDcEvt.MerchandisePage.Merchandises.Add(diffDcMerch);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, diffDcEvt);

            this.GenerateRegDiffDc(dc1, dc5, dc9);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 120);
            this.GenerateRegDiffDc(dc3, dc4, dc8);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 85);
            this.GenerateRegDiffDc(dc2, dc6, dc7);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 95);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("541")]
        public void DiscountCodeLimitTest()
        {
            Event evt = new Event("DiscountCodeLimitTest");
            PaymentMethod paymenMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymenMethod);
            RegType regType = new RegType("RegType");
            regType.Price = 50;
            DiscountCode dc1 = new DiscountCode("dc1");
            dc1.Amount = 100;
            dc1.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc1.CodeKind = FormData.ChangeType.Percent;
            dc1.CodeType = FormData.DiscountCodeType.DiscountCode;
            dc1.Limit = 2;
            DiscountCode dc2 = new DiscountCode("dc2");
            dc2.Amount = 50;
            dc2.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc2.CodeKind = FormData.ChangeType.Percent;
            dc2.CodeType = FormData.DiscountCodeType.DiscountCode;
            regType.DiscountCode.Add(dc1);
            regType.DiscountCode.Add(dc2);
            evt.StartPage.RegTypes.Add(regType);
            AgendaItem_CheckBox agenda = new AgendaItem_CheckBox("Agenda");
            agenda.Price = 60;
            DiscountCode dc3 = new DiscountCode("dc3");
            dc3.Amount = 100;
            dc3.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc3.CodeKind = FormData.ChangeType.Percent;
            dc3.CodeType = FormData.DiscountCodeType.DiscountCode;
            dc3.Limit = 2;
            DiscountCode dc4 = new DiscountCode("dc4");
            dc4.Amount = 50;
            dc4.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc4.CodeKind = FormData.ChangeType.Percent;
            dc4.CodeType = FormData.DiscountCodeType.DiscountCode;
            agenda.DiscountCodes.Add(dc3);
            agenda.DiscountCodes.Add(dc4);
            evt.AgendaPage = new AgendaPage();
            evt.AgendaPage.AgendaItems.Add(agenda);
            MerchandiseItem merchandise = new MerchandiseItem("Merchandise");
            merchandise.Price = 80;
            merchandise.Type = FormData.MerchandiseType.Fixed;
            DiscountCode dc5 = new DiscountCode("dc5");
            dc5.Amount = 100;
            dc5.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc5.CodeKind = FormData.ChangeType.Percent;
            dc5.CodeType = FormData.DiscountCodeType.DiscountCode;
            merchandise.DiscountCodes.Add(dc5);
            evt.MerchandisePage = new MerchandisePage();
            evt.MerchandisePage.Merchandises.Add(merchandise);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            EventFeeResponse resp1 = new EventFeeResponse(regType);
            resp1.Code = dc1;
            EventFeeResponse resp2 = new EventFeeResponse(regType);
            resp2.Code = dc2;
            AgendaResponse_Checkbox resp3 = new AgendaResponse_Checkbox();
            resp3.AgendaItem = agenda;
            resp3.Checked = true;
            resp3.Code = dc3;
            AgendaResponse_Checkbox resp4 = new AgendaResponse_Checkbox();
            resp4.AgendaItem = agenda;
            resp4.Checked = true;
            resp4.Code = dc4;
            MerchResponse_FixedPrice resp5 = new MerchResponse_FixedPrice();
            resp5.Merchandise_Item = merchandise;
            resp5.Quantity = 1;
            resp5.Discount_Code = dc5;

            Registrant reg1 = new Registrant(evt);
            reg1.EventFee_Response = resp1;
            reg1.CustomField_Responses.Add(resp3);
            reg1.Merchandise_Responses.Add(resp5);
            System.Threading.Thread.Sleep(10);
            Registrant reg2 = new Registrant(evt);
            reg2.EventFee_Response = resp1;
            reg2.CustomField_Responses.Add(resp3);
            reg2.Merchandise_Responses.Add(resp5);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg1);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 0);
            KeywordProvider.RegistrationCreation.CreateRegistration(reg2);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 0);

            Registrant reg3 = new Registrant(evt);
            reg3.EventFee_Response = resp1;

            KeywordProvider.RegistrationCreation.Checkin(reg3);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(string.Format(Messages.RegisterError.RegTypeCodeLimitHasReached, dc1.Code)));

            Registrant reg4 = new Registrant(evt);
            reg4.EventFee_Response = resp2;
            reg4.CustomField_Responses.Add(resp3);

            KeywordProvider.RegistrationCreation.Checkin(reg4);
            KeywordProvider.RegistrationCreation.PersonalInfo(reg4);
            KeywordProvider.RegistrationCreation.Agenda(reg4);
            Assert.True(KeywordProvider.RegisterDefault.HasErrorMessage(Messages.RegisterError.AgendaCodeLimitReached));

            Registrant reg5 = new Registrant(evt);
            reg5.Payment_Method = paymenMethod;
            reg5.EventFee_Response = resp2;
            reg5.CustomField_Responses.Add(resp4);
            reg5.Merchandise_Responses.Add(resp5);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg5);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 55);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("691")]
        public void SameDiscountCode()
        {
            Event evt = new Event("DiscountCodeFixture_SameDiscountCode");

            DiscountCode half = new DiscountCode("Half");
            half.Amount = 50;
            half.CodeDirection = FormData.ChangePriceDirection.Decrease;
            half.CodeKind = FormData.ChangeType.Percent;
            half.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode fixedAmount = new DiscountCode("FixedAmount");
            fixedAmount.Amount = 25;
            fixedAmount.CodeDirection = FormData.ChangePriceDirection.Decrease;
            fixedAmount.CodeKind = FormData.ChangeType.FixedAmount;
            fixedAmount.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode enter = new DiscountCode("Enter");
            enter.CodeType = FormData.DiscountCodeType.AccessCode;
            DiscountCode free = new DiscountCode("Free");
            free.Amount = 100;
            free.CodeDirection = FormData.ChangePriceDirection.Decrease;
            free.CodeKind = FormData.ChangeType.Percent;
            free.CodeType = FormData.DiscountCodeType.DiscountCode;

            evt.StartPage.Event_Fee = new EventFee();
            evt.StartPage.Event_Fee.StandardPrice = 100;
            evt.StartPage.Event_Fee.DiscountCodes.Add(half);
            evt.StartPage.Event_Fee.DiscountCodes.Add(fixedAmount);
            evt.StartPage.Event_Fee.DiscountCodes.Add(enter);
            evt.StartPage.Event_Fee.DiscountCodes.Add(free);

            DataCollection.AgendaItem_CheckBox checkbox = new AgendaItem_CheckBox("Checkbox");
            checkbox.Price = 50;
            checkbox.DiscountCodes.Add(half);
            checkbox.DiscountCodes.Add(fixedAmount);
            checkbox.DiscountCodes.Add(enter);
            checkbox.DiscountCodes.Add(free);

            DataCollection.AgendaItem_AlwaysSelected alwaysSelected = new AgendaItem_AlwaysSelected("AlwaysSelected");
            alwaysSelected.Price = 50;
            alwaysSelected.DiscountCodes.Add(half);
            alwaysSelected.DiscountCodes.Add(fixedAmount);
            alwaysSelected.DiscountCodes.Add(enter);
            alwaysSelected.DiscountCodes.Add(free);

            DataCollection.AgendaItem_CheckBox checkboxAndRequireDC = new AgendaItem_CheckBox("CheckboxAndRequireDC");
            checkboxAndRequireDC.Price = 50;
            checkboxAndRequireDC.DiscountCodes.Add(half);
            checkboxAndRequireDC.DiscountCodes.Add(fixedAmount);
            checkboxAndRequireDC.DiscountCodes.Add(enter);
            checkboxAndRequireDC.DiscountCodes.Add(free);
            checkboxAndRequireDC.RequireDC = true;

            DataCollection.AgendaItem_MultipleChoice_RadioButton multipleChoiceRadioButton = new AgendaItem_MultipleChoice_RadioButton("MultipleChoiceRadioButton");
            multipleChoiceRadioButton.Price = 50;
            multipleChoiceRadioButton.DiscountCodes.Add(half);
            multipleChoiceRadioButton.DiscountCodes.Add(fixedAmount);
            multipleChoiceRadioButton.DiscountCodes.Add(enter);
            multipleChoiceRadioButton.DiscountCodes.Add(free);
            DataCollection.ChoiceItem item1 = new ChoiceItem("Item1");
            item1.Price = 5;
            DataCollection.ChoiceItem item2 = new ChoiceItem("Item2");
            item2.Price = 5;
            multipleChoiceRadioButton.ChoiceItems.Add(item1);
            multipleChoiceRadioButton.ChoiceItems.Add(item2);

            evt.AgendaPage = new AgendaPage();
            evt.AgendaPage.AgendaItems.Add(checkbox);
            evt.AgendaPage.AgendaItems.Add(alwaysSelected);
            evt.AgendaPage.AgendaItems.Add(checkboxAndRequireDC);
            evt.AgendaPage.AgendaItems.Add(multipleChoiceRadioButton);

            DataCollection.MerchandiseItem merch = new MerchandiseItem("MerchWithFixedPrice");
            merch.Type = FormData.MerchandiseType.Fixed;
            merch.Price = 25;
            merch.DiscountCodes.Add(half);
            merch.DiscountCodes.Add(fixedAmount);
            merch.DiscountCodes.Add(enter);
            merch.DiscountCodes.Add(free);

            evt.MerchandisePage = new MerchandisePage();
            evt.MerchandisePage.Merchandises.Add(merch);

            evt.CheckoutPage.PaymentMethods.Add(new PaymentMethod(FormData.PaymentMethod.Check));

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            // First registration: no code
            DataCollection.EventFeeResponse evtFeeResponse = new EventFeeResponse();
            evtFeeResponse.Fee = evt.StartPage.Event_Fee.StandardPrice;

            DataCollection.AgendaResponse_Checkbox agendaResponse_Checkbox = new AgendaResponse_Checkbox();
            agendaResponse_Checkbox.AgendaItem = checkbox;
            agendaResponse_Checkbox.Checked = true;
            agendaResponse_Checkbox.Fee = checkbox.Price.Value;

            DataCollection.AgendaResponse_AlwaysSelected agendaResponse_AlwaysSelected = new AgendaResponse_AlwaysSelected();
            agendaResponse_AlwaysSelected.AgendaItem = alwaysSelected;
            agendaResponse_AlwaysSelected.Fee = alwaysSelected.Price.Value;

            DataCollection.AgendaResponse_Checkbox agendaResponse_CheckboxAndRequireDC = new AgendaResponse_Checkbox();
            agendaResponse_CheckboxAndRequireDC.AgendaItem = checkboxAndRequireDC;
            agendaResponse_CheckboxAndRequireDC.Checked = true;
            agendaResponse_CheckboxAndRequireDC.Fee = checkboxAndRequireDC.Price.Value;
            agendaResponse_CheckboxAndRequireDC.Code = enter;

            DataCollection.AgendaResponse_MultipleChoice_RadioButton agendaResponse_MultiChoiceRadioButton = new AgendaResponse_MultipleChoice_RadioButton();
            agendaResponse_MultiChoiceRadioButton.AgendaItem = multipleChoiceRadioButton;
            agendaResponse_MultiChoiceRadioButton.ChoiceItem = item1;
            agendaResponse_MultiChoiceRadioButton.Fee = item1.Price.Value;

            DataCollection.MerchResponse_FixedPrice merchandiseResponse_FixedPrice = new MerchResponse_FixedPrice();
            merchandiseResponse_FixedPrice.Merchandise_Item = merch;
            merchandiseResponse_FixedPrice.Quantity = 2;

            DataCollection.Registrant reg_NoCodes = new Registrant(evt);
            reg_NoCodes.EventFee_Response = evtFeeResponse;
            reg_NoCodes.CustomField_Responses.Add(agendaResponse_Checkbox);
            reg_NoCodes.CustomField_Responses.Add(agendaResponse_AlwaysSelected);
            reg_NoCodes.CustomField_Responses.Add(agendaResponse_CheckboxAndRequireDC);
            reg_NoCodes.CustomField_Responses.Add(agendaResponse_MultiChoiceRadioButton);
            reg_NoCodes.Merchandise_Responses.Add(merchandiseResponse_FixedPrice);
            reg_NoCodes.WhetherToVerifyFeeOnCheckoutPage = true;
            reg_NoCodes.Payment_Method = new PaymentMethod(FormData.PaymentMethod.Check);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg_NoCodes);

            // Second registration: discount code - half
            evtFeeResponse.Code = half;
            agendaResponse_Checkbox.Code = half;
            agendaResponse_AlwaysSelected.Code = half;
            agendaResponse_CheckboxAndRequireDC.Code = half;
            agendaResponse_MultiChoiceRadioButton.Code = half;
            merchandiseResponse_FixedPrice.Discount_Code = half;

            DataCollection.Registrant reg_Code_Half = new Registrant(evt);
            reg_Code_Half.EventFee_Response = evtFeeResponse;
            reg_Code_Half.CustomField_Responses.Add(agendaResponse_Checkbox);
            reg_Code_Half.CustomField_Responses.Add(agendaResponse_AlwaysSelected);
            reg_Code_Half.CustomField_Responses.Add(agendaResponse_CheckboxAndRequireDC);
            reg_Code_Half.CustomField_Responses.Add(agendaResponse_MultiChoiceRadioButton);
            reg_Code_Half.Merchandise_Responses.Add(merchandiseResponse_FixedPrice);
            reg_Code_Half.WhetherToVerifyFeeOnCheckoutPage = true;
            reg_Code_Half.Payment_Method = new PaymentMethod(FormData.PaymentMethod.Check);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg_Code_Half);

            // Third registration: discount code - free
            evtFeeResponse.Code = free;
            agendaResponse_Checkbox.Code = free;
            agendaResponse_AlwaysSelected.Code = free;
            agendaResponse_CheckboxAndRequireDC.Code = free;
            agendaResponse_MultiChoiceRadioButton.Code = free;
            merchandiseResponse_FixedPrice.Discount_Code = free;

            DataCollection.Registrant reg_Code_Free = new Registrant(evt);
            reg_Code_Free.EventFee_Response = evtFeeResponse;
            reg_Code_Free.CustomField_Responses.Add(agendaResponse_Checkbox);
            reg_Code_Free.CustomField_Responses.Add(agendaResponse_AlwaysSelected);
            reg_Code_Free.CustomField_Responses.Add(agendaResponse_CheckboxAndRequireDC);
            reg_Code_Free.CustomField_Responses.Add(agendaResponse_MultiChoiceRadioButton);
            reg_Code_Free.Merchandise_Responses.Add(merchandiseResponse_FixedPrice);
            reg_Code_Free.WhetherToVerifyFeeOnCheckoutPage = true;
            reg_Code_Free.Payment_Method = new PaymentMethod(FormData.PaymentMethod.Check);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg_Code_Free);

            // Fourth registration: discount code - fixed amount
            evtFeeResponse.Code = fixedAmount;
            agendaResponse_Checkbox.Code = fixedAmount;
            agendaResponse_AlwaysSelected.Code = fixedAmount;
            agendaResponse_CheckboxAndRequireDC.Code = fixedAmount;
            agendaResponse_MultiChoiceRadioButton.Code = fixedAmount;
            merchandiseResponse_FixedPrice.Discount_Code = fixedAmount;

            DataCollection.Registrant reg_Code_FixedAmount = new Registrant(evt);
            reg_Code_FixedAmount.EventFee_Response = evtFeeResponse;
            reg_Code_FixedAmount.CustomField_Responses.Add(agendaResponse_Checkbox);
            reg_Code_FixedAmount.CustomField_Responses.Add(agendaResponse_AlwaysSelected);
            reg_Code_FixedAmount.CustomField_Responses.Add(agendaResponse_CheckboxAndRequireDC);
            reg_Code_FixedAmount.CustomField_Responses.Add(agendaResponse_MultiChoiceRadioButton);
            reg_Code_FixedAmount.Merchandise_Responses.Add(merchandiseResponse_FixedPrice);
            reg_Code_FixedAmount.WhetherToVerifyFeeOnCheckoutPage = true;
            reg_Code_FixedAmount.Payment_Method = new PaymentMethod(FormData.PaymentMethod.Check);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg_Code_FixedAmount);

            // Fifth registration: access code - enter
            evtFeeResponse.Code = enter;
            agendaResponse_Checkbox.Code = enter;
            agendaResponse_AlwaysSelected.Code = enter;
            agendaResponse_CheckboxAndRequireDC.Code = enter;
            agendaResponse_MultiChoiceRadioButton.Code = enter;
            merchandiseResponse_FixedPrice.Discount_Code = enter;

            DataCollection.Registrant reg_Code_AccessCode = new Registrant(evt);
            reg_Code_AccessCode.EventFee_Response = evtFeeResponse;
            reg_Code_AccessCode.CustomField_Responses.Add(agendaResponse_Checkbox);
            reg_Code_AccessCode.CustomField_Responses.Add(agendaResponse_AlwaysSelected);
            reg_Code_AccessCode.CustomField_Responses.Add(agendaResponse_CheckboxAndRequireDC);
            reg_Code_AccessCode.CustomField_Responses.Add(agendaResponse_MultiChoiceRadioButton);
            reg_Code_AccessCode.Merchandise_Responses.Add(merchandiseResponse_FixedPrice);
            reg_Code_AccessCode.WhetherToVerifyFeeOnCheckoutPage = true;
            reg_Code_AccessCode.Payment_Method = new PaymentMethod(FormData.PaymentMethod.Check);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg_Code_AccessCode);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("1371")]
        public void DiscountCodeSetting()
        {
            Event evt = new Event("DiscountCodeSetting");
            PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            RegType regType = new RegType("RegType");
            regType.Price = 50;
            DiscountCode dCFix = new DiscountCode("DCFix");
            dCFix.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dCFix.Amount = 10;
            dCFix.CodeKind = FormData.ChangeType.FixedAmount;
            DiscountCode dCPercentUnderZero = new DiscountCode("DCPercentUnderZero");
            dCPercentUnderZero.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dCPercentUnderZero.Amount = -10;
            dCPercentUnderZero.CodeKind = FormData.ChangeType.Percent;
            regType.DiscountCode.Add(dCFix);
            regType.DiscountCode.Add(dCPercentUnderZero);
            evt.StartPage.RegTypes.Add(regType);

            AgendaItem_CheckBox agendaDCFixUnderZero = new AgendaItem_CheckBox("AgendaDCFixUnderZero");
            agendaDCFixUnderZero.Price = 60;
            DiscountCode dCFixUnderZero = new DiscountCode("DCFixUnderZero");
            dCFixUnderZero.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dCFixUnderZero.Amount = -10;
            dCFixUnderZero.CodeKind = FormData.ChangeType.FixedAmount;
            agendaDCFixUnderZero.DiscountCodes.Add(dCFixUnderZero);
            AgendaItem_CheckBox agendaDCPercentIncreace = new AgendaItem_CheckBox("AgendaDCPercentIncreace");
            agendaDCPercentIncreace.Price = 80;
            DiscountCode dCPercentIncreace = new DiscountCode("DCPercentIncreace");
            dCPercentIncreace.CodeDirection = FormData.ChangePriceDirection.Increase;
            dCPercentIncreace.Amount = 10;
            dCPercentIncreace.CodeKind = FormData.ChangeType.Percent;
            agendaDCPercentIncreace.DiscountCodes.Add(dCPercentIncreace);
            AgendaItem_CheckBox agendaDCFixIncreace = new AgendaItem_CheckBox("AgendaDCFixIncreace");
            agendaDCFixIncreace.Price = 90;
            DiscountCode dCFixIncreace = new DiscountCode("DCFixIncreace");
            dCFixIncreace.CodeDirection = FormData.ChangePriceDirection.Increase;
            dCFixIncreace.Amount = 10;
            dCFixIncreace.CodeKind = FormData.ChangeType.FixedAmount;
            agendaDCFixIncreace.DiscountCodes.Add(dCFixIncreace);
            evt.AgendaPage = new AgendaPage();
            evt.AgendaPage.AgendaItems.Add(agendaDCFixUnderZero);
            evt.AgendaPage.AgendaItems.Add(agendaDCPercentIncreace);
            evt.AgendaPage.AgendaItems.Add(agendaDCFixIncreace);

            MerchandiseItem merchDCPercentIncreaceUnderZero = new MerchandiseItem("MerchDCPercentIncreaceUZ");
            merchDCPercentIncreaceUnderZero.Price = 110;
            merchDCPercentIncreaceUnderZero.Type = FormData.MerchandiseType.Fixed;
            DiscountCode dCPercentIncreaceUnderZero = new DiscountCode("PercentIncreace0");
            dCPercentIncreaceUnderZero.CodeDirection = FormData.ChangePriceDirection.Increase;
            dCPercentIncreaceUnderZero.Amount = -10;
            dCPercentIncreaceUnderZero.CodeKind = FormData.ChangeType.Percent;
            merchDCPercentIncreaceUnderZero.DiscountCodes.Add(dCPercentIncreaceUnderZero);
            evt.MerchandisePage = new MerchandisePage();
            evt.MerchandisePage.Merchandises.Add(merchDCPercentIncreaceUnderZero);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt);

            EventFeeResponse resp1 = new EventFeeResponse(regType);
            resp1.Code = dCFix;
            EventFeeResponse resp2 = new EventFeeResponse(regType);
            resp2.Code = dCPercentUnderZero;
            AgendaResponse_Checkbox resp3 = new AgendaResponse_Checkbox();
            resp3.AgendaItem = agendaDCFixUnderZero;
            resp3.Checked = true;
            resp3.Code = dCFixUnderZero;
            AgendaResponse_Checkbox resp4 = new AgendaResponse_Checkbox();
            resp4.AgendaItem = agendaDCPercentIncreace;
            resp4.Checked = true;
            resp4.Code = dCPercentIncreace;
            AgendaResponse_Checkbox resp5 = new AgendaResponse_Checkbox();
            resp5.AgendaItem = agendaDCFixIncreace;
            resp5.Checked = true;
            resp5.Code = dCFixIncreace;
            MerchResponse_FixedPrice resp6 = new MerchResponse_FixedPrice();
            resp6.Quantity = 1;
            resp6.Merchandise_Item = merchDCPercentIncreaceUnderZero;
            resp6.Discount_Code = dCPercentIncreaceUnderZero;

            Registrant reg1 = new Registrant(evt);
            reg1.Payment_Method = paymentMethod;
            reg1.EventFee_Response = resp1;
            reg1.CustomField_Responses.Add(resp3);
            reg1.CustomField_Responses.Add(resp4);
            reg1.CustomField_Responses.Add(resp5);
            reg1.Merchandise_Responses.Add(resp6);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg1);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 398);

            Registrant reg2 = new Registrant(evt);
            reg2.Payment_Method = paymentMethod;
            reg2.EventFee_Response = resp2;
            reg2.CustomField_Responses.Add(resp3);
            reg2.CustomField_Responses.Add(resp4);
            reg2.CustomField_Responses.Add(resp5);
            reg2.Merchandise_Responses.Add(resp6);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg2);
            Assert.True(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation) == 408);
        }

        private void GenerateRegDiffDc(DiscountCode regTypeDc, DiscountCode agendaDc, DiscountCode merchDc)
        {
            Registrant reg = new Registrant(diffDcEvt);
            reg.Payment_Method = diffDcPaymentMethod;
            reg.EventFee_Response = new EventFeeResponse(diffDcRegType);
            reg.EventFee_Response.Code = regTypeDc;
            AgendaResponse_Checkbox resp1 = new AgendaResponse_Checkbox();
            resp1.AgendaItem = diffDcAgenda;
            resp1.Checked = true;
            resp1.Code = agendaDc;
            MerchResponse_FixedPrice resp2 = new MerchResponse_FixedPrice();
            resp2.Merchandise_Item = diffDcMerch;
            resp2.Quantity = 1;
            resp2.Discount_Code = merchDc;
            reg.CustomField_Responses.Add(resp1);
            reg.Merchandise_Responses.Add(resp2);

            KeywordProvider.RegistrationCreation.CreateRegistration(reg);
        }
    }
}
