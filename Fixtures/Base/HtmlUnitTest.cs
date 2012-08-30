namespace RegOnline.RegressionTest.Fixtures.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using RegOnline.RegressionTest.UIUtility;

    public class HtmlUnitTest : FixtureBase
    {
        [Test]
        public void CheckinXiami()
        {
            UIUtilityProvider.UIHelper.OpenUrl("www.xiami.com");
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("top_login", LocateBy.Id);
        }
    }
}
