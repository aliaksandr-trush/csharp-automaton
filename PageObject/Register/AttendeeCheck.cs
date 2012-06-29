namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class AttendeeCheck : Window
    {
        public void SubstituteLink_Click(int index)
        {
            ButtonOrLink Substitute = new ButtonOrLink(string.Format(
                "ctl00_cph_grdMembers_ctl{0}_lnkSubst",
                ((index + 1) < 10 ? "0" : string.Empty) + (index + 1).ToString()),
                LocateBy.Id);

            Substitute.WaitForDisplay();
            Substitute.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
