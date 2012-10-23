namespace RegOnline.RegressionTest.Fixtures.New.FeeOptions
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class FeeOptionsCombination : FixtureBase
    {
        [Test]
        [Category(Priority.Two)]
        public void FeeOptionsCombinationFixture()
        {
            //Set up start page and event level fee options
            Event evt = new Event("FeeOptionsCombinationFixture");
            TaxRate tax1 = new TaxRate("tax1");
            tax1.Rate = 10;
            TaxRate tax2 = new TaxRate("tax2");
            tax2.Rate = 20;
            evt.TaxRateOne = tax1;
            evt.TaxRateTwo = tax2;
            GroupDiscount groupDiscount = new GroupDiscount();
            groupDiscount.GroupSize = 2;
            groupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.SizeOrMore;
            groupDiscount.DiscountAmount = 30;
            groupDiscount.GroupDiscountType = GroupDiscount_DiscountType.Percent;
            evt.StartPage.GroupDiscount = groupDiscount;
            RegType regType1 = new RegType("regType1");
            regType1.Price = 20;
            regType1.ApplyTaxOne = true;
            regType1.ApplyTaxTwo = true;
            CustomFieldCode code1 = new CustomFieldCode("code1");
            code1.Amount = 1;
            code1.CodeKind = DataCollection.EventData_Common.ChangeType.FixedAmount;
            code1.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            code1.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            CustomFieldCode code2 = new CustomFieldCode("code2");
            code2.Amount = 5;
            code2.CodeKind = DataCollection.EventData_Common.ChangeType.Percent;
            code2.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            code2.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            regType1.AllCustomCodes.Add(code1);
            regType1.AllCustomCodes.Add(code2);
            RegType regType2 = new RegType("regType2");
            regType2.Price = 30;
            regType2.ApplyTaxOne = true;
            regType2.ApplyTaxTwo = true;
            CustomFieldCode code3 = new CustomFieldCode("code3");
            code3.Amount = 2;
            code3.CodeKind = DataCollection.EventData_Common.ChangeType.FixedAmount;
            code3.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            code3.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            CustomFieldCode code4 = new CustomFieldCode("code4");
            code4.Amount = 15;
            code4.CodeKind = DataCollection.EventData_Common.ChangeType.Percent;
            code4.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            code4.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            regType2.AllCustomCodes.Add(code3);
            regType2.AllCustomCodes.Add(code4);
            evt.StartPage.RegTypes.Add(regType1);
            evt.StartPage.RegTypes.Add(regType2);

            //Set up agenda page
            evt.AgendaPage = new AgendaPage();
            AgendaItem_CheckBox agenda1 = new AgendaItem_CheckBox("agenda1");
            agenda1.Price = 40;
            agenda1.ApplyTaxOne = true;
            agenda1.ApplyTaxTwo = true;
            CustomFieldCode code5 = new CustomFieldCode("code5");
            code5.Amount = 3;
            code5.CodeKind = DataCollection.EventData_Common.ChangeType.FixedAmount;
            code5.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            code5.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            CustomFieldCode code6 = new CustomFieldCode("code6");
            code6.Amount = 25;
            code6.CodeKind = DataCollection.EventData_Common.ChangeType.Percent;
            code6.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            code6.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            agenda1.DiscountCodes.Add(code5);
            agenda1.DiscountCodes.Add(code6);
            AgendaItem_CheckBox agenda2 = new AgendaItem_CheckBox("agenda2");
            agenda2.Price = 50;
            agenda2.ApplyTaxOne = true;
            agenda2.ApplyTaxTwo = true;
            CustomFieldCode code7 = new CustomFieldCode("code7");
            code7.Amount = 4;
            code7.CodeKind = DataCollection.EventData_Common.ChangeType.FixedAmount;
            code7.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            code7.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            CustomFieldCode code8 = new CustomFieldCode("code8");
            code8.Amount = 35;
            code8.CodeKind = DataCollection.EventData_Common.ChangeType.Percent;
            code8.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            code8.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            agenda2.DiscountCodes.Add(code7);
            agenda2.DiscountCodes.Add(code8);
            evt.AgendaPage.AgendaItems.Add(agenda1);
            evt.AgendaPage.AgendaItems.Add(agenda2);

            //Set up L&T page
            evt.LodgingTravelPage = new LodgingTravelPage();
            evt.LodgingTravelPage.Lodging = new Lodging();
            evt.LodgingTravelPage.Lodging.ChargeLodgingFee = true;
            Hotel hotel = new Hotel("AutoHotel");
            RoomType roomType1 = new RoomType("roomType1");
            roomType1.RoomRate = 100;
            RoomType roomType2 = new RoomType("roomType2");
            roomType2.RoomRate = 200;
            RoomType roomType3 = new RoomType("roomType3");
            roomType3.RoomRate = 300;
            hotel.RoomTypes.Add(roomType1);
            hotel.RoomTypes.Add(roomType2);
            hotel.RoomTypes.Add(roomType3);
            RoomBlock roomBlock = new RoomBlock();
            roomBlock.Date = DateTime.Today.AddDays(1);
            hotel.RoomBlocks.Add(roomBlock);
            evt.LodgingTravelPage.Lodging.Hotels.Add(hotel);

            //Set up merchandise page
            evt.MerchandisePage = new MerchandisePage();
            MerchandiseItem merch1 = new MerchandiseItem("merch1");
            merch1.ApplyTaxOne = true;
            merch1.ApplyTaxTwo = true;
            merch1.Type = DataCollection.EventData_Common.MerchandiseType.Fixed;
            merch1.Price = 60;
            CustomFieldCode code9 = new CustomFieldCode("code9");
            code9.CodeKind = DataCollection.EventData_Common.ChangeType.FixedAmount;
            code9.CodeType = DataCollection.EventData_Common.CustomFieldCodeType.DiscountCode;
            code9.Amount = 5;
            code9.CodeDirection = DataCollection.EventData_Common.ChangePriceDirection.Decrease;
            merch1.DiscountCodes.Add(code9);
            MerchandiseItem merch2 = new MerchandiseItem("merch2");
            merch2.ApplyTaxOne = true;
            merch2.ApplyTaxTwo = true;
            merch2.Type = DataCollection.EventData_Common.MerchandiseType.Variable;
            merch2.MinPrice = 65;
            merch2.MaxPrice = 75;
            evt.MerchandisePage.Merchandises.Add(merch1);
            evt.MerchandisePage.Merchandises.Add(merch2);
            evt.MerchandisePage.ShippingFee = 20;

            //Set up checkout page
            PaymentMethod paymentMethod = new PaymentMethod(DataCollection.EventData_Common.PaymentMethodEnum.Check);
            evt.CheckoutPage.PaymentMethods.Add(paymentMethod);
            evt.CheckoutPage.AddServiceFee = true;

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt, false);

            EventFeeResponse regTypeResp1 = new EventFeeResponse(regType1);
            regTypeResp1.Fee = 20;
            regTypeResp1.Code = code1;
            EventFeeResponse regTypeResp2 = new EventFeeResponse(regType1);
            regTypeResp2.Fee = 20;
            regTypeResp2.Code = code2;
            EventFeeResponse regTypeResp3 = new EventFeeResponse(regType2);
            regTypeResp3.Fee = 30;
            regTypeResp3.Code = code3;
            EventFeeResponse regTypeResp4 = new EventFeeResponse(regType2);
            regTypeResp4.Fee = 30;
            regTypeResp4.Code = code4;
            AgendaResponse_Checkbox agendaResp1 = new AgendaResponse_Checkbox();
            agendaResp1.Checked = true;
            agendaResp1.Fee = 40;
            agendaResp1.AgendaItem = agenda1;
            agendaResp1.Code = code5;
            AgendaResponse_Checkbox agendaResp2 = new AgendaResponse_Checkbox();
            agendaResp2.Checked = true;
            agendaResp2.Fee = 40;
            agendaResp2.AgendaItem = agenda1;
            agendaResp2.Code = code6;
            AgendaResponse_Checkbox agendaResp3 = new AgendaResponse_Checkbox();
            agendaResp3.Checked = true;
            agendaResp3.Fee = 50;
            agendaResp3.AgendaItem = agenda2;
            agendaResp3.Code = code7;
            AgendaResponse_Checkbox agendaResp4 = new AgendaResponse_Checkbox();
            agendaResp4.Checked = true;
            agendaResp4.Fee = 50;
            agendaResp4.AgendaItem = agenda2;
            agendaResp4.Code = code8;
            LodgingResponse lodgingResp1 = new LodgingResponse();
            lodgingResp1.Hotel = hotel;
            lodgingResp1.RoomType = roomType1;
            lodgingResp1.CheckinDate = DateTime.Today.AddDays(-1);
            lodgingResp1.CheckoutDate = DateTime.Today.AddDays(3);
            LodgingResponse lodgingResp2 = new LodgingResponse();
            lodgingResp2.Hotel = hotel;
            lodgingResp2.RoomType = roomType2;
            lodgingResp2.CheckinDate = DateTime.Today.AddDays(3);
            lodgingResp2.CheckoutDate = DateTime.Today.AddDays(5);
            MerchResponse_FixedPrice merchResp1 = new MerchResponse_FixedPrice();
            merchResp1.Fee = 60;
            merchResp1.Merchandise_Item = merch1;
            merchResp1.Quantity = 2;
            merchResp1.Discount_Code = code9;
            MerchResponse_VariableAmount merchResp2 = new MerchResponse_VariableAmount();
            merchResp2.Fee = 70;
            merchResp2.Merchandise_Item = merch2;
            merchResp2.Amount = 70;

            Registrant reg1 = new Registrant(evt);
            reg1.Payment_Method = paymentMethod;
            reg1.EventFee_Response = regTypeResp1;
            reg1.CustomField_Responses.Add(agendaResp1);
            reg1.CustomField_Responses.Add(agendaResp3);
            reg1.Lodging_Responses.Add(lodgingResp1);
            reg1.Merchandise_Responses.Add(merchResp1);
            reg1.Merchandise_Responses.Add(merchResp2);
            Registrant reg2 = new Registrant(evt);
            reg2.EventFee_Response = regTypeResp2;
            reg2.CustomField_Responses.Add(agendaResp2);
            reg2.CustomField_Responses.Add(agendaResp4);
            reg2.Lodging_Responses.Add(lodgingResp2);
            Registrant reg3 = new Registrant(evt);
            reg3.EventFee_Response = regTypeResp3;
            reg3.CustomField_Responses.Add(agendaResp1);
            reg3.CustomField_Responses.Add(agendaResp3);
            reg3.Lodging_Responses.Add(lodgingResp1);
            Registrant reg4 = new Registrant(evt);
            reg4.EventFee_Response = regTypeResp4;
            reg4.CustomField_Responses.Add(agendaResp2);
            reg4.Lodging_Responses.Add(lodgingResp2);

            Group group1 = new Group();
            group1.Primary = reg1;
            group1.Secondaries.Add(reg2);
            group1.Secondaries.Add(reg3);
            group1.Secondaries.Add(reg4);

            Keyword.KeywordProvider.RegistrationCreation.GroupRegistration(group1);
            Assert.AreEqual(Keyword.KeywordProvider.CalculateFee.CalculateTotalFee(group1),
                Keyword.KeywordProvider.Register_Common.GetTotal(DataCollection.EventData_Common.RegisterPage.Confirmation));
        }
    }
}