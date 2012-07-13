namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;

    public class XAuthOld : Manager.XAuthLocator
    {
        public XAuthOld()
        {
            Test = new ButtonOrLink("ctl00_cphDialog_btnTest", LocateBy.Id);
            OK = new ButtonOrLink("//div[@id='ctl00_pnlButtonsBottom']/span/a", LocateBy.XPath);
            ServiceEndpointURL = new TextBox("ctl00_cphDialog_txtEndpointUrl", LocateBy.Id);
            MessageToRegistration = new TextBox("ctl00_cphDialog_txtMessageToRegistrant", LocateBy.Id);
            TestEmail = new TextBox("ctl00_cphDialog_txtTestEmailAddress", LocateBy.Id);
            TestUserName = new TextBox("ctl00_cphDialog_txtTestUserName", LocateBy.Id);
            TestPassword = new TextBox("ctl00_cphDialog_txtTestPassword", LocateBy.Id);
            DescriptionForIdentifer = new TextBox("ctl00_cphDialog_txtLabel", LocateBy.Id);
            ForgetPasswordUrl = new TextBox("ctl00_cphDialog_txtForgetPasswordUrl", LocateBy.Id);
            TestSuccessMessage = new Label("ctl00_lblSuccessMessage", LocateBy.Id);
            ValidateMemberRequirePassword = new CheckBox("ctl00_cphDialog_chkPassword", LocateBy.Id);
            ValidateMemberByUserName = new RadioButton("ctl00_cphDialog_rdoMembership", LocateBy.Id);
            ValidateMemberByEmail = new RadioButton("ctl00_cphDialog_rdoEmail", LocateBy.Id);
            ErrorDIVLocator = new WebElement("//div[@id='ctl00_valSummary']", LocateBy.XPath);
        }
    }
}
