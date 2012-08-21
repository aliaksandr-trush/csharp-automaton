namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.FeeOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Keyword;

    public class DiscountCodeFixture : FixtureBase
    {
        [Test]
        [Category(Priority.Two)]
        [Description("692")]
        public void RegTypeDiscount()
        {
            Event evt = new Event("RegTypeDiscount");
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
            dc2.Amount = 10;
            dc2.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc2.CodeKind = FormData.ChangeType.Percent;
            dc2.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc3 = new DiscountCode("dc3");
            dc3.Amount = 10;
            dc3.CodeDirection = FormData.ChangePriceDirection.Decrease;
            dc3.CodeKind = FormData.ChangeType.Percent;
            dc3.CodeType = FormData.DiscountCodeType.DiscountCode;
            DiscountCode dc4 = new DiscountCode("dc4");
            dc4.Amount = 10;
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
        }
    }
}
