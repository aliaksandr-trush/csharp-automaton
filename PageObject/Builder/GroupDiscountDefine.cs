namespace RegOnline.RegressionTest.PageObject.Builder
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public class GroupDiscountDefine : Frame
    {
        public GroupDiscountDefine(string name) : base(name) { }

        public TextBox GroupSize = new TextBox("ctl00_cphDialog_GroupSizeTextBox_text", LocateBy.Id);
        public MultiChoiceDropdown GroupSizeOption = new MultiChoiceDropdown("ctl00_cphDialog_ddlIsGroupSizeOrMore", LocateBy.Id);
        public TextBox DiscountAmount = new TextBox("ctl00_cphDialog_AmountTextBox_text", LocateBy.Id);
        public MultiChoiceDropdown DiscountType = new MultiChoiceDropdown("ctl00_cphDialog_ddlGroupRuleType", LocateBy.Id);
        public MultiChoiceDropdown AddtionalRegOption = new MultiChoiceDropdown("ctl00_cphDialog_AdditionalAllDropDownList", LocateBy.Id);
        public TextBox AdditionalNumber = new TextBox("ctl00_cphDialog_EffectedSizeTextBox_text", LocateBy.Id);

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        public void SaveAndStay_Click()
        {
            popupFrameHelper.SaveAndStay_Click();
        }

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        public void Cancel_Click()
        {
            popupFrameHelper.Cancel_Click();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }
    }
}
