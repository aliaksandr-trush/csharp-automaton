namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class PersonalInfo : Window
    {
        public ButtonOrLink PersonalInfoPageHeader = new ButtonOrLink("//*[text()='Add Personal Information Page Header']", LocateBy.XPath);
        public HtmlEditor PersonalInfoPageHeaderEditor = new HtmlEditor("dialog");
        public ButtonOrLink PersonalInfoPageFooter = new ButtonOrLink("//*[text()='Add Personal Information Page Footer']", LocateBy.XPath);
        public HtmlEditor PersonalInfoPageFooterEditor = new HtmlEditor("dialog");
        public ButtonOrLink EmptyAddCustomField = new ButtonOrLink("ctl00_cph_grdCustomFieldPersonal_lnkEmptyAdd", LocateBy.Id);
        public ButtonOrLink AddCustomField = new ButtonOrLink("ctl00_cph_grdCustomFieldPersonal_hlAddNew", LocateBy.Id);

        public void SetPersonalInfoFieldVisible(FormData.PersonalInfoField field, bool checkVisibleOption)
        {
            switch (field)
            {
                case FormData.PersonalInfoField.OptOut:
                    CheckBox OptOutVisible = new CheckBox(string.Format("//*[contains(text(),'{0}')]/../../td[2]/input", CustomStringAttribute.GetCustomString(field)), LocateBy.XPath);
                    OptOutVisible.Set(checkVisibleOption);
                    break;
                case FormData.PersonalInfoField.SocialSecurityNumber:
                    CheckBox SSNVisible = new CheckBox("ctl00_cph_chkCollectionFieldscSSNID", LocateBy.Id);
                    SSNVisible.Set(checkVisibleOption);
                    break;
                case FormData.PersonalInfoField.ContactInfo:
                    CheckBox ContactInfoVisible = new CheckBox(string.Format("//*[contains(text(),'{0}')]/../td[2]/input", StringEnum.GetStringValue(field)), LocateBy.XPath);
                    ContactInfoVisible.Set(checkVisibleOption);
                    break;
                default:
                    CheckBox StandardFieldVisible = new CheckBox(string.Format("//*[text()='{0}']/../../../..//td[2]/input", CustomStringAttribute.GetCustomString(field)), LocateBy.XPath);
                    StandardFieldVisible.Set(checkVisibleOption);
                    break;
            }
        }

        public void SetPersonalInfoFieldRequired(FormData.PersonalInfoField field, bool checkRequiredOption)
        {
            switch (field)
            {
                case FormData.PersonalInfoField.SocialSecurityNumber:
                    CheckBox SSNVisible = new CheckBox("ctl00_cph_chkCollectionFieldsrSSNID", LocateBy.Id);
                    SSNVisible.Set(checkRequiredOption);
                    break;
                case FormData.PersonalInfoField.ContactInfo:
                    CheckBox ContactInfoVisible = new CheckBox(string.Format("//*[contains(text(),'{0}')]/../td[3]/input", StringEnum.GetStringValue(field)), LocateBy.XPath);
                    ContactInfoVisible.Set(checkRequiredOption);
                    break;
                default:
                    CheckBox StandardFieldVisible = new CheckBox(string.Format("//*[text()='{0}']/../../../..//td[3]/input", CustomStringAttribute.GetCustomString(field)), LocateBy.XPath);
                    StandardFieldVisible.Set(checkRequiredOption);
                    break;
            }
        }

        public void EmptyAddCustomField_Click()
        {
            this.EmptyAddCustomField.WaitForDisplay();
            this.EmptyAddCustomField.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddCustomField_Click()
        {
            this.AddCustomField.WaitForDisplay();
            this.AddCustomField.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void PersonalInfoPageHeader_Click()
        {
            this.PersonalInfoPageHeader.WaitForDisplay();
            this.PersonalInfoPageHeader.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void PersonalInfoPageFooter_Click()
        {
            this.PersonalInfoPageFooter.WaitForDisplay();
            this.PersonalInfoPageFooter.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class PICustomFieldRow
    {
        public int CustomFieldId;
        public string CustomFIeldName;
        public ButtonOrLink CustomFieldTitle;

        public PICustomFieldRow(string name)
        {
            this.CustomFIeldName = name;

            this.CustomFieldTitle = new ButtonOrLink(
                string.Format("//table[@id='ctl00_cph_grdCustomFieldPersonal_tblGrid']//a[text()='{0}']", this.CustomFIeldName),
                LocateBy.XPath);

            string customFieldHrefAttributeText = this.CustomFieldTitle.GetAttribute("href");

            string tmp = customFieldHrefAttributeText.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries)[2];
            tmp = tmp.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1];

            this.CustomFieldId = Convert.ToInt32(tmp);
        }

        public void Title_Click()
        {
            this.CustomFieldTitle.WaitForDisplay();
            this.CustomFieldTitle.Click();
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }
    }
}
