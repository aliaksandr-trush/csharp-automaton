namespace RegOnline.RegressionTest.Fixtures.Base
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Keyword;

    public class AssertHelper
    {
        public static void VerifyOnPage(FormData.RegisterPage page, bool onPage)
        {
            Assert.AreEqual(onPage, PageObject.PageObjectProvider.Register.RegistationSite.IsOnPage(page));
        }
    }
}
