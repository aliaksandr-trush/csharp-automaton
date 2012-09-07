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
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_btnSaveUser", LocateBy.Id);
            Utility.ThreadSleep(2);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
        }

        [Step]
        public void Cancel()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_btnUserCancel", LocateBy.Id);
            Utility.ThreadSleep(2);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
        }

        [Step]
        public void TypeCurrentPasswordToValidate()
        {
            WebDriverUtility.DefaultProvider.Type(
                Locator_Id_Checkbox_CurrentPasswordValidation,
                ConfigReader.DefaultProvider.AccountConfiguration.Password, 
                LocateBy.Id);

            Utility.ThreadSleep(2);
        }

        [Verify]
        public void VerifyCurrentPasswordValidationVisible(bool visible)
        {
            WebDriverUtility.DefaultProvider.VerifyElementDisplay("CurrentPasswordValidation checkbox", visible, WebDriverUtility.DefaultProvider.IsElementDisplay(Locator_Id_Checkbox_CurrentPasswordValidation, LocateBy.Id));
        }

        [Verify]
        public void VerifyIsPasswordEditable(bool editable)
        {
            WebDriverUtility.DefaultProvider.VerifyElementEditable("Password", editable, this.IsPasswordEditable());
        }

        private bool IsPasswordEditable()
        {
            bool passwordEditable = WebDriverUtility.DefaultProvider.IsEditable("ctl00_cphDialog_ucPassword_txtPwd", LocateBy.Id);

            bool confirmPasswordEditable = WebDriverUtility.DefaultProvider.IsEditable("ctl00_cphDialog_ucPassword_txtPwdConfirm", LocateBy.Id);

            if (passwordEditable != confirmPasswordEditable)
            {
                WebDriverUtility.DefaultProvider.FailTest(string.Format(
                    "Password editable capability and confirm password's do not match! Password:{0}, confirm:{1}",
                    passwordEditable,
                    confirmPasswordEditable));
            }

            return passwordEditable;
        }

        [Verify]
        public void VerifyIsUserRoleEditable(bool editable)
        {
            WebDriverUtility.DefaultProvider.VerifyElementEditable("UserRole", editable, this.IsUserRoleEditable());
        }

        private bool IsUserRoleEditable()
        {
            return WebDriverUtility.DefaultProvider.IsEditable("ctl00_cphDialog_ddlRoles", LocateBy.Id);
        }
    }
}