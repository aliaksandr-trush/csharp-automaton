namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class AttendeeCheck : Window
    {
        public ButtonOrLink PersonalInfo(int index)
        {
            return new ButtonOrLink(string.Format(
                                      "ctl00_cph_grdMembers_ctl{0}_lnkPersInfo",
                                      ((index + 1) < 10 ? "0" : string.Empty) + (index + 1).ToString()),
                                      LocateBy.Id);
        }

        public ButtonOrLink Substitute(int index)
        {
            return new ButtonOrLink(string.Format(
                           "ctl00_cph_grdMembers_ctl{0}_lnkSubst",
                           ((index + 1) < 10 ? "0" : string.Empty) + (index + 1).ToString()),
                           LocateBy.Id);
        }

        public void PersonalInfoLink_Click(int index)
        {
            ButtonOrLink PersonalInfo = this.PersonalInfo(index);

            PersonalInfo.WaitForDisplay();
            PersonalInfo.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void SubstituteLink_Click(int index)
        {
            ButtonOrLink Substitute = this.Substitute(index);

            Substitute.WaitForDisplay();
            Substitute.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
