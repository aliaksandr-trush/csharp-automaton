namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class PersonalInfo
    {
        public TextBox Email = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl02_sf_txtResponse", LocateBy.Id);
        public TextBox FirstName = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl06_sf_txtResponse", LocateBy.Id);
        public TextBox MiddleName = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl07_sf_txtResponse", LocateBy.Id);
        public TextBox LastName = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl08_sf_txtResponse", LocateBy.Id);
        public TextBox JobTitle = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl10_sf_txtResponse", LocateBy.Id);
        public TextBox Company = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl12_sf_txtResponse", LocateBy.Id);
        public TextBox AddressOne = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl14_sf_txtResponse", LocateBy.Id);
        public TextBox City = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl16_sf_txtResponse", LocateBy.Id);
        public MultiChoiceDropdown State = new MultiChoiceDropdown("ctl00_cph_personalInfoStandardFields_rptFields_ctl17_sf_ddlResponse", LocateBy.Id);
        public TextBox Zip = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl19_sf_txtResponse", LocateBy.Id);
        public TextBox WorkPhone = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl21_sf_txtResponse", LocateBy.Id);
        public TextBox Password = new TextBox("ctl00_cph_ctlPassword_txtPassword", LocateBy.Id);
        public TextBox PasswordReEnter = new TextBox("ctl00_cph_ctlPassword_txtVerifyPassword", LocateBy.Id);

        public Label PersonalInfoFields(FormData.PersonalInfoField field)
        {
            return new Label(string.Format("//ol[@class='fieldList']//*[contains(text(),'{0}')]/../following-sibling::*",
                CustomStringAttribute.GetCustomString(field)), LocateBy.XPath);
        }
    }
}
