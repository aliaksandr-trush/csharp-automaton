namespace RegOnline.RegressionTest.Fixtures.RegistrationInventory.AgendaPage
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Fixtures.Base;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class OtherTestsRelatedToAgenda : FixtureBase
    {
        [Test]
        [Category(Priority.Four)]
        public void ButtonTest()
        {
            Event evt = new Event("AgendaButtonTest");
        }
    }
}
