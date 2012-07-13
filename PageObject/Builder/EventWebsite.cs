namespace RegOnline.RegressionTest.PageObject.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;

    public class EventWebsite : Window
    {
        public CheckBox UseEventWebsiteAsTheStartingPageForEvent = new CheckBox("ctl00_cph_chkStartonSite", UIUtility.LocateBy.Id);
    }
}
