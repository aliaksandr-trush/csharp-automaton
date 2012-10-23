namespace RegOnline.RegressionTest.PageObject.Register
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;
    using System;

    public class PersonalInfo : Window
    {
        public Input Email = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl02_sf_txtResponse", LocateBy.Id);
        public Input FirstName = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl06_sf_txtResponse", LocateBy.Id);
        public Input MiddleName = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl07_sf_txtResponse", LocateBy.Id);
        public Input LastName = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl08_sf_txtResponse", LocateBy.Id);
        public Input JobTitle = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl10_sf_txtResponse", LocateBy.Id);
        public Input Company = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl12_sf_txtResponse", LocateBy.Id);
        public Input AddressOne = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl14_sf_txtResponse", LocateBy.Id);
        public Input City = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl16_sf_txtResponse", LocateBy.Id);
        public MultiChoiceDropdown Country = new MultiChoiceDropdown("ctl00_cph_personalInfoStandardFields_rptFields_ctl13_sf_ddlResponse", LocateBy.Id);
        public MultiChoiceDropdown State = new MultiChoiceDropdown("ctl00_cph_personalInfoStandardFields_rptFields_ctl17_sf_ddlResponse", LocateBy.Id);
        public Input NonUSState = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl18_sf_txtResponse", LocateBy.Id);
        public Input Zip = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl19_sf_txtResponse", LocateBy.Id);
        public Input WorkPhone = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl21_sf_txtResponse", LocateBy.Id);
        public Input DateOfBirth = new Input("ctl00_cph_personalInfoStandardFields_rptFields_ctl25_sf_dpResponse", LocateBy.Id);
        public MultiChoiceDropdown Gender = new MultiChoiceDropdown("ctl00_cph_personalInfoStandardFields_rptFields_ctl26_sf_ddlResponse", LocateBy.Id);
        public Input Password = new Input("ctl00_cph_ctlPassword_txtPassword", LocateBy.Id);
        public Input PasswordReEnter = new Input("ctl00_cph_ctlPassword_txtVerifyPassword", LocateBy.Id);
        public Clickable ChangeRegType = new Clickable("ctl00_cph_lnkChangeRegType", LocateBy.Id);
        public RegTypeList RegTypeList = new RegTypeList(0);

        public Label PersonalInfoFields(DataCollection.EventData_Common.PersonalInfoField field)
        {
            return new Label(string.Format("//ol[@class='fieldList']//*[contains(text(),'{0}')]/../following-sibling::*",
                CustomStringAttribute.GetCustomString(field)), LocateBy.XPath);
        }

        public void ChangeRegType_Click()
        {
            this.ChangeRegType.WaitForDisplay();
            this.ChangeRegType.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class RegTypeList : Frame
    {
        public RegTypeList(int index) : base(index) { }

        public Clickable OK = new Clickable("//div[@class='buttonGroup dialogButtonGroup']/button[@type='submit']", LocateBy.XPath);
        public Clickable Cancel = new Clickable("//div[@class='buttonGroup dialogButtonGroup']/a[@class='button closeDialogButton']", LocateBy.XPath);

        public RadioButton RegTypeRadio(RegType regType)
        {
            return new RadioButton(
                string.Format("//input[@value='{0}']", regType.Id), LocateBy.XPath);
        }

        public void OK_Click()
        {
            this.OK.WaitForDisplay();
            this.OK.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            this.Cancel.WaitForDisplay();
            this.Cancel.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            SwitchToMain();
        }
    }

    public class CustomFieldRow
    {
        private string locator = "//li[@data-id='{0}']";

        public ElementBase CustomFieldType = new ElementBase();
        public ElementBase CustomFieldLabel = new ElementBase();

        public CustomFieldRow(CustomField field)
        {
            Label cfNameLabel = new Label(string.Format("//label[text()='{0}']", field.NameOnForm), LocateBy.XPath);
            field.Id = Convert.ToInt32(cfNameLabel.GetAttribute("for"));

            if (field is CFCheckBox)
            {
                CustomFieldType = new CheckBox(string.Format(locator + "//input", field.Id.ToString()), LocateBy.XPath);
                CustomFieldLabel = new Label(string.Format(locator + "//label", field.Id.ToString()), LocateBy.XPath);
            }
        }
    }
}
