namespace RegOnline.RegressionTest.PageObject.Manager
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;

    public class XAuth : XAuthLocator
    {
        public XAuth()
        {
            Test = new ButtonOrLink("ctl00_cphDialog_xAuth_btnTest", LocateBy.Id);
            OK = new ButtonOrLink("ctl00_cphDialog_saveForm", LocateBy.Id);
            ServiceEndpointURL = new TextBox("ctl00_cphDialog_xAuth_txtEndpointUrl", LocateBy.Id);
            MessageToRegistration = new TextBox("ctl00_cphDialog_xAuth_txtMessageToRegistrant", LocateBy.Id);
            TestEmail = new TextBox("ctl00_cphDialog_xAuth_txtTestEmailAddress", LocateBy.Id);
            TestUserName = new TextBox("ctl00_cphDialog_xAuth_txtTestUserName", LocateBy.Id);
            TestPassword = new TextBox("ctl00_cphDialog_xAuth_txtTestPassword", LocateBy.Id);
            DescriptionForIdentifer = new TextBox("ctl00_cphDialog_xAuth_txtLabel", LocateBy.Id);
            ForgetPasswordUrl = new TextBox("ctl00_cphDialog_xAuth_txtForgetPasswordUrl", LocateBy.Id);
            TestSuccessMessage = new Label("ctl00_cphDialog_xAuth_lblSuccessMessage", LocateBy.Id);
            ValidateMemberRequirePassword = new CheckBox("ctl00_cphDialog_xAuth_chkPassword", LocateBy.Id);
            ValidateMemberByUserName = new RadioButton("ctl00_cphDialog_xAuth_rdoMembership", LocateBy.Id);
            ValidateMemberByEmail = new RadioButton("ctl00_cphDialog_xAuth_rdoEmail", LocateBy.Id);
            ErrorDIVLocator = new WebElement("//div[@id='ctl00_cphDialog_xAuth_xAuthValSummary']", LocateBy.XPath);
        }
    }
}
