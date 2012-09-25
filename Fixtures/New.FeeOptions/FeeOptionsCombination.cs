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
            Event evt = new Event("FeeOptionsCombinationFixture");
            RegType regType1 = new RegType("regType1");
        }
    }
}
