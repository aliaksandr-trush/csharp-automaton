namespace RegOnline.RegressionTest.Managers.Builder
{
    using NUnit.Framework;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public partial class FormDetailManager : ManagerBase
    {
        public void VerifyHasRegTypeWhenPreview(string name, bool has)
        {
            SelectPreviewFrame();
            UIUtilityProvider.UIHelper.VerifyElementPresent(string.Format(RegisterSiteLocator.CheckinRegTypeLabelFormat, name), has, LocateBy.XPath);
            SelectBuilderWindow();
        }

        public void VerifyCustomFieldPresentWhenPreview(string name, bool present)
        {
            SelectPreviewFrame();
            VerifyCustomFieldPresent(name, present);
            SelectBuilderWindow();
        }

        public void VerifyCustomFieldRequiredWhenPreview(string name, bool required)
        {
            SelectPreviewFrame();
            VerifyCustomFieldRequired(name, required);
            SelectBuilderWindow();
        }

        public void VerifyContactInfoPresentWhenPreview(bool present)
        {
            SelectPreviewFrame();
            UIUtilityProvider.UIHelper.VerifyElementPresent(RegisterSiteLocator.ContactInfoNameLocator, present, LocateBy.Id);
            UIUtilityProvider.UIHelper.VerifyElementPresent(RegisterSiteLocator.ContactInfoPhoneLocator, present, LocateBy.Id);
            UIUtilityProvider.UIHelper.VerifyElementPresent(RegisterSiteLocator.ContactInfoEmailLocator, present, LocateBy.Id);
            SelectBuilderWindow();
        }

        public void VerifyStandardFieldsVisibilityWhenPreview(bool recommended, bool optional)
        {
            TogglePreviewAndEditMode();
            SetMobileViewMode(false);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.VerifyEmail), recommended);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Prefix), recommended);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Suffix), recommended);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.NameOnBadge), recommended);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Country), recommended);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.NonUSState), recommended);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.HomePhone), recommended);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Cell), recommended);   

            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.DateOfBirth), optional);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Gender), optional);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.EmergencyContactName), optional);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.EmergencyContactPhone), optional);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.SecondaryEmailAddress), optional);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.MembershipNumber), optional);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.CustomerNumber), optional);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.SocialSecurityNumber), optional);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.TaxIdentificationNumber), optional);
            VerifyCustomFieldPresentWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.UploadPhoto), optional);
            this.VerifyContactInfoPresentWhenPreview(optional);
            TogglePreviewAndEditMode();
        }

        public void VerifyStandardFieldsRequiredWhenPreview(bool recommended, bool optional)
        {
            TogglePreviewAndEditMode();
            SetMobileViewMode(false);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Prefix), recommended);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Suffix), recommended);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.NameOnBadge), recommended);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Country), recommended);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.NonUSState), recommended);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.HomePhone), recommended);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Cell), recommended);

            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.DateOfBirth), optional);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.Gender), optional);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.EmergencyContactName), optional);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.EmergencyContactPhone), optional);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.SecondaryEmailAddress), optional);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.MembershipNumber), optional);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.CustomerNumber), optional);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.SocialSecurityNumber), optional);
            VerifyCustomFieldRequiredWhenPreview(StringEnum.GetStringValue(FormDetailManager.PersonalInfoField.TaxIdentificationNumber), optional);
            TogglePreviewAndEditMode();
        }

        [Step]
        public void SetMobileViewMode(bool on)
        {
            string mobileViewCheckboxLocator = "ctl00_cph_cbMobileView";
            bool actual = UIUtilityProvider.UIHelper.IsChecked(mobileViewCheckboxLocator, LocateBy.Id);

            if (actual != on)
            {
                UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_cbMobileView", on, LocateBy.Id);
                UIUtilityProvider.UIHelper.WaitForPageToLoad();
            }
        }

        public void VerifyLodgingOptionsDisplayWhenPreview()
        {
            SelectPreviewFrame();
            Assert.True(UIUtilityProvider.UIHelper.IsElementPresent(DontPreferLodgingLocator, LocateBy.Id));
            Assert.True(UIUtilityProvider.UIHelper.IsElementPresent(PreferLodgingLocator, LocateBy.Id));
            SelectBuilderWindow();
        }

        public void SelectLodgingOpionWhenPreview(PreferLodging pl)
        {
            SelectPreviewFrame();

            switch (pl)
            {
                case PreferLodging.Yes:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(PreferLodgingLocator, LocateBy.Id);
                    break;
                case PreferLodging.No:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(DontPreferLodgingLocator, LocateBy.Id);
                    break;
                default:
                    break;
            }

            SelectBuilderWindow();
        }
    }
}