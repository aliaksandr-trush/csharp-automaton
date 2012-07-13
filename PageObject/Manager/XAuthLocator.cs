namespace RegOnline.RegressionTest.PageObject.Manager
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public abstract class XAuthLocator
    {
        public RadioButton XAuthRadio = new RadioButton("ctl00_cphDialog_xAuth_rdoXAuth", LocateBy.Id);

        public ButtonOrLink Test;
        public ButtonOrLink OK;
        public TextBox ServiceEndpointURL;
        public TextBox MessageToRegistration;
        public TextBox TestEmail;
        public TextBox TestUserName;
        public TextBox TestPassword;
        public TextBox DescriptionForIdentifer;
        public TextBox ForgetPasswordUrl;
        public Label TestSuccessMessage;
        public CheckBox ValidateMemberRequirePassword;
        public RadioButton ValidateMemberByUserName;
        public RadioButton ValidateMemberByEmail;
        public WebElement ErrorDIVLocator;
    }
}
