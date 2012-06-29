namespace RegOnline.RegressionTest.Managers.Manager.Account
{
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Attributes;

    public class EditUserManager
    {
        public const string Locator_Id_Div_EditUserPanel = "ctl00_cphDialog_ctl00_cphDialog_bccPanel";
        private const string Locator_Id_Checkbox_CurrentPasswordValidation = "ctl00_cphDialog_txtCurrentUserPwd";

        [Step]
        public void SaveAndClose()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_btnSaveUser", LocateBy.Id);
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        [Step]
        public void Cancel()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_btnUserCancel", LocateBy.Id);
            Utility.ThreadSleep(2);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        [Step]
        public void TypeCurrentPasswordToValidate()
        {
            UIUtilityProvider.UIHelper.Type(
                Locator_Id_Checkbox_CurrentPasswordValidation,
                ConfigurationProvider.XmlConfig.AccountConfiguration.Password, 
                LocateBy.Id);

            Utility.ThreadSleep(2);
        }

        [Verify]
        public void VerifyCurrentPasswordValidationVisible(bool visible)
        {
            UIUtilityProvider.UIHelper.VerifyElementDisplay("CurrentPasswordValidation checkbox", visible, UIUtilityProvider.UIHelper.IsElementDisplay(Locator_Id_Checkbox_CurrentPasswordValidation, LocateBy.Id));
        }

        [Verify]
        public void VerifyIsPasswordEditable(bool editable)
        {
            UIUtilityProvider.UIHelper.VerifyElementEditable("Password", editable, this.IsPasswordEditable());
        }

        private bool IsPasswordEditable()
        {
            bool passwordEditable = UIUtilityProvider.UIHelper.IsEditable("ctl00_cphDialog_ucPassword_txtPwd", LocateBy.Id);

            bool confirmPasswordEditable = UIUtilityProvider.UIHelper.IsEditable("ctl00_cphDialog_ucPassword_txtPwdConfirm", LocateBy.Id);

            if (passwordEditable != confirmPasswordEditable)
            {
                UIUtilityProvider.UIHelper.FailTest(string.Format(
                    "Password editable capability and confirm password's do not match! Password:{0}, confirm:{1}",
                    passwordEditable,
                    confirmPasswordEditable));
            }

            return passwordEditable;
        }

        [Verify]
        public void VerifyIsUserRoleEditable(bool editable)
        {
            UIUtilityProvider.UIHelper.VerifyElementEditable("UserRole", editable, this.IsUserRoleEditable());
        }

        private bool IsUserRoleEditable()
        {
            return UIUtilityProvider.UIHelper.IsEditable("ctl00_cphDialog_ddlRoles", LocateBy.Id);
        }
    }
}