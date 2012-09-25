namespace RegOnline.RegressionTest.PageObject.Manager
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public abstract class XAuthLocator
    {
        public RadioButton XAuthRadio = new RadioButton("ctl00_cphDialog_xAuth_rdoXAuth", LocateBy.Id);

        public Clickable Test;
        public Clickable OK;
        public Input ServiceEndpointURL;
        public Input MessageToRegistration;
        public Input TestEmail;
        public Input TestUserName;
        public Input TestPassword;
        public Input DescriptionForIdentifer;
        public Input ForgetPasswordUrl;
        public Label TestSuccessMessage;
        public CheckBox ValidateMemberRequirePassword;
        public RadioButton ValidateMemberByUserName;
        public RadioButton ValidateMemberByEmail;
        public ElementBase ErrorDIVLocator;
    }
}
