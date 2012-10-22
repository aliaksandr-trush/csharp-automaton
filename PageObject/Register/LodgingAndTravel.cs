namespace RegOnline.RegressionTest.PageObject.Register
{
    using System;
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;
    using System.Globalization;

    public class LodgingAndTravel : Window
    {
        public RadioButton CollectLodging = new RadioButton("ctl00_cph_lsf_radCollectLodging", LocateBy.Id);
        public RadioButton DontCollectLodging = new RadioButton("ctl00_cph_lsf_radDontCollectLodging", LocateBy.Id);
        public Input CheckinDate = new Input("ctl00_cph_lsf_rptLodgingFields_ctl00_sf_Hotel_valStart", LocateBy.Id);
        public Input CheckoutDate = new Input("ctl00_cph_lsf_rptLodgingFields_ctl01_sf_Hotel_valEnd", LocateBy.Id);
        public MultiChoiceDropdown RoomPreference = new MultiChoiceDropdown("ctl00_cph_lsf_rptLodgingFields_ctl03_sf_ddlResponse", LocateBy.Id);

        public void CheckinDate_Type(DateTime date)
        {
            string dateString = date.ToString(DateTimeFormatInfo.InvariantInfo);
            this.CheckinDate.Type(dateString);
            UIUtil.DefaultProvider.ExecuteJavaScript(string.Format("document.getElementById('{0}').value='{1}';", this.CheckinDate.Locator, dateString));
        }

        public void CheckoutDate_Type(DateTime date)
        {
            string dateString = date.ToString(DateTimeFormatInfo.InvariantInfo);
            this.CheckoutDate.Type(dateString);
            UIUtil.DefaultProvider.ExecuteJavaScript(string.Format("document.getElementById('{0}').value='{1}';", this.CheckoutDate.Locator, dateString));
        }
    }
}
