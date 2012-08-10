namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class PersonalInfo : Window
    {
        public TextBox Email = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl02_sf_txtResponse", LocateBy.Id);
        public TextBox FirstName = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl06_sf_txtResponse", LocateBy.Id);
        public TextBox MiddleName = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl07_sf_txtResponse", LocateBy.Id);
        public TextBox LastName = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl08_sf_txtResponse", LocateBy.Id);
        public TextBox JobTitle = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl10_sf_txtResponse", LocateBy.Id);
        public TextBox Company = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl12_sf_txtResponse", LocateBy.Id);
        public TextBox AddressOne = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl14_sf_txtResponse", LocateBy.Id);
        public TextBox City = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl16_sf_txtResponse", LocateBy.Id);
        public MultiChoiceDropdown Country = new MultiChoiceDropdown("ctl00_cph_personalInfoStandardFields_rptFields_ctl13_sf_ddlResponse", LocateBy.Id);
        public MultiChoiceDropdown State = new MultiChoiceDropdown("ctl00_cph_personalInfoStandardFields_rptFields_ctl17_sf_ddlResponse", LocateBy.Id);
        public TextBox NonUSState = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl18_sf_txtResponse", LocateBy.Id);
        public TextBox Zip = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl19_sf_txtResponse", LocateBy.Id);
        public TextBox WorkPhone = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl21_sf_txtResponse", LocateBy.Id);
        public TextBox DateOfBirth = new TextBox("ctl00_cph_personalInfoStandardFields_rptFields_ctl25_sf_dpResponse", LocateBy.Id);
        public MultiChoiceDropdown Gender = new MultiChoiceDropdown("ctl00_cph_personalInfoStandardFields_rptFields_ctl26_sf_ddlResponse", LocateBy.Id);
        public TextBox Password = new TextBox("ctl00_cph_ctlPassword_txtPassword", LocateBy.Id);
        public TextBox PasswordReEnter = new TextBox("ctl00_cph_ctlPassword_txtVerifyPassword", LocateBy.Id);
        public ButtonOrLink ChangeRegType = new ButtonOrLink("ctl00_cph_lnkChangeRegType", LocateBy.Id);
        public RegTypeList RegTypeList = new RegTypeList(0);

        public Label PersonalInfoFields(FormData.PersonalInfoField field)
        {
            return new Label(string.Format("//ol[@class='fieldList']//*[contains(text(),'{0}')]/../following-sibling::*",
                CustomStringAttribute.GetCustomString(field)), LocateBy.XPath);
        }

        public void ChangeRegType_Click()
        {
            this.ChangeRegType.WaitForDisplay();
            this.ChangeRegType.Click();
            Utility.ThreadSleep(1);
            WaitForLoad();
        }
    }

    public class RegTypeList : Frame
    {
        public RegTypeList(int index) : base(index) { }

        public ButtonOrLink OK = new ButtonOrLink("//div[@class='buttonGroup dialogButtonGroup']/button[@type='submit']", LocateBy.XPath);
        public ButtonOrLink Cancel = new ButtonOrLink("//div[@class='buttonGroup dialogButtonGroup']/a[@class='button closeDialogButton']", LocateBy.XPath);

        public RadioButton RegTypeRadio(RegType regType)
        {
            return new RadioButton(
                string.Format("//input[@value='{0}']", regType.RegTypeId), LocateBy.XPath);
        }

        public void OK_Click()
        {
            this.OK.WaitForDisplay();
            this.OK.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            WaitForLoad();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            this.Cancel.WaitForDisplay();
            this.Cancel.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            SwitchToMain();
        }
    }

    public class CustomFieldRow
    {
        private string locator = "//li[@data-id='{0}']";

        public WebElement CustomFieldType = new WebElement();
        public WebElement CustomFieldLabel = new WebElement();

        public CustomFieldRow(CustomField field)
        {
            if (field is CFCheckBox)
            {
                CustomFieldType = new CheckBox(string.Format(locator + "//input", field.Id.ToString()), LocateBy.XPath);
                CustomFieldLabel = new Label(string.Format(locator + "//label", field.Id.ToString()), LocateBy.XPath);
            }
        }
    }
}
