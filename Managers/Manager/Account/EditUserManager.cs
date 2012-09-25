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
            UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_btnSaveUser", LocateBy.Id);
            Utility.ThreadSleep(2);
            UIUtil.DefaultProvider.SwitchToMainContent();
        }

        [Step]
        public void Cancel()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_btnUserCancel", LocateBy.Id);
            Utility.ThreadSleep(2);
            UIUtil.DefaultProvider.SwitchToMainContent();
        }

        [Step]
        public void TypeCurrentPasswordToValidate()
        {
            UIUtil.DefaultProvider.Type(
                Locator_Id_Checkbox_CurrentPasswordValidation,
                ConfigReader.DefaultProvider.AccountConfiguration.Password, 
                LocateBy.Id);

            Utility.ThreadSleep(2);
        }

        [Verify]
        public void VerifyCurrentPasswordValidationVisible(bool visible)
        {
            UIUtil.DefaultProvider.VerifyElementDisplay("CurrentPasswordValidation checkbox", visible, UIUtil.DefaultProvider.IsElementDisplay(Locator_Id_Checkbox_CurrentPasswordValidation, LocateBy.Id));
        }

        [Verify]
        public void VerifyIsPasswordEditable(bool editable)
        {
            UIUtil.DefaultProvider.VerifyElementEditable("Password", editable, this.IsPasswordEditable());
        }

        private bool IsPasswordEditable()
        {
            bool passwordEditable = UIUtil.DefaultProvider.IsEditable("ctl00_cphDialog_ucPassword_txtPwd", LocateBy.Id);

            bool confirmPasswordEditable = UIUtil.DefaultProvider.IsEditable("ctl00_cphDialog_ucPassword_txtPwdConfirm", LocateBy.Id);

            if (passwordEditable != confirmPasswordEditable)
            {
                UIUtil.DefaultProvider.FailTest(string.Format(
                    "Password editable capability and confirm password's do not match! Password:{0}, confirm:{1}",
                    passwordEditable,
                    confirmPasswordEditable));
            }

            return passwordEditable;
        }

        [Verify]
        public void VerifyIsUserRoleEditable(bool editable)
        {
            UIUtil.DefaultProvider.VerifyElementEditable("UserRole", editable, this.IsUserRoleEditable());
        }

        private bool IsUserRoleEditable()
        {
            return UIUtil.DefaultProvider.IsEditable("ctl00_cphDialog_ddlRoles", LocateBy.Id);
        }
    }
}