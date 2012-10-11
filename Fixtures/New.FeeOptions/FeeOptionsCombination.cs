namespace RegOnline.RegressionTest.Fixtures.New.FeeOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Keyword;

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
            code1.CodeKind = FormData.ChangeType.FixedAmount;
            code1.CodeType = FormData.CustomFieldCodeType.DiscountCode;
            CustomFieldCode code2 = new CustomFieldCode("code2");
            code2.Amount = 5;
            code2.CodeKind = FormData.ChangeType.Percent;
            code2.CodeType = FormData.CustomFieldCodeType.DiscountCode;
            regType1.AllCustomCodes.Add(code1);
            regType1.AllCustomCodes.Add(code2);
            RegType regType2 = new RegType("regType2");
            regType2.Price = 30;
            regType2.ApplyTaxOne = true;
            regType2.ApplyTaxTwo = true;
            CustomFieldCode code3 = new CustomFieldCode("code3");
            code3.Amount = 2;
            code3.CodeKind = FormData.ChangeType.FixedAmount;
            code3.CodeType = FormData.CustomFieldCodeType.DiscountCode;
            CustomFieldCode code4 = new CustomFieldCode("code4");
            code4.Amount = 15;
            code4.CodeKind = FormData.ChangeType.Percent;
            code4.CodeType = FormData.CustomFieldCodeType.DiscountCode;
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
            code5.CodeKind = FormData.ChangeType.FixedAmount;
            code5.CodeType = FormData.CustomFieldCodeType.DiscountCode;
            CustomFieldCode code6 = new CustomFieldCode("code6");
            code6.Amount = 25;
            code6.CodeKind = FormData.ChangeType.Percent;
            code6.CodeType = FormData.CustomFieldCodeType.DiscountCode;
            agenda1.DiscountCodes.Add(code5);
            agenda1.DiscountCodes.Add(code6);
            AgendaItem_CheckBox agenda2 = new AgendaItem_CheckBox("agenda2");
            agenda2.Price = 50;
            agenda2.ApplyTaxOne = true;
            agenda2.ApplyTaxTwo = true;
            CustomFieldCode code7 = new CustomFieldCode("code7");
            code7.Amount = 4;
            code7.CodeKind = FormData.ChangeType.FixedAmount;
            code7.CodeType = FormData.CustomFieldCodeType.DiscountCode;
            CustomFieldCode code8 = new CustomFieldCode("code8");
            code8.Amount = 35;
            code8.CodeKind = FormData.ChangeType.Percent;
            code8.CodeType = FormData.CustomFieldCodeType.DiscountCode;
            agenda2.DiscountCodes.Add(code7);
            agenda2.DiscountCodes.Add(code8);
            evt.AgendaPage.AgendaItems.Add(agenda1);
            evt.AgendaPage.AgendaItems.Add(agenda2);

            //Set up L&T page
            evt.LodgingTravelPage = new LodgingTravelPage();
            Hotel hotel = new Hotel();
        }
    }
}
